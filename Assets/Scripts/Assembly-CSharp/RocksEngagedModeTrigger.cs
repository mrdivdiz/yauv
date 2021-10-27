using System;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class RocksEngagedModeTrigger : MonoBehaviour
{
	// Token: 0x06000819 RID: 2073 RVA: 0x000420F4 File Offset: 0x000402F4
	private void Start()
	{
		this.ba = AnimationHandler.instance.gameObject.GetComponent<BasicAgility>();
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0004210C File Offset: 0x0004030C
	private void Update()
	{
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00042110 File Offset: 0x00040310
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.played)
		{
			AnimationHandler.instance.engagedMode = true;
			this.ba.rollInsteadOfJump = true;
			this.played = true;
		}
	}

	// Token: 0x04000AAA RID: 2730
	private bool played;

	// Token: 0x04000AAB RID: 2731
	private BasicAgility ba;
}
