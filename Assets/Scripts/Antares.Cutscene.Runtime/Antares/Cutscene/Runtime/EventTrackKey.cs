using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000022 RID: 34
	public class EventTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x06000194 RID: 404 RVA: 0x00006A00 File Offset: 0x00004C00
		public void Invoke()
		{
			try
			{
				MethodInfo methodInfo = this.MethodInfo;
				if (methodInfo != null)
				{
					List<object> list = new List<object>();
					foreach (EventParameter parameter in this.parameters)
					{
						list.Add(EventTrackKey.GetValue(parameter));
					}
					if (methodInfo.IsStatic)
					{
						methodInfo.Invoke(null, list.ToArray());
					}
					else
					{
						methodInfo.Invoke(this.monobeh, list.ToArray());
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public MethodInfo MethodInfo
		{
			get
			{
				if (this.b == null)
				{
					this.Refresh();
				}
				return this.b;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006ABC File Offset: 0x00004CBC
		public static object GetValue(EventParameter parameter)
		{
			switch (parameter.parameterType)
			{
			case EventParameterType.Bool:
				return parameter.boolValue;
			case EventParameterType.String:
				return parameter.stringValue;
			case EventParameterType.Integer:
				return parameter.integerValue;
			case EventParameterType.Float:
				return parameter.floatValue;
			case EventParameterType.Object:
				return parameter.objValue;
			default:
				return null;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006B20 File Offset: 0x00004D20
		public static Type GetTypeByEventParameterType(EventParameterType parameterTypes)
		{
			switch (parameterTypes)
			{
			case EventParameterType.Bool:
				return typeof(bool);
			case EventParameterType.String:
				return typeof(string);
			case EventParameterType.Integer:
				return typeof(int);
			case EventParameterType.Float:
				return typeof(float);
			case EventParameterType.Object:
				return typeof(UnityEngine.Object);
			default:
				return null;
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006B84 File Offset: 0x00004D84
		public void Refresh()
		{
			this.b = null;
			if (this.monobeh == null || string.IsNullOrEmpty(this.methodName))
			{
				this.parameters.Clear();
				this.methodName = "";
				return;
			}
			List<Type> list = new List<Type>();
			foreach (EventParameter eventParameter in this.parameters)
			{
				list.Add(EventTrackKey.GetTypeByEventParameterType(eventParameter.parameterType));
			}
			this.b = this.monobeh.GetType().GetMethod(this.methodName, list.ToArray());
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006C44 File Offset: 0x00004E44
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00006C70 File Offset: 0x00004E70
		public EventTrack Track
		{
			get
			{
				if (this.a == null)
				{
					this.a = base.transform.parent.GetComponent<EventTrack>();
				}
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00006C79 File Offset: 0x00004E79
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00006C81 File Offset: 0x00004E81
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.Track.UpdateTrack();
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006C9A File Offset: 0x00004E9A
		public void CaptureCurrentState()
		{
			//this.Track != null;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006CA9 File Offset: 0x00004EA9
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006CB7 File Offset: 0x00004EB7
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006CC5 File Offset: 0x00004EC5
		public void UpdateTrack()
		{
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
			this.Track.UpdateTrack();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006CDF File Offset: 0x00004EDF
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006CED File Offset: 0x00004EED
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006CFB File Offset: 0x00004EFB
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00006D09 File Offset: 0x00004F09
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00006D0C File Offset: 0x00004F0C
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00006D13 File Offset: 0x00004F13
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006D1F File Offset: 0x00004F1F
		public KeyType keyType
		{
			get
			{
				return KeyType.EVENT;
			}
		}

		// Token: 0x040000A9 RID: 169
		public float keyTime;

		// Token: 0x040000AA RID: 170
		public EventCallType callType;

		// Token: 0x040000AB RID: 171
		public Component monobeh;

		// Token: 0x040000AC RID: 172
		public string methodName;

		// Token: 0x040000AD RID: 173
		public List<EventParameter> parameters = new List<EventParameter>();

		// Token: 0x040000AE RID: 174
		private EventTrack a;

		// Token: 0x040000AF RID: 175
		private MethodInfo b;
	}
}
