using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class DarkZone : MonoBehaviour
{
	// Token: 0x06000535 RID: 1333 RVA: 0x00032810 File Offset: 0x00030A10
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			if (!AnimationHandler.instance.holdingTorch)
			{
				if (this.currentFarisSound == 0)
				{
					SpeechManager.PlayConversation(this.FarisSound1);
					this.currentFarisSound = 1;
				}
				else
				{
					SpeechManager.PlayConversation(this.FarisSound2);
					this.currentFarisSound = 0;
				}
			}
			else
			{
				UnityEngine.Object.Destroy(base.transform.parent.gameObject);
			}
		}
	}

	// Token: 0x040007E5 RID: 2021
	public string FarisSound1;

	// Token: 0x040007E6 RID: 2022
	public string FarisSound2;

	// Token: 0x040007E7 RID: 2023
	private int currentFarisSound;
}
