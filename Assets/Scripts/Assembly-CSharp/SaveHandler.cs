using System;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class SaveHandler : MonoBehaviour
{
	// Token: 0x0600082F RID: 2095 RVA: 0x00042A58 File Offset: 0x00040C58
	public void Awake()
	{
		SaveHandler.Instance = this;
		SaveHandler.purchased = true;
		
	}
	void Start(){
		
		if(PlayerPrefs.GetInt("levelReached") == 0){
			Save();
		}
		Load();
		Debug.Log("Started");
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00042A74 File Offset: 0x00040C74
	private void OnDestroy()
	{
		SaveHandler.Instance = null;
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00042A7C File Offset: 0x00040C7C
	public static void SaveCheckpoint(int level, int checkpoint, Vector3 playerPosition, Vector3 playerRotation, string currentSecondaryWeaponName, string currentPrimaryWeaponName, int currSecondaryClips, int currSecondaryBullets, int currPrimaryClips, int currPrimaryBullets, int currGrenades)
	{
		SaveHandler.levelReached = level;
		SaveHandler.checkpointReached = checkpoint;
		SaveHandler.savedPlayerPosition = playerPosition;
		SaveHandler.savedPlayerRotation = playerRotation;
		SaveHandler.currentSecondaryWeaponName = currentSecondaryWeaponName;
		SaveHandler.currentPrimaryWeaponName = currentPrimaryWeaponName;
		SaveHandler.currSecondaryClips = currSecondaryClips;
		SaveHandler.currSecondaryBullets = currSecondaryBullets;
		SaveHandler.currPrimaryClips = currPrimaryClips;
		SaveHandler.currPrimaryBullets = currPrimaryBullets;
		SaveHandler.currGrenades = currGrenades;
		Debug.Log("Checkpoint save!");
			SaveHandler.Save();
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00042AD8 File Offset: 0x00040CD8
	public static void Save()
	{
		PlayerPrefs.SetInt("levelReached", SaveHandler.levelReached);
		PlayerPrefs.SetInt("checkpointReached", SaveHandler.checkpointReached);
		PlayerPrefs.SetFloat("savedPlayerPositionX", SaveHandler.savedPlayerPosition.x);
		PlayerPrefs.SetFloat("savedPlayerPositionY", SaveHandler.savedPlayerPosition.y);
		PlayerPrefs.SetFloat("savedPlayerPositionZ", SaveHandler.savedPlayerPosition.z);
		PlayerPrefs.SetFloat("savedPlayerRotationX", SaveHandler.savedPlayerRotation.x);
		PlayerPrefs.SetFloat("savedPlayerRotationY", SaveHandler.savedPlayerRotation.y);
		PlayerPrefs.SetFloat("savedPlayerRotationZ", SaveHandler.savedPlayerRotation.z);
		PlayerPrefs.SetInt("treasures", SaveHandler.treasures);
		PlayerPrefs.SetInt("currentDifficultyLevel", (int)SaveHandler.currentDifficultyLevel);
		PlayerPrefs.SetInt("gameFinished", SaveHandler.gameFinished);
		PlayerPrefs.SetString("currentSecondaryWeaponName", SaveHandler.currentSecondaryWeaponName);
		PlayerPrefs.SetString("currentPrimaryWeaponName", SaveHandler.currentPrimaryWeaponName);
		PlayerPrefs.SetInt("currSecondaryClips", SaveHandler.currSecondaryClips);
		PlayerPrefs.SetInt("currSecondaryBullets", SaveHandler.currSecondaryBullets);
		PlayerPrefs.SetInt("currPrimaryClips", SaveHandler.currPrimaryClips);
		PlayerPrefs.SetInt("currPrimaryBullets", SaveHandler.currPrimaryBullets);
		PlayerPrefs.SetInt("currGrenades", SaveHandler.currGrenades);
		PlayerPrefs.Save();
		Debug.Log("SEIVD");
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00042C14 File Offset: 0x00040E14
	public static void SaveCheckpointOnReplay(int checkpoint, Vector3 playerPosition, Vector3 playerRotation, string currentSecondaryWeaponName, string currentPrimaryWeaponName, int currSecondaryClips, int currSecondaryBullets, int currPrimaryClips, int currPrimaryBullets, int currGrenades)
	{
		SaveHandler.replayCheckpointReached = checkpoint;
		SaveHandler.replaySavedPlayerPosition = playerPosition;
		SaveHandler.replaySavedPlayerRotation = playerRotation;
		SaveHandler.replayCurrentSecondaryWeaponName = currentSecondaryWeaponName;
		SaveHandler.replayCurrentPrimaryWeaponName = currentPrimaryWeaponName;
		SaveHandler.replayCurrSecondaryClips = currSecondaryClips;
		SaveHandler.replayCurrSecondaryBullets = currSecondaryBullets;
		SaveHandler.replayCurrPrimaryClips = currPrimaryClips;
		SaveHandler.replayCurrPrimaryBullets = currPrimaryBullets;
		SaveHandler.replayCurrGrenades = currGrenades;
		if (SaveHandler.Instance != null)
		{
			SaveHandler.Instance.saveIconTimer = 5f;
		}
		PlayerPrefs.Save();
		Debug.Log("SEIVD");
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00042C84 File Offset: 0x00040E84
	public static void ResetReplayLevelValues()
	{
		SaveHandler.replayCheckpointReached = 0;
		SaveHandler.replaySavedPlayerPosition = Vector3.zero;
		SaveHandler.replaySavedPlayerRotation = Vector3.zero;
		Inventory.dagger1 = false;
		Inventory.dagger2 = false;
		Inventory.fuse1 = false;
		Inventory.fuse2 = false;
		Inventory.fuse3 = false;
		Inventory.fuse4 = false;
		Inventory.enableHinting = false;
		Inventory.fourplayed = false;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00042CDC File Offset: 0x00040EDC
	public static void Load()
	{
		SaveHandler.levelReached = PlayerPrefs.GetInt("levelReached", 1);
		SaveHandler.checkpointReached = PlayerPrefs.GetInt("checkpointReached", 0);
		SaveHandler.savedPlayerPosition = new Vector3(PlayerPrefs.GetFloat("savedPlayerPositionX", 0f), PlayerPrefs.GetFloat("savedPlayerPositionY", 0f), PlayerPrefs.GetFloat("savedPlayerPositionZ", 0f));
		SaveHandler.savedPlayerRotation = new Vector3(PlayerPrefs.GetFloat("savedPlayerRotationX", 0f), PlayerPrefs.GetFloat("savedPlayerRotationY", 0f), PlayerPrefs.GetFloat("savedPlayerRotationZ", 0f));
		SaveHandler.treasures = PlayerPrefs.GetInt("treasures", 0);
		SaveHandler.currentSecondaryWeaponName = PlayerPrefs.GetString("currentSecondaryWeaponName", string.Empty);
		SaveHandler.currentPrimaryWeaponName = PlayerPrefs.GetString("currentPrimaryWeaponName", string.Empty);
		SaveHandler.currSecondaryClips = PlayerPrefs.GetInt("currSecondaryClips", 0);
		SaveHandler.currSecondaryBullets = PlayerPrefs.GetInt("currSecondaryBullets", 0);
		SaveHandler.currPrimaryClips = PlayerPrefs.GetInt("currPrimaryClips", 0);
		SaveHandler.currPrimaryBullets = PlayerPrefs.GetInt("currPrimaryBullets", 0);
		SaveHandler.currGrenades = PlayerPrefs.GetInt("currGrenades", 0);
		SaveHandler.currentDifficultyLevel = (DifficultyManager.Difficulty)PlayerPrefs.GetInt("currentDifficultyLevel", (int)SaveHandler.currentDifficultyLevel);
		SaveHandler.gameFinished = PlayerPrefs.GetInt("gameFinished", 0);
		Debug.Log("luaded");
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00042E24 File Offset: 0x00041024
	public static void LoadLevel()
	{
		SaveHandler.Load();
		if (SaveHandler.checkpointReached > 0)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			gameObject.transform.position = SaveHandler.savedPlayerPosition;
			gameObject.transform.rotation = Quaternion.Euler(SaveHandler.savedPlayerRotation);
			ShooterGameCamera component = Camera.main.GetComponent<ShooterGameCamera>();
			if (component != null)
			{
				component.resetCameraPosition();
			}
			GunManager componentInChildren = gameObject.GetComponentInChildren<GunManager>();
			if (componentInChildren != null)
			{
				componentInChildren.LoadGunManager(SaveHandler.currentSecondaryWeaponName, SaveHandler.currentPrimaryWeaponName, SaveHandler.currSecondaryClips, SaveHandler.currSecondaryBullets, SaveHandler.currPrimaryClips, SaveHandler.currPrimaryBullets, SaveHandler.currGrenades);
			}
		}
		SaveHandler.BroadcastAll(SaveHandler.checkpointReached);
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00042ED4 File Offset: 0x000410D4
	public static void LoadLevelOnReplay()
	{
		if (SaveHandler.replayCheckpointReached > 0)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				gameObject.transform.position = SaveHandler.replaySavedPlayerPosition;
				gameObject.transform.rotation = Quaternion.Euler(SaveHandler.replaySavedPlayerRotation);
				ShooterGameCamera shooterGameCamera = null;
				if (Camera.main != null)
				{
					shooterGameCamera = Camera.main.GetComponent<ShooterGameCamera>();
				}
				if (shooterGameCamera != null)
				{
					shooterGameCamera.resetCameraPosition();
				}
				GunManager componentInChildren = gameObject.GetComponentInChildren<GunManager>();
				if (componentInChildren != null && SaveHandler.replayCurrentSecondaryWeaponName != string.Empty)
				{
					componentInChildren.LoadGunManager(SaveHandler.replayCurrentSecondaryWeaponName, SaveHandler.replayCurrentPrimaryWeaponName, SaveHandler.replayCurrSecondaryClips, SaveHandler.replayCurrSecondaryBullets, SaveHandler.replayCurrPrimaryClips, SaveHandler.replayCurrPrimaryBullets, SaveHandler.replayCurrGrenades);
				}
			}
		}
		SaveHandler.BroadcastAll(SaveHandler.replayCheckpointReached);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00042FB4 File Offset: 0x000411B4
	public static void Reset()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00042FBC File Offset: 0x000411BC
	public static void ResetNewGame()
	{
		PlayerPrefs.DeleteKey("levelReached");
		PlayerPrefs.DeleteKey("checkpointReached");
		PlayerPrefs.DeleteKey("savedPlayerPositionX");
		PlayerPrefs.DeleteKey("savedPlayerPositionY");
		PlayerPrefs.DeleteKey("savedPlayerPositionZ");
		PlayerPrefs.DeleteKey("savedPlayerRotationX");
		PlayerPrefs.DeleteKey("savedPlayerRotationY");
		PlayerPrefs.DeleteKey("savedPlayerRotationZ");
		PlayerPrefs.SetInt("currentDifficultyLevel", (int)SaveHandler.currentDifficultyLevel);
		Inventory.dagger1 = false;
		Inventory.dagger2 = false;
		Inventory.fuse1 = false;
		Inventory.fuse2 = false;
		Inventory.fuse3 = false;
		Inventory.fuse4 = false;
		Inventory.enableHinting = false;
		Inventory.fourplayed = false;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00043058 File Offset: 0x00041258
	public static void BroadcastAll(int checkpoint)
	{
		GameObject[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject gameObject in array)
		{
			if (gameObject && gameObject.transform.parent == null)
			{
				gameObject.gameObject.BroadcastMessage("OnCheckpointLoad", checkpoint, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x000430CC File Offset: 0x000412CC
	public static void SaveSettings()
	{
		PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
		PlayerPrefs.SetFloat("MusicVolume", SpeechManager.musicVolume);
		PlayerPrefs.SetFloat("SpeechVolume", SpeechManager.speechVolume);
		PlayerPrefs.SetFloat("SFXVolume", SpeechManager.sfxVolume);
		PlayerPrefs.SetInt("inverseX", (!ShooterGameCamera.inverseX) ? 0 : 1);
		PlayerPrefs.SetInt("inverseY", (!ShooterGameCamera.inverseY) ? 0 : 1);
		PlayerPrefs.SetFloat("mouseSensitivity", ShooterGameCamera.mouseSensitivity);
		PlayerPrefs.SetInt("currentCameraSensitivity", mainmenu.currentCameraSensitivity);
		PlayerPrefs.SetInt("enableSubtitles", (!SpeechManager.enableSubtitles) ? 0 : 1);
		PlayerPrefs.SetInt("AimAssest", (int)ShooterGameCamera.aimAssestType);
		PlayerPrefs.Save();
		Debug.Log("SEIVD");
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00043194 File Offset: 0x00041394
	public static void LoadSettings()
	{
		AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", AudioListener.volume);
		SpeechManager.musicVolume = PlayerPrefs.GetFloat("MusicVolume", SpeechManager.musicVolume);
		SpeechManager.speechVolume = PlayerPrefs.GetFloat("SpeechVolume", SpeechManager.speechVolume);
		SpeechManager.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", SpeechManager.sfxVolume);
		ShooterGameCamera.inverseX = (PlayerPrefs.GetInt("inverseX", (!ShooterGameCamera.inverseX) ? 0 : 1) == 1);
		ShooterGameCamera.inverseY = (PlayerPrefs.GetInt("inverseY", (!ShooterGameCamera.inverseY) ? 0 : 1) == 1);
		ShooterGameCamera.mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", ShooterGameCamera.mouseSensitivity);
		mainmenu.currentCameraSensitivity = PlayerPrefs.GetInt("currentCameraSensitivity", mainmenu.currentCameraSensitivity);
		SpeechManager.enableSubtitles = (PlayerPrefs.GetInt("enableSubtitles", (!SpeechManager.enableSubtitles) ? 0 : 1) == 1);
		ShooterGameCamera.aimAssestType = (ShooterGameCamera.AimAssestTypes)PlayerPrefs.GetInt("AimAssest", (int)ShooterGameCamera.aimAssestType);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x000432B4 File Offset: 0x000414B4
	public static void ReportTreasure(int treasureID)
	{
		SaveHandler.treasures |= 1 << treasureID;
		SaveHandler.Save();
		for (int i = 0; i <= 6; i++)
		{
			if ((SaveHandler.treasures & 1 << i) == 0)
			{
				return;
			}
		}
		AchievementsManager.ReportAchievement(6);
	}

	// Token: 0x04000AC5 RID: 2757
	public static int levelReached = 0;

	// Token: 0x04000AC6 RID: 2758
	public static int checkpointReached;

	// Token: 0x04000AC7 RID: 2759
	public static Vector3 savedPlayerPosition = Vector3.zero;

	// Token: 0x04000AC8 RID: 2760
	public static Vector3 savedPlayerRotation = Vector3.zero;

	// Token: 0x04000AC9 RID: 2761
	public static int replayCheckpointReached;

	// Token: 0x04000ACA RID: 2762
	public static Vector3 replaySavedPlayerPosition = Vector3.zero;

	// Token: 0x04000ACB RID: 2763
	public static Vector3 replaySavedPlayerRotation = Vector3.zero;

	// Token: 0x04000ACC RID: 2764
	public static DifficultyManager.Difficulty currentDifficultyLevel = DifficultyManager.Difficulty.EASY;

	// Token: 0x04000ACD RID: 2765
	public static int gameFinished = 0;

	// Token: 0x04000ACE RID: 2766
	public static int treasures = 0;

	// Token: 0x04000ACF RID: 2767
	private static string currentSecondaryWeaponName;

	// Token: 0x04000AD0 RID: 2768
	private static string currentPrimaryWeaponName;

	// Token: 0x04000AD1 RID: 2769
	private static int currSecondaryClips;

	// Token: 0x04000AD2 RID: 2770
	private static int currSecondaryBullets;

	// Token: 0x04000AD3 RID: 2771
	private static int currPrimaryClips;

	// Token: 0x04000AD4 RID: 2772
	private static int currPrimaryBullets;

	// Token: 0x04000AD5 RID: 2773
	private static int currGrenades;

	// Token: 0x04000AD6 RID: 2774
	private static string replayCurrentSecondaryWeaponName;

	// Token: 0x04000AD7 RID: 2775
	private static string replayCurrentPrimaryWeaponName;

	// Token: 0x04000AD8 RID: 2776
	private static int replayCurrSecondaryClips;

	// Token: 0x04000AD9 RID: 2777
	private static int replayCurrSecondaryBullets;

	// Token: 0x04000ADA RID: 2778
	private static int replayCurrPrimaryClips;

	// Token: 0x04000ADB RID: 2779
	private static int replayCurrPrimaryBullets;

	// Token: 0x04000ADC RID: 2780
	private static int replayCurrGrenades;

	// Token: 0x04000ADD RID: 2781
	private static SaveHandler Instance;

	// Token: 0x04000ADE RID: 2782
	public Texture saveIcon;

	// Token: 0x04000ADF RID: 2783
	private float saveIconTimer;

	// Token: 0x04000AE0 RID: 2784
	public static bool purchased = false;
}
