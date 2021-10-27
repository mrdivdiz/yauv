using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000036 RID: 54
	[CutsceneObject("Animation", new Type[]
	{
		typeof(Animation)
	})]
	public class AnimationTrackComponent : CutsceneObjectBase
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000CA35 File Offset: 0x0000AC35
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x0000CA40 File Offset: 0x0000AC40
		public override object ControlledObject
		{
			get
			{
				return this.controlledAnimation;
			}
			set
			{
				try
				{
					this.controlledAnimation = (Animation)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild set AnimationTrackComponent.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000CA80 File Offset: 0x0000AC80
		public void Awake()
		{
			this.Init();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000CA88 File Offset: 0x0000AC88
		public override void Init()
		{
			if (this.controlledAnimation == null)
			{
				Debug.Log("AnimationTrackComponent.ControlledObject is null. Please set it before call Init() function!");
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
					if (track is TransformSoundTrack)
					{
						track.ControlledObject = this.controlledAnimation.transform;
					}
					else
					{
						track.ControlledObject = this.controlledAnimation;
					}
					track.InitTrack();
					if (!this._tracks.Contains((MonoBehaviour)track))
					{
						this._tracks.Add((MonoBehaviour)track);
					}
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000CB74 File Offset: 0x0000AD74
		public override void InitDefaultTracks()
		{
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000CB78 File Offset: 0x0000AD78
		public override ITrack[] GetTracks()
		{
			if (this.controlledAnimation == null)
			{
				Debug.Log("AnimationTrackComponent.ControlledObject is null. Please set it before call GetTracks() function!");
				return new ITrack[0];
			}
			List<ITrack> list = new List<ITrack>();
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				list.Add((ITrack)monoBehaviour);
			}
			return list.ToArray();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		public override ITrack CreateTrack(string trackName)
		{
			if (this.controlledAnimation == null)
			{
				Debug.Log("AnimationTrackComponent.ControlledObject is null. Please set it before call CreateTrack() function!");
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
				eventTrack2.controlledObject = this.controlledAnimation;
				eventTrack2.InitTrack();
				this._tracks.Add(eventTrack2);
				return eventTrack2;
			}
			if (trackName.StartsWith("animation"))
			{
				int num3 = 1;
				foreach (AnimationClipTrack animationClipTrack in base.GetComponentsInChildren<AnimationClipTrack>())
				{
					try
					{
						int num4 = int.Parse(animationClipTrack.trackName.Substring(9));
						if (num4 >= num3)
						{
							num3 = num4 + 1;
						}
					}
					catch
					{
					}
				}
				AnimationClipTrack animationClipTrack2 = new GameObject("Animation Track").AddComponent<AnimationClipTrack>();
				animationClipTrack2.trackUserName = "animation" + num3.ToString().Trim();
				animationClipTrack2.transform.parent = base.transform;
				animationClipTrack2.controlledObject = this.controlledAnimation;
				animationClipTrack2.controlledclip = this.controlledAnimation.clip;
				animationClipTrack2.InitTrack();
				this._tracks.Add(animationClipTrack2);
				return animationClipTrack2;
			}
			if (trackName.StartsWith("sound"))
			{
				int num5 = 1;
				foreach (TransformSoundTrack transformSoundTrack in base.GetComponentsInChildren<TransformSoundTrack>())
				{
					try
					{
						int num6 = int.Parse(transformSoundTrack.trackName.Substring(5));
						if (num6 >= num5)
						{
							num5 = num6 + 1;
						}
					}
					catch
					{
					}
				}
				TransformSoundTrack transformSoundTrack2 = new GameObject("Sound Track").AddComponent<TransformSoundTrack>();
				transformSoundTrack2.soundTrackName = "sound" + num5.ToString().Trim();
				transformSoundTrack2.transform.parent = base.transform;
				transformSoundTrack2.controlledObject = this.controlledAnimation.transform;
				transformSoundTrack2.InitTrack();
				this._tracks.Add(transformSoundTrack2);
				return transformSoundTrack2;
			}
			return null;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000CE90 File Offset: 0x0000B090
		public override bool DeleteTrack(ITrack track)
		{
			if (this.controlledAnimation == null)
			{
				Debug.Log("AnimationTrackComponent.ControlledObject is null. Please set it before call DeleteTrack() function!");
				return false;
			}
			this._tracks.Remove((MonoBehaviour)track);
			UnityEngine.Object.DestroyImmediate(((MonoBehaviour)track).gameObject);
			return true;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000CED0 File Offset: 0x0000B0D0
		public override string[] GetSupportedTrackNames()
		{
			if (this.controlledAnimation == null)
			{
				Debug.Log("AnimationTrackComponent.ControlledObject is null. Please set it before call GetSupportedTrackNames() function!");
				return new string[0];
			}
			return new string[]
			{
				"animation",
				"event",
				"sound"
			};
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000CF1C File Offset: 0x0000B11C
		public override string[] GetSupportedTrackNamesToCreate()
		{
			return new List<string>
			{
				"animation",
				"event",
				"sound"
			}.ToArray();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000CF58 File Offset: 0x0000B158
		public override bool IsTrackNameSupported(string trackName)
		{
			return new List<string>
			{
				"event",
				"animation",
				"sound"
			}.Contains(trackName);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000CF94 File Offset: 0x0000B194
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

		// Token: 0x060002CF RID: 719 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		public override void EvaluateTime(float time)
		{
			if (this.controlledAnimation == null)
			{
				Debug.Log("AnimationTrackComponent.ControlledObject is null. Please set it before call EvaluateTime() function!");
				return;
			}
			foreach (object obj in this.controlledAnimation)
			{
				AnimationState animationState = (AnimationState)obj;
				animationState.enabled = false;
				animationState.weight = 0f;
			}
			foreach (MonoBehaviour monoBehaviour in this._tracks)
			{
				ITrack track = (ITrack)monoBehaviour;
				if (!track.IsDisabled)
				{
					track.EvaluateTime(time);
				}
			}
			this.controlledAnimation.Sample();
			foreach (object obj2 in this.controlledAnimation)
			{
				AnimationState animationState2 = (AnimationState)obj2;
				animationState2.enabled = false;
				animationState2.weight = 0f;
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D134 File Offset: 0x0000B334
		public override string ToString()
		{
			return "Animanion";
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000D13C File Offset: 0x0000B33C
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

		// Token: 0x060002D2 RID: 722 RVA: 0x0000D190 File Offset: 0x0000B390
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

		// Token: 0x0400016F RID: 367
		public Animation controlledAnimation;

		// Token: 0x04000170 RID: 368
		[HideInInspector]
		public List<MonoBehaviour> _tracks = new List<MonoBehaviour>();
	}
}
