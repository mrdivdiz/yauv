using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class Car_LayerCulling : MonoBehaviour
{
	// Token: 0x06000619 RID: 1561 RVA: 0x0002CA94 File Offset: 0x0002AC94
	private void Start()
	{
		this.distances[31] = 40f;
		this.distances[28] = 40f;
		this.distances[29] = 80f;
		this.distances[18] = 100f;
		this.distances[22] = 100f;
		this.distances[17] = 180f;
		base.GetComponent<Camera>().layerCullDistances = this.distances;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0002CB08 File Offset: 0x0002AD08
	private void Update()
	{
	}

	// Token: 0x040006D7 RID: 1751
	private float[] distances = new float[32];
}
