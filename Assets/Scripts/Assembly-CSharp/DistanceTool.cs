using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
[ExecuteInEditMode]
public class DistanceTool : MonoBehaviour
{
	// Token: 0x06000184 RID: 388 RVA: 0x0000C308 File Offset: 0x0000A508
	private void OnEnable()
	{
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000C30C File Offset: 0x0000A50C
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = this.lineColor;
		Gizmos.DrawWireSphere(this.startPoint, this.gizmoRadius);
		Gizmos.DrawWireSphere(this.endPoint, this.gizmoRadius);
		Gizmos.DrawLine(this.startPoint, this.endPoint);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000C358 File Offset: 0x0000A558
	private void OnDrawGizmos()
	{
		Gizmos.color = this.lineColor;
		Gizmos.DrawWireSphere(this.startPoint, this.gizmoRadius);
		Gizmos.DrawWireSphere(this.endPoint, this.gizmoRadius);
		Gizmos.DrawLine(this.startPoint, this.endPoint);
	}

	// Token: 0x040001E7 RID: 487
	public string distanceToolName = string.Empty;

	// Token: 0x040001E8 RID: 488
	public Color lineColor = Color.yellow;

	// Token: 0x040001E9 RID: 489
	public bool initialized;

	// Token: 0x040001EA RID: 490
	public string initialName = "Distance Tool";

	// Token: 0x040001EB RID: 491
	public Vector3 startPoint = Vector3.zero;

	// Token: 0x040001EC RID: 492
	public Vector3 endPoint = new Vector3(0f, 1f, 0f);

	// Token: 0x040001ED RID: 493
	public float distance;

	// Token: 0x040001EE RID: 494
	public float gizmoRadius = 0.1f;

	// Token: 0x040001EF RID: 495
	public bool scaleToPixels;

	// Token: 0x040001F0 RID: 496
	public int pixelPerUnit = 128;
}
