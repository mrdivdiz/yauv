using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
[Serializable]
public class ZoomCamera : MonoBehaviour
{
	// Token: 0x060000F4 RID: 244 RVA: 0x00005D34 File Offset: 0x00003F34
	public ZoomCamera()
	{
		this.zoomMin = (float)-5;
		this.zoomMax = (float)5;
		this.seekTime = 1f;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00005D64 File Offset: 0x00003F64
	public virtual void Start()
	{
		this.thisTransform = this.transform;
		this.defaultLocalPosition = this.thisTransform.localPosition;
		this.currentZoom = this.zoom;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00005D90 File Offset: 0x00003F90
	public virtual void Update()
	{
		this.zoom = Mathf.Clamp(this.zoom, this.zoomMin, this.zoomMax);
		int layerMask = -261;
		RaycastHit raycastHit = default(RaycastHit);
		Vector3 position = this.origin.position;
		Vector3 vector = this.defaultLocalPosition + this.thisTransform.parent.InverseTransformDirection(this.thisTransform.forward * this.zoom);
		Vector3 end = this.thisTransform.parent.TransformPoint(vector);
		if (Physics.Linecast(position, end, out raycastHit, layerMask))
		{
			Vector3 a = raycastHit.point + this.thisTransform.TransformDirection(Vector3.forward);
			this.targetZoom = (a - this.thisTransform.parent.TransformPoint(this.defaultLocalPosition)).magnitude;
		}
		else
		{
			this.targetZoom = this.zoom;
		}
		this.targetZoom = Mathf.Clamp(this.targetZoom, this.zoomMin, this.zoomMax);
		if (!this.smoothZoomIn && this.targetZoom - this.currentZoom > (float)0)
		{
			this.currentZoom = this.targetZoom;
		}
		else
		{
			this.currentZoom = Mathf.SmoothDamp(this.currentZoom, this.targetZoom, ref this.zoomVelocity, this.seekTime);
		}
		vector = this.defaultLocalPosition + this.thisTransform.parent.InverseTransformDirection(this.thisTransform.forward * this.currentZoom);
		this.thisTransform.localPosition = vector;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00005F34 File Offset: 0x00004134
	public virtual void Main()
	{
	}

	// Token: 0x040000BC RID: 188
	public Transform origin;

	// Token: 0x040000BD RID: 189
	public float zoom;

	// Token: 0x040000BE RID: 190
	public float zoomMin;

	// Token: 0x040000BF RID: 191
	public float zoomMax;

	// Token: 0x040000C0 RID: 192
	public float seekTime;

	// Token: 0x040000C1 RID: 193
	public bool smoothZoomIn;

	// Token: 0x040000C2 RID: 194
	private Vector3 defaultLocalPosition;

	// Token: 0x040000C3 RID: 195
	private Transform thisTransform;

	// Token: 0x040000C4 RID: 196
	private float currentZoom;

	// Token: 0x040000C5 RID: 197
	private float targetZoom;

	// Token: 0x040000C6 RID: 198
	private float zoomVelocity;
}
