using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000024 RID: 36
	public class CSDestroyObjectAterPlaySoundInternal : MonoBehaviour
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00006D38 File Offset: 0x00004F38
		public void Update()
		{
			if (this.trargetSoundPosition != null)
			{
				base.transform.position = this.trargetSoundPosition.position;
			}
			if (this.paused)
			{
				return;
			}
			if (Application.isPlaying)
			{
				this.timeOffset -= Time.deltaTime;
			}
			else
			{
				this.timeOffset -= 0.01f;
			}
			if (!this.started)
			{
				if (this.timeOffset < 0f)
				{
					this.started = true;
					base.GetComponent<AudioSource>().Play();
					return;
				}
			}
			else if (base.GetComponent<AudioSource>() == null || !base.GetComponent<AudioSource>().isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
		}

		// Token: 0x040000C0 RID: 192
		public Transform trargetSoundPosition;

		// Token: 0x040000C1 RID: 193
		public bool started = true;

		// Token: 0x040000C2 RID: 194
		public float timeOffset;

		// Token: 0x040000C3 RID: 195
		public bool paused;

		// Token: 0x040000C4 RID: 196
		private bool a;
	}
}
