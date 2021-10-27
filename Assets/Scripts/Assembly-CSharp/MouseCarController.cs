using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class MouseCarController : CarController
{
	// Token: 0x06000B18 RID: 2840 RVA: 0x00087F38 File Offset: 0x00086138
	protected override void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out int targetGear)
	{
		if (Input.GetMouseButton(0))
		{
			throttleInput = 1f;
		}
		else if (Input.GetButton("Fire1"))
		{
			transform.GetComponent<Rigidbody>().AddForce(transform.forward * -4500);
			throttleInput = 0f;
		}
		else
		{
			throttleInput = 0f;
		}
		if (Input.GetMouseButton(1))
		{
			brakeInput = 1f;
		}
		else
		{
			brakeInput = 0f;
		}
		if (Input.GetMouseButton(2))
		{
			handbrakeInput = 1f;
		}
		else
		{
			handbrakeInput = 0f;
		}
		steerInput = (Input.mousePosition.x - (float)Screen.width / 2f) / (float)Screen.width * 2f;
		clutchInput = Input.GetAxisRaw("Clutch");
		targetGear = this.drivetrain.gear;
		if (InputManager.GetButtonDown("ShiftUp"))
		{
			targetGear++;
		}
		if (InputManager.GetButtonDown("ShiftDown"))
		{
			targetGear--;
		}
	}
}
