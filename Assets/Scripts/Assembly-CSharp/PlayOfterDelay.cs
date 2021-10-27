using System;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class PlayOfterDelay : MonoBehaviour
{
	// Token: 0x0600093D RID: 2365 RVA: 0x0004DDE4 File Offset: 0x0004BFE4
	private void PlayAndDestroy()
	{
		this.source = base.gameObject.AddComponent<AudioSource>();
		this.source.volume = SpeechManager.sfxVolume;
		this.source.clip = this.clip;
		this.source.Play();
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0004DE30 File Offset: 0x0004C030
	private void Start()
	{
		base.Invoke("PlayAndDestroy", this.delay);
		UnityEngine.Object.Destroy(base.gameObject, this.clip.length + this.delay);
	}

	// Token: 0x04000CA1 RID: 3233
	public AudioClip clip;

	// Token: 0x04000CA2 RID: 3234
	public float volume;

	// Token: 0x04000CA3 RID: 3235
	public float delay;

	// Token: 0x04000CA4 RID: 3236
	private AudioSource source;
}
