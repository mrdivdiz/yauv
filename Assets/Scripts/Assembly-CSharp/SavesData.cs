using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class SavesData : MonoBehaviour
{
	// Token: 0x06000A89 RID: 2697 RVA: 0x00071930 File Offset: 0x0006FB30
	private void Start()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle();
			this.style.font = Language.GetFont18();
			this.style.alignment = TextAnchor.MiddleCenter;
			this.style.wordWrap = true;
			this.style.normal.textColor = Color.white;
		}
		this.guiSkin.font = Language.GetFont29();
		this.buttonStyle = this.guiSkin.button;
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x000719B4 File Offset: 0x0006FBB4
	private void OnDestroy()
	{
		if (this.style != null)
		{
			this.style.font = null;
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x000719D0 File Offset: 0x0006FBD0
	private void Update()
	{
		if (!this.faded && (InputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space) || this.action))
		{
			if (base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.clickSound);
			}
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.startTime = Time.time;
			this.action = false;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel(this.levelToLoad);
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00071A90 File Offset: 0x0006FC90
	private void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		this.style.normal.textColor = Color.black;
		GUI.Label(new Rect(384f, 83f, 600f, 600f), Language.Get(this.messageKeyword, 60), this.style);
		GUI.Label(new Rect(384f, 85f, 600f, 600f), Language.Get(this.messageKeyword, 60), this.style);
		GUI.Label(new Rect(382f, 83f, 600f, 600f), Language.Get(this.messageKeyword, 60), this.style);
		GUI.Label(new Rect(382f, 85f, 600f, 600f), Language.Get(this.messageKeyword, 60), this.style);
		this.style.normal.textColor = Color.white;
		GUI.Label(new Rect(383f, 84f, 600f, 600f), Language.Get(this.messageKeyword, 60), this.style);
		if (!AndroidPlatform.IsJoystickConnected())
		{
			GUILayout.BeginArea(new Rect(0f, 688f, 1366f, 60f));
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.Space(20f);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				if (this.DrawBackButton(Language.Get("M_Continue", 60)))
				{
					this.action = true;
				}
				GUILayout.Space(30f);
				GUILayout.EndHorizontal();
			}
			else
			{
				GUILayout.Space(20f);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(30f);
				if (this.DrawBackButton(Language.Get("M_Continue", 60)))
				{
					this.action = true;
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
		else if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(1197f, 692f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(1197f, 694f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(1195f, 692f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(1195f, 694f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(1196f, 693f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(1276f, 703f, 60f, 60f), this.continuePS3);
		}
		else
		{
			GUI.Label(new Rect(70f, 703f, 60f, 60f), this.continuePS3);
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(101f, 692f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(101f, 694f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(99f, 692f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			GUI.Label(new Rect(99f, 694f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(100f, 693f, 100f, 60f), Language.Get("M_Continue", 60), this.style);
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00071F68 File Offset: 0x00070168
	private bool DrawBackButton(string caption)
	{
		float fixedWidth = this.buttonStyle.fixedWidth;
		this.buttonStyle.fixedWidth = 150f;
		float fixedHeight = this.buttonStyle.fixedHeight;
		this.buttonStyle.fixedHeight = 40f;
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
		return result;
	}

	// Token: 0x040010D6 RID: 4310
	private GUIStyle style;

	// Token: 0x040010D7 RID: 4311
	public Texture continuePS3;

	// Token: 0x040010D8 RID: 4312
	public AudioClip clickSound;

	// Token: 0x040010D9 RID: 4313
	private bool faded;

	// Token: 0x040010DA RID: 4314
	private float startTime;

	// Token: 0x040010DB RID: 4315
	public string messageKeyword = "M_SaveData";

	// Token: 0x040010DC RID: 4316
	public string levelToLoad = "LoadingMainMenu";

	// Token: 0x040010DD RID: 4317
	public GUISkin guiSkin;

	// Token: 0x040010DE RID: 4318
	private GUIStyle buttonStyle;

	// Token: 0x040010DF RID: 4319
	private bool action;
}
