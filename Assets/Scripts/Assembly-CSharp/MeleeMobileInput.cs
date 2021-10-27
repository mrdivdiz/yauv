using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class MeleeMobileInput : MonoBehaviour
{
	// Token: 0x0600092C RID: 2348 RVA: 0x0004BDD8 File Offset: 0x00049FD8
	public void Start()
	{
		float num = 100f;
		if (Screen.width > 1500)
		{
			num = 200f;
		}
		this.rightPunchButton.pixelInset = new Rect((float)Screen.width - (2f * num + 100f), num - 20f, num, num);
		this.rightKickButton.pixelInset = new Rect((float)Screen.width - (num + 60f), num - 20f, num, num);
		if (num == 200f)
		{
			this.blockButton.pixelInset = new Rect((float)Screen.width - (num + 180f), 2f * num - 40f, num, num);
		}
		else
		{
			this.blockButton.pixelInset = new Rect((float)Screen.width - (num + 130f), 2f * num - 40f, num, num);
		}
		this.pauseButton.pixelInset = new Rect(20f, (float)Screen.height - (num + 20f), num, num);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0004BEE4 File Offset: 0x0004A0E4
	public void Awake()
	{
		if (AndroidPlatform.IsJoystickConnected())
		{
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
			{
				this.rightPunchButton.enabled = false;
				this.rightKickButton.enabled = false;
				this.blockButton.enabled = false;
				this.pauseButton.enabled = false;
			}
			else
			{
				UnityEngine.Object.Destroy(base.transform.root.gameObject);
			}
		}
		this.previousIsJoystickConnected = AndroidPlatform.IsJoystickConnected();
		MeleeMobileInput.rightPunch = false;
		MeleeMobileInput.leftPunch = false;
		MeleeMobileInput.rightKick = false;
		MeleeMobileInput.leftKick = false;
		MeleeMobileInput.block = false;
		MobileInput.pause = false;
		MeleeMobileInput.instance = base.gameObject.GetComponent<MobileInput>();
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.InvokeRepeating("CheckJoystick", 0f, 1f);
		}
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0004BFC4 File Offset: 0x0004A1C4
	public void CheckJoystick()
	{
		AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0004BFD8 File Offset: 0x0004A1D8
	public void OnDestroy()
	{
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.CancelInvoke("CheckJoystick");
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0004C008 File Offset: 0x0004A208
	private void Update()
	{
		if ((AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay) && this.previousIsJoystickConnected != AndroidPlatform.isJoystickConnected)
		{
			if (AndroidPlatform.isJoystickConnected)
			{
				this.rightPunchButton.enabled = false;
				this.rightKickButton.enabled = false;
				this.blockButton.enabled = false;
				this.pauseButton.enabled = false;
			}
			else
			{
				this.firstReset = true;
			}
			this.previousIsJoystickConnected = AndroidPlatform.isJoystickConnected;
		}
		this.Rest();
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (this.rightPunchButton.enabled && this.rightPunchButton.HitTest(touch.position))
			{
				if (!MeleeMobileInput.rightPunch && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset = this.rightPunchButton.pixelInset;
					pixelInset.x += this.guiTouchOffset;
					pixelInset.y += this.guiTouchOffset;
					this.rightPunchButton.pixelInset = pixelInset;
					MeleeMobileInput.rightPunch = true;
					this.startedOnBackground = false;
				}
			}
			else if (this.pauseButton.enabled && this.pauseButton.HitTest(touch.position))
			{
				if (!MobileInput.pause && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset2 = this.pauseButton.pixelInset;
					pixelInset2.x += this.guiTouchOffset;
					pixelInset2.y += this.guiTouchOffset;
					this.pauseButton.pixelInset = pixelInset2;
					MobileInput.pause = true;
					this.startedOnBackground = false;
				}
			}
			else if (this.rightKickButton.enabled && this.rightKickButton.HitTest(touch.position))
			{
				if (!MeleeMobileInput.rightKick && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset3 = this.rightKickButton.pixelInset;
					pixelInset3.x += this.guiTouchOffset;
					pixelInset3.y += this.guiTouchOffset;
					this.rightKickButton.pixelInset = pixelInset3;
					MeleeMobileInput.rightKick = true;
					this.startedOnBackground = false;
				}
			}
			else if (this.blockButton.enabled && this.blockButton.HitTest(touch.position))
			{
				if (!MeleeMobileInput.block && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset4 = this.blockButton.pixelInset;
					pixelInset4.x += this.guiTouchOffset;
					pixelInset4.y += this.guiTouchOffset;
					this.blockButton.pixelInset = pixelInset4;
					MeleeMobileInput.block = true;
					this.startedOnBackground = false;
				}
				else if (MeleeMobileInput.block && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
				{
					Rect pixelInset5 = this.blockButton.pixelInset;
					pixelInset5.x -= this.guiTouchOffset;
					pixelInset5.y -= this.guiTouchOffset;
					this.blockButton.pixelInset = pixelInset5;
					MeleeMobileInput.block = false;
					this.startedOnBackground = false;
				}
			}
			else if (touch.position.x > (float)Screen.width / 3f)
			{
				if (touch.phase == TouchPhase.Began)
				{
					this.startedOnBackground = true;
				}
				if (touch.phase == TouchPhase.Moved && this.startedOnBackground)
				{
					MeleeMobileInput.mouseX = touch.deltaPosition.x * 0.25f;
					MeleeMobileInput.mouseY = touch.deltaPosition.y * 0.25f;
				}
			}
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0004C400 File Offset: 0x0004A600
	private void Rest()
	{
		if (!AndroidPlatform.isJoystickConnected && this.firstReset)
		{
			this.rightPunchButton.enabled = true;
			this.rightKickButton.enabled = true;
			this.blockButton.enabled = true;
			this.pauseButton.enabled = true;
			this.firstReset = false;
		}
		MeleeMobileInput.mouseX = 0f;
		MeleeMobileInput.mouseY = 0f;
		if (MeleeMobileInput.rightPunch)
		{
			Rect pixelInset = this.rightPunchButton.pixelInset;
			pixelInset.x -= this.guiTouchOffset;
			pixelInset.y -= this.guiTouchOffset;
			this.rightPunchButton.pixelInset = pixelInset;
			MeleeMobileInput.rightPunch = false;
		}
		if (MobileInput.pause)
		{
			Rect pixelInset2 = this.pauseButton.pixelInset;
			pixelInset2.x -= this.guiTouchOffset;
			pixelInset2.y -= this.guiTouchOffset;
			this.pauseButton.pixelInset = pixelInset2;
			MobileInput.pause = false;
		}
		if (MeleeMobileInput.rightKick)
		{
			Rect pixelInset3 = this.rightKickButton.pixelInset;
			pixelInset3.x -= this.guiTouchOffset;
			pixelInset3.y -= this.guiTouchOffset;
			this.rightKickButton.pixelInset = pixelInset3;
			MeleeMobileInput.rightKick = false;
		}
	}

	// Token: 0x04000C56 RID: 3158
	public static MeleeMobileInput.InputModes inputMode;

	// Token: 0x04000C57 RID: 3159
	public GUITexture rightPunchButton;

	// Token: 0x04000C58 RID: 3160
	public GUITexture rightKickButton;

	// Token: 0x04000C59 RID: 3161
	public GUITexture blockButton;

	// Token: 0x04000C5A RID: 3162
	public GUITexture pauseButton;

	// Token: 0x04000C5B RID: 3163
	public static float mouseX;

	// Token: 0x04000C5C RID: 3164
	public static float mouseY;

	// Token: 0x04000C5D RID: 3165
	public static bool rightPunch;

	// Token: 0x04000C5E RID: 3166
	public static bool pause;

	// Token: 0x04000C5F RID: 3167
	public static bool rightKick;

	// Token: 0x04000C60 RID: 3168
	public static bool leftKick;

	// Token: 0x04000C61 RID: 3169
	public static bool leftPunch;

	// Token: 0x04000C62 RID: 3170
	public static bool block;

	// Token: 0x04000C63 RID: 3171
	private float guiTouchOffset = 5f;

	// Token: 0x04000C64 RID: 3172
	private GameObject punchCaller;

	// Token: 0x04000C65 RID: 3173
	public static MobileInput instance;

	// Token: 0x04000C66 RID: 3174
	private GameObject interactionCaller;

	// Token: 0x04000C67 RID: 3175
	private GameObject climbCaller;

	// Token: 0x04000C68 RID: 3176
	private MeleeMobileInput.InputModes previousInputMode;

	// Token: 0x04000C69 RID: 3177
	private bool firstReset = true;

	// Token: 0x04000C6A RID: 3178
	private bool startedOnBackground;

	// Token: 0x04000C6B RID: 3179
	private bool previousIsJoystickConnected;

	// Token: 0x020001CE RID: 462
	public enum InputModes
	{
		// Token: 0x04000C6D RID: 3181
		ALL_CONTROLS,
		// Token: 0x04000C6E RID: 3182
		MOVING_ONLY,
		// Token: 0x04000C6F RID: 3183
		SHOOTING_ONLY,
		// Token: 0x04000C70 RID: 3184
		AGILITY_ONLY
	}
}
