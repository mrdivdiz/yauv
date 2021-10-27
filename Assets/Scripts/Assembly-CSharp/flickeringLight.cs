using System;
using UnityEngine;

// Token: 0x02000207 RID: 519
[RequireComponent(typeof(Light))]
public class flickeringLight : MonoBehaviour
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x0006FF58 File Offset: 0x0006E158
	private void Start()
	{
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0006FF5C File Offset: 0x0006E15C
	private void Update()
	{
		if (!mainmenu.pause)
		{
			flickeringLight.flickerinLightStyles flickerinLightStyles = this.flickeringLightStyle;
			if (flickerinLightStyles != flickeringLight.flickerinLightStyles.CampFire)
			{
				if (flickerinLightStyles == flickeringLight.flickerinLightStyles.Fluorescent)
				{
					if (UnityEngine.Random.Range(0f, 1f) > this.FluorescentFlicerPercent)
					{
						base.GetComponent<Light>().intensity = this.FluorescentFlickerMin;
						if (this.FluorescentFlickerPlaySound)
						{
						}
					}
					else
					{
						base.GetComponent<Light>().intensity = this.FluorescentFlickerMax;
					}
				}
			}
			else
			{
				if (this.campfireMethod == flickeringLight.campfireMethods.Intensity || this.campfireMethod == flickeringLight.campfireMethods.Both)
				{
					if (this.campfireIntesityStyle == flickeringLight.campfireIntesityStyles.Sine)
					{
						this.CampfireSineCycleIntensity += this.CampfireSineCycleIntensitySpeed;
						if (this.CampfireSineCycleIntensity > 360f)
						{
							this.CampfireSineCycleIntensity = 0f;
						}
						base.GetComponent<Light>().intensity = this.CampfireIntensityBaseValue + (Mathf.Sin(this.CampfireSineCycleIntensity * 0.017453292f) * (this.CampfireIntensityFlickerValue / 2f) + this.CampfireIntensityFlickerValue / 2f);
					}
					else
					{
						base.GetComponent<Light>().intensity = this.CampfireIntensityBaseValue + UnityEngine.Random.Range(0f, this.CampfireIntensityFlickerValue);
					}
				}
				if (this.campfireMethod == flickeringLight.campfireMethods.Range || this.campfireMethod == flickeringLight.campfireMethods.Both)
				{
					if (this.campfireRangeStyle == flickeringLight.campfireRangeStyles.Sine)
					{
						this.CampfireSineCycleRange += this.CampfireSineCycleRangeSpeed;
						if (this.CampfireSineCycleRange > 360f)
						{
							this.CampfireSineCycleRange = 0f;
						}
						base.GetComponent<Light>().range = this.CampfireRangeBaseValue + (Mathf.Sin(this.CampfireSineCycleRange * 0.017453292f) * (this.CampfireSineCycleRange / 2f) + this.CampfireSineCycleRange / 2f);
					}
					else
					{
						base.GetComponent<Light>().range = this.CampfireRangeBaseValue + UnityEngine.Random.Range(0f, this.CampfireRangeFlickerValue);
					}
				}
			}
		}
	}

	// Token: 0x0400104E RID: 4174
	public flickeringLight.flickerinLightStyles flickeringLightStyle;

	// Token: 0x0400104F RID: 4175
	public flickeringLight.campfireMethods campfireMethod;

	// Token: 0x04001050 RID: 4176
	public flickeringLight.campfireIntesityStyles campfireIntesityStyle = flickeringLight.campfireIntesityStyles.Random;

	// Token: 0x04001051 RID: 4177
	public flickeringLight.campfireRangeStyles campfireRangeStyle = flickeringLight.campfireRangeStyles.Random;

	// Token: 0x04001052 RID: 4178
	public float CampfireIntensityBaseValue = 0.5f;

	// Token: 0x04001053 RID: 4179
	public float CampfireIntensityFlickerValue = 0.1f;

	// Token: 0x04001054 RID: 4180
	public float CampfireRangeBaseValue = 10f;

	// Token: 0x04001055 RID: 4181
	public float CampfireRangeFlickerValue = 2f;

	// Token: 0x04001056 RID: 4182
	private float CampfireSineCycleIntensity;

	// Token: 0x04001057 RID: 4183
	private float CampfireSineCycleRange;

	// Token: 0x04001058 RID: 4184
	public float CampfireSineCycleIntensitySpeed = 5f;

	// Token: 0x04001059 RID: 4185
	public float CampfireSineCycleRangeSpeed = 5f;

	// Token: 0x0400105A RID: 4186
	public float FluorescentFlickerMin = 0.4f;

	// Token: 0x0400105B RID: 4187
	public float FluorescentFlickerMax = 0.5f;

	// Token: 0x0400105C RID: 4188
	public float FluorescentFlicerPercent = 0.95f;

	// Token: 0x0400105D RID: 4189
	public bool FluorescentFlickerPlaySound;

	// Token: 0x0400105E RID: 4190
	public AudioClip FluorescentFlickerAudioClip;

	// Token: 0x02000208 RID: 520
	public enum flickerinLightStyles
	{
		// Token: 0x04001060 RID: 4192
		CampFire,
		// Token: 0x04001061 RID: 4193
		Fluorescent
	}

	// Token: 0x02000209 RID: 521
	public enum campfireMethods
	{
		// Token: 0x04001063 RID: 4195
		Intensity,
		// Token: 0x04001064 RID: 4196
		Range,
		// Token: 0x04001065 RID: 4197
		Both
	}

	// Token: 0x0200020A RID: 522
	public enum campfireIntesityStyles
	{
		// Token: 0x04001067 RID: 4199
		Sine,
		// Token: 0x04001068 RID: 4200
		Random
	}

	// Token: 0x0200020B RID: 523
	public enum campfireRangeStyles
	{
		// Token: 0x0400106A RID: 4202
		Sine,
		// Token: 0x0400106B RID: 4203
		Random
	}
}
