using System;
using UnityEngine;

// Token: 0x02000256 RID: 598
public class WinningCondition : MonoBehaviour
{
	// Token: 0x06000B56 RID: 2902 RVA: 0x0008E8FC File Offset: 0x0008CAFC
	private void Start()
	{
		base.Invoke("DisablePullingPolice", 155f);
		base.Invoke("Win", 182f);
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0008E92C File Offset: 0x0008CB2C
	private void Update()
	{
		if (!this.faded && this.startTime != 0f)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			if (!mainmenu.replayLevel)
			{
				if (SaveHandler.levelReached == 11)
				{
					switch (SaveHandler.currentDifficultyLevel)
					{
					case DifficultyManager.Difficulty.EASY:
						//GA.API.Design.NewEvent("FinishedGame:Easy");
						if (SaveHandler.gameFinished < 1)
						{
							SaveHandler.gameFinished = 1;
						}
						break;
					case DifficultyManager.Difficulty.MEDIUM:
						//GA.API.Design.NewEvent("FinishedGame:Medium");
						if (SaveHandler.gameFinished < 2)
						{
							SaveHandler.gameFinished = 2;
						}
						break;
					case DifficultyManager.Difficulty.HARD:
						//GA.API.Design.NewEvent("FinishedGame:Hard");
						if (SaveHandler.gameFinished < 3)
						{
							SaveHandler.gameFinished = 3;
						}
						break;
					}
					SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
				}
			}
			else
			{
				SaveHandler.ResetReplayLevelValues();
			}
			AchievementsManager.ReportAchievement(5);
			Application.LoadLevel("PreLoadingCar-Outro");
		}
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0008EA88 File Offset: 0x0008CC88
	private void Win()
	{
		this.startTime = Time.time;
		this.played = true;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0008EA9C File Offset: 0x0008CC9C
	private void DisablePullingPolice()
	{
		if (this.pullPoliceCars != null)
		{
			this.pullPoliceCars.enabled = false;
		}
	}

	// Token: 0x04001491 RID: 5265
	private bool played;

	// Token: 0x04001492 RID: 5266
	private float startTime;

	// Token: 0x04001493 RID: 5267
	private bool faded;

	// Token: 0x04001494 RID: 5268
	public PullPoliceCars pullPoliceCars;
}
