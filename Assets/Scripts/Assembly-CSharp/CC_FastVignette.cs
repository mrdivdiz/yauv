using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Fast Vignette")]
public class CC_FastVignette : CC_Base
{
	// Token: 0x06000159 RID: 345 RVA: 0x0000A6BC File Offset: 0x000088BC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_sharpness", this.sharpness * 0.01f);
		base.material.SetFloat("_darkness", this.darkness * 0.02f);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000187 RID: 391
	public float sharpness = 10f;

	// Token: 0x04000188 RID: 392
	public float darkness = 30f;
}
