using System;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class Bullet : MonoBehaviour
{
	// Token: 0x060009ED RID: 2541 RVA: 0x00067184 File Offset: 0x00065384
	private void Start()
	{
		this.instTime = Time.time;
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00067194 File Offset: 0x00065394
	private void Update()
	{
		if (Time.time > this.instTime + 2f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, 1f))
		{
			try
			{
				if (raycastHit.collider.gameObject.tag == "Player")
				{
					(raycastHit.collider.gameObject.GetComponent(typeof(Health)) as Health).DecreasHealth(1f);
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
			catch
			{
			}
		}
	}

	// Token: 0x04000F2B RID: 3883
	private float instTime;
}
