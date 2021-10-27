using System;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class StearDerexion : MonoBehaviour
{
	// Token: 0x06000675 RID: 1653 RVA: 0x00031CCC File Offset: 0x0002FECC
	private void Start()
	{
		if (this.player != null)
		{
			this.player.GetComponent<Animation>()["Take 001_21"].wrapMode = WrapMode.Loop;
			this.SetupAdditiveAiming("Faris-Driving-Left");
			this.SetupAdditiveAiming("Faris-Driving-Right");
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00031D1C File Offset: 0x0002FF1C
	private void SetupAdditiveAiming(string anim)
	{
		this.player.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		this.player.GetComponent<Animation>()[anim].enabled = true;
		this.player.GetComponent<Animation>()[anim].weight = 1f;
		this.player.GetComponent<Animation>()[anim].layer = 4;
		this.player.GetComponent<Animation>()[anim].time = 0f;
		this.player.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00031DC0 File Offset: 0x0002FFC0
	private void Update()
	{
		float target;
		if (!AndroidPlatform.IsJoystickConnected())
		{
			target = -Mathf.Clamp(Input.acceleration.x, -1f, 1f) * 70f;
		}
		else
		{
			target = -Mathf.Clamp(Input.GetAxisRaw("Horizontal"), -1f, 1f) * 70f;
		}
		this.smoothedStearingAngle = Mathf.SmoothDamp(this.smoothedStearingAngle, target, ref this.currentAngle, 0.1f);
		base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, this.smoothedStearingAngle);
		if (this.player != null)
		{
			if (!this.player.GetComponent<Animation>().IsPlaying("Take 001_21"))
			{
				this.player.GetComponent<Animation>().Play("Take 001_21");
			}
			if (this.smoothedStearingAngle > 0f)
			{
				this.player.GetComponent<Animation>()["Faris-Driving-Left"].weight = 1f;
				this.player.GetComponent<Animation>()["Faris-Driving-Right"].weight = 0f;
				this.player.GetComponent<Animation>()["Faris-Driving-Left"].time = Mathf.Clamp01(this.smoothedStearingAngle / 90f);
			}
			else if (this.smoothedStearingAngle < 0f)
			{
				this.player.GetComponent<Animation>()["Faris-Driving-Left"].weight = 0f;
				this.player.GetComponent<Animation>()["Faris-Driving-Right"].weight = 1f;
				this.player.GetComponent<Animation>()["Faris-Driving-Right"].time = Mathf.Clamp01(-this.smoothedStearingAngle / 90f);
			}
			else
			{
				this.player.GetComponent<Animation>()["Faris-Driving-Left"].weight = 0f;
				this.player.GetComponent<Animation>()["Faris-Driving-Right"].weight = 0f;
			}
		}
	}

	// Token: 0x040007BE RID: 1982
	public Transform player;

	// Token: 0x040007BF RID: 1983
	private float smoothedStearingAngle;

	// Token: 0x040007C0 RID: 1984
	private float currentAngle;
}
