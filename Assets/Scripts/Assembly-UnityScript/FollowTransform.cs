using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
[Serializable]
public class FollowTransform : MonoBehaviour
{
	// Token: 0x060000D1 RID: 209 RVA: 0x00004AE4 File Offset: 0x00002CE4
	public virtual void Start()
	{
		this.thisTransform = this.transform;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00004AF4 File Offset: 0x00002CF4
	public virtual void Update()
	{
		this.thisTransform.position = this.targetTransform.position;
		if (this.faceForward)
		{
			this.thisTransform.forward = this.targetTransform.forward;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00004B38 File Offset: 0x00002D38
	public virtual void Main()
	{
	}

	// Token: 0x04000081 RID: 129
	public Transform targetTransform;

	// Token: 0x04000082 RID: 130
	public bool faceForward;

	// Token: 0x04000083 RID: 131
	private Transform thisTransform;
}
