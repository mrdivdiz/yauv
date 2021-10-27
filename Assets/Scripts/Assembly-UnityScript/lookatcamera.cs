using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
[Serializable]
public class lookatcamera : MonoBehaviour
{
	// Token: 0x06000103 RID: 259 RVA: 0x00006A14 File Offset: 0x00004C14
	public virtual void Update()
	{
		Vector3 forward = Camera.main.transform.position - this.transform.position;
		forward.y = (float)0;
		this.transform.rotation = Quaternion.LookRotation(forward);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006A5C File Offset: 0x00004C5C
	public virtual void Main()
	{
	}
}
