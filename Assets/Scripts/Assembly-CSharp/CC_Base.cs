using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public class CC_Base : MonoBehaviour
{
	// Token: 0x0600014D RID: 333 RVA: 0x0000A2EC File Offset: 0x000084EC
	protected virtual void Start()
	{
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

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600014E RID: 334 RVA: 0x0000A334 File Offset: 0x00008534
	protected Material material
	{
		get
		{
			if (this._material == null)
			{
				this._material = new Material(this.shader);
				this._material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this._material;
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000A36C File Offset: 0x0000856C
	protected virtual void OnDisable()
	{
		if (this._material)
		{
			UnityEngine.Object.DestroyImmediate(this._material);
		}
	}

	// Token: 0x04000170 RID: 368
	public Shader shader;

	// Token: 0x04000171 RID: 369
	protected Material _material;
}
