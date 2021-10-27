using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class FountainStarter : MonoBehaviour
{
	// Token: 0x06000777 RID: 1911 RVA: 0x0003C690 File Offset: 0x0003A890
	public void OnCheckpointLoad(int checkpointReached)
	{
		if (checkpointReached >= 1)
		{
			this.fountainStream1.GetComponent<ParticleEmitter>().emit = true;
			this.fountainStream2.GetComponent<ParticleEmitter>().emit = true;
			this.fountainStream3.GetComponent<ParticleEmitter>().emit = true;
			this.fountainStream4.GetComponent<ParticleEmitter>().emit = true;
			this.fountainStream5.GetComponent<ParticleEmitter>().emit = true;
			this.fountainSplash1.GetComponent<ParticleEmitter>().emit = true;
			this.fountainSplash2.GetComponent<ParticleEmitter>().emit = true;
			this.fountainSplash3.GetComponent<ParticleEmitter>().emit = true;
			this.fountainSplash4.GetComponent<ParticleEmitter>().emit = true;
			this.fountainAudio.Play();
			iTween.MoveBy(this.waterLevel, iTween.Hash(new object[]
			{
				"y",
				0.8,
				"time",
				7,
				"delay",
				3
			}));
		}
	}

	// Token: 0x04000995 RID: 2453
	public GameObject fountainStream1;

	// Token: 0x04000996 RID: 2454
	public GameObject fountainStream2;

	// Token: 0x04000997 RID: 2455
	public GameObject fountainStream3;

	// Token: 0x04000998 RID: 2456
	public GameObject fountainStream4;

	// Token: 0x04000999 RID: 2457
	public GameObject fountainStream5;

	// Token: 0x0400099A RID: 2458
	public GameObject fountainSplash1;

	// Token: 0x0400099B RID: 2459
	public GameObject fountainSplash2;

	// Token: 0x0400099C RID: 2460
	public GameObject fountainSplash3;

	// Token: 0x0400099D RID: 2461
	public GameObject fountainSplash4;

	// Token: 0x0400099E RID: 2462
	public AudioSource fountainAudio;

	// Token: 0x0400099F RID: 2463
	public GameObject waterLevel;
}
