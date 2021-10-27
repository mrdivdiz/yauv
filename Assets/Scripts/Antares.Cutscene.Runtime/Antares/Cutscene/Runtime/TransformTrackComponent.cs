using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000015 RID: 21
	[CutsceneObject("Game Object", new Type[]
	{
		typeof(Transform)
	})]
	public class TransformTrackComponent : CutsceneObjectBase
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000397F File Offset: 0x00001B7F
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003988 File Offset: 0x00001B88
		public override object ControlledObject
		{
			get
			{
				return this.controlledTransform;
			}
			set
			{
				try
				{
					this.controlledTransform = (Transform)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild set TransformTrackComponent.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000039C8 File Offset: 0x00001BC8
		public void Awake()
		{
			this.Init();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000039D0 File Offset: 0x00001BD0
		public override void Init()
		{
			if (this.controlledTransform == null)
			{
				Debug.Log("TransformTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return;
			}
			for (int i = 0; i < this._tracks.Count; i++)
			{
				MonoBehaviour monoBehaviour = this._tracks[i];
				this._tracks[i] = (MonoBehaviour)monoBehaviour.gameObject.GetComponent(monoBehaviour.GetType());
			}
			foreach (MonoBehaviour monoBehaviour2 in base.GetComponentsInChildren<MonoBehaviour>())
			{
				if (monoBehaviour2 is ITrack)
				{
					ITrack track = monoBehaviour2 as ITrack;
					track.ControlledObject = this.controlledTransform;
					track.InitTrack();
					if (!this._tracks.Contains((MonoBehaviour)track))
					{
						this._tracks.Add((MonoBehaviour)track);
					}
				}
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003AA1 File Offset: 0x00001CA1
		public override void InitDefaultTracks()
		{
			this.CreateTrack("position");
			this.CreateTrack("rotation");
			this.CreateTrack("scale");
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003AC8 File Offset: 0x00001CC8
		public override ITrack[] GetTracks()
		{
			if (this.controlledTransform == null)
			{
				Debug.Log("TransformTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return new ITrack[0];
			}
			List<ITrack> list = new List<ITrack>();
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				list.Add((ITrack)monoBehaviour);
			}
			return list.ToArray();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003B4C File Offset: 0x00001D4C
		public override ITrack CreateTrack(string trackName)
		{
			if (this.controlledTransform == null)
			{
				Debug.Log("TransformTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return null;
			}
			if (trackName == "scale")
			{
				ScaleTrack1 scaleTrack = base.GetComponentInChildren<ScaleTrack1>();
				if (scaleTrack == null)
				{
					scaleTrack = new GameObject("Scale Track").AddComponent<ScaleTrack1>();
					scaleTrack.transform.parent = base.transform;
				}
				scaleTrack.controlledObject = this.controlledTransform;
				scaleTrack.InitTrack();
				this._tracks.Add(scaleTrack);
				return scaleTrack;
			}
			if (trackName == "position")
			{
				PositionTrack1 positionTrack = base.GetComponentInChildren<PositionTrack1>();
				if (positionTrack == null)
				{
					positionTrack = new GameObject("Position Track").AddComponent<PositionTrack1>();
					positionTrack.transform.parent = base.transform;
				}
				positionTrack.controlledObject = this.controlledTransform;
				positionTrack.InitTrack();
				this._tracks.Add(positionTrack);
				return positionTrack;
			}
			if (trackName == "rotation")
			{
				RotationTrack1 rotationTrack = base.GetComponentInChildren<RotationTrack1>();
				if (rotationTrack == null)
				{
					rotationTrack = new GameObject("Rotation Track").AddComponent<RotationTrack1>();
					rotationTrack.transform.parent = base.transform;
				}
				rotationTrack.controlledObject = this.controlledTransform;
				rotationTrack.InitTrack();
				this._tracks.Add(rotationTrack);
				return rotationTrack;
			}
			if (trackName.StartsWith("event"))
			{
				int num = 1;
				foreach (EventTrack eventTrack in base.GetComponentsInChildren<EventTrack>())
				{
					try
					{
						int num2 = int.Parse(eventTrack.trackName.Substring(5));
						if (num2 >= num)
						{
							num = num2 + 1;
						}
					}
					catch
					{
					}
				}
				EventTrack eventTrack2 = new GameObject("Event Track").AddComponent<EventTrack>();
				eventTrack2.eventTrackName = "event" + num.ToString().Trim();
				eventTrack2.transform.parent = base.transform;
				eventTrack2.controlledObject = this.controlledTransform;
				eventTrack2.InitTrack();
				this._tracks.Add(eventTrack2);
				return eventTrack2;
			}
			if (trackName == "enable")
			{
				if (base.GetComponentsInChildren<EnableTrack>().Length > 0)
				{
					return null;
				}
				EnableTrack enableTrack = new GameObject("Enable Track").AddComponent<EnableTrack>();
				enableTrack.transform.parent = base.transform;
				enableTrack.controlledObject = this.controlledTransform;
				enableTrack.InitTrack();
				this._tracks.Add(enableTrack);
				return enableTrack;
			}
			else
			{
				if (trackName.StartsWith("sound"))
				{
					int num3 = 1;
					foreach (TransformSoundTrack transformSoundTrack in base.GetComponentsInChildren<TransformSoundTrack>())
					{
						try
						{
							int num4 = int.Parse(transformSoundTrack.trackName.Substring(5));
							if (num4 >= num3)
							{
								num3 = num4 + 1;
							}
						}
						catch
						{
						}
					}
					TransformSoundTrack transformSoundTrack2 = new GameObject("Sound Track").AddComponent<TransformSoundTrack>();
					transformSoundTrack2.soundTrackName = "sound" + num3.ToString().Trim();
					transformSoundTrack2.transform.parent = base.transform;
					transformSoundTrack2.controlledObject = this.controlledTransform;
					transformSoundTrack2.InitTrack();
					this._tracks.Add(transformSoundTrack2);
					return transformSoundTrack2;
				}
				return null;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003E84 File Offset: 0x00002084
		public override bool DeleteTrack(ITrack track)
		{
			if (this.controlledTransform == null)
			{
				Debug.Log("TransformTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return false;
			}
			this._tracks.Remove((MonoBehaviour)track);
			UnityEngine.Object.DestroyImmediate(((MonoBehaviour)track).gameObject);
			return true;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003EC4 File Offset: 0x000020C4
		public override string[] GetSupportedTrackNames()
		{
			return new string[]
			{
				"position",
				"rotation",
				"scale",
				"event",
				"sound",
				"enable"
			};
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003F0C File Offset: 0x0000210C
		public override string[] GetSupportedTrackNamesToCreate()
		{
			List<string> list = new List<string>();
			if (!this.IsTrackNameCreated("position"))
			{
				list.Add("position");
			}
			if (!this.IsTrackNameCreated("rotation"))
			{
				list.Add("rotation");
			}
			if (!this.IsTrackNameCreated("scale"))
			{
				list.Add("scale");
			}
			if (!this.IsTrackNameCreated("enable"))
			{
				list.Add("enable");
			}
			list.Add("event");
			list.Add("sound");
			return list.ToArray();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003F9C File Offset: 0x0000219C
		public override bool IsTrackNameSupported(string trackName)
		{
			return trackName == "position" || trackName == "rotation" || trackName == "scale" || trackName == "event" || trackName == "sound" || trackName == "enable";
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004004 File Offset: 0x00002204
		public override bool IsTrackNameCreated(string trackName)
		{
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				ITrack track = (ITrack)monoBehaviour;
				if (track.trackName == trackName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000406C File Offset: 0x0000226C
		public override void EvaluateTime(float time)
		{
			if (this.controlledTransform == null)
			{
				Debug.Log("TransformTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return;
			}
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				ITrack track = (ITrack)monoBehaviour;
				if (!track.IsDisabled)
				{
					track.EvaluateTime(time);
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000040E8 File Offset: 0x000022E8
		public override string ToString()
		{
			return "Game Object";
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000040F0 File Offset: 0x000022F0
		public override void MoveTrackUp(ITrack track)
		{
			int num = this._tracks.IndexOf((MonoBehaviour)track);
			if (num < 0)
			{
				return;
			}
			num--;
			if (num < 0)
			{
				num = 0;
			}
			this._tracks.Remove((MonoBehaviour)track);
			this._tracks.Insert(num, (MonoBehaviour)track);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004144 File Offset: 0x00002344
		public override void MoveTrackDown(ITrack track)
		{
			int num = this._tracks.IndexOf((MonoBehaviour)track);
			if (num < 0)
			{
				return;
			}
			if (num == this._tracks.Count - 1)
			{
				return;
			}
			num++;
			this._tracks.Remove((MonoBehaviour)track);
			this._tracks.Insert(num, (MonoBehaviour)track);
		}

		// Token: 0x04000050 RID: 80
		[HideInInspector]
		public Transform objectToLook;

		// Token: 0x04000051 RID: 81
		[HideInInspector]
		public TransformTrackComponent.Wizard selectedWizard = TransformTrackComponent.Wizard.LookAtTrajectory;

		// Token: 0x04000052 RID: 82
		public Transform controlledTransform;

		// Token: 0x04000053 RID: 83
		[HideInInspector]
		public List<MonoBehaviour> _tracks = new List<MonoBehaviour>();

		// Token: 0x02000016 RID: 22
		public enum Wizard
		{
			// Token: 0x04000055 RID: 85
			EditTrackNames,
			// Token: 0x04000056 RID: 86
			TracksOrdering,
			// Token: 0x04000057 RID: 87
			LookAtTrajectory,
			// Token: 0x04000058 RID: 88
			LookAtObject,
			// Token: 0x04000059 RID: 89
			AlignAlongAxis
		}
	}
}
