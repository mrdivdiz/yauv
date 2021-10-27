using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class Clear_Memory : MonoBehaviour
{
	// Token: 0x060006D5 RID: 1749 RVA: 0x000372D0 File Offset: 0x000354D0
	private void OnTriggerEnter()
	{
		UnityEngine.Object.Destroy(this.cutscene);
		UnityEngine.Object.Destroy(this.objects1);
		UnityEngine.Object.Destroy(this.objects2);
		UnityEngine.Object.Destroy(this.objects4);
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0003730C File Offset: 0x0003550C
	private void Update()
	{
	}

	// Token: 0x040008A9 RID: 2217
	public GameObject cutscene;

	// Token: 0x040008AA RID: 2218
	public GameObject objects1;

	// Token: 0x040008AB RID: 2219
	public GameObject objects2;

	// Token: 0x040008AC RID: 2220
	public GameObject objects4;
}
