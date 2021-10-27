using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class GotDown : MonoBehaviour
{
	// Token: 0x06000787 RID: 1927 RVA: 0x0003D8A4 File Offset: 0x0003BAA4
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GotDown.DidGetUp = true;
		}
		this.cylinderObject.SetActive(true);
	}

	// Token: 0x040009D8 RID: 2520
	public static bool DidGetUp;

	// Token: 0x040009D9 RID: 2521
	public GameObject cylinderObject;
}
