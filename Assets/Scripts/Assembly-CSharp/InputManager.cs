using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class InputManager : MonoBehaviour
{
	// Token: 0x060007AE RID: 1966 RVA: 0x0003F6F0 File Offset: 0x0003D8F0
	public static bool GetButton(string buttonName)
	{
		return Input.GetButton(buttonName);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0003F6F8 File Offset: 0x0003D8F8
	public static bool GetButtonDown(string buttonName)
	{
		return Input.GetButtonDown(buttonName);
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0003F700 File Offset: 0x0003D900
	public static bool GetButtonUp(string buttonName)
	{
		return Input.GetButtonUp(buttonName);
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0003F708 File Offset: 0x0003D908
	public static KeyCode GetKeyCode(string buttonName)
	{
		switch (buttonName)
		{
		case "Jump":
			return KeyCode.JoystickButton14;
		case "Cover":
			return KeyCode.JoystickButton13;
		case "Interaction":
			return KeyCode.JoystickButton12;
		case "Attack":
			return KeyCode.JoystickButton15;
		case "Start":
			return KeyCode.JoystickButton0;
		case "Fire1":
			return KeyCode.JoystickButton9;
		case "Fire2":
			return KeyCode.JoystickButton8;
		case "SecondaryWeapon2":
			return KeyCode.JoystickButton7;
		case "PrimaryWeapon2":
			return KeyCode.JoystickButton5;
		case "Grenade":
			return KeyCode.JoystickButton10;
		}
		return KeyCode.None;
	}
}
