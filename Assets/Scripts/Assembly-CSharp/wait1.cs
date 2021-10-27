using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class wait1 : MonoBehaviour
{
	// Token: 0x06000CE3 RID: 3299 RVA: 0x000A3B80 File Offset: 0x000A1D80
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(15f);
		if (!mainmenu.replayLevel)
		{
			SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
		}
		else
		{
			SaveHandler.ResetReplayLevelValues();
		}
		Application.LoadLevel("Egypt-Cutscene5");
		yield break;
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000A3B94 File Offset: 0x000A1D94
	private void Update()
	{
	}

	// Token: 0x04001689 RID: 5769
	public GameObject m;
}
