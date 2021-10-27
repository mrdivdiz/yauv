using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class HideMarkerOnExit : MonoBehaviour
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x00036F48 File Offset: 0x00035148
	private void OnTriggerExit(Collider collisionInfo)
	{
		this.marker.SetActiveRecursively(false);
	}

	// Token: 0x040008D2 RID: 2258
	public GameObject marker;
}
