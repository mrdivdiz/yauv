using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class CameraInfo : MonoBehaviour
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600057B RID: 1403 RVA: 0x00022A18 File Offset: 0x00020C18
	// (set) Token: 0x0600057C RID: 1404 RVA: 0x00022A20 File Offset: 0x00020C20
	public static Matrix4x4 VPMatrix { get; private set; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600057D RID: 1405 RVA: 0x00022A28 File Offset: 0x00020C28
	// (set) Token: 0x0600057E RID: 1406 RVA: 0x00022A30 File Offset: 0x00020C30
	public static Matrix4x4 PrevVPMatrix { get; private set; }

	// Token: 0x0600057F RID: 1407 RVA: 0x00022A38 File Offset: 0x00020C38
	public void Awake()
	{
		this.isDirect3D = (SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1);
		CameraInfo.VPMatrix = Matrix4x4.identity;
		CameraInfo.PrevVPMatrix = Matrix4x4.identity;
		this.updateCurrentMatrices();
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00022A78 File Offset: 0x00020C78
	public void Update()
	{
		CameraInfo.PrevVPMatrix = CameraInfo.VPMatrix;
		this.updateCurrentMatrices();
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00022A8C File Offset: 0x00020C8C
	public void updateCurrentMatrices()
	{
		Matrix4x4 worldToCameraMatrix = base.GetComponent<Camera>().worldToCameraMatrix;
		Matrix4x4 projectionMatrix = base.GetComponent<Camera>().projectionMatrix;
		if (this.isDirect3D)
		{
			for (int i = 0; i < 4; i++)
			{
				projectionMatrix[1, i] = -projectionMatrix[1, i];
			}
			for (int j = 0; j < 4; j++)
			{
				projectionMatrix[2, j] = projectionMatrix[2, j] * 0.5f + projectionMatrix[3, j] * 0.5f;
			}
		}
		CameraInfo.VPMatrix = projectionMatrix * worldToCameraMatrix;
	}

	// Token: 0x040005A6 RID: 1446
	private bool isDirect3D;
}
