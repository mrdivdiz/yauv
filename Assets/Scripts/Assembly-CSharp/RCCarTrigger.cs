using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class RCCarTrigger : MonoBehaviour
{
	// Token: 0x06000806 RID: 2054 RVA: 0x00041554 File Offset: 0x0003F754
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player").transform;
		this.p = this.player.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
		this.wh = this.player.GetComponent<WeaponHandling>();
		this.ba = this.player.GetComponent<BasicAgility>();
		this.pcc = this.player.GetComponent<PlatformCharacterController>();
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		this.rcCarScript = this.rcCar.GetComponent<RCcar>();
		this.shooterGameCamera = Camera.main.GetComponent<ShooterGameCamera>();
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x000415F4 File Offset: 0x0003F7F4
	private void Awake()
	{
		if (Screen.width > 1500)
		{
			this.playerGUI = this.playerGUI2x;
			this.rcCarGUI = this.rcCarGUI2x;
		}
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00041620 File Offset: 0x0003F820
	private void Update()
	{
		if (this.inside && this.cutscenePlayed && (Input.GetKeyDown(KeyCode.Tab) || InputManager.GetButtonDown("Interaction")) && !this.cutscene.IsPlaying && !SpeechManager.instance.playing)
		{
			this.toggleCarMode();
		}
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00041684 File Offset: 0x0003F884
	private void toggleCarMode()
	{
		this.carMode = !this.carMode;
		if (this.carMode)
		{
			if (this.firstTime)
			{
				HintsViewer.ShowHint(Language.Get("Tip_Car", 60), HintSize.OneLine, 1.1f, 1f);
				this.firstTime = false;
			}
			this.wh.gm.currentGun.enabled = false;
			this.wh.gm.enabled = false;
			this.wh.enabled = false;
			this.ba.enabled = false;
			this.ncm.enabled = false;
			AnimationHandler.instance.forceIdleState = true;
			this.rcCarScript.disabled = false;
			this.shooterGameCamera.player = this.rcCar;
			this.previousCamOffsetZ = this.shooterGameCamera.camOffset.z;
			this.shooterGameCamera.camOffset.z = -3f;
			this.previousMaxVerticalAngle = this.shooterGameCamera.maxVerticalAngle;
			this.shooterGameCamera.maxVerticalAngle = 0f;
			this.shooterGameCamera.allowCircleCamera = false;
			if (MobileInput.instance != null)
			{
				MobileInput.instance.aimButton.enabled = false;
				MobileInput.instance.fireButton.enabled = false;
				MobileInput.instance.jumpButton.enabled = false;
				MobileInput.instance.rollButton.enabled = false;
				MobileInput.instance.interactionButton.enabled = false;
			}
		}
		else
		{
			this.wh.gm.currentGun.enabled = true;
			this.wh.gm.enabled = true;
			this.wh.enabled = true;
			this.ba.enabled = true;
			this.ncm.enabled = true;
			AnimationHandler.instance.forceIdleState = false;
			this.rcCarScript.disabled = true;
			this.shooterGameCamera.player = this.p;
			this.shooterGameCamera.camOffset.z = this.previousCamOffsetZ;
			this.shooterGameCamera.maxVerticalAngle = this.previousMaxVerticalAngle;
			if (MobileInput.instance != null && !AndroidPlatform.IsJoystickConnected())
			{
				MobileInput.instance.aimButton.enabled = true;
				MobileInput.instance.fireButton.enabled = true;
				MobileInput.instance.jumpButton.enabled = true;
				MobileInput.instance.rollButton.enabled = true;
			}
		}
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00041900 File Offset: 0x0003FB00
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = true;
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00041920 File Offset: 0x0003FB20
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00041940 File Offset: 0x0003FB40
	public void OnGUI()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.inside && this.cutscenePlayed && CutsceneManager.showGUI)
		{
			if (!this.carMode)
			{
				if (!AndroidPlatform.IsJoystickConnected())
				{
					if (GUI.Button(new Rect((float)Screen.width / 2f - (float)this.rcCarGUI.width / 2f + (float)this.rcCarGUI.width, 20f, (float)this.rcCarGUI.width, (float)this.rcCarGUI.height), this.rcCarGUI))
					{
						this.toggleCarMode();
					}
				}
				else
				{
					GUI.Label(new Rect(60f, (float)(Screen.height - this.rcCarPS3.height) - 40f, (float)this.rcCarPS3.width, (float)this.rcCarPS3.height), this.rcCarPS3);
				}
			}
			else if (!AndroidPlatform.IsJoystickConnected())
			{
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)this.playerGUI.width / 2f + (float)this.rcCarGUI.width, 20f, (float)this.playerGUI.width, (float)this.playerGUI.height), this.playerGUI))
				{
					this.toggleCarMode();
				}
			}
			else
			{
				GUI.Label(new Rect(60f, (float)(Screen.height - this.playerPS3.height) - 40f, (float)this.playerPS3.width, (float)this.playerPS3.height), this.playerPS3);
			}
		}
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00041AF8 File Offset: 0x0003FCF8
	public void SetCutscenePlayed()
	{
		this.cutscenePlayed = true;
	}

	// Token: 0x04000A8B RID: 2699
	public Transform player;

	// Token: 0x04000A8C RID: 2700
	public Transform rcCar;

	// Token: 0x04000A8D RID: 2701
	public Texture rcCarGUI;

	// Token: 0x04000A8E RID: 2702
	public Texture playerGUI;

	// Token: 0x04000A8F RID: 2703
	public Texture rcCarGUI2x;

	// Token: 0x04000A90 RID: 2704
	public Texture playerGUI2x;

	// Token: 0x04000A91 RID: 2705
	public Texture rcCarPS3;

	// Token: 0x04000A92 RID: 2706
	public Texture playerPS3;

	// Token: 0x04000A93 RID: 2707
	public Texture rcCarPC;

	// Token: 0x04000A94 RID: 2708
	public Texture playerPC;

	// Token: 0x04000A95 RID: 2709
	private bool inside;

	// Token: 0x04000A96 RID: 2710
	private bool carMode;

	// Token: 0x04000A97 RID: 2711
	private bool cutscenePlayed = true;

	// Token: 0x04000A98 RID: 2712
	private WeaponHandling wh;

	// Token: 0x04000A99 RID: 2713
	private BasicAgility ba;

	// Token: 0x04000A9A RID: 2714
	private PlatformCharacterController pcc;

	// Token: 0x04000A9B RID: 2715
	private NormalCharacterMotor ncm;

	// Token: 0x04000A9C RID: 2716
	private RCcar rcCarScript;

	// Token: 0x04000A9D RID: 2717
	private ShooterGameCamera shooterGameCamera;

	// Token: 0x04000A9E RID: 2718
	private Transform p;

	// Token: 0x04000A9F RID: 2719
	private float previousCamOffsetZ;

	// Token: 0x04000AA0 RID: 2720
	private float previousMaxVerticalAngle;

	// Token: 0x04000AA1 RID: 2721
	private bool firstTime = true;

	// Token: 0x04000AA2 RID: 2722
	public CSComponent cutscene;
}
