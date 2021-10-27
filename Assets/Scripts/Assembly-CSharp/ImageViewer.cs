using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class ImageViewer : MonoBehaviour
{
	// Token: 0x060007A5 RID: 1957 RVA: 0x0003EA64 File Offset: 0x0003CC64
	private void Start()
	{
		this.guiSkin.font = Language.GetFont29();
		this.buttonStyle = this.guiSkin.button;
		if (this.entryNormalStyle == null)
		{
			this.entryNormalStyle = new GUIStyle();
			this.entryNormalStyle.font = Language.GetFont18();
			this.entryNormalStyle.alignment = TextAnchor.UpperCenter;
			this.entryNormalStyle.wordWrap = true;
			this.entryNormalStyle.normal.textColor = Color.white;
		}
		if (this.largestStyle == null)
		{
			this.largestStyle = new GUIStyle();
			if (Language.CurrentLanguage() == LanguageCode.JA || Language.CurrentLanguage() == LanguageCode.ZH || Language.CurrentLanguage() == LanguageCode.HI || Language.CurrentLanguage() == LanguageCode.KO)
			{
				this.largestStyle.font = Language.GetFont29();
			}
			else
			{
				this.largestStyle.font = this.largestFont;
			}
			this.largestStyle.alignment = TextAnchor.MiddleCenter;
			this.largestStyle.wordWrap = true;
			this.largestStyle.normal.textColor = Color.white;
		}
		this.currentNumber = this.currentImage + 1 + " / " + this.images.Length;
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.InvokeRepeating("CheckJoystick", 0f, 1f);
		}
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0003EBD8 File Offset: 0x0003CDD8
	public void CheckJoystick()
	{
		AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0003EBEC File Offset: 0x0003CDEC
	public void OnDestroy()
	{
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.CancelInvoke("CheckJoystick");
		}
		if (this.entryNormalStyle != null)
		{
			this.entryNormalStyle.font = null;
		}
		if (this.largestStyle != null)
		{
			this.largestStyle.font = null;
		}
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0003EC48 File Offset: 0x0003CE48
	private void Update()
	{
		if (this.cam == null && Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
		if (InputManager.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightArrow) || this.mobileNext)
		{
			this.currentImage = (this.currentImage + 1) % this.images.Length;
			this.currentNumber = this.currentImage + 1 + " / " + this.images.Length;
			if (base.GetComponent<AudioSource>() != null && this.dialSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.mobileNext = false;
		}
		else if (InputManager.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.LeftArrow) || this.mobilePrevious)
		{
			if (this.currentImage > 0)
			{
				this.currentImage--;
			}
			else
			{
				this.currentImage = this.images.Length - 1;
			}
			this.currentNumber = this.currentImage + 1 + " / " + this.images.Length;
			if (base.GetComponent<AudioSource>() != null && this.dialSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.mobilePrevious = false;
		}
		else if (this.cam != null && !this.faded && (Input.GetKeyDown(KeyCode.JoystickButton1) || InputManager.GetButtonDown("Cover") || this.mobileBack))
		{
			this.cam.StartFade(Color.black, 3f);
			this.faded = true;
			this.startTime = Time.time;
			if (base.GetComponent<AudioSource>() != null && this.clickSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound, SpeechManager.sfxVolume);
			}
			this.mobileBack = false;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("LoadingMainMenu");
		}
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0003EED0 File Offset: 0x0003D0D0
	public void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		this.largestStyle.normal.textColor = Color.black;
		GUI.Label(new Rect(282f, 39f, 800f, 50f), Language.Get(this.sceneTitleKeyword, 60), this.largestStyle);
		GUI.Label(new Rect(282f, 41f, 800f, 50f), Language.Get(this.sceneTitleKeyword, 60), this.largestStyle);
		GUI.Label(new Rect(284f, 39f, 800f, 50f), Language.Get(this.sceneTitleKeyword, 60), this.largestStyle);
		GUI.Label(new Rect(284f, 41f, 800f, 50f), Language.Get(this.sceneTitleKeyword, 60), this.largestStyle);
		this.largestStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(283f, 40f, 800f, 50f), Language.Get(this.sceneTitleKeyword, 60), this.largestStyle);
		this.entryNormalStyle.normal.textColor = Color.black;
		GUI.Label(new Rect(482f, 99f, 400f, 50f), this.currentNumber, this.entryNormalStyle);
		GUI.Label(new Rect(482f, 101f, 400f, 50f), this.currentNumber, this.entryNormalStyle);
		GUI.Label(new Rect(484f, 99f, 400f, 50f), this.currentNumber, this.entryNormalStyle);
		GUI.Label(new Rect(484f, 101f, 400f, 50f), this.currentNumber, this.entryNormalStyle);
		this.entryNormalStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(483f, 100f, 400f, 50f), this.currentNumber, this.entryNormalStyle);
		if (Application.loadedLevelName == "StoryBoards" || Application.loadedLevelName == "ConceptArt")
		{
			GUILayout.BeginArea(new Rect(250f, 145f, 1366f, 518f));
		}
		else
		{
			GUILayout.BeginArea(new Rect((1366f - (float)this.images[this.currentImage].width) / 4f, 145f, 1366f, 518f));
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(this.images[this.currentImage], new GUILayoutOption[]
		{
			GUILayout.Height(518f)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(0f, 688f, 1366f, 60f));
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawShadowedText(Language.Get("M_Next", 60), this.entryNormalStyle);
				this.DrawTextureOffseted(this.nextPage);
			}
			else if (this.DrawBackButton(Language.Get("M_Next", 60)))
			{
				this.mobileNext = true;
			}
			GUILayout.Space(30f);
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawShadowedText(Language.Get("M_Previous", 60), this.entryNormalStyle);
				this.DrawTextureOffseted(this.previousPage);
			}
			else if (this.DrawBackButton(Language.Get("M_Previous", 60)))
			{
				this.mobilePrevious = true;
			}
			GUILayout.Space(30f);
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawShadowedText(Language.Get("M_Back", 60), this.entryNormalStyle);
				this.DrawTextureOffseted(this.backButton);
			}
			else if (this.DrawBackButton(Language.Get("M_Back", 60)))
			{
				this.mobileBack = true;
			}
			GUILayout.Space(30f);
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(30f);
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawTextureOffseted(this.backButton);
				this.DrawShadowedText(Language.Get("M_Back", 60), this.entryNormalStyle);
			}
			else if (this.DrawBackButton(Language.Get("M_Back", 60)))
			{
				this.mobileBack = true;
			}
			GUILayout.Space(30f);
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawTextureOffseted(this.previousPage);
				this.DrawShadowedText(Language.Get("M_Previous", 60), this.entryNormalStyle);
			}
			else if (this.DrawBackButton(Language.Get("M_Previous", 60)))
			{
				this.mobilePrevious = true;
			}
			GUILayout.Space(30f);
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawTextureOffseted(this.nextPage);
				this.DrawShadowedText(Language.Get("M_Next", 60), this.entryNormalStyle);
			}
			else if (this.DrawBackButton(Language.Get("M_Next", 60)))
			{
				this.mobileNext = true;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0003F494 File Offset: 0x0003D694
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

	// Token: 0x060007AB RID: 1963 RVA: 0x0003F5CC File Offset: 0x0003D7CC
	private void DrawShadowedText(string p, GUIStyle entryNormalStyle)
	{
		entryNormalStyle.normal.textColor = Color.black;
		GUILayout.Label(p, entryNormalStyle, new GUILayoutOption[0]);
		Rect lastRect = GUILayoutUtility.GetLastRect();
		lastRect.x += 2f;
		GUI.Label(lastRect, p, entryNormalStyle);
		lastRect.y += 2f;
		GUI.Label(lastRect, p, entryNormalStyle);
		lastRect.x -= 2f;
		GUI.Label(lastRect, p, entryNormalStyle);
		lastRect.x += 1f;
		lastRect.y -= 1f;
		entryNormalStyle.normal.textColor = Color.white;
		GUI.Label(lastRect, p, entryNormalStyle);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x0003F68C File Offset: 0x0003D88C
	private void DrawTextureOffseted(Texture tex)
	{
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.Width((float)tex.width),
			GUILayout.Height((float)tex.height)
		});
		Rect lastRect = GUILayoutUtility.GetLastRect();
		lastRect.y -= 13f;
		GUI.Label(lastRect, tex);
	}

	// Token: 0x04000A04 RID: 2564
	public string sceneTitleKeyword = "M_CONCEPTART";

	// Token: 0x04000A05 RID: 2565
	public Texture[] images;

	// Token: 0x04000A06 RID: 2566
	private int currentImage;

	// Token: 0x04000A07 RID: 2567
	private GUIStyle entryNormalStyle;

	// Token: 0x04000A08 RID: 2568
	public Texture backButton;

	// Token: 0x04000A09 RID: 2569
	public Texture previousPage;

	// Token: 0x04000A0A RID: 2570
	public Texture nextPage;

	// Token: 0x04000A0B RID: 2571
	public Texture PcBackButton;

	// Token: 0x04000A0C RID: 2572
	public Texture PcPreviousPage;

	// Token: 0x04000A0D RID: 2573
	public Texture PcNextPage;

	// Token: 0x04000A0E RID: 2574
	public Texture XBoxBackButton;

	// Token: 0x04000A0F RID: 2575
	public Texture XBoxPreviousPage;

	// Token: 0x04000A10 RID: 2576
	public Texture XBoxNextPage;

	// Token: 0x04000A11 RID: 2577
	public Font smallFont;

	// Token: 0x04000A12 RID: 2578
	private GUIStyle largestStyle;

	// Token: 0x04000A13 RID: 2579
	public Font largestFont;

	// Token: 0x04000A14 RID: 2580
	private CameraFade cam;

	// Token: 0x04000A15 RID: 2581
	private bool faded;

	// Token: 0x04000A16 RID: 2582
	private float startTime;

	// Token: 0x04000A17 RID: 2583
	private string currentNumber = "1 / 8";

	// Token: 0x04000A18 RID: 2584
	public AudioClip dialSound;

	// Token: 0x04000A19 RID: 2585
	public AudioClip clickSound;

	// Token: 0x04000A1A RID: 2586
	public GUISkin guiSkin;

	// Token: 0x04000A1B RID: 2587
	private GUIStyle buttonStyle;

	// Token: 0x04000A1C RID: 2588
	private bool mobileNext;

	// Token: 0x04000A1D RID: 2589
	private bool mobilePrevious;

	// Token: 0x04000A1E RID: 2590
	private bool mobileBack;
}
