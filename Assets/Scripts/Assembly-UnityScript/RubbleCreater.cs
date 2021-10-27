using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
[AddComponentMenu("ExplosionScripts/BetCopyer")]
[Serializable]
public class RubbleCreater : MonoBehaviour
{
	// Token: 0x06000129 RID: 297 RVA: 0x00007264 File Offset: 0x00005464
	public RubbleCreater()
	{
		this.vel = (float)20;
		this.up = (float)20;
		this.amount = 20;
		this.amountRandom = 5;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00007290 File Offset: 0x00005490
	public virtual void Start()
	{
		this.CreateObject();
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00007298 File Offset: 0x00005498
	public virtual void CreateObject()
	{
		this.amount += UnityEngine.Random.Range(-this.amountRandom, this.amountRandom);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int i = 0;
		for (i = 0; i < this.amount; i++)
		{
			Rigidbody rigidbody = (Rigidbody)UnityEngine.Object.Instantiate(this.bits, this.transform.position, this.transform.rotation);
			num = (int)UnityEngine.Random.Range(-this.vel, this.vel);
			num2 = (int)UnityEngine.Random.Range((float)5, this.up);
			num3 = (int)UnityEngine.Random.Range(-this.vel, this.vel);
			if (this.useUp)
			{
				rigidbody.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection((float)num, UnityEngine.Random.Range(this.up / (float)2, this.up), (float)num3);
			}
			else
			{
				rigidbody.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection((float)num, UnityEngine.Random.Range(-this.up, this.up), (float)num3);
			}
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000073C8 File Offset: 0x000055C8
	public virtual void Main()
	{
	}

	// Token: 0x0400010B RID: 267
	public Rigidbody bits;

	// Token: 0x0400010C RID: 268
	public float vel;

	// Token: 0x0400010D RID: 269
	public float up;

	// Token: 0x0400010E RID: 270
	public int amount;

	// Token: 0x0400010F RID: 271
	public int amountRandom;

	// Token: 0x04000110 RID: 272
	public bool useUp;

	// Token: 0x04000111 RID: 273
	private AudioClip soundClip;
}
