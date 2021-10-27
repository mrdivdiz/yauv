using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[Serializable]
public class Death_Boulder : MonoBehaviour
{
	// Token: 0x06000076 RID: 118 RVA: 0x000037F0 File Offset: 0x000019F0
	public virtual void OnTriggerEnter(Collider p)
	{
		if (p.gameObject.tag == "Player" && this.transform.parent.GetComponent<Rigidbody>().velocity.magnitude > 0.01f)
		{
			p.SendMessage("DecreasHealthPercentatge", 1f, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00003854 File Offset: 0x00001A54
	public virtual void Main()
	{
	}
}
