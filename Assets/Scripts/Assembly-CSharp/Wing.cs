using System;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class Wing : MonoBehaviour
{
	// Token: 0x06000B53 RID: 2899 RVA: 0x0008E6C8 File Offset: 0x0008C8C8
	private void Start()
	{
		Transform transform = base.transform;
		this.myTransform = base.transform;
		while (transform != null && transform.GetComponent<Rigidbody>() == null)
		{
			transform = transform.parent;
		}
		if (transform != null)
		{
			this.body = transform.GetComponent<Rigidbody>();
		}
		transform = base.transform;
		while (transform.GetComponent<CarDynamics>() == null)
		{
			transform = transform.parent;
		}
		this.cardynamics = transform.GetComponent<CarDynamics>();
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x0008E75C File Offset: 0x0008C95C
	private void FixedUpdate()
	{
		if (this.body != null)
		{
			float num = this.body.velocity.x * this.body.velocity.x + this.body.velocity.z * this.body.velocity.z;
			if (num > 0.1f)
			{
				this.downForce = 0.5f * this.Area * (float)this.angleOfAttack * this.dragCoefficient * this.cardynamics.airDensity * num;
				this.dragForce = 0.5f * this.dragCoefficient * this.Area * this.cardynamics.airDensity * num;
				this.body.AddForceAtPosition(-this.downForce * this.myTransform.up, this.myTransform.position);
				this.body.AddForceAtPosition(-this.dragForce * this.myTransform.forward, this.myTransform.position);
				Debug.DrawRay(this.myTransform.position, -this.downForce * this.myTransform.up / 1000f, Color.white);
				Debug.DrawRay(this.myTransform.position, -this.dragForce * this.myTransform.forward / 1000f, Color.white);
			}
		}
	}

	// Token: 0x04001489 RID: 5257
	public float dragCoefficient;

	// Token: 0x0400148A RID: 5258
	public int angleOfAttack = 1;

	// Token: 0x0400148B RID: 5259
	public float Area = 1f;

	// Token: 0x0400148C RID: 5260
	public float downForce;

	// Token: 0x0400148D RID: 5261
	public float dragForce;

	// Token: 0x0400148E RID: 5262
	private Rigidbody body;

	// Token: 0x0400148F RID: 5263
	private Transform myTransform;

	// Token: 0x04001490 RID: 5264
	private CarDynamics cardynamics;
}
