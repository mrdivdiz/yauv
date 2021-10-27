using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class FireSound : MonoBehaviour
{
	// Token: 0x0600074E RID: 1870 RVA: 0x0003A368 File Offset: 0x00038568
	private void Start()
	{
		if (base.GetComponent<AudioSource>() != null)
		{
			base.GetComponent<AudioSource>().Stop();
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		if (gameObject != null)
		{
			this.player = gameObject.transform;
		}
		this.flickering = base.gameObject.GetComponent<flickeringLight>();
		base.InvokeRepeating("Check", 1f, 1f);
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0003A3DC File Offset: 0x000385DC
	private void Check()
	{
		if (this.player == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (!(gameObject != null))
			{
				return;
			}
			this.player = gameObject.transform;
		}
		if (this.player != null && Vector3.Distance(this.player.position, base.transform.position) < 8f)
		{
			if (!base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().Play();
			}
		}
		else if (base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Stop();
		}
	}

	// Token: 0x0400093B RID: 2363
	private Transform player;

	// Token: 0x0400093C RID: 2364
	private flickeringLight flickering;
}
