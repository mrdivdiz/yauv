using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
public class BikeVibrate : MonoBehaviour
{
	// Token: 0x060006CC RID: 1740 RVA: 0x00036F20 File Offset: 0x00035120
	private void Start()
	{
		PadVibrator.SetBackgroundVibration(true, 0.4f);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00036F30 File Offset: 0x00035130
	public void ExplodeVibrate()
	{
		PadVibrator.VibrateInterval(true, 1f, 1f);
	}
}
