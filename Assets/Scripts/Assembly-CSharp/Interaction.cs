using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class Interaction : MonoBehaviour
{
	// Token: 0x060009A0 RID: 2464 RVA: 0x00058954 File Offset: 0x00056B54
	private void Start()
	{
		this.ncm = base.gameObject.GetComponent<NormalCharacterMotor>();
		this.pcc = base.gameObject.GetComponent<PlatformCharacterController>();
		this.cc = base.gameObject.GetComponent<CharacterController>();
		this.basicAgility = base.gameObject.GetComponent<BasicAgility>();
		this.animHandler = base.gameObject.GetComponent<AnimationHandler>();
		this.weaponHandler = base.gameObject.GetComponent<WeaponHandling>();
		this.cam = GlobalFarisCam.farisCamera.GetComponent<ShooterGameCamera>();
		this.coverOffset = this.cam.shootingCamOffsetX;
		this.mask = 1 << base.gameObject.layer;
		this.mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.mask |= 1 << LayerMask.NameToLayer("Enemy");
		this.mask = ~this.mask;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00058A4C File Offset: 0x00056C4C
	private void Awake()
	{
		if (base.gameObject.tag == "Player")
		{
			Interaction.playerInteraction = this;
		}
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00058A7C File Offset: 0x00056C7C
	private void OnDestroy()
	{
		if (base.gameObject.tag == "Player")
		{
			Interaction.playerInteraction = null;
		}
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00058AAC File Offset: 0x00056CAC
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
		this.coverSwitchAction = MobileInput.switchCover;
		if (this.moveToCover)
		{
			this.MoveToCover();
		}
		if (this.rootMotionShortCover)
		{
			this.rootMotionTimer -= Time.deltaTime;
			if (this.rootMotionTimer <= 0f)
			{
				this.RootMotionShortCover();
				this.rootMotionShortCover = false;
			}
		}
		if (this.rootMotionLongCover)
		{
			this.rootMotionTimer -= Time.deltaTime;
			if (this.rootMotionTimer <= 0f)
			{
				this.RootMotionLongCover();
				this.rootMotionLongCover = false;
			}
		}
		if (this.rootMotionTOCoverRun)
		{
			this.rootMotionTimer -= Time.deltaTime;
			if (this.rootMotionTimer <= 0f)
			{
				this.RootMotionToCoverRun();
				this.rootMotionTOCoverRun = false;
			}
		}
		this.fire = InputManager.GetButton("Fire1");
		this.coverButton = (MobileInput.cover || InputManager.GetButtonDown("Cover"));
		this.exitInteractionButton = (MobileInput.interaction || InputManager.GetButtonDown("Cover"));
		this.jumpButton = (MobileInput.jump || InputManager.GetButtonDown("Jump"));
		this.aim = InputManager.GetButton("Fire2");
		this.HorizontalAxis = PlatformCharacterController.joystickLeft.position.x + Input.GetAxis("Horizontal");
		this.VerticalAxis = PlatformCharacterController.joystickLeft.position.y + Input.GetAxis("Vertical");
		if (this.EnterPushingMode)
		{
			this.pushingMode = true;
			this.EnterPushingMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			this.pushableObjectFirstHeight = this.pushableObject.transform.position.y;
			if (this.pushableObject.GetComponent<Rigidbody>() != null)
			{
				this.pushableObject.GetComponent<Rigidbody>().isKinematic = false;
			}
			else
			{
				this.pushableObject.AddComponent<Rigidbody>();
			}
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			base.transform.position = this.startPosition.position;
			base.transform.rotation = this.startPosition.rotation;
			base.GetComponent<Animation>()["Interaction-Push"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Push"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().CrossFade("Interaction-Push");
			this.basicAgility.FarisHead.PlayOneShot(this.farisPushSound, SpeechManager.speechVolume);
			this.basicAgility.FarisHead.PlayOneShot(this.dragSound, SpeechManager.sfxVolume);
			return;
		}
		if (this.pushingMode)
		{
			if (this.pushableObject.transform.position.y < this.pushableObjectFirstHeight - 0.1f)
			{
				this.pushingMode = false;
				this.ncm.enabled = true;
				if (!AnimationHandler.instance.insuredMode)
				{
					this.ncm.canJump = true;
				}
				this.animHandler.isInInteraction = false;
				base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				base.GetComponent<Animation>().CrossFade("General-Idle", 0.3f, PlayMode.StopAll);
				if (this.weaponHandler != null)
				{
					WeaponHandling.additiveAimingSetup = false;
				}
			}
			else
			{
				base.transform.Translate(0f, 0f, 0.023f);
				this.pushableObject.transform.Translate(0f, 0f, 0.02f);
			}
		}
		if (this.EnterKickingMode)
		{
			this.kickingMode = true;
			this.EnterKickingMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			if (this.pushableObject.tag == "Door")
			{
				base.transform.position = this.startPosition.position;
				base.transform.rotation = this.startPosition.rotation;
			}
			else
			{
				Vector3 position = this.pushableObject.transform.position;
				position.y = base.transform.position.y;
				base.transform.LookAt(position);
			}
			if (this.pushableObject.tag == "Door")
			{
				this.kickAnim = "Interaction-Kick-Door";
			}
			else
			{
				this.kickAnim = "Interaction-Kick";
			}
			base.GetComponent<Animation>()[this.kickAnim].layer = 1;
			base.GetComponent<Animation>()[this.kickAnim].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().CrossFade(this.kickAnim);
			this.rigidbodyAdded = false;
			return;
		}
		if (this.kickingMode)
		{
			if (base.GetComponent<Animation>()[this.kickAnim].normalizedTime > 0.2f)
			{
				if (AnimationHandler.instance.dustParticles != null && Time.time > this.lastDust + 1f)
				{
					UnityEngine.Object.Instantiate(AnimationHandler.instance.dustParticles, this.pushableObject.transform.position, Quaternion.identity);
					this.lastDust = Time.time;
				}
				if (!this.rigidbodyAdded)
				{
					foreach (object obj in this.pushableObject.transform)
					{
						Transform transform = (Transform)obj;
						transform.gameObject.layer = LayerMask.NameToLayer("Grenade");
						Rigidbody rigidbody = null;
						if (transform.gameObject.GetComponent<Rigidbody>() == null)
						{
							rigidbody = transform.gameObject.AddComponent<Rigidbody>();
						}
						transform.gameObject.GetComponent<Rigidbody>().drag = 3f;
						transform.gameObject.GetComponent<Rigidbody>().mass = 2f;
						if (this.pushableObject.tag == "Door" && rigidbody != null)
						{
							rigidbody.AddForce(this.pushableObject.transform.InverseTransformDirection(base.transform.TransformDirection(0f, 0f, UnityEngine.Random.Range(150f, 180f))));
						}
					}
					this.rigidbodyAdded = true;
				}
				if (!this.breakingSoundPlayed)
				{
					this.basicAgility.FarisHead.PlayOneShot(this.breakSound, 0.2f * SpeechManager.sfxVolume);
					this.breakingSoundPlayed = true;
				}
			}
			if (base.GetComponent<Animation>()[this.kickAnim].normalizedTime > 1f)
			{
				this.kickingMode = false;
				this.ncm.enabled = true;
				if (!AnimationHandler.instance.insuredMode)
				{
					this.ncm.canJump = true;
				}
				this.animHandler.isInInteraction = false;
				base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
				this.breakingSoundPlayed = false;
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				base.GetComponent<Animation>().CrossFade("General-Idle", 0.3f, PlayMode.StopAll);
				if (this.weaponHandler != null)
				{
					WeaponHandling.additiveAimingSetup = false;
				}
			}
		}
		if (this.EnterWallBackMode)
		{
			this.wallBackMode = true;
			this.EnterWallBackMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			Transform transform2 = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
			transform2.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
			base.transform.position = transform2.position;
			base.transform.rotation = transform2.rotation;
			UnityEngine.Object.Destroy(transform2.gameObject);
			base.GetComponent<Animation>()["Interaction-Wall-Back-Enter"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Enter"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().CrossFade("Interaction-Wall-Back-Enter");
			iTween.RotateBy(base.gameObject, new Vector3(0f, -0.5f, 0f), base.GetComponent<Animation>()["Interaction-Wall-Back-Enter"].length);
			base.GetComponent<Animation>()["Interaction-Wall-Back-Idle"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFadeQueued("Interaction-Wall-Back-Idle");
			return;
		}
		if (this.EnterCoverTallMode)
		{
			this.coverTallMode = true;
			this.EnterCoverTallMode = false;
			this.ncm.disableMovement = true;
			this.ncm.disableRotation = true;
			this.ncm.canJump = false;
			this.animHandler.isInInteraction = true;
			this.inversedAimingBeforeEnteringCover = this.cam.inversedAiming;
			WeaponHandling.holdFire = true;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.COVER;
			if (this.path == string.Empty)
			{
				Transform transform3 = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
				transform3.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
				base.transform.position = transform3.position;
				base.transform.rotation = transform3.rotation;
				this.playerYRotation = base.transform.eulerAngles.y;
			}
			else
			{
				float num = 100000f;
				for (float num2 = 0f; num2 <= 1f; num2 += 0.1f)
				{
					Vector3 b = iTween.PointOnPath(iTweenPath.GetPath(this.path), num2);
					b.y = base.transform.position.y;
					if (Vector3.Distance(base.transform.position, b) < num)
					{
						num = Vector3.Distance(base.transform.position, b);
						this.closestPointPercentage = num2;
					}
				}
				Vector3 position2 = iTween.PointOnPath(iTweenPath.GetPath(this.path), this.closestPointPercentage);
				position2.y = base.transform.position.y;
				base.transform.position = position2;
				this.lookAtPoint.y = base.transform.position.y;
				base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.lookAtPoint);
				this.playerYRotation = base.transform.eulerAngles.y;
				MobileInput.instance.enableButton("cover", base.gameObject);
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyCoverEnterSound, SpeechManager.sfxVolume);
			if (this.weaponHandler != null)
			{
				this.weaponHandler.inCover = true;
				this.weaponHandler.tallCover = true;
				if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
				{
					this.weaponHandler.EnterIdleFree();
				}
				WeaponHandling.additiveAimingSetup = false;
			}
			return;
		}
		if (this.EnterCoverShortMode)
		{
			this.coverShortMode = true;
			this.EnterCoverShortMode = false;
			this.ncm.disableMovement = true;
			this.ncm.disableRotation = true;
			this.ncm.canJump = false;
			this.animHandler.isInInteraction = true;
			WeaponHandling.holdFire = true;
			this.cam.shortCoverY = 0.6f;
			this.inversedAimingBeforeEnteringCover = this.cam.inversedAiming;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.COVER;
			if (this.path == string.Empty)
			{
				Transform transform4 = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
				transform4.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
				base.transform.position = transform4.position;
				base.transform.rotation = transform4.rotation;
				UnityEngine.Object.Destroy(transform4.gameObject);
			}
			else
			{
				float num3 = 100000f;
				for (float num4 = 0f; num4 <= 1f; num4 += 0.1f)
				{
					Vector3 b2 = iTween.PointOnPath(iTweenPath.GetPath(this.path), num4);
					b2.y = base.transform.position.y;
					if (Vector3.Distance(base.transform.position, b2) < num3)
					{
						num3 = Vector3.Distance(base.transform.position, b2);
						this.closestPointPercentage = num4;
					}
				}
				Vector3 position3 = iTween.PointOnPath(iTweenPath.GetPath(this.path), this.closestPointPercentage);
				position3.y = base.transform.position.y;
				base.transform.position = position3;
				this.lookAtPoint.y = base.transform.position.y;
				base.transform.rotation = Quaternion.LookRotation(this.lookAtPoint - base.transform.position);
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyCoverEnterSound, SpeechManager.sfxVolume);
			base.GetComponent<Animation>()["Cover-Short-Enter-Right"].layer = 1;
			base.GetComponent<Animation>()["Cover-Short-Enter-Right"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().CrossFade("Cover-Short-Enter-Right");
			base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
			base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFadeQueued("Cover-Short-Idle-Right");
			Vector3 position4 = base.transform.position;
			position4.y += 1f;
			RaycastHit raycastHit;
			if (Physics.Raycast(position4, -base.transform.up, out raycastHit, 2f, this.mask))
			{
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - (raycastHit.distance - 1f), base.transform.position.z);
			}
			if (this.weaponHandler != null)
			{
				this.weaponHandler.inCover = true;
				this.weaponHandler.tallCover = false;
				if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
				{
					this.weaponHandler.EnterIdleFree();
				}
				WeaponHandling.additiveAimingSetup = false;
			}
			return;
		}
		if (this.EnterPoleMode)
		{
			this.poleClimbMode = true;
			this.EnterPoleMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			Instructions.instruction = Instructions.Instruction.DROP;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			if (Vector3.Distance(base.transform.position, this.endPosition.position) < Vector3.Distance(base.transform.position, this.startPosition.position))
			{
				Transform transform5 = this.startPosition;
				this.startPosition = this.endPosition;
				this.endPosition = transform5;
			}
			Transform transform6 = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
			transform6.Translate(0f, 0f, this.startPosition.InverseTransformPoint(base.transform.position).z * 7.9f * this.startPosition.parent.localScale.x);
			base.transform.position = new Vector3(transform6.position.x, base.transform.position.y, transform6.position.z);
			base.transform.rotation = transform6.rotation;
			this.ClimbPoleOverTime = base.GetComponent<Animation>()["Interaction-Pole-Climb"].length;
			this.targetPoleY = transform6.position.y;
			base.GetComponent<Animation>()["Interaction-Pole-Climb"].layer = 100;
			base.GetComponent<Animation>()["Interaction-Pole-Climb"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Interaction-Pole-Climb");
			this.basicAgility.FarisHead.PlayOneShot(this.bodyPoleClimbSound, SpeechManager.sfxVolume);
			base.GetComponent<Animation>()["Interaction-Pole-Idle"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Pole-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFadeQueued("Interaction-Pole-Idle");
			this.basicAgility.FarisHead.PlayOneShot(this.farisEnterPoleSound, SpeechManager.speechVolume);
			return;
		}
		if (this.ClimbPoleOverTime > 0f)
		{
			if (base.GetComponent<Animation>()["Interaction-Pole-Climb"].normalizedTime >= 0.3f && base.GetComponent<Animation>()["Interaction-Pole-Climb"].normalizedTime <= 0.7f)
			{
				base.transform.position = new Vector3(base.transform.position.x, Mathf.SmoothDamp(base.transform.position.y, this.targetPoleY, ref this.REFY, this.ClimbPoleOverTime - 0.3f * base.GetComponent<Animation>()["Interaction-Pole-Climb"].length), base.transform.position.z);
			}
			this.ClimbPoleOverTime -= Time.deltaTime;
		}
		if (this.EnterLogMode)
		{
			this.logMode = true;
			this.EnterLogMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			this.falledFromLog = false;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			if (Vector3.Distance(base.transform.position, this.endPosition.position) < Vector3.Distance(base.transform.position, this.startPosition.position))
			{
				Transform transform7 = this.startPosition;
				this.startPosition = this.endPosition;
				this.endPosition = transform7;
			}
			base.transform.position = this.startPosition.position;
			base.transform.rotation = this.startPosition.rotation;
			base.GetComponent<Animation>()["Interaction-Log-Idle"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Log-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Log-Idle");
			this.lastFallAtemptTime = Time.time;
			if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.FarisLogEnterSoundArabic != null)
			{
				this.basicAgility.FarisHead.PlayOneShot(this.FarisLogEnterSoundArabic, SpeechManager.speechVolume);
			}
			else
			{
				this.basicAgility.FarisHead.PlayOneShot(this.FarisLogEnterSound, SpeechManager.speechVolume);
			}
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				this.fallingRightDirection = true;
			}
			else
			{
				this.fallingRightDirection = false;
			}
			return;
		}
		if (this.EnterRopeMode)
		{
			this.EnterRopeMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			Instructions.instruction = Instructions.Instruction.DROP;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			if (Vector3.Distance(base.transform.position, this.endPosition.position) < Vector3.Distance(base.transform.position, this.startPosition.position))
			{
				Transform transform8 = this.startPosition;
				this.startPosition = this.endPosition;
				this.endPosition = transform8;
				this.movingDown = true;
			}
			else
			{
				this.movingDown = false;
			}
			if (!this.movingDown)
			{
				base.transform.position = this.startPosition.position;
				base.transform.rotation = this.startPosition.rotation;
				base.GetComponent<Animation>()["Interaction-Rope-Start"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Rope-Start"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Interaction-Rope-Start");
				this.ropeEnterTimer = base.GetComponent<Animation>()["Interaction-Rope-Start"].length;
				this.ropeClimbMode = true;
			}
			else
			{
				base.transform.position = this.topStartPosition.position;
				base.transform.rotation = this.topStartPosition.rotation;
				base.GetComponent<Animation>()["Interaction-Rope-Enter-Top"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Rope-Enter-Top"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Interaction-Rope-Enter-Top");
				this.ropeEnterTimer = base.GetComponent<Animation>()["Interaction-Rope-Enter-Top"].length;
				this.animatingTimer = base.GetComponent<Animation>()["Interaction-Rope-Enter-Top"].length;
				this.animating = true;
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyRopeClimbSound, SpeechManager.sfxVolume);
			return;
		}
		if (this.EnterLadderMode)
		{
			this.EnterLadderMode = false;
			this.ncm.canJump = false;
			this.ncm.enabled = false;
			this.animHandler.isInInteraction = true;
			if (this.currentInteractionTrigger.canDrop)
			{
				Instructions.instruction = Instructions.Instruction.DROP;
			}
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.INTERACTION;
			if (Vector3.Distance(base.transform.position, this.endPosition.position) < Vector3.Distance(base.transform.position, this.startPosition.position))
			{
				Transform transform9 = this.startPosition;
				this.startPosition = this.endPosition;
				this.endPosition = transform9;
				this.movingDown = true;
			}
			else
			{
				this.movingDown = false;
			}
			if (!this.movingDown)
			{
				base.transform.position = this.startPosition.position;
				base.transform.rotation = this.startPosition.rotation;
				base.GetComponent<Animation>()["Interaction-Ladder-Start"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Ladder-Start"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Start");
				this.ladderEnterTimer = base.GetComponent<Animation>()["Interaction-Ladder-Start"].length;
				this.ladderClimbMode = true;
			}
			else
			{
				base.transform.position = this.topStartPosition.position;
				base.transform.rotation = this.topStartPosition.rotation;
				base.GetComponent<Animation>()["Interaction-Ladder-Start-Top"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Ladder-Start-Top"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Start-Top");
				this.ladderEnterTimer = base.GetComponent<Animation>()["Interaction-Ladder-Start-Top"].length;
				this.animatingTimer = base.GetComponent<Animation>()["Interaction-Ladder-Start-Top"].length;
				this.animating = true;
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyLadderClimbSound, SpeechManager.sfxVolume);
			return;
		}
		if (this.animating)
		{
			this.animatingTimer -= Time.deltaTime;
			if (this.animatingTimer <= 0f)
			{
				Vector3 position5 = GameObject.Find(base.transform.root.name + "/Bip01/Root").transform.position;
				base.GetComponent<Animation>().Stop();
				InteractionTrigger.InteractionTypes interactionTypes = this.interactionType;
				if (interactionTypes != InteractionTrigger.InteractionTypes.ROPE)
				{
					if (interactionTypes == InteractionTrigger.InteractionTypes.LADDER)
					{
						this.ladderClimbMode = true;
						this.HandleLadderClimbMode();
					}
				}
				else
				{
					this.ropeClimbMode = true;
					this.HandleRopeClimbMode();
				}
				base.transform.position = position5;
				base.transform.Rotate(0f, 180f, 0f);
				this.animating = false;
			}
		}
		if (this.wallBackMode)
		{
			this.HandleWallBackMode();
		}
		if (this.coverTallMode)
		{
			this.HandleCoverTallMode();
		}
		if (this.coverShortMode)
		{
			this.HandleCoverShortMode();
		}
		if (this.poleClimbMode)
		{
			this.HandlePoleClimbMode();
		}
		if (this.logMode)
		{
			this.HandleLogMode();
		}
		if (this.ropeClimbMode)
		{
			if (this.ropeEnterTimer <= 0f)
			{
				this.HandleRopeClimbMode();
			}
			else
			{
				this.ropeEnterTimer -= Time.deltaTime;
			}
		}
		if (this.ladderClimbMode)
		{
			if (this.ladderEnterTimer <= 0f)
			{
				this.HandleLadderClimbMode();
			}
			else
			{
				this.ladderEnterTimer -= Time.deltaTime;
			}
		}
		if (this.endingInteraction)
		{
			if (this.endingInteractionTimer <= 0f)
			{
				Vector3 position6 = GameObject.Find(base.transform.root.name + "/Bip01/Root").transform.position;
				base.GetComponent<Animation>().Stop();
				base.GetComponent<Animation>().Play("General-Idle");
				base.transform.position = position6;
				this.animHandler.isInInteraction = false;
				base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				this.ncm.enabled = true;
				if (!AnimationHandler.instance.insuredMode)
				{
					this.ncm.canJump = true;
				}
				this.endingInteraction = false;
			}
			else
			{
				this.endingInteractionTimer -= Time.deltaTime;
			}
		}
		this.coverSwitchAction = false;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x0005A6FC File Offset: 0x000588FC
	private void HandleWallBackMode()
	{
		float num = Vector3.Dot(Camera.main.transform.right, base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.right) * this.VerticalAxis;
		float num2 = Vector3.Dot(Camera.main.transform.right, -base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, -base.transform.right) * this.VerticalAxis;
		if (this.exitInteractionButton)
		{
			if (Time.time <= this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
			{
				return;
			}
			this.wallBackMode = false;
			this.ncm.enabled = true;
			if (!AnimationHandler.instance.insuredMode)
			{
				this.ncm.canJump = true;
			}
			this.animHandler.isInInteraction = false;
			base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Exit"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Exit"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Interaction-Wall-Back-Exit");
			this.lastUserInputTime = Time.time;
			this.basicAgility.lastUserInputTime = Time.time;
		}
		else if (num2 > num && this.endPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0f)
		{
			base.GetComponent<Animation>()["Interaction-Wall-Back-Move-Left"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Move-Left"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Wall-Back-Move-Left");
			base.transform.Translate(-1.5f * Time.deltaTime * base.GetComponent<Animation>()["Interaction-Wall-Back-Move-Left"].length, 0f, 0f);
			this.TriggerSteepsSound("Interaction-Wall-Back-Move-Left");
		}
		else if (num > num2 && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x > 0f)
		{
			base.GetComponent<Animation>()["Interaction-Wall-Back-Move-Right"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Move-Right"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Wall-Back-Move-Right");
			base.transform.Translate(1.5f * Time.deltaTime * base.GetComponent<Animation>()["Interaction-Wall-Back-Move-Right"].length, 0f, 0f);
			this.TriggerSteepsSound("Interaction-Wall-Back-Move-Right");
		}
		else
		{
			base.GetComponent<Animation>()["Interaction-Wall-Back-Idle"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Wall-Back-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Wall-Back-Idle");
		}
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0005AA7C File Offset: 0x00058C7C
	private void HandleCoverTallMode()
	{
		float num = Vector3.Dot(Camera.main.transform.right, base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.right) * this.VerticalAxis;
		float num2 = Vector3.Dot(Camera.main.transform.right, -base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, -base.transform.right) * this.VerticalAxis;
		if (this.startPosition != null && this.path == string.Empty && !this.aim)
		{
			if (this.startPosition.InverseTransformPoint(base.transform.position).x < 0.5f)
			{
				this.cam.coverOffset = this.coverOffset * (1f - this.startPosition.InverseTransformPoint(base.transform.position).x);
			}
			else
			{
				this.cam.coverOffset = -this.coverOffset * this.startPosition.InverseTransformPoint(base.transform.position).x;
			}
		}
		else
		{
			this.cam.coverOffset = 0f;
		}
		if (MobileInput.instance != null && this.spaceToMoveCover)
		{
			MobileInput.instance.disableButton("SwitchCover", base.gameObject);
		}
		this.spaceToCornerCover = false;
		this.spaceToMoveCover = false;
		if (this.coverButton)
		{
			if (Time.time <= this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
			{
				return;
			}
			this.ExitCoverTallMode();
			this.cam.coverOffset = 0f;
			this.cam.inversedAiming = false;
			this.weaponHandler.inversedAiming = false;
			this.lastUserInputTime = Time.time;
			this.basicAgility.lastUserInputTime = Time.time;
		}
		else if (!this.blindFire && num == 0f && num2 == 0f && this.fire && !this.aim && ((this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0.07f) || (this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.07f)) && Vector3.Angle(this.cam.transform.forward, base.transform.forward) > 90f && ((!this.movingRightDirection && Vector3.Cross(this.cam.transform.forward, base.transform.forward).y > 0f) || (this.movingRightDirection && Vector3.Cross(this.cam.transform.forward, base.transform.forward).y < 0f) || Vector3.Angle(this.cam.transform.forward, base.transform.forward) > 160f))
		{
			string text;
			if (this.movingRightDirection && this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.07f)
			{
				base.transform.position = this.endPosition.position;
				text = "Cover-Tall-Handgun-Blindfire-Left-Enter";
			}
			else
			{
				if (this.movingRightDirection || !(this.startPosition != null) || this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x >= 0.07f)
				{
					return;
				}
				base.transform.position = this.startPosition.position;
				text = "Cover-Tall-Handgun-Blindfire-Right-Enter";
			}
			base.GetComponent<Animation>()[text].layer = 1;
			base.GetComponent<Animation>()[text].speed = 1f;
			base.GetComponent<Animation>()[text].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().Play(text, PlayMode.StopAll);
			if (this.weaponHandler != null)
			{
				WeaponHandling.additiveAimingSetup = false;
			}
			this.blindFire = true;
			this.blindFireFirstShot = true;
			this.blindFireStartTime = Time.time;
			this.weaponHandler.blindFire = true;
			this.weaponHandler.gm.currentGun.lastShootTime = Time.time;
		}
		else if ((this.aim && this.blindFire) || (this.blindFire && Time.time > this.blindFireStartTime + base.GetComponent<Animation>()["Cover-Tall-Handgun-Blindfire-Right-Enter"].length && Time.time > this.weaponHandler.gm.currentGun.lastShootTime + 1f))
		{
			this.blindFire = false;
			this.weaponHandler.blindFire = false;
			this.weaponHandler.RepositionTheCurrentWeaponInHand();
			if (!this.aim)
			{
				this.weaponHandler.EnterIdleFree();
				WeaponHandling.holdFire = true;
			}
			WeaponHandling.additiveAimingSetup = false;
		}
		else if (this.blindFire && Vector3.Angle(this.cam.transform.forward, base.transform.forward) > 90f && ((!this.movingRightDirection && Vector3.Cross(this.cam.transform.forward, base.transform.forward).y > 0f) || (this.movingRightDirection && Vector3.Cross(this.cam.transform.forward, base.transform.forward).y < 0f) || Vector3.Angle(this.cam.transform.forward, base.transform.forward) > 160f))
		{
			if (Time.time > this.blindFireStartTime + base.GetComponent<Animation>()["Cover-Tall-Handgun-Blindfire-Right-Enter"].length)
			{
				if (this.blindFireFirstShot)
				{
					WeaponHandling.holdFire = false;
					this.weaponHandler.gm.currentGun.fire = true;
					this.blindFireFirstShot = false;
					string text2;
					if (!this.movingRightDirection)
					{
						text2 = "Cover-Tall-Handgun-Blindfire-Right";
					}
					else
					{
						text2 = "Cover-Tall-Handgun-Blindfire-Left";
					}
					base.GetComponent<Animation>()[text2].layer = 1;
					base.GetComponent<Animation>()[text2].speed = 1f;
					base.GetComponent<Animation>()[text2].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>().Blend(text2);
				}
				else if (this.fire)
				{
					WeaponHandling.holdFire = false;
					this.weaponHandler.gm.currentGun.fire = true;
					string text3;
					if (!this.movingRightDirection)
					{
						text3 = "Cover-Tall-Handgun-Blindfire-Right";
					}
					else
					{
						text3 = "Cover-Tall-Handgun-Blindfire-Left";
					}
					base.GetComponent<Animation>()[text3].layer = 1;
					base.GetComponent<Animation>()[text3].speed = 1f;
					base.GetComponent<Animation>()[text3].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>().Blend(text3);
				}
			}
		}
		else if (this.blindFire)
		{
			this.blindFire = false;
			this.weaponHandler.blindFire = false;
			this.weaponHandler.RepositionTheCurrentWeaponInHand();
			if (!this.aim)
			{
				this.weaponHandler.EnterIdleFree();
				WeaponHandling.holdFire = true;
			}
			WeaponHandling.additiveAimingSetup = false;
		}
		else if (!this.aim && num > num2 && num > 0.5f)
		{
			if ((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage < 1f) || (this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x < 0f))
			{
				base.GetComponent<Animation>()["Cover-Tall-Move-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Move-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Move-Left");
				this.TriggerSteepsSound("Cover-Tall-Move-Left");
				if (this.path == string.Empty)
				{
					base.transform.Translate(2f * Time.deltaTime * base.GetComponent<Animation>()["Cover-Short-Move-Left"].length, 0f, 0f);
				}
				else
				{
					this.closestPointPercentage += 0.3f * Time.deltaTime;
					if (iTweenPath.IsClosed(this.path))
					{
						while (this.closestPointPercentage > 1f)
						{
							this.closestPointPercentage -= 1f;
						}
						while (this.closestPointPercentage < 0f)
						{
							this.closestPointPercentage += 1f;
						}
					}
					if (this.closestPointPercentage > 0f && this.closestPointPercentage < 1f)
					{
						iTween.PutOnPath(base.gameObject, iTweenPath.GetPath(this.path), this.closestPointPercentage);
					}
					this.lookAtPoint.y = base.transform.position.y;
					base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.lookAtPoint);
				}
				if (!this.movingRightDirection)
				{
					this.movingRightDirection = true;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
			}
			else if (this.path == string.Empty && this.cover.leftCover != null)
			{
				base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Left");
				this.movingRightDirection = true;
				WeaponHandling.holdFire = true;
				if (MobileInput.instance != null && !this.spaceToMoveCover)
				{
					MobileInput.instance.enableButton("SwitchCover", base.gameObject);
				}
				this.spaceToMoveCover = true;
				if ((InputManager.GetButtonDown("Jump") || this.coverSwitchAction) && Time.time > this.lastSwitchTime + 1f && this.rootMotionTimer <= 0f)
				{
					this.lastSwitchTime = Time.time;
					if (!this.ncm.disableRotation)
					{
						this.weaponHandler.yRotationOffset = 0f;
						this.setCoverTallRotation();
						this.ncm.disableRotation = true;
						if (this.path == string.Empty)
						{
							this.cam.UnlimitHorizontalAngle();
						}
						this.weaponHandler.resetAiming = true;
					}
					this.coverTallMode = false;
					if (!AnimationHandler.instance.insuredMode)
					{
						this.ncm.canJump = true;
					}
					base.GetComponent<Animation>()["Cover-Run-to-Left"].layer = 2;
					base.GetComponent<Animation>()["Cover-Run-to-Left"].speed = 3f;
					base.GetComponent<Animation>()["Cover-Run-to-Left"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>().Play("Cover-Run-to-Left", PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
					if (!this.movingRightDirection)
					{
						this.movingRightDirection = true;
						if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
						{
							this.weaponHandler.EnterIdleFree();
						}
					}
					this.rootMotionTimer = base.GetComponent<Animation>()["Cover-Run-to-Left"].length / 3f;
					this.rootMotionTOCoverRun = true;
					this.movingFromTallCover = true;
					this.cover = this.cover.leftCover;
					this.startPosition = this.cover.transform.Find("StartPosition").transform;
					this.endPosition = this.cover.transform.Find("EndPosition").transform;
				}
			}
			else if (this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED && this.cover.leftPopout && this.cover.leftCornerCover == null && this.cover.leftCover == null)
			{
				base.GetComponent<Animation>()["Cover-Tall-Peak-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Peak-Left"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Peak-Left");
				if (!this.movingRightDirection)
				{
					this.movingRightDirection = true;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
				if (!this.peeking)
				{
					this.EnterInversedPeekingMode();
					this.peeking = true;
				}
			}
			else
			{
				base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Left");
				if (!this.movingRightDirection)
				{
					this.movingRightDirection = true;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
				WeaponHandling.holdFire = true;
			}
		}
		else if (!this.aim && num2 > num && num2 > 0.5f)
		{
			if ((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage > 0f) || (this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x > 0f))
			{
				base.GetComponent<Animation>()["Cover-Tall-Move-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Move-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Move-Right");
				this.TriggerSteepsSound("Cover-Tall-Move-Right");
				if (this.path == string.Empty)
				{
					base.transform.Translate(-2f * Time.deltaTime * base.GetComponent<Animation>()["Cover-Short-Move-Right"].length, 0f, 0f);
				}
				else
				{
					this.closestPointPercentage -= 0.3f * Time.deltaTime;
					if (iTweenPath.IsClosed(this.path))
					{
						while (this.closestPointPercentage > 1f)
						{
							this.closestPointPercentage -= 1f;
						}
						while (this.closestPointPercentage < 0f)
						{
							this.closestPointPercentage += 1f;
						}
					}
					if (this.closestPointPercentage > 0f && this.closestPointPercentage < 1f)
					{
						iTween.PutOnPath(base.gameObject, iTweenPath.GetPath(this.path), this.closestPointPercentage);
					}
					this.lookAtPoint.y = base.transform.position.y;
					base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.lookAtPoint);
				}
				if (this.movingRightDirection)
				{
					this.movingRightDirection = false;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
			}
			else if (this.path == string.Empty && this.cover.rightCover != null)
			{
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].speed = 1f;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Right");
				if (this.movingRightDirection)
				{
					this.movingRightDirection = false;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
				WeaponHandling.holdFire = true;
				if (MobileInput.instance != null && !this.spaceToMoveCover)
				{
					MobileInput.instance.enableButton("SwitchCover", base.gameObject);
				}
				this.spaceToMoveCover = true;
				if ((InputManager.GetButtonDown("Jump") || this.coverSwitchAction) && Time.time > this.lastSwitchTime + 1f && this.rootMotionTimer <= 0f)
				{
					this.lastSwitchTime = Time.time;
					if (!this.ncm.disableRotation)
					{
						this.weaponHandler.yRotationOffset = 0f;
						this.setCoverTallRotation();
						this.ncm.disableRotation = true;
						if (this.path == string.Empty)
						{
							this.cam.UnlimitHorizontalAngle();
						}
						this.weaponHandler.resetAiming = true;
					}
					this.coverTallMode = false;
					if (!AnimationHandler.instance.insuredMode)
					{
						this.ncm.canJump = true;
					}
					base.GetComponent<Animation>()["Cover-Run-to-Right"].layer = 2;
					base.GetComponent<Animation>()["Cover-Run-to-Right"].speed = 3f;
					base.GetComponent<Animation>()["Cover-Run-to-Right"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>().Play("Cover-Run-to-Right", PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
					this.rootMotionTimer = base.GetComponent<Animation>()["Cover-Run-to-Right"].length / 3f;
					this.rootMotionTOCoverRun = true;
					this.movingFromTallCover = true;
					if (this.movingRightDirection)
					{
						this.movingRightDirection = false;
						if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
						{
							this.weaponHandler.EnterIdleFree();
						}
					}
					this.cover = this.cover.rightCover;
					this.startPosition = this.cover.transform.Find("StartPosition").transform;
					this.endPosition = this.cover.transform.Find("EndPosition").transform;
				}
			}
			else if (this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED && this.cover.rightPopout && this.cover.rightCornerCover == null && this.cover.rightCover == null)
			{
				base.GetComponent<Animation>()["Cover-Tall-Peak-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Peak-Right"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Peak-Right");
				if (this.movingRightDirection)
				{
					this.movingRightDirection = false;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
				if (!this.peeking)
				{
					this.EnterPeekingMode();
					this.peeking = true;
				}
			}
			else
			{
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Right");
				if (this.movingRightDirection)
				{
					this.movingRightDirection = false;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
				WeaponHandling.holdFire = true;
			}
		}
		else if (!this.aim && this.path == string.Empty && this.cover.rightCornerCover != null && this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0.1f)
		{
			if (this.movingRightDirection)
			{
				if (!this.aim || this.weaponHandler.gm.currentGun == null)
				{
					if (!this.ncm.disableRotation)
					{
						this.weaponHandler.yRotationOffset = 0f;
						this.setCoverTallRotation();
						this.ncm.disableRotation = true;
						if (this.path == string.Empty)
						{
							this.cam.UnlimitHorizontalAngle();
						}
					}
					base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Left");
					WeaponHandling.holdFire = true;
					if (this.peeking)
					{
						this.ExitPeekingMode();
						this.peeking = false;
					}
				}
				else if (((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage > 0.9f) || (this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.4f)) && this.cover.leftPopout)
				{
					if (this.path == string.Empty)
					{
						this.cam.LimitHorizontalAngle(this.playerYRotation, true);
					}
					this.weaponHandler.yRotationOffset = 180f;
					this.ncm.disableRotation = false;
					this.weaponHandler.inversedAiming = true;
					WeaponHandling.holdFire = false;
					base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].speed = 2f;
					base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].wrapMode = WrapMode.ClampForever;
					if (AnimationHandler.instance.insuredMode && !base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Left"))
					{
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.1f, PlayMode.StopAll);
						if (this.weaponHandler != null)
						{
							WeaponHandling.additiveAimingSetup = false;
						}
					}
					else
					{
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.1f, PlayMode.StopSameLayer);
					}
					if (this.path == string.Empty)
					{
						base.transform.position = this.endPosition.position;
					}
				}
				else
				{
					this.ExitCoverTallMode();
				}
			}
			else if (!this.aim || this.weaponHandler.gm.currentGun == null)
			{
				if (!this.ncm.disableRotation)
				{
					this.weaponHandler.yRotationOffset = 0f;
					this.setCoverTallRotation();
					this.ncm.disableRotation = true;
					if (this.path == string.Empty)
					{
						this.cam.UnlimitHorizontalAngle();
					}
				}
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Right");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage < 0.1f) || (this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0.4f)) && this.cover.rightPopout)
			{
				if (this.path == string.Empty)
				{
					this.cam.LimitHorizontalAngle(this.playerYRotation, false);
				}
				this.weaponHandler.yRotationOffset = 180f;
				this.weaponHandler.inversedAiming = false;
				this.ncm.disableRotation = false;
				WeaponHandling.holdFire = false;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].speed = 2f;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].wrapMode = WrapMode.ClampForever;
				if (AnimationHandler.instance.insuredMode && !base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Right"))
				{
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.1f, PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
				}
				else
				{
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.1f, PlayMode.StopSameLayer);
				}
				if (this.path == string.Empty)
				{
					base.transform.position = this.startPosition.position;
				}
			}
			else
			{
				this.ExitCoverTallMode();
			}
		}
		else if (!this.aim && this.path == string.Empty && this.cover.leftCornerCover != null && this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.1f)
		{
			if (this.movingRightDirection)
			{
				if (!this.aim || this.weaponHandler.gm.currentGun == null)
				{
					if (!this.ncm.disableRotation)
					{
						this.weaponHandler.yRotationOffset = 0f;
						this.setCoverTallRotation();
						this.ncm.disableRotation = true;
						if (this.path == string.Empty)
						{
							this.cam.UnlimitHorizontalAngle();
						}
					}
					base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Left");
					WeaponHandling.holdFire = true;
					if (this.peeking)
					{
						this.ExitPeekingMode();
						this.peeking = false;
					}
				}
				else if (((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage > 0.9f) || (this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.4f)) && this.cover.leftPopout)
				{
					if (this.path == string.Empty)
					{
						this.cam.LimitHorizontalAngle(this.playerYRotation, true);
					}
					this.weaponHandler.yRotationOffset = 180f;
					this.ncm.disableRotation = false;
					this.weaponHandler.inversedAiming = true;
					WeaponHandling.holdFire = false;
					base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].speed = 2f;
					base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].wrapMode = WrapMode.ClampForever;
					if (AnimationHandler.instance.insuredMode && !base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Left"))
					{
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.1f, PlayMode.StopAll);
						if (this.weaponHandler != null)
						{
							WeaponHandling.additiveAimingSetup = false;
						}
					}
					else
					{
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.1f, PlayMode.StopSameLayer);
					}
					if (this.path == string.Empty)
					{
						base.transform.position = this.endPosition.position;
					}
				}
				else
				{
					this.ExitCoverTallMode();
				}
			}
			else if (!this.aim || this.weaponHandler.gm.currentGun == null)
			{
				if (!this.ncm.disableRotation)
				{
					this.weaponHandler.yRotationOffset = 0f;
					this.setCoverTallRotation();
					this.ncm.disableRotation = true;
					if (this.path == string.Empty)
					{
						this.cam.UnlimitHorizontalAngle();
					}
				}
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Right");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage < 0.1f) || (this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0.4f)) && this.cover.rightPopout)
			{
				if (this.path == string.Empty)
				{
					this.cam.LimitHorizontalAngle(this.playerYRotation, false);
				}
				this.weaponHandler.yRotationOffset = 180f;
				this.weaponHandler.inversedAiming = false;
				this.ncm.disableRotation = false;
				WeaponHandling.holdFire = false;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].speed = 2f;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].wrapMode = WrapMode.ClampForever;
				if (AnimationHandler.instance.insuredMode && !base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Right"))
				{
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.1f, PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
				}
				else
				{
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.1f, PlayMode.StopSameLayer);
				}
				if (this.path == string.Empty)
				{
					base.transform.position = this.startPosition.position;
				}
			}
			else
			{
				this.ExitCoverTallMode();
			}
		}
		else if (this.movingRightDirection)
		{
			if (!this.aim || this.weaponHandler.gm.currentGun == null)
			{
				if (!this.ncm.disableRotation)
				{
					this.weaponHandler.yRotationOffset = 0f;
					this.setCoverTallRotation();
					this.ncm.disableRotation = true;
					if (this.path == string.Empty)
					{
						this.cam.UnlimitHorizontalAngle();
					}
				}
				base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Left");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage > 0.9f) || (this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.4f)) && this.cover.leftPopout)
			{
				if (this.path == string.Empty)
				{
					this.cam.LimitHorizontalAngle(this.playerYRotation, true);
				}
				this.weaponHandler.yRotationOffset = 180f;
				this.ncm.disableRotation = false;
				this.weaponHandler.inversedAiming = true;
				WeaponHandling.holdFire = false;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].speed = 2f;
				base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].wrapMode = WrapMode.ClampForever;
				if (AnimationHandler.instance.insuredMode && !base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Left"))
				{
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.1f, PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
				}
				else
				{
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.1f, PlayMode.StopSameLayer);
				}
				if (this.path == string.Empty)
				{
					base.transform.position = this.endPosition.position;
				}
			}
			else
			{
				this.ExitCoverTallMode();
			}
		}
		else if (!this.aim || this.weaponHandler.gm.currentGun == null)
		{
			if (!this.ncm.disableRotation)
			{
				this.weaponHandler.yRotationOffset = 0f;
				this.setCoverTallRotation();
				this.ncm.disableRotation = true;
				if (this.path == string.Empty)
				{
					this.cam.UnlimitHorizontalAngle();
				}
			}
			base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].layer = 1;
			base.GetComponent<Animation>()["Cover-Tall-Idle-Right"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Right");
			WeaponHandling.holdFire = true;
			if (this.peeking)
			{
				this.ExitPeekingMode();
				this.peeking = false;
			}
		}
		else if (((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage < 0.1f) || (this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0.4f)) && this.cover.rightPopout)
		{
			if (this.path == string.Empty)
			{
				this.cam.LimitHorizontalAngle(this.playerYRotation, false);
			}
			this.weaponHandler.yRotationOffset = 180f;
			this.weaponHandler.inversedAiming = false;
			this.ncm.disableRotation = false;
			WeaponHandling.holdFire = false;
			base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].layer = 1;
			base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].speed = 2f;
			base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].wrapMode = WrapMode.ClampForever;
			if (AnimationHandler.instance.insuredMode && !base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Right"))
			{
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.1f, PlayMode.StopAll);
				if (this.weaponHandler != null)
				{
					WeaponHandling.additiveAimingSetup = false;
				}
			}
			else
			{
				base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.1f, PlayMode.StopSameLayer);
			}
			if (this.path == string.Empty)
			{
				base.transform.position = this.startPosition.position;
			}
		}
		else
		{
			this.ExitCoverTallMode();
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x0005D29C File Offset: 0x0005B49C
	public void ExitCoverTallMode()
	{
		//if (Interaction.preventExitingCover)
		//{
		//	return;
		//}
		if (!this.ncm.disableRotation)
		{
			this.weaponHandler.yRotationOffset = 0f;
			this.setCoverTallRotation();
			this.ncm.disableRotation = true;
			if (this.path == string.Empty)
			{
				this.cam.UnlimitHorizontalAngle();
			}
			this.weaponHandler.resetAiming = true;
		}
		this.coverTallMode = false;
		this.ncm.disableMovement = false;
		this.ncm.disableRotation = false;
		if (this.path != string.Empty)
		{
			MobileInput.instance.disableButton("cover", base.gameObject);
		}
		if (this.cam != null)
		{
			this.cam.aim = false;
			this.cam.coverOffset = 0f;
			this.cam.inversedAiming = false;
			this.weaponHandler.inversedAiming = false;
		}
		if (MobileInput.aim)
		{
			Rect pixelInset = MobileInput.instance.aimButton.pixelInset;
			pixelInset.x -= MobileInput.guiTouchOffset;
			pixelInset.y -= MobileInput.guiTouchOffset;
			MobileInput.instance.aimButton.pixelInset = pixelInset;
			MobileInput.aim = false;
		}
		if (!AnimationHandler.instance.insuredMode)
		{
			this.ncm.canJump = true;
		}
		this.animHandler.isInInteraction = false;
		base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
		this.blindFire = false;
		this.weaponHandler.blindFire = false;
		if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
		{
			this.weaponHandler.RepositionTheCurrentWeaponInHand();
		}
		WeaponHandling.holdFire = false;
		if (this.weaponHandler != null)
		{
			this.weaponHandler.inCover = false;
		}
		WeaponHandling.additiveAimingSetup = false;
		if (this.movingRightDirection)
		{
			base.GetComponent<Animation>()["Cover-Tall-Exit-Left"].layer = 1;
			base.GetComponent<Animation>()["Cover-Tall-Exit-Left"].speed = -1f;
			base.GetComponent<Animation>()["Cover-Tall-Exit-Left"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Cover-Tall-Exit-Left");
		}
		else
		{
			base.GetComponent<Animation>()["Cover-Tall-Exit-Right"].layer = 1;
			base.GetComponent<Animation>()["Cover-Tall-Exit-Right"].speed = -1f;
			base.GetComponent<Animation>()["Cover-Tall-Exit-Right"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Cover-Tall-Exit-Right");
		}
		this.basicAgility.FarisHead.PlayOneShot(this.bodyCoverEnterSound, SpeechManager.sfxVolume);
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0005D58C File Offset: 0x0005B78C
	private void setCoverTallRotation()
	{
		if (this.path == string.Empty)
		{
			Transform transform = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
			transform.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
			base.transform.position = transform.position;
			base.transform.rotation = transform.rotation;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		else
		{
			float num = 100000f;
			for (float num2 = 0f; num2 <= 1f; num2 += 0.1f)
			{
				Vector3 b = iTween.PointOnPath(iTweenPath.GetPath(this.path), num2);
				b.y = base.transform.position.y;
				if (Vector3.Distance(base.transform.position, b) < num)
				{
					num = Vector3.Distance(base.transform.position, b);
					this.closestPointPercentage = num2;
				}
			}
			Vector3 position = iTween.PointOnPath(iTweenPath.GetPath(this.path), this.closestPointPercentage);
			position.y = base.transform.position.y;
			base.transform.position = position;
			this.lookAtPoint.y = base.transform.position.y;
			base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.lookAtPoint);
		}
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x0005D758 File Offset: 0x0005B958
	private void setCoverShortRotation()
	{
		if (this.path == string.Empty)
		{
			Transform transform = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
			transform.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
			base.transform.position = transform.position;
			base.transform.rotation = transform.rotation;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		else
		{
			float num = 100000f;
			for (float num2 = 0f; num2 <= 1f; num2 += 0.1f)
			{
				Vector3 b = iTween.PointOnPath(iTweenPath.GetPath(this.path), num2);
				b.y = base.transform.position.y;
				if (Vector3.Distance(base.transform.position, b) < num)
				{
					num = Vector3.Distance(base.transform.position, b);
					this.closestPointPercentage = num2;
				}
			}
			Vector3 position = iTween.PointOnPath(iTweenPath.GetPath(this.path), this.closestPointPercentage);
			position.y = base.transform.position.y;
			base.transform.position = position;
			this.lookAtPoint.y = base.transform.position.y;
			base.transform.rotation = Quaternion.LookRotation(this.lookAtPoint - base.transform.position);
		}
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0005D924 File Offset: 0x0005BB24
	private void HandleCoverShortMode()
	{
		float num = Vector3.Dot(Camera.main.transform.right, base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.right) * this.VerticalAxis;
		float num2 = Vector3.Dot(Camera.main.transform.right, -base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, -base.transform.right) * this.VerticalAxis;
		if (MobileInput.instance != null && this.spaceToMoveCover)
		{
			MobileInput.instance.disableButton("SwitchCover", base.gameObject);
		}
		this.spaceToCornerCover = false;
		this.spaceToMoveCover = false;
		if (this.coverButton)
		{
			if (Time.time <= this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
			{
				return;
			}
			this.coverShortMode = false;
			this.cam.inversedAiming = false;
			this.weaponHandler.inversedAiming = false;
			base.GetComponent<Animation>()["General-Idle"].layer = 0;
			this.ncm.disableMovement = false;
			this.ncm.disableRotation = false;
			if (!AnimationHandler.instance.insuredMode)
			{
				this.ncm.canJump = true;
			}
			this.animHandler.isInInteraction = false;
			base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			WeaponHandling.holdFire = false;
			this.cam.shortCoverY = 0f;
			if (MobileInput.aim)
			{
				Rect pixelInset = MobileInput.instance.aimButton.pixelInset;
				pixelInset.x -= MobileInput.guiTouchOffset;
				pixelInset.y -= MobileInput.guiTouchOffset;
				MobileInput.instance.aimButton.pixelInset = pixelInset;
				MobileInput.aim = false;
			}
			this.blindFire = false;
			this.weaponHandler.blindFire = false;
			if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
			{
				this.weaponHandler.RepositionTheCurrentWeaponInHand();
			}
			if (this.weaponHandler != null)
			{
				this.weaponHandler.inCover = false;
			}
			WeaponHandling.additiveAimingSetup = false;
			if (this.movingRightDirection)
			{
				base.GetComponent<Animation>()["Cover-Short-Exit-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Exit-Left"].speed = -1f;
				base.GetComponent<Animation>()["Cover-Short-Exit-Left"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Exit-Left");
			}
			else
			{
				base.GetComponent<Animation>()["Cover-Short-Exit-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Exit-Right"].speed = -1f;
				base.GetComponent<Animation>()["Cover-Short-Exit-Right"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Exit-Right");
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyCoverEnterSound, SpeechManager.sfxVolume);
			this.lastUserInputTime = Time.time;
			this.basicAgility.lastUserInputTime = Time.time;
		}
		else if (!this.blindFire && num == 0f && num2 == 0f && this.fire && !this.aim && Vector3.Angle(this.cam.transform.forward, base.transform.forward) < 90f)
		{
			string text;
			if (this.movingRightDirection)
			{
				text = "Cover-Short-Handgun-Blindfire-Right-Enter";
			}
			else
			{
				text = "Cover-Short-Handgun-Blindfire-Left-Enter";
			}
			base.GetComponent<Animation>()[text].layer = 1;
			base.GetComponent<Animation>()[text].speed = 1f;
			base.GetComponent<Animation>()[text].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().Play(text, PlayMode.StopAll);
			WeaponHandling.additiveAimingSetup = false;
			this.blindFire = true;
			this.blindFireFirstShot = true;
			this.blindFireStartTime = Time.time;
			this.weaponHandler.blindFire = true;
			this.weaponHandler.gm.currentGun.lastShootTime = Time.time;
		}
		else if ((this.blindFire && this.aim) || (this.blindFire && Time.time > this.blindFireStartTime + base.GetComponent<Animation>()["Cover-Short-Handgun-Blindfire-Left-Enter"].length && Time.time > this.weaponHandler.gm.currentGun.lastShootTime + 1f))
		{
			this.blindFire = false;
			this.weaponHandler.blindFire = false;
			this.weaponHandler.RepositionTheCurrentWeaponInHand();
			if (!this.aim)
			{
				this.weaponHandler.EnterIdleFree();
				WeaponHandling.holdFire = true;
			}
			WeaponHandling.additiveAimingSetup = false;
		}
		else if (this.blindFire && Vector3.Angle(this.cam.transform.forward, base.transform.forward) < 90f)
		{
			if (Time.time > this.blindFireStartTime + base.GetComponent<Animation>()["Cover-Short-Handgun-Blindfire-Left-Enter"].length)
			{
				if (this.blindFireFirstShot)
				{
					WeaponHandling.holdFire = false;
					this.weaponHandler.gm.currentGun.fire = true;
					this.blindFireFirstShot = false;
					string text2;
					if (this.movingRightDirection)
					{
						text2 = "Cover-Short-Handgun-Blindfire-Right";
					}
					else
					{
						text2 = "Cover-Short-Handgun-Blindfire-Left";
					}
					base.GetComponent<Animation>()[text2].layer = 1;
					base.GetComponent<Animation>()[text2].speed = 1f;
					base.GetComponent<Animation>()[text2].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>().Blend(text2);
				}
				else if (this.fire)
				{
					WeaponHandling.holdFire = false;
					this.weaponHandler.gm.currentGun.fire = true;
					string text3;
					if (this.movingRightDirection)
					{
						text3 = "Cover-Short-Handgun-Blindfire-Right";
					}
					else
					{
						text3 = "Cover-Short-Handgun-Blindfire-Left";
					}
					base.GetComponent<Animation>()[text3].layer = 1;
					base.GetComponent<Animation>()[text3].speed = 1f;
					base.GetComponent<Animation>()[text3].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>().Blend(text3);
				}
			}
		}
		else if (this.blindFire)
		{
			this.blindFire = false;
			this.weaponHandler.blindFire = false;
			this.weaponHandler.RepositionTheCurrentWeaponInHand();
			if (!this.aim)
			{
				this.weaponHandler.EnterIdleFree();
				WeaponHandling.holdFire = true;
			}
			WeaponHandling.additiveAimingSetup = false;
		}
		else if (num > num2 && num > 0.5f)
		{
			if ((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage > 0f) || (this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x < 0f))
			{
				base.GetComponent<Animation>()["Cover-Short-Move-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Move-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Move-Left");
				this.TriggerSteepsSound("Cover-Short-Move-Left");
				if (this.path == string.Empty)
				{
					base.transform.Translate(2f * Time.deltaTime * base.GetComponent<Animation>()["Cover-Short-Move-Left"].length, 0f, 0f);
				}
				else
				{
					this.closestPointPercentage -= 0.3f / Vector3.Distance(iTweenPath.GetPath(this.path)[0], iTweenPath.GetPath(this.path)[1]) * Time.deltaTime;
					if (iTweenPath.IsClosed(this.path))
					{
						while (this.closestPointPercentage > 1f)
						{
							this.closestPointPercentage -= 1f;
						}
						while (this.closestPointPercentage < 0f)
						{
							this.closestPointPercentage += 1f;
						}
					}
					if (this.closestPointPercentage > 0f && this.closestPointPercentage < 1f)
					{
						iTween.PutOnPath(base.gameObject, iTweenPath.GetPath(this.path), this.closestPointPercentage);
					}
					this.lookAtPoint.y = base.transform.position.y;
					base.transform.rotation = Quaternion.LookRotation(this.lookAtPoint - base.transform.position);
				}
				Vector3 position = base.transform.position;
				position.y += 1f;
				RaycastHit raycastHit;
				if (Physics.Raycast(position, -base.transform.up, out raycastHit, 2f, this.mask))
				{
					base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - (raycastHit.distance - 1f), base.transform.position.z);
				}
				if (!this.movingRightDirection)
				{
					this.movingRightDirection = true;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
			}
			else if (this.path == string.Empty && this.cover.rightCover != null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Right");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
				if (MobileInput.instance != null && !this.spaceToMoveCover)
				{
					MobileInput.instance.enableButton("SwitchCover", base.gameObject);
				}
				this.spaceToMoveCover = true;
				if ((InputManager.GetButtonDown("Jump") || this.coverSwitchAction) && Time.time > this.lastSwitchTime + 1f && this.rootMotionTimer <= 0f)
				{
					this.lastSwitchTime = Time.time;
					this.coverShortMode = false;
					base.GetComponent<Animation>()["General-Idle"].layer = 0;
					base.GetComponent<Animation>()["Cover-Run-Short-to-Right"].layer = 2;
					base.GetComponent<Animation>()["Cover-Run-Short-to-Right"].speed = 3f;
					base.GetComponent<Animation>()["Cover-Run-Short-to-Right"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>().Play("Cover-Run-Short-to-Right", PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
					this.rootMotionTimer = base.GetComponent<Animation>()["Cover-Run-Short-to-Right"].length / 3f;
					this.rootMotionTOCoverRun = true;
					this.movingFromTallCover = false;
					if (this.movingRightDirection)
					{
						this.movingRightDirection = false;
						if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
						{
							this.weaponHandler.EnterIdleFree();
						}
					}
					this.cover = this.cover.rightCover;
					this.startPosition = this.cover.transform.Find("StartPosition").transform;
					this.endPosition = this.cover.transform.Find("EndPosition").transform;
				}
			}
			else if (this.cover.rightCornerCover != null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Right");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (this.weaponHandler != null && this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
			{
				if (this.aim)
				{
					this.weaponHandler.inversedAiming = false;
					this.ncm.disableRotation = false;
					base.GetComponent<Animation>()["General-Idle"].layer = 1;
					base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("General-Idle");
					WeaponHandling.holdFire = false;
				}
				else
				{
					if (!this.ncm.disableRotation)
					{
						this.setCoverShortRotation();
						this.ncm.disableRotation = true;
					}
					base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
					base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Right");
					if (!this.movingRightDirection)
					{
						this.movingRightDirection = true;
						if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
						{
							this.weaponHandler.EnterIdleFree();
						}
					}
				}
			}
			else if (this.cover.rightPopout)
			{
				base.GetComponent<Animation>()["Cover-Short-Peek-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Peek-Left"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Left");
				if (!this.peeking)
				{
					this.EnterPeekingMode();
					this.peeking = true;
				}
				this.movingRightDirection = true;
			}
		}
		else if (num2 > num && num2 > 0.5f)
		{
			if ((this.path != string.Empty && iTweenPath.IsClosed(this.path)) || (this.path != string.Empty && !iTweenPath.IsClosed(this.path) && this.closestPointPercentage < 1f) || (this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x > 0f))
			{
				base.GetComponent<Animation>()["Cover-Short-Move-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Move-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Move-Right");
				this.TriggerSteepsSound("Cover-Short-Move-Right");
				if (this.path == string.Empty)
				{
					base.transform.Translate(-2f * Time.deltaTime * base.GetComponent<Animation>()["Cover-Short-Move-Right"].length, 0f, 0f);
				}
				else
				{
					this.closestPointPercentage += 0.3f / Vector3.Distance(iTweenPath.GetPath(this.path)[0], iTweenPath.GetPath(this.path)[1]) * Time.deltaTime;
					if (iTweenPath.IsClosed(this.path))
					{
						while (this.closestPointPercentage > 1f)
						{
							this.closestPointPercentage -= 1f;
						}
						while (this.closestPointPercentage < 0f)
						{
							this.closestPointPercentage += 1f;
						}
					}
					if (this.closestPointPercentage > 0f && this.closestPointPercentage < 1f)
					{
						iTween.PutOnPath(base.gameObject, iTweenPath.GetPath(this.path), this.closestPointPercentage);
					}
					this.lookAtPoint.y = base.transform.position.y;
					base.transform.rotation = Quaternion.LookRotation(this.lookAtPoint - base.transform.position);
				}
				Vector3 position2 = base.transform.position;
				position2.y += 1f;
				RaycastHit raycastHit2;
				if (Physics.Raycast(position2, -base.transform.up, out raycastHit2, 2f, this.mask))
				{
					base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - (raycastHit2.distance - 1f), base.transform.position.z);
				}
				if (this.movingRightDirection)
				{
					this.movingRightDirection = false;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
			}
			else if (this.path == string.Empty && this.cover.leftCover != null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
				if (MobileInput.instance != null && !this.spaceToMoveCover)
				{
					MobileInput.instance.enableButton("SwitchCover", base.gameObject);
				}
				this.spaceToMoveCover = true;
				if ((InputManager.GetButtonDown("Jump") || this.coverSwitchAction) && Time.time > this.lastSwitchTime + 1f && this.rootMotionTimer <= 0f)
				{
					this.lastSwitchTime = Time.time;
					this.coverShortMode = false;
					base.GetComponent<Animation>()["General-Idle"].layer = 0;
					base.GetComponent<Animation>()["Cover-Run-Short-to-Left"].layer = 2;
					base.GetComponent<Animation>()["Cover-Run-Short-to-Left"].speed = 3f;
					base.GetComponent<Animation>()["Cover-Run-Short-to-Left"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>().Play("Cover-Run-Short-to-Left", PlayMode.StopAll);
					if (this.weaponHandler != null)
					{
						WeaponHandling.additiveAimingSetup = false;
					}
					this.rootMotionTimer = base.GetComponent<Animation>()["Cover-Run-Short-to-Left"].length / 3f;
					this.rootMotionTOCoverRun = true;
					this.movingFromTallCover = false;
					if (!this.movingRightDirection)
					{
						this.movingRightDirection = true;
						if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
						{
							this.weaponHandler.EnterIdleFree();
						}
					}
					this.cover = this.cover.leftCover;
					this.startPosition = this.cover.transform.Find("StartPosition").transform;
					this.endPosition = this.cover.transform.Find("EndPosition").transform;
				}
			}
			else if (this.cover.leftCornerCover != null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (this.weaponHandler != null && this.weaponHandler.gm.currentGun != null && this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
			{
				if (this.aim)
				{
					this.weaponHandler.inversedAiming = true;
					this.ncm.disableRotation = false;
					base.GetComponent<Animation>()["General-Idle"].layer = 1;
					base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("General-Idle");
					WeaponHandling.holdFire = false;
				}
				else
				{
					if (!this.ncm.disableRotation)
					{
						this.setCoverShortRotation();
						this.ncm.disableRotation = true;
					}
					base.GetComponent<Animation>()["Cover-Short-Idle-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Short-Idle-Left"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left");
					if (this.movingRightDirection)
					{
						this.movingRightDirection = false;
						if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
						{
							this.weaponHandler.EnterIdleFree();
						}
					}
				}
			}
			else if (this.cover.rightPopout)
			{
				base.GetComponent<Animation>()["Cover-Short-Peek-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Peek-Right"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Right");
				if (!this.peeking)
				{
					this.EnterInversedPeekingMode();
					this.peeking = true;
				}
				if (this.movingRightDirection)
				{
					this.movingRightDirection = false;
					if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
					{
						this.weaponHandler.EnterIdleFree();
					}
				}
			}
		}
		else if (!this.aim && this.path == string.Empty && this.cover.rightCornerCover != null && this.endPosition != null && this.endPosition.InverseTransformPoint(base.transform.position).x * this.endPosition.parent.localScale.x > -0.1f)
		{
			if (this.movingRightDirection)
			{
				if (!this.aim && (!this.fire || this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED))
				{
					if (!this.ncm.disableRotation)
					{
						this.setCoverShortRotation();
						this.ncm.disableRotation = true;
					}
					base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
					base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Right");
					WeaponHandling.holdFire = true;
					if (this.peeking)
					{
						this.ExitPeekingMode();
						this.peeking = false;
					}
				}
				else if (this.weaponHandler == null)
				{
					if (!this.ncm.disableRotation)
					{
						this.setCoverShortRotation();
						this.ncm.disableRotation = true;
					}
					base.GetComponent<Animation>()["Cover-Short-Peek-Top-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Short-Peek-Top-Left"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Top-Left");
				}
				else if (this.aim && this.weaponHandler.gm.currentGun != null)
				{
					this.weaponHandler.inversedAiming = false;
					this.ncm.disableRotation = false;
					base.GetComponent<Animation>()["General-Idle"].layer = 1;
					base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("General-Idle");
					WeaponHandling.holdFire = false;
				}
			}
			else if (!this.aim && (!this.fire || this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED))
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (this.weaponHandler == null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Peek-Top-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Peek-Top-Right"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Top-Right");
			}
			else if (this.aim && this.weaponHandler.gm.currentGun != null)
			{
				this.weaponHandler.inversedAiming = true;
				this.ncm.disableRotation = false;
				base.GetComponent<Animation>()["General-Idle"].layer = 1;
				base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("General-Idle");
				WeaponHandling.holdFire = false;
			}
		}
		else if (!this.aim && this.path == string.Empty && this.cover.leftCornerCover != null && this.startPosition != null && this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x < 0.1f)
		{
			if (this.movingRightDirection)
			{
				if (!this.aim && (!this.fire || this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED))
				{
					if (!this.ncm.disableRotation)
					{
						this.setCoverShortRotation();
						this.ncm.disableRotation = true;
					}
					base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
					base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Right");
					WeaponHandling.holdFire = true;
					if (this.peeking)
					{
						this.ExitPeekingMode();
						this.peeking = false;
					}
				}
				else if (this.weaponHandler == null)
				{
					if (!this.ncm.disableRotation)
					{
						this.setCoverShortRotation();
						this.ncm.disableRotation = true;
					}
					base.GetComponent<Animation>()["Cover-Short-Peek-Top-Left"].layer = 1;
					base.GetComponent<Animation>()["Cover-Short-Peek-Top-Left"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Top-Left");
				}
				else if (this.aim && this.weaponHandler.gm.currentGun != null)
				{
					this.weaponHandler.inversedAiming = false;
					this.ncm.disableRotation = false;
					base.GetComponent<Animation>()["General-Idle"].layer = 1;
					base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
					base.GetComponent<Animation>().CrossFade("General-Idle");
					WeaponHandling.holdFire = false;
				}
			}
			else if (!this.aim && (!this.fire || this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED))
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Left"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (this.weaponHandler == null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Peek-Top-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Peek-Top-Right"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Top-Right");
			}
			else if (this.aim && this.weaponHandler.gm.currentGun != null)
			{
				this.weaponHandler.inversedAiming = true;
				this.ncm.disableRotation = false;
				base.GetComponent<Animation>()["General-Idle"].layer = 1;
				base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("General-Idle");
				WeaponHandling.holdFire = false;
			}
		}
		else if (this.movingRightDirection)
		{
			if (!this.aim && (!this.fire || this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED))
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Idle-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Idle-Right"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Right");
				WeaponHandling.holdFire = true;
				if (this.peeking)
				{
					this.ExitPeekingMode();
					this.peeking = false;
				}
			}
			else if (this.weaponHandler == null)
			{
				if (!this.ncm.disableRotation)
				{
					this.setCoverShortRotation();
					this.ncm.disableRotation = true;
				}
				base.GetComponent<Animation>()["Cover-Short-Peek-Top-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Peek-Top-Left"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Top-Left");
			}
			else if (this.aim && this.weaponHandler.gm.currentGun != null)
			{
				this.weaponHandler.inversedAiming = false;
				this.ncm.disableRotation = false;
				base.GetComponent<Animation>()["General-Idle"].layer = 1;
				base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>().CrossFade("General-Idle");
				WeaponHandling.holdFire = false;
			}
		}
		else if (!this.aim && (!this.fire || this.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED))
		{
			if (!this.ncm.disableRotation)
			{
				this.setCoverShortRotation();
				this.ncm.disableRotation = true;
			}
			base.GetComponent<Animation>()["Cover-Short-Idle-Left"].layer = 1;
			base.GetComponent<Animation>()["Cover-Short-Idle-Left"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left");
			WeaponHandling.holdFire = true;
			if (this.peeking)
			{
				this.ExitPeekingMode();
				this.peeking = false;
			}
		}
		else if (this.weaponHandler == null)
		{
			if (!this.ncm.disableRotation)
			{
				this.setCoverShortRotation();
				this.ncm.disableRotation = true;
			}
			base.GetComponent<Animation>()["Cover-Short-Peek-Top-Right"].layer = 1;
			base.GetComponent<Animation>()["Cover-Short-Peek-Top-Right"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().CrossFade("Cover-Short-Peek-Top-Right");
		}
		else if (this.aim && this.weaponHandler.gm.currentGun != null)
		{
			this.weaponHandler.inversedAiming = true;
			this.ncm.disableRotation = false;
			base.GetComponent<Animation>()["General-Idle"].layer = 1;
			base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("General-Idle");
			WeaponHandling.holdFire = false;
		}
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0005FB38 File Offset: 0x0005DD38
	private void TriggerSteepsSound(string anim)
	{
		float num = 0f;
		float num2 = 0f;
		float num3;
		for (num3 = base.GetComponent<Animation>()[anim].normalizedTime; num3 > 1f; num3 -= 1f)
		{
		}
		switch (anim)
		{
		case "Cover-Short-Move-Left":
			num = 0.3f;
			num2 = 0.76f;
			break;
		case "Cover-Short-Move-Right":
			num = 0.3f;
			num2 = 0.76f;
			break;
		case "Cover-Tall-Move-Left":
			num = 0.17f;
			num2 = 0.69f;
			break;
		case "Cover-Tall-Move-Right":
			num = 0.17f;
			num2 = 0.69f;
			break;
		case "Interaction-Wall-Back-Move-Left":
			num = 0.62f;
			num2 = 0.96f;
			break;
		case "Interaction-Wall-Back-Move-Right":
			num = 0.62f;
			num2 = 0.96f;
			break;
		}
		if (num3 > num && num3 < num + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num3 > num2 && num3 < num2 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else
		{
			this.emmittingStepSound = false;
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0005FD00 File Offset: 0x0005DF00
	private void HandlePoleClimbMode()
	{
		if (this.turnTimer > 0f)
		{
			this.turnTimer -= Time.deltaTime;
			return;
		}
		float num = Vector3.Dot(Camera.main.transform.right, base.transform.forward) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.forward) * this.VerticalAxis;
		float num2 = Vector3.Dot(Camera.main.transform.right, -base.transform.forward) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, -base.transform.forward) * this.VerticalAxis;
		if (this.exitInteractionButton)
		{
			if (Time.time <= this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
			{
				return;
			}
			this.poleClimbMode = false;
			this.ncm.enabled = true;
			if (!AnimationHandler.instance.insuredMode)
			{
				this.ncm.canJump = true;
			}
			this.animHandler.isInInteraction = false;
			base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			base.GetComponent<Animation>()["Interaction-Pole-Drop"].layer = 9;
			base.GetComponent<Animation>()["Interaction-Pole-Drop"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Interaction-Pole-Drop", 0.3f, PlayMode.StopAll);
			if (this.weaponHandler != null)
			{
				WeaponHandling.additiveAimingSetup = false;
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyPoleClimbSound, SpeechManager.sfxVolume);
			this.DontLookDownPlayed = false;
			this.almostTherePlayed = false;
			this.lastUserInputTime = Time.time;
			this.basicAgility.lastUserInputTime = Time.time;
			this.basicAgility.FarisHead.PlayOneShot(this.farisExitPoleSound, SpeechManager.speechVolume);
			Instructions.instruction = Instructions.Instruction.NONE;
		}
		else if (num2 > num && this.startPosition.InverseTransformPoint(base.transform.position).z > 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Interaction-Pole-Turn"))
			{
				base.GetComponent<Animation>()["Interaction-Pole-Turn"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>()["Interaction-Pole-Turn"].layer = 10;
				base.GetComponent<Animation>().CrossFade("Interaction-Pole-Turn");
				iTween.RotateBy(base.gameObject, new Vector3(0f, -0.5f, 0f), base.GetComponent<Animation>()["Interaction-Pole-Turn"].length * 0.95f);
				this.turnTimer = base.GetComponent<Animation>()["Interaction-Pole-Turn"].length;
				Transform transform = this.startPosition;
				this.startPosition = this.endPosition;
				this.endPosition = transform;
			}
		}
		else if (num > num2 && this.endPosition.InverseTransformPoint(base.transform.position).z > 0f)
		{
			base.GetComponent<Animation>()["Interaction-Pole-Move"].layer = 9;
			base.GetComponent<Animation>()["Interaction-Pole-Move"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Pole-Move");
			base.transform.Translate(0f, 0f, 1f * Time.deltaTime * base.GetComponent<Animation>()["Interaction-Pole-Move"].length);
			if (!this.DontLookDownPlayed && this.endPosition.InverseTransformPoint(base.transform.position).z < this.endPosition.InverseTransformPoint(this.startPosition.position).z * 3f / 4f)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.FarisDontLookDownSoundArabic != null)
				{
					this.basicAgility.FarisHead.clip = this.FarisDontLookDownSoundArabic;
				}
				else
				{
					this.basicAgility.FarisHead.clip = this.FarisDontLookDownSound;
				}
				this.basicAgility.FarisHead.Play();
				this.DontLookDownPlayed = true;
			}
			if (!this.almostTherePlayed && this.endPosition.InverseTransformPoint(base.transform.position).z < this.endPosition.InverseTransformPoint(this.startPosition.position).z / 4f)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.FarisPoleAlmostThereSoundArabic != null)
				{
					this.basicAgility.FarisHead.clip = this.FarisPoleAlmostThereSoundArabic;
				}
				else
				{
					this.basicAgility.FarisHead.clip = this.FarisPoleAlmostThereSound;
				}
				this.basicAgility.FarisHead.Play();
				this.almostTherePlayed = true;
			}
			this.PlayLedgeMoveSounds();
			this.TriggerInteractionHandSounds("Interaction-Pole-Move");
		}
		else
		{
			base.GetComponent<Animation>()["Interaction-Pole-Idle"].layer = 9;
			base.GetComponent<Animation>()["Interaction-Pole-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Pole-Idle");
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00060298 File Offset: 0x0005E498
	private void PlayLedgeMoveSounds()
	{
		if (!this.basicAgility.FarisHead.isPlaying && Time.time > this.lastLedgeSoundPlayTime + 5f)
		{
			this.basicAgility.FarisHead.clip = this.FarisInteractionMoveSounds[this.currentInteractionMoveSound];
			this.basicAgility.FarisHead.Play();
			this.currentInteractionMoveSound = (this.currentInteractionMoveSound + 1) % this.FarisInteractionMoveSounds.Length;
			this.lastLedgeSoundPlayTime = Time.time;
		}
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00060320 File Offset: 0x0005E520
	private void PlaInteractionHandSounds()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.FarisInteractionHandSounds[this.currentInteractionHandSound], SpeechManager.speechVolume);
		this.currentInteractionHandSound = (this.currentInteractionHandSound + 1) % this.FarisInteractionHandSounds.Length;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00060364 File Offset: 0x0005E564
	private void TriggerInteractionHandSounds(string anim)
	{
		float num = 0f;
		float num2 = 0f;
		float num3;
		for (num3 = Mathf.Abs(base.GetComponent<Animation>()[anim].normalizedTime); num3 > 1f; num3 -= 1f)
		{
		}
		switch (anim)
		{
		case "Interaction-Pole-Move":
			num = 0.48f;
			num2 = 0.9f;
			break;
		case "Interaction-Rope-Move":
			num = 0.5f;
			num2 = 0.9f;
			break;
		case "Interaction-Ladder-Move":
			num = 0.51f;
			num2 = 0.9f;
			break;
		}
		if (num3 > num && num3 < num + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				this.PlaInteractionHandSounds();
			}
			this.emmittingStepSound = true;
		}
		else if (num3 > num2 && num3 < num2 + 0.1f)
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

	// Token: 0x060009AF RID: 2479 RVA: 0x000604C0 File Offset: 0x0005E6C0
	private void HandleLogMode()
	{
		if (this.falledFromLog)
		{
			if (this.fallingRightDirection)
			{
				if (this.cc.isGrounded)
				{
					base.transform.Translate(0.1f, 0f, 0f);
				}
			}
			else if (this.cc.isGrounded)
			{
				base.transform.Translate(-0.1f, 0f, 0f);
			}
			return;
		}
		if (this.turnTimer > 0f)
		{
			this.turnTimer -= Time.deltaTime;
			return;
		}
		float num = Vector3.Dot(Camera.main.transform.right, base.transform.forward) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.forward) * this.VerticalAxis;
		float num2 = Vector3.Dot(Camera.main.transform.right, -base.transform.forward) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, -base.transform.forward) * this.VerticalAxis;
		float num3 = Vector3.Dot(Camera.main.transform.right, base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.right) * this.VerticalAxis;
		float num4 = Vector3.Dot(Camera.main.transform.right, -base.transform.right) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, -base.transform.right) * this.VerticalAxis;
		if (Time.time > this.lastFallAtemptTime + this.timeBeforeAtemptingToFall && this.endPosition.InverseTransformPoint(base.transform.position).z >= this.endPosition.InverseTransformPoint(this.startPosition.position).z / 3f)
		{
			if (this.fallingRightDirection)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Interaction-Log-Balance-Right"))
				{
					base.GetComponent<Animation>()["Interaction-Log-Balance-Right"].wrapMode = WrapMode.ClampForever;
					base.GetComponent<Animation>()["Interaction-Log-Balance-Right"].layer = 9;
					base.GetComponent<Animation>()["Interaction-Log-Balance-Right"].time = 0f;
					base.GetComponent<Animation>().CrossFade("Interaction-Log-Balance-Right");
					this.safeTime = base.GetComponent<Animation>()["Interaction-Log-Balance-Right"].length;
					this.basicAgility.FarisHead.PlayOneShot(this.FarisLogBalanceSound, SpeechManager.speechVolume);
				}
			}
			else if (!base.GetComponent<Animation>().IsPlaying("Interaction-Log-Balance-Left"))
			{
				base.GetComponent<Animation>()["Interaction-Log-Balance-Left"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>()["Interaction-Log-Balance-Left"].layer = 9;
				base.GetComponent<Animation>()["Interaction-Log-Balance-Left"].time = 0f;
				base.GetComponent<Animation>().CrossFade("Interaction-Log-Balance-Left");
				this.safeTime = base.GetComponent<Animation>()["Interaction-Log-Balance-Left"].length;
				this.basicAgility.FarisHead.PlayOneShot(this.FarisLogBalanceSound2, SpeechManager.speechVolume);
			}
			this.safeTime -= Time.deltaTime;
			if (this.safeTime >= 0f)
			{
				if (!this.fallingRightDirection && num3 > num4 + 0.9f)
				{
					if (!base.GetComponent<Animation>().IsPlaying("Interaction-Log-Balance-Left-Reverse"))
					{
						base.GetComponent<Animation>()["Interaction-Log-Balance-Left-Reverse"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>()["Interaction-Log-Balance-Left-Reverse"].layer = 9;
						base.GetComponent<Animation>()["Interaction-Log-Balance-Left-Reverse"].time = 0f;
						base.GetComponent<Animation>().CrossFade("Interaction-Log-Balance-Left-Reverse");
						this.timeBeforeAtemptingToFall = UnityEngine.Random.Range(3f, 6f);
					}
					this.lastFallAtemptTime = Time.time;
					this.fallingRightDirection = !this.fallingRightDirection;
				}
				else if (this.fallingRightDirection && num4 > num3 + 0.9f)
				{
					if (!base.GetComponent<Animation>().IsPlaying("Interaction-Log-Balance-Right-Reverse"))
					{
						base.GetComponent<Animation>()["Interaction-Log-Balance-Right-Reverse"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>()["Interaction-Log-Balance-Right-Reverse"].layer = 9;
						base.GetComponent<Animation>()["Interaction-Log-Balance-Right-Reverse"].time = 0f;
						base.GetComponent<Animation>().CrossFade("Interaction-Log-Balance-Right-Reverse");
						this.timeBeforeAtemptingToFall = UnityEngine.Random.Range(3f, 6f);
					}
					this.lastFallAtemptTime = Time.time;
					this.fallingRightDirection = !this.fallingRightDirection;
				}
			}
			else if (this.fallingRightDirection)
			{
				this.falledFromLog = true;
				this.ncm.enabled = true;
				this.pcc.acceptUserInput = false;
				this.animHandler.isInInteraction = false;
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.FALLING;
			}
			else
			{
				this.falledFromLog = true;
				this.ncm.enabled = true;
				this.pcc.acceptUserInput = false;
				this.animHandler.isInInteraction = false;
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.FALLING;
			}
			return;
		}
		if (num2 > num + 0.9f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Interaction-Log-Turn"))
			{
				base.GetComponent<Animation>()["Interaction-Log-Turn"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>()["Interaction-Log-Turn"].layer = 10;
				base.GetComponent<Animation>().CrossFade("Interaction-Log-Turn");
				iTween.RotateBy(base.gameObject, new Vector3(0f, -0.5f, 0f), base.GetComponent<Animation>()["Interaction-Log-Turn"].length * 0.95f);
				this.turnTimer = base.GetComponent<Animation>()["Interaction-Log-Turn"].length;
				Transform transform = this.startPosition;
				this.startPosition = this.endPosition;
				this.endPosition = transform;
			}
		}
		else if (num > num2)
		{
			base.GetComponent<Animation>()["Interaction-Log-Walk"].layer = 9;
			base.GetComponent<Animation>()["Interaction-Log-Walk"].speed = 1f;
			base.GetComponent<Animation>()["Interaction-Log-Walk"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Log-Walk");
			base.transform.Translate(0f, 0f, 0.4f * Time.deltaTime * base.GetComponent<Animation>()["Interaction-Log-Walk"].length);
			this.TriggerLogSteepsSound("Interaction-Log-Walk");
			if (!this.almostTherePlayed && this.endPosition.InverseTransformPoint(base.transform.position).z < this.endPosition.InverseTransformPoint(this.startPosition.position).z / 4f)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.FarisLogAlmostThereSoundArabic != null)
				{
					this.basicAgility.FarisHead.PlayOneShot(this.FarisLogAlmostThereSoundArabic, SpeechManager.speechVolume);
				}
				else
				{
					this.basicAgility.FarisHead.PlayOneShot(this.FarisLogAlmostThereSound, SpeechManager.speechVolume);
				}
				this.almostTherePlayed = true;
			}
		}
		else
		{
			base.GetComponent<Animation>()["Interaction-Log-Idle"].layer = 9;
			base.GetComponent<Animation>()["Interaction-Log-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Log-Idle");
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00060CD8 File Offset: 0x0005EED8
	private void TriggerLogSteepsSound(string anim)
	{
		float num = 0.2f;
		float num2 = 0.33f;
		float num3 = 0.52f;
		float num4 = 0.67f;
		float num5 = 0.83f;
		float num6 = 0.98f;
		float num7;
		for (num7 = base.GetComponent<Animation>()[anim].normalizedTime; num7 > 1f; num7 -= 1f)
		{
		}
		if (num7 > num && num7 < num + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num7 > num2 && num7 < num2 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num7 > num3 && num7 < num3 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num7 > num4 && num7 < num4 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num7 > num5 && num7 < num5 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num7 > num6 && num7 < num6 + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else
		{
			this.emmittingStepSound = false;
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00060E9C File Offset: 0x0005F09C
	public void ExitLogMode()
	{
		this.logMode = false;
		this.ncm.enabled = true;
		if (!AnimationHandler.instance.insuredMode)
		{
			this.ncm.canJump = true;
		}
		this.animHandler.isInInteraction = false;
		base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
		this.almostTherePlayed = false;
		base.GetComponent<Animation>().CrossFade("General-Idle", 0.3f, PlayMode.StopAll);
		if (this.weaponHandler != null)
		{
			WeaponHandling.additiveAimingSetup = false;
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00060F58 File Offset: 0x0005F158
	private void HandleRopeClimbMode()
	{
		if (this.exitInteractionButton)
		{
			if (Time.time <= this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
			{
				return;
			}
			this.ropeClimbMode = false;
			this.ncm.enabled = true;
			if (!AnimationHandler.instance.insuredMode)
			{
				this.ncm.canJump = true;
			}
			this.animHandler.isInInteraction = false;
			base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			base.GetComponent<Animation>()["Interaction-Pole-Drop"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Pole-Drop"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Interaction-Pole-Drop");
			this.basicAgility.FarisHead.PlayOneShot(this.bodyRopeClimbSound, SpeechManager.sfxVolume);
			this.lastUserInputTime = Time.time;
			this.basicAgility.lastUserInputTime = Time.time;
			Instructions.instruction = Instructions.Instruction.NONE;
		}
		else if (this.VerticalAxis > 0.1f)
		{
			if ((this.movingDown && base.transform.position.y < this.startPosition.position.y) || (!this.movingDown && base.transform.position.y < this.endPosition.position.y))
			{
				base.GetComponent<Animation>()["Interaction-Rope-Move"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Rope-Move"].wrapMode = WrapMode.Loop;
				base.GetComponent<Animation>()["Interaction-Rope-Move"].speed = 1f;
				base.GetComponent<Animation>().CrossFade("Interaction-Rope-Move");
				base.transform.Translate(0f, 1f * Time.deltaTime * base.GetComponent<Animation>()["Interaction-Rope-Move"].length, 0f);
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds("Interaction-Rope-Move");
			}
			else
			{
				base.GetComponent<Animation>()["Interaction-Rope-End"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Rope-End"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>()["Interaction-Rope-End"].speed = 1f;
				base.GetComponent<Animation>().CrossFade("Interaction-Rope-End");
				this.endingInteractionTimer = base.GetComponent<Animation>()["Interaction-Rope-End"].length;
				this.endingInteraction = true;
				this.ropeClimbMode = false;
				this.basicAgility.FarisHead.PlayOneShot(this.bodyRopeClimbSound, SpeechManager.sfxVolume);
			}
		}
		else if (this.VerticalAxis < -0.1f && ((this.movingDown && base.transform.position.y > this.endPosition.position.y) || (!this.movingDown && base.transform.position.y > this.startPosition.position.y)))
		{
			base.GetComponent<Animation>()["Interaction-Rope-Move"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Rope-Move"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()["Interaction-Rope-Move"].speed = -1f;
			base.GetComponent<Animation>().CrossFade("Interaction-Rope-Move");
			base.transform.Translate(0f, -1f * Time.deltaTime * base.GetComponent<Animation>()["Interaction-Rope-Move"].length, 0f);
			this.PlayLedgeMoveSounds();
			this.TriggerInteractionHandSounds("Interaction-Rope-Move");
		}
		else
		{
			base.GetComponent<Animation>()["Interaction-Rope-Idle"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Rope-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Rope-Idle");
		}
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0006139C File Offset: 0x0005F59C
	private void HandleLadderClimbMode()
	{
		if (this.exitInteractionButton && this.currentInteractionTrigger.canDrop)
		{
			if (Time.time <= this.lastUserInputTime + this.timeBeforeAcceptingNewUserInput)
			{
				return;
			}
			this.ladderClimbMode = false;
			this.ncm.enabled = true;
			if (!AnimationHandler.instance.insuredMode)
			{
				this.ncm.canJump = true;
			}
			this.animHandler.isInInteraction = false;
			base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			base.GetComponent<Animation>()["Interaction-Pole-Drop"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Pole-Drop"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().CrossFade("Interaction-Pole-Drop");
			this.basicAgility.FarisHead.PlayOneShot(this.bodyLadderClimbSound, SpeechManager.sfxVolume);
			this.lastUserInputTime = Time.time;
			this.basicAgility.lastUserInputTime = Time.time;
			Instructions.instruction = Instructions.Instruction.NONE;
		}
		else if (this.VerticalAxis > 0.1f)
		{
			if ((this.movingDown && base.transform.position.y < this.startPosition.position.y) || (!this.movingDown && base.transform.position.y < this.endPosition.position.y))
			{
				if (!base.GetComponent<Animation>().IsPlaying("Interaction-Ladder-Move"))
				{
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].time = 0f;
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].layer = 2;
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].speed = 1.5f;
					base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Move");
					iTween.MoveBy(base.gameObject, iTween.Hash(new object[]
					{
						"time",
						(base.GetComponent<Animation>()["Interaction-Ladder-Move"].length - 0.1f) / 1.5f,
						"amount",
						new Vector3(0f, 0.4f, 0f),
						"easeType",
						iTween.EaseType.linear
					}));
				}
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds("Interaction-Ladder-Move");
			}
			else
			{
				base.GetComponent<Animation>()["Interaction-Ladder-Exit-Top"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Ladder-Exit-Top"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>()["Interaction-Ladder-Exit-Top"].speed = 1f;
				base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Exit-Top");
				this.endingInteractionTimer = base.GetComponent<Animation>()["Interaction-Ladder-Exit-Top"].length;
				this.endingInteraction = true;
				this.ladderClimbMode = false;
				this.basicAgility.FarisHead.PlayOneShot(this.bodyLadderClimbSound, SpeechManager.sfxVolume);
			}
		}
		else if (this.VerticalAxis < -0.1f)
		{
			if ((this.movingDown && base.transform.position.y > this.buttomEndPosition.position.y) || (!this.movingDown && base.transform.position.y > this.buttomEndPosition.position.y))
			{
				if (!base.GetComponent<Animation>().IsPlaying("Interaction-Ladder-Move"))
				{
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].time = base.GetComponent<Animation>()["Interaction-Ladder-Move"].length;
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].layer = 2;
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>()["Interaction-Ladder-Move"].speed = -1.5f;
					base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Move");
					iTween.MoveBy(base.gameObject, iTween.Hash(new object[]
					{
						"time",
						(base.GetComponent<Animation>()["Interaction-Ladder-Move"].length - 0.1f) / 1.5f,
						"amount",
						new Vector3(0f, -0.4f, 0f),
						"easeType",
						iTween.EaseType.linear
					}));
				}
				this.PlayLedgeMoveSounds();
				this.TriggerInteractionHandSounds("Interaction-Ladder-Move");
			}
			else if (this.currentInteractionTrigger.laderExitButtom)
			{
				MonoBehaviour.print("set hieght");
				base.transform.position = new Vector3(base.transform.position.x, this.currentInteractionTrigger.transform.Find("StartPosition").transform.position.y, base.transform.position.z);
				base.GetComponent<Animation>()["Interaction-Ladder-Exit-Bottom"].layer = 1;
				base.GetComponent<Animation>()["Interaction-Ladder-Exit-Bottom"].wrapMode = WrapMode.ClampForever;
				base.GetComponent<Animation>()["Interaction-Ladder-Exit-Bottom"].speed = 1f;
				base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Exit-Bottom");
				this.endingInteractionTimer = base.GetComponent<Animation>()["Interaction-Ladder-Exit-Bottom"].length;
				this.endingInteraction = true;
				this.ladderClimbMode = false;
				this.basicAgility.FarisHead.PlayOneShot(this.bodyLadderClimbSound, SpeechManager.sfxVolume);
			}
			else
			{
				this.ladderClimbMode = false;
				this.animHandler.isInInteraction = false;
				this.basicAgility.ledge = this.currentInteractionTrigger.connectingLedge;
				this.basicAgility.ledgeHanging = true;
			}
		}
		else
		{
			base.GetComponent<Animation>()["Interaction-Ladder-Idle"].layer = 1;
			base.GetComponent<Animation>()["Interaction-Ladder-Idle"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().CrossFade("Interaction-Ladder-Idle");
			this.TriggerInteractionHandSounds("Interaction-Ladder-Move");
		}
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x00061A4C File Offset: 0x0005FC4C
	private IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time, float[][] curve)
	{
		float i = 0f;
		float rate = 1f / time;
		while ((double)i <= 1.0)
		{
			i += Time.deltaTime * rate;
			float[] curveKey = Interaction.GetDisplacement(i, curve);
			thisTransform.position = new Vector3(Mathf.Lerp(startPos.x, endPos.x, curveKey[1]), Mathf.Lerp(startPos.y, endPos.y, curveKey[0]), Mathf.Lerp(startPos.z, endPos.z, curveKey[1]));
			yield return 0;
		}
		yield break;
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00061AAC File Offset: 0x0005FCAC
	private static float[] GetDisplacement(float t, float[][] curve)
	{
		foreach (float[] array in curve)
		{
			if (t <= array[0])
			{
				return new float[]
				{
					Mathf.Lerp(0f, array[1], t / array[0]),
					Mathf.Lerp(0f, array[2], t / array[0])
				};
			}
		}
		return new float[]
		{
			1f,
			1f
		};
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00061B24 File Offset: 0x0005FD24
	public void EnterPeekingMode()
	{
		Vector3 camOffset = this.cam.camOffset;
		Vector3 to = new Vector3(0.6f, 0.1f, -0.9f);
		float num = 2f;
		for (float num2 = 0f; num2 < num; num2 += Time.deltaTime)
		{
			this.cam.camOffset = Vector3.Lerp(camOffset, to, num2 / num);
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00061B88 File Offset: 0x0005FD88
	private void EnterInversedPeekingMode()
	{
		Vector3 camOffset = this.cam.camOffset;
		Vector3 to = new Vector3(-0.6f, 0.1f, -0.9f);
		float num = 2f;
		for (float num2 = 0f; num2 < num; num2 += Time.deltaTime)
		{
			this.cam.camOffset = Vector3.Lerp(camOffset, to, num2 / num);
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00061BEC File Offset: 0x0005FDEC
	private void ExitPeekingMode()
	{
		Vector3 camOffset = this.cam.camOffset;
		Vector3 to = new Vector3(0f, 0f, -4.5f);
		float num = 2f;
		for (float num2 = 0f; num2 < num; num2 += Time.deltaTime)
		{
			this.cam.camOffset = Vector3.Lerp(camOffset, to, num2 / num);
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00061C50 File Offset: 0x0005FE50
	private void RootMotionShortCover()
	{
		if (this.movingRightDirection)
		{
			base.transform.Translate(1f, 0f, 1f);
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
		}
		else
		{
			base.transform.Translate(-1f, 0f, 1f);
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
		}
		if (this.cover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
		{
			this.coverShortMode = false;
			base.GetComponent<Animation>()["General-Idle"].layer = 0;
			this.ncm.disableMovement = false;
			this.ncm.disableRotation = false;
			if (!AnimationHandler.instance.insuredMode)
			{
				this.ncm.canJump = true;
			}
			this.animHandler.isInInteraction = false;
			AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
			WeaponHandling.holdFire = false;
			if (this.weaponHandler != null)
			{
				this.weaponHandler.inCover = false;
			}
			if (this.movingRightDirection)
			{
				base.GetComponent<Animation>()["Cover-Short-Exit-Left"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Exit-Left"].speed = -1f;
				base.GetComponent<Animation>()["Cover-Short-Exit-Left"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Exit-Left");
			}
			else
			{
				base.GetComponent<Animation>()["Cover-Short-Exit-Right"].layer = 1;
				base.GetComponent<Animation>()["Cover-Short-Exit-Right"].speed = -1f;
				base.GetComponent<Animation>()["Cover-Short-Exit-Right"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().CrossFade("Cover-Short-Exit-Right");
			}
			this.basicAgility.FarisHead.PlayOneShot(this.bodyCoverEnterSound, SpeechManager.sfxVolume);
			if (this.path == string.Empty)
			{
				this.startPosition = this.cover.transform.Find("StartPosition").transform;
				this.endPosition = this.cover.transform.Find("EndPosition").transform;
				this.path = string.Empty;
			}
			else
			{
				this.path = this.cover.path;
				this.lookAtPoint = this.cover.transform.position;
			}
			this.EnterCoverTallMode = true;
			this.lastUserInputTime = Time.time;
			this.interactionType = this.cover.TriggerType;
			Instructions.instruction = Instructions.Instruction.NONE;
			this.movingRightDirection = !this.movingRightDirection;
			MobileInput.instance.enableButton("cover", this.cover.gameObject);
		}
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00061FC8 File Offset: 0x000601C8
	private void RootMotionLongCover()
	{
		if (this.movingRightDirection)
		{
			base.transform.Translate(-0.55f * base.transform.localScale.x, 0f, -0.55f * base.transform.localScale.z);
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
		}
		else
		{
			base.transform.Translate(0.55f * base.transform.localScale.x, 0f, -0.55f * base.transform.localScale.z);
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
		}
		this.playerYRotation = base.transform.eulerAngles.y;
		if (this.cover.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
		{
			this.ExitCoverTallMode();
			if (this.path == string.Empty)
			{
				this.startPosition = this.cover.transform.Find("StartPosition").transform;
				this.endPosition = this.cover.transform.Find("EndPosition").transform;
				this.path = string.Empty;
			}
			else
			{
				this.path = this.cover.path;
				this.lookAtPoint = this.cover.transform.position;
			}
			this.EnterCoverShortMode = true;
			this.lastUserInputTime = Time.time;
			this.interactionType = this.cover.TriggerType;
			Instructions.instruction = Instructions.Instruction.NONE;
		}
		else
		{
			this.movingRightDirection = !this.movingRightDirection;
		}
		MobileInput.instance.enableButton("cover", this.cover.gameObject);
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00062258 File Offset: 0x00060458
	private void RootMotionToCoverRun()
	{
		if (this.movingFromTallCover)
		{
			if (!this.movingRightDirection)
			{
				base.transform.Translate(-0.55f, 0f, 0f);
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
			}
			else
			{
				base.transform.Translate(0.55f, 0f, 0f);
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
			}
		}
		else if (!this.movingRightDirection)
		{
			base.transform.Translate(2.55f, 0f, 0f);
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
		}
		else
		{
			base.transform.Translate(-2.55f, 0f, 0f);
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
		}
		base.GetComponent<Animation>()["Cover-Run"].layer = 2;
		base.GetComponent<Animation>()["Cover-Run"].speed = 1f;
		base.GetComponent<Animation>()["Cover-Run"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Cover-Run");
		if (Vector3.Distance(base.transform.position, this.startPosition.position) < Vector3.Distance(base.transform.position, this.endPosition.position))
		{
			this.coverPosition = this.startPosition.position;
		}
		else
		{
			this.coverPosition = this.endPosition.position;
		}
		this.moveToCover = true;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00062570 File Offset: 0x00060770
	private void MoveToCover()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, this.coverPosition, Time.deltaTime / (Vector3.Distance(this.coverPosition, base.transform.position) / 8f));
		if (this.cover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
		{
			if (Vector3.Distance(base.transform.position, this.coverPosition) < 1f)
			{
				if (!this.movingRightDirection)
				{
					base.transform.Translate(0f, 0f, 1f);
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
					base.GetComponent<Animation>()["Cover-Run-to-Right-Arrive"].layer = 2;
					base.GetComponent<Animation>()["Cover-Run-to-Right-Arrive"].speed = 1f;
					base.GetComponent<Animation>()["Cover-Run-to-Right-Arrive"].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>().Play("Cover-Run-to-Right-Arrive");
				}
				else
				{
					base.transform.Translate(0f, 0f, 1f);
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
					base.GetComponent<Animation>()["Cover-Run-to-Left-Arrive"].layer = 2;
					base.GetComponent<Animation>()["Cover-Run-to-Left-Arrive"].speed = 1f;
					base.GetComponent<Animation>()["Cover-Run-to-Left-Arrive"].wrapMode = WrapMode.Once;
					base.GetComponent<Animation>().Play("Cover-Run-to-Left-Arrive");
				}
				this.moveToCover = false;
				base.GetComponent<Animation>()["Cover-Run"].layer = 0;
				base.Invoke("EnterCoverTall", base.GetComponent<Animation>()["Cover-Run-to-Right-Arrive"].length);
			}
		}
		else if (Vector3.Distance(base.transform.position, this.coverPosition) < 2.55f)
		{
			if (!this.movingRightDirection)
			{
				base.transform.Translate(0f, 0f, 2.7f);
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 90f, base.transform.rotation.eulerAngles.z);
				base.GetComponent<Animation>()["Cover-Run-Short-to-Right-Arrrival"].layer = 2;
				base.GetComponent<Animation>()["Cover-Run-Short-to-Right-Arrrival"].speed = 1f;
				base.GetComponent<Animation>()["Cover-Run-Short-to-Right-Arrrival"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().Play("Cover-Run-Short-to-Right-Arrrival");
			}
			else
			{
				base.transform.Translate(0f, 0f, 2.7f);
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y + 90f, base.transform.rotation.eulerAngles.z);
				base.GetComponent<Animation>()["Cover-Run-Short-to-Left-Arrival"].layer = 2;
				base.GetComponent<Animation>()["Cover-Run-Short-to-Left-Arrival"].speed = 1f;
				base.GetComponent<Animation>()["Cover-Run-Short-to-Left-Arrival"].wrapMode = WrapMode.Once;
				base.GetComponent<Animation>().Play("Cover-Run-Short-to-Left-Arrival");
			}
			this.moveToCover = false;
			base.GetComponent<Animation>()["Cover-Run"].layer = 0;
			base.Invoke("EnterCoverShort", base.GetComponent<Animation>()["Cover-Run-Short-to-Right-Arrrival"].length);
		}
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00062A24 File Offset: 0x00060C24
	private void EnterCoverTall()
	{
		if (this.path == string.Empty)
		{
			this.startPosition = this.cover.transform.Find("StartPosition").transform;
			this.endPosition = this.cover.transform.Find("EndPosition").transform;
			this.path = string.Empty;
		}
		else
		{
			this.path = this.cover.path;
			this.lookAtPoint = this.cover.transform.position;
		}
		this.EnterCoverTallMode = true;
		this.lastUserInputTime = Time.time;
		this.interactionType = this.cover.TriggerType;
		Instructions.instruction = Instructions.Instruction.NONE;
		this.movingRightDirection = !this.movingRightDirection;
		this.coverTallMode = true;
		this.EnterCoverTallMode = false;
		this.ncm.disableMovement = true;
		this.ncm.disableRotation = true;
		this.ncm.canJump = false;
		this.animHandler.isInInteraction = true;
		WeaponHandling.holdFire = true;
		AnimationHandler.instance.faceState = AnimationHandler.FaceState.COVER;
		if (this.path == string.Empty)
		{
			Transform transform = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
			transform.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
			base.transform.position = transform.position;
			base.transform.rotation = transform.rotation;
			this.playerYRotation = base.transform.eulerAngles.y;
		}
		else
		{
			float num = 100000f;
			for (float num2 = 0f; num2 <= 1f; num2 += 0.1f)
			{
				Vector3 b = iTween.PointOnPath(iTweenPath.GetPath(this.path), num2);
				b.y = base.transform.position.y;
				if (Vector3.Distance(base.transform.position, b) < num)
				{
					num = Vector3.Distance(base.transform.position, b);
					this.closestPointPercentage = num2;
				}
			}
			Vector3 position = iTween.PointOnPath(iTweenPath.GetPath(this.path), this.closestPointPercentage);
			position.y = base.transform.position.y;
			base.transform.position = position;
			this.lookAtPoint.y = base.transform.position.y;
			base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.lookAtPoint);
			this.playerYRotation = base.transform.eulerAngles.y;
		}
		this.basicAgility.FarisHead.PlayOneShot(this.bodyCoverEnterSound, SpeechManager.sfxVolume);
		if (this.weaponHandler != null)
		{
			this.weaponHandler.inCover = true;
			this.weaponHandler.tallCover = true;
			if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
			{
				this.weaponHandler.EnterIdleFree();
			}
			this.weaponHandler.disableUsingWeapons = false;
			WeaponHandling.additiveAimingSetup = false;
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00062D9C File Offset: 0x00060F9C
	private void EnterCoverShort()
	{
		if (this.path == string.Empty)
		{
			this.startPosition = this.cover.transform.Find("StartPosition").transform;
			this.endPosition = this.cover.transform.Find("EndPosition").transform;
			this.path = string.Empty;
		}
		else
		{
			this.path = this.cover.path;
			this.lookAtPoint = this.cover.transform.position;
		}
		this.EnterCoverShortMode = true;
		this.lastUserInputTime = Time.time;
		this.interactionType = this.cover.TriggerType;
		Instructions.instruction = Instructions.Instruction.NONE;
		this.coverShortMode = true;
		this.EnterCoverShortMode = false;
		this.ncm.disableMovement = true;
		this.ncm.disableRotation = true;
		this.ncm.canJump = false;
		this.animHandler.isInInteraction = true;
		WeaponHandling.holdFire = true;
		AnimationHandler.instance.faceState = AnimationHandler.FaceState.COVER;
		if (this.path == string.Empty)
		{
			Transform transform = UnityEngine.Object.Instantiate(this.startPosition, this.startPosition.position, this.startPosition.rotation) as Transform;
			transform.Translate(this.startPosition.InverseTransformPoint(base.transform.position).x * this.startPosition.parent.localScale.x, 0f, 0f);
			base.transform.rotation = transform.rotation;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		else
		{
			float num = 100000f;
			for (float num2 = 0f; num2 <= 1f; num2 += 0.1f)
			{
				Vector3 b = iTween.PointOnPath(iTweenPath.GetPath(this.path), num2);
				b.y = base.transform.position.y;
				if (Vector3.Distance(base.transform.position, b) < num)
				{
					num = Vector3.Distance(base.transform.position, b);
					this.closestPointPercentage = num2;
				}
			}
			Vector3 position = iTween.PointOnPath(iTweenPath.GetPath(this.path), this.closestPointPercentage);
			position.y = base.transform.position.y;
			base.transform.position = position;
			this.lookAtPoint.y = base.transform.position.y;
			base.transform.rotation = Quaternion.LookRotation(this.lookAtPoint - base.transform.position);
		}
		if (this.weaponHandler != null)
		{
			this.weaponHandler.inCover = true;
			this.weaponHandler.tallCover = false;
			if (this.weaponHandler.status != WeaponHandling.WeaponStatus.RELAXED)
			{
				this.weaponHandler.EnterIdleFree();
			}
			this.weaponHandler.disableUsingWeapons = false;
			WeaponHandling.additiveAimingSetup = false;
		}
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x000630B0 File Offset: 0x000612B0
	public void OnGUI()
	{
		if (mainmenu.pause || (SpeechManager.instance != null && SpeechManager.instance.subtitleString != null && SpeechManager.instance.subtitleString != string.Empty))
		{
			return;
		}
		if (this.spaceToMoveCover)
		{
			Texture texture;
			if (AndroidPlatform.IsJoystickConnected())
			{
				texture = this.moveCoverPS3;
			}
			else
			{
				texture = this.moveCoverMobile;
			}
			if (texture != null)
			{
				if (Screen.width > 1500)
				{
				}
				if (AndroidPlatform.IsJoystickConnected())
				{
					float num = 90f;
					GUI.DrawTexture(new Rect((float)(Screen.width / 2) - num / 2f, (float)(Screen.height / 2) - num / 2f + (float)(Screen.height * 3 / 8), 2f * num, num), texture, ScaleMode.StretchToFill);
				}
			}
		}
		else if (this.spaceToCornerCover)
		{
			Texture texture2;
			if (AndroidPlatform.IsJoystickConnected())
			{
				texture2 = this.cornerCoverPS3;
			}
			else
			{
				texture2 = this.cornerCoverMobile;
			}
			if (texture2 != null)
			{
				float num2 = 128f;
				if (Screen.width > 1500)
				{
					num2 = 256f;
				}
				if (AndroidPlatform.IsJoystickConnected())
				{
					num2 = 90f;
					GUI.DrawTexture(new Rect((float)(Screen.width / 2) - num2 / 2f, (float)(Screen.height / 2) - num2 / 2f + (float)(Screen.height * 3 / 8), 2f * num2, num2), texture2, ScaleMode.StretchToFill);
				}
				else
				{
					GUI.DrawTexture(new Rect((float)(Screen.width / 2) - num2 / 2f, (float)(Screen.height / 2) - num2 / 2f + (float)(Screen.height * 3 / 8), num2, num2), texture2, ScaleMode.StretchToFill);
				}
				int touchCount = Input.touchCount;
				for (int i = 0; i < touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					Rect rect = new Rect((float)(Screen.width / 2) - num2 / 2f, (float)Screen.height - ((float)(Screen.height / 2) + num2 / 2f + (float)(Screen.height * 3 / 8)), num2, num2);
					if (rect.Contains(new Vector2(touch.position.x, touch.position.y)) && Time.time > this.lastCoverSwitchAction + 2f)
					{
						this.coverSwitchAction = true;
						this.lastCoverSwitchAction = Time.time;
					}
				}
			}
		}
	}

	// Token: 0x04000E24 RID: 3620
	[HideInInspector]
	public bool poleClimbMode;

	// Token: 0x04000E25 RID: 3621
	[HideInInspector]
	public bool EnterPoleMode;

	// Token: 0x04000E26 RID: 3622
	[HideInInspector]
	public bool logMode;

	// Token: 0x04000E27 RID: 3623
	[HideInInspector]
	public bool EnterLogMode;

	// Token: 0x04000E28 RID: 3624
	[HideInInspector]
	public bool ropeClimbMode;

	// Token: 0x04000E29 RID: 3625
	[HideInInspector]
	public bool EnterRopeMode;

	// Token: 0x04000E2A RID: 3626
	[HideInInspector]
	public bool ladderClimbMode;

	// Token: 0x04000E2B RID: 3627
	[HideInInspector]
	public bool EnterLadderMode;

	// Token: 0x04000E2C RID: 3628
	[HideInInspector]
	public bool wallBackMode;

	// Token: 0x04000E2D RID: 3629
	[HideInInspector]
	public bool EnterWallBackMode;

	// Token: 0x04000E2E RID: 3630
	[HideInInspector]
	public bool coverTallMode;

	// Token: 0x04000E2F RID: 3631
	[HideInInspector]
	public bool EnterCoverTallMode;

	// Token: 0x04000E30 RID: 3632
	[HideInInspector]
	public bool coverShortMode;

	// Token: 0x04000E31 RID: 3633
	[HideInInspector]
	public bool EnterCoverShortMode;

	// Token: 0x04000E32 RID: 3634
	[HideInInspector]
	public bool pushingMode;

	// Token: 0x04000E33 RID: 3635
	[HideInInspector]
	public bool EnterPushingMode;

	// Token: 0x04000E34 RID: 3636
	[HideInInspector]
	public GameObject pushableObject;

	// Token: 0x04000E35 RID: 3637
	[HideInInspector]
	public bool kickingMode;

	// Token: 0x04000E36 RID: 3638
	[HideInInspector]
	public bool EnterKickingMode;

	// Token: 0x04000E37 RID: 3639
	[HideInInspector]
	public Transform startPosition;

	// Token: 0x04000E38 RID: 3640
	[HideInInspector]
	public Transform endPosition;

	// Token: 0x04000E39 RID: 3641
	[HideInInspector]
	public Transform buttomEndPosition;

	// Token: 0x04000E3A RID: 3642
	[HideInInspector]
	public Transform topStartPosition;

	// Token: 0x04000E3B RID: 3643
	[HideInInspector]
	public string path;

	// Token: 0x04000E3C RID: 3644
	[HideInInspector]
	public float closestPointPercentage;

	// Token: 0x04000E3D RID: 3645
	[HideInInspector]
	public Vector3 lookAtPoint;

	// Token: 0x04000E3E RID: 3646
	private bool recalculatrUserInput = true;

	// Token: 0x04000E3F RID: 3647
	[HideInInspector]
	public BasicAgility basicAgility;

	// Token: 0x04000E40 RID: 3648
	[HideInInspector]
	public AnimationHandler animHandler;

	// Token: 0x04000E41 RID: 3649
	[HideInInspector]
	public NormalCharacterMotor ncm;

	// Token: 0x04000E42 RID: 3650
	[HideInInspector]
	public PlatformCharacterController pcc;

	// Token: 0x04000E43 RID: 3651
	[HideInInspector]
	public CharacterController cc;

	// Token: 0x04000E44 RID: 3652
	public AudioClip bodyCoverEnterSound;

	// Token: 0x04000E45 RID: 3653
	public AudioClip bodyPoleClimbSound;

	// Token: 0x04000E46 RID: 3654
	public AudioClip bodyRopeClimbSound;

	// Token: 0x04000E47 RID: 3655
	public AudioClip bodyLadderClimbSound;

	// Token: 0x04000E48 RID: 3656
	public AudioClip FarisLogEnterSound;

	// Token: 0x04000E49 RID: 3657
	public AudioClip FarisLogBalanceSound;

	// Token: 0x04000E4A RID: 3658
	public AudioClip FarisLogBalanceSound2;

	// Token: 0x04000E4B RID: 3659
	public AudioClip FarisDontLookDownSound;

	// Token: 0x04000E4C RID: 3660
	public AudioClip FarisPoleAlmostThereSound;

	// Token: 0x04000E4D RID: 3661
	public AudioClip FarisLogAlmostThereSound;

	// Token: 0x04000E4E RID: 3662
	public AudioClip FarisLogEnterSoundArabic;

	// Token: 0x04000E4F RID: 3663
	public AudioClip FarisDontLookDownSoundArabic;

	// Token: 0x04000E50 RID: 3664
	public AudioClip FarisPoleAlmostThereSoundArabic;

	// Token: 0x04000E51 RID: 3665
	public AudioClip FarisLogAlmostThereSoundArabic;

	// Token: 0x04000E52 RID: 3666
	private bool DontLookDownPlayed;

	// Token: 0x04000E53 RID: 3667
	private bool almostTherePlayed;

	// Token: 0x04000E54 RID: 3668
	private float ropeEnterTimer;

	// Token: 0x04000E55 RID: 3669
	private bool endingInteraction;

	// Token: 0x04000E56 RID: 3670
	private float endingInteractionTimer;

	// Token: 0x04000E57 RID: 3671
	public bool movingRightDirection = true;

	// Token: 0x04000E58 RID: 3672
	private float turnTimer;

	// Token: 0x04000E59 RID: 3673
	[HideInInspector]
	public float lastUserInputTime;

	// Token: 0x04000E5A RID: 3674
	private float timeBeforeAcceptingNewUserInput = 0.5f;

	// Token: 0x04000E5B RID: 3675
	private bool movingDown;

	// Token: 0x04000E5C RID: 3676
	private bool animating;

	// Token: 0x04000E5D RID: 3677
	private float animatingTimer;

	// Token: 0x04000E5E RID: 3678
	private float ladderEnterTimer;

	// Token: 0x04000E5F RID: 3679
	[HideInInspector]
	public WeaponHandling weaponHandler;

	// Token: 0x04000E60 RID: 3680
	[HideInInspector]
	public InteractionTrigger.InteractionTypes interactionType;

	// Token: 0x04000E61 RID: 3681
	private float blindFireStartTime;

	// Token: 0x04000E62 RID: 3682
	private float lastFallAtemptTime;

	// Token: 0x04000E63 RID: 3683
	private bool fallingRightDirection;

	// Token: 0x04000E64 RID: 3684
	private float timeBeforeAtemptingToFall = 3f;

	// Token: 0x04000E65 RID: 3685
	private float safeTime;

	// Token: 0x04000E66 RID: 3686
	private bool aim;

	// Token: 0x04000E67 RID: 3687
	private bool fire;

	// Token: 0x04000E68 RID: 3688
	private bool coverButton;

	// Token: 0x04000E69 RID: 3689
	private bool exitInteractionButton;

	// Token: 0x04000E6A RID: 3690
	private bool jumpButton;

	// Token: 0x04000E6B RID: 3691
	private float HorizontalAxis;

	// Token: 0x04000E6C RID: 3692
	private float VerticalAxis;

	// Token: 0x04000E6D RID: 3693
	private bool emmittingStepSound;

	// Token: 0x04000E6E RID: 3694
	private bool falledFromLog;

	// Token: 0x04000E6F RID: 3695
	public AudioClip[] FarisInteractionMoveSounds;

	// Token: 0x04000E70 RID: 3696
	private int currentInteractionMoveSound;

	// Token: 0x04000E71 RID: 3697
	public AudioClip[] FarisInteractionHandSounds;

	// Token: 0x04000E72 RID: 3698
	private int currentInteractionHandSound;

	// Token: 0x04000E73 RID: 3699
	private float pushableObjectFirstHeight;

	// Token: 0x04000E74 RID: 3700
	public AudioClip farisPushSound;

	// Token: 0x04000E75 RID: 3701
	public AudioClip dragSound;

	// Token: 0x04000E76 RID: 3702
	public AudioClip farisKickSound;

	// Token: 0x04000E77 RID: 3703
	public AudioClip breakSound;

	// Token: 0x04000E78 RID: 3704
	public AudioClip farisEnterPoleSound;

	// Token: 0x04000E79 RID: 3705
	public AudioClip farisExitPoleSound;

	// Token: 0x04000E7A RID: 3706
	private bool rigidbodyAdded;

	// Token: 0x04000E7B RID: 3707
	private string kickAnim;

	// Token: 0x04000E7C RID: 3708
	private ShooterGameCamera cam;

	// Token: 0x04000E7D RID: 3709
	private float playerYRotation;

	// Token: 0x04000E7E RID: 3710
	private bool breakingSoundPlayed;

	// Token: 0x04000E7F RID: 3711
	private bool peeking;

	// Token: 0x04000E80 RID: 3712
	public InteractionTrigger cover;

	// Token: 0x04000E81 RID: 3713
	private bool rootMotionShortCover;

	// Token: 0x04000E82 RID: 3714
	private bool rootMotionLongCover;

	// Token: 0x04000E83 RID: 3715
	private bool rootMotionTOCoverRun;

	// Token: 0x04000E84 RID: 3716
	private float rootMotionTimer;

	// Token: 0x04000E85 RID: 3717
	private bool moveToCover;

	// Token: 0x04000E86 RID: 3718
	private Vector3 coverPosition;

	// Token: 0x04000E87 RID: 3719
	private bool movingFromTallCover;

	// Token: 0x04000E88 RID: 3720
	private bool blindFire;

	// Token: 0x04000E89 RID: 3721
	private bool blindFireFirstShot;

	// Token: 0x04000E8A RID: 3722
	public static Interaction playerInteraction;

	// Token: 0x04000E8B RID: 3723
	public InteractionTrigger currentInteractionTrigger;

	// Token: 0x04000E8C RID: 3724
	private bool spaceToMoveCover;

	// Token: 0x04000E8D RID: 3725
	private bool spaceToCornerCover;

	// Token: 0x04000E8E RID: 3726
	public Texture moveCoverPC;

	// Token: 0x04000E8F RID: 3727
	public Texture moveCoverPS3;

	// Token: 0x04000E90 RID: 3728
	public Texture moveCoverXBox;

	// Token: 0x04000E91 RID: 3729
	public Texture moveCoverMobile;

	// Token: 0x04000E92 RID: 3730
	public Texture cornerCoverPC;

	// Token: 0x04000E93 RID: 3731
	public Texture cornerCoverPS3;

	// Token: 0x04000E94 RID: 3732
	public Texture cornerCoverXBox;

	// Token: 0x04000E95 RID: 3733
	public Texture cornerCoverMobile;

	// Token: 0x04000E96 RID: 3734
	private bool coverSwitchAction;

	// Token: 0x04000E97 RID: 3735
	private float lastCoverSwitchAction;

	// Token: 0x04000E98 RID: 3736
	private float targetPoleY;

	// Token: 0x04000E99 RID: 3737
	private float REFY;

	// Token: 0x04000E9A RID: 3738
	private float ClimbPoleOverTime;

	// Token: 0x04000E9B RID: 3739
	private float coverOffset = 0.6f;

	// Token: 0x04000E9C RID: 3740
	private bool inversedAimingBeforeEnteringCover;

	// Token: 0x04000E9D RID: 3741
	private int mask;

	// Token: 0x04000E9E RID: 3742
	private float lastSwitchTime;

	// Token: 0x04000E9F RID: 3743
	private float lastDust;

	// Token: 0x04000EA0 RID: 3744
	private float lastLedgeSoundPlayTime;

	// Token: 0x04000EA1 RID: 3745
	public static bool preventExitingCover;
}
