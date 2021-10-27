using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class EnableGrenade : MonoBehaviour
{
	// Token: 0x06000730 RID: 1840 RVA: 0x0003921C File Offset: 0x0003741C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.played)
		{
			MobileInput.grenadeDisabled = false;
			GunManager.grenadeDisabled = false;
			this.played = true;
		}
	}

	// Token: 0x0400090C RID: 2316
	private bool played;
}
