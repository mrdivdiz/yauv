using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class Fountain2_Check : MonoBehaviour
{
	// Token: 0x0600075C RID: 1884 RVA: 0x0003AD7C File Offset: 0x00038F7C
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		this.basicAgility = this.player.GetComponent<BasicAgility>();
		this.weaponHandling = this.player.GetComponent<WeaponHandling>();
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0003ADCC File Offset: 0x00038FCC
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !Interaction.playerInteraction.coverShortMode)
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0003AE1C File Offset: 0x0003901C
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0003AE5C File Offset: 0x0003905C
	private void Update()
	{
		if (this.unpocket)
		{
			if (this.unpocketingTimer < 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				this.fountainModel2.transform.position = this.pieceCorrectPosition;
				this.fountainModel2.transform.rotation = this.pieceCorrectRotation;
				this.fountainModel2.transform.parent = this.previousParent;
				base.Invoke("PlayFountain", 1f);
				base.Invoke("EnablePlayer", 0.393f * this.player.GetComponent<Animation>()["Interaction-Piece"].length);
				this.unpocket = false;
			}
			else if (this.unpocketingTimer < 0.636f * this.player.GetComponent<Animation>()["Interaction-Piece"].length)
			{
				if (!this.peiceInHand)
				{
					Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
					this.pieceCorrectPosition = this.fountainModel2.transform.position;
					this.pieceCorrectRotation = this.fountainModel2.transform.rotation;
					this.fountainModel2.transform.position = transform.transform.position;
					this.fountainModel2.transform.rotation = transform.transform.rotation;
					this.fountainModel2.transform.Translate(-0.1947645f, 0.09594876f, -0.01025693f);
					this.fountainModel2.transform.Rotate(51.65575f, -82.23001f, -80.13065f, Space.Self);
					this.previousParent = this.fountainModel2.transform.parent;
					this.fountainModel2.transform.parent = transform;
					this.fountainModel2.GetComponent<Renderer>().enabled = true;
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

	// Token: 0x06000760 RID: 1888 RVA: 0x0003B4BC File Offset: 0x000396BC
	private void PlayFountain()
	{
		//this.fountainStream2.GetComponent<ParticleEmitter>().emit = true;
		//base.Invoke("Splash", 2.5f);
		this.fountainAudio.Play();
		this.fountainModel2.GetComponent<Renderer>().enabled = true;
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

	// Token: 0x06000761 RID: 1889 RVA: 0x0003B580 File Offset: 0x00039780
	private void Splash()
	{
		//this.fountainSplash2.GetComponent<ParticleEmitter>().emit = true;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0003B594 File Offset: 0x00039794
	private void EnablePlayer()
	{
		this.ncm.disableMovement = false;
		this.ncm.disableRotation = false;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0003B5B0 File Offset: 0x000397B0
	private void OnGUI()
	{
		if (this.inside && !this.unpocketed)
		{
			Instructions.instruction = Instructions.Instruction.INTERACT;
		}
	}

	// Token: 0x04000956 RID: 2390
	public GameObject fountainStream2;

	// Token: 0x04000957 RID: 2391
	public GameObject fountainSplash2;

	// Token: 0x04000958 RID: 2392
	public GameObject fountainModel2;

	// Token: 0x04000959 RID: 2393
	public GameObject waterLevel;

	// Token: 0x0400095A RID: 2394
	public Inventory fountain;

	// Token: 0x0400095B RID: 2395
	public Transform startingPosition;

	// Token: 0x0400095C RID: 2396
	public AudioSource fountainAudio;

	// Token: 0x0400095D RID: 2397
	private bool inside;

	// Token: 0x0400095E RID: 2398
	private GameObject player;

	// Token: 0x0400095F RID: 2399
	private Vector3 correctStartPosition;

	// Token: 0x04000960 RID: 2400
	private Quaternion correctStartRotation;

	// Token: 0x04000961 RID: 2401
	private bool unpocket;

	// Token: 0x04000962 RID: 2402
	private float unpocketingTimer;

	// Token: 0x04000963 RID: 2403
	private bool unpocketed;

	// Token: 0x04000964 RID: 2404
	private bool peiceInHand;

	// Token: 0x04000965 RID: 2405
	private Transform previousParent;

	// Token: 0x04000966 RID: 2406
	private Vector3 pieceCorrectPosition;

	// Token: 0x04000967 RID: 2407
	private Quaternion pieceCorrectRotation;

	// Token: 0x04000968 RID: 2408
	private NormalCharacterMotor ncm;

	// Token: 0x04000969 RID: 2409
	private BasicAgility basicAgility;

	// Token: 0x0400096A RID: 2410
	private WeaponHandling weaponHandling;
}
