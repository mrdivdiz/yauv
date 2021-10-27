using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class Pigeon_Fly_Pond : MonoBehaviour
{
	// Token: 0x0600013F RID: 319 RVA: 0x00009C18 File Offset: 0x00007E18
	public void Awake()
	{
		Pigeon_Fly_Pond.fly = false;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00009C20 File Offset: 0x00007E20
	public void OnTriggerEnter(Collider other)
	{
		if (this.firstTime)
		{
			this.flysound.Play();
			Pigeon_Fly_Pond.fly = true;
			this.firstTime = false;
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			UnityEngine.Object.Destroy(base.gameObject, 6f);
		}
	}

	// Token: 0x04000151 RID: 337
	public static bool fly;

	// Token: 0x04000152 RID: 338
	public AudioSource flysound;

	// Token: 0x04000153 RID: 339
	public bool firstTime = true;
}
