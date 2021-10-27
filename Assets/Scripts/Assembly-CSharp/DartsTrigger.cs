using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class DartsTrigger : MonoBehaviour
{
	// Token: 0x0600070F RID: 1807 RVA: 0x00038CC0 File Offset: 0x00036EC0
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			Darts.on = true;
		}
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x00038CE0 File Offset: 0x00036EE0
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			Darts.on = false;
		}
	}
}
