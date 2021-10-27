using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
[Serializable]
public class ObliqueNear : MonoBehaviour
{
	// Token: 0x060000DF RID: 223 RVA: 0x000053E0 File Offset: 0x000035E0
	public virtual Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(Mathf.Sign(clipPlane.x), Mathf.Sign(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
		return projection;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000054C8 File Offset: 0x000036C8
	public virtual void OnPreCull()
	{
		Matrix4x4 projectionMatrix = this.GetComponent<Camera>().projectionMatrix;
		Matrix4x4 worldToCameraMatrix = this.GetComponent<Camera>().worldToCameraMatrix;
		Vector3 rhs = worldToCameraMatrix.MultiplyPoint(this.plane.position);
		Vector3 vector = worldToCameraMatrix.MultiplyVector(-Vector3.up);
		vector.Normalize();
		Vector4 clipPlane = vector;
		clipPlane.w = -Vector3.Dot(vector, rhs);
		this.GetComponent<Camera>().projectionMatrix = this.CalculateObliqueMatrix(projectionMatrix, clipPlane);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005544 File Offset: 0x00003744
	public virtual void Main()
	{
	}

	// Token: 0x04000099 RID: 153
	public Transform plane;
}
