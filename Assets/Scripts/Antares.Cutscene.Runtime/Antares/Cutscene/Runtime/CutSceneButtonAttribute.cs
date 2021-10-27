using System;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000013 RID: 19
	public class CutSceneButtonAttribute : Attribute
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00003968 File Offset: 0x00001B68
		public CutSceneButtonAttribute(CSButtonTypeEnum type)
		{
			this.a = type;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00003977 File Offset: 0x00001B77
		internal CSButtonTypeEnum buttonType
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x0400004C RID: 76
		private CSButtonTypeEnum a;
	}
}
