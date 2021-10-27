using System;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class Fountain1_Check : MonoBehaviour
{
	// Token: 0x06000753 RID: 1875 RVA: 0x0003A4D0 File Offset: 0x000386D0
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		this.basicAgility = this.player.GetComponent<BasicAgility>();
		this.weaponHandling = this.player.GetComponent<WeaponHandling>();
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0003A520 File Offset: 0x00038720
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !Interaction.playerInteraction.coverShortMode)
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0003A570 File Offset: 0x00038770
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0003A5B0 File Offset: 0x000387B0
	private void Update()
	{
		if (this.unpocket)
		{
			if (this.unpocketingTimer < 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				this.fountainModel1.transform.position = this.pieceCorrectPosition;
				this.fountainModel1.transform.rotation = this.pieceCorrectRotation;
				this.fountainModel1.transform.parent = this.previousParent;
				base.Invoke("PlayFountain", 1f);
				base.Invoke("EnablePlayer", 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length);
				this.unpocket = false;
			}
			else if (this.unpocketingTimer < 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				if (!this.peiceInHand)
				{
					Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
					this.pieceCorrectPosition = this.fountainModel1.transform.position;
					this.pieceCorrectRotation = this.fountainModel1.transform.rotation;
					this.fountainModel1.transform.position = transform.transform.position;
					this.fountainModel1.transform.rotation = transform.transform.rotation;
					this.fountainModel1.transform.Translate(-0.1947645f, 0.09594876f, -0.01025693f);
					this.fountainModel1.transform.Rotate(51.65575f, -82.23001f, -80.13065f, Space.Self);
					this.previousParent = this.fountainModel1.transform.parent;
					this.fountainModel1.transform.parent = transform;
					this.fountainModel1.GetComponent<Renderer>().enabled = true;
					this.peiceInHand = true;
				}
			}
			else
			{
				if (Mathf.Abs(this.player.transform.rotation.eulerAngles.y - this.correctStartRotation.eulerAngles.y) < 180f)
				{
					this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y, this.correctStartRotation.eulerAngles.y, -1f * (this.unpocketingTimer - 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length - 0.364f * this.player.GetComponent<Animation>()["Interaction-Piece"].length) / (0.364f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)), this.player.transform.rotation.eulerAngles.z);
				}
				else
				{
					this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y - 360f, this.correctStartRotation.eulerAngles.y, -1f * (this.unpocketingTimer - 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length - 0.364f * this.player.GetComponent<Animation>()["Interaction-Piece"].length) / (0.364f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)), this.player.transform.rotation.eulerAngles.z);
				}
				this.player.transform.position = Vector3.Lerp(this.player.transform.position, new Vector3(this.correctStartPosition.x, this.correctStartPosition.y, this.correctStartPosition.z), -1f * (this.unpocketingTimer - 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length - 0.364f * this.player.GetComponent<Animation>()["Interaction-Piece"].length) / (0.364f * this.player.GetComponent<Animation>()["Interaction-Piece"].length));
			}
			this.unpocketingTimer -= Time.deltaTime;
		}
		bool flag = InputManager.GetButton("Interaction");
		flag |= MobileInput.interaction;
		if (this.inside && !this.unpocketed && flag && !this.weaponHandling.inCover && this.fountain.fountainPieces > 0)
		{
			this.correctStartPosition = this.startingPosition.position;
			this.correctStartRotation = this.startingPosition.rotation;
			this.unpocket = true;
			this.unpocketingTimer = this.player.GetComponent<Animation>()["Interaction-Piece"].length;
			this.player.GetComponent<Animation>()["Interaction-Piece"].wrapMode = WrapMode.Once;
			this.player.GetComponent<Animation>()["Interaction-Piece"].layer = 100;
			this.player.GetComponent<Animation>().CrossFade("Interaction-Piece");
			this.ncm.disableMovement = true;
			this.ncm.disableRotation = true;
			this.basicAgility.animatingTimer = this.basicAgility.gameObject.GetComponent<Animation>()["Interaction-Piece"].length;
			this.basicAgility.animating = true;
			this.inside = false;
			this.unpocketed = true;
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0003AC74 File Offset: 0x00038E74
	private void PlayFountain()
	{
		//this.fountainStream1.GetComponent<ParticleEmitter>().emit = true;
		//base.Invoke("Splash", 2.5f);
		this.fountainAudio.Play();
		this.fountain.fountainSentence++;
		iTween.MoveBy(this.waterLevel, iTween.Hash(new object[]
		{
			"y",
			0.2,
			"time",
			7,
			"delay",
			3
		}));
		this.fountain.fountainPieces--;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0003AD24 File Offset: 0x00038F24
	private void Splash()
	{
		//this.fountainSplash1.GetComponent<ParticleEmitter>().emit = true;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0003AD38 File Offset: 0x00038F38
	private void EnablePlayer()
	{
		this.ncm.disableMovement = false;
		this.ncm.disableRotation = false;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0003AD54 File Offset: 0x00038F54
	private void OnGUI()
	{
		if (this.inside && !this.unpocketed)
		{
			Instructions.instruction = Instructions.Instruction.INTERACT;
		}
	}

	// Token: 0x0400093D RID: 2365
	public GameObject fountainStream1;

	// Token: 0x0400093E RID: 2366
	public GameObject fountainSplash1;

	// Token: 0x0400093F RID: 2367
	public GameObject fountainModel1;

	// Token: 0x04000940 RID: 2368
	public GameObject fountainStream2;

	// Token: 0x04000941 RID: 2369
	public GameObject fountainStream3;

	// Token: 0x04000942 RID: 2370
	public GameObject fountainStream4;

	// Token: 0x04000943 RID: 2371
	public GameObject fountainStream5;

	// Token: 0x04000944 RID: 2372
	public AudioSource fountainAudio;

	// Token: 0x04000945 RID: 2373
	public Transform startingPosition;

	// Token: 0x04000946 RID: 2374
	public GameObject waterLevel;

	// Token: 0x04000947 RID: 2375
	public Inventory fountain;

	// Token: 0x04000948 RID: 2376
	private bool inside;

	// Token: 0x04000949 RID: 2377
	private GameObject player;

	// Token: 0x0400094A RID: 2378
	private Vector3 correctStartPosition;

	// Token: 0x0400094B RID: 2379
	private Quaternion correctStartRotation;

	// Token: 0x0400094C RID: 2380
	private bool unpocket;

	// Token: 0x0400094D RID: 2381
	private float unpocketingTimer;

	// Token: 0x0400094E RID: 2382
	private bool unpocketed;

	// Token: 0x0400094F RID: 2383
	private bool peiceInHand;

	// Token: 0x04000950 RID: 2384
	private Transform previousParent;

	// Token: 0x04000951 RID: 2385
	private Vector3 pieceCorrectPosition;

	// Token: 0x04000952 RID: 2386
	private Quaternion pieceCorrectRotation;

	// Token: 0x04000953 RID: 2387
	private NormalCharacterMotor ncm;

	// Token: 0x04000954 RID: 2388
	private BasicAgility basicAgility;

	// Token: 0x04000955 RID: 2389
	private WeaponHandling weaponHandling;
}
