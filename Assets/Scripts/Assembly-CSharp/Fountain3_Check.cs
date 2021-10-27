using System;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class Fountain3_Check : MonoBehaviour
{
	// Token: 0x06000765 RID: 1893 RVA: 0x0003B5D8 File Offset: 0x000397D8
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		this.basicAgility = this.player.GetComponent<BasicAgility>();
		this.weaponHandling = this.player.GetComponent<WeaponHandling>();
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0003B628 File Offset: 0x00039828
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !Interaction.playerInteraction.coverShortMode)
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0003B678 File Offset: 0x00039878
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0003B6B8 File Offset: 0x000398B8
	private void Update()
	{
		if (this.unpocket)
		{
			if (this.unpocketingTimer < 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				this.fountainModel3.transform.position = this.pieceCorrectPosition;
				this.fountainModel3.transform.rotation = this.pieceCorrectRotation;
				this.fountainModel3.transform.parent = this.previousParent;
				base.Invoke("PlayFountain", 1f);
				base.Invoke("EnablePlayer", 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length);
				this.unpocket = false;
			}
			else if (this.unpocketingTimer < 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				if (!this.peiceInHand)
				{
					Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
					this.pieceCorrectPosition = this.fountainModel3.transform.position;
					this.pieceCorrectRotation = this.fountainModel3.transform.rotation;
					this.fountainModel3.transform.position = transform.transform.position;
					this.fountainModel3.transform.rotation = transform.transform.rotation;
					this.fountainModel3.transform.Translate(-0.1947645f, 0.09594876f, -0.01025693f);
					this.fountainModel3.transform.Rotate(51.65575f, -82.23001f, -80.13065f, Space.Self);
					this.previousParent = this.fountainModel3.transform.parent;
					this.fountainModel3.transform.parent = transform;
					this.fountainModel3.GetComponent<Renderer>().enabled = true;
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

	// Token: 0x06000769 RID: 1897 RVA: 0x0003BD18 File Offset: 0x00039F18
	private void PlayFountain()
	{
		//this.fountainStream3.GetComponent<ParticleEmitter>().emit = true;
		//this.fountainModel3.GetComponent<Renderer>().enabled = true;
		//base.Invoke("Splash", 2.5f);
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
		this.fountain.fountainPieces--;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0003BDDC File Offset: 0x00039FDC
	private void Splash()
	{
		//this.fountainSplash3.GetComponent<ParticleEmitter>().emit = true;
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0003BDF0 File Offset: 0x00039FF0
	private void EnablePlayer()
	{
		this.ncm.disableMovement = false;
		this.ncm.disableRotation = false;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x0003BE0C File Offset: 0x0003A00C
	private void OnGUI()
	{
		if (this.inside && !this.unpocketed)
		{
			Instructions.instruction = Instructions.Instruction.INTERACT;
		}
	}

	// Token: 0x0400096B RID: 2411
	public GameObject fountainStream3;

	// Token: 0x0400096C RID: 2412
	public GameObject fountainSplash3;

	// Token: 0x0400096D RID: 2413
	public GameObject fountainModel3;

	// Token: 0x0400096E RID: 2414
	public GameObject waterLevel;

	// Token: 0x0400096F RID: 2415
	public Inventory fountain;

	// Token: 0x04000970 RID: 2416
	public Transform startingPosition;

	// Token: 0x04000971 RID: 2417
	public AudioSource fountainAudio;

	// Token: 0x04000972 RID: 2418
	private bool inside;

	// Token: 0x04000973 RID: 2419
	private GameObject player;

	// Token: 0x04000974 RID: 2420
	private Vector3 correctStartPosition;

	// Token: 0x04000975 RID: 2421
	private Quaternion correctStartRotation;

	// Token: 0x04000976 RID: 2422
	private bool unpocket;

	// Token: 0x04000977 RID: 2423
	private float unpocketingTimer;

	// Token: 0x04000978 RID: 2424
	private bool unpocketed;

	// Token: 0x04000979 RID: 2425
	private bool peiceInHand;

	// Token: 0x0400097A RID: 2426
	private Transform previousParent;

	// Token: 0x0400097B RID: 2427
	private Vector3 pieceCorrectPosition;

	// Token: 0x0400097C RID: 2428
	private Quaternion pieceCorrectRotation;

	// Token: 0x0400097D RID: 2429
	private NormalCharacterMotor ncm;

	// Token: 0x0400097E RID: 2430
	private BasicAgility basicAgility;

	// Token: 0x0400097F RID: 2431
	private WeaponHandling weaponHandling;
}
