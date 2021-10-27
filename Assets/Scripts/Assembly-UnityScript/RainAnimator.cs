using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[Serializable]
public class RainAnimator : MonoBehaviour
{
	// Token: 0x06000033 RID: 51 RVA: 0x00002604 File Offset: 0x00000804
	public RainAnimator()
	{
		this.framesPerSecond = 16f;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002618 File Offset: 0x00000818
	public virtual void Start()
	{
		RainAnimator.on = true;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002620 File Offset: 0x00000820
	public virtual void Update()
	{
		if (RainAnimator.on)
		{
			int num = (int)(Time.time * this.framesPerSecond);
			num %= this.frames.Length;
			this.GetComponent<Renderer>().material.SetTexture("_BumpMap", this.frames[num]);
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002670 File Offset: 0x00000870
	public virtual void Main()
	{
	}

	// Token: 0x0400000C RID: 12
	public Texture2D[] frames;

	// Token: 0x0400000D RID: 13
	public float framesPerSecond;

	// Token: 0x0400000E RID: 14
	[NonSerialized]
	public static bool on;
}
