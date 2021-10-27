using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class Decal : MonoBehaviour
{
	// Token: 0x06000179 RID: 377 RVA: 0x0000B108 File Offset: 0x00009308
	private void OnDrawGizmosSelected()
	{
		this.CalculateBounds();
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000B13C File Offset: 0x0000933C
	public void CalculateBounds()
	{
		this.bounds = new Bounds(base.transform.position, base.transform.lossyScale * 1.414214f);
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000B174 File Offset: 0x00009374
	public void CalculateDecal()
	{
		this.ClearDecals();
		this.maxAngle = Mathf.Clamp(this.maxAngle, 0f, 180f);
		this.angleCosine = Mathf.Cos(this.maxAngle * 0.017453292f);
		this.uvAngle = Mathf.Clamp(this.uvAngle, 0f, 360f);
		this.uCos = Mathf.Cos(this.uvAngle * 0.017453292f);
		this.vSin = Mathf.Sin(this.uvAngle * 0.017453292f);
		if (this.affectedObjects == null)
		{
			return;
		}
		if (this.affectedObjects.Length <= 0)
		{
			return;
		}
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		this.instancesList = new List<MeshCombineUtility.MeshInstance>();
		for (int i = 0; i < this.affectedObjects.Length; i++)
		{
			if (!(this.affectedObjects[i] == null))
			{
				this.CalculateObjectDecal(this.affectedObjects[i], worldToLocalMatrix);
			}
		}
		for (int j = 0; j < this.affectedObjects.Length; j++)
		{
			this.affectedObjects[j] = null;
		}
		if (this.instancesList.Count > 0)
		{
			MeshCombineUtility.MeshInstance[] array = new MeshCombineUtility.MeshInstance[this.instancesList.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = this.instancesList[k];
			}
			MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			meshRenderer.material = this.decalMaterial;
			MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(meshFilter.sharedMesh);
			}
			Mesh mesh = MeshCombineUtility.Combine(array, true);
			if (this.pushDistance > 0f)
			{
				List<List<int>> list = new List<List<int>>();
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;
				bool[] array2 = new bool[vertices.Length];
				for (int l = 0; l < array2.Length; l++)
				{
					array2[l] = false;
				}
				for (int m = 0; m < vertices.Length; m++)
				{
					if (!array2[m])
					{
						List<int> list2 = new List<int>();
						list2.Add(m);
						array2[m] = true;
						for (int n = m + 1; n < vertices.Length; n++)
						{
							if (!array2[n])
							{
								if (Vector3.Distance(vertices[m], vertices[n]) < 0.001f)
								{
									list2.Add(n);
									array2[n] = true;
								}
							}
						}
						list.Add(list2);
					}
				}
				foreach (List<int> list3 in list)
				{
					Vector3 a = Vector3.zero;
					foreach (int num in list3)
					{
						a += normals[num];
					}
					a = (a / (float)list3.Count).normalized;
					foreach (int num2 in list3)
					{
						vertices[num2] += a * this.pushDistance;
					}
				}
				mesh.vertices = vertices;
			}
			mesh.name = "DecalMesh";
			meshFilter.mesh = mesh;
			for (int num3 = 0; num3 < this.instancesList.Count; num3++)
			{
				UnityEngine.Object.DestroyImmediate(this.instancesList[num3].mesh);
			}
		}
		this.instancesList.Clear();
		this.instancesList = null;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000B614 File Offset: 0x00009814
	public void ClearDecals()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(component.sharedMesh);
			UnityEngine.Object.DestroyImmediate(component);
		}
		MeshRenderer component2 = base.gameObject.GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			UnityEngine.Object.DestroyImmediate(component2);
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000B668 File Offset: 0x00009868
	public void CalculateObjectDecal(GameObject obj, Matrix4x4 cTransform)
	{
		if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameShield)
		{
			return;
		}
		Mesh mesh = null;
		if (this.decalMode == DecalMode.MESH_COLLIDER)
		{
			if (obj.GetComponent<MeshCollider>() != null)
			{
				mesh = obj.GetComponent<MeshCollider>().sharedMesh;
			}
			else
			{
				mesh = null;
			}
		}
		if (mesh == null || this.decalMode == DecalMode.MESH_FILTER)
		{
			if (obj.GetComponent<MeshFilter>() == null)
			{
				return;
			}
			mesh = obj.GetComponent<MeshFilter>().sharedMesh;
		}
		if (mesh == null || mesh.name.ToLower().Contains("combined"))
		{
			return;
		}
		this.decalNormal = obj.transform.InverseTransformDirection(base.transform.forward);
		this.decalCenter = obj.transform.InverseTransformPoint(base.transform.position);
		this.decalTangent = obj.transform.InverseTransformDirection(base.transform.right);
		this.decalBinormal = obj.transform.InverseTransformDirection(base.transform.up);
		this.decalSize = new Vector3(base.transform.lossyScale.x / obj.transform.lossyScale.x, base.transform.lossyScale.y / obj.transform.lossyScale.y, base.transform.lossyScale.z / obj.transform.lossyScale.z);
		this.bottomPlane = new Vector4(-this.decalBinormal.x, -this.decalBinormal.y, -this.decalBinormal.z, this.decalSize.y * 0.5f + Vector3.Dot(this.decalCenter, this.decalBinormal));
		this.topPlane = new Vector4(this.decalBinormal.x, this.decalBinormal.y, this.decalBinormal.z, this.decalSize.y * 0.5f - Vector3.Dot(this.decalCenter, this.decalBinormal));
		this.rightPlane = new Vector4(-this.decalTangent.x, -this.decalTangent.y, -this.decalTangent.z, this.decalSize.x * 0.5f + Vector3.Dot(this.decalCenter, this.decalTangent));
		this.leftPlane = new Vector4(this.decalTangent.x, this.decalTangent.y, this.decalTangent.z, this.decalSize.x * 0.5f - Vector3.Dot(this.decalCenter, this.decalTangent));
		this.frontPlane = new Vector4(this.decalNormal.x, this.decalNormal.y, this.decalNormal.z, this.decalSize.z * 0.5f - Vector3.Dot(this.decalCenter, this.decalNormal));
		this.backPlane = new Vector4(-this.decalNormal.x, -this.decalNormal.y, -this.decalNormal.z, this.decalSize.z * 0.5f + Vector3.Dot(this.decalCenter, this.decalNormal));
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		Vector4[] tangents = mesh.tangents;
		this.startPolygons = new List<DecalPolygon>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			Vector3 vector = vertices[num];
			Vector3 vector2 = vertices[num2];
			Vector3 vector3 = vertices[num3];
			Vector3 vector4 = normals[num];
			float num4 = Vector3.Dot(vector4, -this.decalNormal);
			if (num4 > this.angleCosine)
			{
				Vector3 v = tangents[num];
				Vector3 v2 = tangents[num2];
				Vector3 v3 = tangents[num3];
				DecalPolygon decalPolygon = new DecalPolygon();
				decalPolygon.verticeCount = 3;
				decalPolygon.vertice[0] = vector;
				decalPolygon.vertice[1] = vector2;
				decalPolygon.vertice[2] = vector3;
				decalPolygon.normal[0] = vector4;
				decalPolygon.normal[1] = vector4;
				decalPolygon.normal[2] = vector4;
				decalPolygon.tangent[0] = v;
				decalPolygon.tangent[1] = v2;
				decalPolygon.tangent[2] = v3;
				this.startPolygons.Add(decalPolygon);
			}
		}
		Mesh mesh2 = this.CreateMesh(this.ClipMesh(), obj.transform);
		if (mesh2 != null)
		{
			MeshCombineUtility.MeshInstance item = default(MeshCombineUtility.MeshInstance);
			item.mesh = mesh2;
			item.subMeshIndex = 0;
			item.transform = base.transform.worldToLocalMatrix * obj.transform.localToWorldMatrix;
			this.instancesList.Add(item);
		}
		this.startPolygons.Clear();
		this.startPolygons = null;
		this.clippedPolygons.Clear();
		this.clippedPolygons = null;
		GC.Collect();
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000BC5C File Offset: 0x00009E5C
	private Mesh CreateMesh(int totalVertices, Transform to)
	{
		if (this.clippedPolygons == null)
		{
			return null;
		}
		if (this.clippedPolygons.Count <= 0)
		{
			return null;
		}
		if (totalVertices < 3)
		{
			return null;
		}
		int[] array = new int[(totalVertices - 2) * 3];
		Vector3[] array2 = new Vector3[totalVertices];
		Vector3[] array3 = new Vector3[totalVertices];
		Vector2[] array4 = new Vector2[totalVertices];
		Vector4[] array5 = new Vector4[totalVertices];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		float num4 = 1f / this.decalSize.x;
		float num5 = 1f / this.decalSize.y;
		foreach (DecalPolygon decalPolygon in this.clippedPolygons)
		{
			for (int i = 0; i < decalPolygon.verticeCount; i++)
			{
				array2[num] = decalPolygon.vertice[i];
				array3[num] = decalPolygon.normal[i];
				array5[num] = decalPolygon.tangent[i];
				if (i < decalPolygon.verticeCount - 2)
				{
					array[num2] = num3;
					array[num2 + 1] = num + 1;
					array[num2 + 2] = num + 2;
					num2 += 3;
				}
				num++;
			}
			num3 = num;
		}
		for (int j = 0; j < array2.Length; j++)
		{
			Vector3 lhs = array2[j] - this.decalCenter;
			float num6 = Vector3.Dot(lhs, this.decalTangent) * num4;
			float num7 = Vector3.Dot(lhs, this.decalBinormal) * num5;
			float num8 = num6 * this.uCos - num7 * this.vSin + 0.5f;
			float num9 = num6 * this.vSin + num7 * this.uCos + 0.5f;
			num8 *= this.tiling.x;
			num9 *= this.tiling.y;
			num8 += this.offset.x;
			num9 += this.offset.y;
			array4[j] = new Vector2(num8, num9);
		}
		return new Mesh
		{
			vertices = array2,
			normals = array3,
			triangles = array,
			uv = array4/*,
			uv1 = array4*/,
			uv2 = array4,
			tangents = array5
		};
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000BF1C File Offset: 0x0000A11C
	private int ClipMesh()
	{
		int num = 0;
		if (this.clippedPolygons == null)
		{
			this.clippedPolygons = new List<DecalPolygon>();
		}
		else
		{
			this.clippedPolygons.Clear();
		}
		for (int i = 0; i < this.startPolygons.Count; i++)
		{
			DecalPolygon polygon = this.startPolygons[i];
			DecalPolygon decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(polygon, this.frontPlane);
			if (decalPolygon != null)
			{
				decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, this.backPlane);
				if (decalPolygon != null)
				{
					decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, this.rightPlane);
					if (decalPolygon != null)
					{
						decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, this.leftPlane);
						if (decalPolygon != null)
						{
							decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, this.bottomPlane);
							if (decalPolygon != null)
							{
								decalPolygon = DecalPolygon.ClipPolygonAgainstPlane(decalPolygon, this.topPlane);
								if (decalPolygon != null)
								{
									num += decalPolygon.verticeCount;
									this.clippedPolygons.Add(decalPolygon);
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000C004 File Offset: 0x0000A204
	public bool HasChanged()
	{
		Transform transform = base.transform;
		bool result = false;
		this.maxAngle = Mathf.Clamp(this.maxAngle, 0f, 180f);
		this.uvAngle = Mathf.Clamp(this.uvAngle, 0f, 360f);
		if (this.previousPosition != transform.position)
		{
			result = true;
		}
		else if (this.previousScale != transform.lossyScale)
		{
			result = true;
		}
		else if (this.previousRotation != transform.rotation)
		{
			result = true;
		}
		else if (this.previousPushDistance != this.pushDistance)
		{
			result = true;
		}
		else if (this.previousTiling != this.tiling)
		{
			result = true;
		}
		else if (this.previousOffset != this.offset)
		{
			result = true;
		}
		else if (this.previousMaxAngle != this.maxAngle)
		{
			result = true;
		}
		else if (this.previousUVAngle != this.uvAngle)
		{
			result = true;
		}
		this.previousUVAngle = this.uvAngle;
		this.previousMaxAngle = this.maxAngle;
		this.previousTiling = this.tiling;
		this.previousOffset = this.offset;
		this.previousPushDistance = this.pushDistance;
		this.previousPosition = transform.position;
		this.previousRotation = transform.rotation;
		this.previousScale = transform.lossyScale;
		return result;
	}

	// Token: 0x040001BD RID: 445
	public static int dCount;

	// Token: 0x040001BE RID: 446
	[HideInInspector]
	public GameObject[] affectedObjects;

	// Token: 0x040001BF RID: 447
	public float maxAngle = 90f;

	// Token: 0x040001C0 RID: 448
	private float angleCosine;

	// Token: 0x040001C1 RID: 449
	[HideInInspector]
	public Bounds bounds;

	// Token: 0x040001C2 RID: 450
	[HideInInspector]
	public float previousUVAngle;

	// Token: 0x040001C3 RID: 451
	[HideInInspector]
	public Vector3 previousPosition;

	// Token: 0x040001C4 RID: 452
	[HideInInspector]
	public Quaternion previousRotation;

	// Token: 0x040001C5 RID: 453
	[HideInInspector]
	public Vector3 previousScale;

	// Token: 0x040001C6 RID: 454
	[HideInInspector]
	public float previousMaxAngle;

	// Token: 0x040001C7 RID: 455
	[HideInInspector]
	public float previousPushDistance = 0.009f;

	// Token: 0x040001C8 RID: 456
	[HideInInspector]
	public Vector2 previousTiling;

	// Token: 0x040001C9 RID: 457
	[HideInInspector]
	public Vector2 previousOffset;

	// Token: 0x040001CA RID: 458
	[HideInInspector]
	public bool useMeshCollider;

	// Token: 0x040001CB RID: 459
	public Vector2 tiling = Vector2.one;

	// Token: 0x040001CC RID: 460
	public Vector2 offset = Vector2.zero;

	// Token: 0x040001CD RID: 461
	[HideInInspector]
	public float uvAngle;

	// Token: 0x040001CE RID: 462
	private float uCos;

	// Token: 0x040001CF RID: 463
	private float vSin;

	// Token: 0x040001D0 RID: 464
	public Material decalMaterial;

	// Token: 0x040001D1 RID: 465
	[HideInInspector]
	public DecalMode decalMode;

	// Token: 0x040001D2 RID: 466
	private List<DecalPolygon> startPolygons;

	// Token: 0x040001D3 RID: 467
	private List<DecalPolygon> clippedPolygons;

	// Token: 0x040001D4 RID: 468
	[HideInInspector]
	public Vector4 bottomPlane;

	// Token: 0x040001D5 RID: 469
	[HideInInspector]
	public Vector4 topPlane;

	// Token: 0x040001D6 RID: 470
	[HideInInspector]
	public Vector4 leftPlane;

	// Token: 0x040001D7 RID: 471
	[HideInInspector]
	public Vector4 rightPlane;

	// Token: 0x040001D8 RID: 472
	[HideInInspector]
	public Vector4 frontPlane;

	// Token: 0x040001D9 RID: 473
	[HideInInspector]
	public Vector4 backPlane;

	// Token: 0x040001DA RID: 474
	[HideInInspector]
	public Vector3 decalNormal;

	// Token: 0x040001DB RID: 475
	[HideInInspector]
	public Vector3 decalCenter;

	// Token: 0x040001DC RID: 476
	[HideInInspector]
	public Vector3 decalTangent;

	// Token: 0x040001DD RID: 477
	[HideInInspector]
	public Vector3 decalBinormal;

	// Token: 0x040001DE RID: 478
	[HideInInspector]
	public Vector3 decalSize;

	// Token: 0x040001DF RID: 479
	public float pushDistance = 0.009f;

	// Token: 0x040001E0 RID: 480
	private List<MeshCombineUtility.MeshInstance> instancesList;

	// Token: 0x040001E1 RID: 481
	public bool checkAutomatically;

	// Token: 0x040001E2 RID: 482
	public LayerMask affectedLayers;

	// Token: 0x040001E3 RID: 483
	public bool affectOtherDecals;

	// Token: 0x040001E4 RID: 484
	public bool affectInactiveRenderers;

	// Token: 0x040001E5 RID: 485
	[HideInInspector]
	public bool showAffectedObjectsOptions;

	// Token: 0x040001E6 RID: 486
	[HideInInspector]
	public bool showObjects;
}
