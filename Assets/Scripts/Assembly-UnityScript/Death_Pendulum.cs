using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
[Serializable]
public class Death_Pendulum : MonoBehaviour
{
	// Token: 0x0600007C RID: 124 RVA: 0x000038AC File Offset: 0x00001AAC
	public virtual void OnTriggerEnter(Collider p)
	{
		if (p.gameObject.tag == "Player")
		{
			p.SendMessage("DecreasHealthPercentatge", 1f, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000038EC File Offset: 0x00001AEC
	public virtual void Main()
	{
	}
}
