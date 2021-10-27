using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Hue, Saturation, Value")]
public class CC_HueSaturationValue : CC_Base
{
	// Token: 0x0600015F RID: 351 RVA: 0x0000A89C File Offset: 0x00008A9C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_hue", this.hue / 360f);
		base.material.SetFloat("_saturation", this.saturation * 0.01f);
		base.material.SetFloat("_value", this.value * 0.01f);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000191 RID: 401
	public float hue;

	// Token: 0x04000192 RID: 402
	public float saturation;

	// Token: 0x04000193 RID: 403
	public float value;
}
