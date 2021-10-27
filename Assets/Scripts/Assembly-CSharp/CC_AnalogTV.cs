using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Analog TV")]
public class CC_AnalogTV : CC_Base
{
	// Token: 0x0600014B RID: 331 RVA: 0x0000A204 File Offset: 0x00008404
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_phase", this.phase);
		base.material.SetFloat("_grayscale", (!this.grayscale) ? 0f : 1f);
		base.material.SetFloat("_noiseIntensity", this.noiseIntensity);
		base.material.SetFloat("_scanlinesIntensity", this.scanlinesIntensity);
		base.material.SetFloat("_scanlinesCount", (float)((int)this.scanlinesCount));
		base.material.SetFloat("_distortion", this.distortion);
		base.material.SetFloat("_cubicDistortion", this.cubicDistortion);
		base.material.SetFloat("_scale", this.scale);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000168 RID: 360
	public float phase = 0.5f;

	// Token: 0x04000169 RID: 361
	public bool grayscale;

	// Token: 0x0400016A RID: 362
	public float noiseIntensity = 0.5f;

	// Token: 0x0400016B RID: 363
	public float scanlinesIntensity = 2f;

	// Token: 0x0400016C RID: 364
	public float scanlinesCount = 768f;

	// Token: 0x0400016D RID: 365
	public float distortion = 0.2f;

	// Token: 0x0400016E RID: 366
	public float cubicDistortion = 0.6f;

	// Token: 0x0400016F RID: 367
	public float scale = 0.8f;
}
