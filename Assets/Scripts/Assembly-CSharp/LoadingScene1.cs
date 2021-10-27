using System;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class LoadingScene1 : MonoBehaviour
{
	// Token: 0x06000A74 RID: 2676 RVA: 0x00071400 File Offset: 0x0006F600
	private void Start()
	{
		Application.LoadLevelAsync(this.levelToBeLoaded);
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00071410 File Offset: 0x0006F610
	private void Update()
	{
	}

	// Token: 0x040010B4 RID: 4276
	public string levelToBeLoaded = "Prototype";

	// Token: 0x040010B5 RID: 4277
	public string levelTitleKeyword = "M_Prototype";
}
