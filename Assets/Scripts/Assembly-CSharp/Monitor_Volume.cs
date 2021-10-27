using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class Monitor_Volume : MonoBehaviour
{
	// Token: 0x060007D7 RID: 2007 RVA: 0x00040B5C File Offset: 0x0003ED5C
	private void Update()
	{
		if (SpeechManager.musicVolume != this.previousMusicVolume)
		{
			if (this.previousMusicVolume != 0f && base.GetComponent<AudioSource>().volume != 0f && this.previousMusicVolume != -1f)
			{
				base.GetComponent<AudioSource>().volume = base.GetComponent<AudioSource>().volume / this.previousMusicVolume * SpeechManager.musicVolume;
			}
			else
			{
				base.GetComponent<AudioSource>().volume = this.defaultVolume * SpeechManager.musicVolume;
			}
			this.previousMusicVolume = SpeechManager.musicVolume;
		}
	}

	// Token: 0x04000A64 RID: 2660
	private float previousMusicVolume = -1f;

	// Token: 0x04000A65 RID: 2661
	public float defaultVolume = 1f;
}
