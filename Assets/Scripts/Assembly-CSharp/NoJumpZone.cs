using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class NoJumpZone : MonoBehaviour
{
	// Token: 0x060007E8 RID: 2024 RVA: 0x000410A8 File Offset: 0x0003F2A8
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<NormalCharacterMotor>().canJump = false;
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x000410CC File Offset: 0x0003F2CC
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<NormalCharacterMotor>().canJump = true;
		}
	}
}
