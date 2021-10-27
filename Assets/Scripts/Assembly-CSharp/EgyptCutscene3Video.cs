using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class EgyptCutscene3Video : MonoBehaviour
{
	// Token: 0x06000A5C RID: 2652 RVA: 0x00070770 File Offset: 0x0006E970
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

	// Token: 0x06000A5D RID: 2653 RVA: 0x000707CC File Offset: 0x0006E9CC
	private void Update()
	{
		if (Input.anyKeyDown || Input.touchCount > 0)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.fadeStartTime = Time.time;
			base.StartCoroutine(this.FadeAudio(2f, EgyptCutscene3Video.Fade.Out));
			Application.LoadLevel("Ozgur_Melee");
		}
		if (!this.faded || Time.time > this.fadeStartTime + 3f)
		{
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00070858 File Offset: 0x0006EA58
	private IEnumerator FadeAudio(float timer, EgyptCutscene3Video.Fade fadeType)
	{
		float start = (fadeType != EgyptCutscene3Video.Fade.In) ? 1f : 0f;
		float end = (fadeType != EgyptCutscene3Video.Fade.In) ? 0f : 1f;
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

	// Token: 0x04001090 RID: 4240
	public float startTime;

	// Token: 0x04001091 RID: 4241
	private float movieDuration = 59f;

	// Token: 0x04001092 RID: 4242
	private bool faded;

	// Token: 0x04001093 RID: 4243
	private float fadeStartTime;

	// Token: 0x04001094 RID: 4244
	public float fadeTime = 4f;

	// Token: 0x02000216 RID: 534
	public enum Fade
	{
		// Token: 0x04001096 RID: 4246
		In,
		// Token: 0x04001097 RID: 4247
		Out
	}
}
