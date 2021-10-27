using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class AgilityTrigger : MonoBehaviour
{
	// Token: 0x06000944 RID: 2372 RVA: 0x0004DF64 File Offset: 0x0004C164
	private void Awake()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		if (this.player != null)
		{
			this.basicAgility = this.player.GetComponent<BasicAgility>();
		}
		if (this.path == string.Empty && base.gameObject.GetComponent<iTweenPath>() != null)
		{
			this.path = base.gameObject.GetComponent<iTweenPath>().pathName;
		}
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0004DFE4 File Offset: 0x0004C1E4
	private void Update()
	{
		if (this.basicAgility != null && Time.time <= this.basicAgility.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
		{
			return;
		}
		AgilityTrigger.AgilityTypes triggerType = this.TriggerType;
		if (triggerType != AgilityTrigger.AgilityTypes.MEDIUM_JUMP)
		{
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0004E03C File Offset: 0x0004C23C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && (this.TriggerType == AgilityTrigger.AgilityTypes.MEDIUM_JUMP || this.TriggerType == AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE1 || this.TriggerType == AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE2 || this.TriggerType == AgilityTrigger.AgilityTypes.SHORT_JUMP || this.TriggerType == AgilityTrigger.AgilityTypes.JUMP_LEDGE1))
		{
			this.basicAgility.agilityType = this.TriggerType;
			this.basicAgility.insideLongJump = true;
			this.basicAgility.longJumpEndPosition = base.transform.Find("LongJumpEndPosition").transform;
			if (this.ledge != null)
			{
				this.basicAgility.ledge = this.ledge;
			}
			else
			{
				this.basicAgility.ledge = null;
			}
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0004E10C File Offset: 0x0004C30C
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && (this.TriggerType == AgilityTrigger.AgilityTypes.MEDIUM_JUMP || this.TriggerType == AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE1 || this.TriggerType == AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE2 || this.TriggerType == AgilityTrigger.AgilityTypes.SHORT_JUMP || this.TriggerType == AgilityTrigger.AgilityTypes.JUMP_LEDGE1) && this.basicAgility.longJumpEndPosition == base.transform.Find("LongJumpEndPosition").transform)
		{
			this.basicAgility.insideLongJump = false;
		}
	}

	// Token: 0x04000CBE RID: 3262
	public AgilityTrigger.AgilityTypes TriggerType;

	// Token: 0x04000CBF RID: 3263
	public string path;

	// Token: 0x04000CC0 RID: 3264
	public AgilityLedge ledge;

	// Token: 0x04000CC1 RID: 3265
	private GameObject player;

	// Token: 0x04000CC2 RID: 3266
	private BasicAgility basicAgility;

	// Token: 0x04000CC3 RID: 3267
	private float timeBeforeAcceptingNewUserInput = 0.5f;

	// Token: 0x020001D5 RID: 469
	public enum AgilityTypes
	{
		// Token: 0x04000CC5 RID: 3269
		MEDIUM_JUMP,
		// Token: 0x04000CC6 RID: 3270
		LONG_JUMP_LEDGE1,
		// Token: 0x04000CC7 RID: 3271
		LONG_JUMP_LEDGE2,
		// Token: 0x04000CC8 RID: 3272
		SHORT_JUMP,
		// Token: 0x04000CC9 RID: 3273
		JUMP_LEDGE1
	}
}
