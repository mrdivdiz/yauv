using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class salamCheck : MonoBehaviour
{
	// Token: 0x060008DC RID: 2268 RVA: 0x000495F8 File Offset: 0x000477F8
	private void Start()
	{
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x000495FC File Offset: 0x000477FC
	private void Update()
	{
		if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}
}
