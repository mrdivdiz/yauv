using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Pixelate")]
public class CC_Pixelate : CC_Base
{
	// Token: 0x06000167 RID: 359 RVA: 0x0000AD1C File Offset: 0x00008F1C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_scale", this.scale);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040001AD RID: 429
	public float scale = 80f;
}
