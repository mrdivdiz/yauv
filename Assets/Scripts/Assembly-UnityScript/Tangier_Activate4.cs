using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
[Serializable]
public class Tangier_Activate4 : MonoBehaviour
{
	// Token: 0x06000064 RID: 100 RVA: 0x00003554 File Offset: 0x00001754
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !this.activated4)
		{
			Application.LoadLevelAdditiveAsync("TangierMarket-Block4");
			this.activated4 = true;
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003594 File Offset: 0x00001794
	public virtual void Main()
	{
	}

	// Token: 0x0400003F RID: 63
	private bool activated4;
}
