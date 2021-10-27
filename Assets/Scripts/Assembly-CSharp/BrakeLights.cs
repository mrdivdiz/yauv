using System;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class BrakeLights : MonoBehaviour
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x00080264 File Offset: 0x0007E464
	private void Start()
	{
		this.carcontroller = base.transform.GetComponent<CarController>();
		if (this.brakeLights)
		{
			this.startValue = this.brakeLights.GetFloat("_Intensity");
		}
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x000802A8 File Offset: 0x0007E4A8
	private void FixedUpdate()
	{
		if (this.brakeLights)
		{
			if (this.carcontroller.brakeKey)
			{
				if (this.intensityValue < this.startValue + 1f)
				{
					this.intensityValue += Time.deltaTime / 0.1f;
					this.brakeLights.SetFloat("_Intensity", this.intensityValue);
					this.brake1.gameObject.SetActive(true);
					this.brake2.gameObject.SetActive(true);
				}
			}
			else if (this.intensityValue > this.startValue)
			{
				this.intensityValue -= Time.deltaTime / 0.1f;
				this.brakeLights.SetFloat("_Intensity", this.intensityValue);
				this.brake1.gameObject.SetActive(false);
				this.brake2.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x040011C0 RID: 4544
	[HideInInspector]
	public CarController carcontroller;

	// Token: 0x040011C1 RID: 4545
	public Material brakeLights;

	// Token: 0x040011C2 RID: 4546
	private float startValue;

	// Token: 0x040011C3 RID: 4547
	private float intensityValue;

	// Token: 0x040011C4 RID: 4548
	public Light brake1;

	// Token: 0x040011C5 RID: 4549
	public Light brake2;
}
