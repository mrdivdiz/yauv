using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class water_Type : MonoBehaviour
{
	// Token: 0x06000653 RID: 1619 RVA: 0x0002FDB4 File Offset: 0x0002DFB4
	private void Start()
	{
		base.gameObject.GetComponent<Renderer>().material = this.MobileWaterMat;
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0002FDCC File Offset: 0x0002DFCC
	private void Update()
	{
	}

	// Token: 0x04000779 RID: 1913
	public Material MobileWaterMat;

	// Token: 0x0400077A RID: 1914
	public Material PCWaterMat;
}
