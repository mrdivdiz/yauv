using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class DrowsyEffect : MonoBehaviour
{
	// Token: 0x06000439 RID: 1081 RVA: 0x00025CC4 File Offset: 0x00023EC4
	private void Start()
	{
		this.fisheye = base.GetComponent<Fisheye>();
		this.tiltShift = base.GetComponent<TiltShift>();
		this.vignetting = base.GetComponent<Vignetting>();
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00025CF8 File Offset: 0x00023EF8
	private void Update()
	{
		if (this.drowsy && Time.time > this.startTime + 1f)
		{
			base.StartCoroutine(this.FadeFishEye(1f, DrowsyEffect.Fade.Out));
			this.drowsy = false;
			this.turnOffTimer = 1f;
		}
		if (this.dizzy && Time.time > this.startTime + 5f)
		{
			base.StartCoroutine(this.FadeTiltShift(3f, DrowsyEffect.Fade.Out));
			this.dizzy = false;
			this.turnOffTimer = 3f;
		}
		if (this.fadeVignetting && Time.time > this.startTime + 1f)
		{
			base.StartCoroutine(this.FadeVignetting(1f, DrowsyEffect.Fade.Out));
			this.fadeVignetting = false;
			this.turnOffTimer = 1f;
		}
		if (this.turnOffTimer > 0f)
		{
			this.turnOffTimer -= Time.deltaTime;
			if (this.turnOffTimer < 0f)
			{
				this.fisheye.enabled = false;
				this.tiltShift.enabled = false;
			}
		}
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00025E24 File Offset: 0x00024024
	public void FeelDrowsy()
	{
		switch (this.type)
		{
		case 0:
			this.fisheye.enabled = true;
			base.StartCoroutine(this.FadeFishEye(1f, DrowsyEffect.Fade.In));
			this.drowsy = true;
			this.startTime = Time.time;
			break;
		case 1:
			this.tiltShift.enabled = true;
			base.StartCoroutine(this.FadeTiltShift(3f, DrowsyEffect.Fade.In));
			this.dizzy = true;
			this.startTime = Time.time;
			break;
		case 2:
			base.StartCoroutine(this.FadeVignetting(1f, DrowsyEffect.Fade.In));
			this.fadeVignetting = true;
			this.startTime = Time.time;
			break;
		}
		this.type = (this.type + 1) % 3;
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00025EF8 File Offset: 0x000240F8
	private IEnumerator FadeFishEye(float timer, DrowsyEffect.Fade fadeType)
	{
		float start = (fadeType != DrowsyEffect.Fade.In) ? 0.5f : 0f;
		float end = (fadeType != DrowsyEffect.Fade.In) ? 0f : 0.5f;
		float i = 0f;
		float step = 1f / timer;
		while (i <= 1f)
		{
			i += step * Time.deltaTime;
			this.fisheye.strengthX = Mathf.Lerp(start, end, i);
			this.fisheye.strengthY = Mathf.Lerp(start, end, i);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00025F30 File Offset: 0x00024130
	private IEnumerator FadeTiltShift(float timer, DrowsyEffect.Fade fadeType)
	{
		float start = (fadeType != DrowsyEffect.Fade.In) ? 0f : 130f;
		float end = (fadeType != DrowsyEffect.Fade.In) ? 130f : 0f;
		float i = 0f;
		float step = 1f / timer;
		while (i <= 1f)
		{
			i += step * Time.deltaTime;
			this.tiltShift.focalPoint = Mathf.Lerp(start, end, i);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00025F68 File Offset: 0x00024168
	private IEnumerator FadeVignetting(float timer, DrowsyEffect.Fade fadeType)
	{
		float start = (fadeType != DrowsyEffect.Fade.In) ? 10f : 4f;
		float end = (fadeType != DrowsyEffect.Fade.In) ? 4f : 10f;
		float i = 0f;
		float step = 1f / timer;
		while (i <= 1f)
		{
			i += step * Time.deltaTime;
			this.vignetting.intensity = Mathf.Lerp(start, end, i);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x040005B8 RID: 1464
	private Fisheye fisheye;

	// Token: 0x040005B9 RID: 1465
	private TiltShift tiltShift;

	// Token: 0x040005BA RID: 1466
	private Vignetting vignetting;

	// Token: 0x040005BB RID: 1467
	private bool drowsy;

	// Token: 0x040005BC RID: 1468
	private bool dizzy;

	// Token: 0x040005BD RID: 1469
	private bool fadeVignetting;

	// Token: 0x040005BE RID: 1470
	private float startTime;

	// Token: 0x040005BF RID: 1471
	private float turnOffTimer;

	// Token: 0x040005C0 RID: 1472
	private int type;

	// Token: 0x020000D0 RID: 208
	public enum Fade
	{
		// Token: 0x040005C2 RID: 1474
		In,
		// Token: 0x040005C3 RID: 1475
		Out
	}
}
