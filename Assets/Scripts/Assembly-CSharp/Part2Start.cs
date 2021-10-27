using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class Part2Start : MonoBehaviour
{
	// Token: 0x060007FF RID: 2047 RVA: 0x000414B0 File Offset: 0x0003F6B0
	private void Start()
	{
		if ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0))
		{
			CutsceneManager.PlayMeleeEncounter(this.encounterObjetcts);
		}
	}

	// Token: 0x04000A89 RID: 2697
	public GameObject encounterObjetcts;
}
