using System;
using UnityEngine;

// Token: 0x020001ED RID: 493
[Serializable]
public class TakedownAnim
{
	// Token: 0x04000EC5 RID: 3781
	public AnimationClip playerAnimation;

	// Token: 0x04000EC6 RID: 3782
	public AnimationClip enemyAnimation;

	// Token: 0x04000EC7 RID: 3783
	public float distance = 1.12f;

	// Token: 0x04000EC8 RID: 3784
	public AudioClip takedownSound;

	// Token: 0x04000EC9 RID: 3785
	public bool useInPrologue = true;
}
