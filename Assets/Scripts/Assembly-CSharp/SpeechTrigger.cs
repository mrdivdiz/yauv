using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class SpeechTrigger : MonoBehaviour
{
	// Token: 0x0600085C RID: 2140 RVA: 0x00045F38 File Offset: 0x00044138
	private void Start()
	{
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00045F3C File Offset: 0x0004413C
	private void Update()
	{
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00045F40 File Offset: 0x00044140
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((collisionInfo.tag == "Player" || collisionInfo.tag == "PlayerCar" || (collisionInfo.tag == "Bike" && QBAnimationHandler.Instance != null)) && this.conversationID != string.Empty && !this.played)
		{
			SpeechManager.PlayConversation(this.conversationID);
			this.played = true;
		}
	}

	// Token: 0x04000B34 RID: 2868
	public string conversationID;

	// Token: 0x04000B35 RID: 2869
	private bool played;
}
