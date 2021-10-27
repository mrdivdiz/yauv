using System;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class GrenadeGUI : MonoBehaviour
{
	// Token: 0x06000A07 RID: 2567 RVA: 0x00068730 File Offset: 0x00066930
	private void Start()
	{
		this.mainCameraTransform = Camera.main.transform;
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00068744 File Offset: 0x00066944
	private void Update()
	{
		if (base.transform != null && this.mainCameraTransform != null)
		{
			base.transform.LookAt(this.mainCameraTransform.position);
		}
	}

	// Token: 0x04000F67 RID: 3943
	private Transform mainCameraTransform;
}
