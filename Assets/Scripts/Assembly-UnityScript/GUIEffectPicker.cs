using System;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x02000065 RID: 101
[Serializable]
public class GUIEffectPicker : MonoBehaviour
{
	// Token: 0x06000134 RID: 308 RVA: 0x000076A8 File Offset: 0x000058A8
	public virtual void OnGUI()
	{
		int i = 0;
		for (i = 0; i < Extensions.get_length(this.FX); i++)
		{
			if (GUI.Button(new Rect((float)(120 * i), (float)0, (float)120, (float)80), this.FX[i].gameObject.name))
			{
			}
		}
		Time.timeScale = GUI.HorizontalSlider(new Rect((float)0, (float)130, (float)Screen.width, (float)10), Time.timeScale, (float)0, (float)15);
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00007734 File Offset: 0x00005934
	public virtual void Main()
	{
	}

	// Token: 0x0400011E RID: 286
	public Transform[] FX;
}
