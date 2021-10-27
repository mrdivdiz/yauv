using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Posterize")]
public class CC_Posterize : CC_Base
{
	// Token: 0x06000169 RID: 361 RVA: 0x0000AD5C File Offset: 0x00008F5C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_levels", (float)this.levels);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x040001AE RID: 430
	public int levels = 4;
}
