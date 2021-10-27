using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class loadchase2 : MonoBehaviour
{
	// Token: 0x060008C8 RID: 2248 RVA: 0x0004941C File Offset: 0x0004761C
	private void Start()
	{
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00049420 File Offset: 0x00047620
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
		Application.LoadLevel("PreLoadingChase2");
	}
}
