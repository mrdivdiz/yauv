using System;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class showbody : MonoBehaviour
{
	// Token: 0x06000B8B RID: 2955 RVA: 0x00091A90 File Offset: 0x0008FC90
	private void Start()
	{
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00091A94 File Offset: 0x0008FC94
	public void ShowBody()
	{
		this.body.SetActive(true);
	}

	// Token: 0x04001516 RID: 5398
	public GameObject body;
}
