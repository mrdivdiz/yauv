using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200000D RID: 13
	public class ClippingPlanesTrackKey : IKeyframe
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000032FC File Offset: 0x000014FC
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003304 File Offset: 0x00001504
		public ClippingPlanesTrack Track { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000330D File Offset: 0x0000150D
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003315 File Offset: 0x00001515
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

		// Token: 0x0600007E RID: 126 RVA: 0x00003330 File Offset: 0x00001530
		public void CaptureCurrentState()
		{
			if (this.Track != null)
			{
				Camera controlledObject = this.Track.controlledObject;
				if (controlledObject != null)
				{
					this.data = new Vector2(controlledObject.nearClipPlane, controlledObject.farClipPlane);
					this.Track.UpdateTrack();
					return;
				}
				Debug.Log("Can't call ClippingPlanesTrackKey.CaptureCurrentState() then FovTrack.ControlledObject not set!");
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000338D File Offset: 0x0000158D
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000339B File Offset: 0x0000159B
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000033A9 File Offset: 0x000015A9
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000033B6 File Offset: 0x000015B6
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000033C4 File Offset: 0x000015C4
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000033D2 File Offset: 0x000015D2
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000033E0 File Offset: 0x000015E0
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000033E3 File Offset: 0x000015E3
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000033EA File Offset: 0x000015EA
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000033F6 File Offset: 0x000015F6
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000030 RID: 48
		public float keyTime;

		// Token: 0x04000031 RID: 49
		public Vector2 data;

		// Token: 0x04000032 RID: 50
		public float smooth;

		// Token: 0x04000033 RID: 51
		[CompilerGenerated]
		private ClippingPlanesTrack a;
	}
}
