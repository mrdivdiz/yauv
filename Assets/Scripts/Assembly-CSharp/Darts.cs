using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class Darts : MonoBehaviour
{
	// Token: 0x0600070B RID: 1803 RVA: 0x00038B44 File Offset: 0x00036D44
	public void Awake()
	{
		Darts.on = false;
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00038B4C File Offset: 0x00036D4C
	private void Start()
	{
		this.timer += this.delayTime;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00038B64 File Offset: 0x00036D64
	private void Update()
	{
		if (Darts.on)
		{
			if (this.timer <= 1.5f && this.timer >= 0.5f)
			{
				if (this.dart == null)
				{
					this.dart = (Transform)UnityEngine.Object.Instantiate(this.dartTransform, base.transform.position, base.transform.rotation);
					if (this.dartOutSound != null)
					{
						base.GetComponent<AudioSource>().PlayOneShot(this.dartOutSound, SpeechManager.sfxVolume);
					}
				}
				else
				{
					this.dart.Translate(0f, 0f, 1f * Time.deltaTime, Space.World);
				}
				this.timer -= Time.deltaTime;
			}
			if (this.timer <= 0f)
			{
				this.dart.GetComponent<Rigidbody>().AddForce(0f, 0f, 30f, ForceMode.Impulse);
				if (this.dartSound != null)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.dartSound, SpeechManager.sfxVolume);
				}
				this.timer = 4f;
				this.dart = null;
			}
			else
			{
				this.timer -= Time.deltaTime;
			}
		}
	}

	// Token: 0x040008E5 RID: 2277
	public static bool on;

	// Token: 0x040008E6 RID: 2278
	public Transform dartTransform;

	// Token: 0x040008E7 RID: 2279
	private float timer = 2f;

	// Token: 0x040008E8 RID: 2280
	public float delayTime;

	// Token: 0x040008E9 RID: 2281
	public AudioClip dartSound;

	// Token: 0x040008EA RID: 2282
	public AudioClip dartOutSound;

	// Token: 0x040008EB RID: 2283
	private Transform dart;
}
