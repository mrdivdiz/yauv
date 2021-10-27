using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000023 RID: 35
[Serializable]
public class Tangier_Cart : MonoBehaviour
{
	// Token: 0x06000067 RID: 103 RVA: 0x000035A0 File Offset: 0x000017A0
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.cartScene.StartCutscene();
		}
	}

	// Token: 0x06000068 RID: 104 RVA: 0x000035D0 File Offset: 0x000017D0
	public virtual void Main()
	{
	}

	// Token: 0x04000040 RID: 64
	public CSComponent cartScene;
}
