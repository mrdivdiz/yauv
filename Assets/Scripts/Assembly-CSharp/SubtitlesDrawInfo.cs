using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000028 RID: 40
	public class SubtitlesDrawInfo
	{
		// Token: 0x040000CD RID: 205
		public string text;

		// Token: 0x040000CE RID: 206
		public Color color;

		// Token: 0x040000CF RID: 207
		public Font font;

		// Token: 0x040000D0 RID: 208
		public int fontSize;

		// Token: 0x040000D1 RID: 209
		public FontStyle fontStyle;

		// Token: 0x040000D2 RID: 210
		public bool useCustomPlacement;

		// Token: 0x040000D3 RID: 211
		public TextAnchor textAnchor = TextAnchor.LowerCenter;

		// Token: 0x040000D4 RID: 212
		public Vector2 customMarginX = new Vector2(0.01f, 0.01f);

		// Token: 0x040000D5 RID: 213
		public Vector2 customMarginY = new Vector2(0.1f, 0.1f);

		// Token: 0x040000D6 RID: 214
		public bool useCustomStyle;
	}
}
