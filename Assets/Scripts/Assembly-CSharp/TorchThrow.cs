using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class TorchThrow : MonoBehaviour
{
	// Token: 0x06000882 RID: 2178 RVA: 0x00046C08 File Offset: 0x00044E08
	private void Start()
	{
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.wh = this.player.GetComponent<WeaponHandling>();
		this.ba = this.player.GetComponent<BasicAgility>();
		this.pcc = this.player.GetComponent<PlatformCharacterController>();
		this.ncm = this.player.GetComponent<NormalCharacterMotor>();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00046C6C File Offset: 0x00044E6C
	private void Update()
	{
		if (this.Thrown)
		{
			if (!this.unparented)
			{
				if (this.pickingTimer < 0.45f * this.player.GetComponent<Animation>()["Interaction-Drop-Torch"].length)
				{
					TorchThrow.Torch.parent = null;
					this.unparented = true;
				}
				else
				{
					this.pickingTimer -= Time.deltaTime;
				}
			}
			else if (AnimationHandler.instance.holdingTorch)
			{
				if (this.pickingTimer <= 0f)
				{
					this.wh.disableUsingWeapons = false;
					this.wh.disableAiming = false;
					this.wh.gm.currentGun.enabled = true;
					this.wh.gm.enabled = true;
					this.wh.enabled = true;
					this.ba.enabled = true;
					AnimationHandler.instance.holdingTorch = false;
					AnimationHandler.instance.stopIdleAnimations = false;
					this.ncm.disableMovement = false;
					this.ncm.disableRotation = false;
					this.player.GetComponent<Animation>().Stop("General-Torch-Holding");
					TorchThrow.Torch = null;
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					this.pickingTimer -= Time.deltaTime;
				}
			}
			return;
		}
		if (this.inside)
		{
			this.player.GetComponent<Animation>()["Interaction-Drop-Torch"].wrapMode = WrapMode.Once;
			this.player.GetComponent<Animation>()["Interaction-Drop-Torch"].layer = 15000;
			this.player.GetComponent<Animation>()["Interaction-Drop-Torch"].speed = 1f;
			this.player.GetComponent<Animation>().CrossFade("Interaction-Drop-Torch", 0.01f, PlayMode.StopAll);
			this.Thrown = true;
			this.pickingTimer = this.player.GetComponent<Animation>()["Interaction-Drop-Torch"].length;
			this.ncm.disableMovement = true;
			this.ncm.disableRotation = true;
			TorchThrow.lastPickTime = Time.time;
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00046E94 File Offset: 0x00045094
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = true;
		}
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00046EB4 File Offset: 0x000450B4
	private void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.inside = false;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00046ED4 File Offset: 0x000450D4
	public void OnDestroy()
	{
		TorchThrow.Torch = null;
	}

	// Token: 0x04000B5A RID: 2906
	public static Transform Torch;

	// Token: 0x04000B5B RID: 2907
	private GameObject player;

	// Token: 0x04000B5C RID: 2908
	private bool inside;

	// Token: 0x04000B5D RID: 2909
	private static float lastPickTime;

	// Token: 0x04000B5E RID: 2910
	private bool Thrown;

	// Token: 0x04000B5F RID: 2911
	private float pickingTimer;

	// Token: 0x04000B60 RID: 2912
	private WeaponHandling wh;

	// Token: 0x04000B61 RID: 2913
	private BasicAgility ba;

	// Token: 0x04000B62 RID: 2914
	private PlatformCharacterController pcc;

	// Token: 0x04000B63 RID: 2915
	private NormalCharacterMotor ncm;

	// Token: 0x04000B64 RID: 2916
	private bool unparented;
}
