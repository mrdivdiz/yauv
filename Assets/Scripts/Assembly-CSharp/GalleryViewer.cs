using System;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class GalleryViewer : MonoBehaviour
{
	// Token: 0x06000779 RID: 1913 RVA: 0x0003C7D8 File Offset: 0x0003A9D8
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
		foreach (Transform transform in this.characters)
		{
			transform.gameObject.SetActive(false);
		}
		for (int j = 0; j < this.characters.Length; j++)
		{
			if (SaveHandler.levelReached > this.unlockAfterLevel[j] || mainmenu.demoMode || SaveHandler.gameFinished >= 1)
			{
				this.currentCharacter = j;
				break;
			}
		}
		if (this.currentCharacter != -1)
		{
			this.characters[this.currentCharacter].gameObject.SetActive(true);
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.InvokeRepeating("CheckJoystick", 0f, 1f);
		}
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0003C9BC File Offset: 0x0003ABBC
	public void CheckJoystick()
	{
		AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x0003C9D0 File Offset: 0x0003ABD0
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

	// Token: 0x0600077C RID: 1916 RVA: 0x0003CA2C File Offset: 0x0003AC2C
	private void Update()
	{
		if (this.cam == null && Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
		if (InputManager.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightArrow) || this.mobileNext)
		{
			do
			{
				this.currentCharacter = (this.currentCharacter + 1) % this.characters.Length;
			}
			while (SaveHandler.levelReached <= this.unlockAfterLevel[this.currentCharacter] && !mainmenu.demoMode && SaveHandler.gameFinished < 1);
			this.ShowSelectedCharacter();
			if (base.GetComponent<AudioSource>() != null && this.dialSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.mobileNext = false;
		}
		else if (InputManager.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.LeftArrow) || this.mobilePrevious)
		{
			do
			{
				if (this.currentCharacter == 0)
				{
					this.currentCharacter = this.characters.Length - 1;
				}
				else
				{
					this.currentCharacter--;
				}
			}
			while (SaveHandler.levelReached <= this.unlockAfterLevel[this.currentCharacter] && !mainmenu.demoMode && SaveHandler.gameFinished < 1);
			this.ShowSelectedCharacter();
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
		if (Mathf.Abs(Input.GetAxis("Horizontal2")) + Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Input.touchCount > 0)
		{
			this.xDeg -= (Input.GetAxis("Horizontal2") + Input.GetAxis("Mouse X")) * (float)this.speed * this.friction;
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
			{
				this.xDeg -= Input.touches[0].deltaPosition.x * (float)this.speed * this.friction * 0.25f;
			}
			this.fromRotation = this.characters[this.currentCharacter].rotation;
			this.toRotation = Quaternion.Euler(this.fromRotation.eulerAngles.x, this.xDeg, this.fromRotation.eulerAngles.z);
			this.characters[this.currentCharacter].rotation = Quaternion.Lerp(this.fromRotation, this.toRotation, Time.deltaTime * this.lerpSpeed);
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("LoadingMainMenu");
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0003CE04 File Offset: 0x0003B004
	private void ShowSelectedCharacter()
	{
		foreach (Transform transform in this.characters)
		{
			transform.gameObject.SetActive(false);
		}
		if (this.currentCharacter != -1)
		{
			this.characters[this.currentCharacter].gameObject.SetActive(true);
		}
		this.plain.GetComponent<Renderer>().sharedMaterial = this.backgroundMaterial[this.currentCharacter];
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0003CE80 File Offset: 0x0003B080
	public void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		this.largestStyle.normal.textColor = Color.black;
		GUI.Label(new Rect(282f, 39f, 800f, 50f), Language.Get("M_Characters", 60), this.largestStyle);
		GUI.Label(new Rect(282f, 41f, 800f, 50f), Language.Get("M_Characters", 60), this.largestStyle);
		GUI.Label(new Rect(284f, 39f, 800f, 50f), Language.Get("M_Characters", 60), this.largestStyle);
		GUI.Label(new Rect(284f, 41f, 800f, 50f), Language.Get("M_Characters", 60), this.largestStyle);
		this.largestStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(283f, 40f, 800f, 50f), Language.Get("M_Characters", 60), this.largestStyle);
		GUILayout.BeginArea(new Rect(0f, 688f, 1366f, 60f));
		GUILayout.Space(20f);
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawShadowedText(Language.Get("M_Rotation", 60), this.entryNormalStyle);
				this.DrawTextureOffseted(this.rightStick);
			}
			GUILayout.Space(30f);
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
			GUILayout.Space(30f);
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.DrawTextureOffseted(this.rightStick);
				this.DrawShadowedText(Language.Get("M_Rotation", 60), this.entryNormalStyle);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0003D2E4 File Offset: 0x0003B4E4
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

	// Token: 0x06000780 RID: 1920 RVA: 0x0003D41C File Offset: 0x0003B61C
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

	// Token: 0x06000781 RID: 1921 RVA: 0x0003D4DC File Offset: 0x0003B6DC
	private void DrawTextureOffseted(Texture tex)
	{
		GUILayout.Label(string.Empty, new GUILayoutOption[]
		{
			GUILayout.Width((float)tex.width),
			GUILayout.Height((float)tex.height)
		});
		Rect lastRect = GUILayoutUtility.GetLastRect();
		lastRect.y -= 10f;
		GUI.Label(lastRect, tex);
	}

	// Token: 0x040009A0 RID: 2464
	public Transform[] characters;

	// Token: 0x040009A1 RID: 2465
	public Material[] backgroundMaterial;

	// Token: 0x040009A2 RID: 2466
	public int[] unlockAfterLevel;

	// Token: 0x040009A3 RID: 2467
	public Transform plain;

	// Token: 0x040009A4 RID: 2468
	public int speed = 250;

	// Token: 0x040009A5 RID: 2469
	public float friction = 0.1f;

	// Token: 0x040009A6 RID: 2470
	public float lerpSpeed = 10f;

	// Token: 0x040009A7 RID: 2471
	private int currentCharacter = -1;

	// Token: 0x040009A8 RID: 2472
	private float xDeg;

	// Token: 0x040009A9 RID: 2473
	private float yDeg;

	// Token: 0x040009AA RID: 2474
	private Quaternion fromRotation;

	// Token: 0x040009AB RID: 2475
	private Quaternion toRotation;

	// Token: 0x040009AC RID: 2476
	private GUIStyle entryNormalStyle;

	// Token: 0x040009AD RID: 2477
	public Texture backButton;

	// Token: 0x040009AE RID: 2478
	public Texture previousPage;

	// Token: 0x040009AF RID: 2479
	public Texture nextPage;

	// Token: 0x040009B0 RID: 2480
	public Texture rightStick;

	// Token: 0x040009B1 RID: 2481
	public Texture PcBackButton;

	// Token: 0x040009B2 RID: 2482
	public Texture PcPreviousPage;

	// Token: 0x040009B3 RID: 2483
	public Texture PcNextPage;

	// Token: 0x040009B4 RID: 2484
	public Texture PcRightStick;

	// Token: 0x040009B5 RID: 2485
	public Texture XBoxBackButton;

	// Token: 0x040009B6 RID: 2486
	public Texture XBoxPreviousPage;

	// Token: 0x040009B7 RID: 2487
	public Texture XBoxNextPage;

	// Token: 0x040009B8 RID: 2488
	public Texture XBoxRightStick;

	// Token: 0x040009B9 RID: 2489
	public Font smallFont;

	// Token: 0x040009BA RID: 2490
	private GUIStyle largestStyle;

	// Token: 0x040009BB RID: 2491
	public Font largestFont;

	// Token: 0x040009BC RID: 2492
	private CameraFade cam;

	// Token: 0x040009BD RID: 2493
	private bool faded;

	// Token: 0x040009BE RID: 2494
	private float startTime;

	// Token: 0x040009BF RID: 2495
	public AudioClip dialSound;

	// Token: 0x040009C0 RID: 2496
	public AudioClip clickSound;

	// Token: 0x040009C1 RID: 2497
	public GUISkin guiSkin;

	// Token: 0x040009C2 RID: 2498
	private GUIStyle buttonStyle;

	// Token: 0x040009C3 RID: 2499
	private bool mobileNext;

	// Token: 0x040009C4 RID: 2500
	private bool mobilePrevious;

	// Token: 0x040009C5 RID: 2501
	private bool mobileBack;
}
