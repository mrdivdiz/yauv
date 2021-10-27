using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
[AddComponentMenu("Colorful/Frost")]
[ExecuteInEditMode]
public class CC_Frost : CC_Base
{
	// Token: 0x0600015B RID: 347 RVA: 0x0000A74C File Offset: 0x0000894C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_scale", this.scale);
		base.material.SetFloat("_enableVignette", (!this.enableVignette) ? 0f : 1f);
		base.material.SetFloat("_sharpness", this.sharpness * 0.01f);
		base.material.SetFloat("_darkness", this.darkness * 0.02f);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000189 RID: 393
	public float scale = 1.2f;

	// Token: 0x0400018A RID: 394
	public float sharpness = 40f;

	// Token: 0x0400018B RID: 395
	public float darkness = 35f;

	// Token: 0x0400018C RID: 396
	public bool enableVignette = true;
}
