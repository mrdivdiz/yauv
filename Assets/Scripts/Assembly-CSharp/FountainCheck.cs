using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class FountainCheck : MonoBehaviour
{
	// Token: 0x0600059E RID: 1438 RVA: 0x000362CC File Offset: 0x000344CC
	private void FixedUpdate()
	{
		if (SaveHandler.checkpointReached > 0)
		{
			this.w1.active = false;
			this.w2.active = false;
			this.w3.active = false;
			this.w4.active = false;
			this.w5.active = false;
			this.w6.active = false;
			this.w7.active = false;
			this.w8.active = false;
			this.w9.active = false;
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00036350 File Offset: 0x00034550
	private void Update()
	{
	}

	// Token: 0x04000897 RID: 2199
	public GameObject w1;

	// Token: 0x04000898 RID: 2200
	public GameObject w2;

	// Token: 0x04000899 RID: 2201
	public GameObject w3;

	// Token: 0x0400089A RID: 2202
	public GameObject w4;

	// Token: 0x0400089B RID: 2203
	public GameObject w5;

	// Token: 0x0400089C RID: 2204
	public GameObject w6;

	// Token: 0x0400089D RID: 2205
	public GameObject w7;

	// Token: 0x0400089E RID: 2206
	public GameObject w8;

	// Token: 0x0400089F RID: 2207
	public GameObject w9;
}
