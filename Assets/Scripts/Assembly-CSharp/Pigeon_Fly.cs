using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class Pigeon_Fly : MonoBehaviour
{
	// Token: 0x0600013B RID: 315 RVA: 0x00009BA4 File Offset: 0x00007DA4
	public void Awake()
	{
		Pigeon_Fly.fly = false;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00009BAC File Offset: 0x00007DAC
	public void OnTriggerEnter(Collider other)
	{
		if (this.firstTime)
		{
			this.flysound.Play();
			Pigeon_Fly.fly = true;
			this.firstTime = false;
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			UnityEngine.Object.Destroy(base.gameObject, 6f);
		}
	}

	// Token: 0x0400014E RID: 334
	public static bool fly;

	// Token: 0x0400014F RID: 335
	public AudioSource flysound;

	// Token: 0x04000150 RID: 336
	public bool firstTime = true;
}
