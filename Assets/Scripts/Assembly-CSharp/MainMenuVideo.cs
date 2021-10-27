using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class MainMenuVideo : MonoBehaviour
{
	// Token: 0x06000908 RID: 2312 RVA: 0x0004A87C File Offset: 0x00048A7C
	private void Awake()
	{
		this.MobileVideo.SetActive(true);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0004A88C File Offset: 0x00048A8C
	private void Start()
	{
		base.Invoke("turnon", 0.5f);
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0004A8A0 File Offset: 0x00048AA0
	private void turnon()
	{
		base.GetComponent<AudioSource>().enabled = true;
	}

	// Token: 0x04000C2E RID: 3118
	public GameObject PcVideo;

	// Token: 0x04000C2F RID: 3119
	public GameObject Ps3Video;

	// Token: 0x04000C30 RID: 3120
	public GameObject MobileVideo;
}
