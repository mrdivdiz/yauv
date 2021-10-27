using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
[Serializable]
public class TimedObjectDestructor : MonoBehaviour
{
	// Token: 0x060000D4 RID: 212 RVA: 0x0000A688 File Offset: 0x00008888
	public TimedObjectDestructor()
	{
		this.timeOut = 1f;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000A69C File Offset: 0x0000889C
	public virtual void Awake()
	{
		this.Invoke("DestroyNow", this.timeOut);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0000A6B0 File Offset: 0x000088B0
	public virtual void DestroyNow()
	{
		if (this.detachChildren)
		{
			this.transform.DetachChildren();
		}
		UnityEngine.Object.DestroyObject(this.gameObject);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000A6D4 File Offset: 0x000088D4
	public virtual void Main()
	{
	}

	// Token: 0x040001BD RID: 445
	public float timeOut;

	// Token: 0x040001BE RID: 446
	public bool detachChildren;
}
