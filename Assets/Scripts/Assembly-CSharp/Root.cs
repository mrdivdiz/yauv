using System;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class Root : MonoBehaviour
{
	// Token: 0x06000B1C RID: 2844 RVA: 0x000880EC File Offset: 0x000862EC
	private void Awake()
	{
		if (Application.isEditor)
		{
			base.gameObject.BroadcastMessage("SetDebug", this.debug);
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00088114 File Offset: 0x00086314
	private void Update()
	{
		if (Application.isEditor)
		{
			base.gameObject.BroadcastMessage("SetDebug", this.debug);
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0008813C File Offset: 0x0008633C
	public float RoundTo(float value, int precision)
	{
		int num = 1;
		for (int i = 1; i <= precision; i++)
		{
			num *= 10;
		}
		return Mathf.Round(value * (float)num) / (float)num;
	}

	// Token: 0x04001353 RID: 4947
	public bool debug;

	// Token: 0x04001354 RID: 4948
	public static float D3_EPSILON = 0.001f;
}
