using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
[Serializable]
public class RotationConstraint : MonoBehaviour
{
	// Token: 0x060000ED RID: 237 RVA: 0x00005A94 File Offset: 0x00003C94
	public virtual void Start()
	{
		this.thisTransform = this.transform;
		ConstraintAxis constraintAxis = this.axis;
		if (constraintAxis == ConstraintAxis.X)
		{
			this.rotateAround = Vector3.right;
		}
		else if (constraintAxis == ConstraintAxis.Y)
		{
			this.rotateAround = Vector3.up;
		}
		else if (constraintAxis == ConstraintAxis.Z)
		{
			this.rotateAround = Vector3.forward;
		}
		Quaternion lhs = Quaternion.AngleAxis(this.thisTransform.localRotation.eulerAngles[(int)this.axis], this.rotateAround);
		this.minQuaternion = lhs * Quaternion.AngleAxis(this.min, this.rotateAround);
		this.maxQuaternion = lhs * Quaternion.AngleAxis(this.max, this.rotateAround);
		this.range = this.max - this.min;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00005B74 File Offset: 0x00003D74
	public virtual void LateUpdate()
	{
		Quaternion localRotation = this.thisTransform.localRotation;
		Quaternion a = Quaternion.AngleAxis(localRotation.eulerAngles[(int)this.axis], this.rotateAround);
		float num = Quaternion.Angle(a, this.minQuaternion);
		float num2 = Quaternion.Angle(a, this.maxQuaternion);
		if (num > this.range || num2 > this.range)
		{
			Vector3 eulerAngles = localRotation.eulerAngles;
			if (num > num2)
			{
				eulerAngles[(int)this.axis] = this.maxQuaternion.eulerAngles[(int)this.axis];
			}
			else
			{
				eulerAngles[(int)this.axis] = this.minQuaternion.eulerAngles[(int)this.axis];
			}
			this.thisTransform.localEulerAngles = eulerAngles;
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00005C58 File Offset: 0x00003E58
	public virtual void Main()
	{
	}

	// Token: 0x040000AF RID: 175
	public ConstraintAxis axis;

	// Token: 0x040000B0 RID: 176
	public float min;

	// Token: 0x040000B1 RID: 177
	public float max;

	// Token: 0x040000B2 RID: 178
	private Transform thisTransform;

	// Token: 0x040000B3 RID: 179
	private Vector3 rotateAround;

	// Token: 0x040000B4 RID: 180
	private Quaternion minQuaternion;

	// Token: 0x040000B5 RID: 181
	private Quaternion maxQuaternion;

	// Token: 0x040000B6 RID: 182
	private float range;
}
