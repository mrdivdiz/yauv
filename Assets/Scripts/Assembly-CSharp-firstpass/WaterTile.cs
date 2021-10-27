using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[ExecuteInEditMode]
public class WaterTile : MonoBehaviour
{
	// Token: 0x060000A5 RID: 165 RVA: 0x0000786C File Offset: 0x00005A6C
	public void Start()
	{
		this.AcquireComponents();
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00007874 File Offset: 0x00005A74
	private void AcquireComponents()
	{
		if (!this.reflection)
		{
			if (base.transform.parent)
			{
				this.reflection = base.transform.parent.GetComponent<PlanarReflection>();
			}
			else
			{
				this.reflection = base.transform.GetComponent<PlanarReflection>();
			}
		}
		if (!this.waterBase)
		{
			if (base.transform.parent)
			{
				this.waterBase = base.transform.parent.GetComponent<WaterBase>();
			}
			else
			{
				this.waterBase = base.transform.GetComponent<WaterBase>();
			}
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00007924 File Offset: 0x00005B24
	public void Update()
	{
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00007928 File Offset: 0x00005B28
	public void OnWillRenderObject()
	{
		if (this.reflection)
		{
			this.reflection.WaterTileBeingRendered(base.transform, Camera.current);
		}
		if (this.waterBase)
		{
			this.waterBase.WaterTileBeingRendered(base.transform, Camera.current);
		}
	}

	// Token: 0x040000D0 RID: 208
	public PlanarReflection reflection;

	// Token: 0x040000D1 RID: 209
	public WaterBase waterBase;
}
