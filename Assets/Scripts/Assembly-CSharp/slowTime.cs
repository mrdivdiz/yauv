using System;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class slowTime : MonoBehaviour
{
	// Token: 0x06000BD7 RID: 3031 RVA: 0x00097128 File Offset: 0x00095328
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Bike")
		{
			Camera.main.GetComponent<QuadGameCamera>().resetCameraPosition();
			if (ShooterGameCamera.aimAssestType == ShooterGameCamera.AimAssestTypes.HARD)
			{
				Camera.main.GetComponent<QuadGameCamera>().lockTarget = this.lookTargetTransform;
				Camera.main.GetComponent<QuadGameCamera>().focusOnTarget = true;
			}
			Time.timeScale = 0.15f;
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x00097194 File Offset: 0x00095394
	private void Update()
	{
	}

	// Token: 0x04001610 RID: 5648
	public Transform lookTargetTransform;
}
