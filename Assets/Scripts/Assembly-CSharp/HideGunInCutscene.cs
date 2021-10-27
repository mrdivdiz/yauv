using System;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class HideGunInCutscene : MonoBehaviour
{
	// Token: 0x0600078C RID: 1932 RVA: 0x0003D910 File Offset: 0x0003BB10
	private void OnEnable()
	{
		if (WeaponsHUD.weaponName != base.gameObject.name)
		{
			base.gameObject.SetActive(false);
		}
	}
}
