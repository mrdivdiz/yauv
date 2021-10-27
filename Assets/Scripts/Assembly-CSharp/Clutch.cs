using System;
using UnityEngine;

// Token: 0x02000246 RID: 582
public class Clutch
{
	// Token: 0x06000B0F RID: 2831 RVA: 0x00087750 File Offset: 0x00085950
	public float GetDrag(float engine_speed, float drive_speed, float powerMultiplier)
	{
		this.pressure = this.clutch_position * this.maxPressure;
		float num = this.pressure * this.area;
		if (Mathf.Abs(engine_speed - drive_speed) < this.threshold * num)
		{
			this.locked = true;
			return 0f;
		}
		this.locked = false;
		float num2 = this.slidingFriction * num * powerMultiplier;
		return Mathf.Sign(engine_speed - drive_speed) * num2 * this.radius;
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x000877C8 File Offset: 0x000859C8
	public void SetClutchPosition(float value)
	{
		this.clutch_position = value;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x000877D4 File Offset: 0x000859D4
	public float GetClutchPosition()
	{
		return this.clutch_position;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x000877DC File Offset: 0x000859DC
	public bool IsLocked()
	{
		return this.locked;
	}

	// Token: 0x04001325 RID: 4901
	public float slidingFriction = 0.27f;

	// Token: 0x04001326 RID: 4902
	public float radius = 0.15f;

	// Token: 0x04001327 RID: 4903
	public float area = 1f;

	// Token: 0x04001328 RID: 4904
	public float maxPressure = 20000f;

	// Token: 0x04001329 RID: 4905
	public float maxTorque;

	// Token: 0x0400132A RID: 4906
	public float threshold = 0.005f;

	// Token: 0x0400132B RID: 4907
	private float pressure;

	// Token: 0x0400132C RID: 4908
	private float clutch_position;

	// Token: 0x0400132D RID: 4909
	private bool locked;

	// Token: 0x0400132E RID: 4910
	private float max_torque;

	// Token: 0x0400132F RID: 4911
	private float friction_torque;

	// Token: 0x04001330 RID: 4912
	private float new_speed_diff;

	// Token: 0x04001331 RID: 4913
	private float normal_force;

	// Token: 0x04001332 RID: 4914
	private float n_engine_max_torque;

	// Token: 0x04001333 RID: 4915
	private float last_torque;
}
