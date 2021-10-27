using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000031 RID: 49
[AddComponentMenu("Mixamo/Root Motion Computer")]
public class RootMotionComputer : MonoBehaviour
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000114 RID: 276 RVA: 0x00007924 File Offset: 0x00005B24
	public Vector3 deltaPosition
	{
		get
		{
			return this.dPosition;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000115 RID: 277 RVA: 0x0000792C File Offset: 0x00005B2C
	public Quaternion deltaRotation
	{
		get
		{
			return this.dRotation;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000116 RID: 278 RVA: 0x00007934 File Offset: 0x00005B34
	public Vector3 deltaEulerAngles
	{
		get
		{
			return this.dRotation.eulerAngles;
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00007944 File Offset: 0x00005B44
	private void Start()
	{
		if (!this.isManagedExternally)
		{
			this.Initialize();
		}
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00007958 File Offset: 0x00005B58
	public void Initialize()
	{
		if (this.anim == null)
		{
			this.anim = (base.gameObject.GetComponentInChildren(typeof(Animation)) as Animation);
			if (this.anim == null)
			{
				Debug.LogError("No animation component has been specified.", this);
			}
			else if (this.isDebugMode)
			{
				Debug.LogWarning(string.Format("No animation component has been specified. Using the animation component on {0}.", base.gameObject.name), this);
			}
		}
		if (this.rootNode == null)
		{
			this.rootNode = base.transform;
			if (this.isDebugMode)
			{
				Debug.LogWarning(string.Format("No root object has been manually specified. Assuming that {0} is the root object to be moved.", base.gameObject.name), this);
			}
		}
		if (this.pelvis == null)
		{
			Component[] componentsInChildren = base.GetComponentsInChildren(typeof(Transform));
			foreach (Transform transform in componentsInChildren)
			{
				if (this.pelvis == null && (transform.name.ToLower() == "hips" || transform.name.ToLower().Contains("pelvis")))
				{
					this.pelvis = transform;
				}
			}
			if (this.pelvis == null)
			{
				foreach (Transform transform2 in componentsInChildren)
				{
					if (!(transform2.GetComponent(typeof(SkinnedMeshRenderer)) == null))
					{
						Component[] componentsInChildren2 = transform2.GetComponentsInChildren(typeof(Transform));
						if (componentsInChildren2.Length > 1)
						{
							this.pelvis = transform2;
						}
					}
				}
			}
			if (this.pelvis == null)
			{
				Debug.LogError("No pelvis transform has been specified.", this);
			}
			else if (this.isDebugMode)
			{
				Debug.LogWarning(string.Format("No pelvis object as been manually specified. Assuming that {0} is the pelvis object to track.", this.pelvis.name));
			}
		}
		bool isPlaying = this.anim.isPlaying;
		this.animInfoTable = new Hashtable();
		foreach (object obj in this.anim)
		{
			AnimationState aState = (AnimationState)obj;
			this.AddAnimInfoToTable(aState);
		}
		this.anim.Sample();
		this.anim.Stop();
		this.anim.enabled = true;
		foreach (object obj2 in this.anim)
		{
			AnimationState aState2 = (AnimationState)obj2;
			this.SetupNewAnimInfo(aState2);
		}
		foreach (object obj3 in this.anim)
		{
			AnimationState animationState = (AnimationState)obj3;
			this.info = (AnimInfo)this.animInfoTable[animationState];
			animationState.weight = this.info.currentWeight;
			animationState.normalizedTime = this.info.currentNormalizedTime;
		}
		if (isPlaying)
		{
			this.anim.Play();
		}
		else
		{
			this.anim.Stop();
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00007D3C File Offset: 0x00005F3C
	public void AddAnimInfoToTable(AnimationState aState)
	{
		AnimInfo animInfo = default(AnimInfo);
		animInfo.currentNormalizedTime = aState.normalizedTime;
		animInfo.currentWeight = aState.weight;
		this.animInfoTable.Add(aState, animInfo);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00007D80 File Offset: 0x00005F80
	public void SetupNewAnimInfo(AnimationState aState)
	{
		AnimInfo animInfo = (AnimInfo)this.animInfoTable[aState];
		bool enabled = aState.enabled;
		WrapMode wrapMode = aState.wrapMode;
		aState.weight = 1f;
		aState.enabled = true;
		aState.wrapMode = WrapMode.Once;
		aState.normalizedTime = 0f;
		this.anim.Sample();
		animInfo.startPosition = this.GetProjectedPosition(this.pelvis);
		animInfo.previousPosition = this.GetProjectedPosition(this.pelvis);
		animInfo.startAxis = this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis);
		animInfo.previousAxis = this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis);
		aState.normalizedTime = 1f;
		this.anim.Sample();
		animInfo.endPosition = this.GetProjectedPosition(this.pelvis);
		animInfo.endAxis = this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis);
		animInfo.totalRotation = Quaternion.FromToRotation(animInfo.startAxis, animInfo.endAxis);
		aState.normalizedTime = 0f;
		aState.weight = 0f;
		aState.enabled = enabled;
		aState.wrapMode = wrapMode;
		this.anim.Sample();
		this.animInfoTable[aState] = animInfo;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00007ED4 File Offset: 0x000060D4
	private void LateUpdate()
	{
		if (!this.isManagedExternally)
		{
			this.ComputeRootMotion();
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00007EE8 File Offset: 0x000060E8
	public void ComputeRootMotion()
	{
		if (!this.anim.isPlaying)
		{
			return;
		}
		bool flag = this.computationMode == RootMotionComputationMode.TranslationAndRotation;
		ArrayList arrayList = null;
		foreach (object obj in this.anim)
		{
			AnimationState animationState = (AnimationState)obj;
			this.highestLayer = Mathf.Max(this.highestLayer, animationState.layer);
			this.lowestLayer = Mathf.Min(this.lowestLayer, animationState.layer);
			if (!this.animInfoTable.ContainsKey(animationState))
			{
				this.AddAnimInfoToTable(animationState);
				arrayList.Add(animationState);
			}
			else
			{
				this.info = (AnimInfo)this.animInfoTable[animationState];
				this.info.currentNormalizedTime = animationState.normalizedTime;
				this.info.currentWeight = animationState.weight;
				this.animInfoTable[animationState] = this.info;
				animationState.weight = 0f;
			}
		}
		if (arrayList != null && arrayList.Count > 0)
		{
			foreach (object obj2 in this.anim)
			{
				AnimationState animationState2 = (AnimationState)obj2;
				animationState2.weight = 0f;
			}
			foreach (object obj3 in arrayList)
			{
				AnimationState aState = (AnimationState)obj3;
				this.SetupNewAnimInfo(aState);
			}
		}
		float num = 1f;
		for (int i = this.highestLayer; i >= this.lowestLayer; i--)
		{
			float num2 = 0f;
			foreach (object obj4 in this.anim)
			{
				AnimationState animationState3 = (AnimationState)obj4;
				if (animationState3.layer == i)
				{
					this.info = (AnimInfo)this.animInfoTable[animationState3];
					if (!animationState3.enabled || num <= 0f)
					{
						this.info.contributingWeight = 0f;
					}
					else
					{
						this.info.contributingWeight = num * this.info.currentWeight;
					}
					num2 += this.info.contributingWeight;
					this.animInfoTable[animationState3] = this.info;
				}
			}
			if (num2 > 1f)
			{
				float num3 = 1f / num2;
				foreach (object obj5 in this.anim)
				{
					AnimationState animationState4 = (AnimationState)obj5;
					if (animationState4.layer == i)
					{
						this.info = (AnimInfo)this.animInfoTable[animationState4];
						this.info.contributingWeight = this.info.contributingWeight * num3;
						this.animInfoTable[animationState4] = this.info;
					}
				}
				num2 = 1f;
			}
			num -= num2;
		}
		this.dPosition = Vector3.zero;
		this.dRotation = Quaternion.identity;
		foreach (object obj6 in this.anim)
		{
			AnimationState animationState5 = (AnimationState)obj6;
			this.info = (AnimInfo)this.animInfoTable[animationState5];
			if (this.info.contributingWeight != 0f)
			{
				if (animationState5.blendMode != AnimationBlendMode.Additive)
				{
					animationState5.weight = 1f;
					animationState5.time -= Time.deltaTime * animationState5.speed;
					this.info.previousNormalizedTime = animationState5.normalizedTime;
					this.anim.Sample();
					this.info.previousAxis = this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis);
					this.info.previousPosition = this.GetProjectedPosition(this.pelvis);
					animationState5.normalizedTime = this.info.currentNormalizedTime;
					this.anim.Sample();
					this.info.currentPosition = this.GetProjectedPosition(this.pelvis);
					this.info.currentAxis = this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis);
					this.info.previousNormalizedTime = 1f + this.info.previousNormalizedTime - (float)((int)this.info.previousNormalizedTime);
					this.info.currentNormalizedTime = 1f + this.info.currentNormalizedTime - (float)((int)this.info.currentNormalizedTime);
					if (this.info.previousNormalizedTime - (float)((int)this.info.previousNormalizedTime) > this.info.currentNormalizedTime - (float)((int)this.info.currentNormalizedTime))
					{
						this.p = this.info.contributingWeight * (this.info.endPosition - this.info.previousPosition + (this.info.currentPosition - this.info.startPosition));
						if (flag)
						{
							this.p = Quaternion.FromToRotation(this.info.currentAxis, this.info.totalRotation * Vector3.right) * this.p;
							this.dRotation *= Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.info.previousAxis, this.info.endAxis) * Quaternion.FromToRotation(this.info.startAxis, this.info.currentAxis), this.info.contributingWeight);
						}
						this.dPosition += this.p;
					}
					else
					{
						this.p = this.info.contributingWeight * (this.info.currentPosition - this.info.previousPosition);
						if (flag)
						{
							this.p = Quaternion.FromToRotation(this.info.currentAxis, Vector3.right) * this.p;
							this.dRotation *= Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.info.previousAxis, this.info.currentAxis), this.info.contributingWeight);
						}
						this.dPosition += this.p;
					}
					animationState5.weight = 0f;
				}
			}
		}
		foreach (object obj7 in this.anim)
		{
			AnimationState animationState6 = (AnimationState)obj7;
			this.info = (AnimInfo)this.animInfoTable[animationState6];
			animationState6.weight = this.info.currentWeight;
		}
		this.anim.Sample();
		if (this.isFirstFrame)
		{
			this.dPosition = this.GetProjectedPosition(this.pelvis);
			this.dRotation = Quaternion.FromToRotation(Vector3.right, this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis));
			if (flag)
			{
				this.dPosition = Quaternion.FromToRotation(this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis), Vector3.right) * this.dPosition;
			}
			this.isFirstFrame = false;
		}
		this.pLocalPosition = this.pelvis.localPosition;
		if (this.computationMode == RootMotionComputationMode.ZTranslation)
		{
			this.dPosition = Vector3.forward * Vector3.Dot(this.dPosition, Vector3.forward);
		}
		else
		{
			this.pLocalPosition.x = 0f;
		}
		this.pLocalPosition.z = 0f;
		this.pelvis.localPosition = this.pLocalPosition;
		if (flag)
		{
			this.pelvis.localRotation = Quaternion.FromToRotation(this.GetProjectedAxis(this.pelvis, this.pelvisRightAxis), Vector3.right) * this.pelvis.localRotation;
		}
		if (this.isDebugMode)
		{
			this.DrawDebug();
		}
		if (!this.applyMotion)
		{
			return;
		}
		if (flag)
		{
			this.rootNode.localRotation *= this.dRotation;
		}
		this.rootNode.Translate(this.dPosition, Space.Self);
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000088E8 File Offset: 0x00006AE8
	private Vector3 GetProjectedPosition(Transform t)
	{
		Vector3 result = this.rootNode.InverseTransformPoint(t.position);
		result.y = 0f;
		return result;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008914 File Offset: 0x00006B14
	private Vector3 GetProjectedAxis(Transform t, Vector3 axis)
	{
		Vector3 result = this.rootNode.InverseTransformDirection(t.TransformDirection(axis));
		result.y = 0f;
		return result;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00008944 File Offset: 0x00006B44
	private void DrawDebug()
	{
		Debug.DrawRay(this.pelvis.position, this.pelvis.TransformDirection(this.pelvisRightAxis) * this.debugGizmoSize, Color.red);
		Debug.DrawRay(this.rootNode.position, this.rootNode.rotation * Vector3.forward * this.debugGizmoSize, Color.blue);
		Debug.DrawRay(this.rootNode.position, this.rootNode.rotation * Vector3.right * this.debugGizmoSize, Color.red);
		Debug.DrawRay(this.rootNode.position, this.rootNode.rotation * Vector3.up * this.debugGizmoSize, Color.green);
	}

	// Token: 0x0400010E RID: 270
	public Transform rootNode;

	// Token: 0x0400010F RID: 271
	public Animation anim;

	// Token: 0x04000110 RID: 272
	public Transform pelvis;

	// Token: 0x04000111 RID: 273
	public Vector3 pelvisRightAxis = Vector3.right;

	// Token: 0x04000112 RID: 274
	private Vector3 pLocalPosition;

	// Token: 0x04000113 RID: 275
	public bool isManagedExternally;

	// Token: 0x04000114 RID: 276
	public RootMotionComputationMode computationMode = RootMotionComputationMode.TranslationAndRotation;

	// Token: 0x04000115 RID: 277
	public bool applyMotion = true;

	// Token: 0x04000116 RID: 278
	private Vector3 dPosition = Vector3.zero;

	// Token: 0x04000117 RID: 279
	private Vector3 p;

	// Token: 0x04000118 RID: 280
	private Quaternion dRotation = Quaternion.identity;

	// Token: 0x04000119 RID: 281
	private Hashtable animInfoTable;

	// Token: 0x0400011A RID: 282
	private AnimInfo info;

	// Token: 0x0400011B RID: 283
	public bool isDebugMode = true;

	// Token: 0x0400011C RID: 284
	public float debugGizmoSize = 0.25f;

	// Token: 0x0400011D RID: 285
	private bool isFirstFrame = true;

	// Token: 0x0400011E RID: 286
	private int highestLayer;

	// Token: 0x0400011F RID: 287
	private int lowestLayer;
}
