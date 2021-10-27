using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class LanguageSelection : MonoBehaviour
{
	// Token: 0x060008FC RID: 2300 RVA: 0x00049D5C File Offset: 0x00047F5C
	public void Awake()
	{
		Screen.sleepTimeout = -1;
		if (SystemInfo.deviceModel.ToLower().Contains("shield"))
		{
			AndroidPlatform.platform = AndroidPlatform.AndroidPlatforms.GameShield;
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
		{
			Screen.SetResolution(1138, 640, false);
		}
		//if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		//{
		//	base.InvokeRepeating("CheckJoystick", 0f, 1f);
		//}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00049DE4 File Offset: 0x00047FE4
	public void CheckJoystick()
	{
	//	AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00049DF8 File Offset: 0x00047FF8
	private void Start()
	{
		this.EnglishButtonStyle = this.languageButtons.GetStyle("English");
		this.ArabicButtonStyle = this.languageButtons.GetStyle("Arabic");
		this.FrancaisButtonStyle = this.languageButtons.GetStyle("Francais");
		this.DeutschButtonStyle = this.languageButtons.GetStyle("Deutsch");
		this.EspanolButtonStyle = this.languageButtons.GetStyle("Espanol");
		this.ItalianoButtonStyle = this.languageButtons.GetStyle("Italiano");
		this.NorwegianButtonStyle = this.languageButtons.GetStyle("Norwegian");
		this.DanishButtonStyle = this.languageButtons.GetStyle("Danish");
		this.SwedishButtonStyle = this.languageButtons.GetStyle("Swedish");
		this.GreekButtonStyle = this.languageButtons.GetStyle("Greek");
		this.DutchButtonStyle = this.languageButtons.GetStyle("Dutch");
		this.PortogueseButtonStyle = this.languageButtons.GetStyle("Portoguese");
		this.RussianButtonStyle = this.languageButtons.GetStyle("Russian");
		this.PolckiButtonStyle = this.languageButtons.GetStyle("Polcki");
		this.JapaneseButtonStyle = this.languageButtons.GetStyle("Japanese");
		this.ChineseButtonStyle = this.languageButtons.GetStyle("Chinese");
		this.TurkisButtonStyle = this.languageButtons.GetStyle("Turkish");
		this.MalayButtonStyle = this.languageButtons.GetStyle("Malay");
		this.HindiButtonStyle = this.languageButtons.GetStyle("Hindi");
		this.KorianButtonStyle = this.languageButtons.GetStyle("Korean");
		this.FarsiButtonStyle = this.languageButtons.GetStyle("Farsi");
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00049FD4 File Offset: 0x000481D4
	private void Update()
	{
		if (this.acceptMovement && !this.faded)
		{
			if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("DpadH") > 0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton5)))
			{
				if (!this.useVertHor)
				{
					this.useVertHor = true;
				}
				else if (this.selectedCol < this.totlalCols)
				{
					this.selectedCol++;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				this.acceptMovement = false;
			}
			else if (Input.GetAxisRaw("Horizontal") < -0.3f || Input.GetAxisRaw("DpadH") < -0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton7)))
			{
				if (!this.useVertHor)
				{
					this.useVertHor = true;
				}
				else if (this.selectedCol > 1)
				{
					this.selectedCol--;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				this.acceptMovement = false;
			}
			if (Input.GetAxisRaw("Vertical") > 0.3f || Input.GetAxisRaw("DpadV") > 0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton4)))
			{
				if (!this.useVertHor)
				{
					this.useVertHor = true;
				}
				else if (this.selectedRow > 1)
				{
					this.selectedRow--;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				this.acceptMovement = false;
			}
			else if (Input.GetAxisRaw("Vertical") < -0.3f || Input.GetAxisRaw("DpadV") < -0.3f || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetKey(KeyCode.JoystickButton6)))
			{
				if (!this.useVertHor)
				{
					this.useVertHor = true;
				}
				else if (this.selectedRow < this.totlalRows)
				{
					this.selectedRow++;
					base.GetComponent<AudioSource>().PlayOneShot(this.hoverSound, SpeechManager.sfxVolume);
				}
				this.acceptMovement = false;
			}
		}
		if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("DpadH")) < 0.3f && Mathf.Abs(Input.GetAxisRaw("DpadV")) < 0.3f && (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS || (!Input.GetKey(KeyCode.JoystickButton4) && !Input.GetKey(KeyCode.JoystickButton5) && !Input.GetKey(KeyCode.JoystickButton6) && !Input.GetKey(KeyCode.JoystickButton7))))
		{
			this.acceptMovement = true;
		}
		if (InputManager.GetButtonDown("Jump"))
		{
			this.action = true;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Resources.UnloadUnusedAssets();
			Application.LoadLevel("Intro");
		}
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0004A344 File Offset: 0x00048544
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		GUILayout.BeginArea(new Rect(0f, 548f, 1366f, 220f));
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(50f);
		this.DrawButton(this.EnglishButtonStyle, LanguageCode.EN, SpeechManager.VoiceLanguage.English, 1, 1);
		this.DrawButton(this.ArabicButtonStyle, LanguageCode.AR, SpeechManager.VoiceLanguage.Arabic, 1, 2);
		this.DrawButton(this.FrancaisButtonStyle, LanguageCode.FR, SpeechManager.VoiceLanguage.English, 1, 3);
		this.DrawButton(this.DeutschButtonStyle, LanguageCode.DE, SpeechManager.VoiceLanguage.English, 1, 4);
		this.DrawButton(this.EspanolButtonStyle, LanguageCode.ES, SpeechManager.VoiceLanguage.English, 1, 5);
		this.DrawButton(this.ItalianoButtonStyle, LanguageCode.IT, SpeechManager.VoiceLanguage.English, 1, 6);
		this.DrawButton(this.HindiButtonStyle, LanguageCode.HI, SpeechManager.VoiceLanguage.English, 1, 7);
		GUILayout.Space(60f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(50f);
		this.DrawButton(this.NorwegianButtonStyle, LanguageCode.NO, SpeechManager.VoiceLanguage.English, 2, 1);
		this.DrawButton(this.DanishButtonStyle, LanguageCode.DA, SpeechManager.VoiceLanguage.English, 2, 2);
		this.DrawButton(this.SwedishButtonStyle, LanguageCode.SV, SpeechManager.VoiceLanguage.English, 2, 3);
		this.DrawButton(this.GreekButtonStyle, LanguageCode.EL, SpeechManager.VoiceLanguage.English, 2, 4);
		this.DrawButton(this.DutchButtonStyle, LanguageCode.NL, SpeechManager.VoiceLanguage.English, 2, 5);
		this.DrawButton(this.PortogueseButtonStyle, LanguageCode.PT, SpeechManager.VoiceLanguage.English, 2, 6);
		this.DrawButton(this.MalayButtonStyle, LanguageCode.MS, SpeechManager.VoiceLanguage.English, 2, 7);
		GUILayout.Space(60f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(50f);
		this.DrawButton(this.RussianButtonStyle, LanguageCode.RU, SpeechManager.VoiceLanguage.English, 3, 1);
		this.DrawButton(this.PolckiButtonStyle, LanguageCode.PL, SpeechManager.VoiceLanguage.English, 3, 2);
		this.DrawButton(this.JapaneseButtonStyle, LanguageCode.JA, SpeechManager.VoiceLanguage.English, 3, 3);
		this.DrawButton(this.ChineseButtonStyle, LanguageCode.ZH, SpeechManager.VoiceLanguage.English, 3, 4);
		this.DrawButton(this.KorianButtonStyle, LanguageCode.KO, SpeechManager.VoiceLanguage.English, 3, 5);
		this.DrawButton(this.TurkisButtonStyle, LanguageCode.TR, SpeechManager.VoiceLanguage.English, 3, 6);
		this.DrawButton(this.FarsiButtonStyle, LanguageCode.FA, SpeechManager.VoiceLanguage.English, 3, 7);
		GUILayout.Space(60f);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		GUI.FocusControl("focusedButton");
		this.action = false;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0004A590 File Offset: 0x00048790
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

	// Token: 0x06000902 RID: 2306 RVA: 0x0004A634 File Offset: 0x00048834
	private void DrawButton(GUIStyle style, LanguageCode menuesLanguage, SpeechManager.VoiceLanguage speechLanguage, int raw, int col)
	{
		if (this.selectedRow == raw && this.selectedCol == col)
		{
			GUI.SetNextControlName("focusedButton");
		}
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.MaxWidth((float)style.normal.background.width),
			GUILayout.MaxHeight((float)style.normal.background.height)
		});
		if ((GUI.Button(GUILayoutUtility.GetLastRect(), string.Empty, style) || (this.action && this.selectedRow == raw && this.selectedCol == col)) && !this.faded)
		{
			Language.SwitchLanguageEnm(menuesLanguage);
			SpeechManager.currentVoiceLanguage = speechLanguage;
			//GA.API.Design.NewEvent("LanguageSelected:" + menuesLanguage.ToString());
			this.Proceed();
		}
		this.checkForHoverAndPlaySound();
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0004A72C File Offset: 0x0004892C
	private void Proceed()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
		Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
		this.faded = true;
		this.startTime = Time.time;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0004A77C File Offset: 0x0004897C
	private void OnDestroy()
	{
		this.languageButtons = null;
		this.hoverSound = null;
		this.clickSound = null;
		this.ArabicButtonStyle = null;
		this.EnglishButtonStyle = null;
		this.DanishButtonStyle = null;
		this.DeutschButtonStyle = null;
		this.DutchButtonStyle = null;
		this.EspanolButtonStyle = null;
		this.FrancaisButtonStyle = null;
		this.ItalianoButtonStyle = null;
		this.JapaneseButtonStyle = null;
		this.PolckiButtonStyle = null;
		this.PortogueseButtonStyle = null;
		this.SwedishButtonStyle = null;
		this.TurkisButtonStyle = null;
		this.MalayButtonStyle = null;
		this.NorwegianButtonStyle = null;
		this.ChineseButtonStyle = null;
		this.GreekButtonStyle = null;
		this.RussianButtonStyle = null;
		Resources.UnloadUnusedAssets();
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.CancelInvoke("CheckJoystick");
		}
	}

	// Token: 0x04000BEF RID: 3055
	public GUISkin languageButtons;

	// Token: 0x04000BF0 RID: 3056
	public AudioClip hoverSound;

	// Token: 0x04000BF1 RID: 3057
	public AudioClip clickSound;

	// Token: 0x04000BF2 RID: 3058
	private GUIStyle ArabicButtonStyle;

	// Token: 0x04000BF3 RID: 3059
	private GUIStyle EnglishButtonStyle;

	// Token: 0x04000BF4 RID: 3060
	private GUIStyle DanishButtonStyle;

	// Token: 0x04000BF5 RID: 3061
	private GUIStyle DeutschButtonStyle;

	// Token: 0x04000BF6 RID: 3062
	private GUIStyle DutchButtonStyle;

	// Token: 0x04000BF7 RID: 3063
	private GUIStyle EspanolButtonStyle;

	// Token: 0x04000BF8 RID: 3064
	private GUIStyle FrancaisButtonStyle;

	// Token: 0x04000BF9 RID: 3065
	private GUIStyle ItalianoButtonStyle;

	// Token: 0x04000BFA RID: 3066
	private GUIStyle JapaneseButtonStyle;

	// Token: 0x04000BFB RID: 3067
	private GUIStyle PolckiButtonStyle;

	// Token: 0x04000BFC RID: 3068
	private GUIStyle PortogueseButtonStyle;

	// Token: 0x04000BFD RID: 3069
	private GUIStyle SwedishButtonStyle;

	// Token: 0x04000BFE RID: 3070
	private GUIStyle TurkisButtonStyle;

	// Token: 0x04000BFF RID: 3071
	private GUIStyle MalayButtonStyle;

	// Token: 0x04000C00 RID: 3072
	private GUIStyle NorwegianButtonStyle;

	// Token: 0x04000C01 RID: 3073
	private GUIStyle ChineseButtonStyle;

	// Token: 0x04000C02 RID: 3074
	private GUIStyle GreekButtonStyle;

	// Token: 0x04000C03 RID: 3075
	private GUIStyle RussianButtonStyle;

	// Token: 0x04000C04 RID: 3076
	private GUIStyle HindiButtonStyle;

	// Token: 0x04000C05 RID: 3077
	private GUIStyle KorianButtonStyle;

	// Token: 0x04000C06 RID: 3078
	private GUIStyle FarsiButtonStyle;

	// Token: 0x04000C07 RID: 3079
	private Rect lastRect;

	// Token: 0x04000C08 RID: 3080
	private int selectedRow = 1;

	// Token: 0x04000C09 RID: 3081
	private int selectedCol = 1;

	// Token: 0x04000C0A RID: 3082
	private bool acceptMovement = true;

	// Token: 0x04000C0B RID: 3083
	private int totlalRows = 3;

	// Token: 0x04000C0C RID: 3084
	private int totlalCols = 7;

	// Token: 0x04000C0D RID: 3085
	private bool action;

	// Token: 0x04000C0E RID: 3086
	private bool faded;

	// Token: 0x04000C0F RID: 3087
	private float startTime;

	// Token: 0x04000C10 RID: 3088
	private bool useVertHor;
}
