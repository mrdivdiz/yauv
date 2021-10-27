using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200001E RID: 30
	public class CutsceneTrack : MonoBehaviour, ITrack
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00004D41 File Offset: 0x00002F41
		public string trackName
		{
			get
			{
				return this.cutsceneTrackName;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00004D49 File Offset: 0x00002F49
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00004D51 File Offset: 0x00002F51
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004D5A File Offset: 0x00002F5A
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00004D64 File Offset: 0x00002F64
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
					this.controlledObject = (CSComponent)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set CutsceneTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004DA4 File Offset: 0x00002FA4
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00004DAC File Offset: 0x00002FAC
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

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004DB5 File Offset: 0x00002FB5
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00004DBD File Offset: 0x00002FBD
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00004DC6 File Offset: 0x00002FC6
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00004DCE File Offset: 0x00002FCE
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00004DD6 File Offset: 0x00002FD6
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00004DE5 File Offset: 0x00002FE5
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004DF2 File Offset: 0x00002FF2
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004DF4 File Offset: 0x00002FF4
		public IKeyframe CreateKey(float time)
		{
			CutsceneTrackKey cutsceneTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<CutsceneTrackKey>();
			cutsceneTrackKey.ctTrack = this;
			cutsceneTrackKey.keyTime = time;
			this.c.Add(cutsceneTrackKey);
			this.UpdateTrack();
			return cutsceneTrackKey;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004E48 File Offset: 0x00003048
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			CutsceneTrackKey cutsceneTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<CutsceneTrackKey>();
			cutsceneTrackKey.ctTrack = this;
			cutsceneTrackKey.time = keyframe.time;
			cutsceneTrackKey.cutscene = ((CutsceneTrackKey)keyframe).cutscene;
			this.c.Add(cutsceneTrackKey);
			this.UpdateTrack();
			return cutsceneTrackKey;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004EB0 File Offset: 0x000030B0
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((CutsceneTrackKey)keyframe);
			if (this.d == (CutsceneTrackKey)keyframe)
			{
				this.d = null;
			}
			UnityEngine.Object.DestroyImmediate(((CutsceneTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004EFF File Offset: 0x000030FF
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((CutsceneTrackKey)keyframe);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004F14 File Offset: 0x00003114
		public void FindLeftAndRight(float time, out CutsceneTrackKey left, out CutsceneTrackKey right)
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

		// Token: 0x06000141 RID: 321 RVA: 0x00004FE0 File Offset: 0x000031E0
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			CutsceneTrackKey cutsceneTrackKey;
			CutsceneTrackKey cutsceneTrackKey2;
			this.FindLeftAndRight(time, out cutsceneTrackKey, out cutsceneTrackKey2);
			if (cutsceneTrackKey)
			{
				if (this.d != cutsceneTrackKey)
				{
					if (this.d != null && this.d.cutscene != null)
					{
						this.d.cutscene.a((time - this.d.time) * this.d.playSpeed);
					}
					if (cutsceneTrackKey.cutscene != null)
					{
						cutsceneTrackKey.cutscene.Init();
					}
				}
				if (cutsceneTrackKey.cutscene != null)
				{
					cutsceneTrackKey.cutscene.StartCutscene((time - cutsceneTrackKey.time) * cutsceneTrackKey.playSpeed);
					this.controlledObject.i = cutsceneTrackKey.cutscene;
				}
			}
			this.d = cutsceneTrackKey;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000050C4 File Offset: 0x000032C4
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			CutsceneTrackKey[] componentsInChildren = base.GetComponentsInChildren<CutsceneTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005104 File Offset: 0x00003304
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<CutsceneTrackKey>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.c[i].gameObject.name = "cutsceneKey" + i;
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

		// Token: 0x06000144 RID: 324 RVA: 0x000051D0 File Offset: 0x000033D0
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			CutsceneTrackKey cutsceneTrackKey = (CutsceneTrackKey)one;
			CutsceneTrackKey cutsceneTrackKey2 = (CutsceneTrackKey)other;
			if (cutsceneTrackKey.keyTime > cutsceneTrackKey2.keyTime)
			{
				return 1;
			}
			if (cutsceneTrackKey.keyTime < cutsceneTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000520C File Offset: 0x0000340C
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000520F File Offset: 0x0000340F
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00005212 File Offset: 0x00003412
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("ShowInScene");
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005220 File Offset: 0x00003420
		public IKeyframe GetNextKey(CutsceneTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005268 File Offset: 0x00003468
		public IKeyframe GetPreviousKey(CutsceneTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000080 RID: 128
		public CSComponent controlledObject;

		// Token: 0x04000081 RID: 129
		public string cutsceneTrackName = "cutscene";

		// Token: 0x04000082 RID: 130
		public bool isDisabled;

		// Token: 0x04000083 RID: 131
		public WrapMode mode = WrapMode.Once;

		// Token: 0x04000084 RID: 132
		public Color trackColor = Color.white;

		// Token: 0x04000085 RID: 133
		private float a;

		// Token: 0x04000086 RID: 134
		private float b;

		// Token: 0x04000087 RID: 135
		private readonly List<CutsceneTrackKey> c = new List<CutsceneTrackKey>();

		// Token: 0x04000088 RID: 136
		private CutsceneTrackKey d;
	}
}
