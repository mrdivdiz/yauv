using System;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class HideObject : MonoBehaviour
{
	// Token: 0x0600078E RID: 1934 RVA: 0x0003D94C File Offset: 0x0003BB4C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		this.object1.SetActive(false);
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0003D95C File Offset: 0x0003BB5C
	private void Update()
	{
	}

	// Token: 0x040009DC RID: 2524
	public GameObject object1;
}
