using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class deletecrowds : MonoBehaviour
{
	// Token: 0x060008C0 RID: 2240 RVA: 0x00049394 File Offset: 0x00047594
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.played)
		{
			UnityEngine.Object.Destroy(this.Crowds);
			Resources.UnloadUnusedAssets();
			this.played = true;
		}
	}

	// Token: 0x04000BC1 RID: 3009
	private bool played;

	// Token: 0x04000BC2 RID: 3010
	public GameObject Crowds;
}
