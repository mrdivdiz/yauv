using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
public class DystroyAfterTime : MonoBehaviour
{
	// Token: 0x06000B95 RID: 2965 RVA: 0x000922C0 File Offset: 0x000904C0
	private void Awake()
	{
		base.Invoke("DystroyObject", this.time);
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x000922D4 File Offset: 0x000904D4
	private void DystroyObject()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001520 RID: 5408
	public float time = 8f;
}
