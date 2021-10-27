using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Photo Filter")]
public class CC_PhotoFilter : CC_Base
{
	// Token: 0x06000165 RID: 357 RVA: 0x0000ACC0 File Offset: 0x00008EC0
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetColor("_rgb", this.color);
		base.material.SetFloat("_density", this.density);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040001AB RID: 427
	public Color color = new Color(1f, 0.5f, 0.2f, 1f);

	// Token: 0x040001AC RID: 428
	public float density = 0.35f;
}
