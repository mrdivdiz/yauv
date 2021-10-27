using System;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class FightingControl : MonoBehaviour
{
	// Token: 0x0600067C RID: 1660 RVA: 0x00032284 File Offset: 0x00030484
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
			this.takeDown1,
			this.takeDown2,
			this.takeDown3,
			this.takeDown4,
			this.takeDown5,
			this.kickedRight,
			this.kickedLeft,
			this.punchedRight,
			this.punchedLeft,
			this.blok,
			this.idle,
			this.walkLeft,
			this.walkRight,
			this.walkForward,
			this.walkBeckward
		});
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00032364 File Offset: 0x00030564
	private void PrepareAttack()
	{
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.blokAnimPlayed = false;
		this.blokSoundIsPlayed = false;
		this.StopAnims();
		this.attackButtonPressed = true;
		this.dmg = true;
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x000323AC File Offset: 0x000305AC
	private void CheckSpecials()
	{
		this.tdCount = 0;
		this.availableTakeDowns = new int[6];
		if (!this.enemyController.spacialPainAnimPlayed)
		{
			if (this.distToTrunk <= this.takeDown1.range)
			{
				AnimControl.StoreAttacks(ref this.availableTakeDowns, 1);
				this.tdCount++;
			}
			if (this.distToTrunk <= this.takeDown2.range)
			{
				AnimControl.StoreAttacks(ref this.availableTakeDowns, 2);
				this.tdCount++;
			}
			if (this.distToTrunk <= this.takeDown3.range)
			{
				AnimControl.StoreAttacks(ref this.availableTakeDowns, 3);
				this.tdCount++;
			}
			if (this.distToTrunk <= this.takeDown4.range)
			{
				AnimControl.StoreAttacks(ref this.availableTakeDowns, 4);
				this.tdCount++;
			}
			if (this.distToTrunk <= this.takeDown5.range)
			{
				AnimControl.StoreAttacks(ref this.availableTakeDowns, 5);
				this.tdCount++;
			}
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x000324CC File Offset: 0x000306CC
	private void TurnOffCam1()
	{
		if (this.currentTakeDown == "1")
		{
			this.TakeDownCam1.SetActive(false);
			this.TakeDownCam1.GetComponent<Camera>().enabled = false;
		}
		else if (this.currentTakeDown == "2")
		{
			this.TakeDownCam2.SetActive(false);
			this.TakeDownCam2.GetComponent<Camera>().enabled = false;
		}
		else if (this.currentTakeDown == "3")
		{
			this.TakeDownCam3.SetActive(false);
			this.TakeDownCam3.GetComponent<Camera>().enabled = false;
		}
		else if (this.currentTakeDown == "4")
		{
			this.TakeDownCam4.SetActive(false);
			this.TakeDownCam4.GetComponent<Camera>().enabled = false;
		}
		if (this.enemyController.health > 0)
		{
			this.TakeDownCamEnemy.SetActive(true);
			this.TakeDownCamEnemy.GetComponent<Camera>().enabled = true;
			this.TakeDownCamEnemy.GetComponent<AudioSource>().clip = this.EnemyStandupSound[this.currentEnemyStandupSound];
			this.TakeDownCamEnemy.GetComponent<AudioSource>().Play();
			this.currentEnemyStandupSound = (this.currentEnemyStandupSound + 1) % this.EnemyStandupSound.Length;
			this.ShowPlayer(false);
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0003262C File Offset: 0x0003082C
	private void TurnOffCam2()
	{
		this.TakeDownCamEnemy.SetActive(false);
		this.TakeDownCamEnemy.GetComponent<Camera>().enabled = false;
		this.cam.GetComponent<Camera>().enabled = true;
		this.ShowPlayer(true);
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00032670 File Offset: 0x00030870
	public void TurnOffCams()
	{
		this.TakeDownCam1.SetActive(false);
		this.TakeDownCam1.GetComponent<Camera>().enabled = false;
		this.TakeDownCam2.SetActive(false);
		this.TakeDownCam2.GetComponent<Camera>().enabled = false;
		this.TakeDownCam3.SetActive(false);
		this.TakeDownCam3.GetComponent<Camera>().enabled = false;
		this.TakeDownCam4.SetActive(false);
		this.TakeDownCam4.GetComponent<Camera>().enabled = false;
		this.TakeDownCamEnemy.SetActive(false);
		this.TakeDownCamEnemy.GetComponent<Camera>().enabled = false;
		this.cam.GetComponent<Camera>().enabled = true;
		this.smoothFollowCam.unaimedZoom = this.previousUnaimedZoom;
		this.smoothFollowCam.meleeCamera = false;
		this.smoothFollowCam.camOffset = new Vector3(this.smoothFollowCam.camOffset.x, this.previousCamOffsetY, this.smoothFollowCam.camOffset.z);
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00032774 File Offset: 0x00030974
	private void Start()
	{
		this.forwardSpeed = 1f;
		this.sideSpeed = 0.5f;
		this.wallOffset = 0.5f;
		this.TakeDownCam1.SetActive(false);
		this.TakeDownCam1.GetComponent<Camera>().enabled = false;
		this.TakeDownCam2.SetActive(false);
		this.TakeDownCam2.GetComponent<Camera>().enabled = false;
		this.TakeDownCam3.SetActive(false);
		this.TakeDownCam3.GetComponent<Camera>().enabled = false;
		this.TakeDownCam4.SetActive(false);
		this.TakeDownCam4.GetComponent<Camera>().enabled = false;
		this.TakeDownCamEnemy.SetActive(false);
		this.TakeDownCamEnemy.GetComponent<Camera>().enabled = false;
		this.smoothFollowCam = this.cam.GetComponent<ShooterGameCamera>();
		this.smoothFollowCam.player = base.transform;
		if (GameObject.Find("cameraTarget") != null)
		{
			this.smoothFollowCam.player = GameObject.Find("cameraTarget").transform;
		}
		this.previousUnaimedZoom = this.smoothFollowCam.unaimedZoom;
		this.smoothFollowCam.unaimedZoom = -3.5f;
		this.smoothFollowCam.meleeCamera = true;
		this.previousCamOffsetY = this.smoothFollowCam.camOffset.y;
		this.smoothFollowCam.camOffset = new Vector3(this.smoothFollowCam.camOffset.x, -1.1f, this.smoothFollowCam.camOffset.z);
		this.smoothFollowCam.enabled = true;
		this.smoothFollowCam.hitAlpha = 0f;
		this.cam.GetComponent<Camera>().enabled = true;
		base.GetComponent<Animation>()["Face-Cover"].AddMixingTransform(this.head);
		base.GetComponent<Animation>()["Face-Cover"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Face-Cover"].layer = 1000;
		base.GetComponent<Animation>().Blend("Face-Cover");
		switch (DifficultyManager.difficulty)
		{
		case DifficultyManager.Difficulty.EASY:
			this.maxHealth = (this.health = (int)((float)this.health / DifficultyManager.easyMelee));
			break;
		case DifficultyManager.Difficulty.HARD:
			this.maxHealth = (this.health = (int)((float)this.health / DifficultyManager.hardMelee));
			break;
		}
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x000329E8 File Offset: 0x00030BE8
	public void ShowPlayer(bool show)
	{
		foreach (Renderer renderer in base.gameObject.GetComponentsInChildren<Renderer>())
		{
			renderer.enabled = show;
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00032A20 File Offset: 0x00030C20
	private void TakeDownStart()
	{
		if (this.tdCount > 0)
		{
			if (this.currentSelectTakeDown >= this.tdCount || this.currentSelectTakeDown < 0)
			{
				this.currentSelectTakeDown = UnityEngine.Random.Range(0, this.tdCount);
			}
			this.currentSelectTakeDown = (this.currentSelectTakeDown + 1) % this.tdCount;
			this.PrepareAttack();
			switch (this.availableTakeDowns[this.currentSelectTakeDown])
			{
			case 1:
				this.cam.GetComponent<Camera>().enabled = false;
				this.TakeDownCam1.SetActive(true);
				this.TakeDownCam1.GetComponent<Camera>().enabled = true;
				this.currentTakeDown = "1";
				AnimControl.StartAnimBlending(this.go, this.takeDown1);
				this.ingeniousAttacks = 0;
				this.attackAnim = this.takeDown1;
				this.enemyController.TakeHit("takeDown1");
				this.displaceForSpecial(this.takeDown1);
				base.Invoke("TurnOffCam1", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front1"].length / 2f);
				base.Invoke("TurnOffCam2", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front1"].length);
				this.disableControlForTakedown = this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front1"].length;
				break;
			case 2:
				this.cam.GetComponent<Camera>().enabled = false;
				this.TakeDownCam2.SetActive(true);
				this.TakeDownCam2.GetComponent<Camera>().enabled = true;
				this.currentTakeDown = "2";
				AnimControl.StartAnimBlending(this.go, this.takeDown2);
				this.ingeniousAttacks = 0;
				this.attackAnim = this.takeDown2;
				this.enemyController.TakeHit("takeDown2");
				this.displaceForSpecial(this.takeDown2);
				base.Invoke("TurnOffCam1", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front2"].length / 2f);
				base.Invoke("TurnOffCam2", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front2"].length);
				this.disableControlForTakedown = this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front2"].length;
				break;
			case 3:
				this.cam.GetComponent<Camera>().enabled = false;
				this.TakeDownCam3.SetActive(true);
				this.TakeDownCam3.GetComponent<Camera>().enabled = true;
				this.currentTakeDown = "3";
				AnimControl.StartAnimBlending(this.go, this.takeDown3);
				this.ingeniousAttacks = 0;
				this.attackAnim = this.takeDown3;
				this.enemyController.TakeHit("takeDown3");
				this.displaceForSpecial(this.takeDown3);
				base.Invoke("TurnOffCam1", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front3"].length / 2f);
				base.Invoke("TurnOffCam2", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front3"].length);
				this.disableControlForTakedown = this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front3"].length;
				break;
			case 4:
				this.cam.GetComponent<Camera>().enabled = false;
				this.TakeDownCam4.SetActive(true);
				this.TakeDownCam4.GetComponent<Camera>().enabled = true;
				this.currentTakeDown = "4";
				AnimControl.StartAnimBlending(this.go, this.takeDown4);
				this.ingeniousAttacks = 0;
				this.attackAnim = this.takeDown4;
				this.enemyController.TakeHit("takeDown4");
				this.displaceForSpecial(this.takeDown4);
				base.Invoke("TurnOffCam1", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front4"].length / 2f);
				base.Invoke("TurnOffCam2", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front4"].length);
				this.disableControlForTakedown = this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown-front4"].length;
				break;
			case 5:
				this.cam.GetComponent<Camera>().enabled = false;
				this.TakeDownCam2.SetActive(true);
				this.TakeDownCam2.GetComponent<Camera>().enabled = true;
				this.currentTakeDown = "2";
				AnimControl.StartAnimBlending(this.go, this.takeDown5);
				this.ingeniousAttacks = 0;
				this.attackAnim = this.takeDown5;
				this.enemyController.TakeHit("takeDown5");
				this.displaceForSpecial(this.takeDown5);
				base.Invoke("TurnOffCam1", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown1-Front6"].length / 2f);
				base.Invoke("TurnOffCam2", this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown1-Front6"].length);
				this.disableControlForTakedown = this.enemyController.transform.GetComponent<Animation>()["Enemy-Takedown1-Front6"].length;
				break;
			}
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00032FA4 File Offset: 0x000311A4
	private void displaceForSpecial(AnimOptions a)
	{
		this.tr.rotation = Quaternion.LookRotation(new Vector3(this.enemyPointer.position.x, 0f, this.enemyPointer.position.z) - new Vector3(this.trPos.x, 0f, this.trPos.z));
		Vector3 vector = new Vector3(this.enemyTransform.position.x, 0f, this.enemyTransform.position.z);
		Vector3 vector2 = new Vector3(this.trPos.x, 0f, this.trPos.z);
		Vector3 vector3 = this.enemyTransform.position - (vector - vector2).normalized * a.range;
		if (!Physics.CapsuleCast(this.bootom, this.top, this.charRadius, vector3 - this.trPos, out this.hit, (this.trPos - vector3).magnitude + this.wallOffset, this.mask))
		{
			this.tr.position = vector3;
		}
		else
		{
			this.enemyTransform.position = this.trPos - (vector2 - vector).normalized * a.range;
		}
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00033130 File Offset: 0x00031330
	private void PlayerLost()
	{
		if (Application.loadedLevelName == "Ozgur_Melee")
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
		else
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00033180 File Offset: 0x00031380
	private void HudUpd()
	{
		if (this.health <= 0)
		{
			base.Invoke("PlayerLost", 2f);
			this.smoothFollowCam.hitAlpha = 1f;
			if (base.GetComponent<AudioSource>() != null && this.deathSound != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.deathSound, SpeechManager.speechVolume);
			}
		}
		if (this.health > 0)
		{
			this.smoothFollowCam.hitAlpha = ((float)this.maxHealth - (float)this.health) / (float)this.maxHealth;
		}
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00033220 File Offset: 0x00031420
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
			this.takeDown1,
			this.takeDown2,
			this.takeDown3,
			this.takeDown4,
			this.takeDown5,
			this.rightPunchFailure,
			this.leftPunchFailure
		});
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x000332CC File Offset: 0x000314CC
	private void CheckAnimPlaying()
	{
		this.takeDownPlayed = AnimControl.GroupAnimActivity(this.go, new AnimOptions[]
		{
			this.takeDown1,
			this.takeDown2,
			this.takeDown3,
			this.takeDown4,
			this.takeDown5
		});
		this.attackAnim = AnimControl.PlayingAttack(this.go, new AnimOptions[]
		{
			this.leftKick,
			this.rightKick,
			this.rightPunch,
			this.leftPunch,
			this.takeDown1,
			this.takeDown2,
			this.takeDown3,
			this.takeDown4,
			this.takeDown5,
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
		if (this.attackAnim == this.rightKick)
		{
			if (base.GetComponent<Animation>()[this.rightKick.Anim()].time > this.rightKick.hitTime + 0.1f)
			{
				this.enemyController.dodge = false;
			}
		}
		else if (this.attackAnim == this.leftKick && base.GetComponent<Animation>()[this.leftKick.Anim()].time > this.leftKick.hitTime + 0.1f)
		{
			this.enemyController.dodge1 = false;
		}
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x00033478 File Offset: 0x00031678
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

	// Token: 0x0600068B RID: 1675 RVA: 0x000334D4 File Offset: 0x000316D4
	public void TakeHit(string hit)
	{
		switch (hit)
		{
		case "rightPunch":
			if (!this.blokAnimPlayed)
			{
				this.HitedArm();
				AnimControl.StartAnimBlending(this.go, this.punchedRight, ref this.gettingHitSounds);
			}
			else
			{
				AnimControl.PlaySound(this.tr, this.blokSlap, this.slapVol, this.slapDealay);
				this.enemyController.dmg = false;
				this.enemyController.PunchFailure();
			}
			break;
		case "leftPunch":
			if (!this.blokAnimPlayed)
			{
				this.HitedArm();
				AnimControl.StartAnimBlending(this.go, this.punchedLeft, ref this.gettingHitSounds);
			}
			else
			{
				AnimControl.PlaySound(this.tr, this.blokSlap, this.slapVol, this.slapDealay);
				this.enemyController.dmg = false;
				this.enemyController.PunchFailure();
			}
			break;
		case "leftKick":
			this.HitedLeg();
			AnimControl.StartAnimBlending(this.go, this.kickedLeft, ref this.gettingHitSounds);
			break;
		case "rightKick":
			this.HitedLeg();
			AnimControl.StartAnimBlending(this.go, this.kickedRight, ref this.gettingHitSounds);
			break;
		}
		this.CheckAnimPlaying();
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x00033674 File Offset: 0x00031874
	private void HitedArm()
	{
		this.LookAtEnemy();
		if (!mainmenu.disableHUD)
		{
			this.health -= 10;
		}
		PadVibrator.VibrateInterval(true, 0.6f, 0.6f);
		this.enemyController.dmg = false;
		this.dmg = false;
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x000336C4 File Offset: 0x000318C4
	private void HitedLeg()
	{
		this.LookAtEnemy();
		base.GetComponent<Animation>().Stop(this.blok.Anim());
		this.blokAnimPlayed = false;
		this.blokSoundIsPlayed = false;
		this.painAnimPlayed = true;
		if (!mainmenu.disableHUD)
		{
			this.health -= 10;
		}
		PadVibrator.VibrateInterval(true, 0.3f, 0.3f);
		this.enemyController.dmg = false;
		this.dmg = false;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00033740 File Offset: 0x00031940
	private void LookAtEnemy()
	{
		if (!this.takeDownPlayed)
		{
			this.tr.rotation = Quaternion.LookRotation(new Vector3(this.enemyPointer.position.x, 0f, this.enemyPointer.position.z) - new Vector3(this.trPos.x, 0f, this.trPos.z));
			this.tr.Rotate(Vector3.up, -15f, Space.Self);
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x000337D4 File Offset: 0x000319D4
	private void MoveCharacter()
	{
		if (Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -Vector3.up, out this.hit, 20f, this.floorMask))
		{
			this.tr.position = new Vector3(this.tr.position.x, this.hit.point.y, this.tr.position.z);
		}
		if ((double)this.distance < (double)this.realmMinDistance * 0.9 && !Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -this.tr.forward, out this.hit, this.wallOffset, this.mask))
		{
			this.tr.position += -this.tr.forward * this.forwardSpeed * Time.deltaTime;
			if (!base.GetComponent<Animation>().IsPlaying(this.walkBeckward.Anim()) && !this.painAnimPlayed)
			{
				AnimControl.StartAnimBlending(this.go, this.walkBeckward);
			}
		}
		if (this.attackAnim == null && !this.painAnimPlayed && !this.attackButtonPressed)
		{
			float num = 0f;
			float num2 = 0f;
			if (this.attackAnim == null)
			{
				this.distance = (new Vector3(this.trPos.x, 0f, this.trPos.z) - new Vector3(this.enemyPointer.position.x, 0f, this.enemyPointer.position.z)).magnitude;
				float num3 = FightingControl.meleeJoystickLeft.position.x + Input.GetAxisRaw(this.vertical);
				float num4 = FightingControl.meleeJoystickLeft.position.y + Input.GetAxisRaw(this.horizontal);
				float num5 = Vector3.Dot(this.cam.transform.forward, base.transform.forward) * num4 + Vector3.Dot(this.cam.transform.right, base.transform.forward) * num3;
				float num6 = Vector3.Dot(this.cam.transform.forward, -base.transform.right) * -num4 + Vector3.Dot(this.cam.transform.right, -base.transform.right) * -num3;
				if (this.disableControlForTakedown > 0f)
				{
					num5 = 0f;
					num6 = 0f;
				}
				if (num5 > 0.1f)
				{
					if (this.distance > this.realmMinDistance)
					{
						num = 1f;
					}
				}
				else if (num5 < -0.1f && !Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -this.tr.forward, out this.hit, this.wallOffset, this.mask))
				{
					num = -1f;
				}
				if (num6 > 0.1f)
				{
					if (!Physics.CapsuleCast(this.bootom, this.top, this.charRadius, this.tr.right, out this.hit, this.wallOffset, this.mask))
					{
						num2 = 1f;
					}
				}
				else if (num6 < -0.1f && !Physics.CapsuleCast(this.bootom, this.top, this.charRadius, -this.tr.right, out this.hit, this.wallOffset, this.mask))
				{
					num2 = -1f;
				}
			}
			this.moveDirection = new Vector3(num2 * this.forwardSpeed, 0f, num * this.forwardSpeed) * Time.deltaTime;
			if (num2 > 0f)
			{
				if (!base.GetComponent<Animation>().IsPlaying(this.walkRight.Anim()))
				{
					base.GetComponent<Animation>()[this.walkLeft.Anim()].weight *= 0.2f * Time.deltaTime;
					AnimControl.StartAnimBlending(this.go, this.walkRight);
				}
			}
			else if (num2 < 0f)
			{
				if (!base.GetComponent<Animation>().IsPlaying(this.walkLeft.Anim()))
				{
					base.GetComponent<Animation>()[this.walkRight.Anim()].weight *= 0.2f * Time.deltaTime;
					AnimControl.StartAnimBlending(this.go, this.walkLeft);
				}
			}
			else if (num > 0f)
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
			else if (!base.GetComponent<Animation>().IsPlaying(this.idle.Anim()))
			{
				AnimControl.StartAnimBlending(this.go, this.idle);
			}
			this.tr.position += this.tr.TransformDirection(this.moveDirection);
		}
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00033DC4 File Offset: 0x00031FC4
	private void BlowsController()
	{
		if (this.disableControlForTakedown > 0f)
		{
			return;
		}
		bool flag = MeleeMobileInput.rightKick;
		bool flag2 = MeleeMobileInput.rightPunch;
		bool flag3 = MeleeMobileInput.block;
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS)
		{
			flag |= Input.GetKeyDown(KeyCode.JoystickButton13);
			flag2 |= Input.GetKeyDown(KeyCode.JoystickButton15);
			flag3 |= Input.GetKey(KeyCode.JoystickButton12);
		}
		else
		{
			flag |= Input.GetKeyDown(KeyCode.JoystickButton1);
			flag2 |= Input.GetKeyDown(KeyCode.JoystickButton2);
			flag3 |= Input.GetKey(KeyCode.JoystickButton3);
		}
		this.attackButtonPressed = false;
		this.distance = (new Vector3(this.trPos.x, 0f, this.trPos.z) - new Vector3(this.enemyPointer.position.x, 0f, this.enemyPointer.position.z)).magnitude;
		if (flag && (this.lastAttackTime == 0f || Time.timeSinceLevelLoad > this.lastAttackTime + 0.75f))
		{
			if ((!this.firstTakedownPerformed && (float)this.enemyController.health <= (float)this.enemyController.maxHealth * 0.5f + 40f) || this.enemyController.health <= 40)
			{
				this.TakeDownStart();
				this.firstTakedownPerformed = true;
			}
			else
			{
				if (this.currentRightKick)
				{
					this.PrepareAttack();
					this.enemyController.Defense("rightKick");
					AnimControl.StartAnimBlending(this.go, this.rightKick);
					this.attackAnim = this.rightKick;
				}
				else
				{
					this.PrepareAttack();
					this.enemyController.Defense("leftKick");
					AnimControl.StartAnimBlending(this.go, this.leftKick);
					this.attackAnim = this.leftKick;
				}
				this.currentRightKick = !this.currentRightKick;
			}
			this.lastAttackTime = Time.timeSinceLevelLoad;
		}
		else if (flag2 && (this.lastAttackTime == 0f || Time.timeSinceLevelLoad > this.lastAttackTime + 0.75f))
		{
			if ((!this.firstTakedownPerformed && (float)this.enemyController.health <= (float)this.enemyController.maxHealth * 0.5f + 40f) || this.enemyController.health <= 40)
			{
				this.TakeDownStart();
				this.firstTakedownPerformed = true;
			}
			else
			{
				if (this.currentRightPunch)
				{
					this.PrepareAttack();
					this.enemyController.Defense("punch");
					AnimControl.StartAnimBlending(this.go, this.leftPunch);
					this.attackAnim = this.leftPunch;
				}
				else
				{
					this.PrepareAttack();
					this.enemyController.Defense("punch");
					AnimControl.StartAnimBlending(this.go, this.rightPunch);
					this.attackAnim = this.rightPunch;
				}
				this.currentRightPunch = !this.currentRightPunch;
			}
			this.lastAttackTime = Time.timeSinceLevelLoad;
		}
		if (flag3 && this.attackAnim == null)
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
		if (!flag3 && base.GetComponent<Animation>().IsPlaying(this.blok.Anim()))
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
				base.GetComponent<Animation>()[this.blok.Anim()].time = this.blok.startTime;
			}
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x000343AC File Offset: 0x000325AC
	private void Awake()
	{
		if (MobileInput.instance != null)
		{
			FightingControl.normalMobileControls = MobileInput.instance.transform.parent.gameObject;
			FightingControl.normalMobileControls.SetActive(false);
		}
		if (this.joystickPrefab)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.joystickPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.name = "Melee Joystick Left";
			FightingControl.meleeJoystickLeft = gameObject.GetComponent<Joystick>();
			gameObject.transform.parent = base.transform.parent;
		}
		this.go = base.gameObject;
		this.tr = base.transform;
		this.realmMinDistance = this.minDistance;
		base.GetComponent<Animation>()[this.blok.Anim()].AddMixingTransform(this.spine);
		base.GetComponent<Animation>()[this.blok.Anim()].layer = 1;
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x000344A8 File Offset: 0x000326A8
	public void BattleMode(GameObject e)
	{
		this.enemy = e;
		this.enemyTransform = this.enemy.transform;
		this.enemyController = this.enemy.GetComponent<FightingEnemy>();
		this.enemyPointer = this.enemyController.pointer;
		this.enemyHead = this.enemyController.head;
		this.enemyTrunk = this.enemyController.Trunk;
		this.mask = ~(1 << this.go.layer | 1 << this.enemy.layer | 1 << this.bar.layer | 2 | 4 | 16 | 1 << LayerMask.NameToLayer("floor"));
		this.floorMask = 1 << LayerMask.NameToLayer("floor");
		this.isActive = true;
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x000345A4 File Offset: 0x000327A4
	public void BattleMode()
	{
		if (!this.isActive)
		{
			return;
		}
		this.isActive = false;
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x000345D8 File Offset: 0x000327D8
	private void OnDestroy()
	{
		if (this.enemyController != null)
		{
			this.enemyController.BattleMode();
		}
		this.TakeDownCam1 = null;
		this.TakeDownCam2 = null;
		this.TakeDownCam3 = null;
		this.TakeDownCam4 = null;
		this.TakeDownCamEnemy = null;
		FightingControl.normalMobileControls = null;
		FightingControl.meleeJoystickLeft = null;
		this.cam = null;
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00034638 File Offset: 0x00032838
	private void LateUpdate()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.isActive && this.enemyController.isActive)
		{
			this.trPos = this.tr.position;
			this.distance = (new Vector3(this.trPos.x, 0f, this.trPos.z) - new Vector3(this.enemyPointer.position.x, 0f, this.enemyPointer.position.z)).magnitude;
			this.distToHead = (new Vector3(this.trPos.x, this.enemyHead.position.y, this.trPos.z) - this.enemyHead.position).magnitude;
			this.distToTrunk = (new Vector3(this.trPos.x, this.enemyTrunk.position.y, this.trPos.z) - this.enemyTrunk.position).magnitude;
			if (this.health > 0)
			{
				if (this.disableControlForTakedown <= 0f)
				{
					this.LookAtEnemy();
				}
				this.CheckAnimPlaying();
				this.BlowsController();
				if (this.ingeniousAttacks >= this.hitsBeforeTakeDown)
				{
					this.CheckSpecials();
				}
				this.SendDmgMessage();
			}
			else
			{
				this.StopAnims();
				base.GetComponent<Animation>().Stop(this.idle.Anim());
				base.GetComponent<Animation>().Stop(this.blok.Anim());
				AnimControl.StartAnimBlending(this.go, this.knokdown);
				this.isActive = false;
			}
			this.HudUpd();
		}
		else
		{
			this.BattleMode();
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00034828 File Offset: 0x00032A28
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.disableControlForTakedown > 0f)
		{
			this.disableControlForTakedown -= Time.deltaTime;
		}
		if (this.isActive && this.enemyController.isActive)
		{
			this.trPos = this.tr.position;
			this.CalcCapsule();
			this.MoveCharacter();
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0003489C File Offset: 0x00032A9C
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

	// Token: 0x06000698 RID: 1688 RVA: 0x0003492C File Offset: 0x00032B2C
	private void OnDrawGizmosSelected()
	{
		this.trPos = base.transform.position;
		this.CalcCapsule();
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(this.bootom, this.charRadius);
		Gizmos.DrawWireSphere(this.top, this.charRadius);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0003497C File Offset: 0x00032B7C
	private void CalcCapsule()
	{
		this.bootom = this.trPos + this.charCenter + new Vector3(0f, -this.charHeight * 0.5f + this.charRadius, 0f);
		this.top = this.trPos + this.charCenter + new Vector3(0f, this.charHeight * 0.5f - this.charRadius, 0f);
	}

	// Token: 0x040007C1 RID: 1985
	private LayerMask mask;

	// Token: 0x040007C2 RID: 1986
	public float charHeight = 1.6f;

	// Token: 0x040007C3 RID: 1987
	public float charRadius = 0.4f;

	// Token: 0x040007C4 RID: 1988
	public Vector3 charCenter = new Vector3(0f, 0.9f, 0f);

	// Token: 0x040007C5 RID: 1989
	private Vector3 trPos;

	// Token: 0x040007C6 RID: 1990
	private Vector3 bootom;

	// Token: 0x040007C7 RID: 1991
	private Vector3 top;

	// Token: 0x040007C8 RID: 1992
	public Transform head;

	// Token: 0x040007C9 RID: 1993
	public Transform Trunk;

	// Token: 0x040007CA RID: 1994
	private RaycastHit hit;

	// Token: 0x040007CB RID: 1995
	public float wallOffset = 2f;

	// Token: 0x040007CC RID: 1996
	[NonSerialized]
	public bool isActive;

	// Token: 0x040007CD RID: 1997
	private string currentTakeDown;

	// Token: 0x040007CE RID: 1998
	public GameObject bar;

	// Token: 0x040007CF RID: 1999
	private Transform barTr;

	// Token: 0x040007D0 RID: 2000
	public float barYPos = 1.7f;

	// Token: 0x040007D1 RID: 2001
	public GameObject hudSpecialAvailable;

	// Token: 0x040007D2 RID: 2002
	private Transform hudSpAvailableTr;

	// Token: 0x040007D3 RID: 2003
	public GameObject HudSpecialInRange;

	// Token: 0x040007D4 RID: 2004
	private Transform hudSpInRangeTr;

	// Token: 0x040007D5 RID: 2005
	public float SpIndicatorYPosition = 0.35f;

	// Token: 0x040007D6 RID: 2006
	public Transform cam;

	// Token: 0x040007D7 RID: 2007
	public int hitsBeforeTakeDown = 4;

	// Token: 0x040007D8 RID: 2008
	private Transform tr;

	// Token: 0x040007D9 RID: 2009
	private GameObject go;

	// Token: 0x040007DA RID: 2010
	private GameObject enemy;

	// Token: 0x040007DB RID: 2011
	private Transform enemyTransform;

	// Token: 0x040007DC RID: 2012
	private Transform enemyPointer;

	// Token: 0x040007DD RID: 2013
	private FightingEnemy enemyController;

	// Token: 0x040007DE RID: 2014
	private Transform enemyHead;

	// Token: 0x040007DF RID: 2015
	private float distToHead;

	// Token: 0x040007E0 RID: 2016
	private Transform enemyTrunk;

	// Token: 0x040007E1 RID: 2017
	private float distToTrunk;

	// Token: 0x040007E2 RID: 2018
	public int health = 100;

	// Token: 0x040007E3 RID: 2019
	public int maxHealth = 100;

	// Token: 0x040007E4 RID: 2020
	public float minDistance = 0.7f;

	// Token: 0x040007E5 RID: 2021
	[NonSerialized]
	public float realmMinDistance = 0.7f;

	// Token: 0x040007E6 RID: 2022
	private float distance;

	// Token: 0x040007E7 RID: 2023
	private string attackN;

	// Token: 0x040007E8 RID: 2024
	public Transform spine;

	// Token: 0x040007E9 RID: 2025
	public GameObject TakeDownCam1;

	// Token: 0x040007EA RID: 2026
	public GameObject TakeDownCam2;

	// Token: 0x040007EB RID: 2027
	public GameObject TakeDownCam3;

	// Token: 0x040007EC RID: 2028
	public GameObject TakeDownCam4;

	// Token: 0x040007ED RID: 2029
	public GameObject TakeDownCamEnemy;

	// Token: 0x040007EE RID: 2030
	public float forwardSpeed = 1f;

	// Token: 0x040007EF RID: 2031
	public float sideSpeed = 1f;

	// Token: 0x040007F0 RID: 2032
	private Vector3 moveDirection;

	// Token: 0x040007F1 RID: 2033
	public ShooterGameCamera smoothFollowCam;

	// Token: 0x040007F2 RID: 2034
	public string vertical = "Vertical";

	// Token: 0x040007F3 RID: 2035
	public string horizontal = "Horizontal";

	// Token: 0x040007F4 RID: 2036
	public string blokButton = "Blok";

	// Token: 0x040007F5 RID: 2037
	public string leftPunchButton = "LeftPunch";

	// Token: 0x040007F6 RID: 2038
	public string rightPunchButton = "RightPunch";

	// Token: 0x040007F7 RID: 2039
	public string leftKickButton = "LeftKick";

	// Token: 0x040007F8 RID: 2040
	public string rightKickButton = "RightKick";

	// Token: 0x040007F9 RID: 2041
	public string takeDownButton = "TakeDown";

	// Token: 0x040007FA RID: 2042
	private bool attackButtonPressed;

	// Token: 0x040007FB RID: 2043
	public GameObject joystickPrefab;

	// Token: 0x040007FC RID: 2044
	public AnimOptions idle = new AnimOptions();

	// Token: 0x040007FD RID: 2045
	public AnimOptions blok = new AnimOptions();

	// Token: 0x040007FE RID: 2046
	public AudioClip blokSlap;

	// Token: 0x040007FF RID: 2047
	public float slapVol = 1f;

	// Token: 0x04000800 RID: 2048
	public float slapDealay;

	// Token: 0x04000801 RID: 2049
	private bool blokSoundIsPlayed;

	// Token: 0x04000802 RID: 2050
	public AnimOptions leftPunch = new AnimOptions();

	// Token: 0x04000803 RID: 2051
	public AnimOptions leftPunchFailure = new AnimOptions();

	// Token: 0x04000804 RID: 2052
	public AnimOptions rightPunch = new AnimOptions();

	// Token: 0x04000805 RID: 2053
	public AnimOptions rightPunchFailure = new AnimOptions();

	// Token: 0x04000806 RID: 2054
	public AnimOptions leftKick = new AnimOptions();

	// Token: 0x04000807 RID: 2055
	public AnimOptions rightKick = new AnimOptions();

	// Token: 0x04000808 RID: 2056
	public AnimOptions punchedRight = new AnimOptions();

	// Token: 0x04000809 RID: 2057
	public AnimOptions punchedLeft = new AnimOptions();

	// Token: 0x0400080A RID: 2058
	public AnimOptions kickedLeft = new AnimOptions();

	// Token: 0x0400080B RID: 2059
	public AnimOptions kickedRight = new AnimOptions();

	// Token: 0x0400080C RID: 2060
	public AnimOptions takeDown1 = new AnimOptions();

	// Token: 0x0400080D RID: 2061
	public AnimOptions takeDown2 = new AnimOptions();

	// Token: 0x0400080E RID: 2062
	public AnimOptions takeDown3 = new AnimOptions();

	// Token: 0x0400080F RID: 2063
	public AnimOptions takeDown4 = new AnimOptions();

	// Token: 0x04000810 RID: 2064
	public AnimOptions takeDown5 = new AnimOptions();

	// Token: 0x04000811 RID: 2065
	public AnimOptions knokdown = new AnimOptions();

	// Token: 0x04000812 RID: 2066
	public AnimOptions walkForward = new AnimOptions();

	// Token: 0x04000813 RID: 2067
	public AnimOptions walkBeckward = new AnimOptions();

	// Token: 0x04000814 RID: 2068
	public AnimOptions walkLeft = new AnimOptions();

	// Token: 0x04000815 RID: 2069
	public AnimOptions walkRight = new AnimOptions();

	// Token: 0x04000816 RID: 2070
	private AnimOptions attackAnim;

	// Token: 0x04000817 RID: 2071
	[NonSerialized]
	public bool blokAnimPlayed;

	// Token: 0x04000818 RID: 2072
	[NonSerialized]
	public bool painAnimPlayed;

	// Token: 0x04000819 RID: 2073
	private bool takeDownPlayed;

	// Token: 0x0400081A RID: 2074
	[NonSerialized]
	public int ingeniousAttacks = 1;

	// Token: 0x0400081B RID: 2075
	[NonSerialized]
	public bool dmg;

	// Token: 0x0400081C RID: 2076
	private int tdCount;

	// Token: 0x0400081D RID: 2077
	private int[] availableTakeDowns = new int[6];

	// Token: 0x0400081E RID: 2078
	public static Joystick meleeJoystickLeft;

	// Token: 0x0400081F RID: 2079
	public static GameObject normalMobileControls;

	// Token: 0x04000820 RID: 2080
	private int currentSelectTakeDown;

	// Token: 0x04000821 RID: 2081
	public Texture healthBackGround;

	// Token: 0x04000822 RID: 2082
	public Texture FarisBar;

	// Token: 0x04000823 RID: 2083
	public Texture EnemyBar;

	// Token: 0x04000824 RID: 2084
	public Texture TakedownBackGround;

	// Token: 0x04000825 RID: 2085
	public Texture TakedownF1;

	// Token: 0x04000826 RID: 2086
	public Texture TakedownF2;

	// Token: 0x04000827 RID: 2087
	public Texture TakedownF3;

	// Token: 0x04000828 RID: 2088
	public Texture TakedownPC;

	// Token: 0x04000829 RID: 2089
	public Texture TakedownPS3;

	// Token: 0x0400082A RID: 2090
	public Texture TakedownPS3Move;

	// Token: 0x0400082B RID: 2091
	public Texture TakedownXBox;

	// Token: 0x0400082C RID: 2092
	public Texture TakedownMobile;

	// Token: 0x0400082D RID: 2093
	private float disableControlForTakedown;

	// Token: 0x0400082E RID: 2094
	private int currentEnemyStandupSound;

	// Token: 0x0400082F RID: 2095
	public AudioClip[] EnemyStandupSound;

	// Token: 0x04000830 RID: 2096
	private bool showBlinkingText = true;

	// Token: 0x04000831 RID: 2097
	private float showBlinkingTextTimer;

	// Token: 0x04000832 RID: 2098
	public AudioClip[] gettingHitSounds;

	// Token: 0x04000833 RID: 2099
	public AudioClip deathSound;

	// Token: 0x04000834 RID: 2100
	private LayerMask floorMask;

	// Token: 0x04000835 RID: 2101
	private bool currentRightKick;

	// Token: 0x04000836 RID: 2102
	private bool currentRightPunch;

	// Token: 0x04000837 RID: 2103
	private bool firstTakedownPerformed;

	// Token: 0x04000838 RID: 2104
	private float previousUnaimedZoom;

	// Token: 0x04000839 RID: 2105
	private float previousCamOffsetY;

	// Token: 0x0400083A RID: 2106
	private float lastAttackTime;
}
