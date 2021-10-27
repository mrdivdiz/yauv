using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class SpeechTriggerRes : MonoBehaviour
{
	// Token: 0x06000860 RID: 2144 RVA: 0x00045FD8 File Offset: 0x000441D8
	private void Start()
	{
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00045FDC File Offset: 0x000441DC
	private void Update()
	{
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00045FE0 File Offset: 0x000441E0
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((collisionInfo.tag == "Player" || collisionInfo.tag == "Bike") && this.conversationID != string.Empty && !this.played)
		{
			SpeechManagerRes.PlayConversation(this.conversationID);
			this.played = true;
		}
	}

	// Token: 0x04000B36 RID: 2870
	public string conversationID;

	// Token: 0x04000B37 RID: 2871
	private bool played;
}
