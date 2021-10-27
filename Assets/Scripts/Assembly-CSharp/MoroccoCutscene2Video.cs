using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class MoroccoCutscene2Video : MonoBehaviour
{
	// Token: 0x06000A7B RID: 2683 RVA: 0x00071568 File Offset: 0x0006F768
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

	// Token: 0x06000A7C RID: 2684 RVA: 0x000715C4 File Offset: 0x0006F7C4
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, MoroccoCutscene2Video.Fade.Out));
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00071648 File Offset: 0x0006F848
	private IEnumerator FadeAudio(float timer, MoroccoCutscene2Video.Fade fadeType)
	{
		float start = (fadeType != MoroccoCutscene2Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != MoroccoCutscene2Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x040010BD RID: 4285
	public float startTime;

	// Token: 0x040010BE RID: 4286
	private float movieDuration = 19.68f;

	// Token: 0x040010BF RID: 4287
	private bool faded;

	// Token: 0x040010C0 RID: 4288
	private float fadeStartTime;

	// Token: 0x040010C1 RID: 4289
	public float fadeTime = 4f;

	// Token: 0x02000222 RID: 546
	public enum Fade
	{
		// Token: 0x040010C3 RID: 4291
		In,
		// Token: 0x040010C4 RID: 4292
		Out
	}
}
