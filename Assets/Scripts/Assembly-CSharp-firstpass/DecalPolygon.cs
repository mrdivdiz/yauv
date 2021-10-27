using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class DecalPolygon
{
	// Token: 0x060000BA RID: 186 RVA: 0x00003D98 File Offset: 0x00001F98
	public DecalPolygon()
	{
		this.verticeCount = 0;
		this.vertice = new Vector3[9];
		this.normal = new Vector3[9];
		this.tangent = new Vector4[9];
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00003DDC File Offset: 0x00001FDC
	public static DecalPolygon ClipPolygonAgainstPlane(DecalPolygon polygon, Vector4 plane)
	{
		bool[] array = new bool[10];
		int num = 0;
		Vector3 vector = new Vector3(plane.x, plane.y, plane.z);
		for (int i = 0; i < polygon.verticeCount; i++)
		{
			array[i] = (Vector3.Dot(polygon.vertice[i], vector) + plane.w < 0f);
			if (array[i])
			{
				num++;
			}
		}
		if (num == polygon.verticeCount)
		{
			return null;
		}
		if (num == 0)
		{
			return polygon;
		}
		DecalPolygon decalPolygon = new DecalPolygon();
		decalPolygon.verticeCount = 0;
		for (int j = 0; j < polygon.verticeCount; j++)
		{
			int num2 = (j != 0) ? (j - 1) : (polygon.verticeCount - 1);
			if (array[j])
			{
				if (!array[num2])
				{
					Vector3 vector2 = polygon.vertice[j];
					Vector3 a = polygon.vertice[num2];
					Vector3 normalized = (a - vector2).normalized;
					float d = -(Vector3.Dot(vector, vector2) + plane.w) / Vector3.Dot(vector, normalized);
					decalPolygon.tangent[decalPolygon.verticeCount] = polygon.tangent[j] + (polygon.tangent[num2] - polygon.tangent[j]).normalized * d;
					decalPolygon.vertice[decalPolygon.verticeCount] = vector2 + (a - vector2).normalized * d;
					decalPolygon.normal[decalPolygon.verticeCount] = polygon.normal[j] + (polygon.normal[num2] - polygon.normal[j]).normalized * d;
					decalPolygon.verticeCount++;
				}
			}
			else
			{
				if (array[num2])
				{
					Vector3 vector2 = polygon.vertice[num2];
					Vector3 a = polygon.vertice[j];
					Vector3 normalized = (a - vector2).normalized;
					float d = -(Vector3.Dot(vector, vector2) + plane.w) / Vector3.Dot(vector, normalized);
					decalPolygon.tangent[decalPolygon.verticeCount] = polygon.tangent[num2] + (polygon.tangent[j] - polygon.tangent[num2]).normalized * d;
					decalPolygon.vertice[decalPolygon.verticeCount] = vector2 + (a - vector2).normalized * d;
					decalPolygon.normal[decalPolygon.verticeCount] = polygon.normal[num2] + (polygon.normal[j] - polygon.normal[num2]).normalized * d;
					decalPolygon.verticeCount++;
				}
				decalPolygon.tangent[decalPolygon.verticeCount] = polygon.tangent[j];
				decalPolygon.vertice[decalPolygon.verticeCount] = polygon.vertice[j];
				decalPolygon.normal[decalPolygon.verticeCount] = polygon.normal[j];
				decalPolygon.verticeCount++;
			}
		}
		return decalPolygon;
	}

	// Token: 0x04000079 RID: 121
	public int verticeCount;

	// Token: 0x0400007A RID: 122
	public Vector3[] normal;

	// Token: 0x0400007B RID: 123
	public Vector3[] vertice;

	// Token: 0x0400007C RID: 124
	public Vector4[] tangent;
}
