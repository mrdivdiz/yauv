using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
[Serializable]
public class addforce : MonoBehaviour
{
	// Token: 0x0600007D RID: 125 RVA: 0x000036CC File Offset: 0x000018CC
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Rock")
		{
			other.GetComponent<Rigidbody>().AddForce(this.transform.forward * (float)100);
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00003714 File Offset: 0x00001914
	public virtual void Main()
	{
	}
}
