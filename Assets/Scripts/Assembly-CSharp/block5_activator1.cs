using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class block5_activator1 : MonoBehaviour
{
	// Token: 0x0600064D RID: 1613 RVA: 0x0002FD14 File Offset: 0x0002DF14
	private void Start()
	{
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0002FD18 File Offset: 0x0002DF18
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			UnityEngine.Object.Destroy(this.RasheedApartment);
			Resources.UnloadUnusedAssets();
		}
	}

	// Token: 0x04000776 RID: 1910
	public GameObject RasheedApartment;
}
