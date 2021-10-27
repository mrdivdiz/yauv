using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class Pot_Breakage : MonoBehaviour
{
	// Token: 0x06000801 RID: 2049 RVA: 0x000414F0 File Offset: 0x0003F6F0
	private void OnCollisionEnter()
	{
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00041500 File Offset: 0x0003F700
	private void Update()
	{
	}
}
