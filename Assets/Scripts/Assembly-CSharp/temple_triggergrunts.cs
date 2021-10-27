using System;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class temple_triggergrunts : MonoBehaviour
{
	// Token: 0x060008F8 RID: 2296 RVA: 0x00049C4C File Offset: 0x00047E4C
	private void PerformAction()
	{
		if (!this.grunted && ((!mainmenu.replayLevel && SaveHandler.levelReached == 2 && SaveHandler.checkpointReached >= 7) || (mainmenu.replayLevel && SaveHandler.replayCheckpointReached >= 7)))
		{
			this.audio8.SetActive(true);
			this.audio9.SetActive(true);
			this.audio10.SetActive(true);
			this.audio11.SetActive(true);
			this.audio12.SetActive(true);
			this.PendulumShooting.SetActive(true);
			this.DropTorch.SetActive(true);
			this.trouble.SetActive(true);
			this.cover.SetActive(true);
			this.endLevel.SetActive(true);
			this.grunted = true;
		}
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00049D18 File Offset: 0x00047F18
	private void OnTriggerEnter()
	{
		this.PerformAction();
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00049D20 File Offset: 0x00047F20
	public void OnCheckpointLoad(int checkpointReached)
	{
		if (checkpointReached >= 7)
		{
			this.PerformAction();
		}
	}

	// Token: 0x04000BE4 RID: 3044
	public GameObject PendulumShooting;

	// Token: 0x04000BE5 RID: 3045
	public GameObject DropTorch;

	// Token: 0x04000BE6 RID: 3046
	public GameObject trouble;

	// Token: 0x04000BE7 RID: 3047
	public GameObject cover;

	// Token: 0x04000BE8 RID: 3048
	public GameObject audio8;

	// Token: 0x04000BE9 RID: 3049
	public GameObject audio9;

	// Token: 0x04000BEA RID: 3050
	public GameObject audio10;

	// Token: 0x04000BEB RID: 3051
	public GameObject audio11;

	// Token: 0x04000BEC RID: 3052
	public GameObject audio12;

	// Token: 0x04000BED RID: 3053
	public GameObject endLevel;

	// Token: 0x04000BEE RID: 3054
	private bool grunted;
}
