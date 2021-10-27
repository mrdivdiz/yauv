using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class CarStart : MonoBehaviour
{
	// Token: 0x060006CF RID: 1743 RVA: 0x00036F4C File Offset: 0x0003514C
	private void Start()
	{
		base.Invoke("startconv", 20f);
		base.Invoke("startconv2", 80f);
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00036F7C File Offset: 0x0003517C
	private void startconv()
	{
		SpeechManager.PlayConversation("City");
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00036F88 File Offset: 0x00035188
	private void startconv2()
	{
		SpeechManager.PlayConversation("City2");
	}
}
