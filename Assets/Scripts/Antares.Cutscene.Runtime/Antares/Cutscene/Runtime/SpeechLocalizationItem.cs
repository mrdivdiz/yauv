using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class SpeechLocalizationItem
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000041BB File Offset: 0x000023BB
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

		// Token: 0x0400005A RID: 90
		public string locale;

		// Token: 0x0400005B RID: 91
		public AudioClip speech;

		// Token: 0x0400005C RID: 92
		private string a;
	}
}
