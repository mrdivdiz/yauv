using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000261 RID: 609
public class TowerHit : MonoBehaviour
{
	// Token: 0x06000B80 RID: 2944 RVA: 0x00091570 File Offset: 0x0008F770
	public void Hit(Hashtable hitInfo)
	{
		if (Time.timeScale != 0.15f)
		{
			return;
		}
		this.health -= (float)hitInfo["D"];
		if (this.health < 0f)
		{
			this.health = 0f;
			this.hit = true;
		}
	}

	// Token: 0x04001502 RID: 5378
	public float health = 20f;

	// Token: 0x04001503 RID: 5379
	public bool hit;
}
