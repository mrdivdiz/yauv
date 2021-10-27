using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
[Serializable]
public class Death_Dart : MonoBehaviour
{
	// Token: 0x06000079 RID: 121 RVA: 0x00003860 File Offset: 0x00001A60
	public virtual void OnTriggerEnter(Collider p)
	{
		if (p.gameObject.tag == "Player")
		{
			p.SendMessage("DecreasHealthPercentatge", 0.9f, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000038A0 File Offset: 0x00001AA0
	public virtual void Main()
	{
	}
}
