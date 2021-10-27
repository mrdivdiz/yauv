using System;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class Skidmarks : MonoBehaviour
{
	// Token: 0x06000B20 RID: 2848 RVA: 0x000881B0 File Offset: 0x000863B0
	private void Awake()
	{
		this.skidmarks = new Skidmarks.MarkSection[this.maxMarks];
		for (int i = 0; i < this.maxMarks; i++)
		{
			this.skidmarks[i] = new Skidmarks.MarkSection();
		}
		if (((MeshFilter)base.GetComponent(typeof(MeshFilter))).mesh == null)
		{
			((MeshFilter)base.GetComponent(typeof(MeshFilter))).mesh = new Mesh();
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x00088238 File Offset: 0x00086438
	public int AddSkidMark(Vector3 pos, Vector3 normal, float intensity, int lastIndex)
	{
		if (intensity > 1f)
		{
			intensity = 1f;
		}
		if (intensity <= 0f)
		{
			return -1;
		}
		Skidmarks.MarkSection markSection = this.skidmarks[this.numMarks % this.maxMarks];
		markSection.pos = pos + normal * this.groundOffset;
		markSection.normal = normal;
		markSection.intensity = intensity;
		markSection.lastIndex = lastIndex;
		if (lastIndex != -1)
		{
			Skidmarks.MarkSection markSection2 = this.skidmarks[lastIndex % this.maxMarks];
			Vector3 lhs = markSection.pos - markSection2.pos;
			Vector3 normalized = Vector3.Cross(lhs, normal).normalized;
			markSection.posl = markSection.pos + normalized * this.markWidth * 0.5f;
			markSection.posr = markSection.pos - normalized * this.markWidth * 0.5f;
			markSection.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			if (markSection2.lastIndex == -1)
			{
				markSection2.tangent = markSection.tangent;
				markSection2.posl = markSection.pos + normalized * this.markWidth * 0.5f;
				markSection2.posr = markSection.pos - normalized * this.markWidth * 0.5f;
			}
		}
		this.numMarks++;
		this.updated = true;
		return this.numMarks - 1;
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x000883DC File Offset: 0x000865DC
	private void LateUpdate()
	{
		if (!this.updated)
		{
			return;
		}
		this.updated = false;
		Mesh mesh = ((MeshFilter)base.GetComponent(typeof(MeshFilter))).mesh;
		mesh.Clear();
		int num = 0;
		int num2 = 0;
		while (num2 < this.numMarks && num2 < this.maxMarks)
		{
			if (this.skidmarks[num2].lastIndex != -1 && this.skidmarks[num2].lastIndex > this.numMarks - this.maxMarks)
			{
				num++;
			}
			num2++;
		}
		Vector3[] array = new Vector3[num * 4];
		Vector3[] array2 = new Vector3[num * 4];
		Vector4[] array3 = new Vector4[num * 4];
		Color[] array4 = new Color[num * 4];
		Vector2[] array5 = new Vector2[num * 4];
		int[] array6 = new int[num * 6];
		num = 0;
		int num3 = 0;
		while (num3 < this.numMarks && num3 < this.maxMarks)
		{
			if (this.skidmarks[num3].lastIndex != -1 && this.skidmarks[num3].lastIndex > this.numMarks - this.maxMarks)
			{
				Skidmarks.MarkSection markSection = this.skidmarks[num3];
				Skidmarks.MarkSection markSection2 = this.skidmarks[markSection.lastIndex % this.maxMarks];
				array[num * 4] = markSection2.posl;
				array[num * 4 + 1] = markSection2.posr;
				array[num * 4 + 2] = markSection.posl;
				array[num * 4 + 3] = markSection.posr;
				array2[num * 4] = markSection2.normal;
				array2[num * 4 + 1] = markSection2.normal;
				array2[num * 4 + 2] = markSection.normal;
				array2[num * 4 + 3] = markSection.normal;
				array3[num * 4] = markSection2.tangent;
				array3[num * 4 + 1] = markSection2.tangent;
				array3[num * 4 + 2] = markSection.tangent;
				array3[num * 4 + 3] = markSection.tangent;
				array4[num * 4] = new Color(0f, 0f, 0f, markSection2.intensity);
				array4[num * 4 + 1] = new Color(0f, 0f, 0f, markSection2.intensity);
				array4[num * 4 + 2] = new Color(0f, 0f, 0f, markSection.intensity);
				array4[num * 4 + 3] = new Color(0f, 0f, 0f, markSection.intensity);
				array5[num * 4] = new Vector2(0f, 0f);
				array5[num * 4 + 1] = new Vector2(1f, 0f);
				array5[num * 4 + 2] = new Vector2(0f, 1f);
				array5[num * 4 + 3] = new Vector2(1f, 1f);
				array6[num * 6] = num * 4;
				array6[num * 6 + 2] = num * 4 + 1;
				array6[num * 6 + 1] = num * 4 + 2;
				array6[num * 6 + 3] = num * 4 + 2;
				array6[num * 6 + 5] = num * 4 + 1;
				array6[num * 6 + 4] = num * 4 + 3;
				num++;
			}
			num3++;
		}
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.tangents = array3;
		mesh.triangles = array6;
		mesh.colors = array4;
		mesh.uv = array5;
	}

	// Token: 0x04001355 RID: 4949
	public int maxMarks = 1024;

	// Token: 0x04001356 RID: 4950
	public float markWidth = 0.275f;

	// Token: 0x04001357 RID: 4951
	public float groundOffset = 0.02f;

	// Token: 0x04001358 RID: 4952
	public float minDistance = 0.1f;

	// Token: 0x04001359 RID: 4953
	private int indexShift;

	// Token: 0x0400135A RID: 4954
	private int numMarks;

	// Token: 0x0400135B RID: 4955
	private Skidmarks.MarkSection[] skidmarks;

	// Token: 0x0400135C RID: 4956
	private bool updated;

	// Token: 0x0200024D RID: 589
	private class MarkSection
	{
		// Token: 0x0400135D RID: 4957
		public Vector3 pos = Vector3.zero;

		// Token: 0x0400135E RID: 4958
		public Vector3 normal = Vector3.zero;

		// Token: 0x0400135F RID: 4959
		public Vector4 tangent = Vector4.zero;

		// Token: 0x04001360 RID: 4960
		public Vector3 posl = Vector3.zero;

		// Token: 0x04001361 RID: 4961
		public Vector3 posr = Vector3.zero;

		// Token: 0x04001362 RID: 4962
		public float intensity;

		// Token: 0x04001363 RID: 4963
		public int lastIndex;
	}
}
