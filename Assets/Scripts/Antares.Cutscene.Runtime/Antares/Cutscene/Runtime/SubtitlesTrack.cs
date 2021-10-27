using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200003D RID: 61
	public class SubtitlesTrack : MonoBehaviour, ITrack
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000E8A7 File Offset: 0x0000CAA7
		public string trackName
		{
			get
			{
				return this.subtitlesTrackName;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000E8AF File Offset: 0x0000CAAF
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000E8B7 File Offset: 0x0000CAB7
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

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
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
					Debug.Log("Faild to set SubtitlesTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000E908 File Offset: 0x0000CB08
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000E910 File Offset: 0x0000CB10
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

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000E919 File Offset: 0x0000CB19
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000E921 File Offset: 0x0000CB21
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

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000E92A File Offset: 0x0000CB2A
		public float startTime
		{
			get
			{
				return this.afl;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000E932 File Offset: 0x0000CB32
		public float endTime
		{
			get
			{
				return this.bfl;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000E93A File Offset: 0x0000CB3A
		public float length
		{
			get
			{
				return this.bfl - this.afl;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000E949 File Offset: 0x0000CB49
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E956 File Offset: 0x0000CB56
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E958 File Offset: 0x0000CB58
		public IKeyframe CreateKey(float time)
		{
			SubtitlesTrackKey subtitlesTrackKey = new GameObject("subtitlesKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<SubtitlesTrackKey>();
			subtitlesTrackKey.sbTrack = this;
			subtitlesTrackKey.keyTime = time;
			this.c.Add(subtitlesTrackKey);
			this.UpdateTrack();
			return subtitlesTrackKey;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E9AC File Offset: 0x0000CBAC
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			SubtitlesTrackKey subtitlesTrackKey = new GameObject("subtitlesKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<SubtitlesTrackKey>();
			subtitlesTrackKey.sbTrack = this;
			subtitlesTrackKey.time = keyframe.time;
			subtitlesTrackKey.duration = ((SubtitlesTrackKey)keyframe).duration;
			subtitlesTrackKey.fadeInTime = ((SubtitlesTrackKey)keyframe).fadeInTime;
			subtitlesTrackKey.fadeOutTime = ((SubtitlesTrackKey)keyframe).fadeOutTime;
			subtitlesTrackKey.color = ((SubtitlesTrackKey)keyframe).color;
			subtitlesTrackKey.font = ((SubtitlesTrackKey)keyframe).font;
			subtitlesTrackKey.fontSize = ((SubtitlesTrackKey)keyframe).fontSize + 3;
			subtitlesTrackKey.fontStyle = ((SubtitlesTrackKey)keyframe).fontStyle;
			subtitlesTrackKey.customPlacement = ((SubtitlesTrackKey)keyframe).customPlacement;
			subtitlesTrackKey.textAnchor = ((SubtitlesTrackKey)keyframe).textAnchor;
			subtitlesTrackKey.customMarginX = ((SubtitlesTrackKey)keyframe).customMarginX;
			subtitlesTrackKey.customMarginY = ((SubtitlesTrackKey)keyframe).customMarginY;
			this.c.Add(subtitlesTrackKey);
			this.UpdateTrack();
			return subtitlesTrackKey;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((SubtitlesTrackKey)keyframe);
			if (this.d == (SubtitlesTrackKey)keyframe)
			{
				this.d = null;
			}
			UnityEngine.Object.DestroyImmediate(((SubtitlesTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000EB0F File Offset: 0x0000CD0F
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((SubtitlesTrackKey)keyframe);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000EB24 File Offset: 0x0000CD24
		public void FindLeftAndRight(float time, out SubtitlesTrackKey left, out SubtitlesTrackKey right)
		{
			left = null;
			right = null;
			if (time <= this.afl)
			{
				if (this.c.Count > 0)
				{
					right = this.c[0];
				}
				return;
			}
			if (time >= this.bfl)
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

		// Token: 0x06000353 RID: 851 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		private void ad2f(float A_0, SubtitlesTrackKey A_1)
		{
			float num;
			if (A_0 < A_1.time + A_1.fadeInTime)
			{
				num = Mathf.Lerp(0f, 1f, (A_0 - A_1.time) / A_1.fadeInTime);
			}
			else if (A_0 > A_1.time + A_1.duration - A_1.fadeOutTime)
			{
				float num2 = A_0 - (A_1.time + A_1.duration - A_1.fadeOutTime);
				num = Mathf.Lerp(1f, 0f, num2 / A_1.fadeOutTime);
			}
			else
			{
				num = 1f;
			}
			SubtitlesDrawInfo subtitlesDrawInfo = new SubtitlesDrawInfo();
			subtitlesDrawInfo.color = A_1.color;
			subtitlesDrawInfo.color.a = num;
			subtitlesDrawInfo.text = A_1.text;
			if (A_1.keyword != null)
			{
				string text = A_1.keyword.Trim();
				if (!string.IsNullOrEmpty(text) && CSComponent.Localization != null)
				{
					string value = CSComponent.Localization.GetText(text) ?? "";
					if (!string.IsNullOrEmpty(value))
					{
						subtitlesDrawInfo.text = CSComponent.Localization.GetText(text);
					}
				}
			}
			subtitlesDrawInfo.font = A_1.font;
			subtitlesDrawInfo.fontSize = A_1.fontSize + 3;
			subtitlesDrawInfo.fontStyle = A_1.fontStyle;
			subtitlesDrawInfo.useCustomPlacement = A_1.customPlacement;
			subtitlesDrawInfo.textAnchor = A_1.textAnchor;
			subtitlesDrawInfo.customMarginX = A_1.customMarginX;
			subtitlesDrawInfo.customMarginY = A_1.customMarginY;
			subtitlesDrawInfo.useCustomStyle = A_1.customStyle;
			this.controlledObject.f.Add(subtitlesDrawInfo);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000ED70 File Offset: 0x0000CF70
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			SubtitlesTrackKey subtitlesTrackKey;
			SubtitlesTrackKey subtitlesTrackKey2;
			this.FindLeftAndRight(time, out subtitlesTrackKey, out subtitlesTrackKey2);
			if (subtitlesTrackKey)
			{
				SubtitlesTrackKey subtitlesTrackKey3 = (SubtitlesTrackKey)subtitlesTrackKey.GetPreviousKey();
				if (subtitlesTrackKey3 != null && subtitlesTrackKey3.time < subtitlesTrackKey.time && time < subtitlesTrackKey3.time + subtitlesTrackKey3.duration)
				{
					this.ad2f(time, subtitlesTrackKey3);
				}
				if (time < subtitlesTrackKey.time + subtitlesTrackKey.duration)
				{
					this.ad2f(time, subtitlesTrackKey);
				}
			}
			this.d = subtitlesTrackKey;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000EDF8 File Offset: 0x0000CFF8
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			SubtitlesTrackKey[] componentsInChildren = base.GetComponentsInChildren<SubtitlesTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000EE38 File Offset: 0x0000D038
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<SubtitlesTrackKey>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.c[i].gameObject.name = "subtitlesKey" + i;
			}
			if (this.c.Count == 0)
			{
				this.afl = (this.bfl = 0f);
			}
			else
			{
				this.afl = this.c[0].time;
				this.bfl = this.c[this.c.Count - 1].time;
			}
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000EF04 File Offset: 0x0000D104
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			SubtitlesTrackKey subtitlesTrackKey = (SubtitlesTrackKey)one;
			SubtitlesTrackKey subtitlesTrackKey2 = (SubtitlesTrackKey)other;
			if (subtitlesTrackKey.keyTime > subtitlesTrackKey2.keyTime)
			{
				return 1;
			}
			if (subtitlesTrackKey.keyTime < subtitlesTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000EF40 File Offset: 0x0000D140
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000EF43 File Offset: 0x0000D143
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000EF46 File Offset: 0x0000D146
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

		// Token: 0x0600035B RID: 859 RVA: 0x0000EF54 File Offset: 0x0000D154
		public IKeyframe GetNextKey(SubtitlesTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EF9C File Offset: 0x0000D19C
		public IKeyframe GetPreviousKey(SubtitlesTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x040001A5 RID: 421
		public CSComponent controlledObject;

		// Token: 0x040001A6 RID: 422
		public string subtitlesTrackName = "subtitles";

		// Token: 0x040001A7 RID: 423
		public bool isDisabled;

		// Token: 0x040001A8 RID: 424
		public WrapMode mode = WrapMode.Once;

		// Token: 0x040001A9 RID: 425
		public Color trackColor = Color.white;

		// Token: 0x040001AA RID: 426
		private float afl;

		// Token: 0x040001AB RID: 427
		private float bfl;

		// Token: 0x040001AC RID: 428
		private readonly List<SubtitlesTrackKey> c = new List<SubtitlesTrackKey>();

		// Token: 0x040001AD RID: 429
		private SubtitlesTrackKey d;
	}
}
