using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000021 RID: 33
	public class RotationTrack1 : MonoBehaviour, ITrack
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00005F58 File Offset: 0x00004158
		public string trackName
		{
			get
			{
				return "rotation";
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00005F5F File Offset: 0x0000415F
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00005F67 File Offset: 0x00004167
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

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00005F70 File Offset: 0x00004170
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00005F78 File Offset: 0x00004178
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
					this.controlledObject = (Transform)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set RotationTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00005FB8 File Offset: 0x000041B8
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00005FC0 File Offset: 0x000041C0
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00005FC9 File Offset: 0x000041C9
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00005FD1 File Offset: 0x000041D1
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00005FDA File Offset: 0x000041DA
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00005FE2 File Offset: 0x000041E2
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00005FEA File Offset: 0x000041EA
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005FF9 File Offset: 0x000041F9
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006006 File Offset: 0x00004206
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006008 File Offset: 0x00004208
		public IKeyframe CreateKey(float time)
		{
			RotationTrackKey1 rotationTrackKey = new RotationTrackKey1();
			rotationTrackKey.Track = this;
			rotationTrackKey.data = this.controlledObject.rotation;
			rotationTrackKey.keyTime = time;
			this.c.Add(rotationTrackKey);
			if (this.updateOnAddKeys)
			{
				this.UpdateTrack();
			}
			return rotationTrackKey;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006058 File Offset: 0x00004258
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			RotationTrackKey1 rotationTrackKey = new RotationTrackKey1();
			rotationTrackKey.Track = this;
			rotationTrackKey.data = ((RotationTrackKey1)keyframe).data;
			rotationTrackKey.keyTime = ((RotationTrackKey1)keyframe).time;
			this.c.Add(rotationTrackKey);
			if (this.updateOnAddKeys)
			{
				this.UpdateTrack();
			}
			return rotationTrackKey;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000060AF File Offset: 0x000042AF
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((RotationTrackKey1)keyframe);
			this.UpdateTrack();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000060C9 File Offset: 0x000042C9
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((RotationTrackKey1)keyframe);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000060DC File Offset: 0x000042DC
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			Vector3 forward = new Vector3(this.d.Evaluate(time), this.e.Evaluate(time), this.f.Evaluate(time));
			Vector3 upwards = new Vector3(this.g.Evaluate(time), this.h.Evaluate(time), this.i.Evaluate(time));
			if (this.worldSpace)
			{
				this.controlledObject.rotation = Quaternion.LookRotation(forward, upwards);
				return;
			}
			this.controlledObject.localRotation = Quaternion.LookRotation(forward, upwards);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000617C File Offset: 0x0000437C
		public void InitTrack()
		{
			if (this.times == null || this.rotations == null || this.smooths == null)
			{
				this.times = new float[0];
				this.rotations = new Vector4[0];
				this.smooths = new float[0];
			}
			if (this.corners == null)
			{
				this.corners = new bool[this.times.Length];
			}
			if (this.corners.Length != this.times.Length)
			{
				this.corners = new bool[this.times.Length];
			}
			this.c.Clear();
			for (int i = 0; i < this.times.Length; i++)
			{
				this.c.Add(new RotationTrackKey1
				{
					data = new Quaternion(this.rotations[i].x, this.rotations[i].y, this.rotations[i].z, this.rotations[i].w),
					keyTime = this.times[i],
					Track = this,
					smooth = this.smooths[i],
					corner = this.corners[i]
				});
			}
			this.UpdateTrack();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000062C4 File Offset: 0x000044C4
		public void UpdateTrack()
		{
			this.times = new float[this.c.Count];
			this.rotations = new Vector4[this.c.Count];
			this.smooths = new float[this.c.Count];
			this.corners = new bool[this.c.Count];
			this.c.Sort(new Comparison<RotationTrackKey1>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.times[i] = this.c[i].keyTime;
				this.rotations[i] = new Vector4(this.c[i].data.x, this.c[i].data.y, this.c[i].data.z, this.c[i].data.w);
				this.smooths[i] = this.c[i].smooth;
				this.corners[i] = this.c[i].corner;
			}
			List<Keyframe> list = new List<Keyframe>();
			List<Keyframe> list2 = new List<Keyframe>();
			List<Keyframe> list3 = new List<Keyframe>();
			List<Keyframe> list4 = new List<Keyframe>();
			List<Keyframe> list5 = new List<Keyframe>();
			List<Keyframe> list6 = new List<Keyframe>();
			List<float> list7 = new List<float>();
			for (int j = 0; j < this.c.Count; j++)
			{
				Quaternion rotation = new Quaternion(this.rotations[j].x, this.rotations[j].y, this.rotations[j].z, this.rotations[j].w);
				Vector3 vector = rotation * Vector3.forward;
				Vector3 vector2 = rotation * Vector3.up;
				list7.Add(this.c[j].smooth);
				list.Add(new Keyframe(this.times[j], vector.x));
				list2.Add(new Keyframe(this.times[j], vector.y));
				list3.Add(new Keyframe(this.times[j], vector.z));
				list4.Add(new Keyframe(this.times[j], vector2.x));
				list5.Add(new Keyframe(this.times[j], vector2.y));
				list6.Add(new Keyframe(this.times[j], vector2.z));
				if (this.c[j].corner)
				{
					list.Add(new Keyframe(this.times[j], vector.x));
					list2.Add(new Keyframe(this.times[j], vector.y));
					list3.Add(new Keyframe(this.times[j], vector.z));
					list4.Add(new Keyframe(this.times[j], vector2.x));
					list5.Add(new Keyframe(this.times[j], vector2.y));
					list6.Add(new Keyframe(this.times[j], vector2.z));
					list7.Add(this.c[j].smooth);
				}
			}
			this.d.keys = list.ToArray();
			this.e.keys = list2.ToArray();
			this.f.keys = list3.ToArray();
			this.g.keys = list4.ToArray();
			this.h.keys = list5.ToArray();
			this.i.keys = list6.ToArray();
			for (int k = 0; k < list.Count; k++)
			{
				this.d.SmoothTangents(k, list7[k]);
				this.e.SmoothTangents(k, list7[k]);
				this.f.SmoothTangents(k, list7[k]);
				this.g.SmoothTangents(k, list7[k]);
				this.h.SmoothTangents(k, list7[k]);
				this.i.SmoothTangents(k, list7[k]);
			}
			this.d.postWrapMode = (this.d.preWrapMode = this.playMode);
			this.e.postWrapMode = (this.e.preWrapMode = this.playMode);
			this.f.postWrapMode = (this.f.preWrapMode = this.playMode);
			this.g.postWrapMode = (this.g.preWrapMode = this.playMode);
			this.h.postWrapMode = (this.h.preWrapMode = this.playMode);
			this.i.postWrapMode = (this.i.preWrapMode = this.playMode);
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

		// Token: 0x0600018D RID: 397 RVA: 0x0000689C File Offset: 0x00004A9C
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			RotationTrackKey1 rotationTrackKey = (RotationTrackKey1)one;
			RotationTrackKey1 rotationTrackKey2 = (RotationTrackKey1)other;
			if (rotationTrackKey.keyTime > rotationTrackKey2.keyTime)
			{
				return 1;
			}
			if (rotationTrackKey.keyTime < rotationTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000068D8 File Offset: 0x00004AD8
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000068DB File Offset: 0x00004ADB
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000068DE File Offset: 0x00004ADE
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				Debug.Log("RotationTrack can't show in scene. Use ITrack.IsCanShowInScene to check this.");
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000068EC File Offset: 0x00004AEC
		public IKeyframe GetNextKey(RotationTrackKey1 key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006934 File Offset: 0x00004B34
		public IKeyframe GetPreviousKey(RotationTrackKey1 key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000095 RID: 149
		public Transform controlledObject;

		// Token: 0x04000096 RID: 150
		public bool worldSpace = true;

		// Token: 0x04000097 RID: 151
		public bool enableSnap = true;

		// Token: 0x04000098 RID: 152
		public bool updateOnAddKeys = true;

		// Token: 0x04000099 RID: 153
		public bool isDisabled;

		// Token: 0x0400009A RID: 154
		public WrapMode mode = WrapMode.Once;

		// Token: 0x0400009B RID: 155
		public Color trackColor = Color.white;

		// Token: 0x0400009C RID: 156
		private float a;

		// Token: 0x0400009D RID: 157
		private float b;

		// Token: 0x0400009E RID: 158
		private readonly List<RotationTrackKey1> c = new List<RotationTrackKey1>();

		// Token: 0x0400009F RID: 159
		public float[] times;

		// Token: 0x040000A0 RID: 160
		public Vector4[] rotations;

		// Token: 0x040000A1 RID: 161
		public float[] smooths;

		// Token: 0x040000A2 RID: 162
		public bool[] corners;

		// Token: 0x040000A3 RID: 163
		private AnimationCurve d = new AnimationCurve();

		// Token: 0x040000A4 RID: 164
		private AnimationCurve e = new AnimationCurve();

		// Token: 0x040000A5 RID: 165
		private AnimationCurve f = new AnimationCurve();

		// Token: 0x040000A6 RID: 166
		private AnimationCurve g = new AnimationCurve();

		// Token: 0x040000A7 RID: 167
		private AnimationCurve h = new AnimationCurve();

		// Token: 0x040000A8 RID: 168
		private AnimationCurve i = new AnimationCurve();
	}
}
