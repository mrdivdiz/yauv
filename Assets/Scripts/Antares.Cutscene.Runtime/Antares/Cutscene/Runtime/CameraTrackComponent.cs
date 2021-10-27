using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000039 RID: 57
	[CutsceneObject("Camera", new Type[]
	{
		typeof(Camera)
	})]
	public class CameraTrackComponent : CutsceneObjectBase
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000D8AD File Offset: 0x0000BAAD
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		public override object ControlledObject
		{
			get
			{
				return this.controlledCamera;
			}
			set
			{
				try
				{
					this.controlledCamera = (Camera)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild set CameraTrackComponent.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
		public void Awake()
		{
			this.Init();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000D900 File Offset: 0x0000BB00
		public override void Init()
		{
			if (this.controlledCamera == null)
			{
				Debug.Log("CameraTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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
					track.ControlledObject = this.controlledCamera;
					track.InitTrack();
					if (!this._tracks.Contains((MonoBehaviour)track))
					{
						this._tracks.Add((MonoBehaviour)track);
					}
				}
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000D9D1 File Offset: 0x0000BBD1
		public override void InitDefaultTracks()
		{
			this.CreateTrack("FOV");
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000D9E0 File Offset: 0x0000BBE0
		public override ITrack[] GetTracks()
		{
			if (this.controlledCamera == null)
			{
				Debug.Log("CameraTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return new ITrack[0];
			}
			List<ITrack> list = new List<ITrack>();
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				list.Add((ITrack)monoBehaviour);
			}
			return list.ToArray();
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000DA64 File Offset: 0x0000BC64
		public override ITrack CreateTrack(string trackName)
		{
			if (this.controlledCamera == null)
			{
				Debug.Log("CameraTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return null;
			}
			if (trackName == "Clipping Planes")
			{
				ClippingPlanesTrack clippingPlanesTrack = base.GetComponentInChildren<ClippingPlanesTrack>();
				if (clippingPlanesTrack == null)
				{
					clippingPlanesTrack = new GameObject("Clipping Planes Track").AddComponent<ClippingPlanesTrack>();
					clippingPlanesTrack.transform.parent = base.transform;
				}
				clippingPlanesTrack.controlledObject = this.controlledCamera;
				clippingPlanesTrack.InitTrack();
				this._tracks.Add(clippingPlanesTrack);
				return clippingPlanesTrack;
			}
			if (trackName == "FOV")
			{
				FovTrack fovTrack = base.GetComponentInChildren<FovTrack>();
				if (fovTrack == null)
				{
					fovTrack = new GameObject("FOV Track").AddComponent<FovTrack>();
					fovTrack.transform.parent = base.transform;
				}
				fovTrack.controlledObject = this.controlledCamera;
				fovTrack.InitTrack();
				this._tracks.Add(fovTrack);
				return fovTrack;
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
				eventTrack2.controlledObject = this.controlledCamera;
				eventTrack2.InitTrack();
				this._tracks.Add(eventTrack2);
				return eventTrack2;
			}
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
				transformSoundTrack2.controlledObject = this.controlledCamera.transform;
				transformSoundTrack2.InitTrack();
				this._tracks.Add(transformSoundTrack2);
				return transformSoundTrack2;
			}
			return null;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		public override bool DeleteTrack(ITrack track)
		{
			if (this.controlledCamera == null)
			{
				Debug.Log("CameraTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return false;
			}
			this._tracks.Remove((MonoBehaviour)track);
			UnityEngine.Object.DestroyImmediate(((MonoBehaviour)track).gameObject);
			return true;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000DD20 File Offset: 0x0000BF20
		public override string[] GetSupportedTrackNames()
		{
			return new string[]
			{
				"FOV",
				"event",
				"sound",
				"Clipping Planes"
			};
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public override string[] GetSupportedTrackNamesToCreate()
		{
			List<string> list = new List<string>();
			if (!this.IsTrackNameCreated("FOV"))
			{
				list.Add("FOV");
			}
			if (!this.IsTrackNameCreated("Clipping Planes"))
			{
				list.Add("Clipping Planes");
			}
			list.Add("event");
			list.Add("sound");
			return list.ToArray();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000DDB7 File Offset: 0x0000BFB7
		public override bool IsTrackNameSupported(string trackName)
		{
			return trackName == "FOV" || trackName == "event" || trackName == "sound" || trackName == "Clipping Planes";
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
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

		// Token: 0x0600030D RID: 781 RVA: 0x0000DE60 File Offset: 0x0000C060
		public override void EvaluateTime(float time)
		{
			if (this.controlledCamera == null)
			{
				Debug.Log("CameraTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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

		// Token: 0x0600030E RID: 782 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public override string ToString()
		{
			return "Camera";
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
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

		// Token: 0x06000310 RID: 784 RVA: 0x0000DF38 File Offset: 0x0000C138
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

		// Token: 0x0400018A RID: 394
		public Camera controlledCamera;

		// Token: 0x0400018B RID: 395
		[HideInInspector]
		public List<MonoBehaviour> _tracks = new List<MonoBehaviour>();
	}
}
