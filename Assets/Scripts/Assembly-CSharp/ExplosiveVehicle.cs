using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026F RID: 623
public class ExplosiveVehicle : MonoBehaviour
{
	// Token: 0x06000BBC RID: 3004 RVA: 0x00094DF0 File Offset: 0x00092FF0
	private void Start()
	{
		this.soldierCamera = Camera.main.GetComponent<QuadGameCamera>();
		this.audioChild = base.transform.GetChild(0);
		if (this.audioChild.GetComponent<AudioSource>() == null)
		{
			this.audioChild.gameObject.AddComponent<AudioSource>();
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00094E48 File Offset: 0x00093048
	private void Update()
	{
		if (!this.exploded && this.health <= 0f)
		{
			if (this.ragdollizeChildren)
			{
				foreach (object obj in base.transform)
				{
					Transform transform = (Transform)obj;
					if (transform.tag != "Enemy" && transform.tag != "nonexplosive")
					{
						if (transform.GetComponent<Collider>() == null)
						{
							transform.gameObject.AddComponent<BoxCollider>();
						}
						if (transform.GetComponent<Rigidbody>() == null)
						{
							transform.gameObject.AddComponent<Rigidbody>();
							transform.GetComponent<Rigidbody>().mass = 10f;
						}
						if (this.DestroyAfterExplosion)
						{
							transform.gameObject.AddComponent<DystroyAfterTime>();
						}
					}
					if (transform.name == "MachineGun" && transform.childCount > 0)
					{
						foreach (object obj2 in transform)
						{
							Transform transform2 = (Transform)obj2;
							if (transform2.tag != "Enemy" && transform.tag != "nonexplosive")
							{
								if (transform2.GetComponent<Collider>() == null)
								{
									transform2.gameObject.AddComponent<BoxCollider>();
								}
								if (transform2.GetComponent<Rigidbody>() == null)
								{
									transform2.gameObject.AddComponent<Rigidbody>();
									transform2.GetComponent<Rigidbody>().mass = 10f;
								}
							}
							if (this.DestroyAfterExplosion)
							{
								transform2.gameObject.AddComponent<DystroyAfterTime>();
							}
						}
						transform.DetachChildren();
					}
				}
			}
			else
			{
				foreach (object obj3 in base.transform)
				{
					Transform transform3 = (Transform)obj3;
					if (transform3.tag != "Enemy" && this.DestroyAfterExplosion)
					{
						transform3.gameObject.AddComponent<DystroyAfterTime>();
					}
				}
			}
			base.transform.DetachChildren();
			this.Detonate();
		}
		if (this.exploded && this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f && this.DestroyAfterExplosion)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x00095164 File Offset: 0x00093364
	public void Explode()
	{
		this.health = 0f;
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00095174 File Offset: 0x00093374
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
			if (this.smoke != null && this.DestroyAfterExplosion)
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
		float distance = Vector3.Distance(this.soldierCamera.transform.position, position);
		this.soldierCamera.StartShake(distance);
		if (array2 != null)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("D", 200f * (1f - Vector3.Distance(position, array2[j].transform.position) / this.explosionRadius));
				hashtable.Add("E", this.power);
				hashtable.Add("EP", position);
				hashtable.Add("ER", this.explosionRadius);
				array2[j].gameObject.SendMessage("Hit", hashtable, SendMessageOptions.DontRequireReceiver);
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
		if (this.DestroyAfterExplosion)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00095518 File Offset: 0x00093718
	private void PlaySound(float distance)
	{
		if (distance < this.farSoundDistance)
		{
			int num = UnityEngine.Random.Range(0, this.nearSounds.Length);
			if (this.audioChild != null)
			{
				this.audioChild.GetComponent<AudioSource>().PlayOneShot(this.nearSounds[num]);
			}
			else
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.nearSounds[num]);
			}
			this.timer = this.nearSounds[num].length + 1f;
		}
		else
		{
			int num = UnityEngine.Random.Range(0, this.farSounds.Length);
			if (this.audioChild != null)
			{
				this.audioChild.GetComponent<AudioSource>().PlayOneShot(this.farSounds[num]);
			}
			else
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.farSounds[num]);
			}
			this.timer = this.farSounds[num].length + 1f;
		}
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00095608 File Offset: 0x00093808
	public void Hit(Hashtable hitInfo)
	{
		this.health -= (float)hitInfo["D"];
		if (this.health < 0f)
		{
			this.health = 0f;
		}
	}

	// Token: 0x040015A4 RID: 5540
	public GameObject smoke;

	// Token: 0x040015A5 RID: 5541
	public GameObject explosionEmitter;

	// Token: 0x040015A6 RID: 5542
	public float explosionTime = 3f;

	// Token: 0x040015A7 RID: 5543
	public float explosionRadius;

	// Token: 0x040015A8 RID: 5544
	public float power = 3200f;

	// Token: 0x040015A9 RID: 5545
	private float timer;

	// Token: 0x040015AA RID: 5546
	private bool exploded;

	// Token: 0x040015AB RID: 5547
	public AudioClip[] nearSounds;

	// Token: 0x040015AC RID: 5548
	public AudioClip[] farSounds;

	// Token: 0x040015AD RID: 5549
	public float farSoundDistance = 25f;

	// Token: 0x040015AE RID: 5550
	public QuadGameCamera soldierCamera;

	// Token: 0x040015AF RID: 5551
	public float health = 100f;

	// Token: 0x040015B0 RID: 5552
	public bool ragdollizeChildren = true;

	// Token: 0x040015B1 RID: 5553
	public Transform audioChild;

	// Token: 0x040015B2 RID: 5554
	public bool DestroyAfterExplosion = true;
}
