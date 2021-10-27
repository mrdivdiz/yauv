using System;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class MobileCarController : CarController
{
	// Token: 0x06000B15 RID: 2837 RVA: 0x000877F0 File Offset: 0x000859F0
	protected override void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out int targetGear)
	{
		clutchInput = 0f;
		steerInput = Mathf.Clamp(Input.acceleration.x, -1f, 1f);
		throttleInput = 0f;
		brakeInput = 0f;
		handbrakeInput = 0f;
		bool flag = false;
		bool flag2 = false;
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.phase != TouchPhase.Ended && this.throttleButton != null && this.throttleButton.texture != null && this.throttleButton.texture.HitTest(touch.position))
			{
				throttleInput = 1f;
				Color color = this.throttleButton.texture.color;
				color.a = this.throttleButton.alphaButtonDown;
				this.throttleButton.texture.color = color;
			}
			if (touch.phase != TouchPhase.Ended && this.brakeButton != null && this.brakeButton.texture != null && this.brakeButton.texture.HitTest(touch.position))
			{
				brakeInput = 1f;
				Color color2 = this.brakeButton.texture.color;
				color2.a = this.brakeButton.alphaButtonDown;
				this.brakeButton.texture.color = color2;
			}
			if (touch.phase != TouchPhase.Ended && this.handbrakeButton != null && this.handbrakeButton.texture != null && this.handbrakeButton.texture.HitTest(touch.position))
			{
				handbrakeInput = 1f;
				Color color3 = this.handbrakeButton.texture.color;
				color3.a = this.handbrakeButton.alphaButtonDown;
				this.handbrakeButton.texture.color = color3;
			}
			if (touch.phase == TouchPhase.Began && this.gearUpButton != null && this.gearUpButton.texture != null && this.gearUpButton.texture.HitTest(touch.position))
			{
				flag = true;
				Color color4 = this.gearUpButton.texture.color;
				color4.a = this.gearUpButton.alphaButtonDown;
				this.gearUpButton.texture.color = color4;
			}
			if (touch.phase == TouchPhase.Began && this.gearDownButton != null && this.gearDownButton.texture != null && this.gearDownButton.texture.HitTest(touch.position))
			{
				flag2 = true;
				Color color5 = this.gearDownButton.texture.color;
				color5.a = this.gearDownButton.alphaButtonDown;
				this.gearDownButton.texture.color = color5;
			}
			if (touch.phase == TouchPhase.Began && this.cameraButton != null && this.cameraButton.texture != null && this.cameraButton.texture.HitTest(touch.position))
			{
				MobileCarController.changeCamera = true;
				Color color6 = this.cameraButton.texture.color;
				color6.a = this.cameraButton.alphaButtonDown;
				this.cameraButton.texture.color = color6;
			}
			if (touch.phase == TouchPhase.Began && this.pauseButton != null && this.pauseButton.texture != null && this.pauseButton.texture.HitTest(touch.position))
			{
				MobileInput.pause = true;
				Color color7 = this.pauseButton.texture.color;
				color7.a = this.pauseButton.alphaButtonDown;
				this.pauseButton.texture.color = color7;
			}
		}
		targetGear = this.drivetrain.gear;
		if (flag && !flag2)
		{
			targetGear++;
		}
		else if (flag2 && !flag)
		{
			targetGear--;
		}
		if (this.throttleButton != null && this.throttleButton.texture != null && throttleInput < 1E-45f)
		{
			Color color8 = this.throttleButton.texture.color;
			color8.a = this.throttleButton.alphaButtonReleased;
			this.throttleButton.texture.color = color8;
		}
		if (this.brakeButton != null && this.brakeButton.texture != null && brakeInput < 1E-45f)
		{
			Color color9 = this.brakeButton.texture.color;
			color9.a = this.brakeButton.alphaButtonReleased;
			this.brakeButton.texture.color = color9;
		}
		if (this.handbrakeButton != null && this.handbrakeButton.texture != null && handbrakeInput < 1E-45f)
		{
			Color color10 = this.handbrakeButton.texture.color;
			color10.a = this.handbrakeButton.alphaButtonReleased;
			this.handbrakeButton.texture.color = color10;
		}
		if (this.gearUpButton != null && this.gearUpButton.texture != null && !flag)
		{
			Color color11 = this.gearUpButton.texture.color;
			color11.a = this.gearUpButton.alphaButtonReleased;
			this.gearUpButton.texture.color = color11;
		}
		if (this.gearDownButton != null && this.gearDownButton.texture != null && !flag2)
		{
			Color color12 = this.gearDownButton.texture.color;
			color12.a = this.gearDownButton.alphaButtonReleased;
			this.gearDownButton.texture.color = color12;
		}
		if (this.cameraButton != null && this.cameraButton.texture != null && !MobileCarController.changeCamera)
		{
			Color color13 = this.cameraButton.texture.color;
			color13.a = this.cameraButton.alphaButtonReleased;
			this.cameraButton.texture.color = color13;
		}
		if (this.pauseButton != null && this.pauseButton.texture != null && !MobileInput.pause)
		{
			Color color14 = this.pauseButton.texture.color;
			color14.a = this.pauseButton.alphaButtonReleased;
			this.pauseButton.texture.color = color14;
		}
	}

	// Token: 0x04001334 RID: 4916
	public MobileCarController.TouchButton throttleButton;

	// Token: 0x04001335 RID: 4917
	public MobileCarController.TouchButton brakeButton;

	// Token: 0x04001336 RID: 4918
	public MobileCarController.TouchButton handbrakeButton;

	// Token: 0x04001337 RID: 4919
	public MobileCarController.TouchButton gearUpButton;

	// Token: 0x04001338 RID: 4920
	public MobileCarController.TouchButton gearDownButton;

	// Token: 0x04001339 RID: 4921
	public MobileCarController.TouchButton cameraButton;

	// Token: 0x0400133A RID: 4922
	public MobileCarController.TouchButton pauseButton;

	// Token: 0x0400133B RID: 4923
	public static bool changeCamera;

	// Token: 0x02000248 RID: 584
	[Serializable]
	public class TouchButton
	{
		// Token: 0x0400133C RID: 4924
		public GUITexture texture;

		// Token: 0x0400133D RID: 4925
		public float alphaButtonDown = 0.2f;

		// Token: 0x0400133E RID: 4926
		public float alphaButtonReleased = 0.3f;
	}
}
