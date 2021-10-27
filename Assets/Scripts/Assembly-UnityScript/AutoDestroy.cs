using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
[Serializable]
public class AutoDestroy : MonoBehaviour
{
	// Token: 0x060000C1 RID: 193 RVA: 0x000041AC File Offset: 0x000023AC
	public virtual void Update()
	{
		this.time -= Time.deltaTime;
		if (this.time < (float)0)
		{
			UnityEngine.Object.Destroy(this.gameObject);
		}
	}

	// Token: 0x0400005E RID: 94
	public float time;
}
