using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class AgilityLedge : MonoBehaviour
{
	// Token: 0x06000940 RID: 2368 RVA: 0x0004DEBC File Offset: 0x0004C0BC
	private void Start()
	{
		if (this.rightPoint == null)
		{
			this.rightPoint = base.transform.Find("RightPoint");
		}
		if (this.leftPoint == null)
		{
			this.leftPoint = base.transform.Find("LeftPoint");
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0004DF18 File Offset: 0x0004C118
	private void Update()
	{
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0004DF1C File Offset: 0x0004C11C
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(this.leftPoint.position, this.rightPoint.position);
	}

	// Token: 0x04000CA5 RID: 3237
	public Transform rightPoint;

	// Token: 0x04000CA6 RID: 3238
	public Transform leftPoint;

	// Token: 0x04000CA7 RID: 3239
	public AgilityLedge.LedgeTypes ledgeType;

	// Token: 0x04000CA8 RID: 3240
	public bool canClimb = true;

	// Token: 0x04000CA9 RID: 3241
	public bool canMove = true;

	// Token: 0x04000CAA RID: 3242
	public AgilityLedge leftLedge;

	// Token: 0x04000CAB RID: 3243
	public bool isLeftCorner;

	// Token: 0x04000CAC RID: 3244
	public bool isLeftCorner2;

	// Token: 0x04000CAD RID: 3245
	public AgilityLedge rightLedge;

	// Token: 0x04000CAE RID: 3246
	public bool isRightCorner;

	// Token: 0x04000CAF RID: 3247
	public AgilityLedge upperLedge;

	// Token: 0x04000CB0 RID: 3248
	public bool isRightCorner2;

	// Token: 0x04000CB1 RID: 3249
	public float upperLedgeStartPercentage;

	// Token: 0x04000CB2 RID: 3250
	public float upperLedgeEndPercentage = 100f;

	// Token: 0x04000CB3 RID: 3251
	public AgilityLedge lowerLedge;

	// Token: 0x04000CB4 RID: 3252
	public float lowerLedgeStartPercentage;

	// Token: 0x04000CB5 RID: 3253
	public float lowerLedgeEndPercentage = 100f;

	// Token: 0x04000CB6 RID: 3254
	public AgilityLedge backLedge;

	// Token: 0x04000CB7 RID: 3255
	public float backLedgeStartPercentage;

	// Token: 0x04000CB8 RID: 3256
	public float backLedgeEndPercentage = 100f;

	// Token: 0x04000CB9 RID: 3257
	public bool setDistances = true;

	// Token: 0x04000CBA RID: 3258
	public bool canDrop = true;

	// Token: 0x020001D3 RID: 467
	public enum LedgeTypes
	{
		// Token: 0x04000CBC RID: 3260
		LEDGE1,
		// Token: 0x04000CBD RID: 3261
		LEDGE2
	}
}
