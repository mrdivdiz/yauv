using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
[Serializable]
public class RainScreenEffectPro : MonoBehaviour
{
	// Token: 0x0600003B RID: 59 RVA: 0x000029E4 File Offset: 0x00000BE4
	public RainScreenEffectPro()
	{
		this.transitionSpeed = 0.5f;
		this.effectIntensity = 0.3f;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002A04 File Offset: 0x00000C04
	public virtual void LateUpdate()
	{
		float @float = this.GetComponent<Renderer>().material.GetFloat("_BumpAmt");
		this.GetComponent<Renderer>().material.SetFloat("_BumpAmt", Mathf.Lerp(@float, this.cam1.transform.localEulerAngles.x * this.effectIntensity, Time.deltaTime * this.transitionSpeed));
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002A70 File Offset: 0x00000C70
	public virtual void Main()
	{
	}

	// Token: 0x04000012 RID: 18
	public GameObject cam1;

	// Token: 0x04000013 RID: 19
	public float transitionSpeed;

	// Token: 0x04000014 RID: 20
	public float effectIntensity;
}
