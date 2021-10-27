using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class Monitor_Volume_SFX : MonoBehaviour
{
	// Token: 0x060007D9 RID: 2009 RVA: 0x00040C18 File Offset: 0x0003EE18
	private void Update()
	{
		if (SpeechManager.sfxVolume != this.previousSFXVolume)
		{
			if (this.previousSFXVolume != 0f && base.GetComponent<AudioSource>().volume != 0f && this.previousSFXVolume != -1f)
			{
				base.GetComponent<AudioSource>().volume = base.GetComponent<AudioSource>().volume / this.previousSFXVolume * SpeechManager.sfxVolume;
			}
			else
			{
				base.GetComponent<AudioSource>().volume = this.defaultVolume * SpeechManager.sfxVolume;
			}
			this.previousSFXVolume = SpeechManager.sfxVolume;
		}
	}

	// Token: 0x04000A66 RID: 2662
	private float previousSFXVolume = -1f;

	// Token: 0x04000A67 RID: 2663
	public float defaultVolume = 1f;
}
