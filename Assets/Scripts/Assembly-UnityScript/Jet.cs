using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x0200002E RID: 46
[Serializable]
public class Jet : MonoBehaviour
{
	// Token: 0x06000086 RID: 134 RVA: 0x00003984 File Offset: 0x00001B84
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.cutscene2.StartCutscene();
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000039B4 File Offset: 0x00001BB4
	public virtual void Main()
	{
	}

	// Token: 0x0400004A RID: 74
	public CSComponent cutscene2;
}
