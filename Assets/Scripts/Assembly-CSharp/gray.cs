using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class gray : MonoBehaviour
{
	// Token: 0x06000474 RID: 1140 RVA: 0x000291C8 File Offset: 0x000273C8
	private void Start()
	{
		this.grayscale = base.gameObject.GetComponent<CC_Grayscale>();
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x000291DC File Offset: 0x000273DC
	private void Update()
	{
		this.grayscale.amount = this.Main.amount;
	}

	// Token: 0x04000670 RID: 1648
	private CC_Grayscale grayscale;

	// Token: 0x04000671 RID: 1649
	public CC_Grayscale Main;
}
