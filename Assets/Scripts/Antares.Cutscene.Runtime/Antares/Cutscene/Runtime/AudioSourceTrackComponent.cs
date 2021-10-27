using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000032 RID: 50
	[CutsceneObject("Audio Source", new Type[]
	{
		typeof(AudioSource)
	})]
	public class AudioSourceTrackComponent : CutsceneObjectBase
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000BC20 File Offset: 0x00009E20
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000BC28 File Offset: 0x00009E28
		public override object ControlledObject
		{
			get
			{
				return this.audioSource;
			}
			set
			{
				try
				{
					this.audioSource = (AudioSource)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild set AudioSourceTrackComponent.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000BC68 File Offset: 0x00009E68
		public void Awake()
		{
			this.Init();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000BC70 File Offset: 0x00009E70
		public override void Init()
		{
			if (this.audioSource == null)
			{
				Debug.Log("AudiosourceTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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
					track.ControlledObject = this.audioSource;
					track.InitTrack();
					if (!this._tracks.Contains((MonoBehaviour)track))
					{
						this._tracks.Add((MonoBehaviour)track);
					}
				}
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000BD41 File Offset: 0x00009F41
		public override void InitDefaultTracks()
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000BD44 File Offset: 0x00009F44
		public override ITrack[] GetTracks()
		{
			if (this.audioSource == null)
			{
				Debug.Log("AudiosourceTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return new ITrack[0];
			}
			List<ITrack> list = new List<ITrack>();
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				list.Add((ITrack)monoBehaviour);
			}
			return list.ToArray();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000BDC8 File Offset: 0x00009FC8
		public override ITrack CreateTrack(string trackName)
		{
			if (this.audioSource == null)
			{
				Debug.Log("AudiosourceTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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
				eventTrack2.controlledObject = this.audioSource;
				eventTrack2.InitTrack();
				this._tracks.Add(eventTrack2);
				return eventTrack2;
			}
			if (trackName.StartsWith("sound"))
			{
				base.GetComponentsInChildren<SoundTrack>();
				SoundTrack soundTrack = null;
				int num3 = 1;
				foreach (SoundTrack soundTrack2 in base.GetComponentsInChildren<SoundTrack>())
				{
					try
					{
						int num4 = int.Parse(soundTrack2.trackName.Substring(5));
						if (num4 >= num3)
						{
							num3 = num4 + 1;
						}
					}
					catch
					{
					}
				}
				if (soundTrack == null)
				{
					soundTrack = new GameObject("Sound Track").AddComponent<SoundTrack>();
					soundTrack.transform.parent = base.transform;
					soundTrack.soundTrackName = trackName + num3;
				}
				soundTrack.controlledObject = this.audioSource;
				soundTrack.InitTrack();
				this._tracks.Add(soundTrack);
				return soundTrack;
			}
			return null;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000BF84 File Offset: 0x0000A184
		public override bool DeleteTrack(ITrack track)
		{
			if (this.audioSource == null)
			{
				Debug.Log("AudiosourceTrackComponent.ControlledObject is null. Please set it before call Init() function!");
				return false;
			}
			this._tracks.Remove((MonoBehaviour)track);
			UnityEngine.Object.DestroyImmediate(((MonoBehaviour)track).gameObject);
			return true;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		public override string[] GetSupportedTrackNames()
		{
			return new string[]
			{
				"sound",
				"event"
			};
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000BFEC File Offset: 0x0000A1EC
		public override string[] GetSupportedTrackNamesToCreate()
		{
			return new List<string>
			{
				"sound",
				"event"
			}.ToArray();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000C01B File Offset: 0x0000A21B
		public override bool IsTrackNameSupported(string trackName)
		{
			return trackName == "sound" || trackName == "event";
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000C03C File Offset: 0x0000A23C
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

		// Token: 0x0600027F RID: 639 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		public override void EvaluateTime(float time)
		{
			if (this.audioSource == null)
			{
				Debug.Log("AudiosourceTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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

		// Token: 0x06000280 RID: 640 RVA: 0x0000C120 File Offset: 0x0000A320
		public override string ToString()
		{
			return "Audio Source";
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000C128 File Offset: 0x0000A328
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

		// Token: 0x06000282 RID: 642 RVA: 0x0000C17C File Offset: 0x0000A37C
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

		// Token: 0x0400014A RID: 330
		public AudioSource audioSource;

		// Token: 0x0400014B RID: 331
		public List<MonoBehaviour> _tracks = new List<MonoBehaviour>();
	}
}
