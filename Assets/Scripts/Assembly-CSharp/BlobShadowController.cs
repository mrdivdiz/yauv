using System;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class BlobShadowController : MonoBehaviour
{
	// Token: 0x06000AC5 RID: 2757 RVA: 0x000801E4 File Offset: 0x0007E3E4
	private void Start()
	{
		this.mTranform = base.transform;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x000801F4 File Offset: 0x0007E3F4
	private void Update()
	{
		this.mTranform.position = this.mTranform.parent.position + Vector3.up * 8.246965f;
		this.mTranform.rotation = Quaternion.LookRotation(-Vector3.up, this.mTranform.parent.forward);
	}

	// Token: 0x040011BF RID: 4543
	private Transform mTranform;
}
