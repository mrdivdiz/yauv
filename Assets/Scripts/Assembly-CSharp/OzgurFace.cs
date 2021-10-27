using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class OzgurFace : MonoBehaviour
{
	// Token: 0x060007F5 RID: 2037 RVA: 0x00041404 File Offset: 0x0003F604
	private void Start()
	{
		base.GetComponent<Animation>()["ozgur-face"].AddMixingTransform(this.head);
		base.GetComponent<Animation>()["ozgur-face"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["ozgur-face"].layer = 1000;
		base.GetComponent<Animation>().Blend("ozgur-face");
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0004146C File Offset: 0x0003F66C
	private void Update()
	{
		base.GetComponent<Animation>().Blend("ozgur-face");
	}

	// Token: 0x04000A7E RID: 2686
	public Transform head;
}
