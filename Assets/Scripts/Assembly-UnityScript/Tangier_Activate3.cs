using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[Serializable]
public class Tangier_Activate3 : MonoBehaviour
{
	// Token: 0x06000061 RID: 97 RVA: 0x00003508 File Offset: 0x00001708
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !this.activated3)
		{
			Application.LoadLevelAdditiveAsync("TangierMarket-Block3");
			this.activated3 = true;
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003548 File Offset: 0x00001748
	public virtual void Main()
	{
	}

	// Token: 0x0400003E RID: 62
	public bool activated3;
}
