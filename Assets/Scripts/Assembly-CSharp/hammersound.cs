using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class hammersound : MonoBehaviour
{
	// Token: 0x060005F9 RID: 1529 RVA: 0x0002A198 File Offset: 0x00028398
	private void Start()
	{
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0002A19C File Offset: 0x0002839C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		base.GetComponent<AudioSource>().Play();
	}
}
