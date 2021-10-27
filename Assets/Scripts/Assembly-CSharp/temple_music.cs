using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class temple_music : MonoBehaviour
{
	// Token: 0x060008E8 RID: 2280 RVA: 0x000496D0 File Offset: 0x000478D0
	private void Awake()
	{
		this.played = false;
		this.cleared = false;
		this.musicLooper = (UnityEngine.Object.FindObjectOfType(typeof(MusicLooper)) as MusicLooper);
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00049708 File Offset: 0x00047908
	private void Update()
	{
		if (!base.transform.parent.gameObject.GetComponent<AudioSource>().isPlaying && this.played && !this.cleared && !mainmenu.pause && !this.looping)
		{
			if (base.transform.parent.gameObject.GetComponent<AudioSource>().clip != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				base.transform.parent.gameObject.GetComponent<AudioSource>().clip = null;
			}
			this.cleared = true;
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x000497B4 File Offset: 0x000479B4
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.played)
		{
			if (!this.finishPreviousClipFirst)
			{
				if (base.transform.parent.gameObject.GetComponent<AudioSource>().clip != null)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					base.transform.parent.gameObject.GetComponent<AudioSource>().clip = null;
				}
				base.transform.parent.gameObject.GetComponent<AudioSource>().clip = this.clip;
				base.transform.parent.GetComponent<AudioSource>().volume = this.volume * SpeechManager.musicVolume;
				base.transform.parent.gameObject.GetComponent<AudioSource>().loop = this.looping;
				base.transform.parent.gameObject.GetComponent<AudioSource>().Play();
				if (this.musicLooper != null)
				{
					this.musicLooper.nextClipToBePlayed = null;
					this.musicLooper.loop = this.looping;
				}
				this.played = true;
			}
			else if (this.musicLooper != null)
			{
				this.musicLooper.nextClipToBePlayed = this.clip;
				this.musicLooper.nextClipVolume = this.volume * SpeechManager.musicVolume;
				this.musicLooper.loop = this.looping;
			}
		}
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00049938 File Offset: 0x00047B38
	public void FadeOut()
	{
		base.StartCoroutine(this.FadeAudio(6f, temple_music.Fade.Out));
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00049950 File Offset: 0x00047B50
	private IEnumerator FadeAudio(float timer, temple_music.Fade fadeType)
	{
		float start = (fadeType != temple_music.Fade.In) ? 1f : 0f;
		float end = (fadeType != temple_music.Fade.In) ? 0f : 1f;
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

	// Token: 0x04000BCC RID: 3020
	private bool played;

	// Token: 0x04000BCD RID: 3021
	private bool cleared;

	// Token: 0x04000BCE RID: 3022
	public AudioClip clip;

	// Token: 0x04000BCF RID: 3023
	public string clipName;

	// Token: 0x04000BD0 RID: 3024
	public float volume;

	// Token: 0x04000BD1 RID: 3025
	public bool looping;

	// Token: 0x04000BD2 RID: 3026
	public bool finishPreviousClipFirst;

	// Token: 0x04000BD3 RID: 3027
	public float fadeTime = 4f;

	// Token: 0x04000BD4 RID: 3028
	private MusicLooper musicLooper;

	// Token: 0x020001BD RID: 445
	public enum Fade
	{
		// Token: 0x04000BD6 RID: 3030
		In,
		// Token: 0x04000BD7 RID: 3031
		Out
	}
}
