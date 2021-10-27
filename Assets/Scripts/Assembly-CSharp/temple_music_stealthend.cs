using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class temple_music_stealthend : MonoBehaviour
{
	// Token: 0x060008EE RID: 2286 RVA: 0x0004999C File Offset: 0x00047B9C
	private void Awake()
	{
		this.played = false;
		this.cleared = false;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x000499AC File Offset: 0x00047BAC
	private void Update()
	{
		if (!base.transform.parent.gameObject.GetComponent<AudioSource>().isPlaying && this.played && !this.cleared && !mainmenu.pause && !this.looping)
		{
			if (base.transform.parent.gameObject.GetComponent<AudioSource>().clip != null)
			{
				base.transform.parent.gameObject.GetComponent<AudioSource>().clip = null;
			}
			this.cleared = true;
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00049A4C File Offset: 0x00047C4C
	public void StealthEnded()
	{
		if (base.transform.parent.gameObject.GetComponent<AudioSource>().clip.name == "Middle Eastern Ambience")
		{
			if (base.transform.parent.gameObject.GetComponent<AudioSource>().clip != null)
			{
				base.transform.parent.gameObject.GetComponent<AudioSource>().clip = null;
			}
			base.transform.parent.gameObject.GetComponent<AudioSource>().clip = this.clip;
			base.transform.parent.GetComponent<AudioSource>().volume = this.volume * SpeechManager.musicVolume;
			base.transform.parent.gameObject.GetComponent<AudioSource>().loop = this.looping;
			base.transform.parent.gameObject.GetComponent<AudioSource>().Play();
			this.played = true;
		}
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00049B4C File Offset: 0x00047D4C
	private IEnumerator FadeAudio(float timer, temple_music_stealthend.Fade fadeType)
	{
		float start = (fadeType != temple_music_stealthend.Fade.In) ? 1f : 0f;
		float end = (fadeType != temple_music_stealthend.Fade.In) ? 0f : 1f;
		float i = 0f;
		float step = 1f / timer;
		while (i <= 1f)
		{
			i += step * Time.deltaTime;
			base.transform.parent.gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(start, end, i);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x04000BD8 RID: 3032
	private bool played;

	// Token: 0x04000BD9 RID: 3033
	private bool cleared;

	// Token: 0x04000BDA RID: 3034
	public AudioClip clip;

	// Token: 0x04000BDB RID: 3035
	public string clipName;

	// Token: 0x04000BDC RID: 3036
	public float volume;

	// Token: 0x04000BDD RID: 3037
	public bool looping;

	// Token: 0x04000BDE RID: 3038
	public float fadeTime = 4f;

	// Token: 0x020001BF RID: 447
	public enum Fade
	{
		// Token: 0x04000BE0 RID: 3040
		In,
		// Token: 0x04000BE1 RID: 3041
		Out
	}
}
