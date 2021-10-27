using System;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class Grenade : MonoBehaviour
{
	// Token: 0x060009FF RID: 2559 RVA: 0x00067E1C File Offset: 0x0006601C
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.exploded = false;
		this.timer = 0f;
		this.thisTransform = base.transform;
		this.soldierCamera = Camera.main.GetComponent<ShooterGameCamera>();
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00067E68 File Offset: 0x00066068
	private void Detonate()
	{
		if (this.exploded)
		{
			return;
		}
		this.exploded = true;
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = false;
			if (this.smoke != null)
			{
				UnityEngine.Object.Destroy(this.smoke);
			}
		}
		else
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.enabled = false;
			}
		}
		Vector3 position = this.thisTransform.position;
		Collider[] array2 = Physics.OverlapSphere(position, this.explosionRadius * this.scaleFactor);
		BotAI.PlayerFiredHisWeapon(position);
		float distance = Vector3.Distance(this.soldierCamera.transform.position, position);
		this.soldierCamera.StartShake(distance);
		if (array2 != null)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				HitInfo hitInfo = default(HitInfo);
				hitInfo.D = 200f * (1f - Vector3.Distance(position, array2[j].transform.position) / (this.explosionRadius * this.scaleFactor));
				hitInfo.E = this.power;
				hitInfo.EP = position;
				hitInfo.ER = this.explosionRadius * this.scaleFactor;
				array2[j].gameObject.SendMessage("Hit", hitInfo, SendMessageOptions.DontRequireReceiver);
				Rigidbody rigidbody = array2[j].gameObject.GetComponent<Rigidbody>();
				if (rigidbody != null)
				{
					rigidbody.isKinematic = false;
				}
				else if (array2[j].gameObject.transform.parent != null)
				{
					rigidbody = array2[j].gameObject.transform.parent.GetComponent<Rigidbody>();
					if (rigidbody != null)
					{
						rigidbody.isKinematic = false;
					}
				}
				if (rigidbody != null)
				{
					rigidbody.AddExplosionForce(this.power, position, this.explosionRadius * this.scaleFactor, 3f);
				}
				if (array2[j].GetComponent<Collider>().tag == "glass")
				{
					array2[j].gameObject.SendMessage("BreakAll", SendMessageOptions.DontRequireReceiver);
				}
				if (array2[j].GetComponent<Collider>().tag == "Breakable")
				{
					Transform parent = array2[j].GetComponent<Collider>().transform.parent;
					if (array2[j].GetComponent<Collider>().GetComponent<InteractionTrigger>() != null)
					{
						UnityEngine.Object.Destroy(array2[j].GetComponent<Collider>().gameObject);
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
			}
		}
		base.gameObject.SendMessage("Explode", SendMessageOptions.DontRequireReceiver);
		this.PlaySound(distance);
		if (this.explosionEmitter != null)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.explosionEmitter, base.transform.position, Quaternion.identity);
			foreach (ParticleEmitter particleEmitter in gameObject.GetComponentsInChildren<ParticleEmitter>())
			{
				particleEmitter.maxSize *= this.scaleFactor;
				particleEmitter.minSize *= this.scaleFactor;
			}
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00068244 File Offset: 0x00066444
	private void PlaySound(float distance)
	{
		if (distance < this.farSoundDistance * this.scaleFactor)
		{
			int num = UnityEngine.Random.Range(0, this.nearSounds.Length);
			base.GetComponent<AudioSource>().PlayOneShot(this.nearSounds[num], SpeechManager.sfxVolume);
			this.timer = this.nearSounds[num].length + 1f;
		}
		else
		{
			int num = UnityEngine.Random.Range(0, this.farSounds.Length);
			base.GetComponent<AudioSource>().PlayOneShot(this.farSounds[num], SpeechManager.sfxVolume);
			this.timer = this.farSounds[num].length + 1f;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000682EC File Offset: 0x000664EC
	private void Update()
	{
		if (this.detonate)
		{
			this.detonateTimer -= Time.deltaTime;
			if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.noNoSoundArabic != null)
			{
				if (!this.noNoSoundPlayed && this.player != null && Vector3.Distance(this.player.transform.position, base.transform.position) < 3f && this.detonateTimer <= this.noNoSoundArabic.length)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.noNoSoundArabic, SpeechManager.speechVolume);
					this.noNoSoundPlayed = true;
				}
			}
			else if (this.noNoSound != null && !this.noNoSoundPlayed && this.player != null && Vector3.Distance(this.player.transform.position, base.transform.position) < 3f && this.detonateTimer <= this.noNoSound.length)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.noNoSound, SpeechManager.speechVolume);
				this.noNoSoundPlayed = true;
			}
			if (!this.enemySoundPlayed)
			{
				UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
				foreach (UnityEngine.Object obj in array)
				{
					BotAI botAI = (BotAI)obj;
					Vector3 position = botAI.transform.position;
					position.y = base.transform.position.y;
					if (Vector3.Distance(position, base.transform.position) < this.explosionRadius * this.scaleFactor && this.detonateTimer <= this.enemySounds[Grenade.currentEnemySound].length)
					{
						if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic && this.enemySoundsArabic[Grenade.currentEnemySound] != null)
						{
							base.GetComponent<AudioSource>().PlayOneShot(this.enemySoundsArabic[Grenade.currentEnemySound], SpeechManager.speechVolume);
							this.enemySoundPlayed = true;
							Grenade.currentEnemySound = (Grenade.currentEnemySound + 1) % this.enemySoundsArabic.Length;
						}
						else
						{
							base.GetComponent<AudioSource>().PlayOneShot(this.enemySounds[Grenade.currentEnemySound], SpeechManager.speechVolume);
							this.enemySoundPlayed = true;
							Grenade.currentEnemySound = (Grenade.currentEnemySound + 1) % this.enemySounds.Length;
						}
					}
				}
			}
			if (!this.grenadeSoundPlayed && this.detonateTimer <= this.grenadeSound.length)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.grenadeSound, 0.5f * SpeechManager.sfxVolume);
				this.grenadeSoundPlayed = true;
			}
			if (this.detonateTimer <= 0f)
			{
				this.Detonate();
			}
		}
		if (this.thisTransform.position.y < this.minY)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.exploded && this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00068644 File Offset: 0x00066844
	private void OnCollisionEnter(Collision c)
	{
		if (!this.exploded)
		{
			if (!this.explodeOnCollision)
			{
				if (!this.detonate)
				{
					this.detonateTimer = this.explosionTime;
					this.detonate = true;
				}
			}
			else
			{
				this.Detonate();
			}
		}
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00068690 File Offset: 0x00066890
	private void OnCollisionStay(Collision c)
	{
		if (!this.exploded)
		{
			if (!this.explodeOnCollision)
			{
				if (!this.detonate)
				{
					this.detonateTimer = this.explosionTime;
					this.detonate = true;
				}
			}
			else
			{
				this.Detonate();
			}
		}
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x000686DC File Offset: 0x000668DC
	private void OnCollisionExit(Collision c)
	{
		if (!this.exploded)
		{
			if (!this.explodeOnCollision)
			{
				if (!this.detonate)
				{
					this.detonateTimer = this.explosionTime;
					this.detonate = true;
				}
			}
			else
			{
				this.Detonate();
			}
		}
	}

	// Token: 0x04000F4B RID: 3915
	private Transform thisTransform;

	// Token: 0x04000F4C RID: 3916
	public float minY = -10f;

	// Token: 0x04000F4D RID: 3917
	public GameObject smoke;

	// Token: 0x04000F4E RID: 3918
	public GameObject explosionEmitter;

	// Token: 0x04000F4F RID: 3919
	public float explosionTime = 3f;

	// Token: 0x04000F50 RID: 3920
	public float explosionRadius;

	// Token: 0x04000F51 RID: 3921
	public float power = 3200f;

	// Token: 0x04000F52 RID: 3922
	private float timer;

	// Token: 0x04000F53 RID: 3923
	public bool explodeOnCollision = true;

	// Token: 0x04000F54 RID: 3924
	public ShooterGameCamera soldierCamera;

	// Token: 0x04000F55 RID: 3925
	public AudioClip[] nearSounds;

	// Token: 0x04000F56 RID: 3926
	public AudioClip[] farSounds;

	// Token: 0x04000F57 RID: 3927
	public float farSoundDistance = 25f;

	// Token: 0x04000F58 RID: 3928
	private bool exploded;

	// Token: 0x04000F59 RID: 3929
	private RaycastHit hit;

	// Token: 0x04000F5A RID: 3930
	private bool detonate;

	// Token: 0x04000F5B RID: 3931
	private float detonateTimer;

	// Token: 0x04000F5C RID: 3932
	private GameObject player;

	// Token: 0x04000F5D RID: 3933
	public AudioClip noNoSound;

	// Token: 0x04000F5E RID: 3934
	public AudioClip[] enemySounds;

	// Token: 0x04000F5F RID: 3935
	public AudioClip noNoSoundArabic;

	// Token: 0x04000F60 RID: 3936
	public AudioClip[] enemySoundsArabic;

	// Token: 0x04000F61 RID: 3937
	private static int currentEnemySound;

	// Token: 0x04000F62 RID: 3938
	private bool noNoSoundPlayed;

	// Token: 0x04000F63 RID: 3939
	private bool enemySoundPlayed;

	// Token: 0x04000F64 RID: 3940
	public AudioClip grenadeSound;

	// Token: 0x04000F65 RID: 3941
	private bool grenadeSoundPlayed;

	// Token: 0x04000F66 RID: 3942
	public float scaleFactor = 1f;
}
