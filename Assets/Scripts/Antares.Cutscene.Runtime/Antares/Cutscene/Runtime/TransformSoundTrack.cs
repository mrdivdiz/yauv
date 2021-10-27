using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200003B RID: 59
	public class TransformSoundTrack : MonoBehaviour, ITrack
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		public string trackName
		{
			get
			{
				return this.soundTrackName;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000DFB0 File Offset: 0x0000C1B0
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000DFC1 File Offset: 0x0000C1C1
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000DFCC File Offset: 0x0000C1CC
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
					Debug.Log("Faild to set TransformSoundTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000E00C File Offset: 0x0000C20C
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0000E014 File Offset: 0x0000C214
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

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000E01D File Offset: 0x0000C21D
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000E025 File Offset: 0x0000C225
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000E02E File Offset: 0x0000C22E
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000E036 File Offset: 0x0000C236
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000E03E File Offset: 0x0000C23E
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000E04D File Offset: 0x0000C24D
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E05A File Offset: 0x0000C25A
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E05C File Offset: 0x0000C25C
		public IKeyframe CreateKey(float time)
		{
			TransformSoundTrackKey transformSoundTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<TransformSoundTrackKey>();
			transformSoundTrackKey.Track = this;
			transformSoundTrackKey.keyTime = time;
			this.c.Add(transformSoundTrackKey);
			this.UpdateTrack();
			return transformSoundTrackKey;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			TransformSoundTrackKey transformSoundTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<TransformSoundTrackKey>();
			transformSoundTrackKey.Track = this;
			transformSoundTrackKey.time = ((TransformSoundTrackKey)keyframe).time;
			transformSoundTrackKey.audioClip = ((TransformSoundTrackKey)keyframe).audioClip;
			transformSoundTrackKey.volume = ((TransformSoundTrackKey)keyframe).volume;
			transformSoundTrackKey.pitch = ((TransformSoundTrackKey)keyframe).pitch;
			this.c.Add(transformSoundTrackKey);
			this.UpdateTrack();
			return transformSoundTrackKey;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E140 File Offset: 0x0000C340
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((TransformSoundTrackKey)keyframe);
			if (this.d == (TransformSoundTrackKey)keyframe)
			{
				this.d = null;
			}
			UnityEngine.Object.DestroyImmediate(((TransformSoundTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E18F File Offset: 0x0000C38F
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((TransformSoundTrackKey)keyframe);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		public void FindLeftAndRight(float time, out TransformSoundTrackKey left, out TransformSoundTrackKey right)
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

		// Token: 0x06000325 RID: 805 RVA: 0x0000E26D File Offset: 0x0000C46D
		public void DestroyLastAudio()
		{
			if (this.e != null)
			{
				UnityEngine.Object.DestroyImmediate(this.e.gameObject);
			}
			this.d = null;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E294 File Offset: 0x0000C494
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			TransformSoundTrackKey transformSoundTrackKey;
			TransformSoundTrackKey transformSoundTrackKey2;
			this.FindLeftAndRight(time, out transformSoundTrackKey, out transformSoundTrackKey2);
			if (transformSoundTrackKey)
			{
				if (time >= transformSoundTrackKey.time + transformSoundTrackKey.Length)
				{
					this.d = null;
					return;
				}
				if (this.d != transformSoundTrackKey)
				{
					if (!Application.isPlaying)
					{
						this.DestroyLastAudio();
					}
					if (transformSoundTrackKey.audioClip != null)
					{
						GameObject gameObject = new GameObject("CSOneShotAudio");
						CSDestroyObjectAterPlaySoundInternal csdestroyObjectAterPlaySoundInternal = gameObject.AddComponent<CSDestroyObjectAterPlaySoundInternal>();
						if (csdestroyObjectAterPlaySoundInternal)
						{
							csdestroyObjectAterPlaySoundInternal.trargetSoundPosition = this.controlledObject;
							csdestroyObjectAterPlaySoundInternal.transform.parent = base.transform;
						}
						AudioSource audioSource = gameObject.AddComponent<AudioSource>();
						audioSource.clip = transformSoundTrackKey.audioClip;
						switch (transformSoundTrackKey.type)
						{
						case CutSceneAudioType.Custom:
							audioSource.volume = transformSoundTrackKey.volume;
							audioSource.volume *= CSComponent.AudioVolume.customVolume;
							break;
						case CutSceneAudioType.SFX:
							audioSource.volume = CSComponent.AudioVolume.sfxVolume;
							break;
						case CutSceneAudioType.Speech:
							if (transformSoundTrackKey.localized != null && CSComponent.Localization != null)
							{
								string text = CSComponent.Localization.GetCurrentLanguage().ToLower();
								foreach (SpeechLocalizationItem speechLocalizationItem in transformSoundTrackKey.localized)
								{
									if (speechLocalizationItem.LowerdLocale == text)
									{
										audioSource.clip = speechLocalizationItem.speech;
										break;
									}
								}
							}
							audioSource.volume = CSComponent.AudioVolume.speechVolume;
							break;
						case CutSceneAudioType.Music:
							audioSource.volume = CSComponent.AudioVolume.musicVolume;
							break;
						default:
							audioSource.volume = transformSoundTrackKey.volume;
							audioSource.volume *= CSComponent.AudioVolume.customVolume;
							break;
						}
						audioSource.pitch = transformSoundTrackKey.pitch;
						audioSource.Play();
						this.e = audioSource;
					}
				}
				else if (this.e != null)
				{
					switch (transformSoundTrackKey.type)
					{
					case CutSceneAudioType.Custom:
						this.e.volume = transformSoundTrackKey.volume;
						this.e.volume *= CSComponent.AudioVolume.customVolume;
						break;
					case CutSceneAudioType.SFX:
						this.e.volume = CSComponent.AudioVolume.sfxVolume;
						break;
					case CutSceneAudioType.Speech:
						this.e.volume = CSComponent.AudioVolume.speechVolume;
						break;
					case CutSceneAudioType.Music:
						this.e.volume = CSComponent.AudioVolume.musicVolume;
						break;
					default:
						this.e.volume = transformSoundTrackKey.volume;
						this.e.volume *= CSComponent.AudioVolume.customVolume;
						break;
					}
				}
			}
			this.d = transformSoundTrackKey;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E588 File Offset: 0x0000C788
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			TransformSoundTrackKey[] componentsInChildren = base.GetComponentsInChildren<TransformSoundTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E5C8 File Offset: 0x0000C7C8
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<TransformSoundTrackKey>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.c[i].gameObject.name = "soundKey" + i;
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

		// Token: 0x06000329 RID: 809 RVA: 0x0000E692 File Offset: 0x0000C892
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

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000E6B5 File Offset: 0x0000C8B5
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000E6B8 File Offset: 0x0000C8B8
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000E6BB File Offset: 0x0000C8BB
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

		// Token: 0x0600032D RID: 813 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		public IKeyframe GetNextKey(TransformSoundTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E710 File Offset: 0x0000C910
		public IKeyframe GetPreviousKey(TransformSoundTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000192 RID: 402
		public Transform controlledObject;

		// Token: 0x04000193 RID: 403
		public string soundTrackName = "sound";

		// Token: 0x04000194 RID: 404
		public bool isDisabled;

		// Token: 0x04000195 RID: 405
		public WrapMode mode = WrapMode.Once;

		// Token: 0x04000196 RID: 406
		public Color trackColor = Color.white;

		// Token: 0x04000197 RID: 407
		private float a;

		// Token: 0x04000198 RID: 408
		private float b;

		// Token: 0x04000199 RID: 409
		private readonly List<TransformSoundTrackKey> c = new List<TransformSoundTrackKey>();

		// Token: 0x0400019A RID: 410
		private TransformSoundTrackKey d;

		// Token: 0x0400019B RID: 411
		private AudioSource e;
	}
}
