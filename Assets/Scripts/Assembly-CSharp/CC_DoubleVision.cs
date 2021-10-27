using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Double Vision")]
public class CC_DoubleVision : CC_Base
{
	// Token: 0x06000157 RID: 343 RVA: 0x0000A62C File Offset: 0x0000882C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_displace", new Vector2(this.displace.x / (float)Screen.width, this.displace.y / (float)Screen.height));
		base.material.SetFloat("_amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04000185 RID: 389
	public Vector2 displace = new Vector2(0.7f, 0f);

	// Token: 0x04000186 RID: 390
	public float amount = 1f;
}
