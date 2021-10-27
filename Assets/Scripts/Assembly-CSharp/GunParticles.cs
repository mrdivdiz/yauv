using System;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class GunParticles : MonoBehaviour
{
	// Token: 0x06000A2E RID: 2606 RVA: 0x0006DC4C File Offset: 0x0006BE4C
	private void Start()
	{
		this.cState = true;
		this.emitters = base.GetComponentsInChildren<ParticleEmitter>();
		this.bulletTrace = base.transform.Find("bullet_trace");
		this.ChangeState(false, FireMode.FULL_AUTO);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0006DC8C File Offset: 0x0006BE8C
	private void Awake()
	{
		this.cState = true;
		this.emitters = base.GetComponentsInChildren<ParticleEmitter>();
		this.bulletTrace = base.transform.Find("bullet_trace");
		this.ChangeState(false, FireMode.FULL_AUTO);
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0006DCCC File Offset: 0x0006BECC
	public void ChangeState(bool p_newState, FireMode fireMode)
	{
		if (this.bulletTrace != null)
		{
			this.bulletTrace.position = base.transform.position;
			this.bulletTrace.Translate(0f, 0f, 5f, Space.Self);
		}
		if (this.cState == p_newState)
		{
			return;
		}
		this.cState = p_newState;
		if (this.emitters != null)
		{
			for (int i = 0; i < this.emitters.Length; i++)
			{
				if (this.emitters[i] != null && this.emitters[i].gameObject != null && this.emitters[i].gameObject.name == "bullet_trace" && this.disableBulletTrace)
				{
					this.emitters[i].emit = false;
				}
				else if (fireMode == FireMode.SEMI_AUTO)
				{
					if (p_newState && this.emitters[i].enabled)
					{
						this.emitters[i].Emit();
					}
				}
				else if (this.emitters[i] != null)
				{
					this.emitters[i].emit = p_newState;
				}
			}
		}
	}

	// Token: 0x0400100D RID: 4109
	private bool cState;

	// Token: 0x0400100E RID: 4110
	private ParticleEmitter[] emitters;

	// Token: 0x0400100F RID: 4111
	private Transform bulletTrace;

	// Token: 0x04001010 RID: 4112
	public bool disableBulletTrace;
}
