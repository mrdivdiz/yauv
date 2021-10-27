using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
[Serializable]
public class BendingSegment
{
	// Token: 0x04000DA5 RID: 3493
	public Transform firstTransform;

	// Token: 0x04000DA6 RID: 3494
	public Transform lastTransform;

	// Token: 0x04000DA7 RID: 3495
	public float thresholdAngleDifference;

	// Token: 0x04000DA8 RID: 3496
	public float bendingMultiplier = 0.6f;

	// Token: 0x04000DA9 RID: 3497
	public float maxAngleDifference = 30f;

	// Token: 0x04000DAA RID: 3498
	public float maxBendingAngle = 80f;

	// Token: 0x04000DAB RID: 3499
	public float responsiveness = 5f;

	// Token: 0x04000DAC RID: 3500
	internal float angleH;

	// Token: 0x04000DAD RID: 3501
	internal float angleV;

	// Token: 0x04000DAE RID: 3502
	internal Vector3 dirUp;

	// Token: 0x04000DAF RID: 3503
	internal Vector3 referenceLookDir;

	// Token: 0x04000DB0 RID: 3504
	internal Vector3 referenceUpDir;

	// Token: 0x04000DB1 RID: 3505
	internal int chainLength;

	// Token: 0x04000DB2 RID: 3506
	internal Quaternion[] origRotations;
}
