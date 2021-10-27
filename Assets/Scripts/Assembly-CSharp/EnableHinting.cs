using System;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class EnableHinting : MonoBehaviour
{
	// Token: 0x06000732 RID: 1842 RVA: 0x0003925C File Offset: 0x0003745C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((collisionInfo.tag == "Player" || collisionInfo.tag == "Bike") && !this.played)
		{
			Inventory.enableHinting = true;
			this.played = true;
		}
	}

	// Token: 0x0400090D RID: 2317
	private bool played;
}
