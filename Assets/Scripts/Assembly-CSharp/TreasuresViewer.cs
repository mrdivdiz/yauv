using System;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class TreasuresViewer : MonoBehaviour
{
	// Token: 0x06000888 RID: 2184 RVA: 0x00046F18 File Offset: 0x00045118
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
		foreach (Transform transform in this.treasures)
		{
			transform.gameObject.SetActive(false);
		}
		for (int j = 0; j < this.treasures.Length; j++)
		{
			if ((SaveHandler.treasures & 1 << j) != 0)
			{
				this.currentTreasure = j;
				break;
			}
		}
		if (this.currentTreasure != -1)
		{
			this.treasures[this.currentTreasure].gameObject.SetActive(true);
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay)
		{
			base.InvokeRepeating("CheckJoystick", 0f, 1f);
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x000470E4 File Offset: 0x000452E4
	public void CheckJoystick()
	{
		AndroidPlatform.isJoystickConnected = (Input.GetJoystickNames().Length > 0);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x000470F8 File Offset: 0x000452F8
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

	// Token: 0x0600088B RID: 2187 RVA: 0x00047154 File Offset: 0x00045354
	private void Update()
	{
		if (this.cam == null && Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
		if ((InputManager.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightArrow) || this.mobileNext) && SaveHandler.treasures != 0)
		{
			do
			{
				this.currentTreasure = (this.currentTreasure + 1) % this.treasures.Length;
			}
			while ((SaveHandler.treasures & 1 << this.currentTreasure) == 0);
			this.ShowSelectedTreasure();
			if (base.GetComponent<AudioSource>() != null && this.dialSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.dialSound, SpeechManager.sfxVolume);
			}
			this.mobileNext = false;
		}
		else if ((InputManager.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.LeftArrow) || this.mobilePrevious) && SaveHandler.treasures != 0)
		{
			do
			{
				if (this.currentTreasure == 0)
				{
					this.currentTreasure = this.treasures.Length - 1;
				}
				else
				{
					this.currentTreasure--;
				}
			}
			while ((SaveHandler.treasures & 1 << this.currentTreasure) == 0);
			this.ShowSelectedTreasure();
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
		if (Mathf.Abs(Input.GetAxis("Horizontal2")) + Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical2")) + Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f || Input.touchCount > 0)
		{
			this.xDeg -= (Input.GetAxis("Horizontal2") + Input.GetAxis("Mouse X")) * (float)this.speed * this.friction;
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
			{
				this.xDeg -= Input.touches[0].deltaPosition.x * (float)this.speed * this.friction * 0.25f;
			}
			this.yDeg += (Input.GetAxis("Vertical2") + Input.GetAxis("Mouse Y")) * (float)this.speed * this.friction;
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
			{
				this.yDeg -= Input.touches[0].deltaPosition.y * (float)this.speed * this.friction * 0.25f;
			}
			this.fromRotation = this.treasures[this.currentTreasure].rotation;
			this.toRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.treasures[this.currentTreasure].rotation = Quaternion.Lerp(this.fromRotation, this.toRotation, Time.deltaTime * this.lerpSpeed);
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("LoadingMainMenu");
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x000475AC File Offset: 0x000457AC
	private void ShowSelectedTreasure()
	{
		foreach (Transform transform in this.treasures)
		{
			transform.gameObject.SetActive(false);
		}
		if (this.currentTreasure != -1)
		{
			this.treasures[this.currentTreasure].gameObject.SetActive(true);
		}
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00047608 File Offset: 0x00045808
	public void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		this.largestStyle.normal.textColor = Color.black;
		GUI.Label(new Rect(482f, 39f, 400f, 50f), Language.Get("Treasures", 60), this.largestStyle);
		GUI.Label(new Rect(482f, 41f, 400f, 50f), Language.Get("Treasures", 60), this.largestStyle);
		GUI.Label(new Rect(484f, 39f, 400f, 50f), Language.Get("Treasures", 60), this.largestStyle);
		GUI.Label(new Rect(484f, 41f, 400f, 50f), Language.Get("Treasures", 60), this.largestStyle);
		this.largestStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(483f, 40f, 400f, 50f), Language.Get("Treasures", 60), this.largestStyle);
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

	// Token: 0x0600088E RID: 2190 RVA: 0x00047A6C File Offset: 0x00045C6C
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

	// Token: 0x0600088F RID: 2191 RVA: 0x00047BA4 File Offset: 0x00045DA4
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

	// Token: 0x06000890 RID: 2192 RVA: 0x00047C64 File Offset: 0x00045E64
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

	// Token: 0x04000B65 RID: 2917
	public Transform[] treasures;

	// Token: 0x04000B66 RID: 2918
	public int speed = 500;

	// Token: 0x04000B67 RID: 2919
	public float friction = 0.1f;

	// Token: 0x04000B68 RID: 2920
	public float lerpSpeed = 10f;

	// Token: 0x04000B69 RID: 2921
	private int currentTreasure = -1;

	// Token: 0x04000B6A RID: 2922
	private float xDeg;

	// Token: 0x04000B6B RID: 2923
	private float yDeg;

	// Token: 0x04000B6C RID: 2924
	private Quaternion fromRotation;

	// Token: 0x04000B6D RID: 2925
	private Quaternion toRotation;

	// Token: 0x04000B6E RID: 2926
	private GUIStyle entryNormalStyle;

	// Token: 0x04000B6F RID: 2927
	public Texture backButton;

	// Token: 0x04000B70 RID: 2928
	public Texture previousPage;

	// Token: 0x04000B71 RID: 2929
	public Texture nextPage;

	// Token: 0x04000B72 RID: 2930
	public Texture rightStick;

	// Token: 0x04000B73 RID: 2931
	public Texture PcBackButton;

	// Token: 0x04000B74 RID: 2932
	public Texture PcPreviousPage;

	// Token: 0x04000B75 RID: 2933
	public Texture PcNextPage;

	// Token: 0x04000B76 RID: 2934
	public Texture PcRightStick;

	// Token: 0x04000B77 RID: 2935
	public Texture XBoxBackButton;

	// Token: 0x04000B78 RID: 2936
	public Texture XBoxPreviousPage;

	// Token: 0x04000B79 RID: 2937
	public Texture XBoxNextPage;

	// Token: 0x04000B7A RID: 2938
	public Texture XBoxRightStick;

	// Token: 0x04000B7B RID: 2939
	public Font smallFont;

	// Token: 0x04000B7C RID: 2940
	private GUIStyle largestStyle;

	// Token: 0x04000B7D RID: 2941
	public Font largestFont;

	// Token: 0x04000B7E RID: 2942
	private CameraFade cam;

	// Token: 0x04000B7F RID: 2943
	private bool faded;

	// Token: 0x04000B80 RID: 2944
	private float startTime;

	// Token: 0x04000B81 RID: 2945
	public AudioClip dialSound;

	// Token: 0x04000B82 RID: 2946
	public AudioClip clickSound;

	// Token: 0x04000B83 RID: 2947
	public GUISkin guiSkin;

	// Token: 0x04000B84 RID: 2948
	private GUIStyle buttonStyle;

	// Token: 0x04000B85 RID: 2949
	private bool mobileNext;

	// Token: 0x04000B86 RID: 2950
	private bool mobilePrevious;

	// Token: 0x04000B87 RID: 2951
	private bool mobileBack;
}
