using System;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class TorchPickup : MonoBehaviour
{
	// Token: 0x0600087D RID: 2173 RVA: 0x00046304 File Offset: 0x00044504
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.wh = this.player.GetComponent<WeaponHandling>();
		this.ba = this.player.GetComponent<BasicAgility>();
		this.pcc = this.player.GetComponent<PlatformCharacterController>();
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
		if (this.farisRightHand == null)
		{
			this.farisRightHand = this.player.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand");
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00046394 File Offset: 0x00044594
	private void Update()
	{
		if (this.pickedUp)
		{
			TorchThrow.Torch = this.Torch;
			if (!this.cought)
			{
				if (this.pickingTimer < 0.7833f * this.player.GetComponent<Animation>()["Torch"].length)
				{
					if (this.farisRightHand != null && this.Torch != null)
					{
						this.Torch.parent = this.farisRightHand;
						this.Torch.position = this.farisRightHand.position;
						this.Torch.rotation = this.farisRightHand.rotation;
						this.Torch.Translate(-0.1135496f, 0.06459115f, 0.1028337f);
						this.Torch.Rotate(328.2697f, 353.7529f, 352.7576f, Space.Self);
					}
					this.cought = true;
				}
				else
				{
					if (Mathf.Abs(this.player.transform.rotation.eulerAngles.y - base.transform.rotation.eulerAngles.y) < 180f)
					{
						this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.y, -1f * (this.pickingTimer - 0.7833f * this.player.GetComponent<Animation>()["Torch"].length - 0.2167f * this.player.GetComponent<Animation>()["Torch"].length) / (0.2167f * this.player.GetComponent<Animation>()["Torch"].length)), this.player.transform.rotation.eulerAngles.z);
					}
					else
					{
						this.player.transform.rotation = Quaternion.Euler(this.player.transform.rotation.eulerAngles.x, Mathf.Lerp(this.player.transform.rotation.eulerAngles.y - 360f, base.transform.rotation.eulerAngles.y, -1f * (this.pickingTimer - 0.7833f * this.player.GetComponent<Animation>()["Torch"].length - 0.2167f * this.player.GetComponent<Animation>()["Torch"].length) / (0.2167f * this.player.GetComponent<Animation>()["Torch"].length)), this.player.transform.rotation.eulerAngles.z);
					}
					this.player.transform.position = Vector3.Lerp(this.player.transform.position, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z), -1f * (this.pickingTimer - 0.7833f * this.player.GetComponent<Animation>()["Torch"].length - 0.2167f * this.player.GetComponent<Animation>()["Torch"].length) / (0.2167f * this.player.GetComponent<Animation>()["Torch"].length));
					this.pickingTimer -= Time.deltaTime;
				}
			}
			else if (!this.ignited)
			{
				if ((double)this.pickingTimer < 0.5167 * (double)this.player.GetComponent<Animation>()["Torch"].length)
				{
					if (this.farisRightHand != null && this.Torch != null)
					{
						foreach (ParticleEmitter particleEmitter in this.torchEmitters)
						{
							//particleEmitter.enabled = true;
						}
						this.torchLight.enabled = true;
					}
					this.ignited = true;
				}
				else
				{
					this.pickingTimer -= Time.deltaTime;
				}
			}
			else if (!this.holding)
			{
				if (this.pickingTimer <= 0.1f)
				{
					this.player.GetComponent<Animation>().Stop();
					this.player.GetComponent<Animation>().Play("General-Idle");
					this.player.transform.Rotate(0f, 180f, 0f, Space.Self);
					AnimationHandler.instance.holdingTorch = true;
					AnimationHandler.instance.stopIdleAnimations = true;
					this.ncm.disableMovement = false;
					this.ncm.disableRotation = false;
					this.player.GetComponent<Animation>()["General-Torch-Holding"].wrapMode = WrapMode.Loop;
					this.player.GetComponent<Animation>()["General-Torch-Holding"].layer = 5000;
					this.player.GetComponent<Animation>()["General-Torch-Holding"].speed = 1f;
					this.player.GetComponent<Animation>()["General-Torch-Holding"].AddMixingTransform(this.holdingMixingTransforms);
					this.player.GetComponent<Animation>().CrossFade("General-Torch-Holding", 0.3f);
					this.holding = true;
				}
				else
				{
					this.pickingTimer -= Time.deltaTime;
				}
			}
			return;
		}
		this.interactionButton = MobileInput.interaction;
		this.interactionButton |= InputManager.GetButton("Interaction");
		if (this.inside && this.interactionButton && !AnimationHandler.instance.holdingTorch)
		{
			Instructions.instruction = Instructions.Instruction.NONE;
			if (Time.time <= TorchPickup.lastPickTime + 2f)
			{
				return;
			}
			this.wh.disableUsingWeapons = true;
			this.wh.disableAiming = true;
			this.wh.gm.currentGun.enabled = false;
			this.wh.gm.enabled = false;
			this.ba.enabled = false;
			this.ncm.disableMovement = true;
			this.ncm.disableRotation = true;
			this.player.GetComponent<Animation>()["Torch"].wrapMode = WrapMode.Once;
			this.player.GetComponent<Animation>()["Torch"].layer = 5;
			this.player.GetComponent<Animation>()["Torch"].speed = 1f;
			this.player.GetComponent<Animation>().CrossFade("Torch", 0.01f, PlayMode.StopAll);
			this.pickedUp = true;
			this.pickingTimer = this.player.GetComponent<Animation>()["Torch"].length;
			MobileInput.instance.disableButton("interaction", base.gameObject);
			TorchPickup.lastPickTime = Time.time;
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00046B5C File Offset: 0x00044D5C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.pickedUp)
		{
			this.inside = true;
			MobileInput.instance.enableButton("interaction", base.gameObject);
			if (!AnimationHandler.instance.insuredMode)
			{
				Instructions.instruction = Instructions.Instruction.INTERACT;
			}
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00046BBC File Offset: 0x00044DBC
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
			MobileInput.instance.disableButton("interaction", base.gameObject);
			Instructions.instruction = Instructions.Instruction.NONE;
		}
	}

	// Token: 0x04000B48 RID: 2888
	public Transform Torch;

	// Token: 0x04000B49 RID: 2889
	public ParticleEmitter[] torchEmitters;

	// Token: 0x04000B4A RID: 2890
	public Light torchLight;

	// Token: 0x04000B4B RID: 2891
	public Transform holdingMixingTransforms;

	// Token: 0x04000B4C RID: 2892
	public Transform farisRightHand;

	// Token: 0x04000B4D RID: 2893
	private GameObject player;

	// Token: 0x04000B4E RID: 2894
	private bool inside;

	// Token: 0x04000B4F RID: 2895
	private static float lastPickTime;

	// Token: 0x04000B50 RID: 2896
	private bool pickedUp;

	// Token: 0x04000B51 RID: 2897
	private bool interactionButton;

	// Token: 0x04000B52 RID: 2898
	private float pickingTimer;

	// Token: 0x04000B53 RID: 2899
	private bool cought;

	// Token: 0x04000B54 RID: 2900
	private bool ignited;

	// Token: 0x04000B55 RID: 2901
	private bool holding;

	// Token: 0x04000B56 RID: 2902
	private WeaponHandling wh;

	// Token: 0x04000B57 RID: 2903
	private BasicAgility ba;

	// Token: 0x04000B58 RID: 2904
	private PlatformCharacterController pcc;

	// Token: 0x04000B59 RID: 2905
	private NormalCharacterMotor ncm;
}
