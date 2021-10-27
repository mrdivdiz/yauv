using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Terrain_Type : MonoBehaviour
{
	// Token: 0x0600063E RID: 1598 RVA: 0x0002EED4 File Offset: 0x0002D0D4
	private void Start()
	{
		base.gameObject.GetComponent<Renderer>().material = this.MobileTerrainMat;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0002EEEC File Offset: 0x0002D0EC
	private void Update()
	{
	}

	// Token: 0x0400074B RID: 1867
	public Material MobileTerrainMat;

	// Token: 0x0400074C RID: 1868
	public Material PCTerrainMat;
}
