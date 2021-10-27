using System;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class ForceRoll : MonoBehaviour
{
	// Token: 0x06000751 RID: 1873 RVA: 0x0003A49C File Offset: 0x0003869C
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			AnimationHandler.forceRoleFromFall = true;
			other.GetComponent<NormalCharacterMotor>().canJump = false;
		}
	}
}
