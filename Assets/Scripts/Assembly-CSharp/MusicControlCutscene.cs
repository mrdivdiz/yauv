using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class MusicControlCutscene : MonoBehaviour
{
	// Token: 0x060007DF RID: 2015 RVA: 0x00040F88 File Offset: 0x0003F188
	public void LevelSet1()
	{
		base.GetComponent<AudioSource>().volume = 0.22f;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00040F9C File Offset: 0x0003F19C
	public void LevelSet2()
	{
		base.GetComponent<AudioSource>().volume = 0.2f;
	}
}
