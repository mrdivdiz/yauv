using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Channel Mixer")]
public class CC_ChannelMixer : CC_Base
{
	// Token: 0x06000155 RID: 341 RVA: 0x0000A4F8 File Offset: 0x000086F8
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_red", new Vector4(this.redR * 0.01f, this.greenR * 0.01f, this.blueR * 0.01f));
		base.material.SetVector("_green", new Vector4(this.redG * 0.01f, this.greenG * 0.01f, this.blueG * 0.01f));
		base.material.SetVector("_blue", new Vector4(this.redB * 0.01f, this.greenB * 0.01f, this.blueB * 0.01f));
		base.material.SetVector("_constant", new Vector4(this.constantR * 0.01f, this.constantG * 0.01f, this.constantB * 0.01f));
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000179 RID: 377
	public float redR = 100f;

	// Token: 0x0400017A RID: 378
	public float redG;

	// Token: 0x0400017B RID: 379
	public float redB;

	// Token: 0x0400017C RID: 380
	public float greenR;

	// Token: 0x0400017D RID: 381
	public float greenG = 100f;

	// Token: 0x0400017E RID: 382
	public float greenB;

	// Token: 0x0400017F RID: 383
	public float blueR;

	// Token: 0x04000180 RID: 384
	public float blueG;

	// Token: 0x04000181 RID: 385
	public float blueB = 100f;

	// Token: 0x04000182 RID: 386
	public float constantR;

	// Token: 0x04000183 RID: 387
	public float constantG;

	// Token: 0x04000184 RID: 388
	public float constantB;
}
