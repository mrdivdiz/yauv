using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
[AddComponentMenu("Colorful/LED")]
[ExecuteInEditMode]
public class CC_Led : CC_Base
{
	// Token: 0x06000161 RID: 353 RVA: 0x0000A92C File Offset: 0x00008B2C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_scale", this.scale);
		base.material.SetFloat("_brightness", this.brightness);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000194 RID: 404
	public float scale = 80f;

	// Token: 0x04000195 RID: 405
	public float brightness = 1f;
}
