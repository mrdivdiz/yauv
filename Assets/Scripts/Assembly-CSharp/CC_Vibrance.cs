using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vibrance")]
public class CC_Vibrance : CC_Base
{
	// Token: 0x06000177 RID: 375 RVA: 0x0000B090 File Offset: 0x00009290
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_amount", this.amount * 0.02f);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040001BC RID: 444
	public float amount;
}
