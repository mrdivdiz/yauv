using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Radial Blur")]
[RequireComponent(typeof(Camera))]
public class CC_RadialBlur : MonoBehaviour
{
	// Token: 0x0600016D RID: 365 RVA: 0x0000AE34 File Offset: 0x00009034
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000AE48 File Offset: 0x00009048
	private bool CheckShader()
	{
		return this._currentShader && this._currentShader.isSupported;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000AE70 File Offset: 0x00009070
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetFloat("amount", this.amount);
		this._material.SetVector("center", this.center);
		if (!this.CheckShader())
		{
			Graphics.Blit(source, destination);
			return;
		}
		Graphics.Blit(source, destination, this._material);
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000170 RID: 368 RVA: 0x0000AED0 File Offset: 0x000090D0
	private Material material
	{
		get
		{
			if (this.quality == 0)
			{
				this._currentShader = this.shaderLow;
			}
			else if (this.quality == 1)
			{
				this._currentShader = this.shaderMed;
			}
			else if (this.quality == 2)
			{
				this._currentShader = this.shaderHigh;
			}
			if (this._material == null)
			{
				this._material = new Material(this._currentShader);
				this._material.hideFlags = HideFlags.HideAndDontSave;
			}
			else
			{
				this._material.shader = this._currentShader;
			}
			return this._material;
		}
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000AF7C File Offset: 0x0000917C
	private void OnDisable()
	{
		if (this._material)
		{
			UnityEngine.Object.DestroyImmediate(this._material);
		}
	}

	// Token: 0x040001B1 RID: 433
	public float amount = 0.1f;

	// Token: 0x040001B2 RID: 434
	public Vector2 center = new Vector2(0.5f, 0.5f);

	// Token: 0x040001B3 RID: 435
	public int quality = 1;

	// Token: 0x040001B4 RID: 436
	public Shader shaderLow;

	// Token: 0x040001B5 RID: 437
	public Shader shaderMed;

	// Token: 0x040001B6 RID: 438
	public Shader shaderHigh;

	// Token: 0x040001B7 RID: 439
	private Shader _currentShader;

	// Token: 0x040001B8 RID: 440
	private Material _material;
}
