using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class DetachChildrenAndDestroy : MonoBehaviour
{
	// Token: 0x06000182 RID: 386 RVA: 0x0000C190 File Offset: 0x0000A390
	private void Start()
	{
		if (base.transform.parent == null)
		{
			base.transform.DetachChildren();
		}
		else
		{
			List<Transform> list = new List<Transform>();
			foreach (object obj in base.transform)
			{
				Transform item = (Transform)obj;
				list.Add(item);
			}
			foreach (Transform transform in list)
			{
				transform.parent = base.transform.parent;
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
