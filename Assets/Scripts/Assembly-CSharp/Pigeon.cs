using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class Pigeon : MonoBehaviour
{
	// Token: 0x06000137 RID: 311 RVA: 0x000098C8 File Offset: 0x00007AC8
	private void Start()
	{
		this.timeBeforeFly = UnityEngine.Random.Range(0f, 0.5f);
		AnimationClip animationClip = null;
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				animationClip = this.PigeonAnimationStanding.GetComponent<Animation>()["Eat"].clip;
			}
		}
		else
		{
			animationClip = this.PigeonAnimationStanding.GetComponent<Animation>()["Idle"].clip;
		}
		this.PigeonAnimationStanding.GetComponent<Animation>()[animationClip.name].speed = UnityEngine.Random.Range(0.5f, 1f);
		animationClip.wrapMode = WrapMode.Loop;
		this.PigeonAnimationStanding.GetComponent<Animation>().CrossFade(animationClip.name);
		this.PigeonMeshFlying.enabled = false;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000099A0 File Offset: 0x00007BA0
	private void Update()
	{
		if (Pigeon_Fly.fly)
		{
			if (this.timeBeforeFly > 0f)
			{
				this.timeBeforeFly -= Time.deltaTime;
			}
			else
			{
				this.PigeonAnimationFlying.GetComponent<Animation>()["fly"].wrapMode = WrapMode.Loop;
				this.PigeonAnimationFlying.GetComponent<Animation>().CrossFade("fly");
				this.PigeonMeshStanding.enabled = false;
				this.PigeonMeshFlying.enabled = true;
				iTween.RotateTo(base.gameObject, iTween.Hash(new object[]
				{
					"z",
					this.FlyTarget.transform.position.z,
					"y",
					this.FlyTarget.transform.position.y,
					"x",
					this.FlyTarget.transform.position.x,
					"time",
					2,
					"EaseType",
					iTween.EaseType.easeInOutSine
				}));
				iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
				{
					"z",
					this.FlyTarget.transform.position.z,
					"y",
					this.FlyTarget.transform.position.y,
					"x",
					this.FlyTarget.transform.position.x,
					"time",
					14,
					"EaseType",
					iTween.EaseType.easeInOutSine
				}));
			}
		}
	}

	// Token: 0x04000148 RID: 328
	public Animation PigeonAnimationFlying;

	// Token: 0x04000149 RID: 329
	public Animation PigeonAnimationStanding;

	// Token: 0x0400014A RID: 330
	public Renderer PigeonMeshStanding;

	// Token: 0x0400014B RID: 331
	public Renderer PigeonMeshFlying;

	// Token: 0x0400014C RID: 332
	public GameObject FlyTarget;

	// Token: 0x0400014D RID: 333
	private float timeBeforeFly;
}
