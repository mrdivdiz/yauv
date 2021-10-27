using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class Trigger : MonoBehaviour
{
	// Token: 0x06000892 RID: 2194 RVA: 0x00047CC8 File Offset: 0x00045EC8
	private void SignalCutscene()
	{
		this.a.loadlevel();
	}

	// Token: 0x04000B88 RID: 2952
	public loadmain a;
}
