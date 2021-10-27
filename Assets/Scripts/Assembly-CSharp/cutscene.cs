using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class cutscene : MonoBehaviour
{
	// Token: 0x06000B93 RID: 2963 RVA: 0x0009227C File Offset: 0x0009047C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Bike")
		{
			this.cutscene1.StartCutscene();
		}
	}

	// Token: 0x0400151F RID: 5407
	public CSComponent cutscene1;
}
