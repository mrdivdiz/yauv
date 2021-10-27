using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[Serializable]
public class AntiRollBar : MonoBehaviour
{
	// Token: 0x06000072 RID: 114 RVA: 0x00003690 File Offset: 0x00001890
	public AntiRollBar()
	{
		this.AntiRoll = 5000f;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000036A4 File Offset: 0x000018A4
	public virtual void FixedUpdate()
	{
		WheelHit wheelHit = default(WheelHit);
		float num = 1f;
		float num2 = 1f;
		bool groundHit = this.WheelL.GetGroundHit(out wheelHit);
		if (groundHit)
		{
			num = (-this.WheelL.transform.InverseTransformPoint(wheelHit.point).y - this.WheelL.radius) / this.WheelL.suspensionDistance;
		}
		bool groundHit2 = this.WheelR.GetGroundHit(out wheelHit);
		if (groundHit2)
		{
			num2 = (-this.WheelR.transform.InverseTransformPoint(wheelHit.point).y - this.WheelR.radius) / this.WheelR.suspensionDistance;
		}
		float num3 = (num - num2) * this.AntiRoll;
		if (groundHit)
		{
			this.GetComponent<Rigidbody>().AddForceAtPosition(this.WheelL.transform.up * -num3, this.WheelL.transform.position);
		}
		if (groundHit2)
		{
			this.GetComponent<Rigidbody>().AddForceAtPosition(this.WheelR.transform.up * num3, this.WheelR.transform.position);
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000037E4 File Offset: 0x000019E4
	public virtual void Main()
	{
	}

	// Token: 0x04000044 RID: 68
	public WheelCollider WheelL;

	// Token: 0x04000045 RID: 69
	public WheelCollider WheelR;

	// Token: 0x04000046 RID: 70
	public float AntiRoll;
}
