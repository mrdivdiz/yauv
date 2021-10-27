using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class LayerCulling : MonoBehaviour
{
	// Token: 0x06000627 RID: 1575 RVA: 0x0002CE0C File Offset: 0x0002B00C
	private void Start()
	{
		float[] array = new float[32];
		array[LayerMask.NameToLayer("Default")] = 180f;
		array[LayerMask.NameToLayer("BigColumns")] = 210f;
		array[LayerMask.NameToLayer("TorchFlames")] = 100f;
		array[LayerMask.NameToLayer("TorchMetal")] = 30f;
		base.GetComponent<Camera>().layerCullDistances = array;
	}
}
