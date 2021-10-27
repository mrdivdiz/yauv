using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
[AddComponentMenu("Image Effects/Max P/Dirty Lens Flare")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DirtyLensFlare : MonoBehaviour
{
	// Token: 0x0600051E RID: 1310 RVA: 0x00021618 File Offset: 0x0001F818
	protected virtual void Start()
	{
		this.CheckResources();
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shader || !this.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600051F RID: 1311 RVA: 0x00021668 File Offset: 0x0001F868
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.shader);
				this.m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000216A0 File Offset: 0x0001F8A0
	protected virtual void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000216C0 File Offset: 0x0001F8C0
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration, Material blurMtl)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, blurMtl, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0002173C File Offset: 0x0001F93C
	private void ApplyBlurPass(RenderTexture source, RenderTexture destination, Material blurMtl)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
		Graphics.Blit(source, temporary);
		bool flag = true;
		for (int i = 0; i < this.iterations; i++)
		{
			if (flag)
			{
				this.FourTapCone(temporary, temporary2, i, blurMtl);
			}
			else
			{
				this.FourTapCone(temporary2, temporary, i, blurMtl);
			}
			flag = !flag;
		}
		if (flag)
		{
			Graphics.Blit(temporary, destination);
		}
		else
		{
			Graphics.Blit(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x000217D8 File Offset: 0x0001F9D8
	private bool CheckResources()
	{
		if (!this.blurShader)
		{
			this.blurShader = Shader.Find("Hidden/Dirty Lens Flare Blur");
			if (!this.blurShader)
			{
				return false;
			}
		}
		if (!this.blurMaterial)
		{
			this.blurMaterial = new Material(this.blurShader);
			if (!this.blurMaterial)
			{
				return false;
			}
		}
		if (!this.shader)
		{
			this.shader = Shader.Find("Hidden/Dirty Lens Flare");
			if (!this.shader)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00021880 File Offset: 0x0001FA80
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.CheckResources())
		{
			this.material.SetFloat("_Threshold", this.threshold);
			this.material.SetFloat("_Scale", this.flareIntensity);
			this.material.SetFloat("_BloomScale", this.bloomIntensity);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, RenderTextureFormat.Default);
			this.material.SetFloat("_desaturate", 1f - this.saturation);
			switch (this.lensFlareType)
			{
			case LensFlareTypes.BloomAndFlare:
				Graphics.Blit(source, temporary, this.material, 1);
				break;
			case LensFlareTypes.Bloom:
				Graphics.Blit(source, temporary, this.material, 2);
				break;
			case LensFlareTypes.Flare:
				Graphics.Blit(source, temporary, this.material, 0);
				break;
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(temporary.width, temporary.height, 0, RenderTextureFormat.Default);
			this.ApplyBlurPass(temporary, temporary2, this.blurMaterial);
			this.material.SetTexture("_Flare", temporary2);
			if (this.useDirt)
			{
				this.material.SetTexture("_Dirt", this.screenDirt);
				Graphics.Blit(source, destination, this.material, 3);
			}
			else
			{
				Graphics.Blit(source, destination, this.material, 4);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}
	}

	// Token: 0x04000558 RID: 1368
	public LensFlareTypes lensFlareType;

	// Token: 0x04000559 RID: 1369
	public float saturation = 0.9f;

	// Token: 0x0400055A RID: 1370
	public float threshold = 0.5f;

	// Token: 0x0400055B RID: 1371
	public float flareIntensity = 2.5f;

	// Token: 0x0400055C RID: 1372
	public float bloomIntensity = 2f;

	// Token: 0x0400055D RID: 1373
	public int iterations = 10;

	// Token: 0x0400055E RID: 1374
	public float blurSpread = 0.6f;

	// Token: 0x0400055F RID: 1375
	public bool useDirt = true;

	// Token: 0x04000560 RID: 1376
	public Texture2D screenDirt;

	// Token: 0x04000561 RID: 1377
	private Shader blurShader;

	// Token: 0x04000562 RID: 1378
	private Material blurMaterial;

	// Token: 0x04000563 RID: 1379
	private Shader shader;

	// Token: 0x04000564 RID: 1380
	private Material m_Material;
}
