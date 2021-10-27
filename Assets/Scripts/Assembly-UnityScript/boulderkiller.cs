using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[Serializable]
public class boulderkiller : MonoBehaviour
{
	// Token: 0x0600008C RID: 140 RVA: 0x00003A14 File Offset: 0x00001C14
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Rock")
		{
			UnityEngine.Object.Destroy(other.gameObject);
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00003A44 File Offset: 0x00001C44
	public virtual void Main()
	{
	}
}
