using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class MusicLooper : MonoBehaviour
{
	// Token: 0x060007E2 RID: 2018 RVA: 0x00040FB8 File Offset: 0x0003F1B8
	private void Start()
	{
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00040FBC File Offset: 0x0003F1BC
	private void Update()
	{
		if (!base.GetComponent<AudioSource>().isPlaying && !mainmenu.pause)
		{
			if (this.nextClipToBePlayed != null)
			{
				base.GetComponent<AudioSource>().clip = this.nextClipToBePlayed;
				base.GetComponent<AudioSource>().volume = this.nextClipVolume * SpeechManager.musicVolume;
				this.nextClipToBePlayed = null;
				base.GetComponent<AudioSource>().loop = false;
				base.GetComponent<AudioSource>().Play();
			}
			else if (this.loop)
			{
				base.GetComponent<AudioSource>().Play();
			}
		}
	}

	// Token: 0x04000A72 RID: 2674
	public AudioClip nextClipToBePlayed;

	// Token: 0x04000A73 RID: 2675
	public float nextClipVolume;

	// Token: 0x04000A74 RID: 2676
	public bool loop;
}
