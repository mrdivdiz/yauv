using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class BotAI : MonoBehaviour
{
	// Token: 0x0600059D RID: 1437 RVA: 0x00023504 File Offset: 0x00021704
	private void Start()
	{
		foreach (Transform transform in this.patrolPath)
		{
			if (transform != null)
			{
				transform.parent = null;
			}
		}
		this.startingYPosition = base.transform.position.y;
		CapsuleCollider[] componentsInChildren = base.transform.GetComponentsInChildren<CapsuleCollider>();
		//foreach (CapsuleCollider capsuleCollider in componentsInChildren)
		//{
			//capsuleCollider.isTrigger = true;
		//}
		if (base.GetComponent<Animation>()["Grunts-Facial-Angry"] != null)
		{
			if (this.head == null)
			{
				this.head = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
			}
			base.GetComponent<Animation>()["Grunts-Facial-Angry"].AddMixingTransform(this.head);
			base.GetComponent<Animation>()["Grunts-Facial-Angry"].layer = 2;
			base.GetComponent<Animation>().CrossFade("Grunts-Facial-Angry");
		}
		if (this.headLookPosition == null)
		{
			this.headLookPosition = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head/HeadLookPosition");
		}
		if (this.headLookPosition == null)
		{
			this.headLookPosition = base.transform;
		}
		BotAI.lastGrenadeShootTime = Time.time;
		this.spawnPosition = base.transform.position;
		this.animState = BotAI.AnimStates.IDLE;
		this.previousAnimState = this.animState;
		if (BotAI.randomAnimState == -1)
		{
			BotAI.randomAnimState = (this.currentAnimState = UnityEngine.Random.Range(0, (!this.priorityToCover) ? 6 : 5));
		}
		else
		{
			BotAI.randomAnimState = (BotAI.randomAnimState + 1) % ((!this.priorityToCover) ? 6 : 5);
			this.currentAnimState = BotAI.randomAnimState;
		}
		this.cc = base.gameObject.GetComponent<CharacterController>();
		if (this.target == null)
		{
			this.target = GameObject.FindGameObjectWithTag("Player");
			this.target = this.target.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1").gameObject;
		}
		base.GetComponent<Animation>()["General-Walk"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["General-Run"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["General-Idle"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Engaged2-Idle"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Shooting-Walk-Forward"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Shooting-Walk-Back"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Shooting-Strafe-Right"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Shooting-Strafe-Left"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Shooting-Strafe-Right"].speed = 1f;
		base.GetComponent<Animation>()["Shooting-Strafe-Left"].speed = 1f;
		base.GetComponent<Animation>()["General-Walk"].speed = 0.7f;
		base.GetComponent<Animation>()["General-Land-Medium"].layer = 2;
		base.GetComponent<Animation>()["General-Land-Medium"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].layer = 100;
		if (this.weaponPrefab != null)
		{
			this.EquipWeapon();
		}
		if (this.grenadePrefab != null)
		{
			Vector3 position = this.leftHand.position;
			position.y += 0.5f;
			Transform transform2 = (Transform)UnityEngine.Object.Instantiate(this.grenadePrefab.transform, position, base.transform.rotation);
			transform2.parent = this.leftHand;
			this.grenade = transform2.GetComponent<Gun>();
			this.grenade.weaponHolder = base.gameObject.GetComponent<WeaponHandling>();
		}
		if (this.groupID != "NONE")
		{
			if (BotAI.lastSeenPositions == null)
			{
				BotAI.lastSeenPositions = new Hashtable();
			}
			if (!BotAI.lastSeenPositions.ContainsKey(this.groupID))
			{
				BotAI.lastSeenPositions.Add(this.groupID, Vector3.zero);
			}
		}
		else
		{
			this.lastSeenPosition = Vector3.zero;
		}
		this.coverMask = 1 << LayerMask.NameToLayer("Cover");
		this.visualMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.visualMask |= 1 << LayerMask.NameToLayer("Enemy");
		this.visualMask = ~this.visualMask;
		if (this.EntryAnim != null)
		{
			this.startingYPosition = base.transform.position.y;
			if (!base.GetComponent<Animation>().IsPlaying(this.EntryAnim.name))
			{
				base.GetComponent<Animation>().CrossFade(this.EntryAnim.name);
			}
			this.entered = true;
			this.entryDuration = base.GetComponent<Animation>()[this.EntryAnim.name].length;
		}
		if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
		{
			this.SetupAdditiveAiming("Handgun-Aiming-Left");
			this.SetupAdditiveAiming("Handgun-Aiming-Right");
		}
		else
		{
			this.SetupAdditiveAiming("AK47-Aiming-Left");
			this.SetupAdditiveAiming("AK47-Aiming-Right");
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00023ACC File Offset: 0x00021CCC
	private void Awake()
	{
		if (this.groupID != "NONE" && BotAI.lastSeenPositions != null && BotAI.lastSeenPositions.ContainsKey(this.groupID))
		{
			BotAI.lastSeenPositions[this.groupID] = Vector3.zero;
		}
		this.lastSeenPosition = Vector3.zero;
		BotAI.lastRelaxedSoundTime = Time.time;
		if (base.transform.parent != null)
		{
			base.transform.parent = null;
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00023B60 File Offset: 0x00021D60
	private void OnDestroy()
	{
		for (int i = 0; i < this.enemyRelaxedSounds.Length; i++)
		{
			this.enemyRelaxedSounds[i] = null;
		}
		for (int j = 0; j < this.enemyEngagedSounds.Length; j++)
		{
			this.enemyEngagedSounds[j] = null;
		}
		for (int k = 0; k < this.enemyTakingCoverSounds.Length; k++)
		{
			this.enemyTakingCoverSounds[k] = null;
		}
		for (int l = 0; l < this.enemyJustSawSounds.Length; l++)
		{
			this.enemyJustSawSounds[l] = null;
		}
		this.enemyRelaxedSounds = new AudioClip[0];
		this.enemyEngagedSounds = new AudioClip[0];
		this.enemyTakingCoverSounds = new AudioClip[0];
		this.enemyJustSawSounds = new AudioClip[0];
		this.weaponPrefab = null;
		this.grenadePrefab = null;
		this.rightHandSecondary = null;
		this.rightHandPrimary = null;
		this.leftHand = null;
		this.aimPivot = null;
		this.weapon = null;
		this.enemyRelaxedSounds = null;
		this.enemyEngagedSounds = null;
		this.enemyTakingCoverSounds = null;
		this.enemyJustSawSounds = null;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00023C74 File Offset: 0x00021E74
	private void Destroy2()
	{
		Application.LoadLevel("Egypt-Cutscene1");
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00023C80 File Offset: 0x00021E80
	private void OnEnable()
	{
		this.SetupAdditiveAiming("Handgun-Aiming-Up");
		this.SetupAdditiveAiming("Handgun-Aiming-Down");
		this.SetupAdditiveAiming("Shooting-AK47-Aim-Up");
		this.SetupAdditiveAiming("Shooting-AK47-Aim-Down");
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00023CBC File Offset: 0x00021EBC
	private void SetupAdditiveAiming(string anim)
	{
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 1f;
		base.GetComponent<Animation>()[anim].layer = 20;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00023D44 File Offset: 0x00021F44
	private float CrossFadeUp(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight + Time.deltaTime / fadeTime);
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00023D54 File Offset: 0x00021F54
	private float CrossFadeDown(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight - Time.deltaTime / fadeTime);
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00023D64 File Offset: 0x00021F64
	private void EquipWeapon()
	{
		if (this.rightHandSecondary == null)
		{
			this.rightHandSecondary = base.transform.Find("SecondaryRightHand");
		}
		if (this.rightHandPrimary == null)
		{
			this.rightHandPrimary = base.transform.Find("PrimaryRightHand");
		}
		WeaponTypes weaponType = this.weaponPrefab.WeaponType;
		if (weaponType != WeaponTypes.PRIMARY)
		{
			if (weaponType == WeaponTypes.SECONDARY)
			{
				if (this.rightHandSecondary != null)
				{
					this.weapon = ((Transform)UnityEngine.Object.Instantiate(this.weaponPrefab.transform, this.rightHandSecondary.position, this.rightHandSecondary.rotation)).GetComponent<Gun>();
				}
				this.weapon.weaponTransform.position = this.rightHandSecondary.position;
				this.weapon.weaponTransform.rotation = this.rightHandSecondary.rotation;
				this.weapon.transform.parent = this.rightHandSecondary;
				this.weapon.weaponTransform.parent = this.rightHandSecondary;
			}
		}
		else
		{
			if (this.rightHandPrimary != null)
			{
				this.weapon = ((Transform)UnityEngine.Object.Instantiate(this.weaponPrefab.transform, this.rightHandPrimary.position, this.rightHandPrimary.rotation)).GetComponent<Gun>();
			}
			this.weapon.weaponTransform.position = this.rightHandPrimary.position;
			this.weapon.weaponTransform.rotation = this.rightHandPrimary.rotation;
			this.weapon.transform.parent = this.rightHandPrimary;
			this.weapon.weaponTransform.parent = this.rightHandPrimary;
		}
		if (this.weapon.rightHandOffsetPosition != Vector3.zero || this.weapon.rightHandOffsetRotation != Vector3.zero)
		{
			this.weapon.weaponTransform.transform.Translate(this.weapon.rightHandOffsetPosition);
			this.weapon.weaponTransform.transform.Rotate(this.weapon.rightHandOffsetRotation, Space.Self);
		}
		this.weapon.weaponHolder = base.transform.GetComponent<WeaponHandling>();
		this.weapon.weaponHolder.weapon = this.weapon;
		this.weapon.weaponTransform.localScale = base.transform.localScale.x * this.weapon.weaponTransform.localScale;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00024014 File Offset: 0x00022214
	private void RootMotion()
	{
		Vector3 position = base.transform.Find("Bip01/Root").transform.position;
		base.GetComponent<Animation>().Stop();
		base.GetComponent<Animation>().Play("Engaged2-Idle");
		position.y = this.startingYPosition;
		base.transform.position = position;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00024074 File Offset: 0x00022274
	public void Update()
	{
		if (this.turret && this.weapon != null)
		{
			this.weapon.DisableBulletTrace();
		}
		if (base.transform.position.y < this.startingYPosition - 10f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.entered && this.entryDuration > 0f)
		{
			this.entryDuration -= Time.deltaTime;
			if (this.entryDuration <= 0f)
			{
				this.RootMotion();
				if (this.target != null)
				{
					if (this.groupID != "NONE")
					{
						BotAI.lastSeenPositions[this.groupID] = this.target.transform.position;
					}
					else
					{
						this.lastSeenPosition = this.target.transform.position;
					}
				}
			}
			return;
		}
		this.SetupAdditiveAiming("Handgun-Aiming-Up");
		this.SetupAdditiveAiming("Handgun-Aiming-Down");
		this.SetupAdditiveAiming("Shooting-AK47-Aim-Up");
		this.SetupAdditiveAiming("Shooting-AK47-Aim-Down");
		if (this.startInCover && this.coverState != BotAI.CoverStates.EXITING_COVER && this.target != null)
		{
			this.state = BotAI.AIState.ENGAGED;
			this.animState = BotAI.AnimStates.TAKECOVER;
			this.previousAnimState = this.animState;
			if (this.groupID != "NONE")
			{
				BotAI.lastSeenPositions[this.groupID] = this.target.transform.position;
			}
			else
			{
				this.lastSeenPosition = this.target.transform.position;
			}
		}
		if (this.state == BotAI.AIState.RELAXED && Time.time > BotAI.lastRelaxedSoundTime + 10f && this.enemyRelaxedSounds.Length > 0)
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
			bool flag = true;
			foreach (UnityEngine.Object obj in array)
			{
				BotAI botAI = (BotAI)obj;
				if (botAI.state == BotAI.AIState.ENGAGED)
				{
					flag = false;
				}
			}
			if (flag)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.enemyRelaxedSoundsArabic[BotAI.currentenemyRelaxedSound] != null)
				{
					base.GetComponent<AudioSource>().pitch = this.currentPitch;
					if (this.currentPitch == 1f)
					{
						this.currentPitch = 1.05f;
					}
					else if (this.currentPitch == 1.05f)
					{
						this.currentPitch = 0.9f;
					}
					else if (this.currentPitch == 0.9f)
					{
						this.currentPitch = 1f;
					}
					base.GetComponent<AudioSource>().PlayOneShot(this.enemyRelaxedSoundsArabic[BotAI.currentenemyRelaxedSound], SpeechManager.speechVolume);
					BotAI.currentenemyRelaxedSound = (BotAI.currentenemyRelaxedSound + 1) % this.enemyRelaxedSoundsArabic.Length;
				}
				else
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.enemyRelaxedSounds[BotAI.currentenemyRelaxedSound], SpeechManager.speechVolume);
					BotAI.currentenemyRelaxedSound = (BotAI.currentenemyRelaxedSound + 1) % this.enemyRelaxedSounds.Length;
				}
				BotAI.lastRelaxedSoundTime = Time.time;
			}
		}
		else if (this.state == BotAI.AIState.ENGAGED && Time.time > BotAI.lastEngagedSoundTime + 10f && this.enemyEngagedSounds.Length > 0)
		{
			if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.enemyEngagedSoundsArabic[BotAI.currentenemyEngagedSound] != null)
			{
				base.GetComponent<AudioSource>().pitch = this.currentPitch;
				if (this.currentPitch == 1f)
				{
					this.currentPitch = 1.05f;
				}
				else if (this.currentPitch == 1.05f)
				{
					this.currentPitch = 0.9f;
				}
				else if (this.currentPitch == 0.9f)
				{
					this.currentPitch = 1f;
				}
				base.GetComponent<AudioSource>().PlayOneShot(this.enemyEngagedSoundsArabic[BotAI.currentenemyEngagedSound], SpeechManager.speechVolume);
				BotAI.currentenemyEngagedSound = (BotAI.currentenemyEngagedSound + 1) % this.enemyEngagedSoundsArabic.Length;
			}
			else
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.enemyEngagedSounds[BotAI.currentenemyEngagedSound], SpeechManager.speechVolume);
				BotAI.currentenemyEngagedSound = (BotAI.currentenemyEngagedSound + 1) % this.enemyEngagedSounds.Length;
			}
			BotAI.lastEngagedSoundTime = Time.time;
		}
		if (this.cc.isGrounded)
		{
			if (this.previousAnimState == BotAI.AnimStates.FALLING)
			{
				if (!this.disableFalling && !base.GetComponent<Animation>().IsPlaying("General-Land-Medium"))
				{
					base.GetComponent<Animation>().CrossFade("General-Land-Medium");
				}
				this.animState = BotAI.AnimStates.IDLE;
			}
		}
		else
		{
			this.animState = BotAI.AnimStates.FALLING;
		}
		this.moveDirection = Vector3.zero;
		switch (this.animState)
		{
		case BotAI.AnimStates.IDLE:
			this.Idle();
			break;
		case BotAI.AnimStates.WALKFORWARD:
			this.WalkForward();
			break;
		case BotAI.AnimStates.WALKBACKWARD:
			this.WalkBackward();
			break;
		case BotAI.AnimStates.STRAFELEFT:
			this.StrafeLeft();
			break;
		case BotAI.AnimStates.STRAFERIGHT:
			this.StrafeRight();
			break;
		case BotAI.AnimStates.RUN:
			this.Run();
			break;
		case BotAI.AnimStates.FALLING:
			if (!this.disableFalling)
			{
				this.Fall();
			}
			break;
		case BotAI.AnimStates.HIT:
			this.Hit();
			break;
		case BotAI.AnimStates.CROUCH:
			this.Crouch();
			break;
		case BotAI.AnimStates.TAKECOVER:
			this.HandleCover();
			break;
		case BotAI.AnimStates.MOVETOPOINT:
			this.MoveToPoint();
			break;
		}
		this.previousAnimState = this.animState;
		this.moveDirection.y = this.moveDirection.y - this.gravity;
		this.cc.Move(this.moveDirection * Time.deltaTime);
		if (this.state == BotAI.AIState.RELAXED && (this.IsAware() || (this.groupID != "NONE" && (Vector3)BotAI.lastSeenPositions[this.groupID] != Vector3.zero) || this.lastSeenPosition != Vector3.zero))
		{
			this.state = BotAI.AIState.ENGAGED;
			BotAI.lastEngagedSoundTime = Time.time;
		}
		if (this.state == BotAI.AIState.ENGAGED)
		{
			if (this.animState != BotAI.AnimStates.TAKECOVER)
			{
				if (this.launchGrenades && this.target != null && Time.time > BotAI.lastGrenadeShootTime + 30f)
				{
					Vector3 position = base.transform.position;
					position.y = 0f;
					Vector3 position2 = this.target.transform.position;
					position2.y = 0f;
					float num = Vector3.Distance(position, position2);
					if (num > 10f && num < 25f)
					{
						Vector3 forward = base.transform.forward;
						Vector3 vector = this.target.transform.position - base.transform.position;
						forward.y = 0f;
						vector.y = 0f;
						float num2 = Vector3.Angle(forward, vector);
						if (Vector3.Cross(forward, vector).y < 0f)
						{
							num2 *= -1f;
						}
						num2 += 20f;
						base.GetComponent<Animation>().Play("Shooting-Grenade-QuickThrow", PlayMode.StopAll);
						this.launchProjectile = true;
						this.luanchTimer = base.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].length * 0.5f;
						iTween.RotateBy(base.gameObject, new Vector3(0f, num2 / 360f, 0f), this.luanchTimer);
						BotAI.lastGrenadeShootTime = Time.time;
					}
				}
				if (this.launchProjectile)
				{
					this.luanchTimer -= Time.deltaTime;
					if (this.luanchTimer <= 0f)
					{
						this.grenade.hitPosition = this.target.transform.position;
						this.grenade.LaunchProjectile();
						base.GetComponent<AudioSource>().PlayOneShot(this.grenadeLunchSound, SpeechManager.sfxVolume);
						this.launchProjectile = false;
					}
					return;
				}
				if (this.turret)
				{
					this.animState = BotAI.AnimStates.IDLE;
				}
				else
				{
					this.changeAnimStateTimer -= Time.deltaTime;
					if (this.changeAnimStateTimer <= 0f)
					{
						if (this.animState == BotAI.AnimStates.IDLE)
						{
							if (!this.priorityToCover)
							{
								this.currentAnimState = (this.currentAnimState + 1) % 6;
								switch (this.currentAnimState)
								{
								case 0:
									this.animState = BotAI.AnimStates.STRAFELEFT;
									break;
								case 1:
									this.animState = BotAI.AnimStates.STRAFERIGHT;
									break;
								case 2:
									this.animState = BotAI.AnimStates.WALKBACKWARD;
									break;
								case 3:
									this.animState = BotAI.AnimStates.WALKFORWARD;
									break;
								case 4:
									this.animState = BotAI.AnimStates.CROUCH;
									break;
								case 5:
									this.coverState = BotAI.CoverStates.TAKING_COVER;
									this.animState = BotAI.AnimStates.TAKECOVER;
									break;
								}
							}
							else if (this.lookingForCover)
							{
								this.currentAnimState = (this.currentAnimState + 1) % 5;
								switch (this.currentAnimState)
								{
								case 0:
									this.animState = BotAI.AnimStates.STRAFELEFT;
									break;
								case 1:
									this.animState = BotAI.AnimStates.STRAFERIGHT;
									break;
								case 2:
									this.animState = BotAI.AnimStates.WALKBACKWARD;
									break;
								case 3:
									this.animState = BotAI.AnimStates.WALKFORWARD;
									break;
								case 4:
									this.animState = BotAI.AnimStates.CROUCH;
									break;
								}
								this.lookingForCover = false;
							}
							else
							{
								this.coverState = BotAI.CoverStates.TAKING_COVER;
								this.animState = BotAI.AnimStates.TAKECOVER;
								this.lookingForCover = true;
							}
							this.changeAnimStateTimer = UnityEngine.Random.Range(3f, 6f);
						}
						else
						{
							this.animState = BotAI.AnimStates.IDLE;
							this.changeAnimStateTimer = UnityEngine.Random.Range(2f, 4f);
						}
					}
				}
			}
		}
		else if (this.state == BotAI.AIState.RELAXED && this.turret)
		{
			this.animState = BotAI.AnimStates.IDLE;
			if ((this.lastRotationSet == 0f || Time.time > this.lastRotationSet + 4f) && this.currentRotation >= 0f)
			{
				float num3 = UnityEngine.Random.Range(0.25f * (this.rotationCone / 2f), this.rotationCone / 3f);
				this.currentRotation -= num3;
				iTween.RotateBy(base.gameObject, new Vector3(0f, num3 / 360f, 0f), 1f);
				this.lastRotationSet = Time.time;
			}
			else if (this.lastRotationSet == 0f || Time.time > this.lastRotationSet + 4f)
			{
				float num4 = UnityEngine.Random.Range(-0.25f * (this.rotationCone / 2f), -this.rotationCone / 2f);
				this.currentRotation -= num4;
				iTween.RotateBy(base.gameObject, new Vector3(0f, num4 / 360f, 0f), 3f);
				this.lastRotationSet = Time.time;
			}
		}
		else if (this.state == BotAI.AIState.RELAXED && this.idleOnRelax)
		{
			if (this.moveToPoint == Vector3.zero)
			{
				this.moveToPoint = base.transform.TransformPoint(0f, 0f, this.distanceBeforeIdle);
				this.animState = BotAI.AnimStates.MOVETOPOINT;
			}
			else if (Vector3.Distance(this.moveToPoint, base.transform.position) > 1f)
			{
				this.animState = BotAI.AnimStates.MOVETOPOINT;
			}
			else
			{
				this.animState = BotAI.AnimStates.IDLE;
			}
		}
		else if (this.state == BotAI.AIState.RELAXED && this.relaxedRadius != 0f)
		{
			if (this.patrolPath.Length == 0)
			{
				if (this.moveToPoint == Vector3.zero || Vector3.Distance(this.moveToPoint, base.transform.position) < 2f)
				{
					this.animState = BotAI.AnimStates.IDLE;
					this.idleTimer -= Time.deltaTime;
					if (this.idleTimer <= 0f)
					{
						do
						{
							this.moveToPoint = this.spawnPosition;
							this.moveToPoint.x = this.moveToPoint.x + UnityEngine.Random.Range(-this.relaxedRadius, this.relaxedRadius);
							this.moveToPoint.z = this.moveToPoint.z + UnityEngine.Random.Range(-this.relaxedRadius, this.relaxedRadius);
							this.moveToPoint.y = base.transform.position.y;
							this.animState = BotAI.AnimStates.MOVETOPOINT;
							this.idleTimer = 3f;
						}
						while (Physics.Linecast(base.transform.position, this.moveToPoint));
					}
				}
			}
			else if (this.currentPathPoint == -1 || Vector3.Distance(this.patrolPath[this.currentPathPoint].position, base.transform.position) < 2f)
			{
				this.animState = BotAI.AnimStates.IDLE;
				this.idleTimer -= Time.deltaTime;
				if (this.idleTimer <= 0f)
				{
					this.currentPathPoint = (this.currentPathPoint + 1) % this.patrolPath.Length;
					this.moveToPoint = this.patrolPath[this.currentPathPoint].position;
					this.moveToPoint.y = base.transform.position.y;
					this.animState = BotAI.AnimStates.MOVETOPOINT;
					this.idleTimer = 3f;
				}
			}
		}
		else if (this.state == BotAI.AIState.RELAXED && this.relaxedRadius == 0f && Time.time > this.randSeekRotationTime + 4f)
		{
			this.randSeekRotation = UnityEngine.Random.Range(-0.25f, 0.25f);
			this.randSeekRotationTime = Time.time;
			iTween.RotateBy(base.gameObject, new Vector3(0f, this.randSeekRotation, 0f), 2f);
		}
		this.HandleFire();
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00024EEC File Offset: 0x000230EC
	private void MoveToPoint()
	{
		this.moveToPoint.y = base.transform.position.y;
		base.transform.LookAt(this.moveToPoint);
		Debug.DrawLine(this.moveToPoint, base.transform.position, Color.blue);
		base.transform.position = Vector3.Lerp(base.transform.position, this.moveToPoint, Time.deltaTime / (Vector3.Distance(this.moveToPoint, base.transform.position) / 1f));
		if (!base.GetComponent<Animation>().IsPlaying("General-Walk"))
		{
			base.GetComponent<Animation>().CrossFade("General-Walk");
		}
		this.TriggerSteepsSound("General-Walk");
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00024FB8 File Offset: 0x000231B8
	private void HandleCover()
	{
		if (this.target == null)
		{
			this.coverState = BotAI.CoverStates.EXITING_COVER;
		}
		switch (this.coverState)
		{
		case BotAI.CoverStates.TAKING_COVER:
			if (this.currentCover == null)
			{
				this.currentCover = this.GetCover();
				if (this.currentCover == null)
				{
					this.animState = BotAI.AnimStates.STRAFELEFT;
					this.currentAnimState = 0;
					this.holdingFire = false;
					return;
				}
				this.currentCover.isUsedByAI = true;
				if (!this.facePlayerWhileTakingCover)
				{
					this.holdingFire = true;
				}
				if (this.enemyTakingCoverSounds.Length > 0)
				{
					if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.enemyTakingCoverSoundsArabic[BotAI.currentenemyTakingCoverSound] != null)
					{
						base.GetComponent<AudioSource>().pitch = this.currentPitch;
						if (this.currentPitch == 1f)
						{
							this.currentPitch = 1.05f;
						}
						else if (this.currentPitch == 1.05f)
						{
							this.currentPitch = 0.9f;
						}
						else if (this.currentPitch == 0.9f)
						{
							this.currentPitch = 1f;
						}
						base.GetComponent<AudioSource>().PlayOneShot(this.enemyTakingCoverSoundsArabic[BotAI.currentenemyTakingCoverSound], SpeechManager.speechVolume);
						BotAI.currentenemyTakingCoverSound = (BotAI.currentenemyTakingCoverSound + 1) % this.enemyTakingCoverSoundsArabic.Length;
					}
					else
					{
						base.GetComponent<AudioSource>().PlayOneShot(this.enemyTakingCoverSounds[BotAI.currentenemyTakingCoverSound], SpeechManager.speechVolume);
						BotAI.currentenemyTakingCoverSound = (BotAI.currentenemyTakingCoverSound + 1) % this.enemyTakingCoverSounds.Length;
					}
				}
			}
			if (this.weapon != null)
			{
				this.weapon.DisableBulletTrace();
			}
			if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
			{
				if (this.currentCover.path == string.Empty)
				{
					Vector3 position = this.currentCover.transform.Find("StartPosition").transform.position;
					Vector3 position2 = this.currentCover.transform.Find("EndPosition").transform.position;
					position.y = base.transform.position.y;
					position2.y = base.transform.position.y;
					if (Vector3.Distance(this.target.transform.position, position) < Vector3.Distance(this.target.transform.position, position2))
					{
						if (this.currentCover.rightPopout)
						{
							this.coverPosition = position;
						}
						else
						{
							this.coverPosition = position2;
						}
					}
					else if (this.currentCover.leftPopout)
					{
						this.coverPosition = position2;
					}
					else
					{
						this.coverPosition = position;
					}
				}
				else
				{
					float num = 1000f;
					for (float num2 = 0f; num2 < 1f; num2 += 0.01f)
					{
						Vector3 vector = iTween.PointOnPath(iTweenPath.GetPath(this.currentCover.path), num2);
						if (Physics.Linecast(this.target.transform.position, vector, this.coverMask) && Vector3.Distance(vector, this.target.transform.position) < num)
						{
							this.coverPosition = vector;
							this.coverPosition.y = base.transform.position.y;
							num = Vector3.Distance(vector, this.target.transform.position);
						}
					}
				}
			}
			else if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
			{
				if (this.currentCover.path == string.Empty)
				{
					this.coverPosition = this.currentCover.transform.position;
					this.coverPosition.y = base.transform.position.y;
				}
				else
				{
					Vector3[] path = iTweenPath.GetPath(this.currentCover.path);
					float num3 = 0f;
					foreach (Vector3 vector2 in path)
					{
						if (Physics.Linecast(this.target.transform.position, vector2, this.coverMask) && Vector3.Distance(vector2, this.target.transform.position) > num3)
						{
							this.coverPosition = vector2;
							this.coverPosition.y = base.transform.position.y;
							num3 = Vector3.Distance(vector2, this.target.transform.position);
						}
					}
				}
			}
			if (!this.facePlayerWhileTakingCover)
			{
				base.transform.LookAt(this.coverPosition);
				base.transform.position = Vector3.Lerp(base.transform.position, this.coverPosition, Time.deltaTime / (Vector3.Distance(this.coverPosition, base.transform.position) / 4f));
				if (!base.GetComponent<Animation>().IsPlaying("General-Run"))
				{
					base.GetComponent<Animation>().CrossFade("General-Run");
				}
				this.TriggerSteepsSound("General-Run");
			}
			else
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.coverPosition, Time.deltaTime / (Vector3.Distance(this.coverPosition, base.transform.position) / 1f));
				if (Vector3.Cross(this.coverPosition - base.transform.position, base.transform.forward).y > 0f)
				{
					if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Right"))
					{
						base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Right");
					}
				}
				else if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Left"))
				{
					base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Left");
				}
			}
			if (Vector3.Distance(base.transform.position, this.coverPosition) < 0.2f)
			{
				this.coverState = BotAI.CoverStates.IN_COVER;
				this.holdingFire = true;
				this.popingOutTimer = UnityEngine.Random.Range(2f, 4f);
			}
			break;
		case BotAI.CoverStates.IN_COVER:
			if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
			{
				this.facePlayerWhileShooting = false;
				if (base.transform.InverseTransformPoint(this.target.transform.position).x > 0f)
				{
					if (!base.GetComponent<Animation>().IsPlaying("Cover-Tall-Idle-Left"))
					{
						this.weapon.Reload();
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Left", 0.5f);
					}
				}
				else if (!base.GetComponent<Animation>().IsPlaying("Cover-Tall-Idle-Right"))
				{
					this.weapon.Reload();
					base.GetComponent<Animation>().CrossFade("Cover-Tall-Idle-Right", 0.5f);
				}
			}
			else if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
			{
				this.facePlayerWhileShooting = true;
				if (!base.GetComponent<Animation>().IsPlaying("Cover-Short-Idle-Left"))
				{
					this.weapon.Reload();
					base.GetComponent<Animation>().CrossFade("Cover-Short-Idle-Left", 0.5f);
				}
			}
			if (this.currentCover.path == string.Empty)
			{
				if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
				{
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, this.currentCover.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z);
				}
				else if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
				{
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, this.currentCover.transform.rotation.eulerAngles.y + 180f, base.transform.rotation.eulerAngles.z);
				}
			}
			else
			{
				Vector3 position3 = this.currentCover.transform.position;
				position3.y = base.transform.position.y;
				base.transform.rotation = Quaternion.LookRotation(base.transform.position - position3);
			}
			this.popingOutTimer -= Time.deltaTime;
			if (this.popingOutTimer <= 0f)
			{
				this.coverState = BotAI.CoverStates.POPING_OUT;
				this.popingOutTimer = UnityEngine.Random.Range(2f, 4f);
				if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
				{
					this.SetupAdditiveAiming("Handgun-Aiming-Left");
					this.SetupAdditiveAiming("Handgun-Aiming-Right");
				}
				else
				{
					this.SetupAdditiveAiming("AK47-Aiming-Left");
					this.SetupAdditiveAiming("AK47-Aiming-Right");
				}
			}
			if (this.target != null && !Physics.Linecast(this.target.transform.position, this.coverPosition, this.coverMask))
			{
				this.coverState = BotAI.CoverStates.EXITING_COVER;
			}
			break;
		case BotAI.CoverStates.POPING_OUT:
			if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
			{
				Vector3 from = -base.transform.forward;
				from.y = 0f;
				Vector3 to = this.target.transform.position - base.transform.position;
				Debug.DrawLine(this.rightHandSecondary.position, this.target.transform.position, Color.red);
				to.y = 0f;
				float num4 = Vector3.Angle(from, to);
				if (base.transform.InverseTransformPoint(this.target.transform.position).x > 0f)
				{
					if (this.currentCover.leftPopout)
					{
						if (!base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Left"))
						{
							base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.5f);
							this.holdFire = base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].length - 0.6f;
							this.holdingFire = false;
						}
						if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
						{
							base.GetComponent<Animation>()["Handgun-Aiming-Left"].weight = 1f;
							base.GetComponent<Animation>()["Handgun-Aiming-Right"].weight = 0f;
							base.GetComponent<Animation>()["Handgun-Aiming-Left"].normalizedTime = Mathf.Clamp01(num4 / 150f);
						}
						else
						{
							base.GetComponent<Animation>()["AK47-Aiming-Left"].weight = 1f;
							base.GetComponent<Animation>()["AK47-Aiming-Right"].weight = 0f;
							base.GetComponent<Animation>()["AK47-Aiming-Left"].normalizedTime = Mathf.Clamp01(num4 / 120f);
						}
					}
					else
					{
						if (!base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Right"))
						{
							base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.5f);
							this.holdFire = base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].length - 0.6f;
							this.holdingFire = false;
						}
						if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
						{
							base.GetComponent<Animation>()["Handgun-Aiming-Left"].weight = 0f;
							base.GetComponent<Animation>()["Handgun-Aiming-Right"].weight = 1f;
							base.GetComponent<Animation>()["Handgun-Aiming-Right"].normalizedTime = Mathf.Clamp01(num4 / 90f);
						}
						else
						{
							base.GetComponent<Animation>()["AK47-Aiming-Left"].weight = 0f;
							base.GetComponent<Animation>()["AK47-Aiming-Right"].weight = 1f;
							base.GetComponent<Animation>()["AK47-Aiming-Right"].normalizedTime = Mathf.Clamp01(num4 / 150f);
						}
					}
				}
				else if (this.currentCover.rightPopout)
				{
					if (!base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Right"))
					{
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Right", 0.5f);
						this.holdFire = base.GetComponent<Animation>()["Cover-Tall-Aim-Right"].length - 0.6f;
						this.holdingFire = false;
					}
					if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
					{
						base.GetComponent<Animation>()["Handgun-Aiming-Left"].weight = 0f;
						base.GetComponent<Animation>()["Handgun-Aiming-Right"].weight = 1f;
						base.GetComponent<Animation>()["Handgun-Aiming-Right"].normalizedTime = Mathf.Clamp01(num4 / 90f);
					}
					else
					{
						base.GetComponent<Animation>()["AK47-Aiming-Left"].weight = 0f;
						base.GetComponent<Animation>()["AK47-Aiming-Right"].weight = 1f;
						base.GetComponent<Animation>()["AK47-Aiming-Right"].normalizedTime = Mathf.Clamp01(num4 / 150f);
					}
				}
				else
				{
					if (!base.GetComponent<Animation>().IsPlaying("Cover-Tall-Aim-Left"))
					{
						base.GetComponent<Animation>().CrossFade("Cover-Tall-Aim-Left", 0.5f);
						this.holdFire = base.GetComponent<Animation>()["Cover-Tall-Aim-Left"].length - 0.6f;
						this.holdingFire = false;
					}
					if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
					{
						base.GetComponent<Animation>()["Handgun-Aiming-Left"].weight = 1f;
						base.GetComponent<Animation>()["Handgun-Aiming-Right"].weight = 0f;
						base.GetComponent<Animation>()["Handgun-Aiming-Left"].normalizedTime = Mathf.Clamp01(num4 / 150f);
					}
					else
					{
						base.GetComponent<Animation>()["AK47-Aiming-Left"].weight = 1f;
						base.GetComponent<Animation>()["AK47-Aiming-Right"].weight = 0f;
						base.GetComponent<Animation>()["AK47-Aiming-Left"].normalizedTime = Mathf.Clamp01(num4 / 120f);
					}
				}
			}
			else if (this.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT && !base.GetComponent<Animation>().IsPlaying("Engaged2-Idle"))
			{
				base.GetComponent<Animation>().CrossFade("Engaged2-Idle", 0.5f);
				this.holdingFire = false;
			}
			this.popingOutTimer -= Time.deltaTime;
			if (this.popingOutTimer <= 0f)
			{
				this.coverState = BotAI.CoverStates.IN_COVER;
				this.holdingFire = true;
				this.popingOutTimer = UnityEngine.Random.Range(2f, 4f);
				base.Invoke("UnSetAdditiveCoverAnimations", 0.19f);
			}
			if (!Physics.Linecast(this.target.transform.position, this.coverPosition, this.coverMask))
			{
				this.coverState = BotAI.CoverStates.EXITING_COVER;
			}
			break;
		case BotAI.CoverStates.EXITING_COVER:
			base.GetComponent<Animation>().CrossFade("General-Idle", 0.5f);
			this.holdingFire = false;
			if (this.currentCover != null)
			{
				this.currentCover.isUsedByAI = false;
				this.currentCover = null;
			}
			if (this.weapon != null)
			{
				this.weapon.EnableBulletTrace();
			}
			base.Invoke("ExitCover", 0.5f);
			break;
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00025FF0 File Offset: 0x000241F0
	private void ExitCover()
	{
		this.animState = BotAI.AnimStates.IDLE;
		base.GetComponent<Animation>()["Handgun-Aiming-Left"].weight = 0f;
		base.GetComponent<Animation>()["Handgun-Aiming-Right"].weight = 0f;
		base.GetComponent<Animation>()["AK47-Aiming-Left"].weight = 0f;
		base.GetComponent<Animation>()["AK47-Aiming-Right"].weight = 0f;
		this.facePlayerWhileShooting = true;
		this.currentAnimState = 0;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0002607C File Offset: 0x0002427C
	private void UnSetAdditiveCoverAnimations()
	{
		base.GetComponent<Animation>()["Handgun-Aiming-Left"].weight = 0f;
		base.GetComponent<Animation>()["Handgun-Aiming-Right"].weight = 0f;
		base.GetComponent<Animation>()["AK47-Aiming-Left"].weight = 0f;
		base.GetComponent<Animation>()["AK47-Aiming-Right"].weight = 0f;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x000260F4 File Offset: 0x000242F4
	private InteractionTrigger GetCover()
	{
		if (this.target == null)
		{
			return null;
		}
		InteractionTrigger result = null;
		float num = 10000f;
		object[] array = UnityEngine.Object.FindObjectsOfType(typeof(InteractionTrigger));
		foreach (object obj in array)
		{
			InteractionTrigger interactionTrigger = (InteractionTrigger)obj;
			if ((interactionTrigger.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT || interactionTrigger.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL) && !interactionTrigger.isUsedByAI)
			{
				Vector3 vector = Vector3.zero;
				if (interactionTrigger.path == string.Empty)
				{
					vector = interactionTrigger.transform.position;
					vector.y = base.transform.position.y;
				}
				else if (interactionTrigger.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
				{
					float num2 = 1000f;
					for (float num3 = 0f; num3 < 1f; num3 += 0.01f)
					{
						Vector3 vector2 = iTween.PointOnPath(iTweenPath.GetPath(interactionTrigger.path), num3);
						if (Physics.Linecast(this.target.transform.position, vector2, this.coverMask) && Vector3.Distance(vector2, this.target.transform.position) < num2)
						{
							vector = vector2;
							vector.y = base.transform.position.y;
							num2 = Vector3.Distance(vector2, this.target.transform.position);
						}
					}
				}
				else if (interactionTrigger.TriggerType == InteractionTrigger.InteractionTypes.COVERSHORT)
				{
					Vector3[] path = iTweenPath.GetPath(interactionTrigger.path);
					float num4 = 0f;
					foreach (Vector3 vector3 in path)
					{
						if (Physics.Linecast(this.target.transform.position, vector3, this.coverMask) && Vector3.Distance(vector3, this.target.transform.position) > num4)
						{
							vector = vector3;
							vector.y = base.transform.position.y;
							num4 = Vector3.Distance(vector3, this.target.transform.position);
						}
					}
				}
				float num5 = Vector3.Distance(vector, base.transform.position);
				if (num5 < num && num5 < this.maxCoverDistance && Physics.Linecast(this.target.transform.position, vector, this.coverMask) && !Physics.Linecast(base.transform.position, vector))
				{
					if (interactionTrigger.TriggerType != InteractionTrigger.InteractionTypes.COVERTALL || interactionTrigger.path != string.Empty)
					{
						result = interactionTrigger;
						num = num5;
					}
					else
					{
						Vector3 from = this.target.transform.position - interactionTrigger.transform.position;
						from.y = 0f;
						Vector3 forward = interactionTrigger.transform.forward;
						forward.y = 0f;
						if (Vector3.Angle(from, forward) < 90f)
						{
							result = interactionTrigger;
							num = num5;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0002644C File Offset: 0x0002464C
	private void Crouch()
	{
		if (!base.GetComponent<Animation>().IsPlaying("Shooting-Crouch-Idle"))
		{
			base.GetComponent<Animation>().CrossFade("Shooting-Crouch-Idle", 0.5f);
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00026484 File Offset: 0x00024684
	private void HandleFire()
	{
		if (this.holdFire > 0f || this.holdingFire)
		{
			this.weapon.fire = false;
			this.holdFire -= Time.deltaTime;
			return;
		}
		if (this.state == BotAI.AIState.ENGAGED && this.target != null)
		{
			Vector3 worldPosition = this.target.transform.position;
			if (!this.IsAware() && this.animState != BotAI.AnimStates.TAKECOVER)
			{
				if (this.groupID != "NONE")
				{
					worldPosition = (Vector3)BotAI.lastSeenPositions[this.groupID];
					Debug.DrawLine(base.transform.position, (Vector3)BotAI.lastSeenPositions[this.groupID], Color.gray);
				}
				else
				{
					worldPosition = this.lastSeenPosition;
					Debug.DrawLine(base.transform.position, this.lastSeenPosition, Color.gray);
				}
				worldPosition.y = base.transform.position.y;
				if (this.facePlayerWhileShooting)
				{
					base.transform.LookAt(worldPosition);
				}
				this.nextBurstTimer = 0f;
				this.weapon.fire = false;
				return;
			}
			worldPosition.y = base.transform.position.y;
			if (this.facePlayerWhileShooting)
			{
				base.transform.LookAt(worldPosition);
			}
			this.nextBurstTimer -= Time.deltaTime;
			if (!this.weapon.fire)
			{
				if (this.nextBurstTimer <= 0f)
				{
					this.burstTimer = UnityEngine.Random.Range(1f, this.burstInterval);
					this.weapon.fire = true;
				}
			}
			else if (this.weapon.fireMode == FireMode.SEMI_AUTO)
			{
				this.nextBurstTimer = UnityEngine.Random.Range(0.5f, 1f);
				this.weapon.fire = false;
			}
			else
			{
				this.burstTimer -= Time.deltaTime;
				if (this.burstTimer <= 0f)
				{
					this.nextBurstTimer = UnityEngine.Random.Range(0.5f, 2f);
					this.weapon.fire = false;
				}
			}
			this.aimWeight = 1f;
			Vector3 position = this.target.transform.position;
			float to = Mathf.Asin((position - this.aimPivot.position).normalized.y) * 57.29578f;
			this.aimAngleY = Mathf.Lerp(this.aimAngleY, to, Time.deltaTime * 8f);
			if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
			{
				base.GetComponent<Animation>()["Handgun-Aiming-Up"].weight = this.uprightWeight * this.aimWeight;
				base.GetComponent<Animation>()["Handgun-Aiming-Down"].weight = this.uprightWeight * this.aimWeight;
			}
			else
			{
				base.GetComponent<Animation>()["Shooting-AK47-Aim-Up"].weight = this.uprightWeight * this.aimWeight;
				base.GetComponent<Animation>()["Shooting-AK47-Aim-Down"].weight = this.uprightWeight * this.aimWeight;
			}
			if (this.weapon.WeaponType == WeaponTypes.SECONDARY)
			{
				base.GetComponent<Animation>()["Handgun-Aiming-Up"].time = Mathf.Clamp01(this.aimAngleY / 90f);
				base.GetComponent<Animation>()["Handgun-Aiming-Down"].time = Mathf.Clamp01(-this.aimAngleY / 90f);
			}
			else
			{
				base.GetComponent<Animation>()["Shooting-AK47-Aim-Up"].time = Mathf.Clamp01(this.aimAngleY / 90f);
				base.GetComponent<Animation>()["Shooting-AK47-Aim-Down"].time = Mathf.Clamp01(-this.aimAngleY / 90f);
			}
		}
		else if (this.weapon.fireMode != FireMode.SEMI_AUTO)
		{
			this.weapon.fire = false;
			this.aimWeight = 0f;
		}
		else
		{
			this.aimWeight = 0f;
		}
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x000268CC File Offset: 0x00024ACC
	private void Idle()
	{
		if (this.state == BotAI.AIState.RELAXED)
		{
			if (!base.GetComponent<Animation>().IsPlaying("General-Idle"))
			{
				base.GetComponent<Animation>().CrossFade("General-Idle", 0.5f);
			}
		}
		else if (!base.GetComponent<Animation>().IsPlaying("Engaged2-Idle"))
		{
			base.GetComponent<Animation>().CrossFade("Engaged2-Idle", 0.5f);
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00026940 File Offset: 0x00024B40
	private void WalkForward()
	{
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 0.5f, 0f), base.transform.forward, 1f, this.visualMask))
		{
			this.Idle();
			return;
		}
		string text = "General-Walk";
		BotAI.AIState aistate = this.state;
		if (aistate != BotAI.AIState.RELAXED)
		{
			if (aistate == BotAI.AIState.ENGAGED)
			{
				text = "Shooting-Walk-Forward";
			}
		}
		else
		{
			text = "General-Walk";
		}
		if (!base.GetComponent<Animation>().IsPlaying(text))
		{
			base.GetComponent<Animation>().CrossFade(text);
		}
		this.TriggerSteepsSound(text);
		this.moveDirection = base.transform.forward * 0.5f;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00026A0C File Offset: 0x00024C0C
	private void WalkBackward()
	{
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 0.5f, 0f), -base.transform.forward, 1f, this.visualMask))
		{
			this.Idle();
			return;
		}
		if (!base.GetComponent<Animation>().IsPlaying("Shooting-Walk-Back"))
		{
			base.GetComponent<Animation>().CrossFade("Shooting-Walk-Back");
		}
		this.TriggerSteepsSound("Shooting-Walk-Back");
		this.moveDirection = -base.transform.forward * 0.5f;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00026ABC File Offset: 0x00024CBC
	private void StrafeLeft()
	{
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 0.5f, 0f), -base.transform.right, 1f, this.visualMask))
		{
			this.Idle();
			return;
		}
		if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Left"))
		{
			base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Left");
		}
		this.TriggerSteepsSound("Shooting-Strafe-Left");
		this.moveDirection = -base.transform.right * 0.5f;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00026B6C File Offset: 0x00024D6C
	private void StrafeRight()
	{
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 0.5f, 0f), base.transform.right, 1f, this.visualMask))
		{
			this.Idle();
			return;
		}
		if (!base.GetComponent<Animation>().IsPlaying("Shooting-Strafe-Right"))
		{
			base.GetComponent<Animation>().CrossFade("Shooting-Strafe-Right");
		}
		this.TriggerSteepsSound("Shooting-Strafe-Right");
		this.moveDirection = base.transform.right * 0.5f;
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00026C10 File Offset: 0x00024E10
	private void Run()
	{
		if (!base.GetComponent<Animation>().IsPlaying("General-Run"))
		{
			base.GetComponent<Animation>().CrossFade("General-Run");
		}
		this.TriggerSteepsSound("General-Run");
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00026C50 File Offset: 0x00024E50
	private void TriggerSteepsSound(string anim)
	{
		if (base.GetComponent<Animation>()[anim].normalizedTime > this.WalkStep1StrikeTime && base.GetComponent<Animation>()[anim].normalizedTime < this.WalkStep1StrikeTime + 0.1f)
		{
			if (!this.emmittingStepSound)
			{
				base.SendMessage("OnFootStrike", SendMessageOptions.DontRequireReceiver);
			}
			this.emmittingStepSound = true;
		}
		else if (base.GetComponent<Animation>()[anim].normalizedTime > this.WalkStep2StrikeTime && base.GetComponent<Animation>()[anim].normalizedTime < this.WalkStep2StrikeTime + 0.1f)
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

	// Token: 0x060005B6 RID: 1462 RVA: 0x00026D28 File Offset: 0x00024F28
	private void Fall()
	{
		if (!base.GetComponent<Animation>().IsPlaying("General-Fall-Medium"))
		{
			base.GetComponent<Animation>().CrossFade("General-Fall-Medium");
		}
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00026D5C File Offset: 0x00024F5C
	public void Hit()
	{
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00026D60 File Offset: 0x00024F60
	public void HitExplosion()
	{
		if (this.entered)
		{
			this.RootMotion();
			if (this.target != null)
			{
				if (this.groupID != "NONE")
				{
					BotAI.lastSeenPositions[this.groupID] = this.target.transform.position;
				}
				else
				{
					this.lastSeenPosition = this.target.transform.position;
				}
			}
			this.entered = false;
			this.entryDuration = 0f;
			this.enteryStopped = true;
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00026E00 File Offset: 0x00025000
	private bool IsAware()
	{
		if (this.target == null)
		{
			this.isAware = false;
			this.previousIsAware = this.isAware;
			return false;
		}
		Vector3 position = this.target.transform.position;
		position.y = base.transform.position.y + 1.8f;
		Vector3 position2 = this.headLookPosition.position;
		RaycastHit raycastHit;
		if ((!Physics.Linecast(position2, position, out raycastHit, this.visualMask) || raycastHit.collider.gameObject.layer == this.target.layer) && Vector3.Distance(position2, position) < this.viewConeLength && Mathf.Abs(Vector3.Angle(this.headLookPosition.forward, (position - position2).normalized)) <= this.viewCone / 2f)
		{
			if (this.groupID != "NONE")
			{
				BotAI.lastSeenPositions[this.groupID] = position;
			}
			else
			{
				this.lastSeenPosition = position;
			}
			AnimationHandler.instance.stealthMode = false;
			this.isAware = true;
			if (this.previousIsAware != this.isAware && ((Time.time < 10f && BotAI.lastSawSomthingSound == 0f) || Time.time > BotAI.lastSawSomthingSound + 10f))
			{
				UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
				bool flag = true;
				foreach (UnityEngine.Object obj in array)
				{
					BotAI botAI = (BotAI)obj;
					if (botAI.state == BotAI.AIState.ENGAGED)
					{
						flag = false;
					}
				}
				if (flag)
				{
					if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.enemyJustSawSoundsArabic[BotAI.currentEnemyJustSawSound] != null)
					{
						base.GetComponent<AudioSource>().pitch = this.currentPitch;
						if (this.currentPitch == 1f)
						{
							this.currentPitch = 1.05f;
						}
						else if (this.currentPitch == 1.05f)
						{
							this.currentPitch = 0.9f;
						}
						else if (this.currentPitch == 0.9f)
						{
							this.currentPitch = 1f;
						}
						base.GetComponent<AudioSource>().PlayOneShot(this.enemyJustSawSoundsArabic[BotAI.currentEnemyJustSawSound], SpeechManager.speechVolume);
						BotAI.currentEnemyJustSawSound = (BotAI.currentEnemyJustSawSound + 1) % this.enemyJustSawSoundsArabic.Length;
					}
					else
					{
						base.GetComponent<AudioSource>().PlayOneShot(this.enemyJustSawSounds[BotAI.currentEnemyJustSawSound], SpeechManager.speechVolume);
						BotAI.currentEnemyJustSawSound = (BotAI.currentEnemyJustSawSound + 1) % this.enemyJustSawSounds.Length;
					}
					BotAI.lastSawSomthingSound = Time.time;
					BotAI.lastGrenadeShootTime = Time.time;
					if (this.animState != BotAI.AnimStates.TAKECOVER && this.EntryAnim == null)
					{
						this.holdFire = UnityEngine.Random.Range(2f, 3f);
					}
				}
			}
			this.previousIsAware = this.isAware;
			return true;
		}
		if (Debug.isDebugBuild)
		{
			float num = 0.017453292f * (this.viewCone / 2f);
			Vector3 vector = new Vector3(0f, 0f, 0f);
			vector.z += this.viewConeLength * Mathf.Cos(num);
			vector.x += this.viewConeLength * Mathf.Sin(num);
			Vector3 vector2 = new Vector3(0f, 0f, 0f);
			vector2.z += this.viewConeLength * Mathf.Cos(num);
			vector2.x -= this.viewConeLength * Mathf.Sin(num);
			vector = this.headLookPosition.TransformDirection(vector);
			vector2 = this.headLookPosition.TransformDirection(vector2);
			Vector3 b = this.headLookPosition.TransformDirection(0f, 0f, this.viewConeLength);
			Debug.DrawLine(position2, position2 + vector, Color.magenta);
			Debug.DrawLine(position2, position2 + vector2, Color.magenta);
			Vector3 vector3 = new Vector3(0f, 0f, 0f);
			vector3.z += this.viewConeLength * Mathf.Cos(num / 2f);
			vector3.x += this.viewConeLength * Mathf.Sin(num / 2f);
			Vector3 vector4 = new Vector3(0f, 0f, 0f);
			vector4.z += this.viewConeLength * Mathf.Cos(num / 2f);
			vector4.x -= this.viewConeLength * Mathf.Sin(num / 2f);
			vector3 = this.headLookPosition.TransformDirection(vector3);
			vector4 = this.headLookPosition.TransformDirection(vector4);
			Debug.DrawLine(position2 + vector, position2 + vector3, Color.magenta);
			Debug.DrawLine(position2 + vector3, position2 + b, Color.magenta);
			Debug.DrawLine(position2 + b, position2 + vector4, Color.magenta);
			Debug.DrawLine(position2 + vector4, position2 + vector2, Color.magenta);
		}
		this.isAware = false;
		this.previousIsAware = this.isAware;
		return false;
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00027384 File Offset: 0x00025584
	public static void PlayerFiredHisWeapon(Vector3 firePosition)
	{
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
		foreach (UnityEngine.Object @object in array)
		{
			BotAI botAI = (BotAI)@object;
			Vector3 position = botAI.transform.position;
			position.y = firePosition.y;
			if (Vector3.Distance(position, firePosition) < botAI.hearingDistance)
			{
				if (botAI.groupID != "NONE")
				{
					BotAI.lastSeenPositions[botAI.groupID] = firePosition;
				}
				else
				{
					botAI.lastSeenPosition = firePosition;
				}
				if (AnimationHandler.instance != null)
				{
					AnimationHandler.instance.stealthMode = false;
				}
			}
		}
	}

	// Token: 0x040005C4 RID: 1476
	public BotAI.AnimStates animState;

	// Token: 0x040005C5 RID: 1477
	private BotAI.AnimStates previousAnimState;

	// Token: 0x040005C6 RID: 1478
	private int currentAnimState;

	// Token: 0x040005C7 RID: 1479
	private float changeAnimStateTimer;

	// Token: 0x040005C8 RID: 1480
	private static int randomAnimState = -1;

	// Token: 0x040005C9 RID: 1481
	public BotAI.AIState state;

	// Token: 0x040005CA RID: 1482
	public CharacterController cc;

	// Token: 0x040005CB RID: 1483
	private float maxCoverDistance = 20f;

	// Token: 0x040005CC RID: 1484
	private bool facePlayerWhileTakingCover;

	// Token: 0x040005CD RID: 1485
	public Gun weaponPrefab;

	// Token: 0x040005CE RID: 1486
	public Gun weapon;

	// Token: 0x040005CF RID: 1487
	public Gun grenadePrefab;

	// Token: 0x040005D0 RID: 1488
	public Gun grenade;

	// Token: 0x040005D1 RID: 1489
	public Transform rightHandSecondary;

	// Token: 0x040005D2 RID: 1490
	public Transform rightHandPrimary;

	// Token: 0x040005D3 RID: 1491
	public Transform leftHand;

	// Token: 0x040005D4 RID: 1492
	private float WalkStep1StrikeTime = 0.1f;

	// Token: 0x040005D5 RID: 1493
	private float WalkStep2StrikeTime = 0.6f;

	// Token: 0x040005D6 RID: 1494
	private bool emmittingStepSound;

	// Token: 0x040005D7 RID: 1495
	private float burstInterval = 2f;

	// Token: 0x040005D8 RID: 1496
	public float burstTimer;

	// Token: 0x040005D9 RID: 1497
	public float nextBurstTimer;

	// Token: 0x040005DA RID: 1498
	[HideInInspector]
	public GameObject target;

	// Token: 0x040005DB RID: 1499
	public float holdFire;

	// Token: 0x040005DC RID: 1500
	public bool holdingFire;

	// Token: 0x040005DD RID: 1501
	private bool facePlayerWhileShooting = true;

	// Token: 0x040005DE RID: 1502
	private float gravity = 9.8f;

	// Token: 0x040005DF RID: 1503
	private Vector3 moveDirection = Vector3.zero;

	// Token: 0x040005E0 RID: 1504
	public float viewCone = 90f;

	// Token: 0x040005E1 RID: 1505
	public float viewConeLength = 30f;

	// Token: 0x040005E2 RID: 1506
	public string groupID = "NONE";

	// Token: 0x040005E3 RID: 1507
	public Transform[] patrolPath;

	// Token: 0x040005E4 RID: 1508
	public float relaxedRadius = 10f;

	// Token: 0x040005E5 RID: 1509
	public Transform aimPivot;

	// Token: 0x040005E6 RID: 1510
	private float aimAngleY;

	// Token: 0x040005E7 RID: 1511
	private float aimWeight = 1f;

	// Token: 0x040005E8 RID: 1512
	private float uprightWeight = 1f;

	// Token: 0x040005E9 RID: 1513
	[HideInInspector]
	public static Hashtable lastSeenPositions;

	// Token: 0x040005EA RID: 1514
	public Vector3 lastSeenPosition = Vector3.zero;

	// Token: 0x040005EB RID: 1515
	[HideInInspector]
	public bool isAware;

	// Token: 0x040005EC RID: 1516
	[HideInInspector]
	public bool previousIsAware;

	// Token: 0x040005ED RID: 1517
	[HideInInspector]
	public InteractionTrigger currentCover;

	// Token: 0x040005EE RID: 1518
	public BotAI.CoverStates coverState;

	// Token: 0x040005EF RID: 1519
	private float popingOutTimer;

	// Token: 0x040005F0 RID: 1520
	private int coverMask;

	// Token: 0x040005F1 RID: 1521
	private Vector3 coverPosition;

	// Token: 0x040005F2 RID: 1522
	private Vector3 spawnPosition = Vector3.zero;

	// Token: 0x040005F3 RID: 1523
	private Vector3 moveToPoint = Vector3.zero;

	// Token: 0x040005F4 RID: 1524
	private float idleTimer = 3f;

	// Token: 0x040005F5 RID: 1525
	private int currentPathPoint = -1;

	// Token: 0x040005F6 RID: 1526
	private float randSeekRotationTime;

	// Token: 0x040005F7 RID: 1527
	private float randSeekRotation;

	// Token: 0x040005F8 RID: 1528
	private static float lastGrenadeShootTime;

	// Token: 0x040005F9 RID: 1529
	private bool launchProjectile;

	// Token: 0x040005FA RID: 1530
	private float luanchTimer;

	// Token: 0x040005FB RID: 1531
	public AudioClip grenadeLunchSound;

	// Token: 0x040005FC RID: 1532
	public float hearingDistance = 30f;

	// Token: 0x040005FD RID: 1533
	private static float lastSawSomthingSound;

	// Token: 0x040005FE RID: 1534
	public AudioClip[] enemyRelaxedSounds;

	// Token: 0x040005FF RID: 1535
	public AudioClip[] enemyRelaxedSoundsArabic;

	// Token: 0x04000600 RID: 1536
	private static int currentenemyRelaxedSound;

	// Token: 0x04000601 RID: 1537
	private static float lastRelaxedSoundTime;

	// Token: 0x04000602 RID: 1538
	public AudioClip[] enemyEngagedSounds;

	// Token: 0x04000603 RID: 1539
	public AudioClip[] enemyEngagedSoundsArabic;

	// Token: 0x04000604 RID: 1540
	private static int currentenemyEngagedSound;

	// Token: 0x04000605 RID: 1541
	private static float lastEngagedSoundTime;

	// Token: 0x04000606 RID: 1542
	public AudioClip[] enemyTakingCoverSounds;

	// Token: 0x04000607 RID: 1543
	public AudioClip[] enemyTakingCoverSoundsArabic;

	// Token: 0x04000608 RID: 1544
	private static int currentenemyTakingCoverSound;

	// Token: 0x04000609 RID: 1545
	public AudioClip[] enemyJustSawSounds;

	// Token: 0x0400060A RID: 1546
	public AudioClip[] enemyJustSawSoundsArabic;

	// Token: 0x0400060B RID: 1547
	private static int currentEnemyJustSawSound;

	// Token: 0x0400060C RID: 1548
	public bool idleOnRelax;

	// Token: 0x0400060D RID: 1549
	public float distanceBeforeIdle = 10f;

	// Token: 0x0400060E RID: 1550
	public bool turret;

	// Token: 0x0400060F RID: 1551
	public float rotationCone = 180f;

	// Token: 0x04000610 RID: 1552
	private float currentRotation;

	// Token: 0x04000611 RID: 1553
	private float lastRotationSet;

	// Token: 0x04000612 RID: 1554
	public bool launchGrenades = true;

	// Token: 0x04000613 RID: 1555
	public GameObject pickupableGrenadePrefab;

	// Token: 0x04000614 RID: 1556
	public bool startInCover;

	// Token: 0x04000615 RID: 1557
	public AnimationClip EntryAnim;

	// Token: 0x04000616 RID: 1558
	[HideInInspector]
	public bool entered;

	// Token: 0x04000617 RID: 1559
	[HideInInspector]
	public float entryDuration;

	// Token: 0x04000618 RID: 1560
	private int visualMask;

	// Token: 0x04000619 RID: 1561
	public Transform headLookPosition;

	// Token: 0x0400061A RID: 1562
	public bool disableFalling = true;

	// Token: 0x0400061B RID: 1563
	private Transform head;

	// Token: 0x0400061C RID: 1564
	public bool priorityToCover;

	// Token: 0x0400061D RID: 1565
	private bool lookingForCover;

	// Token: 0x0400061E RID: 1566
	private float startingYPosition;

	// Token: 0x0400061F RID: 1567
	public bool enteryStopped;

	// Token: 0x04000620 RID: 1568
	private float currentPitch = 1f;

	// Token: 0x020000F6 RID: 246
	public enum AnimStates
	{
		// Token: 0x04000622 RID: 1570
		IDLE,
		// Token: 0x04000623 RID: 1571
		WALKFORWARD,
		// Token: 0x04000624 RID: 1572
		WALKBACKWARD,
		// Token: 0x04000625 RID: 1573
		STRAFELEFT,
		// Token: 0x04000626 RID: 1574
		STRAFERIGHT,
		// Token: 0x04000627 RID: 1575
		RUN,
		// Token: 0x04000628 RID: 1576
		FALLING,
		// Token: 0x04000629 RID: 1577
		HIT,
		// Token: 0x0400062A RID: 1578
		CROUCH,
		// Token: 0x0400062B RID: 1579
		TAKECOVER,
		// Token: 0x0400062C RID: 1580
		MOVETOPOINT
	}

	// Token: 0x020000F7 RID: 247
	public enum AIState
	{
		// Token: 0x0400062E RID: 1582
		RELAXED,
		// Token: 0x0400062F RID: 1583
		ENGAGED
	}

	// Token: 0x020000F8 RID: 248
	public enum CoverStates
	{
		// Token: 0x04000631 RID: 1585
		TAKING_COVER,
		// Token: 0x04000632 RID: 1586
		IN_COVER,
		// Token: 0x04000633 RID: 1587
		POPING_OUT,
		// Token: 0x04000634 RID: 1588
		EXITING_COVER
	}
}
