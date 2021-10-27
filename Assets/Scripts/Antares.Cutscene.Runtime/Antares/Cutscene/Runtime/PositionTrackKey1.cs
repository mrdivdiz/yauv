using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200001B RID: 27
	public class PositionTrackKey1 : MonoBehaviour, IKeyframe
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00004B41 File Offset: 0x00002D41
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00004B68 File Offset: 0x00002D68
		public Vector3 data
		{
			get
			{
				if (this.a == null)
				{
					this.a = base.transform;
				}
				return this.a.position;
			}
			set
			{
				if (this.a == null)
				{
					this.a = base.transform;
				}
				base.transform.position = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00004B90 File Offset: 0x00002D90
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00004BBC File Offset: 0x00002DBC
		public PositionTrack1 Track
		{
			get
			{
				if (this.b == null)
				{
					this.b = base.transform.parent.GetComponent<PositionTrack1>();
				}
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00004BC5 File Offset: 0x00002DC5
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00004BD0 File Offset: 0x00002DD0
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				if (this.b != null)
				{
					if (this.b.enableSnap)
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

		// Token: 0x0600011F RID: 287 RVA: 0x00004C28 File Offset: 0x00002E28
		public void CaptureCurrentState()
		{
			if (this.Track != null)
			{
				Transform controlledObject = this.Track.controlledObject;
				if (controlledObject != null)
				{
					this.data = controlledObject.position;
					this.Track.UpdateTrack();
					return;
				}
				Debug.Log("Can't call RotationTrackKey.CaptureCurrentState() then RotationTrack.ControlledObject not set!");
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004C7A File Offset: 0x00002E7A
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004C88 File Offset: 0x00002E88
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004C96 File Offset: 0x00002E96
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004CA3 File Offset: 0x00002EA3
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004CB1 File Offset: 0x00002EB1
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004CBF File Offset: 0x00002EBF
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004CCD File Offset: 0x00002ECD
		public void OnDrawGizmos()
		{
			if (this.Track.ShowInScene)
			{
				Gizmos.DrawIcon(base.transform.position, "Cutscene Ed/point.png");
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004CF1 File Offset: 0x00002EF1
		public void OnDrawGizmosSelected()
		{
			if (this.Track.ShowInScene)
			{
				Gizmos.DrawIcon(base.transform.position, "Cutscene Ed/point_selected.png");
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00004D15 File Offset: 0x00002F15
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00004D18 File Offset: 0x00002F18
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00004D1F File Offset: 0x00002F1F
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004D2B File Offset: 0x00002F2B
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000075 RID: 117
		public float keyTime;

		// Token: 0x04000076 RID: 118
		private Transform a;

		// Token: 0x04000077 RID: 119
		public float smooth;

		// Token: 0x04000078 RID: 120
		public float offset;

		// Token: 0x04000079 RID: 121
		public bool corner;

		// Token: 0x0400007A RID: 122
		public string tip = "";

		// Token: 0x0400007B RID: 123
		private PositionTrack1 b;
	}
}
