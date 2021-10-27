using System;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class pendulum_stop : MonoBehaviour
{
	// Token: 0x060008D7 RID: 2263 RVA: 0x0004958C File Offset: 0x0004778C
	private void OnTriggerEnter()
	{
		playagain.endreached = true;
	}
}
