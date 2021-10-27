using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class FixedCameraBox : MonoBehaviour
{
	// Token: 0x0600061D RID: 1565 RVA: 0x0002CB34 File Offset: 0x0002AD34
	private void Awake()
	{
		this.cam = Camera.main.GetComponent<ShooterGameCamera>();
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0002CB48 File Offset: 0x0002AD48
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.cam.cameraPosition = this.CameraPosition;
			this.cam.inTransitionTime = this.inTransitionTime;
			this.cam.outTransitionTime = this.outTransitionTime;
			this.cam.fixedCameraPosition = true;
		}
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0002CBAC File Offset: 0x0002ADAC
	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.cam.fixedCameraPosition = false;
		}
	}

	// Token: 0x040006D8 RID: 1752
	public Transform CameraPosition;

	// Token: 0x040006D9 RID: 1753
	public float inTransitionTime = 1f;

	// Token: 0x040006DA RID: 1754
	public float outTransitionTime = 1f;

	// Token: 0x040006DB RID: 1755
	private ShooterGameCamera cam;
}
