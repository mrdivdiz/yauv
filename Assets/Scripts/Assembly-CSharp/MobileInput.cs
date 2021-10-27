using System;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class MobileInput : MonoBehaviour
{
	// Token: 0x06000934 RID: 2356 RVA: 0x0004C57C File Offset: 0x0004A77C
	public void Start()
	{
		float num = 100f;
		if ((float)Screen.width > 1500f)
		{
			num = 200f;
		}
		this.aimButton.pixelInset = new Rect((float)Screen.width - (num + 60f), 60f, num, num);
		this.jumpButton.pixelInset = new Rect((float)Screen.width - (num + 60f), 2f * num + 80f, num, num);
		this.climbButton.pixelInset = new Rect((float)Screen.width - (num + 60f), 2f * num + 80f, num, num);
		this.coverButton.pixelInset = new Rect((float)Screen.width - (num + 60f), num + 70f, num, num);
		this.rollButton.pixelInset = new Rect((float)Screen.width - (num + 60f), num + 70f, num, num);
		this.interactionButton.pixelInset = new Rect((float)Screen.width - (num + 60f), num + 70f, num, num);
		this.rollButton.enabled = false;
		this.coverButton.enabled = false;
		this.interactionButton.enabled = false;
		this.fireButton.pixelInset = new Rect((float)Screen.width - 2f * num - 70f, 60f, num, num);
		this.punchButton.pixelInset = new Rect((float)Screen.width - 2f * num - 70f, 60f, num, num);
		if (this.grenadeButton != null)
		{
			this.grenadeButton.pixelInset = new Rect((float)Screen.width - 3f * num - 80f, num + 70f, num, num);
			this.grenadeButton.enabled = false;
		}
		if (this.switchCoverButton != null)
		{
			this.switchCoverButton.pixelInset = new Rect((float)Screen.width - 2f * num - 75f, num + 65f, num * 1.1f, num * 1.1f);
			this.switchCoverButton.enabled = false;
		}
		this.pauseButton.pixelInset = new Rect(20f, (float)Screen.height - (num + 20f), num, num);
		this.punchButton.enabled = false;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0004C7E4 File Offset: 0x0004A9E4
	public void Awake()
	{
		this.highlitedButtonTexture = (Texture2D)Resources.Load("Generic_button_highlighted");
		if (AndroidPlatform.IsJoystickConnected())
		{
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
			{
				this.jumpButton.enabled = false;
				this.aimButton.enabled = false;
				this.fireButton.enabled = false;
				this.coverButton.enabled = false;
				this.rollButton.enabled = false;
				this.interactionButton.enabled = false;
				this.punchButton.enabled = false;
				this.climbButton.enabled = false;
				this.pauseButton.enabled = false;
			}
			else
			{
				UnityEngine.Object.Destroy(base.transform.root.gameObject);
			}
		}
		this.previousIsJoystickConnected = AndroidPlatform.IsJoystickConnected();
		MobileInput.jump = false;
		MobileInput.aim = false;
		MobileInput.fire = false;
		MobileInput.cover = false;
		MobileInput.roll = false;
		MobileInput.interaction = false;
		MobileInput.punch = false;
		MobileInput.pause = false;
		MobileInput.instance = base.gameObject.GetComponent<MobileInput>();
		if (Application.loadedLevelName == "QuadChase")
		{
			MobileInput.inputMode = MobileInput.InputModes.SHOOTING_ONLY;
		}
		if (Application.loadedLevelName == "Prologue" && ((!mainmenu.replayLevel && SaveHandler.checkpointReached <= 0) || (mainmenu.replayLevel && SaveHandler.replayCheckpointReached <= 0)))
		{
			MobileInput.grenadeDisabled = true;
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.InvokeRepeating("CheckJoystick", 0f, 1f);
		}
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0004C980 File Offset: 0x0004AB80
	public void CheckJoystick()
	{
		AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
		if (AndroidPlatform.IsJoystickConnected())
		{
			mainmenu.Instance.buttonStyle.hover.background = this.highlitedButtonTexture;
			mainmenu.Instance.buttonStyle.focused.background = this.highlitedButtonTexture;
		}
		else
		{
			mainmenu.Instance.buttonStyle.hover.background = mainmenu.Instance.buttonStyle.normal.background;
			mainmenu.Instance.buttonStyle.focused.background = mainmenu.Instance.buttonStyle.normal.background;
		}
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0004CA30 File Offset: 0x0004AC30
	public void OnDestroy()
	{
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.CancelInvoke("CheckJoystick");
		}
		if (this.highlitedButtonTexture != null)
		{
			Resources.UnloadAsset(this.highlitedButtonTexture);
		}
		this.interactionCaller = null;
		this.coverCaller = null;
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0004CA88 File Offset: 0x0004AC88
	private void Update()
	{
		if ((AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay) && this.previousIsJoystickConnected != AndroidPlatform.isJoystickConnected)
		{
			if (AndroidPlatform.isJoystickConnected)
			{
				this.jumpButton.enabled = false;
				this.aimButton.enabled = false;
				this.fireButton.enabled = false;
				this.coverButton.enabled = false;
				this.rollButton.enabled = false;
				this.interactionButton.enabled = false;
				this.punchButton.enabled = false;
				this.climbButton.enabled = false;
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
			if (touch.phase != TouchPhase.Moved && ((this.jumpButton.enabled && this.jumpButton.HitTest(touch.position)) || (this.climbButton.enabled && this.climbButton.HitTest(touch.position))))
			{
				if (!MobileInput.jump && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset = this.jumpButton.pixelInset;
					pixelInset.x += MobileInput.guiTouchOffset;
					pixelInset.y += MobileInput.guiTouchOffset;
					this.jumpButton.pixelInset = pixelInset;
					pixelInset = this.climbButton.pixelInset;
					pixelInset.x += MobileInput.guiTouchOffset;
					pixelInset.y += MobileInput.guiTouchOffset;
					this.climbButton.pixelInset = pixelInset;
					MobileInput.jump = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.pauseButton.enabled && this.pauseButton.HitTest(touch.position))
			{
				if (!MobileInput.pause && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset2 = this.pauseButton.pixelInset;
					pixelInset2.x += MobileInput.guiTouchOffset;
					pixelInset2.y += MobileInput.guiTouchOffset;
					this.pauseButton.pixelInset = pixelInset2;
					MobileInput.pause = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.aimButton.enabled && this.aimButton.HitTest(touch.position))
			{
				if (touch.phase == TouchPhase.Began)
				{
					if (MobileInput.aim)
					{
						Rect pixelInset3 = this.aimButton.pixelInset;
						pixelInset3.x -= MobileInput.guiTouchOffset;
						pixelInset3.y -= MobileInput.guiTouchOffset;
						this.aimButton.pixelInset = pixelInset3;
					}
					else
					{
						Rect pixelInset4 = this.aimButton.pixelInset;
						pixelInset4.x += MobileInput.guiTouchOffset;
						pixelInset4.y += MobileInput.guiTouchOffset;
						this.aimButton.pixelInset = pixelInset4;
					}
					MobileInput.aim = !MobileInput.aim;
					this.fireButton.enabled = MobileInput.aim;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.fireButton.enabled && this.fireButton.HitTest(touch.position))
			{
				if (touch.phase == TouchPhase.Began)
				{
					MobileInput.fire = true;
					this.startedOnBackground = false;
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					MobileInput.fire = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.coverButton.HitTest(touch.position) && this.coverButton.enabled)
			{
				if (!MobileInput.cover && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset5 = this.coverButton.pixelInset;
					pixelInset5.x += MobileInput.guiTouchOffset;
					pixelInset5.y += MobileInput.guiTouchOffset;
					this.coverButton.pixelInset = pixelInset5;
					MobileInput.cover = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.switchCoverButton != null && this.switchCoverButton.HitTest(touch.position) && this.switchCoverButton.enabled)
			{
				if (!MobileInput.cover && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset6 = this.switchCoverButton.pixelInset;
					pixelInset6.x += MobileInput.guiTouchOffset;
					pixelInset6.y += MobileInput.guiTouchOffset;
					this.switchCoverButton.pixelInset = pixelInset6;
					MobileInput.switchCover = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.rollButton.HitTest(touch.position) && this.rollButton.enabled)
			{
				if (!MobileInput.roll && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset7 = this.rollButton.pixelInset;
					pixelInset7.x += MobileInput.guiTouchOffset;
					pixelInset7.y += MobileInput.guiTouchOffset;
					this.rollButton.pixelInset = pixelInset7;
					MobileInput.roll = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.grenadeButton.HitTest(touch.position) && this.grenadeButton.enabled)
			{
				if (!MobileInput.throwGrenade && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset8 = this.grenadeButton.pixelInset;
					pixelInset8.x += MobileInput.guiTouchOffset;
					pixelInset8.y += MobileInput.guiTouchOffset;
					this.grenadeButton.pixelInset = pixelInset8;
					MobileInput.throwGrenade = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.interactionButton.HitTest(touch.position) && this.interactionButton.enabled)
			{
				if (!MobileInput.interaction && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset9 = this.interactionButton.pixelInset;
					pixelInset9.x += MobileInput.guiTouchOffset;
					pixelInset9.y += MobileInput.guiTouchOffset;
					this.interactionButton.pixelInset = pixelInset9;
					MobileInput.interaction = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && this.punchButton.HitTest(touch.position) && this.punchButton.enabled)
			{
				if (!MobileInput.punch && touch.phase == TouchPhase.Began)
				{
					Rect pixelInset10 = this.punchButton.pixelInset;
					pixelInset10.x += MobileInput.guiTouchOffset;
					pixelInset10.y += MobileInput.guiTouchOffset;
					this.punchButton.pixelInset = pixelInset10;
					MobileInput.punch = true;
					this.startedOnBackground = false;
				}
			}
			else if (touch.phase != TouchPhase.Moved && WeaponsHUD.weaponsHUDRect.Contains(new Vector2(touch.position.x * 1366f / (float)Screen.width, touch.position.y * 768f / (float)Screen.height)))
			{
				if (!MobileInput.changeWeapon && touch.phase == TouchPhase.Began)
				{
					MobileInput.changeWeapon = true;
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
					if (!MobileInput.aim)
					{
						MobileInput.mouseX = touch.deltaPosition.x * 0.25f;
						MobileInput.mouseY = touch.deltaPosition.y * 0.25f;
					}
					else
					{
						MobileInput.mouseX = touch.deltaPosition.x * 0.15f;
						MobileInput.mouseY = touch.deltaPosition.y * 0.15f;
					}
				}
			}
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0004D3B0 File Offset: 0x0004B5B0
	private void Rest()
	{
		if (!AndroidPlatform.isJoystickConnected && (this.previousInputMode != MobileInput.inputMode || this.firstReset))
		{
			switch (MobileInput.inputMode)
			{
			case MobileInput.InputModes.ALL_CONTROLS:
				this.jumpButton.enabled = true;
				this.aimButton.enabled = true;
				this.fireButton.enabled = false;
				this.coverButton.enabled = false;
				this.rollButton.enabled = false;
				this.interactionButton.enabled = false;
				this.punchButton.enabled = false;
				this.climbButton.enabled = false;
				break;
			case MobileInput.InputModes.MOVING_ONLY:
				this.jumpButton.enabled = false;
				this.aimButton.enabled = false;
				this.fireButton.enabled = false;
				this.coverButton.enabled = false;
				this.rollButton.enabled = false;
				this.interactionButton.enabled = false;
				this.punchButton.enabled = false;
				this.climbButton.enabled = false;
				break;
			case MobileInput.InputModes.SHOOTING_ONLY:
				this.jumpButton.enabled = false;
				this.rollButton.enabled = false;
				this.interactionButton.enabled = false;
				this.aimButton.enabled = true;
				this.fireButton.enabled = false;
				this.coverButton.enabled = false;
				this.punchButton.enabled = false;
				this.climbButton.enabled = false;
				break;
			case MobileInput.InputModes.AGILITY_ONLY:
				this.aimButton.enabled = false;
				this.fireButton.enabled = false;
				this.coverButton.enabled = false;
				this.punchButton.enabled = false;
				this.interactionButton.enabled = false;
				this.jumpButton.enabled = true;
				this.rollButton.enabled = false;
				this.climbButton.enabled = false;
				if (Application.loadedLevelName == "Chase2")
				{
					this.jumpButton.enabled = false;
				}
				break;
			}
			this.pauseButton.enabled = true;
			this.previousInputMode = MobileInput.inputMode;
			this.firstReset = false;
		}
		MobileInput.mouseX = 0f;
		MobileInput.mouseY = 0f;
		if (MobileInput.jump)
		{
			Rect pixelInset = this.jumpButton.pixelInset;
			pixelInset.x -= MobileInput.guiTouchOffset;
			pixelInset.y -= MobileInput.guiTouchOffset;
			this.jumpButton.pixelInset = pixelInset;
			pixelInset = this.climbButton.pixelInset;
			pixelInset.x -= MobileInput.guiTouchOffset;
			pixelInset.y -= MobileInput.guiTouchOffset;
			this.climbButton.pixelInset = pixelInset;
			MobileInput.jump = false;
		}
		if (MobileInput.pause)
		{
			Rect pixelInset2 = this.pauseButton.pixelInset;
			pixelInset2.x -= MobileInput.guiTouchOffset;
			pixelInset2.y -= MobileInput.guiTouchOffset;
			this.pauseButton.pixelInset = pixelInset2;
			MobileInput.pause = false;
		}
		if (MobileInput.cover)
		{
			Rect pixelInset3 = this.coverButton.pixelInset;
			pixelInset3.x -= MobileInput.guiTouchOffset;
			pixelInset3.y -= MobileInput.guiTouchOffset;
			this.coverButton.pixelInset = pixelInset3;
			MobileInput.cover = false;
			this.grenadeButton.enabled = false;
		}
		if (MobileInput.switchCover)
		{
			Rect pixelInset4 = this.switchCoverButton.pixelInset;
			pixelInset4.x -= MobileInput.guiTouchOffset;
			pixelInset4.y -= MobileInput.guiTouchOffset;
			this.switchCoverButton.pixelInset = pixelInset4;
			MobileInput.switchCover = false;
		}
		if (MobileInput.roll)
		{
			Rect pixelInset5 = this.rollButton.pixelInset;
			pixelInset5.x -= MobileInput.guiTouchOffset;
			pixelInset5.y -= MobileInput.guiTouchOffset;
			this.rollButton.pixelInset = pixelInset5;
			MobileInput.roll = false;
		}
		if (MobileInput.throwGrenade)
		{
			Rect pixelInset6 = this.grenadeButton.pixelInset;
			pixelInset6.x -= MobileInput.guiTouchOffset;
			pixelInset6.y -= MobileInput.guiTouchOffset;
			this.grenadeButton.pixelInset = pixelInset6;
			MobileInput.throwGrenade = false;
		}
		if (MobileInput.interaction)
		{
			Rect pixelInset7 = this.interactionButton.pixelInset;
			pixelInset7.x -= MobileInput.guiTouchOffset;
			pixelInset7.y -= MobileInput.guiTouchOffset;
			this.interactionButton.pixelInset = pixelInset7;
			MobileInput.interaction = false;
		}
		if (MobileInput.punch)
		{
			Rect pixelInset8 = this.punchButton.pixelInset;
			pixelInset8.x -= MobileInput.guiTouchOffset;
			pixelInset8.y -= MobileInput.guiTouchOffset;
			this.punchButton.pixelInset = pixelInset8;
			MobileInput.punch = false;
		}
		if (MobileInput.changeWeapon)
		{
			MobileInput.changeWeapon = false;
		}
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0004D8C0 File Offset: 0x0004BAC0
	public void enableButton(string buttonName, GameObject caller)
	{
		if (AndroidPlatform.IsJoystickConnected())
		{
			return;
		}
		switch (buttonName)
		{
		case "punch":
			if (MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.AGILITY_ONLY)
			{
				this.fireButton.enabled = false;
				this.punchButton.enabled = true;
				this.punchCaller = caller;
			}
			break;
		case "interaction":
			if (MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.rollButton.enabled = false;
				this.coverBeforeInteractionEnabled = this.coverButton.enabled;
				if (this.coverBeforeInteractionEnabled)
				{
					this.coverCaller = this.interactionCaller;
				}
				this.coverButton.enabled = false;
				this.interactionButton.enabled = true;
				this.interactionCaller = caller;
			}
			if (MobileInput.inputMode == MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.coverBeforeInteractionEnabled = this.coverButton.enabled;
				if (this.coverBeforeInteractionEnabled)
				{
					this.coverCaller = this.interactionCaller;
				}
				this.coverButton.enabled = false;
				this.interactionButton.enabled = true;
				this.interactionCaller = caller;
			}
			if (MobileInput.inputMode == MobileInput.InputModes.MOVING_ONLY)
			{
				this.interactionButton.enabled = true;
				this.interactionCaller = caller;
			}
			break;
		case "cover":
			if (MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.AGILITY_ONLY && MobileInput.inputMode != MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.rollButton.enabled = false;
				this.coverButton.enabled = true;
				this.interactionButton.enabled = false;
				this.interactionCaller = caller;
			}
			if (MobileInput.inputMode == MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.interactionButton.enabled = false;
				this.coverButton.enabled = true;
				this.interactionCaller = caller;
			}
			break;
		case "climb":
			if (MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.climbButton.enabled = true;
				this.jumpButton.enabled = false;
				this.climbCaller = caller;
			}
			break;
		case "SwitchCover":
			this.switchCoverButton.enabled = true;
			break;
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0004DB44 File Offset: 0x0004BD44
	public void disableButton(string buttonName, GameObject caller)
	{
		if (AndroidPlatform.IsJoystickConnected())
		{
			return;
		}
		switch (buttonName)
		{
		case "punch":
			if (caller == this.punchCaller && MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.AGILITY_ONLY)
			{
				this.fireButton.enabled = MobileInput.aim;
				this.punchButton.enabled = false;
			}
			break;
		case "interaction":
			if (caller == this.interactionCaller && MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.SHOOTING_ONLY && MobileInput.inputMode != MobileInput.InputModes.AGILITY_ONLY)
			{
				this.rollButton.enabled = false;
				this.coverButton.enabled = this.coverBeforeInteractionEnabled;
				if (this.coverBeforeInteractionEnabled)
				{
					this.interactionCaller = this.coverCaller;
				}
				this.interactionButton.enabled = false;
			}
			if (caller == this.interactionCaller && (MobileInput.inputMode == MobileInput.InputModes.SHOOTING_ONLY || MobileInput.inputMode == MobileInput.InputModes.AGILITY_ONLY))
			{
				this.interactionButton.enabled = false;
			}
			if (caller == this.interactionCaller && MobileInput.inputMode == MobileInput.InputModes.MOVING_ONLY)
			{
				this.interactionButton.enabled = false;
			}
			break;
		case "cover":
			if (caller == this.interactionCaller && MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.AGILITY_ONLY && MobileInput.inputMode != MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.rollButton.enabled = false;
				this.coverButton.enabled = false;
				this.interactionButton.enabled = false;
			}
			if (caller == this.interactionCaller && MobileInput.inputMode == MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.coverButton.enabled = false;
			}
			break;
		case "climb":
			if (caller == this.climbCaller && MobileInput.inputMode != MobileInput.InputModes.MOVING_ONLY && MobileInput.inputMode != MobileInput.InputModes.SHOOTING_ONLY)
			{
				this.climbButton.enabled = false;
				this.jumpButton.enabled = true;
			}
			break;
		case "SwitchCover":
			this.switchCoverButton.enabled = false;
			break;
		}
	}

	// Token: 0x04000C71 RID: 3185
	public static MobileInput.InputModes inputMode;

	// Token: 0x04000C72 RID: 3186
	public GUITexture jumpButton;

	// Token: 0x04000C73 RID: 3187
	public GUITexture aimButton;

	// Token: 0x04000C74 RID: 3188
	public GUITexture fireButton;

	// Token: 0x04000C75 RID: 3189
	public GUITexture coverButton;

	// Token: 0x04000C76 RID: 3190
	public GUITexture rollButton;

	// Token: 0x04000C77 RID: 3191
	public GUITexture interactionButton;

	// Token: 0x04000C78 RID: 3192
	public GUITexture punchButton;

	// Token: 0x04000C79 RID: 3193
	public GUITexture pauseButton;

	// Token: 0x04000C7A RID: 3194
	public GUITexture climbButton;

	// Token: 0x04000C7B RID: 3195
	public GUITexture switchCoverButton;

	// Token: 0x04000C7C RID: 3196
	public GUITexture grenadeButton;

	// Token: 0x04000C7D RID: 3197
	public static float mouseX;

	// Token: 0x04000C7E RID: 3198
	public static float mouseY;

	// Token: 0x04000C7F RID: 3199
	public static bool jump;

	// Token: 0x04000C80 RID: 3200
	public static bool pause;

	// Token: 0x04000C81 RID: 3201
	public static bool aim;

	// Token: 0x04000C82 RID: 3202
	public static bool fire;

	// Token: 0x04000C83 RID: 3203
	public static bool cover;

	// Token: 0x04000C84 RID: 3204
	public static bool switchCover;

	// Token: 0x04000C85 RID: 3205
	public static bool roll;

	// Token: 0x04000C86 RID: 3206
	public static bool interaction;

	// Token: 0x04000C87 RID: 3207
	public static bool punch;

	// Token: 0x04000C88 RID: 3208
	public static bool changeWeapon;

	// Token: 0x04000C89 RID: 3209
	public static bool throwGrenade;

	// Token: 0x04000C8A RID: 3210
	public static float guiTouchOffset = 5f;

	// Token: 0x04000C8B RID: 3211
	private GameObject punchCaller;

	// Token: 0x04000C8C RID: 3212
	public static MobileInput instance;

	// Token: 0x04000C8D RID: 3213
	private GameObject interactionCaller;

	// Token: 0x04000C8E RID: 3214
	private GameObject coverCaller;

	// Token: 0x04000C8F RID: 3215
	private GameObject climbCaller;

	// Token: 0x04000C90 RID: 3216
	private MobileInput.InputModes previousInputMode;

	// Token: 0x04000C91 RID: 3217
	private bool firstReset = true;

	// Token: 0x04000C92 RID: 3218
	public static bool joystickShown;

	// Token: 0x04000C93 RID: 3219
	public static bool gotGrenade;

	// Token: 0x04000C94 RID: 3220
	public static bool grenadeDisabled;

	// Token: 0x04000C95 RID: 3221
	private float mousexpos;

	// Token: 0x04000C96 RID: 3222
	private bool startedOnBackground;

	// Token: 0x04000C97 RID: 3223
	private bool coverBeforeInteractionEnabled;

	// Token: 0x04000C98 RID: 3224
	private bool previousIsJoystickConnected;

	// Token: 0x04000C99 RID: 3225
	private Texture2D highlitedButtonTexture;

	// Token: 0x020001D0 RID: 464
	public enum InputModes
	{
		// Token: 0x04000C9D RID: 3229
		ALL_CONTROLS,
		// Token: 0x04000C9E RID: 3230
		MOVING_ONLY,
		// Token: 0x04000C9F RID: 3231
		SHOOTING_ONLY,
		// Token: 0x04000CA0 RID: 3232
		AGILITY_ONLY
	}
}
