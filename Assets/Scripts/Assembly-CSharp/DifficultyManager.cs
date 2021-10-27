using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class DifficultyManager : MonoBehaviour
{
	// Token: 0x040008F6 RID: 2294
	public static DifficultyManager.Difficulty difficulty = DifficultyManager.Difficulty.MEDIUM;

	// Token: 0x040008F7 RID: 2295
	public static float easyGeneral = 0.5f;

	// Token: 0x040008F8 RID: 2296
	public static float hardGeneral = 1.5f;

	// Token: 0x040008F9 RID: 2297
	public static float easyQuad = 0.5f;

	// Token: 0x040008FA RID: 2298
	public static float hardQuad = 1.5f;

	// Token: 0x040008FB RID: 2299
	public static float easyMelee = 0.5f;

	// Token: 0x040008FC RID: 2300
	public static float hardMelee = 1.1f;

	// Token: 0x040008FD RID: 2301
	public static int easyCarHits = 10;

	// Token: 0x040008FE RID: 2302
	public static int mediumCarHits = 8;

	// Token: 0x040008FF RID: 2303
	public static int hardCarHits = 6;

	// Token: 0x02000149 RID: 329
	public enum Difficulty
	{
		// Token: 0x04000901 RID: 2305
		EASY,
		// Token: 0x04000902 RID: 2306
		MEDIUM,
		// Token: 0x04000903 RID: 2307
		HARD
	}
}
