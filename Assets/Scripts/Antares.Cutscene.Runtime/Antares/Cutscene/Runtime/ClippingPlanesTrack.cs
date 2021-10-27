using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000035 RID: 53
	public class ClippingPlanesTrack : MonoBehaviour, ITrack
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000C497 File Offset: 0x0000A697
		public string trackName
		{
			get
			{
				return "Clipping Planes";
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000C49E File Offset: 0x0000A69E
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000C4A6 File Offset: 0x0000A6A6
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000C4AF File Offset: 0x0000A6AF
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
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
					Debug.Log("Faild to set ClippingPlanesTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000C500 File Offset: 0x0000A700
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000C509 File Offset: 0x0000A709
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000C511 File Offset: 0x0000A711
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000C51A File Offset: 0x0000A71A
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000C522 File Offset: 0x0000A722
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000C52A File Offset: 0x0000A72A
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000C539 File Offset: 0x0000A739
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000C546 File Offset: 0x0000A746
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000C548 File Offset: 0x0000A748
		public IKeyframe CreateKey(float time)
		{
			ClippingPlanesTrackKey clippingPlanesTrackKey = new ClippingPlanesTrackKey();
			clippingPlanesTrackKey.Track = this;
			clippingPlanesTrackKey.data = new Vector2(this.controlledObject.nearClipPlane, this.controlledObject.farClipPlane);
			clippingPlanesTrackKey.keyTime = time;
			this.c.Add(clippingPlanesTrackKey);
			this.UpdateTrack();
			return clippingPlanesTrackKey;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			ClippingPlanesTrackKey clippingPlanesTrackKey = new ClippingPlanesTrackKey();
			clippingPlanesTrackKey.Track = this;
			clippingPlanesTrackKey.data = ((ClippingPlanesTrackKey)keyframe).data;
			clippingPlanesTrackKey.time = ((ClippingPlanesTrackKey)keyframe).time;
			this.c.Add(clippingPlanesTrackKey);
			this.UpdateTrack();
			return clippingPlanesTrackKey;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000C5EF File Offset: 0x0000A7EF
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((ClippingPlanesTrackKey)keyframe);
			this.UpdateTrack();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000C609 File Offset: 0x0000A809
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((ClippingPlanesTrackKey)keyframe);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000C61C File Offset: 0x0000A81C
		public void FindLeftAndRight(float time, out ClippingPlanesTrackKey left, out ClippingPlanesTrackKey right)
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

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			ClippingPlanesTrackKey clippingPlanesTrackKey;
			ClippingPlanesTrackKey clippingPlanesTrackKey2;
			this.FindLeftAndRight(time, out clippingPlanesTrackKey, out clippingPlanesTrackKey2);
			if (clippingPlanesTrackKey != null)
			{
				if (clippingPlanesTrackKey2 != null)
				{
					float t = (time - clippingPlanesTrackKey.time) / (clippingPlanesTrackKey2.time - clippingPlanesTrackKey.time);
					Vector2 vector = Vector2.Lerp(clippingPlanesTrackKey.data, clippingPlanesTrackKey2.data, t);
					this.controlledObject.nearClipPlane = vector.x;
					this.controlledObject.farClipPlane = vector.y;
					return;
				}
				this.controlledObject.nearClipPlane = clippingPlanesTrackKey.data.x;
				this.controlledObject.farClipPlane = clippingPlanesTrackKey.data.y;
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C790 File Offset: 0x0000A990
		public void InitTrack()
		{
			if (this.times == null || this.clipPlanes == null)
			{
				this.times = new float[0];
				this.clipPlanes = new Vector2[0];
			}
			this.c.Clear();
			for (int i = 0; i < this.times.Length; i++)
			{
				this.c.Add(new ClippingPlanesTrackKey
				{
					data = this.clipPlanes[i],
					keyTime = this.times[i],
					Track = this
				});
			}
			this.UpdateTrack();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C828 File Offset: 0x0000AA28
		public void UpdateTrack()
		{
			this.times = new float[this.c.Count];
			this.clipPlanes = new Vector2[this.c.Count];
			this.c.Sort(new Comparison<ClippingPlanesTrackKey>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.times[i] = this.c[i].keyTime;
				this.clipPlanes[i] = this.c[i].data;
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

		// Token: 0x060002BC RID: 700 RVA: 0x0000C934 File Offset: 0x0000AB34
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			ClippingPlanesTrackKey clippingPlanesTrackKey = (ClippingPlanesTrackKey)one;
			ClippingPlanesTrackKey clippingPlanesTrackKey2 = (ClippingPlanesTrackKey)other;
			if (clippingPlanesTrackKey.keyTime > clippingPlanesTrackKey2.keyTime)
			{
				return 1;
			}
			if (clippingPlanesTrackKey.keyTime < clippingPlanesTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000C970 File Offset: 0x0000AB70
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000C973 File Offset: 0x0000AB73
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000C976 File Offset: 0x0000AB76
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				Debug.Log("ClippingPlanesTrack can't show in scene. Use ITrack.IsCanShowInScene to check this.");
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C984 File Offset: 0x0000AB84
		public IKeyframe GetNextKey(ClippingPlanesTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C9CC File Offset: 0x0000ABCC
		public IKeyframe GetPreviousKey(ClippingPlanesTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000166 RID: 358
		public Camera controlledObject;

		// Token: 0x04000167 RID: 359
		public bool isDisabled;

		// Token: 0x04000168 RID: 360
		public WrapMode mode = WrapMode.Once;

		// Token: 0x04000169 RID: 361
		public Color trackColor = Color.white;

		// Token: 0x0400016A RID: 362
		private float a;

		// Token: 0x0400016B RID: 363
		private float b;

		// Token: 0x0400016C RID: 364
		private readonly List<ClippingPlanesTrackKey> c = new List<ClippingPlanesTrackKey>();

		// Token: 0x0400016D RID: 365
		public float[] times;

		// Token: 0x0400016E RID: 366
		public Vector2[] clipPlanes;
	}
}
