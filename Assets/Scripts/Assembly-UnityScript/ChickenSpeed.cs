using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
[Serializable]
public class ChickenSpeed : MonoBehaviour
{
	// Token: 0x06000045 RID: 69 RVA: 0x00002E14 File Offset: 0x00001014
	public virtual void Start()
	{
		this.GetComponent<Animation>()["WLObjChicken_Idle2"].speed = 0.7f;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002E30 File Offset: 0x00001030
	public virtual void Main()
	{
	}
}
