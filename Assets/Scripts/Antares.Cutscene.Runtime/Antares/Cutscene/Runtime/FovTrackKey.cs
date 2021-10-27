using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200000E RID: 14
	public class FovTrackKey : IKeyframe
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003401 File Offset: 0x00001601
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003409 File Offset: 0x00001609
		public FovTrack Track { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003412 File Offset: 0x00001612
		// (set) Token: 0x0600008D RID: 141 RVA: 0x0000341A File Offset: 0x0000161A
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

		// Token: 0x0600008E RID: 142 RVA: 0x00003434 File Offset: 0x00001634
		public void CaptureCurrentState()
		{
			if (this.Track != null)
			{
				Camera controlledObject = this.Track.controlledObject;
				if (controlledObject != null)
				{
					this.data = controlledObject.fieldOfView;
					this.Track.UpdateTrack();
					return;
				}
				Debug.Log("Can't call FovTrackKey.CaptureCurrentState() then FovTrack.ControlledObject not set!");
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003486 File Offset: 0x00001686
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003494 File Offset: 0x00001694
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000034A2 File Offset: 0x000016A2
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000034AF File Offset: 0x000016AF
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000034BD File Offset: 0x000016BD
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000034CB File Offset: 0x000016CB
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000034D9 File Offset: 0x000016D9
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000034DC File Offset: 0x000016DC
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000034E3 File Offset: 0x000016E3
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000034EF File Offset: 0x000016EF
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000034 RID: 52
		public float keyTime;

		// Token: 0x04000035 RID: 53
		public float data;

		// Token: 0x04000036 RID: 54
		public float smooth;

		// Token: 0x04000037 RID: 55
		[CompilerGenerated]
		private FovTrack a;
	}
}
