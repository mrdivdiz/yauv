using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200001F RID: 31
	public class DirectorSoundTrack : MonoBehaviour, ITrack
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000052DC File Offset: 0x000034DC
		public string trackName
		{
			get
			{
				return this.soundTrackName;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000052E4 File Offset: 0x000034E4
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000052EC File Offset: 0x000034EC
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

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000052F5 File Offset: 0x000034F5
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00005300 File Offset: 0x00003500
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
					this.controlledObject = ((Component)value).GetComponent<AudioSource>();
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set DirectorSoundTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00005344 File Offset: 0x00003544
		// (set) Token: 0x06000151 RID: 337 RVA: 0x0000534C File Offset: 0x0000354C
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00005355 File Offset: 0x00003555
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000535D File Offset: 0x0000355D
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005366 File Offset: 0x00003566
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000536E File Offset: 0x0000356E
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00005376 File Offset: 0x00003576
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00005385 File Offset: 0x00003585
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005392 File Offset: 0x00003592
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005394 File Offset: 0x00003594
		public IKeyframe CreateKey(float time)
		{
			DirectorSoundTrackKey directorSoundTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<DirectorSoundTrackKey>();
			directorSoundTrackKey.dsTrack = this;
			directorSoundTrackKey.keyTime = time;
			this.c.Add(directorSoundTrackKey);
			this.UpdateTrack();
			return directorSoundTrackKey;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000053E8 File Offset: 0x000035E8
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			DirectorSoundTrackKey directorSoundTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<DirectorSoundTrackKey>();
			directorSoundTrackKey.dsTrack = this;
			directorSoundTrackKey.time = ((DirectorSoundTrackKey)keyframe).time;
			directorSoundTrackKey.audioClip = ((DirectorSoundTrackKey)keyframe).audioClip;
			directorSoundTrackKey.volume = ((DirectorSoundTrackKey)keyframe).volume;
			directorSoundTrackKey.pitch = ((DirectorSoundTrackKey)keyframe).pitch;
			this.c.Add(directorSoundTrackKey);
			this.UpdateTrack();
			return directorSoundTrackKey;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005478 File Offset: 0x00003678
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((DirectorSoundTrackKey)keyframe);
			if (this.d == (DirectorSoundTrackKey)keyframe)
			{
				this.d = null;
			}
			UnityEngine.Object.DestroyImmediate(((DirectorSoundTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000054C7 File Offset: 0x000036C7
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((DirectorSoundTrackKey)keyframe);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000054DC File Offset: 0x000036DC
		public void FindLeftAndRight(float time, out DirectorSoundTrackKey left, out DirectorSoundTrackKey right)
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

		// Token: 0x0600015E RID: 350 RVA: 0x000055A8 File Offset: 0x000037A8
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			DirectorSoundTrackKey directorSoundTrackKey;
			DirectorSoundTrackKey directorSoundTrackKey2;
			this.FindLeftAndRight(time, out directorSoundTrackKey, out directorSoundTrackKey2);
			if (directorSoundTrackKey)
			{
				if (time < directorSoundTrackKey.time + directorSoundTrackKey.Length)
				{
					if (this.d != directorSoundTrackKey)
					{
						if (!(directorSoundTrackKey.audioClip != null))
						{
							goto IL_640;
						}
						try
						{
							this.controlledObject.clip = directorSoundTrackKey.audioClip;
							if (directorSoundTrackKey2 != null)
							{
								float t = (time - directorSoundTrackKey.time) / (directorSoundTrackKey2.time - directorSoundTrackKey.time);
								switch (directorSoundTrackKey.type)
								{
								case CutSceneAudioType.Custom:
									this.controlledObject.volume = (directorSoundTrackKey.volumeLerp ? Mathf.Lerp(directorSoundTrackKey.volume, directorSoundTrackKey2.volume, t) : directorSoundTrackKey.volume);
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								case CutSceneAudioType.SFX:
									this.controlledObject.volume = CSComponent.AudioVolume.sfxVolume;
									break;
								case CutSceneAudioType.Speech:
									if (directorSoundTrackKey.localized != null && CSComponent.Localization != null)
									{
										string text = CSComponent.Localization.GetCurrentLanguage().ToLower();
										foreach (SpeechLocalizationItem speechLocalizationItem in directorSoundTrackKey.localized)
										{
											if (speechLocalizationItem.LowerdLocale == text)
											{
												this.controlledObject.clip = speechLocalizationItem.speech;
												break;
											}
										}
									}
									this.controlledObject.volume = CSComponent.AudioVolume.speechVolume;
									break;
								case CutSceneAudioType.Music:
									this.controlledObject.volume = CSComponent.AudioVolume.musicVolume;
									break;
								default:
									this.controlledObject.volume = (directorSoundTrackKey.volumeLerp ? Mathf.Lerp(directorSoundTrackKey.volume, directorSoundTrackKey2.volume, t) : directorSoundTrackKey.volume);
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								}
								this.controlledObject.pitch = (directorSoundTrackKey.pitchLerp ? Mathf.Lerp(directorSoundTrackKey.pitch, directorSoundTrackKey2.pitch, t) : directorSoundTrackKey.pitch);
							}
							else
							{
								switch (directorSoundTrackKey.type)
								{
								case CutSceneAudioType.Custom:
									this.controlledObject.volume = directorSoundTrackKey.volume;
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								case CutSceneAudioType.SFX:
									this.controlledObject.volume = CSComponent.AudioVolume.sfxVolume;
									break;
								case CutSceneAudioType.Speech:
									if (directorSoundTrackKey.localized != null && CSComponent.Localization != null)
									{
										string text2 = CSComponent.Localization.GetCurrentLanguage().ToLower();
										foreach (SpeechLocalizationItem speechLocalizationItem2 in directorSoundTrackKey.localized)
										{
											if (speechLocalizationItem2.LowerdLocale == text2)
											{
												this.controlledObject.clip = speechLocalizationItem2.speech;
												break;
											}
										}
									}
									this.controlledObject.volume = CSComponent.AudioVolume.speechVolume;
									break;
								case CutSceneAudioType.Music:
									this.controlledObject.volume = CSComponent.AudioVolume.musicVolume;
									break;
								default:
									this.controlledObject.volume = directorSoundTrackKey.volume;
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								}
								this.controlledObject.pitch = directorSoundTrackKey.pitch;
							}
							this.controlledObject.Play();
							goto IL_640;
						}
						catch (Exception arg)
						{
							Debug.Log("play\n" + arg);
							goto IL_640;
						}
					}
					if (!(this.controlledObject.clip != null))
					{
						goto IL_640;
					}
					try
					{
						if (directorSoundTrackKey2 != null)
						{
							float t2 = (time - directorSoundTrackKey.time) / (directorSoundTrackKey2.time - directorSoundTrackKey.time);
							switch (directorSoundTrackKey.type)
							{
							case CutSceneAudioType.Custom:
								this.controlledObject.volume = (directorSoundTrackKey.volumeLerp ? Mathf.Lerp(directorSoundTrackKey.volume, directorSoundTrackKey2.volume, t2) : directorSoundTrackKey.volume);
								this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
								break;
							case CutSceneAudioType.SFX:
								this.controlledObject.volume = CSComponent.AudioVolume.sfxVolume;
								break;
							case CutSceneAudioType.Speech:
								this.controlledObject.volume = CSComponent.AudioVolume.speechVolume;
								break;
							case CutSceneAudioType.Music:
								this.controlledObject.volume = CSComponent.AudioVolume.musicVolume;
								break;
							default:
								this.controlledObject.volume = (directorSoundTrackKey.volumeLerp ? Mathf.Lerp(directorSoundTrackKey.volume, directorSoundTrackKey2.volume, t2) : directorSoundTrackKey.volume);
								this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
								break;
							}
							this.controlledObject.pitch = (directorSoundTrackKey.pitchLerp ? Mathf.Lerp(directorSoundTrackKey.pitch, directorSoundTrackKey2.pitch, t2) : directorSoundTrackKey.pitch);
						}
						else
						{
							switch (directorSoundTrackKey.type)
							{
							case CutSceneAudioType.Custom:
								this.controlledObject.volume = directorSoundTrackKey.volume;
								this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
								break;
							case CutSceneAudioType.SFX:
								this.controlledObject.volume = CSComponent.AudioVolume.sfxVolume;
								break;
							case CutSceneAudioType.Speech:
								this.controlledObject.volume = CSComponent.AudioVolume.speechVolume;
								break;
							case CutSceneAudioType.Music:
								this.controlledObject.volume = CSComponent.AudioVolume.musicVolume;
								break;
							default:
								this.controlledObject.volume = directorSoundTrackKey.volume;
								this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
								break;
							}
							this.controlledObject.pitch = directorSoundTrackKey.pitch;
						}
						goto IL_640;
					}
					catch (Exception arg2)
					{
						Debug.Log("play\n" + arg2);
						goto IL_640;
					}
				}
				this.d = null;
				return;
			}
			if (this.controlledObject)
			{
				if (this.controlledObject.isPlaying)
				{
					this.controlledObject.Stop();
				}
				this.controlledObject.clip = null;
			}
			IL_640:
			this.d = directorSoundTrackKey;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005C60 File Offset: 0x00003E60
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			DirectorSoundTrackKey[] componentsInChildren = base.GetComponentsInChildren<DirectorSoundTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005CA0 File Offset: 0x00003EA0
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<DirectorSoundTrackKey>(this.CompareKeys));
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

		// Token: 0x06000161 RID: 353 RVA: 0x00005D6C File Offset: 0x00003F6C
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			DirectorSoundTrackKey directorSoundTrackKey = (DirectorSoundTrackKey)one;
			DirectorSoundTrackKey directorSoundTrackKey2 = (DirectorSoundTrackKey)other;
			if (directorSoundTrackKey.keyTime > directorSoundTrackKey2.keyTime)
			{
				return 1;
			}
			if (directorSoundTrackKey.keyTime < directorSoundTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00005DAB File Offset: 0x00003FAB
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00005DAE File Offset: 0x00003FAE
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

		// Token: 0x06000165 RID: 357 RVA: 0x00005DBC File Offset: 0x00003FBC
		public IKeyframe GetNextKey(DirectorSoundTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005E04 File Offset: 0x00004004
		public IKeyframe GetPreviousKey(DirectorSoundTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000089 RID: 137
		public AudioSource controlledObject;

		// Token: 0x0400008A RID: 138
		public string soundTrackName = "sound";

		// Token: 0x0400008B RID: 139
		public bool isDisabled;

		// Token: 0x0400008C RID: 140
		public WrapMode mode = WrapMode.Once;

		// Token: 0x0400008D RID: 141
		public Color trackColor = Color.white;

		// Token: 0x0400008E RID: 142
		private float a;

		// Token: 0x0400008F RID: 143
		private float b;

		// Token: 0x04000090 RID: 144
		private readonly List<DirectorSoundTrackKey> c = new List<DirectorSoundTrackKey>();

		// Token: 0x04000091 RID: 145
		private DirectorSoundTrackKey d;
	}
}
