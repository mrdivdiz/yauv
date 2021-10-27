using System;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class WaterFootFollower : MonoBehaviour
{
	// Token: 0x06000897 RID: 2199 RVA: 0x00047D30 File Offset: 0x00045F30
	private void Start()
	{
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00047D34 File Offset: 0x00045F34
	private void Update()
	{
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00047D38 File Offset: 0x00045F38
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			this.WaterFootTransform.position = new Vector3(other.transform.position.x, this.WaterFootTransform.position.y, other.transform.position.z);
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00047DA4 File Offset: 0x00045FA4
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.WaterFootTransform.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00047DD8 File Offset: 0x00045FD8
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.WaterFootTransform.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000B89 RID: 2953
	public Transform WaterFootTransform;
}
