using System;
using UnityEngine;

// Token: 0x02000254 RID: 596
[ExecuteInEditMode]
public class Wheels : MonoBehaviour
{
	// Token: 0x06000B51 RID: 2897 RVA: 0x0008E56C File Offset: 0x0008C76C
	private void Update()
	{
		if (Application.isEditor)
		{
			if (this.frontLeftWheel)
			{
				this.frontLeftWheel.wheelPos = WheelPos.FRONT_LEFT;
			}
			if (this.frontRightWheel)
			{
				this.frontRightWheel.wheelPos = WheelPos.FRONT_RIGHT;
			}
			if (this.rearLeftWheel)
			{
				this.rearLeftWheel.wheelPos = WheelPos.REAR_LEFT;
			}
			if (this.rearRightWheel)
			{
				this.rearRightWheel.wheelPos = WheelPos.REAR_RIGHT;
			}
			this.frontWheels[0] = this.frontLeftWheel;
			this.frontWheels[1] = this.frontRightWheel;
			this.rearWheels[0] = this.rearLeftWheel;
			this.rearWheels[1] = this.rearRightWheel;
			this.allWheels = new Wheel[this.frontWheels.Length + this.rearWheels.Length + this.otherWheels.Length];
			this.frontWheels.CopyTo(this.allWheels, 0);
			this.rearWheels.CopyTo(this.allWheels, this.frontWheels.Length);
			if (this.otherWheels.Length != 0)
			{
				this.otherWheels.CopyTo(this.allWheels, this.frontWheels.Length + this.rearWheels.Length);
			}
		}
	}

	// Token: 0x04001481 RID: 5249
	public Wheel frontLeftWheel;

	// Token: 0x04001482 RID: 5250
	public Wheel frontRightWheel;

	// Token: 0x04001483 RID: 5251
	public Wheel rearLeftWheel;

	// Token: 0x04001484 RID: 5252
	public Wheel rearRightWheel;

	// Token: 0x04001485 RID: 5253
	public Wheel[] otherWheels = new Wheel[0];

	// Token: 0x04001486 RID: 5254
	[HideInInspector]
	public Wheel[] frontWheels = new Wheel[2];

	// Token: 0x04001487 RID: 5255
	[HideInInspector]
	public Wheel[] rearWheels = new Wheel[2];

	// Token: 0x04001488 RID: 5256
	[HideInInspector]
	public Wheel[] allWheels;
}
