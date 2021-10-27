using System;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class TangierStopCutscene : MonoBehaviour
{
	// Token: 0x06000870 RID: 2160 RVA: 0x000460D4 File Offset: 0x000442D4
	private void Start()
	{
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x000460D8 File Offset: 0x000442D8
	private void StopCutscene()
	{
		this.Rasheed.SetActive(true);
		this.Dania.SetActive(true);
		CutsceneManager.ExitCutscene(false);
	}

	// Token: 0x04000B3B RID: 2875
	public GameObject Rasheed;

	// Token: 0x04000B3C RID: 2876
	public GameObject Dania;
}
