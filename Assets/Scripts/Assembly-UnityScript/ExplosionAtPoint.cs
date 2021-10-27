using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
[Serializable]
public class ExplosionAtPoint : MonoBehaviour
{
	// Token: 0x06000131 RID: 305 RVA: 0x0000761C File Offset: 0x0000581C
	public virtual void Update()
	{
		if (Input.GetKeyDown("mouse 0"))
		{
			this.nextCopy = Time.time + this.rate;
			Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(ray, out raycastHit))
			{
				Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
				UnityEngine.Object.Instantiate(this.explosionPrefab, raycastHit.point, Quaternion.identity);
			}
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000769C File Offset: 0x0000589C
	public virtual void Main()
	{
	}

	// Token: 0x0400011B RID: 283
	public Transform explosionPrefab;

	// Token: 0x0400011C RID: 284
	public float rate;

	// Token: 0x0400011D RID: 285
	public float nextCopy;
}
