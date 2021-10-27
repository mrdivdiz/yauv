using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class StopMusic1 : MonoBehaviour
{
	// Token: 0x06000867 RID: 2151 RVA: 0x00046070 File Offset: 0x00044270
	private void Start()
	{
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00046074 File Offset: 0x00044274
	private void OnTriggerEnter()
	{
		this.music.Stop();
	}

	// Token: 0x04000B38 RID: 2872
	public AudioSource music;
}
