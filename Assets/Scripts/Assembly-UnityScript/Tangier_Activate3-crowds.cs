using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[Serializable]
public class Tangier_Activate3crowds : MonoBehaviour
{
	// Token: 0x0600005E RID: 94 RVA: 0x000034BC File Offset: 0x000016BC
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !this.activated3crowds)
		{
			Application.LoadLevelAdditiveAsync("TangierMarket-Block3-Crowds");
			this.activated3crowds = true;
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000034FC File Offset: 0x000016FC
	public virtual void Main()
	{
	}

	// Token: 0x0400003D RID: 61
	public bool activated3crowds;
}
