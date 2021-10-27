using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class AnimationHandler : MonoBehaviour
{
	// Token: 0x0600094E RID: 2382 RVA: 0x0004E9D0 File Offset: 0x0004CBD0
	private void Start()
	{
		this.animState = AnimationHandler.AnimStates.IDLE;
		this.previousAnimState = this.animState;
		this.cc = base.GetComponent<CharacterController>();
		this.currentWaitingAnimation = 1;
		this.idleTimer = 5f;
		this.basicAgility = base.gameObject.GetComponent<BasicAgility>();
		this.weaponHandler = base.gameObject.GetComponent<WeaponHandling>();
		if (this.head == null)
		{
			this.head = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
		}
		if (this.upperBody == null)
		{
			this.upperBody = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2");
		}
		if (this.rightFoot == null)
		{
			this.rightFoot = base.transform.Find("rightFoot");
		}
		if (this.leftFoot == null)
		{
			this.leftFoot = base.transform.Find("leftFoot");
		}
		if (base.GetComponent<Animation>()[this.fallMediumAnimation.name] != null)
		{
			base.GetComponent<Animation>()[this.fallMediumAnimation.name].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()[this.fallMediumAnimation.name].layer = 10;
		}
		if (base.GetComponent<Animation>()[this.fallLowAnimation.name] != null)
		{
			base.GetComponent<Animation>()[this.fallLowAnimation.name].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()[this.fallLowAnimation.name].layer = 10;
		}
		if (base.GetComponent<Animation>()[this.landMediumAnimation.name] != null)
		{
			base.GetComponent<Animation>()[this.landMediumAnimation.name].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[this.landMediumAnimation.name].layer = 10;
		}
		if (base.GetComponent<Animation>()[this.landLowAnimation.name] != null)
		{
			base.GetComponent<Animation>()[this.landLowAnimation.name].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[this.landLowAnimation.name].layer = 10;
		}
		if (base.GetComponent<Animation>()["Agility-Jump-Forward-Fall"] != null)
		{
			base.GetComponent<Animation>()["Agility-Jump-Forward-Fall"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>()["Agility-Jump-Forward-Fall"].layer = 10;
		}
		if (base.GetComponent<Animation>()["Agility-Jump-Forward-Land"] != null)
		{
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land"].layer = 100;
		}
		if (base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Roll"] != null)
		{
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Roll"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Roll"].layer = 100;
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Roll"].speed = 1.7f;
		}
		if (base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Run"] != null)
		{
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Run"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Run"].layer = 100;
		}
		if (base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Trip-Run"] != null)
		{
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Trip-Run"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()["Agility-Jump-Forward-Land-Trip-Run"].layer = 100;
		}
		if (base.GetComponent<Animation>()["Shooting-Walk-Forward"] != null)
		{
			base.GetComponent<Animation>()["Shooting-Walk-Forward"].wrapMode = WrapMode.Loop;
		}
		if (base.GetComponent<Animation>()["Shooting-Walk-Back"] != null)
		{
			base.GetComponent<Animation>()["Shooting-Walk-Back"].wrapMode = WrapMode.Loop;
		}
		if (base.GetComponent<Animation>()["Shooting-Strafe-Right"] != null)
		{
			base.GetComponent<Animation>()["Shooting-Strafe-Right"].wrapMode = WrapMode.Loop;
		}
		if (base.GetComponent<Animation>()["Shooting-Strafe-Left"] != null)
		{
			base.GetComponent<Animation>()["Shooting-Strafe-Left"].wrapMode = WrapMode.Loop;
		}
		if (base.GetComponent<Animation>()["Shooting-Strafe-Right"] != null)
		{
			base.GetComponent<Animation>()["Shooting-Strafe-Right"].speed = 1.5f;
		}
		if (base.GetComponent<Animation>()["Shooting-Strafe-Left"] != null)
		{
			base.GetComponent<Animation>()["Shooting-Strafe-Left"].speed = 1.5f;
		}
		if (!this.disableFacialAnims)
		{
			if (base.GetComponent<Animation>()["Facial"] != null)
			{
				base.GetComponent<Animation>()["Facial"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Facial"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Faris-Pain-Facial"] != null)
			{
				base.GetComponent<Animation>()["Faris-Pain-Facial"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Faris-Pain-Facial"].layer = 2;
			}
			if (base.GetComponent<Animation>()["General-Facial-Pain"] != null)
			{
				base.GetComponent<Animation>()["General-Facial-Pain"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["General-Facial-Pain"].layer = 20;
			}
			if (base.GetComponent<Animation>()["Face-Agility"] != null)
			{
				base.GetComponent<Animation>()["Face-Agility"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Agility"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-Cover"] != null)
			{
				base.GetComponent<Animation>()["Face-Cover"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Cover"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-Dead"] != null)
			{
				base.GetComponent<Animation>()["Face-Dead"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Dead"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-Falling"] != null)
			{
				base.GetComponent<Animation>()["Face-Falling"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Falling"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-Interaction"] != null)
			{
				base.GetComponent<Animation>()["Face-Interaction"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Interaction"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-LookAt"] != null)
			{
				base.GetComponent<Animation>()["Face-LookAt"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-LookAt"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-Takedown"] != null)
			{
				base.GetComponent<Animation>()["Face-Takedown"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Takedown"].layer = 2;
			}
			if (base.GetComponent<Animation>()["Face-Weapon"] != null)
			{
				base.GetComponent<Animation>()["Face-Weapon"].AddMixingTransform(this.head);
				base.GetComponent<Animation>()["Face-Weapon"].layer = 2;
			}
		}
		this.mask = 1 << base.gameObject.layer;
		this.mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.mask = ~this.mask;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0004F290 File Offset: 0x0004D490
	private void Awake()
	{
		AnimationHandler.instance = this;
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0004F298 File Offset: 0x0004D498
	private void OnDestroy()
	{
		AnimationHandler.instance = null;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0004F2A0 File Offset: 0x0004D4A0
	public void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.previousStealthMode != this.stealthMode)
		{
			if (!this.stealthMode)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("MusicPlayer");
				if (gameObject != null)
				{
					gameObject.BroadcastMessage("StealthEnded", SendMessageOptions.DontRequireReceiver);
				}
			}
			this.previousStealthMode = this.stealthMode;
		}
		if (this.holdingTorch && !base.GetComponent<Animation>().IsPlaying("General-Torch-Holding"))
		{
			base.GetComponent<Animation>().CrossFade("General-Torch-Holding", 0.1f);
		}
		if (this.landSteppingSoundsTimer > 0f)
		{
			this.landSteppingSoundsTimer -= Time.deltaTime;
			this.TriggerLandingSteepsSound(this.landSteppingSoundAnim);
		}
		this.HorizontalAxis = PlatformCharacterController.joystickLeft.position.x + Input.GetAxisRaw("Horizontal");
		this.VerticalAxis = PlatformCharacterController.joystickLeft.position.y + Input.GetAxisRaw("Vertical");
		if (this.isholdingWeapon)
		{
			if (this.faceState == AnimationHandler.FaceState.NEUTRAL)
			{
				this.faceState = AnimationHandler.FaceState.WEAPON;
			}
		}
		else if (this.faceState == AnimationHandler.FaceState.WEAPON)
		{
			this.faceState = AnimationHandler.FaceState.NEUTRAL;
		}
		if (this.basicAgility.animating || this.isInInteraction || this.basicAgility.ledgeHanging || this.basicAgility.longJumping)
		{
			if (!this.insuredMode && this.weaponHandler != null && !this.weaponHandler.inCover)
			{
				this.weaponHandler.disableUsingWeapons = true;
			}
			this.remainInIdle = 5;
			this.PlayNeutralFace();
			AnimationHandler.dontRotateCamera = true;
			return;
		}
		if (!this.insuredMode && !SpeechManager.letterBox && this.weaponHandler != null)
		{
			this.weaponHandler.disableUsingWeapons = false;
		}
		AnimationHandler.dontRotateCamera = false;
		if (this.cc.isGrounded || this.animState == AnimationHandler.AnimStates.JUMPING)
		{
			if (this.previousAnimState == AnimationHandler.AnimStates.FALLING)
			{
				base.GetComponent<Animation>().Stop(this.fallMediumAnimation.name);
				base.GetComponent<Animation>().Stop(this.fallLowAnimation.name);
				base.GetComponent<Animation>().Stop("Agility-Jump-Forward-Fall");
				WeaponHandling.additiveAimingSetup = false;
				if (this.fallingFromJump)
				{
					if (AnimationHandler.forceRoleFromFall)
					{
						if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Land-Roll"))
						{
							base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Land-Roll");
							base.gameObject.GetComponent<NormalCharacterMotor>().Impulse = 10f * base.transform.forward;
							this.landSteppingSoundAnim = "Agility-Jump-Forward-Land-Roll";
							this.landSteppingSoundsTimer = base.GetComponent<Animation>()[this.landSteppingSoundAnim].length;
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
							AnimationHandler.forceRoleFromFall = false;
						}
					}
					else
					{
						float num = Vector3.Dot(Camera.main.transform.right, base.transform.forward) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.forward) * this.VerticalAxis;
						if (num <= 0f)
						{
							if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Land"))
							{
								base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Land");
								base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
								this.faceState = AnimationHandler.FaceState.NEUTRAL;
							}
						}
						else if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Land-Trip-Run"))
						{
							base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Land-Trip-Run");
							base.gameObject.GetComponent<NormalCharacterMotor>().Impulse = 5f * base.transform.forward;
							this.landSteppingSoundAnim = "Agility-Jump-Forward-Land-Trip-Run";
							this.landSteppingSoundsTimer = base.GetComponent<Animation>()[this.landSteppingSoundAnim].length;
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
						}
					}
					this.fallingFromJump = false;
					switch (this.fallingState)
					{
					case AnimationHandler.FallingTypes.NON:
					case AnimationHandler.FallingTypes.LOW:
						this.basicAgility.FarisHead.PlayOneShot(this.BodyFallLowSound, SpeechManager.sfxVolume);
						break;
					case AnimationHandler.FallingTypes.MEDIUM:
						this.basicAgility.FarisHead.PlayOneShot(this.BodyFallMediumSound, SpeechManager.sfxVolume);
						if (!AnimationHandler.noDeathFallZone)
						{
							base.transform.GetComponent<Health>().health -= 25f;
						}
						else
						{
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
						}
						break;
					case AnimationHandler.FallingTypes.HIGH:
						this.basicAgility.FarisHead.PlayOneShot(this.BodyFallMediumSound, SpeechManager.sfxVolume);
						if (!AnimationHandler.noDeathFallZone)
						{
							base.transform.GetComponent<Health>().health = 0f;
						}
						else
						{
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
						}
						break;
					}
				}
				else
				{
					switch (this.fallingState)
					{
					case AnimationHandler.FallingTypes.LOW:
						if (!base.GetComponent<Animation>().IsPlaying(this.landLowAnimation.name))
						{
							base.GetComponent<Animation>().Play(this.landLowAnimation.name);
							base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
						}
						this.basicAgility.FarisHead.PlayOneShot(this.BodyFallMediumSound, SpeechManager.sfxVolume);
						break;
					case AnimationHandler.FallingTypes.MEDIUM:
						if (AnimationHandler.forceRoleFromFall)
						{
							if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Land-Roll"))
							{
								base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Land-Roll");
								base.gameObject.GetComponent<NormalCharacterMotor>().Impulse = 10f * base.transform.forward;
								this.landSteppingSoundAnim = "Agility-Jump-Forward-Land-Roll";
								this.landSteppingSoundsTimer = base.GetComponent<Animation>()[this.landSteppingSoundAnim].length;
								this.faceState = AnimationHandler.FaceState.NEUTRAL;
								AnimationHandler.forceRoleFromFall = false;
							}
						}
						else
						{
							float num = Vector3.Dot(Camera.main.transform.right, base.transform.forward) * this.HorizontalAxis + Vector3.Dot(Camera.main.transform.forward, base.transform.forward) * this.VerticalAxis;
							if (num <= 0f)
							{
								if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Land"))
								{
									base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Land");
									this.landSteppingSoundAnim = "Agility-Jump-Forward-Land";
									this.landSteppingSoundsTimer = base.GetComponent<Animation>()[this.landSteppingSoundAnim].length;
									this.faceState = AnimationHandler.FaceState.NEUTRAL;
								}
							}
							else if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Land-Trip-Run"))
							{
								base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Land-Trip-Run");
								base.gameObject.GetComponent<NormalCharacterMotor>().Impulse = 5f * base.transform.forward;
								this.landSteppingSoundAnim = "Agility-Jump-Forward-Land-Trip-Run";
								this.landSteppingSoundsTimer = base.GetComponent<Animation>()[this.landSteppingSoundAnim].length;
								this.faceState = AnimationHandler.FaceState.NEUTRAL;
							}
						}
						this.basicAgility.FarisHead.PlayOneShot(this.FarisFallMediumSound, SpeechManager.speechVolume);
						this.basicAgility.FarisHead.PlayOneShot(this.BodyFallMediumSound, SpeechManager.sfxVolume);
						PadVibrator.VibrateInterval(true, 0.3f, 0.3f);
						if (!AnimationHandler.noDeathFallZone)
						{
							base.transform.GetComponent<Health>().health -= 25f;
						}
						else
						{
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
						}
						break;
					case AnimationHandler.FallingTypes.HIGH:
						if (!base.GetComponent<Animation>().IsPlaying(this.landMediumAnimation.name))
						{
							base.GetComponent<Animation>().CrossFade(this.landMediumAnimation.name);
						}
						if (!AnimationHandler.noDeathFallZone)
						{
							this.faceState = AnimationHandler.FaceState.DEAD;
							base.transform.GetComponent<Health>().health = 0f;
						}
						else
						{
							this.faceState = AnimationHandler.FaceState.NEUTRAL;
						}
						break;
					}
				}
			}
			if (this.animState != AnimationHandler.AnimStates.JUMPING)
			{
				if (this.faceState == AnimationHandler.FaceState.FALLING)
				{
					this.faceState = AnimationHandler.FaceState.NEUTRAL;
				}
				if (this.remainInIdle <= 0)
				{
					Vector3 normalized = new Vector3(PlatformCharacterController.joystickLeft.position.x + Input.GetAxis("Horizontal"), PlatformCharacterController.joystickLeft.position.y + Input.GetAxis("Vertical"), 0f);
					if (normalized.magnitude > 1f)
					{
						normalized = normalized.normalized;
					}
					float magnitude = this.cc.velocity.magnitude;
					if (magnitude <= 0f || this.forceIdleState)
					{
						this.animState = AnimationHandler.AnimStates.IDLE;
					}
					else if (this.isShooting || magnitude < this.maxWalkSpeed || Input.GetKey(KeyCode.LeftControl) || normalized.magnitude < 0.7f || AnimationHandler.forcedWalk)
					{
						this.animState = AnimationHandler.AnimStates.WALK;
					}
					else
					{
						this.animState = AnimationHandler.AnimStates.RUN;
					}
				}
				else
				{
					this.remainInIdle--;
				}
			}
		}
		else if (!Physics.Raycast(base.transform.position, -base.transform.up, 0.6f * this.scaleFactor))
		{
			this.animState = AnimationHandler.AnimStates.FALLING;
		}
		switch (this.animState)
		{
		case AnimationHandler.AnimStates.IDLE:
			if (!this.isInInteraction && !this.isInAgility)
			{
				this.Idle();
			}
			break;
		case AnimationHandler.AnimStates.WALK:
			this.Walk();
			break;
		case AnimationHandler.AnimStates.RUN:
			this.Run();
			break;
		case AnimationHandler.AnimStates.FALLING:
			this.Fall();
			break;
		case AnimationHandler.AnimStates.JUMPING:
			this.Jump();
			break;
		}
		this.PlayNeutralFace();
		this.previousAnimState = this.animState;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0004FCB0 File Offset: 0x0004DEB0
	private void PlayNeutralFace()
	{
		if (this.disableFacialAnims)
		{
			return;
		}
		string text;
		if (this.insuredMode)
		{
			text = "Faris-Pain-Facial";
		}
		else
		{
			text = "Facial";
		}
		if (this.previousFaceState != this.faceState)
		{
			switch (this.previousFaceState)
			{
			case AnimationHandler.FaceState.NEUTRAL:
				if (this.insuredMode)
				{
					text = "General-Facial-Pain";
				}
				else
				{
					text = "Facial";
				}
				break;
			case AnimationHandler.FaceState.AGILITY:
				text = "Face-Agility";
				break;
			case AnimationHandler.FaceState.COVER:
				text = "Face-Cover";
				break;
			case AnimationHandler.FaceState.DEAD:
				text = "Face-Dead";
				break;
			case AnimationHandler.FaceState.FALLING:
				text = "Face-Falling";
				break;
			case AnimationHandler.FaceState.INTERACTION:
				text = "Face-Interaction";
				break;
			case AnimationHandler.FaceState.LOOKAT:
				text = "Face-LookAt";
				break;
			case AnimationHandler.FaceState.TAKEDOWN:
				text = "Face-Takedown";
				break;
			case AnimationHandler.FaceState.WEAPON:
				if (this.insuredMode)
				{
					text = "General-Facial-Pain";
				}
				else
				{
					text = "Face-Weapon";
				}
				break;
			}
			base.GetComponent<Animation>().Stop(text);
			this.previousFaceState = this.faceState;
		}
		switch (this.faceState)
		{
		case AnimationHandler.FaceState.NEUTRAL:
			if (this.insuredMode)
			{
				text = "General-Facial-Pain";
			}
			else
			{
				text = "Facial";
			}
			break;
		case AnimationHandler.FaceState.AGILITY:
			text = "Face-Agility";
			break;
		case AnimationHandler.FaceState.COVER:
			text = "Face-Cover";
			break;
		case AnimationHandler.FaceState.DEAD:
			text = "Face-Dead";
			break;
		case AnimationHandler.FaceState.FALLING:
			text = "Face-Falling";
			break;
		case AnimationHandler.FaceState.INTERACTION:
			text = "Face-Interaction";
			break;
		case AnimationHandler.FaceState.LOOKAT:
			text = "Face-LookAt";
			break;
		case AnimationHandler.FaceState.TAKEDOWN:
			text = "Face-Takedown";
			break;
		case AnimationHandler.FaceState.WEAPON:
			if (this.insuredMode)
			{
				text = "General-Facial-Pain";
			}
			else
			{
				text = "Face-Weapon";
			}
			break;
		}
		if (!base.GetComponent<Animation>().IsPlaying(text))
		{
			base.GetComponent<Animation>().Blend(text, 1f, 0.3f);
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0004FED0 File Offset: 0x0004E0D0
	private void Idle()
	{
		this.stopToRun = 0.5f;
		if (this.insuredMode)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Injured-Idle"))
			{
				base.GetComponent<Animation>().CrossFade("Injured-Idle");
				this.walkingCounter = 0f;
				this.tripTimer = 0f;
			}
			return;
		}
		if (this.stealthMode && !this.isholdingWeapon)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Stealth-Idle"))
			{
				base.GetComponent<Animation>().CrossFade("Stealth-Idle");
			}
			return;
		}
		if (this.engagedMode && !this.isholdingWeapon)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Engaged-Idle"))
			{
				base.GetComponent<Animation>().CrossFade("Engaged-Idle");
			}
			return;
		}
		if (SpeechManager.instance != null && SpeechManager.instance.playing)
		{
			if (!base.GetComponent<Animation>().IsPlaying(this.waitingAnimations[0].name))
			{
				base.GetComponent<Animation>().CrossFade(this.waitingAnimations[0].name);
			}
			return;
		}
		this.idleTimer -= Time.deltaTime;
		if (this.holdIdleAnimations > 0f)
		{
			this.holdIdleAnimations -= Time.deltaTime;
		}
		if (this.holdIdleAnimations > 0f || this.stopIdleAnimations || this.isholdingWeapon || this.isLookingAtSomthing || (this.idleTimer <= 8.3f && this.idleTimer > 0.3f))
		{
			if (!base.GetComponent<Animation>().IsPlaying(this.waitingAnimations[0].name))
			{
				base.GetComponent<Animation>().CrossFade(this.waitingAnimations[0].name);
			}
		}
		else if (this.idleTimer <= 0.3f)
		{
			if (this.currentRandomAnimation != this.waitingAnimations.Length - 1)
			{
				this.currentRandomAnimation++;
			}
			else
			{
				this.currentRandomAnimation = 1;
			}
			this.currentWaitingAnimation = this.currentRandomAnimation;
			if (!base.GetComponent<Animation>().IsPlaying(this.waitingAnimations[this.currentWaitingAnimation].name))
			{
				base.GetComponent<Animation>().CrossFade(this.waitingAnimations[this.currentWaitingAnimation].name);
				this.idleTimer = this.waitingAnimations[this.currentWaitingAnimation].length + 8f;
			}
		}
		else if (!base.GetComponent<Animation>().IsPlaying(this.waitingAnimations[this.currentWaitingAnimation].name))
		{
			base.GetComponent<Animation>().CrossFade(this.waitingAnimations[0].name);
			this.idleTimer = 5f;
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x000501B0 File Offset: 0x0004E3B0
	private void Walk()
	{
		this.stopToRun = 0.5f;
		if (this.insuredMode)
		{
			if (!this.isShooting)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Injured-Walk"))
				{
					base.GetComponent<Animation>().CrossFade("Injured-Walk");
				}
				float magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["Injured-Walk"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f);
				this.TriggerSteepsSound("Injured-Walk");
			}
			else if (this.HorizontalAxis > 0f)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Right"))
				{
					base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Right");
				}
				float magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["Shooting-Strafe-Right"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f);
			}
			else if (this.HorizontalAxis < 0f)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Left"))
				{
					base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Left");
				}
				float magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["Shooting-Strafe-Left"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f);
			}
			else if (this.VerticalAxis > 0f)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Injured-Walk"))
				{
					base.GetComponent<Animation>().CrossFade("Injured-Walk");
				}
				float magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["Injured-Walk"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f);
			}
			else if (this.VerticalAxis < 0f)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Shooting-Walk-Back"))
				{
					base.GetComponent<Animation>().CrossFade("Shooting-Walk-Back");
				}
				float magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["Shooting-Walk-Back"].speed = 1f * Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f);
			}
			return;
		}
		if ((this.engagedMode || this.stealthMode) && !this.isholdingWeapon)
		{
			this.Run();
			return;
		}
		if (!this.isShooting)
		{
			if (!base.GetComponent<Animation>().IsPlaying("General-Walk"))
			{
				base.GetComponent<Animation>().CrossFade("General-Walk");
			}
			float magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["General-Walk_0"].speed = 1f;
			this.TriggerSteepsSound("General-Walk_0");
		}
		else if (this.HorizontalAxis > 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Right"))
			{
				base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Right");
			}
			float magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["Shooting-Strafe-Right"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f) * 1.2f;
		}
		else if (this.HorizontalAxis < 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Left"))
			{
				base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Left");
			}
			float magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["Shooting-Strafe-Left"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f) * 1.2f;
		}
		else if (this.VerticalAxis > 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Shooting-Walk-Forward"))
			{
				base.GetComponent<Animation>().CrossFade("Shooting-Walk-Forward");
			}
			float magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["Shooting-Walk-Forward"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f) * 1.2f;
		}
		else if (this.VerticalAxis < 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Shooting-Walk-Back"))
			{
				base.GetComponent<Animation>().CrossFade("Shooting-Walk-Back");
			}
			float magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["Shooting-Walk-Back"].speed = Mathf.Clamp01(magnitude / this.maxWalkSpeed + 0.4f) * 1.2f;
		}
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x000506B4 File Offset: 0x0004E8B4
	private void Run()
	{
		float magnitude;
		if (this.stealthMode && !this.isholdingWeapon)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Stealth-Run"))
			{
				base.GetComponent<Animation>().CrossFade("Stealth-Run");
			}
			magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["Stealth-Run"].speed = Mathf.Clamp01(magnitude / 3f - 0.1f);
			this.TriggerSteepsSound("Stealth-Run");
			return;
		}
		if (this.engagedMode && !this.isholdingWeapon)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Cover-Run"))
			{
				base.GetComponent<Animation>().CrossFade("Cover-Run");
			}
			magnitude = this.cc.velocity.magnitude;
			base.GetComponent<Animation>()["Cover-Run"].speed = Mathf.Clamp01(magnitude / 3f - 0.1f);
			this.TriggerSteepsSound("Cover-Run");
			return;
		}
		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton3)) && !this.isholdingWeapon && (Application.loadedLevelName == "part1" || Application.loadedLevelName == "part2" || Application.loadedLevelName == "Survival_Temple" || Application.loadedLevelName == "Survival_Prologue" || Application.loadedLevelName == "Survival_Desert" || Application.loadedLevelName == "Survival_TangierCity" || Application.loadedLevelName == "Survival_TangierCafe" || Application.loadedLevelName == "Survival_TangierMarket" || Application.loadedLevelName == "Survival_NightJungle") && !this.stealthMode)
		{
			if (!this.holdingTorch)
			{
				if (!base.GetComponent<Animation>().IsPlaying("Sprint"))
				{
					base.GetComponent<Animation>().CrossFade("Sprint");
				}
				magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["Sprint"].speed = 0.95f;
				this.TriggerSteepsSound("Sprint");
			}
			else
			{
				if (!base.GetComponent<Animation>().IsPlaying("General-Run"))
				{
					base.GetComponent<Animation>().CrossFade("General-Run");
				}
				magnitude = this.cc.velocity.magnitude;
				base.GetComponent<Animation>()["General-Run"].speed = 0.95f;
				this.TriggerSteepsSound("General-Run");
			}
			return;
		}
		if (!base.GetComponent<Animation>().IsPlaying("General-Run"))
		{
			base.GetComponent<Animation>().CrossFade("General-Run");
		}
		magnitude = this.cc.velocity.magnitude;
		base.GetComponent<Animation>()["General-Run"].speed = 1.1f * ((0.5f - this.stopToRun) / 0.5f);
		if (this.stopToRun > 0f)
		{
			this.stopToRun = Mathf.Clamp(this.stopToRun - Time.deltaTime, 0f, 0.5f);
		}
		this.TriggerSteepsSound("General-Run");
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00050A20 File Offset: 0x0004EC20
	private void TriggerSteepsSound(string anim)
	{
		float num = base.GetComponent<Animation>()[anim].normalizedTime - Mathf.Floor(base.GetComponent<Animation>()[anim].normalizedTime);
		if (num > this.WalkStep1StrikeTime && num < this.WalkStep1StrikeTime + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				if (anim == "Stealth-Run")
				{
					base.SendMessage("OnFootStrikeVol", 0.06f, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
				}
			}
			this.emmittingStepSound = true;
		}
		else if (num > this.WalkStep2StrikeTime && num < this.WalkStep2StrikeTime + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				if (anim == "Stealth-Run")
				{
					base.SendMessage("OnFootStrikeVol", 0.06f, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
				}
			}
			this.emmittingStepSound = true;
		}
		else
		{
			this.emmittingStepSound = false;
		}
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x00050B38 File Offset: 0x0004ED38
	private void TriggerLandingSteepsSound(string anim)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float normalizedTime = base.GetComponent<Animation>()[anim].normalizedTime;
		switch (anim)
		{
		case "Agility-Jump-Forward-Land":
			num = 0f;
			num2 = 0.2f;
			break;
		case "Agility-Jump-Forward-Land-Roll":
			num = 0f;
			num2 = 0.2f;
			break;
		case "Agility-Jump-Forward-Land-Run":
			num = 0f;
			num2 = 0.21f;
			break;
		case "Agility-Jump-Forward-Trip-Roll":
			num = 0f;
			num2 = 0.09f;
			num3 = 0.44f;
			num4 = 0.75f;
			break;
		}
		if (normalizedTime > num && normalizedTime < num + 0.03f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (normalizedTime > num2 && normalizedTime < num2 + 0.03f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num3 != 0f && normalizedTime > num3 && normalizedTime < num3 + 0.03f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (num4 != 0f && normalizedTime > num4 && normalizedTime < num4 + 0.03f)
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

	// Token: 0x06000958 RID: 2392 RVA: 0x00050D4C File Offset: 0x0004EF4C
	private bool ApplyLeaning()
	{
		float num = this.HorizontalAxis;
		if (Vector3.Angle(base.transform.forward, Camera.main.transform.forward) > 90f)
		{
			num = 0f;
		}
		if ((double)num > 0.3)
		{
			if (!base.GetComponent<Animation>().IsPlaying("General-Run-Lean-Right"))
			{
				base.GetComponent<Animation>().CrossFade("General-Run-Lean-Right");
			}
			this.TriggerSteepsSound("General-Run-Lean-Right");
			return true;
		}
		if ((double)num < -0.3)
		{
			if (!base.GetComponent<Animation>().IsPlaying("General-Run-Lean-Left"))
			{
				base.GetComponent<Animation>().CrossFade("General-Run-Lean-Left");
			}
			this.TriggerSteepsSound("General-Run-Lean-Left");
			return true;
		}
		base.GetComponent<Animation>().Stop("General-Run-Lean-Right");
		base.GetComponent<Animation>().Stop("General-Run-Lean-Left");
		return false;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x00050E34 File Offset: 0x0004F034
	private void Fall()
	{
		if (this.previousAnimState != AnimationHandler.AnimStates.FALLING)
		{
			this.fallingState = AnimationHandler.FallingTypes.NON;
			if (this.previousAnimState != AnimationHandler.AnimStates.JUMPING)
			{
				this.fallingStartHieght = base.transform.position.y;
			}
		}
		float num = this.fallingStartHieght - base.transform.position.y;
		if (this.previousAnimState == AnimationHandler.AnimStates.JUMPING || this.fallingState == AnimationHandler.FallingTypes.FROMJUMP)
		{
			this.fallingFromJump = true;
		}
		if (num >= this.HighFallDistance || this.fallingState == AnimationHandler.FallingTypes.HIGH)
		{
			this.fallingState = AnimationHandler.FallingTypes.HIGH;
		}
		else if (num >= this.MediumFallDistance || this.fallingState == AnimationHandler.FallingTypes.MEDIUM)
		{
			this.fallingState = AnimationHandler.FallingTypes.MEDIUM;
		}
		else if (num >= this.lowFallDistance || this.fallingState == AnimationHandler.FallingTypes.LOW)
		{
			this.fallingState = AnimationHandler.FallingTypes.LOW;
		}
		if (this.fallingFromJump)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Agility-Jump-Forward-Fall"))
			{
				base.GetComponent<Animation>().CrossFade("Agility-Jump-Forward-Fall");
			}
		}
		else
		{
			switch (this.fallingState)
			{
			case AnimationHandler.FallingTypes.LOW:
				if (!base.GetComponent<Animation>().IsPlaying(this.fallLowAnimation.name))
				{
					base.GetComponent<Animation>().CrossFade(this.fallLowAnimation.name, 0.3f, PlayMode.StopAll);
					this.faceState = AnimationHandler.FaceState.FALLING;
				}
				break;
			case AnimationHandler.FallingTypes.MEDIUM:
				if (!base.GetComponent<Animation>().IsPlaying(this.fallMediumAnimation.name))
				{
					base.GetComponent<Animation>().CrossFade(this.fallMediumAnimation.name, 0.3f, PlayMode.StopAll);
					this.faceState = AnimationHandler.FaceState.FALLING;
				}
				break;
			case AnimationHandler.FallingTypes.HIGH:
				if (!base.GetComponent<Animation>().IsPlaying(this.fallMediumAnimation.name))
				{
					base.GetComponent<Animation>().CrossFade(this.fallMediumAnimation.name, 0.3f, PlayMode.StopAll);
					this.faceState = AnimationHandler.FaceState.FALLING;
				}
				break;
			}
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00051040 File Offset: 0x0004F240
	private void Jump()
	{
		if (!base.GetComponent<Animation>().IsPlaying("Jump-Short-Start"))
		{
			this.animState = AnimationHandler.AnimStates.FALLING;
			this.fallingState = AnimationHandler.FallingTypes.FROMJUMP;
			this.fallingFromJump = true;
			this.fallingStartHieght = base.transform.position.y;
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00051090 File Offset: 0x0004F290
	private void UnloadUnuesdAssets()
	{
	}

	// Token: 0x04000CD4 RID: 3284
	public AnimationHandler.AnimStates animState;

	// Token: 0x04000CD5 RID: 3285
	private AnimationHandler.AnimStates previousAnimState;

	// Token: 0x04000CD6 RID: 3286
	public AnimationHandler.FaceState faceState;

	// Token: 0x04000CD7 RID: 3287
	private AnimationHandler.FaceState previousFaceState;

	// Token: 0x04000CD8 RID: 3288
	private float runTimer;

	// Token: 0x04000CD9 RID: 3289
	public Transform leanMixingTransform;

	// Token: 0x04000CDA RID: 3290
	public AnimationClip[] waitingAnimations;

	// Token: 0x04000CDB RID: 3291
	public AnimationClip fallLowAnimation;

	// Token: 0x04000CDC RID: 3292
	public AnimationClip landLowAnimation;

	// Token: 0x04000CDD RID: 3293
	public AnimationClip fallMediumAnimation;

	// Token: 0x04000CDE RID: 3294
	public AnimationClip landMediumAnimation;

	// Token: 0x04000CDF RID: 3295
	public float lowFallDistance = 0.2f;

	// Token: 0x04000CE0 RID: 3296
	public float MediumFallDistance = 2f;

	// Token: 0x04000CE1 RID: 3297
	public float HighFallDistance = 5f;

	// Token: 0x04000CE2 RID: 3298
	public Transform head;

	// Token: 0x04000CE3 RID: 3299
	public Transform upperBody;

	// Token: 0x04000CE4 RID: 3300
	public AudioClip BodyFallLowSound;

	// Token: 0x04000CE5 RID: 3301
	public AudioClip FarisFallMediumSound;

	// Token: 0x04000CE6 RID: 3302
	public AudioClip BodyFallMediumSound;

	// Token: 0x04000CE7 RID: 3303
	public bool stopIdleAnimations;

	// Token: 0x04000CE8 RID: 3304
	public bool isInInteraction;

	// Token: 0x04000CE9 RID: 3305
	public bool isInAgility;

	// Token: 0x04000CEA RID: 3306
	public bool isholdingWeapon;

	// Token: 0x04000CEB RID: 3307
	public bool isShooting;

	// Token: 0x04000CEC RID: 3308
	public bool isLookingAtSomthing;

	// Token: 0x04000CED RID: 3309
	public CharacterController cc;

	// Token: 0x04000CEE RID: 3310
	public Transform rightFoot;

	// Token: 0x04000CEF RID: 3311
	public Transform leftFoot;

	// Token: 0x04000CF0 RID: 3312
	public bool stealthMode;

	// Token: 0x04000CF1 RID: 3313
	public bool engagedMode;

	// Token: 0x04000CF2 RID: 3314
	private bool previousStealthMode;

	// Token: 0x04000CF3 RID: 3315
	public bool insuredMode;

	// Token: 0x04000CF4 RID: 3316
	public bool holdingTorch;

	// Token: 0x04000CF5 RID: 3317
	private LayerMask mask;

	// Token: 0x04000CF6 RID: 3318
	private float idleTimer;

	// Token: 0x04000CF7 RID: 3319
	private int currentWaitingAnimation;

	// Token: 0x04000CF8 RID: 3320
	private int currentRandomAnimation;

	// Token: 0x04000CF9 RID: 3321
	private BasicAgility basicAgility;

	// Token: 0x04000CFA RID: 3322
	private AnimationHandler.FallingTypes fallingState;

	// Token: 0x04000CFB RID: 3323
	private float WalkStep1StrikeTime = 0.1f;

	// Token: 0x04000CFC RID: 3324
	private float WalkStep2StrikeTime = 0.6f;

	// Token: 0x04000CFD RID: 3325
	private bool emmittingStepSound;

	// Token: 0x04000CFE RID: 3326
	private float maxWalkSpeed = 2.9f;

	// Token: 0x04000CFF RID: 3327
	private float checkLegsTimer;

	// Token: 0x04000D00 RID: 3328
	private int remainInIdle;

	// Token: 0x04000D01 RID: 3329
	private int currentLandAnimation;

	// Token: 0x04000D02 RID: 3330
	private WeaponHandling weaponHandler;

	// Token: 0x04000D03 RID: 3331
	private float HorizontalAxis;

	// Token: 0x04000D04 RID: 3332
	private float VerticalAxis;

	// Token: 0x04000D05 RID: 3333
	private bool fallingFromJump;

	// Token: 0x04000D06 RID: 3334
	[HideInInspector]
	public float holdIdleAnimations;

	// Token: 0x04000D07 RID: 3335
	public static AnimationHandler instance;

	// Token: 0x04000D08 RID: 3336
	private string landSteppingSoundAnim;

	// Token: 0x04000D09 RID: 3337
	private float landSteppingSoundsTimer;

	// Token: 0x04000D0A RID: 3338
	private float fallingStartHieght;

	// Token: 0x04000D0B RID: 3339
	public Transform LowerBodyTransform1;

	// Token: 0x04000D0C RID: 3340
	public Transform LowerBodyTransform2;

	// Token: 0x04000D0D RID: 3341
	private float walkingCounter;

	// Token: 0x04000D0E RID: 3342
	private float tripTimer;

	// Token: 0x04000D0F RID: 3343
	private bool firstTrip = true;

	// Token: 0x04000D10 RID: 3344
	public static bool dontRotateCamera;

	// Token: 0x04000D11 RID: 3345
	public static bool forceRoleFromFall;

	// Token: 0x04000D12 RID: 3346
	public static bool noDeathFallZone;

	// Token: 0x04000D13 RID: 3347
	public bool forceIdleState;

	// Token: 0x04000D14 RID: 3348
	public Transform dustParticles;

	// Token: 0x04000D15 RID: 3349
	public static bool noTakedownZone;

	// Token: 0x04000D16 RID: 3350
	private float stopToRun;

	// Token: 0x04000D17 RID: 3351
	public float scaleFactor = 1f;

	// Token: 0x04000D18 RID: 3352
	public bool disableFacialAnims;

	// Token: 0x04000D19 RID: 3353
	public static bool forcedWalk;

	// Token: 0x020001D8 RID: 472
	public enum AnimStates
	{
		// Token: 0x04000D1C RID: 3356
		IDLE,
		// Token: 0x04000D1D RID: 3357
		WALK,
		// Token: 0x04000D1E RID: 3358
		RUN,
		// Token: 0x04000D1F RID: 3359
		FALLING,
		// Token: 0x04000D20 RID: 3360
		JUMPING
	}

	// Token: 0x020001D9 RID: 473
	public enum FallingTypes
	{
		// Token: 0x04000D22 RID: 3362
		NON,
		// Token: 0x04000D23 RID: 3363
		LOW,
		// Token: 0x04000D24 RID: 3364
		MEDIUM,
		// Token: 0x04000D25 RID: 3365
		HIGH,
		// Token: 0x04000D26 RID: 3366
		FROMJUMP
	}

	// Token: 0x020001DA RID: 474
	public enum FaceState
	{
		// Token: 0x04000D28 RID: 3368
		NEUTRAL,
		// Token: 0x04000D29 RID: 3369
		AGILITY,
		// Token: 0x04000D2A RID: 3370
		COVER,
		// Token: 0x04000D2B RID: 3371
		DEAD,
		// Token: 0x04000D2C RID: 3372
		FALLING,
		// Token: 0x04000D2D RID: 3373
		INTERACTION,
		// Token: 0x04000D2E RID: 3374
		LOOKAT,
		// Token: 0x04000D2F RID: 3375
		TAKEDOWN,
		// Token: 0x04000D30 RID: 3376
		WEAPON
	}
}
