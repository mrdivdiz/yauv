using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class NoTakedownZone : MonoBehaviour
{
	// Token: 0x060007EB RID: 2027 RVA: 0x000410F8 File Offset: 0x0003F2F8
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			AnimationHandler.noTakedownZone = true;
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00041118 File Offset: 0x0003F318
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			AnimationHandler.noTakedownZone = false;
		}
	}
}
