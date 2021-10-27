using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000007 RID: 7
	public interface ITrack
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8
		string trackName { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9
		// (set) Token: 0x0600000A RID: 10
		object ControlledObject { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11
		// (set) Token: 0x0600000C RID: 12
		WrapMode playMode { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13
		// (set) Token: 0x0600000E RID: 14
		Color color { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15
		float startTime { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16
		float endTime { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17
		float length { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18
		// (set) Token: 0x06000013 RID: 19
		bool IsDisabled { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20
		IKeyframe[] Keys { get; }

		// Token: 0x06000015 RID: 21
		void SelectKeyframe(IKeyframe keyframe);

		// Token: 0x06000016 RID: 22
		IKeyframe CreateKey(float time);

		// Token: 0x06000017 RID: 23
		IKeyframe DuplicateKey(IKeyframe keyframe);

		// Token: 0x06000018 RID: 24
		void DeleteKeyframe(IKeyframe keyframe);

		// Token: 0x06000019 RID: 25
		int GetKeyframeIndex(IKeyframe keyframe);

		// Token: 0x0600001A RID: 26
		void EvaluateTime(float time);

		// Token: 0x0600001B RID: 27
		void InitTrack();

		// Token: 0x0600001C RID: 28
		void UpdateTrack();

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29
		bool IsCanShowInScene { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001E RID: 30
		// (set) Token: 0x0600001F RID: 31
		bool ShowInScene { get; set; }
	}
}
