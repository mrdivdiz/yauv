using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class AdvancedRagdoll : MonoBehaviour
{
	// Token: 0x060000FB RID: 251 RVA: 0x0000590C File Offset: 0x00003B0C
	private void Start()
	{
		if (this.debug)
		{
			Debug.Log("AdvancedRagdoll.Start() " + base.name);
		}
		if (this.groundLayer != -1)
		{
			this.groundMask = 1 << this.groundLayer;
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00005960 File Offset: 0x00003B60
	public void SynchRagdollIn(Transform src)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.SynchRagdollIn() " + base.name + " passed null src!");
			return;
		}
		if (this.copyMaterials)
		{
			this.CopyMaterials(src, base.transform);
		}
		Vector3 velocity = Vector3.zero;
		if (src.gameObject.GetComponent<Rigidbody>())
		{
			velocity = src.gameObject.GetComponent<Rigidbody>().velocity;
		}
		else
		{
			CharacterController component = src.gameObject.GetComponent<CharacterController>();
			velocity = component.velocity;
		}
		this.CopyTransforms(src, base.transform, velocity);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00005A00 File Offset: 0x00003C00
	public void SynchRagdollOut(Transform dest)
	{
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.SynchRagdollOut() " + base.name + " passed null dest!");
			return;
		}
		if (this.copyMaterials)
		{
			this.CopyMaterials(base.transform, dest);
		}
		Ray ray = new Ray(this.ragdollRoot.position, -Vector3.up);
		Vector3 b = Vector3.zero;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, (this.groundMask == -1) ? (1 << LayerMask.NameToLayer("Default")) : this.groundMask))
		{
			if (this.debug)
			{
				Debug.Log("AdvancedRagdoll.SynchRagdollOut() " + base.name + " looking for ground found " + raycastHit.collider.name);
			}
			b = this.ragdollRoot.position - raycastHit.point;
			base.transform.position = raycastHit.point;
		}
		else
		{
			Debug.LogWarning("AdvancedRagdoll.SynchRagdollOut() " + base.name + " could not find ground beneath the ragdoll's root!");
		}
		this.ragdollRoot.position = base.transform.position + b;
		this.PoseToAnimation(base.transform, dest);
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00005B48 File Offset: 0x00003D48
	private void CopyTransforms(Transform src, Transform dest, Vector3 velocity)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.CopyTransforms() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.CopyTransforms() " + base.name + " passed null dest!");
			return;
		}
		Rigidbody rigidbody = dest.GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			rigidbody.velocity = velocity;
			rigidbody.useGravity = true;
		}
		dest.localPosition = src.localPosition;
		dest.localRotation = src.localRotation;
		dest.localScale = src.localScale;
		foreach (object obj in src)
		{
			Transform transform = (Transform)obj;
			Transform transform2 = dest.Find(transform.name);
			if (transform2)
			{
				this.CopyTransforms(transform, transform2, velocity);
			}
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00005C60 File Offset: 0x00003E60
	private void CopyTransforms(Transform src, Transform dest)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.CopyTransforms() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.CopyTransforms() " + base.name + " passed null dest!");
			return;
		}
		dest.localPosition = src.localPosition;
		dest.localRotation = src.localRotation;
		dest.localScale = src.localScale;
		foreach (object obj in src)
		{
			Transform transform = (Transform)obj;
			Transform transform2 = dest.Find(transform.name);
			if (transform2)
			{
				this.CopyTransforms(transform, transform2);
			}
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00005D54 File Offset: 0x00003F54
	public void PoseToAnimation(Transform src, Transform dest)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.PoseToAnimation() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.PoseToAnimation() " + base.name + " passed null dest!");
			return;
		}
		if (!dest.GetComponent<Animation>())
		{
			dest.gameObject.AddComponent<Animation>();
		}
		this.CopyTransforms(src, dest);
		AnimationClip clip = new AnimationClip();
		this.TransformToAnimationCurve(src, src, dest, dest, clip);
		dest.gameObject.GetComponent<Animation>().AddClip(clip, "RagdollPose");
		dest.gameObject.GetComponent<Animation>()["RagdollPose"].wrapMode = WrapMode.Loop;
		dest.gameObject.GetComponent<Animation>().Play("RagdollPose");
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00005E30 File Offset: 0x00004030
	private void TransformToAnimationCurve(Transform src, Transform srcRoot, Transform dest, Transform destRoot, AnimationClip clip)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.TransformToAnimationClip() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.TransformToAnimationClip() " + base.name + " passed null dest!");
			return;
		}
		if (!clip)
		{
			Debug.LogError("AdvancedRagdoll.TransformToAnimationClip() " + base.name + " passed null clip!");
			return;
		}
		string transformPathToRoot = this.GetTransformPathToRoot(src, srcRoot);
		string transformPathToRoot2 = this.GetTransformPathToRoot(dest, destRoot);
		if (this.debug)
		{
			Debug.Log(string.Concat(new string[]
			{
				"AdvancedRagdoll.TransformToAnimationClip() ",
				base.name,
				"\n srcPath = ",
				transformPathToRoot,
				"\n destPath = ",
				transformPathToRoot2
			}));
		}
		AnimationCurve curve = AnimationCurve.Linear(0f, src.localPosition.x, 1f, src.localPosition.x);
		AnimationCurve curve2 = AnimationCurve.Linear(0f, src.localPosition.y, 1f, src.localPosition.y);
		AnimationCurve curve3 = AnimationCurve.Linear(0f, src.localPosition.z, 1f, src.localPosition.z);
		AnimationCurve curve4 = AnimationCurve.Linear(0f, src.localRotation.w, 1f, src.localRotation.w);
		AnimationCurve curve5 = AnimationCurve.Linear(0f, src.localRotation.x, 1f, src.localRotation.x);
		AnimationCurve curve6 = AnimationCurve.Linear(0f, src.localRotation.y, 1f, src.localRotation.y);
		AnimationCurve curve7 = AnimationCurve.Linear(0f, src.localRotation.z, 1f, src.localRotation.z);
		AnimationCurve curve8 = AnimationCurve.Linear(0f, src.localScale.x, 1f, src.localScale.x);
		AnimationCurve curve9 = AnimationCurve.Linear(0f, src.localScale.y, 1f, src.localScale.y);
		AnimationCurve curve10 = AnimationCurve.Linear(0f, src.localScale.z, 1f, src.localScale.z);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localPosition.x", curve);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localPosition.y", curve2);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localPosition.z", curve3);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.w", curve4);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.x", curve5);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.y", curve6);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.z", curve7);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localScale.x", curve8);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localScale.y", curve9);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localScale.z", curve10);
		foreach (object obj in src)
		{
			Transform transform = (Transform)obj;
			Transform transform2 = dest.Find(transform.name);
			if (transform2)
			{
				this.TransformToAnimationCurve(transform, srcRoot, transform2, destRoot, clip);
			}
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0000625C File Offset: 0x0000445C
	private string GetTransformPathToRoot(Transform t, Transform root)
	{
		if (!t)
		{
			Debug.LogError("AdvancedRagdoll.GetTransformPathToRoot() " + base.name + " passed null t!");
			return string.Empty;
		}
		if (!root)
		{
			Debug.LogError("AdvancedRagdoll.GetTransformPathToRoot() " + base.name + " passed null root!");
			return string.Empty;
		}
		string text = t.name;
		Transform parent = t.parent;
		while (parent != null && parent != root)
		{
			text = text.Insert(0, parent.name + "/");
			parent = parent.parent;
		}
		return text;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000630C File Offset: 0x0000450C
	private void CopyMaterials(Transform src, Transform dest)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " passed null dest!");
			return;
		}
		Renderer[] componentsInChildren = src.GetComponentsInChildren<Renderer>();
		Renderer[] componentsInChildren2 = dest.GetComponentsInChildren<Renderer>();
		if (componentsInChildren.Length != componentsInChildren2.Length)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"AdvancedRagdoll.CopyMaterials() ",
				base.name,
				" number of src renderers (",
				componentsInChildren.Length,
				") does not match dest (",
				componentsInChildren2.Length,
				")"
			}));
		}
		else if (componentsInChildren.Length == 0)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " has no renderers!");
		}
		else
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren2[i].materials = componentsInChildren[i].materials;
			}
		}
	}

	// Token: 0x040000C5 RID: 197
	public bool debug;

	// Token: 0x040000C6 RID: 198
	public Transform ragdollRoot;

	// Token: 0x040000C7 RID: 199
	public bool copyMaterials;

	// Token: 0x040000C8 RID: 200
	public LayerMask groundLayer = -1;

	// Token: 0x040000C9 RID: 201
	private int groundMask = -1;
}
