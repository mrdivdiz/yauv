using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class PreventExitingCoverTrigger : MonoBehaviour
{
	// Token: 0x06000804 RID: 2052 RVA: 0x0004150C File Offset: 0x0003F70C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			Interaction.preventExitingCover = this.preventExitingCover;
		}
	}

	// Token: 0x04000A8A RID: 2698
	public bool preventExitingCover;
}
