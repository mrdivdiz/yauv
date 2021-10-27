using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class SimpleShine_Controller : MonoBehaviour
{
	// Token: 0x060001AE RID: 430 RVA: 0x0000E404 File Offset: 0x0000C604
	private void Start()
	{
		if (this.ss_renderer)
		{
			this._material = this.ss_renderer.material;
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000E428 File Offset: 0x0000C628
	private void Update()
	{
		if (!this._material || this.offsetCurve == null)
		{
			return;
		}
		this._material.SetFloat("_Offset", this.offsetCurve.Evaluate(this._delta));
		this._delta += Time.deltaTime;
		if (this._delta > this.offsetCurve[this.offsetCurve.length - 1].time)
		{
			this._delta -= this.offsetCurve[this.offsetCurve.length - 1].time;
		}
	}

	// Token: 0x04000229 RID: 553
	public Renderer ss_renderer;

	// Token: 0x0400022A RID: 554
	public AnimationCurve offsetCurve;

	// Token: 0x0400022B RID: 555
	private Material _material;

	// Token: 0x0400022C RID: 556
	private float _delta;
}
