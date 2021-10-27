using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class temple_musicfountain : MonoBehaviour
{
	// Token: 0x060008F3 RID: 2291 RVA: 0x00049B8C File Offset: 0x00047D8C
	private void Start()
	{
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00049B90 File Offset: 0x00047D90
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.played && Inventory.fourplayed)
		{
			base.Invoke("music", 1f);
		}
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00049BD8 File Offset: 0x00047DD8
	private void Update()
	{
		if (!base.gameObject.GetComponent<AudioSource>().isPlaying && this.played && !this.cleared)
		{
			base.gameObject.GetComponent<AudioSource>().clip = null;
			this.cleared = true;
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00049C28 File Offset: 0x00047E28
	private void music()
	{
		base.gameObject.GetComponent<AudioSource>().Play();
		this.played = true;
	}

	// Token: 0x04000BE2 RID: 3042
	private bool played;

	// Token: 0x04000BE3 RID: 3043
	private bool cleared;
}
