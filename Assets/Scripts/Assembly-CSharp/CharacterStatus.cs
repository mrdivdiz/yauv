using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class CharacterStatus : MonoBehaviour
{
	// Token: 0x06000968 RID: 2408 RVA: 0x00054BD8 File Offset: 0x00052DD8
	private void Start()
	{
		this.currentStatus = this.charachterStatus;
		this.currentLocomotionGroup = this.relaxedLocomotionGroup;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00054BF4 File Offset: 0x00052DF4
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.currentStatus != this.charachterStatus)
		{
			switch (this.charachterStatus)
			{
			case CharacterStatus.CharacterStatusTypes.RELAXED:
				this.currentLocomotionGroup = this.relaxedLocomotionGroup;
				break;
			case CharacterStatus.CharacterStatusTypes.ENGAGED:
				this.currentLocomotionGroup = this.engagedLocomotionGroup;
				break;
			case CharacterStatus.CharacterStatusTypes.STEALTH:
				this.currentLocomotionGroup = this.stealthLocomotionGroup;
				break;
			}
			this.currentStatus = this.charachterStatus;
		}
	}

	// Token: 0x04000D7D RID: 3453
	public CharacterStatus.CharacterStatusTypes charachterStatus;

	// Token: 0x04000D7E RID: 3454
	public string relaxedLocomotionGroup = "locomotion";

	// Token: 0x04000D7F RID: 3455
	public string engagedLocomotionGroup = "engagedLocomotion";

	// Token: 0x04000D80 RID: 3456
	public string stealthLocomotionGroup = "stealthLocomotion";

	// Token: 0x04000D81 RID: 3457
	[HideInInspector]
	public string currentLocomotionGroup;

	// Token: 0x04000D82 RID: 3458
	private CharacterStatus.CharacterStatusTypes currentStatus;

	// Token: 0x020001DD RID: 477
	public enum CharacterStatusTypes
	{
		// Token: 0x04000D84 RID: 3460
		RELAXED,
		// Token: 0x04000D85 RID: 3461
		ENGAGED,
		// Token: 0x04000D86 RID: 3462
		STEALTH
	}
}
