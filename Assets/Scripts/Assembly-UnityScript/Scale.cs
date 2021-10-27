using System;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x02000063 RID: 99
[Serializable]
public class Scale : MonoBehaviour
{
	// Token: 0x0600012D RID: 301 RVA: 0x000073CC File Offset: 0x000055CC
	public Scale()
	{
		this.scale = (float)1;
		this.firstUpdate = true;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x000073E4 File Offset: 0x000055E4
	public virtual void UpdateScale()
	{
		int num = Extensions.get_length(this.particleEmitters);
		if (this.firstUpdate)
		{
			this.minsize = new float[num];
			this.maxsize = new float[num];
			this.worldvelocity = new Vector3[num];
			this.localvelocity = new Vector3[num];
			this.rndvelocity = new Vector3[num];
			this.scaleBackUp = new Vector3[num];
		}
		int i = 0;
		for (i = 0; i < Extensions.get_length(this.particleEmitters); i++)
		{
			if (this.firstUpdate)
			{
				this.minsize[i] = this.particleEmitters[i].minSize;
				this.maxsize[i] = this.particleEmitters[i].maxSize;
				this.worldvelocity[i] = this.particleEmitters[i].worldVelocity;
				this.localvelocity[i] = this.particleEmitters[i].localVelocity;
				this.rndvelocity[i] = this.particleEmitters[i].rndVelocity;
				this.scaleBackUp[i] = this.particleEmitters[i].transform.localScale;
			}
			this.particleEmitters[i].minSize = this.minsize[i] * this.scale;
			this.particleEmitters[i].maxSize = this.maxsize[i] * this.scale;
			this.particleEmitters[i].worldVelocity = this.worldvelocity[i] * this.scale;
			this.particleEmitters[i].localVelocity = this.localvelocity[i] * this.scale;
			this.particleEmitters[i].rndVelocity = this.rndvelocity[i] * this.scale;
			this.particleEmitters[i].transform.localScale = this.scaleBackUp[i] * this.scale;
		}
		this.firstUpdate = false;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00007610 File Offset: 0x00005810
	public virtual void Main()
	{
	}

	// Token: 0x04000112 RID: 274
	public ParticleEmitter[] particleEmitters;

	// Token: 0x04000113 RID: 275
	public float scale;

	// Token: 0x04000114 RID: 276
	[HideInInspector]
	[SerializeField]
	private float[] minsize;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	[HideInInspector]
	private float[] maxsize;

	// Token: 0x04000116 RID: 278
	[HideInInspector]
	[SerializeField]
	private Vector3[] worldvelocity;

	// Token: 0x04000117 RID: 279
	[HideInInspector]
	[SerializeField]
	private Vector3[] localvelocity;

	// Token: 0x04000118 RID: 280
	[HideInInspector]
	[SerializeField]
	private Vector3[] rndvelocity;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	[HideInInspector]
	private Vector3[] scaleBackUp;

	// Token: 0x0400011A RID: 282
	[SerializeField]
	[HideInInspector]
	private bool firstUpdate;
}
