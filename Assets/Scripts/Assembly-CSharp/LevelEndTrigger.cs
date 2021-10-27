using System;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class LevelEndTrigger : MonoBehaviour
{
	// Token: 0x060007C6 RID: 1990 RVA: 0x000407FC File Offset: 0x0003E9FC
	private void Start()
	{
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00040800 File Offset: 0x0003EA00
	private void Update()
	{
		if (!this.faded && this.startTime != 0f)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
		}
		if (this.faded && Time.time > this.startTime + 3f && (SpeechManager.instance == null || !SpeechManager.instance.playing))
		{
			if (!mainmenu.replayLevel)
			{
				SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
				//GA.API.Design.NewEvent("FinishedLevel:" + Application.loadedLevelName);
			}
			else
			{
				SaveHandler.ResetReplayLevelValues();
			}
			string loadedLevelName = Application.loadedLevelName;
			switch (loadedLevelName)
			{
			case "part2":
				AchievementsManager.ReportAchievement(1);
				break;
			case "QuadChase":
				AchievementsManager.ReportAchievement(2);
				break;
			case "TangierMarket-Block5":
				AchievementsManager.ReportAchievement(3);
				break;
			case "Chase2":
				AchievementsManager.ReportAchievement(4);
				break;
			}
			Application.LoadLevel(this.nextLevelName);
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00040998 File Offset: 0x0003EB98
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" || (collisionInfo.tag == "Bike" && this.nextLevelName != string.Empty && !this.played))
		{
			this.startTime = Time.time;
			this.played = true;
		}
	}

	// Token: 0x04000A56 RID: 2646
	public string nextLevelName;

	// Token: 0x04000A57 RID: 2647
	public string tempLevelName;

	// Token: 0x04000A58 RID: 2648
	private bool played;

	// Token: 0x04000A59 RID: 2649
	private float startTime;

	// Token: 0x04000A5A RID: 2650
	private bool faded;
}
