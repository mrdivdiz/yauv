using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class EnableDepthTexture : MonoBehaviour
{
	// Token: 0x060004AF RID: 1199 RVA: 0x0001E678 File Offset: 0x0001C878
	private void OnEnable()
	{
		base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0001E688 File Offset: 0x0001C888
	private void OnDisable()
	{
		base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0001E698 File Offset: 0x0001C898
	private void Update()
	{
	}

	// Token: 0x0400049E RID: 1182
	public bool EnableInEditor = true;
}
