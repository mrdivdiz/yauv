using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class QBHealth : MonoBehaviour
{
	// Token: 0x06000BA1 RID: 2977 RVA: 0x00092A9C File Offset: 0x00090C9C
	private void Start()
	{
		switch (DifficultyManager.difficulty)
		{
		case DifficultyManager.Difficulty.EASY:
			if (base.gameObject.tag == "Enemy")
			{
				this.health *= DifficultyManager.easyQuad;
			}
			else if (base.gameObject.tag == "Player")
			{
				this.health /= DifficultyManager.easyQuad;
			}
			break;
		case DifficultyManager.Difficulty.HARD:
			if (base.gameObject.tag == "Enemy")
			{
				this.health *= DifficultyManager.hardQuad;
			}
			else if (base.gameObject.tag == "Player")
			{
				this.health /= DifficultyManager.hardQuad;
			}
			break;
		}
		this.maxHealth = this.health;
		Screen.lockCursor = true;
		this.cam = Camera.main.gameObject.GetComponent<QuadGameCamera>();
		this.botAI = base.gameObject.GetComponent<BotAI>();
		this.cameraTransform = Camera.main.transform;
		this.currentHitAnimation = UnityEngine.Random.Range(0, 4);
		if (base.gameObject.tag == "Enemy")
		{
			if (GameObject.FindGameObjectWithTag("Player") != null)
			{
				this.player = GameObject.FindGameObjectWithTag("Player").transform;
			}
			if (this.player != null)
			{
				this.playerHealth = this.player.gameObject.GetComponent<Health>();
			}
		}
		else if (base.gameObject.tag == "Player")
		{
			this.animHandler = base.gameObject.GetComponent<AnimationHandler>();
		}
		this.cam.hitAlpha = 0f;
		this.cam.blackAlpha = 0f;
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00092C9C File Offset: 0x00090E9C
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.activateDiyingTimer)
		{
			this.diyingTimer -= Time.deltaTime;
			if (this.diyingTimer <= 1f)
			{
				Vector3 camOffset = this.cam.camOffset;
				Vector3 to = new Vector3(0f, 0f, -4.5f);
				if (AnimationHandler.instance.insuredMode)
				{
					to = new Vector3(0.7f, 0.3f, -1.5f);
				}
				float num = 2f;
				for (float num2 = 0f; num2 < num; num2 += Time.deltaTime)
				{
					this.cam.camOffset = Vector3.Lerp(camOffset, to, num2 / num);
				}
				NormalCharacterMotor component = this.player.gameObject.GetComponent<NormalCharacterMotor>();
				if (component != null)
				{
					component.disableMovement = false;
					component.disableRotation = false;
				}
				AnimationHandler.instance.faceState = AnimationHandler.FaceState.NEUTRAL;
				this.activateDiyingTimer = false;
				this.Die();
				if (this.botAI != null && this.botAI.weapon != null)
				{
					UnityEngine.Object.Instantiate(this.botAI.weapon.pickablePrefab, this.botAI.weapon.weaponTransform.position, this.botAI.weapon.weaponTransform.rotation);
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
				if (this.botAI != null && this.botAI.weapon != null)
				{
					UnityEngine.Object.Instantiate(this.botAI.weapon.pickablePrefab, this.botAI.weapon.weaponTransform.position, this.botAI.weapon.weaponTransform.rotation);
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
		if (base.gameObject.tag == "Player")
		{
			if (base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().clip = this.heartBeatSound;
			}
			if (this.health < 50f)
			{
				if (!base.GetComponent<AudioSource>().isPlaying && base.GetComponent<AudioSource>().enabled)
				{
					base.GetComponent<AudioSource>().Play();
				}
			}
			else if (base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().Stop();
			}
		}
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			this.health = 0f;
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

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00093150 File Offset: 0x00091350
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

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00093188 File Offset: 0x00091388
	public void IncreaseHealth(float amount)
	{
		this.health += amount;
		if (this.health > this.maxHealth)
		{
			this.health = this.maxHealth;
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x000931B8 File Offset: 0x000913B8
	public void Hit(Hashtable hitInfo)
	{
		if (hitInfo.ContainsKey("E"))
		{
			this.explosionPower = (float)hitInfo["E"];
			this.explosionPosition = (Vector3)hitInfo["EP"];
			this.explosionRadius = (float)hitInfo["ER"];
		}
		else
		{
			this.explosionPower = 0f;
		}
		if (!(base.gameObject.tag != "Player") || hitInfo.ContainsKey("H"))
		{
		}
		if (base.gameObject.tag == "Player" && hitInfo.ContainsKey("source"))
		{
			Vector3 a = (Vector3)hitInfo["source"];
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
		}
		if (this.unlimitedHealth)
		{
			return;
		}
		this.health -= (float)hitInfo["D"];
		if (this.health < 0f)
		{
			this.health = 0f;
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0009344C File Offset: 0x0009164C
	private void Die()
	{
		if (this.RagdollPrefab)
		{
			this.dead = (Transform)UnityEngine.Object.Instantiate(this.RagdollPrefab, base.transform.position, base.transform.rotation);
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
				if (component != null)
				{
					velocity = component.velocity;
				}
			}
			this.CopyTransformsRecurse(base.transform, this.dead, velocity);
			base.gameObject.SetActive(false);
			if (base.gameObject.tag == "Player")
			{
				this.cam.pivotOffset = new Vector3(0f, 1.6f, 0f);
				this.cam.camOffset = new Vector3(0f, 0f, -4.5f);
				this.cam.closeOffset = new Vector3(0.35f, 1.7f, 0f);
				this.cam.player = this.dead.Find("Bip01");
				this.cam.shakingCamera = false;
				this.cam.reticle = null;
			}
		}
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x000935EC File Offset: 0x000917EC
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

	// Token: 0x06000BA8 RID: 2984 RVA: 0x000936DC File Offset: 0x000918DC
	private void OnGUI()
	{
		if (base.gameObject.tag != "Player" || mainmenu.pause)
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

	// Token: 0x04001529 RID: 5417
	public float health = 100f;

	// Token: 0x0400152A RID: 5418
	public float healTime = 40f;

	// Token: 0x0400152B RID: 5419
	public Transform RagdollPrefab;

	// Token: 0x0400152C RID: 5420
	public bool unlimitedHealth;

	// Token: 0x0400152D RID: 5421
	public Texture2D northHitTexture;

	// Token: 0x0400152E RID: 5422
	public Texture2D northWestTexture;

	// Token: 0x0400152F RID: 5423
	public Texture2D westTexture;

	// Token: 0x04001530 RID: 5424
	public Texture2D southWestTexture;

	// Token: 0x04001531 RID: 5425
	public Texture2D southTexture;

	// Token: 0x04001532 RID: 5426
	public Texture2D northEastTexture;

	// Token: 0x04001533 RID: 5427
	public Texture2D eastTexture;

	// Token: 0x04001534 RID: 5428
	public Texture2D southEastTexture;

	// Token: 0x04001535 RID: 5429
	public AudioClip heartBeatSound;

	// Token: 0x04001536 RID: 5430
	public AudioClip[] bodyHitSounds;

	// Token: 0x04001537 RID: 5431
	public AudioClip[] headHitSounds;

	// Token: 0x04001538 RID: 5432
	public AudioClip[] leftArmHitSounds;

	// Token: 0x04001539 RID: 5433
	public AudioClip[] rightArmHitSounds;

	// Token: 0x0400153A RID: 5434
	public AudioClip[] leftLegHitSounds;

	// Token: 0x0400153B RID: 5435
	public AudioClip[] rightLegHitSounds;

	// Token: 0x0400153C RID: 5436
	private Transform dead;

	// Token: 0x0400153D RID: 5437
	private QuadGameCamera cam;

	// Token: 0x0400153E RID: 5438
	private int currentHitAnimation;

	// Token: 0x0400153F RID: 5439
	private Transform player;

	// Token: 0x04001540 RID: 5440
	private float diyingTimer;

	// Token: 0x04001541 RID: 5441
	private bool activateDiyingTimer;

	// Token: 0x04001542 RID: 5442
	private BotAI botAI;

	// Token: 0x04001543 RID: 5443
	private float hitDisplayTime = 0.5f;

	// Token: 0x04001544 RID: 5444
	private float NorthHit;

	// Token: 0x04001545 RID: 5445
	private float NorthEastHit;

	// Token: 0x04001546 RID: 5446
	private float EastHit;

	// Token: 0x04001547 RID: 5447
	private float SouthEastHit;

	// Token: 0x04001548 RID: 5448
	private float SouthHit;

	// Token: 0x04001549 RID: 5449
	private float NorthWestHit;

	// Token: 0x0400154A RID: 5450
	private float WestHit;

	// Token: 0x0400154B RID: 5451
	private float SouthWestHit;

	// Token: 0x0400154C RID: 5452
	private Transform cameraTransform;

	// Token: 0x0400154D RID: 5453
	private bool attack;

	// Token: 0x0400154E RID: 5454
	private bool punched;

	// Token: 0x0400154F RID: 5455
	private bool punchNeedsDisabling;

	// Token: 0x04001550 RID: 5456
	public AudioClip[] FarisSoundsforHeadHit;

	// Token: 0x04001551 RID: 5457
	private static int currentHeadHitFarisSound;

	// Token: 0x04001552 RID: 5458
	private Health playerHealth;

	// Token: 0x04001553 RID: 5459
	private AnimationHandler animHandler;

	// Token: 0x04001554 RID: 5460
	private float explosionPower;

	// Token: 0x04001555 RID: 5461
	private Vector3 explosionPosition;

	// Token: 0x04001556 RID: 5462
	private float explosionRadius;

	// Token: 0x04001557 RID: 5463
	private float maxHealth;
}
