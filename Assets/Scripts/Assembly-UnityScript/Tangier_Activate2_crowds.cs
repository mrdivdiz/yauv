using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[Serializable]
public class Tangier_Activate2_crowds : MonoBehaviour
{
	// Token: 0x0600005B RID: 91 RVA: 0x0000348C File Offset: 0x0000168C
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.Block2crowds.SetActiveRecursively(true);
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000034B0 File Offset: 0x000016B0
	public virtual void Main()
	{
	}

	// Token: 0x0400003C RID: 60
	public GameObject Block2crowds;
}
