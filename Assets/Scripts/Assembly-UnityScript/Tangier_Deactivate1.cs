using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[Serializable]
public class Tangier_Deactivate1 : MonoBehaviour
{
	// Token: 0x0600006A RID: 106 RVA: 0x000035DC File Offset: 0x000017DC
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			UnityEngine.Object.Destroy(this.Block1);
			Resources.UnloadUnusedAssets();
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003610 File Offset: 0x00001810
	public virtual void Main()
	{
	}

	// Token: 0x04000041 RID: 65
	public GameObject Block1;
}
