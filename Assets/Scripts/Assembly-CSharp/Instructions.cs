using System;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class Instructions : MonoBehaviour
{
	// Token: 0x0600099A RID: 2458 RVA: 0x00058560 File Offset: 0x00056760
	private void Awake()
	{
		Instructions.instance = this;
		this.Attack = this.PS3Attack;
		this.Climb = this.PS3Climb;
		this.Cover = this.PS3Cover;
		this.Interact = this.PS3Interact;
		this.Drop = this.PS3Drop;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x000585B0 File Offset: 0x000567B0
	private void OnDestroy()
	{
		Instructions.instance = null;
		Instructions.instructionSetter = null;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x000585C0 File Offset: 0x000567C0
	private void Update()
	{
		if ((this.previousInstruction != Instructions.instruction && Instructions.instruction != Instructions.Instruction.NONE) || (Instructions.previousBasicAgilityJumpButton != Instructions.basicAgilityJumpButton && Instructions.basicAgilityJumpButton))
		{
			Instructions.lastInstructionTime = Time.time;
		}
		if ((Instructions.instruction != Instructions.Instruction.NONE || Instructions.basicAgilityJumpButton) && Time.time > Instructions.lastInstructionTime + Instructions.timeToShowInstructions)
		{
			Instructions.instruction = Instructions.Instruction.NONE;
			Instructions.basicAgilityJumpButton = false;
		}
		this.previousInstruction = Instructions.instruction;
		Instructions.previousBasicAgilityJumpButton = Instructions.basicAgilityJumpButton;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00058654 File Offset: 0x00056854
	private void OnGUI()
	{
		if (!AndroidPlatform.IsJoystickConnected())
		{
			return;
		}
		if (mainmenu.pause || mainmenu.disableHUD)
		{
			return;
		}
		if (SpeechManager.instance != null && SpeechManager.instance.subtitleString != null && SpeechManager.instance.subtitleString != string.Empty)
		{
			return;
		}
		float num = 0f;
		if (Instructions.basicAgilityJumpButton)
		{
			Texture texture = this.Climb;
			if (Instructions.instruction == Instructions.Instruction.NONE)
			{
				GUI.Label(new Rect((float)(Screen.width / 2 - texture.width / 2), (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			}
			else
			{
				num = (float)(Screen.width / 2 - texture.width / 2) - (float)texture.width / 2f;
				GUI.Label(new Rect(num + (float)texture.width, (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			}
		}
		else
		{
			Texture texture = this.Attack;
			num = (float)(Screen.width / 2 - texture.width / 2);
		}
		if (Instructions.instruction == Instructions.Instruction.NONE)
		{
			return;
		}
		switch (Instructions.instruction)
		{
		case Instructions.Instruction.ATTACK:
		{
			Texture texture = this.Attack;
			GUI.Label(new Rect(num, (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			break;
		}
		case Instructions.Instruction.CLIMB:
		{
			Texture texture = this.Climb;
			GUI.Label(new Rect(num, (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			break;
		}
		case Instructions.Instruction.COVER:
		{
			Texture texture = this.Cover;
			GUI.Label(new Rect(num, (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			break;
		}
		case Instructions.Instruction.INTERACT:
		{
			Texture texture = this.Interact;
			GUI.Label(new Rect(num, (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			break;
		}
		case Instructions.Instruction.DROP:
		{
			Texture texture = this.Drop;
			GUI.Label(new Rect(num, (float)(Screen.height / 2 - texture.height / 2 + Screen.height * 3 / 8), (float)texture.width, (float)texture.height), texture);
			break;
		}
		}
	}

	// Token: 0x04000E00 RID: 3584
	[HideInInspector]
	public Texture Attack;

	// Token: 0x04000E01 RID: 3585
	[HideInInspector]
	public Texture Climb;

	// Token: 0x04000E02 RID: 3586
	[HideInInspector]
	public Texture Cover;

	// Token: 0x04000E03 RID: 3587
	[HideInInspector]
	public Texture Interact;

	// Token: 0x04000E04 RID: 3588
	[HideInInspector]
	public Texture Drop;

	// Token: 0x04000E05 RID: 3589
	public Texture PCAttack;

	// Token: 0x04000E06 RID: 3590
	public Texture PCClimb;

	// Token: 0x04000E07 RID: 3591
	public Texture PCCover;

	// Token: 0x04000E08 RID: 3592
	public Texture PCInteract;

	// Token: 0x04000E09 RID: 3593
	public Texture PCDrop;

	// Token: 0x04000E0A RID: 3594
	public Texture xBoxAttack;

	// Token: 0x04000E0B RID: 3595
	public Texture xBoxClimb;

	// Token: 0x04000E0C RID: 3596
	public Texture xBoxCover;

	// Token: 0x04000E0D RID: 3597
	public Texture xBoxInteract;

	// Token: 0x04000E0E RID: 3598
	public Texture xBoxDrop;

	// Token: 0x04000E0F RID: 3599
	public Texture PS3Attack;

	// Token: 0x04000E10 RID: 3600
	public Texture PS3Climb;

	// Token: 0x04000E11 RID: 3601
	public Texture PS3Cover;

	// Token: 0x04000E12 RID: 3602
	public Texture PS3Interact;

	// Token: 0x04000E13 RID: 3603
	public Texture PS3Drop;

	// Token: 0x04000E14 RID: 3604
	public static Instructions instance;

	// Token: 0x04000E15 RID: 3605
	public static Instructions.Instruction instruction;

	// Token: 0x04000E16 RID: 3606
	public static float lastInstructionTime;

	// Token: 0x04000E17 RID: 3607
	public static GameObject instructionSetter;

	// Token: 0x04000E18 RID: 3608
	public static float timeToShowInstructions = 5f;

	// Token: 0x04000E19 RID: 3609
	private Instructions.Instruction previousInstruction;

	// Token: 0x04000E1A RID: 3610
	public static bool basicAgilityJumpButton;

	// Token: 0x04000E1B RID: 3611
	public static bool previousBasicAgilityJumpButton;

	// Token: 0x04000E1C RID: 3612
	public bool joystickOnPC;

	// Token: 0x020001E8 RID: 488
	public enum Instruction
	{
		// Token: 0x04000E1E RID: 3614
		NONE,
		// Token: 0x04000E1F RID: 3615
		ATTACK,
		// Token: 0x04000E20 RID: 3616
		CLIMB,
		// Token: 0x04000E21 RID: 3617
		COVER,
		// Token: 0x04000E22 RID: 3618
		INTERACT,
		// Token: 0x04000E23 RID: 3619
		DROP
	}
}
