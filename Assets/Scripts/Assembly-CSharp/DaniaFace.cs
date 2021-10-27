using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class DaniaFace : MonoBehaviour
{
	// Token: 0x06000705 RID: 1797 RVA: 0x00038A0C File Offset: 0x00036C0C
	private void Start()
	{
		base.GetComponent<Animation>()["Dania-Facial"].AddMixingTransform(this.head);
		base.GetComponent<Animation>()["Dania-Facial"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Dania-Facial"].layer = 100000;
		base.GetComponent<Animation>()["Dania-Facial"].weight = 0.8f;
		base.GetComponent<Animation>().Blend("Dania-Facial");
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00038A90 File Offset: 0x00036C90
	private void Update()
	{
		base.GetComponent<Animation>().Blend("Dania-Facial");
	}

	// Token: 0x040008E1 RID: 2273
	public Transform head;
}
