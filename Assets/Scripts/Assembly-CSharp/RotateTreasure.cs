using System;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class RotateTreasure : MonoBehaviour
{
	// Token: 0x0600081E RID: 2078 RVA: 0x0004218C File Offset: 0x0004038C
	private void OnEnable()
	{
		if (RotateTreasure.awardTreasure)
		{
			this.startingTime = Time.time;
			RotateTreasure.awardTreasure = false;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x000421C8 File Offset: 0x000403C8
	private void Update()
	{
		base.transform.Rotate(Vector3.up, 1f * Time.deltaTime);
		if (Time.time < this.startingTime + this.enteringTime)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0.3f, base.transform.localPosition.y, base.transform.localPosition.z), 1f - (this.startingTime + this.enteringTime - Time.time) / this.enteringTime);
		}
		else if (Time.time > this.startingTime + this.enteringTime + this.stayingTime && Time.time < this.startingTime + this.enteringTime + this.stayingTime + this.exitingTime)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(1f, base.transform.localPosition.y, base.transform.localPosition.z), 1f - (this.startingTime + this.enteringTime + this.stayingTime + this.exitingTime - Time.time) / this.exitingTime);
		}
		else if (Time.time > this.startingTime + this.enteringTime + this.stayingTime + this.exitingTime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000AAC RID: 2732
	private float enteringTime = 3f;

	// Token: 0x04000AAD RID: 2733
	private float stayingTime = 2.5f;

	// Token: 0x04000AAE RID: 2734
	private float exitingTime = 2.8f;

	// Token: 0x04000AAF RID: 2735
	private float startingTime;

	// Token: 0x04000AB0 RID: 2736
	public static bool awardTreasure;
}
