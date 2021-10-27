using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class Pickupable : MonoBehaviour
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x0006DE34 File Offset: 0x0006C034
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		if (this.player != null)
		{
			this.gm = this.player.GetComponentInChildren<GunManager>();
			this.basicAgility = this.player.GetComponentInChildren<BasicAgility>();
			this.playerInteraction = this.player.GetComponentInChildren<Interaction>();
			this.inventory = this.player.GetComponent<Inventory>();
		}
		if (this.pickupableType == Pickupable.PickupableTypes.ITEM && ((this.pickupableName == "Dagger2a" && Inventory.dagger1) || (this.pickupableName == "Dagger2b" && Inventory.dagger2)))
		{
			if (this.containingObject != null)
			{
				UnityEngine.Object.Destroy(this.containingObject);
			}
			UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		}
		if (this.pickupableType == Pickupable.PickupableTypes.FOUNTAINPIECE && ((this.pickupableName == "1" && Inventory.fuse1) || (this.pickupableName == "2" && Inventory.fuse2) || (this.pickupableName == "3" && Inventory.fuse3) || (this.pickupableName == "4" && Inventory.fuse4)))
		{
			if (this.containingObject != null)
			{
				UnityEngine.Object.Destroy(this.containingObject);
			}
			UnityEngine.Object.Destroy(base.transform.root.gameObject);
		}
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0006DFDC File Offset: 0x0006C1DC
	private void Update()
	{
		if (this.player == null)
		{
			this.player = GameObject.FindGameObjectWithTag("Player");
			if (this.player != null)
			{
				this.gm = this.player.GetComponentInChildren<GunManager>();
				this.basicAgility = this.player.GetComponentInChildren<BasicAgility>();
				this.playerInteraction = this.player.GetComponentInChildren<Interaction>();
				this.inventory = this.player.GetComponent<Inventory>();
			}
		}
		if (this.pickedUp)
		{
			if (this.pickupableName == "Dagger2a" || this.pickupableName == "Dagger2b")
			{
				this.dagger2Timer -= Time.deltaTime;
				if (this.dagger2Timer < 0.1896f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length)
				{
					if (!this.d23)
					{
						UnityEngine.Object.Destroy(this.pickupedObject.gameObject);
						this.d23 = true;
					}
				}
				else if (this.dagger2Timer < 0.534f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length)
				{
					if (!this.d22)
					{
						Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
						this.pickupedObject.position = transform.transform.position;
						this.pickupedObject.rotation = transform.transform.rotation;
						this.pickupedObject.Translate(-0.1533306f, 0.03636586f, -0.1839846f);
						this.pickupedObject.Rotate(315.1796f, 328.49f, 333.5505f, Space.Self);
						this.pickupedObject.parent = transform;
						this.d22 = true;
					}
				}
				else if (this.dagger2Timer < 0.776f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length)
				{
					if (!this.d21)
					{
						Transform transform2 = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
						this.pickupedObject.position = transform2.transform.position;
						this.pickupedObject.rotation = transform2.transform.rotation;
						this.pickupedObject.Translate(-0.08267575f, 0.02514862f, 0.01886448f);
						this.pickupedObject.Rotate(332.0813f, 221.2551f, 179.9666f, Space.Self);
						this.pickupedObject.parent = transform2;
						this.d21 = true;
					}
				}
				else if (this.startingPoint != null)
				{
					if (Mathf.Abs(this.player.transform.rotation.eulerAngles.y - this.startingPoint.rotation.eulerAngles.y) < 180f)
					{
						this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y, this.startingPoint.rotation.eulerAngles.y, -1f * (this.dagger2Timer - 0.776f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length - 0.224f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length) / (0.224f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length)), this.player.transform.rotation.eulerAngles.z);
					}
					else
					{
						this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y - 360f, this.startingPoint.rotation.eulerAngles.y, -1f * (this.dagger2Timer - 0.776f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length - 0.224f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length) / (0.224f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length)), this.player.transform.rotation.eulerAngles.z);
					}
					this.player.transform.position = Vector3.Lerp(this.player.transform.position, new Vector3(this.startingPoint.position.x, this.startingPoint.position.y, this.startingPoint.position.z), -1f * (this.dagger2Timer - 0.776f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length - 0.224f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length) / (0.224f * this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length));
				}
			}
			return;
		}
		this.interactionButton = (MobileInput.interaction || InputManager.GetButtonDown("Interaction"));
		if (this.inside && (this.interactionButton || (this.pickupableType == Pickupable.PickupableTypes.WEAPON && this.gm != null && !this.gm.pickingup && ((this.gm.currentPrimaryGun != null && this.gm.currentPrimaryGun.gunName == this.pickupableName) || (this.gm.currentSecondaryGun != null && this.gm.currentSecondaryGun.gunName == this.pickupableName)))) && this.playerInteraction != null && Time.time > this.playerInteraction.lastUserInputTime + 1.5f && AnimationHandler.instance != null && AnimationHandler.instance.animState != AnimationHandler.AnimStates.JUMPING && AnimationHandler.instance.animState != AnimationHandler.AnimStates.FALLING && (this.playerInteraction == null || (!this.playerInteraction.coverShortMode && !this.playerInteraction.coverShortMode)))
		{
			Instructions.instruction = Instructions.Instruction.NONE;
			if (Time.time <= Pickupable.lastPickTime + 2f)
			{
				return;
			}
			if (this.pickupableType == Pickupable.PickupableTypes.WEAPON)
			{
				if (this.pickupableName == "Grenade")
				{
					this.gm.PickupGrenade(base.transform.parent.gameObject);
					AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = false;
					AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = false;
					this.pickedUp = true;
				}
				else
				{
					this.gm.ChangeToGun(this.pickupableName, base.transform.parent.gameObject, this.currentBullets, this.currentRounds);
					this.pickedUp = true;
				}
				if (!AnimationHandler.instance.insuredMode)
				{
					if (Pickupable.WeaponSound == 0)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.weaponPickupSoundsArabic[Pickupable.currentWeaponPickupSound] != null)
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.weaponPickupSoundsArabic[Pickupable.currentWeaponPickupSound], SpeechManager.speechVolume);
							Pickupable.currentWeaponPickupSound = (Pickupable.currentWeaponPickupSound + 1) % this.basicAgility.weaponPickupSoundsArabic.Length;
						}
						else
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.weaponPickupSounds[Pickupable.currentWeaponPickupSound], SpeechManager.speechVolume);
							Pickupable.currentWeaponPickupSound = (Pickupable.currentWeaponPickupSound + 1) % this.basicAgility.weaponPickupSounds.Length;
						}
						Pickupable.WeaponSound = 4;
					}
					else
					{
						Pickupable.WeaponSound--;
					}
				}
				if (this.pickedUp && !this.spawned)
				{
					if (this.spawnGroupID != string.Empty)
					{
						if (this.enterStealthMode)
						{
							AnimationHandler.instance.stealthMode = true;
						}
						this.spawned = true;
					}
					return;
				}
			}
			if (this.pickupableType == Pickupable.PickupableTypes.ITEM)
			{
				if (this.pickupableName == "Dagger")
				{
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 1;
					this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Ground");
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, 1f);
					this.pickedUp = true;
					if (Pickupable.pickupSound == 0)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
						}
						else
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
						}
						Pickupable.pickupSound = 4;
					}
					else
					{
						Pickupable.pickupSound--;
					}
					this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length;
					this.basicAgility.animating = true;
				}
				if (this.pickupableName == "Dagger1")
				{
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 1;
					this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Ground");
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, 1f);
					this.pickedUp = true;
					if (Pickupable.pickupSound == 0)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
						}
						else
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
						}
						Pickupable.pickupSound = 4;
					}
					else
					{
						Pickupable.pickupSound--;
					}
					this.inventory.daggers++;
					this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length;
					this.basicAgility.animating = true;
				}
				if (this.pickupableName == "Dagger2a")
				{
					Vector3 position = this.pickupedObject.transform.position;
					position.y = this.player.transform.position.y;
					this.player.transform.LookAt(position);
					this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].layer = 1;
					this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Dagger");
					Inventory.dagger1 = true;
					this.dagger2Timer = this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length;
					base.Invoke("PlayPickupSound", this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length / 4f);
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length);
					this.pickedUp = true;
					if (Pickupable.pickupSound == 0)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
						}
						else
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
						}
						Pickupable.pickupSound = 4;
					}
					else
					{
						Pickupable.pickupSound--;
					}
					this.inventory.daggers++;
					this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Dagger"].length;
					this.basicAgility.animating = true;
				}
				if (this.pickupableName == "Dagger2b")
				{
					Vector3 position2 = this.pickupedObject.transform.position;
					position2.y = this.player.transform.position.y;
					this.player.transform.LookAt(position2);
					this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].layer = 1;
					this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Dagger");
					Inventory.dagger2 = true;
					this.dagger2Timer = this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length;
					base.Invoke("PlayPickupSound", this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length / 4f);
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, this.player.GetComponent<Animation>()["Interaction-Pick-Dagger"].length);
					this.pickedUp = true;
					if (Pickupable.pickupSound == 0)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
						}
						else
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
						}
						Pickupable.pickupSound = 4;
					}
					else
					{
						Pickupable.pickupSound--;
					}
					this.inventory.daggers++;
					this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Dagger"].length;
					this.basicAgility.animating = true;
				}
				if (this.pickupableName == "HealthPack")
				{
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 1;
					this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Ground");
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, 1f);
					this.pickedUp = true;
					if (Pickupable.pickupSound == 0)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
						}
						else
						{
							this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
							Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
						}
						Pickupable.pickupSound = 4;
					}
					else
					{
						Pickupable.pickupSound--;
					}
					this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length;
					this.basicAgility.animating = true;
					this.player.GetComponent<Health>().IncreaseHealth(25f);
				}
				if (this.pickedUp && !this.spawned)
				{
					if (this.spawnGroupID != string.Empty)
					{
						if (this.spawner != null)
						{
							this.spawner.Spawn(this.spawnGroupID);
						}
						else
						{
							Debug.Log("you should assign the spawner object to the pickupable");
						}
						if (this.enterStealthMode)
						{
							AnimationHandler.instance.stealthMode = true;
						}
						this.spawned = true;
					}
					return;
				}
			}
			if (this.pickupableType == Pickupable.PickupableTypes.TREASURE)
			{
				this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
				this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 1;
				this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Ground");
				UnityEngine.Object.Destroy(base.transform.parent.gameObject, 1f);
				this.pickedUp = true;
				if (Pickupable.pickupSound == 0)
				{
					if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
					{
						this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
						Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
					}
					else
					{
						this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
						Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
					}
					Pickupable.pickupSound = 4;
				}
				else
				{
					Pickupable.pickupSound--;
				}
				if (this.treasureSound != null)
				{
					this.basicAgility.FarisHead.PlayOneShot(this.treasureSound, SpeechManager.sfxVolume);
				}
				this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length;
				this.basicAgility.animating = true;
				if (this.treasureID != -1)
				{
					SaveHandler.ReportTreasure(this.treasureID);
					if (this.treasureObject != null)
					{
						RotateTreasure.awardTreasure = true;
						this.treasureObject.SetActive(true);
					}
				}
			}
			if (this.pickupableType == Pickupable.PickupableTypes.FOUNTAINPIECE && this.kickableObject == null)
			{
				this.inventory.fountainPieces++;
				Inventory.SetFuse(int.Parse(this.pickupableName));
				if (!this.basicAgility.ledgeHanging)
				{
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].layer = 100;
					this.player.GetComponent<Animation>().CrossFade("Interaction-Pick-Ground", 0.1f, PlayMode.StopAll);
					base.Invoke("HandPick", this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].length * 0.5f);
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].length);
					UnityEngine.Object.Destroy(this.pickupedObject.gameObject, this.player.GetComponent<Animation>()["Interaction-Pick-Ground"].length);
					this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Pick-Ground"].length;
					this.basicAgility.animating = true;
				}
				else
				{
					this.player.GetComponent<Animation>()["Agility-Ledge1-Pick"].wrapMode = WrapMode.Once;
					this.player.GetComponent<Animation>()["Agility-Ledge1-Pick"].layer = 100;
					this.player.GetComponent<Animation>().CrossFade("Agility-Ledge1-Pick");
					base.Invoke("HandPick", this.player.GetComponent<Animation>()["Agility-Ledge1-Pick"].length * 0.37f);
					UnityEngine.Object.Destroy(base.transform.parent.gameObject, this.player.GetComponent<Animation>()["Agility-Ledge1-Pick"].length * 0.83f);
					UnityEngine.Object.Destroy(this.pickupedObject.gameObject, this.player.GetComponent<Animation>()["Agility-Ledge1-Pick"].length * 0.83f);
				}
				base.Invoke("PlayPickupSound", 0.5f);
				Vector3 forward = this.pickupedObject.position - this.player.transform.position;
				forward.y = 0f;
				this.player.transform.rotation = Quaternion.LookRotation(forward);
				this.pickedUp = true;
				if (Pickupable.pickupSound == 0)
				{
					if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound] != null)
					{
						this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSoundsArabic[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
						Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSoundsArabic.Length;
					}
					else
					{
						this.basicAgility.FarisHead.PlayOneShot(this.basicAgility.generalPickupSounds[Pickupable.currentGeneralPickupSound], SpeechManager.speechVolume);
						Pickupable.currentGeneralPickupSound = (Pickupable.currentGeneralPickupSound + 1) % this.basicAgility.generalPickupSounds.Length;
					}
					Pickupable.pickupSound = 4;
				}
				else
				{
					Pickupable.pickupSound--;
				}
				if(m_isNeedFaris){
					//IEnumerator e-erie = new IEnumerator(Eerie);
				StartCoroutine(Eerie());
				}
			}
			MobileInput.instance.disableButton("interaction", base.gameObject);
			Pickupable.lastPickTime = Time.time;
		}
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0006F830 File Offset: 0x0006DA30
	public void PlayPickupSound()
	{
		if (this.GenericSound != null && this.PickSound != null)
		{
			this.GenericSound.clip = this.PickSound;
			this.GenericSound.Play();
		}
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0006F87C File Offset: 0x0006DA7C
	public void HandPick()
	{
		Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
		this.pickupedObject.position = transform.transform.position;
		this.pickupedObject.rotation = transform.transform.rotation;
		this.pickupedObject.Translate(-0.1947645f, 0.09594876f, -0.01025693f);
		this.pickupedObject.Rotate(51.65575f, -82.23001f, -80.13065f, Space.Self);
		this.pickupedObject.parent = transform;
		if (this.basicAgility.ledgeHanging)
		{
			this.basicAgility.DropFromLedge();
		}
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0006F928 File Offset: 0x0006DB28
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (this.pickupableName == "Grenade" && this.gm != null && this.gm.currentGrenades >= 4)
		{
			return;
		}
		if (collisionInfo.tag == "Player" && (this.pickupableType == Pickupable.PickupableTypes.TREASURE || collisionInfo.GetComponent<WeaponHandling>() != null) && (this.playerInteraction == null || (!this.playerInteraction.coverShortMode && !this.playerInteraction.coverTallMode)))
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
			Instructions.instruction = Instructions.Instruction.INTERACT;
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0006F9F4 File Offset: 0x0006DBF4
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (this.pickupableName == "Grenade" && this.gm != null && this.gm.currentGrenades >= 4)
		{
			return;
		}
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
			Instructions.instruction = Instructions.Instruction.NONE;
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0006FA70 File Offset: 0x0006DC70
	public void OnCheckpointLoad(int checkpointReached)
	{
		if (this.pickupableType == Pickupable.PickupableTypes.TREASURE && this.treasureID != -1 && (SaveHandler.treasures & 1 << this.treasureID) != 0)
		{
			UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		}
	}

	// Token: 0x04001011 RID: 4113
	public Pickupable.PickupableTypes pickupableType;

	// Token: 0x04001012 RID: 4114
	public string pickupableName;

	// Token: 0x04001013 RID: 4115
	private Inventory inventory;

	// Token: 0x04001014 RID: 4116
	private GameObject player;

	// Token: 0x04001015 RID: 4117
	private GunManager gm;

	// Token: 0x04001016 RID: 4118
	private BasicAgility basicAgility;

	// Token: 0x04001017 RID: 4119
	public bool inside;

	// Token: 0x04001018 RID: 4120
	public AudioClip PickSound;

	// Token: 0x04001019 RID: 4121
	public AudioSource GenericSound;

	// Token: 0x0400101A RID: 4122
	public string spawnGroupID;

	// Token: 0x0400101B RID: 4123
	public bool enterStealthMode;

	// Token: 0x0400101C RID: 4124
	public Transform pickupedObject;

	// Token: 0x0400101D RID: 4125
	private static float lastPickTime;

	// Token: 0x0400101E RID: 4126
	private bool pickedUp;

	// Token: 0x0400101F RID: 4127
	private bool interactionButton;

	// Token: 0x04001020 RID: 4128
	private static int currentWeaponPickupSound;

	// Token: 0x04001021 RID: 4129
	private static int currentGeneralPickupSound;

	// Token: 0x04001022 RID: 4130
	private bool spawned;

	// Token: 0x04001023 RID: 4131
	private float dagger2Timer;

	// Token: 0x04001024 RID: 4132
	private bool d21;

	// Token: 0x04001025 RID: 4133
	private bool d22;

	// Token: 0x04001026 RID: 4134
	private bool d23;

	// Token: 0x04001027 RID: 4135
	public Transform startingPoint;

	// Token: 0x04001028 RID: 4136
	private Interaction playerInteraction;

	// Token: 0x04001029 RID: 4137
	public GameObject containingObject;

	// Token: 0x0400102A RID: 4138
	public int treasureID = -1;

	// Token: 0x0400102B RID: 4139
	public GameObject treasureObject;

	// Token: 0x0400102C RID: 4140
	public AudioClip treasureSound;

	// Token: 0x0400102D RID: 4141
	public InteractionTrigger kickableObject;

	// Token: 0x0400102E RID: 4142
	public Spawner spawner;

	// Token: 0x0400102F RID: 4143
	private static int WeaponSound;

	// Token: 0x04001030 RID: 4144
	private static int pickupSound;

	// Token: 0x04001031 RID: 4145
	public int currentBullets = -1;

	// Token: 0x04001032 RID: 4146
	public int currentRounds = -1;
	
	public bool m_isNeedFaris = false;
	public Transform farisItself;
	public Transform farisSpuwner;

	// Token: 0x02000205 RID: 517
	public enum PickupableTypes
	{
		// Token: 0x04001034 RID: 4148
		WEAPON,
		// Token: 0x04001035 RID: 4149
		ITEM,
		// Token: 0x04001036 RID: 4150
		FOUNTAINPIECE,
		// Token: 0x04001037 RID: 4151
		TREASURE
	}
	
	IEnumerator Eerie(){
		yield return new WaitForSeconds(0.2f);
		farisItself.position = farisSpuwner.position;
		Eerie();
		Debug.Log("EEBOI");
		}
}
