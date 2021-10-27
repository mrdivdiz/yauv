using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class RemoveParticle : MonoBehaviour
{
	// Token: 0x06000816 RID: 2070 RVA: 0x000420C8 File Offset: 0x000402C8
	private void Start()
	{
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x000420CC File Offset: 0x000402CC
	private void FixedUpdate()
	{
		if (this.cutscene1.IsSkipped)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000AA9 RID: 2729
	public CSComponent cutscene1;
}
