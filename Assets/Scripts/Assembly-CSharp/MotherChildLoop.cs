using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class MotherChildLoop : MonoBehaviour
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x00027C58 File Offset: 0x00025E58
	public void StartLoop()
	{
		base.GetComponent<Animation>().CrossFade("Mother-Child-Loop");
	}
}
