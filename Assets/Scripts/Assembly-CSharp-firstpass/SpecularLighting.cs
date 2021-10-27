using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[ExecuteInEditMode]
[RequireComponent(typeof(WaterBase))]
public class SpecularLighting : MonoBehaviour
{
	// Token: 0x0600009E RID: 158 RVA: 0x0000767C File Offset: 0x0000587C
	public void Start()
	{
		this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000076AC File Offset: 0x000058AC
	public void Update()
	{
		if (!this.waterBase)
		{
			this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}
		if (this.specularLight && this.waterBase.sharedMaterial)
		{
			this.waterBase.sharedMaterial.SetVector("_WorldLightDir", this.specularLight.transform.forward);
		}
	}

	// Token: 0x040000C7 RID: 199
	public Transform specularLight;

	// Token: 0x040000C8 RID: 200
	private WaterBase waterBase;
}
