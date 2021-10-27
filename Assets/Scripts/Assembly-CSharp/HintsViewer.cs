using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class HintsViewer : MonoBehaviour
{
	// Token: 0x06000798 RID: 1944 RVA: 0x0003DC18 File Offset: 0x0003BE18
	private void Awake()
	{
		HintsViewer.Instance = this;
		if (this.continueButton == null)
		{
			this.continueButton = (Texture)Resources.Load("X");
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0003DC54 File Offset: 0x0003BE54
	private void Start()
	{
		this.style = new GUIStyle();
		this.style.font = this.GetCurrentLanguageFont();
		this.style.alignment = HintsViewer.allignment;
		this.style.wordWrap = true;
		this.style.normal.textColor = Color.white;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0003DCB0 File Offset: 0x0003BEB0
	public void ChangedLanguage()
	{
		this.style = new GUIStyle();
		this.style.font = this.GetCurrentLanguageFont();
		this.style.alignment = HintsViewer.allignment;
		this.style.wordWrap = true;
		this.style.normal.textColor = Color.white;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0003DD0C File Offset: 0x0003BF0C
	private void OnDestroy()
	{
		HintsViewer.Instance = null;
		this.largeBackground = null;
		this.smallBackground = null;
		if (this.style != null)
		{
			this.style.font = null;
		}
		if (this.continueButton != null)
		{
			Resources.UnloadAsset(this.continueButton);
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0003DD60 File Offset: 0x0003BF60
	private void Update()
	{
		if (mainmenu.pause && !mainmenu.hintPause)
		{
			return;
		}
		if (this.hintTimer > 0f)
		{
			this.hintTimer -= Time.deltaTime;
			if (mainmenu.pause && mainmenu.hintPause && InputManager.GetButtonDown("Jump"))
			{
				this.ResumeFromPause();
			}
		}
		if (this.previousHintText != this.hintText)
		{
			this.hintTextArray = this.hintText.Split(this.specialChars);
			this.previousHintText = this.hintText;
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0003DE08 File Offset: 0x0003C008
	private void ResumeFromPause()
	{
		mainmenu.pause = false;
		mainmenu.hintPause = false;
		Time.timeScale = 1f;
		AudioListener.pause = false;
		this.hintTimer = 0f;
		if (Camera.main != null)
		{
			ShooterGameCamera component = Camera.main.GetComponent<ShooterGameCamera>();
			if (component != null)
			{
				component.lockTarget = null;
				component.focusOnTarget = false;
				component.hintFocus = false;
			}
		}
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0003DE78 File Offset: 0x0003C078
	public static void ShowHint(string hint, HintSize size, float scale = 1f)
	{
		HintsViewer.Instance.hintText = hint;
		HintsViewer.Instance.hintTimer = 5f;
		HintsViewer.Instance.scale = scale;
		HintsViewer.Instance.size = size;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			HintsViewer.allignment = TextAnchor.MiddleCenter;
		}
		else
		{
			HintsViewer.allignment = TextAnchor.MiddleCenter;
		}
		if (HintsViewer.Instance != null && HintsViewer.Instance.hintSound != null)
		{
			HintsViewer.Instance.GetComponent<AudioSource>().PlayOneShot(HintsViewer.Instance.hintSound, SpeechManager.sfxVolume);
		}
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0003DF20 File Offset: 0x0003C120
	public static void ShowFact(string hint, HintSize size, float scale, float time)
	{
		HintsViewer.Instance.hintText = hint;
		HintsViewer.Instance.hintTimer = time;
		HintsViewer.Instance.scale = scale;
		HintsViewer.Instance.size = size;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			HintsViewer.allignment = TextAnchor.MiddleRight;
		}
		else
		{
			HintsViewer.allignment = TextAnchor.MiddleLeft;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0003DF84 File Offset: 0x0003C184
	public static void ShowHint(string hint, HintSize size, float scale, float delay)
	{
		HintsViewer.Instance.hintText = hint;
		HintsViewer.Instance.scale = scale;
		HintsViewer.Instance.size = size;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			HintsViewer.allignment = TextAnchor.MiddleCenter;
		}
		else
		{
			HintsViewer.allignment = TextAnchor.MiddleCenter;
		}
		HintsViewer.Instance.Invoke("ShowDelayed", delay);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0003DFEC File Offset: 0x0003C1EC
	private void ShowDelayed()
	{
		HintsViewer.Instance.hintTimer = 5f;
		if (HintsViewer.Instance != null && HintsViewer.Instance.hintSound != null)
		{
			HintsViewer.Instance.GetComponent<AudioSource>().PlayOneShot(HintsViewer.Instance.hintSound, SpeechManager.sfxVolume);
		}
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0003E04C File Offset: 0x0003C24C
	public void OnGUI()
	{
		if ((mainmenu.pause && !mainmenu.hintPause) || mainmenu.disableHUD)
		{
			return;
		}
		if (this.hintTimer > 0f && this.hintTextArray != null && this.hintTextArray.Length > 0)
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
			HintSize hintSize = this.size;
			if (hintSize != HintSize.OneLine)
			{
				if (hintSize == HintSize.MulitLines)
				{
					GUI.DrawTexture(new Rect(80f, 384f - 256f * this.scale / 2f, 512f * this.scale, 256f * this.scale), this.largeBackground);
					GUILayout.BeginArea(new Rect(80f, 384f - 256f * this.scale / 2f, 512f * this.scale, 256f * this.scale));
				}
			}
			else
			{
				GUI.DrawTexture(new Rect(683f - 1024f * this.scale / 2f, 120f, 1024f * this.scale, 128f * this.scale), this.smallBackground);
				GUILayout.BeginArea(new Rect(683f - 1024f * this.scale / 2f, 120f, 1024f * this.scale, 128f * this.scale));
			}
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(1024f * this.scale * 0.9f)
			});
			GUILayout.FlexibleSpace();
			int num = 0;
			foreach (string text in this.hintTextArray)
			{
				bool flag = false;
				if (this.hintTextArray.Length > 1 && num + text.Length > 120)
				{
					if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
					{
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(new GUILayoutOption[]
					{
						GUILayout.Width(1024f * this.scale * 0.9f)
					});
					if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
					{
						GUILayout.FlexibleSpace();
					}
					else
					{
						GUILayout.Space(1024f * this.scale * 0.1f);
					}
					num = 0;
				}
				foreach (Keyword keyword in this.keywords)
				{
					if (text == keyword.keyword)
					{
						if (AndroidPlatform.IsJoystickConnected())
						{
							if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetJoystickNames()[0].Contains("extended"))
							{
								if (keyword.MobileExtendedProfile != null)
								{
									GUILayout.Label(string.Empty, new GUILayoutOption[]
									{
										GUILayout.Width((float)keyword.MobileExtendedProfile.width),
										GUILayout.Height((float)keyword.MobileExtendedProfile.height)
									});
									GUI.Label(GUILayoutUtility.GetLastRect(), keyword.MobileExtendedProfile, this.style);
								}
								else if (keyword.ps3Icon != null)
								{
									GUILayout.Label(string.Empty, new GUILayoutOption[]
									{
										GUILayout.Width((float)keyword.ps3Icon.width),
										GUILayout.Height((float)keyword.ps3Icon.height)
									});
									GUI.Label(GUILayoutUtility.GetLastRect(), keyword.ps3Icon, this.style);
								}
							}
							else if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetJoystickNames()[0].Contains("basic"))
							{
								if (keyword.MobileBasicProfile != null)
								{
									GUILayout.Label(string.Empty, new GUILayoutOption[]
									{
										GUILayout.Width((float)keyword.MobileBasicProfile.width),
										GUILayout.Height((float)keyword.MobileBasicProfile.height)
									});
									GUI.Label(GUILayoutUtility.GetLastRect(), keyword.MobileBasicProfile, this.style);
								}
								else if (keyword.ps3Icon != null)
								{
									GUILayout.Label(string.Empty, new GUILayoutOption[]
									{
										GUILayout.Width((float)keyword.ps3Icon.width),
										GUILayout.Height((float)keyword.ps3Icon.height)
									});
									GUI.Label(GUILayoutUtility.GetLastRect(), keyword.ps3Icon, this.style);
								}
							}
							else if (keyword.ps3Icon != null)
							{
								GUILayout.Label(string.Empty, new GUILayoutOption[]
								{
									GUILayout.Width((float)keyword.ps3Icon.width),
									GUILayout.Height((float)keyword.ps3Icon.height)
								});
								GUI.Label(GUILayoutUtility.GetLastRect(), keyword.ps3Icon, this.style);
							}
						}
						else if (keyword.mobileIcon != null)
						{
							GUILayout.Label(string.Empty, new GUILayoutOption[]
							{
								GUILayout.Width((float)keyword.mobileIcon.width),
								GUILayout.Height((float)keyword.mobileIcon.height)
							});
							GUI.Label(GUILayoutUtility.GetLastRect(), keyword.mobileIcon, this.style);
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.style.normal.textColor = Color.black;
					this.style.alignment = HintsViewer.allignment;
					GUILayout.Label(text, this.style, new GUILayoutOption[]
					{
						GUILayout.ExpandHeight(true),
						GUILayout.ExpandHeight(true)
					});
					Rect lastRect = GUILayoutUtility.GetLastRect();
					GUI.Label(new Rect(lastRect.x + 2f, lastRect.y + 2f, lastRect.width, lastRect.height), text, this.style);
					GUI.Label(new Rect(lastRect.x + 2f, lastRect.y, lastRect.width, lastRect.height), text, this.style);
					GUI.Label(new Rect(lastRect.x, lastRect.y + 2f, lastRect.width, lastRect.height), text, this.style);
					this.style.normal.textColor = Color.white;
					GUI.Label(new Rect(lastRect.x + 1f, lastRect.y + 1f, lastRect.width, lastRect.height), text, this.style);
					num += text.Length;
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
			if (mainmenu.pause && mainmenu.hintPause)
			{
				if (AndroidPlatform.IsJoystickConnected())
				{
					if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
					{
						if (this.continueButton != null)
						{
							GUI.Label(new Rect(1110f, 220f, 50f, 50f), this.continueButton);
						}
						this.style.alignment = TextAnchor.MiddleRight;
						this.style.normal.textColor = Color.black;
						GUI.Label(new Rect(902f, 192f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
						GUI.Label(new Rect(902f, 190f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
						GUI.Label(new Rect(900f, 192f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
						this.style.normal.textColor = Color.white;
						GUI.Label(new Rect(901f, 191f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
					}
					else
					{
						if (this.continueButton != null)
						{
							GUI.Label(new Rect(220f, 220f, 50f, 50f), this.continueButton);
						}
						this.style.alignment = TextAnchor.MiddleLeft;
						this.style.normal.textColor = Color.black;
						GUI.Label(new Rect(262f, 192f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
						GUI.Label(new Rect(262f, 190f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
						GUI.Label(new Rect(260f, 192f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
						this.style.normal.textColor = Color.white;
						GUI.Label(new Rect(261f, 191f, 200f, 100f), Language.Get("M_Continue", 60), this.style);
					}
				}
				else
				{
					GUI.skin.font = this.GetCurrentLanguageFont();
					if (GUI.Button(new Rect(945f, 220f, 200f, 50f), Language.Get("M_Continue", 60)))
					{
						this.ResumeFromPause();
					}
					GUI.skin.font = null;
				}
			}
		}
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0003EA3C File Offset: 0x0003CC3C
	private Font GetCurrentLanguageFont()
	{
		return Language.GetFont18();
	}

	// Token: 0x040009F5 RID: 2549
	public Texture largeBackground;

	// Token: 0x040009F6 RID: 2550
	public Texture smallBackground;

	// Token: 0x040009F7 RID: 2551
	public Keyword[] keywords;

	// Token: 0x040009F8 RID: 2552
	private string hintText = string.Empty;

	// Token: 0x040009F9 RID: 2553
	private string previousHintText = string.Empty;

	// Token: 0x040009FA RID: 2554
	private string[] hintTextArray;

	// Token: 0x040009FB RID: 2555
	private float hintTimer;

	// Token: 0x040009FC RID: 2556
	private static HintsViewer Instance;

	// Token: 0x040009FD RID: 2557
	private float scale = 0.8f;

	// Token: 0x040009FE RID: 2558
	private HintSize size;

	// Token: 0x040009FF RID: 2559
	private char[] specialChars = new char[]
	{
		'<',
		'>'
	};

	// Token: 0x04000A00 RID: 2560
	public AudioClip hintSound;

	// Token: 0x04000A01 RID: 2561
	private GUIStyle style;

	// Token: 0x04000A02 RID: 2562
	private static TextAnchor allignment = TextAnchor.MiddleLeft;

	// Token: 0x04000A03 RID: 2563
	public Texture continueButton;
}
