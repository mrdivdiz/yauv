using System;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class RagdollizeHands : MonoBehaviour
{
	// Token: 0x06000813 RID: 2067 RVA: 0x0004209C File Offset: 0x0004029C
	private void Start()
	{
		base.Invoke("Ragdollize", 2f);
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x000420B0 File Offset: 0x000402B0
	private void Ragdollize()
	{
		base.GetComponent<Collider>().enabled = true;
	}
}
