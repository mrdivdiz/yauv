using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class fastTime : MonoBehaviour
{
	// Token: 0x06000BD4 RID: 3028 RVA: 0x000970A8 File Offset: 0x000952A8
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Bike")
		{
			Time.timeScale = 1f;
			Camera.main.GetComponent<QuadGameCamera>().focusOnTarget = false;
			if (this.tower.hit)
			{
				this.cutscene1.StartCutscene();
			}
			else
			{
				collisionInfo.GetComponent<Collider>().GetComponentInChildren<QBHealth>().health = 0f;
			}
		}
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0009711C File Offset: 0x0009531C
	private void Update()
	{
	}

	// Token: 0x0400160E RID: 5646
	public CSComponent cutscene1;

	// Token: 0x0400160F RID: 5647
	public TowerHit tower;
}
