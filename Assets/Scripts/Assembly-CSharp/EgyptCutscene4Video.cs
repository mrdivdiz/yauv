using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class EgyptCutscene4Video : MonoBehaviour
{
	// Token: 0x06000A60 RID: 2656 RVA: 0x000708B0 File Offset: 0x0006EAB0
	private void Start()
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
		GC.WaitForPendingFinalizers();
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

	// Token: 0x06000A61 RID: 2657 RVA: 0x00070914 File Offset: 0x0006EB14
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, EgyptCutscene4Video.Fade.Out));
			Application.LoadLevel("LoadingQuadChase");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x000709A0 File Offset: 0x0006EBA0
	private IEnumerator FadeAudio(float timer, EgyptCutscene4Video.Fade fadeType)
	{
		float start = (fadeType != EgyptCutscene4Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != EgyptCutscene4Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x04001098 RID: 4248
	public float startTime;

	// Token: 0x04001099 RID: 4249
	private float movieDuration = 39f;

	// Token: 0x0400109A RID: 4250
	private bool faded;

	// Token: 0x0400109B RID: 4251
	private float fadeStartTime;

	// Token: 0x0400109C RID: 4252
	public float fadeTime = 4f;

	// Token: 0x02000218 RID: 536
	public enum Fade
	{
		// Token: 0x0400109E RID: 4254
		In,
		// Token: 0x0400109F RID: 4255
		Out
	}
}
