using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[Serializable]
public class Tangier_Deactivate2 : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x0000361C File Offset: 0x0000181C
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			UnityEngine.Object.Destroy(this.Block2);
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000364C File Offset: 0x0000184C
	public virtual void Main()
	{
	}

	// Token: 0x04000042 RID: 66
	public GameObject Block2;
}
