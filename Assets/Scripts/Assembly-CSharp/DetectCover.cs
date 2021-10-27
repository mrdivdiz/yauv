using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class DetectCover : MonoBehaviour
{
	// Token: 0x0600071E RID: 1822 RVA: 0x00038EE4 File Offset: 0x000370E4
	private void OnCoverEnter()
	{
		this.marker.SetActive(false);
		this.coverMoveHint.SetActive(true);
	}

	// Token: 0x040008F4 RID: 2292
	public GameObject marker;

	// Token: 0x040008F5 RID: 2293
	public GameObject coverMoveHint;
}
