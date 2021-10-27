using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200002E RID: 46
	public class PositionTrack1 : MonoBehaviour, ITrack
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000A784 File Offset: 0x00008984
		public string trackName
		{
			get
			{
				return "position";
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000A78B File Offset: 0x0000898B
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000A793 File Offset: 0x00008993
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000A79C File Offset: 0x0000899C
		// (set) Token: 0x0600021B RID: 539 RVA: 0x0000A7A4 File Offset: 0x000089A4
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
					Debug.Log("Faild to set PositionTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000A7E4 File Offset: 0x000089E4
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000A7EC File Offset: 0x000089EC
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000A7F5 File Offset: 0x000089F5
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000A7FD File Offset: 0x000089FD
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A806 File Offset: 0x00008A06
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000A80E File Offset: 0x00008A0E
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000A816 File Offset: 0x00008A16
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000A825 File Offset: 0x00008A25
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A832 File Offset: 0x00008A32
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A834 File Offset: 0x00008A34
		public IKeyframe CreateKey(float time)
		{
			PositionTrackKey1 positionTrackKey = new GameObject("posKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<PositionTrackKey1>();
			positionTrackKey.Track = this;
			positionTrackKey.data = this.controlledObject.position;
			positionTrackKey.keyTime = time;
			this.c.Add(positionTrackKey);
			if (this.updateOnAddKeys)
			{
				this.UpdateTrack();
			}
			return positionTrackKey;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			PositionTrackKey1 positionTrackKey = new GameObject("posKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<PositionTrackKey1>();
			positionTrackKey.Track = this;
			positionTrackKey.data = ((PositionTrackKey1)keyframe).data;
			positionTrackKey.keyTime = ((PositionTrackKey1)keyframe).time;
			this.c.Add(positionTrackKey);
			if (this.updateOnAddKeys)
			{
				this.UpdateTrack();
			}
			return positionTrackKey;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A914 File Offset: 0x00008B14
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((PositionTrackKey1)keyframe);
			UnityEngine.Object.DestroyImmediate(((PositionTrackKey1)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A93E File Offset: 0x00008B3E
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((PositionTrackKey1)keyframe);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A954 File Offset: 0x00008B54
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			if (this.worldSpace)
			{
				this.controlledObject.position = new Vector3(this.curveX.Evaluate(time), this.curveY.Evaluate(time), this.curveZ.Evaluate(time));
				return;
			}
			this.controlledObject.localPosition = new Vector3(this.curveX.Evaluate(time), this.curveY.Evaluate(time), this.curveZ.Evaluate(time));
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A9E0 File Offset: 0x00008BE0
		public Vector3 Evaluate(float time)
		{
			if (this.c.Count == 0)
			{
				return Vector3.zero;
			}
			return new Vector3(this.curveX.Evaluate(time), this.curveY.Evaluate(time), this.curveZ.Evaluate(time));
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000AA20 File Offset: 0x00008C20
		public void InitTrack()
		{
			if (this.times == null || this.positions == null || this.smooths == null)
			{
				this.times = new float[0];
				this.positions = new Vector3[0];
				this.smooths = new float[0];
			}
			this.c.Clear();
			PositionTrackKey1[] componentsInChildren = base.GetComponentsInChildren<PositionTrackKey1>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
				this.times = new float[0];
				this.positions = new Vector3[0];
				this.smooths = new float[0];
			}
			else
			{
				for (int i = 0; i < this.times.Length; i++)
				{
					PositionTrackKey1 positionTrackKey = new GameObject("posKey" + i)
					{
						transform = 
						{
							parent = base.transform
						}
					}.AddComponent<PositionTrackKey1>();
					positionTrackKey.data = new Vector3(this.positions[i].x, this.positions[i].y, this.positions[i].z);
					positionTrackKey.keyTime = this.times[i];
					positionTrackKey.Track = this;
					positionTrackKey.smooth = this.smooths[i];
					this.c.Add(positionTrackKey);
				}
			}
			this.UpdateTrack();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000AB70 File Offset: 0x00008D70
		public AnimationCurve GetCurveX()
		{
			return this.curveX;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000AB78 File Offset: 0x00008D78
		public AnimationCurve GetCurveY()
		{
			return this.curveY;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000AB80 File Offset: 0x00008D80
		public AnimationCurve GetCurveZ()
		{
			return this.curveZ;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000AB88 File Offset: 0x00008D88
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<PositionTrackKey1>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.c[i].gameObject.name = "posKey" + i;
			}
			List<PositionTrackKey1> list = new List<PositionTrackKey1>();
			foreach (PositionTrackKey1 positionTrackKey in this.c)
			{
				list.Add(positionTrackKey);
				if (positionTrackKey.corner)
				{
					list.Add(positionTrackKey);
				}
			}
			Keyframe[] array = new Keyframe[list.Count];
			Keyframe[] array2 = new Keyframe[list.Count];
			Keyframe[] array3 = new Keyframe[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				array[j].time = (array2[j].time = (array3[j].time = list[j].time + list[j].offset));
				array[j].value = list[j].data.x;
				array2[j].value = list[j].data.y;
				array3[j].value = list[j].data.z;
			}
			this.curveX.keys = array;
			this.curveY.keys = array2;
			this.curveZ.keys = array3;
			for (int k = 0; k < list.Count; k++)
			{
				this.curveX.SmoothTangents(k, list[k].smooth);
				this.curveY.SmoothTangents(k, list[k].smooth);
				this.curveZ.SmoothTangents(k, list[k].smooth);
			}
			this.curveX.postWrapMode = (this.curveX.preWrapMode = this.playMode);
			this.curveY.postWrapMode = (this.curveY.preWrapMode = this.playMode);
			this.curveZ.postWrapMode = (this.curveZ.preWrapMode = this.playMode);
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

		// Token: 0x06000230 RID: 560 RVA: 0x0000AE8C File Offset: 0x0000908C
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			PositionTrackKey1 positionTrackKey = (PositionTrackKey1)one;
			PositionTrackKey1 positionTrackKey2 = (PositionTrackKey1)other;
			if (positionTrackKey.keyTime + positionTrackKey.offset > positionTrackKey2.keyTime + positionTrackKey2.offset)
			{
				return 1;
			}
			if (positionTrackKey.keyTime + positionTrackKey.offset < positionTrackKey2.keyTime + positionTrackKey2.offset)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000AEE4 File Offset: 0x000090E4
		public bool IsCanShowInScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000AEE7 File Offset: 0x000090E7
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000AEEF File Offset: 0x000090EF
		public bool ShowInScene
		{
			get
			{
				return this.showInScene;
			}
			set
			{
				this.showInScene = value;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000AEF8 File Offset: 0x000090F8
		public IKeyframe GetNextKey(PositionTrackKey1 key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000AF40 File Offset: 0x00009140
		public IKeyframe GetPreviousKey(PositionTrackKey1 key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AF84 File Offset: 0x00009184
		public void OnDrawGizmos()
		{
			try
			{
				if (this.ShowInScene)
				{
					if (this.c.Count == 0)
					{
						this.InitTrack();
					}
					Color color = Gizmos.color;
					Gizmos.color = this.color;
					int num = (int)(this.length * 30f);
					for (int i = 0; i < num; i++)
					{
						float time = this.startTime + this.length * ((float)i / ((float)num - 1f));
						Vector3 from = new Vector3(this.GetCurveX().Evaluate(time), this.GetCurveY().Evaluate(time), this.GetCurveZ().Evaluate(time));
						time = this.startTime + this.length * ((float)(i + 1) / ((float)num - 1f));
						Vector3 to = new Vector3(this.GetCurveX().Evaluate(time), this.GetCurveY().Evaluate(time), this.GetCurveZ().Evaluate(time));
						Gizmos.DrawLine(from, to);
					}
					Gizmos.color = color;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400011F RID: 287
		public Transform controlledObject;

		// Token: 0x04000120 RID: 288
		public bool worldSpace = true;

		// Token: 0x04000121 RID: 289
		public bool enableSnap = true;

		// Token: 0x04000122 RID: 290
		public bool updateOnAddKeys = true;

		// Token: 0x04000123 RID: 291
		public bool isDisabled;

		// Token: 0x04000124 RID: 292
		public WrapMode mode = WrapMode.Once;

		// Token: 0x04000125 RID: 293
		public Color trackColor = Color.white;

		// Token: 0x04000126 RID: 294
		private float a;

		// Token: 0x04000127 RID: 295
		private float b;

		// Token: 0x04000128 RID: 296
		private readonly List<PositionTrackKey1> c = new List<PositionTrackKey1>();

		// Token: 0x04000129 RID: 297
		public float[] times;

		// Token: 0x0400012A RID: 298
		public Vector3[] positions;

		// Token: 0x0400012B RID: 299
		public float[] smooths;

		// Token: 0x0400012C RID: 300
		public AnimationCurve curveX = new AnimationCurve();

		// Token: 0x0400012D RID: 301
		public AnimationCurve curveY = new AnimationCurve();

		// Token: 0x0400012E RID: 302
		public AnimationCurve curveZ = new AnimationCurve();

		// Token: 0x0400012F RID: 303
		public bool showInScene;
	}
}
