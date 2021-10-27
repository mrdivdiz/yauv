using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class HeadLookController : MonoBehaviour
{
	// Token: 0x06000986 RID: 2438 RVA: 0x0005554C File Offset: 0x0005374C
	private void Start()
	{
		this.animHandler = base.transform.GetComponent<AnimationHandler>();
		if (this.rootNode == null)
		{
			this.rootNode = base.transform;
		}
		foreach (BendingSegment bendingSegment in this.segments)
		{
			Quaternion rotation = bendingSegment.firstTransform.parent.rotation;
			Quaternion lhs = Quaternion.Inverse(rotation);
			bendingSegment.referenceLookDir = lhs * this.rootNode.rotation * this.headLookVector.normalized;
			bendingSegment.referenceUpDir = lhs * this.rootNode.rotation * this.headUpVector.normalized;
			bendingSegment.angleH = 0f;
			bendingSegment.angleV = 0f;
			bendingSegment.dirUp = bendingSegment.referenceUpDir;
			bendingSegment.chainLength = 1;
			Transform transform = bendingSegment.lastTransform;
			while (transform != bendingSegment.firstTransform && transform != transform.root)
			{
				bendingSegment.chainLength++;
				transform = transform.parent;
			}
			bendingSegment.origRotations = new Quaternion[bendingSegment.chainLength];
			transform = bendingSegment.lastTransform;
			for (int j = bendingSegment.chainLength - 1; j >= 0; j--)
			{
				bendingSegment.origRotations[j] = transform.localRotation;
				transform = transform.parent;
			}
		}
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x000556DC File Offset: 0x000538DC
	private void LateUpdate()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (Time.deltaTime == 0f)
		{
			return;
		}
		if (this.targetTransform != null)
		{
			this.target = this.targetTransform.position;
		}
		if (this.effect == 0f && this.overrideAnimation)
		{
			this.overrideAnimation = false;
		}
		if (this.animHandler != null && (this.animHandler.isholdingWeapon || this.animHandler.stealthMode))
		{
			this.effect = 0f;
			this.overrideAnimation = false;
			if (base.gameObject.tag == "Player" && AnimationHandler.instance.faceState == AnimationHandler.FaceState.LOOKAT)
			{
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			}
		}
		else if (this.targetTransform != null)
		{
			float num = Vector3.Angle(base.transform.forward, new Vector3(this.targetTransform.position.x, 0f, this.targetTransform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z));
			if (num <= 90f)
			{
				this.effect = Mathf.SmoothDamp(this.effect, 1f, ref this.effectIn, 0.3f);
				this.overrideAnimation = true;
				if (base.gameObject.tag == "Player")
				{
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.LOOKAT;
				}
			}
			else if (this.effect != 0f)
			{
				this.effect = Mathf.SmoothDamp(this.effect, -0.5f, ref this.effectOut, 0.3f);
				if (this.effect <= 0.05f)
				{
					this.overrideAnimation = false;
					this.effect = 0f;
					if (base.gameObject.tag == "Player" && AnimationHandler.instance.faceState == AnimationHandler.FaceState.LOOKAT)
					{
						AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
					}
				}
			}
		}
		else if (this.effect != 0f)
		{
			this.effect = Mathf.SmoothDamp(this.effect, -0.5f, ref this.effectEnd, 0.3f);
			if (this.effect <= 0.05f)
			{
				this.overrideAnimation = false;
				this.effect = 0f;
				if (base.gameObject.tag == "Player" && AnimationHandler.instance.faceState == AnimationHandler.FaceState.LOOKAT)
				{
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				}
			}
		}
		Vector3[] array = new Vector3[this.nonAffectedJoints.Length];
		for (int i = 0; i < this.nonAffectedJoints.Length; i++)
		{/*
			using (IEnumerator enumerator = this.nonAffectedJoints[i].joint.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					array[i] = transform.position - this.nonAffectedJoints[i].joint.position;
				}
			}
		*/}
		foreach (BendingSegment bendingSegment in this.segments)
		{
			Transform transform2 = bendingSegment.lastTransform;
			if (this.overrideAnimation)
			{
				for (int k = bendingSegment.chainLength - 1; k >= 0; k--)
				{
					transform2.localRotation = bendingSegment.origRotations[k];
					transform2 = transform2.parent;
				}
			}
			Quaternion rotation = bendingSegment.firstTransform.parent.rotation;
			Quaternion rotation2 = Quaternion.Inverse(rotation);
			Vector3 normalized = (this.target - bendingSegment.lastTransform.position).normalized;
			Vector3 vector = rotation2 * normalized;
			float num2 = HeadLookController.AngleAroundAxis(bendingSegment.referenceLookDir, vector, bendingSegment.referenceUpDir);
			Vector3 axis = Vector3.Cross(bendingSegment.referenceUpDir, vector);
			Vector3 dirA = vector - Vector3.Project(vector, bendingSegment.referenceUpDir);
			float num3 = HeadLookController.AngleAroundAxis(dirA, vector, axis);
			float f = Mathf.Max(0f, Mathf.Abs(num2) - bendingSegment.thresholdAngleDifference) * Mathf.Sign(num2);
			float f2 = Mathf.Max(0f, Mathf.Abs(num3) - bendingSegment.thresholdAngleDifference) * Mathf.Sign(num3);
			num2 = Mathf.Max(Mathf.Abs(f) * Mathf.Abs(bendingSegment.bendingMultiplier), Mathf.Abs(num2) - bendingSegment.maxAngleDifference) * Mathf.Sign(num2) * Mathf.Sign(bendingSegment.bendingMultiplier);
			num3 = Mathf.Max(Mathf.Abs(f2) * Mathf.Abs(bendingSegment.bendingMultiplier), Mathf.Abs(num3) - bendingSegment.maxAngleDifference) * Mathf.Sign(num3) * Mathf.Sign(bendingSegment.bendingMultiplier);
			num2 = Mathf.Clamp(num2, -bendingSegment.maxBendingAngle, bendingSegment.maxBendingAngle);
			num3 = Mathf.Clamp(num3, -bendingSegment.maxBendingAngle, bendingSegment.maxBendingAngle);
			Vector3 axis2 = Vector3.Cross(bendingSegment.referenceUpDir, bendingSegment.referenceLookDir);
			bendingSegment.angleH = Mathf.Lerp(bendingSegment.angleH, num2, Time.deltaTime * bendingSegment.responsiveness);
			bendingSegment.angleV = Mathf.Lerp(bendingSegment.angleV, num3, Time.deltaTime * bendingSegment.responsiveness);
			vector = Quaternion.AngleAxis(bendingSegment.angleH, bendingSegment.referenceUpDir) * Quaternion.AngleAxis(bendingSegment.angleV, axis2) * bendingSegment.referenceLookDir;
			Vector3 referenceUpDir = bendingSegment.referenceUpDir;
			Vector3.OrthoNormalize(ref vector, ref referenceUpDir);
			Vector3 forward = vector;
			bendingSegment.dirUp = Vector3.Slerp(bendingSegment.dirUp, referenceUpDir, Time.deltaTime * 5f);
			Vector3.OrthoNormalize(ref forward, ref bendingSegment.dirUp);
			Quaternion to = rotation * Quaternion.LookRotation(forward, bendingSegment.dirUp) * Quaternion.Inverse(rotation * Quaternion.LookRotation(bendingSegment.referenceLookDir, bendingSegment.referenceUpDir));
			Quaternion lhs = Quaternion.Slerp(Quaternion.identity, to, this.effect / (float)bendingSegment.chainLength);
			transform2 = bendingSegment.lastTransform;
			for (int l = 0; l < bendingSegment.chainLength; l++)
			{
				transform2.rotation = lhs * transform2.rotation;
				transform2 = transform2.parent;
			}
		}
		for (int m = 0; m < this.nonAffectedJoints.Length; m++)
		{/*
			Vector3 vector2 = Vector3.zero;
			using (IEnumerator enumerator2 = this.nonAffectedJoints[m].joint.GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Transform transform3 = (Transform)enumerator2.Current;
					vector2 = transform3.position - this.nonAffectedJoints[m].joint.position;
				}
			}
			Vector3 toDirection = Vector3.Slerp(array[m], vector2, this.nonAffectedJoints[m].effect);
			this.nonAffectedJoints[m].joint.rotation = Quaternion.FromToRotation(vector2, toDirection) * this.nonAffectedJoints[m].joint.rotation;
		*/}
		if (this.targetTransform != null && this.effect > 0f && this.animHandler != null)
		{
			this.animHandler.isLookingAtSomthing = true;
		}
		else if (this.animHandler != null)
		{
			this.animHandler.isLookingAtSomthing = false;
		}
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00055F5C File Offset: 0x0005415C
	public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0f) ? 1 : -1);
	}

	// Token: 0x04000DB5 RID: 3509
	public Transform rootNode;

	// Token: 0x04000DB6 RID: 3510
	public BendingSegment[] segments;

	// Token: 0x04000DB7 RID: 3511
	public NonAffectedJoints[] nonAffectedJoints;

	// Token: 0x04000DB8 RID: 3512
	public Vector3 headLookVector = Vector3.forward;

	// Token: 0x04000DB9 RID: 3513
	public Vector3 headUpVector = Vector3.up;

	// Token: 0x04000DBA RID: 3514
	public Vector3 target = Vector3.zero;

	// Token: 0x04000DBB RID: 3515
	public Transform targetTransform;

	// Token: 0x04000DBC RID: 3516
	public float effect = 1f;

	// Token: 0x04000DBD RID: 3517
	public bool overrideAnimation;

	// Token: 0x04000DBE RID: 3518
	private float effectIn;

	// Token: 0x04000DBF RID: 3519
	private float effectOut;

	// Token: 0x04000DC0 RID: 3520
	private float effectEnd;

	// Token: 0x04000DC1 RID: 3521
	private AnimationHandler animHandler;
}
