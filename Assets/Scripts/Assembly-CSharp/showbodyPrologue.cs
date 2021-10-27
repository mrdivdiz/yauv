using System;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class showbodyPrologue : MonoBehaviour
{
	// Token: 0x060008DF RID: 2271 RVA: 0x00049638 File Offset: 0x00047838
	private void Start()
	{
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0004963C File Offset: 0x0004783C
	public void ShowBody()
	{
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00049640 File Offset: 0x00047840
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.body.SetActive(true);
		}
	}

	// Token: 0x04000BCA RID: 3018
	public GameObject body;
}
