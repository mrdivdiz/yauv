using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[ExecuteInEditMode]
public class WaterBase : MonoBehaviour
{
	// Token: 0x060000A1 RID: 161 RVA: 0x00007750 File Offset: 0x00005950
	public void UpdateShader()
	{
		if (this.waterQuality > WaterQuality.Medium)
		{
			this.sharedMaterial.shader.maximumLOD = 501;
		}
		else if (this.waterQuality > WaterQuality.Low)
		{
			this.sharedMaterial.shader.maximumLOD = 301;
		}
		else
		{
			this.sharedMaterial.shader.maximumLOD = 201;
		}
		if (this.edgeBlend)
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_ON");
			Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
			if (Camera.main)
			{
				Camera.main.depthTextureMode |= DepthTextureMode.Depth;
			}
		}
		else
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
			Shader.DisableKeyword("WATER_EDGEBLEND_ON");
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00007818 File Offset: 0x00005A18
	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		if (currentCam && this.edgeBlend)
		{
			currentCam.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x0000784C File Offset: 0x00005A4C
	public void Update()
	{
		if (this.sharedMaterial)
		{
			this.UpdateShader();
		}
	}

	// Token: 0x040000CD RID: 205
	public Material sharedMaterial;

	// Token: 0x040000CE RID: 206
	public WaterQuality waterQuality = WaterQuality.High;

	// Token: 0x040000CF RID: 207
	public bool edgeBlend = true;
}
