using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public struct AnimInfo
{
	// Token: 0x040000FD RID: 253
	public float currentNormalizedTime;

	// Token: 0x040000FE RID: 254
	public float previousNormalizedTime;

	// Token: 0x040000FF RID: 255
	public float currentWeight;

	// Token: 0x04000100 RID: 256
	public float contributingWeight;

	// Token: 0x04000101 RID: 257
	public Vector3 currentPosition;

	// Token: 0x04000102 RID: 258
	public Vector3 previousPosition;

	// Token: 0x04000103 RID: 259
	public Vector3 startPosition;

	// Token: 0x04000104 RID: 260
	public Vector3 endPosition;

	// Token: 0x04000105 RID: 261
	public Vector3 currentAxis;

	// Token: 0x04000106 RID: 262
	public Vector3 previousAxis;

	// Token: 0x04000107 RID: 263
	public Vector3 startAxis;

	// Token: 0x04000108 RID: 264
	public Vector3 endAxis;

	// Token: 0x04000109 RID: 265
	public Quaternion totalRotation;
}
