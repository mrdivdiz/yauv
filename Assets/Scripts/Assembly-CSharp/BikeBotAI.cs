using System;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class BikeBotAI : MonoBehaviour
{
	// Token: 0x06000B8E RID: 2958 RVA: 0x00091AAC File Offset: 0x0008FCAC
	private void Start()
	{
		if (this.playerTransform == null && GameObject.FindGameObjectWithTag("Player") != null)
		{
			this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		}
		Screen.lockCursor = true;
		this.cam = Camera.main;
		this.SetupAdditiveAiming("Bike_Rider_Aim_Right_Additive");
		this.SetupAdditiveAiming("Bike_Rider_Aim_Left_Additive");
		this.SetupAdditiveAiming("Bike_Rider_Aim_Right_Back_Additive");
		this.SetupAdditiveAiming("Bike_Rider_Aim_Left_Back_Additive");
		base.GetComponent<Animation>()["Bike_Rider_Aim_Back"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Bike_Rider_Aim_Back"].speed = 2f;
		base.GetComponent<Animation>()["Bike_Rider_Aim_Forward"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Bike_Rider_Aim_Right"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Bike_Rider_Aim_Left"].wrapMode = WrapMode.Loop;
		base.transform.GetComponent<Animation>()[this.weapon.reloadAnimation.name].AddMixingTransform(this.spineBone);
		base.transform.GetComponent<Animation>()[this.weapon.reloadAnimation.name].layer = 5;
		if (this.weapon != null)
		{
			this.weapon.damageAmount = 2f;
		}
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00091C18 File Offset: 0x0008FE18
	private void SetupAdditiveAiming(string anim)
	{
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 1f;
		base.GetComponent<Animation>()[anim].layer = 4;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x00091CA0 File Offset: 0x0008FEA0
	private void Update()
	{
		if (this.movementScript.turning)
		{
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].weight = 0f;
			this.weapon.fire = false;
			return;
		}
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
			if (!base.GetComponent<Animation>().IsPlaying("Bike_Rider_Aim_Forward"))
			{
				base.GetComponent<Animation>().Play("Bike_Rider_Aim_Forward");
			}
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].weight = 1f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].time = Mathf.Clamp01(num / 90f);
		}
		else if (num > 90f && num <= 150f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Bike_Rider_Aim_Right"))
			{
				base.GetComponent<Animation>().Play("Bike_Rider_Aim_Right");
			}
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].weight = 1f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].time = Mathf.Clamp01((num - 90f) / 60f);
		}
		else if (num < 0f && num >= -90f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Bike_Rider_Aim_Forward"))
			{
				base.GetComponent<Animation>().Play("Bike_Rider_Aim_Forward");
			}
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].weight = 1f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].time = Mathf.Clamp01(-num / 90f);
		}
		else if (num < -90f && num >= -150f)
		{
			if (!base.GetComponent<Animation>().IsPlaying("Bike_Rider_Aim_Left"))
			{
				base.GetComponent<Animation>().Play("Bike_Rider_Aim_Left");
			}
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].weight = 1f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].time = Mathf.Clamp01((-num - 90f) / 60f);
		}
		else
		{
			if (!base.GetComponent<Animation>().IsPlaying("Bike_Rider_Aim_Back"))
			{
				base.GetComponent<Animation>().CrossFade("Bike_Rider_Aim_Back");
			}
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Right_Back_Additive"].weight = 0f;
			base.GetComponent<Animation>()["Bike_Rider_Aim_Left_Back_Additive"].weight = 0f;
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

	// Token: 0x06000B91 RID: 2961 RVA: 0x000921D4 File Offset: 0x000903D4
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

	// Token: 0x04001517 RID: 5399
	public QuadBikeGun weapon;

	// Token: 0x04001518 RID: 5400
	public Transform spineBone;

	// Token: 0x04001519 RID: 5401
	public Transform playerTransform;

	// Token: 0x0400151A RID: 5402
	public QuadBikeMovement movementScript;

	// Token: 0x0400151B RID: 5403
	private Camera cam;

	// Token: 0x0400151C RID: 5404
	private bool aiming;

	// Token: 0x0400151D RID: 5405
	private bool aim;

	// Token: 0x0400151E RID: 5406
	private bool aimPressed;
}
