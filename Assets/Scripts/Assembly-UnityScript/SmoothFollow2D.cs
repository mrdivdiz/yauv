using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
[Serializable]
public class SmoothFollow2D : MonoBehaviour
{
	// Token: 0x060000F5 RID: 245 RVA: 0x00005EA4 File Offset: 0x000040A4
	public SmoothFollow2D()
	{
		this.smoothTime = 0.3f;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00005EB8 File Offset: 0x000040B8
	public virtual void Start()
	{
		this.thisTransform = this.transform;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00005EC8 File Offset: 0x000040C8
	public virtual void Update()
	{
		float x = Mathf.SmoothDamp(this.thisTransform.position.x, this.target.position.x, ref this.velocity.x, this.smoothTime);
		Vector3 position = this.thisTransform.position;
		float num = position.x = x;
		Vector3 vector = this.thisTransform.position = position;
		float y = Mathf.SmoothDamp(this.thisTransform.position.y, this.target.position.y, ref this.velocity.y, this.smoothTime);
		Vector3 position2 = this.thisTransform.position;
		float num2 = position2.y = y;
		Vector3 vector2 = this.thisTransform.position = position2;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00005FB4 File Offset: 0x000041B4
	public virtual void Main()
	{
	}

	// Token: 0x040000C1 RID: 193
	public Transform target;

	// Token: 0x040000C2 RID: 194
	public float smoothTime;

	// Token: 0x040000C3 RID: 195
	private Transform thisTransform;

	// Token: 0x040000C4 RID: 196
	private Vector2 velocity;
}
