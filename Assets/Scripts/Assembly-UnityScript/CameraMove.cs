using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
[Serializable]
public class CameraMove : MonoBehaviour
{
	// Token: 0x06000131 RID: 305 RVA: 0x00007468 File Offset: 0x00005668
	public virtual void Update()
	{
		this.transform.Translate(Input.GetAxis("Horizontal"), (float)0, Input.GetAxis("Vertical"));
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00007498 File Offset: 0x00005698
	public virtual void Main()
	{
	}
}
