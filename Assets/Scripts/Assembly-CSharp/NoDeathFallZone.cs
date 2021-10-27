using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class NoDeathFallZone : MonoBehaviour
{
	// Token: 0x060007E5 RID: 2021 RVA: 0x00041060 File Offset: 0x0003F260
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			AnimationHandler.noDeathFallZone = true;
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00041080 File Offset: 0x0003F280
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			AnimationHandler.noDeathFallZone = false;
		}
	}
}
