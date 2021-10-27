using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000030 RID: 48
	public class EventTrack : MonoBehaviour, ITrack
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000B59C File Offset: 0x0000979C
		private bool runEventsInEditMode
		{
			get
			{
				if (this.a != null)
				{
					return this.a.runEventsInEditMode;
				}
				try
				{
					this.a = base.transform.parent.parent.GetComponent<CSComponent>();
					if (this.a != null)
					{
						return this.a.runEventsInEditMode;
					}
				}
				catch
				{
				}
				return false;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000B614 File Offset: 0x00009814
		public string trackName
		{
			get
			{
				return this.eventTrackName;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000B61C File Offset: 0x0000981C
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000B624 File Offset: 0x00009824
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000B62D File Offset: 0x0000982D
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000B638 File Offset: 0x00009838
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
					this.controlledObject = (Component)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set EventTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000B678 File Offset: 0x00009878
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000B680 File Offset: 0x00009880
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000B689 File Offset: 0x00009889
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000B691 File Offset: 0x00009891
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000B69A File Offset: 0x0000989A
		public float startTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000B6A2 File Offset: 0x000098A2
		public float endTime
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000B6AA File Offset: 0x000098AA
		public float length
		{
			get
			{
				return this.c - this.b;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000B6B9 File Offset: 0x000098B9
		public IKeyframe[] Keys
		{
			get
			{
				return this.d.ToArray();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B6C6 File Offset: 0x000098C6
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B6C8 File Offset: 0x000098C8
		public IKeyframe CreateKey(float time)
		{
			EventTrackKey eventTrackKey = new GameObject("eventKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<EventTrackKey>();
			eventTrackKey.Track = this;
			eventTrackKey.keyTime = time;
			this.d.Add(eventTrackKey);
			this.UpdateTrack();
			return eventTrackKey;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B71C File Offset: 0x0000991C
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			EventTrackKey eventTrackKey = new GameObject("eventKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<EventTrackKey>();
			eventTrackKey.Track = this;
			eventTrackKey.time = ((EventTrackKey)keyframe).time;
			eventTrackKey.methodName = ((EventTrackKey)keyframe).methodName;
			eventTrackKey.monobeh = ((EventTrackKey)keyframe).monobeh;
			eventTrackKey.callType = ((EventTrackKey)keyframe).callType;
			foreach (EventParameter eventParameter in ((EventTrackKey)keyframe).parameters)
			{
				EventParameter eventParameter2 = new EventParameter();
				eventParameter2.boolValue = eventParameter.boolValue;
				eventParameter2.integerValue = eventParameter.integerValue;
				eventParameter2.floatValue = eventParameter.floatValue;
				eventParameter2.stringValue = eventParameter.stringValue;
				eventParameter2.objValue = eventParameter.objValue;
				eventParameter2.parameterType = eventParameter.parameterType;
				eventTrackKey.parameters.Add(eventParameter2);
			}
			eventTrackKey.time = ((EventTrackKey)keyframe).time;
			this.d.Add(eventTrackKey);
			this.UpdateTrack();
			return eventTrackKey;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B85C File Offset: 0x00009A5C
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.d.Remove((EventTrackKey)keyframe);
			if (this.e == (EventTrackKey)keyframe)
			{
				this.e = null;
			}
			UnityEngine.Object.DestroyImmediate(((EventTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B8AB File Offset: 0x00009AAB
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.d.IndexOf((EventTrackKey)keyframe);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B8C0 File Offset: 0x00009AC0
		public void FindLeftAndRight(float time, out EventTrackKey left, out EventTrackKey right)
		{
			left = null;
			right = null;
			if (time <= this.b + 0.03f)
			{
				if (this.d.Count > 0)
				{
					right = this.d[0];
				}
				return;
			}
			if (time >= this.c)
			{
				if (this.d.Count > 0)
				{
					left = this.d[this.d.Count - 1];
				}
				return;
			}
			int num = 0;
			int num2 = this.d.Count - 1;
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
				if (time > this.d[num3].keyTime)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			left = this.d[num];
			right = this.d[num2];
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000B990 File Offset: 0x00009B90
		public void EvaluateTime(float time)
		{
			if (this.d.Count == 0)
			{
				return;
			}
			if (Application.isPlaying || this.runEventsInEditMode)
			{
				EventTrackKey eventTrackKey;
				EventTrackKey eventTrackKey2;
				this.FindLeftAndRight(time, out eventTrackKey, out eventTrackKey2);
				if (eventTrackKey)
				{
					switch (eventTrackKey.callType)
					{
					case EventCallType.Once:
						if (this.e != eventTrackKey)
						{
							this.e = eventTrackKey;
							eventTrackKey.Invoke();
						}
						break;
					case EventCallType.UntilNext:
						this.e = eventTrackKey;
						eventTrackKey.Invoke();
						break;
					case EventCallType.Stop:
						this.e = eventTrackKey;
						break;
					}
				}
				this.e = eventTrackKey;
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000BA24 File Offset: 0x00009C24
		public void InitTrack()
		{
			this.d.Clear();
			this.e = null;
			EventTrackKey[] componentsInChildren = base.GetComponentsInChildren<EventTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.d.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000BA64 File Offset: 0x00009C64
		public void UpdateTrack()
		{
			this.d.Sort(new Comparison<EventTrackKey>(this.CompareKeys));
			for (int i = 0; i < this.d.Count; i++)
			{
				this.d[i].gameObject.name = "eventKey" + i;
			}
			if (this.d.Count == 0)
			{
				this.b = (this.c = 0f);
			}
			else
			{
				this.b = this.d[0].time;
				this.c = this.d[this.d.Count - 1].time;
			}
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000BB2E File Offset: 0x00009D2E
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000BB51 File Offset: 0x00009D51
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000BB54 File Offset: 0x00009D54
		// (set) Token: 0x0600026F RID: 623 RVA: 0x0000BB57 File Offset: 0x00009D57
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

		// Token: 0x06000270 RID: 624 RVA: 0x0000BB64 File Offset: 0x00009D64
		public IKeyframe GetNextKey(EventTrackKey key)
		{
			int num = this.d.IndexOf(key);
			if (num == this.d.Count - 1)
			{
				return this.d[0];
			}
			return this.d[num + 1];
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000BBAC File Offset: 0x00009DAC
		public IKeyframe GetPreviousKey(EventTrackKey key)
		{
			int num = this.d.IndexOf(key);
			if (num == 0)
			{
				return this.d[this.d.Count - 1];
			}
			return this.d[num - 1];
		}

		// Token: 0x04000139 RID: 313
		public Component controlledObject;

		// Token: 0x0400013A RID: 314
		public string eventTrackName = "event";

		// Token: 0x0400013B RID: 315
		private CSComponent a;

		// Token: 0x0400013C RID: 316
		public bool isDisabled;

		// Token: 0x0400013D RID: 317
		public WrapMode mode = WrapMode.Once;

		// Token: 0x0400013E RID: 318
		public Color trackColor = Color.white;

		// Token: 0x0400013F RID: 319
		private float b;

		// Token: 0x04000140 RID: 320
		private float c;

		// Token: 0x04000141 RID: 321
		private readonly List<EventTrackKey> d = new List<EventTrackKey>();

		// Token: 0x04000142 RID: 322
		private EventTrackKey e;
	}
}
