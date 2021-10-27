using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200000B RID: 11
	public abstract class CutsceneObjectBase : MonoBehaviour
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005A RID: 90
		// (set) Token: 0x0600005B RID: 91
		public abstract object ControlledObject { get; set; }

		// Token: 0x0600005C RID: 92
		public abstract void Init();

		// Token: 0x0600005D RID: 93
		public abstract void InitDefaultTracks();

		// Token: 0x0600005E RID: 94
		public abstract void MoveTrackUp(ITrack track);

		// Token: 0x0600005F RID: 95
		public abstract void MoveTrackDown(ITrack track);

		// Token: 0x06000060 RID: 96
		public abstract ITrack[] GetTracks();

		// Token: 0x06000061 RID: 97
		public abstract ITrack CreateTrack(string trackName);

		// Token: 0x06000062 RID: 98
		public abstract bool DeleteTrack(ITrack track);

		// Token: 0x06000063 RID: 99
		public abstract string[] GetSupportedTrackNames();

		// Token: 0x06000064 RID: 100
		public abstract string[] GetSupportedTrackNamesToCreate();

		// Token: 0x06000065 RID: 101
		public abstract bool IsTrackNameSupported(string trackName);

		// Token: 0x06000066 RID: 102
		public abstract bool IsTrackNameCreated(string trackName);

		// Token: 0x06000067 RID: 103
		public abstract void EvaluateTime(float time);

		// Token: 0x0400002C RID: 44
		public bool foldout;

		// Token: 0x0400002D RID: 45
		public bool createTrackFoldout;
	}
}
