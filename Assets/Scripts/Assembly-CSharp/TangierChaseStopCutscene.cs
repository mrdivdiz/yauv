using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class TangierChaseStopCutscene : MonoBehaviour
{
	// Token: 0x0600086D RID: 2157 RVA: 0x000460A8 File Offset: 0x000442A8
	private void Start()
	{
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x000460AC File Offset: 0x000442AC
	private void StopCutscene()
	{
		this.Thief.SetActive(true);
		Timer.cutsceneFinishTime = Time.timeSinceLevelLoad;
		CutsceneManager.ExitCutscene(false);
	}

	// Token: 0x04000B39 RID: 2873
	public GameObject Thief;

	// Token: 0x04000B3A RID: 2874
	public GameObject Dania;
}
