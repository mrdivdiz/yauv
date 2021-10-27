using System;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class AndroidPlatform : MonoBehaviour
{
	// Token: 0x06000A4A RID: 2634 RVA: 0x00070250 File Offset: 0x0006E450
	internal static bool IsIAPavailable()
	{
		return AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS;
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00070260 File Offset: 0x0006E460
	public static bool IsJoystickConnected()
	{
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.Ouya)
		{
			return true;
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS)
		{
			return AndroidPlatform.isJoystickConnected;
		}
		return AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GooglePlay && AndroidPlatform.isJoystickConnected;
	}

	// Token: 0x0400106E RID: 4206
	public static AndroidPlatform.AndroidPlatforms platform;

	// Token: 0x0400106F RID: 4207
	public static bool isJoystickConnected;

	// Token: 0x04001070 RID: 4208
	public static bool isTegra4;

	// Token: 0x04001071 RID: 4209
	public static bool isAmazonBuld = true;

	// Token: 0x0200020E RID: 526
	public enum AndroidPlatforms
	{
		// Token: 0x04001073 RID: 4211
		GooglePlay,
		// Token: 0x04001074 RID: 4212
		Amazon,
		// Token: 0x04001075 RID: 4213
		GameStick,
		// Token: 0x04001076 RID: 4214
		Ouya,
		// Token: 0x04001077 RID: 4215
		GameShield,
		// Token: 0x04001078 RID: 4216
		IOS
	}
}
