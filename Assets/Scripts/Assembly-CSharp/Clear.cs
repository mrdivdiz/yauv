using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class Clear : MonoBehaviour
{
	// Token: 0x06000500 RID: 1280 RVA: 0x000310B8 File Offset: 0x0002F2B8
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			UnityEngine.Object.Destroy(this.spwn);
			Resources.UnloadUnusedAssets();
		}
	}

	// Token: 0x040007AB RID: 1963
	public GameObject spwn;
}
