using System;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class PlayerRagdollScript : MonoBehaviour
{
	// Token: 0x060009C9 RID: 2505 RVA: 0x000641D4 File Offset: 0x000623D4
	private void Start()
	{
		this.startTime = Time.time;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x000641E4 File Offset: 0x000623E4
	private void Update()
	{
		if (!this.faded && Time.time > this.startTime + 5f)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
		}
		if (this.faded && Time.time > this.startTime + 8f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
	}

	// Token: 0x04000EC3 RID: 3779
	private float startTime;

	// Token: 0x04000EC4 RID: 3780
	private bool faded;
}
