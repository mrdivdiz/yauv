using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000219 RID: 537
public class EgyptCutscene5Video : MonoBehaviour
{
	// Token: 0x06000A64 RID: 2660 RVA: 0x000709F8 File Offset: 0x0006EBF8
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

	// Token: 0x06000A65 RID: 2661 RVA: 0x00070A54 File Offset: 0x0006EC54
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, EgyptCutscene5Video.Fade.Out));
			Application.LoadLevel("Morocco-Cutscene1");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00070AE0 File Offset: 0x0006ECE0
	private IEnumerator FadeAudio(float timer, EgyptCutscene5Video.Fade fadeType)
	{
		float start = (fadeType != EgyptCutscene5Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != EgyptCutscene5Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x040010A0 RID: 4256
	public float startTime;

	// Token: 0x040010A1 RID: 4257
	private float movieDuration = 100f;

	// Token: 0x040010A2 RID: 4258
	private bool faded;

	// Token: 0x040010A3 RID: 4259
	private float fadeStartTime;

	// Token: 0x040010A4 RID: 4260
	public float fadeTime = 4f;

	// Token: 0x0200021A RID: 538
	public enum Fade
	{
		// Token: 0x040010A6 RID: 4262
		In,
		// Token: 0x040010A7 RID: 4263
		Out
	}
}
