using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class prolouge_start : MonoBehaviour
{
	// Token: 0x06000650 RID: 1616 RVA: 0x0002FD54 File Offset: 0x0002DF54
	private void Start()
	{
		if ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0))
		{
			CutsceneManager.PlayCutscene(this.cutscene, this.objects);
			this.cutscene = null;
			this.objects = null;
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0002FDA8 File Offset: 0x0002DFA8
	private void Update()
	{
	}

	// Token: 0x04000777 RID: 1911
	public CSComponent cutscene;

	// Token: 0x04000778 RID: 1912
	public GameObject objects;
}
