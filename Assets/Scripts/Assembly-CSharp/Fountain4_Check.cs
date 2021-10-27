using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class Fountain4_Check : MonoBehaviour
{
	// Token: 0x0600076E RID: 1902 RVA: 0x0003BE34 File Offset: 0x0003A034
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		this.basicAgility = this.player.GetComponent<BasicAgility>();
		this.weaponHandling = this.player.GetComponent<WeaponHandling>();
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0003BE84 File Offset: 0x0003A084
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !Interaction.playerInteraction.coverShortMode)
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0003BED4 File Offset: 0x0003A0D4
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0003BF14 File Offset: 0x0003A114
	private void Update()
	{
		if (this.unpocket)
		{
			if (this.unpocketingTimer < 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				this.fountainModel4.transform.position = this.pieceCorrectPosition;
				this.fountainModel4.transform.rotation = this.pieceCorrectRotation;
				this.fountainModel4.transform.parent = this.previousParent;
				base.Invoke("PlayFountain", 1f);
				base.Invoke("EnablePlayer", 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length);
				this.unpocket = false;
			}
			else if (this.unpocketingTimer < 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				if (!this.peiceInHand)
				{
					Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
					this.pieceCorrectPosition = this.fountainModel4.transform.position;
					this.pieceCorrectRotation = this.fountainModel4.transform.rotation;
					this.fountainModel4.transform.position = transform.transform.position;
					this.fountainModel4.transform.rotation = transform.transform.rotation;
					this.fountainModel4.transform.Translate(-0.1947645f, 0.09594876f, -0.01025693f);
					this.fountainModel4.transform.Rotate(51.65575f, -82.23001f, -80.13065f, Space.Self);
					this.previousParent = this.fountainModel4.transform.parent;
					this.fountainModel4.transform.parent = transform;
					this.fountainModel4.GetComponent<Renderer>().enabled = true;
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

	// Token: 0x06000772 RID: 1906 RVA: 0x0003C574 File Offset: 0x0003A774
	private void PlayFountain()
	{
		//this.fountainStream4.GetComponent<ParticleEmitter>().emit = true;
		//this.fountainModel4.GetComponent<Renderer>().enabled = true;
		//base.Invoke("Splash", 2.5f);
		this.fountain.fountainPieces--;
		this.fountain.fountainSentence++;
		this.fountainAudio.Play();
		iTween.MoveBy(this.waterLevel, iTween.Hash(new object[]
		{
			"y",
			0.2,
			"time",
			7,
			"delay",
			3
		}));
		
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0003C638 File Offset: 0x0003A838
	private void Splash()
	{
		//this.fountainSplash4.GetComponent<ParticleEmitter>().emit = true;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0003C64C File Offset: 0x0003A84C
	private void EnablePlayer()
	{
		this.ncm.disableMovement = false;
		this.ncm.disableRotation = false;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0003C668 File Offset: 0x0003A868
	private void OnGUI()
	{
		if (this.inside && !this.unpocketed)
		{
			Instructions.instruction = Instructions.Instruction.INTERACT;
		}
	}

	// Token: 0x04000980 RID: 2432
	public GameObject fountainStream4;

	// Token: 0x04000981 RID: 2433
	public GameObject fountainSplash4;

	// Token: 0x04000982 RID: 2434
	public GameObject fountainModel4;

	// Token: 0x04000983 RID: 2435
	public AudioSource fountainAudio;

	// Token: 0x04000984 RID: 2436
	public GameObject waterLevel;

	// Token: 0x04000985 RID: 2437
	public Inventory fountain;

	// Token: 0x04000986 RID: 2438
	public Transform startingPosition;

	// Token: 0x04000987 RID: 2439
	private bool inside;

	// Token: 0x04000988 RID: 2440
	private GameObject player;

	// Token: 0x04000989 RID: 2441
	private Vector3 correctStartPosition;

	// Token: 0x0400098A RID: 2442
	private Quaternion correctStartRotation;

	// Token: 0x0400098B RID: 2443
	private bool unpocket;

	// Token: 0x0400098C RID: 2444
	private float unpocketingTimer;

	// Token: 0x0400098D RID: 2445
	private bool unpocketed;

	// Token: 0x0400098E RID: 2446
	private bool peiceInHand;

	// Token: 0x0400098F RID: 2447
	private Transform previousParent;

	// Token: 0x04000990 RID: 2448
	private Vector3 pieceCorrectPosition;

	// Token: 0x04000991 RID: 2449
	private Quaternion pieceCorrectRotation;

	// Token: 0x04000992 RID: 2450
	private NormalCharacterMotor ncm;

	// Token: 0x04000993 RID: 2451
	private BasicAgility basicAgility;

	// Token: 0x04000994 RID: 2452
	private WeaponHandling weaponHandling;
}
