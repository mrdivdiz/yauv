using System;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class ThiefPlayTrigger : MonoBehaviour
{
	// Token: 0x06000876 RID: 2166 RVA: 0x00046174 File Offset: 0x00044374
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && this.thief != null)
		{
			this.thief.Play();
		}
	}

	// Token: 0x04000B41 RID: 2881
	public RunningThief thief;
}
