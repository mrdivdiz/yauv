using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class GotUp : MonoBehaviour
{
	// Token: 0x0600078A RID: 1930 RVA: 0x0003D8DC File Offset: 0x0003BADC
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GotUp.DidGetUp = true;
		}
		this.cylinderObject.SetActive(false);
	}

	// Token: 0x040009DA RID: 2522
	public static bool DidGetUp;

	// Token: 0x040009DB RID: 2523
	public GameObject cylinderObject;
}
