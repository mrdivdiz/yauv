using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200000C RID: 12
	[CutsceneObject("Director Node", new Type[]
	{
		typeof(CSComponent)
	})]
	public class DirectorTrackComponent : CutsceneObjectBase
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002B6B File Offset: 0x00000D6B
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002B74 File Offset: 0x00000D74
		public override object ControlledObject
		{
			get
			{
				return this.cutscene;
			}
			set
			{
				try
				{
					this.cutscene = (CSComponent)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild set DirectorTrackComponent.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public void Awake()
		{
			this.Init();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002BBC File Offset: 0x00000DBC
		public override void Init()
		{
			if (base.GetComponent<AudioSource>() == null)
			{
				base.gameObject.AddComponent<AudioSource>();
			}
			base.GetComponent<AudioSource>().playOnAwake = false;
			if (this.cutscene == null)
			{
				Debug.Log("DirectorTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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
					if (track is DirectorSoundTrack)
					{
						track.ControlledObject = base.GetComponent<AudioSource>();
					}
					else
					{
						track.ControlledObject = this.cutscene;
					}
					track.InitTrack();
					if (!this._tracks.Contains((MonoBehaviour)track))
					{
						this._tracks.Add((MonoBehaviour)track);
					}
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002CC9 File Offset: 0x00000EC9
		public override void InitDefaultTracks()
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002CCC File Offset: 0x00000ECC
		public override ITrack[] GetTracks()
		{
			if (this.cutscene == null)
			{
				Debug.Log("DirectorTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return new ITrack[0];
			}
			List<ITrack> list = new List<ITrack>();
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				list.Add((ITrack)monoBehaviour);
			}
			return list.ToArray();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002D50 File Offset: 0x00000F50
		public override ITrack CreateTrack(string trackName)
		{
			if (this.cutscene == null)
			{
				Debug.Log("DirectorTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return null;
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
				eventTrack2.controlledObject = this.cutscene;
				eventTrack2.InitTrack();
				this._tracks.Add(eventTrack2);
				return eventTrack2;
			}
			if (trackName.StartsWith("music"))
			{
				DirectorSoundTrack directorSoundTrack = null;
				if (directorSoundTrack == null)
				{
					directorSoundTrack = new GameObject("Music Track").AddComponent<DirectorSoundTrack>();
					directorSoundTrack.transform.parent = base.transform;
					directorSoundTrack.soundTrackName = "music";
				}
				directorSoundTrack.ControlledObject = base.GetComponent<AudioSource>();
				directorSoundTrack.InitTrack();
				this._tracks.Add(directorSoundTrack);
				return directorSoundTrack;
			}
			if (trackName.StartsWith("cutscene"))
			{
				CutsceneTrack cutsceneTrack = null;
				if (cutsceneTrack == null)
				{
					cutsceneTrack = new GameObject("Cutscene Track").AddComponent<CutsceneTrack>();
					cutsceneTrack.transform.parent = base.transform;
					cutsceneTrack.cutsceneTrackName = "cutscene";
				}
				cutsceneTrack.ControlledObject = this.cutscene;
				cutsceneTrack.InitTrack();
				this._tracks.Add(cutsceneTrack);
				return cutsceneTrack;
			}
			if (trackName.StartsWith("subtitles"))
			{
				SubtitlesTrack subtitlesTrack = null;
				if (subtitlesTrack == null)
				{
					subtitlesTrack = new GameObject("Subtitles Track").AddComponent<SubtitlesTrack>();
					subtitlesTrack.transform.parent = base.transform;
					subtitlesTrack.subtitlesTrackName = "subtitles";
				}
				subtitlesTrack.ControlledObject = this.cutscene;
				subtitlesTrack.InitTrack();
				this._tracks.Add(subtitlesTrack);
				return subtitlesTrack;
			}
			if (trackName.StartsWith("camera"))
			{
				DirectorCameraTrack directorCameraTrack = null;
				if (directorCameraTrack == null)
				{
					directorCameraTrack = new GameObject("Camera Track").AddComponent<DirectorCameraTrack>();
					directorCameraTrack.transform.parent = base.transform;
					directorCameraTrack.cameraTrackName = "camera";
				}
				directorCameraTrack.ControlledObject = this.cutscene;
				directorCameraTrack.InitTrack();
				this._tracks.Add(directorCameraTrack);
				return directorCameraTrack;
			}
			return null;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002FEC File Offset: 0x000011EC
		public override bool DeleteTrack(ITrack track)
		{
			if (this.cutscene == null)
			{
				Debug.Log("DirectorTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return false;
			}
			this._tracks.Remove((MonoBehaviour)track);
			UnityEngine.Object.DestroyImmediate(((MonoBehaviour)track).gameObject);
			return true;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000302C File Offset: 0x0000122C
		public override string[] GetSupportedTrackNames()
		{
			return new string[]
			{
				"music",
				"event",
				"cutscene",
				"subtitles",
				"camera"
			};
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000306C File Offset: 0x0000126C
		public override string[] GetSupportedTrackNamesToCreate()
		{
			List<string> list = new List<string>();
			if (!this.IsTrackNameCreated("music"))
			{
				list.Add("music");
			}
			if (!this.IsTrackNameCreated("cutscene"))
			{
				list.Add("cutscene");
			}
			if (!this.IsTrackNameCreated("subtitles"))
			{
				list.Add("subtitles");
			}
			if (!this.IsTrackNameCreated("camera"))
			{
				list.Add("camera");
			}
			list.Add("event");
			return list.ToArray();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000030F0 File Offset: 0x000012F0
		public override bool IsTrackNameSupported(string trackName)
		{
			return trackName == "music" || trackName == "event" || trackName == "cutscene" || trackName == "subtitles" || trackName == "camera";
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000314C File Offset: 0x0000134C
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

		// Token: 0x06000075 RID: 117 RVA: 0x000031B4 File Offset: 0x000013B4
		public override void EvaluateTime(float time)
		{
			if (this.cutscene == null)
			{
				Debug.Log("DirectorTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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

		// Token: 0x06000076 RID: 118 RVA: 0x00003230 File Offset: 0x00001430
		public override string ToString()
		{
			return "Director Node";
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003238 File Offset: 0x00001438
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

		// Token: 0x06000078 RID: 120 RVA: 0x0000328C File Offset: 0x0000148C
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

		// Token: 0x0400002E RID: 46
		public CSComponent cutscene;

		// Token: 0x0400002F RID: 47
		public List<MonoBehaviour> _tracks = new List<MonoBehaviour>();
	}
}
