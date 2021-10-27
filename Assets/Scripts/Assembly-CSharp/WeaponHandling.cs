using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class WeaponHandling : MonoBehaviour
{
	// Token: 0x060009CE RID: 2510 RVA: 0x0006431C File Offset: 0x0006251C
	private void Awake()
	{
		if (this.gm == null && this.GunManagerPrefab != null)
		{
			this.gm = (GunManager)UnityEngine.Object.Instantiate(this.GunManagerPrefab);
			this.gm.transform.parent = base.transform;
			this.gm.transform.localPosition = new Vector3(3.80304f, -0.1768036f, 14.09909f);
			this.gm.transform.localRotation = Quaternion.Euler(-13.89008f, 47.99628f, -23.29486f);
			foreach (GunKeyBinder gunKeyBinder in this.gm.guns)
			{
				gunKeyBinder.gun.weaponHolder = this;
			}
			this.gm.grenade.weaponHolder = this;
			this.gm.grenade.weaponTransformReference = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
		}
		WeaponHandling.holdFire = false;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00064428 File Offset: 0x00062628
	private void Start()
	{
		if (this.cam == null && this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			this.cam = (ShooterGameCamera)UnityEngine.Object.FindObjectOfType(typeof(ShooterGameCamera));
			this.aimTarget = this.cam.aimTarget;
			this.cam.pivotOffset = Vector3.zero;
		}
		if (this.isTemple)
		{
			this.shootingMode.Scale(new Vector3(1.25f, 1.25f, 1.25f));
			this.aimingMode.Scale(new Vector3(1.25f, 1.25f, 1.25f));
			this.inversedAimingMode.Scale(new Vector3(1.25f, 1.25f, 1.25f));
			this.relaxedMode.Scale(new Vector3(1.25f, 1.25f, 1.25f));
		}
		if (!AnimationHandler.instance.insuredMode)
		{
			this.status = WeaponHandling.WeaponStatus.RELAXED;
		}
		else
		{
			this.status = WeaponHandling.WeaponStatus.ENGAGED;
		}
		this.motor = (base.GetComponent(typeof(NormalCharacterMotor)) as NormalCharacterMotor);
		this.animHandler = base.transform.GetComponent<AnimationHandler>();
		this.charachterStatus = base.gameObject.GetComponent<CharacterStatus>();
		if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
		{
			this.AIScript = base.GetComponent<BotAI>();
		}
		else if (!AnimationHandler.instance.insuredMode)
		{
			this.SetupAdditiveAiming("Handgun-Aiming-Up");
			this.SetupAdditiveAiming("Handgun-Aiming-Down");
			this.SetupAdditiveAiming("Shooting-AK47-Aim-Up");
			this.SetupAdditiveAiming("Shooting-AK47-Aim-Down");
		}
		else
		{
			this.SetupAdditiveAiming("Injured-Aim-Up");
			this.SetupAdditiveAiming("Injured-Aim-Down");
			this.SetupAdditiveAiming("Handgun-Aiming-Up");
			this.SetupAdditiveAiming("Handgun-Aiming-Down");
		}
		if (this.handBone == null)
		{
			this.handBone = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00064624 File Offset: 0x00062824
	private void OnEnable()
	{
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00064628 File Offset: 0x00062828
	private void SetupAdditiveAiming(string anim)
	{
		if ((base.gameObject.tag == "Player" && anim == "Injured-Aim-Up") || anim == "Injured-Aim-Down")
		{
			base.GetComponent<Animation>()[anim].AddMixingTransform(this.rightArm);
		}
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 1f;
		base.GetComponent<Animation>()[anim].layer = 4;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00064700 File Offset: 0x00062900
	private void SetupMixingTransforms(string gunName)
	{
		if (gunName != this.configuredGun)
		{
			if (this.gm != null)
			{
				base.transform.GetComponent<Animation>()[this.gm.currentGun.reloadAnimation.name].AddMixingTransform(this.spineBone);
				if (this.gm.currentGun.idleFreeAnimation != null)
				{
					base.transform.GetComponent<Animation>()[this.gm.currentGun.idleFreeAnimation.name].AddMixingTransform(this.spineBone);
				}
				base.transform.GetComponent<Animation>()[this.gm.currentGun.idleAimedAnimation.name].AddMixingTransform(this.spineBone);
				//base.transform.GetComponent<Animation>()[this.gm.currentGun.fireAimedAnimation.name].AddMixingTransform(this.spineBone);
				//base.transform.GetComponent<Animation>()[this.gm.currentGun.fireFreeAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.gm.currentGun.drawAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.gm.currentGun.holsterAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.gm.currentGun.reloadAnimation.name].layer = 51;
				if (this.gm.currentGun.idleFreeAnimation != null)
				{
					base.transform.GetComponent<Animation>()[this.gm.currentGun.idleFreeAnimation.name].layer = 2;
				}
				base.transform.GetComponent<Animation>()[this.gm.currentGun.idleAimedAnimation.name].layer = 2;
				//base.transform.GetComponent<Animation>()[this.gm.currentGun.fireAimedAnimation.name].layer = 50;
				//base.transform.GetComponent<Animation>()[this.gm.currentGun.fireFreeAnimation.name].layer = 50;
				base.transform.GetComponent<Animation>()[this.gm.currentGun.drawAnimation.name].layer = 2;
				base.transform.GetComponent<Animation>()[this.gm.currentGun.holsterAnimation.name].layer = 2;
				if (this.inCoverFireAnim != null && this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && AnimationHandler.instance.insuredMode)
				{
					base.transform.GetComponent<Animation>()[this.inCoverFireAnim.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.inCoverFireAnim.name].layer = 50;
				}
				if (this.tallCoverIdleFreeSecondaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryL.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryL.name].layer = 2;
				}
				if (this.shortCoverIdleFreeSecondaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryL.name].AddMixingTransform(this.handBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryL.name].layer = 2;
				}
				if (this.tallCoverIdleFreePrimaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryL.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryL.name].layer = 2;
				}
				if (this.shortCoverIdleFreePrimaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryL.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryL.name].layer = 2;
				}
				if (this.tallCoverIdleFreeSecondaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryR.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryR.name].layer = 2;
				}
				if (this.shortCoverIdleFreeSecondaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryR.name].AddMixingTransform(this.handBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryR.name].layer = 2;
				}
				if (this.tallCoverIdleFreePrimaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryR.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryR.name].layer = 2;
				}
				if (this.shortCoverIdleFreePrimaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryR.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryR.name].layer = 2;
				}
				this.configuredGun = gunName;
				WeaponHandling.additiveAimingSetup = false;
			}
			else if (this.weapon != null)
			{
				base.transform.GetComponent<Animation>()[this.weapon.reloadAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.weapon.idleFreeAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.weapon.idleAimedAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.weapon.fireAimedAnimation.name].AddMixingTransform(this.spineBone);
				base.transform.GetComponent<Animation>()[this.weapon.reloadAnimation.name].layer = 5;
				base.transform.GetComponent<Animation>()[this.weapon.idleFreeAnimation.name].layer = 2;
				base.transform.GetComponent<Animation>()[this.weapon.idleAimedAnimation.name].layer = 2;
				base.transform.GetComponent<Animation>()[this.weapon.fireAimedAnimation.name].layer = 50;
				if (this.tallCoverIdleFreeSecondaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryL.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryL.name].layer = 2;
				}
				if (this.shortCoverIdleFreeSecondaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryL.name].AddMixingTransform(this.handBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryL.name].layer = 2;
				}
				if (this.tallCoverIdleFreePrimaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryL.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryL.name].layer = 2;
				}
				if (this.shortCoverIdleFreePrimaryL != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryL.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryL.name].layer = 2;
				}
				if (this.tallCoverIdleFreeSecondaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryR.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreeSecondaryR.name].layer = 2;
				}
				if (this.shortCoverIdleFreeSecondaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryR.name].AddMixingTransform(this.handBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreeSecondaryR.name].layer = 2;
				}
				if (this.tallCoverIdleFreePrimaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryR.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.tallCoverIdleFreePrimaryR.name].layer = 2;
				}
				if (this.shortCoverIdleFreePrimaryR != null)
				{
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryR.name].AddMixingTransform(this.spineBone);
					base.transform.GetComponent<Animation>()[this.shortCoverIdleFreePrimaryR.name].layer = 2;
				}
				this.configuredGun = gunName;
				WeaponHandling.additiveAimingSetup = false;
			}
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00065150 File Offset: 0x00063350
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.gm != null && this.gm.currentGun != null)
		{
			this.SetupMixingTransforms(this.gm.currentGun.gunName);
		}
		else if (this.weapon != null)
		{
			this.SetupMixingTransforms(this.weapon.gunName);
		}
		if (AnimationHandler.instance != null && AnimationHandler.instance.animState != AnimationHandler.AnimStates.JUMPING && AnimationHandler.instance.animState != AnimationHandler.AnimStates.FALLING && this.cam != null && !this.cam.fixedCameraPosition)
		{
			this.aim = false;
			//this.aim = MobileInput.aim;
			this.aim = InputManager.GetButton("Fire2");
			//this.aim = (this.aim || Input.GetAxisRaw("Fire2A") > 0.3f);
			this.aimPressed = false;
			if (this.aim && this.aim != this.previousAim)
			{
				this.aimPressed = true;
			}
			if (this.aim != this.previousAim && MobileInput.instance != null && (!AndroidPlatform.IsJoystickConnected() || (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetJoystickNames()[0].Contains("basic"))))
			{
				if (this.aim && MobileInput.gotGrenade && !MobileInput.grenadeDisabled)
				{
					MobileInput.instance.grenadeButton.enabled = this.aim;
				}
				else
				{
					MobileInput.instance.grenadeButton.enabled = false;
				}
			}
			this.previousAim = this.aim;
			this.aimPressed = InputManager.GetButtonDown("Fire2");
		}
		if (this.cam != null && this.cam.fixedCameraPosition)
		{
			this.aim = false;
			this.aimPressed = false;
		}
		if (this.pressAim)
		{
			this.aim = true;
			this.aimPressed = true;
			this.pressAim = false;
		}
		if (this.resetAiming)
		{
			this.aim = false;
			this.aimPressed = false;
			this.resetAiming = false;
			this.pressAim = true;
		}
		if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.NPC || this.gm.currentGun == null)
		{
			return;
		}
		if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && this.disableUsingWeapons)
		{
			if (this.prevousDisableUsingWeapons != this.disableUsingWeapons)
			{
				if (!AnimationHandler.instance.insuredMode)
				{
					Transform transform;
					if (this.gm.currentGun.WeaponType == WeaponTypes.PRIMARY)
					{
						transform = this.PrimaryPocket;
					}
					else
					{
						transform = this.SecondaryPocket;
					}
					this.gm.currentGun.weaponTransform.transform.position = transform.position;
					this.gm.currentGun.weaponTransform.transform.rotation = transform.rotation;
					this.gm.currentGun.weaponTransform.parent = transform;
					if (this.gm.currentGun.pocketOffsetPosition != Vector3.zero || this.gm.currentGun.pocketOffsetRotation != Vector3.zero)
					{
						this.gm.currentGun.weaponTransform.transform.Translate(this.gm.currentGun.pocketOffsetPosition);
						this.gm.currentGun.weaponTransform.transform.Rotate(this.gm.currentGun.pocketOffsetRotation, Space.Self);
					}
					if (this.gm.currentGun.WeaponType == WeaponTypes.PRIMARY)
					{
						float x = 0f;
						foreach (WeaponHandling.WeaponOffset weaponOffset in this.weaponOffsets)
						{
							if (weaponOffset.weaponName == this.gm.currentGun.gunName)
							{
								x = weaponOffset.offset;
							}
						}
						this.gm.currentGun.weaponTransform.transform.Translate(x, 0f, 0f);
					}
					this.status = WeaponHandling.WeaponStatus.RELAXED;
					this.ExitShootingMode();
					this.cam.aim = false;
					this.cam.shoot = false;
					if (this.gm.currentGun.idleAimedAnimation != null)
					{
						base.GetComponent<Animation>().Stop(this.gm.currentGun.idleAimedAnimation.name);
					}
					if (this.gm.currentGun.idleFreeAnimation != null)
					{
						base.GetComponent<Animation>().Stop(this.gm.currentGun.idleFreeAnimation.name);
					}
				}
				this.cam.reticle = null;
				WeaponHandling.additiveAimingSetup = false;
				this.prevousDisableUsingWeapons = this.disableUsingWeapons;
			}
			return;
		}
		this.prevousDisableUsingWeapons = this.disableUsingWeapons;
		if (!WeaponHandling.additiveAimingSetup)
		{
			if (!AnimationHandler.instance.insuredMode)
			{
				this.SetupAdditiveAiming("Handgun-Aiming-Up");
				this.SetupAdditiveAiming("Handgun-Aiming-Down");
				this.SetupAdditiveAiming("Shooting-AK47-Aim-Up");
				this.SetupAdditiveAiming("Shooting-AK47-Aim-Down");
			}
			else
			{
				this.SetupAdditiveAiming("Injured-Aim-Up");
				this.SetupAdditiveAiming("Injured-Aim-Down");
				this.SetupAdditiveAiming("Handgun-Aiming-Up");
				this.SetupAdditiveAiming("Handgun-Aiming-Down");
			}
			WeaponHandling.additiveAimingSetup = true;
		}
		if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			if (this.previousInCover != this.inCover)
			{
				if (this.status != WeaponHandling.WeaponStatus.RELAXED)
				{
					this.EnterIdleFree();
					this.EnterShootingMode();
				}
				this.cam.inCover = this.inCover;
				this.previousInCover = this.inCover;
			}
			if (this.aimPressed && !this.disableAiming && (this.status == WeaponHandling.WeaponStatus.SHOOTING || this.status == WeaponHandling.WeaponStatus.ENGAGED))
			{
				this.status = WeaponHandling.WeaponStatus.AIMING;
				this.EnterAimingMode();
				this.EnterIdleAimed();
				this.cam.reticle = this.gm.currentGun.crosshairWhite;
			}
			else if (this.aimPressed && !this.disableAiming && this.status == WeaponHandling.WeaponStatus.RELAXED)
			{
				this.Draw();
				this.status = WeaponHandling.WeaponStatus.AIMING;
				this.EnterAimingMode();
				this.drawingTimer = this.gm.currentGun.drawAnimation.length / 3f * 0.75f;
				this.drawToAim = true;
				this.cam.reticle = this.gm.currentGun.crosshairWhite;
			}
			else if (!this.aim && this.status == WeaponHandling.WeaponStatus.AIMING && !this.disableAiming)
			{
				if (this.gm.currentGun.idleAimedAnimation != null)
				{
					base.GetComponent<Animation>().Stop(this.gm.currentGun.idleAimedAnimation.name);
				}
				this.status = WeaponHandling.WeaponStatus.ENGAGED;
				this.EnterShootingMode();
				this.EnterIdleFree();
				if (AnimationHandler.instance != null && AnimationHandler.instance.insuredMode)
				{
					this.cam.reticle = null;
				}
				else
				{
					this.cam.reticle = this.crossHairWhite;
				}
				this.cam.aim = false;
			}
			if ((this.status == WeaponHandling.WeaponStatus.AIMING || this.status == WeaponHandling.WeaponStatus.SHOOTING || this.status == WeaponHandling.WeaponStatus.ENGAGED) && (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.JoystickButton9)) && AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS)
			{
				this.inversedAiming = !this.inversedAiming;
			}
			if (this.status == WeaponHandling.WeaponStatus.AIMING || this.status == WeaponHandling.WeaponStatus.SHOOTING)
			{
				this.animHandler.isShooting = true;
				this.cam.smoothingTime = 0f;
				if (!AnimationHandler.instance.insuredMode)
				{
					if (this.previousMaxForwardSpeed == 0f)
					{
						this.previousMaxForwardSpeed = this.motor.maxForwardSpeed;
						this.previousMaxBackwardsSpeed = this.motor.maxBackwardsSpeed;
						this.previousMaxSidewaysSpeed = this.motor.maxSidewaysSpeed;
					}
					this.motor.maxForwardSpeed = 2f;
					this.motor.maxBackwardsSpeed = 2f;
					this.motor.maxSidewaysSpeed = 1.5f;
				}
				Vector3 vector = this.aimTarget.position - base.transform.position;
				vector.y = 0f;
				if (this.yRotationOffset != 0f)
				{
					vector = Quaternion.Euler(0f, this.yRotationOffset, 0f) * vector;
				}
				if (this.motor.desiredMovementDirection.sqrMagnitude > 0.1f || Vector3.Angle(base.transform.forward, vector) > this.maxHorizontalAimAngle)
				{
					this.motor.desiredFacingDirection = Vector3.SmoothDamp(this.motor.desiredFacingDirection, vector, ref this.currentLookAtVector, 0.1f);
				}
				if (this.status == WeaponHandling.WeaponStatus.AIMING || this.status == WeaponHandling.WeaponStatus.SHOOTING || this.status == WeaponHandling.WeaponStatus.ENGAGED)
				{
					this.aimWeight = this.CrossFadeUp(this.aimWeight, 0.3f);
				}
				else
				{
					this.aimWeight = this.CrossFadeDown(this.aimWeight, 0.3f);
				}
			}
			else
			{
				this.animHandler.isShooting = false;
				this.cam.smoothingTime = 0.15f;
				if (!AnimationHandler.instance.insuredMode && this.previousMaxForwardSpeed != 0f)
				{
					this.motor.maxForwardSpeed = this.previousMaxForwardSpeed;
					this.motor.maxBackwardsSpeed = this.previousMaxBackwardsSpeed;
					this.motor.maxSidewaysSpeed = this.previousMaxSidewaysSpeed;
				}
				this.motor.desiredFacingDirection = Vector3.zero;
				this.aimWeight = this.CrossFadeDown(this.aimWeight, 0.3f);
			}
			if (this.pockiting)
			{
				this.Pocket();
			}
			if (this.unpockiting)
			{
				this.UnPocket();
			}
			if (this.drawToAim)
			{
				if (this.drawingTimer <= 0f)
				{
					this.drawToAim = false;
					if (this.aim)
					{
						this.EnterIdleAimed();
					}
				}
				else
				{
					this.drawingTimer -= Time.deltaTime;
				}
			}
			float to = Mathf.Asin((this.aimTarget.position - this.aimPivot.position).normalized.y) * 57.29578f;
			this.aimAngleY = Mathf.Lerp(this.aimAngleY, to, Time.deltaTime * 8f);
			if (!AnimationHandler.instance.insuredMode || this.inCover)
			{
				if (this.blindFire || (this.inCover && !this.aim))
				{
					base.GetComponent<Animation>()["Shooting-AK47-Aim-Up"].weight = 0f;
					base.GetComponent<Animation>()["Shooting-AK47-Aim-Down"].weight = 0f;
					base.GetComponent<Animation>()["Handgun-Aiming-Up"].weight = 0f;
					base.GetComponent<Animation>()["Handgun-Aiming-Down"].weight = 0f;
				}
				else
				{
					if (AnimationHandler.instance.insuredMode)
					{
						base.GetComponent<Animation>()["Injured-Aim-Up"].weight = 0f;
						base.GetComponent<Animation>()["Injured-Aim-Down"].weight = 0f;
					}
					if (this.gm.currentGun.WeaponType == WeaponTypes.SECONDARY)
					{
						base.GetComponent<Animation>()["Shooting-AK47-Aim-Up"].weight = 0f;
						base.GetComponent<Animation>()["Shooting-AK47-Aim-Down"].weight = 0f;
						base.GetComponent<Animation>()["Handgun-Aiming-Up"].weight = this.uprightWeight * this.aimWeight;
						base.GetComponent<Animation>()["Handgun-Aiming-Down"].weight = this.uprightWeight * this.aimWeight;
					}
					else
					{
						base.GetComponent<Animation>()["Handgun-Aiming-Up"].weight = 0f;
						base.GetComponent<Animation>()["Handgun-Aiming-Down"].weight = 0f;
						base.GetComponent<Animation>()["Shooting-AK47-Aim-Up"].weight = this.uprightWeight * this.aimWeight;
						base.GetComponent<Animation>()["Shooting-AK47-Aim-Down"].weight = this.uprightWeight * this.aimWeight;
					}
				}
			}
			else
			{
				base.GetComponent<Animation>()["Handgun-Aiming-Up"].weight = 0f;
				base.GetComponent<Animation>()["Handgun-Aiming-Down"].weight = 0f;
				base.GetComponent<Animation>()["Injured-Aim-Up"].weight = this.uprightWeight * this.aimWeight;
				base.GetComponent<Animation>()["Injured-Aim-Down"].weight = this.uprightWeight * this.aimWeight;
			}
			if (!AnimationHandler.instance.insuredMode || this.inCover)
			{
				if (this.gm.currentGun.WeaponType == WeaponTypes.SECONDARY)
				{
					base.GetComponent<Animation>()["Handgun-Aiming-Up"].time = Mathf.Clamp01(this.aimAngleY / 90f) * 1.1f;
					base.GetComponent<Animation>()["Handgun-Aiming-Down"].time = Mathf.Clamp01(-this.aimAngleY / 90f);
				}
				else
				{
					base.GetComponent<Animation>()["Shooting-AK47-Aim-Up"].time = Mathf.Clamp01(this.aimAngleY / 90f) * 0.9f;
					base.GetComponent<Animation>()["Shooting-AK47-Aim-Down"].time = Mathf.Clamp01(-this.aimAngleY / 90f);
				}
			}
			else
			{
				base.GetComponent<Animation>()["Injured-Aim-Up"].time = Mathf.Clamp01(this.aimAngleY / 90f);
				base.GetComponent<Animation>()["Injured-Aim-Down"].time = Mathf.Clamp01(-this.aimAngleY / 80f);
			}
			if (this.status != WeaponHandling.WeaponStatus.RELAXED)
			{
				this.animHandler.isholdingWeapon = true;
				this.cam.allowCircleCamera = false;
			}
			else
			{
				this.animHandler.isholdingWeapon = false;
				this.cam.allowCircleCamera = true;
			}
		}
		if (this.previousInversedAiming != this.inversedAiming || this.inCover)
		{
			if (this.status == WeaponHandling.WeaponStatus.AIMING)
			{
				this.EnterAimingMode();
			}
			this.cam.inversedAiming = this.inversedAiming;
			this.previousInversedAiming = this.inversedAiming;
		}
		if (AnimationHandler.instance.insuredMode)
		{
			if (this.status == WeaponHandling.WeaponStatus.ENGAGED)
			{
				base.GetComponent<Animation>()["Injured-Aim"].weight = Mathf.Clamp01(base.GetComponent<Animation>()["Injured-Aim"].weight - 2f * Time.deltaTime);
				base.GetComponent<Animation>()["Injured-Shoot"].weight = Mathf.Clamp01(base.GetComponent<Animation>()["Injured-Aim"].weight - 2f * Time.deltaTime);
				this.cam.shoot = false;
			}
			else
			{
				base.GetComponent<Animation>()["Injured-Aim"].weight = Mathf.Clamp01(base.GetComponent<Animation>()["Injured-Aim"].weight + 4f * Time.deltaTime);
				base.GetComponent<Animation>()["Injured-Shoot"].weight = 1f;
				this.cam.shoot = true;
			}
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000661F0 File Offset: 0x000643F0
	private float CrossFadeUp(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight + Time.deltaTime / fadeTime);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00066200 File Offset: 0x00064400
	private float CrossFadeDown(float weight, float fadeTime)
	{
		return Mathf.Clamp01(weight - Time.deltaTime / fadeTime);
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00066210 File Offset: 0x00064410
	public void Reload()
	{
		if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && this.farisReloadEnemySounds.Length > 0)
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
			foreach (UnityEngine.Object obj in array)
			{
				BotAI botAI = (BotAI)obj;
				if (botAI.isAware)
				{
					if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.farisReloadEnemySoundsArabic[WeaponHandling.currentfarisReloadEnemySound] != null)
					{
						botAI.GetComponent<AudioSource>().PlayOneShot(this.farisReloadEnemySoundsArabic[WeaponHandling.currentfarisReloadEnemySound], SpeechManager.speechVolume);
						WeaponHandling.currentfarisReloadEnemySound = (WeaponHandling.currentfarisReloadEnemySound + 1) % this.farisReloadEnemySoundsArabic.Length;
					}
					else
					{
						botAI.GetComponent<AudioSource>().PlayOneShot(this.farisReloadEnemySounds[WeaponHandling.currentfarisReloadEnemySound], SpeechManager.speechVolume);
						WeaponHandling.currentfarisReloadEnemySound = (WeaponHandling.currentfarisReloadEnemySound + 1) % this.farisReloadEnemySounds.Length;
					}
					break;
				}
			}
		}
		else if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.NPC && this.enemyReloadSounds.Length > 0 && ((Time.time < 15f && WeaponHandling.lastEnemyReloadSound == 0f) || Time.time > WeaponHandling.lastEnemyReloadSound + 15f))
		{
			if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.enemyReloadSoundsArabic[WeaponHandling.currentEnemyReloadSound] != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.enemyReloadSoundsArabic[WeaponHandling.currentEnemyReloadSound], SpeechManager.speechVolume);
				WeaponHandling.currentEnemyReloadSound = (WeaponHandling.currentEnemyReloadSound + 1) % this.enemyReloadSoundsArabic.Length;
			}
			else
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.enemyReloadSounds[WeaponHandling.currentEnemyReloadSound], SpeechManager.speechVolume);
				WeaponHandling.currentEnemyReloadSound = (WeaponHandling.currentEnemyReloadSound + 1) % this.enemyReloadSounds.Length;
			}
			WeaponHandling.lastEnemyReloadSound = Time.time;
		}
		if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			this.RepositionTheCurrentWeaponInHand();
		}
		AnimationClip reloadAnimation;
		if (this.gm != null)
		{
			reloadAnimation = this.gm.currentGun.reloadAnimation;
		}
		else
		{
			reloadAnimation = this.weapon.reloadAnimation;
		}
		if (reloadAnimation != null)
		{
			base.transform.GetComponent<Animation>()[reloadAnimation.name].time = 0f;
			base.transform.GetComponent<Animation>()[reloadAnimation.name].wrapMode = WrapMode.Once;
			base.transform.GetComponent<Animation>()[reloadAnimation.name].speed = 1.5f;
			base.transform.GetComponent<Animation>().Blend(reloadAnimation.name, reloadAnimation.length);
		}
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x000664B8 File Offset: 0x000646B8
	public void RepositionTheCurrentWeaponInHand()
	{
		Transform transform;
		if (this.gm.currentGun.WeaponType == WeaponTypes.PRIMARY)
		{
			transform = this.PrimaryRightHand;
		}
		else
		{
			transform = this.SecondaryRightHand;
		}
		this.gm.currentGun.weaponTransform.transform.position = transform.position;
		this.gm.currentGun.weaponTransform.transform.rotation = transform.rotation;
		this.gm.currentGun.weaponTransform.parent = transform;
		if (this.gm.currentGun.rightHandOffsetPosition != Vector3.zero || this.gm.currentGun.rightHandOffsetRotation != Vector3.zero)
		{
			this.gm.currentGun.weaponTransform.transform.Translate(this.gm.currentGun.rightHandOffsetPosition);
			this.gm.currentGun.weaponTransform.transform.Rotate(this.gm.currentGun.rightHandOffsetRotation, Space.Self);
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000665D8 File Offset: 0x000647D8
	public void EnterIdleFree()
	{
		if (this.blindFire)
		{
			return;
		}
		AnimationClip animationClip = null;
		if (this.gm != null)
		{
			if (!this.inCover)
			{
				animationClip = this.gm.currentGun.idleFreeAnimation;
			}
			else
			{
				WeaponTypes weaponType = this.gm.currentGun.WeaponType;
				if (weaponType != WeaponTypes.PRIMARY)
				{
					if (weaponType == WeaponTypes.SECONDARY)
					{
						if (this.tallCover)
						{
							if (Interaction.playerInteraction.movingRightDirection)
							{
								animationClip = this.tallCoverIdleFreeSecondaryR;
							}
							else
							{
								animationClip = this.tallCoverIdleFreeSecondaryL;
							}
						}
						else if (Interaction.playerInteraction.movingRightDirection)
						{
							animationClip = this.shortCoverIdleFreeSecondaryR;
						}
						else
						{
							animationClip = this.shortCoverIdleFreeSecondaryL;
						}
					}
				}
				else if (this.tallCover)
				{
					if (Interaction.playerInteraction.movingRightDirection)
					{
						animationClip = this.tallCoverIdleFreePrimaryR;
					}
					else
					{
						animationClip = this.tallCoverIdleFreePrimaryL;
					}
				}
				else if (Interaction.playerInteraction.movingRightDirection)
				{
					animationClip = this.shortCoverIdleFreePrimaryR;
				}
				else
				{
					animationClip = this.shortCoverIdleFreePrimaryL;
				}
			}
		}
		else if (this.AIScript != null && this.AIScript.coverState == BotAI.CoverStates.IN_COVER)
		{
			WeaponTypes weaponType = this.weapon.WeaponType;
			if (weaponType != WeaponTypes.PRIMARY)
			{
				if (weaponType == WeaponTypes.SECONDARY)
				{
					if (this.AIScript.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
					{
						animationClip = this.tallCoverIdleFreeSecondaryL;
					}
					else
					{
						animationClip = this.shortCoverIdleFreeSecondaryL;
					}
				}
			}
			else if (this.AIScript.currentCover.TriggerType == InteractionTrigger.InteractionTypes.COVERTALL)
			{
				animationClip = this.tallCoverIdleFreePrimaryL;
			}
			else
			{
				animationClip = this.shortCoverIdleFreePrimaryL;
			}
		}
		else
		{
			animationClip = this.weapon.idleFreeAnimation;
		}
		if (animationClip != null)
		{
			base.transform.GetComponent<Animation>()[animationClip.name].time = 0f;
			base.transform.GetComponent<Animation>()[animationClip.name].wrapMode = WrapMode.Loop;
			base.transform.GetComponent<Animation>().CrossFade(animationClip.name);
		}
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x0006680C File Offset: 0x00064A0C
	public void EnterIdleAimed()
	{
		AnimationClip idleAimedAnimation;
		if (this.gm != null)
		{
			if (!this.inCover || !AnimationHandler.instance.insuredMode)
			{
				idleAimedAnimation = this.gm.currentGun.idleAimedAnimation;
			}
			else
			{
				idleAimedAnimation = this.inCoverIdleAimAnim;
			}
		}
		else
		{
			idleAimedAnimation = this.weapon.idleAimedAnimation;
		}
		if (idleAimedAnimation != null && !base.GetComponent<Animation>().IsPlaying(idleAimedAnimation.name))
		{
			base.transform.GetComponent<Animation>()[idleAimedAnimation.name].time = 0f;
			base.transform.GetComponent<Animation>()[idleAimedAnimation.name].wrapMode = WrapMode.Loop;
			base.transform.GetComponent<Animation>()[idleAimedAnimation.name].speed = 2f;
			base.transform.GetComponent<Animation>().CrossFade(idleAimedAnimation.name);
		}
		WeaponHandling.additiveAimingSetup = false;
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0006690C File Offset: 0x00064B0C
	public void Fire()
	{
		AnimationClip animationClip;
		if (this.status == WeaponHandling.WeaponStatus.AIMING)
		{
			if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && AnimationHandler.instance.insuredMode && this.inCover)
			{
				animationClip = this.inCoverFireAnim;
			}
			else if (this.gm != null)
			{
				animationClip = this.gm.currentGun.fireAimedAnimation;
			}
			else
			{
				animationClip = this.weapon.fireAimedAnimation;
			}
		}
		else if (this.gm != null)
		{
			animationClip = this.gm.currentGun.fireFreeAnimation;
		}
		else
		{
			animationClip = this.weapon.fireFreeAnimation;
		}
		if (animationClip != null)
		{
			base.transform.GetComponent<Animation>()[animationClip.name].time = 0f;
			base.transform.GetComponent<Animation>()[animationClip.name].wrapMode = WrapMode.Once;
			if (!base.GetComponent<Animation>().IsPlaying(animationClip.name))
			{
				base.transform.GetComponent<Animation>().Play(animationClip.name, PlayMode.StopSameLayer);
			}
		}
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x00066A38 File Offset: 0x00064C38
	public void Draw()
	{
		AnimationClip drawAnimation = this.gm.currentGun.drawAnimation;
		if (drawAnimation != null)
		{
			base.transform.GetComponent<Animation>()[drawAnimation.name].time = 0f;
			base.transform.GetComponent<Animation>()[drawAnimation.name].wrapMode = WrapMode.Once;
			base.transform.GetComponent<Animation>()[drawAnimation.name].speed = 3f;
			base.transform.GetComponent<Animation>().CrossFade(drawAnimation.name, 0.1f);
			if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
			{
				this.FarisHead.PlayOneShot(this.DrawBodySound, SpeechManager.sfxVolume);
			}
		}
		if (this.status == WeaponHandling.WeaponStatus.RELAXED)
		{
			this.EnterShootingMode();
		}
		this.cam.reticle = this.crossHairWhite;
		this.unpockiting = true;
		this.unpockitingGun = this.gm.currentGun;
		if (this.aim)
		{
			this.status = WeaponHandling.WeaponStatus.AIMING;
			this.EnterAimingMode();
		}
		else
		{
			this.status = WeaponHandling.WeaponStatus.SHOOTING;
		}
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00066B58 File Offset: 0x00064D58
	public void StartDrawn()
	{
		if (this.status == WeaponHandling.WeaponStatus.RELAXED)
		{
			this.EnterShootingMode();
		}
		this.status = WeaponHandling.WeaponStatus.SHOOTING;
		this.unpockiting = true;
		this.unpockitingGun = this.gm.currentGun;
		if (this.aim)
		{
			this.status = WeaponHandling.WeaponStatus.AIMING;
			this.EnterAimingMode();
		}
		else
		{
			this.status = WeaponHandling.WeaponStatus.SHOOTING;
		}
		Transform transform;
		if (this.unpockitingGun.WeaponType == WeaponTypes.PRIMARY)
		{
			transform = this.PrimaryRightHand;
		}
		else
		{
			transform = this.SecondaryRightHand;
		}
		this.unpockitingGun.weaponTransform.transform.position = transform.position;
		this.unpockitingGun.weaponTransform.transform.rotation = transform.rotation;
		this.unpockitingGun.weaponTransform.parent = transform;
		if (this.unpockitingGun.rightHandOffsetPosition != Vector3.zero || this.unpockitingGun.rightHandOffsetRotation != Vector3.zero)
		{
			this.unpockitingGun.weaponTransform.transform.Translate(this.unpockitingGun.rightHandOffsetPosition);
			this.unpockitingGun.weaponTransform.transform.Rotate(this.unpockitingGun.rightHandOffsetRotation, Space.Self);
		}
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00066C98 File Offset: 0x00064E98
	public void Holster()
	{
		AnimationClip holsterAnimation = this.gm.currentGun.holsterAnimation;
		if (holsterAnimation != null)
		{
			base.transform.GetComponent<Animation>()[holsterAnimation.name].time = 0f;
			base.transform.GetComponent<Animation>()[holsterAnimation.name].wrapMode = WrapMode.Once;
			base.transform.GetComponent<Animation>()[holsterAnimation.name].speed = 3f;
			base.transform.GetComponent<Animation>().CrossFade(holsterAnimation.name);
			if (this.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
			{
				this.FarisHead.PlayOneShot(this.HolsterBodySound, SpeechManager.sfxVolume);
			}
		}
		if (this.status == WeaponHandling.WeaponStatus.RELAXED)
		{
			this.ExitShootingMode();
		}
		this.cam.reticle = null;
		this.pockiting = true;
		this.pockitingGun = this.gm.currentGun;
		this.status = WeaponHandling.WeaponStatus.RELAXED;
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00066D94 File Offset: 0x00064F94
	private void Pocket()
	{
		if (this.pockitingTimer <= 0f)
		{
			this.pockitingTimer = this.pockitingGun.holsterAnimation.length / 3f * 0.63f;
		}
		this.pockitingTimer -= Time.deltaTime;
		if (this.pockitingTimer <= 0f)
		{
			Transform transform;
			if (this.pockitingGun.WeaponType == WeaponTypes.PRIMARY)
			{
				transform = this.PrimaryPocket;
			}
			else
			{
				transform = this.SecondaryPocket;
			}
			this.pockitingGun.weaponTransform.transform.position = transform.position;
			this.pockitingGun.weaponTransform.transform.rotation = transform.rotation;
			this.pockitingGun.weaponTransform.parent = transform;
			if (this.pockitingGun.pocketOffsetPosition != Vector3.zero || this.pockitingGun.pocketOffsetRotation != Vector3.zero)
			{
				this.pockitingGun.weaponTransform.transform.Translate(this.pockitingGun.pocketOffsetPosition);
				this.pockitingGun.weaponTransform.transform.Rotate(this.pockitingGun.pocketOffsetRotation, Space.Self);
			}
			if (this.pockitingGun.WeaponType == WeaponTypes.PRIMARY)
			{
				float x = 0f;
				foreach (WeaponHandling.WeaponOffset weaponOffset in this.weaponOffsets)
				{
					if (weaponOffset.weaponName == this.pockitingGun.gunName)
					{
						x = weaponOffset.offset;
					}
				}
				this.pockitingGun.weaponTransform.transform.Translate(x, 0f, 0f);
			}
			this.pockiting = false;
		}
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00066F58 File Offset: 0x00065158
	private void UnPocket()
	{
		if (this.unpockitingTimer <= 0f)
		{
			this.unpockitingTimer = this.unpockitingGun.drawAnimation.length / 3f * 0.28f;
			if (this.pockiting && this.unpockitingGun == this.pockitingGun)
			{
				this.pockiting = false;
				this.pockitingTimer = 0f;
			}
		}
		this.unpockitingTimer -= Time.deltaTime;
		if (this.unpockitingTimer <= 0f)
		{
			Transform transform;
			if (this.unpockitingGun.WeaponType == WeaponTypes.PRIMARY)
			{
				transform = this.PrimaryRightHand;
			}
			else
			{
				transform = this.SecondaryRightHand;
			}
			this.unpockitingGun.weaponTransform.transform.position = transform.position;
			this.unpockitingGun.weaponTransform.transform.rotation = transform.rotation;
			this.unpockitingGun.weaponTransform.parent = transform;
			if (this.unpockitingGun.rightHandOffsetPosition != Vector3.zero || this.unpockitingGun.rightHandOffsetRotation != Vector3.zero)
			{
				this.unpockitingGun.weaponTransform.transform.Translate(this.unpockitingGun.rightHandOffsetPosition);
				this.unpockitingGun.weaponTransform.transform.Rotate(this.unpockitingGun.rightHandOffsetRotation, Space.Self);
			}
			this.unpockiting = false;
		}
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x000670D4 File Offset: 0x000652D4
	public void EnterShootingMode()
	{
		this.cam.shoot = true;
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x000670E4 File Offset: 0x000652E4
	public void EnterAimingMode()
	{
		this.cam.aim = true;
		this.cam.SelectAimTarget();
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x00067100 File Offset: 0x00065300
	private void EnterInversedAimingMode()
	{
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00067104 File Offset: 0x00065304
	private void ExitShootingMode()
	{
		if (!AnimationHandler.instance.insuredMode)
		{
			this.cam.shoot = false;
		}
	}

	// Token: 0x04000ECA RID: 3786
	public WeaponHandling.WeaponHolderTypes WeaponHolderType;

	// Token: 0x04000ECB RID: 3787
	public Gun weapon;

	// Token: 0x04000ECC RID: 3788
	public ShooterGameCamera cam;

	// Token: 0x04000ECD RID: 3789
	public Transform spineBone;

	// Token: 0x04000ECE RID: 3790
	public Transform handBone;

	// Token: 0x04000ECF RID: 3791
	public Transform aimTarget;

	// Token: 0x04000ED0 RID: 3792
	public Transform aimPivot;

	// Token: 0x04000ED1 RID: 3793
	private float aimAngleY;

	// Token: 0x04000ED2 RID: 3794
	private float aimWeight;

	// Token: 0x04000ED3 RID: 3795
	private float uprightWeight = 1f;

	// Token: 0x04000ED4 RID: 3796
	public float forwardOffset;

	// Token: 0x04000ED5 RID: 3797
	public Transform spine;

	// Token: 0x04000ED6 RID: 3798
	public Transform SecondaryPocket;

	// Token: 0x04000ED7 RID: 3799
	public Transform SecondaryRightHand;

	// Token: 0x04000ED8 RID: 3800
	public Transform PrimaryPocket;

	// Token: 0x04000ED9 RID: 3801
	public Transform PrimaryRightHand;

	// Token: 0x04000EDA RID: 3802
	public Texture crossHairWhite;

	// Token: 0x04000EDB RID: 3803
	public Texture crossHairRed;

	// Token: 0x04000EDC RID: 3804
	public AudioSource FarisHead;

	// Token: 0x04000EDD RID: 3805
	public AudioClip DrawBodySound;

	// Token: 0x04000EDE RID: 3806
	public AudioClip HolsterBodySound;

	// Token: 0x04000EDF RID: 3807
	public AudioClip[] enemyReloadSounds;

	// Token: 0x04000EE0 RID: 3808
	public AudioClip[] farisReloadEnemySounds;

	// Token: 0x04000EE1 RID: 3809
	public AudioClip[] enemyReloadSoundsArabic;

	// Token: 0x04000EE2 RID: 3810
	public AudioClip[] farisReloadEnemySoundsArabic;

	// Token: 0x04000EE3 RID: 3811
	private static int currentEnemyReloadSound;

	// Token: 0x04000EE4 RID: 3812
	private static int currentfarisReloadEnemySound;

	// Token: 0x04000EE5 RID: 3813
	public float maxHorizontalAimAngle = 50f;

	// Token: 0x04000EE6 RID: 3814
	private float drawingTimer;

	// Token: 0x04000EE7 RID: 3815
	private bool drawToAim;

	// Token: 0x04000EE8 RID: 3816
	private Vector3 currentLookAtVector;

	// Token: 0x04000EE9 RID: 3817
	[HideInInspector]
	public bool inversedAiming;

	// Token: 0x04000EEA RID: 3818
	private bool previousInversedAiming;

	// Token: 0x04000EEB RID: 3819
	private PlatformCharacterController cc;

	// Token: 0x04000EEC RID: 3820
	public WeaponHandling.WeaponStatus status;

	// Token: 0x04000EED RID: 3821
	[HideInInspector]
	public BotAI AIScript;

	// Token: 0x04000EEE RID: 3822
	[HideInInspector]
	public CharacterStatus charachterStatus;

	// Token: 0x04000EEF RID: 3823
	public GunManager gm;

	// Token: 0x04000EF0 RID: 3824
	public AnimationClip tallCoverIdleFreeSecondaryL;

	// Token: 0x04000EF1 RID: 3825
	public AnimationClip shortCoverIdleFreeSecondaryL;

	// Token: 0x04000EF2 RID: 3826
	public AnimationClip tallCoverIdleFreePrimaryL;

	// Token: 0x04000EF3 RID: 3827
	public AnimationClip shortCoverIdleFreePrimaryL;

	// Token: 0x04000EF4 RID: 3828
	public AnimationClip tallCoverIdleFreeSecondaryR;

	// Token: 0x04000EF5 RID: 3829
	public AnimationClip shortCoverIdleFreeSecondaryR;

	// Token: 0x04000EF6 RID: 3830
	public AnimationClip tallCoverIdleFreePrimaryR;

	// Token: 0x04000EF7 RID: 3831
	public AnimationClip shortCoverIdleFreePrimaryR;

	// Token: 0x04000EF8 RID: 3832
	private NormalCharacterMotor motor;

	// Token: 0x04000EF9 RID: 3833
	private AnimationHandler animHandler;

	// Token: 0x04000EFA RID: 3834
	[HideInInspector]
	public bool pockiting;

	// Token: 0x04000EFB RID: 3835
	[HideInInspector]
	public float pockitingTimer;

	// Token: 0x04000EFC RID: 3836
	[HideInInspector]
	public Gun pockitingGun;

	// Token: 0x04000EFD RID: 3837
	[HideInInspector]
	public bool unpockiting;

	// Token: 0x04000EFE RID: 3838
	[HideInInspector]
	public float unpockitingTimer;

	// Token: 0x04000EFF RID: 3839
	[HideInInspector]
	public Gun unpockitingGun;

	// Token: 0x04000F00 RID: 3840
	private float previousMaxForwardSpeed;

	// Token: 0x04000F01 RID: 3841
	private float previousMaxBackwardsSpeed;

	// Token: 0x04000F02 RID: 3842
	private float previousMaxSidewaysSpeed;

	// Token: 0x04000F03 RID: 3843
	public float yRotationOffset;

	// Token: 0x04000F04 RID: 3844
	public static bool holdFire;

	// Token: 0x04000F05 RID: 3845
	public bool inCover;

	// Token: 0x04000F06 RID: 3846
	public bool previousInCover;

	// Token: 0x04000F07 RID: 3847
	public bool disableUsingWeapons;

	// Token: 0x04000F08 RID: 3848
	private bool prevousDisableUsingWeapons;

	// Token: 0x04000F09 RID: 3849
	public bool aim;

	// Token: 0x04000F0A RID: 3850
	private bool aimPressed;

	// Token: 0x04000F0B RID: 3851
	private bool previousAim;

	// Token: 0x04000F0C RID: 3852
	private static float lastEnemyReloadSound;

	// Token: 0x04000F0D RID: 3853
	private string configuredGun;

	// Token: 0x04000F0E RID: 3854
	public Transform rightArm;

	// Token: 0x04000F0F RID: 3855
	public bool tallCover;

	// Token: 0x04000F10 RID: 3856
	public AnimationClip inCoverFireAnim;

	// Token: 0x04000F11 RID: 3857
	public AnimationClip inCoverIdleAimAnim;

	// Token: 0x04000F12 RID: 3858
	public static bool additiveAimingSetup;

	// Token: 0x04000F13 RID: 3859
	public bool isTemple;

	// Token: 0x04000F14 RID: 3860
	public Vector3 shootingMode = new Vector3(1f, 0f, -4.5f);

	// Token: 0x04000F15 RID: 3861
	public Vector3 aimingMode = new Vector3(0.6f, 0.1f, -0.9f);

	// Token: 0x04000F16 RID: 3862
	public Vector3 inversedAimingMode = new Vector3(-0.6f, 0.1f, -0.9f);

	// Token: 0x04000F17 RID: 3863
	public Vector3 relaxedMode = new Vector3(0f, 0f, -4.5f);

	// Token: 0x04000F18 RID: 3864
	public bool resetAiming;

	// Token: 0x04000F19 RID: 3865
	private bool pressAim;

	// Token: 0x04000F1A RID: 3866
	public bool blindFire;

	// Token: 0x04000F1B RID: 3867
	public GunManager GunManagerPrefab;

	// Token: 0x04000F1C RID: 3868
	public WeaponHandling.WeaponOffset[] weaponOffsets;

	// Token: 0x04000F1D RID: 3869
	public bool disableAiming;

	// Token: 0x020001EF RID: 495
	[Serializable]
	public class WeaponOffset
	{
		// Token: 0x04000F1E RID: 3870
		public string weaponName = string.Empty;

		// Token: 0x04000F1F RID: 3871
		public float offset;
	}

	// Token: 0x020001F0 RID: 496
	public enum WeaponStatus
	{
		// Token: 0x04000F21 RID: 3873
		RELAXED,
		// Token: 0x04000F22 RID: 3874
		ENGAGED,
		// Token: 0x04000F23 RID: 3875
		SHOOTING,
		// Token: 0x04000F24 RID: 3876
		AIMING
	}

	// Token: 0x020001F1 RID: 497
	public enum WeaponHolderTypes
	{
		// Token: 0x04000F26 RID: 3878
		PLAYER,
		// Token: 0x04000F27 RID: 3879
		NPC
	}
}
