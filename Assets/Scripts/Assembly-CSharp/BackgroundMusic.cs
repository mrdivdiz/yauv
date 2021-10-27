using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class BackgroundMusic : MonoBehaviour
{
	// Token: 0x060006C5 RID: 1733 RVA: 0x00036D04 File Offset: 0x00034F04
	private void Start()
	{
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00036D08 File Offset: 0x00034F08
	private void Update()
	{
		if (this.start)
		{
			if (this.audioClips.Length > 0 && base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().clip = this.audioClips[0];
				base.GetComponent<AudioSource>().loop = false;
				base.GetComponent<AudioSource>().volume = SpeechManager.musicVolume / 0.5f;
				base.GetComponent<AudioSource>().Play();
			}
			this.start = false;
			this.started = true;
		}
		if (this.started && !base.GetComponent<AudioSource>().isPlaying && (!mainmenu.pause || mainmenu.hintPause))
		{
			if (this.playNextAfterFinishing && this.currentAudioClip < this.audioClips.Length - 1)
			{
				this.audioClips[this.currentAudioClip] = null;
				this.currentAudioClip++;
				this.playNextAfterFinishing = false;
			}
			base.GetComponent<AudioSource>().clip = this.audioClips[this.currentAudioClip];
			base.GetComponent<AudioSource>().loop = false;
			base.GetComponent<AudioSource>().volume = SpeechManager.musicVolume * 0.5f;
			base.GetComponent<AudioSource>().Play();
		}
		base.GetComponent<AudioSource>().volume = SpeechManager.musicVolume * 0.75f;
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00036E6C File Offset: 0x0003506C
	public void PlayNextAudioClip()
	{
		this.playNextAfterFinishing = true;
	}

	// Token: 0x040008A0 RID: 2208
	public AudioClip[] audioClips;

	// Token: 0x040008A1 RID: 2209
	public int currentAudioClip;

	// Token: 0x040008A2 RID: 2210
	private bool playNextAfterFinishing;

	// Token: 0x040008A3 RID: 2211
	public bool start;

	// Token: 0x040008A4 RID: 2212
	public bool started;

	// Token: 0x040008A5 RID: 2213
	public CSComponent currentCutscene;
}
