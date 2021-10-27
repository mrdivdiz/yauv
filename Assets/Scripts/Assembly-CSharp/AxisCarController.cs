using System;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class AxisCarController : CarController
{
	// Token: 0x06000AC2 RID: 2754 RVA: 0x0007FF0C File Offset: 0x0007E10C
	protected override void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out int targetGear)
	{
		if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS)
		{
			steerInput = Input.GetAxisRaw("Horizontal");
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
			{
				throttleInput = Input.GetAxisRaw("Throttle");
				brakeInput = Input.GetAxisRaw("Brake");
			}
			else
			{
				throttleInput = Input.GetAxisRaw("Brake");
				brakeInput = Input.GetAxisRaw("Throttle");
			}
			handbrakeInput = Input.GetAxisRaw("Handbrake");
		}
		else
		{
			steerInput = Input.GetAxisRaw("Horizontal");
			if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("basic"))
			{
				throttleInput = ((!InputManager.GetButton("Fire1")) ? 0f : 1f);
				brakeInput = ((!InputManager.GetButton("Fire2")) ? 0f : 1f);
			}
			else
			{
				throttleInput = ((!Input.GetKey(KeyCode.JoystickButton11)) ? 0f : 1f);
				brakeInput = ((!Input.GetKey(KeyCode.JoystickButton10)) ? 0f : 1f);
			}
			handbrakeInput = ((!InputManager.GetButton("Cover")) ? 0f : 1f);
		}
		if (Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			handbrakeInput = 1f;
		}
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

	// Token: 0x06000AC3 RID: 2755 RVA: 0x000800B8 File Offset: 0x0007E2B8
	private void OnGUI()
	{
		if (this.debug)
		{
			GUI.Label(new Rect(0f, 160f, 700f, 200f), "                                                                                      accelKey = Input.GetKey (KeyCode.DownArrow)  : " + (this.accelKey == Input.GetKey(KeyCode.DownArrow)));
			GUI.Label(new Rect(0f, 180f, 500f, 200f), "throttle: " + this.throttle);
			GUI.Label(new Rect(0f, 200f, 200f, 200f), "m/s: " + this.fVelo);
			GUI.Label(new Rect(0f, 220f, 200f, 200f), "steering:   " + this.steering);
			GUI.Label(new Rect(0f, 240f, 200f, 200f), "steerInput: " + this.steerInput);
		}
	}
}
