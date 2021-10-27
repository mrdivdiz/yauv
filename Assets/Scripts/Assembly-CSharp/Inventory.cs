using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class Inventory : MonoBehaviour
{
	// Token: 0x060007B4 RID: 1972 RVA: 0x0003F89C File Offset: 0x0003DA9C
	private void Start()
	{
		if (Application.loadedLevelName == "part1" && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0)))
		{
			if (Inventory.fuse1)
			{
				this.fountainPieces++;
			}
			if (Inventory.fuse2)
			{
				this.fountainPieces++;
			}
			if (Inventory.fuse3)
			{
				this.fountainPieces++;
			}
			if (Inventory.fuse4)
			{
				this.fountainPieces++;
			}
		}
		if (Application.loadedLevelName == "part1" && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 3) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 3)))
		{
			if (Inventory.dagger1)
			{
				this.daggers++;
			}
			if (Inventory.dagger2)
			{
				this.daggers++;
			}
		}
		if (Application.loadedLevelName == "part1" && (Inventory.fuse1 || Inventory.fuse2 || Inventory.fuse3 || Inventory.fuse4) && this.fountainCheckpointPosition != null && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0)))
		{
			base.transform.position = this.fountainCheckpointPosition.position;
			base.transform.rotation = this.fountainCheckpointPosition.rotation;
			foreach (GameObject gameObject in this.fountainCheckpointDestroyObjects)
			{
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
		if (Application.loadedLevelName == "part1" && (Inventory.dagger1 || Inventory.dagger2) && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 3) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 3)))
		{
			if (this.daggersSpeech != null)
			{
				UnityEngine.Object.Destroy(this.daggersSpeech);
			}
			if (this.dagger1Speech != null && Inventory.dagger1)
			{
				UnityEngine.Object.Destroy(this.dagger1Speech);
			}
			if (this.dagger2Speech != null && Inventory.dagger2)
			{
				UnityEngine.Object.Destroy(this.dagger2Speech);
			}
		}
		if (Application.loadedLevelName == "part1" && (Inventory.fuse1 || Inventory.fuse2 || Inventory.fuse3 || Inventory.fuse4) && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0)))
		{
			if (this.fusesSpeech != null)
			{
				UnityEngine.Object.Destroy(this.fusesSpeech);
			}
			if (this.fuse2Speech != null && Inventory.fuse2)
			{
				UnityEngine.Object.Destroy(this.fuse2Speech);
			}
		}
		this.basicAgility = base.transform.GetComponent<BasicAgility>();
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x0003FBF8 File Offset: 0x0003DDF8
	public static void SetFuse(int fuseNumber)
	{
		switch (fuseNumber)
		{
		case 1:
			Inventory.fuse1 = true;
			break;
		case 2:
			Inventory.fuse2 = true;
			break;
		case 3:
			Inventory.fuse3 = true;
			break;
		case 4:
			Inventory.fuse4 = true;
			break;
		}
		Inventory.askToReveal = false;
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x0003FC58 File Offset: 0x0003DE58
	private void ExitReveal()
	{
		if (this.playerTransform != null)
		{
			this.playerTransform.gameObject.SetActive(true);
			if (this.fuse1Location != null)
			{
				this.fuse1Location.gameObject.SetActive(false);
			}
			if (this.fuse2Location != null)
			{
				this.fuse2Location.gameObject.SetActive(false);
			}
			if (this.fuse3Location != null)
			{
				this.fuse3Location.gameObject.SetActive(false);
			}
			if (this.fuse4Location != null)
			{
				this.fuse4Location.gameObject.SetActive(false);
			}
			this.playerTransform = null;
			if (PlatformCharacterController.joystickLeft != null)
			{
				PlatformCharacterController.joystickLeft.gameObject.SetActive(true);
			}
			WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
			if (weaponsHUD != null)
			{
				weaponsHUD.enabled = true;
			}
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0003FD60 File Offset: 0x0003DF60
	private void Update()
	{
		if (Application.loadedLevelName != "part1" || (mainmenu.replayLevel && SaveHandler.replayCheckpointReached != 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached != 0) || !Inventory.enableHinting || mainmenu.pause)
		{
			return;
		}
		if (this.lastPressTime > 0f)
		{
			this.lastPressTime -= Time.deltaTime;
		}
		if (Input.GetAxisRaw("Hint") == 0f)
		{
			this.acceptDPadInput = true;
		}
		if ((Input.GetKeyDown(KeyCode.Tab) || (Input.GetAxisRaw("Hint") > 0f && this.acceptDPadInput) || this.showHintMobile || (AndroidPlatform.IsJoystickConnected() && AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetJoystickNames()[0].Contains("extended") && Input.GetKeyDown(KeyCode.JoystickButton4))) && (this.basicAgility == null || !this.basicAgility.ledgeHanging))
		{
			this.acceptDPadInput = false;
			if (this.lastPressTime <= 5f && this.lastPressTime > 0f && Inventory.askToReveal)
			{
				if (!Inventory.fuse1 && this.fuse1Location != null && this.playerTransform == null)
				{
					this.playerTransform = Camera.main;
					this.fuse1Location.gameObject.SetActive(true);
					Camera.main.gameObject.SetActive(false);
					base.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
					this.lastPressTime = 5f;
					if (PlatformCharacterController.joystickLeft != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
					}
					WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD != null)
					{
						weaponsHUD.enabled = false;
					}
					base.Invoke("ExitReveal", 5f);
				}
				else if (!Inventory.fuse2 && this.fuse2Location != null && this.playerTransform == null)
				{
					this.playerTransform = Camera.main;
					this.fuse2Location.gameObject.SetActive(true);
					Camera.main.gameObject.SetActive(false);
					base.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
					this.lastPressTime = 5f;
					if (PlatformCharacterController.joystickLeft != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
					}
					WeaponsHUD weaponsHUD2 = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD2 != null)
					{
						weaponsHUD2.enabled = false;
					}
					base.Invoke("ExitReveal", 5f);
				}
				else if (!Inventory.fuse3 && this.fuse3Location != null && this.playerTransform == null)
				{
					this.playerTransform = Camera.main;
					this.fuse3Location.gameObject.SetActive(true);
					Camera.main.gameObject.SetActive(false);
					base.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
					this.lastPressTime = 5f;
					if (PlatformCharacterController.joystickLeft != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
					}
					WeaponsHUD weaponsHUD3 = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD3 != null)
					{
						weaponsHUD3.enabled = false;
					}
					base.Invoke("ExitReveal", 5f);
				}
				else if (!Inventory.fuse4 && this.fuse4Location != null && this.playerTransform == null)
				{
					this.playerTransform = Camera.main;
					this.fuse4Location.gameObject.SetActive(true);
					Camera.main.gameObject.SetActive(false);
					base.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
					this.lastPressTime = 5f;
					if (PlatformCharacterController.joystickLeft != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
					}
					WeaponsHUD weaponsHUD4 = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD4 != null)
					{
						weaponsHUD4.enabled = false;
					}
					base.Invoke("ExitReveal", 5f);
				}
			}
			else if (!Inventory.fuse1)
			{
				if (!Inventory.askToReveal)
				{
					HintsViewer.ShowHint(Language.Get(this.fuse1Hint, 60), HintSize.OneLine, 0.9f);
					Inventory.askToReveal = true;
				}
				else
				{
					HintsViewer.ShowHint(Language.Get(this.fuse1Hint, 60) + "<BR>" + Language.Get("GameTip_3", 60), HintSize.OneLine, 1f);
					this.lastPressTime = 5f;
				}
			}
			else if (!Inventory.fuse2)
			{
				if (!Inventory.askToReveal)
				{
					HintsViewer.ShowHint(Language.Get(this.fuse2Hint, 60), HintSize.OneLine, 0.9f);
					Inventory.askToReveal = true;
				}
				else
				{
					HintsViewer.ShowHint(Language.Get(this.fuse2Hint, 60) + "<BR>" + Language.Get("GameTip_3", 60), HintSize.OneLine, 1f);
					this.lastPressTime = 5f;
				}
			}
			else if (!Inventory.fuse3)
			{
				if (!Inventory.askToReveal)
				{
					HintsViewer.ShowHint(Language.Get(this.fuse3Hint, 60), HintSize.OneLine, 0.9f);
					Inventory.askToReveal = true;
				}
				else
				{
					HintsViewer.ShowHint(Language.Get(this.fuse3Hint, 60) + "<BR>" + Language.Get("GameTip_3", 60), HintSize.OneLine, 1f);
					this.lastPressTime = 5f;
				}
			}
			else if (!Inventory.fuse4)
			{
				if (!Inventory.askToReveal)
				{
					HintsViewer.ShowHint(Language.Get(this.fuse4Hint, 60), HintSize.OneLine, 0.9f);
					Inventory.askToReveal = true;
				}
				else
				{
					HintsViewer.ShowHint(Language.Get(this.fuse4Hint, 60) + "<BR>" + Language.Get("GameTip_3", 60), HintSize.OneLine, 1f);
					this.lastPressTime = 5f;
				}
			}
			this.showHintMobile = false;
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x000403D8 File Offset: 0x0003E5D8
	private void LateUpdate()
	{
		if (this.fountainSentence == 1 && !this.oneplayed)
		{
			SpeechManager.PlayConversation("Fountain1");
			this.oneplayed = true;
		}
		if (this.fountainSentence == 2 && !this.twoplayed)
		{
			SpeechManager.PlayConversation("Fountain2");
			this.twoplayed = true;
		}
		if (this.fountainSentence == 3 && !this.threeplayed)
		{
			SpeechManager.PlayConversation("Fountain3");
			this.threeplayed = true;
		}
		if (this.fountainSentence == 4 && !Inventory.fourplayed)
		{
			Inventory.fourplayed = true;
			CutsceneManager.PlayCutscene(this.Fountain_Cutscene, null);
			if (this.m != null)
			{
				this.m.FadeOut();
			}
			if (this.gateSpeech != null)
			{
				UnityEngine.Object.Destroy(this.gateSpeech);
			}
		}
		if (this.fountainSentence > 4)
		{
			this.fountainSentence = 0;
		}
		if (this.previousFountainPieces != this.fountainPieces)
		{
			this.strFountainPieces = "X " + this.fountainPieces;
			this.previousFountainPieces = this.fountainPieces;
		}
		if (this.previousDaggers != this.daggers)
		{
			this.strDaggers = "X " + this.daggers;
			this.previousDaggers = this.daggers;
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00040540 File Offset: 0x0003E740
	public void OnGUI()
	{
		if (mainmenu.pause)
		{
			return;
		}
		float num = 1f;
		if (Screen.width > 1500)
		{
			num = 2f;
		}
		if (this.fountainPieces > 0 && CutsceneManager.showGUI)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - (128f * num / 2f + 64f), 10f, 128f * num, 128f * num), this.fountainPieceTexture, ScaleMode.StretchToFill);
			GUI.Label(new Rect((float)Screen.width / 2f - (128f * num / 2f + 64f) + 50f, 100f, 50f * num, 50f * num), this.strFountainPieces);
		}
		if (this.daggers > 0 && CutsceneManager.showGUI)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - (128f * num / 2f + 64f), 10f, 128f * num, 128f * num), this.daggerTexture, ScaleMode.StretchToFill);
			GUI.Label(new Rect((float)Screen.width / 2f - (128f * num / 2f + 64f) + 50f, 100f, 50f * num, 50f * num), this.strDaggers);
		}
		if ((!AndroidPlatform.IsJoystickConnected() || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetJoystickNames()[0].Contains("basic"))) && Application.loadedLevelName == "part1" && Inventory.enableHinting && (!Inventory.fuse1 || !Inventory.fuse2 || !Inventory.fuse3 || !Inventory.fuse4) && GUI.Button(new Rect(20f, 100f * num + 30f, 100f * num, 100f * num), this.showHintMobileTexture))
		{
			this.showHintMobile = true;
		}
	}

	// Token: 0x04000A20 RID: 2592
	public float[] items;

	// Token: 0x04000A21 RID: 2593
	public int fountainPieces;

	// Token: 0x04000A22 RID: 2594
	public int daggers;

	// Token: 0x04000A23 RID: 2595
	public int previousFountainPieces;

	// Token: 0x04000A24 RID: 2596
	public int previousDaggers;

	// Token: 0x04000A25 RID: 2597
	public string strFountainPieces = "X 1";

	// Token: 0x04000A26 RID: 2598
	public string strDaggers = "X 1";

	// Token: 0x04000A27 RID: 2599
	public int fountainSentence;

	// Token: 0x04000A28 RID: 2600
	public AudioSource FarisFountainSound;

	// Token: 0x04000A29 RID: 2601
	public AudioSource FarisHead;

	// Token: 0x04000A2A RID: 2602
	public Transform head;

	// Token: 0x04000A2B RID: 2603
	public GameObject FarisObject;

	// Token: 0x04000A2C RID: 2604
	public ShooterGameCamera PlayerCam;

	// Token: 0x04000A2D RID: 2605
	public bool oneplayed;

	// Token: 0x04000A2E RID: 2606
	public bool twoplayed;

	// Token: 0x04000A2F RID: 2607
	public bool threeplayed;

	// Token: 0x04000A30 RID: 2608
	public static bool fourplayed;

	// Token: 0x04000A31 RID: 2609
	public CSComponent Fountain_Cutscene;

	// Token: 0x04000A32 RID: 2610
	public Texture fountainPieceTexture;

	// Token: 0x04000A33 RID: 2611
	public Texture daggerTexture;

	// Token: 0x04000A34 RID: 2612
	public static bool fuse1;

	// Token: 0x04000A35 RID: 2613
	public static bool fuse2;

	// Token: 0x04000A36 RID: 2614
	public static bool fuse3;

	// Token: 0x04000A37 RID: 2615
	public static bool fuse4;

	// Token: 0x04000A38 RID: 2616
	public static bool dagger1;

	// Token: 0x04000A39 RID: 2617
	public static bool dagger2;

	// Token: 0x04000A3A RID: 2618
	public string fuse1Hint = "GameTip_2";

	// Token: 0x04000A3B RID: 2619
	public string fuse2Hint = "GameTip_4";

	// Token: 0x04000A3C RID: 2620
	public string fuse3Hint = "GameTip_2";

	// Token: 0x04000A3D RID: 2621
	public string fuse4Hint = "GameTip_4";

	// Token: 0x04000A3E RID: 2622
	public string revealHint = "GameTip_3";

	// Token: 0x04000A3F RID: 2623
	public Transform fountainCheckpointPosition;

	// Token: 0x04000A40 RID: 2624
	public GameObject[] fountainCheckpointDestroyObjects;

	// Token: 0x04000A41 RID: 2625
	public static bool enableHinting;

	// Token: 0x04000A42 RID: 2626
	private static bool askToReveal;

	// Token: 0x04000A43 RID: 2627
	private float lastPressTime;

	// Token: 0x04000A44 RID: 2628
	public Camera fuse1Location;

	// Token: 0x04000A45 RID: 2629
	public Camera fuse2Location;

	// Token: 0x04000A46 RID: 2630
	public Camera fuse3Location;

	// Token: 0x04000A47 RID: 2631
	public Camera fuse4Location;

	// Token: 0x04000A48 RID: 2632
	private Camera playerTransform;

	// Token: 0x04000A49 RID: 2633
	private bool acceptDPadInput = true;

	// Token: 0x04000A4A RID: 2634
	public GameObject daggersSpeech;

	// Token: 0x04000A4B RID: 2635
	public GameObject dagger1Speech;

	// Token: 0x04000A4C RID: 2636
	public GameObject dagger2Speech;

	// Token: 0x04000A4D RID: 2637
	public GameObject fusesSpeech;

	// Token: 0x04000A4E RID: 2638
	public GameObject fuse2Speech;

	// Token: 0x04000A4F RID: 2639
	public Texture showHintMobileTexture;

	// Token: 0x04000A50 RID: 2640
	private bool showHintMobile;

	// Token: 0x04000A51 RID: 2641
	private BasicAgility basicAgility;

	// Token: 0x04000A52 RID: 2642
	public GameObject gateSpeech;

	// Token: 0x04000A53 RID: 2643
	public temple_music m;
}
