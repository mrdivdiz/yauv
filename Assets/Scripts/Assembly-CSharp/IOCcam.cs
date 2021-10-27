using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class IOCcam : MonoBehaviour
{
	// Token: 0x06000316 RID: 790 RVA: 0x00017780 File Offset: 0x00015980
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
		this.hit = default(RaycastHit);
		if (this.viewDistance == 0f)
		{
			this.viewDistance = 100f;
		}
		this.cam.farClipPlane = this.viewDistance;
		this.haltonIndex = 0;
		if (base.GetComponent<SphereCollider>() == null)
		{
			base.gameObject.AddComponent<SphereCollider>().GetComponent<Collider>().isTrigger = true;
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00017804 File Offset: 0x00015A04
	private void Start()
	{
		this.pixels = Mathf.FloorToInt((float)(Screen.width * Screen.height) / 2f);
		this.hx = new float[this.pixels];
		this.hy = new float[this.pixels];
		for (int i = 0; i < this.pixels; i++)
		{
			this.hx[i] = this.HaltonSequence(i, 2);
			this.hy[i] = this.HaltonSequence(i, 3);
		}
		foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
		{
			if (gameObject.tag == this.iocTag && gameObject.GetComponent<IOClod>() == null)
			{
				gameObject.AddComponent<IOClod>();
			}
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x000178E4 File Offset: 0x00015AE4
	private void Update()
	{
		for (int i = 0; i <= this.samples; i++)
		{
			this.r = this.cam.ViewportPointToRay(new Vector3(this.hx[this.haltonIndex], this.hy[this.haltonIndex], 0f));
			this.haltonIndex++;
			if (this.haltonIndex >= this.pixels)
			{
				this.haltonIndex = 0;
			}
			if (Physics.Raycast(this.r, out this.hit, this.viewDistance, this.layerMsk.value))
			{
				if (this.l = this.hit.transform.GetComponent<IOClod>())
				{
					this.l.UnHide(this.hit);
				}
				else if (this.l = this.hit.transform.parent.GetComponent<IOClod>())
				{
					this.l.UnHide(this.hit);
				}
			}
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x000179FC File Offset: 0x00015BFC
	private float HaltonSequence(int index, int b)
	{
		float num = 0f;
		float num2 = 1f / (float)b;
		int i = index;
		while (i > 0)
		{
			num += num2 * (float)(i % b);
			i = Mathf.FloorToInt((float)(i / b));
			num2 /= (float)b;
		}
		return num;
	}

	// Token: 0x04000393 RID: 915
	public LayerMask layerMsk;

	// Token: 0x04000394 RID: 916
	public string iocTag;

	// Token: 0x04000395 RID: 917
	public int samples;

	// Token: 0x04000396 RID: 918
	public float viewDistance;

	// Token: 0x04000397 RID: 919
	public int hideDelay;

	// Token: 0x04000398 RID: 920
	public bool realtimeShadows;

	// Token: 0x04000399 RID: 921
	public float lod1Distance;

	// Token: 0x0400039A RID: 922
	public float lod2Distance;

	// Token: 0x0400039B RID: 923
	public float lodMargin;

	// Token: 0x0400039C RID: 924
	private RaycastHit hit;

	// Token: 0x0400039D RID: 925
	private Ray r;

	// Token: 0x0400039E RID: 926
	private int layerMask;

	// Token: 0x0400039F RID: 927
	private IOClod l;

	// Token: 0x040003A0 RID: 928
	private int haltonIndex;

	// Token: 0x040003A1 RID: 929
	private float[] hx;

	// Token: 0x040003A2 RID: 930
	private float[] hy;

	// Token: 0x040003A3 RID: 931
	private int pixels;

	// Token: 0x040003A4 RID: 932
	private Camera cam;
}
