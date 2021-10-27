using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000019 RID: 25
	public class ScaleTrackKey1 : IKeyframe
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004310 File Offset: 0x00002510
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004318 File Offset: 0x00002518
		public ScaleTrack1 Track { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004321 File Offset: 0x00002521
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004329 File Offset: 0x00002529
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

		// Token: 0x060000EE RID: 238 RVA: 0x00004344 File Offset: 0x00002544
		public void CaptureCurrentState()
		{
			if (this.Track != null)
			{
				Transform controlledObject = this.Track.controlledObject;
				if (controlledObject != null)
				{
					this.data = controlledObject.localScale;
					this.Track.UpdateTrack();
					return;
				}
				Debug.Log("Can't call ScaleTrackKey.CaptureCurrentState() then ScaleTrack.ControlledObject not set!");
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004396 File Offset: 0x00002596
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000043A4 File Offset: 0x000025A4
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000043B2 File Offset: 0x000025B2
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000043BF File Offset: 0x000025BF
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000043CD File Offset: 0x000025CD
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000043DB File Offset: 0x000025DB
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000043E9 File Offset: 0x000025E9
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000043EC File Offset: 0x000025EC
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000043F3 File Offset: 0x000025F3
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000043FF File Offset: 0x000025FF
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000064 RID: 100
		public float keyTime;

		// Token: 0x04000065 RID: 101
		public Vector3 data;

		// Token: 0x04000066 RID: 102
		public float smooth;

		// Token: 0x04000067 RID: 103
		[CompilerGenerated]
		private ScaleTrack1 a;
	}
}
