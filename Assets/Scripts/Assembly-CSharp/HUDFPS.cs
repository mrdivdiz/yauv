using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class HUDFPS : MonoBehaviour
{
	// Token: 0x06000A51 RID: 2641 RVA: 0x00070340 File Offset: 0x0006E540
	private void Start()
	{
		if (!base.GetComponent<GUIText>())
		{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			base.enabled = false;
			return;
		}
		this.timeleft = this.updateInterval;
		if (!HUDFPS.showFPS)
		{
			base.GetComponent<GUIText>().text = string.Empty;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00070398 File Offset: 0x0006E598
	private void Update()
	{
		if (!HUDFPS.showFPS)
		{
			if (base.GetComponent<GUIText>().text != string.Empty)
			{
				base.GetComponent<GUIText>().text = string.Empty;
			}
			return;
		}
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			string text = string.Format("{0:F2} FPS", num);
			base.GetComponent<GUIText>().text = text;
			if (num < 29f)
			{
				base.GetComponent<GUIText>().material.color = Color.black;
			}
			else if (num < 10f)
			{
				base.GetComponent<GUIText>().material.color = Color.red;
			}
			else
			{
				base.GetComponent<GUIText>().material.color = Color.green;
			}
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x0400107B RID: 4219
	public float updateInterval = 0.5f;

	// Token: 0x0400107C RID: 4220
	private float accum;

	// Token: 0x0400107D RID: 4221
	private int frames;

	// Token: 0x0400107E RID: 4222
	private float timeleft;

	// Token: 0x0400107F RID: 4223
	public static bool showFPS;
}
