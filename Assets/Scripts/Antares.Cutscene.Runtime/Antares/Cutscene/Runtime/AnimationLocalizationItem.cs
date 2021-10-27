using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class AnimationLocalizationItem
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00006DFC File Offset: 0x00004FFC
		internal string LowerdLocale
		{
			get
			{
				if (this.a == null)
				{
					this.a = (this.locale ?? "").ToLower();
				}
				return this.a;
			}
		}

		// Token: 0x040000CA RID: 202
		public string locale;

		// Token: 0x040000CB RID: 203
		public AnimationClip anim;

		// Token: 0x040000CC RID: 204
		private string a;
	}
}
