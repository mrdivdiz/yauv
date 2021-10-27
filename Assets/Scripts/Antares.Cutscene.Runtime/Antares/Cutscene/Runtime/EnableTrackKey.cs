using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000020 RID: 32
	public class EnableTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00005E78 File Offset: 0x00004078
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00005EA4 File Offset: 0x000040A4
		public EnableTrack Track
		{
			get
			{
				if (this.a == null)
				{
					this.a = base.transform.parent.GetComponent<EnableTrack>();
				}
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00005EAD File Offset: 0x000040AD
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00005EB5 File Offset: 0x000040B5
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.Track.UpdateTrack();
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005ECE File Offset: 0x000040CE
		public void CaptureCurrentState()
		{
			//this.Track != null;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005EDD File Offset: 0x000040DD
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005EEB File Offset: 0x000040EB
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005EF9 File Offset: 0x000040F9
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005F06 File Offset: 0x00004106
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005F14 File Offset: 0x00004114
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005F22 File Offset: 0x00004122
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00005F30 File Offset: 0x00004130
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00005F33 File Offset: 0x00004133
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00005F3A File Offset: 0x0000413A
		public float Length
		{
			get
			{
				return 0f;
			}
			set
			{
				Debug.Log("Length");
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00005F46 File Offset: 0x00004146
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000092 RID: 146
		public float keyTime;

		// Token: 0x04000093 RID: 147
		public bool enable = true;

		// Token: 0x04000094 RID: 148
		private EnableTrack a;
	}
}
