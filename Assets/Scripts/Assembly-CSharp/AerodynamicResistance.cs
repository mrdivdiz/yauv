using System;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class AerodynamicResistance : MonoBehaviour
{
	// Token: 0x06000ABF RID: 2751 RVA: 0x0007FE48 File Offset: 0x0007E048
	private void Start()
	{
		this.body = base.GetComponent<Rigidbody>();
		this.cardynamics = base.GetComponent<CarDynamics>();
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0007FE64 File Offset: 0x0007E064
	private void FixedUpdate()
	{
		if (this.body.velocity.sqrMagnitude <= 0.001f)
		{
			this.dragForce = 0f;
		}
		else
		{
			this.dragForce = 0.5f * this.Cx * this.Area * this.cardynamics.airDensity * this.body.velocity.sqrMagnitude;
		}
		this.body.AddForce(-this.dragForce * this.body.velocity.normalized);
	}

	// Token: 0x040011BA RID: 4538
	public float Cx = 0.3f;

	// Token: 0x040011BB RID: 4539
	public float Area = 1.858f;

	// Token: 0x040011BC RID: 4540
	public float dragForce;

	// Token: 0x040011BD RID: 4541
	private Rigidbody body;

	// Token: 0x040011BE RID: 4542
	private CarDynamics cardynamics;
}
