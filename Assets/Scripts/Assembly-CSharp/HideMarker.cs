using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class HideMarker : MonoBehaviour
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x00036F24 File Offset: 0x00035124
	private void OnTriggerEnter(Collider collisionInfo)
	{
		this.marker.SetActiveRecursively(false);
		this.coverMoveHint.SetActiveRecursively(true);
	}

	// Token: 0x040008D0 RID: 2256
	public GameObject marker;

	// Token: 0x040008D1 RID: 2257
	public GameObject coverMoveHint;
}
