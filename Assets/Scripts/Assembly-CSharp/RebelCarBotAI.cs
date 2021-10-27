using System;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class RebelCarBotAI : MonoBehaviour
{
	// Token: 0x06000B7B RID: 2939 RVA: 0x000913B8 File Offset: 0x0008F5B8
	private void Start()
	{
		if (this.playerTransform == null && GameObject.FindGameObjectWithTag("Player") != null)
		{
			this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (this.weapon != null)
		{
			this.weapon.damageAmount = 2f;
		}
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00091424 File Offset: 0x0008F624
	private void SetupAdditiveAiming(string anim)
	{
		base.GetComponent<Animation>()[anim].blendMode = AnimationBlendMode.Additive;
		base.GetComponent<Animation>()[anim].enabled = true;
		base.GetComponent<Animation>()[anim].weight = 1f;
		base.GetComponent<Animation>()[anim].layer = 4;
		base.GetComponent<Animation>()[anim].time = 0f;
		base.GetComponent<Animation>()[anim].speed = 0f;
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x000914AC File Offset: 0x0008F6AC
	private void Update()
	{
		if (this.playerTransform != null)
		{
			Vector3 position = this.playerTransform.position;
			position.y = base.transform.position.y;
			base.transform.LookAt(position);
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

	// Token: 0x06000B7E RID: 2942 RVA: 0x00091558 File Offset: 0x0008F758
	public void PlayReloadAnim()
	{
	}

	// Token: 0x040014FA RID: 5370
	public QuadBikeGun weapon;

	// Token: 0x040014FB RID: 5371
	public Transform spineBone;

	// Token: 0x040014FC RID: 5372
	public Transform playerTransform;

	// Token: 0x040014FD RID: 5373
	public QuadBikeMovement movementScript;

	// Token: 0x040014FE RID: 5374
	private Camera cam;

	// Token: 0x040014FF RID: 5375
	private bool aiming;

	// Token: 0x04001500 RID: 5376
	private bool aim;

	// Token: 0x04001501 RID: 5377
	private bool aimPressed;
}
