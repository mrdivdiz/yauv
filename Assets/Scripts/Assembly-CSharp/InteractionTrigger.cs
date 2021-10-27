using System;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class InteractionTrigger : MonoBehaviour
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x00063370 File Offset: 0x00061570
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		if (this.player != null)
		{
			this.playerInteraction = this.player.GetComponent<Interaction>();
			this.basicAgility = this.player.GetComponent<BasicAgility>();
		}
		if (this.path == string.Empty && base.gameObject.GetComponent<iTweenPath>() != null)
		{
			this.path = base.gameObject.GetComponent<iTweenPath>().pathName;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00063404 File Offset: 0x00061604
	private void Awake()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		if (this.player != null)
		{
			this.playerInteraction = this.player.GetComponent<Interaction>();
			this.basicAgility = this.player.GetComponent<BasicAgility>();
		}
		if (this.path == string.Empty && base.gameObject.GetComponent<iTweenPath>() != null)
		{
			this.path = base.gameObject.GetComponent<iTweenPath>().pathName;
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00063498 File Offset: 0x00061698
	private void Update()
	{
		if (this.playerInteraction == null || Time.time <= this.playerInteraction.lastUserInputTime + this.timeBeforeAcceptingNewUserInput || (AnimationHandler.instance != null && (AnimationHandler.instance.animState == AnimationHandler.AnimStates.JUMPING || AnimationHandler.instance.animState == AnimationHandler.AnimStates.FALLING)))
		{
			return;
		}
		this.coverButton = (MobileInput.cover || InputManager.GetButtonDown("Cover"));
		this.interactionButton = (MobileInput.interaction || InputManager.GetButtonDown("Interaction"));
		switch (this.TriggerType)
		{
		case InteractionTrigger.InteractionTypes.POLE:
			if (this.inside && this.interactionButton && !this.playerInteraction.poleClimbMode)
			{
				this.playerInteraction.EnterPoleMode = true;
				this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				Instructions.instruction = Instructions.Instruction.NONE;
			}
			break;
		case InteractionTrigger.InteractionTypes.ROPE:
			if (this.inside && this.interactionButton && !this.playerInteraction.ropeClimbMode)
			{
				this.playerInteraction.EnterRopeMode = true;
				this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
				this.playerInteraction.topStartPosition = base.transform.Find("TopStartPosition").transform;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				Instructions.instruction = Instructions.Instruction.NONE;
			}
			break;
		case InteractionTrigger.InteractionTypes.LADDER:
			if (this.inside && this.interactionButton && !this.playerInteraction.ladderClimbMode)
			{
				this.playerInteraction.EnterLadderMode = true;
				this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
				this.playerInteraction.buttomEndPosition = base.transform.Find("ButtomEndPosition").transform;
				this.playerInteraction.topStartPosition = base.transform.Find("TopStartPosition").transform;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				this.playerInteraction.currentInteractionTrigger = this;
				Instructions.instruction = Instructions.Instruction.NONE;
				if (!this.canDrop)
				{
					MobileInput.instance.disableButton("interaction", base.gameObject);
				}
			}
			break;
		case InteractionTrigger.InteractionTypes.WALL_BACK:
			if (this.inside && this.interactionButton && !this.playerInteraction.wallBackMode)
			{
				this.playerInteraction.EnterWallBackMode = true;
				this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				Instructions.instruction = Instructions.Instruction.NONE;
			}
			break;
		case InteractionTrigger.InteractionTypes.COVERTALL:
			if (this.playerInteraction.weaponHandler != null && this.inside && this.coverButton && !this.playerInteraction.coverTallMode && !this.basicAgility.animating)
			{
				if (this.path == string.Empty)
				{
					this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
					this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
					this.playerInteraction.path = string.Empty;
				}
				else
				{
					this.playerInteraction.path = this.path;
					this.playerInteraction.lookAtPoint = base.transform.position;
				}
				this.playerInteraction.EnterCoverTallMode = true;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				this.playerInteraction.cover = this;
				Instructions.instruction = Instructions.Instruction.NONE;
				Interaction.preventExitingCover = this.preventExitingCover;
				this.preventExitingCover = false;
				base.gameObject.SendMessage("OnCoverEnter", SendMessageOptions.DontRequireReceiver);
			}
			break;
		case InteractionTrigger.InteractionTypes.COVERSHORT:
			if (this.playerInteraction.weaponHandler != null && this.inside && this.coverButton && !this.playerInteraction.coverShortMode && !this.basicAgility.animating)
			{
				if (this.path == string.Empty)
				{
					this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
					this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
					this.playerInteraction.path = string.Empty;
				}
				else
				{
					this.playerInteraction.path = this.path;
					this.playerInteraction.lookAtPoint = base.transform.position;
				}
				this.playerInteraction.EnterCoverShortMode = true;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				this.playerInteraction.cover = this;
				Instructions.instruction = Instructions.Instruction.NONE;
			}
			break;
		case InteractionTrigger.InteractionTypes.LOG:
			if (this.inside && !this.playerInteraction.logMode)
			{
				this.playerInteraction.EnterLogMode = true;
				this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				this.playerInteraction.endPosition = base.transform.Find("EndPosition").transform;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				Instructions.instruction = Instructions.Instruction.NONE;
			}
			break;
		case InteractionTrigger.InteractionTypes.PUSH:
			if (this.inside && this.interactionButton && !this.playerInteraction.pushingMode)
			{
				this.playerInteraction.EnterPushingMode = true;
				this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				this.playerInteraction.pushableObject = this.pushableObject;
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				Instructions.instruction = Instructions.Instruction.NONE;
				UnityEngine.Object.Destroy(base.gameObject, 0.5f);
			}
			break;
		case InteractionTrigger.InteractionTypes.KICK:
			if (this.inside && this.interactionButton && !this.playerInteraction.kickingMode)
			{
				this.playerInteraction.EnterKickingMode = true;
				if (base.transform.Find("StartPosition") != null)
				{
					this.playerInteraction.startPosition = base.transform.Find("StartPosition").transform;
				}
				if (this.pushableObject != null)
				{
					this.playerInteraction.pushableObject = this.pushableObject;
				}
				else
				{
					this.playerInteraction.pushableObject = base.transform.parent.gameObject;
				}
				this.playerInteraction.lastUserInputTime = Time.time;
				this.playerInteraction.interactionType = this.TriggerType;
				Instructions.instruction = Instructions.Instruction.NONE;
				UnityEngine.Object.Destroy(base.gameObject, 0.5f);
			}
			break;
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00063D00 File Offset: 0x00061F00
	public void OnDestroy()
	{
		if (MobileInput.instance != null)
		{
			MobileInput.instance.disableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00063D28 File Offset: 0x00061F28
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.playerInteraction.coverTallMode && !this.playerInteraction.coverShortMode)
		{
			this.inside = true;
			if (this.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL || this.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
			{
				MobileInput.instance.enableButton("cover", base.gameObject);
			}
			else
			{
				MobileInput.instance.enableButton("interaction", base.gameObject);
			}
			if (this.playerInteraction.basicAgility != null && (this.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT || this.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL))
			{
				this.playerInteraction.basicAgility.canRoll = false;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.POLE && this.inside && !this.playerInteraction.poleClimbMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.ROPE && this.inside && !this.playerInteraction.ropeClimbMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.LADDER && this.inside && !this.playerInteraction.ladderClimbMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.WALL_BACK && this.inside && !this.playerInteraction.wallBackMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL && this.inside && !this.playerInteraction.coverTallMode && this.playerInteraction.weaponHandler != null && !AnimationHandler.instance.isInInteraction)
			{
				Instructions.instruction = Instructions.Instruction.COVER;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT && this.inside && !this.playerInteraction.coverShortMode && this.playerInteraction.weaponHandler != null && !AnimationHandler.instance.isInInteraction)
			{
				Instructions.instruction = Instructions.Instruction.COVER;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.PUSH && this.inside && !this.playerInteraction.pushingMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
				Instructions.instructionSetter = base.gameObject;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.KICK && this.inside && !this.playerInteraction.kickingMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
				Instructions.instructionSetter = base.gameObject;
			}
		}
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00064000 File Offset: 0x00062200
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			if (this.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL || this.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
			{
				MobileInput.instance.disableButton("cover", base.gameObject);
			}
			else
			{
				MobileInput.instance.disableButton("interaction", base.gameObject);
			}
			if (this.playerInteraction.basicAgility != null && (this.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT || this.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL))
			{
				this.playerInteraction.basicAgility.canRoll = true;
			}
			if (this.TriggerType == InteractionTrigger.InteractionTypes.LOG)
			{
				this.playerInteraction.ExitLogMode();
			}
			if (Instructions.instructionSetter == base.gameObject)
			{
				Instructions.instruction = Instructions.Instruction.NONE;
			}
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x000640E4 File Offset: 0x000622E4
	private void OnDrawGizmos()
	{
		if (base.gameObject.GetComponent<iTweenPath>() == null && (this.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT || this.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL))
		{
			Vector3 position = base.transform.Find("StartPosition").transform.position;
			Vector3 position2 = base.transform.Find("EndPosition").transform.position;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(position, position2);
		}
		else if (this.TriggerType == InteractionTrigger.InteractionTypes.WALL_BACK || this.TriggerType == InteractionTrigger.InteractionTypes.LOG)
		{
			Vector3 position3 = base.transform.Find("StartPosition").transform.position;
			Vector3 position4 = base.transform.Find("EndPosition").transform.position;
			Gizmos.color = Color.green;
			Gizmos.DrawLine(position3, position4);
		}
	}

	// Token: 0x04000EA4 RID: 3748
	public InteractionTrigger.InteractionTypes TriggerType;

	// Token: 0x04000EA5 RID: 3749
	public string path;

	// Token: 0x04000EA6 RID: 3750
	public GameObject pushableObject;

	// Token: 0x04000EA7 RID: 3751
	[HideInInspector]
	public bool isUsedByAI;

	// Token: 0x04000EA8 RID: 3752
	private GameObject player;

	// Token: 0x04000EA9 RID: 3753
	private Interaction playerInteraction;

	// Token: 0x04000EAA RID: 3754
	public bool inside;

	// Token: 0x04000EAB RID: 3755
	private float timeBeforeAcceptingNewUserInput = 0.5f;

	// Token: 0x04000EAC RID: 3756
	private bool coverButton;

	// Token: 0x04000EAD RID: 3757
	private bool interactionButton;

	// Token: 0x04000EAE RID: 3758
	public bool rightPopout = true;

	// Token: 0x04000EAF RID: 3759
	public bool leftPopout = true;

	// Token: 0x04000EB0 RID: 3760
	public InteractionTrigger rightCover;

	// Token: 0x04000EB1 RID: 3761
	public InteractionTrigger rightCornerCover;

	// Token: 0x04000EB2 RID: 3762
	public InteractionTrigger leftCover;

	// Token: 0x04000EB3 RID: 3763
	public InteractionTrigger leftCornerCover;

	// Token: 0x04000EB4 RID: 3764
	public bool laderExitButtom = true;

	// Token: 0x04000EB5 RID: 3765
	public AgilityLedge connectingLedge;

	// Token: 0x04000EB6 RID: 3766
	public bool canDrop = true;

	// Token: 0x04000EB7 RID: 3767
	public BasicAgility basicAgility;

	// Token: 0x04000EB8 RID: 3768
	public bool preventExitingCover;

	// Token: 0x020001EB RID: 491
	public enum InteractionTypes
	{
		// Token: 0x04000EBA RID: 3770
		POLE,
		// Token: 0x04000EBB RID: 3771
		ROPE,
		// Token: 0x04000EBC RID: 3772
		LADDER,
		// Token: 0x04000EBD RID: 3773
		WALL_BACK,
		// Token: 0x04000EBE RID: 3774
		COVERTALL,
		// Token: 0x04000EBF RID: 3775
		COVERSHORT,
		// Token: 0x04000EC0 RID: 3776
		LOG,
		// Token: 0x04000EC1 RID: 3777
		PUSH,
		// Token: 0x04000EC2 RID: 3778
		KICK
	}
}
