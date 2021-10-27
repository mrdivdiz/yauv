using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class AtmoXfade : MonoBehaviour
{
	// Token: 0x060000EF RID: 239 RVA: 0x00005E78 File Offset: 0x00004078
	private void Start()
	{
		if (this.skyMat)
		{
			this.skyMat.SetColor("_Tint", this.skyBright);
		}
		if (this.dirLight)
		{
			this.dirLight.color = this.lightBright;
		}
		if (this.useRenderFog)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = this.fogBright;
		}
		else
		{
			RenderSettings.fog = false;
		}
		this.curIntensity = this.maxLightIntensity;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00005F00 File Offset: 0x00004100
	private void OnTriggerEnter(Collider c)
	{
		if (c.sharedMaterial != null && c.sharedMaterial.name == "Player")
		{
			this.fadeState = AtmoXfade.FadeState.FadeDark;
			base.StartCoroutine(this.FadeDark());
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00005F4C File Offset: 0x0000414C
	private void OnTriggerExit(Collider c)
	{
		if (c.sharedMaterial != null && c.sharedMaterial.name == "Player")
		{
			this.fadeState = AtmoXfade.FadeState.FadeBright;
			base.StartCoroutine(this.FadeBright());
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00005F98 File Offset: 0x00004198
	private IEnumerator FadeDark()
	{
		float t = 1E-05f;
		while (this.fadeState == AtmoXfade.FadeState.FadeDark && this.curIntensity > this.minLightIntensity)
		{
			this.skyMat.SetColor("_Tint", Color.Lerp(this.skyMat.GetColor("_Tint"), this.skyDark, t));
			this.dirLight.color = Color.Lerp(this.dirLight.color, this.lightDark, t);
			this.curIntensity = this.dirLight.intensity;
			this.dirLight.intensity = Mathf.SmoothStep(this.curIntensity, this.minLightIntensity, t);
			if (this.useRenderFog)
			{
				RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, this.fogDark, t);
				RenderSettings.fogDensity = Mathf.SmoothStep(RenderSettings.fogDensity, this.maxFog, t);
			}
			yield return null;
			t += Time.deltaTime / this.fadeTime;
		}
		yield break;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00005FB4 File Offset: 0x000041B4
	private IEnumerator FadeBright()
	{
		float t = 1E-05f;
		while (this.fadeState == AtmoXfade.FadeState.FadeBright && this.curIntensity < this.maxLightIntensity)
		{
			this.skyMat.SetColor("_Tint", Color.Lerp(this.skyMat.GetColor("_Tint"), this.skyBright, t));
			this.dirLight.color = Color.Lerp(this.dirLight.color, this.lightBright, t);
			this.curIntensity = this.dirLight.intensity;
			this.dirLight.intensity = Mathf.SmoothStep(this.curIntensity, this.maxLightIntensity, t);
			if (this.useRenderFog)
			{
				RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, this.fogBright, t);
				RenderSettings.fogDensity = Mathf.SmoothStep(RenderSettings.fogDensity, this.minFog, t);
			}
			yield return null;
			t += Time.deltaTime / this.fadeTime;
		}
		yield break;
	}

	// Token: 0x040000BD RID: 189
	public Material skyMat;

	// Token: 0x040000BE RID: 190
	public Color skyBright = Color.grey;

	// Token: 0x040000BF RID: 191
	public Color skyDark = Color.black;

	// Token: 0x040000C0 RID: 192
	public Light dirLight;

	// Token: 0x040000C1 RID: 193
	public Color lightBright = Color.grey;

	// Token: 0x040000C2 RID: 194
	public Color lightDark = Color.black;

	// Token: 0x040000C3 RID: 195
	public float minLightIntensity = 0.2f;

	// Token: 0x040000C4 RID: 196
	public float maxLightIntensity = 0.85f;

	// Token: 0x040000C5 RID: 197
	private float curIntensity;

	// Token: 0x040000C6 RID: 198
	public bool useRenderFog = true;

	// Token: 0x040000C7 RID: 199
	public Color fogBright = Color.grey;

	// Token: 0x040000C8 RID: 200
	public Color fogDark = Color.black;

	// Token: 0x040000C9 RID: 201
	public float minFog = 0.004f;

	// Token: 0x040000CA RID: 202
	public float maxFog = 0.02f;

	// Token: 0x040000CB RID: 203
	public AtmoXfade.FadeState fadeState = AtmoXfade.FadeState.FadeBright;

	// Token: 0x040000CC RID: 204
	public float fadeTime = 80f;

	// Token: 0x02000025 RID: 37
	public enum FadeState
	{
		// Token: 0x040000CE RID: 206
		FadeDark,
		// Token: 0x040000CF RID: 207
		FadeBright
	}
}
