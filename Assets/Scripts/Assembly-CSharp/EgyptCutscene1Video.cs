using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class EgyptCutscene1Video : MonoBehaviour
{
	// Token: 0x06000A58 RID: 2648 RVA: 0x00070630 File Offset: 0x0006E830
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

	// Token: 0x06000A59 RID: 2649 RVA: 0x0007068C File Offset: 0x0006E88C
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, EgyptCutscene1Video.Fade.Out));
			Application.LoadLevel("LoadingAhmoseTemple3");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00070718 File Offset: 0x0006E918
	private IEnumerator FadeAudio(float timer, EgyptCutscene1Video.Fade fadeType)
	{
		float start = (fadeType != EgyptCutscene1Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != EgyptCutscene1Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x04001088 RID: 4232
	public float startTime;

	// Token: 0x04001089 RID: 4233
	private float movieDuration = 162f;

	// Token: 0x0400108A RID: 4234
	private bool faded;

	// Token: 0x0400108B RID: 4235
	private float fadeStartTime;

	// Token: 0x0400108C RID: 4236
	public float fadeTime = 4f;

	// Token: 0x02000214 RID: 532
	public enum Fade
	{
		// Token: 0x0400108E RID: 4238
		In,
		// Token: 0x0400108F RID: 4239
		Out
	}
}
