using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class StopMusic : MonoBehaviour
{
	// Token: 0x06000864 RID: 2148 RVA: 0x00046054 File Offset: 0x00044254
	private void Start()
	{
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00046058 File Offset: 0x00044258
	public void Stop()
	{
		base.GetComponent<AudioSource>().Stop();
	}
}
