using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[Serializable]
public class RainVolume : MonoBehaviour
{
	// Token: 0x0600003E RID: 62 RVA: 0x00002A74 File Offset: 0x00000C74
	public RainVolume()
	{
		this.containerMinScale = 0.3f;
		this.containerMaxScale = 0.34f;
		this.timeRate = 0.01f;
		this.fillHeight = 0.03f;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002AB4 File Offset: 0x00000CB4
	public virtual void Update()
	{
		float rainLevel = RainGlobalControl.rainLevel;
		if (this.transform.position.y < this.fillHeight)
		{
			float num = this.timeRate * 0.005f;
			this.timeSpeed = Time.deltaTime * this.timeRate;
			this.radius = Mathf.Clamp(this.timeSpeed, this.containerMinScale * num, this.containerMaxScale * num);
			this.transform.position = this.transform.position + new Vector3((float)0, rainLevel * this.timeSpeed, (float)0);
			this.transform.localScale = this.transform.localScale + new Vector3(this.radius * rainLevel, (float)0, this.radius * rainLevel);
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002B84 File Offset: 0x00000D84
	public virtual void Main()
	{
	}

	// Token: 0x04000015 RID: 21
	public GameObject rainGlobalControl;

	// Token: 0x04000016 RID: 22
	public float containerMinScale;

	// Token: 0x04000017 RID: 23
	public float containerMaxScale;

	// Token: 0x04000018 RID: 24
	public float timeRate;

	// Token: 0x04000019 RID: 25
	public float fillHeight;

	// Token: 0x0400001A RID: 26
	public float radius;

	// Token: 0x0400001B RID: 27
	public float timeSpeed;
}
