using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class MoroccoCutscene1Video : MonoBehaviour
{
	// Token: 0x06000A77 RID: 2679 RVA: 0x00071428 File Offset: 0x0006F628
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

	// Token: 0x06000A78 RID: 2680 RVA: 0x00071484 File Offset: 0x0006F684
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, MoroccoCutscene1Video.Fade.Out));
			Application.LoadLevel("LoadingTangier");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00071510 File Offset: 0x0006F710
	private IEnumerator FadeAudio(float timer, MoroccoCutscene1Video.Fade fadeType)
	{
		float start = (fadeType != MoroccoCutscene1Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != MoroccoCutscene1Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x040010B6 RID: 4278
	public float startTime;

	// Token: 0x040010B7 RID: 4279
	private bool faded;

	// Token: 0x040010B8 RID: 4280
	private float fadeStartTime;

	// Token: 0x040010B9 RID: 4281
	public float fadeTime = 4f;

	// Token: 0x02000220 RID: 544
	public enum Fade
	{
		// Token: 0x040010BB RID: 4283
		In,
		// Token: 0x040010BC RID: 4284
		Out
	}
}
