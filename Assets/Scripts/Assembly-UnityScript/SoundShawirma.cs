using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[Serializable]
public class SoundShawirma : MonoBehaviour
{
	// Token: 0x0600004B RID: 75 RVA: 0x00002E90 File Offset: 0x00001090
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.characterModel.GetComponent<Animation>().CrossFade("Shawirma");
		}
		this.Hammer.GetComponent<AudioSource>().Play();
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00002ED8 File Offset: 0x000010D8
	public virtual void Main()
	{
	}

	// Token: 0x04000021 RID: 33
	public GameObject Hammer;

	// Token: 0x04000022 RID: 34
	public GameObject characterModel;
}
