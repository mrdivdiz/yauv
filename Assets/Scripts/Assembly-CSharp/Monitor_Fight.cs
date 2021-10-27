using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class Monitor_Fight : MonoBehaviour
{
	// Token: 0x060007D4 RID: 2004 RVA: 0x00040B18 File Offset: 0x0003ED18
	private void Start()
	{
		if (this.a.IsFinished)
		{
			this.car.SetActive(true);
		}
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00040B38 File Offset: 0x0003ED38
	private void Update()
	{
	}

	// Token: 0x04000A62 RID: 2658
	public CSComponent a;

	// Token: 0x04000A63 RID: 2659
	public GameObject car;
}
