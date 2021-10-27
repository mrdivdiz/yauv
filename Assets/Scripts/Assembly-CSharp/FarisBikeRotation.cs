using System;
using UnityEngine;

// Token: 0x02000258 RID: 600
public class FarisBikeRotation : MonoBehaviour
{
	// Token: 0x06000B5F RID: 2911 RVA: 0x0008EF28 File Offset: 0x0008D128
	private void Start()
	{
		this.D23 = Vector3.Distance(this.w1.transform.position, this.w2.transform.position);
		this.D14 = Vector3.Distance(this.w1.transform.position, this.w4.transform.position);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0008EF8C File Offset: 0x0008D18C
	private void Update()
	{
		RaycastHit raycastHit;
		Physics.Raycast(this.w1.transform.position, Vector3.down, out raycastHit, 10f);
		float distance = raycastHit.distance;
		Physics.Raycast(this.w4.transform.position, Vector3.down, out raycastHit, 10f);
		float distance2 = raycastHit.distance;
		Physics.Raycast(this.w2.transform.position, Vector3.down, out raycastHit, 10f);
		float distance3 = raycastHit.distance;
		Physics.Raycast(this.w3.transform.position, Vector3.down, out raycastHit, 10f);
		float distance4 = raycastHit.distance;
		float num = Mathf.Atan((distance3 - distance4) / this.D23);
		float num2 = Mathf.Atan((distance - distance2) / this.D14);
		num = Mathf.SmoothDampAngle(base.transform.localEulerAngles.x, num * 57.29578f, ref this.currentVelocityX, 0.15f);
		num2 = Mathf.SmoothDampAngle(base.transform.localEulerAngles.z, num2 * 57.29578f, ref this.currentVelocityZ, 0.15f);
		base.transform.localEulerAngles = new Vector3(num, 45f, num2);
	}

	// Token: 0x04001498 RID: 5272
	public Transform w1;

	// Token: 0x04001499 RID: 5273
	public Transform w2;

	// Token: 0x0400149A RID: 5274
	public Transform w3;

	// Token: 0x0400149B RID: 5275
	public Transform w4;

	// Token: 0x0400149C RID: 5276
	private float D23;

	// Token: 0x0400149D RID: 5277
	private float D14;

	// Token: 0x0400149E RID: 5278
	private float currentVelocityX;

	// Token: 0x0400149F RID: 5279
	private float currentVelocityZ;
}
