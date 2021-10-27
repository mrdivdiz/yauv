using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class FallCamera : MonoBehaviour
{
	// Token: 0x0600074A RID: 1866 RVA: 0x0003A1FC File Offset: 0x000383FC
	public void Start()
	{
		if (Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
			this.shooterCam = Camera.main.GetComponent<ShooterGameCamera>();
		}
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0003A23C File Offset: 0x0003843C
	public void Update()
	{
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0003A274 File Offset: 0x00038474
	public void OnTriggerEnter(Collider p)
	{
		if (p.gameObject.tag == "Player" && this.cam != null && !this.faded)
		{
			this.shooterCam.hitAlpha = 1f;
			this.shooterCam.cameraPosition = null;
			this.shooterCam.fixedCameraPosition = true;
			this.cam.StartFade(Color.black, 3f);
			this.faded = true;
			this.startTime = Time.time;
			if (this.cam.GetComponent<AudioSource>() != null && this.shooterCam.fallingSound != null)
			{
				this.cam.GetComponent<AudioSource>().PlayOneShot(this.shooterCam.fallingSound);
			}
			p.gameObject.GetComponent<Health>().enabled = false;
		}
	}

	// Token: 0x04000936 RID: 2358
	private float startTime;

	// Token: 0x04000937 RID: 2359
	private bool stopTimer;

	// Token: 0x04000938 RID: 2360
	private bool faded;

	// Token: 0x04000939 RID: 2361
	private CameraFade cam;

	// Token: 0x0400093A RID: 2362
	private ShooterGameCamera shooterCam;
}
