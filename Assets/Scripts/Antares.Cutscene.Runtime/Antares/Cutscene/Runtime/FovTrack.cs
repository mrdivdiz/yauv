using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000038 RID: 56
	public class FovTrack : MonoBehaviour, ITrack
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000D362 File Offset: 0x0000B562
		public string trackName
		{
			get
			{
				return "FOV";
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000D369 File Offset: 0x0000B569
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000D371 File Offset: 0x0000B571
		public bool IsDisabled
		{
			get
			{
				return this.isDisabled;
			}
			set
			{
				this.isDisabled = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000D37A File Offset: 0x0000B57A
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000D384 File Offset: 0x0000B584
		public object ControlledObject
		{
			get
			{
				return this.controlledObject;
			}
			set
			{
				try
				{
					this.controlledObject = (Camera)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set CameraTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		public WrapMode playMode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000D3D5 File Offset: 0x0000B5D5
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000D3DD File Offset: 0x0000B5DD
		public Color color
		{
			get
			{
				return this.trackColor;
			}
			set
			{
				this.trackColor = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000D3E6 File Offset: 0x0000B5E6
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000D3EE File Offset: 0x0000B5EE
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000D3F6 File Offset: 0x0000B5F6
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000D405 File Offset: 0x0000B605
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D412 File Offset: 0x0000B612
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D414 File Offset: 0x0000B614
		public IKeyframe CreateKey(float time)
		{
			FovTrackKey fovTrackKey = new FovTrackKey();
			fovTrackKey.Track = this;
			fovTrackKey.data = this.controlledObject.fieldOfView;
			fovTrackKey.keyTime = time;
			this.c.Add(fovTrackKey);
			this.UpdateTrack();
			return fovTrackKey;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D45C File Offset: 0x0000B65C
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			FovTrackKey fovTrackKey = new FovTrackKey();
			fovTrackKey.Track = this;
			fovTrackKey.data = ((FovTrackKey)keyframe).data;
			fovTrackKey.time = ((FovTrackKey)keyframe).time;
			this.c.Add(fovTrackKey);
			this.UpdateTrack();
			return fovTrackKey;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D4AB File Offset: 0x0000B6AB
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((FovTrackKey)keyframe);
			this.UpdateTrack();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D4C5 File Offset: 0x0000B6C5
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((FovTrackKey)keyframe);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
		public void FindLeftAndRight(float time, out FovTrackKey left, out FovTrackKey right)
		{
			left = null;
			right = null;
			if (time <= this.a)
			{
				if (this.c.Count > 0)
				{
					right = this.c[0];
				}
				return;
			}
			if (time >= this.b)
			{
				if (this.c.Count > 0)
				{
					left = this.c[this.c.Count - 1];
				}
				return;
			}
			int num = 0;
			int num2 = this.c.Count - 1;
			while (num != num2)
			{
				int num3 = (num + num2) / 2;
				if (num3 == num)
				{
					break;
				}
				if (num3 == num2)
				{
					num = num2;
					break;
				}
				if (time > this.c[num3].keyTime)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			left = this.c[num];
			right = this.c[num2];
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000D5A4 File Offset: 0x0000B7A4
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			FovTrackKey fovTrackKey;
			FovTrackKey fovTrackKey2;
			this.FindLeftAndRight(time, out fovTrackKey, out fovTrackKey2);
			if (fovTrackKey != null)
			{
				if (fovTrackKey2 != null)
				{
					float t = (time - fovTrackKey.time) / (fovTrackKey2.time - fovTrackKey.time);
					this.controlledObject.fieldOfView = Mathf.Lerp(fovTrackKey.data, fovTrackKey2.data, t);
					return;
				}
				this.controlledObject.fieldOfView = fovTrackKey.data;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D618 File Offset: 0x0000B818
		public void InitTrack()
		{
			if (this.times == null || this.fovs == null)
			{
				this.times = new float[0];
				this.fovs = new float[0];
			}
			this.c.Clear();
			for (int i = 0; i < this.times.Length; i++)
			{
				this.c.Add(new FovTrackKey
				{
					data = this.fovs[i],
					keyTime = this.times[i],
					Track = this
				});
			}
			this.UpdateTrack();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
		public void UpdateTrack()
		{
			this.times = new float[this.c.Count];
			this.fovs = new float[this.c.Count];
			this.c.Sort(new Comparison<FovTrackKey>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.times[i] = this.c[i].keyTime;
				this.fovs[i] = this.c[i].data;
			}
			if (this.c.Count == 0)
			{
				this.a = (this.b = 0f);
			}
			else
			{
				this.a = this.c[0].time;
				this.b = this.c[this.c.Count - 1].time;
			}
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			FovTrackKey fovTrackKey = (FovTrackKey)one;
			FovTrackKey fovTrackKey2 = (FovTrackKey)other;
			if (fovTrackKey.keyTime > fovTrackKey2.keyTime)
			{
				return 1;
			}
			if (fovTrackKey.keyTime < fovTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000D7E8 File Offset: 0x0000B9E8
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000D7EB File Offset: 0x0000B9EB
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000D7EE File Offset: 0x0000B9EE
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				Debug.Log("FovTrack can't show in scene. Use ITrack.IsCanShowInScene to check this.");
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000D7FC File Offset: 0x0000B9FC
		public IKeyframe GetNextKey(FovTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000D844 File Offset: 0x0000BA44
		public IKeyframe GetPreviousKey(FovTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000181 RID: 385
		public Camera controlledObject;

		// Token: 0x04000182 RID: 386
		public bool isDisabled;

		// Token: 0x04000183 RID: 387
		public WrapMode mode = WrapMode.Once;

		// Token: 0x04000184 RID: 388
		public Color trackColor = Color.white;

		// Token: 0x04000185 RID: 389
		private float a;

		// Token: 0x04000186 RID: 390
		private float b;

		// Token: 0x04000187 RID: 391
		private readonly List<FovTrackKey> c = new List<FovTrackKey>();

		// Token: 0x04000188 RID: 392
		public float[] times;

		// Token: 0x04000189 RID: 393
		public float[] fovs;
	}
}
