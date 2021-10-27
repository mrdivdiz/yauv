using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[ExecuteInEditMode]
[RequireComponent(typeof(WaterBase))]
public class Displace : MonoBehaviour
{
	// Token: 0x06000087 RID: 135 RVA: 0x00006CFC File Offset: 0x00004EFC
	public void Start()
	{
		if (!this.waterBase)
		{
			this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00006D3C File Offset: 0x00004F3C
	public void OnEnable()
	{
		Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
		Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00006D54 File Offset: 0x00004F54
	public void OnDisable()
	{
		Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
		Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
	}

	// Token: 0x040000BA RID: 186
	private WaterBase waterBase;
}
