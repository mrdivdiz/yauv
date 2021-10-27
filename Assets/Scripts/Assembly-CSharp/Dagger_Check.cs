using System;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class Dagger_Check : MonoBehaviour
{
	// Token: 0x060006FC RID: 1788 RVA: 0x00038158 File Offset: 0x00036358
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		this.inventory = this.player.GetComponent<Inventory>();
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00038198 File Offset: 0x00036398
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000381D8 File Offset: 0x000363D8
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00038218 File Offset: 0x00036418
	private void Update()
	{
		if (this.unpocket)
		{
			if (this.unpocketingTimer < 0.267f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length)
			{
				base.transform.parent.position = this.pieceCorrectPosition;
				base.transform.parent.rotation = this.pieceCorrectRotation;
				base.transform.parent.parent = this.previousParent;
				base.Invoke("PlayFountain", 1f);
				this.unpocket = false;
			}
			else if (this.unpocketingTimer < 0.8f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length)
			{
				if (!this.peiceInHand)
				{
					Transform transform = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand");
					this.pieceCorrectPosition = base.transform.parent.position;
					this.pieceCorrectRotation = base.transform.parent.transform.rotation;
					base.transform.parent.position = transform.transform.position;
					base.transform.parent.rotation = transform.transform.rotation;
					base.transform.parent.Translate(-0.1971295f, 0.09540137f, -0.221573f);
					base.transform.parent.Rotate(96.81954f, 224.2404f, 288.2571f, Space.Self);
					this.previousParent = base.transform.parent.parent;
					base.transform.parent.parent = transform;
					base.transform.parent.GetComponent<Renderer>().enabled = true;
					this.peiceInHand = true;
				}
			}
			else
			{
				if (Mathf.Abs(this.player.transform.rotation.eulerAngles.y - this.correctStartRotation.eulerAngles.y) < 180f)
				{
					this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y, this.correctStartRotation.eulerAngles.y, -1f * (this.unpocketingTimer - 0.8f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length - 0.2f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length) / (0.2f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length)), this.player.transform.rotation.eulerAngles.z);
				}
				else
				{
					this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y - 360f, this.correctStartRotation.eulerAngles.y, -1f * (this.unpocketingTimer - 0.8f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length - 0.2f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length) / (0.2f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length)), this.player.transform.rotation.eulerAngles.z);
				}
				this.player.transform.position = Vector3.Lerp(this.player.transform.position, new Vector3(this.correctStartPosition.x, this.correctStartPosition.y, this.correctStartPosition.z), -1f * (this.unpocketingTimer - 0.8f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length - 0.2f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length) / (0.2f * this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length));
			}
			this.unpocketingTimer -= Time.deltaTime;
		}
		bool flag = InputManager.GetButton("Interaction");
		flag |= MobileInput.interaction;
		if (this.inside && !this.unpocketed && flag && this.inventory.daggers > 0)
		{
			this.correctStartPosition = this.startingPosition.position;
			this.correctStartRotation = this.startingPosition.rotation;
			this.unpocket = true;
			this.unpocketingTimer = this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length;
			base.Invoke("PlaySound", this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].length / 2f);
			this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].wrapMode = WrapMode.Once;
			this.player.GetComponent<Animation>()["Interaction-Put-Dagger"].layer = 100;
			this.player.GetComponent<Animation>().CrossFade("Interaction-Put-Dagger");
			this.ncm.disableMovement = true;
			this.ncm.disableRotation = true;
			this.inside = false;
			this.unpocketed = true;
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0003883C File Offset: 0x00036A3C
	private void PlaySound()
	{
		if (this.PickupSound != null)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.PickupSound, SpeechManager.sfxVolume);
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00038868 File Offset: 0x00036A68
	private void PlayFountain()
	{
		Dagger_Check.daggersInplace++;
		this.inventory.daggers--;
		if (this.secondDaggerCheck != null)
		{
			this.secondDaggerCheck.enabled = true;
		}
		if (Dagger_Check.daggersInplace == 2)
		{
			iTween.MoveBy(this.leftGate, iTween.Hash(new object[]
			{
				"x",
				-2.2f,
				"time",
				6f,
				"delay",
				1f,
				"easetype",
				iTween.EaseType.linear
			}));
			iTween.MoveBy(this.rightGate, iTween.Hash(new object[]
			{
				"x",
				2.2f,
				"time",
				6f,
				"delay",
				1f,
				"easetype",
				iTween.EaseType.linear
			}));
			if (this.openGateSound != null && base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.openGateSound, SpeechManager.sfxVolume);
			}
		}
		this.ncm.disableMovement = false;
		this.ncm.disableRotation = false;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x000389DC File Offset: 0x00036BDC
	private void OnGUI()
	{
		if (this.inside && !this.unpocketed)
		{
			Instructions.instruction = Instructions.Instruction.INTERACT;
		}
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x000389FC File Offset: 0x00036BFC
	public void OnCheckpointLoad(int checkpointReached)
	{
		Dagger_Check.daggersInplace = 0;
	}

	// Token: 0x040008CD RID: 2253
	public Transform startingPosition;

	// Token: 0x040008CE RID: 2254
	public Dagger_Check secondDaggerCheck;

	// Token: 0x040008CF RID: 2255
	public GameObject leftGate;

	// Token: 0x040008D0 RID: 2256
	public GameObject rightGate;

	// Token: 0x040008D1 RID: 2257
	private Inventory inventory;

	// Token: 0x040008D2 RID: 2258
	private bool inside;

	// Token: 0x040008D3 RID: 2259
	private GameObject player;

	// Token: 0x040008D4 RID: 2260
	private Vector3 correctStartPosition;

	// Token: 0x040008D5 RID: 2261
	private Quaternion correctStartRotation;

	// Token: 0x040008D6 RID: 2262
	private bool unpocket;

	// Token: 0x040008D7 RID: 2263
	private float unpocketingTimer;

	// Token: 0x040008D8 RID: 2264
	private bool unpocketed;

	// Token: 0x040008D9 RID: 2265
	private bool peiceInHand;

	// Token: 0x040008DA RID: 2266
	private Transform previousParent;

	// Token: 0x040008DB RID: 2267
	private Vector3 pieceCorrectPosition;

	// Token: 0x040008DC RID: 2268
	private Quaternion pieceCorrectRotation;

	// Token: 0x040008DD RID: 2269
	private NormalCharacterMotor ncm;

	// Token: 0x040008DE RID: 2270
	private static int daggersInplace;

	// Token: 0x040008DF RID: 2271
	public AudioClip openGateSound;

	// Token: 0x040008E0 RID: 2272
	public AudioClip PickupSound;
}
