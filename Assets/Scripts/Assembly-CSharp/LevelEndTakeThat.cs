using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class LevelEndTakeThat : MonoBehaviour
{
	// Token: 0x060007C3 RID: 1987 RVA: 0x0004079C File Offset: 0x0003E99C
	private void Start()
	{
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x000407A0 File Offset: 0x0003E9A0
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" || collisionInfo.tag == "Bike")
		{
			this.Source.clip = this.TakeThat;
			this.Source.Play();
		}
	}

	// Token: 0x04000A54 RID: 2644
	public AudioClip TakeThat;

	// Token: 0x04000A55 RID: 2645
	public AudioSource Source;
}
