using System;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class GunManager : MonoBehaviour
{
	// Token: 0x06000A21 RID: 2593 RVA: 0x0006B64C File Offset: 0x0006984C
	private void OnDestroy()
	{
		this.pickupSound = null;
		this.grenadeLunchSound = null;
		this.grenade = null;
		for (int i = 0; i < this.guns.Length; i++)
		{
			this.guns[i] = null;
		}
		this.currentGun = null;
		this.currentPrimaryGun = null;
		this.currentSecondaryGun = null;
		this.PickupableObject = null;
		this.cGun = null;
		this.playerHealth = null;
		this.cStatus = null;
		this.hud = null;
		this.PickupableGrenade = null;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0006B6D0 File Offset: 0x000698D0
	private void Start()
	{
		if (this.loaded)
		{
			this.loaded = false;
			return;
		}
		if (Application.loadedLevelName == "Prologue" && ((!mainmenu.replayLevel && SaveHandler.checkpointReached <= 0) || (mainmenu.replayLevel && SaveHandler.replayCheckpointReached <= 0)))
		{
			GunManager.grenadeDisabled = true;
		}
		for (int i = 0; i < this.guns.Length; i++)
		{
			if (this.guns[i].gun.WeaponType == WeaponTypes.PRIMARY)
			{
				this.guns[i].gun.weaponTransform.position = this.guns[i].gun.weaponHolder.PrimaryPocket.position;
				this.guns[i].gun.weaponTransform.rotation = this.guns[i].gun.weaponHolder.PrimaryPocket.rotation;
				this.guns[i].gun.weaponTransform.parent = this.guns[i].gun.weaponHolder.PrimaryPocket;
				if (this.guns[i].gun.pocketOffsetPosition != Vector3.zero || this.guns[i].gun.pocketOffsetRotation != Vector3.zero)
				{
					this.guns[i].gun.weaponTransform.transform.Translate(this.guns[i].gun.pocketOffsetPosition);
					this.guns[i].gun.weaponTransform.transform.Rotate(this.guns[i].gun.pocketOffsetRotation, Space.Self);
				}
				if (this.guns[i].gun.weaponHolder != null)
				{
					float x = 0f;
					foreach (WeaponHandling.WeaponOffset weaponOffset in this.guns[i].gun.weaponHolder.weaponOffsets)
					{
						if (weaponOffset.weaponName == this.guns[i].gun.gunName)
						{
							x = weaponOffset.offset;
						}
					}
					this.guns[i].gun.weaponTransform.transform.Translate(x, 0f, 0f);
				}
			}
			else if (this.guns[i].gun.WeaponType == WeaponTypes.SECONDARY)
			{
				this.guns[i].gun.weaponTransform.position = this.guns[i].gun.weaponHolder.SecondaryPocket.position;
				this.guns[i].gun.weaponTransform.rotation = this.guns[i].gun.weaponHolder.SecondaryPocket.rotation;
				this.guns[i].gun.weaponTransform.parent = this.guns[i].gun.weaponHolder.SecondaryPocket;
				if (this.guns[i].gun.pocketOffsetPosition != Vector3.zero || this.guns[i].gun.pocketOffsetRotation != Vector3.zero)
				{
					this.guns[i].gun.weaponTransform.transform.Translate(this.guns[i].gun.pocketOffsetPosition);
					this.guns[i].gun.weaponTransform.transform.Rotate(this.guns[i].gun.pocketOffsetRotation, Space.Self);
				}
			}
			this.changeVisibility(this.guns[i].gun, false);
			this.guns[i].gun.enabled = false;
			this.hud = GameObject.Find("WeaponsHUD").GetComponent<WeaponsHUD>();
		}
		if (!this.startWithNoSecondaryWeapon)
		{
			for (int k = 0; k < this.guns.Length; k++)
			{
				if (this.guns[k].gun.WeaponType == WeaponTypes.SECONDARY)
				{
					this.currentWeapon = k;
					this.changeVisibility(this.guns[k].gun, true);
					this.currentSecondaryGun = this.guns[k].gun;
					break;
				}
			}
		}
		if (!this.startWithNoPrimaryWeapon)
		{
			for (int l = 0; l < this.guns.Length; l++)
			{
				if (this.guns[l].gun.WeaponType == WeaponTypes.PRIMARY)
				{
					if (this.currentSecondaryGun == null)
					{
						this.currentWeapon = l;
					}
					this.changeVisibility(this.guns[l].gun, true);
					this.currentPrimaryGun = this.guns[l].gun;
					break;
				}
			}
		}
		if (this.currentSecondaryGun != null)
		{
			this.currentGun = this.currentSecondaryGun;
			this.currentGun.enabled = true;
		}
		else if (this.currentPrimaryGun != null)
		{
			this.currentGun = this.currentPrimaryGun;
			this.currentGun.enabled = true;
		}
		if (this.guns[0].gun != null)
		{
			this.playerHealth = this.guns[0].gun.weaponHolder.GetComponent<Health>();
		}
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].AddMixingTransform(this.grenade.weaponHolder.spineBone);
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].layer = 10;
		GunManager.stealthMode = AnimationHandler.instance.stealthMode;
		if (AnimationHandler.instance.insuredMode)
		{
			this.currentGun.StartDrawn();
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0006BCD8 File Offset: 0x00069ED8
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		this.changeWeaponButton = MobileInput.changeWeapon;
		this.throwGrenadeButton = MobileInput.throwGrenade;
		this.changeWeaponButton |= Input.GetKeyDown(this.changeWeaponsKey);
		this.throwGrenadeButton |= InputManager.GetButtonDown("Grenade");
		if (this.currentGun != null)
		{
			if (this.submittedWeaponName != this.currentGun.name || this.submittedCurrentRounds != this.currentGun.currentRounds || this.submittedCurrentClips != this.currentGun.totalBullets)
			{
				if (!this.hud.enabled)
				{
					this.hud.enabled = true;
				}
				this.hud.setValues(this.currentGun.name, this.currentGun.currentRounds, this.currentGun.totalBullets, this.currentGun.clipSize, this.currentGun.WeaponType);
				this.submittedWeaponName = this.currentGun.name;
				this.submittedCurrentRounds = this.currentGun.currentRounds;
				this.submittedCurrentClips = this.currentGun.totalBullets;
			}
		}
		else
		{
			this.hud.enabled = false;
		}
		if (this.submittedCurrentGrenades != this.currentGrenades)
		{
			this.hud.setGenades(this.currentGrenades);
			this.submittedCurrentGrenades = this.currentGrenades;
			MobileInput.gotGrenade = (this.currentGrenades > 0);
		}
		if (!GunManager.grenadeDisabled && !this.grenade.weaponHolder.disableUsingWeapons && this.grenade != null && this.currentGrenades > 0 && !this.launchProjectile && this.throwGrenadeButton && Time.time > this.grenadeTrhoughTime + (this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].length + 3f) && (!this.grenade.weaponHolder.inCover || this.grenade.weaponHolder.aim))
		{
			Vector3 forward = base.transform.forward;
			Vector3 forward2 = Camera.main.transform.forward;
			forward.y = 0f;
			forward2.y = 0f;
			float num = Vector3.Angle(forward, forward2);
			if (Vector3.Cross(forward, forward2).y < 0f)
			{
				num *= -1f;
			}
			num += 20f;
			this.grenade.weaponHolder.gameObject.GetComponent<Animation>().CrossFade("Shooting-Grenade-QuickThrow");
			this.launchProjectile = true;
			this.grenadeTrhoughTime = Time.time;
			this.luanchTimer = this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].length * 0.5f;
			if (!this.grenade.weaponHolder.inCover)
			{
				iTween.RotateBy(this.grenade.weaponHolder.gameObject.gameObject, new Vector3(0f, num / 360f, 0f), this.luanchTimer);
			}
		}
		if (this.launchProjectile)
		{
			this.luanchTimer -= Time.deltaTime;
			if (this.luanchTimer <= 0f)
			{
				this.grenade.LaunchProjectile();
				base.GetComponent<AudioSource>().PlayOneShot(this.grenadeLunchSound, SpeechManager.sfxVolume);
				this.currentGrenades--;
				this.launchProjectile = false;
			}
		}
		if (this.currentPrimaryGun != null && (InputManager.GetButtonDown("PrimaryWeapon2") || Input.GetAxisRaw("PrimaryWeapon") > 0.99f) && !AnimationHandler.instance.insuredMode)
		{
			bool flag = false;
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && AndroidPlatform.isJoystickConnected && Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("basic"))
			{
				flag = true;
			}
			if (!flag)
			{
				if (this.currentGun.WeaponType == WeaponTypes.SECONDARY)
				{
					this.cGun = this.currentPrimaryGun;
					this.changing = true;
					this.pickingup = false;
				}
				else if (this.allowAllWeapons)
				{
					for (int i = this.currentWeapon; i < this.guns.Length; i++)
					{
						if (this.guns[i].gun.WeaponType == WeaponTypes.PRIMARY && this.currentPrimaryGun != this.guns[i].gun)
						{
							this.cGun = this.guns[i].gun;
							this.changing = true;
							this.pickingup = false;
							this.currentWeapon = i;
							break;
						}
						if (i == this.guns.Length - 1)
						{
							i = -1;
						}
					}
				}
				else if (this.currentGun.weaponHolder.status == WeaponHandling.WeaponStatus.RELAXED)
				{
					if (Time.time > this.lastHolsterTime + 0.5f)
					{
						this.currentGun.Draw();
					}
				}
				else if (!this.currentGun.drawing)
				{
					this.currentGun.weaponHolder.status = WeaponHandling.WeaponStatus.RELAXED;
					this.currentGun.weaponHolder.Holster();
					this.lastHolsterTime = Time.time;
				}
			}
		}
		else if (this.currentSecondaryGun != null && (InputManager.GetButtonDown("SecondaryWeapon2") || Input.GetAxisRaw("SecondaryWeapon") > 0.99f) && !AnimationHandler.instance.insuredMode)
		{
			bool flag2 = false;
			if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && AndroidPlatform.isJoystickConnected && Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("basic"))
			{
				flag2 = true;
			}
			if (!flag2)
			{
				if (this.currentGun.WeaponType == WeaponTypes.PRIMARY)
				{
					this.cGun = this.currentSecondaryGun;
					this.changing = true;
					this.pickingup = false;
				}
				else if (this.allowAllWeapons)
				{
					for (int j = this.currentWeapon; j < this.guns.Length; j++)
					{
						if (this.guns[j].gun.WeaponType == WeaponTypes.SECONDARY && this.currentSecondaryGun != this.guns[j].gun)
						{
							this.cGun = this.guns[j].gun;
							this.changing = true;
							this.pickingup = false;
							this.currentWeapon = j;
							break;
						}
						if (j == this.guns.Length - 1)
						{
							j = -1;
						}
					}
				}
				else if (this.currentGun.weaponHolder.status == WeaponHandling.WeaponStatus.RELAXED)
				{
					if (Time.time > this.lastHolsterTime + 0.5f)
					{
						this.currentGun.Draw();
					}
				}
				else if (!this.currentGun.drawing)
				{
					this.currentGun.weaponHolder.status = WeaponHandling.WeaponStatus.RELAXED;
					this.currentGun.weaponHolder.Holster();
					this.lastHolsterTime = Time.time;
				}
			}
		}
		else if (this.changeWeaponButton && !AnimationHandler.instance.insuredMode)
		{
			if ((this.currentGun == null || this.currentGun.WeaponType == WeaponTypes.PRIMARY) && this.currentSecondaryGun != null)
			{
				this.cGun = this.currentSecondaryGun;
				this.changing = true;
				this.pickingup = false;
			}
			else if (this.currentPrimaryGun != null)
			{
				this.cGun = this.currentPrimaryGun;
				this.changing = true;
				this.pickingup = false;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Home))
		{
			this.demoMode = !this.demoMode;
			if (!this.demoMode)
			{
				this.playerHealth.unlimitedHealth = false;
				this.allowAllWeapons = false;
			}
		}
		else if (this.demoMode && Input.GetKeyDown(KeyCode.F1))
		{
			this.playerHealth.unlimitedHealth = !this.playerHealth.unlimitedHealth;
		}
		else if (this.demoMode && Input.GetKeyDown(KeyCode.F2))
		{
			this.allowAllWeapons = !this.allowAllWeapons;
		}
		else if (this.demoMode && Input.GetKeyDown(KeyCode.F3))
		{
			GunManager.stealthMode = !GunManager.stealthMode;
			AnimationHandler.instance.stealthMode = GunManager.stealthMode;
		}
		else if (Input.GetKeyDown(KeyCode.F5))
		{
			if (mainmenu.replayLevel)
			{
				Checkpoint[] array = (Checkpoint[])UnityEngine.Object.FindObjectsOfType(typeof(Checkpoint));
				foreach (Checkpoint checkpoint in array)
				{
					if (checkpoint.checkpointNo == this.startFromCheckpoint)
					{
						SaveHandler.SaveCheckpointOnReplay(this.startFromCheckpoint, checkpoint.transform.position, checkpoint.transform.rotation.eulerAngles, (!(this.currentSecondaryGun != null)) ? string.Empty : this.currentSecondaryGun.gunName, (!(this.currentPrimaryGun != null)) ? string.Empty : this.currentPrimaryGun.gunName, (!(this.currentSecondaryGun != null)) ? 0 : this.currentSecondaryGun.totalBullets, (!(this.currentSecondaryGun != null)) ? 0 : this.currentSecondaryGun.currentRounds, (!(this.currentPrimaryGun != null)) ? 0 : this.currentPrimaryGun.totalBullets, (!(this.currentPrimaryGun != null)) ? 0 : this.currentPrimaryGun.currentRounds, this.currentGrenades);
						Application.LoadLevel("PreLoading" + Application.loadedLevelName);
						break;
					}
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
		}
		if (this.changing)
		{
			this.ChangeToGun();
		}
		if (this.pickGrenade)
		{
			this.pickGrenadeTimer -= Time.deltaTime;
			if (this.pickGrenadeTimer <= 0f)
			{
				this.currentGrenades++;
				base.GetComponent<AudioSource>().PlayOneShot(this.pickupSound, SpeechManager.sfxVolume);
				UnityEngine.Object.Destroy(this.PickupableGrenade);
				this.pickGrenade = false;
				AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = true;
				AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = true;
			}
		}
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0006C804 File Offset: 0x0006AA04
	private void ChangeToGun()
	{
		if (this.currentGun != null && (!this.pickingup || (this.pickingup && this.currentGun.WeaponType != this.cGun.WeaponType)) && this.currentGun.weaponHolder.status != WeaponHandling.WeaponStatus.RELAXED && this.changingTimer <= 0f)
		{
			this.currentGun.weaponHolder.Holster();
			this.changingTimer = this.currentGun.holsterAnimation.length / 3f;
		}
		this.changingTimer -= Time.deltaTime;
		if (this.currentGun != null && this.pickingup && this.currentGun.weaponHolder.status == WeaponHandling.WeaponStatus.RELAXED && this.changingTimer <= 0f)
		{
			this.currentGun.enabled = false;
			if (this.cGun.WeaponType == WeaponTypes.PRIMARY)
			{
				this.currentGun = this.currentPrimaryGun;
			}
			else if (this.cGun.WeaponType == WeaponTypes.SECONDARY)
			{
				this.currentGun = this.currentSecondaryGun;
			}
			if (this.currentGun != null)
			{
				this.currentGun.Draw();
				this.changingTimer = this.currentGun.drawAnimation.length / 3f;
			}
		}
		if (this.changingTimer <= 0f)
		{
			if (this.pickingup)
			{
				if (this.pickingupTimer <= 0f && !this.pickedup)
				{
					if (this.currentGun != null)
					{
						if (this.cGun.WeaponType == WeaponTypes.PRIMARY)
						{
							this.changeVisibility(this.currentPrimaryGun, false);
							this.currentPrimaryGun.weaponTransform.position = this.currentPrimaryGun.weaponHolder.PrimaryPocket.position;
							this.currentPrimaryGun.weaponTransform.rotation = this.currentPrimaryGun.weaponHolder.PrimaryPocket.rotation;
							this.currentPrimaryGun.weaponTransform.parent = this.currentPrimaryGun.weaponHolder.PrimaryPocket;
						}
						else
						{
							this.changeVisibility(this.currentSecondaryGun, false);
							this.currentSecondaryGun.weaponTransform.position = this.currentSecondaryGun.weaponHolder.SecondaryPocket.position;
							this.currentSecondaryGun.weaponTransform.rotation = this.currentSecondaryGun.weaponHolder.SecondaryPocket.rotation;
							this.currentSecondaryGun.weaponTransform.parent = this.currentSecondaryGun.weaponHolder.SecondaryPocket;
						}
						Quaternion rotation = this.currentGun.weaponHolder.PrimaryRightHand.rotation;
						rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, rotation.eulerAngles.z - 45f);
						GameObject gameObject = UnityEngine.Object.Instantiate(this.currentGun.pickablePrefab, this.currentGun.weaponHolder.PrimaryRightHand.position, rotation) as GameObject;
						Pickupable componentInChildren = gameObject.GetComponentInChildren<Pickupable>();
						if (componentInChildren != null)
						{
							componentInChildren.currentBullets = this.currentGun.totalBullets;
							componentInChildren.currentRounds = this.currentGun.currentRounds;
						}
					}
					this.cGun.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
					this.cGun.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 15;
					this.cGun.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].speed = 1.5f;
					this.cGun.weaponHolder.gameObject.GetComponent<Animation>().Play("Interaction-Pick-Ground");
					this.pickingupTimer = this.cGun.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length * 0.33f;
					this.pickedup = true;
					return;
				}
				this.pickingupTimer -= Time.deltaTime;
				if (this.pickingupTimer > 0f)
				{
					return;
				}
			}
			else if (this.currentGun != null && this.currentGun.WeaponType == this.cGun.WeaponType)
			{
				this.changeVisibility(this.currentGun, false);
			}
			if (this.currentGun != null)
			{
				this.currentGun.enabled = false;
			}
			this.cGun.enabled = true;
			this.changeVisibility(this.cGun, true);
			if (this.pickingup)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.pickupSound, SpeechManager.sfxVolume);
				UnityEngine.Object.Destroy(this.PickupableObject);
			}
			this.currentGun = this.cGun;
			if (this.currentGun.WeaponType == WeaponTypes.PRIMARY)
			{
				this.currentPrimaryGun = this.cGun;
			}
			else
			{
				this.currentSecondaryGun = this.cGun;
			}
			if (!this.pickingup)
			{
				this.currentGun.Draw();
			}
			else
			{
				this.currentGun.weaponHolder.unpockitingTimer = 0.1f;
				this.currentGun.weaponHolder.unpockitingGun = this.currentGun;
				this.currentGun.weaponHolder.unpockiting = true;
				this.currentGun.weaponHolder.EnterIdleFree();
				this.currentGun.weaponHolder.EnterShootingMode();
				this.currentGun.weaponHolder.status = WeaponHandling.WeaponStatus.ENGAGED;
			}
			this.changing = false;
			if (this.pickedup)
			{
				AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = true;
				AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = true;
				this.pickingup = false;
			}
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0006CE00 File Offset: 0x0006B000
	public void ChangeToGun(string gun, GameObject PickupableObject, int currentBullets, int currentRounds)
	{
		if (this.currentPrimaryGun != null && this.currentPrimaryGun.gunName == gun)
		{
			UnityEngine.Object.Destroy(PickupableObject);
			if (currentBullets == -1)
			{
				this.currentPrimaryGun.totalBullets += this.currentPrimaryGun.clipSize;
			}
			else
			{
				this.currentPrimaryGun.totalBullets += currentBullets + currentRounds;
			}
			if (this.pickupSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.pickupSound, SpeechManager.sfxVolume);
			}
		}
		else if (this.currentSecondaryGun != null && this.currentSecondaryGun.gunName == gun)
		{
			UnityEngine.Object.Destroy(PickupableObject);
			if (currentBullets == -1)
			{
				this.currentSecondaryGun.totalBullets += this.currentSecondaryGun.clipSize;
			}
			else
			{
				this.currentSecondaryGun.totalBullets += currentBullets + currentRounds;
			}
			if (this.pickupSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.pickupSound, SpeechManager.sfxVolume);
			}
		}
		else
		{
			AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = false;
			AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = false;
			this.PickupableObject = PickupableObject;
			for (int i = 0; i < this.guns.Length; i++)
			{
				if (this.guns[i].gun.gunName == gun)
				{
					this.cGun = this.guns[i].gun;
					if (currentBullets != -1)
					{
						this.cGun.totalBullets = currentBullets;
						this.cGun.currentRounds = currentRounds;
					}
					else
					{
						this.cGun.totalBullets = this.cGun.totalClips * this.cGun.clipSize;
						this.cGun.currentRounds = this.cGun.clipSize;
					}
					this.changing = true;
					this.pickingup = true;
					this.pickedup = false;
					this.currentWeapon = i;
					break;
				}
			}
		}
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0006D038 File Offset: 0x0006B238
	public void PickupGrenade(GameObject PickupableObject)
	{
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 15;
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].speed = 1.5f;
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>().Play("Interaction-Pick-Ground");
		this.pickGrenade = true;
		this.pickGrenadeTimer = this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length * 0.33f;
		this.PickupableGrenade = PickupableObject;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0006D118 File Offset: 0x0006B318
	private void changeVisibility(Gun g, bool visible)
	{
		if (g.weaponTransform.GetComponent<Renderer>() != null)
		{
			g.weaponTransform.GetComponent<Renderer>().enabled = visible;
		}
		else
		{
			Renderer[] componentsInChildren = g.weaponTransform.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.enabled = visible;
			}
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0006D180 File Offset: 0x0006B380
	public void ForceColt()
	{
		for (int i = 0; i < this.guns.Length; i++)
		{
			if (this.guns[i].gun.gunName == "Colt")
			{
				if (this.currentGun == this.currentPrimaryGun)
				{
					this.currentPrimaryGun.weaponTransform.position = this.currentPrimaryGun.weaponHolder.PrimaryPocket.position;
					this.currentPrimaryGun.weaponTransform.rotation = this.currentPrimaryGun.weaponHolder.PrimaryPocket.rotation;
					this.currentPrimaryGun.weaponTransform.parent = this.currentPrimaryGun.weaponHolder.PrimaryPocket;
				}
				else if (this.currentGun == this.currentSecondaryGun)
				{
					this.currentSecondaryGun.weaponTransform.position = this.currentSecondaryGun.weaponHolder.SecondaryPocket.position;
					this.currentSecondaryGun.weaponTransform.rotation = this.currentSecondaryGun.weaponHolder.SecondaryPocket.rotation;
					this.currentSecondaryGun.weaponTransform.parent = this.currentSecondaryGun.weaponHolder.SecondaryPocket;
				}
				this.currentGun.enabled = false;
				this.currentSecondaryGun = this.guns[i].gun;
				this.currentGun = this.guns[i].gun;
				this.currentGun.enabled = true;
				this.currentGun.weaponTransform.position = this.currentGun.weaponHolder.SecondaryRightHand.position;
				this.currentGun.weaponTransform.rotation = this.currentGun.weaponHolder.SecondaryRightHand.rotation;
				this.currentGun.weaponTransform.parent = this.currentGun.weaponHolder.SecondaryRightHand;
				if (this.currentGun.rightHandOffsetPosition != Vector3.zero || this.currentGun.rightHandOffsetRotation != Vector3.zero)
				{
					this.currentGun.weaponTransform.transform.Translate(this.currentGun.rightHandOffsetPosition);
					this.currentGun.weaponTransform.transform.Rotate(this.currentGun.rightHandOffsetRotation, Space.Self);
				}
				break;
			}
		}
		this.HideUnusedWeapons();
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0006D3F4 File Offset: 0x0006B5F4
	public void HideUnusedWeapons()
	{
		for (int i = 0; i < this.guns.Length; i++)
		{
			this.changeVisibility(this.guns[i].gun, false);
		}
		if (this.currentPrimaryGun != null)
		{
			this.changeVisibility(this.currentPrimaryGun, true);
		}
		if (this.currentSecondaryGun != null)
		{
			this.changeVisibility(this.currentSecondaryGun, true);
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0006D46C File Offset: 0x0006B66C
	private void EnableMoving()
	{
		AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = true;
		AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = true;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0006D4A4 File Offset: 0x0006B6A4
	private void OnGUI()
	{
		GUIStyle guistyle = new GUIStyle();
		guistyle.alignment = TextAnchor.MiddleRight;
		guistyle.normal.textColor = Color.white;
		guistyle.fontSize = 20;
		guistyle.alignment = TextAnchor.MiddleLeft;
		if (this.demoMode)
		{
			GUI.Label(new Rect(10f, (float)(Screen.height - 25), 200f, 25f), "DEBUG MODE ON", guistyle);
			GUI.Label(new Rect(10f, (float)(Screen.height - 50), 200f, 25f), "Unlimited Health " + ((!this.playerHealth.unlimitedHealth) ? "OFF" : "ON"), guistyle);
			GUI.Label(new Rect(10f, (float)(Screen.height - 75), 200f, 25f), "All Weapons " + ((!this.allowAllWeapons) ? "OFF" : "ON"), guistyle);
			GUI.Label(new Rect(10f, (float)(Screen.height - 100), 200f, 25f), "Stealth " + ((!GunManager.stealthMode) ? "OFF" : "ON"), guistyle);
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0006D5EC File Offset: 0x0006B7EC
	public void LoadGunManager(string currentSecondaryWeaponName, string currentPrimaryWeaponName, int currSecondaryClips, int currSecondaryBullets, int currPrimaryClips, int currPrimaryBullets, int currGrenades)
	{
		for (int i = 0; i < this.guns.Length; i++)
		{
			if (this.guns[i].gun.WeaponType == WeaponTypes.PRIMARY)
			{
				this.guns[i].gun.weaponTransform.position = this.guns[i].gun.weaponHolder.PrimaryPocket.position;
				this.guns[i].gun.weaponTransform.rotation = this.guns[i].gun.weaponHolder.PrimaryPocket.rotation;
				this.guns[i].gun.weaponTransform.parent = this.guns[i].gun.weaponHolder.PrimaryPocket;
				if (this.guns[i].gun.pocketOffsetPosition != Vector3.zero || this.guns[i].gun.pocketOffsetRotation != Vector3.zero)
				{
					this.guns[i].gun.weaponTransform.transform.Translate(this.guns[i].gun.pocketOffsetPosition);
					this.guns[i].gun.weaponTransform.transform.Rotate(this.guns[i].gun.pocketOffsetRotation, Space.Self);
				}
				if (this.guns[i].gun.weaponHolder != null)
				{
					float x = 0f;
					foreach (WeaponHandling.WeaponOffset weaponOffset in this.guns[i].gun.weaponHolder.weaponOffsets)
					{
						if (weaponOffset.weaponName == this.guns[i].gun.gunName)
						{
							x = weaponOffset.offset;
						}
					}
					this.guns[i].gun.weaponTransform.transform.Translate(x, 0f, 0f);
				}
			}
			else if (this.guns[i].gun.WeaponType == WeaponTypes.SECONDARY)
			{
				this.guns[i].gun.weaponTransform.position = this.guns[i].gun.weaponHolder.SecondaryPocket.position;
				this.guns[i].gun.weaponTransform.rotation = this.guns[i].gun.weaponHolder.SecondaryPocket.rotation;
				this.guns[i].gun.weaponTransform.parent = this.guns[i].gun.weaponHolder.SecondaryPocket;
				if (this.guns[i].gun.pocketOffsetPosition != Vector3.zero || this.guns[i].gun.pocketOffsetRotation != Vector3.zero)
				{
					this.guns[i].gun.weaponTransform.transform.Translate(this.guns[i].gun.pocketOffsetPosition);
					this.guns[i].gun.weaponTransform.transform.Rotate(this.guns[i].gun.pocketOffsetRotation, Space.Self);
				}
			}
			this.changeVisibility(this.guns[i].gun, false);
			this.guns[i].gun.enabled = false;
			this.hud = GameObject.Find("WeaponsHUD").GetComponent<WeaponsHUD>();
		}
		if (!this.startWithNoSecondaryWeapon)
		{
			for (int k = 0; k < this.guns.Length; k++)
			{
				if (this.guns[k].gun.WeaponType == WeaponTypes.SECONDARY && this.guns[k].gun.gunName == currentSecondaryWeaponName)
				{
					this.currentWeapon = k;
					this.changeVisibility(this.guns[k].gun, true);
					this.currentSecondaryGun = this.guns[k].gun;
					break;
				}
			}
		}
		if (!this.startWithNoPrimaryWeapon)
		{
			for (int l = 0; l < this.guns.Length; l++)
			{
				if (this.guns[l].gun.WeaponType == WeaponTypes.PRIMARY && this.guns[l].gun.gunName == currentPrimaryWeaponName)
				{
					if (this.currentSecondaryGun == null)
					{
						this.currentWeapon = l;
					}
					this.changeVisibility(this.guns[l].gun, true);
					this.currentPrimaryGun = this.guns[l].gun;
					break;
				}
			}
		}
		if (this.currentSecondaryGun != null)
		{
			this.currentGun = this.currentSecondaryGun;
			this.currentGun.enabled = true;
			this.currentSecondaryGun.totalBullets = currSecondaryClips;
			this.currentSecondaryGun.currentRounds = currSecondaryBullets;
			this.currentSecondaryGun.bulletsSaved = true;
		}
		else if (this.currentPrimaryGun != null)
		{
			this.currentGun = this.currentPrimaryGun;
			this.currentGun.enabled = true;
		}
		if (this.currentPrimaryGun != null)
		{
			this.currentPrimaryGun.totalBullets = currPrimaryClips;
			this.currentPrimaryGun.currentRounds = currPrimaryBullets;
			this.currentPrimaryGun.bulletsSaved = true;
		}
		if (this.guns[0].gun != null)
		{
			this.playerHealth = this.guns[0].gun.weaponHolder.GetComponent<Health>();
		}
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].AddMixingTransform(this.grenade.weaponHolder.spineBone);
		this.grenade.weaponHolder.gameObject.GetComponent<Animation>()["Shooting-Grenade-QuickThrow"].layer = 10;
		GunManager.stealthMode = AnimationHandler.instance.stealthMode;
		if (AnimationHandler.instance.insuredMode)
		{
			this.currentGun.StartDrawn();
		}
		this.currentGrenades = currGrenades;
		this.loaded = true;
	}

	// Token: 0x04000FE2 RID: 4066
	public KeyCode primaryKey = KeyCode.Alpha1;

	// Token: 0x04000FE3 RID: 4067
	public KeyCode secondaryKey = KeyCode.Alpha2;

	// Token: 0x04000FE4 RID: 4068
	public KeyCode changeWeaponsKey = KeyCode.Q;

	// Token: 0x04000FE5 RID: 4069
	public bool demoMode;

	// Token: 0x04000FE6 RID: 4070
	public bool allowAllWeapons;

	// Token: 0x04000FE7 RID: 4071
	public static bool stealthMode;

	// Token: 0x04000FE8 RID: 4072
	public AudioClip pickupSound;

	// Token: 0x04000FE9 RID: 4073
	public AudioClip grenadeLunchSound;

	// Token: 0x04000FEA RID: 4074
	public Gun grenade;

	// Token: 0x04000FEB RID: 4075
	public GunKeyBinder[] guns;

	// Token: 0x04000FEC RID: 4076
	[HideInInspector]
	public Gun currentGun;

	// Token: 0x04000FED RID: 4077
	[HideInInspector]
	public Gun currentPrimaryGun;

	// Token: 0x04000FEE RID: 4078
	[HideInInspector]
	public Gun currentSecondaryGun;

	// Token: 0x04000FEF RID: 4079
	[HideInInspector]
	public int currentWeapon;

	// Token: 0x04000FF0 RID: 4080
	private bool changing;

	// Token: 0x04000FF1 RID: 4081
	private float changingTimer;

	// Token: 0x04000FF2 RID: 4082
	public bool pickingup;

	// Token: 0x04000FF3 RID: 4083
	private float pickingupTimer;

	// Token: 0x04000FF4 RID: 4084
	private bool pickedup;

	// Token: 0x04000FF5 RID: 4085
	private GameObject PickupableObject;

	// Token: 0x04000FF6 RID: 4086
	private Gun cGun;

	// Token: 0x04000FF7 RID: 4087
	private Health playerHealth;

	// Token: 0x04000FF8 RID: 4088
	private CharacterStatus cStatus;

	// Token: 0x04000FF9 RID: 4089
	private WeaponsHUD hud;

	// Token: 0x04000FFA RID: 4090
	private string submittedWeaponName;

	// Token: 0x04000FFB RID: 4091
	private int submittedCurrentRounds;

	// Token: 0x04000FFC RID: 4092
	private int submittedCurrentClips;

	// Token: 0x04000FFD RID: 4093
	private int submittedCurrentGrenades;

	// Token: 0x04000FFE RID: 4094
	public int currentGrenades = 3;

	// Token: 0x04000FFF RID: 4095
	private bool launchProjectile;

	// Token: 0x04001000 RID: 4096
	private float luanchTimer;

	// Token: 0x04001001 RID: 4097
	private float grenadeTrhoughTime;

	// Token: 0x04001002 RID: 4098
	private bool pickGrenade;

	// Token: 0x04001003 RID: 4099
	private float pickGrenadeTimer;

	// Token: 0x04001004 RID: 4100
	private GameObject PickupableGrenade;

	// Token: 0x04001005 RID: 4101
	private bool changeWeaponButton;

	// Token: 0x04001006 RID: 4102
	private bool throwGrenadeButton;

	// Token: 0x04001007 RID: 4103
	public bool startWithNoSecondaryWeapon;

	// Token: 0x04001008 RID: 4104
	public bool startWithNoPrimaryWeapon;

	// Token: 0x04001009 RID: 4105
	private bool loaded;

	// Token: 0x0400100A RID: 4106
	private float lastHolsterTime;

	// Token: 0x0400100B RID: 4107
	public int startFromCheckpoint;

	// Token: 0x0400100C RID: 4108
	public static bool grenadeDisabled;
}
