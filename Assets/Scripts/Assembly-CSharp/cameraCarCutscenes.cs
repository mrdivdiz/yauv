using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class cameraCarCutscenes : MonoBehaviour
{
	// Token: 0x060008B1 RID: 2225 RVA: 0x00048DA4 File Offset: 0x00046FA4
	public void disableCam()
	{
		this.PlayerCam.enabled = false;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00048DB4 File Offset: 0x00046FB4
	public void enableCam()
	{
		this.PlayerCam.enabled = true;
	}

	// Token: 0x04000BAA RID: 2986
	public ShooterGameCamera PlayerCam;
}
