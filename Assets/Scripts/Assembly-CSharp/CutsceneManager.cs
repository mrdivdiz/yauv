using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class CutsceneManager : MonoBehaviour, ICutsceneLocalization, ICutsceneCustomDraw
{
	// Token: 0x060006E1 RID: 1761 RVA: 0x00037794 File Offset: 0x00035994
	public void Awake()
	{
		CutsceneManager.Instance = this;
		CSComponent.Localization = this;
		CSComponent.CustomDrawer = this;
		CSComponent.AudioVolume.musicVolume = SpeechManager.musicVolume;
		CSComponent.AudioVolume.sfxVolume = SpeechManager.sfxVolume;
		CSComponent.AudioVolume.speechVolume = SpeechManager.speechVolume;
		CutsceneManager.showGUI = true;
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x000377E8 File Offset: 0x000359E8
	public void Start()
	{
		this.style = new GUIStyle();
		this.style.font = Language.GetFont29();
		this.style.alignment = TextAnchor.LowerCenter;
		this.style.wordWrap = true;
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00037820 File Offset: 0x00035A20
	public void ChangedLanguage()
	{
		this.style = new GUIStyle();
		this.style.font = Language.GetFont29();
		this.style.alignment = TextAnchor.LowerCenter;
		this.style.wordWrap = true;
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00037858 File Offset: 0x00035A58
	public void Update()
	{
		if (this.showMeleeInstructions && Application.loadedLevelName == "Prologue" && (InputManager.GetButtonDown("Jump") || !mainmenu.showMeleeInstructions))
		{
			mainmenu.showMeleeInstructions = false;
			mainmenu.Instance.enabled = false;
			this.showMeleeInstructions = false;
			Time.timeScale = 1f;
			if (FightingControl.meleeJoystickLeft != null)
			{
				FightingControl.meleeJoystickLeft.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x000378E0 File Offset: 0x00035AE0
	private void OnDestroy()
	{
		CutsceneManager.Instance = null;
		CSComponent.Localization = null;
		CSComponent.CustomDrawer = null;
		this.style.font = null;
		this.style = null;
		CutsceneManager.playerGameObject = null;
		CutsceneManager.mainCamera = null;
		CutsceneManager.mobileControls = null;
		CutsceneManager.currentCutscene = null;
		CutsceneManager.currentCutsceneObjects = null;
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00037930 File Offset: 0x00035B30
	public static void PlayCutscene(CSComponent cutscene, GameObject cutsceneObjects)
	{
		CutsceneManager.DisablePlayer();
		if (cutsceneObjects != null)
		{
			cutsceneObjects.SetActive(true);
		}
		cutscene.StartCutscene();
		CutsceneManager.currentCutscene = cutscene;
		CutsceneManager.currentCutsceneObjects = cutsceneObjects;
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00037968 File Offset: 0x00035B68
	public static void ExitCutscene(bool playMeleeEncounter)
	{
		if (CutsceneManager.currentCutscene != null)
		{
			UnityEngine.Object.Destroy(CutsceneManager.currentCutscene.gameObject);
		}
		if (CutsceneManager.currentCutsceneObjects != null)
		{
			UnityEngine.Object.Destroy(CutsceneManager.currentCutsceneObjects);
		}
		GameObject gameObject = GameObject.Find("_Cinematic Objects");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
			gameObject.SetActive(true);
		}
		if (playMeleeEncounter)
		{
			CutsceneManager.PlayMeleeEncounter();
		}
		else
		{
			CutsceneManager.EnablePlayer();
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x000379E8 File Offset: 0x00035BE8
	public static void PlayMeleeEncounter(GameObject encounterObjetcts)
	{
		CutsceneManager.DisablePlayer();
		encounterObjetcts.SetActive(true);
		if (encounterObjetcts.transform.childCount > 0)
		{
			encounterObjetcts.transform.GetChild(0).gameObject.SetActive(true);
		}
		if (CutsceneManager.Instance != null && Application.loadedLevelName == "Prologue")
		{
			CutsceneManager.Instance.Invoke("ShowMeleeInstructions", 0.5f);
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00037A64 File Offset: 0x00035C64
	public static void PlayMeleeEncounter()
	{
		GameObject encounterObjetcts = GameObject.Find("_EncounterParent");
		CutsceneManager.PlayMeleeEncounter(encounterObjetcts);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00037A84 File Offset: 0x00035C84
	public void ShowMeleeInstructions()
	{
		mainmenu.showMeleeInstructions = true;
		mainmenu.Instance.enabled = true;
		this.showMeleeInstructions = true;
		Time.timeScale = 1E-05f;
		if (FightingControl.meleeJoystickLeft != null)
		{
			FightingControl.meleeJoystickLeft.gameObject.SetActive(false);
		}
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00037AD4 File Offset: 0x00035CD4
	public static void ExitMeleeEncounter(GameObject encounterObjetcts)
	{
		if (Camera.main != null && Camera.main.transform != null)
		{
			Camera.main.transform.parent = null;
		}
		else if (CutsceneManager.disabledCamera != null)
		{
			CutsceneManager.disabledCamera.transform.parent = null;
		}
		UnityEngine.Object.Destroy(encounterObjetcts);
		CutsceneManager.EnablePlayer();
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00037B48 File Offset: 0x00035D48
	public static void ExitMeleeEncounter()
	{
		if (FightingControl.normalMobileControls != null)
		{
			FightingControl.normalMobileControls.SetActive(true);
		}
		GameObject encounterObjetcts = GameObject.Find("_EncounterParent");
		GameObject gameObject = GameObject.Find("_AfterMelee");
		if (CutsceneManager.playerGameObject != null && gameObject != null)
		{
			CutsceneManager.playerGameObject.transform.position = gameObject.transform.position;
			CutsceneManager.playerGameObject.transform.rotation = gameObject.transform.rotation;
		}
		CutsceneManager.ExitMeleeEncounter(encounterObjetcts);
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00037BDC File Offset: 0x00035DDC
	private static void DisablePlayer()
	{
		ShooterGameCamera component = Camera.main.GetComponent<ShooterGameCamera>();
		if (component != null)
		{
			component.enabled = false;
			if (Application.loadedLevelName != "Prologue" && Application.loadedLevelName != "part1")
			{
				component.GetComponent<Camera>().enabled = false;
				CutsceneManager.disabledCamera = component.GetComponent<Camera>();
			}
			component.aim = false;
			component.reticle = null;
		}
		if (CutsceneManager.playerGameObject == null)
		{
			CutsceneManager.playerGameObject = GameObject.FindGameObjectWithTag("Player");
		}
		Interaction component2 = CutsceneManager.playerGameObject.GetComponent<Interaction>();
		if (component2 != null && component2.coverTallMode)
		{
			component2.cover.inside = false;
			component2.ExitCoverTallMode();
		}
		if (CutsceneManager.playerGameObject != null)
		{
			CutsceneManager.playerGameObject.SetActive(false);
		}
		WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
		if (weaponsHUD != null)
		{
			weaponsHUD.enabled = false;
		}
		CutsceneManager.showGUI = false;
		if (CutsceneManager.mobileControls != null)
		{
			CutsceneManager.mobileControls.SetActive(false);
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00037D0C File Offset: 0x00035F0C
	private static void EnablePlayer()
	{
		if (CutsceneManager.playerGameObject != null)
		{
			CutsceneManager.playerGameObject.SetActive(true);
		}
		if (CutsceneManager.mainCamera != null)
		{
			CutsceneManager.mainCamera.gameObject.SetActive(true);
		}
		if (CutsceneManager.disabledCamera != null)
		{
			CutsceneManager.disabledCamera.enabled = true;
			CutsceneManager.disabledCamera = null;
		}
		ShooterGameCamera shooterGameCamera = null;
		if (Camera.main != null)
		{
			shooterGameCamera = Camera.main.GetComponent<ShooterGameCamera>();
		}
		if (shooterGameCamera != null)
		{
			shooterGameCamera.resetCameraPosition();
			shooterGameCamera.enabled = true;
		}
		WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
		if (weaponsHUD != null)
		{
			weaponsHUD.enabled = true;
		}
		CutsceneManager.showGUI = true;
		if (CutsceneManager.mobileControls != null)
		{
			CutsceneManager.mobileControls.SetActive(true);
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00037DF4 File Offset: 0x00035FF4
	public string GetCurrentLanguage()
	{
		return SpeechManager.currentVoiceLanguage.ToString();
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00037E08 File Offset: 0x00036008
	public Font GetCurrentLanguageFont()
	{
		return SpeechManager.getCurrentLanguageFont();
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00037E10 File Offset: 0x00036010
	public string GetText(string keyword)
	{
		return Language.Get(keyword, 60);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00037E1C File Offset: 0x0003601C
	public void DrawSubtitles(SubtitlesDrawInfo drawInfo)
	{
		if (mainmenu.pause)
		{
			return;
		}
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		if (drawInfo.text != null && drawInfo.text != string.Empty)
		{
			string text = drawInfo.text;
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(51f, 407f, 1266f, 300f), text, this.style);
			GUI.Label(new Rect(51f, 409f, 1266f, 300f), text, this.style);
			GUI.Label(new Rect(49f, 407f, 1266f, 300f), text, this.style);
			GUI.Label(new Rect(49f, 409f, 1266f, 300f), text, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(50f, 408f, 1266f, 300f), text, this.style);
		}
	}

	// Token: 0x040008B8 RID: 2232
	private static GameObject playerGameObject;

	// Token: 0x040008B9 RID: 2233
	private static Camera mainCamera;

	// Token: 0x040008BA RID: 2234
	public static GameObject mobileControls;

	// Token: 0x040008BB RID: 2235
	public static bool showGUI = true;

	// Token: 0x040008BC RID: 2236
	private static CSComponent currentCutscene;

	// Token: 0x040008BD RID: 2237
	private static GameObject currentCutsceneObjects;

	// Token: 0x040008BE RID: 2238
	private bool showMeleeInstructions;

	// Token: 0x040008BF RID: 2239
	private static CutsceneManager Instance;

	// Token: 0x040008C0 RID: 2240
	private static Camera disabledCamera;

	// Token: 0x040008C1 RID: 2241
	private GUIStyle style;
}
