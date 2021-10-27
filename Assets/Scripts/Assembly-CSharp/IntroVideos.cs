using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class IntroVideos : MonoBehaviour
{
	// Token: 0x06000A68 RID: 2664 RVA: 0x00070B38 File Offset: 0x0006ED38
	private void Start()
	{
		Resources.UnloadUnusedAssets();
		if (Language.CurrentLanguage() == LanguageCode.AR)
		{
			//Handheld.PlayFullScreenMovie("Movies/Intro-Arabic-Mobile.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			Application.LoadLevel("LoadingMainMenu");
		}
		else
		{
			//Handheld.PlayFullScreenMovie("Movies/Intro-English-Mobile.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			Application.LoadLevel("LoadingMainMenu");
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00070B94 File Offset: 0x0006ED94
	private void FixedUpdate()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, IntroVideos.Fade.Out));
			Application.LoadLevel("LoadingMainMenu");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00070C20 File Offset: 0x0006EE20
	private IEnumerator FadeAudio(float timer, IntroVideos.Fade fadeType)
	{
		float start = (fadeType != IntroVideos.Fade.In) ? 1f : 0f;
		float end = (fadeType != IntroVideos.Fade.In) ? 0f : 1f;
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

	// Token: 0x040010A8 RID: 4264
	public float startTime;

	// Token: 0x040010A9 RID: 4265
	private float movieDuration = 28f;

	// Token: 0x040010AA RID: 4266
	private bool faded;

	// Token: 0x040010AB RID: 4267
	private float fadeStartTime;

	// Token: 0x040010AC RID: 4268
	public float fadeTime = 4f;

	// Token: 0x040010AD RID: 4269
	private bool stopped;

	// Token: 0x0200021C RID: 540
	public enum Fade
	{
		// Token: 0x040010AF RID: 4271
		In,
		// Token: 0x040010B0 RID: 4272
		Out
	}
}
