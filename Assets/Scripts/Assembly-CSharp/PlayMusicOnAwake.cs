using System;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class PlayMusicOnAwake : MonoBehaviour
{
	// Token: 0x06000915 RID: 2325 RVA: 0x0004ADDC File Offset: 0x00048FDC
	private void Awake()
	{
		if (this.musicClips != null && this.musicClips.Length > 0 && base.GetComponent<AudioSource>() != null)
		{
			this.currentclip = UnityEngine.Random.Range(0, this.musicClips.Length);
			this.PlayMusic();
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0004AE30 File Offset: 0x00049030
	private void PlayMusic()
	{
		base.GetComponent<AudioSource>().clip = this.musicClips[this.currentclip];
		base.GetComponent<AudioSource>().loop = false;
		base.GetComponent<AudioSource>().volume = base.GetComponent<AudioSource>().volume * SpeechManager.musicVolume;
		base.GetComponent<AudioSource>().Play();
		base.Invoke("PlayMusic", this.musicClips[this.currentclip].length);
		this.currentclip = (this.currentclip + 1) % this.musicClips.Length;
	}

	// Token: 0x04000C36 RID: 3126
	public AudioClip[] musicClips;

	// Token: 0x04000C37 RID: 3127
	private int currentclip;
}
