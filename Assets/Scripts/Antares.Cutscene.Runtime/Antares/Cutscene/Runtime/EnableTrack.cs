using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200002F RID: 47
	public class EnableTrack : MonoBehaviour, ITrack
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000B0FA File Offset: 0x000092FA
		public string trackName
		{
			get
			{
				return this.enableTrackName;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000B102 File Offset: 0x00009302
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000B10A File Offset: 0x0000930A
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000B113 File Offset: 0x00009313
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000B11C File Offset: 0x0000931C
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
					Debug.Log("Faild to set EnableTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000B15C File Offset: 0x0000935C
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000B164 File Offset: 0x00009364
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000B16D File Offset: 0x0000936D
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000B175 File Offset: 0x00009375
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000B17E File Offset: 0x0000937E
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000B186 File Offset: 0x00009386
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000B18E File Offset: 0x0000938E
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000B19D File Offset: 0x0000939D
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000B1AA File Offset: 0x000093AA
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000B1AC File Offset: 0x000093AC
		public IKeyframe CreateKey(float time)
		{
			EnableTrackKey enableTrackKey = new GameObject("enableKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<EnableTrackKey>();
			enableTrackKey.Track = this;
			enableTrackKey.keyTime = time;
			this.c.Add(enableTrackKey);
			this.UpdateTrack();
			return enableTrackKey;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000B200 File Offset: 0x00009400
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			EnableTrackKey enableTrackKey = new GameObject("enableKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<EnableTrackKey>();
			enableTrackKey.enable = ((EnableTrackKey)keyframe).enable;
			enableTrackKey.time = ((EnableTrackKey)keyframe).time;
			this.c.Add(enableTrackKey);
			this.UpdateTrack();
			return enableTrackKey;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B265 File Offset: 0x00009465
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((EnableTrackKey)keyframe);
			UnityEngine.Object.DestroyImmediate(((EnableTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000B28F File Offset: 0x0000948F
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((EnableTrackKey)keyframe);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000B2A4 File Offset: 0x000094A4
		public void FindLeftAndRight(float time, out EnableTrackKey left, out EnableTrackKey right)
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

		// Token: 0x0600024B RID: 587 RVA: 0x0000B370 File Offset: 0x00009570
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			EnableTrackKey enableTrackKey;
			EnableTrackKey enableTrackKey2;
			this.FindLeftAndRight(time, out enableTrackKey, out enableTrackKey2);
			if (enableTrackKey)
			{
				if (this.d != enableTrackKey)
				{
					this.controlledObject.gameObject.SetActiveRecursively(enableTrackKey.enable);
				}
			}
			else
			{
				this.controlledObject.gameObject.SetActiveRecursively(true);
			}
			this.d = enableTrackKey;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B3DC File Offset: 0x000095DC
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			EnableTrackKey[] componentsInChildren = base.GetComponentsInChildren<EnableTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B41C File Offset: 0x0000961C
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<EnableTrackKey>(this.CompareKeys));
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

		// Token: 0x0600024E RID: 590 RVA: 0x0000B4AA File Offset: 0x000096AA
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			if (one.time > other.time)
			{
				return 1;
			}
			if (one.time < other.time)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000B4CD File Offset: 0x000096CD
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000B4D0 File Offset: 0x000096D0
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000B4D3 File Offset: 0x000096D3
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				Debug.Log("ShowInScene");
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000B4E0 File Offset: 0x000096E0
		public IKeyframe GetNextKey(EnableTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000B528 File Offset: 0x00009728
		public IKeyframe GetPreviousKey(EnableTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000130 RID: 304
		public Transform controlledObject;

		// Token: 0x04000131 RID: 305
		public string enableTrackName = "enable";

		// Token: 0x04000132 RID: 306
		public bool isDisabled;

		// Token: 0x04000133 RID: 307
		public WrapMode mode = WrapMode.Once;

		// Token: 0x04000134 RID: 308
		public Color trackColor = Color.white;

		// Token: 0x04000135 RID: 309
		private float a;

		// Token: 0x04000136 RID: 310
		private float b;

		// Token: 0x04000137 RID: 311
		private readonly List<EnableTrackKey> c = new List<EnableTrackKey>();

		// Token: 0x04000138 RID: 312
		private EnableTrackKey d;
	}
}
