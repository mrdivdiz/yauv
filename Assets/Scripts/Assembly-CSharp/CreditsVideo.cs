using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class CreditsVideo : MonoBehaviour
{
	// Token: 0x06000A54 RID: 2644 RVA: 0x000704F0 File Offset: 0x0006E6F0
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

	// Token: 0x06000A55 RID: 2645 RVA: 0x0007054C File Offset: 0x0006E74C
	private void Update()
	{
		if (Input.anyKey || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, CreditsVideo.Fade.Out));
			Application.LoadLevel("LoadingMainMenu");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000705D8 File Offset: 0x0006E7D8
	private IEnumerator FadeAudio(float timer, CreditsVideo.Fade fadeType)
	{
		float start = (fadeType != CreditsVideo.Fade.In) ? 1f : 0f;
		float end = (fadeType != CreditsVideo.Fade.In) ? 0f : 1f;
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

	// Token: 0x04001080 RID: 4224
	public float startTime;

	// Token: 0x04001081 RID: 4225
	private float movieDuration = 118f;

	// Token: 0x04001082 RID: 4226
	private bool faded;

	// Token: 0x04001083 RID: 4227
	private float fadeStartTime;

	// Token: 0x04001084 RID: 4228
	public float fadeTime = 4f;

	// Token: 0x02000212 RID: 530
	public enum Fade
	{
		// Token: 0x04001086 RID: 4230
		In,
		// Token: 0x04001087 RID: 4231
		Out
	}
}
