using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class Language : MonoBehaviour
{
	// Token: 0x060004C4 RID: 1220 RVA: 0x0001F01C File Offset: 0x0001D21C
	private void Awake()
	{
		Language.Instance = this;
		Language.LoadAvailableLanguages();
		LanguageCode languageEnum = Language.userSelectedLanguage;
		TextAsset textAsset = (TextAsset)Resources.Load("Languages/settings.txt", typeof(TextAsset));
		if (textAsset != null)
		{
			string[] array = textAsset.text.Split(new char[]
			{
				'\n'
			});
			if (array.Length >= 2)
			{
				languageEnum = Language.GetLanguageEnum(array[1]);
			}
		}
		string @string = PlayerPrefs.GetString("M2H_lastLanguage", string.Empty);
		if (@string != string.Empty && Language.availableLanguages.Contains(@string))
		{
			Language.SwitchLanguageStr(@string);
		}
		else
		{
			Language.SwitchLanguageEnm(languageEnum);
		}
		LoadCurrentLanguageFonts();
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0001F0D4 File Offset: 0x0001D2D4
	public void LoadCurrentLanguageFonts()
	{
		currentFont14 = null;
		currentFont18 = null;
		currentFont29 = null;
		Resources.UnloadUnusedAssets();
		LanguageCode languageCode = Language.CurrentLanguage();
		switch(Language.CurrentLanguage()){
		case LanguageCode.RU:
		currentFont14 = Resources.Load("RU14")as Font;
		currentFont18 = Resources.Load("RU18")as Font;
		currentFont29 = Resources.Load("RU29")as Font;
			break;
			default:
		currentFont14 = Resources.Load("arialbd14")as Font;
		currentFont18 = Resources.Load("arialbd18")as Font;
		currentFont29 = Resources.Load("arialbd29")as Font;	
			break;
		}
/*
		if (languageCode == LanguageCode.RU){
		currentFont14 = (Font)Resources.Load("RU14");
		currentFont18 = (Font)Resources.Load("RU18");
		currentFont29 = (Font)Resources.Load("RU29");
		}else{
			
		}
		if (languageCode != LanguageCode.EL)
		{
			if (languageCode == LanguageCode.HI)
			{
				currentFont14 = (Font)Resources.Load("HI14");
				currentFont18 = (Font)Resources.Load("HI18");
				currentFont29 = (Font)Resources.Load("HI29");
				return;
			}
			if (languageCode == LanguageCode.JA)
			{
				currentFont14 = (Font)Resources.Load("JA18");
				currentFont18 = null;
				currentFont29 = null;
				return;
			}
			if (languageCode == LanguageCode.KO)
			{
				currentFont14 = (Font)Resources.Load("KO19");
				currentFont18 = null;
				currentFont29 = null;
				return;
			}
			if (languageCode != LanguageCode.PL && languageCode != LanguageCode.RU)
			{
				if (languageCode != LanguageCode.ZH)
				{
					currentFont14 = (Font)Resources.Load("AdobeArabic14");
					currentFont18 = (Font)Resources.Load("AdobeArabic18");
					currentFont29 = (Font)Resources.Load("AdobeArabic29");
					return;
				}
				currentFont14 = (Font)Resources.Load("ZH18");
				currentFont18 = null;
				currentFont29 = null;
				return;
			}
		}*/
		
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0001F288 File Offset: 0x0001D488
	private static void LoadAvailableLanguages()
	{
		Language.availableLanguages = new List<string>();
		foreach (object obj in Enum.GetValues(typeof(LanguageCode)))
		{
			LanguageCode languageCode = (LanguageCode)((int)obj);
			if (Language.HasLanguageFile(languageCode + string.Empty, 0))
			{
				Language.availableLanguages.Add(languageCode + string.Empty);
			}
		}
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0001F340 File Offset: 0x0001D540
	public static LanguageCode GetLanguageEnum(string langCode)
	{
		langCode = langCode.ToUpper();
		foreach (object obj in Enum.GetValues(typeof(LanguageCode)))
		{
			LanguageCode languageCode = (LanguageCode)((int)obj);
			if (languageCode + string.Empty == langCode)
			{
				return languageCode;
			}
		}
		Debug.LogError("ERORR: There is no language: [" + langCode + "]");
		return Language.currentLanguage;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
	public static string[] GetLanguages()
	{
		return Language.availableLanguages.ToArray();
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001F404 File Offset: 0x0001D604
	public static bool SwitchLanguageStr(string langCode)
	{
		return Language.SwitchLanguageEnm(Language.GetLanguageEnum(langCode));
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0001F414 File Offset: 0x0001D614
	public static bool SwitchLanguageEnm(LanguageCode code)
	{
		if (Language.availableLanguages.Contains(code + string.Empty))
		{
			Language.DoSwitch(code);
			return true;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Could not switch from language ",
			Language.currentLanguage,
			" to ",
			code
		}));
		if (Language.currentLanguage == LanguageCode.N)
		{
			if (Language.availableLanguages.Count > 0)
			{
				Language.DoSwitch(Language.GetLanguageEnum(Language.availableLanguages[0]));
				Debug.LogError("Switched to " + Language.currentLanguage + " instead");
			}
			else
			{
				Debug.LogError("Please verify that you have the file: Resources/Languages/" + code + string.Empty);
				Debug.Break();
			}
		}
		return false;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001F4F0 File Offset: 0x0001D6F0
	private static void DoSwitch(LanguageCode newLang)
	{
		PlayerPrefs.GetString("M2H_lastLanguage", newLang + string.Empty);
		Language.currentLanguage = newLang;
		Language.currentEntrySheets = new Dictionary<int, Hashtable>();
		XMLParser xmlparser = new XMLParser();
		int num = 0;
		while (Language.HasLanguageFile(newLang + string.Empty, num))
		{
			Language.currentEntrySheets[num] = new Hashtable();
			Hashtable hashtable = (Hashtable)xmlparser.Parse(Language.GetLanguageFileContents(num));
			ArrayList arrayList = (ArrayList)(((ArrayList)hashtable["entries"])[0] as Hashtable)["entry"];
			foreach (object obj in arrayList)
			{
				Hashtable hashtable2 = (Hashtable)obj;
				string key = (string)hashtable2["@name"];
				string text = (hashtable2["_text"] + string.Empty).Trim();
				//text = text.UnescapeXML();
				Language.currentEntrySheets[num][key] = text;
			}
			num++;
		}
		LocalizedAsset[] array = (LocalizedAsset[])UnityEngine.Object.FindObjectsOfType(typeof(LocalizedAsset));
		foreach (LocalizedAsset localizedAsset in array)
		{
			localizedAsset.LocalizeAsset();
		}
		Language.Instance.LoadCurrentLanguageFonts();
		Language.SendMonoMessage("ChangedLanguage", new object[]
		{
			Language.currentLanguage
		});
		Language.userSelectedLanguage = Language.currentLanguage;
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0001F6C0 File Offset: 0x0001D8C0
	public static UnityEngine.Object GetAsset(string name)
	{
		return Resources.Load(string.Concat(new object[]
		{
			"Languages/Assets/",
			Language.CurrentLanguage(),
			"/",
			name
		}));
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001F6F4 File Offset: 0x0001D8F4
	private static bool HasLanguageFile(string lang, int sheet)
	{
		return (TextAsset)Resources.Load(string.Concat(new object[]
		{
			"Languages/",
			lang,
			"_",
			sheet
		}), typeof(TextAsset)) != null;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0001F744 File Offset: 0x0001D944
	private static string GetLanguageFileContents(int sheet)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(string.Concat(new object[]
		{
			"Languages/",
			Language.currentLanguage,
			"_",
			sheet
		}), typeof(TextAsset));
		return textAsset.text;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0001F7A0 File Offset: 0x0001D9A0
	public static LanguageCode CurrentLanguage()
	{
		return Language.currentLanguage;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001F7A8 File Offset: 0x0001D9A8
	public static string Get(string key, int charsEachLine = 60)
	{
		if (Language.currentLanguage == LanguageCode.AR || Language.currentLanguage == LanguageCode.FA)
		{
			return Arabic.Parse(Language.Get(0, key), charsEachLine);
		}
		return Language.Get(0, key);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001F7E4 File Offset: 0x0001D9E4
	public static string Get(int sheet, string key)
	{
		if (Language.currentEntrySheets == null || !Language.currentEntrySheets.ContainsKey(sheet))
		{
			return string.Empty;
		}
		if (Language.currentEntrySheets[sheet].ContainsKey(key))
		{
			return (string)Language.currentEntrySheets[sheet][key];
		}
		return "MISSING LANG:" + key;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0001F84C File Offset: 0x0001DA4C
	private static void SendMonoMessage(string methodString, params object[] parameters)
	{
		if (parameters != null && parameters.Length > 1)
		{
			Debug.LogError("We cannot pass more than one argument currently!");
		}
		GameObject[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject gameObject in array)
		{
			if (gameObject && gameObject.transform.parent == null)
			{
				if (parameters != null && parameters.Length == 1)
				{
					gameObject.gameObject.BroadcastMessage(methodString, parameters[0], SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					gameObject.gameObject.BroadcastMessage(methodString, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0001F8F4 File Offset: 0x0001DAF4
	public void OnDestroy()
	{
		currentFont14 = null;
		currentFont18 = null;
		currentFont29 = null;
		Language.Instance = null;
		Language.availableLanguages = null;
		Language.currentEntrySheets.Clear();
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0001F924 File Offset: 0x0001DB24
	public static Font GetFont14()
	{
		if (Language.Instance == null)
		{
			return null;
		}
		return Language.Instance.currentFont14;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0001F944 File Offset: 0x0001DB44
	public static Font GetFont18()
	{
		if (Language.Instance == null)
		{
			return null;
		}
		if (Language.Instance.currentFont18 != null)
		{
			return Language.Instance.currentFont18;
		}
		return Language.Instance.currentFont14;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001F990 File Offset: 0x0001DB90
	public static Font GetFont29()
	{
		if (Language.Instance == null)
		{
			return null;
		}
		if (Language.Instance.currentFont29 != null)
		{
			return Language.Instance.currentFont29;
		}
		if (Language.Instance.currentFont18 != null)
		{
			return Language.Instance.currentFont18;
		}
		return Language.Instance.currentFont14;
	}

	// Token: 0x040004A7 RID: 1191
	private static List<string> availableLanguages;

	// Token: 0x040004A8 RID: 1192
	private static LanguageCode currentLanguage;

	// Token: 0x040004A9 RID: 1193
	private static LanguageCode userSelectedLanguage = LanguageCode.EN;

	// Token: 0x040004AA RID: 1194
	private static Dictionary<int, Hashtable> currentEntrySheets;

	// Token: 0x040004AB RID: 1195
	public static Language Instance;

	// Token: 0x040004AC RID: 1196
	private Font currentFont14;

	// Token: 0x040004AD RID: 1197
	private Font currentFont18;

	// Token: 0x040004AE RID: 1198
	private Font currentFont29;
}
