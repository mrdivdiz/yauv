using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[Serializable]
public class Tangier_Deactivate3 : MonoBehaviour
{
	// Token: 0x06000070 RID: 112 RVA: 0x00003658 File Offset: 0x00001858
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			UnityEngine.Object.Destroy(this.Block3);
		}
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000368C File Offset: 0x0000188C
	public virtual void Main()
	{
	}

	// Token: 0x04000043 RID: 67
	public GameObject Block3;
}
