using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class MeshContainer
{
	// Token: 0x06000114 RID: 276 RVA: 0x00007088 File Offset: 0x00005288
	public MeshContainer(Mesh m)
	{
		this.mesh = m;
		this.vertices = m.vertices;
		this.normals = m.normals;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000070B0 File Offset: 0x000052B0
	public void Update()
	{
		this.mesh.vertices = this.vertices;
		this.mesh.normals = this.normals;
	}

	// Token: 0x040000CF RID: 207
	public Mesh mesh;

	// Token: 0x040000D0 RID: 208
	public Vector3[] vertices;

	// Token: 0x040000D1 RID: 209
	public Vector3[] normals;
}
