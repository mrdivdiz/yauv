using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class FailZone : MonoBehaviour
{
	// Token: 0x06000747 RID: 1863 RVA: 0x0003A144 File Offset: 0x00038344
	private void Update()
	{
		if (!this.faded && this.startTime != 0f)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0003A1C4 File Offset: 0x000383C4
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.startTime = Time.time;
		}
	}

	// Token: 0x04000934 RID: 2356
	private float startTime;

	// Token: 0x04000935 RID: 2357
	private bool faded;
}
