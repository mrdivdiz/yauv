using System;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class ForcedInstruction : MonoBehaviour
{
	// Token: 0x06000982 RID: 2434 RVA: 0x00055494 File Offset: 0x00053694
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			Instructions.instruction = this.instruction;
		}
	}

	// Token: 0x04000DA4 RID: 3492
	public Instructions.Instruction instruction;
}
