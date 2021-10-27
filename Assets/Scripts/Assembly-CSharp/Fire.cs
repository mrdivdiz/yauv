using System;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class Fire : MonoBehaviour
{
	// Token: 0x06000979 RID: 2425 RVA: 0x00055350 File Offset: 0x00053550
	private void Start()
	{
		if (this.shootingEmitter != null)
		{
			this.shootingEmitter.ChangeState(false, FireMode.SEMI_AUTO);
		}
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00055370 File Offset: 0x00053570
	public void LateUpdate()
	{
		if (this.fired)
		{
			this.fired = false;
			this.shootingEmitter.ChangeState(false, FireMode.SEMI_AUTO);
		}
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00055394 File Offset: 0x00053594
	public void FireWeapon()
	{
		if (this.shootingEmitter != null)
		{
			this.shootingEmitter.ChangeState(true, FireMode.SEMI_AUTO);
			this.fired = true;
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x000553BC File Offset: 0x000535BC
	public void FullAutoFire()
	{
		if (this.shootingEmitter != null)
		{
			this.shootingEmitter.ChangeState(true, FireMode.FULL_AUTO);
		}
		if (this.woodParticle != null)
		{
			this.woodParticle.SetActive(true);
		}
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x000553FC File Offset: 0x000535FC
	public void FullAutoStop()
	{
		if (this.shootingEmitter != null)
		{
			this.shootingEmitter.ChangeState(false, FireMode.FULL_AUTO);
		}
		if (this.woodParticle != null)
		{
			this.woodParticle.SetActive(false);
		}
	}

	// Token: 0x04000DA1 RID: 3489
	public GunParticles shootingEmitter;

	// Token: 0x04000DA2 RID: 3490
	public GameObject woodParticle;

	// Token: 0x04000DA3 RID: 3491
	private bool fired;
}
