using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class QuadBikeGun : MonoBehaviour
{
	// Token: 0x06000BC4 RID: 3012 RVA: 0x00095724 File Offset: 0x00093924
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

	// Token: 0x06000BC5 RID: 3013 RVA: 0x000957B8 File Offset: 0x000939B8
	private void OnEnable()
	{
		if (this.soldierCamera == null)
		{
			this.soldierCamera = Camera.main;
		}
		this.cam = this.soldierCamera;
		this.shooterCamera = this.cam.GetComponent<QuadGameCamera>();
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

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00095988 File Offset: 0x00093B88
	public void Start()
	{
		if (base.gameObject.tag == "Enemy")
		{
			if (GameObject.FindGameObjectWithTag("Player") != null)
			{
				this.target = GameObject.FindGameObjectWithTag("Player").transform;
			}
			this.sb = new ShuffleBag();
			this.sb.add(1, Mathf.CeilToInt(this.aiAccuracy * 10f));
			this.sb.add(0, Mathf.CeilToInt((1f - this.aiAccuracy) * 10f));
		}
		if (this.weaponHolder != null)
		{
			this.weaponHolder.GetComponent<Animation>()["Quad-Fire"].AddMixingTransform(this.rightHand);
		}
		if (this.bulletsSaved)
		{
			this.bulletsSaved = false;
		}
		else
		{
			this.totalClips--;
			this.currentRounds = this.clipSize;
		}
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00095A90 File Offset: 0x00093C90
	private void ShotTheTarget()
	{
		if (this.fire && !this.reloading && !this.drawing)
		{
			if (this.weaponHolder != null && !this.weaponHolder.GetComponent<Animation>().IsPlaying("Quad-Fire"))
			{
				this.weaponHolder.GetComponent<Animation>().Blend("Quad-Fire");
			}
			if (this.currentRounds > 0)
			{
				if (Time.time > this.lastShootTime && this.freeToShoot && this.cBurst > 0)
				{
					this.lastShootTime = Time.time + this.shootDelay;
					switch (this.fireMode)
					{
					case FireMode.SEMI_AUTO:
						this.freeToShoot = false;
						break;
					case FireMode.BURST:
						this.cBurst--;
						break;
					}
					//if (this.capsuleEmitter != null)
					//{
						//for (int i = 0; i < this.capsuleEmitter.Length; i++)
						//{
							//this.capsuleEmitter[i].Emit();
						//}
					//}
					this.PlayShootSound();
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
			if (this.weaponHolder != null && this.weaponHolder.GetComponent<Animation>().IsPlaying("Quad-Fire"))
			{
				this.weaponHolder.GetComponent<Animation>().Stop("Quad-Fire");
			}
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

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00095DA4 File Offset: 0x00093FA4
	public void LaunchProjectile()
	{
		Ray ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.6f, 0f));
		Vector3 position;
		if (this.weaponTransformReference != null)
		{
			position = this.weaponTransformReference.position;
		}
		else
		{
			position = this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0.5f));
		}
		if (this.gunName == "RPJ7" && this.projectile != null)
		{
			UnityEngine.Object.Destroy(this.projectile);
		}
		this.projectile = (UnityEngine.Object.Instantiate(this.projectilePrefab, position, Quaternion.identity) as GameObject);
		Grenade component = this.projectile.GetComponent<Grenade>();
		component.soldierCamera = this.soldierCamera.GetComponent<ShooterGameCamera>();
		this.projectile.transform.rotation = Quaternion.LookRotation(ray.direction);
		Rigidbody rigidbody = this.projectile.GetComponent<Rigidbody>();
		if (this.projectile.GetComponent<Rigidbody>() == null)
		{
			rigidbody = this.projectile.AddComponent<Rigidbody>();
		}
		Ray ray2 = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.55f, 0f));
		RaycastHit raycastHit;
		if (Physics.Raycast(ray2.origin, ray2.direction, out raycastHit, this.fireRange, this.hitLayer))
		{
			rigidbody.velocity = (raycastHit.point - this.weaponTransformReference.position).normalized * this.projectileSpeed;
		}
		else
		{
			rigidbody.velocity = (this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.55f, 40f)) - this.weaponTransformReference.position).normalized * this.projectileSpeed;
		}
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00095FCC File Offset: 0x000941CC
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
			if (base.gameObject.tag != "Enemy")
			{
				ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f + num, (float)Screen.height * 0.5f + num2, 0f));
			}
			else
			{
				if (this.target != null)
				{
					a = this.target.position;
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
					ray = new Ray(this.weaponTransformReference.position, a - this.weaponTransformReference.position);
				}
				else
				{
					if (this.target != null)
					{
						Hashtable hashtable = new Hashtable();
						hashtable.Add("D", this.damageAmount);
						hashtable.Add("H", "Body");
						hashtable.Add("source", base.transform.position);
						this.target.BroadcastMessage("Hit", hashtable, SendMessageOptions.DontRequireReceiver);
						return;
					}
					return;
				}
			}
			Vector3 vector = this.weaponTransformReference.position + this.weaponTransformReference.right * 0.2f;
			Debug.DrawRay(ray.origin + ray.direction * 0.1f, ray.direction, Color.red);
			if (Time.time > this.lastRaycastCheckTime + this.timeBeforeRaycastChecks)
			{
				this.hitFound = Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out this.hit, this.fireRange, this.hitLayer);
				this.lastRaycastCheckTime = Time.time;
				this.generateDecal = true;
			}
			else
			{
				this.generateDecal = false;
			}
			if (this.hitFound && this.hit.collider != null && this.hit.collider.name == "Pigeons")
			{
				this.hit.collider.GetComponent<Pigeon_Fly>().OnTriggerEnter(new Collider());
			}
			else if (this.hitFound && this.hit.collider != null && this.hit.collider.name == "Pigeons_Pond")
			{
				this.hit.collider.GetComponent<Pigeon_Fly_Pond>().OnTriggerEnter(new Collider());
			}
			if (this.hitFound && this.hit.collider != null && this.hit.collider.tag == "Breakable")
			{
				Transform parent = this.hit.collider.transform.parent;
				if (this.hit.collider.GetComponent<InteractionTrigger>() != null)
				{
					UnityEngine.Object.Destroy(this.hit.collider.gameObject);
				}
				foreach (object obj in parent)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Rigidbody>() == null)
					{
						transform.gameObject.AddComponent<Rigidbody>();
					}
				}
			}
			if (this.hitFound && this.hit.collider != null && this.hit.collider.tag == "Explosive")
			{
				this.hit.collider.SendMessage("Detonate", SendMessageOptions.DontRequireReceiver);
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
		if (base.gameObject.tag != "Enemy")
		{
			if (this.shottingParticles != null)
			{
				this.shottingParticles.rotation = Quaternion.FromToRotation(Vector3.forward, (this.cam.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, this.cam.farClipPlane)) - this.weaponTransformReference.position).normalized);
			}
		}
		else if (this.shottingParticles != null)
		{
			this.shottingParticles.rotation = Quaternion.FromToRotation(Vector3.forward, (a - this.weaponTransformReference.position).normalized);
		}
		if (this.hitFound)
		{
			if (this.hit.collider != null && this.hit.collider.tag == "Head")
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2.Add("D", 100f);
				hashtable2.Add("H", "Head");
				hashtable2.Add("source", base.transform.position);
				this.hit.collider.transform.gameObject.SendMessage("Hit", hashtable2, SendMessageOptions.DontRequireReceiver);
			}
			else if (this.hit.collider != null)
			{
				Hashtable hashtable3 = new Hashtable();
				hashtable3.Add("D", this.damageAmount);
				hashtable3.Add("H", this.hit.collider.tag);
				hashtable3.Add("source", base.transform.position);
				this.hit.collider.transform.gameObject.SendMessage("Hit", hashtable3, SendMessageOptions.DontRequireReceiver);
			}
			this.GenerateGraphicStuff(this.hit);
		}
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00096760 File Offset: 0x00094960
	private void GenerateGraphicStuff(RaycastHit hit)
	{
		if (hit.collider == null)
		{
			return;
		}
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
		string tag = hit.collider.tag;
		GameObject gameObject;
		switch (tag)
		{
		case "wood":
			gameObject = (UnityEngine.Object.Instantiate(this.woodParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "metal":
			gameObject = (UnityEngine.Object.Instantiate(this.metalParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "car":
			gameObject = (UnityEngine.Object.Instantiate(this.metalParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "concrete":
			gameObject = (UnityEngine.Object.Instantiate(this.concreteParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "dirt":
			gameObject = (UnityEngine.Object.Instantiate(this.sandParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "sand":
			gameObject = (UnityEngine.Object.Instantiate(this.sandParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "water":
			gameObject = (UnityEngine.Object.Instantiate(this.waterParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		case "Enemy":
		case "Head":
		case "Left-Leg":
		case "Right-Leg":
		case "Left-Arm":
		case "Right-Arm":
		case "Player":
			gameObject = (UnityEngine.Object.Instantiate(this.blodParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
			goto IL_3BD;
		}
		gameObject = (UnityEngine.Object.Instantiate(this.sandParticle, position, Quaternion.FromToRotation(Vector3.up, normal)) as GameObject);
		IL_3BD:
		gameObject.layer = hit.collider.gameObject.layer;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00096B44 File Offset: 0x00094D44
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.gunName == "Grenade")
		{
			return;
		}
		this.timerToCreateDecal -= Time.deltaTime;
		if (base.gameObject.tag != "Enemy")
		{
			this.fire =  InputManager.GetButton("Fire1");
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
			this.aim = InputManager.GetButton("Fire2");
			this.previousFire = this.fire;
		}
		if (this.fireButtonDown && this.currentRounds == 0 && !this.reloading && this.freeToShoot)
		{
			this.PlayOutOfAmmoSound();
		}
		if (this.fireButtonUp)
		{
			this.freeToShoot = true;
			this.cBurst = this.burstRate;
		}
		Ray ray = this.cam.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
		Vector3 a = ray.origin;
		Vector3 direction = ray.direction;
		a += direction * 0.1f;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray.origin + ray.direction * 0.1f, ray.direction, out raycastHit, this.fireRange, this.hitLayer) && this.weaponHolder != null)
		{
			if (raycastHit.collider.tag == "Enemy" || raycastHit.collider.tag == "Head" || raycastHit.collider.tag == "Right-Leg" || raycastHit.collider.tag == "Left-Leg" || raycastHit.collider.tag == "Right-Arm" || raycastHit.collider.tag == "Left-Arm")
			{
				this.shooterCamera.reticle = this.crosshairRed;
			}
			else
			{
				this.shooterCamera.reticle = this.crosshairWhite;
			}
		}
		this.HandleReloading();
		this.ShotTheTarget();
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00096E68 File Offset: 0x00095068
	private void HandleReloading()
	{
		if ((InputManager.GetButtonDown("Reload") || Input.GetAxis("Reload") > 0f) && !this.reloading)
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
				if (!this.unlimited)
				{
					this.totalClips--;
				}
				this.currentRounds = this.clipSize;
			}
		}
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00096F64 File Offset: 0x00095164
	public void Reload()
	{
		if (this.totalClips > 0 && this.currentRounds < this.clipSize)
		{
			this.PlayReloadSound();
			this.reloading = true;
			this.reloadTime = this.reloadAnimation.length / 1.5f;
			this.reloadTimer = this.reloadTime;
			base.transform.SendMessageUpwards("PlayReloadAnim", SendMessageOptions.DontRequireReceiver);
			if (this.gunName == "RPJ7")
			{
				this.reloadTimer = this.reloadTime * 0.9f;
				Transform transform = this.leftHand.transform;
				this.projectile = (UnityEngine.Object.Instantiate(this.projectileDummyPrefab, transform.position, transform.rotation) as GameObject);
				this.projectile.transform.parent = this.leftHand.transform;
			}
		}
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00097040 File Offset: 0x00095240
	private void PlayOutOfAmmoSound()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.outOfAmmoSound, 1.5f);
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x00097058 File Offset: 0x00095258
	private void PlayReloadSound()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.reloadSound, 1.5f);
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00097070 File Offset: 0x00095270
	private void PlayShootSound()
	{
		base.GetComponent<AudioSource>().PlayOneShot(this.shootSound);
	}

	// Token: 0x040015B3 RID: 5555
	public string gunName;

	// Token: 0x040015B4 RID: 5556
	public WeaponTypes WeaponType;

	// Token: 0x040015B5 RID: 5557
	public GameObject bulletMark;

	// Token: 0x040015B6 RID: 5558
	public GameObject projectilePrefab;

	// Token: 0x040015B7 RID: 5559
	public GameObject projectileDummyPrefab;

	// Token: 0x040015B8 RID: 5560
	public GameObject pickablePrefab;

	// Token: 0x040015B9 RID: 5561
	public Transform weaponTransformReference;

	// Token: 0x040015BA RID: 5562
	public Transform weaponTransform;

	// Token: 0x040015BB RID: 5563
	public LayerMask hitLayer;

	// Token: 0x040015BC RID: 5564
	public GameObject woodParticle;

	// Token: 0x040015BD RID: 5565
	public GameObject metalParticle;

	// Token: 0x040015BE RID: 5566
	public GameObject concreteParticle;

	// Token: 0x040015BF RID: 5567
	public GameObject sandParticle;

	// Token: 0x040015C0 RID: 5568
	public GameObject waterParticle;

	// Token: 0x040015C1 RID: 5569
	public GameObject blodParticle;

	// Token: 0x040015C2 RID: 5570
	public AnimationClip fireFreeAnimation;

	// Token: 0x040015C3 RID: 5571
	public AnimationClip fireAimedAnimation;

	// Token: 0x040015C4 RID: 5572
	public AnimationClip reloadAnimation;

	// Token: 0x040015C5 RID: 5573
	public AnimationClip drawAnimation;

	// Token: 0x040015C6 RID: 5574
	public AnimationClip holsterAnimation;

	// Token: 0x040015C7 RID: 5575
	public AnimationClip idleFreeAnimation;

	// Token: 0x040015C8 RID: 5576
	public AnimationClip idleAimedAnimation;

	// Token: 0x040015C9 RID: 5577
	public Vector3 pocketOffsetPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x040015CA RID: 5578
	public Vector3 pocketOffsetRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x040015CB RID: 5579
	public Vector3 rightHandOffsetPosition = new Vector3(0f, 0f, 0f);

	// Token: 0x040015CC RID: 5580
	public Vector3 rightHandOffsetRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x040015CD RID: 5581
	public float accuricyRadius;

	// Token: 0x040015CE RID: 5582
	public float fireRate;

	// Token: 0x040015CF RID: 5583
	public bool useGravity;

	// Token: 0x040015D0 RID: 5584
	private FireType fireType;

	// Token: 0x040015D1 RID: 5585
	public FireMode fireMode;

	// Token: 0x040015D2 RID: 5586
	public int burstRate;

	// Token: 0x040015D3 RID: 5587
	public float fireRange;

	// Token: 0x040015D4 RID: 5588
	public float projectileSpeed;

	// Token: 0x040015D5 RID: 5589
	public int clipSize;

	// Token: 0x040015D6 RID: 5590
	public int totalClips;

	// Token: 0x040015D7 RID: 5591
	public float reloadTime;

	// Token: 0x040015D8 RID: 5592
	public bool autoReload;

	// Token: 0x040015D9 RID: 5593
	public int currentRounds;

	// Token: 0x040015DA RID: 5594
	public Texture crosshairWhite;

	// Token: 0x040015DB RID: 5595
	public Texture crosshairRed;

	// Token: 0x040015DC RID: 5596
	public float shootVolume = 0.4f;

	// Token: 0x040015DD RID: 5597
	public AudioClip shootSound;

	// Token: 0x040015DE RID: 5598
	private AudioSource shootSoundSource;

	// Token: 0x040015DF RID: 5599
	public AudioClip reloadSound;

	// Token: 0x040015E0 RID: 5600
	private AudioSource reloadSoundSource;

	// Token: 0x040015E1 RID: 5601
	public AudioClip outOfAmmoSound;

	// Token: 0x040015E2 RID: 5602
	private AudioSource outOfAmmoSoundSource;

	// Token: 0x040015E3 RID: 5603
	private float reloadTimer;

	// Token: 0x040015E4 RID: 5604
	private float drawTimer;

	// Token: 0x040015E5 RID: 5605
	[HideInInspector]
	public bool freeToShoot;

	// Token: 0x040015E6 RID: 5606
	[HideInInspector]
	public bool reloading;

	// Token: 0x040015E7 RID: 5607
	public bool drawing;

	// Token: 0x040015E8 RID: 5608
	public float lastShootTime;

	// Token: 0x040015E9 RID: 5609
	private float shootDelay;

	// Token: 0x040015EA RID: 5610
	private int cBurst;

	// Token: 0x040015EB RID: 5611
	public bool fire;

	// Token: 0x040015EC RID: 5612
	public GameObject hitParticles;

	// Token: 0x040015ED RID: 5613
	public GunParticles shotingEmitter;

	// Token: 0x040015EE RID: 5614
	private Transform shottingParticles;

	// Token: 0x040015EF RID: 5615
	public ParticleEmitter[] capsuleEmitter;

	// Token: 0x040015F0 RID: 5616
	public ShotLight shotLight;

	// Token: 0x040015F1 RID: 5617
	public bool unlimited = true;

	// Token: 0x040015F2 RID: 5618
	private float timerToCreateDecal;

	// Token: 0x040015F3 RID: 5619
	public float pushPower = 3f;

	// Token: 0x040015F4 RID: 5620
	public float damageAmount = 25f;

	// Token: 0x040015F5 RID: 5621
	public Camera soldierCamera;

	// Token: 0x040015F6 RID: 5622
	private Camera cam;

	// Token: 0x040015F7 RID: 5623
	private QuadGameCamera shooterCamera;

	// Token: 0x040015F8 RID: 5624
	private GameObject projectile;

	// Token: 0x040015F9 RID: 5625
	public Transform leftHand;

	// Token: 0x040015FA RID: 5626
	private float timeBeforeHolster;

	// Token: 0x040015FB RID: 5627
	private bool rightMissAim = true;

	// Token: 0x040015FC RID: 5628
	private ShuffleBag sb;

	// Token: 0x040015FD RID: 5629
	public Vector3 hitPosition;

	// Token: 0x040015FE RID: 5630
	private bool aim;

	// Token: 0x040015FF RID: 5631
	private bool previousFire;

	// Token: 0x04001600 RID: 5632
	private bool fireButtonDown;

	// Token: 0x04001601 RID: 5633
	private bool fireButtonUp;

	// Token: 0x04001602 RID: 5634
	private static float lastFireReportTime;

	// Token: 0x04001603 RID: 5635
	private RaycastHit hit = default(RaycastHit);

	// Token: 0x04001604 RID: 5636
	private bool hitFound;

	// Token: 0x04001605 RID: 5637
	private float lastRaycastCheckTime;

	// Token: 0x04001606 RID: 5638
	private float timeBeforeRaycastChecks = 0.1f;

	// Token: 0x04001607 RID: 5639
	private bool generateDecal;

	// Token: 0x04001608 RID: 5640
	public bool bulletsSaved;

	// Token: 0x04001609 RID: 5641
	public float aiAccuracy = 0.35f;

	// Token: 0x0400160A RID: 5642
	public Transform weaponHolder;

	// Token: 0x0400160B RID: 5643
	public Transform rightHand;

	// Token: 0x0400160C RID: 5644
	private Transform target;
}
