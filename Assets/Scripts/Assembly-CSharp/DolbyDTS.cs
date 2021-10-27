using System;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class DolbyDTS : MonoBehaviour
{
	// Token: 0x06000726 RID: 1830 RVA: 0x00038FE4 File Offset: 0x000371E4
	public void Awake()
	{
		if (Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
		base.Invoke("Proceed", 4f);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00039024 File Offset: 0x00037224
	public void Proceed()
	{
		this.cam.StartFade(Color.black, 3f);
		this.faded = true;
		this.startTime = Time.time;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00039050 File Offset: 0x00037250
	private void Update()
	{
		if (this.cam == null && Camera.main != null)
		{
			this.cam = Camera.main.GetComponent<CameraFade>();
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("SavesData");
		}
	}

	// Token: 0x04000906 RID: 2310
	public float timeLimit;

	// Token: 0x04000907 RID: 2311
	private float startTime;

	// Token: 0x04000908 RID: 2312
	private bool faded;

	// Token: 0x04000909 RID: 2313
	public static float cutsceneFinishTime = -1f;

	// Token: 0x0400090A RID: 2314
	private CameraFade cam;
}
