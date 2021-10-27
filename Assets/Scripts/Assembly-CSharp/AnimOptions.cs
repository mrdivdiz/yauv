using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
[Serializable]
public class AnimOptions
{
	// Token: 0x0600060A RID: 1546 RVA: 0x0002A844 File Offset: 0x00028A44
	public AnimOptions()
	{
		this.anim = null;
		this.speed = 1f;
		this.startTime = 0f;
		this.endTime = 1f;
		this.fadeTime = 0.2f;
		this.weight = 3f;
		this.range = 0f;
		this.hitTime = 0.5f;
		this.soundStartOfter = 0f;
		this.volume = 1f;
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0002A8C4 File Offset: 0x00028AC4
	public string Anim()
	{
		return this.anim.name;
	}

	// Token: 0x040006BF RID: 1727
	public AnimationClip anim;

	// Token: 0x040006C0 RID: 1728
	public float speed;

	// Token: 0x040006C1 RID: 1729
	public float startTime;

	// Token: 0x040006C2 RID: 1730
	public float endTime;

	// Token: 0x040006C3 RID: 1731
	public float fadeTime;

	// Token: 0x040006C4 RID: 1732
	public float weight;

	// Token: 0x040006C5 RID: 1733
	public float range;

	// Token: 0x040006C6 RID: 1734
	public float hitTime;

	// Token: 0x040006C7 RID: 1735
	public AudioClip sound;

	// Token: 0x040006C8 RID: 1736
	public AudioClip sound2;

	// Token: 0x040006C9 RID: 1737
	public float soundStartOfter;

	// Token: 0x040006CA RID: 1738
	public float volume;
}
