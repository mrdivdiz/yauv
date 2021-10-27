using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class StreamTempleGrunts : MonoBehaviour
{
	// Token: 0x0600086A RID: 2154 RVA: 0x0004608C File Offset: 0x0004428C
	private void Start()
	{
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00046090 File Offset: 0x00044290
	private void OnTriggerEnter()
	{
		Application.LoadLevelAdditiveAsync("AhmoseTemple3-Grunts");
	}
}
