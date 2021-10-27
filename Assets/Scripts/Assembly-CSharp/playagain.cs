using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class playagain : MonoBehaviour
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x00067150 File Offset: 0x00065350
	private void Awake()
	{
		playagain.endreached = true;
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00067158 File Offset: 0x00065358
	private void Update()
	{
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0006715C File Offset: 0x0006535C
	public void again()
	{
		playagain.endreached = false;
		if (!playagain.endreached)
		{
			this.pendulumCutscene.StartCutscene();
		}
	}

	// Token: 0x04000F29 RID: 3881
	public CSComponent pendulumCutscene;

	// Token: 0x04000F2A RID: 3882
	public static bool endreached;
}
