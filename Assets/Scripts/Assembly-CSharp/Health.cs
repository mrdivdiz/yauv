using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class Health : MonoBehaviour
{
	// Token: 0x0600098B RID: 2443 RVA: 0x00055FF4 File Offset: 0x000541F4
	private void Start()
	{
		//while(this.cam == null){
		this.cam = GlobalFarisCam.farisCamera.gameObject.GetComponent<ShooterGameCamera>();
		//}
		switch (DifficultyManager.difficulty)
		{
		case DifficultyManager.Difficulty.EASY:
			if (base.gameObject.tag == "Enemy")
			{
				this.health *= DifficultyManager.easyGeneral;
			}
			else if (base.gameObject.tag == "Player")
			{
				this.health /= DifficultyManager.easyGeneral;
			}
			break;
		case DifficultyManager.Difficulty.HARD:
			if (base.gameObject.tag == "Enemy")
			{
				this.health *= DifficultyManager.hardGeneral;
			}
			else if (base.gameObject.tag == "Player")
			{
				this.health /= DifficultyManager.hardGeneral;
			}
			break;
		}
		this.maxHealth = this.health;
		Screen.lockCursor = true;
		this.botAI = base.gameObject.GetComponent<BotAI>();
		this.cameraTransform = GlobalFarisCam.farisCamera.transform;
		this.currentHitAnimation = UnityEngine.Random.Range(0, 4);
		if (base.gameObject.tag == "Enemy")
		{
			this.player = GameObject.FindGameObjectWithTag("Player").transform;
			this.playerHealth = this.player.gameObject.GetComponent<Health>();
		}
		else if (base.gameObject.tag == "Player")
		{
			this.animHandler = base.gameObject.GetComponent<AnimationHandler>();
		}
		if (base.gameObject.tag == "Enemy")
		{
			Health.currentFrontTakedownAnim = UnityEngine.Random.Range(0, this.frontTakedowns.Length);
			Health.currentBackTakedownAnim = UnityEngine.Random.Range(0, this.backTakedowns.Length);
		}
		this.cam.hitAlpha = 0f;
		this.cam.blackAlpha = 0f;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0005620C File Offset: 0x0005440C
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.hitTimer > 0f)
		{
			this.hitTimer -= Time.deltaTime;
		}
		if (this.activateDiyingTimer)
		{
			this.diyingTimer -= Time.deltaTime;
			if (this.diyingTimer <= 1f)
			{
				this.cam.takeDown = false;
				this.player.gameObject.GetComponent<NormalCharacterMotor>().disableMovement = false;
				this.player.gameObject.GetComponent<NormalCharacterMotor>().disableRotation = false;
				this.player.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = false;
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				this.activateDiyingTimer = false;
				this.Die();
				if (this.botAI != null)
				{
					if (this.botAI.weapon != null)
					{
						GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.botAI.weapon.pickablePrefab, this.botAI.weapon.weaponTransform.position, this.botAI.weapon.weaponTransform.rotation);
						gameObject.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(0f, 2.5f, 3f));
					}
					if (this.botAI.pickupableGrenadePrefab != null && this.botAI.launchGrenades)
					{
						GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(this.botAI.pickupableGrenadePrefab, this.botAI.weapon.weaponTransform.position, this.botAI.weapon.weaponTransform.rotation);
						gameObject2.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(0f, 2.5f, 3f));
					}
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		if (this.health <= 0f)
		{
			if (this.dead == null)
			{
				this.Die();
				if (this.botAI != null && this.botAI.currentCover != null)
				{
					this.botAI.currentCover.isUsedByAI = false;
				}
				if (this.botAI != null)
				{
					if (this.botAI.weapon != null)
					{
						GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(this.botAI.weapon.pickablePrefab, this.botAI.weapon.weaponTransform.position, this.botAI.weapon.weaponTransform.rotation);
						gameObject3.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(0f, 2.5f, 3f));
					}
					if (this.botAI.pickupableGrenadePrefab != null && this.botAI.launchGrenades)
					{
						GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(this.botAI.pickupableGrenadePrefab, this.botAI.weapon.weaponTransform.position, this.botAI.weapon.weaponTransform.rotation);
						gameObject4.GetComponent<Rigidbody>().velocity = base.transform.TransformDirection(new Vector3(0f, 2.5f, 3f));
					}
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (base.gameObject.tag == "Player" && this.healTime != 0f && this.health < this.maxHealth)
		{
			this.health += this.maxHealth / this.healTime * Time.deltaTime;
		}
		if (this.health > 0f && base.gameObject.tag == "Player")
		{
			this.cam.hitAlpha = (this.maxHealth - this.health) / this.maxHealth;
		}
		if (this.health > this.maxHealth)
		{
			this.health = this.maxHealth;
		}
		if (base.gameObject.tag == "Player")
		{
			if ((double)this.health < 0.5 * (double)this.maxHealth)
			{
				if (!base.GetComponent<AudioSource>().isPlaying && base.GetComponent<AudioSource>().enabled)
				{
					base.GetComponent<AudioSource>().clip = this.heartBeatSound;
					base.GetComponent<AudioSource>().volume = SpeechManager.sfxVolume;
					base.GetComponent<AudioSource>().Play();
				}
			}
			else if (base.GetComponent<AudioSource>().isPlaying && base.GetComponent<AudioSource>().clip == this.heartBeatSound)
			{
				base.GetComponent<AudioSource>().Stop();
			}
		}
		if (base.gameObject.tag == "Enemy" && AnimationHandler.instance != null && !AnimationHandler.instance.isInInteraction && AnimationHandler.instance.animState != AnimationHandler.AnimStates.JUMPING && AnimationHandler.instance.animState != AnimationHandler.AnimStates.FALLING && this.CheckForTakeDown())
		{
			this.PerformTakeDown();
		}
		if (this.NorthHit > 0f)
		{
			this.NorthHit -= Time.deltaTime;
		}
		if (this.NorthEastHit > 0f)
		{
			this.NorthEastHit -= Time.deltaTime;
		}
		if (this.EastHit > 0f)
		{
			this.EastHit -= Time.deltaTime;
		}
		if (this.SouthEastHit > 0f)
		{
			this.SouthEastHit -= Time.deltaTime;
		}
		if (this.SouthHit > 0f)
		{
			this.SouthHit -= Time.deltaTime;
		}
		if (this.NorthWestHit > 0f)
		{
			this.NorthWestHit -= Time.deltaTime;
		}
		if (this.WestHit > 0f)
		{
			this.WestHit -= Time.deltaTime;
		}
		if (this.SouthWestHit > 0f)
		{
			this.SouthWestHit -= Time.deltaTime;
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00056898 File Offset: 0x00054A98
	private bool CheckForTakeDown()
	{
		if (this.player == null)
		{
			return false;
		}
		Vector3 position = base.transform.position;
		position.y = 0f;
		Vector3 position2 = this.player.position;
		position2.y = 0f;
		if (this.punched || Vector3.Distance(position, position2) >= 2f || AnimationHandler.noTakedownZone)
		{
			if (AndroidPlatform.IsJoystickConnected())
			{
				if (this.punchNeedsDisabling && Instructions.instructionSetter == base.gameObject)
				{
					Instructions.instruction = Instructions.Instruction.NONE;
					this.punchNeedsDisabling = false;
				}
			}
			else if (this.punchNeedsDisabling)
			{
				MobileInput.instance.disableButton("punch", base.gameObject);
				this.punchNeedsDisabling = false;
			}
			return false;
		}
		if (base.transform.InverseTransformPoint(this.player.position).z > 0f || Application.loadedLevelName != "Prologue")
		{
			if (AndroidPlatform.IsJoystickConnected())
			{
				Instructions.instructionSetter = base.gameObject;
				Instructions.instruction = Instructions.Instruction.ATTACK;
				this.punchNeedsDisabling = true;
				this.attack = InputManager.GetButton("Attack");
			}
			else
			{
				MobileInput.instance.enableButton("punch", base.gameObject);
				this.punchNeedsDisabling = true;
				this.attack = (MobileInput.punch || InputManager.GetButton("Attack"));
			}
		}
		if (this.attack)
		{
			MobileInput.instance.disableButton("punch", base.gameObject);
			this.punched = true;
			return true;
		}
		return false;
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00056A4C File Offset: 0x00054C4C
	private void PerformTakeDown()
	{
		Vector3 position = base.transform.position;
		position.y = this.player.position.y;
		Vector3 vector = position;
		string name;
		string name2;
		if (base.transform.InverseTransformPoint(this.player.position).z > 0f)
		{
			if (Application.loadedLevelName == "Prologue")
			{
				while (!this.frontTakedowns[Health.currentFrontTakedownAnim].useInPrologue)
				{
					Health.currentFrontTakedownAnim = (Health.currentFrontTakedownAnim + 1) % this.frontTakedowns.Length;
				}
			}
			name = this.frontTakedowns[Health.currentFrontTakedownAnim].playerAnimation.name;
			name2 = this.frontTakedowns[Health.currentFrontTakedownAnim].enemyAnimation.name;
			vector += base.transform.forward * this.frontTakedowns[Health.currentFrontTakedownAnim].distance * base.transform.localScale.z;
			this.player.gameObject.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y - 180f, base.transform.rotation.eulerAngles.z);
			if (this.frontTakedowns[Health.currentFrontTakedownAnim].takedownSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.frontTakedowns[Health.currentFrontTakedownAnim].takedownSound, SpeechManager.speechVolume);
			}
			Health.currentFrontTakedownAnim = (Health.currentFrontTakedownAnim + 1) % this.frontTakedowns.Length;
		}
		else
		{
			name = this.backTakedowns[Health.currentBackTakedownAnim].playerAnimation.name;
			name2 = this.backTakedowns[Health.currentBackTakedownAnim].enemyAnimation.name;
			vector -= base.transform.forward * this.backTakedowns[Health.currentBackTakedownAnim].distance * base.transform.localScale.z;
			this.player.gameObject.transform.rotation = base.transform.rotation;
			if (this.backTakedowns[Health.currentBackTakedownAnim].takedownSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.backTakedowns[Health.currentBackTakedownAnim].takedownSound, SpeechManager.speechVolume);
			}
			Health.currentBackTakedownAnim = (Health.currentBackTakedownAnim + 1) % this.backTakedowns.Length;
		}
		this.player.gameObject.transform.position = vector;
		this.botAI.animState = BotAI.AnimStates.IDLE;
		base.GetComponent<Animation>()[name2].layer = 11;
		base.GetComponent<Animation>().CrossFade(name2, 0.3f, PlayMode.StopAll);
		this.botAI.weapon.fire = false;
		this.botAI.holdFire = base.GetComponent<Animation>()[name2].length;
		this.botAI.enabled = false;
		this.player.gameObject.GetComponent<Animation>()[name].layer = 11;
		this.player.gameObject.GetComponent<Animation>().CrossFade(name, 0.3f, PlayMode.StopAll);
		AnimationHandler.instance.faceState = AnimationHandler.FaceState.TAKEDOWN;
		this.cam.takeDown = true;
		this.player.gameObject.GetComponent<NormalCharacterMotor>().disableMovement = true;
		this.player.gameObject.GetComponent<NormalCharacterMotor>().disableRotation = true;
		this.player.gameObject.GetComponent<BasicAgility>().animatingTimer = this.player.gameObject.GetComponent<Animation>()[name].length;
		this.player.gameObject.GetComponent<BasicAgility>().rootMotion = true;
		this.player.gameObject.GetComponent<BasicAgility>().animating = true;
		this.player.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
		this.diyingTimer = base.GetComponent<Animation>()[name2].length;
		this.activateDiyingTimer = true;
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00056E9C File Offset: 0x0005509C
	public void DecreasHealth(float amount)
	{
		if (this.unlimitedHealth)
		{
			return;
		}
		this.health -= amount;
		if (this.health < 0f)
		{
			this.health = 0f;
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00056ED4 File Offset: 0x000550D4
	public void DecreasHealthPercentatge(float amount)
	{
		if (this.unlimitedHealth)
		{
			return;
		}
		this.health -= amount * this.maxHealth;
		if (this.health < 0f)
		{
			this.health = 0f;
		}
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00056F20 File Offset: 0x00055120
	public void IncreaseHealth(float amount)
	{
		this.health += amount;
		if (this.health > this.maxHealth)
		{
			this.health = this.maxHealth;
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00056F50 File Offset: 0x00055150
	public void Hit(HitInfo hitInfo)
	{
		if (hitInfo.E != 0f)
		{
			if (this.botAI != null)
			{
				this.botAI.HitExplosion();
			}
			this.explosionPower = hitInfo.E;
			this.explosionPosition = hitInfo.EP;
			this.explosionRadius = hitInfo.ER;
			if (this.botAI != null && this.botAI.enteryStopped)
			{
				this.DecreasHealth(this.maxHealth);
			}
		}
		else
		{
			this.explosionPower = 0f;
		}
		if (base.gameObject.tag != "Player" && hitInfo.H != string.Empty)
		{
			if (this.botAI.EntryAnim != null && this.botAI.entered && this.botAI.entryDuration > 0f)
			{
				return;
			}
			if (hitInfo.source != Vector3.zero)
			{
				if (this.botAI.groupID != "NONE")
				{
					BotAI.lastSeenPositions[this.botAI.groupID] = hitInfo.source;
				}
				else
				{
					this.botAI.lastSeenPosition = hitInfo.source;
				}
			}
			if (this.botAI.animState != BotAI.AnimStates.TAKECOVER)
			{
				this.botAI.animState = BotAI.AnimStates.IDLE;
			}
			string h = hitInfo.H;
			switch (h)
			{
			case "Left-Leg":
				if (!base.GetComponent<Animation>().IsPlaying("Hit-Leg-Left"))
				{
					if (this.botAI.animState != BotAI.AnimStates.TAKECOVER)
					{
						base.GetComponent<Animation>()["Hit-Leg-Left"].layer = 10;
						base.GetComponent<Animation>()["Hit-Leg-Left"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Leg-Left");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Leg-Left"].length;
					}
					if (this.hitTimer <= 0f)
					{
						AudioClip audioClip = this.leftLegHitSounds[UnityEngine.Random.Range(0, this.leftLegHitSounds.Length)];
						base.GetComponent<AudioSource>().PlayOneShot(audioClip, SpeechManager.speechVolume);
						this.hitTimer = audioClip.length;
					}
				}
				goto IL_8FD;
			case "Right-Leg":
				if (!base.GetComponent<Animation>().IsPlaying("Hit-Leg-Right"))
				{
					if (this.botAI.animState != BotAI.AnimStates.TAKECOVER)
					{
						base.GetComponent<Animation>()["Hit-Leg-Right"].layer = 10;
						base.GetComponent<Animation>()["Hit-Leg-Right"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Leg-Right");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Leg-Right"].length;
					}
					if (this.hitTimer <= 0f)
					{
						AudioClip audioClip2 = this.rightLegHitSounds[UnityEngine.Random.Range(0, this.rightLegHitSounds.Length)];
						base.GetComponent<AudioSource>().PlayOneShot(audioClip2, SpeechManager.speechVolume);
						this.hitTimer = audioClip2.length;
					}
				}
				goto IL_8FD;
			case "Left-Arm":
				if (!base.GetComponent<Animation>().IsPlaying("Hit-Arm-Left"))
				{
					if (this.botAI.animState != BotAI.AnimStates.TAKECOVER)
					{
						base.GetComponent<Animation>()["Hit-Arm-Left"].layer = 10;
						base.GetComponent<Animation>()["Hit-Arm-Left"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Arm-Left");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Arm-Left"].length;
					}
					if (this.hitTimer <= 0f)
					{
						AudioClip audioClip3 = this.leftArmHitSounds[UnityEngine.Random.Range(0, this.leftArmHitSounds.Length)];
						base.GetComponent<AudioSource>().PlayOneShot(audioClip3, SpeechManager.speechVolume);
						this.hitTimer = audioClip3.length;
					}
				}
				goto IL_8FD;
			case "Right-Arm":
				if (!base.GetComponent<Animation>().IsPlaying("Hit-Arm-Right"))
				{
					if (this.botAI.animState != BotAI.AnimStates.TAKECOVER)
					{
						base.GetComponent<Animation>()["Hit-Arm-Right"].layer = 10;
						base.GetComponent<Animation>()["Hit-Arm-Right"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Arm-Right");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Arm-Right"].length;
					}
					if (this.hitTimer <= 0f)
					{
						AudioClip audioClip4 = this.rightArmHitSounds[UnityEngine.Random.Range(0, this.rightArmHitSounds.Length)];
						base.GetComponent<AudioSource>().PlayOneShot(audioClip4, SpeechManager.speechVolume);
						this.hitTimer = audioClip4.length;
					}
				}
				goto IL_8FD;
			case "Head":
				base.GetComponent<AudioSource>().PlayOneShot(this.headHitSounds[UnityEngine.Random.Range(0, this.headHitSounds.Length)], SpeechManager.speechVolume);
				this.playerHealth.GetComponent<AudioSource>().Stop();
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.playerHealth.FarisSoundsforHeadHitArabic[Health.currentHeadHitFarisSound] != null)
				{
					if (Health.HeadHitSound == 0)
					{
						this.playerHealth.GetComponent<AudioSource>().PlayOneShot(this.playerHealth.FarisSoundsforHeadHitArabic[Health.currentHeadHitFarisSound], SpeechManager.speechVolume);
						Health.currentHeadHitFarisSound = (Health.currentHeadHitFarisSound + 1) % this.playerHealth.FarisSoundsforHeadHitArabic.Length;
						Health.HeadHitSound = 3;
					}
					else
					{
						Health.HeadHitSound--;
					}
				}
				else if (Health.HeadHitSound == 0)
				{
					this.playerHealth.GetComponent<AudioSource>().PlayOneShot(this.playerHealth.FarisSoundsforHeadHit[Health.currentHeadHitFarisSound], SpeechManager.speechVolume);
					Health.currentHeadHitFarisSound = (Health.currentHeadHitFarisSound + 1) % this.playerHealth.FarisSoundsforHeadHit.Length;
					Health.HeadHitSound = 3;
				}
				else
				{
					Health.HeadHitSound--;
				}
				goto IL_8FD;
			}
			if (this.botAI.animState != BotAI.AnimStates.TAKECOVER)
			{
				switch (this.currentHitAnimation)
				{
				case 0:
					if (!base.GetComponent<Animation>().IsPlaying("Hit-Body-4"))
					{
						base.GetComponent<Animation>()["Hit-Body-1"].layer = 10;
						base.GetComponent<Animation>()["Hit-Body-1"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Body-1");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Body-1"].length;
						this.currentHitAnimation++;
					}
					break;
				case 1:
					if (!base.GetComponent<Animation>().IsPlaying("Hit-Body-1"))
					{
						base.GetComponent<Animation>()["Hit-Body-2"].layer = 10;
						base.GetComponent<Animation>()["Hit-Body-2"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Body-2");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Body-2"].length;
						this.currentHitAnimation++;
					}
					break;
				case 2:
					if (!base.GetComponent<Animation>().IsPlaying("Hit-Body-2"))
					{
						base.GetComponent<Animation>()["Hit-Body-3"].layer = 10;
						base.GetComponent<Animation>()["Hit-Body-3"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Body-3");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Body-3"].length;
						this.currentHitAnimation++;
					}
					break;
				case 3:
					if (!base.GetComponent<Animation>().IsPlaying("Hit-Body-3"))
					{
						base.GetComponent<Animation>()["Hit-Body-4"].layer = 10;
						base.GetComponent<Animation>()["Hit-Body-4"].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade("Hit-Body-4");
						this.botAI.holdFire = base.GetComponent<Animation>()["Hit-Body-4"].length;
						this.currentHitAnimation = 0;
					}
					break;
				}
			}
			if (this.hitTimer <= 0f)
			{
				AudioClip audioClip5 = this.bodyHitSounds[this.currentHitAnimation];
				base.GetComponent<AudioSource>().PlayOneShot(audioClip5, SpeechManager.speechVolume);
				this.hitTimer = audioClip5.length;
			}
		}
		IL_8FD:
		if (base.gameObject.tag == "Player" && hitInfo.source != Vector3.zero)
		{
			this.animHandler.holdIdleAnimations = 10f;
			Vector3 source = hitInfo.source;
			source.y = this.cameraTransform.position.y;
			Vector3 forward = this.cameraTransform.forward;
			forward.y = 0f;
			float num2 = Vector3.Angle(forward, source - base.transform.position);
			if (Vector3.Cross(forward, (source - this.cameraTransform.position).normalized).y > 0f)
			{
				if (num2 < 22.5f)
				{
					this.NorthHit = this.hitDisplayTime;
				}
				else if (num2 < 45f)
				{
					this.NorthEastHit = this.hitDisplayTime;
				}
				else if (num2 < 67.5f)
				{
					this.EastHit = this.hitDisplayTime;
				}
				else if (num2 < 90f)
				{
					this.SouthEastHit = this.hitDisplayTime;
				}
				else
				{
					this.SouthHit = this.hitDisplayTime;
				}
			}
			else if (num2 < 22.5f)
			{
				this.NorthHit = this.hitDisplayTime;
			}
			else if (num2 < 45f)
			{
				this.NorthWestHit = this.hitDisplayTime;
			}
			else if (num2 < 67.5f)
			{
				this.WestHit = this.hitDisplayTime;
			}
			else if (num2 < 90f)
			{
				this.SouthWestHit = this.hitDisplayTime;
			}
			else
			{
				this.SouthHit = this.hitDisplayTime;
			}
		}
		if (this.unlimitedHealth)
		{
			return;
		}
		this.health -= hitInfo.D;
		if (this.health < 0f)
		{
			this.health = 0f;
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x00057A68 File Offset: 0x00055C68
	public void playerHit(Vector3 source, float damage)
	{
		PadVibrator.VibrateInterval(true, 0.3f, 0.3f);
		this.explosionPower = 0f;
		this.animHandler.holdIdleAnimations = 10f;
		Vector3 a = source;
		a.y = this.cameraTransform.position.y;
		Vector3 forward = this.cameraTransform.forward;
		forward.y = 0f;
		float num = Vector3.Angle(forward, a - base.transform.position);
		if (Vector3.Cross(forward, (a - this.cameraTransform.position).normalized).y > 0f)
		{
			if (num < 22.5f)
			{
				this.NorthHit = this.hitDisplayTime;
			}
			else if (num < 45f)
			{
				this.NorthEastHit = this.hitDisplayTime;
			}
			else if (num < 67.5f)
			{
				this.EastHit = this.hitDisplayTime;
			}
			else if (num < 90f)
			{
				this.SouthEastHit = this.hitDisplayTime;
			}
			else
			{
				this.SouthHit = this.hitDisplayTime;
			}
		}
		else if (num < 22.5f)
		{
			this.NorthHit = this.hitDisplayTime;
		}
		else if (num < 45f)
		{
			this.NorthWestHit = this.hitDisplayTime;
		}
		else if (num < 67.5f)
		{
			this.WestHit = this.hitDisplayTime;
		}
		else if (num < 90f)
		{
			this.SouthWestHit = this.hitDisplayTime;
		}
		else
		{
			this.SouthHit = this.hitDisplayTime;
		}
		if (this.unlimitedHealth)
		{
			return;
		}
		this.health -= damage;
		if (this.health < 0f)
		{
			this.health = 0f;
		}
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x00057C54 File Offset: 0x00055E54
	private void Die()
	{
		if (Application.loadedLevelName == "Prototype" && base.gameObject.tag != "Player")
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
			if (array.Length == 1)
			{
				mainmenu.Instance.GoToLevel("LoadingMainMenu");
			}
		}
		if (Application.loadedLevelName == "Prologue" && base.gameObject.tag != "Player")
		{
			this.cam.prioritizedAimTarget = null;
		}
		if (this.CoffeeMachine)
		{
			OpenElevator.GruntPrologueDied = true;
		}
		if (this.ElevatorMan)
		{
			OpenElevator.ElevatorManDied = true;
		}
		if (this.RagdollPrefab)
		{/*
			if (this.RagdollPrefab != null && this.scaleFactor != 1f)
			{
				this.RagdollPrefab.localScale *= this.scaleFactor;
			}
			this.dead = (Transform)UnityEngine.Object.Instantiate(this.RagdollPrefab, base.transform.position, base.transform.rotation);
			if (this.RagdollPrefab != null && this.scaleFactor != 1f)
			{
				this.RagdollPrefab.localScale /= this.scaleFactor;
			}
			if (this.addPlayerRagdollScript)
			{
				this.dead.gameObject.AddComponent<PlayerRagdollScript>();
			}
			if (base.gameObject.tag == "Player")
			{
				this.cam.hitAlpha = 1f;
			}
			Vector3 velocity = Vector3.zero;
			if (base.GetComponent<Rigidbody>())
			{
				velocity = base.GetComponent<Rigidbody>().velocity;
				base.GetComponent<Rigidbody>().AddExplosionForce(this.explosionPower, this.explosionPosition, this.explosionRadius, 3f);
			}
			else
			{
				CharacterController component = base.GetComponent<CharacterController>();
				velocity = component.velocity;
			}
			this.CopyTransformsRecurse(base.transform, this.dead, velocity);
			if (base.gameObject.tag == "Player")
			{
				if (SpeechManager.instance != null && SpeechManager.instance.GetComponent<AudioSource>() != null && this.deathSound != null)
				{
					SpeechManager.instance.GetComponent<AudioSource>().PlayOneShot(this.deathSound);
				}
				if (!AnimationHandler.instance.insuredMode)
				{
					this.cam.pivotOffset = new Vector3(0f, 1.6f, 0f);
					this.cam.camOffset = new Vector3(0f, 0f, -4.5f);
					this.cam.closeOffset = new Vector3(0.35f, 1.7f, 0f);
				}
				else
				{
					this.cam.pivotOffset = new Vector3(0f, 1.6f, 0f);
					this.cam.camOffset = new Vector3(0f, -1.5f, -4.5f);
					this.cam.closeOffset = new Vector3(0.35f, 1.7f, 0f);
				}
				this.cam.player = this.dead.Find("Bip01");
			}
		*/}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00057FC8 File Offset: 0x000561C8
	private void CopyTransformsRecurse(Transform src, Transform dst, Vector3 velocity)
	{
		Rigidbody rigidbody = dst.GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			rigidbody.velocity = velocity;
			rigidbody.useGravity = true;
			if (this.explosionPower != 0f)
			{
				rigidbody.AddExplosionForce(this.explosionPower, this.explosionPosition, this.explosionRadius, 3f);
			}
		}
		dst.position = src.position;
		dst.rotation = src.rotation;
		foreach (object obj in dst)
		{
			Transform transform = (Transform)obj;
			Transform transform2 = src.Find(transform.name);
			if (transform2)
			{
				this.CopyTransformsRecurse(transform2, transform, velocity);
			}
		}
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x000580B8 File Offset: 0x000562B8
	public void Destroy()
	{
		for (int i = 0; i < this.frontTakedowns.Length; i++)
		{
			UnityEngine.Object.Destroy(this.frontTakedowns[i].playerAnimation);
			UnityEngine.Object.Destroy(this.frontTakedowns[i].enemyAnimation);
			UnityEngine.Object.Destroy(this.frontTakedowns[i].takedownSound);
			this.frontTakedowns[i].playerAnimation = null;
			this.frontTakedowns[i].enemyAnimation = null;
			this.frontTakedowns[i].takedownSound = null;
			this.frontTakedowns[i] = null;
		}
		this.frontTakedowns = new TakedownAnim[0];
		for (int j = 0; j < this.backTakedowns.Length; j++)
		{
			UnityEngine.Object.Destroy(this.backTakedowns[j].playerAnimation);
			UnityEngine.Object.Destroy(this.backTakedowns[j].enemyAnimation);
			UnityEngine.Object.Destroy(this.backTakedowns[j].takedownSound);
			this.backTakedowns[j].playerAnimation = null;
			this.backTakedowns[j].enemyAnimation = null;
			this.backTakedowns[j].takedownSound = null;
			this.backTakedowns[j] = null;
		}
		this.backTakedowns = new TakedownAnim[0];
		for (int k = 0; k < this.bodyHitSounds.Length; k++)
		{
			UnityEngine.Object.Destroy(this.bodyHitSounds[k]);
			this.bodyHitSounds[k] = null;
		}
		this.bodyHitSounds = new AudioClip[0];
		for (int l = 0; l < this.headHitSounds.Length; l++)
		{
			UnityEngine.Object.Destroy(this.headHitSounds[l]);
			this.headHitSounds[l] = null;
		}
		this.headHitSounds = new AudioClip[0];
		for (int m = 0; m < this.leftArmHitSounds.Length; m++)
		{
			UnityEngine.Object.Destroy(this.leftArmHitSounds[m]);
			this.leftArmHitSounds[m] = null;
		}
		this.leftArmHitSounds = new AudioClip[0];
		for (int n = 0; n < this.rightArmHitSounds.Length; n++)
		{
			UnityEngine.Object.Destroy(this.rightArmHitSounds[n]);
			this.rightArmHitSounds[n] = null;
		}
		this.rightArmHitSounds = new AudioClip[0];
		for (int num = 0; num < this.leftLegHitSounds.Length; num++)
		{
			UnityEngine.Object.Destroy(this.leftLegHitSounds[num]);
			this.leftLegHitSounds[num] = null;
		}
		this.leftLegHitSounds = new AudioClip[0];
		for (int num2 = 0; num2 < this.rightLegHitSounds.Length; num2++)
		{
			UnityEngine.Object.Destroy(this.rightLegHitSounds[num2]);
			this.rightLegHitSounds[num2] = null;
		}
		this.rightLegHitSounds = new AudioClip[0];
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0005835C File Offset: 0x0005655C
	private void OnGUI()
	{
		if (mainmenu.pause || mainmenu.disableHUD)
		{
			return;
		}
		if (base.gameObject.tag != "Player")
		{
			return;
		}
		if (this.NorthHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.northHitTexture);
		}
		if (this.NorthEastHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.northEastTexture);
		}
		if (this.EastHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.eastTexture);
		}
		if (this.SouthEastHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.southEastTexture);
		}
		if (this.SouthHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.southTexture);
		}
		if (this.NorthWestHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.northWestTexture);
		}
		if (this.WestHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.westTexture);
		}
		if (this.SouthWestHit > 0f)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.southWestTexture);
		}
	}

	// Token: 0x04000DC2 RID: 3522
	public float health = 100f;

	// Token: 0x04000DC3 RID: 3523
	public float healTime = 40f;

	// Token: 0x04000DC4 RID: 3524
	public Transform RagdollPrefab;

	// Token: 0x04000DC5 RID: 3525
	public bool unlimitedHealth;

	// Token: 0x04000DC6 RID: 3526
	public Texture2D northHitTexture;

	// Token: 0x04000DC7 RID: 3527
	public Texture2D northWestTexture;

	// Token: 0x04000DC8 RID: 3528
	public Texture2D westTexture;

	// Token: 0x04000DC9 RID: 3529
	public Texture2D southWestTexture;

	// Token: 0x04000DCA RID: 3530
	public Texture2D southTexture;

	// Token: 0x04000DCB RID: 3531
	public Texture2D northEastTexture;

	// Token: 0x04000DCC RID: 3532
	public Texture2D eastTexture;

	// Token: 0x04000DCD RID: 3533
	public Texture2D southEastTexture;

	// Token: 0x04000DCE RID: 3534
	public AudioClip heartBeatSound;

	// Token: 0x04000DCF RID: 3535
	public TakedownAnim[] frontTakedowns;

	// Token: 0x04000DD0 RID: 3536
	public TakedownAnim[] backTakedowns;

	// Token: 0x04000DD1 RID: 3537
	public AudioClip[] bodyHitSounds;

	// Token: 0x04000DD2 RID: 3538
	public AudioClip[] headHitSounds;

	// Token: 0x04000DD3 RID: 3539
	public AudioClip[] leftArmHitSounds;

	// Token: 0x04000DD4 RID: 3540
	public AudioClip[] rightArmHitSounds;

	// Token: 0x04000DD5 RID: 3541
	public AudioClip[] leftLegHitSounds;

	// Token: 0x04000DD6 RID: 3542
	public AudioClip[] rightLegHitSounds;

	// Token: 0x04000DD7 RID: 3543
	private Transform dead;

	// Token: 0x04000DD8 RID: 3544
	private ShooterGameCamera cam;

	// Token: 0x04000DD9 RID: 3545
	private int currentHitAnimation;

	// Token: 0x04000DDA RID: 3546
	private Transform player;

	// Token: 0x04000DDB RID: 3547
	private static int currentFrontTakedownAnim;

	// Token: 0x04000DDC RID: 3548
	private static int currentBackTakedownAnim;

	// Token: 0x04000DDD RID: 3549
	private float diyingTimer;

	// Token: 0x04000DDE RID: 3550
	private bool activateDiyingTimer;

	// Token: 0x04000DDF RID: 3551
	private BotAI botAI;

	// Token: 0x04000DE0 RID: 3552
	private float hitDisplayTime = 0.5f;

	// Token: 0x04000DE1 RID: 3553
	private float NorthHit;

	// Token: 0x04000DE2 RID: 3554
	private float NorthEastHit;

	// Token: 0x04000DE3 RID: 3555
	private float EastHit;

	// Token: 0x04000DE4 RID: 3556
	private float SouthEastHit;

	// Token: 0x04000DE5 RID: 3557
	private float SouthHit;

	// Token: 0x04000DE6 RID: 3558
	private float NorthWestHit;

	// Token: 0x04000DE7 RID: 3559
	private float WestHit;

	// Token: 0x04000DE8 RID: 3560
	private float SouthWestHit;

	// Token: 0x04000DE9 RID: 3561
	private Transform cameraTransform;

	// Token: 0x04000DEA RID: 3562
	private bool attack;

	// Token: 0x04000DEB RID: 3563
	private bool punched;

	// Token: 0x04000DEC RID: 3564
	private bool punchNeedsDisabling;

	// Token: 0x04000DED RID: 3565
	public AudioClip[] FarisSoundsforHeadHit;

	// Token: 0x04000DEE RID: 3566
	public AudioClip[] FarisSoundsforHeadHitArabic;

	// Token: 0x04000DEF RID: 3567
	private static int currentHeadHitFarisSound;

	// Token: 0x04000DF0 RID: 3568
	private Health playerHealth;

	// Token: 0x04000DF1 RID: 3569
	private AnimationHandler animHandler;

	// Token: 0x04000DF2 RID: 3570
	private float explosionPower;

	// Token: 0x04000DF3 RID: 3571
	private Vector3 explosionPosition;

	// Token: 0x04000DF4 RID: 3572
	private float explosionRadius;

	// Token: 0x04000DF5 RID: 3573
	private float hitTimer;

	// Token: 0x04000DF6 RID: 3574
	public bool addPlayerRagdollScript;

	// Token: 0x04000DF7 RID: 3575
	public float scaleFactor = 1f;

	// Token: 0x04000DF8 RID: 3576
	public AudioClip deathSound;

	// Token: 0x04000DF9 RID: 3577
	private float maxHealth;

	// Token: 0x04000DFA RID: 3578
	public string SpawnGroupAfterDeath = string.Empty;

	// Token: 0x04000DFB RID: 3579
	public float SpawnDelayAfterDeath;

	// Token: 0x04000DFC RID: 3580
	private static int HeadHitSound;

	// Token: 0x04000DFD RID: 3581
	public bool CoffeeMachine;

	// Token: 0x04000DFE RID: 3582
	public bool ElevatorMan;
}
