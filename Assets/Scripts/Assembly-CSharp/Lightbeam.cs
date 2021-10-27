using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
[ExecuteInEditMode]
public class Lightbeam : MonoBehaviour
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
	// (set) Token: 0x060004B4 RID: 1204 RVA: 0x0001E6B4 File Offset: 0x0001C8B4
	public float RadiusTop
	{
		get
		{
			return this.Settings.RadiusTop;
		}
		set
		{
			this.Settings.RadiusTop = value;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001E6C4 File Offset: 0x0001C8C4
	// (set) Token: 0x060004B6 RID: 1206 RVA: 0x0001E6D4 File Offset: 0x0001C8D4
	public float RadiusBottom
	{
		get
		{
			return this.Settings.RadiusBottom;
		}
		set
		{
			this.Settings.RadiusBottom = value;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001E6E4 File Offset: 0x0001C8E4
	// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0001E6F4 File Offset: 0x0001C8F4
	public float Length
	{
		get
		{
			return this.Settings.Length;
		}
		set
		{
			this.Settings.Length = value;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001E704 File Offset: 0x0001C904
	// (set) Token: 0x060004BA RID: 1210 RVA: 0x0001E714 File Offset: 0x0001C914
	public int Subdivisions
	{
		get
		{
			return this.Settings.Subdivisions;
		}
		set
		{
			this.Settings.Subdivisions = value;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060004BB RID: 1211 RVA: 0x0001E724 File Offset: 0x0001C924
	// (set) Token: 0x060004BC RID: 1212 RVA: 0x0001E734 File Offset: 0x0001C934
	public int SubdivisionsHeight
	{
		get
		{
			return this.Settings.SubdivisionsHeight;
		}
		set
		{
			this.Settings.SubdivisionsHeight = value;
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001E744 File Offset: 0x0001C944
	public void GenerateBeam()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		CombineInstance[] array = new CombineInstance[2];
		array[0].mesh = this.GenerateMesh(false);
		array[0].transform = Matrix4x4.identity;
		array[1].mesh = this.GenerateMesh(true);
		array[1].transform = Matrix4x4.identity;
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array);
		if (component.sharedMesh == null)
		{
			component.sharedMesh = new Mesh();
		}
		component.sharedMesh.Clear();
		component.sharedMesh.vertices = mesh.vertices;
		component.sharedMesh.uv = mesh.uv;
		component.sharedMesh.triangles = mesh.triangles;
		component.sharedMesh.tangents = mesh.tangents;
		component.sharedMesh.normals = mesh.normals;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0001E830 File Offset: 0x0001CA30
	private Mesh GenerateMesh(bool reverseNormals)
	{
		int num = this.Settings.Subdivisions * (this.Settings.SubdivisionsHeight + 1);
		num += this.Settings.SubdivisionsHeight + 1;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		Vector3[] array3 = new Vector3[num];
		int num2 = this.Settings.Subdivisions * 2 * this.Settings.SubdivisionsHeight * 3;
		int[] array4 = new int[num2];
		int num3 = this.Settings.SubdivisionsHeight + 1;
		float num4 = 6.2831855f / (float)this.Settings.Subdivisions;
		float lengthFrac = this.Settings.Length / (float)this.Settings.SubdivisionsHeight;
		float num5 = 1f / (float)this.Settings.Subdivisions;
		float num6 = 1f / (float)this.Settings.SubdivisionsHeight;
		for (int i = 0; i < this.Settings.Subdivisions + 1; i++)
		{
			float xAngle = Mathf.Cos((float)i * num4);
			float yAngle = Mathf.Sin((float)i * num4);
			Vector3 b = Lightbeam.CalculateVertex(lengthFrac, xAngle, yAngle, 0, this.Settings.RadiusTop);
			Vector3 a = Lightbeam.CalculateVertex(lengthFrac, xAngle, yAngle, num3 - 1, this.Settings.RadiusBottom);
			Vector3 vector = a - b;
			for (int j = 0; j < num3; j++)
			{
				float radius = Mathf.Lerp(this.Settings.RadiusTop, this.Settings.RadiusBottom, num6 * (float)j);
				Vector3 vector2 = Lightbeam.CalculateVertex(lengthFrac, xAngle, yAngle, j, radius);
				Vector3 normalized = vector.normalized;
				Vector3 vector3 = new Vector3(vector2.x, 0f, vector2.z);
				Vector3 vector4 = Vector3.Cross(normalized, vector3.normalized);
				if (reverseNormals)
				{
					vector4 = Vector3.Cross(vector.normalized, vector4.normalized);
				}
				else
				{
					vector4 = Vector3.Cross(vector4.normalized, vector.normalized);
				}
				int num7 = i * num3 + j;
				array[num7] = vector2;
				array2[num7] = new Vector2(num5 * (float)i, 1f - num6 * (float)j);
				array3[num7] = vector4.normalized;
				array2[num7] = new Vector2(num5 * (float)i, 1f - num6 * (float)j);
			}
		}
		int num8 = 0;
		for (int k = 0; k < this.Settings.Subdivisions; k++)
		{
			for (int l = 0; l < num3 - 1; l++)
			{
				int num9 = k * num3 + l;
				int num10 = num9 + 1;
				int num11 = num9 + num3;
				if (num11 >= num)
				{
					num11 %= num;
				}
				if (reverseNormals)
				{
					array4[num8++] = num9;
					array4[num8++] = num10;
					array4[num8++] = num11;
				}
				else
				{
					array4[num8++] = num10;
					array4[num8++] = num9;
					array4[num8++] = num11;
				}
				int num12 = num9 + 1;
				int num13 = num9 + num3;
				if (num13 >= num)
				{
					num13 %= num;
				}
				int num14 = num13 + 1;
				if (reverseNormals)
				{
					array4[num8++] = num12;
					array4[num8++] = num14;
					array4[num8++] = num13;
				}
				else
				{
					array4[num8++] = num12;
					array4[num8++] = num13;
					array4[num8++] = num14;
				}
			}
		}
		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array4;
		mesh.normals = array3;
		mesh.RecalculateBounds();
		;
		Lightbeam.CalculateMeshTangents(mesh);
		return mesh;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0001EC18 File Offset: 0x0001CE18
	private static Vector3 CalculateVertex(float lengthFrac, float xAngle, float yAngle, int j, float radius)
	{
		float x = radius * xAngle;
		float z = radius * yAngle;
		Vector3 result = new Vector3(x, (float)j * (lengthFrac * -1f), z);
		return result;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0001EC44 File Offset: 0x0001CE44
	private static void CalculateMeshTangents(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int num = triangles.Length;
		int num2 = vertices.Length;
		Vector3[] array = new Vector3[num2];
		Vector3[] array2 = new Vector3[num2];
		Vector4[] array3 = new Vector4[num2];
		for (long num3 = 0L; num3 < (long)num; num3 += 3L)
		{
			long num4 = (long)triangles[(int)(checked((IntPtr)num3))];
			long num5 = (long)triangles[(int)(checked((IntPtr)(unchecked(num3 + 1L))))];
			long num6 = (long)triangles[(int)(checked((IntPtr)(unchecked(num3 + 2L))))];
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			Vector2 vector4;
			Vector2 vector5;
			Vector2 vector6;
			checked
			{
				vector = vertices[(int)((IntPtr)num4)];
				vector2 = vertices[(int)((IntPtr)num5)];
				vector3 = vertices[(int)((IntPtr)num6)];
				vector4 = uv[(int)((IntPtr)num4)];
				vector5 = uv[(int)((IntPtr)num5)];
				vector6 = uv[(int)((IntPtr)num6)];
			}
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 b = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 b2 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			checked
			{
				array[(int)((IntPtr)num4)] += b;
				array[(int)((IntPtr)num5)] += b;
				array[(int)((IntPtr)num6)] += b;
				array2[(int)((IntPtr)num4)] += b2;
				array2[(int)((IntPtr)num5)] += b2;
				array2[(int)((IntPtr)num6)] += b2;
			}
		}
		for (long num18 = 0L; num18 < (long)num2; num18 += 1L)
		{
			checked
			{
				Vector3 lhs = normals[(int)((IntPtr)num18)];
				Vector3 rhs = array[(int)((IntPtr)num18)];
				Vector3.OrthoNormalize(ref lhs, ref rhs);
				array3[(int)((IntPtr)num18)].x = rhs.x;
				array3[(int)((IntPtr)num18)].y = rhs.y;
				array3[(int)((IntPtr)num18)].z = rhs.z;
				array3[(int)((IntPtr)num18)].w = ((Vector3.Dot(Vector3.Cross(lhs, rhs), array2[(int)((IntPtr)num18)]) >= 0f) ? 1f : -1f);
			}
		}
		mesh.tangents = array3;
	}

	// Token: 0x0400049F RID: 1183
	public bool IsModifyingMesh;

	// Token: 0x040004A0 RID: 1184
	public Material DefaultMaterial;

	// Token: 0x040004A1 RID: 1185
	public LightbeamSettings Settings;
}
