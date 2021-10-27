using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class RotateWheel1 : MonoBehaviour
{
	// Token: 0x06000823 RID: 2083 RVA: 0x000423B8 File Offset: 0x000405B8
	private void Update()
	{
		base.transform.Rotate(Time.deltaTime * 180f, 0f, 0f, Space.Self);
	}

	// Token: 0x04000AB2 RID: 2738
	public CSComponent cutscene;
}
