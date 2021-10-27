using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Brightness, Contrast, Gamma")]
public class CC_BrightnessContrastGamma : CC_Base
{
	// Token: 0x06000153 RID: 339 RVA: 0x0000A410 File Offset: 0x00008610
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_rCoeff", this.redCoeff);
		base.material.SetFloat("_gCoeff", this.greenCoeff);
		base.material.SetFloat("_bCoeff", this.blueCoeff);
		base.material.SetFloat("_brightness", (this.brightness + 100f) * 0.01f);
		base.material.SetFloat("_contrast", (this.contrast + 100f) * 0.01f);
		base.material.SetFloat("_gamma", 1f / this.gamma);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000173 RID: 371
	public float redCoeff = 0.5f;

	// Token: 0x04000174 RID: 372
	public float greenCoeff = 0.5f;

	// Token: 0x04000175 RID: 373
	public float blueCoeff = 0.5f;

	// Token: 0x04000176 RID: 374
	public float brightness;

	// Token: 0x04000177 RID: 375
	public float contrast;

	// Token: 0x04000178 RID: 376
	public float gamma = 1f;
}
