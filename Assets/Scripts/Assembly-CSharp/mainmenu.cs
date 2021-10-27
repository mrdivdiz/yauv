using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class mainmenu : MonoBehaviour
{
	// Token: 0x06000A97 RID: 2711 RVA: 0x00072288 File Offset: 0x00070488
	public mainmenu()
	{
		string[,] array = new string[21, 2];
		array[0, 0] = "EN";
		array[0, 1] = "English";
		array[1, 0] = "AR";
		array[1, 1] = "Arabic";
		array[2, 0] = "FR";
		array[2, 1] = "French";
		array[3, 0] = "DE";
		array[3, 1] = "German";
		array[4, 0] = "ES";
		array[4, 1] = "Spanish";
		array[5, 0] = "IT";
		array[5, 1] = "Italian";
		array[6, 0] = "NO";
		array[6, 1] = "Norwegian";
		array[7, 0] = "DA";
		array[7, 1] = "Danish";
		array[8, 0] = "SV";
		array[8, 1] = "Swedish";
		array[9, 0] = "EL";
		array[9, 1] = "Greek";
		array[10, 0] = "NL";
		array[10, 1] = "Dutch";
		array[11, 0] = "PT";
		array[11, 1] = "Portuguese";
		array[12, 0] = "RU";
		array[12, 1] = "Russian";
		array[13, 0] = "PL";
		array[13, 1] = "Polish";
		array[14, 0] = "JA";
		array[14, 1] = "Japanese";
		array[15, 0] = "ZH";
		array[15, 1] = "Chinese";
		array[16, 0] = "TR";
		array[16, 1] = "Turkish";
		array[17, 0] = "MS";
		array[17, 1] = "Malay";
		array[18, 0] = "FA";
		array[18, 1] = "Farsi";
		array[19, 0] = "KO";
		array[19, 1] = "Korean";
		array[20, 0] = "HI";
		array[20, 1] = "Hindi";
		this.languages = array;
		this.nextLevelName = string.Empty;
		this.iconNo = -1;
		this.overlay1 = true;
		this.overlayTimer = 6f;
		return;
		//base..ctor();
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00072548 File Offset: 0x00070748
	private void Awake()
	{
		mainmenu.Instance = this;
		if (AndroidPlatform.IsIAPavailable() && !SaveHandler.purchased && !mainmenu.trailerShown)
		{
			if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
			{
				//Handheld.PlayFullScreenMovie("Movies-OGG/Trailer-iOS-Version-Arabic.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
			}
			else
			{
				//Handheld.PlayFullScreenMovie("Movies-OGG/Trailer-iOS-Version-English.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
			}
			mainmenu.trailerShown = true;
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x000725B4 File Offset: 0x000707B4
	private void OnDestroy()
	{
		if (!this.isPauseMenu)
		{
			this.buttonStyle.hover.background = this.highlitedButtonTexture;
			this.buttonStyle.focused.background = this.highlitedButtonTexture;
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.CancelInvoke("CheckJoystick");
		}
		if (Application.loadedLevelName == "QuadChase")
		{
			Physics.gravity = new Vector3(0f, -9.81f, 0f);
		}
		mainmenu.Instance = null;
		if (this.guiSkin != null)
		{
			this.guiSkin.font = null;
		}
		if (this.instructionStyle != null)
		{
			this.instructionStyle.font = null;
		}
		if (this.largeInstructionStyle != null)
		{
			this.largeInstructionStyle.font = null;
		}
		if (this.plaingTextLargeStyle != null)
		{
			this.plaingTextLargeStyle.font = null;
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x000726B0 File Offset: 0x000708B0
	//private void productListReceivedEvent(List<IAPProduct> productList)
	//{
	//	Debug.Log("received total products: " + productList.Count);
	//	this._products = productList;
	//}

	// Token: 0x06000A9C RID: 2716 RVA: 0x000726D4 File Offset: 0x000708D4
	private void purchaseComplete(bool success)
	{
		if (success)
		{
			SaveHandler.purchased = true;
			PlayerPrefs.SetInt("purchased", 1);
			//GA.API.Business.NewEvent("PurchaseComplete:FullGameUnlock", "USD", 299);
			mainmenu.showCart = true;
			this.selectedCol = 1;
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00072724 File Offset: 0x00070924
	public void CheckJoystick()
	{
		AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
		if (AndroidPlatform.IsJoystickConnected())
		{
			this.buttonStyle.hover.background = this.highlitedButtonTexture;
			this.buttonStyle.focused.background = this.highlitedButtonTexture;
		}
		else
		{
			this.buttonStyle.hover.background = this.buttonStyle.normal.background;
			this.buttonStyle.focused.background = this.buttonStyle.normal.background;
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x000727BC File Offset: 0x000709BC
	private void Start()
	{
		if (!this.isPauseMenu)
		{
			if (mainmenu.returnToMenu != mainmenu.menus.MAIN)
			{
				this.currentMenu = mainmenu.returnToMenu;
				mainmenu.returnToMenu = mainmenu.menus.MAIN;
			}
			if (AndroidPlatform.IsIAPavailable())
			{
				//string androidPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAktfR22/KzFoqSgNhy1UTlkFNEVDNM1yHq1jEIBbJv7H+wYNWAxi59v2yFRaszfGYAaGBsrlwIYHqfbvJk1MadbGKffeN0rhByTWjlD+6g3fnvIB5cny/EnNTr2/4nCuumMgDcoviiLGtUuB/JjL4/0j0jxaUSVGUP1TkEaQmADcdv7+QFEJebLBK/PGOQnegiVpaEyfnV4jqvJCG+EDu5HXJ/w+OPwGqWCcLPRpWc62L2RESgy+Q5ZrYx+Nt4pRA/jGsNwR7E4aGFijH0Z3GSQwF2z0CHl7A/EDcSWzXrr7OzWYN5EnjH73pCclOmiF0hk1lDhVbCrxaWEeS80o1HQIDAQAB";
				//IAP.init(androidPublicKey);
				string[] iosProductIdentifiers = new string[]
				{
					"FullGameUnlock"
				};
				string[] androidSkus = new string[]
				{
					"FullGameUnlock"
				};
				//IAP.requestProductData(iosProductIdentifiers, androidSkus, new Action<List<IAPProduct>>(this.productListReceivedEvent));
			}
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
			{
				base.InvokeRepeating("CheckJoystick", 0f, 1f);
			}
			this.StartMethod();
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0007286C File Offset: 0x00070A6C
	public void StartMethod()
	{
		if (Application.loadedLevelName == "QuadChase")
		{
			Physics.gravity = new Vector3(0f, -45.81f, 0f);
		}
		this.rightArrowStyle = this.guiSkin.GetStyle("rightArrow");
		this.leftArrowStyle = this.guiSkin.GetStyle("leftArrow");
		this.buttonStyle = this.guiSkin.button;
		this.LabelStyle = this.guiSkin.label;
		this.plaingTextStyle = this.guiSkin.GetStyle("PlainText");
		this.plaingTextLargeStyle = this.guiSkin.GetStyle("PlainTextLarge");
		this.plaingTextSelectedStyle = this.guiSkin.GetStyle("PlainTextSelected");
		this.BoldOutlineTextStyle = this.guiSkin.GetStyle("BoldOutlineText");
		SaveHandler.Load();
		if (!this.isPauseMenu)
		{
			////GA.API.Design.NewEvent("LoadedScene:MainMenu");
			this.SetBackground();
			SaveHandler.LoadSettings();
		}
		else if (Application.loadedLevelName != "QuadChase")
		{
			if (!mainmenu.replayLevel)
			{
				SaveHandler.LoadLevel();
			}
			else
			{
				SaveHandler.LoadLevelOnReplay();
			}
			////GA.API.Design.NewEvent("LoadedScene:" + Application.loadedLevelName);
		}
		else
		{
			////GA.API.Design.NewEvent("LoadedScene:" + Application.loadedLevelName);
		}
		for (int i = 0; i < this.languages.Length; i++)
		{
			if (Language.CurrentLanguage().ToString() == this.languages[i, 0])
			{
				this.currentLanguage = i;
				break;
			}
		}
		for (int j = 0; j < Screen.resolutions.Length; j++)
		{
			if (Screen.resolutions[j].width == Screen.currentResolution.width)
			{
				this.currentResolution = j;
			}
		}
		this.SetCurrentLanguageFont();
		this.emptyStyle = new GUIStyle();
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00072A90 File Offset: 0x00070C90
	private void SetBackground()
	{
		if (mainmenu.started)
		{
			if (Language.CurrentLanguage() == LanguageCode.AR)
			{
				//this.backgroundTexture.texture = this.ArabicMobile;
			}
			else
			{
				//this.backgroundTexture.texture = this.EnglishMobile;
			}
		}
		else if (Language.CurrentLanguage() == LanguageCode.AR)
		{
			//this.backgroundTexture.texture = this.StartArabicOther;
		}
		else
		{
			//this.backgroundTexture.texture = this.StartEnglishOther;
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00072B10 File Offset: 0x00070D10
	private void SetCurrentLanguageFont()
	{
		this.instructionStyle = this.guiSkin.GetStyle("instructionStyle");
		this.largeInstructionStyle = this.guiSkin.GetStyle("largeInstructionStyle");
		this.guiSkin.font = Language.GetFont29();
		this.instructionStyle.font = Language.GetFont18();
		this.largeInstructionStyle.font = Language.GetFont29();
		this.plaingTextLargeStyle.font = Language.GetFont29();
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00072B8C File Offset: 0x00070D8C
	private void exitlevel()
	{
		mainmenu.Instance = null;
		UnityEngine.Object.Destroy(this.videoObject);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00072BA0 File Offset: 0x00070DA0
	private void Update()
	{
		
		if (mainmenu.showCart)
		{
			if (this.cartAction)
			{
				if (Time.time > this.lastPurchaseTime + 2f)
				{
					if (!true)
					{
						this.inAppPurchasesInactive = true;
						//GA.API.Design.NewEvent("CartScreen:inAppPurchasesInactive");
					}
					else if (Application.internetReachability == NetworkReachability.NotReachable)
					{
						this.internetDisconnected = true;
						//GA.API.Design.NewEvent("CartScreen:internetDisconnected");
					}
					else
					{
						string[] iosProductIdentifiers = new string[]
						{
							"FullGameUnlock"
						};
						string[] androidSkus = new string[]
						{
							"FullGameUnlock"
						};
						//IAP.requestProductData(iosProductIdentifiers, androidSkus, new Action<List<IAPProduct>>(this.productListReceivedEvent));
					}
					this.lastPurchaseTime = Time.time;
				}
				this.cartAction = false;
			}
			if (this.overlayTimer <= 0f)
			{
				this.overlay1 = !this.overlay1;
				this.currentFeatures = (this.currentFeatures + 1) % 3;
				this.overlayTimer = 6f;
			}
			else
			{
				this.overlayTimer -= Time.deltaTime;
			}
			if (this.acceptMovement && !this.faded && (this.isPauseMenu || mainmenu.started))
			{
				if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("DpadH") > 0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton5)))
				{
					this.rightArrowAction = true;
					this.acceptMovement = false;
					if (this.selectedCol < this.totlalCols)
					{
						this.selectedCol++;
						base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.selectedCol = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
					}
				}
				else if (Input.GetAxisRaw("Horizontal") < -0.3f || Input.GetAxisRaw("DpadH") < -0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton7)))
				{
					this.leftArrowAction = true;
					this.acceptMovement = false;
					if (this.selectedCol > 1)
					{
						this.selectedCol--;
						base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.selectedCol = this.totlalCols;
						base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
					}
				}
			}
			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("DpadH")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("DpadV")) < 0.3f && (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS || (!Input.GetKey(KeyCode.JoystickButton4) && !Input.GetKey(KeyCode.JoystickButton5) && !Input.GetKey(KeyCode.JoystickButton6) && !Input.GetKey(KeyCode.JoystickButton7))))
			{
				this.acceptMovement = true;
			}
			if (InputManager.GetButtonDown("Jump") && Time.time > this.startedTime + 1f && !this.faded)
			{
				this.action = true;
			}
			return;
		}
		if (this.ControlsInstruction != 0)
		{
			if (Input.GetKeyDown(KeyCode.JoystickButton5))
			{
				this.ControlsInstruction++;
				if (this.ControlsInstruction > 3)
				{
					this.ControlsInstruction = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			else if (Input.GetKeyDown(KeyCode.JoystickButton4))
			{
				this.ControlsInstruction--;
				if (this.ControlsInstruction < 1)
				{
					this.ControlsInstruction = 3;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			if (Input.GetButtonDown("Cover"))
			{
				this.ControlsInstruction = 0;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			return;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			mainmenu.Instance = null;
			Application.LoadLevel(this.levelToLoad);
		}
		if (InputManager.GetButtonDown("Cover") && !this.faded)
		{
			this.backAction = true;
		}
		if (this.isPauseMenu && !mainmenu.pause)
		{
			return;
		}
		if (this.acceptMovement && !this.faded && (this.isPauseMenu || mainmenu.started))
		{
			if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("DpadH") > 0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton5)))
			{
				this.rightArrowAction = true;
				this.acceptMovement = false;
				if (this.selectedCol < this.totlalCols)
				{
					this.selectedCol++;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.selectedCol = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				MonoBehaviour.print(this.selectedCol);
			}
			else if (Input.GetAxisRaw("Horizontal") < -0.3f || Input.GetAxisRaw("DpadH") < -0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton7)))
			{
				this.leftArrowAction = true;
				this.acceptMovement = false;
				if (this.selectedCol > 1)
				{
					this.selectedCol--;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.selectedCol = this.totlalCols;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				MonoBehaviour.print(this.selectedCol);
			}
			if ((Input.GetAxisRaw("Vertical") > 0.3f || Input.GetAxisRaw("DpadV") > 0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton4))) && !this.faded)
			{
				if (this.selectedRow > 1)
				{
					this.selectedRow--;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.selectedRow = this.totlalRows;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				this.acceptMovement = false;
			}
			else if ((Input.GetAxisRaw("Vertical") < -0.3f || Input.GetAxisRaw("DpadV") < -0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton6))) && !this.faded)
			{
				if (this.selectedRow < this.totlalRows)
				{
					this.selectedRow++;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				this.acceptMovement = false;
			}
		}
		if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("DpadH")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("DpadV")) < 0.3f && (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS || (!Input.GetKey(KeyCode.JoystickButton4) && !Input.GetKey(KeyCode.JoystickButton5) && !Input.GetKey(KeyCode.JoystickButton6) && !Input.GetKey(KeyCode.JoystickButton7))))
		{
			this.acceptMovement = true;
		}
		if ((this.isPauseMenu || mainmenu.started) && InputManager.GetButtonDown("Jump") && Time.time > this.startedTime + 1f && !this.faded)
		{
			this.action = true;
		}
		if (!this.isPauseMenu && Input.GetKeyDown(KeyCode.Escape) && Time.time > this.startedTime + 1f && !this.faded)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			this.currentMenu = mainmenu.menus.CONFIRMEXIT;
			this.selectedRow = 1;
		}
		this.currentMenuAfterUpdate = this.currentMenu;
		this.calebrationStep = this.calebrationStepUpdate;
		this.iconNo = this.iconNoUpdate;
		if (!this.isPauseMenu)
		{
		}
		if (this.facebookAction)
		{
			if (Language.CurrentLanguage() == LanguageCode.AR)
			{
				Application.OpenURL("http://www.unearthedgame.com/facebook/android/arabic");
			}
			else
			{
				Application.OpenURL("http://www.unearthedgame.com/facebook/android/english");
			}
			//GA.API.Design.NewEvent("MainMenu:ShareFacebook");
			this.facebookAction = false;
		}
		if (this.twitterAction)
		{
			if (Language.CurrentLanguage() == LanguageCode.AR)
			{
				Application.OpenURL("http://www.unearthedgame.com/twitter/android/arabic");
			}
			else
			{
				Application.OpenURL("http://www.unearthedgame.com/twitter/android/english");
			}
			//GA.API.Design.NewEvent("MainMenu:ShareTwitter");
			this.twitterAction = false;
		}
		if (this.websiteAction)
		{
			if (Language.CurrentLanguage() == LanguageCode.AR)
			{
				Application.OpenURL("http://www.unearthedgame.com/arabic/");
			}
			else
			{
				Application.OpenURL("http://www.unearthedgame.com/");
			}
			//GA.API.Design.NewEvent("MainMenu:OpenedWebsite");
			this.websiteAction = false;
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00073628 File Offset: 0x00071828
	private bool DrawButton(string caption, GUIStyle style)
	{
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.MaxWidth((float)style.normal.background.width),
			GUILayout.MaxHeight(84f)
		});
		if (style.font == null || style.font != this.guiSkin.font)
		{
			style.font = this.guiSkin.font;
		}
		return GUI.Button(GUILayoutUtility.GetLastRect(), caption, style);
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x000736B4 File Offset: 0x000718B4
	private bool DrawButton(string caption)
	{
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.MaxWidth((float)this.buttonStyle.normal.background.width),
			GUILayout.MaxHeight(84f)
		});
		if (this.buttonStyle.font == null || this.buttonStyle.font != this.guiSkin.font)
		{
			this.buttonStyle.font = this.guiSkin.font;
		}
		if (caption == "Journey to Mecca Trailer" && Language.CurrentLanguage() == LanguageCode.ZH)
		{
			this.buttonStyle.font = null;
		}
		return GUI.Button(GUILayoutUtility.GetLastRect(), caption, this.buttonStyle);
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x00073784 File Offset: 0x00071984
	private bool DrawBackButton(string caption)
	{
		float fixedWidth = this.buttonStyle.fixedWidth;
		this.buttonStyle.fixedWidth = 150f;
		float fixedHeight = this.buttonStyle.fixedHeight;
		this.buttonStyle.fixedHeight = 40f;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			this.buttonStyle.contentOffset = new Vector2(0f, -6f);
		}
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.MaxWidth(this.buttonStyle.fixedWidth),
			GUILayout.MaxHeight(40f)
		});
		if (this.buttonStyle.font == null || this.buttonStyle.font != this.guiSkin.font)
		{
			this.buttonStyle.font = this.guiSkin.font;
		}
		bool result = GUI.Button(GUILayoutUtility.GetLastRect(), caption, this.buttonStyle);
		this.buttonStyle.fixedWidth = fixedWidth;
		this.buttonStyle.fixedHeight = fixedHeight;
		this.buttonStyle.contentOffset = new Vector2(0f, 0f);
		return result;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x000738BC File Offset: 0x00071ABC
	private void DrawLabel(string caption, GUIStyle style, GUILayoutOption option = null)
	{
		if (style.normal.background != null)
		{
			if (option != null)
			{
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxWidth((float)style.normal.background.width),
					GUILayout.MaxHeight(15f),
					option
				});
			}
			else
			{
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxWidth((float)style.normal.background.width),
					GUILayout.MaxHeight(15f)
				});
			}
		}
		else if (option != null)
		{
			GUILayout.Label(string.Empty, new GUILayoutOption[]
			{
				GUILayout.MaxHeight(15f),
				option
			});
		}
		else
		{
			GUILayout.Label(string.Empty, new GUILayoutOption[]
			{
				GUILayout.MaxHeight(15f)
			});
		}
		if (style != this.plaingTextLargeStyle && (style.font == null || style.font != this.guiSkin.font))
		{
			style.font = this.guiSkin.font;
		}
		GUI.Label(GUILayoutUtility.GetLastRect(), caption, style);
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00073A00 File Offset: 0x00071C00
	private void DrawLabel(string caption)
	{
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.MaxWidth((float)this.LabelStyle.normal.background.width),
			GUILayout.MaxHeight(34f)
		});
		if (this.LabelStyle.font == null || this.LabelStyle.font != this.guiSkin.font)
		{
			this.LabelStyle.font = this.guiSkin.font;
		}
		GUI.Label(GUILayoutUtility.GetLastRect(), caption, this.LabelStyle);
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00073AA8 File Offset: 0x00071CA8
	private void OnGUI()
	{
		if (mainmenu.showCart)
		{
			float num = 1366f;
			if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.4f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.2f)
			{
				num = 1024f;
			}
			else if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.6f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.4f)
			{
				num = 1024f;
			}
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / num, (float)Screen.height / 768f, 1f));
			GUI.DrawTexture(new Rect(0f, 0f, num, 768f), this.cartBG);
			if (this.inAppPurchasesInactive)
			{
				this.totlalCols = 1;
				if (this.style == null)
				{
					this.style = new GUIStyle();
					this.style.font = Language.GetFont18();
					this.style.alignment = TextAnchor.MiddleCenter;
					this.style.wordWrap = true;
					this.style.normal.textColor = Color.white;
				}
				this.style.normal.textColor = Color.black;
				GUI.Label(new Rect(num / 2f - 300f + 1f, 83f, 600f, 600f), Language.Get("PaymentFailed", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f + 1f, 85f, 600f, 600f), Language.Get("PaymentFailed", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f - 1f, 83f, 600f, 600f), Language.Get("PaymentFailed", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f - 1f, 85f, 600f, 600f), Language.Get("PaymentFailed", 60), this.style);
				this.style.normal.textColor = Color.white;
				GUI.Label(new Rect(num / 2f - 300f, 84f, 600f, 600f), Language.Get("PaymentFailed", 60), this.style);
				GUILayout.BeginArea(new Rect(50f, 708f, num, 60f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.buttonStyle.fixedWidth = 310f;
				GUI.SetNextControlName("focusedButton");
				if ((GUILayout.Button(Language.Get("M_Back", 60), this.buttonStyle, new GUILayoutOption[0]) || this.cartButtonAction) && Event.current.type != EventType.Repaint)
				{
					this.inAppPurchasesInactive = false;
					this.selectedCol = 1;
					this.cartButtonAction = false;
				}
				this.buttonStyle.fixedWidth = 395f;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
			else if (this.internetDisconnected)
			{
				this.totlalCols = 1;
				if (this.style == null)
				{
					this.style = new GUIStyle();
					this.style.font = Language.GetFont18();
					this.style.alignment = TextAnchor.MiddleCenter;
					this.style.wordWrap = true;
					this.style.normal.textColor = Color.white;
				}
				this.style.normal.textColor = Color.black;
				GUI.Label(new Rect(num / 2f - 300f + 1f, 83f, 600f, 600f), Language.Get("Internet_Disconnected", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f + 1f, 85f, 600f, 600f), Language.Get("Internet_Disconnected", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f - 1f, 83f, 600f, 600f), Language.Get("Internet_Disconnected", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f - 1f, 85f, 600f, 600f), Language.Get("Internet_Disconnected", 60), this.style);
				this.style.normal.textColor = Color.white;
				GUI.Label(new Rect(num / 2f - 300f, 84f, 600f, 600f), Language.Get("Internet_Disconnected", 60), this.style);
				GUILayout.BeginArea(new Rect(50f, 708f, num, 60f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.buttonStyle.fixedWidth = 310f;
				GUI.SetNextControlName("focusedButton");
				if ((GUILayout.Button(Language.Get("M_Back", 60), this.buttonStyle, new GUILayoutOption[0]) || this.cartButtonAction) && Event.current.type != EventType.Repaint)
				{
					this.internetDisconnected = false;
					this.selectedCol = 1;
					this.cartButtonAction = false;
				}
				this.buttonStyle.fixedWidth = 395f;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
			else if (!SaveHandler.purchased)
			{
				this.totlalCols = 3;
				if (this.overlay1)
				{
					GUI.DrawTexture(new Rect((num - (float)this.cartOverlay1.width) / 2f, 0f, (float)this.cartOverlay1.width, (float)this.cartOverlay1.height), this.cartOverlay1);
				}
				else
				{
					GUI.DrawTexture(new Rect((num - (float)this.cartOverlay2.width) / 2f, 0f, (float)this.cartOverlay2.width, (float)this.cartOverlay2.height), this.cartOverlay2);
				}
				if (Language.CurrentLanguage() == LanguageCode.AR)
				{
					GUI.DrawTexture(new Rect((num - (float)this.cartOverlay1.width) / 2f + 20f, 10f, (float)this.cartLogoAr.width, (float)this.cartLogoAr.height), this.cartLogoAr);
				}
				else
				{
					GUI.DrawTexture(new Rect((num - (float)this.cartOverlay1.width) / 2f + 20f, 10f, (float)this.cartLogoEn.width, (float)this.cartLogoEn.height), this.cartLogoEn);
				}
				if (this.style2 == null)
				{
					this.style2 = new GUIStyle();
					this.style2.font = Language.GetFont18();
					this.style2.alignment = TextAnchor.UpperCenter;
					this.style2.wordWrap = true;
					this.style2.normal.textColor = Color.white;
				}
				int num2 = this.currentFeatures;
				switch (num2)
				{
				case 0:
					GUI.Label(new Rect(50f, 280f, 300f, 400f), Language.Get("Point1", 30), this.style2);
					GUI.Label(new Rect(50f, 480f, 300f, 400f), Language.Get("Point2", 30), this.style2);
					break;
				case 1:
					GUI.Label(new Rect(50f, 280f, 300f, 400f), Language.Get("Point3", 30), this.style2);
					GUI.Label(new Rect(50f, 480f, 300f, 400f), Language.Get("Point4", 30), this.style2);
					break;
				case 2:
					GUI.Label(new Rect(50f, 280f, 300f, 400f), Language.Get("Point5", 30), this.style2);
					GUI.Label(new Rect(50f, 480f, 300f, 400f), Language.Get("PressBuy", 30), this.style2);
					break;
				}
				GUILayout.BeginArea(new Rect(50f, 708f, num, 60f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.buttonStyle.fixedWidth = 310f;
				if (this.selectedCol == 1)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((GUILayout.Button(Language.Get("M_Back_To_Mainmenu", 60), this.buttonStyle, new GUILayoutOption[0]) || (this.cartButtonAction && this.selectedCol == 1)) && Event.current.type != EventType.Repaint)
				{
					mainmenu.showCart = false;
					this.currentMenu = mainmenu.menus.MAIN;
					this.selectedRow = 1;
					//GA.API.Design.NewEvent("CartScreen:BackButtonPressed");
					this.cartButtonAction = false;
				}
				if (this.selectedCol == 2)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((GUILayout.Button(Language.Get("M_Trailer", 60), this.buttonStyle, new GUILayoutOption[0]) || (this.cartButtonAction && this.selectedCol == 2)) && Event.current.type != EventType.Repaint)
				{
					//GA.API.Design.NewEvent("CartScreen:TrailerButtonPressed");
					this.cartButtonAction = false;
					if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
					{
						//Handheld.PlayFullScreenMovie("Movies-OGG/Trailer-iOS-Version-Arabic.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
					}
					else
					{
						//Handheld.PlayFullScreenMovie("Movies-OGG/Trailer-iOS-Version-English.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
					}
				}
				if (this.selectedCol == 3)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((GUILayout.Button(Language.Get("M_Buy", 60), this.buttonStyle, new GUILayoutOption[0]) || (this.cartButtonAction && this.selectedCol == 3)) && Event.current.type != EventType.Repaint)
				{
					//GA.API.Design.NewEvent("CartScreen:BuyButtonPressed");
					this.cartAction = true;
					this.action = false;
				}
				this.buttonStyle.fixedWidth = 395f;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
			else
			{
				this.totlalCols = 1;
				if (this.style == null)
				{
					this.style = new GUIStyle();
					this.style.font = Language.GetFont18();
					this.style.alignment = TextAnchor.MiddleCenter;
					this.style.wordWrap = true;
					this.style.normal.textColor = Color.white;
				}
				this.style.normal.textColor = Color.black;
				GUI.Label(new Rect(num / 2f - 300f + 1f, 83f, 600f, 600f), Language.Get("Thanks", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f + 1f, 85f, 600f, 600f), Language.Get("Thanks", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f - 1f, 83f, 600f, 600f), Language.Get("Thanks", 60), this.style);
				GUI.Label(new Rect(num / 2f - 300f - 1f, 85f, 600f, 600f), Language.Get("Thanks", 60), this.style);
				this.style.normal.textColor = Color.white;
				GUI.Label(new Rect(num / 2f - 300f, 84f, 600f, 600f), Language.Get("Thanks", 60), this.style);
				GUILayout.BeginArea(new Rect(50f, 708f, num, 60f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.buttonStyle.fixedWidth = 310f;
				GUI.SetNextControlName("focusedButton");
				if ((GUILayout.Button(Language.Get("M_Back_To_Mainmenu", 60), this.buttonStyle, new GUILayoutOption[0]) || this.cartButtonAction) && Event.current.type != EventType.Repaint)
				{
					mainmenu.showCart = false;
					this.currentMenu = mainmenu.menus.MAIN;
					this.selectedRow = 1;
					this.selectedCol = 1;
					this.cartButtonAction = false;
				}
				this.buttonStyle.fixedWidth = 395f;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
			GUI.FocusControl("focusedButton");
			return;
		}
		if (mainmenu.showMeleeInstructions)
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick)
			{
				GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.gameStickMeleeInstIngame);
				this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
				this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
				this.DrawShadowedText(new Rect(420f, 645f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(780f, 645f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(660f, 200f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.normal.textColor = Color.yellow;
				GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.normal.textColor = Color.white;
				this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
				this.DrawShadowedText(new Rect(1050f, 325f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(1050f, 225f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(1050f, 285f, 100f, 60f), Language.Get("Controller_21a", 60), this.largeInstructionStyle);
				if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
				{
					this.DrawShadowedText(new Rect(1176f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
					GUI.Label(new Rect(1246f, 703f, 60f, 60f), this.selectPS3);
				}
				else
				{
					GUI.Label(new Rect(70f, 703f, 60f, 60f), this.selectPS3);
					this.DrawShadowedText(new Rect(110f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
				}
			}
			else if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay && AndroidPlatform.IsJoystickConnected()))
			{
				GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.gameStickMeleeInstIngame);
				this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
				this.DrawShadowedText(new Rect(550f, 645f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(700f, 645f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
				this.DrawShadowedText(new Rect(670f, 150f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.normal.textColor = Color.yellow;
				GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.normal.textColor = Color.white;
				this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
				this.DrawShadowedText(new Rect(1000f, 330f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(1000f, 250f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(1000f, 290f, 100f, 60f), Language.Get("Controller_21a", 60), this.largeInstructionStyle);
				if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
				{
					this.DrawShadowedText(new Rect(1176f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
					GUI.Label(new Rect(1246f, 703f, 60f, 60f), this.selectPS3);
				}
				else
				{
					GUI.Label(new Rect(70f, 703f, 60f, 60f), this.selectPS3);
					this.DrawShadowedText(new Rect(110f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
				}
			}
			else if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && AndroidPlatform.IsJoystickConnected())
			{
				if (Input.GetJoystickNames()[0].Contains("basic"))
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.iOSBasicMeleeInstIngame);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(350f, 545f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(650f, 545f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(880f, 545f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(1120f, 325f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1120f, 225f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1120f, 285f, 100f, 60f), Language.Get("Controller_21a", 60), this.largeInstructionStyle);
					if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
					{
						this.DrawShadowedText(new Rect(1176f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
						GUI.Label(new Rect(1246f, 703f, 60f, 60f), this.selectPS3);
					}
					else
					{
						GUI.Label(new Rect(70f, 703f, 60f, 60f), this.selectPS3);
						this.DrawShadowedText(new Rect(110f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
					}
				}
				else
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.iOSExtendedMeleeInstIngame);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(370f, 545f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(880f, 545f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(390f, 200f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(1120f, 325f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1120f, 225f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1120f, 285f, 100f, 60f), Language.Get("Controller_21a", 60), this.largeInstructionStyle);
					if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
					{
						this.DrawShadowedText(new Rect(1176f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
						GUI.Label(new Rect(1246f, 703f, 60f, 60f), this.selectPS3);
					}
					else
					{
						GUI.Label(new Rect(70f, 703f, 60f, 60f), this.selectPS3);
						this.DrawShadowedText(new Rect(110f, 683f, 100f, 60f), Language.Get("M_Continue", 60), this.largeInstructionStyle);
					}
				}
			}
			else
			{
				GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.GenericMeleeInstIngame);
				this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
				this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
				this.DrawShadowedText(new Rect(1210f, 700f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(1050f, 700f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(1130f, 400f, 100f, 60f), Language.Get("Controller_21a", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(633f, 354f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.normal.textColor = Color.yellow;
				GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
				this.largeInstructionStyle.normal.textColor = Color.white;
				this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
				this.DrawShadowedText(new Rect(380f, 560f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
				this.DrawShadowedText(new Rect(230f, 40f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
				float fixedWidth = this.buttonStyle.fixedWidth;
				this.buttonStyle.fixedWidth = 150f;
				float fixedHeight = this.buttonStyle.fixedHeight;
				this.buttonStyle.fixedHeight = 40f;
				if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
				{
					this.buttonStyle.contentOffset = new Vector2(0f, -6f);
				}
				if (this.buttonStyle.font == null || this.buttonStyle.font != this.guiSkin.font)
				{
					this.buttonStyle.font = this.guiSkin.font;
				}
				if (GUI.Button(new Rect(603f, 654f, 150f, 60f), Language.Get("M_Continue", 60), this.buttonStyle))
				{
					mainmenu.showMeleeInstructions = false;
				}
				this.buttonStyle.fixedWidth = fixedWidth;
				this.buttonStyle.fixedHeight = fixedHeight;
				this.buttonStyle.contentOffset = new Vector2(0f, 0f);
			}
		}
		if (this.ControlsInstruction != 0)
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
			int num2 = this.ControlsInstruction;
			switch (num2)
			{
			case 1:
				if (!ShooterGameCamera.moveCalibrated)
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.PS3ExplorationInst);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.DrawShadowedText(new Rect(280f, 165f, 100f, 60f), Language.Get("Controller_15a", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(280f, 220f, 100f, 60f), Language.Get("Controller_5", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(280f, 280f, 100f, 60f), Language.Get("Controller_6", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(280f, 350f, 100f, 60f), Language.Get("Controller_7", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(280f, 490f, 100f, 60f), Language.Get("Controller_8", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(655f, 220f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(560f, 580f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(710f, 580f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_2", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(1000f, 220f, 100f, 60f), Language.Get("Controller_15", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 280f, 100f, 60f), Language.Get("Controller_14", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 350f, 100f, 60f), Language.Get("Controller_13", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 440f, 100f, 60f), Language.Get("Controller_12", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 490f, 100f, 60f), Language.Get("Controller_11", 60), this.largeInstructionStyle);
				}
				else
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.PS3MoveExplorationInst);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.DrawShadowedText(new Rect(360f, 165f, 100f, 60f), Language.Get("Controller_5", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(150f, 220f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(150f, 300f, 100f, 60f), Language.Get("Controller_6", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(150f, 350f, 100f, 60f), Language.Get("Controller_7", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(150f, 420f, 100f, 60f), Language.Get("Controller_8", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(600f, 490f, 100f, 60f), Language.Get("Controller_5", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_2", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(610f, 165f, 100f, 60f), Language.Get("Controller_15a", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1080f, 180f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1080f, 220f, 100f, 60f), Language.Get("Controller_15", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1080f, 300f, 100f, 60f), Language.Get("Controller_14", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1080f, 330f, 100f, 60f), Language.Get("Controller_13", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1080f, 370f, 100f, 60f), Language.Get("Controller_12", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1080f, 425f, 100f, 60f), Language.Get("Controller_11", 60), this.largeInstructionStyle);
				}
				break;
			case 2:
				if (!ShooterGameCamera.moveCalibrated)
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.PS3MeleeInst);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.DrawShadowedText(new Rect(280f, 165f, 100f, 60f), Language.Get("Controller_18", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(280f, 220f, 100f, 60f), Language.Get("Controller_20", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(655f, 220f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(633f, 140f, 100f, 60f), Language.Get("Controller_21", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(560f, 580f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(1000f, 165f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 220f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 490f, 100f, 60f), Language.Get("Controller_22", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 520f, 100f, 60f), Language.Get("Controller_22a", 60), this.largeInstructionStyle);
				}
				else
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.PS3MoveMeleeInst);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.DrawShadowedText(new Rect(240f, 220f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(530f, 320f, 100f, 60f), Language.Get("Controller_21a", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_3", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(985f, 330f, 100f, 60f), Language.Get("Controller_17", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(985f, 300f, 100f, 60f), Language.Get("Controller_19", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(985f, 220f, 100f, 60f), Language.Get("Controller_22", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(985f, 370f, 100f, 60f), Language.Get("Controller_18", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(985f, 425f, 100f, 60f), Language.Get("Controller_20", 60), this.largeInstructionStyle);
				}
				break;
			case 3:
				if (!ShooterGameCamera.moveCalibrated)
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.PS3DrivingInst);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.DrawShadowedText(new Rect(280f, 165f, 100f, 60f), Language.Get("Controller_24", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(655f, 220f, 100f, 60f), Language.Get("Controller_16", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(585f, 220f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(560f, 580f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_4", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(1000f, 165f, 100f, 60f), Language.Get("Controller_23", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1000f, 350f, 100f, 60f), Language.Get("Controller_25", 60), this.largeInstructionStyle);
				}
				else
				{
					GUI.DrawTexture(new Rect(0f, 0f, 1366f, 768f), this.PS3MoveDrivingInst);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleRight;
					this.DrawShadowedText(new Rect(150f, 220f, 100f, 60f), Language.Get("Controller_9", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(580f, 325f, 100f, 60f), Language.Get("Controller_23", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(390f, 165f, 100f, 60f), Language.Get("Controller_24", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.alignment = TextAnchor.MiddleCenter;
					this.DrawShadowedText(new Rect(633f, 40f, 100f, 60f), Language.Get("Controller_1", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.yellow;
					GUI.Label(new Rect(633f, 80f, 100f, 60f), Language.Get("Controller_4", 60), this.largeInstructionStyle);
					this.largeInstructionStyle.normal.textColor = Color.white;
					this.largeInstructionStyle.alignment = TextAnchor.MiddleLeft;
					this.DrawShadowedText(new Rect(850f, 380f, 100f, 60f), Language.Get("Controller_10", 60), this.largeInstructionStyle);
					this.DrawShadowedText(new Rect(1180f, 330f, 100f, 60f), Language.Get("Controller_25", 60), this.largeInstructionStyle);
				}
				break;
			}
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				this.DrawShadowedText(new Rect(1196f, 683f, 100f, 60f), Language.Get("M_Back", 60), this.largeInstructionStyle);
				GUI.Label(new Rect(1246f, 703f, 60f, 60f), this.backPS3);
			}
			else
			{
				GUI.Label(new Rect(70f, 703f, 60f, 60f), this.backPS3);
				this.DrawShadowedText(new Rect(110f, 683f, 100f, 60f), Language.Get("M_Back", 60), this.largeInstructionStyle);
			}
			return;
		}
		if (this.showMoveIcon)
		{
			if (this.iconNo == 0)
			{
				GUI.DrawTexture(new Rect(60f, 30f, 128f, 128f), this.moveCurser);
			}
			else if (this.iconNo == 1)
			{
				GUI.DrawTexture(new Rect((float)Screen.width - 188f, (float)Screen.height - 158f, 128f, 128f), this.moveCurser, ScaleMode.StretchToFill);
			}
		}
		if (this.isPauseMenu)
		{
			if (!mainmenu.pause || mainmenu.hintPause)
			{
				return;
			}
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
			GUI.DrawTexture(new Rect(683f - ((float)this.PauseBackground.width + 100f) / 2f, 384f - (float)this.PauseBackground.height / 2f, (float)this.PauseBackground.width + 100f, (float)this.PauseBackground.height + 50f), this.PauseBackground);
			GUI.depth = 1;
			GUILayout.BeginArea(new Rect(50f, 728f, 1266f, 60f));
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.currentMenuAfterUpdate != mainmenu.menus.PAUSE)
			{
				if (this.DrawBackButton(Language.Get("M_Back", 60)) && Event.current.type != EventType.Repaint)
				{
					this.backAction = true;
					base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				}
				GUILayout.FlexibleSpace();
				if ((this.currentMenuAfterUpdate == mainmenu.menus.ANIMATICSSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.CHAPTERSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.CUTSCENESSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.SURVIVALCHARSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.SURVIVALSELECTION) && this.DrawBackButton(Language.Get("M_Select", 60)) && Event.current.type != EventType.Repaint)
				{
					this.action = true;
					base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			GUI.depth = 2;
		}
		else
		{
			if (mainmenu.started && AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick && AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.Ouya)
			{
				float num3 = 64f;
				if (Screen.width > 1500)
				{
					num3 = 128f;
				}
				if (this.facebookButton != null)
				{
					GUI.DrawTexture(new Rect(20f, 20f, num3, num3), this.facebookButton, ScaleMode.ScaleToFit);
					if (Input.touchCount > 0)
					{
						Rect rect = new Rect(20f, (float)Screen.height - 20f - num3, num3, num3);
						if (rect.Contains(Input.touches[0].position))
						{
							this.facebookAction = true;
						}
					}
				}
				if (this.twitterButton != null)
				{
					GUI.DrawTexture(new Rect(num3 + 40f, 20f, num3, num3), this.twitterButton, ScaleMode.ScaleToFit);
					if (Input.touchCount > 0)
					{
						Rect rect2 = new Rect(num3 + 40f, (float)Screen.height - 20f - num3, num3, num3);
						if (rect2.Contains(Input.touches[0].position))
						{
							this.twitterAction = true;
						}
					}
				}
				if (this.websiteButton != null)
				{
					GUI.DrawTexture(new Rect(2f * num3 + 60f, 20f, num3, num3), this.websiteButton, ScaleMode.ScaleToFit);
					if (Input.touchCount > 0)
					{
						Rect rect3 = new Rect(2f * num3 + 60f, (float)Screen.height - 20f - num3, num3, num3);
						if (rect3.Contains(Input.touches[0].position))
						{
							this.websiteAction = true;
						}
					}
				}
				if (this.cartButton != null && (!SaveHandler.purchased || mainmenu.demoMode) && AndroidPlatform.IsIAPavailable())
				{
					GUI.DrawTexture(new Rect(3f * num3 + 80f, 20f, num3, num3), this.cartButton, ScaleMode.ScaleToFit);
					if (Input.touchCount > 0)
					{
						Rect rect4 = new Rect(3f * num3 + 80f, (float)Screen.height - 20f - num3, num3, num3);
						if (rect4.Contains(Input.touches[0].position))
						{
							mainmenu.showCart = true;
							this.selectedCol = 1;
							//GA.API.Design.NewEvent("CartScreenShown:fromCartButton");
						}
					}
				}
			}
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
			if (!mainmenu.started)
			{
				if (this.showBlinkingText)
				{
					if (AndroidPlatform.IsJoystickConnected())
					{
						this.largeInstructionStyle.normal.textColor = Color.black;
						GUI.Label(new Rect(583f, 537.6f, 200f, 100f), Language.Get("M_Start_Desktop", 60), this.largeInstructionStyle);
						this.largeInstructionStyle.normal.textColor = Color.white;
						GUI.Label(new Rect(581f, 535.6f, 200f, 100f), Language.Get("M_Start_Desktop", 60), this.largeInstructionStyle);
					}
					else
					{
						this.largeInstructionStyle.normal.textColor = Color.black;
						GUI.Label(new Rect(583f, 537.6f, 200f, 100f), Language.Get("M_Start_Mobile", 60), this.largeInstructionStyle);
						this.largeInstructionStyle.normal.textColor = Color.white;
						GUI.Label(new Rect(581f, 535.6f, 200f, 100f), Language.Get("M_Start_Mobile", 60), this.largeInstructionStyle);
					}
				}
				if (Time.time - this.showBlinkingTextTimer > 0.5f)
				{
					this.showBlinkingText = !this.showBlinkingText;
					this.showBlinkingTextTimer = Time.time;
				}
				if ((Input.touchCount > 0 || Input.anyKey) && Event.current.type == EventType.Repaint)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					mainmenu.started = true;
					this.SetBackground();
					this.startedTime = Time.time;
					if (AndroidPlatform.IsIAPavailable() && !SaveHandler.purchased)
					{
						this.currentMenu = mainmenu.menus.TRYORBUY;
						this.selectedRow = 1;
					}
				}
				return;
			}
			if (AndroidPlatform.IsJoystickConnected())
			{
				GUI.depth = 1;
				GUILayout.BeginArea(new Rect(0f, 708f, 1366f, 60f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
				{
					GUILayout.FlexibleSpace();
					if (this.currentMenuAfterUpdate != mainmenu.menus.MAIN)
					{
						this.DrawShadowedText(Language.Get("M_Back", 60), this.instructionStyle, null);
						GUILayout.Label(this.backPS3, new GUILayoutOption[0]);
					}
					this.DrawShadowedText(Language.Get("M_Select", 60), this.instructionStyle, null);
					GUILayout.Label(this.selectPS3, new GUILayoutOption[0]);
					GUILayout.Space(80f);
				}
				else
				{
					GUILayout.Space(80f);
					GUILayout.Label(this.selectPS3, new GUILayoutOption[0]);
					this.DrawShadowedText(Language.Get("M_Select", 60), this.instructionStyle, null);
					if (this.currentMenuAfterUpdate != mainmenu.menus.MAIN)
					{
						GUILayout.Label(this.backPS3, new GUILayoutOption[0]);
						this.DrawShadowedText(Language.Get("M_Back", 60), this.instructionStyle, null);
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
				GUI.depth = 2;
			}
			else
			{
				GUI.depth = 1;
				GUILayout.BeginArea(new Rect(50f, 728f, 1266f, 60f));
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (this.currentMenuAfterUpdate != mainmenu.menus.MAIN)
				{
					if (this.DrawBackButton(Language.Get("M_Back", 60)) && Event.current.type != EventType.Repaint)
					{
						this.backAction = true;
						base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
					}
					GUILayout.FlexibleSpace();
					if ((this.currentMenuAfterUpdate == mainmenu.menus.ANIMATICSSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.CHAPTERSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.CUTSCENESSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.SURVIVALCHARSELECTION || this.currentMenuAfterUpdate == mainmenu.menus.SURVIVALSELECTION) && this.DrawBackButton(Language.Get("M_Select", 60)) && Event.current.type != EventType.Repaint)
					{
						this.action = true;
						base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
				GUI.depth = 2;
			}
		}
		string caption = string.Empty;
		if (this.isPauseMenu && mainmenu.pause)
		{
			GUILayout.BeginArea(new Rect(263f, 208f, 900f, 400f));
		}
		else
		{
			GUILayout.BeginArea(new Rect(263f, 308f, 900f, 300f));
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		if (Language.CurrentLanguage() == LanguageCode.HI)
		{
			this.LabelStyle.contentOffset = new Vector2(0f, 0f);
		}
		switch (this.currentMenuAfterUpdate)
		{
		case mainmenu.menus.MAIN:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Main_Menu", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (mainmenu.demoMode)
			{
			}
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				if ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick && this.totlalRows >= 8) || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick && this.totlalRows >= 7))
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Options", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						this.currentMenu = mainmenu.menus.OPTIONS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Extras", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						this.currentMenu = mainmenu.menus.EXTRAS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Achievements", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						if (Social.localUser.authenticated)
						{
							Social.ShowAchievementsUI();
						}
						else
						{
							Social.localUser.Authenticate(delegate(bool success)
							{
								if (success)
								{
									Social.ShowAchievementsUI();
								}
							});
						}
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_Credits", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.GoToLevel("Credits", false);
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				num4++;
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Exit", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						this.currentMenu = mainmenu.menus.CONFIRMEXIT;
						this.selectedRow = 1;
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				GUILayout.EndVertical();
				GUILayout.Space(10f);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if (SaveHandler.levelReached > 1 || (SaveHandler.levelReached == 1 && SaveHandler.checkpointReached > 0))
				{
					if ((this.DrawButton(Language.Get("M_Continue", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						if (((SaveHandler.levelReached == 2 && SaveHandler.checkpointReached >= 1) || SaveHandler.levelReached >= 2) && !SaveHandler.purchased && !mainmenu.demoMode && AndroidPlatform.IsIAPavailable())
						{
							mainmenu.showCart = true;
							this.selectedCol = 1;
							//GA.API.Design.NewEvent("CartScreenShown:fromContinueButton");
						}
						else
						{
							this.newGame = false;
							DifficultyManager.difficulty = SaveHandler.currentDifficultyLevel;
							this.LoadLevel();
							this.selectedRow = 1;
							base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						}
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (mainmenu.demoMode || SaveHandler.levelReached > 1)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Chapter_Selection", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						this.currentMenu = mainmenu.menus.CHAPTERSELECTION;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_New_Game", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					if (SaveHandler.levelReached > 1 || (SaveHandler.levelReached == 1 && SaveHandler.checkpointReached > 0))
					{
						this.currentMenu = mainmenu.menus.NEWGAMECONFIRMATION;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.newGame = true;
						this.currentMenu = mainmenu.menus.DIFFICULTY;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
				}
				this.checkForHoverAndPlaySound();
				num4++;
				if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Survival_Mode", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						if (mainmenu.demoMode || SaveHandler.gameFinished > 0)
						{
							if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
							{
								this.currentMenu = mainmenu.menus.SURVIVALMENU;
							}
							else
							{
								this.currentMenu = mainmenu.menus.SURVIVALSELECTION;
							}
							this.selectedRow = 1;
							base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						}
						else
						{
							this.errorMessage = "M_SURVIVAL_LOCKED";
							this.currentMenu = mainmenu.menus.ERRORMESSAGE;
							this.selectedRow = 1;
							base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						}
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick && this.totlalRows <= 7) || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick && this.totlalRows <= 6))
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Options", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						this.currentMenu = mainmenu.menus.OPTIONS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
			}
			else
			{
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if (SaveHandler.levelReached > 1 || (SaveHandler.levelReached == 1 && SaveHandler.checkpointReached > 0))
				{
					if ((this.DrawButton(Language.Get("M_Continue", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						if (((SaveHandler.levelReached == 2 && SaveHandler.checkpointReached >= 1) || SaveHandler.levelReached >= 2) && !SaveHandler.purchased && !mainmenu.demoMode && AndroidPlatform.IsIAPavailable())
						{
							mainmenu.showCart = true;
							this.selectedCol = 1;
							//GA.API.Design.NewEvent("CartScreenShown:fromContinueButton");
						}
						else
						{
							this.newGame = false;
							DifficultyManager.difficulty = SaveHandler.currentDifficultyLevel;
							this.LoadLevel();
							this.selectedRow = 1;
							base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						}
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (mainmenu.demoMode || SaveHandler.levelReached > 1)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Chapter_Selection", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						this.currentMenu = mainmenu.menus.CHAPTERSELECTION;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_New_Game", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					if (SaveHandler.levelReached > 1 || (SaveHandler.levelReached == 1 && SaveHandler.checkpointReached > 0))
					{
						this.currentMenu = mainmenu.menus.NEWGAMECONFIRMATION;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.newGame = true;
						this.currentMenu = mainmenu.menus.DIFFICULTY;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
				}
				this.checkForHoverAndPlaySound();
				num4++;
				if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Survival_Mode", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						if (mainmenu.demoMode || SaveHandler.gameFinished > 0)
						{
							if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
							{
								this.currentMenu = mainmenu.menus.SURVIVALMENU;
							}
							else
							{
								this.currentMenu = mainmenu.menus.SURVIVALSELECTION;
							}
							this.selectedRow = 1;
							base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						}
						else
						{
							this.errorMessage = "M_SURVIVAL_LOCKED";
							this.currentMenu = mainmenu.menus.ERRORMESSAGE;
							this.selectedRow = 1;
							base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						}
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick && num4 > 3) || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick && num4 > 2))
				{
					GUILayout.EndVertical();
					GUILayout.Space(10f);
					GUILayout.BeginVertical(new GUILayoutOption[0]);
				}
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_Options", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.currentMenu = mainmenu.menus.OPTIONS;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				num4++;
				if ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick && num4 == 4) || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick && num4 == 3))
				{
					GUILayout.EndVertical();
					GUILayout.Space(10f);
					GUILayout.BeginVertical(new GUILayoutOption[0]);
				}
				if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Extras", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						this.currentMenu = mainmenu.menus.EXTRAS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
				{
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Achievements", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						if (Social.localUser.authenticated)
						{
							Social.ShowAchievementsUI();
						}
						else
						{
							Social.localUser.Authenticate(delegate(bool success)
							{
								if (success)
								{
									Social.ShowAchievementsUI();
								}
							});
						}
					}
					this.checkForHoverAndPlaySound();
					num4++;
				}
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_Credits", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.GoToLevel("Credits", false);
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick)
				{
					num4++;
					if (this.selectedRow == num4)
					{
						GUI.SetNextControlName("focusedButton");
					}
					if ((this.DrawButton(Language.Get("M_Exit", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
					{
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
						this.currentMenu = mainmenu.menus.CONFIRMEXIT;
						this.selectedRow = 1;
					}
					this.checkForHoverAndPlaySound();
				}
			}
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.DIFFICULTY:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Difficulty_Level", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Easy", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint && !this.faded)
			{
				DifficultyManager.difficulty = DifficultyManager.Difficulty.EASY;
				this.currentMenu = mainmenu.menus.AIMINGMODE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Medium", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint && !this.faded)
			{
				DifficultyManager.difficulty = DifficultyManager.Difficulty.MEDIUM;
				this.currentMenu = mainmenu.menus.AIMINGMODE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Hard", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint && !this.faded)
			{
				DifficultyManager.difficulty = DifficultyManager.Difficulty.HARD;
				this.currentMenu = mainmenu.menus.AIMINGMODE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			GUILayout.Space(10f);
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (((this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			this.totlalRows = num4;
			GUI.FocusControl("focusedButton");
			break;
		}
		case mainmenu.menus.CHAPTERSELECTION:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Chapter_Selection", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(188f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || this.leftArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (this.currentChapter > 0)
				{
					this.currentChapter--;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(50f);
			int num2 = this.currentChapter;
			switch (num2)
			{
			case 0:
				this.DrawLabel(Language.Get("M_Prolouge", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
				break;
			case 1:
				this.DrawLabel(Language.Get("M_Egypt", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
				break;
			case 2:
				this.DrawLabel(Language.Get("M_Desert_Escape", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
				break;
			case 3:
				this.DrawLabel(Language.Get("M_Tangier", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
				break;
			case 4:
				this.DrawLabel(Language.Get("M_TangierChase", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
				break;
			case 5:
				this.DrawLabel(Language.Get("M_CarChase", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
				break;
			}
			GUILayout.Space(50f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || this.rightArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (this.currentChapter < this.chapterSelectPics.Length - 1)
				{
					num2 = this.currentChapter;
					switch (num2)
					{
					case 0:
						if (mainmenu.demoMode || SaveHandler.levelReached >= 5)
						{
							this.currentChapter++;
						}
						break;
					case 1:
						if (mainmenu.demoMode || SaveHandler.levelReached >= 6)
						{
							this.currentChapter++;
						}
						break;
					case 2:
						if (mainmenu.demoMode || SaveHandler.levelReached >= 9)
						{
							this.currentChapter++;
						}
						break;
					case 3:
						if (mainmenu.demoMode || SaveHandler.levelReached >= 11)
						{
							this.currentChapter++;
						}
						break;
					case 4:
						if ((mainmenu.demoMode || SaveHandler.levelReached >= 12) && AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick)
						{
							this.currentChapter++;
						}
						break;
					}
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(110f);
			GUILayout.Label(this.chapterSelectPics[this.currentChapter], this.emptyStyle, new GUILayoutOption[]
			{
				GUILayout.Height(150f)
			});
			GUILayout.EndHorizontal();
			if (this.action && Event.current.type != EventType.Repaint)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				num2 = this.currentChapter;
				switch (num2)
				{
				case 0:
					this.GoToLevel("PreLoadingPrologue", true);
					break;
				case 1:
					this.GoToLevel("Egypt-Cutscene1", true);
					break;
				case 2:
					this.GoToLevel("LoadingEgyptCutscene4", true);
					break;
				case 3:
					this.GoToLevel("Morocco-Cutscene1-Video", true);
					break;
				case 4:
					this.GoToLevel("LoadingMoroccoCutscene3", true);
					break;
				case 5:
					this.GoToLevel("Car-Intro-Video", true);
					break;
				}
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.totlalRows = 0;
			break;
		}
		case mainmenu.menus.OPTIONS:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Options", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Audio", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.AUDIO;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Controls", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.CONTROLS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Language", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.LANGUAGE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(10f);
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (((this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				if (!this.isPauseMenu)
				{
					this.currentMenu = mainmenu.menus.MAIN;
					this.selectedRow = 1;
				}
				else
				{
					this.currentMenu = mainmenu.menus.PAUSE;
					this.selectedRow = 1;
				}
				SaveHandler.SaveSettings();
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.GRAPHICS:
		{
			this.DrawLabel(Language.Get("M_Graphics_Options", 60));
			int num4 = 1;
			this.DrawLabel(Language.Get("M_Quality_Level", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (QualitySettings.GetQualityLevel() > 0)
				{
					QualitySettings.DecreaseLevel();
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			string text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			switch (text)
			{
			case "Fantastic":
				caption = Language.Get("M_Very_High", 60);
				break;
			case "Beautiful":
				caption = Language.Get("M_High", 60);
				break;
			case "Good":
				caption = Language.Get("M_Medium_Quality", 60);
				break;
			case "Simple":
				caption = Language.Get("M_Low", 60);
				break;
			}
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.FlexibleSpace();
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (QualitySettings.GetQualityLevel() < QualitySettings.names.Length - 1)
				{
					QualitySettings.IncreaseLevel();
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.EndHorizontal();
			num4++;
			this.DrawLabel(Language.Get("M_Resolution", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (this.currentResolution > 0)
				{
					this.currentResolution--;
					Screen.SetResolution(Screen.resolutions[this.currentResolution].width, Screen.resolutions[this.currentResolution].height, Screen.fullScreen);
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			caption = Screen.width + " x " + Screen.height;
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.FlexibleSpace();
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (this.currentResolution < Screen.resolutions.Length - 1)
				{
					this.currentResolution++;
					Screen.SetResolution(Screen.resolutions[this.currentResolution].width, Screen.resolutions[this.currentResolution].height, Screen.fullScreen);
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.EndHorizontal();
			num4++;
			this.DrawLabel(Language.Get("M_Fullscreen", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				Screen.fullScreen = !Screen.fullScreen;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			caption = ((!Screen.fullScreen) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.FlexibleSpace();
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				Screen.fullScreen = !Screen.fullScreen;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.EndHorizontal();
			num4++;
			this.DrawLabel(Language.Get("M_Enable_Anaglyph_3D", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				SpeechManager.enable3D = !SpeechManager.enable3D;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			caption = ((!SpeechManager.enable3D) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.FlexibleSpace();
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				SpeechManager.enable3D = !SpeechManager.enable3D;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.EndHorizontal();
			num4++;
			this.DrawLabel(Language.Get("M_VSync", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (QualitySettings.vSyncCount > 0)
				{
					QualitySettings.vSyncCount--;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			int num2 = QualitySettings.vSyncCount;
			switch (num2)
			{
			case 0:
				caption = Language.Get("M_VSync0", 60);
				break;
			case 1:
				caption = Language.Get("M_VSync1", 60);
				break;
			case 2:
				caption = Language.Get("M_VSync2", 60);
				break;
			}
			GUILayout.FlexibleSpace();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.FlexibleSpace();
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (QualitySettings.vSyncCount < 2)
				{
					QualitySettings.vSyncCount++;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.EndHorizontal();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (((this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.OPTIONS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (this.selectedRow == num4)
			{
				GUI.FocusControl("focusedButton");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
			}
			else
			{
				GUI.SetNextControlName("loseFocus");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
				GUI.FocusControl("loseFocus");
			}
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.AUDIO:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Audio_Options", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Master_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			int num4 = 1;
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (AudioListener.volume >= 0.1f)
				{
					AudioListener.volume -= 0.1f;
				}
				AudioListener.volume = Mathf.Round(AudioListener.volume * 100f) / 100f;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = (AudioListener.volume * 10f).ToString();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (AudioListener.volume <= 0.9f)
				{
					AudioListener.volume += 0.1f;
				}
				AudioListener.volume = Mathf.Round(AudioListener.volume * 100f) / 100f;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Master_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Music_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.musicVolume >= 0.1f)
				{
					SpeechManager.musicVolume -= 0.1f;
				}
				SpeechManager.musicVolume = Mathf.Round(SpeechManager.musicVolume * 100f) / 100f;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = (SpeechManager.musicVolume * 10f).ToString();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.musicVolume <= 0.9f)
				{
					SpeechManager.musicVolume += 0.1f;
				}
				SpeechManager.musicVolume = Mathf.Round(SpeechManager.musicVolume * 100f) / 100f;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Music_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Speech_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.speechVolume >= 0.1f)
				{
					SpeechManager.speechVolume -= 0.1f;
				}
				SpeechManager.speechVolume = Mathf.Round(SpeechManager.speechVolume * 100f) / 100f;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = (SpeechManager.speechVolume * 10f).ToString();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.speechVolume <= 0.9f)
				{
					SpeechManager.speechVolume += 0.1f;
				}
				SpeechManager.speechVolume = Mathf.Round(SpeechManager.speechVolume * 100f) / 100f;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Speech_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_SFX_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.sfxVolume >= 0.1f)
				{
					SpeechManager.sfxVolume -= 0.1f;
				}
				SpeechManager.sfxVolume = Mathf.Round(SpeechManager.sfxVolume * 100f) / 100f;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = (SpeechManager.sfxVolume * 10f).ToString();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.sfxVolume <= 0.9f)
				{
					SpeechManager.sfxVolume += 0.1f;
				}
				SpeechManager.sfxVolume = Mathf.Round(SpeechManager.sfxVolume * 100f) / 100f;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_SFX_Volume", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.OPTIONS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (this.selectedRow == num4)
			{
				GUI.FocusControl("focusedButton");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
			}
			else
			{
				GUI.SetNextControlName("loseFocus");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
				GUI.FocusControl("loseFocus");
			}
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.CONTROLS:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Controls_Options", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Inverse_X_Axis", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				ShooterGameCamera.inverseX = !ShooterGameCamera.inverseX;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = ((!ShooterGameCamera.inverseX) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				ShooterGameCamera.inverseX = !ShooterGameCamera.inverseX;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Inverse_X_Axis", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Inverse_Y_Axis", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				ShooterGameCamera.inverseY = !ShooterGameCamera.inverseY;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = ((!ShooterGameCamera.inverseY) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				ShooterGameCamera.inverseY = !ShooterGameCamera.inverseY;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Inverse_Y_Axis", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Camera_Sensitivity", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.currentCameraSensitivity > 0)
				{
					mainmenu.currentCameraSensitivity--;
				}
				int num2 = mainmenu.currentCameraSensitivity;
				switch (num2)
				{
				case 0:
					ShooterGameCamera.mouseSensitivity = 0.035f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 1:
					ShooterGameCamera.mouseSensitivity = 0.045f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 2:
					ShooterGameCamera.mouseSensitivity = 0.055f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 3:
					ShooterGameCamera.mouseSensitivity = 0.065f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 4:
					ShooterGameCamera.mouseSensitivity = 0.075f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 5:
					ShooterGameCamera.mouseSensitivity = 0.085f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 6:
					ShooterGameCamera.mouseSensitivity = 0.095f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 7:
					ShooterGameCamera.mouseSensitivity = 0.105f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 8:
					ShooterGameCamera.mouseSensitivity = 0.115f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 9:
					ShooterGameCamera.mouseSensitivity = 0.125f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 10:
					ShooterGameCamera.mouseSensitivity = 0.135f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				default:
					ShooterGameCamera.mouseSensitivity = 0.085f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = mainmenu.currentCameraSensitivity.ToString();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.currentCameraSensitivity < 10)
				{
					mainmenu.currentCameraSensitivity++;
				}
				int num2 = mainmenu.currentCameraSensitivity;
				switch (num2)
				{
				case 0:
					ShooterGameCamera.mouseSensitivity = 0.035f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 1:
					ShooterGameCamera.mouseSensitivity = 0.045f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 2:
					ShooterGameCamera.mouseSensitivity = 0.055f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 3:
					ShooterGameCamera.mouseSensitivity = 0.065f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 4:
					ShooterGameCamera.mouseSensitivity = 0.075f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 5:
					ShooterGameCamera.mouseSensitivity = 0.085f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 6:
					ShooterGameCamera.mouseSensitivity = 0.095f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 7:
					ShooterGameCamera.mouseSensitivity = 0.105f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 8:
					ShooterGameCamera.mouseSensitivity = 0.115f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 9:
					ShooterGameCamera.mouseSensitivity = 0.125f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				case 10:
					ShooterGameCamera.mouseSensitivity = 0.135f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				default:
					ShooterGameCamera.mouseSensitivity = 0.085f * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameStick) ? 4f : 8f) : 1f);
					break;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Camera_Sensitivity", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Aim_Assest", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (ShooterGameCamera.aimAssestType != ShooterGameCamera.AimAssestTypes.OFF)
				{
					ShooterGameCamera.aimAssestType--;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			switch (ShooterGameCamera.aimAssestType)
			{
			case ShooterGameCamera.AimAssestTypes.OFF:
				caption = Language.Get("M_Aim_OFF", 60);
				break;
			case ShooterGameCamera.AimAssestTypes.SEMI:
				caption = Language.Get("M_Aim_SEMI", 60);
				break;
			case ShooterGameCamera.AimAssestTypes.HARD:
				caption = Language.Get("M_Aim_AUTO", 60);
				break;
			}
			GUILayout.Space(150f);
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (ShooterGameCamera.aimAssestType != ShooterGameCamera.AimAssestTypes.HARD)
				{
					ShooterGameCamera.aimAssestType++;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Aim_Assest", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.Space(10f);
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.OPTIONS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (this.selectedRow >= 5)
			{
				GUI.FocusControl("focusedButton");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
			}
			else
			{
				GUI.SetNextControlName("loseFocus");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
				GUI.FocusControl("loseFocus");
			}
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.LANGUAGE:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Language_Options", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Voice", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
				{
					SpeechManager.currentVoiceLanguage = SpeechManager.VoiceLanguage.English;
				}
				else
				{
					SpeechManager.currentVoiceLanguage = SpeechManager.VoiceLanguage.Arabic;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = SpeechManager.currentVoiceLanguage.ToString();
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
				{
					SpeechManager.currentVoiceLanguage = SpeechManager.VoiceLanguage.English;
				}
				else
				{
					SpeechManager.currentVoiceLanguage = SpeechManager.VoiceLanguage.Arabic;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Voice", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Menus_Subtitles", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				if (this.currentLanguage == 0)
				{
					this.currentLanguage = this.languages.GetLength(0) - 1;
				}
				else
				{
					this.currentLanguage = (this.currentLanguage - 1) % this.languages.GetLength(0);
				}
				Language.SwitchLanguageStr(this.languages[this.currentLanguage, 0]);
				if (!this.isPauseMenu)
				{
					this.SetBackground();
				}
				this.SetCurrentLanguageFont();
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = this.languages[this.currentLanguage, 1];
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				this.currentLanguage = (this.currentLanguage + 1) % this.languages.GetLength(0);
				Language.SwitchLanguageStr(this.languages[this.currentLanguage, 0]);
				if (!this.isPauseMenu)
				{
					this.SetBackground();
				}
				this.SetCurrentLanguageFont();
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Menus_Subtitles", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				this.DrawLabel(Language.Get("M_Subtitles", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				SpeechManager.enableSubtitles = !SpeechManager.enableSubtitles;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(150f);
			caption = ((!SpeechManager.enableSubtitles) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(150f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				SpeechManager.enableSubtitles = !SpeechManager.enableSubtitles;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(80f);
				this.DrawLabel(Language.Get("M_Subtitles", 60), this.BoldOutlineTextStyle, GUILayout.Width(290f));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			num4++;
			GUILayout.Space(10f);
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.OPTIONS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (this.selectedRow == num4)
			{
				GUI.FocusControl("focusedButton");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
			}
			else
			{
				GUI.SetNextControlName("loseFocus");
				GUILayout.Label(string.Empty, new GUILayoutOption[]
				{
					GUILayout.MaxHeight(0f)
				});
				GUI.FocusControl("loseFocus");
			}
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.PAUSE:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Pause_Menu", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Resume", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if ((Camera.main.GetComponent<ShooterGameCamera>() != null && !Camera.main.GetComponent<ShooterGameCamera>().meleeCamera) || Camera.main.GetComponent<QuadGameCamera>() != null || Camera.main.GetComponent<CarCameras>() != null)
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
				mainmenu.pause = false;
				Time.timeScale = 1f;
				AudioListener.pause = false;
				if (Camera.main != null && Camera.main.GetComponent<BlurEffect>() != null)
				{
					Camera.main.GetComponent<BlurEffect>().enabled = false;
				}
				Screen.lockCursor = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get(this.survivalLevel ? "M_Restart_Survival" : "M_Restart", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.LASTCHECKPOINTCONFIRM;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Options", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.OPTIONS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Back_To_Mainmenu", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.EXITTOMAINCONFIRM;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (mainmenu.demoMode)
			{
				num4++;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton("Debug") || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.currentMenu = mainmenu.menus.DEBUG;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
			}
			this.totlalRows = num4;
			GUI.FocusControl("focusedButton");
			break;
		}
		case mainmenu.menus.NEWGAMECONFIRMATION:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_New_Game", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			this.DrawLabel(Language.Get("M_New_Game_Confirm1", 60), this.BoldOutlineTextStyle, GUILayout.Width(390f));
			GUILayout.Space(30f);
			this.DrawLabel(Language.Get("M_New_Game_Confirm2", 60), this.BoldOutlineTextStyle, GUILayout.Width(390f));
			GUILayout.Space(20f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Yes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.newGame = true;
				this.currentMenu = mainmenu.menus.DIFFICULTY;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_No", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.DEBUG:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("Debug", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(10f);
			int num4 = 1;
			if (Application.loadedLevelName != "Ozgur_Melee" && Application.loadedLevelName != "Tangier" && Application.loadedLevelName != "Tangier2" && Application.loadedLevelName != "TangierMarket-Block5" && Application.loadedLevelName != "Chase1" && Application.loadedLevelName != "Chase2" && Application.loadedLevelName != "CarChase")
			{
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton("Skip to next Checkpoint") || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					GunManager gm = AnimationHandler.instance.GetComponent<WeaponHandling>().gm;
					if (mainmenu.replayLevel)
					{
						Checkpoint[] array = (Checkpoint[])UnityEngine.Object.FindObjectsOfType(typeof(Checkpoint));
						foreach (Checkpoint checkpoint in array)
						{
							if (checkpoint.checkpointNo == SaveHandler.replayCheckpointReached + 1)
							{
								if (gm != null)
								{
									SaveHandler.SaveCheckpointOnReplay(SaveHandler.replayCheckpointReached + 1, checkpoint.transform.position, checkpoint.transform.rotation.eulerAngles, (!(gm.currentSecondaryGun != null)) ? string.Empty : gm.currentSecondaryGun.gunName, (!(gm.currentPrimaryGun != null)) ? string.Empty : gm.currentPrimaryGun.gunName, (!(gm.currentSecondaryGun != null)) ? 0 : gm.currentSecondaryGun.totalClips, (!(gm.currentSecondaryGun != null)) ? 0 : gm.currentSecondaryGun.currentRounds, (!(gm.currentPrimaryGun != null)) ? 0 : gm.currentPrimaryGun.totalClips, (!(gm.currentPrimaryGun != null)) ? 0 : gm.currentPrimaryGun.currentRounds, gm.currentGrenades);
								}
								else
								{
									SaveHandler.SaveCheckpointOnReplay(SaveHandler.replayCheckpointReached + 1, checkpoint.transform.position, checkpoint.transform.rotation.eulerAngles, string.Empty, string.Empty, 0, 0, 0, 0, 0);
								}
								mainmenu.pause = false;
								Time.timeScale = 1f;
								AudioListener.pause = false;
								if (Camera.main != null && Camera.main.GetComponent<BlurEffect>() != null)
								{
									Camera.main.GetComponent<BlurEffect>().enabled = false;
								}
								Screen.lockCursor = false;
								Application.LoadLevel("PreLoading" + Application.loadedLevelName);
								break;
							}
						}
					}
					else
					{
						Checkpoint[] array3 = (Checkpoint[])UnityEngine.Object.FindObjectsOfType(typeof(Checkpoint));
						foreach (Checkpoint checkpoint2 in array3)
						{
							if (checkpoint2.checkpointNo == SaveHandler.checkpointReached + 1)
							{
								if (gm != null)
								{
									SaveHandler.SaveCheckpoint(SaveHandler.levelReached, SaveHandler.checkpointReached + 1, checkpoint2.transform.position, checkpoint2.transform.rotation.eulerAngles, (!(gm.currentSecondaryGun != null)) ? string.Empty : gm.currentSecondaryGun.gunName, (!(gm.currentPrimaryGun != null)) ? string.Empty : gm.currentPrimaryGun.gunName, (!(gm.currentSecondaryGun != null)) ? 0 : gm.currentSecondaryGun.totalClips, (!(gm.currentSecondaryGun != null)) ? 0 : gm.currentSecondaryGun.currentRounds, (!(gm.currentPrimaryGun != null)) ? 0 : gm.currentPrimaryGun.totalClips, (!(gm.currentPrimaryGun != null)) ? 0 : gm.currentPrimaryGun.currentRounds, gm.currentGrenades);
								}
								else
								{
									SaveHandler.SaveCheckpoint(SaveHandler.levelReached, SaveHandler.checkpointReached + 1, checkpoint2.transform.position, checkpoint2.transform.rotation.eulerAngles, string.Empty, string.Empty, 0, 0, 0, 0, 0);
								}
								mainmenu.pause = false;
								Time.timeScale = 1f;
								AudioListener.pause = false;
								if (Camera.main != null && Camera.main.GetComponent<BlurEffect>() != null)
								{
									Camera.main.GetComponent<BlurEffect>().enabled = false;
								}
								Screen.lockCursor = false;
								Application.LoadLevel("PreLoading" + Application.loadedLevelName);
								break;
							}
						}
					}
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				num4++;
			}
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton("Skip to next Level") || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (this.nextLevelName != string.Empty)
				{
					mainmenu.pause = false;
					Time.timeScale = 1f;
					AudioListener.pause = false;
					if (Camera.main != null && Camera.main.GetComponent<BlurEffect>() != null)
					{
						Camera.main.GetComponent<BlurEffect>().enabled = false;
					}
					Screen.lockCursor = false;
					if (!mainmenu.replayLevel)
					{
						SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
					}
					else
					{
						SaveHandler.ResetReplayLevelValues();
					}
					Application.LoadLevel(this.nextLevelName);
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (Application.loadedLevelName != "Ozgur_Melee" && Application.loadedLevelName != "Tangier" && Application.loadedLevelName != "Tangier_Backup" && Application.loadedLevelName != "Tangier2" && Application.loadedLevelName != "TangierMarket-Block5" && Application.loadedLevelName != "Chase1" && Application.loadedLevelName != "Chase2" && Application.loadedLevelName != "CityChase" && Application.loadedLevelName != "prologue2" && Application.loadedLevelName != "CityChasePS3")
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.DrawLabel("Unlimited Health", this.BoldOutlineTextStyle, GUILayout.Width(290f));
				if (Application.loadedLevelName != "QuadChase" && Application.loadedLevelName != "CarChase" && Application.loadedLevelName != "prologue2")
				{
					Health component = AnimationHandler.instance.GetComponent<Health>();
					if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
					{
						component.unlimitedHealth = !component.unlimitedHealth;
						this.leftArrowAction = false;
						base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					GUILayout.Space(50f);
					caption = ((!component.unlimitedHealth) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
					if (this.selectedRow == num4)
					{
						this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
					}
					else
					{
						this.DrawLabel(caption, this.plaingTextStyle, null);
					}
					GUILayout.Space(30f);
					if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
					{
						component.unlimitedHealth = !component.unlimitedHealth;
						this.rightArrowAction = false;
						base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
					}
				}
				else
				{
					QBHealth qbhealth = null;
					if (GameObject.FindGameObjectWithTag("Player") != null)
					{
						qbhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<QBHealth>();
					}
					if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
					{
						qbhealth.unlimitedHealth = !qbhealth.unlimitedHealth;
						this.leftArrowAction = false;
						base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					GUILayout.Space(50f);
					caption = ((!qbhealth.unlimitedHealth) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
					if (this.selectedRow == num4)
					{
						this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
					}
					else
					{
						this.DrawLabel(caption, this.plaingTextStyle, null);
					}
					GUILayout.Space(30f);
					if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
					{
						qbhealth.unlimitedHealth = !qbhealth.unlimitedHealth;
						this.rightArrowAction = false;
						base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
					}
				}
				this.checkForHoverAndPlaySound();
				num4++;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				if (Application.loadedLevelName != "QuadChase" && Application.loadedLevelName != "Prologue" && Application.loadedLevelName != "part2")
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					this.DrawLabel("Unlimited Ammo", this.BoldOutlineTextStyle, GUILayout.Width(290f));
					GunManager gm2 = AnimationHandler.instance.GetComponent<WeaponHandling>().gm;
					if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
					{
						gm2.currentGun.unlimited = !gm2.currentGun.unlimited;
						this.leftArrowAction = false;
						base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					GUILayout.Space(50f);
					caption = ((!gm2.currentGun.unlimited) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
					if (this.selectedRow == num4)
					{
						this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
					}
					else
					{
						this.DrawLabel(caption, this.plaingTextStyle, null);
					}
					GUILayout.Space(30f);
					if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
					{
						gm2.currentGun.unlimited = !gm2.currentGun.unlimited;
						this.rightArrowAction = false;
						base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
					}
					this.checkForHoverAndPlaySound();
					num4++;
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.DrawLabel("Show FPS Counter", this.BoldOutlineTextStyle, GUILayout.Width(290f));
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || (this.selectedRow == num4 && this.leftArrowAction)) && Event.current.type != EventType.Repaint)
			{
				HUDFPS.showFPS = !HUDFPS.showFPS;
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(50f);
			caption = ((!HUDFPS.showFPS) ? Language.Get("M_Off", 60) : Language.Get("M_On", 60));
			if (this.selectedRow == num4)
			{
				this.DrawLabel(caption, this.plaingTextSelectedStyle, null);
			}
			else
			{
				this.DrawLabel(caption, this.plaingTextStyle, null);
			}
			GUILayout.Space(30f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || (this.selectedRow == num4 && this.rightArrowAction)) && Event.current.type != EventType.Repaint)
			{
				HUDFPS.showFPS = !HUDFPS.showFPS;
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.PAUSE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			this.totlalRows = num4;
			GUI.FocusControl("focusedButton");
			break;
		}
		case mainmenu.menus.REQUESTCALIBRATION:
		{
			this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get("M_Move_Request", 60), this.plaingTextStyle, GUILayout.Width(290f));
			GUILayout.Space(50f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Yes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.returnToMainAfterCalibtation = true;
				this.calebrationStepUpdate = 0;
				this.calebrationStep = 0;
				this.currentMenu = mainmenu.menus.MOVECALIBRATION;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_No", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.CAMERADISCONNECTED:
		{
			this.DrawLabel(Language.Get("M_Camera_Disconnected", 60));
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get("M_Cam_Disconnected_MSG", 60), this.plaingTextStyle, GUILayout.Width(290f));
			GUILayout.Space(50f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_CANCEL", 60)) || this.action || this.backAction) && Event.current.type != EventType.Repaint)
			{
				if (this.returnToMainAfterCalibtation && !this.isPauseMenu)
				{
					this.currentMenu = mainmenu.menus.MAIN;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.currentMenu = mainmenu.menus.CONTROLS;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.MOVECALIBRATION:
		{
			int num2 = this.calebrationStep;
			switch (num2)
			{
			case 0:
				if (this.calebrationStepUpdate == 0)
				{
					this.calebrationStepUpdate = 1;
				}
				break;
			case 1:
			{
				this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
				GUILayout.Space(50f);
				this.DrawLabel(Language.Get("M_CAM_BLUE", 30), this.plaingTextStyle, GUILayout.Width(290f));
				GUILayout.Space(110f);
				int num4 = 1;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_OK", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.calebrationStepUpdate = 2;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				num4++;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_CANCEL", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
				{
					if (this.returnToMainAfterCalibtation)
					{
						this.currentMenu = mainmenu.menus.MAIN;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.currentMenu = mainmenu.menus.CONTROLS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
				}
				this.checkForHoverAndPlaySound();
				GUI.FocusControl("focusedButton");
				this.totlalRows = num4;
				break;
			}
			case 2:
			{
				this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
				GUILayout.Space(50f);
				this.DrawLabel(Language.Get("M_SETUP_INST1", 30), this.plaingTextStyle, GUILayout.Width(290f));
				GUILayout.Space(110f);
				int num4 = 1;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_CANCEL", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
				{
					this.currentMenu = mainmenu.menus.CONTROLS;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				GUI.FocusControl("focusedButton");
				if (this.calibrating)
				{
					this.calebrationStepUpdate = 3;
				}
				this.totlalRows = num4;
				break;
			}
			case 3:
				this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
				GUILayout.Space(50f);
				this.DrawLabel(Language.Get("M_Calibrating", 30), this.BoldOutlineTextStyle, GUILayout.Width(290f));
				GUILayout.Space(110f);
				this.totlalRows = 0;
				break;
			case 4:
			{
				this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
				GUILayout.Space(50f);
				this.DrawLabel(Language.Get("M_Calibrating_ERR", 30), this.plaingTextStyle, GUILayout.Width(290f));
				GUILayout.Space(110f);
				int num4 = 1;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_OK", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.calebrationStepUpdate = 2;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				this.checkForHoverAndPlaySound();
				GUI.FocusControl("focusedButton");
				this.totlalRows = num4;
				break;
			}
			case 5:
			{
				this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
				GUILayout.Space(50f);
				this.DrawLabel(Language.Get("M_SETUP_INST2", 30), this.plaingTextStyle, GUILayout.Width(290f));
				if (this.iconNo == 0)
				{
					this.showMoveIcon = true;
				}
				else if (this.iconNo == 1)
				{
					this.showMoveIcon = true;
				}
				else if (this.iconNo == 2)
				{
					this.showMoveIcon = false;
					this.iconNoUpdate = 0;
					this.calebrationStepUpdate = 6;
				}
				GUILayout.Space(110f);
				int num4 = 1;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_CANCEL", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
				{
					if (this.returnToMainAfterCalibtation)
					{
						this.showMoveIcon = false;
						this.iconNoUpdate = -1;
						this.iconNo = -1;
						this.currentMenu = mainmenu.menus.MAIN;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.showMoveIcon = false;
						this.iconNoUpdate = -1;
						this.iconNo = -1;
						this.currentMenu = mainmenu.menus.CONTROLS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
				}
				this.checkForHoverAndPlaySound();
				GUI.FocusControl("focusedButton");
				this.totlalRows = num4;
				break;
			}
			case 6:
			{
				this.DrawLabel(Language.Get("M_Motion_Controler_Setup", 60));
				GUILayout.Space(100f);
				this.DrawLabel(Language.Get("M_SETUP_INST3", 30), this.plaingTextStyle, GUILayout.Width(290f));
				GUILayout.Space(110f);
				int num4 = 1;
				if (this.selectedRow == num4)
				{
					GUI.SetNextControlName("focusedButton");
				}
				if ((this.DrawButton(Language.Get("M_OK", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
				{
					this.calebrationStepUpdate = 0;
					ShooterGameCamera.moveCalibrated = true;
					if (this.returnToMainAfterCalibtation)
					{
						this.currentMenu = mainmenu.menus.MAIN;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
					}
					else
					{
						this.currentMenu = mainmenu.menus.CONTROLS;
						this.selectedRow = 1;
						base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
					}
				}
				this.checkForHoverAndPlaySound();
				GUI.FocusControl("focusedButton");
				this.totlalRows = num4;
				break;
			}
			}
			break;
		}
		case mainmenu.menus.SURVIVALMENU:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Survival_Mode", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Play", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.newGame = true;
				this.currentMenu = mainmenu.menus.SURVIVALSELECTION;
				this.selectedRow = 1;
				this.tryingToSignIn = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Leader_Boards", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (Social.localUser.authenticated)
				{
					Social.ShowLeaderboardUI();
				}
				else
				{
					Social.localUser.Authenticate(delegate(bool success)
					{
						if (success)
						{
							Social.ShowLeaderboardUI();
						}
					});
				}
			}
			this.checkForHoverAndPlaySound();
			num4++;
			GUILayout.Space(10f);
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				this.tryingToSignIn = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.SURVIVALSELECTION:
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Survival_Mode", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(188f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || this.leftArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (this.currentSurvivalLevel > 0)
				{
					int num5 = this.currentSurvivalLevel;
					for (;;)
					{
						num5--;
						bool availableOnMobile = this.survivalLevels[num5].availableOnMobile;
						if (availableOnMobile)
						{
							break;
						}
						if (num5 <= 0)
						{
							goto IL_70C7;
						}
					}
					this.currentSurvivalLevel = num5;
				}
				IL_70C7:
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get(this.survivalLevels[this.currentSurvivalLevel].levelTitleKeyword, 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
			GUILayout.Space(50f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || this.rightArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (this.currentSurvivalLevel < this.survivalLevels.Length - 1)
				{
					int num6 = this.currentSurvivalLevel;
					for (;;)
					{
						num6++;
						bool availableOnMobile2 = this.survivalLevels[num6].availableOnMobile;
						if (availableOnMobile2)
						{
							break;
						}
						if (num6 >= this.survivalLevels.Length - 1)
						{
							goto IL_71B9;
						}
					}
					this.currentSurvivalLevel = num6;
				}
				IL_71B9:
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(110f);
			GUILayout.Label(this.survivalLevels[this.currentSurvivalLevel].levelPhoto, this.emptyStyle, new GUILayoutOption[]
			{
				GUILayout.Height(150f)
			});
			GUILayout.EndHorizontal();
			if (this.action && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.SURVIVALCHARSELECTION;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
				{
					this.currentMenu = mainmenu.menus.SURVIVALMENU;
				}
				else
				{
					this.currentMenu = mainmenu.menus.MAIN;
				}
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.totlalRows = 0;
			break;
		case mainmenu.menus.SURVIVALCHARSELECTION:
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Survival_Mode", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(188f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || this.leftArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (this.currentSurvivalCharacter > 0)
				{
					this.currentSurvivalCharacter--;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(25f);
			this.DrawLabel(Language.Get("M_CHOOSECHARACTER", 60), this.BoldOutlineTextStyle, GUILayout.Width(250f));
			GUILayout.Space(25f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || this.rightArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (this.currentSurvivalCharacter < this.survivalCharacters.Length - 1)
				{
					this.currentSurvivalCharacter++;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(3f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.4f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.2f)
			{
				GUI.DrawTexture(new Rect(280f, 100f, 280f, 220f), this.survivalCharacters[this.currentSurvivalCharacter].charPhoto, ScaleMode.StretchToFill);
			}
			else if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.6f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.4f)
			{
				GUI.DrawTexture(new Rect(280f, 100f, 270f, 220f), this.survivalCharacters[this.currentSurvivalCharacter].charPhoto, ScaleMode.StretchToFill);
			}
			else
			{
				GUI.DrawTexture(new Rect(310f, 100f, 220f, 220f), this.survivalCharacters[this.currentSurvivalCharacter].charPhoto, ScaleMode.StretchToFill);
			}
			GUILayout.EndHorizontal();
			if (this.action && Event.current.type != EventType.Repaint)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				WaveSpawner.playerCharacter = this.survivalCharacters[this.currentSurvivalCharacter].charName;
				if ((AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield) && !Social.localUser.authenticated)
				{
					this.currentMenu = mainmenu.menus.PS3SIGNIN;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.GoToLevel(this.survivalLevels[this.currentSurvivalLevel].levelName, true);
				}
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.SURVIVALSELECTION;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.totlalRows = 0;
			break;
		case mainmenu.menus.PS3SIGNIN:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_GameServices_Sign_in", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(45f);
			this.plaingTextStyle.contentOffset = new Vector2(-70f, 0f);
			this.DrawLabel(Language.Get("M_GameServices_Sign_in_Confirm", 40), this.plaingTextStyle, GUILayout.Width(550f));
			this.plaingTextStyle.contentOffset = new Vector2(0f, 0f);
			GUILayout.Space(50f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Yes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (Social.localUser.authenticated)
				{
					this.GoToLevel(this.survivalLevels[this.currentSurvivalLevel].levelName, true);
				}
				else
				{
					Social.localUser.Authenticate(delegate(bool success)
					{
						if (success)
						{
							this.GoToLevel(this.survivalLevels[this.currentSurvivalLevel].levelName, true);
						}
					});
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_No", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.GoToLevel(this.survivalLevels[this.currentSurvivalLevel].levelName, true);
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.ERRORMESSAGE:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_LOCKED", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get(this.errorMessage, 30), this.plaingTextStyle, null);
			GUILayout.Space(50f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				if (this.errorMessage == "M_SURVIVAL_LOCKED")
				{
					this.currentMenu = mainmenu.menus.MAIN;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.currentMenu = mainmenu.menus.EXTRAS;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.EXTRAS:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Extras", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_CONCEPTART", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.demoMode || SaveHandler.gameFinished >= 1)
				{
					//GA.API.Design.NewEvent("OpenedExtras:CONCEPTART");
					mainmenu.returnToMenu = mainmenu.menus.EXTRAS;
					this.GoToLevel("ConceptArt", false);
				}
				else
				{
					this.errorMessage = "M_RENDERS_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Renders", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.demoMode || SaveHandler.gameFinished >= 1)
				{
					//GA.API.Design.NewEvent("OpenedExtras:Renders");
					mainmenu.returnToMenu = mainmenu.menus.EXTRAS;
					this.GoToLevel("Renders", false);
				}
				else
				{
					this.errorMessage = "M_RENDERS_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Storyboards", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.demoMode || SaveHandler.gameFinished >= 2)
				{
					//GA.API.Design.NewEvent("OpenedExtras:Storyboards");
					mainmenu.returnToMenu = mainmenu.menus.EXTRAS;
					this.GoToLevel("StoryBoards", false);
				}
				else
				{
					this.errorMessage = "M_STORYBOARD_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Animatics", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.demoMode || SaveHandler.gameFinished >= 3)
				{
					//GA.API.Design.NewEvent("OpenedExtras:Animatics");
					this.currentMenu = mainmenu.menus.ANIMATICSSELECTION;
					mainmenu.currentAnimatic = 0;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.errorMessage = "M_ANIMATICS_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Cutscenes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.demoMode || SaveHandler.gameFinished >= 2)
				{
					//GA.API.Design.NewEvent("OpenedExtras:Cutscenes");
					this.currentMenu = mainmenu.menus.CUTSCENESSELECTION;
					mainmenu.currentCutscene = 0;
					this.selectedRow = 1;
					base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				}
				else
				{
					this.errorMessage = "M_CUTSCENES_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Characters", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.demoMode || SaveHandler.gameFinished >= 1 || SaveHandler.levelReached > 1)
				{
					//GA.API.Design.NewEvent("OpenedExtras:Characters");
					mainmenu.returnToMenu = mainmenu.menus.EXTRAS;
					this.GoToLevel("Gallery", false);
				}
				else
				{
					this.errorMessage = "M_CHAR_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("Treasures", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				if (SaveHandler.treasures != 0)
				{
					//GA.API.Design.NewEvent("OpenedExtras:Treasures");
					mainmenu.returnToMenu = mainmenu.menus.EXTRAS;
					this.GoToLevel("Treasures", false);
				}
				else
				{
					this.errorMessage = "M_TREASURES_LOCKED";
					this.currentMenu = mainmenu.menus.ERRORMESSAGE;
					this.selectedRow = 1;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_JTM", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				//GA.API.Design.NewEvent("OpenedExtras:JTM");
				mainmenu.returnToMenu = mainmenu.menus.EXTRAS;
				this.GoToLevel("jtm", false);
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.totlalRows = num4;
			GUI.FocusControl("focusedButton");
			break;
		}
		case mainmenu.menus.CUTSCENESSELECTION:
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Cutscenes", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(188f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || this.leftArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.currentCutscene > 0)
				{
					mainmenu.currentCutscene--;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get("M_Cutscenes", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
			GUILayout.Space(50f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || this.rightArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.currentCutscene < this.cutsceneSelectPics.Length - 1)
				{
					mainmenu.currentCutscene++;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(110f);
			GUILayout.Label(this.cutsceneSelectPics[mainmenu.currentCutscene], this.emptyStyle, new GUILayoutOption[]
			{
				GUILayout.Height(150f)
			});
			GUILayout.EndHorizontal();
			if (this.action && Event.current.type != EventType.Repaint)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				mainmenu.replayCutscene = true;
				mainmenu.returnToMenu = mainmenu.menus.CUTSCENESSELECTION;
				this.GoToLevel(this.cutsceneScenes[mainmenu.currentCutscene], true);
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.EXTRAS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.totlalRows = 0;
			break;
		case mainmenu.menus.ANIMATICSSELECTION:
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Animatics", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(200f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if ((this.DrawButton(string.Empty, this.leftArrowStyle) || this.leftArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.currentAnimatic > 0)
				{
					mainmenu.currentAnimatic--;
				}
				this.leftArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get("M_Animatics", 60), this.BoldOutlineTextStyle, GUILayout.Width(200f));
			GUILayout.Space(30f);
			if ((this.DrawButton(string.Empty, this.rightArrowStyle) || this.rightArrowAction) && Event.current.type != EventType.Repaint)
			{
				if (mainmenu.currentAnimatic < this.animaticsSelectPics.Length - 1)
				{
					mainmenu.currentAnimatic++;
				}
				this.rightArrowAction = false;
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(90f);
			GUILayout.Label(this.animaticsSelectPics[mainmenu.currentAnimatic], this.emptyStyle, new GUILayoutOption[]
			{
				GUILayout.Height(150f)
			});
			GUILayout.EndHorizontal();
			if (this.action && Event.current.type != EventType.Repaint)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
				mainmenu.replayCutscene = true;
				mainmenu.returnToMenu = mainmenu.menus.ANIMATICSSELECTION;
				this.GoToLevel(this.animaticsScenes[mainmenu.currentAnimatic], true);
			}
			if (this.backAction && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.EXTRAS;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.totlalRows = 0;
			break;
		case mainmenu.menus.AIMINGMODE:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Aim_Assest", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Aim_OFF", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint && !this.faded)
			{
				ShooterGameCamera.aimAssestType = ShooterGameCamera.AimAssestTypes.OFF;
				SaveHandler.SaveSettings();
				this.LoadLevel();
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Aim_SEMI", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint && !this.faded)
			{
				ShooterGameCamera.aimAssestType = ShooterGameCamera.AimAssestTypes.SEMI;
				SaveHandler.SaveSettings();
				this.LoadLevel();
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Aim_AUTO", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint && !this.faded)
			{
				ShooterGameCamera.aimAssestType = ShooterGameCamera.AimAssestTypes.HARD;
				SaveHandler.SaveSettings();
				this.LoadLevel();
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			GUILayout.Space(10f);
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if (((this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			this.totlalRows = num4;
			GUI.FocusControl("focusedButton");
			break;
		}
		case mainmenu.menus.CONFIRMEXIT:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Exit", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(50f);
			this.DrawLabel(Language.Get("M_Exit_Confirm", 60), this.plaingTextStyle, GUILayout.Width(390f));
			GUILayout.Space(40f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Yes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
				Application.Quit();
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_No", 60)) || this.backAction || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.TRYORBUY:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(50f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Try", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.MAIN;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Buy", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				mainmenu.showCart = true;
				this.selectedCol = 1;
				//GA.API.Design.NewEvent("CartScreenShown:fromBuyOrTryMenue");
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.EXITTOMAINCONFIRM:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get("M_Back_To_Mainmenu", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(30f);
			this.DrawLabel(Language.Get("M_Checkpoint_Confirm", 60), this.BoldOutlineTextStyle, GUILayout.Width(390f));
			GUILayout.Space(30f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Yes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				mainmenu.replayLevel = false;
				mainmenu.pause = false;
				Time.timeScale = 1f;
				AudioListener.pause = false;
				mainmenu.Instance = null;
				Application.LoadLevel("LoadingMainMenu");
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_No", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.PAUSE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		case mainmenu.menus.LASTCHECKPOINTCONFIRM:
		{
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.DrawLabel(Language.Get(this.survivalLevel ? "M_Restart_Survival" : "M_Restart", 60));
			GUILayout.FlexibleSpace();
			GUILayout.Space(220f);
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(225f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(30f);
			this.DrawLabel(Language.Get("M_Checkpoint_Confirm", 60), this.BoldOutlineTextStyle, GUILayout.Width(390f));
			GUILayout.Space(30f);
			int num4 = 1;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_Yes", 60)) || (this.selectedRow == num4 && this.action)) && Event.current.type != EventType.Repaint)
			{
				mainmenu.pause = false;
				Time.timeScale = 1f;
				AudioListener.pause = false;
				mainmenu.Instance = null;
				Application.LoadLevel("PreLoading" + Application.loadedLevelName);
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			num4++;
			if (this.selectedRow == num4)
			{
				GUI.SetNextControlName("focusedButton");
			}
			if ((this.DrawButton(Language.Get("M_No", 60)) || (this.selectedRow == num4 && this.action) || this.backAction) && Event.current.type != EventType.Repaint)
			{
				this.currentMenu = mainmenu.menus.PAUSE;
				this.selectedRow = 1;
				base.GetComponent<AudioSource>().PlayOneShot(this.backClickSound, SpeechManager.sfxVolume);
			}
			this.checkForHoverAndPlaySound();
			GUI.FocusControl("focusedButton");
			this.totlalRows = num4;
			break;
		}
		}
		this.action = false;
		this.backAction = false;
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		this.LabelStyle.contentOffset = new Vector2(0f, -6f);
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0007F71C File Offset: 0x0007D91C
	private void DrawShadowedText(string p, GUIStyle entryNormalStyle, GUILayoutOption options = null)
	{
		entryNormalStyle.normal.textColor = Color.black;
		if (options == null)
		{
			GUILayout.Label(p, entryNormalStyle, new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.Label(p, entryNormalStyle, new GUILayoutOption[]
			{
				options
			});
		}
		Rect position = GUILayoutUtility.GetLastRect();
		position.x += 2f;
		GUI.Label(position, p, entryNormalStyle);
		position.y += 2f;
		GUI.Label(position, p, entryNormalStyle);
		position.x -= 2f;
		GUI.Label(position, p, entryNormalStyle);
		position.x += 1f;
		position.y -= 1f;
		entryNormalStyle.normal.textColor = Color.white;
		GUI.Label(position, p, entryNormalStyle);
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0007F7F8 File Offset: 0x0007D9F8
	private void DrawShadowedText(Rect rect, string p, GUIStyle entryNormalStyle)
	{
		entryNormalStyle.normal.textColor = Color.black;
		rect.x -= 1f;
		rect.y -= 1f;
		GUI.Label(rect, p, entryNormalStyle);
		rect.y += 2f;
		GUI.Label(rect, p, entryNormalStyle);
		rect.x += 2f;
		GUI.Label(rect, p, entryNormalStyle);
		rect.y -= 2f;
		GUI.Label(rect, p, entryNormalStyle);
		entryNormalStyle.normal.textColor = Color.white;
		rect.y += 1f;
		rect.y -= 1f;
		GUI.Label(rect, p, entryNormalStyle);
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0007F8D4 File Offset: 0x0007DAD4
	private void GoToLevel(string levelName, bool replay)
	{
		if (replay)
		{
			mainmenu.replayLevel = true;
			SaveHandler.ResetReplayLevelValues();
		}
		else
		{
			mainmenu.replayLevel = false;
		}
		this.levelToLoad = levelName;
		Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
		this.faded = true;
		this.startTime = Time.time;
		base.StartCoroutine(this.FadeAudio(2f, mainmenu.Fade.Out));
		base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0007F958 File Offset: 0x0007DB58
	public void GoToLevel(string levelName)
	{
		this.GoToLevel(levelName, mainmenu.replayLevel);
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0007F968 File Offset: 0x0007DB68
	private void LoadLevel()
	{
		if (this.newGame)
		{
			SaveHandler.currentDifficultyLevel = DifficultyManager.difficulty;
			SaveHandler.ResetNewGame();
			SaveHandler.levelReached = 1;
			SaveHandler.checkpointReached = 0;
			this.GoToLevel("Titles", false);
		}
		else
		{
			switch (SaveHandler.levelReached)
			{
			case 1:
				this.GoToLevel("PreLoadingPrologue", false);
				break;
			case 2:
				if (SaveHandler.checkpointReached == 0)
				{
					this.GoToLevel("Egypt-Cutscene1", false);
				}
				else
				{
					this.GoToLevel("PreLoadingpart1", false);
				}
				break;
			case 3:
				if (SaveHandler.checkpointReached == 0)
				{
					this.GoToLevel("LoadingEgyptCutscene2", false);
				}
				else
				{
					this.GoToLevel("PreLoadingpart2", false);
				}
				break;
			case 4:
				this.GoToLevel("LoadingEgyptCutscene3", false);
				break;
			case 5:
				if (SaveHandler.checkpointReached == 0)
				{
					this.GoToLevel("LoadingEgyptCutscene4", false);
				}
				else
				{
					this.GoToLevel("PreLoadingQuadChase", false);
				}
				break;
			case 6:
				this.GoToLevel("Morocco-Cutscene1-Video", false);
				break;
			case 7:
				this.GoToLevel("PreLoadingTangier2", false);
				break;
			case 8:
				this.GoToLevel("LoadingMoroccoCutscene2", false);
				break;
			case 9:
				if (SaveHandler.checkpointReached == 0)
				{
					this.GoToLevel("LoadingMoroccoCutscene3", false);
				}
				else
				{
					this.GoToLevel("PreLoadingChase1", false);
				}
				break;
			case 10:
				this.GoToLevel("LoadingMoroccoCutscene4", false);
				break;
			case 11:
			case 12:
				this.GoToLevel("PreLoadingCar-Intro", false);
				break;
			}
		}
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0007FB1C File Offset: 0x0007DD1C
	private void checkForHoverAndPlaySound()
	{
		if (this.lastRect != GUILayoutUtility.GetLastRect() && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
			this.lastRect = GUILayoutUtility.GetLastRect();
		}
		else if (this.lastRect == GUILayoutUtility.GetLastRect() && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
		{
			this.lastRect = default(Rect);
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0007FBC0 File Offset: 0x0007DDC0
	private IEnumerator FadeAudio(float timer, mainmenu.Fade fadeType)
	{
		float start = (fadeType != mainmenu.Fade.In) ? 1f : 0f;
		float end = (fadeType != mainmenu.Fade.In) ? 0f : 1f;
		if (this.music != null && this.music.GetComponent<AudioSource>() != null)
		{
			start = ((fadeType != mainmenu.Fade.In) ? this.music.GetComponent<AudioSource>().volume : 0f);
			end = ((fadeType != mainmenu.Fade.In) ? 0f : this.music.GetComponent<AudioSource>().volume);
		}
		float i = 0f;
		float step = 1f / timer;
		while (i <= 1f)
		{
			i += step * Time.deltaTime;
			if (this.music != null)
			{
				this.music.GetComponent<AudioSource>().volume = Mathf.Lerp(start, end, i);
			}
			yield return 0;
		}
		yield break;
	}

	// Token: 0x040010EA RID: 4330
	public mainmenu.menus currentMenu;

	// Token: 0x040010EB RID: 4331
	public mainmenu.menus currentMenuAfterUpdate;

	// Token: 0x040010EC RID: 4332
	//public GUITexture backgroundTexture;

	// Token: 0x040010ED RID: 4333
	public GameObject videoObject;

	// Token: 0x040010EE RID: 4334
	public Texture ArabicPC;

	// Token: 0x040010EF RID: 4335
	public Texture EnglishPC;

	// Token: 0x040010F0 RID: 4336
	public Texture ArabicOther;

	// Token: 0x040010F1 RID: 4337
	public Texture EnglishOther;

	// Token: 0x040010F2 RID: 4338
	public Texture ArabicMobile;

	// Token: 0x040010F3 RID: 4339
	public Texture EnglishMobile;

	// Token: 0x040010F4 RID: 4340
	public Texture PauseBackground;

	// Token: 0x040010F5 RID: 4341
	public Texture StartArabicPC;

	// Token: 0x040010F6 RID: 4342
	public Texture StartEnglishPC;

	// Token: 0x040010F7 RID: 4343
	public Texture StartArabicOther;

	// Token: 0x040010F8 RID: 4344
	public Texture StartEnglishOther;

	// Token: 0x040010F9 RID: 4345
	public Texture[] chapterSelectPics;

	// Token: 0x040010FA RID: 4346
	public Texture[] cutsceneSelectPics;

	// Token: 0x040010FB RID: 4347
	public string[] cutsceneScenes;

	// Token: 0x040010FC RID: 4348
	public Texture[] animaticsSelectPics;

	// Token: 0x040010FD RID: 4349
	public string[] animaticsScenes;

	// Token: 0x040010FE RID: 4350
	public Texture selectPC;

	// Token: 0x040010FF RID: 4351
	public Texture backPC;

	// Token: 0x04001100 RID: 4352
	public Texture selectPS3;

	// Token: 0x04001101 RID: 4353
	public Texture backPS3;

	// Token: 0x04001102 RID: 4354
	public Texture selectXbox;

	// Token: 0x04001103 RID: 4355
	public Texture backXbox;

	// Token: 0x04001104 RID: 4356
	private bool faded;

	// Token: 0x04001105 RID: 4357
	private float startTime;

	// Token: 0x04001106 RID: 4358
	public float fadeTime = 4f;

	// Token: 0x04001107 RID: 4359
	public AudioSource music;

	// Token: 0x04001108 RID: 4360
	public GUISkin guiSkin;

	// Token: 0x04001109 RID: 4361
	public bool isPauseMenu;

	// Token: 0x0400110A RID: 4362
	private bool pauseButton;

	// Token: 0x0400110B RID: 4363
	public int selectedRow = 1;

	// Token: 0x0400110C RID: 4364
	public int selectedCol = 1;

	// Token: 0x0400110D RID: 4365
	private bool acceptMovement = true;

	// Token: 0x0400110E RID: 4366
	public int totlalRows = 6;

	// Token: 0x0400110F RID: 4367
	public int totlalCols = 3;

	// Token: 0x04001110 RID: 4368
	private bool action;

	// Token: 0x04001111 RID: 4369
	private bool backAction;

	// Token: 0x04001112 RID: 4370
	private GUIStyle instructionStyle;

	// Token: 0x04001113 RID: 4371
	private GUIStyle largeInstructionStyle;

	// Token: 0x04001114 RID: 4372
	private string[,] languages;

	// Token: 0x04001115 RID: 4373
	private int currentLanguage;

	// Token: 0x04001116 RID: 4374
	private bool rightArrowAction;

	// Token: 0x04001117 RID: 4375
	private bool leftArrowAction;

	// Token: 0x04001118 RID: 4376
	private int currentResolution;

	// Token: 0x04001119 RID: 4377
	public static int currentCameraSensitivity = 5;

	// Token: 0x0400111A RID: 4378
	public static bool pause;

	// Token: 0x0400111B RID: 4379
	private string levelToLoad;

	// Token: 0x0400111C RID: 4380
	private static bool started;

	// Token: 0x0400111D RID: 4381
	private float startedTime;

	// Token: 0x0400111E RID: 4382
	public AudioClip clickSound;

	// Token: 0x0400111F RID: 4383
	public AudioClip hoverSound;

	// Token: 0x04001120 RID: 4384
	public AudioClip backClickSound;

	// Token: 0x04001121 RID: 4385
	public AudioClip dialSound;

	// Token: 0x04001122 RID: 4386
	private Rect lastRect;

	// Token: 0x04001123 RID: 4387
	public static bool replayLevel = true;

	// Token: 0x04001124 RID: 4388
	private bool newGame;

	// Token: 0x04001125 RID: 4389
	public static bool demoMode;

	// Token: 0x04001126 RID: 4390
	public static mainmenu Instance;

	// Token: 0x04001127 RID: 4391
	private bool savedAction;

	// Token: 0x04001128 RID: 4392
	private bool savedBackAction;

	// Token: 0x04001129 RID: 4393
	private bool showBlinkingText;

	// Token: 0x0400112A RID: 4394
	private float showBlinkingTextTimer;

	// Token: 0x0400112B RID: 4395
	public int debugStartCheckpointNo;

	// Token: 0x0400112C RID: 4396
	public string nextLevelName;

	// Token: 0x0400112D RID: 4397
	private int currentChapter;

	// Token: 0x0400112E RID: 4398
	public static int currentCutscene;

	// Token: 0x0400112F RID: 4399
	public static int currentAnimatic;

	// Token: 0x04001130 RID: 4400
	private int currentSurvivalLevel;

	// Token: 0x04001131 RID: 4401
	private int currentSurvivalCharacter;

	// Token: 0x04001132 RID: 4402
	private GUIStyle emptyStyle;

	// Token: 0x04001133 RID: 4403
	private GUIStyle rightArrowStyle;

	// Token: 0x04001134 RID: 4404
	private GUIStyle leftArrowStyle;

	// Token: 0x04001135 RID: 4405
	public GUIStyle buttonStyle;

	// Token: 0x04001136 RID: 4406
	private GUIStyle LabelStyle;

	// Token: 0x04001137 RID: 4407
	private GUIStyle plaingTextStyle;

	// Token: 0x04001138 RID: 4408
	private GUIStyle plaingTextLargeStyle;

	// Token: 0x04001139 RID: 4409
	private GUIStyle plaingTextSelectedStyle;

	// Token: 0x0400113A RID: 4410
	private GUIStyle BoldOutlineTextStyle;

	// Token: 0x0400113B RID: 4411
	private int calebrationStep;

	// Token: 0x0400113C RID: 4412
	private int calebrationStepUpdate;

	// Token: 0x0400113D RID: 4413
	private bool calibrating;

	// Token: 0x0400113E RID: 4414
	public Texture moveCurser;

	// Token: 0x0400113F RID: 4415
	private int iconNo;

	// Token: 0x04001140 RID: 4416
	private int iconNoUpdate;

	// Token: 0x04001141 RID: 4417
	private Vector3 icon1Position;

	// Token: 0x04001142 RID: 4418
	private Vector3 icon2Position;

	// Token: 0x04001143 RID: 4419
	private bool showMoveIcon;

	// Token: 0x04001144 RID: 4420
	private bool returnToMainAfterCalibtation;

	// Token: 0x04001145 RID: 4421
	public mainmenu.SurvivalLevel[] survivalLevels;

	// Token: 0x04001146 RID: 4422
	public mainmenu.SurvivalCharacter[] survivalCharacters;

	// Token: 0x04001147 RID: 4423
	private bool tryingToSignIn;

	// Token: 0x04001148 RID: 4424
	private bool tryingToSignInToPlay;

	// Token: 0x04001149 RID: 4425
	public int ControlsInstruction;

	// Token: 0x0400114A RID: 4426
	public Texture PS3ExplorationInst;

	// Token: 0x0400114B RID: 4427
	public Texture PS3MeleeInst;

	// Token: 0x0400114C RID: 4428
	public Texture PS3DrivingInst;

	// Token: 0x0400114D RID: 4429
	public Texture PS3MeleeInstIngame;

	// Token: 0x0400114E RID: 4430
	public Texture PS3MoveExplorationInst;

	// Token: 0x0400114F RID: 4431
	public Texture PS3MoveMeleeInst;

	// Token: 0x04001150 RID: 4432
	public Texture PS3MoveDrivingInst;

	// Token: 0x04001151 RID: 4433
	public Texture PS3MoveMeleeInstIngame;

	// Token: 0x04001152 RID: 4434
	public Texture PC3MeleeInstIngame;

	// Token: 0x04001153 RID: 4435
	public Texture XBoxMeleeInstIngame;

	// Token: 0x04001154 RID: 4436
	public Texture gameStickMeleeInstIngame;

	// Token: 0x04001155 RID: 4437
	public Texture GenericMeleeInstIngame;

	// Token: 0x04001156 RID: 4438
	public Texture iOSBasicMeleeInstIngame;

	// Token: 0x04001157 RID: 4439
	public Texture iOSExtendedMeleeInstIngame;

	// Token: 0x04001158 RID: 4440
	public static bool showMeleeInstructions;

	// Token: 0x04001159 RID: 4441
	private string errorMessage;

	// Token: 0x0400115A RID: 4442
	public static bool replayCutscene;

	// Token: 0x0400115B RID: 4443
	public static bool disableHUD;

	// Token: 0x0400115C RID: 4444
	public static mainmenu.menus returnToMenu;

	// Token: 0x0400115D RID: 4445
	public bool survivalLevel;

	// Token: 0x0400115E RID: 4446
	public bool joystickOnPC;

	// Token: 0x0400115F RID: 4447
	public Texture facebookButton;

	// Token: 0x04001160 RID: 4448
	public Texture twitterButton;

	// Token: 0x04001161 RID: 4449
	public Texture websiteButton;

	// Token: 0x04001162 RID: 4450
	public Texture cartButton;

	// Token: 0x04001163 RID: 4451
	private bool facebookAction;

	// Token: 0x04001164 RID: 4452
	private bool twitterAction;

	// Token: 0x04001165 RID: 4453
	private bool websiteAction;

	// Token: 0x04001166 RID: 4454
	private bool cartButtonAction;

	// Token: 0x04001167 RID: 4455
	private bool cartAction;

	// Token: 0x04001168 RID: 4456
	//private List<IAPProduct> _products;

	// Token: 0x04001169 RID: 4457
	private float lastPurchaseTime;

	// Token: 0x0400116A RID: 4458
	public Texture cartBG;

	// Token: 0x0400116B RID: 4459
	public Texture cartOverlay1;

	// Token: 0x0400116C RID: 4460
	public Texture cartOverlay2;

	// Token: 0x0400116D RID: 4461
	public Texture cartLogoAr;

	// Token: 0x0400116E RID: 4462
	public Texture cartLogoEn;

	// Token: 0x0400116F RID: 4463
	public static bool showCart;

	// Token: 0x04001170 RID: 4464
	private bool inAppPurchasesInactive;

	// Token: 0x04001171 RID: 4465
	private bool internetDisconnected;

	// Token: 0x04001172 RID: 4466
	private bool overlay1;

	// Token: 0x04001173 RID: 4467
	private float overlayTimer;

	// Token: 0x04001174 RID: 4468
	private int currentFeatures;

	// Token: 0x04001175 RID: 4469
	private GUIStyle style;

	// Token: 0x04001176 RID: 4470
	private GUIStyle style2;

	// Token: 0x04001177 RID: 4471
	public static bool hintPause;

	// Token: 0x04001178 RID: 4472
	public static bool trailerShown;

	// Token: 0x04001179 RID: 4473
	public Texture2D highlitedButtonTexture;

	// Token: 0x0200022E RID: 558
	public enum menus
	{
		// Token: 0x0400117F RID: 4479
		MAIN,
		// Token: 0x04001180 RID: 4480
		DIFFICULTY,
		// Token: 0x04001181 RID: 4481
		CHAPTERSELECTION,
		// Token: 0x04001182 RID: 4482
		OPTIONS,
		// Token: 0x04001183 RID: 4483
		CREDITS,
		// Token: 0x04001184 RID: 4484
		GRAPHICS,
		// Token: 0x04001185 RID: 4485
		AUDIO,
		// Token: 0x04001186 RID: 4486
		CONTROLS,
		// Token: 0x04001187 RID: 4487
		LANGUAGE,
		// Token: 0x04001188 RID: 4488
		PAUSE,
		// Token: 0x04001189 RID: 4489
		NEWGAMECONFIRMATION,
		// Token: 0x0400118A RID: 4490
		DEBUG,
		// Token: 0x0400118B RID: 4491
		REQUESTCALIBRATION,
		// Token: 0x0400118C RID: 4492
		CAMERADISCONNECTED,
		// Token: 0x0400118D RID: 4493
		MOVECALIBRATION,
		// Token: 0x0400118E RID: 4494
		SURVIVALMENU,
		// Token: 0x0400118F RID: 4495
		SURVIVALSELECTION,
		// Token: 0x04001190 RID: 4496
		SURVIVALCHARSELECTION,
		// Token: 0x04001191 RID: 4497
		PS3SIGNIN,
		// Token: 0x04001192 RID: 4498
		ERRORMESSAGE,
		// Token: 0x04001193 RID: 4499
		EXTRAS,
		// Token: 0x04001194 RID: 4500
		CUTSCENESSELECTION,
		// Token: 0x04001195 RID: 4501
		ANIMATICSSELECTION,
		// Token: 0x04001196 RID: 4502
		AIMINGMODE,
		// Token: 0x04001197 RID: 4503
		CONFIRMEXIT,
		// Token: 0x04001198 RID: 4504
		TRYORBUY,
		// Token: 0x04001199 RID: 4505
		EXITTOMAINCONFIRM,
		// Token: 0x0400119A RID: 4506
		LASTCHECKPOINTCONFIRM
	}

	// Token: 0x0200022F RID: 559
	[Serializable]
	public class SurvivalLevel
	{
		// Token: 0x0400119B RID: 4507
		public string levelName;

		// Token: 0x0400119C RID: 4508
		public string levelTitleKeyword;

		// Token: 0x0400119D RID: 4509
		public Texture levelPhoto;

		// Token: 0x0400119E RID: 4510
		public bool locked;

		// Token: 0x0400119F RID: 4511
		public int pointsToUnlock;

		// Token: 0x040011A0 RID: 4512
		public bool availableOnPC;

		// Token: 0x040011A1 RID: 4513
		public bool availableOnPS3;

		// Token: 0x040011A2 RID: 4514
		public bool availableOnXBOX;

		// Token: 0x040011A3 RID: 4515
		public bool availableOnMobile;
	}

	// Token: 0x02000230 RID: 560
	[Serializable]
	public class SurvivalCharacter
	{
		// Token: 0x040011A4 RID: 4516
		public string charName;

		// Token: 0x040011A5 RID: 4517
		public Texture charPhoto;

		// Token: 0x040011A6 RID: 4518
		public bool locked;

		// Token: 0x040011A7 RID: 4519
		public int pointsToUnlock;
	}

	// Token: 0x02000231 RID: 561
	public enum Fade
	{
		// Token: 0x040011A9 RID: 4521
		In,
		// Token: 0x040011AA RID: 4522
		Out
	}
}
