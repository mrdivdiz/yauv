using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class Temple_LayerCulling : MonoBehaviour
{
	// Token: 0x0600063B RID: 1595 RVA: 0x0002EE38 File Offset: 0x0002D038
	private void Start()
	{
		this.distances[14] = 10f;
		this.distances[28] = 20f;
		this.distances[29] = 40f;
		this.distances[15] = 30f;
		this.distances[16] = 80f;
		this.distances[4] = 50f;
		this.distances[17] = 90f;
		this.distances[18] = 100f;
		base.GetComponent<Camera>().layerCullDistances = this.distances;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0002EEC8 File Offset: 0x0002D0C8
	private void Update()
	{
	}

	// Token: 0x0400074A RID: 1866
	private float[] distances = new float[32];
}
