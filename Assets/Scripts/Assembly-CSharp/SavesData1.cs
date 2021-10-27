using System;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class SavesData1 : MonoBehaviour
{
	// Token: 0x06000A8F RID: 2703 RVA: 0x0007205C File Offset: 0x0007025C
	private void Start()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle();
			this.style.font = this.defaultFont;
			this.style.alignment = TextAnchor.MiddleCenter;
			this.style.wordWrap = true;
			this.style.normal.textColor = Color.white;
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x000720C0 File Offset: 0x000702C0
	private void Update()
	{
		if (InputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("LoadingMainMenu");
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x000720E8 File Offset: 0x000702E8
	private void OnGUI()
	{
		GUI.Label(new Rect((float)Screen.width / 2f - 300f, (float)Screen.height / 2f - 300f, 600f, 600f), Language.Get("M_Unlocked", 60), this.style);
	}

	// Token: 0x040010E0 RID: 4320
	private GUIStyle style;

	// Token: 0x040010E1 RID: 4321
	public Font defaultFont;
}
