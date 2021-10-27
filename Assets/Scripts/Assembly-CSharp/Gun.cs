using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class Gun : MonoBehaviour
{
	// Token: 0x06000A0B RID: 2571 RVA: 0x00068880 File Offset: 0x00066A80
	private void OnDestroy()
	{
		this.bulletMark = null;
		this.projectilePrefab = null;
		this.projectileDummyPrefab = null;
		this.pickablePrefab = null;
		this.weaponTransformReference = null;
		this.weaponTransform = null;
		this.woodParticle = null;
		this.metalParticle = null;
		this.concreteParticle = null;
		this.sandParticle = null;
		this.waterParticle = null;
		this.blodParticle = null;
		this.glassParticle = null;
		this.fireFreeAnimation = null;
		this.fireAimedAnimation = null;
		this.reloadAnimation = null;
		this.drawAnimation = null;
		this.holsterAnimation = null;
		this.idleFreeAnimation = null;
		this.idleAimedAnimation = null;
		this.crosshairWhite = null;
		this.crosshairRed = null;
		this.shootSoundSource = null;
		this.reloadSound = null;
		this.reloadSoundSource = null;
		this.outOfAmmoSound = null;
		this.outOfAmmoSoundSource = null;
		this.shootSoundSource = null;
		this.hitParticles = null;
		this.shotingEmitter = null;
		this.shottingParticles = null;
		for (int i = 0; i < this.capsuleEmitter.Length; i++)
		{
			this.capsuleEmitter[i] = null;
		}
		for (int j = 0; j < this.shootSounds.Length; j++)
		{
			this.shootSounds[j] = null;
		}
		this.shotLight = null;
		this.weaponHolder = null;
		this.soldierCamera = null;
		this.cam = null;
		this.shooterCamera = null;
		this.projectile = null;
		this.leftHand = null;
		this.bulletTrace = null;
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x000689E4 File Offset: 0x00066BE4
	private void OnDisable()
	{
		if (this.shotingEmitter != null)
		{
			this.shotingEmitter.ChangeState(false, this.fireMode);
		}
		if (this.capsuleEmitter != null)
		{
			for (int i = 0; i < this.capsuleEmitter.Length; i++)
			{
				if (this.capsuleEmitter[i] != null)
				{
					this.capsuleEmitter[i].emit = false;
				}
			}
		}
		if (this.shotLight != null)
		{
			this.shotLight.enabled = false;
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x00068A78 File Offset: 0x00066C78
	private void OnEnable()
	{
		if (this.soldierCamera == null)
		{
			this.soldierCamera = Camera.main.GetComponent<Camera>();
		}
		this.cam = this.soldierCamera;
		this.shooterCamera = this.cam.GetComponent<ShooterGameCamera>();
		this.reloadTimer = 0f;
		this.reloading = false;
		this.drawing = false;
		this.freeToShoot = true;
		this.shootDelay = 1f / this.fireRate;
		this.cBurst = this.burstRate;
		if (this.gunName == "RPJ7" && this.projectile == null)
		{
			this.projectile = (UnityEngine.Object.Instantiate(this.projectileDummyPrefab, this.weaponTransformReference.position, this.weaponTransformReference.rotation) as GameObject);
			this.projectile.transform.parent = this.weaponTransformReference;
		}
		if (this.projectilePrefab != null)
		{
			this.fireType = FireType.PHYSIC_PROJECTILE;
		}
		if (this.shotLight != null)
		{
			this.shotLight.enabled = false;
		}
		this.shottingParticles = null;
		if (this.shotingEmitter != null)
		{
			for (int i = 0; i < this.shotingEmitter.transform.childCount; i++)
			{
				if (this.shotingEmitter.transform.GetChild(i).name == "bullet_trace")
				{
					this.shottingParticles = this.shotingEmitter.transform.GetChild(i);
					break;
				}
			}
		}
		if (this.WeaponType == WeaponTypes.PRIMARY)
		{
			this.timeBeforeHolster = 10f;
		}
		else if (this.WeaponType == WeaponTypes.SECONDARY)
		{
			this.timeBeforeHolster = 2f;
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00068C48 File Offset: 0x00066E48
	public void Awake()
	{
		this.totalBullets = this.totalClips * this.clipSize;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00068C60 File Offset: 0x00066E60
	public void Start()
	{
		if (this.weaponHolder != null)
		{
			string text = this.gunName;
			switch (text)
			{
			case "Pistol":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.8f;
						this.damageAmount = 25f;
					}
				}
				else
				{
					this.damageAmount = 25f;
				}
				this.fireRange = 50f;
				break;
			}
			case "Colt":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.8f;
						this.damageAmount = 34f;
					}
				}
				else
				{
					this.damageAmount = 34f;
				}
				this.fireRange = 50f;
				break;
			}
			case "Uzi":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.6f;
						this.damageAmount = 8f;
					}
				}
				else
				{
					this.damageAmount = 8f;
				}
				this.fireRange = 50f;
				break;
			}
			case "Scorpion":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.6f;
						this.damageAmount = 9f;
					}
				}
				else
				{
					this.damageAmount = 9f;
				}
				this.fireRange = 50f;
				break;
			}
			case "Spas12":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.5f;
						this.damageAmount = 180f;
					}
				}
				else
				{
					this.damageAmount = 180f;
				}
				this.fireRange = 13f;
				break;
			}
			case "AK47":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.5f;
						this.damageAmount = 12f;
					}
				}
				else
				{
					this.damageAmount = 12f;
				}
				this.fireRange = 50f;
				break;
			}
			case "G36":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.5f;
						this.damageAmount = 15f;
					}
				}
				else
				{
					this.damageAmount = 15f;
				}
				this.fireRange = 30f;
				break;
			}
			case "MP5":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.4f;
						this.damageAmount = 17f;
					}
				}
				else
				{
					this.damageAmount = 17f;
				}
				this.fireRange = 50f;
				break;
			}
			case "M4A1":
			{
				WeaponHandling.WeaponHolderTypes weaponHolderType = this.weaponHolder.WeaponHolderType;
				if (weaponHolderType != WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (weaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
					{
						this.aiAccuracy = 0.5f;
						this.damageAmount = 19f;
					}
				}
				else
				{
					this.damageAmount = 19f;
				}
				this.fireRange = 50f;
				break;
			}
			}
		}
		this.hitInfo = default(HitInfo);
		this.decalInfo = default(DecalInfo);
		if (this.weaponHolder != null && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.NPC)
		{
			this.sb = new ShuffleBag();
			this.sb.add(1, Mathf.CeilToInt(this.aiAccuracy * 10f));
			this.sb.add(0, Mathf.CeilToInt((1f - this.aiAccuracy) * 10f));
			if (this.shotingEmitter != null)
			{
				Transform transform = this.shotingEmitter.transform.Find("bullet_trace");
				this.bulletTrace = transform.GetComponent<ParticleEmitter>();
			}
			this.EnableBulletTrace();
		}
		if (this.weaponHolder != null && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			this.hitLayer &= ~(1 << LayerMask.NameToLayer("Player"));
		}
		if (this.bulletsSaved)
		{
			this.bulletsSaved = false;
		}
		else
		{
			this.currentRounds = this.clipSize;
		}
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x000691CC File Offset: 0x000673CC
	public void EnableBulletTrace()
	{
		if (this.shotingEmitter != null)
		{
			this.shotingEmitter.disableBulletTrace = false;
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x000691EC File Offset: 0x000673EC
	public void DisableBulletTrace()
	{
		if (this.shotingEmitter != null)
		{
			this.shotingEmitter.disableBulletTrace = true;
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0006920C File Offset: 0x0006740C
	public void Draw()
	{
		this.weaponHolder.Draw();
		this.weaponHolder.status = WeaponHandling.WeaponStatus.ENGAGED;
		this.drawing = true;
		this.lastShootTime = Time.time;
		if (this.drawAnimation != null)
		{
			this.drawTimer = this.drawAnimation.length / 3f * 0.75f;
		}
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00069270 File Offset: 0x00067470
	public void StartDrawn()
	{
		this.weaponHolder.StartDrawn();
		this.weaponHolder.status = WeaponHandling.WeaponStatus.ENGAGED;
		this.drawing = true;
		this.lastShootTime = Time.time;
		this.drawTimer = 0f;
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x000692B4 File Offset: 0x000674B4
	private void ShotTheTarget()
	{
		if (this.fire && !this.reloading && !this.drawing)
		{
			if (this.currentRounds > 0)
			{
				if (Time.time > this.lastShootTime && this.freeToShoot && this.cBurst > 0)
				{
					this.lastShootTime = Time.time + this.shootDelay;
					switch (this.fireMode)
					{
					case FireMode.SEMI_AUTO:
						if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
						{
							this.freeToShoot = false;
						}
						break;
					case FireMode.BURST:
						this.cBurst--;
						break;
					}
					if (this.capsuleEmitter != null)
					{
						for (int i = 0; i < this.capsuleEmitter.Length; i++)
						{
							//this.capsuleEmitter[i].Emit();
						}
					}
					this.PlayShootSound();
					if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
					{
						PadVibrator.VibrateInterval(true, 0.5f, 0.3f);
						if (Time.time < 5f || Time.time > Gun.lastFireReportTime + 5f)
						{
							BotAI.PlayerFiredHisWeapon(this.weaponHolder.transform.position);
							Gun.lastFireReportTime = Time.time;
						}
					}
					if (!this.weaponHolder.blindFire)
					{
						this.weaponHolder.Fire();
					}
					if (this.shotingEmitter != null)
					{
						this.shotingEmitter.ChangeState(true, this.fireMode);
					}
					if (this.shotLight != null)
					{
						this.shotLight.enabled = true;
					}
					FireType fireType = this.fireType;
					if (fireType != FireType.RAYCAST)
					{
						if (fireType == FireType.PHYSIC_PROJECTILE)
						{
							this.LaunchProjectile();
						}
					}
					else
					{
						this.CheckRaycastHit();
					}
					this.currentRounds--;
					if (this.currentRounds <= 0)
					{
						this.Reload();
					}
					if (this.weaponHolder != null && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
					{
						this.shooterCamera.recoil = this.recoil;
					}
				}
			}
			else if (this.autoReload && this.freeToShoot)
			{
				if (this.shotingEmitter != null)
				{
					this.shotingEmitter.ChangeState(false, this.fireMode);
				}
				if (this.shotLight != null)
				{
					this.shotLight.enabled = false;
				}
				if (!this.reloading)
				{
					this.Reload();
				}
			}
		}
		else
		{
			if (this.shotingEmitter != null)
			{
				this.shotingEmitter.ChangeState(false, this.fireMode);
			}
			if (this.shotLight != null)
			{
				this.shotLight.enabled = false;
			}
		}
		if ((!this.freeToShoot && Time.time > this.lastShootTime + 0.25f) || this.cBurst == 0)
		{
			if (this.shotingEmitter != null)
			{
				this.shotingEmitter.ChangeState(false, this.fireMode);
			}
			if (this.shotLight != null)
			{
				this.shotLight.enabled = false;
			}
		}
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x00069608 File Offset: 0x00067808
	public void LaunchProjectile()
	{
		Ray ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.6f, 0f));
		Vector3 position;
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			if (this.weaponTransformReference != null)
			{
				position = this.weaponTransformReference.position;
			}
			else
			{
				position = this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0.5f));
			}
		}
		else
		{
			position = base.transform.position;
			position.y += 1f;
		}
		if (this.gunName == "RPJ7" && this.projectile != null)
		{
			UnityEngine.Object.Destroy(this.projectile);
		}
		if (this.projectilePrefab != null)
		{
			this.projectilePrefab.transform.localScale = new Vector3(this.scaleFactor, this.scaleFactor, this.scaleFactor);
		}
		this.projectile = (UnityEngine.Object.Instantiate(this.projectilePrefab, position, Quaternion.identity) as GameObject);
		Grenade component = this.projectile.GetComponent<Grenade>();
		component.soldierCamera = this.soldierCamera.GetComponent<ShooterGameCamera>();
		component.scaleFactor = this.scaleFactor;
		this.projectile.transform.rotation = Quaternion.LookRotation(ray.direction);
		Rigidbody rigidbody = this.projectile.GetComponent<Rigidbody>();
		if (this.projectile.GetComponent<Rigidbody>() == null)
		{
			rigidbody = this.projectile.AddComponent<Rigidbody>();
		}
		Ray ray2 = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.55f, 0f));
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(ray2.origin, ray2.direction, out raycastHit, this.fireRange * this.scaleFactor, this.hitLayer))
			{
				rigidbody.velocity = (raycastHit.point - this.weaponTransformReference.position).normalized * this.projectileSpeed * this.scaleFactor;
			}
			else
			{
				rigidbody.velocity = (this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.55f, 40f)) - this.weaponTransformReference.position).normalized * this.projectileSpeed * this.scaleFactor;
			}
		}
		else
		{
			this.projectileSpeed = Vector3.Distance(this.hitPosition, base.transform.position) * 1.5f;
			rigidbody.velocity = (this.hitPosition - base.transform.position).normalized * this.projectileSpeed;
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0006992C File Offset: 0x00067B2C
	private void CheckRaycastHit()
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		float num = UnityEngine.Random.Range(-this.accuricyRadius, this.accuricyRadius);
		float num2 = UnityEngine.Random.Range(-this.accuricyRadius, this.accuricyRadius);
		Vector3 a = Vector3.zero;
		if (this.weaponTransformReference == null)
		{
			Ray ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			Vector3 vector = ray.origin;
			Vector3 a2 = ray.direction;
			vector += a2 * 0.1f;
		}
		else
		{
			Ray ray;
			if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
			{
				ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f + num, (float)Screen.height * 0.5f + num2, 0f));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out raycastHit, this.fireRange * this.scaleFactor, this.hitLayer))
				{
					ray = new Ray(this.weaponTransformReference.position, ray.GetPoint(raycastHit.distance) - this.weaponTransformReference.position);
				}
				if (this.weaponHolder.blindFire)
				{
					Vector3 forward = this.weaponTransformReference.forward;
					Vector3 direction = ray.direction;
					forward.y = 0f;
					direction.y = 0f;
					float num3 = Vector3.Angle(forward, direction);
					if (Vector3.Cross(forward, direction).y < 0f)
					{
						num3 *= -1f;
					}
					this.weaponTransform.RotateAround(this.weaponHolder.SecondaryRightHand.position, Vector3.up, num3);
				}
			}
			else
			{
				Transform transform = this.weaponHolder.AIScript.target.transform;
				if (transform != null)
				{
					if (this.weaponHolder.AIScript.isAware)
					{
						a = transform.position;
					}
					else if (this.weaponHolder.AIScript.groupID != "NONE")
					{
						a = (Vector3)BotAI.lastSeenPositions[this.weaponHolder.AIScript.groupID];
					}
					else
					{
						a = this.weaponHolder.AIScript.lastSeenPosition;
					}
				}
				a.y += UnityEngine.Random.Range(-0.35f, 0.35f);
				if (this.sb.next() == 0)
				{
					if (this.rightMissAim)
					{
						a += base.transform.right * UnityEngine.Random.Range(0.5f, 1f);
					}
					else
					{
						a -= base.transform.right * UnityEngine.Random.Range(0.5f, 1f);
					}
					this.rightMissAim = !this.rightMissAim;
				}
				ray = new Ray(this.weaponTransformReference.position, a - this.weaponTransformReference.position);
			}
			Vector3 vector = this.weaponTransformReference.position + this.weaponTransformReference.right * 0.2f;
			Debug.DrawRay(ray.origin + ray.direction * 0.1f, ray.direction, Color.red);
			if (Time.time > this.lastRaycastCheckTime + this.timeBeforeRaycastChecks)
			{
				this.hitFound = Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out this.hit, this.fireRange * this.scaleFactor, this.hitLayer);
				if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
				{
					if (this.hitFound && this.hit.distance <= 2f)
					{
						if (this.shotingEmitter != null)
						{
							this.shotingEmitter.disableBulletTrace = true;
						}
					}
					else if (this.shotingEmitter != null)
					{
						this.shotingEmitter.disableBulletTrace = false;
					}
				}
				this.lastRaycastCheckTime = Time.time;
				this.generateDecal = true;
			}
			else
			{
				this.generateDecal = false;
			}
			if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
			{
				if (this.hitFound && this.hit.collider != null && this.hit.collider.name == "Pigeons")
				{
					this.hit.collider.GetComponent<Pigeon_Fly>().OnTriggerEnter(new Collider());
					return;
				}
				if (this.hitFound && this.hit.collider != null && this.hit.collider.name == "Pigeons_Pond")
				{
					this.hit.collider.GetComponent<Pigeon_Fly_Pond>().OnTriggerEnter(new Collider());
					return;
				}
				if (this.hitFound && this.hit.collider != null && this.hit.collider.tag == "Breakable")
				{
					Transform parent = this.hit.collider.transform.parent;
					if (AnimationHandler.instance.dustParticles != null)
					{
						UnityEngine.Object.Instantiate(AnimationHandler.instance.dustParticles, parent.transform.position, Quaternion.identity);
					}
					if (this.hit.collider.GetComponent<InteractionTrigger>() != null)
					{
						UnityEngine.Object.Destroy(this.hit.collider.gameObject);
					}
					foreach (object obj in parent)
					{
						Transform transform2 = (Transform)obj;
						transform2.gameObject.layer = LayerMask.NameToLayer("Grenade");
						if (transform2.gameObject.GetComponent<Rigidbody>() == null)
						{
							transform2.gameObject.AddComponent<Rigidbody>();
						}
						transform2.gameObject.GetComponent<Rigidbody>().drag = 3f;
						transform2.gameObject.GetComponent<Rigidbody>().mass = 2f;
					}
				}
				if (this.hitFound && this.hit.collider != null && this.hit.collider.tag == "Explosive")
				{
					this.hit.collider.SendMessage("Detonate", SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this.hitFound)
			{
				Vector3 a2 = (this.hit.point - vector).normalized;
			}
			else
			{
				Vector3 a2 = this.weaponTransformReference.forward;
			}
		}
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && this.shottingParticles != null)
		{
			this.shottingParticles.rotation = Quaternion.FromToRotation(Vector3.forward, (this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, this.cam.farClipPlane)) - this.weaponTransformReference.position).normalized);
		}
		else
		{
			this.shottingParticles.rotation = Quaternion.FromToRotation(Vector3.forward, (a - this.weaponTransformReference.position).normalized);
		}
		if (this.hitFound)
		{
			Debug.Log("TUSOVKA" + this.hit.collider.tag);
			if (this.hit.collider != null && this.hit.collider.tag == "Head" && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
			{
				this.hitInfo.D = 1000f;
				this.hitInfo.H = "Head";
				this.hitInfo.source = this.weaponHolder.transform.position;
				this.hitInfo.E = 0f;
				this.hit.collider.transform.root.gameObject.SendMessage("Hit", this.hitInfo, SendMessageOptions.DontRequireReceiver);
			}
			else if (this.hit.collider != null && this.hit.collider.tag == "Player")
			{
				if (this.playerHealth == null)
				{
					if (AnimationHandler.instance != null)
					{
						this.playerHealth = AnimationHandler.instance.GetComponent<Health>();
					}
				}
				else
				{
					this.playerHealth.playerHit(this.weaponHolder.transform.position, this.damageAmount);
				}
			}
			else if (this.hit.collider != null && LayerMask.LayerToName(this.hit.collider.gameObject.layer) == "Enemy" && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
			{
				this.hitInfo.D = this.damageAmount;
				this.hitInfo.H = this.hit.collider.tag;
				this.hitInfo.source = this.weaponHolder.transform.position;
				this.hitInfo.E = 0f;
				this.hit.collider.transform.root.gameObject.SendMessage("Hit", this.hitInfo, SendMessageOptions.DontRequireReceiver);
			}
			this.GenerateGraphicStuff(this.hit);
		}
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0006A3A0 File Offset: 0x000685A0
	private void GenerateGraphicStuff(RaycastHit hit)
	{
		if (hit.collider == null)
		{
			return;
		}
		HitType type = HitType.GENERIC;
		Rigidbody rigidbody = hit.collider.GetComponent<Rigidbody>();
		if (rigidbody == null && hit.collider.transform.parent != null)
		{
			rigidbody = hit.collider.transform.parent.GetComponent<Rigidbody>();
		}
		if (rigidbody != null)
		{
			if (rigidbody.gameObject.layer != 10 && !rigidbody.gameObject.name.ToLower().Contains("door"))
			{
				rigidbody.isKinematic = false;
			}
			if (!rigidbody.isKinematic)
			{
				rigidbody.AddForceAtPosition((hit.collider.transform.position - this.weaponTransformReference.position).normalized * this.pushPower, hit.point, ForceMode.Impulse);
			}
		}
		if (!this.generateDecal || hit.collider.tag == "ObstacleCollider")
		{
			return;
		}
		float d = -0.02f;
		Vector3 normal = hit.normal;
		Vector3 position = hit.point + hit.normal * d;
		GameObject gameObject;
		if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Police")
		{
			type = HitType.BLOD;
			gameObject = (UnityEngine.Object.Instantiate(this.blodParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
		}
		else
		{
			string tag = hit.collider.tag;
			switch (tag)
			{
			case "wood":
				type = HitType.WOOD;
				gameObject = (UnityEngine.Object.Instantiate(this.woodParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "metal":
				type = HitType.METAL;
				gameObject = (UnityEngine.Object.Instantiate(this.metalParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "car":
				type = HitType.METAL;
				gameObject = (UnityEngine.Object.Instantiate(this.metalParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "concrete":
				type = HitType.CONCRETE;
				gameObject = (UnityEngine.Object.Instantiate(this.concreteParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "dirt":
				type = HitType.CONCRETE;
				gameObject = (UnityEngine.Object.Instantiate(this.sandParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "sand":
				type = HitType.CONCRETE;
				gameObject = (UnityEngine.Object.Instantiate(this.sandParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "glass":
				type = HitType.GLASS;
				gameObject = (UnityEngine.Object.Instantiate(this.glassParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "water":
				gameObject = (UnityEngine.Object.Instantiate(this.waterParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			case "Enemy":
			case "Head":
			case "Left-Leg":
			case "Right-Leg":
			case "Left-Arm":
			case "Right-Arm":
			case "Player":
				type = HitType.BLOD;
				gameObject = (UnityEngine.Object.Instantiate(this.blodParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
				goto IL_43F;
			}
			type = HitType.CONCRETE;
			gameObject = (UnityEngine.Object.Instantiate(this.sandParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
		}
		IL_43F:
		gameObject.layer = hit.collider.gameObject.layer;
		if (hit.collider.GetComponent<Renderer>() == null)
		{
			return;
		}
		if (AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.GameShield)
		{
			return;
		}
		if (this.timerToCreateDecal < 0f && hit.collider.tag != "water")
		{
			gameObject = (UnityEngine.Object.Instantiate(this.bulletMark, hit.point, Quaternion.FromToRotation(Vector3.forward, -hit.normal)) as GameObject);
			this.decalInfo.type = type;
			this.decalInfo.obj = hit.collider.gameObject;
			gameObject.SendMessage("GenerateDecal", this.decalInfo, SendMessageOptions.DontRequireReceiver);
			this.decalInfo.obj = null;
			this.timerToCreateDecal = 0.02f;
		}
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0006A8D4 File Offset: 0x00068AD4
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && this.weaponHolder.disableUsingWeapons)
		{
			return;
		}
		if (this.gunName == "Grenade")
		{
			return;
		}
		this.timerToCreateDecal -= Time.deltaTime;
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			this.aim = InputManager.GetButton("Fire2");
			if (AnimationHandler.instance != null && AnimationHandler.instance.animState != AnimationHandler.AnimStates.JUMPING && AnimationHandler.instance.animState != AnimationHandler.AnimStates.FALLING && this.shooterCamera != null && !this.shooterCamera.fixedCameraPosition)
			{
				if (!this.fire)
				{
					this.fire = (MobileInput.fire && this.aim);
					this.fireButtonDown = false;
					this.fireButtonUp = false;
					if (this.previousFire != this.fire)
					{
						if (this.fire)
						{
							this.fireButtonDown = true;
						}
						else
						{
							this.fireButtonUp = true;
						}
					}
				}
				if (AndroidPlatform.IsJoystickConnected() && !this.fire)
				{
					if (this.fireMode == FireMode.SEMI_AUTO)
					{
						this.fire = (InputManager.GetButtonDown("Fire1") && this.aim);
					}
					else
					{
						this.fire = (InputManager.GetButton("Fire1") && this.aim);
					}
					this.fire |= InputManager.GetButtonDown("Fire1");
					this.fireButtonDown = false;
					this.fireButtonUp = false;
					if (this.previousFire != this.fire)
					{
						if (this.fire)
						{
							this.fireButtonDown = true;
						}
						else
						{
							this.fireButtonUp = true;
						}
					}
					this.fireButtonDown |= InputManager.GetButtonDown("Fire1");
					this.fireButtonUp |= InputManager.GetButtonUp("Fire1");
				}
				this.previousFire = this.fire;
			}
			if (this.fireButtonDown && this.currentRounds == 0 && !this.reloading && this.freeToShoot)
			{
				this.PlayOutOfAmmoSound();
			}
			if (this.fireButtonUp || this.weaponHolder.blindFire)
			{
				this.freeToShoot = true;
				this.cBurst = this.burstRate;
			}
			if (this.fire && this.weaponHolder.status == WeaponHandling.WeaponStatus.RELAXED && !this.drawing)
			{
				if (!AnimationHandler.instance.insuredMode)
				{
					this.Draw();
				}
				else
				{
					this.weaponHolder.status = WeaponHandling.WeaponStatus.ENGAGED;
				}
			}
		}
		else if (!this.fire)
		{
			this.weaponHolder.status = WeaponHandling.WeaponStatus.ENGAGED;
			this.weaponHolder.EnterIdleFree();
		}
		if (this.drawing)
		{
			this.drawTimer -= Time.deltaTime;
			if (this.drawTimer <= 0f)
			{
				this.drawing = false;
				if (this.aim)
				{
					this.weaponHolder.status = WeaponHandling.WeaponStatus.AIMING;
					this.weaponHolder.EnterAimingMode();
					this.weaponHolder.EnterIdleAimed();
					this.weaponHolder.cam.reticle = this.crosshairWhite;
				}
				else
				{
					this.weaponHolder.EnterIdleFree();
				}
			}
		}
		if ((this.fire || (AnimationHandler.instance != null && AnimationHandler.instance.stealthMode)) && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && this.weaponHolder.status == WeaponHandling.WeaponStatus.ENGAGED && !this.drawing)
		{
			this.lastShootTime = Time.time;
			this.weaponHolder.status = WeaponHandling.WeaponStatus.SHOOTING;
			this.weaponHolder.EnterIdleFree();
			this.ignoreFire = 0.25f;
		}
		if (!this.reloading && !this.weaponHolder.inCover && AnimationHandler.instance != null && !AnimationHandler.instance.insuredMode && Time.time > this.lastShootTime + this.timeBeforeHolster && (this.weaponHolder.status == WeaponHandling.WeaponStatus.ENGAGED || (this.weaponHolder.status == WeaponHandling.WeaponStatus.SHOOTING && AnimationHandler.instance.stealthMode)) && !this.drawing && !this.fire && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			this.weaponHolder.status = WeaponHandling.WeaponStatus.RELAXED;
			this.weaponHolder.Holster();
		}
		if ((Time.time > this.lastShootTime + 5f && this.weaponHolder.status == WeaponHandling.WeaponStatus.SHOOTING && !this.fire && (AnimationHandler.instance == null || !AnimationHandler.instance.stealthMode)) || (AnimationHandler.instance != null && AnimationHandler.instance.insuredMode && this.weaponHolder.status == WeaponHandling.WeaponStatus.SHOOTING && Physics.Raycast(this.weaponTransformReference.position, this.weaponTransformReference.forward, 0.25f)))
		{
			this.weaponHolder.EnterIdleFree();
			this.weaponHolder.status = WeaponHandling.WeaponStatus.ENGAGED;
			this.lastShootTime = Time.time;
			if (AnimationHandler.instance != null && AnimationHandler.instance.insuredMode)
			{
				this.shooterCamera.reticle = null;
			}
		}
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			Ray ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out raycastHit, this.fireRange * this.scaleFactor, this.hitLayer))
			{
				ray = new Ray(this.weaponTransformReference.position, ray.GetPoint(raycastHit.distance) - this.weaponTransformReference.position);
				if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out raycastHit, this.fireRange * this.scaleFactor, this.hitLayer))
				{
					if (raycastHit.collider.tag == "Enemy" || raycastHit.collider.tag == "Head" || raycastHit.collider.tag == "Right-Leg" || raycastHit.collider.tag == "Left-Leg" || raycastHit.collider.tag == "Right-Arm" || raycastHit.collider.tag == "Left-Arm" || raycastHit.collider.tag == "Explosive")
					{
						if (this.weaponHolder.status == WeaponHandling.WeaponStatus.AIMING)
						{
							this.shooterCamera.reticle = this.crosshairRed;
						}
						else if (this.weaponHolder.status != WeaponHandling.WeaponStatus.RELAXED && ((AnimationHandler.instance != null && !AnimationHandler.instance.insuredMode) || this.weaponHolder.status != WeaponHandling.WeaponStatus.ENGAGED))
						{
							this.shooterCamera.reticle = this.weaponHolder.crossHairRed;
						}
					}
					else if (this.weaponHolder.status == WeaponHandling.WeaponStatus.AIMING)
					{
						this.shooterCamera.reticle = this.crosshairWhite;
					}
					else if (this.weaponHolder.status != WeaponHandling.WeaponStatus.RELAXED && ((AnimationHandler.instance != null && !AnimationHandler.instance.insuredMode) || this.weaponHolder.status != WeaponHandling.WeaponStatus.ENGAGED))
					{
						this.shooterCamera.reticle = this.weaponHolder.crossHairWhite;
					}
				}
			}
		}
		this.HandleReloading();
		if (this.ignoreFire > 0f)
		{
			this.ignoreFire -= Time.deltaTime;
		}
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && (WeaponHandling.holdFire || this.ignoreFire > 0f))
		{
			this.fire = false;
			if (this.shotingEmitter != null)
			{
				this.shotingEmitter.ChangeState(false, this.fireMode);
			}
			if (this.capsuleEmitter != null)
			{
				for (int i = 0; i < this.capsuleEmitter.Length; i++)
				{
					if (this.capsuleEmitter[i] != null)
					{
						this.capsuleEmitter[i].emit = false;
					}
				}
			}
			if (this.shotLight != null)
			{
				this.shotLight.enabled = false;
			}
			return;
		}
		this.ShotTheTarget();
		if (this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER)
		{
			this.fire = false;
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0006B2BC File Offset: 0x000694BC
	private void HandleReloading()
	{
		if (InputManager.GetButtonDown("Reload") && !this.reloading && this.weaponHolder.WeaponHolderType == WeaponHandling.WeaponHolderTypes.PLAYER && this.weaponHolder.status != WeaponHandling.WeaponStatus.RELAXED)
		{
			this.Reload();
		}
		if (this.reloading)
		{
			this.reloadTimer -= Time.deltaTime;
			if (this.reloadTimer <= 0f)
			{
				if (this.gunName == "RPJ7")
				{
					this.projectile.transform.position = this.weaponTransformReference.position;
					this.projectile.transform.rotation = this.weaponTransformReference.rotation;
					this.projectile.transform.parent = this.weaponTransformReference;
				}
				this.reloading = false;
				int num = this.currentRounds;
				if (this.currentRounds + this.totalBullets > this.clipSize)
				{
					this.currentRounds = this.clipSize;
				}
				else
				{
					this.currentRounds += this.totalBullets;
				}
				if (!this.unlimited)
				{
					this.totalBullets -= this.clipSize - num;
				}
				if (this.totalBullets < 0)
				{
					this.totalBullets = 0;
				}
			}
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0006B414 File Offset: 0x00069614
	public void Reload()
	{
		if (this.totalBullets > 0 && this.currentRounds < this.clipSize)
		{
			this.PlayReloadSound();
			this.weaponHolder.Reload();
			this.reloading = true;
			this.reloadTime = this.reloadAnimation.length / 1.5f;
			this.reloadTimer = this.reloadTime;
			if (this.gunName == "RPJ7")
			{
				this.reloadTimer = this.reloadTime * 0.9f;
				Transform transform = this.leftHand.transform;
				this.projectile = (UnityEngine.Object.Instantiate(this.projectileDummyPrefab, transform.position, transform.rotation) as GameObject);
				this.projectile.transform.parent = this.leftHand.transform;
			}
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0006B4EC File Offset: 0x000696EC
	private void PlayOutOfAmmoSound()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.outOfAmmoSound, 1.5f * SpeechManager.sfxVolume);
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0006B50C File Offset: 0x0006970C
	private void PlayReloadSound()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.reloadSound, 1.5f * SpeechManager.sfxVolume);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0006B52C File Offset: 0x0006972C
	private void PlayShootSound()
	{
		this.currentShootSound = (this.currentShootSound + 1) % this.shootSounds.Length;
		base.GetComponent<AudioSource>().minDistance = 3f;
		base.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
		base.GetComponent<AudioSource>().spatialBlend = 0f;
		base.GetComponent<AudioSource>().spread = 0f;
		base.GetComponent<AudioSource>().pitch = this.currentPitch;
		if (this.currentPitch == 1f)
		{
			this.currentPitch = 1.1f;
		}
		else if (this.currentPitch == 1.1f)
		{
			this.currentPitch = 0.9f;
		}
		else if (this.currentPitch == 0.9f)
		{
			this.currentPitch = 1f;
		}
		base.GetComponent<AudioSource>().PlayOneShot(this.shootSounds[this.currentShootSound], SpeechManager.sfxVolume);
	}

	// Token: 0x04000F7A RID: 3962
	public string gunName;

	// Token: 0x04000F7B RID: 3963
	public WeaponTypes WeaponType;

	// Token: 0x04000F7C RID: 3964
	public GameObject bulletMark;

	// Token: 0x04000F7D RID: 3965
	public GameObject projectilePrefab;

	// Token: 0x04000F7E RID: 3966
	public GameObject projectileDummyPrefab;

	// Token: 0x04000F7F RID: 3967
	public GameObject pickablePrefab;

	// Token: 0x04000F80 RID: 3968
	public Transform weaponTransformReference;

	// Token: 0x04000F81 RID: 3969
	public Transform weaponTransform;

	// Token: 0x04000F82 RID: 3970
	public LayerMask hitLayer;

	// Token: 0x04000F83 RID: 3971
	public GameObject woodParticle;

	// Token: 0x04000F84 RID: 3972
	public GameObject metalParticle;

	// Token: 0x04000F85 RID: 3973
	public GameObject concreteParticle;

	// Token: 0x04000F86 RID: 3974
	public GameObject sandParticle;

	// Token: 0x04000F87 RID: 3975
	public GameObject waterParticle;

	// Token: 0x04000F88 RID: 3976
	public GameObject blodParticle;

	// Token: 0x04000F89 RID: 3977
	public GameObject glassParticle;

	// Token: 0x04000F8A RID: 3978
	public AnimationClip fireFreeAnimation;

	// Token: 0x04000F8B RID: 3979
	public AnimationClip fireAimedAnimation;

	// Token: 0x04000F8C RID: 3980
	public AnimationClip reloadAnimation;

	// Token: 0x04000F8D RID: 3981
	public AnimationClip drawAnimation;

	// Token: 0x04000F8E RID: 3982
	public AnimationClip holsterAnimation;

	// Token: 0x04000F8F RID: 3983
	public AnimationClip idleFreeAnimation;

	// Token: 0x04000F90 RID: 3984
	public AnimationClip idleAimedAnimation;

	// Token: 0x04000F91 RID: 3985
	public Vector3 pocketOffsetPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x04000F92 RID: 3986
	public Vector3 pocketOffsetRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x04000F93 RID: 3987
	public Vector3 rightHandOffsetPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x04000F94 RID: 3988
	public Vector3 rightHandOffsetRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x04000F95 RID: 3989
	public float accuricyRadius;

	// Token: 0x04000F96 RID: 3990
	public float fireRate;

	// Token: 0x04000F97 RID: 3991
	public bool useGravity;

	// Token: 0x04000F98 RID: 3992
	private FireType fireType;

	// Token: 0x04000F99 RID: 3993
	public FireMode fireMode;

	// Token: 0x04000F9A RID: 3994
	public int burstRate;

	// Token: 0x04000F9B RID: 3995
	public float fireRange;

	// Token: 0x04000F9C RID: 3996
	public float projectileSpeed;

	// Token: 0x04000F9D RID: 3997
	public int clipSize;

	// Token: 0x04000F9E RID: 3998
	public int totalBullets;

	// Token: 0x04000F9F RID: 3999
	public int totalClips;

	// Token: 0x04000FA0 RID: 4000
	public float reloadTime;

	// Token: 0x04000FA1 RID: 4001
	public bool autoReload;

	// Token: 0x04000FA2 RID: 4002
	public int currentRounds;

	// Token: 0x04000FA3 RID: 4003
	public Texture crosshairWhite;

	// Token: 0x04000FA4 RID: 4004
	public Texture crosshairRed;

	// Token: 0x04000FA5 RID: 4005
	public float shootVolume = 0.4f;

	// Token: 0x04000FA6 RID: 4006
	public AudioClip[] shootSounds;

	// Token: 0x04000FA7 RID: 4007
	private int currentShootSound;

	// Token: 0x04000FA8 RID: 4008
	private AudioSource shootSoundSource;

	// Token: 0x04000FA9 RID: 4009
	public AudioClip reloadSound;

	// Token: 0x04000FAA RID: 4010
	private AudioSource reloadSoundSource;

	// Token: 0x04000FAB RID: 4011
	public AudioClip outOfAmmoSound;

	// Token: 0x04000FAC RID: 4012
	private AudioSource outOfAmmoSoundSource;

	// Token: 0x04000FAD RID: 4013
	private float reloadTimer;

	// Token: 0x04000FAE RID: 4014
	private float drawTimer;

	// Token: 0x04000FAF RID: 4015
	[HideInInspector]
	public bool freeToShoot;

	// Token: 0x04000FB0 RID: 4016
	[HideInInspector]
	public bool reloading;

	// Token: 0x04000FB1 RID: 4017
	public bool drawing;

	// Token: 0x04000FB2 RID: 4018
	public float lastShootTime;

	// Token: 0x04000FB3 RID: 4019
	private float shootDelay;

	// Token: 0x04000FB4 RID: 4020
	private int cBurst;

	// Token: 0x04000FB5 RID: 4021
	public bool fire;

	// Token: 0x04000FB6 RID: 4022
	public GameObject hitParticles;

	// Token: 0x04000FB7 RID: 4023
	public GunParticles shotingEmitter;

	// Token: 0x04000FB8 RID: 4024
	private Transform shottingParticles;

	// Token: 0x04000FB9 RID: 4025
	public ParticleEmitter[] capsuleEmitter;

	// Token: 0x04000FBA RID: 4026
	public ShotLight shotLight;

	// Token: 0x04000FBB RID: 4027
	public bool unlimited = true;

	// Token: 0x04000FBC RID: 4028
	private float timerToCreateDecal;

	// Token: 0x04000FBD RID: 4029
	public float pushPower = 3f;

	// Token: 0x04000FBE RID: 4030
	public float damageAmount = 25f;

	// Token: 0x04000FBF RID: 4031
	public WeaponHandling weaponHolder;

	// Token: 0x04000FC0 RID: 4032
	public Camera soldierCamera;

	// Token: 0x04000FC1 RID: 4033
	private Camera cam;

	// Token: 0x04000FC2 RID: 4034
	private ShooterGameCamera shooterCamera;

	// Token: 0x04000FC3 RID: 4035
	private GameObject projectile;

	// Token: 0x04000FC4 RID: 4036
	public Transform leftHand;

	// Token: 0x04000FC5 RID: 4037
	private float timeBeforeHolster;

	// Token: 0x04000FC6 RID: 4038
	private bool rightMissAim = true;

	// Token: 0x04000FC7 RID: 4039
	private ShuffleBag sb;

	// Token: 0x04000FC8 RID: 4040
	public Vector3 hitPosition;

	// Token: 0x04000FC9 RID: 4041
	private bool aim;

	// Token: 0x04000FCA RID: 4042
	private bool previousFire;

	// Token: 0x04000FCB RID: 4043
	private bool fireButtonDown;

	// Token: 0x04000FCC RID: 4044
	private bool fireButtonUp;

	// Token: 0x04000FCD RID: 4045
	private static float lastFireReportTime;

	// Token: 0x04000FCE RID: 4046
	private RaycastHit hit = default(RaycastHit);

	// Token: 0x04000FCF RID: 4047
	private bool hitFound;

	// Token: 0x04000FD0 RID: 4048
	private float lastRaycastCheckTime;

	// Token: 0x04000FD1 RID: 4049
	private float timeBeforeRaycastChecks = 0.1f;

	// Token: 0x04000FD2 RID: 4050
	private bool generateDecal;

	// Token: 0x04000FD3 RID: 4051
	public bool bulletsSaved;

	// Token: 0x04000FD4 RID: 4052
	public float aiAccuracy = 0.35f;

	// Token: 0x04000FD5 RID: 4053
	private ParticleEmitter bulletTrace;

	// Token: 0x04000FD6 RID: 4054
	private Health playerHealth;

	// Token: 0x04000FD7 RID: 4055
	private HitInfo hitInfo;

	// Token: 0x04000FD8 RID: 4056
	private DecalInfo decalInfo;

	// Token: 0x04000FD9 RID: 4057
	private float ignoreFire;

	// Token: 0x04000FDA RID: 4058
	public float scaleFactor = 1f;

	// Token: 0x04000FDB RID: 4059
	public float recoil = 0.2f;

	// Token: 0x04000FDC RID: 4060
	private float currentPitch = 1f;
}
