using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
[Serializable]
public class SoundHammer : MonoBehaviour
{
	// Token: 0x06000048 RID: 72 RVA: 0x00002E3C File Offset: 0x0000103C
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.characterModel.GetComponent<Animation>().CrossFade("Fixing");
		}
		this.Hammer.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00002E84 File Offset: 0x00001084
	public virtual void Main()
	{
	}

	// Token: 0x0400001F RID: 31
	public GameObject Hammer;

	// Token: 0x04000020 RID: 32
	public GameObject characterModel;
}
