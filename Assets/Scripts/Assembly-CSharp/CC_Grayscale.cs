using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Grayscale")]
public class CC_Grayscale : CC_Base
{
	// Token: 0x0600015D RID: 349 RVA: 0x0000A820 File Offset: 0x00008A20
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_rLum", this.redLuminance);
		base.material.SetFloat("_gLum", this.greenLuminance);
		base.material.SetFloat("_bLum", this.blueLuminance);
		base.material.SetFloat("_amount", this.amount);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x0400018D RID: 397
	public float redLuminance = 0.3f;

	// Token: 0x0400018E RID: 398
	public float greenLuminance = 0.59f;

	// Token: 0x0400018F RID: 399
	public float blueLuminance = 0.11f;

	// Token: 0x04000190 RID: 400
	public float amount = 1f;
}
