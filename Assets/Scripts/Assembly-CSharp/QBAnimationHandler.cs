using System;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class QBAnimationHandler : MonoBehaviour
{
	// Token: 0x06000B98 RID: 2968 RVA: 0x000922EC File Offset: 0x000904EC
	private void Start()
	{
		QBAnimationHandler.Instance = this;
		Screen.lockCursor = true;
		this.cam = Camera.main;
		this.SetupAdditiveAiming("Faris-Bike-Aim-Right");
		this.SetupAdditiveAiming("Faris-Bike-Aim-Left");
		this.SetupAdditiveAiming("Faris-Bike-Aim-Right-Back");
		this.SetupAdditiveAiming("Faris-Bike-Aim-Left-Back");
		base.GetComponent<Animation>()["Faris-Bike-Aim-Back"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Faris-Bike-Aim-Back"].speed = 2f;
		base.GetComponent<Animation>()["Faris-Bike-Aim"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Faris-Bike-Right"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Faris-Bike-Left"].wrapMode = WrapMode.Loop;
		base.transform.GetComponent<Animation>()[this.weapon.reloadAnimation.name].AddMixingTransform(this.spineBone);
		base.transform.GetComponent<Animation>()[this.weapon.reloadAnimation.name].layer = 5;
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00092400 File Offset: 0x00090600
	private void OnDestroy()
	{
		QBAnimationHandler.Instance = null;
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00092408 File Offset: 0x00090608
	private void SetupAdditiveAiming(string anim)
	{
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 1f;
		base.GetComponent<Animation>()[anim].layer = 4;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00092490 File Offset: 0x00090690
	private void Update()
	{
		this.aim =  InputManager.GetButton("Fire2");
		this.aimPressed = false;
		if (this.aim && this.aim != this.previousAim)
		{
			this.aimPressed = true;
		}
		this.previousAim = this.aim;
		if (!this.aiming && this.aimPressed)
		{
			this.EnterAimingMode();
			this.aiming = true;
		}
		if (this.aiming && !this.aim)
		{
			this.ExitAimingMode();
			this.aiming = false;
		}
		Vector3 forward = this.cam.transform.forward;
		forward.y = 0f;
		Vector3 forward2 = base.transform.forward;
		forward2.y = 0f;
		float num = Vector3.Angle(forward2, forward);
		if (Vector3.Cross(forward2, forward).y < 0f)
		{
			num *= -1f;
		}
		if (num >= 0f && num <= 90f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Faris-Bike-Aim"))
			{
				base.GetComponent<Animation>().Play("Faris-Bike-Aim");
			}
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right"].weight = 1f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right"].time = Mathf.Clamp01(num / 90f);
		}
		else if (num > 90f && num <= 150f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Faris-Bike-Right"))
			{
				base.GetComponent<Animation>().Play("Faris-Bike-Right");
			}
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right-Back"].weight = 1f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right-Back"].time = Mathf.Clamp01((num - 90f) / 60f);
		}
		else if (num < 0f && num >= -90f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Faris-Bike-Aim"))
			{
				base.GetComponent<Animation>().Play("Faris-Bike-Aim");
			}
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left"].weight = 1f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left"].time = Mathf.Clamp01(-num / 90f);
		}
		else if (num < -90f && num >= -150f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Faris-Bike-Left"))
			{
				base.GetComponent<Animation>().Play("Faris-Bike-Left");
			}
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left-Back"].weight = 1f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left-Back"].time = Mathf.Clamp01((-num - 90f) / 60f);
		}
		else
		{
			if (!base.GetComponent<Animation>().IsPlaying("Faris-Bike-Aim-Back"))
			{
				base.GetComponent<Animation>().CrossFade("Faris-Bike-Aim-Back");
			}
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Right-Back"].weight = 0f;
			base.GetComponent<Animation>()["Faris-Bike-Aim-Left-Back"].weight = 0f;
		}
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00092994 File Offset: 0x00090B94
	public void PlayReloadAnim()
	{
		AnimationClip reloadAnimation = this.weapon.reloadAnimation;
		if (reloadAnimation != null)
		{
			base.transform.GetComponent<Animation>()[reloadAnimation.name].time = 0f;
			base.transform.GetComponent<Animation>()[reloadAnimation.name].wrapMode = WrapMode.Once;
			base.transform.GetComponent<Animation>()[reloadAnimation.name].speed = 1.5f;
			base.transform.GetComponent<Animation>().Blend(reloadAnimation.name, reloadAnimation.length);
		}
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00092A34 File Offset: 0x00090C34
	public void EnterAimingMode()
	{
		this.cam.GetComponent<QuadGameCamera>().aim = true;
		this.cam.GetComponent<QuadGameCamera>().SelectAimTarget();
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00092A58 File Offset: 0x00090C58
	public void ExitAimingMode()
	{
		this.cam.GetComponent<QuadGameCamera>().aim = false;
	}

	// Token: 0x04001521 RID: 5409
	public QuadBikeGun weapon;

	// Token: 0x04001522 RID: 5410
	public Transform spineBone;

	// Token: 0x04001523 RID: 5411
	private Camera cam;

	// Token: 0x04001524 RID: 5412
	private bool aiming;

	// Token: 0x04001525 RID: 5413
	private bool aim;

	// Token: 0x04001526 RID: 5414
	private bool previousAim;

	// Token: 0x04001527 RID: 5415
	private bool aimPressed;

	// Token: 0x04001528 RID: 5416
	public static QBAnimationHandler Instance;
}
