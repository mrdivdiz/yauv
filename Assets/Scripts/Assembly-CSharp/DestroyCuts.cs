using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class DestroyCuts : MonoBehaviour
{
	// Token: 0x06000714 RID: 1812 RVA: 0x00038D4C File Offset: 0x00036F4C
	private void OnTriggerEnter()
	{
		UnityEngine.Object.Destroy(this.outro1);
		UnityEngine.Object.Destroy(this.outro2);
		UnityEngine.Object.Destroy(this.outro3);
	}

	// Token: 0x040008EC RID: 2284
	public GameObject outro1;

	// Token: 0x040008ED RID: 2285
	public GameObject outro2;

	// Token: 0x040008EE RID: 2286
	public GameObject outro3;
}
