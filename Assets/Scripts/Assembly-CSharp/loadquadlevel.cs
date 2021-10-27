using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class loadquadlevel : MonoBehaviour
{
	// Token: 0x060008D1 RID: 2257 RVA: 0x0004951C File Offset: 0x0004771C
	private void Start()
	{
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00049520 File Offset: 0x00047720
	private void loadquadlevelfunc()
	{
		if (!mainmenu.replayLevel)
		{
			SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
		}
		else
		{
			SaveHandler.ResetReplayLevelValues();
		}
		Application.LoadLevel("PreLoadingQuadChase");
	}
}
