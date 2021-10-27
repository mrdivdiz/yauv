using System;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class ForceWalkZone : MonoBehaviour
{
	// Token: 0x0600097F RID: 2431 RVA: 0x00055444 File Offset: 0x00053644
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlatformCharacterController>().forceWalk = true;
		}
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00055468 File Offset: 0x00053668
	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlatformCharacterController>().forceWalk = false;
		}
	}
}
