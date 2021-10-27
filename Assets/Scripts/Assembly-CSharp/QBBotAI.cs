using System;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class QBBotAI : MonoBehaviour
{
	// Token: 0x06000B68 RID: 2920 RVA: 0x0008FA18 File Offset: 0x0008DC18
	private void Start()
	{
		if (this.playerTransform == null)
		{
			this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		}
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
		if (this.weapon != null)
		{
			this.weapon.damageAmount = 2f;
		}
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0008FB6C File Offset: 0x0008DD6C
	private void SetupAdditiveAiming(string anim)
	{
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 1f;
		base.GetComponent<Animation>()[anim].layer = 4;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0008FBF4 File Offset: 0x0008DDF4
	private void Update()
	{
		Vector3 vector = base.transform.forward;
		if (this.playerTransform != null)
		{
			vector = this.playerTransform.position - base.transform.position;
			vector.y = 0f;
		}
		Vector3 forward = base.transform.forward;
		forward.y = 0f;
		float num = Vector3.Angle(forward, vector);
		if (Vector3.Cross(forward, vector).y < 0f)
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
		if (this.playerTransform != null && Vector3.Distance(base.transform.position, this.playerTransform.position) < this.weapon.fireRange)
		{
			this.weapon.fire = true;
		}
		else
		{
			this.weapon.fire = false;
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x000900A4 File Offset: 0x0008E2A4
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

	// Token: 0x040014B5 RID: 5301
	public QuadBikeGun weapon;

	// Token: 0x040014B6 RID: 5302
	public Transform spineBone;

	// Token: 0x040014B7 RID: 5303
	public Transform playerTransform;

	// Token: 0x040014B8 RID: 5304
	private Camera cam;

	// Token: 0x040014B9 RID: 5305
	private bool aiming;

	// Token: 0x040014BA RID: 5306
	private bool aim;

	// Token: 0x040014BB RID: 5307
	private bool aimPressed;
}
