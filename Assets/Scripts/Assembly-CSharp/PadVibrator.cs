using System;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class PadVibrator : MonoBehaviour
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00041494 File Offset: 0x0003F694
	public static void VibrateInterval(bool smallMotor, float bigMotor, float time)
	{
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00041498 File Offset: 0x0003F698
	public static void SetForcedVibration(bool smallMotor, float bigMotor)
	{
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0004149C File Offset: 0x0003F69C
	public static void UnsetForcedVibration()
	{
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x000414A0 File Offset: 0x0003F6A0
	public static void SetBackgroundVibration(bool smallMotor, float bigMotor)
	{
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x000414A4 File Offset: 0x0003F6A4
	public static void UnsetBackgroundVibration()
	{
	}

	// Token: 0x04000A7F RID: 2687
	private static float time;

	// Token: 0x04000A80 RID: 2688
	private float lastVibrationSet;

	// Token: 0x04000A81 RID: 2689
	private bool vibrating;

	// Token: 0x04000A82 RID: 2690
	private static bool forceVibration;

	// Token: 0x04000A83 RID: 2691
	private static bool smallMotor;

	// Token: 0x04000A84 RID: 2692
	private static float bigMotor = 1f;

	// Token: 0x04000A85 RID: 2693
	private static bool backgroundVibration;

	// Token: 0x04000A86 RID: 2694
	private static bool backgroundSmallMotor;

	// Token: 0x04000A87 RID: 2695
	private static float backgroundBigMotor;

	// Token: 0x04000A88 RID: 2696
	private float lastBackgroundVibrationSet;
}
