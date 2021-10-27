using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class ZombieAI : MonoBehaviour
{
	// Token: 0x060005F0 RID: 1520 RVA: 0x00029810 File Offset: 0x00027A10
	private void Start()
	{
		WaveSpawner.ZombiesNo++;
		this.currentZombieSound = UnityEngine.Random.Range(0, this.ZombieSounds.Length);
		this.agent = base.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		this.agent.acceleration = 10f;
		this.movingMode = (++ZombieAI.currentMovingMode + 1) % 2;
		if (this.target == null)
		{
			this.target = GameObject.FindGameObjectWithTag("Player").transform;
		}
		foreach (string name in this.hitAnims)
		{
			base.GetComponent<Animation>()[name].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[name].layer = 2;
		}
		foreach (string name2 in this.attackAnims)
		{
			base.GetComponent<Animation>()[name2].wrapMode = WrapMode.Once;
		}
		this.idleAnim = this.idleAnims[UnityEngine.Random.Range(0, this.idleAnims.Length)];
		this.walkAnim = this.walkAnims[UnityEngine.Random.Range(0, this.walkAnims.Length)];
		this.runAnim = this.runAnims[UnityEngine.Random.Range(0, this.runAnims.Length)];
		this.currentAttackAnim = UnityEngine.Random.Range(0, this.attackAnims.Length);
		this.currentHitAnim = UnityEngine.Random.Range(0, this.hitAnims.Length);
		if (this.bodyMaterials != null && this.bodyMaterials.Length != 0)
		{
			Renderer componentInChildren = base.GetComponentInChildren<Renderer>();
			if (componentInChildren != null)
			{
				for (int k = 0; k < componentInChildren.sharedMaterials.Length; k++)
				{
					bool flag = false;
					foreach (Material x in this.bodyMaterials)
					{
						if (x == componentInChildren.sharedMaterials[k])
						{
							if (k == 0)
							{
								componentInChildren.sharedMaterial = this.bodyMaterials[UnityEngine.Random.Range(0, this.bodyMaterials.Length)];
							}
							else
							{
								Material[] sharedMaterials = componentInChildren.sharedMaterials;
								sharedMaterials[k] = this.bodyMaterials[UnityEngine.Random.Range(0, this.bodyMaterials.Length)];
								componentInChildren.sharedMaterials = sharedMaterials;
							}
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00029A84 File Offset: 0x00027C84
	private void OnDestroy()
	{
		WaveSpawner.ZombiesNo--;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00029A94 File Offset: 0x00027C94
	private void Update()
	{
		if (this.animState == ZombieAI.AnimState.HIT)
		{
			return;
		}
		if (this.target == null)
		{
			this.animState = ZombieAI.AnimState.IDLE;
			this.agent.Stop();
		}
		else if (this.animState != ZombieAI.AnimState.HIT)
		{
			if (this.agent.speed > 2f)
			{
				this.animState = ZombieAI.AnimState.RUN;
			}
			else if (this.agent.speed > 0f)
			{
				this.animState = ZombieAI.AnimState.WALK;
			}
			else
			{
				this.animState = ZombieAI.AnimState.IDLE;
			}
			float num = Vector3.Distance(this.target.position, base.transform.position);
			if (num > 1.5f * this.scaleFactor)
			{
				this.agent.SetDestination(this.target.position);
				int num2 = this.movingMode;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						if (num > 5f * this.scaleFactor)
						{
							this.agent.speed = this.runningSpeed;
						}
						else
						{
							this.agent.speed = 1f;
						}
					}
				}
				else
				{
					this.agent.speed = this.runningSpeed;
				}
			}
			else
			{
				this.animState = ZombieAI.AnimState.ATTACK;
				base.transform.LookAt(this.target);
				if (this.playerHealth == null)
				{
					if (AnimationHandler.instance != null)
					{
						this.playerHealth = AnimationHandler.instance.GetComponent<Health>();
					}
				}
				else
				{
					this.playerHealth.playerHit(base.transform.position, 10f * Time.deltaTime);
				}
			}
		}
		switch (this.animState)
		{
		case ZombieAI.AnimState.IDLE:
			if (!base.GetComponent<Animation>().IsPlaying(this.idleAnim))
			{
				base.GetComponent<Animation>().CrossFade(this.idleAnim);
				this.PlayRandomSound();
			}
			break;
		case ZombieAI.AnimState.WALK:
			if (!base.GetComponent<Animation>().IsPlaying(this.walkAnim))
			{
				base.GetComponent<Animation>().CrossFade(this.walkAnim);
				this.PlayRandomSound();
			}
			break;
		case ZombieAI.AnimState.RUN:
			if (!base.GetComponent<Animation>().IsPlaying(this.runAnim))
			{
				base.GetComponent<Animation>().CrossFade(this.runAnim);
				this.PlayRandomSound();
			}
			break;
		case ZombieAI.AnimState.ATTACK:
			if (!base.GetComponent<Animation>().IsPlaying(this.attackAnims[this.currentAttackAnim]))
			{
				this.currentAttackAnim = (this.currentAttackAnim + 1) % this.attackAnims.Length;
				base.GetComponent<Animation>().CrossFade(this.attackAnims[this.currentAttackAnim]);
				this.PlayRandomSound();
			}
			break;
		}
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00029D60 File Offset: 0x00027F60
	public void PlayRandomSound()
	{
		if (base.GetComponent<AudioSource>() != null && this.ZombieSounds != null && this.ZombieSounds.Length != 0)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.ZombieSounds[this.currentZombieSound]);
			this.currentZombieSound = (this.currentZombieSound + 1) % this.ZombieSounds.Length;
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00029DC8 File Offset: 0x00027FC8
	public void Hit(HitInfo hitInfo)
	{
		if (hitInfo.E != 0f)
		{
			this.explosionPower = hitInfo.E;
			this.explosionPosition = hitInfo.EP;
			this.explosionRadius = hitInfo.ER;
			this.health -= 100f;
		}
		else
		{
			this.explosionPower = 0f;
		}
		this.health -= hitInfo.D;
		if (this.health <= 0f)
		{
			this.Die();
			if (this.pickupablePrefab != null)
			{
				UnityEngine.Object.Instantiate(this.pickupablePrefab, base.transform.position, Quaternion.identity);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.GetComponent<Animation>()[this.hitAnims[this.currentHitAnim]].time = 0f;
		base.GetComponent<Animation>().CrossFade(this.hitAnims[this.currentHitAnim]);
		this.agent.speed = 0f;
		this.agent.Stop();
		this.animState = ZombieAI.AnimState.HIT;
		base.CancelInvoke("StopHit");
		base.Invoke("StopHit", base.GetComponent<Animation>()[this.hitAnims[this.currentHitAnim]].length);
		this.currentHitAnim = (this.currentHitAnim + 1) % this.hitAnims.Length;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00029F38 File Offset: 0x00028138
	public void StopHit()
	{
		this.animState = ZombieAI.AnimState.IDLE;
		this.agent.Resume();
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00029F4C File Offset: 0x0002814C
	private void Die()
	{
		WaveSpawner.points = (int.Parse(WaveSpawner.points) + 10).ToString();
		if (this.RagdollPrefab)
		{
			if (this.scaleFactor != 1f)
			{
				this.RagdollPrefab.localScale *= this.scaleFactor;
			}
			this.dead = (Transform)UnityEngine.Object.Instantiate(this.RagdollPrefab, base.transform.position, base.transform.rotation);
			this.dead.GetComponentInChildren<Renderer>().sharedMaterial = base.GetComponentInChildren<Renderer>().sharedMaterial;
			UnityEngine.Object.Destroy(this.dead.gameObject, 10f);
			if (this.scaleFactor != 1f)
			{
				this.RagdollPrefab.localScale /= this.scaleFactor;
			}
			Vector3 velocity = Vector3.zero;
			if (base.GetComponent<Rigidbody>())
			{
				velocity = base.GetComponent<Rigidbody>().velocity;
				base.GetComponent<Rigidbody>().AddExplosionForce(this.explosionPower, this.explosionPosition, this.explosionRadius, 3f);
			}
			else
			{
				velocity = this.agent.velocity;
			}
			this.CopyTransformsRecurse(base.transform, this.dead, velocity);
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002A0A0 File Offset: 0x000282A0
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

	// Token: 0x0400069D RID: 1693
	public Transform target;

	// Token: 0x0400069E RID: 1694
	public float health = 100f;

	// Token: 0x0400069F RID: 1695
	public Transform RagdollPrefab;

	// Token: 0x040006A0 RID: 1696
	private UnityEngine.AI.NavMeshAgent agent;

	// Token: 0x040006A1 RID: 1697
	private ZombieAI.AnimState animState;

	// Token: 0x040006A2 RID: 1698
	private Health playerHealth;

	// Token: 0x040006A3 RID: 1699
	private Transform dead;

	// Token: 0x040006A4 RID: 1700
	private float explosionPower;

	// Token: 0x040006A5 RID: 1701
	private Vector3 explosionPosition;

	// Token: 0x040006A6 RID: 1702
	private float explosionRadius;

	// Token: 0x040006A7 RID: 1703
	public Transform pickupablePrefab;

	// Token: 0x040006A8 RID: 1704
	public string[] idleAnims;

	// Token: 0x040006A9 RID: 1705
	public string[] walkAnims;

	// Token: 0x040006AA RID: 1706
	private string idleAnim = string.Empty;

	// Token: 0x040006AB RID: 1707
	private string walkAnim = string.Empty;

	// Token: 0x040006AC RID: 1708
	public string[] runAnims;

	// Token: 0x040006AD RID: 1709
	private string runAnim = string.Empty;

	// Token: 0x040006AE RID: 1710
	public string[] attackAnims;

	// Token: 0x040006AF RID: 1711
	public string[] hitAnims;

	// Token: 0x040006B0 RID: 1712
	private int currentHitAnim;

	// Token: 0x040006B1 RID: 1713
	private int currentAttackAnim;

	// Token: 0x040006B2 RID: 1714
	public float scaleFactor = 1f;

	// Token: 0x040006B3 RID: 1715
	public Material[] bodyMaterials;

	// Token: 0x040006B4 RID: 1716
	private static int currentMovingMode;

	// Token: 0x040006B5 RID: 1717
	private int movingMode;

	// Token: 0x040006B6 RID: 1718
	private int currentZombieSound;

	// Token: 0x040006B7 RID: 1719
	public AudioClip[] ZombieSounds;

	// Token: 0x040006B8 RID: 1720
	[HideInInspector]
	public float runningSpeed = 4.5f;

	// Token: 0x0200010B RID: 267
	private enum AnimState
	{
		// Token: 0x040006BA RID: 1722
		IDLE,
		// Token: 0x040006BB RID: 1723
		WALK,
		// Token: 0x040006BC RID: 1724
		RUN,
		// Token: 0x040006BD RID: 1725
		ATTACK,
		// Token: 0x040006BE RID: 1726
		HIT
	}
}
