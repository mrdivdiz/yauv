using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class Pigeon_pond : MonoBehaviour
{
	// Token: 0x06000142 RID: 322 RVA: 0x00009C80 File Offset: 0x00007E80
	private void Start()
	{
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

	// Token: 0x06000143 RID: 323 RVA: 0x00009D44 File Offset: 0x00007F44
	private void Update()
	{
		if (Pigeon_Fly_Pond.fly)
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

	// Token: 0x04000154 RID: 340
	public Animation PigeonAnimationFlying;

	// Token: 0x04000155 RID: 341
	public Animation PigeonAnimationStanding;

	// Token: 0x04000156 RID: 342
	public Renderer PigeonMeshStanding;

	// Token: 0x04000157 RID: 343
	public Renderer PigeonMeshFlying;

	// Token: 0x04000158 RID: 344
	public GameObject FlyTarget;
}
