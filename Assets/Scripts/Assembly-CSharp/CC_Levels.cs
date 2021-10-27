using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Levels")]
public class CC_Levels : CC_Base
{
	// Token: 0x06000163 RID: 355 RVA: 0x0000AA0C File Offset: 0x00008C0C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.mode == 0)
		{
			base.material.SetVector("_inputMin", new Vector4(this.inputMinL / 255f, this.inputMinL / 255f, this.inputMinL / 255f, 1f));
			base.material.SetVector("_inputMax", new Vector4(this.inputMaxL / 255f, this.inputMaxL / 255f, this.inputMaxL / 255f, 1f));
			base.material.SetVector("_inputGamma", new Vector4(this.inputGammaL, this.inputGammaL, this.inputGammaL, 1f));
			base.material.SetVector("_outputMin", new Vector4(this.outputMinL / 255f, this.outputMinL / 255f, this.outputMinL / 255f, 1f));
			base.material.SetVector("_outputMax", new Vector4(this.outputMaxL / 255f, this.outputMaxL / 255f, this.outputMaxL / 255f, 1f));
		}
		else
		{
			base.material.SetVector("_inputMin", new Vector4(this.inputMinR / 255f, this.inputMinG / 255f, this.inputMinB / 255f, 1f));
			base.material.SetVector("_inputMax", new Vector4(this.inputMaxR / 255f, this.inputMaxG / 255f, this.inputMaxB / 255f, 1f));
			base.material.SetVector("_inputGamma", new Vector4(this.inputGammaR, this.inputGammaG, this.inputGammaB, 1f));
			base.material.SetVector("_outputMin", new Vector4(this.outputMinR / 255f, this.outputMinG / 255f, this.outputMinB / 255f, 1f));
			base.material.SetVector("_outputMax", new Vector4(this.outputMaxR / 255f, this.outputMaxG / 255f, this.outputMaxB / 255f, 1f));
		}
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000196 RID: 406
	public int mode;

	// Token: 0x04000197 RID: 407
	public float inputMinL;

	// Token: 0x04000198 RID: 408
	public float inputMaxL = 255f;

	// Token: 0x04000199 RID: 409
	public float inputGammaL = 1f;

	// Token: 0x0400019A RID: 410
	public float inputMinR;

	// Token: 0x0400019B RID: 411
	public float inputMaxR = 255f;

	// Token: 0x0400019C RID: 412
	public float inputGammaR = 1f;

	// Token: 0x0400019D RID: 413
	public float inputMinG;

	// Token: 0x0400019E RID: 414
	public float inputMaxG = 255f;

	// Token: 0x0400019F RID: 415
	public float inputGammaG = 1f;

	// Token: 0x040001A0 RID: 416
	public float inputMinB;

	// Token: 0x040001A1 RID: 417
	public float inputMaxB = 255f;

	// Token: 0x040001A2 RID: 418
	public float inputGammaB = 1f;

	// Token: 0x040001A3 RID: 419
	public float outputMinL;

	// Token: 0x040001A4 RID: 420
	public float outputMaxL = 255f;

	// Token: 0x040001A5 RID: 421
	public float outputMinR;

	// Token: 0x040001A6 RID: 422
	public float outputMaxR = 255f;

	// Token: 0x040001A7 RID: 423
	public float outputMinG;

	// Token: 0x040001A8 RID: 424
	public float outputMaxG = 255f;

	// Token: 0x040001A9 RID: 425
	public float outputMinB;

	// Token: 0x040001AA RID: 426
	public float outputMaxB = 255f;
}
