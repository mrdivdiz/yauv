using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class DestroyOnCheckpointLoad : MonoBehaviour
{
	// Token: 0x06000716 RID: 1814 RVA: 0x00038D78 File Offset: 0x00036F78
	public void OnCheckpointLoad(int checkpointReached)
	{
		if (checkpointReached > this.checkpointLimit)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040008EF RID: 2287
	public int checkpointLimit;
}
