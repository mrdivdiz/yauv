using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class loadmain : MonoBehaviour
{
	// Token: 0x060008CB RID: 2251 RVA: 0x0004947C File Offset: 0x0004767C
	private void Start()
	{
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00049480 File Offset: 0x00047680
	private void Update()
	{
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00049484 File Offset: 0x00047684
	public void loadlevel()
	{
		this.Black.SetActive(true);
		RenderTexture.active = null;
		if (!mainmenu.replayLevel)
		{
			SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
		}
		else
		{
			SaveHandler.ResetReplayLevelValues();
		}
		AchievementsManager.ReportAchievement(0);
		UnityEngine.Object.Destroy(this.AntaresObject1);
		UnityEngine.Object.Destroy(this.AntaresObject2);
		CSComponent.Localization = null;
		Application.LoadLevel("PreLoadingTemple_Intro");
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0004950C File Offset: 0x0004770C
	private void OnTriggerEnter()
	{
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00049510 File Offset: 0x00047710
	private void OnDestroy()
	{
	}

	// Token: 0x04000BC3 RID: 3011
	public GameObject cam;

	// Token: 0x04000BC4 RID: 3012
	public GameObject player;

	// Token: 0x04000BC5 RID: 3013
	public GameObject AntaresObject1;

	// Token: 0x04000BC6 RID: 3014
	public GameObject AntaresObject2;

	// Token: 0x04000BC7 RID: 3015
	public GameObject Black;
}
