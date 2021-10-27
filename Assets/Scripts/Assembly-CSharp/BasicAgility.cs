using System;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class BasicAgility : MonoBehaviour
{
	// Token: 0x0600095D RID: 2397 RVA: 0x000510BC File Offset: 0x0004F2BC
	private void Start()
	{
		this.animationHandller = base.GetComponent<AnimationHandler>();
		this.cameraTransform = GlobalFarisCam.farisCamera.transform;
		if (this.ncm == null)
		{
			this.ncm = base.transform.GetComponent<NormalCharacterMotor>();
		}
		this.mask = 1 << base.gameObject.layer;
		this.mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.mask = ~this.mask;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00051164 File Offset: 0x0004F364
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		if (this.survivalCharacter)
		{
			if (this.ncm.grounded)
			{
				this.lastGroundedY = base.transform.position.y;
			}
			else if (this.lastGroundedY != 999f && !Physics.Raycast(base.transform.position, -base.transform.up, 10f, this.mask))
			{
				base.transform.position = new Vector3(base.transform.position.x, this.lastGroundedY, base.transform.position.z);
			}
		}
		if (AnimationHandler.instance.isInInteraction)
		{
			Instructions.basicAgilityJumpButton = false;
			return;
		}
		this.jumpButton = (MobileInput.jump || InputManager.GetButtonDown("Jump"));
		this.coverButton = (MobileInput.interaction || InputManager.GetButton("Cover"));
		this.interactionButton = (MobileInput.interaction || InputManager.GetButton("Interaction"));
		this.HorizontalAxis = PlatformCharacterController.joystickLeft.position.x + Input.GetAxis("Horizontal");
		this.VerticalAxis = PlatformCharacterController.joystickLeft.position.y + Input.GetAxis("Vertical");
		if (this.animating && !this.longJumping)
		{
			this.animatingTimer -= Time.deltaTime;
			if (this.animatingTimer <= 0f)
			{
				this.animating = false;
				this.Physicalize();
			}
			if (this.shortJumpingFixed)
			{
				float num = 0.13157f;
				float num2 = 0.44736f;
				string name = "Agility-Jump-Short-Full";
				if (base.GetComponent<Animation>()[name].normalizedTime > num && base.GetComponent<Animation>()[name].normalizedTime < num2)
				{
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, Mathf.Lerp(base.transform.rotation.eulerAngles.y, this.longJumpEndPosition.rotation.eulerAngles.y, (base.GetComponent<Animation>()[name].normalizedTime - num) / (num2 - num)), base.transform.rotation.eulerAngles.z);
					base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(this.longJumpEndPosition.position.x, base.transform.position.y, this.longJumpEndPosition.position.z), (base.GetComponent<Animation>()[name].normalizedTime - num) / (num2 - num));
				}
			}
			return;
		}
		if ((AnimationHandler.instance != null && AnimationHandler.instance.insuredMode) || this.dontPerformAgility)
		{
			return;
		}
		if (this.longJumping)
		{
			this.longJumpTimer -= Time.deltaTime;
			float num3 = 0.143f;
			float num4 = 0.6286f;
			string name2 = "Agility-Jump-Medium";
			string text = "Agility-Ledge1-Idle";
			switch (this.agilityType)
			{
			case AgilityTrigger.AgilityTypes.MEDIUM_JUMP:
				num3 = 0.143f;
				num4 = 0.6286f;
				name2 = "Agility-Jump-Medium";
				text = "Agility-Ledge1-Idle";
				break;
			case AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE1:
				num3 = 0.176f;
				num4 = 0.4153f;
				name2 = "Agility-Jump-Long-Ledge1";
				text = "Agility-Ledge1-Idle";
				break;
			case AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE2:
				num3 = 0.176f;
				num4 = 0.4153f;
				name2 = "Agility-Jump-Long-Ledge2";
				text = "Agility-Ledge2-Idle";
				break;
			case AgilityTrigger.AgilityTypes.JUMP_LEDGE1:
				num3 = 0.45f;
				num4 = 0.7f;
				name2 = "Agility-Ledge1-Jump";
				text = "Agility-Ledge1-Idle";
				break;
			}
			if (this.jumpingBackwords)
			{
				num3 = 0.21f;
				num4 = 0.38f;
				name2 = this.jumpingBackAnimation;
				text = "Agility-Ledge1-Idle";
			}
			if (this.adjustPosition)
			{
				num3 = 0f;
				num4 = 1f;
				name2 = this.adjustAnim;
				text = "Agility-Ledge1-Idle";
			}
			if (!this.disableAdjustment && base.GetComponent<Animation>()[name2].normalizedTime > num3 && base.GetComponent<Animation>()[name2].normalizedTime < num4)
			{
				if (this.jumpingBackwords)
				{
					base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(this.longJumpEndPositionPosition.x, this.longJumpEndPositionPosition.y, this.longJumpEndPositionPosition.z), (base.GetComponent<Animation>()[name2].normalizedTime - num3) / (num4 - num3));
				}
				else if (this.adjustPosition)
				{
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, Mathf.Lerp(base.transform.rotation.eulerAngles.y, this.longJumpEndPositionRotation.eulerAngles.y, (base.GetComponent<Animation>()[name2].normalizedTime - num3) / (num4 - num3)), base.transform.rotation.eulerAngles.z);
					base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(this.longJumpEndPositionPosition.x, this.longJumpEndPositionPosition.y, this.longJumpEndPositionPosition.z), (base.GetComponent<Animation>()[name2].normalizedTime - num3) / (num4 - num3));
				}
				else
				{
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, Mathf.Lerp(base.transform.rotation.eulerAngles.y, this.longJumpEndPosition.rotation.eulerAngles.y, (base.GetComponent<Animation>()[name2].normalizedTime - num3) / (num4 - num3)), base.transform.rotation.eulerAngles.z);
					base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(this.longJumpEndPosition.position.x, this.longJumpEndPosition.position.y, this.longJumpEndPosition.position.z), (base.GetComponent<Animation>()[name2].normalizedTime - num3) / (num4 - num3));
				}
			}
			if (this.longJumpTimer <= 0f)
			{
				Vector3 position = GameObject.Find(base.transform.root.name + "/Bip01/Root").transform.position;
				base.GetComponent<Animation>().Stop();
				this.adjustPosition = false;
				if (this.rootMotion)
				{
					base.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>()[text].layer = 7;
					base.GetComponent<Animation>()[text].speed = 1f;
					base.GetComponent<Animation>().Play(text, PlayMode.StopAll);
					base.transform.position = position;
					if (this.agilityType == AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE2)
					{
						if (!this.turningLeftCorner && !this.turningRightCorner)
						{
							base.transform.Translate(0f, 0.25f, -0.45f);
						}
						else if (this.turningLeftCorner && !this.corner2)
						{
							base.transform.Translate(-0.31f, 0.2475f, 0.1f);
						}
						else if (this.turningRightCorner && !this.corner2)
						{
							base.transform.Translate(0.32f, 0.2475f, 0.1f);
						}
						else if (this.turningLeftCorner && this.corner2)
						{
							base.transform.Translate(-0.23f, 0.2525f, -0.2f);
						}
						else if (this.turningRightCorner && this.corner2)
						{
							base.transform.Translate(0.23f, 0.2525f, -0.2f);
						}
						this.corner2 = false;
					}
					if (this.jumpingBackwords)
					{
						base.transform.Translate(0f, 0f, -0.05f);
					}
					if (this.turningLeftCorner)
					{
						base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
						this.turningLeftCorner = false;
					}
					if (this.turningRightCorner)
					{
						base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
						this.turningRightCorner = false;
					}
					if (this.jumpingBackwords)
					{
						base.transform.Rotate(0f, 180f, 0f);
						this.jumpingBackwords = false;
					}
				}
				this.ledgeHanging = true;
				Instructions.instruction = Instructions.Instruction.DROP;
				if (this.ledge == null || (this.ledge != null && this.ledge.canClimb))
				{
					Instructions.basicAgilityJumpButton = true;
				}
				MobileInput.instance.enableButton("interaction", base.gameObject);
				MobileInput.instance.enableButton("climb", base.gameObject);
				this.longJumping = false;
				this.insideLongJump = false;
				this.animating = false;
				this.longJumpEndPosition = null;
				this.disableAdjustment = false;
			}
			return;
		}
		if (this.ledgeHanging)
		{
			this.HandleLedgeHanging();
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(this.agilityRaycaster.position, this.agilityRaycaster.forward, out raycastHit, 1f))
		{
			if (raycastHit.collider.tag == "kickable" && this.interactionButton)
			{
				this.lastUserInputTime = Time.time;
				base.GetComponent<Animation>()["Interaction-Kick-Door"].layer = 2;
				base.GetComponent<Animation>()["Interaction-Kick-Door"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().CrossFade("Interaction-Kick-Door", 0.3f, PlayMode.StopAll);
				return;
			}
			if (raycastHit.collider.tag == "Agility-WaistLevel")
			{
				Instructions.basicAgilityJumpButton = true;
				MobileInput.instance.enableButton("climb", base.gameObject);
				if (this.jumpButton)
				{
					this.lastUserInputTime = Time.time;
					float num5 = Vector3.Angle(base.transform.forward, raycastHit.normal * -1f);
					if (Vector3.Cross(raycastHit.normal * -1f, base.transform.forward).normalized.y > 0f)
					{
						num5 *= -1f;
					}
					iTween.RotateBy(base.gameObject, new Vector3(0f, num5 / 360f, 0f), 0.5f);
					RaycastHit raycastHit2;
					Physics.Raycast(this.agilityRaycaster.position, base.transform.forward, out raycastHit2, 1f);
					base.transform.Translate(0f, 0f, raycastHit2.distance - 0.5f);
					this.animatingTimer = base.GetComponent<Animation>()["Agility-Vault"].length;
					this.rootMotion = true;
					this.animating = true;
					this.ncm.disableMovement = true;
					this.ncm.disableRotation = true;
					base.GetComponent<Animation>()["Agility-Vault"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>()["Agility-Vault"].layer = 5;
					base.GetComponent<Animation>()["Agility-Vault"].speed = 1f;
					base.GetComponent<Animation>().CrossFade("Agility-Vault", 0.01f, PlayMode.StopAll);
					this.FarisHead.PlayOneShot(this.bodyVaultSound, SpeechManager.sfxVolume);
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
					MobileInput.instance.disableButton("climb", base.gameObject);
				}
				return;
			}
			if (raycastHit.collider.tag == "Agility-WaistLevel-Climb")
			{
				Instructions.basicAgilityJumpButton = true;
				MobileInput.instance.enableButton("climb", base.gameObject);
				if (this.jumpButton)
				{
					this.lastUserInputTime = Time.time;
					float num6 = Vector3.Angle(base.transform.forward, raycastHit.normal * -1f);
					if (Vector3.Cross(raycastHit.normal * -1f, base.transform.forward).normalized.y > 0f)
					{
						num6 *= -1f;
					}
					iTween.RotateBy(base.gameObject, new Vector3(0f, num6 / 360f, 0f), 0.5f);
					RaycastHit raycastHit3;
					Physics.Raycast(this.agilityRaycaster.position, base.transform.forward, out raycastHit3, 1f);
					base.transform.Translate(0f, 0f, raycastHit3.distance - 0.3f);
					this.animatingTimer = base.GetComponent<Animation>()["Agility-Climb-Waist"].length;
					this.rootMotion = true;
					this.animating = true;
					this.ncm.disableMovement = true;
					this.ncm.disableRotation = true;
					base.GetComponent<Animation>()["Agility-Climb-Waist"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>()["Agility-Climb-Waist"].layer = 5;
					base.GetComponent<Animation>()["Agility-Climb-Waist"].speed = 1f;
					base.GetComponent<Animation>().CrossFade("Agility-Climb-Waist", 0.01f, PlayMode.StopAll);
					this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
					MobileInput.instance.disableButton("climb", base.gameObject);
				}
				return;
			}
			if (raycastHit.collider.tag == "Agility-ChestLevel-Climb")
			{
				Instructions.basicAgilityJumpButton = true;
				MobileInput.instance.enableButton("climb", base.gameObject);
				if (this.jumpButton)
				{
					this.lastUserInputTime = Time.time;
					float num7 = Vector3.Angle(base.transform.forward, raycastHit.normal * -1f);
					if (Vector3.Cross(raycastHit.normal * -1f, base.transform.forward).normalized.y > 0f)
					{
						num7 *= -1f;
					}
					RaycastHit raycastHit4;
					Physics.Raycast(this.agilityRaycaster.position, base.transform.forward, out raycastHit4, 1f);
					base.transform.Translate(0f, 0f, raycastHit4.distance - 0.5f);
					iTween.RotateBy(base.gameObject, new Vector3(0f, num7 / 360f, 0f), 0.5f);
					this.animatingTimer = base.GetComponent<Animation>()["Agility-Climb-Head"].length;
					this.rootMotion = true;
					this.animating = true;
					this.ncm.disableMovement = true;
					this.ncm.disableRotation = true;
					base.GetComponent<Animation>()["Agility-Climb-Head"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>()["Agility-Climb-Head"].layer = 5;
					base.GetComponent<Animation>()["Agility-Climb-Head"].speed = 1f;
					base.GetComponent<Animation>().CrossFade("Agility-Climb-Head", 0.01f, PlayMode.StopAll);
					this.FarisHead.PlayOneShot(this.bodyChestLevelSound, SpeechManager.sfxVolume);
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
					MobileInput.instance.disableButton("climb", base.gameObject);
				}
				return;
			}
			if (raycastHit.collider.tag == "Agility-Fense")
			{
				Instructions.basicAgilityJumpButton = true;
				MobileInput.instance.enableButton("climb", base.gameObject);
				if (this.jumpButton && Time.time > this.lastUserInputTime + 2f)
				{
					this.lastUserInputTime = Time.time;
					float num8 = Vector3.Angle(base.transform.forward, raycastHit.normal * -1f);
					if (Vector3.Cross(raycastHit.normal * -1f, base.transform.forward).normalized.y > 0f)
					{
						num8 *= -1f;
					}
					RaycastHit raycastHit5;
					Physics.Raycast(this.agilityRaycaster.position, base.transform.forward, out raycastHit5, 1f);
					base.transform.Translate(0f, 0f, raycastHit5.distance - 0.5f);
					iTween.RotateBy(base.gameObject, new Vector3(0f, num8 / 360f, 0f), 0.5f);
					this.animatingTimer = base.GetComponent<Animation>()["Agility-Fence-Faris"].length;
					this.rootMotion = true;
					this.animating = true;
					this.ncm.disableMovement = true;
					this.ncm.disableRotation = true;
					base.GetComponent<Animation>()["Agility-Fence-Faris"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>()["Agility-Fence-Faris"].layer = 5;
					base.GetComponent<Animation>()["Agility-Fence-Faris"].speed = 1f;
					base.GetComponent<Animation>().CrossFade("Agility-Fence-Faris", 0.01f, PlayMode.StopAll);
					this.FarisHead.PlayOneShot(this.bodyChestLevelSound, SpeechManager.sfxVolume);
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
					MobileInput.instance.disableButton("climb", base.gameObject);
				}
				return;
			}
			if (raycastHit.collider.tag == "Torch" && this.interactionButton)
			{
				this.lastUserInputTime = Time.time;
				float num9 = Vector3.Angle(base.transform.forward, raycastHit.normal * -2f);
				if (Vector3.Cross(raycastHit.normal * -1f, base.transform.forward).normalized.y > 0f)
				{
					num9 *= -1f;
				}
				RaycastHit raycastHit6;
				Physics.Raycast(this.agilityRaycaster.position, base.transform.forward, out raycastHit6, 1f);
				base.transform.Translate(0f, 0f, raycastHit6.distance - 0.4f);
				iTween.RotateBy(base.gameObject, new Vector3(0f, num9 / 360f, 0f), 0.5f);
				this.animatingTimer = base.GetComponent<Animation>()["Torch"].length;
				this.animating = true;
				base.GetComponent<Animation>()["Torch"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>()["Torch"].layer = 5;
				base.GetComponent<Animation>()["Torch"].speed = 1f;
				base.GetComponent<Animation>().CrossFade("Torch", 0.01f, PlayMode.StopAll);
				return;
			}
		}
		else
		{
			MobileInput.instance.disableButton("climb", base.gameObject);
		}
		Instructions.basicAgilityJumpButton = false;
		Vector3 vector = Vector3.zero;
		if (this.insideLongJump)
		{
			vector = new Vector3(this.HorizontalAxis, 0f, this.VerticalAxis);
			vector = this.cameraTransform.TransformDirection(vector);
			vector.y = 0f;
		}
		if (this.insideLongJump && this.agilityType == AgilityTrigger.AgilityTypes.JUMP_LEDGE1 && this.ncm.grounded && this.ncm.canJump)
		{
			if (Vector3.Angle(vector, this.longJumpEndPosition.forward) < 40f || Vector3.Angle(base.transform.forward, this.longJumpEndPosition.forward) < 40f)
			{
				if (this.jumpButton)
				{
					this.lastUserInputTime = Time.time;
					string text2 = "Agility-Ledge1-Jump";
					iTween.RotateTo(base.gameObject, new Vector3(base.transform.eulerAngles.x, this.longJumpEndPosition.eulerAngles.y, base.transform.eulerAngles.z), 0.5f);
					base.GetComponent<Animation>()[text2].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>()[text2].layer = 7;
					base.GetComponent<Animation>()[text2].speed = 0.75f;
					base.GetComponent<Animation>().CrossFade(text2, 0.1f, PlayMode.StopAll);
					this.longJumpTimer = base.GetComponent<Animation>()[text2].length * 1.33f;
					this.FarisHead.PlayOneShot(this.bodyLedgeClimbSound, SpeechManager.sfxVolume);
					this.ncm.enabled = false;
					this.rootMotion = true;
					this.longJumping = true;
					this.animating = true;
					AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
					Instructions.instruction = Instructions.Instruction.NONE;
					MobileInput.instance.disableButton("climb", base.gameObject);
				}
				else
				{
					Instructions.instruction = Instructions.Instruction.CLIMB;
					MobileInput.instance.enableButton("climb", base.gameObject);
				}
			}
			else
			{
				if (Instructions.instruction != Instructions.Instruction.NONE)
				{
					Instructions.instruction = Instructions.Instruction.NONE;
				}
				MobileInput.instance.disableButton("climb", base.gameObject);
				if (this.jumpButton)
				{
					this.JumpInPlace();
				}
			}
		}
		else if (this.ncm.canJump && this.jumpButton && Time.time > this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput && this.VerticalAxis == 0f && this.HorizontalAxis == 0f && !AnimationHandler.instance.stealthMode)
		{
			this.JumpInPlace();
		}
		else if (this.insideLongJump && this.agilityType != AgilityTrigger.AgilityTypes.SHORT_JUMP && this.agilityType != AgilityTrigger.AgilityTypes.JUMP_LEDGE1 && Vector3.Angle(vector, this.longJumpEndPosition.forward) < 40f && this.ncm.grounded && this.ncm.canJump && this.jumpButton && (this.VerticalAxis > 0f || this.VerticalAxis < 0f || this.HorizontalAxis > 0f || this.HorizontalAxis < 0f))
		{
			iTween.RotateTo(base.gameObject, new Vector3(base.transform.eulerAngles.x, this.longJumpEndPosition.eulerAngles.y, base.transform.eulerAngles.z), 0.5f);
			this.lastUserInputTime = Time.time;
			string text3 = "Agility-Jump-Medium";
			switch (this.agilityType)
			{
			case AgilityTrigger.AgilityTypes.MEDIUM_JUMP:
				text3 = "Agility-Jump-Medium";
				break;
			case AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE1:
				text3 = "Agility-Jump-Long-Ledge1";
				break;
			case AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE2:
				text3 = "Agility-Jump-Long-Ledge2";
				break;
			case AgilityTrigger.AgilityTypes.JUMP_LEDGE1:
				text3 = "Agility-Ledge1-Jump";
				break;
			}
			base.GetComponent<Animation>()[text3].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>()[text3].layer = 7;
			base.GetComponent<Animation>()[text3].speed = 1f;
			base.GetComponent<Animation>().CrossFade(text3, 0.1f, PlayMode.StopAll);
			this.longJumpTimer = base.GetComponent<Animation>()[text3].length;
			this.ncm.enabled = false;
			this.rootMotion = true;
			this.longJumping = true;
			this.animating = true;
			this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
		}
		else if (this.insideLongJump && this.agilityType == AgilityTrigger.AgilityTypes.SHORT_JUMP && Vector3.Angle(vector, this.longJumpEndPosition.forward) < 40f && this.ncm.grounded && this.ncm.canJump && this.jumpButton && Time.time > this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput && (this.VerticalAxis > 0f || this.VerticalAxis < 0f || this.HorizontalAxis > 0f || this.HorizontalAxis < 0f))
		{
			iTween.RotateTo(base.gameObject, new Vector3(base.transform.eulerAngles.x, this.longJumpEndPosition.eulerAngles.y, base.transform.eulerAngles.z), 0.5f);
			this.lastUserInputTime = Time.time;
			this.animatingTimer = base.GetComponent<Animation>()["Agility-Jump-Short-Full"].length / 1.25f;
			this.rootMotion = true;
			this.animating = true;
			this.shortJumpingFixed = true;
			base.GetComponent<Animation>()["Agility-Jump-Short-Full"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>()["Agility-Jump-Short-Full"].layer = 5;
			base.GetComponent<Animation>()["Agility-Jump-Short-Full"].speed = 1.25f;
			base.GetComponent<Animation>().CrossFade("Agility-Jump-Short-Full", 0.01f, PlayMode.StopAll);
			this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
		}
		else if (!this.rollInsteadOfJump && this.ncm.grounded && this.ncm.canJump && this.jumpButton && Time.time > this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput && (this.VerticalAxis > 0f || this.VerticalAxis < 0f || this.HorizontalAxis > 0f || this.HorizontalAxis < 0f) && !AnimationHandler.instance.stealthMode)
		{
			this.lastUserInputTime = Time.time;
			vector = new Vector3(this.HorizontalAxis, 0f, this.VerticalAxis);
			vector = this.cameraTransform.TransformDirection(vector);
			vector.y = 0f;
			base.transform.rotation = Quaternion.Euler(base.transform.eulerAngles.x, base.transform.eulerAngles.y + Quaternion.FromToRotation(base.transform.forward, vector).eulerAngles.y, base.transform.eulerAngles.z);
			if (Application.loadedLevelName == "part1" || Application.loadedLevelName == "part2")
			{
				base.transform.GetComponent<CharacterMotorScript>().Impulse = base.transform.TransformDirection(new Vector3(0f, 4.4f, 7f));
			}
			else
			{
				base.transform.GetComponent<CharacterMotorScript>().Impulse = base.transform.TransformDirection(new Vector3(0f, 4f, 7f));
			}
			base.GetComponent<Animation>()["Jump-Short-Start"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()["Jump-Short-Start"].speed = 1f;
			base.GetComponent<Animation>().CrossFade("Jump-Short-Start", 0.1f, PlayMode.StopAll);
			this.animationHandller.animState = AnimationHandler.AnimStates.JUMPING;
			this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
			WeaponHandling.additiveAimingSetup = false;
		}
		else if (this.canRoll && this.ncm.grounded && this.ncm.canJump && ((this.rollInsteadOfJump && this.jumpButton) || this.rollButton) && Time.time > this.lastUserInputTime + 3f && (this.VerticalAxis > 0f || this.VerticalAxis < 0f || this.HorizontalAxis > 0f || this.HorizontalAxis < 0f))
		{
			this.lastUserInputTime = Time.time;
			this.ncm.canJump = false;
			vector = new Vector3(this.HorizontalAxis, 0f, this.VerticalAxis);
			vector = this.cameraTransform.TransformDirection(vector);
			vector.y = 0f;
			base.transform.rotation = Quaternion.Euler(base.transform.eulerAngles.x, base.transform.eulerAngles.y + Quaternion.FromToRotation(base.transform.forward, vector).eulerAngles.y, base.transform.eulerAngles.z);
			base.transform.GetComponent<CharacterMotorScript>().Impulse = base.transform.TransformDirection(new Vector3(0f, 3f, 10f));
			string text4 = "Agility-Roll-Run";
			if (AnimationHandler.instance.stealthMode)
			{
				text4 = "Stealth-Roll";
			}
			base.GetComponent<Animation>()[text4].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[text4].speed = 1.5f;
			base.GetComponent<Animation>().CrossFade(text4, 0.3f, PlayMode.StopAll);
			this.animatingTimer = base.GetComponent<Animation>()[text4].length * 0.67f;
			base.Invoke("ImpulseDelayed", base.GetComponent<Animation>()[text4].length * 0.65f);
			this.rootMotion = false;
			this.animating = true;
			this.ncm.justRolled = true;
			this.FarisHead.clip = this.FarisRollSound;
			this.FarisHead.Play();
			this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
			WeaponHandling.additiveAimingSetup = false;
		}
		else if (this.canRoll && this.ncm.grounded && this.ncm.canJump && ((this.rollInsteadOfJump && this.jumpButton) || this.rollButton) && Time.time > this.lastUserInputTime + 3f)
		{
			this.lastUserInputTime = Time.time;
			this.ncm.canJump = false;
			vector = new Vector3(this.HorizontalAxis, 0f, this.VerticalAxis);
			vector = this.cameraTransform.TransformDirection(vector);
			vector.y = 0f;
			base.transform.rotation = Quaternion.Euler(base.transform.eulerAngles.x, base.transform.eulerAngles.y + Quaternion.FromToRotation(base.transform.forward, vector).eulerAngles.y, base.transform.eulerAngles.z);
			base.transform.GetComponent<CharacterMotorScript>().Impulse = base.transform.TransformDirection(new Vector3(0f, 3f, 10f));
			string text5 = "Agility-Roll";
			if (AnimationHandler.instance.stealthMode)
			{
				text5 = "Stealth-Roll";
			}
			base.GetComponent<Animation>()[text5].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[text5].speed = 1.5f;
			base.GetComponent<Animation>().CrossFade(text5, 0.3f, PlayMode.StopAll);
			this.animatingTimer = base.GetComponent<Animation>()[text5].length * 0.67f;
			this.rootMotion = false;
			this.animating = true;
			this.ncm.justRolled = true;
			this.FarisHead.clip = this.FarisRollSound;
			this.FarisHead.Play();
			this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.AGILITY;
			WeaponHandling.additiveAimingSetup = false;
		}
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00053444 File Offset: 0x00051644
	private void ImpulseDelayed()
	{
		if (this.VerticalAxis > 0f || this.VerticalAxis < 0f || this.HorizontalAxis > 0f || this.HorizontalAxis < 0f)
		{
			base.transform.GetComponent<CharacterMotorScript>().Impulse = base.transform.TransformDirection(new Vector3(0f, 0f, 15f));
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x000534C0 File Offset: 0x000516C0
	private void JumpInPlace()
	{
		this.lastUserInputTime = Time.time;
		base.GetComponent<Animation>()["General-Jump-Inplace"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["General-Jump-Inplace"].speed = 1.5f;
		base.GetComponent<Animation>().CrossFade("General-Jump-Inplace", 0.1f, PlayMode.StopAll);
		this.animatingTimer = base.GetComponent<Animation>()["General-Jump-Inplace"].length / 1.5f;
		this.rootMotion = false;
		this.animating = true;
		this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x00053564 File Offset: 0x00051764
	private void HandleLedgeHanging()
	{
		float num = 0f;
		float num2 = Vector3.Dot(this.cameraTransform.right, base.transform.right) * this.HorizontalAxis + Vector3.Dot(this.cameraTransform.forward, base.transform.right) * this.VerticalAxis;
		float num3 = Vector3.Dot(this.cameraTransform.right, -base.transform.right) * this.HorizontalAxis + Vector3.Dot(this.cameraTransform.forward, -base.transform.right) * this.VerticalAxis;
		float num4 = Vector3.Dot(this.cameraTransform.right, -base.transform.forward) * this.HorizontalAxis + Vector3.Dot(this.cameraTransform.forward, -base.transform.forward) * this.VerticalAxis;
		if (this.ledge != null)
		{
			num = this.ledge.leftPoint.InverseTransformPoint(base.transform.position).x / Vector3.Distance(this.ledge.leftPoint.position, this.ledge.rightPoint.position) * 100f;
		}
		string text = "Agility-Ledge1-Idle";
		string text2 = "Agility-Ledge1-Climb";
		string text3 = "Agility-Ledge1-Left";
		string text4 = "Agility-Ledge1-Right";
		string text5 = "Agility-Ledge1-Corner1-Left";
		string text6 = "Agility-Ledge1-Corner1-Right";
		string text7 = "Agility-Ledge1-Corner2-Left";
		string text8 = "Agility-Ledge1-Corner2-Right";
		if (this.ledge != null)
		{
			AgilityLedge.LedgeTypes ledgeType = this.ledge.ledgeType;
			if (ledgeType != AgilityLedge.LedgeTypes.LEDGE1)
			{
				if (ledgeType == AgilityLedge.LedgeTypes.LEDGE2)
				{
					text = "Agility-Ledge2-Idle";
					text2 = "Agility-Ledge2-Climb";
					text3 = "Agility-Ledge2-Left";
					text4 = "Agility-Ledge2-Right";
					text5 = "Agility-Ledge2-Corner1-Left";
					text6 = "Agility-Ledge2-Corner1-Right";
					text7 = "Agility-Ledge2-Corner2-Left";
					text8 = "Agility-Ledge2-Corner2-Right";
				}
			}
			else
			{
				text = "Agility-Ledge1-Idle";
				text2 = "Agility-Ledge1-Climb";
				text3 = "Agility-Ledge1-Left";
				text4 = "Agility-Ledge1-Right";
				text5 = "Agility-Ledge1-Corner1-Left";
				text6 = "Agility-Ledge1-Corner1-Right";
				text7 = "Agility-Ledge1-Corner2-Left";
				text8 = "Agility-Ledge1-Corner2-Right";
			}
		}
		else
		{
			switch (this.agilityType)
			{
			case AgilityTrigger.AgilityTypes.MEDIUM_JUMP:
				text = "Agility-Ledge1-Idle";
				text2 = "Agility-Ledge1-Climb";
				break;
			case AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE1:
				text = "Agility-Ledge1-Idle";
				text2 = "Agility-Ledge1-Climb";
				break;
			case AgilityTrigger.AgilityTypes.LONG_JUMP_LEDGE2:
				text = "Agility-Ledge2-Idle";
				text2 = "Agility-Ledge2-Climb";
				break;
			case AgilityTrigger.AgilityTypes.JUMP_LEDGE1:
				text = "Agility-Ledge1-Idle";
				text2 = "Agility-Ledge1-Climb";
				break;
			}
		}
		if (this.jumpButton && (this.ledge == null || (this.ledge.canClimb && (this.ledge.backLedge == null || num4 == 0f))))
		{
			this.animatingTimer = base.GetComponent<Animation>()[text2].length;
			this.rootMotion = true;
			this.animating = true;
			base.GetComponent<Animation>()[text2].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>()[text2].layer = 5;
			base.GetComponent<Animation>()[text2].speed = 1f;
			base.GetComponent<Animation>().CrossFade(text2, 0.01f, PlayMode.StopAll);
			this.FarisHead.PlayOneShot(this.bodyLedgeClimbSound, SpeechManager.sfxVolume);
			this.ledgeHanging = false;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			Instructions.instruction = Instructions.Instruction.NONE;
			Instructions.basicAgilityJumpButton = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
			MobileInput.instance.disableButton("climb", base.gameObject);
			this.ledge = null;
		}
		else if (this.coverButton && (this.ledge == null || this.ledge.canDrop))
		{
			if (this.ledge != null && num > this.ledge.lowerLedgeStartPercentage && num < this.ledge.lowerLedgeEndPercentage && this.ledge.lowerLedge != null)
			{
				base.GetComponent<Animation>()["Agility-Ledge1-Jump-Down"].layer = 7;
				base.GetComponent<Animation>()["Agility-Ledge1-Jump-Down"].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Jump-Down"))
				{
					base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Jump-Down", 0.1f);
					this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
				}
				this.ledge = this.ledge.lowerLedge;
				this.ledgeHanging = false;
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				Instructions.instruction = Instructions.Instruction.NONE;
				Instructions.basicAgilityJumpButton = false;
				MobileInput.instance.disableButton("interaction", base.gameObject);
				MobileInput.instance.disableButton("climb", base.gameObject);
				this.longJumpTimer = base.GetComponent<Animation>()["Agility-Ledge1-Jump-Down"].length;
				this.disableAdjustment = true;
				this.ncm.enabled = false;
				this.rootMotion = true;
				this.longJumping = true;
				this.animating = true;
			}
			else
			{
				this.rootMotion = false;
				this.Physicalize();
				this.animating = false;
				this.ledgeHanging = false;
				Instructions.instruction = Instructions.Instruction.NONE;
				Instructions.basicAgilityJumpButton = false;
				MobileInput.instance.disableButton("interaction", base.gameObject);
				MobileInput.instance.disableButton("climb", base.gameObject);
				this.ledge = null;
			}
			this.lastUserInputTime = Time.time;
		}
		else if (this.ledge != null && this.ledge.upperLedge != null && num > this.ledge.upperLedgeStartPercentage && num < this.ledge.upperLedgeEndPercentage && this.VerticalAxis > 0f)
		{
			base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Up"].layer = 7;
			base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Up"].wrapMode = WrapMode.ClampForever;
			if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Anticipation-Up"))
			{
				base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Anticipation-Up", 0.1f);
				Instructions.basicAgilityJumpButton = true;
				this.FarisHead.PlayOneShot(this.bodyVaultSound, SpeechManager.sfxVolume);
			}
			if (this.jumpButton)
			{
				base.GetComponent<Animation>()["Agility-Ledge1-Jump-Up"].layer = 7;
				base.GetComponent<Animation>()["Agility-Ledge1-Jump-Up"].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Jump-Up"))
				{
					base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Jump-Up", 0.1f);
					Instructions.instruction = Instructions.Instruction.NONE;
					this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
				}
				this.ledge = this.ledge.upperLedge;
				this.ledgeHanging = false;
				Instructions.instruction = Instructions.Instruction.NONE;
				Instructions.basicAgilityJumpButton = false;
				MobileInput.instance.disableButton("interaction", base.gameObject);
				MobileInput.instance.disableButton("climb", base.gameObject);
				this.longJumpTimer = base.GetComponent<Animation>()["Agility-Ledge1-Jump-Up"].length;
				this.disableAdjustment = true;
				this.ncm.enabled = false;
				this.rootMotion = true;
				this.longJumping = true;
				this.animating = true;
			}
		}
		else if (this.ledge != null && this.ledge.backLedge != null && num > this.ledge.backLedgeStartPercentage && num < this.ledge.backLedgeEndPercentage && num4 > num2 && num4 > num3)
		{
			base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Back"].layer = 7;
			base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Back"].wrapMode = WrapMode.ClampForever;
			if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Anticipation-Back"))
			{
				base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Anticipation-Back", 0.1f);
				Instructions.basicAgilityJumpButton = true;
				this.FarisHead.PlayOneShot(this.bodyVaultSound, SpeechManager.sfxVolume);
			}
			if (this.jumpButton)
			{
				this.jumpingBackAnimation = "Agility-Ledge1-Jump-Back";
				base.GetComponent<Animation>()[this.jumpingBackAnimation].layer = 7;
				base.GetComponent<Animation>()[this.jumpingBackAnimation].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying(this.jumpingBackAnimation))
				{
					base.GetComponent<Animation>().CrossFade(this.jumpingBackAnimation, 0.1f);
					Instructions.instruction = Instructions.Instruction.NONE;
					this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
				}
				this.ledge = this.ledge.backLedge;
				this.ledgeHanging = false;
				Instructions.instruction = Instructions.Instruction.NONE;
				Instructions.basicAgilityJumpButton = false;
				MobileInput.instance.disableButton("interaction", base.gameObject);
				MobileInput.instance.disableButton("climb", base.gameObject);
				this.longJumpTimer = base.GetComponent<Animation>()[this.jumpingBackAnimation].length;
				this.disableAdjustment = false;
				this.longJumpEndPosition = this.ledge.transform.Find("LeftPoint");
				Transform transform = this.ledge.transform.Find("RightPoint");
				this.longJumpEndPositionPosition = this.longJumpEndPosition.TransformPoint(Vector3.Distance(this.longJumpEndPosition.transform.position, transform.transform.position) / 2f, -1.9f, -2.15f);
				this.ncm.enabled = false;
				this.rootMotion = true;
				this.jumpingBackwords = true;
				this.longJumping = true;
				this.animating = true;
			}
		}
		else if (this.ledge != null && this.ledge.canMove && num2 > num3)
		{
			if (this.ledge.rightPoint.InverseTransformPoint(base.transform.position).x < -0.3f)
			{
				base.GetComponent<Animation>()[text4].layer = 7;
				base.GetComponent<Animation>()[text4].wrapMode = WrapMode.Loop;
				if (!base.GetComponent<Animation>().IsPlaying(text4))
				{
					base.GetComponent<Animation>().CrossFade(text4, 0.1f);
				}
				base.transform.Translate(1f * Time.deltaTime * base.GetComponent<Animation>()[text4].length, 0f, 0f);
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds(text4);
			}
			else if (this.ledge != null && this.ledge.rightLedge != null && this.ledge.isRightCorner)
			{
				string text9;
				if (this.ledge.isRightCorner2)
				{
					text9 = text8;
					this.turningLeftCorner = true;
					this.corner2 = true;
				}
				else
				{
					text9 = text6;
					this.turningRightCorner = true;
				}
				base.GetComponent<Animation>()[text9].layer = 7;
				base.GetComponent<Animation>()[text9].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying(text9))
				{
					base.GetComponent<Animation>().CrossFade(text9, 0.1f);
				}
				this.FarisHead.PlayOneShot(this.bodyLedgeCornerSound, SpeechManager.sfxVolume);
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds(text9);
				this.ledge = this.ledge.rightLedge;
				this.ledgeHanging = false;
				Instructions.instruction = Instructions.Instruction.NONE;
				Instructions.basicAgilityJumpButton = false;
				MobileInput.instance.disableButton("interaction", base.gameObject);
				MobileInput.instance.disableButton("climb", base.gameObject);
				this.longJumpTimer = base.GetComponent<Animation>()[text9].length;
				this.disableAdjustment = true;
				this.ncm.enabled = false;
				this.rootMotion = true;
				this.longJumping = true;
				this.animating = true;
			}
			else if (this.ledge != null && this.ledge.rightLedge != null)
			{
				base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Right"].layer = 7;
				base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Right"].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Anticipation-Right"))
				{
					base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Anticipation-Right", 0.1f);
					Instructions.basicAgilityJumpButton = true;
					this.FarisHead.PlayOneShot(this.bodyVaultSound, SpeechManager.sfxVolume);
				}
				if (this.jumpButton)
				{
					base.GetComponent<Animation>()["Agility-Ledge1-Jump-Right"].layer = 7;
					base.GetComponent<Animation>()["Agility-Ledge1-Jump-Right"].wrapMode = WrapMode.ClampForever;
					if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Jump-Right"))
					{
						base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Jump-Right", 0.1f);
						Instructions.instruction = Instructions.Instruction.NONE;
						this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
					}
					this.ledge = this.ledge.rightLedge;
					this.ledgeHanging = false;
					Instructions.instruction = Instructions.Instruction.NONE;
					Instructions.basicAgilityJumpButton = false;
					MobileInput.instance.disableButton("interaction", base.gameObject);
					MobileInput.instance.disableButton("climb", base.gameObject);
					this.longJumpTimer = base.GetComponent<Animation>()["Agility-Ledge1-Jump-Right"].length;
					this.disableAdjustment = true;
					this.ncm.enabled = false;
					this.rootMotion = true;
					this.longJumping = true;
					this.animating = true;
				}
			}
			else if (!base.GetComponent<Animation>().IsPlaying(text))
			{
				base.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>()[text].layer = 7;
				base.GetComponent<Animation>()[text].speed = 1f;
				base.GetComponent<Animation>().CrossFade(text, 0.3f, PlayMode.StopAll);
			}
		}
		else if (this.ledge != null && this.ledge.canMove && num3 > num2)
		{
			if (this.ledge.leftPoint.InverseTransformPoint(base.transform.position).x > 0.405f)
			{
				base.GetComponent<Animation>()[text3].layer = 7;
				base.GetComponent<Animation>()[text3].wrapMode = WrapMode.Loop;
				if (!base.GetComponent<Animation>().IsPlaying(text3))
				{
					base.GetComponent<Animation>().CrossFade(text3, 0.1f);
				}
				base.transform.Translate(-1f * Time.deltaTime * base.GetComponent<Animation>()[text3].length, 0f, 0f);
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds(text3);
			}
			else if (this.ledge != null && this.ledge.leftLedge != null && this.ledge.isLeftCorner)
			{
				string text10;
				if (this.ledge.isLeftCorner2)
				{
					text10 = text7;
					this.turningRightCorner = true;
					this.corner2 = true;
				}
				else
				{
					text10 = text5;
					this.turningLeftCorner = true;
				}
				base.GetComponent<Animation>()[text10].layer = 7;
				base.GetComponent<Animation>()[text10].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying(text10))
				{
					base.GetComponent<Animation>().CrossFade(text10, 0.1f);
				}
				this.FarisHead.PlayOneShot(this.bodyLedgeCornerSound, SpeechManager.sfxVolume);
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds(text10);
				this.ledge = this.ledge.leftLedge;
				this.ledgeHanging = false;
				Instructions.instruction = Instructions.Instruction.NONE;
				Instructions.basicAgilityJumpButton = false;
				MobileInput.instance.disableButton("interaction", base.gameObject);
				MobileInput.instance.disableButton("climb", base.gameObject);
				this.longJumpTimer = base.GetComponent<Animation>()[text10].length;
				this.disableAdjustment = true;
				this.ncm.enabled = false;
				this.rootMotion = true;
				this.longJumping = true;
				this.animating = true;
			}
			else if (this.ledge != null && this.ledge.leftLedge != null)
			{
				base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Left"].layer = 7;
				base.GetComponent<Animation>()["Agility-Ledge1-Anticipation-Left"].wrapMode = WrapMode.ClampForever;
				if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Anticipation-Left"))
				{
					base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Anticipation-Left", 0.1f);
					Instructions.basicAgilityJumpButton = true;
					this.FarisHead.PlayOneShot(this.bodyVaultSound, SpeechManager.sfxVolume);
				}
				if (this.jumpButton)
				{
					base.GetComponent<Animation>()["Agility-Ledge1-Jump-Left"].layer = 7;
					base.GetComponent<Animation>()["Agility-Ledge1-Jump-Left"].wrapMode = WrapMode.ClampForever;
					if (!base.GetComponent<Animation>().IsPlaying("Agility-Ledge1-Jump-Left"))
					{
						base.GetComponent<Animation>().CrossFade("Agility-Ledge1-Jump-Left", 0.1f);
						Instructions.instruction = Instructions.Instruction.NONE;
						this.FarisHead.PlayOneShot(this.bodyWaistLevelSound, SpeechManager.sfxVolume);
					}
					this.ledge = this.ledge.leftLedge;
					this.ledgeHanging = false;
					Instructions.instruction = Instructions.Instruction.NONE;
					Instructions.basicAgilityJumpButton = false;
					MobileInput.instance.disableButton("interaction", base.gameObject);
					MobileInput.instance.disableButton("climb", base.gameObject);
					this.longJumpTimer = base.GetComponent<Animation>()["Agility-Ledge1-Jump-Left"].length;
					if (!this.ledge.setDistances)
					{
						this.longJumpEndPosition = this.ledge.transform.Find("LeftPoint");
						Transform transform2 = this.ledge.transform.Find("RightPoint");
						this.longJumpEndPositionPosition = this.longJumpEndPosition.TransformPoint(Vector3.Distance(this.longJumpEndPosition.transform.position, transform2.transform.position) / 2f + 3f, -2.3f, -0.4f);
						this.longJumpEndPositionRotation = this.longJumpEndPosition.rotation;
						this.adjustPosition = true;
						this.adjustAnim = "Agility-Ledge1-Jump-Left";
						this.disableAdjustment = false;
					}
					else
					{
						this.disableAdjustment = true;
					}
					this.ncm.enabled = false;
					this.rootMotion = true;
					this.longJumping = true;
					this.animating = true;
				}
			}
			else if (!base.GetComponent<Animation>().IsPlaying(text))
			{
				base.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>()[text].layer = 7;
				base.GetComponent<Animation>()[text].speed = 1f;
				base.GetComponent<Animation>().CrossFade(text, 0.3f, PlayMode.StopAll);
			}
		}
		else if (!base.GetComponent<Animation>().IsPlaying(text))
		{
			base.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()[text].layer = 7;
			base.GetComponent<Animation>()[text].speed = 1f;
			base.GetComponent<Animation>().CrossFade(text, 0.3f, PlayMode.StopAll);
			if (Instructions.instruction != Instructions.Instruction.NONE)
			{
				Instructions.instruction = Instructions.Instruction.NONE;
			}
		}
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00054984 File Offset: 0x00052B84
	private void PlayLedgeMoveSounds()
	{
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00054988 File Offset: 0x00052B88
	private void TriggerInteractionHandSounds(string anim)
	{
		float num;
		for (num = Mathf.Abs(base.GetComponent<Animation>()[anim].normalizedTime); num > 1f; num -= 1f)
		{
		}
		float num2 = 0.48f;
		float num3 = 0.9f;
		if (num > num2 && num < num2 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				this.PlaInteractionHandSounds();
			}
			this.emmittingStepSound = true;
		}
		else if (num > num3 && num < num3 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				this.PlaInteractionHandSounds();
			}
			this.emmittingStepSound = true;
		}
		else
		{
			this.emmittingStepSound = false;
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00054A48 File Offset: 0x00052C48
	private void PlaInteractionHandSounds()
	{
		this.FarisHead.PlayOneShot(this.FarisInteractionHandSounds[this.currentInteractionHandSound], SpeechManager.sfxVolume);
		this.currentInteractionHandSound = (this.currentInteractionHandSound + 1) % this.FarisInteractionHandSounds.Length;
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00054A8C File Offset: 0x00052C8C
	public void DropFromLedge()
	{
		this.rootMotion = false;
		this.Physicalize();
		this.animating = false;
		this.ledgeHanging = false;
		Instructions.instruction = Instructions.Instruction.NONE;
		Instructions.basicAgilityJumpButton = false;
		MobileInput.instance.disableButton("interaction", base.gameObject);
		MobileInput.instance.disableButton("climb", base.gameObject);
		this.ledge = null;
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00054AF4 File Offset: 0x00052CF4
	private void Physicalize()
	{
		Vector3 position = base.transform.Find("Bip01/Root").transform.position;
		if (this.rootMotion)
		{
			base.GetComponent<Animation>().Stop();
			base.GetComponent<Animation>().Play("General-Idle");
			base.transform.position = position;
		}
		AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
		this.ncm.enabled = true;
		this.ncm.gravity = 10f;
		if (!AnimationHandler.instance.insuredMode)
		{
			this.ncm.canJump = true;
		}
		this.ncm.disableRotation = false;
		this.ncm.disableMovement = false;
	}

	// Token: 0x04000D31 RID: 3377
	public NormalCharacterMotor ncm;

	// Token: 0x04000D32 RID: 3378
	public Transform pelvis;

	// Token: 0x04000D33 RID: 3379
	public float raycastLength;

	// Token: 0x04000D34 RID: 3380
	public bool RopeMode;

	// Token: 0x04000D35 RID: 3381
	public Transform agilityRaycaster;

	// Token: 0x04000D36 RID: 3382
	[HideInInspector]
	public float animatingTimer;

	// Token: 0x04000D37 RID: 3383
	[HideInInspector]
	public bool animating;

	// Token: 0x04000D38 RID: 3384
	private float longJumpTimer;

	// Token: 0x04000D39 RID: 3385
	public bool longJumping;

	// Token: 0x04000D3A RID: 3386
	private bool shortJumpingFixed;

	// Token: 0x04000D3B RID: 3387
	public Transform longJumpEndPosition;

	// Token: 0x04000D3C RID: 3388
	public bool ledgeHanging;

	// Token: 0x04000D3D RID: 3389
	public AudioClip FarisJumpSound;

	// Token: 0x04000D3E RID: 3390
	public AudioClip FarisJumpshortSound;

	// Token: 0x04000D3F RID: 3391
	public AudioClip FarisRollSound;

	// Token: 0x04000D40 RID: 3392
	public AudioClip FarisVaultSound;

	// Token: 0x04000D41 RID: 3393
	public AudioClip FarisWaistLevelSound;

	// Token: 0x04000D42 RID: 3394
	public AudioClip FarisChestLevelSound;

	// Token: 0x04000D43 RID: 3395
	public AudioClip FarisJumpLedgeSound;

	// Token: 0x04000D44 RID: 3396
	public AudioClip bodyVaultSound;

	// Token: 0x04000D45 RID: 3397
	public AudioClip bodyWaistLevelSound;

	// Token: 0x04000D46 RID: 3398
	public AudioClip bodyChestLevelSound;

	// Token: 0x04000D47 RID: 3399
	public AudioClip bodyLedgeClimbSound;

	// Token: 0x04000D48 RID: 3400
	public AudioClip bodyLedgeCornerSound;

	// Token: 0x04000D49 RID: 3401
	public AudioClip ledgeJumpRightSound;

	// Token: 0x04000D4A RID: 3402
	public AudioClip ledgeJumpLeftSound;

	// Token: 0x04000D4B RID: 3403
	public AudioClip ledgeJumpUpSound;

	// Token: 0x04000D4C RID: 3404
	public AudioClip ledgeJumpDownSound;

	// Token: 0x04000D4D RID: 3405
	public AudioClip ledgeJumpBackSound;

	// Token: 0x04000D4E RID: 3406
	public AudioClip[] FarisLedgeMoveSounds;

	// Token: 0x04000D4F RID: 3407
	public AudioClip[] lookAtThatSounds;

	// Token: 0x04000D50 RID: 3408
	private int currentLedgeMoveSound;

	// Token: 0x04000D51 RID: 3409
	public AnimationClip FarisJumpAnim;

	// Token: 0x04000D52 RID: 3410
	public AudioSource FarisHead;

	// Token: 0x04000D53 RID: 3411
	public Transform head;

	// Token: 0x04000D54 RID: 3412
	public GameObject FarisObject;

	// Token: 0x04000D55 RID: 3413
	public bool canRoll = true;

	// Token: 0x04000D56 RID: 3414
	public bool insideLongJump;

	// Token: 0x04000D57 RID: 3415
	public AgilityTrigger.AgilityTypes agilityType;

	// Token: 0x04000D58 RID: 3416
	public AgilityLedge ledge;

	// Token: 0x04000D59 RID: 3417
	[HideInInspector]
	public bool rootMotion;

	// Token: 0x04000D5A RID: 3418
	public float lastUserInputTime;

	// Token: 0x04000D5B RID: 3419
	private float timeBeforeAcceptingNewUserInput = 1f;

	// Token: 0x04000D5C RID: 3420
	private AnimationHandler animationHandller;

	// Token: 0x04000D5D RID: 3421
	private bool turningLeftCorner;

	// Token: 0x04000D5E RID: 3422
	private bool turningRightCorner;

	// Token: 0x04000D5F RID: 3423
	private bool jumpingBackwords;

	// Token: 0x04000D60 RID: 3424
	private bool disableAdjustment;

	// Token: 0x04000D61 RID: 3425
	private Vector3 longJumpEndPositionPosition;

	// Token: 0x04000D62 RID: 3426
	private Quaternion longJumpEndPositionRotation;

	// Token: 0x04000D63 RID: 3427
	private Transform cameraTransform;

	// Token: 0x04000D64 RID: 3428
	private bool jumpButton;

	// Token: 0x04000D65 RID: 3429
	private bool coverButton;

	// Token: 0x04000D66 RID: 3430
	private bool rollButton;

	// Token: 0x04000D67 RID: 3431
	private bool interactionButton;

	// Token: 0x04000D68 RID: 3432
	private float HorizontalAxis;

	// Token: 0x04000D69 RID: 3433
	private float VerticalAxis;

	// Token: 0x04000D6A RID: 3434
	public AudioClip[] weaponPickupSounds;

	// Token: 0x04000D6B RID: 3435
	public AudioClip[] generalPickupSounds;

	// Token: 0x04000D6C RID: 3436
	public AudioClip[] weaponPickupSoundsArabic;

	// Token: 0x04000D6D RID: 3437
	public AudioClip[] generalPickupSoundsArabic;

	// Token: 0x04000D6E RID: 3438
	private bool emmittingStepSound;

	// Token: 0x04000D6F RID: 3439
	public AudioClip[] FarisInteractionHandSounds;

	// Token: 0x04000D70 RID: 3440
	private int currentInteractionHandSound;

	// Token: 0x04000D71 RID: 3441
	private bool corner2;

	// Token: 0x04000D72 RID: 3442
	public bool rollInsteadOfJump;

	// Token: 0x04000D73 RID: 3443
	private string jumpingBackAnimation;

	// Token: 0x04000D74 RID: 3444
	private bool adjustPosition;

	// Token: 0x04000D75 RID: 3445
	private string adjustAnim;

	// Token: 0x04000D76 RID: 3446
	public bool dontPerformAgility;

	// Token: 0x04000D77 RID: 3447
	public bool survivalCharacter;

	// Token: 0x04000D78 RID: 3448
	private float lastGroundedY = 999f;

	// Token: 0x04000D79 RID: 3449
	private LayerMask mask;

	// Token: 0x04000D7A RID: 3450
	private float lastLedgeSoundPlayTime;

	// Token: 0x04000D7B RID: 3451
	private int jumpInPlaceSound;

	// Token: 0x04000D7C RID: 3452
	private int jumpForwardSound;
}
