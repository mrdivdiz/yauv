using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x0200012F RID: 303
internal class FightingEnemy : MonoBehaviour
{
	// Token: 0x0600069B RID: 1691 RVA: 0x00034BD4 File Offset: 0x00032DD4
	[ContextMenu("Reset options")]
	private void ResetTime()
	{
		this.go = base.gameObject;
		AnimControl.ResetOptions(this.go, new AnimOptions[]
		{
			this.leftKick,
			this.rightKick,
			this.rightPunch,
			this.leftPunch,
			this.takedDown1,
			this.takedDown2,
			this.takedDown3,
			this.takedDown4,
			this.takedDown5,
			this.kickedRight,
			this.kickedLeft,
			this.punchedRight,
			this.punchedLeft,
			this.blok,
			this.idle,
			this.walkForward,
			this.walkBeckward
		});
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00034CA0 File Offset: 0x00032EA0
	public void Start()
	{
		this.forwardSpeed = 1f;
		this.wallOffset = 0.1f;
		switch (DifficultyManager.difficulty)
		{
		case DifficultyManager.Difficulty.EASY:
			this.maxHealth = (this.health = (int)((float)this.health * DifficultyManager.easyMelee));
			break;
		case DifficultyManager.Difficulty.HARD:
			this.maxHealth = (this.health = (int)((float)this.health * DifficultyManager.hardMelee));
			break;
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00034D24 File Offset: 0x00032F24
	private void CanStand(float t)
	{
		if (this.health <= 0)
		{
			AnimOptions animOptions = AnimControl.PlayingAttack(this.go, new AnimOptions[]
			{
				this.takedDown1,
				this.takedDown2,
				this.takedDown3,
				this.takedDown4,
				this.takedDown5
			});
			base.GetComponent<Animation>()[animOptions.Anim()].speed = 0f;
			base.GetComponent<Animation>()[animOptions.Anim()].time = t;
		}
		else
		{
			this.enemyController.realmMinDistance = this.onFloorRadius * 2f;
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00034DCC File Offset: 0x00032FCC
	private void StandingEnd()
	{
		this.enemyController.realmMinDistance = this.enemyController.minDistance;
		this.pointer.position = this.trPos;
		this.pointer.parent = this.tr;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00034E14 File Offset: 0x00033014
	public void Defense(string a)
	{
		switch (a)
		{
		case "rightKick":
			this.dodge = true;
			break;
		case "leftKick":
			this.dodge1 = true;
			break;
		case "punch":
			if (this.distance < 1f && UnityEngine.Random.Range(0, 2) == 0)
			{
				this.blokeD = true;
				this.blockTime = Time.time + 0.7f;
				this.dodge = false;
				this.dodge1 = false;
			}
			break;
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00034EEC File Offset: 0x000330EC
	public void TakeHit(string hit)
	{
		if (this.health > 0)
		{
			switch (hit)
			{
			case "rightPunch":
				if (!this.spacialPainAnimPlayed && !this.blokAnimPlayed)
				{
					this.HitedArm();
					AnimControl.StartAnimBlending(this.go, this.punchedRight, ref this.gettingHitSounds);
				}
				else if (!this.spacialPainAnimPlayed)
				{
					AnimControl.PlaySound(this.tr, this.blokSlap, this.slapVol, this.slapDealay);
					this.enemyController.dmg = false;
					this.enemyController.PunchFailure();
				}
				break;
			case "leftPunch":
				if (!this.spacialPainAnimPlayed && !this.blokAnimPlayed)
				{
					this.HitedArm();
					AnimControl.StartAnimBlending(this.go, this.punchedLeft, ref this.gettingHitSounds);
				}
				else if (!this.spacialPainAnimPlayed)
				{
					AnimControl.PlaySound(this.tr, this.blokSlap, this.slapVol, this.slapDealay);
					this.enemyController.dmg = false;
					this.enemyController.PunchFailure();
				}
				break;
			case "leftKick":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedLeg();
					AnimControl.StartAnimBlending(this.go, this.kickedLeft, ref this.gettingHitSounds);
				}
				break;
			case "rightKick":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedLeg();
					AnimControl.StartAnimBlending(this.go, this.kickedRight, ref this.gettingHitSounds);
				}
				break;
			case "takeDown1":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedSpecial();
					AnimControl.StartAnimBlending(this.go, this.takedDown1, ref this.gettingHitSounds);
				}
				break;
			case "takeDown2":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedSpecial();
					AnimControl.StartAnimBlending(this.go, this.takedDown2);
				}
				break;
			case "takeDown3":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedSpecial();
					AnimControl.StartAnimBlending(this.go, this.takedDown3);
				}
				break;
			case "takeDown4":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedSpecial();
					AnimControl.StartAnimBlending(this.go, this.takedDown4);
				}
				break;
			case "takeDown5":
				if (!this.spacialPainAnimPlayed)
				{
					this.HitedSpecial();
					AnimControl.StartAnimBlending(this.go, this.takedDown5);
				}
				break;
			}
			this.CheckAnimPlaying();
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x000351F4 File Offset: 0x000333F4
	private void HitedSpecial()
	{
		this.LookAtEnemy(true);
		this.StopAnims();
		base.GetComponent<Animation>().Stop(this.idle.Anim());
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.blokeD = false;
		this.blokAnimPlayed = false;
		this.health -= 40;
		this.enemyController.ingeniousAttacks = 0;
		this.dmg = false;
		this.blokSoundIsPlayed = false;
		this.pointer.parent = this.pelvis;
		this.pointer.position = new Vector3(this.pelvis.position.x, this.trPos.y, this.pelvis.position.z);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x000352C4 File Offset: 0x000334C4
	private void HitedLeg()
	{
		this.LookAtEnemy();
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.blokeD = false;
		this.blokAnimPlayed = false;
		this.health -= 10;
		this.enemyController.ingeniousAttacks++;
		this.retreat++;
		this.nextArmAttack = Time.deltaTime + 1f;
		this.nextLegAttack = Time.deltaTime + 1f;
		this.enemyController.dmg = false;
		this.dmg = false;
		this.blokSoundIsPlayed = false;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00035368 File Offset: 0x00033568
	private void HitedArm()
	{
		this.LookAtEnemy();
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.health -= 10;
		this.enemyController.ingeniousAttacks++;
		this.nextArmAttack = Time.deltaTime + 1f;
		this.nextLegAttack = Time.deltaTime + 1f;
		this.enemyController.dmg = false;
		this.dmg = false;
		this.blokSoundIsPlayed = false;
		this.retreat++;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00035400 File Offset: 0x00033600
	private void StopAnims()
	{
		AnimControl.StopAnims(this.go, new AnimOptions[]
		{
			this.leftKick,
			this.rightKick,
			this.leftPunch,
			this.rightPunch,
			this.punchedRight,
			this.punchedLeft,
			this.kickedLeft,
			this.kickedRight,
			this.takedDown1,
			this.takedDown2,
			this.takedDown3,
			this.takedDown4,
			this.takedDown5,
			this.rightPunchFailure,
			this.leftPunchFailure
		});
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000354AC File Offset: 0x000336AC
	private void CheckAnimPlaying()
	{
		this.attackAnim = AnimControl.PlayingAttack(this.go, new AnimOptions[]
		{
			this.leftKick,
			this.rightKick,
			this.rightPunch,
			this.leftPunch,
			this.rightPunchFailure,
			this.leftPunchFailure
		});
		this.painAnimPlayed = AnimControl.GroupAnimActivity(this.go, new AnimOptions[]
		{
			this.kickedRight,
			this.kickedLeft,
			this.punchedRight,
			this.punchedLeft
		});
		this.spacialPainAnimPlayed = AnimControl.GroupAnimActivity(this.go, new AnimOptions[]
		{
			this.takedDown1,
			this.takedDown2,
			this.takedDown3,
			this.takedDown4,
			this.takedDown5,
			this.knokdown
		});
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00035590 File Offset: 0x00033790
	public void PunchFailure()
	{
		if (this.attackAnim == this.rightPunch)
		{
			AnimControl.StartAnimBlending(this.go, this.rightPunchFailure);
		}
		else
		{
			AnimControl.StartAnimBlending(this.go, this.leftPunchFailure);
		}
		base.GetComponent<Animation>().Stop(this.attackAnim.Anim());
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000355EC File Offset: 0x000337EC
	private void LookAtEnemy()
	{
		if (!this.painAnimPlayed && !this.spacialPainAnimPlayed)
		{
			this.tr.rotation = Quaternion.LookRotation(new Vector3(this.enemyTransform.position.x, 0f, this.enemyTransform.position.z) - new Vector3(this.trPos.x, 0f, this.trPos.z));
			this.tr.Rotate(Vector3.up, -15f, Space.Self);
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x0003568C File Offset: 0x0003388C
	private void LookAtEnemy(bool s)
	{
		if (!this.painAnimPlayed)
		{
			this.tr.rotation = Quaternion.LookRotation(new Vector3(this.enemyTransform.position.x, 0f, this.enemyTransform.position.z) - new Vector3(this.trPos.x, 0f, this.trPos.z));
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0003570C File Offset: 0x0003390C
	private void MoveCharacter()
	{
		if (Application.loadedLevelName != "Chase1" && Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -Vector3.up, out this.hit, 20f, this.mask))
		{
			this.tr.position = new Vector3(this.tr.position.x, this.hit.point.y, this.tr.position.z);
		}
		float num = 0f;
		if ((double)this.distance < (double)this.minDistance * 0.9 && !Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -this.tr.forward, out this.hit, this.wallOffset, this.mask))
		{
			this.tr.position += -this.tr.forward * num * Time.deltaTime;
			if (!base.GetComponent<Animation>().IsPlaying(this.walkBeckward.Anim()) && !this.painAnimPlayed)
			{
				AnimControl.StartAnimBlending(this.go, this.walkBeckward);
			}
		}
		if (this.attackAnim == null && !this.painAnimPlayed && !this.spacialPainAnimPlayed && !this.dodge && !this.dodge1)
		{
			if (this.retreat >= this.kicksBeforRetreat)
			{
				if (Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -this.tr.forward, out this.hit, this.wallOffset, this.mask))
				{
					this.retreat = 0;
				}
				else
				{
					num = -this.forwardSpeed;
				}
			}
			else
			{
				num = this.forwardSpeed;
			}
			if (this.distance > this.retreatDistance)
			{
				this.retreat = 0;
			}
			if (this.distance < this.minDistance)
			{
				num = 0f;
			}
			else if (this.distance > this.maxDistance)
			{
				num = this.forwardSpeed;
			}
		}
		if (this.dodge && !this.spacialPainAnimPlayed && this.attackAnim == null && !this.dodge1)
		{
			num = -this.forwardSpeed;
			if (this.distance > this.enemyController.rightKick.range + 0.5f)
			{
				this.dodge = false;
			}
		}
		if (this.dodge1 && !this.spacialPainAnimPlayed && this.attackAnim == null && !this.dodge)
		{
			num = -this.forwardSpeed;
			if (this.distance > this.enemyController.leftKick.range + 0.5f)
			{
				this.dodge1 = false;
			}
		}
		if (num > 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying(this.walkForward.Anim()))
			{
				AnimControl.StartAnimBlending(this.go, this.walkForward);
			}
		}
		else if (num < 0f)
		{
			if (!base.GetComponent<Animation>().IsPlaying(this.walkBeckward.Anim()))
			{
				AnimControl.StartAnimBlending(this.go, this.walkBeckward);
			}
		}
		else if (this.attackAnim == null && !this.painAnimPlayed && !this.spacialPainAnimPlayed && !this.dodge && !this.dodge1 && !base.GetComponent<Animation>().IsPlaying(this.idle.Anim()))
		{
			AnimControl.StartAnimBlending(this.go, this.idle);
		}
		if (num != 0f)
		{
			this.tr.position += this.tr.forward * num * Time.deltaTime;
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00035B4C File Offset: 0x00033D4C
	private void BlowsController()
	{
		if (this.distance <= this.activeZone)
		{
			if (this.blokeD && Time.time < this.blockTime)
			{
				if (!base.GetComponent<Animation>().IsPlaying(this.blok.Anim()))
				{
					base.GetComponent<Animation>().CrossFade(this.blok.Anim());
					base.GetComponent<Animation>()[this.blok.Anim()].time = this.blok.startTime;
					base.GetComponent<Animation>()[this.blok.Anim()].speed = this.blok.speed;
				}
				if (base.GetComponent<Animation>()[this.blok.Anim()].time > this.blok.hitTime)
				{
					this.blokAnimPlayed = true;
				}
				if (base.GetComponent<Animation>()[this.blok.Anim()].time >= this.blok.endTime)
				{
					base.GetComponent<Animation>()[this.blok.Anim()].speed = 0f;
					base.GetComponent<Animation>()[this.blok.Anim()].time = this.blok.endTime;
				}
				else
				{
					base.GetComponent<Animation>()[this.blok.Anim()].speed = this.blok.speed;
					base.GetComponent<Animation>()[this.blok.Anim()].weight = 1f;
					if (!this.blokSoundIsPlayed)
					{
						AnimControl.PlaySound(this.tr, this.blok);
						this.blokSoundIsPlayed = true;
					}
				}
			}
			else if (base.GetComponent<Animation>().IsPlaying(this.blok.Anim()))
			{
				this.blokSoundIsPlayed = false;
				if (base.GetComponent<Animation>()[this.blok.Anim()].time < this.blok.hitTime)
				{
					this.blokAnimPlayed = false;
				}
				base.GetComponent<Animation>()[this.blok.Anim()].speed = -this.blok.speed;
				base.GetComponent<Animation>()[this.blok.Anim()].weight = base.GetComponent<Animation>()[this.blok.Anim()].time / this.blok.endTime;
				if (base.GetComponent<Animation>()[this.blok.Anim()].time <= this.blok.startTime)
				{
					this.blokAnimPlayed = false;
					this.blokeD = false;
					base.GetComponent<Animation>()[this.blok.Anim()].time = this.blok.startTime;
				}
			}
			this.rndAttack = UnityEngine.Random.Range(1, 100) + 1;
			if (this.rndAttack > 50)
			{
				this.rndAttack = ((this.rndAttack % 2 == 0) ? 1 : 2);
			}
			else
			{
				this.rndAttack = ((this.rndAttack % 2 == 0) ? 3 : 4);
			}
			if (this.attackAnim == null && !this.painAnimPlayed && !this.spacialPainAnimPlayed && !this.blokAnimPlayed)
			{
				this.StopAnims();
				switch (this.rndAttack)
				{
				case 1:
					if (Time.time > this.nextLegAttack && this.distToTrunk <= this.leftKick.range)
					{
						this.PrepareAttackLeg();
						AnimControl.StartAnimBlending(this.go, this.leftKick);
					}
					break;
				case 2:
					if (Time.time > this.nextLegAttack && this.distToTrunk <= this.rightKick.range)
					{
						this.PrepareAttackLeg();
						AnimControl.StartAnimBlending(this.go, this.rightKick);
					}
					break;
				case 3:
					if (Time.time > this.nextArmAttack && this.distToHead <= this.leftPunch.range)
					{
						if (!this.enemyController.blokAnimPlayed)
						{
							this.PrepareAttackArm();
							AnimControl.StartAnimBlending(this.go, this.leftPunch);
						}
						else if (Time.time > this.nextLegAttack)
						{
							this.PrepareAttackLeg();
							AnimControl.StartAnimBlending(this.go, this.leftKick);
						}
					}
					break;
				case 4:
					if (Time.time > this.nextArmAttack && this.distToHead <= this.rightPunch.range)
					{
						if (!this.enemyController.blokAnimPlayed)
						{
							this.PrepareAttackArm();
							AnimControl.StartAnimBlending(this.go, this.rightPunch);
						}
						else if (Time.time > this.nextLegAttack)
						{
							this.PrepareAttackLeg();
							AnimControl.StartAnimBlending(this.go, this.rightKick);
						}
					}
					break;
				}
			}
		}
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00036060 File Offset: 0x00034260
	private void PrepareAttackArm()
	{
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.blokAnimPlayed = false;
		this.nextArmAttack = Time.time + this.chargeArmTime;
		this.dodge = false;
		this.dodge1 = false;
		this.dmg = true;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000360B4 File Offset: 0x000342B4
	private void PrepareAttackLeg()
	{
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.blokAnimPlayed = false;
		this.nextLegAttack = Time.time + this.chargeLegTime;
		this.dodge = false;
		this.dodge1 = false;
		this.dmg = true;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00036108 File Offset: 0x00034308
	private void HbarUpd()
	{
		if (this.health <= 0)
		{
			base.Invoke("PlayerWon", 4f);
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0003612C File Offset: 0x0003432C
	public void PlayerWon()
	{
		this.enemyController.TurnOffCams();
		if (this.enemyController.smoothFollowCam != null)
		{
			this.enemyController.smoothFollowCam.hitAlpha = 0f;
			this.enemyController.smoothFollowCam.enabled = false;
		}
		CutsceneManager.ExitMeleeEncounter();
		if (this.winningCutscene != null)
		{
			CutsceneManager.PlayCutscene(this.winningCutscene, this.winningCutsceneObjects);
		}
		if (Application.loadedLevelName == "Ozgur_Melee")
		{
			if (!mainmenu.replayLevel)
			{
				SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
			}
			else
			{
				SaveHandler.ResetReplayLevelValues();
			}
			Application.LoadLevel("LoadingEgyptCutscene4");
		}
		if (Application.loadedLevelName == "Chase1")
		{
			if (!mainmenu.replayLevel)
			{
				SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
			}
			else
			{
				SaveHandler.ResetReplayLevelValues();
			}
			Application.LoadLevel("LoadingMoroccoCutscene4");
		}
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00036258 File Offset: 0x00034458
	public void ProceedToLevel()
	{
		if (!mainmenu.replayLevel)
		{
			SaveHandler.SaveCheckpoint(SaveHandler.levelReached + 1, 0, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
		}
		else
		{
			SaveHandler.ResetReplayLevelValues();
		}
		Application.LoadLevel("LoadingQuadLoad");
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x000362AC File Offset: 0x000344AC
	private void Awake()
	{
		this.tr = base.transform;
		this.go = base.gameObject;
		this.pointer = new GameObject("pointer").transform;
		this.pointer.position = this.tr.position;
		this.pointer.parent = this.tr;
		base.GetComponent<Animation>()[this.blok.Anim()].AddMixingTransform(this.spine);
		base.GetComponent<Animation>()[this.blok.Anim()].layer = 1;
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0003634C File Offset: 0x0003454C
	public void BattleMode(GameObject e)
	{
		this.enemy = e;
		this.enemyTransform = this.enemy.transform;
		this.enemyController = this.enemy.GetComponent<FightingControl>();
		this.cam = this.enemyController.cam;
		this.enemyHead = this.enemyController.head;
		this.enemyTrunk = this.enemyController.Trunk;
		this.mask = ~(1 << this.go.layer | 1 << this.enemy.layer | 1 << this.bar.layer | 2 | 4 | 16);
		this.isActive = true;
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00036420 File Offset: 0x00034620
	public void BattleMode()
	{
		if (!this.isActive)
		{
			return;
		}
		this.isActive = false;
		if (this.bar != null && base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00036470 File Offset: 0x00034670
	private void OnDestroy()
	{
		if (this.enemyController != null)
		{
			this.enemyController.BattleMode();
		}
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00036490 File Offset: 0x00034690
	private void LateUpdate()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.isActive && this.enemyController.isActive)
		{
			this.trPos = this.tr.position;
			this.distance = (new Vector3(this.trPos.x, 0f, this.trPos.z) - new Vector3(this.enemyTransform.position.x, 0f, this.enemyTransform.position.z)).magnitude;
			this.distToHead = (new Vector3(this.trPos.x, this.enemyHead.position.y, this.trPos.z) - this.enemyHead.position).magnitude;
			this.distToTrunk = (new Vector3(this.trPos.x, this.enemyTrunk.position.y, this.trPos.z) - this.enemyTrunk.position).magnitude;
			this.CheckAnimPlaying();
			if (this.health > 0)
			{
				this.LookAtEnemy();
				this.BlowsController();
				this.SendDmgMessage();
				this.DisplasePivot();
			}
			else if (!this.spacialPainAnimPlayed)
			{
				this.StopAnims();
				base.GetComponent<Animation>().Stop(this.idle.Anim());
				base.GetComponent<Animation>().Stop(this.blok.Anim());
				AnimControl.StartAnimBlending(this.go, this.knokdown);
				this.isActive = false;
			}
			else
			{
				this.isActive = false;
			}
			this.HbarUpd();
		}
		else
		{
			this.BattleMode();
		}
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00036674 File Offset: 0x00034874
	private void DisplasePivot()
	{
		if (this.spacialPainAnimPlayed)
		{
			Vector3 vector = new Vector3(this.pelvis.position.x, 0f, this.pelvis.position.z) - new Vector3(this.trPos.x, 0f, this.trPos.z);
			if (Physics.CapsuleCast(this.bootom + vector, this.top + vector, this.charRadius, -Vector3.up, out this.hit, 20f, this.mask))
			{
				this.tr.position = new Vector3(this.tr.position.x, this.hit.point.y, this.tr.position.z);
			}
			if (Physics.CapsuleCast(this.bootom, this.top, this.charRadius, vector, out this.hit, vector.magnitude + this.onFloorRadius, this.mask))
			{
				this.pelvis.localPosition = new Vector3(this.lastPelPos.x, this.pelvis.localPosition.y, this.lastPelPos.z);
			}
			else
			{
				this.lastPelPos = this.pelvis.localPosition;
			}
		}
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00036804 File Offset: 0x00034A04
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.isActive && this.enemyController.isActive)
		{
			this.trPos = this.tr.position;
			this.CalcCapsule();
			this.MoveCharacter();
		}
		if (!this.painAnimPlayed && !this.youCanHitMe)
		{
			if (this.youCanHitMeAfter < 0f)
			{
				this.youCanHitMe = true;
				this.youCanHitMeAfter = 1E-06f;
			}
			else
			{
				this.youCanHitMeAfter -= Time.deltaTime;
			}
		}
		else if (this.painAnimPlayed && this.youCanHitMe)
		{
			this.youCanHitMe = false;
		}
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x000368C8 File Offset: 0x00034AC8
	private void SendDmgMessage()
	{
		if (this.dmg && !this.painAnimPlayed && this.attackAnim != null)
		{
			this.attackN = AnimControl.AttackSwitcer(this.go, this.distToHead, this.distToTrunk, this.attackAnim, this.rightPunch, this.leftPunch, this.leftKick, this.rightKick);
			if (this.attackN != string.Empty)
			{
				this.enemyController.TakeHit(this.attackN);
			}
		}
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00036958 File Offset: 0x00034B58
	private void OnDrawGizmosSelected()
	{
		this.trPos = base.transform.position;
		this.CalcCapsule();
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(this.bootom, this.charRadius);
		Gizmos.DrawWireSphere(this.top, this.charRadius);
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000369A8 File Offset: 0x00034BA8
	private void CalcCapsule()
	{
		this.bootom = this.trPos + this.charCenter + new Vector3(0f, -this.charHeight * 0.5f + this.charRadius, 0f);
		this.top = this.trPos + this.charCenter + new Vector3(0f, this.charHeight * 0.5f - this.charRadius, 0f);
	}

	// Token: 0x0400083C RID: 2108
	private LayerMask mask;

	// Token: 0x0400083D RID: 2109
	public float charHeight = 1.6f;

	// Token: 0x0400083E RID: 2110
	public float charRadius = 0.4f;

	// Token: 0x0400083F RID: 2111
	public float onFloorRadius = 1f;

	// Token: 0x04000840 RID: 2112
	public Vector3 charCenter = new Vector3(0f, 0.9f, 0f);

	// Token: 0x04000841 RID: 2113
	private Vector3 bootom;

	// Token: 0x04000842 RID: 2114
	private Vector3 top;

	// Token: 0x04000843 RID: 2115
	public Transform head;

	// Token: 0x04000844 RID: 2116
	public Transform Trunk;

	// Token: 0x04000845 RID: 2117
	private RaycastHit hit;

	// Token: 0x04000846 RID: 2118
	private Vector3 lastPelPos;

	// Token: 0x04000847 RID: 2119
	[NonSerialized]
	public Transform pointer;

	// Token: 0x04000848 RID: 2120
	public float wallOffset = 2f;

	// Token: 0x04000849 RID: 2121
	[NonSerialized]
	public bool isActive;

	// Token: 0x0400084A RID: 2122
	public GameObject bar;

	// Token: 0x0400084B RID: 2123
	private Transform barTr;

	// Token: 0x0400084C RID: 2124
	public float barYPos = 1.7f;

	// Token: 0x0400084D RID: 2125
	private Transform cam;

	// Token: 0x0400084E RID: 2126
	private Transform tr;

	// Token: 0x0400084F RID: 2127
	private GameObject go;

	// Token: 0x04000850 RID: 2128
	private GameObject enemy;

	// Token: 0x04000851 RID: 2129
	private Transform enemyTransform;

	// Token: 0x04000852 RID: 2130
	private FightingControl enemyController;

	// Token: 0x04000853 RID: 2131
	private Transform enemyHead;

	// Token: 0x04000854 RID: 2132
	private float distToHead;

	// Token: 0x04000855 RID: 2133
	private Transform enemyTrunk;

	// Token: 0x04000856 RID: 2134
	private float distToTrunk;

	// Token: 0x04000857 RID: 2135
	public int health = 100;

	// Token: 0x04000858 RID: 2136
	public int maxHealth = 100;

	// Token: 0x04000859 RID: 2137
	public float minDistance = 0.5f;

	// Token: 0x0400085A RID: 2138
	public float maxDistance = 4f;

	// Token: 0x0400085B RID: 2139
	public float activeZone = 1f;

	// Token: 0x0400085C RID: 2140
	public float retreatDistance = 2f;

	// Token: 0x0400085D RID: 2141
	public float forwardSpeed = 1f;

	// Token: 0x0400085E RID: 2142
	private float distance;

	// Token: 0x0400085F RID: 2143
	private string attackN;

	// Token: 0x04000860 RID: 2144
	public Transform spine;

	// Token: 0x04000861 RID: 2145
	public Transform pelvis;

	// Token: 0x04000862 RID: 2146
	private int retreat;

	// Token: 0x04000863 RID: 2147
	public int kicksBeforRetreat = 2;

	// Token: 0x04000864 RID: 2148
	private float nextLegAttack;

	// Token: 0x04000865 RID: 2149
	private float nextArmAttack;

	// Token: 0x04000866 RID: 2150
	public float chargeLegTime = 5f;

	// Token: 0x04000867 RID: 2151
	public float chargeArmTime = 2f;

	// Token: 0x04000868 RID: 2152
	public AnimOptions idle = new AnimOptions();

	// Token: 0x04000869 RID: 2153
	public AnimOptions blok = new AnimOptions();

	// Token: 0x0400086A RID: 2154
	public AudioClip blokSlap;

	// Token: 0x0400086B RID: 2155
	public float slapVol = 1f;

	// Token: 0x0400086C RID: 2156
	public float slapDealay;

	// Token: 0x0400086D RID: 2157
	private bool blokSoundIsPlayed;

	// Token: 0x0400086E RID: 2158
	public AnimOptions leftPunch = new AnimOptions();

	// Token: 0x0400086F RID: 2159
	public AnimOptions leftPunchFailure = new AnimOptions();

	// Token: 0x04000870 RID: 2160
	public AnimOptions rightPunch = new AnimOptions();

	// Token: 0x04000871 RID: 2161
	public AnimOptions rightPunchFailure = new AnimOptions();

	// Token: 0x04000872 RID: 2162
	public AnimOptions leftKick = new AnimOptions();

	// Token: 0x04000873 RID: 2163
	public AnimOptions rightKick = new AnimOptions();

	// Token: 0x04000874 RID: 2164
	public AnimOptions punchedRight = new AnimOptions();

	// Token: 0x04000875 RID: 2165
	public AnimOptions punchedLeft = new AnimOptions();

	// Token: 0x04000876 RID: 2166
	public AnimOptions kickedLeft = new AnimOptions();

	// Token: 0x04000877 RID: 2167
	public AnimOptions kickedRight = new AnimOptions();

	// Token: 0x04000878 RID: 2168
	public AnimOptions takedDown1 = new AnimOptions();

	// Token: 0x04000879 RID: 2169
	public AnimOptions takedDown2 = new AnimOptions();

	// Token: 0x0400087A RID: 2170
	public AnimOptions takedDown3 = new AnimOptions();

	// Token: 0x0400087B RID: 2171
	public AnimOptions takedDown4 = new AnimOptions();

	// Token: 0x0400087C RID: 2172
	public AnimOptions takedDown5 = new AnimOptions();

	// Token: 0x0400087D RID: 2173
	public AnimOptions knokdown = new AnimOptions();

	// Token: 0x0400087E RID: 2174
	public AnimOptions standing = new AnimOptions();

	// Token: 0x0400087F RID: 2175
	public AnimOptions walkForward = new AnimOptions();

	// Token: 0x04000880 RID: 2176
	public AnimOptions walkBeckward = new AnimOptions();

	// Token: 0x04000881 RID: 2177
	private Vector3 trPos;

	// Token: 0x04000882 RID: 2178
	[NonSerialized]
	public AnimOptions attackAnim;

	// Token: 0x04000883 RID: 2179
	private bool blokAnimPlayed;

	// Token: 0x04000884 RID: 2180
	public bool painAnimPlayed;

	// Token: 0x04000885 RID: 2181
	[NonSerialized]
	public bool spacialPainAnimPlayed;

	// Token: 0x04000886 RID: 2182
	private bool standingPlayed;

	// Token: 0x04000887 RID: 2183
	[NonSerialized]
	public bool dodge;

	// Token: 0x04000888 RID: 2184
	[NonSerialized]
	public bool dodge1;

	// Token: 0x04000889 RID: 2185
	private bool blokeD;

	// Token: 0x0400088A RID: 2186
	private float nexBlok;

	// Token: 0x0400088B RID: 2187
	private float blockTime;

	// Token: 0x0400088C RID: 2188
	private int rndAttack;

	// Token: 0x0400088D RID: 2189
	[NonSerialized]
	public bool dmg;

	// Token: 0x0400088E RID: 2190
	public CSComponent winningCutscene;

	// Token: 0x0400088F RID: 2191
	public GameObject winningCutsceneObjects;

	// Token: 0x04000890 RID: 2192
	public AudioClip[] gettingHitSounds;

	// Token: 0x04000891 RID: 2193
	public bool youCanHitMe = true;

	// Token: 0x04000892 RID: 2194
	private float youCanHitMeAfter = 1E-06f;
}
