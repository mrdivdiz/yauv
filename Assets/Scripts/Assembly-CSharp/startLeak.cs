using System;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class startLeak : MonoBehaviour
{
	// Token: 0x060008E3 RID: 2275 RVA: 0x0004966C File Offset: 0x0004786C
	private void Start()
	{
		Resources.UnloadUnusedAssets();
		DontDestroy component = GameObject.Find("_DontDestroy").GetComponent<DontDestroy>();
		component.Start();
	}
}
