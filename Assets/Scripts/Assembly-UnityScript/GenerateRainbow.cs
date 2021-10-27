using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
[Serializable]
public class GenerateRainbow : MonoBehaviour
{
	// Token: 0x0600013A RID: 314 RVA: 0x0000777C File Offset: 0x0000597C
	public virtual void New()
	{
		LineRenderer lineRenderer = (LineRenderer)this.GetComponent(typeof(LineRenderer));
		lineRenderer.SetVertexCount(this.segments);
		int i = 0;
		for (i = (int)((float)0); i < this.segments; i++)
		{
			if (this.curveTypeA)
			{
				lineRenderer.SetPosition(i, new Vector3((float)0, (float)i * this.ySpacing, (float)(i * i) * this.zSpacing));
			}
			else
			{
				lineRenderer.SetPosition(i, new Vector3((float)0, (float)(-(float)i * (i - this.segments) / (3 * this.segments)) * this.ySpacing, (float)i * this.zSpacing));
			}
		}
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00007834 File Offset: 0x00005A34
	public virtual void Main()
	{
	}

	// Token: 0x0400011F RID: 287
	public int segments;

	// Token: 0x04000120 RID: 288
	public float zSpacing;

	// Token: 0x04000121 RID: 289
	public float ySpacing;

	// Token: 0x04000122 RID: 290
	public bool curveTypeA;
}
