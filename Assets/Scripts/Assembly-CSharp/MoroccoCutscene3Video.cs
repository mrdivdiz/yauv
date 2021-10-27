using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class MoroccoCutscene3Video : MonoBehaviour
{
	// Token: 0x06000A7F RID: 2687 RVA: 0x000716A0 File Offset: 0x0006F8A0
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

	// Token: 0x06000A80 RID: 2688 RVA: 0x000716FC File Offset: 0x0006F8FC
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, MoroccoCutscene3Video.Fade.Out));
			Application.LoadLevel("Tangier-Block5B");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00071788 File Offset: 0x0006F988
	private IEnumerator FadeAudio(float timer, MoroccoCutscene3Video.Fade fadeType)
	{
		float start = (fadeType != MoroccoCutscene3Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != MoroccoCutscene3Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x040010C5 RID: 4293
	public float startTime;

	// Token: 0x040010C6 RID: 4294
	private float movieDuration = 42f;

	// Token: 0x040010C7 RID: 4295
	private bool faded;

	// Token: 0x040010C8 RID: 4296
	private float fadeStartTime;

	// Token: 0x040010C9 RID: 4297
	public float fadeTime = 4f;

	// Token: 0x02000224 RID: 548
	public enum Fade
	{
		// Token: 0x040010CB RID: 4299
		In,
		// Token: 0x040010CC RID: 4300
		Out
	}
}
