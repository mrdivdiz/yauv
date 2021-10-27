using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class TrafficSign : MonoBehaviour
{
	// Token: 0x06000679 RID: 1657 RVA: 0x00032004 File Offset: 0x00030204
	public void Start()
	{
		if (QualitySettings.names[QualitySettings.GetQualityLevel()] == "Fantastic")
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00032038 File Offset: 0x00030238
	public void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "PlayerCar")
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
	}
}
