using System;
using UnityEngine;
// Token: 0x02000130 RID: 304
public class AchievementsManager : MonoBehaviour
{
	// Token: 0x060006BC RID: 1724 RVA: 0x00036B44 File Offset: 0x00034D44
	public void Awake()
	{
		AchievementsManager.Instance = this;
		if (!AchievementsManager.Initialized && Application.loadedLevelName == "MainMenu")
		{
			AchievementsManager.InitializeAchievementSystem();
		}
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00036B70 File Offset: 0x00034D70
	public void OnDestroy()
	{
		AchievementsManager.Instance = null;
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00036B78 File Offset: 0x00034D78
	public static void InitializeAchievementSystem()
	{
		Debug.Log("Dummy");
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00036BD0 File Offset: 0x00034DD0
	public static void ReportAchievement(int achievementID)
	{
		Debug.Log("Dummy");
	}

	// Token: 0x04000895 RID: 2197
	private static string[] steamAchievmentIDs = new string[]
	{
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7"
	};

	// Token: 0x04000896 RID: 2198
	private static string[] gameCenterAchievmentIDs = new string[]
	{
		"Semphore.Unearthed.Ep1.Ach1",
		"Semphore.Unearthed.Ep1.Ach2",
		"Semphore.Unearthed.Ep1.Ach3",
		"Semphore.Unearthed.Ep1.Ach4",
		"Semphore.Unearthed.Ep1.Ach5",
		"Semphore.Unearthed.Ep1.Ach6",
		"Semphore.Unearthed.Ep1.Ach7"
	};

	// Token: 0x04000897 RID: 2199
	private static int[] PS3AchievmentIDs = new int[]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		6
	};

	// Token: 0x04000898 RID: 2200
	private static int[] XBOXAchievmentIDs = new int[]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		6
	};

	// Token: 0x04000899 RID: 2201
	private static string[] googlePlayAchievmentIDs = new string[]
	{
		"CgkItfLiwroeEAIQAQ",
		"CgkItfLiwroeEAIQAg",
		"CgkItfLiwroeEAIQAw",
		"CgkItfLiwroeEAIQBA",
		"CgkItfLiwroeEAIQBQ",
		"CgkItfLiwroeEAIQBg",
		"CgkItfLiwroeEAIQBw"
	};

	// Token: 0x0400089A RID: 2202
	public static AchievementsManager Instance;

	// Token: 0x0400089B RID: 2203
	public static bool Initialized = false;
}
