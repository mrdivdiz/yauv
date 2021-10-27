using System;
using UnityEngine;

// Token: 0x02000234 RID: 564
public class CustomTerrainScriptAtsV2 : MonoBehaviour
{
	// Token: 0x06000ABD RID: 2749 RVA: 0x0007FCF0 File Offset: 0x0007DEF0
	private void Start()
	{
		Terrain terrain = (Terrain)base.GetComponent(typeof(Terrain));
		if (this.Bump0)
		{
			Shader.SetGlobalTexture("_BumpMap0", this.Bump0);
		}
		if (this.Bump1)
		{
			Shader.SetGlobalTexture("_BumpMap1", this.Bump1);
		}
		if (this.Bump2)
		{
			Shader.SetGlobalTexture("_BumpMap2", this.Bump2);
		}
		if (this.Bump3)
		{
			Shader.SetGlobalTexture("_BumpMap3", this.Bump3);
		}
		Shader.SetGlobalFloat("_Tile0", this.Tile0);
		Shader.SetGlobalFloat("_Tile1", this.Tile1);
		Shader.SetGlobalFloat("_Tile2", this.Tile2);
		Shader.SetGlobalFloat("_Tile3", this.Tile3);
		this.terrainSizeX = terrain.terrainData.size.x;
		this.terrainSizeZ = terrain.terrainData.size.z;
		Shader.SetGlobalFloat("_TerrainX", this.terrainSizeX);
		Shader.SetGlobalFloat("_TerrainZ", this.terrainSizeZ);
	}

	// Token: 0x040011B0 RID: 4528
	public Texture2D Bump0;

	// Token: 0x040011B1 RID: 4529
	public Texture2D Bump1;

	// Token: 0x040011B2 RID: 4530
	public Texture2D Bump2;

	// Token: 0x040011B3 RID: 4531
	public Texture2D Bump3;

	// Token: 0x040011B4 RID: 4532
	public float Tile0;

	// Token: 0x040011B5 RID: 4533
	public float Tile1;

	// Token: 0x040011B6 RID: 4534
	public float Tile2;

	// Token: 0x040011B7 RID: 4535
	public float Tile3;

	// Token: 0x040011B8 RID: 4536
	public float terrainSizeX;

	// Token: 0x040011B9 RID: 4537
	public float terrainSizeZ;
}
