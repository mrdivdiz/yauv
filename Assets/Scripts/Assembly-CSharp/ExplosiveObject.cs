using System;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class ExplosiveObject : MonoBehaviour
{
	// Token: 0x060009F8 RID: 2552 RVA: 0x000678B4 File Offset: 0x00065AB4
	private void Start()
	{
		this.soldierCamera = Camera.main.GetComponent<ShooterGameCamera>();
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x000678C8 File Offset: 0x00065AC8
	private void OnDestroy()
	{
		this.smoke = null;
		this.explosionEmitter = null;
		this.destroyedVersion = null;
		for (int i = 0; i < this.nearSounds.Length; i++)
		{
			this.nearSounds[i] = null;
		}
		for (int j = 0; j < this.farSounds.Length; j++)
		{
			this.farSounds[j] = null;
		}
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00067930 File Offset: 0x00065B30
	private void Update()
	{
		if (this.exploded && this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00067988 File Offset: 0x00065B88
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
		Vector3 position = base.transform.position;
		Collider[] array2 = Physics.OverlapSphere(position, this.explosionRadius);
		BotAI.PlayerFiredHisWeapon(position);
		float distance = Vector3.Distance(this.soldierCamera.transform.position, position);
		this.soldierCamera.StartShake(distance);
		if (array2 != null)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				HitInfo hitInfo = default(HitInfo);
				hitInfo.D = 200f * (1f - Vector3.Distance(position, array2[j].transform.position) / this.explosionRadius);
				hitInfo.E = this.power;
				hitInfo.EP = position;
				hitInfo.ER = this.explosionRadius;
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
					rigidbody.AddExplosionForce(this.power, position, this.explosionRadius, 3f);
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
			UnityEngine.Object.Instantiate(this.explosionEmitter, base.transform.position, Quaternion.identity);
		}
		if (this.destroyedVersion != null)
		{
			UnityEngine.Object.Instantiate(this.destroyedVersion, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00067D24 File Offset: 0x00065F24
	private void PlaySound(float distance)
	{
		if (distance < this.farSoundDistance)
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

	// Token: 0x04000F3F RID: 3903
	public GameObject smoke;

	// Token: 0x04000F40 RID: 3904
	public GameObject explosionEmitter;

	// Token: 0x04000F41 RID: 3905
	public Transform destroyedVersion;

	// Token: 0x04000F42 RID: 3906
	public float explosionTime = 3f;

	// Token: 0x04000F43 RID: 3907
	public float explosionRadius;

	// Token: 0x04000F44 RID: 3908
	public float power = 3200f;

	// Token: 0x04000F45 RID: 3909
	private float timer;

	// Token: 0x04000F46 RID: 3910
	private bool exploded;

	// Token: 0x04000F47 RID: 3911
	public AudioClip[] nearSounds;

	// Token: 0x04000F48 RID: 3912
	public AudioClip[] farSounds;

	// Token: 0x04000F49 RID: 3913
	public float farSoundDistance = 25f;

	// Token: 0x04000F4A RID: 3914
	public ShooterGameCamera soldierCamera;
}
