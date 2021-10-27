using System;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000009 RID: 9
	public interface IKeyframe
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003D RID: 61
		// (set) Token: 0x0600003E RID: 62
		float time { get; set; }

		// Token: 0x0600003F RID: 63
		void CaptureCurrentState();

		// Token: 0x06000040 RID: 64
		void DeleteThis();

		// Token: 0x06000041 RID: 65
		void SelectThis();

		// Token: 0x06000042 RID: 66
		void UpdateTrack();

		// Token: 0x06000043 RID: 67
		IKeyframe GetNextKey();

		// Token: 0x06000044 RID: 68
		IKeyframe GetPreviousKey();

		// Token: 0x06000045 RID: 69
		int GetIndex();

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000046 RID: 70
		KeyType keyType { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000047 RID: 71
		bool HaveLength { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000048 RID: 72
		// (set) Token: 0x06000049 RID: 73
		float Length { get; set; }
	}
}
