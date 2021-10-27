using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class FlickerLight : MonoBehaviour
{
	// Token: 0x060000F5 RID: 245 RVA: 0x0000605C File Offset: 0x0000425C
	private void Start()
	{
		this.lamp = base.gameObject;
		this.intens = this.lamp.GetComponent<Light>().intensity;
		this.range = this.lamp.GetComponent<Light>().range;
		this.lamp.GetComponent<Light>().color = this.col_Main;
		base.StartCoroutine(this.Timer());
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000060C4 File Offset: 0x000042C4
	private void LateUpdate()
	{
		if (this.loopEnd)
		{
			base.StartCoroutine(this.Timer());
		}
		this.intens = Mathf.SmoothStep(this.intens, this.randomIntens, Time.deltaTime * this.intens_Speed);
		this.range = Mathf.SmoothStep(this.range, this.randomRange, Time.deltaTime * this.range_Speed);
		this.lamp.GetComponent<Light>().intensity = this.intens;
		this.lamp.GetComponent<Light>().range = this.range;
		this.col_Main = Color.Lerp(this.col_Main, this.refCol, Time.deltaTime * this.col_Speed);
		this.lamp.GetComponent<Light>().color = this.col_Main;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00006194 File Offset: 0x00004394
	private IEnumerator Timer()
	{
		this.timung = false;
		this.randomIntens = UnityEngine.Random.Range(this.minIntens, this.maxIntens);
		this.randomRange = UnityEngine.Random.Range(this.minRange, this.maxRange);
		this.refCol = Color.Lerp(this.col_Blend1, this.col_Blend2, UnityEngine.Random.value);
		yield return new WaitForSeconds(this.lampSpeed);
		this.timung = true;
		this.randomIntens = UnityEngine.Random.Range(this.minIntens, this.maxIntens);
		this.randomRange = UnityEngine.Random.Range(this.minRange, this.maxRange);
		this.refCol = Color.Lerp(this.col_Blend1, this.col_Blend2, UnityEngine.Random.value);
		yield return new WaitForSeconds(this.lampSpeed);
		this.loopEnd = true;
		this.randomIntens = UnityEngine.Random.Range(this.minIntens, this.maxIntens);
		this.randomRange = UnityEngine.Random.Range(this.minRange, this.maxRange);
		this.refCol = Color.Lerp(this.col_Blend1, this.col_Blend2, UnityEngine.Random.value);
		yield return null;
		yield break;
	}

	// Token: 0x040000D0 RID: 208
	public float lampSpeed = 0.1f;

	// Token: 0x040000D1 RID: 209
	public float intens_Speed = 9f;

	// Token: 0x040000D2 RID: 210
	public bool timung;

	// Token: 0x040000D3 RID: 211
	public float minIntens = 0.8f;

	// Token: 0x040000D4 RID: 212
	public float maxIntens = 3.5f;

	// Token: 0x040000D5 RID: 213
	public bool loopEnd;

	// Token: 0x040000D6 RID: 214
	public float range_Speed = 12f;

	// Token: 0x040000D7 RID: 215
	public float minRange = 2.8f;

	// Token: 0x040000D8 RID: 216
	public float maxRange = 13.5f;

	// Token: 0x040000D9 RID: 217
	public Color col_Main = Color.white;

	// Token: 0x040000DA RID: 218
	public float col_Speed = 1.5f;

	// Token: 0x040000DB RID: 219
	public Color col_Blend1 = Color.yellow;

	// Token: 0x040000DC RID: 220
	public Color col_Blend2 = Color.red;

	// Token: 0x040000DD RID: 221
	private Color refCol;

	// Token: 0x040000DE RID: 222
	private float intens;

	// Token: 0x040000DF RID: 223
	private float randomIntens;

	// Token: 0x040000E0 RID: 224
	private float range;

	// Token: 0x040000E1 RID: 225
	private float randomRange;

	// Token: 0x040000E2 RID: 226
	private GameObject lamp;
}
