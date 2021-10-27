using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000096 RID: 150
[AddComponentMenu("Utilities/HUDFPS")]
public class FPSCounter : MonoBehaviour
{
	// Token: 0x06000325 RID: 805 RVA: 0x0001888C File Offset: 0x00016A8C
	private void Start()
	{
		this.startRect = new Rect((float)(Screen.width / 2) - 37f, 10f, 75f, 50f);
		base.StartCoroutine(this.FPS());
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000188C4 File Offset: 0x00016AC4
	private void Update()
	{
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000188F8 File Offset: 0x00016AF8
	private IEnumerator FPS()
	{
		for (;;)
		{
			float fps = this.accum / (float)this.frames;
			this.sFPS = fps.ToString("f" + Mathf.Clamp(this.nbDecimal, 0, 10));
			this.color = ((fps < 30f) ? ((fps <= 10f) ? Color.red : Color.yellow) : Color.green);
			this.accum = 0f;
			this.frames = 0;
			yield return new WaitForSeconds(this.frequency);
		}
		yield break;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00018914 File Offset: 0x00016B14
	private void OnGUI()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.label);
			this.style.normal.textColor = Color.white;
			this.style.alignment = TextAnchor.MiddleCenter;
		}
		GUI.color = ((!this.updateColor) ? Color.white : this.color);
		this.startRect = GUI.Window(0, this.startRect, new GUI.WindowFunction(this.DoMyWindow), string.Empty);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x000189A8 File Offset: 0x00016BA8
	private void DoMyWindow(int windowID)
	{
		GUI.Label(new Rect(0f, 0f, this.startRect.width, this.startRect.height), this.sFPS + " FPS", this.style);
		if (this.allowDrag)
		{
			GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		}
	}

	// Token: 0x040003C8 RID: 968
	public Rect startRect;

	// Token: 0x040003C9 RID: 969
	public bool updateColor = true;

	// Token: 0x040003CA RID: 970
	public bool allowDrag = true;

	// Token: 0x040003CB RID: 971
	public float frequency = 0.5f;

	// Token: 0x040003CC RID: 972
	public int nbDecimal = 1;

	// Token: 0x040003CD RID: 973
	private float accum;

	// Token: 0x040003CE RID: 974
	private int frames;

	// Token: 0x040003CF RID: 975
	private Color color = Color.white;

	// Token: 0x040003D0 RID: 976
	private string sFPS = string.Empty;

	// Token: 0x040003D1 RID: 977
	private GUIStyle style;
}
