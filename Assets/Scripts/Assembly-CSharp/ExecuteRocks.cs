using System;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class ExecuteRocks : MonoBehaviour
{
	// Token: 0x06000735 RID: 1845 RVA: 0x000392D8 File Offset: 0x000374D8
	private void Awake()
	{
		ExecuteRocks.ee_active = false;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x000392E0 File Offset: 0x000374E0
	private void Start()
	{
		this.wh = AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>();
		this.ba = AnimationHandler.instance.gameObject.GetComponent<BasicAgility>();
		this.sb = new ShuffleBag();
		this.sb.add(0, 1);
		this.sb.add(1, 1);
		this.sb.add(2, 1);
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x00039358 File Offset: 0x00037558
	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
		if (this.triggerType == ExecuteRocks.triggerTypes.ENTER && ExecuteRocks.ee_active && this.timer <= 0f)
		{
			int num = this.sb.next();
			Transform transform = ((GameObject)UnityEngine.Object.Instantiate(this.Boulder, this.Points[num].transform.position, this.Points[num].transform.rotation)).transform;
			transform.gameObject.SetActive(true);
			float num2 = UnityEngine.Random.Range(100f, 200f);
			if (UnityEngine.Random.Range(0, 2) == 1)
			{
				num2 *= -1f;
			}
			transform.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, num2, ForceMode.Acceleration);
			this.twoBouldersCounter--;
			int num3 = this.twoBouldersCounter;
			if (num3 != 0)
			{
				if (num3 == 1)
				{
					transform.localScale = new Vector3(0.8f * transform.localScale.x, 0.8f * transform.localScale.y, 0.8f * transform.localScale.z);
				}
			}
			else
			{
				transform.localScale = new Vector3(0.7f * transform.localScale.x, 0.7f * transform.localScale.y, 0.7f * transform.localScale.z);
			}
			if (this.twoBouldersCounter <= 0)
			{
				do
				{
					num = UnityEngine.Random.Range(0, 3);
				}
				while (num == this.priviousRand);
				this.priviousRand = num;
				transform = ((GameObject)UnityEngine.Object.Instantiate(this.Boulder, this.Points[num].transform.position, this.Points[num].transform.rotation)).transform;
				transform.localScale = new Vector3(0.7f * transform.localScale.x, 0.7f * transform.localScale.y, 0.7f * transform.localScale.z);
				this.twoBouldersCounter = 3;
			}
			this.timer += UnityEngine.Random.Range(this.minTimeBetweenBoulders, this.maxTimeBetweenBoulders);
		}
		if (this.triggerType == ExecuteRocks.triggerTypes.EXIT && this.soundStop)
		{
			if (this.soundStopingTime > 0f)
			{
				this.sound.volume = this.soundStopingTime / 6f;
				this.soundStopingTime -= Time.deltaTime;
			}
			else
			{
				this.sound.Stop();
				this.soundStop = false;
			}
		}
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0003963C File Offset: 0x0003783C
	private void OnTriggerEnter(Collider p)
	{
		if (!this.triggered && p.gameObject.tag == "Player")
		{
			if (this.triggerType == ExecuteRocks.triggerTypes.ENTER)
			{
				PadVibrator.SetForcedVibration(true, 0.6f);
				ExecuteRocks.ee_active = true;
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
				{
					//this.Dust1.emit = ExecuteRocks.ee_active;
					//this.Dust2.emit = ExecuteRocks.ee_active;
				}
				this.sound.Play();
				Camera.main.GetComponent<ShooterGameCamera>().shakingCamera = true;
				Camera.main.GetComponent<ShooterGameCamera>().allowCircleCamera = false;
				this.wh.gm.currentGun.enabled = false;
				this.wh.gm.enabled = false;
				this.wh.enabled = false;
				this.triggered = true;
				if (p.gameObject.GetComponent<CharacterController>() != null)
				{
					p.gameObject.GetComponent<CharacterController>().radius = 0.6f;
				}
			}
			else if (this.triggerType == ExecuteRocks.triggerTypes.EXIT)
			{
				PadVibrator.UnsetForcedVibration();
				ExecuteRocks.ee_active = false;
				if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
				{
					//this.Dust1.emit = ExecuteRocks.ee_active;
					//this.Dust2.emit = ExecuteRocks.ee_active;
				}
				this.soundStopingTime = 6f;
				this.soundStop = true;
				Camera.main.GetComponent<ShooterGameCamera>().shakingCamera = false;
				Camera.main.GetComponent<ShooterGameCamera>().allowCircleCamera = true;
				AnimationHandler.instance.engagedMode = false;
				this.wh.gm.currentGun.enabled = true;
				this.wh.gm.enabled = true;
				this.wh.enabled = true;
				this.ba.rollInsteadOfJump = false;
				this.triggered = true;
				if (p.gameObject.GetComponent<CharacterController>() != null)
				{
					p.gameObject.GetComponent<CharacterController>().radius = 0.3f;
				}
			}
		}
	}

	// Token: 0x0400090E RID: 2318
	public GameObject[] Points;

	// Token: 0x0400090F RID: 2319
	public GameObject Boulder;

	// Token: 0x04000910 RID: 2320
	public AudioSource sound;

	// Token: 0x04000911 RID: 2321
	public ParticleEmitter Dust1;

	// Token: 0x04000912 RID: 2322
	public ParticleEmitter Dust2;

	// Token: 0x04000913 RID: 2323
	[HideInInspector]
	public static bool ee_active;

	// Token: 0x04000914 RID: 2324
	public ExecuteRocks.triggerTypes triggerType;

	// Token: 0x04000915 RID: 2325
	public float minTimeBetweenBoulders = 3f;

	// Token: 0x04000916 RID: 2326
	public float maxTimeBetweenBoulders = 5f;

	// Token: 0x04000917 RID: 2327
	private float timer;

	// Token: 0x04000918 RID: 2328
	private int priviousRand;

	// Token: 0x04000919 RID: 2329
	private int twoBouldersCounter = 3;

	// Token: 0x0400091A RID: 2330
	private WeaponHandling wh;

	// Token: 0x0400091B RID: 2331
	private BasicAgility ba;

	// Token: 0x0400091C RID: 2332
	private bool triggered;

	// Token: 0x0400091D RID: 2333
	private ShuffleBag sb;

	// Token: 0x0400091E RID: 2334
	private float soundStopingTime;

	// Token: 0x0400091F RID: 2335
	private bool soundStop;

	// Token: 0x02000151 RID: 337
	public enum triggerTypes
	{
		// Token: 0x04000921 RID: 2337
		ENTER,
		// Token: 0x04000922 RID: 2338
		EXIT
	}
}
