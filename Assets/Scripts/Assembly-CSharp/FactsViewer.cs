using System;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class FactsViewer : MonoBehaviour
{
	// Token: 0x0600073B RID: 1851 RVA: 0x00039884 File Offset: 0x00037A84
	private void Awake()
	{
		FactsViewer.Instance = this;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x0003988C File Offset: 0x00037A8C
	private void Start()
	{
		this.style = new GUIStyle();
		this.style.font = this.GetCurrentLanguageFont();
		this.style.alignment = FactsViewer.allignment;
		this.style.wordWrap = true;
		this.style.normal.textColor = Color.white;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x000398E8 File Offset: 0x00037AE8
	public void ChangedLanguage()
	{
		this.style = new GUIStyle();
		this.style.font = this.GetCurrentLanguageFont();
		this.style.alignment = FactsViewer.allignment;
		this.style.wordWrap = true;
		this.style.normal.textColor = Color.white;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00039944 File Offset: 0x00037B44
	private void OnDestroy()
	{
		FactsViewer.Instance = null;
		this.largeBackground = null;
		this.smallBackground = null;
		if (this.style != null)
		{
			this.style.font = null;
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00039974 File Offset: 0x00037B74
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.hintTimer > 0f)
		{
			this.hintTimer -= Time.deltaTime;
		}
		if (this.previousHintText != this.hintText)
		{
			this.hintTextArray = this.hintText.Split(this.specialChars);
			this.previousHintText = this.hintText;
		}
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x000399E8 File Offset: 0x00037BE8
	public static void ShowHint(string hint, FactSize size, float scale = 1f)
	{
		FactsViewer.Instance.hintText = hint;
		FactsViewer.Instance.hintTimer = 5f;
		FactsViewer.Instance.scale = scale;
		FactsViewer.Instance.size = size;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			FactsViewer.allignment = TextAnchor.MiddleRight;
		}
		else
		{
			FactsViewer.allignment = TextAnchor.MiddleLeft;
		}
		if (FactsViewer.Instance != null && FactsViewer.Instance.hintSound != null)
		{
			FactsViewer.Instance.GetComponent<AudioSource>().PlayOneShot(FactsViewer.Instance.hintSound, SpeechManager.sfxVolume);
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00039A90 File Offset: 0x00037C90
	public static void ShowFact(string hint, FactSize size, float scale, float time)
	{
		FactsViewer.Instance.hintText = hint;
		FactsViewer.Instance.hintTimer = time;
		FactsViewer.Instance.scale = scale;
		FactsViewer.Instance.size = size;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			FactsViewer.allignment = TextAnchor.MiddleRight;
		}
		else
		{
			FactsViewer.allignment = TextAnchor.MiddleLeft;
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00039AF4 File Offset: 0x00037CF4
	public static void ShowHint(string hint, FactSize size, float scale, float delay)
	{
		FactsViewer.Instance.hintText = hint;
		FactsViewer.Instance.scale = scale;
		FactsViewer.Instance.size = size;
		if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
		{
			FactsViewer.allignment = TextAnchor.MiddleRight;
		}
		else
		{
			FactsViewer.allignment = TextAnchor.MiddleLeft;
		}
		FactsViewer.Instance.Invoke("ShowDelayed", delay);
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00039B5C File Offset: 0x00037D5C
	private void ShowDelayed()
	{
		FactsViewer.Instance.hintTimer = 5f;
		if (FactsViewer.Instance != null && FactsViewer.Instance.hintSound != null)
		{
			FactsViewer.Instance.GetComponent<AudioSource>().PlayOneShot(FactsViewer.Instance.hintSound, SpeechManager.sfxVolume);
		}
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00039BBC File Offset: 0x00037DBC
	public void OnGUI()
	{
		if (mainmenu.pause || mainmenu.disableHUD)
		{
			return;
		}
		if (this.hintTimer > 0f && this.hintTextArray != null && this.hintTextArray.Length > 0)
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
			FactSize factSize = this.size;
			if (factSize != FactSize.OneLine)
			{
				if (factSize == FactSize.MulitLines)
				{
					GUI.DrawTexture(new Rect(80f, 384f - 256f * this.scale / 2f, 512f * this.scale, 256f * this.scale), this.largeBackground);
					GUILayout.BeginArea(new Rect(80f, 384f - 256f * this.scale / 2f, 512f * this.scale, 256f * this.scale));
				}
			}
			else
			{
				GUI.DrawTexture(new Rect(80f, 384f - 128f * this.scale / 2f, 512f * this.scale, 128f * this.scale), this.smallBackground);
				GUILayout.BeginArea(new Rect(80f, 384f - 128f * this.scale / 2f, 512f * this.scale, 128f * this.scale));
			}
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(512f * this.scale * 0.9f)
			});
			if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
			{
				GUILayout.FlexibleSpace();
			}
			else
			{
				GUILayout.Space(512f * this.scale * 0.1f);
			}
			int num = 0;
			foreach (string text in this.hintTextArray)
			{
				bool flag = false;
				if ((this.hintTextArray.Length > 1 && num + text.Length > 60) || text == "BR")
				{
					if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
					{
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(new GUILayoutOption[]
					{
						GUILayout.Width(512f * this.scale * 0.9f)
					});
					if (Language.CurrentLanguage() == LanguageCode.AR || Language.CurrentLanguage() == LanguageCode.FA)
					{
						GUILayout.FlexibleSpace();
					}
					else
					{
						GUILayout.Space(512f * this.scale * 0.1f);
					}
					num = 0;
				}
				foreach (Keyword keyword in this.keywords)
				{
					if (text == keyword.keyword)
					{
						if (AndroidPlatform.IsJoystickConnected())
						{
							if (keyword.ps3Icon != null)
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
					this.style.alignment = FactsViewer.allignment;
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
			if (Language.CurrentLanguage() != LanguageCode.AR && Language.CurrentLanguage() != LanguageCode.FA)
			{
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0003A134 File Offset: 0x00038334
	private Font GetCurrentLanguageFont()
	{
		return Language.GetFont18();
	}

	// Token: 0x04000926 RID: 2342
	public Texture largeBackground;

	// Token: 0x04000927 RID: 2343
	public Texture smallBackground;

	// Token: 0x04000928 RID: 2344
	public Keyword[] keywords;

	// Token: 0x04000929 RID: 2345
	private string hintText = string.Empty;

	// Token: 0x0400092A RID: 2346
	private string previousHintText = string.Empty;

	// Token: 0x0400092B RID: 2347
	private string[] hintTextArray;

	// Token: 0x0400092C RID: 2348
	private float hintTimer;

	// Token: 0x0400092D RID: 2349
	private static FactsViewer Instance;

	// Token: 0x0400092E RID: 2350
	private float scale = 0.8f;

	// Token: 0x0400092F RID: 2351
	private FactSize size;

	// Token: 0x04000930 RID: 2352
	private char[] specialChars = new char[]
	{
		'<',
		'>'
	};

	// Token: 0x04000931 RID: 2353
	public AudioClip hintSound;

	// Token: 0x04000932 RID: 2354
	private GUIStyle style;

	// Token: 0x04000933 RID: 2355
	private static TextAnchor allignment = TextAnchor.MiddleLeft;
}
