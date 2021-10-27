using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[RequireComponent(typeof(Renderer))]
[AddComponentMenu("Effects/SetRenderQueue")]
public class SetRenderQueue : MonoBehaviour
{
	// Token: 0x060000F9 RID: 249 RVA: 0x000061C0 File Offset: 0x000043C0
	protected void Start()
	{
		if (!base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || this.queues == null)
		{
			return;
		}
		base.GetComponent<Renderer>().sharedMaterial.renderQueue = this.queue;
		int num = 0;
		while (num < this.queues.Length && num < base.GetComponent<Renderer>().sharedMaterials.Length)
		{
			base.GetComponent<Renderer>().sharedMaterials[num].renderQueue = this.queues[num];
			num++;
		}
	}

	// Token: 0x040000E3 RID: 227
	public int queue = 1;

	// Token: 0x040000E4 RID: 228
	public int[] queues;
}
