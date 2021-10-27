using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public static class AnimControl
{
	// Token: 0x060005FB RID: 1531 RVA: 0x0002A1AC File Offset: 0x000283AC
	public static bool CheckAnimActivity(GameObject go, AnimOptions ao)
	{
		return go.GetComponent<Animation>().IsPlaying(ao.Anim()) && go.GetComponent<Animation>()[ao.Anim()].time < ao.endTime;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0002A1F4 File Offset: 0x000283F4
	public static bool CheckAnimActivity(GameObject go, AnimOptions ao, bool inverse)
	{
		return go.GetComponent<Animation>().IsPlaying(ao.Anim()) && go.GetComponent<Animation>()[ao.Anim()].time > ao.endTime;
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0002A23C File Offset: 0x0002843C
	public static bool CheckAnimActivity(GameObject go, AnimOptions ao, ref Vector3 pp, Transform tr)
	{
		if (go.GetComponent<Animation>().IsPlaying(ao.Anim()) && go.GetComponent<Animation>()[ao.Anim()].time < ao.endTime)
		{
			if (go.GetComponent<Animation>()[ao.Anim()].time > ao.hitTime)
			{
				pp = tr.position;
				go.GetComponent<Animation>()[ao.Anim()].speed = 0f;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002A2CC File Offset: 0x000284CC
	public static bool GroupAnimActivity(GameObject go, AnimOptions[] anims)
	{
		foreach (AnimOptions ao in anims)
		{
			if (AnimControl.CheckAnimActivity(go, ao))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0002A304 File Offset: 0x00028504
	public static AnimOptions PlayingAttack(GameObject go, AnimOptions[] anims)
	{
		foreach (AnimOptions animOptions in anims)
		{
			if (AnimControl.CheckAnimActivity(go, animOptions))
			{
				return animOptions;
			}
		}
		return null;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002A33C File Offset: 0x0002853C
	public static bool CheckAnimAndRange(GameObject go, AnimOptions ao, float distance)
	{
		return go.GetComponent<Animation>()[ao.Anim()].time > ao.hitTime && go.GetComponent<Animation>()[ao.Anim()].time < ao.hitTime + 0.05f && distance <= ao.range;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002A3A0 File Offset: 0x000285A0
	public static void StartAnimBlending(GameObject go, AnimOptions ao)
	{
		AudioClip[] array = null;
		AnimControl.StartAnimBlending(go, ao, ref array);
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002A3B8 File Offset: 0x000285B8
	public static void StartAnimBlending(GameObject go, AnimOptions ao, ref AudioClip[] gettingHitSounds)
	{
		go.GetComponent<Animation>().CrossFade(ao.Anim(), 0.3f);
		if (ao.sound != null)
		{
			PlayOfterDelay playOfterDelay = new GameObject
			{
				transform = 
				{
					position = go.transform.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay.clip = ao.sound;
			playOfterDelay.volume = ao.volume;
			playOfterDelay.delay = ao.soundStartOfter;
		}
		if (ao.sound2 != null)
		{
			PlayOfterDelay playOfterDelay2 = new GameObject
			{
				transform = 
				{
					position = go.transform.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay2.clip = ao.sound2;
			playOfterDelay2.volume = ao.volume;
			playOfterDelay2.delay = ao.soundStartOfter;
		}
		if (gettingHitSounds != null && gettingHitSounds.Length > 0)
		{
			PlayOfterDelay playOfterDelay3 = new GameObject
			{
				transform = 
				{
					position = go.transform.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay3.clip = gettingHitSounds[UnityEngine.Random.Range(0, gettingHitSounds.Length)];
			playOfterDelay3.volume = ao.volume;
			playOfterDelay3.delay = ao.soundStartOfter;
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002A4F4 File Offset: 0x000286F4
	public static void PlaySound(Transform tr, AnimOptions ao)
	{
		AudioClip[] array = null;
		AnimControl.PlaySound(tr, ao, ref array);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002A50C File Offset: 0x0002870C
	public static void PlaySound(Transform tr, AnimOptions ao, ref AudioClip[] gettingHitSounds)
	{
		if (ao.sound != null)
		{
			PlayOfterDelay playOfterDelay = new GameObject
			{
				transform = 
				{
					position = tr.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay.clip = ao.sound;
			playOfterDelay.volume = ao.volume;
			playOfterDelay.delay = ao.soundStartOfter;
		}
		if (ao.sound2 != null)
		{
			PlayOfterDelay playOfterDelay2 = new GameObject
			{
				transform = 
				{
					position = tr.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay2.clip = ao.sound2;
			playOfterDelay2.volume = ao.volume;
			playOfterDelay2.delay = ao.soundStartOfter;
		}
		if (gettingHitSounds != null && gettingHitSounds.Length > 0)
		{
			PlayOfterDelay playOfterDelay3 = new GameObject
			{
				transform = 
				{
					position = tr.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay3.clip = gettingHitSounds[UnityEngine.Random.Range(0, gettingHitSounds.Length)];
			playOfterDelay3.volume = ao.volume;
			playOfterDelay3.delay = ao.soundStartOfter;
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002A620 File Offset: 0x00028820
	public static void PlaySound(Transform tr, AudioClip ac, float vol, float dealay)
	{
		if (ac != null)
		{
			PlayOfterDelay playOfterDelay = new GameObject
			{
				transform = 
				{
					position = tr.position
				}
			}.AddComponent<PlayOfterDelay>();
			playOfterDelay.clip = ac;
			playOfterDelay.volume = vol;
			playOfterDelay.delay = dealay;
		}
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002A66C File Offset: 0x0002886C
	public static void StopAnims(GameObject go, AnimOptions[] anim)
	{
		foreach (AnimOptions animOptions in anim)
		{
			go.GetComponent<Animation>().Stop(animOptions.Anim());
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0002A6A4 File Offset: 0x000288A4
	public static void ResetOptions(GameObject go, AnimOptions[] anim)
	{
		foreach (AnimOptions animOptions in anim)
		{
			if (animOptions != null)
			{
				animOptions.startTime = 0f;
				animOptions.endTime = go.GetComponent<Animation>()[animOptions.Anim()].length;
				animOptions.hitTime = go.GetComponent<Animation>()[animOptions.Anim()].length * 0.5f;
				animOptions.fadeTime = 0.2f;
				animOptions.range = 0f;
				animOptions.weight = 3f;
			}
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0002A73C File Offset: 0x0002893C
	public static void StoreAttacks(ref int[] attacks, int No)
	{
		for (int i = 0; i < attacks.Length; i++)
		{
			if (attacks[i] == 0)
			{
				attacks[i] = No;
				break;
			}
		}
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0002A774 File Offset: 0x00028974
	public static string AttackSwitcer(GameObject go, float distToHead, float distToTrunk, AnimOptions attackAnim, AnimOptions rightPunch, AnimOptions leftPunch, AnimOptions leftKick, AnimOptions rightKick)
	{
		int num = 0;
		if (attackAnim == rightPunch)
		{
			num = 1;
		}
		else if (attackAnim == leftPunch)
		{
			num = 2;
		}
		else if (attackAnim == leftKick)
		{
			num = 3;
		}
		else if (attackAnim == rightKick)
		{
			num = 4;
		}
		switch (num)
		{
		case 1:
			if (AnimControl.CheckAnimAndRange(go, rightPunch, distToHead))
			{
				return "rightPunch";
			}
			break;
		case 2:
			if (AnimControl.CheckAnimAndRange(go, leftPunch, distToHead))
			{
				return "leftPunch";
			}
			break;
		case 3:
			if (AnimControl.CheckAnimAndRange(go, leftKick, distToTrunk))
			{
				return "leftKick";
			}
			break;
		case 4:
			if (AnimControl.CheckAnimAndRange(go, rightKick, distToTrunk))
			{
				return "rightKick";
			}
			break;
		}
		return string.Empty;
	}
}
