using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class WhatWould : MonoBehaviour
{
	// Token: 0x060008AB RID: 2219 RVA: 0x00048D7C File Offset: 0x00046F7C
	private void Start()
	{
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00048D80 File Offset: 0x00046F80
	public void WhatWouldYou()
	{
		SpeechManager.PlayConversation("WhatWould");
	}
}
