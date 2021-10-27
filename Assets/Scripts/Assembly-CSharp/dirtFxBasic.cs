using System;
using UnityEngine;

// Token: 0x02000231 RID: 561
[ExecuteInEditMode]
public class dirtFxBasic : MonoBehaviour
{
	// Token: 0x060009F7 RID: 2551 RVA: 0x0008B6E4 File Offset: 0x000898E4
	private Material GetMaterial()
	{
		if (this.rbMaterial == null)
		{
			this.rbMaterial = new Material(this.shader);
			this.rbMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
		return this.rbMaterial;
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0008B71C File Offset: 0x0008991C
	protected Material blurMaterial
	{
		get
		{
			if (this.m_BlurMaterial == null)
			{
				this.m_BlurMaterial = new Material(this.blurShader);
				this.m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_BlurMaterial;
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0008B754 File Offset: 0x00089954
	protected void OnDisable()
	{
		if (this.m_BlurMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.m_BlurMaterial);
		}
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0008B774 File Offset: 0x00089974
	private void Start()
	{
		if (this.shader == null)
		{
			Debug.LogError("No glare shader assigned!", this);
			base.enabled = false;
		}
		if (this.blurShader == null)
		{
			Debug.LogError("No blur shader assigned!", this);
			base.enabled = false;
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0008B7C8 File Offset: 0x000899C8
	public void FourTapCone(RenderTexture source, RenderTexture dest, float off)
	{
		Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
		{
			new Vector2(-off, -off),
			new Vector2(-off, off),
			new Vector2(off, off),
			new Vector2(off, -off)
		});
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0008B838 File Offset: 0x00089A38
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		this.threshold = Mathf.Clamp01(this.threshold);
		this.intensity = Mathf.Clamp(this.intensity, 0f, 10f);
		this.blurIteration = Mathf.Clamp(this.blurIteration, 1, 1000);
		this.downSample = Mathf.Clamp(this.downSample, 1, 16);
		this.GetMaterial().SetFloat("_int", this.intensity);
		this.GetMaterial().SetTexture("_OrgTex", source);
		if (this.visualizeMask)
		{
			this.GetMaterial().SetTexture("_lensDirt", null);
		}
		else
		{
			this.GetMaterial().SetTexture("_lensDirt", this.lensDirt);
		}
		this.GetMaterial().SetFloat("_threshold", this.threshold);
		this.GetMaterial().SetColor("tintColor", this.tintColor);
		if (this.visualizeMask)
		{
			this.GetMaterial().SetFloat("_visualize", 0f);
		}
		else
		{
			this.GetMaterial().SetFloat("_visualize", 1f);
		}
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / this.downSample, source.height / this.downSample, 0);
		Graphics.Blit(source, temporary, this.blurMaterial);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
		RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, 0);
		Graphics.Blit(temporary, temporary2, this.GetMaterial(), 0);
		bool flag = true;
		for (int i = 0; i < this.blurIteration; i++)
		{
			if (flag)
			{
				this.FourTapCone(temporary2, temporary3, 0.5f + (float)i * 0.5f);
			}
			else
			{
				this.FourTapCone(temporary3, temporary2, 0.5f + (float)i * 0.5f);
			}
			flag = !flag;
		}
		if (flag)
		{
			Graphics.Blit(temporary2, dest, this.GetMaterial(), 1);
		}
		else
		{
			Graphics.Blit(temporary3, dest, this.GetMaterial(), 1);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(temporary3);
	}

	// Token: 0x040014EE RID: 5358
	public Color tintColor = Color.white;

	// Token: 0x040014EF RID: 5359
	public float intensity = 1f;

	// Token: 0x040014F0 RID: 5360
	public float threshold = 0.5f;

	// Token: 0x040014F1 RID: 5361
	public int blurIteration = 2;

	// Token: 0x040014F2 RID: 5362
	public int downSample = 4;

	// Token: 0x040014F3 RID: 5363
	public bool visualizeMask;

	// Token: 0x040014F4 RID: 5364
	public Texture2D lensDirt;

	// Token: 0x040014F5 RID: 5365
	public Shader shader;

	// Token: 0x040014F6 RID: 5366
	private Material rbMaterial;

	// Token: 0x040014F7 RID: 5367
	public Shader blurShader;

	// Token: 0x040014F8 RID: 5368
	private Material m_BlurMaterial;
}
