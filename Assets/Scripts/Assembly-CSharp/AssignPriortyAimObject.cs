using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class AssignPriortyAimObject : MonoBehaviour
{
	// Token: 0x060006C3 RID: 1731 RVA: 0x00036C74 File Offset: 0x00034E74
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((collisionInfo.tag == "Player" || collisionInfo.tag == "Bike" || collisionInfo.tag == "PlayerCar") && !this.played)
		{
			if (Camera.main != null)
			{
				ShooterGameCamera component = Camera.main.GetComponent<ShooterGameCamera>();
				component.prioritizedAimTarget = this.prioritizedAimObject;
			}
			this.played = true;
		}
	}

	// Token: 0x0400089E RID: 2206
	public Transform prioritizedAimObject;

	// Token: 0x0400089F RID: 2207
	private bool played;
}
