using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000225 RID: 549
public class MoroccoCutscene4Video : MonoBehaviour
{
	// Token: 0x06000A83 RID: 2691 RVA: 0x000717E0 File Offset: 0x0006F9E0
	private void Start()
	{
		Resources.UnloadUnusedAssets();
		if (Language.CurrentLanguage() == LanguageCode.AR)
		{
			//Handheld.PlayFullScreenMovie("Movies/Intro-Arabic-Mobile.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			Application.LoadLevel("LoadingTemple");
		}
		else
		{
			//Handheld.PlayFullScreenMovie("Movies/Intro-English-Mobile.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			Application.LoadLevel("LoadingTemple");
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0007183C File Offset: 0x0006FA3C
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, MoroccoCutscene4Video.Fade.Out));
			Application.LoadLevel("Credits");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x000718C8 File Offset: 0x0006FAC8
	private IEnumerator FadeAudio(float timer, MoroccoCutscene4Video.Fade fadeType)
	{
		float start = (fadeType != MoroccoCutscene4Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != MoroccoCutscene4Video.Fade.In) ? 0f : 1f;
		float i = 0f;
		float step = 1f / timer;
		while (i <= 1f)
		{
			i += step * Time.deltaTime;
			base.GetComponent<AudioSource>().volume = Mathf.Lerp(start, end, i);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x040010CD RID: 4301
	public float startTime;

	// Token: 0x040010CE RID: 4302
	private float movieDuration = 55f;

	// Token: 0x040010CF RID: 4303
	private bool faded;

	// Token: 0x040010D0 RID: 4304
	private float fadeStartTime;

	// Token: 0x040010D1 RID: 4305
	public float fadeTime = 4f;

	// Token: 0x02000226 RID: 550
	public enum Fade
	{
		// Token: 0x040010D3 RID: 4307
		In,
		// Token: 0x040010D4 RID: 4308
		Out
	}
}
