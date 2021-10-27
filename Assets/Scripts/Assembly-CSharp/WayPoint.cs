using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class WayPoint : MonoBehaviour
{
	// Token: 0x060005EB RID: 1515 RVA: 0x00029794 File Offset: 0x00027994
	private void Start()
	{
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00029798 File Offset: 0x00027998
	private void Update()
	{
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002979C File Offset: 0x0002799C
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, "Waypoint.tif");
	}
}
