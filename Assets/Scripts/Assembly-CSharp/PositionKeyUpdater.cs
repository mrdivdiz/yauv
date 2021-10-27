using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x0200002C RID: 44
[ExecuteInEditMode]
public class PositionKeyUpdater : MonoBehaviour
{
	// Token: 0x0600010B RID: 267 RVA: 0x00007440 File Offset: 0x00005640
	public void Update()
	{
		if (this.key != null)
		{
			Vector3 data = this.key.data;
			if (data != this.lastPos)
			{
				this.key.UpdateTrack();
			}
			this.lastPos = data;
		}
	}

	// Token: 0x040000F8 RID: 248
	public PositionTrackKey1 key;

	// Token: 0x040000F9 RID: 249
	public Vector3 lastPos;
}
