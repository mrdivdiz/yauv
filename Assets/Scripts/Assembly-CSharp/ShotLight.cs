using System;
using UnityEngine;

// Token: 0x0200020C RID: 524
public class ShotLight : MonoBehaviour
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x00070164 File Offset: 0x0006E364
	private void OnEnable()
	{
		if (base.GetComponent<Light>() == null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			this.timer = this.time;
			base.GetComponent<Light>().enabled = false;
		}
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x000701A8 File Offset: 0x0006E3A8
	private void OnDisable()
	{
		if (base.GetComponent<Light>() == null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			this.timer = this.time;
			base.GetComponent<Light>().enabled = false;
		}
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x000701EC File Offset: 0x0006E3EC
	private void LateUpdate()
	{
		this.timer -= Time.deltaTime;
		if (this.timer <= 0f)
		{
			this.timer = this.time;
			base.GetComponent<Light>().enabled = !base.GetComponent<Light>().enabled;
		}
	}

	// Token: 0x0400106C RID: 4204
	public float time = 0.02f;

	// Token: 0x0400106D RID: 4205
	private float timer;
}
