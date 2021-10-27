using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class CSB_RotateScript : MonoBehaviour
{
	// Token: 0x0600019A RID: 410 RVA: 0x0000C174 File Offset: 0x0000A374
	private void Start()
	{
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000C178 File Offset: 0x0000A378
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.rotate = !this.rotate;
		}
		if (this.rotate)
		{
			base.transform.RotateAround(this.Rotator.transform.position, this.axis, this.angle * Time.deltaTime);
		}
	}

	// Token: 0x040001A7 RID: 423
	public GameObject Rotator;

	// Token: 0x040001A8 RID: 424
	public Vector3 axis;

	// Token: 0x040001A9 RID: 425
	public float angle;

	// Token: 0x040001AA RID: 426
	private bool rotate = true;
}
