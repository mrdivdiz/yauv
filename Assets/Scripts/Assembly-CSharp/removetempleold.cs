using System;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class removetempleold : MonoBehaviour
{
	// Token: 0x060008DA RID: 2266 RVA: 0x000495A4 File Offset: 0x000477A4
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			UnityEngine.Object.Destroy(this.Fountain);
			UnityEngine.Object.Destroy(this.Environment);
			Resources.UnloadUnusedAssets();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000BC8 RID: 3016
	public GameObject Fountain;

	// Token: 0x04000BC9 RID: 3017
	public GameObject Environment;
}
