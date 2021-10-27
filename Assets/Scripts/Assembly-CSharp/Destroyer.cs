using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class Destroyer : MonoBehaviour
{
	// Token: 0x0600071A RID: 1818 RVA: 0x00038DE0 File Offset: 0x00036FE0
	private void Start()
	{
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00038DE4 File Offset: 0x00036FE4
	private void Update()
	{
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x00038DE8 File Offset: 0x00036FE8
	private void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Breakable")
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.breakSound, SpeechManager.sfxVolume);
			foreach (object obj in c.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.GetComponent<Rigidbody>() == null)
				{
					transform.gameObject.AddComponent<Rigidbody>();
				}
			}
			if (this.collectablePrefab != null && !this.spawoned)
			{
				UnityEngine.Object.Instantiate(this.collectablePrefab, c.transform.position, Quaternion.identity);
				this.spawoned = true;
			}
		}
	}

	// Token: 0x040008F1 RID: 2289
	public AudioClip breakSound;

	// Token: 0x040008F2 RID: 2290
	public GameObject collectablePrefab;

	// Token: 0x040008F3 RID: 2291
	private bool spawoned;
}
