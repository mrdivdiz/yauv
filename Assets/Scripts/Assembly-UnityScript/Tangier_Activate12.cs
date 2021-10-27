using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
[Serializable]
public class Tangier_Activate12 : MonoBehaviour
{
	// Token: 0x06000058 RID: 88 RVA: 0x00003404 File Offset: 0x00001604
	public virtual void Start()
	{
		this.Block3a.SetActiveRecursively(false);
		this.Block3b.SetActiveRecursively(false);
		this.Block3c.SetActiveRecursively(false);
		this.Block3d.SetActiveRecursively(false);
		this.Block4a.SetActiveRecursively(false);
		this.Block4b.SetActiveRecursively(false);
		this.Block4c.SetActiveRecursively(false);
		this.Block4d.SetActiveRecursively(false);
		this.Block4e.SetActiveRecursively(false);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003480 File Offset: 0x00001680
	public virtual void Main()
	{
	}

	// Token: 0x04000033 RID: 51
	public GameObject Block3a;

	// Token: 0x04000034 RID: 52
	public GameObject Block3b;

	// Token: 0x04000035 RID: 53
	public GameObject Block3c;

	// Token: 0x04000036 RID: 54
	public GameObject Block3d;

	// Token: 0x04000037 RID: 55
	public GameObject Block4a;

	// Token: 0x04000038 RID: 56
	public GameObject Block4b;

	// Token: 0x04000039 RID: 57
	public GameObject Block4c;

	// Token: 0x0400003A RID: 58
	public GameObject Block4d;

	// Token: 0x0400003B RID: 59
	public GameObject Block4e;
}
