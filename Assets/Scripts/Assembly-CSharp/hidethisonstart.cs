using System;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class hidethisonstart : MonoBehaviour
{
	// Token: 0x060008C5 RID: 2245 RVA: 0x00049400 File Offset: 0x00047600
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00049410 File Offset: 0x00047610
	private void Update()
	{
	}
}
