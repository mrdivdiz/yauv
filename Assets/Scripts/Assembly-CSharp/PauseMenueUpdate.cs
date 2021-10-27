using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class PauseMenueUpdate : MonoBehaviour
{
	// Token: 0x0600090C RID: 2316 RVA: 0x0004A8B8 File Offset: 0x00048AB8
	private void Awake()
	{
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0004A8BC File Offset: 0x00048ABC
	private void Start()
	{
		mainmenu.Instance.enabled = false;
		mainmenu.Instance.StartMethod();
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0004A8D4 File Offset: 0x00048AD4
	public void OnDestory()
	{
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0004A8D8 File Offset: 0x00048AD8
	private void Update()
	{
		this.pauseButton = false;
		this.pauseButton = MobileInput.pause;
		if (this.pauseButton)
		{
			if (MobileInput.instance != null)
			{
				Rect pixelInset = MobileInput.instance.pauseButton.pixelInset;
				pixelInset.x -= MobileInput.guiTouchOffset;
				pixelInset.y -= MobileInput.guiTouchOffset;
				MobileInput.instance.pauseButton.pixelInset = pixelInset;
			}
			MobileInput.pause = false;
		}
		this.pauseButton |= InputManager.GetButtonDown("Start");
		if (mainmenu.Instance != null && mainmenu.Instance.isPauseMenu && ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick && Input.GetKeyDown(KeyCode.Escape)) || (this.cameraConnected && mainmenu.Instance.currentMenu == mainmenu.menus.CAMERADISCONNECTED) || this.cameraDisconnected || this.systemPause || this.pauseButton || (InputManager.GetButtonDown("Cover") && mainmenu.pause && mainmenu.Instance != null && mainmenu.Instance.currentMenu == mainmenu.menus.PAUSE)))
		{
			if (this.cameraDisconnected)
			{
				mainmenu.Instance.currentMenu = mainmenu.menus.PAUSE;
				mainmenu.Instance.currentMenuAfterUpdate = mainmenu.menus.PAUSE;
				mainmenu.Instance.ControlsInstruction = 0;
				mainmenu.pause = true;
			}
			else if (this.systemPause)
			{
				mainmenu.Instance.currentMenu = mainmenu.menus.PAUSE;
				mainmenu.Instance.currentMenuAfterUpdate = mainmenu.menus.PAUSE;
				mainmenu.Instance.ControlsInstruction = 0;
				mainmenu.pause = true;
				this.systemPause = false;
			}
			else if (this.cameraConnected)
			{
				mainmenu.Instance.currentMenu = mainmenu.menus.PAUSE;
				mainmenu.Instance.currentMenuAfterUpdate = mainmenu.menus.PAUSE;
				mainmenu.Instance.ControlsInstruction = 0;
				mainmenu.pause = false;
				this.cameraLastConnected = Time.time;
				this.cameraConnected = false;
			}
			else
			{
				if (mainmenu.hintPause)
				{
					return;
				}
				mainmenu.Instance.currentMenu = mainmenu.menus.PAUSE;
				mainmenu.Instance.currentMenuAfterUpdate = mainmenu.menus.PAUSE;
				mainmenu.Instance.ControlsInstruction = 0;
				mainmenu.pause = !mainmenu.pause;
			}
			if (mainmenu.pause)
			{
				if (PlatformCharacterController.joystickLeft != null && PlatformCharacterController.joystickLeft.gameObject != null)
				{
					PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
				}
				if (FightingControl.meleeJoystickLeft != null)
				{
					FightingControl.meleeJoystickLeft.gameObject.SetActive(false);
				}
				Time.timeScale = 1E-05f;
				AudioListener.pause = true;
				if (Camera.main != null && GlobalFarisCam.farisCamera.GetComponent<BlurEffect>() != null)
				{
					GlobalFarisCam.farisCamera.GetComponent<BlurEffect>().enabled = true;
				}
				Screen.lockCursor = false;
				if (this.cameraDisconnected)
				{
					mainmenu.Instance.currentMenu = mainmenu.menus.CAMERADISCONNECTED;
					mainmenu.Instance.selectedRow = 1;
					mainmenu.Instance.totlalRows = 1;
					mainmenu.Instance.ControlsInstruction = 0;
					this.cameraDisconnected = false;
				}
				else
				{
					mainmenu.Instance.currentMenu = mainmenu.menus.PAUSE;
					mainmenu.Instance.selectedRow = 1;
					mainmenu.Instance.totlalRows = 5;
					mainmenu.Instance.ControlsInstruction = 0;
				}
				mainmenu.Instance.enabled = true;
			}
			else
			{
				if (AnimationHandler.instance != null)
				{
					BasicAgility component = AnimationHandler.instance.GetComponent<BasicAgility>();
					if (component != null)
					{
						component.lastUserInputTime = Time.time;
					}
				}
				if ((GlobalFarisCam.farisCamera.GetComponent<ShooterGameCamera>() != null && !GlobalFarisCam.farisCamera.GetComponent<ShooterGameCamera>().meleeCamera) || GlobalFarisCam.farisCamera.GetComponent<QuadGameCamera>() != null || GlobalFarisCam.farisCamera.GetComponent<CarCameras>() != null)
				{
					if (PlatformCharacterController.joystickLeft != null && PlatformCharacterController.joystickLeft.gameObject != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(true);
					}
				}
				else if (FightingControl.meleeJoystickLeft != null)
				{
					FightingControl.meleeJoystickLeft.gameObject.SetActive(true);
				}
				Time.timeScale = 1f;
				AudioListener.pause = false;
				if (Camera.main != null && GlobalFarisCam.farisCamera.GetComponent<BlurEffect>() != null)
				{
					GlobalFarisCam.farisCamera.GetComponent<BlurEffect>().enabled = false;
				}
				Screen.lockCursor = false;
				mainmenu.Instance.enabled = false;
			}
		}
		if (this.cameraConnected && mainmenu.Instance != null && mainmenu.Instance.currentMenu != mainmenu.menus.CAMERADISCONNECTED)
		{
			this.cameraConnected = false;
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0004ADB4 File Offset: 0x00048FB4
	public void OnGUI()
	{
	}

	// Token: 0x04000C31 RID: 3121
	private bool pauseButton;

	// Token: 0x04000C32 RID: 3122
	private bool systemPause;

	// Token: 0x04000C33 RID: 3123
	private bool cameraDisconnected;

	// Token: 0x04000C34 RID: 3124
	private bool cameraConnected;

	// Token: 0x04000C35 RID: 3125
	private float cameraLastConnected;
}
