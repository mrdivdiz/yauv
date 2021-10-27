using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200000F RID: 15
	public class RotationTrackKey1 : IKeyframe
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000034FA File Offset: 0x000016FA
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003502 File Offset: 0x00001702
		public RotationTrack1 Track { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009C RID: 156 RVA: 0x0000350B File Offset: 0x0000170B
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003514 File Offset: 0x00001714
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				if (this.Track != null)
				{
					if (this.Track.enableSnap)
					{
						this.keyTime = CSCore.a(value);
					}
					else
					{
						this.keyTime = value;
					}
				}
				else
				{
					this.keyTime = CSCore.a(value);
				}
				this.Track.UpdateTrack();
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000356C File Offset: 0x0000176C
		public void CaptureCurrentState()
		{
			if (this.Track != null)
			{
				Transform controlledObject = this.Track.controlledObject;
				if (controlledObject != null)
				{
					this.data = controlledObject.rotation;
					this.Track.UpdateTrack();
					return;
				}
				Debug.Log("Can't call RotationTrackKey.CaptureCurrentState() then RotationTrack.ControlledObject not set!");
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000035BE File Offset: 0x000017BE
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000035CC File Offset: 0x000017CC
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000035DA File Offset: 0x000017DA
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000035E7 File Offset: 0x000017E7
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000035F5 File Offset: 0x000017F5
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003603 File Offset: 0x00001803
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003611 File Offset: 0x00001811
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003614 File Offset: 0x00001814
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000361B File Offset: 0x0000181B
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003627 File Offset: 0x00001827
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000038 RID: 56
		public float keyTime;

		// Token: 0x04000039 RID: 57
		public Quaternion data;

		// Token: 0x0400003A RID: 58
		public float smooth;

		// Token: 0x0400003B RID: 59
		public bool corner;

		// Token: 0x0400003C RID: 60
		[CompilerGenerated]
		private RotationTrack1 a;
	}
}
