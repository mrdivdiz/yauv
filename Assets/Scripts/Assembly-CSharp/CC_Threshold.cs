using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
[AddComponentMenu("Colorful/Threshold")]
[ExecuteInEditMode]
public class CC_Threshold : CC_Base
{
	// Token: 0x06000175 RID: 373 RVA: 0x0000B050 File Offset: 0x00009250
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_threshold", this.threshold / 255f);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040001BB RID: 443
	public float threshold = 128f;
}
