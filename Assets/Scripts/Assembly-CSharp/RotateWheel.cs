using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class RotateWheel : MonoBehaviour
{
	// Token: 0x06000821 RID: 2081 RVA: 0x00042370 File Offset: 0x00040570
	private void Update()
	{
		if (this.cutscene.IsPlaying)
		{
			base.transform.Rotate(0f, 0f, Time.deltaTime * 180f, Space.Self);
		}
	}

	// Token: 0x04000AB1 RID: 2737
	public CSComponent cutscene;
}
