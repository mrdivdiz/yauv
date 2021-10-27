using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class Timer : MonoBehaviour
{
	// Token: 0x06000879 RID: 2169 RVA: 0x000461BC File Offset: 0x000443BC
	public void Awake()
	{
		Timer.cutsceneFinishTime = Time.timeSinceLevelLoad;
		if (Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x000461F4 File Offset: 0x000443F4
	private void Update()
	{
		if (this.cam == null && Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
		if (this.stopTimer || Timer.cutsceneFinishTime == -1f)
		{
			return;
		}
		if (this.cam != null && !this.faded && Time.timeSinceLevelLoad - Timer.cutsceneFinishTime > this.timeLimit)
		{
			this.cam.StartFade(Color.red, 3f);
			this.faded = true;
			this.startTime = Time.time;
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x000462DC File Offset: 0x000444DC
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.stopTimer = true;
		}
	}

	// Token: 0x04000B42 RID: 2882
	public float timeLimit;

	// Token: 0x04000B43 RID: 2883
	private float startTime;

	// Token: 0x04000B44 RID: 2884
	private bool stopTimer;

	// Token: 0x04000B45 RID: 2885
	private bool faded;

	// Token: 0x04000B46 RID: 2886
	public static float cutsceneFinishTime = -1f;

	// Token: 0x04000B47 RID: 2887
	private CameraFade cam;
}
