using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200022A RID: 554
public class TitleVideos : MonoBehaviour
{
	// Token: 0x06000A93 RID: 2707 RVA: 0x00072160 File Offset: 0x00070360
	private void Start()
	{
		Resources.UnloadUnusedAssets();
		if (Language.CurrentLanguage() == LanguageCode.AR)
		{
			//Handheld.PlayFullScreenMovie("Movies/Intro-Arabic-Mobile.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			Application.LoadLevel("LoadingPrologue");
		}
		else
		{
			//Handheld.PlayFullScreenMovie("Movies/Intro-English-Mobile.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			Application.LoadLevel("LoadingPrologue");
		}
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x000721BC File Offset: 0x000703BC
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, TitleVideos.Fade.Out));
			Application.LoadLevel("LoadingPrologue");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00072248 File Offset: 0x00070448
	private IEnumerator FadeAudio(float timer, TitleVideos.Fade fadeType)
	{
		float start = (fadeType != TitleVideos.Fade.In) ? 1f : 0f;
		float end = (fadeType != TitleVideos.Fade.In) ? 0f : 1f;
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

	// Token: 0x040010E2 RID: 4322
	public float startTime;

	// Token: 0x040010E3 RID: 4323
	private float movieDuration = 66f;

	// Token: 0x040010E4 RID: 4324
	private bool faded;

	// Token: 0x040010E5 RID: 4325
	private float fadeStartTime;

	// Token: 0x040010E6 RID: 4326
	public float fadeTime = 4f;

	// Token: 0x0200022B RID: 555
	public enum Fade
	{
		// Token: 0x040010E8 RID: 4328
		In,
		// Token: 0x040010E9 RID: 4329
		Out
	}
}
