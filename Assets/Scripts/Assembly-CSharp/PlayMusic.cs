using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class PlayMusic : MonoBehaviour
{
	// Token: 0x06000912 RID: 2322 RVA: 0x0004ADC0 File Offset: 0x00048FC0
	private void Start()
	{
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0004ADC4 File Offset: 0x00048FC4
	private void OnTriggerEnter()
	{
		base.GetComponent<AudioSource>().Play();
	}
}
