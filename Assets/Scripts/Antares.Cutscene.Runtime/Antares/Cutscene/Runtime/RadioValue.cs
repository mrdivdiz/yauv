using System;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000011 RID: 17
	public struct RadioValue<T>
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00003743 File Offset: 0x00001943
		public RadioValue(T value)
		{
			this = default(RadioValue<T>);
			this.a = value;
			this.b = false;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000375A File Offset: 0x0000195A
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00003769 File Offset: 0x00001969
		public T Value
		{
			get
			{
				this.b = false;
				return this.a;
			}
			set
			{
				this.b = !value.Equals(this.a);
				this.a = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003794 File Offset: 0x00001994
		public bool Changed
		{
			get
			{
				bool result = this.b;
				this.b = false;
				return result;
			}
		}

		// Token: 0x04000048 RID: 72
		private T a;

		// Token: 0x04000049 RID: 73
		private bool b;
	}
}
