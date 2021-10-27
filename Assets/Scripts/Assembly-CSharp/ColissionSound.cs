using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class ColissionSound : MonoBehaviour
{
	// Token: 0x060006D8 RID: 1752 RVA: 0x00037318 File Offset: 0x00035518
	private void OnCollisionEnter(Collision collision)
	{
		if (this.collissionSound != null && (collision.collider.tag == "barrel" || collision.collider.tag == "wood"))
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.collissionSound);
		}
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x0003737C File Offset: 0x0003557C
	private void OnTriggerEnter(Collider other)
	{
		if (this.collissionSound != null && other.tag == "Player" && !base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.collissionSound);
		}
	}

	// Token: 0x040008AD RID: 2221
	public AudioClip collissionSound;
}
