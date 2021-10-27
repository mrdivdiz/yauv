using System;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class QBMusic : MonoBehaviour
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x000938C8 File Offset: 0x00091AC8
	private void Start()
	{
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x000938CC File Offset: 0x00091ACC
	private void Update()
	{
		if (!base.GetComponent<AudioSource>().loop && !base.GetComponent<AudioSource>().isPlaying && base.GetComponent<AudioSource>().clip != this.EndingMusic && !mainmenu.pause)
		{
			base.GetComponent<AudioSource>().clip = this.LoopingMusic;
			base.GetComponent<AudioSource>().Play();
			base.GetComponent<AudioSource>().loop = true;
		}
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00093948 File Offset: 0x00091B48
	public void EndMusic()
	{
		base.GetComponent<AudioSource>().clip = this.EndingMusic;
		base.GetComponent<AudioSource>().Play();
		base.GetComponent<AudioSource>().loop = false;
	}

	// Token: 0x04001558 RID: 5464
	public AudioClip LoopingMusic;

	// Token: 0x04001559 RID: 5465
	public AudioClip EndingMusic;
}
