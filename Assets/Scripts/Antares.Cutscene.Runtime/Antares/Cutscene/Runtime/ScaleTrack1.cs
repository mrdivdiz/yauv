using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200001A RID: 26
	public class ScaleTrack1 : MonoBehaviour, ITrack
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000440A File Offset: 0x0000260A
		public string trackName
		{
			get
			{
				return "scale";
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004411 File Offset: 0x00002611
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004419 File Offset: 0x00002619
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004422 File Offset: 0x00002622
		// (set) Token: 0x060000FE RID: 254 RVA: 0x0000442C File Offset: 0x0000262C
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
					Debug.Log("Faild to set ScaleTracl.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000446C File Offset: 0x0000266C
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004474 File Offset: 0x00002674
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000447D File Offset: 0x0000267D
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00004485 File Offset: 0x00002685
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000448E File Offset: 0x0000268E
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004496 File Offset: 0x00002696
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000449E File Offset: 0x0000269E
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000044AD File Offset: 0x000026AD
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000044BA File Offset: 0x000026BA
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000044BC File Offset: 0x000026BC
		public IKeyframe CreateKey(float time)
		{
			ScaleTrackKey1 scaleTrackKey = new ScaleTrackKey1();
			scaleTrackKey.Track = this;
			scaleTrackKey.data = this.controlledObject.localScale;
			scaleTrackKey.keyTime = time;
			this.c.Add(scaleTrackKey);
			this.UpdateTrack();
			return scaleTrackKey;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004504 File Offset: 0x00002704
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			ScaleTrackKey1 scaleTrackKey = new ScaleTrackKey1();
			scaleTrackKey.Track = this;
			scaleTrackKey.data = ((ScaleTrackKey1)keyframe).data;
			scaleTrackKey.time = ((ScaleTrackKey1)keyframe).time;
			this.c.Add(scaleTrackKey);
			this.UpdateTrack();
			return scaleTrackKey;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004553 File Offset: 0x00002753
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((ScaleTrackKey1)keyframe);
			this.UpdateTrack();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000456D File Offset: 0x0000276D
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((ScaleTrackKey1)keyframe);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004580 File Offset: 0x00002780
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			this.controlledObject.localScale = new Vector3(this.d.Evaluate(time), this.e.Evaluate(time), this.f.Evaluate(time));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000045D0 File Offset: 0x000027D0
		public void InitTrack()
		{
			if (this.times == null || this.scales == null || this.smooths == null)
			{
				this.times = new float[0];
				this.scales = new Vector3[0];
				this.smooths = new float[0];
			}
			this.c.Clear();
			for (int i = 0; i < this.times.Length; i++)
			{
				this.c.Add(new ScaleTrackKey1
				{
					data = new Vector3(this.scales[i].x, this.scales[i].y, this.scales[i].z),
					keyTime = this.times[i],
					Track = this,
					smooth = this.smooths[i]
				});
			}
			this.UpdateTrack();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000046B3 File Offset: 0x000028B3
		public AnimationCurve GetCurveX()
		{
			return this.d;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000046BB File Offset: 0x000028BB
		public AnimationCurve GetCurveY()
		{
			return this.e;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000046C3 File Offset: 0x000028C3
		public AnimationCurve GetCurveZ()
		{
			return this.f;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000046CC File Offset: 0x000028CC
		public void UpdateTrack()
		{
			this.times = new float[this.c.Count];
			this.scales = new Vector3[this.c.Count];
			this.smooths = new float[this.c.Count];
			this.c.Sort(new Comparison<ScaleTrackKey1>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.times[i] = this.c[i].keyTime;
				this.scales[i] = new Vector3(this.c[i].data.x, this.c[i].data.y, this.c[i].data.z);
				this.smooths[i] = this.c[i].smooth;
			}
			Keyframe[] array = new Keyframe[this.c.Count];
			Keyframe[] array2 = new Keyframe[this.c.Count];
			Keyframe[] array3 = new Keyframe[this.c.Count];
			for (int j = 0; j < this.c.Count; j++)
			{
				array[j].time = (array2[j].time = (array3[j].time = this.times[j]));
				array[j].value = this.scales[j].x;
				array2[j].value = this.scales[j].y;
				array3[j].value = this.scales[j].z;
			}
			this.d.keys = array;
			this.e.keys = array2;
			this.f.keys = array3;
			for (int k = 0; k < this.c.Count; k++)
			{
				this.d.SmoothTangents(k, this.smooths[k]);
				this.e.SmoothTangents(k, this.smooths[k]);
				this.f.SmoothTangents(k, this.smooths[k]);
			}
			this.d.postWrapMode = (this.d.preWrapMode = this.playMode);
			this.e.postWrapMode = (this.e.preWrapMode = this.playMode);
			this.f.postWrapMode = (this.f.preWrapMode = this.playMode);
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

		// Token: 0x06000112 RID: 274 RVA: 0x00004A14 File Offset: 0x00002C14
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			ScaleTrackKey1 scaleTrackKey = (ScaleTrackKey1)one;
			ScaleTrackKey1 scaleTrackKey2 = (ScaleTrackKey1)other;
			if (scaleTrackKey.keyTime > scaleTrackKey2.keyTime)
			{
				return 1;
			}
			if (scaleTrackKey.keyTime < scaleTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004A50 File Offset: 0x00002C50
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00004A53 File Offset: 0x00002C53
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00004A56 File Offset: 0x00002C56
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				Debug.Log("ScaleTrack can't show in scene. Use ITrack.IsCanShowInScene to check this.");
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004A64 File Offset: 0x00002C64
		public IKeyframe GetNextKey(ScaleTrackKey1 key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004AAC File Offset: 0x00002CAC
		public IKeyframe GetPreviousKey(ScaleTrackKey1 key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000068 RID: 104
		public Transform controlledObject;

		// Token: 0x04000069 RID: 105
		public bool isDisabled;

		// Token: 0x0400006A RID: 106
		public WrapMode mode = WrapMode.Once;

		// Token: 0x0400006B RID: 107
		public Color trackColor = Color.white;

		// Token: 0x0400006C RID: 108
		private float a;

		// Token: 0x0400006D RID: 109
		private float b;

		// Token: 0x0400006E RID: 110
		private readonly List<ScaleTrackKey1> c = new List<ScaleTrackKey1>();

		// Token: 0x0400006F RID: 111
		public float[] times;

		// Token: 0x04000070 RID: 112
		public Vector3[] scales;

		// Token: 0x04000071 RID: 113
		public float[] smooths;

		// Token: 0x04000072 RID: 114
		private AnimationCurve d = new AnimationCurve();

		// Token: 0x04000073 RID: 115
		private AnimationCurve e = new AnimationCurve();

		// Token: 0x04000074 RID: 116
		private AnimationCurve f = new AnimationCurve();
	}
}
