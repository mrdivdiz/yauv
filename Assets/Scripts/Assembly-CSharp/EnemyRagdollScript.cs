using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class EnemyRagdollScript : MonoBehaviour
{
	// Token: 0x060005C3 RID: 1475 RVA: 0x00027AC0 File Offset: 0x00025CC0
	private void Start()
	{
		if (this.head == null)
		{
			this.head = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
		}
		base.GetComponent<Animation>()["Grunts-Facial-Dead"].AddMixingTransform(this.head);
		base.GetComponent<Animation>()["Grunts-Facial-Dead"].layer = 2;
		base.GetComponent<Animation>().CrossFade("Grunts-Facial-Dead");
	}

	// Token: 0x04000648 RID: 1608
	private Transform head;
}
