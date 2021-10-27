using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Bleach Bypass")]
public class CC_BleachBypass : CC_Base
{
	// Token: 0x06000151 RID: 337 RVA: 0x0000A3A0 File Offset: 0x000085A0
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000172 RID: 370
	public float amount = 1f;
}
