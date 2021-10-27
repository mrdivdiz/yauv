using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200002D RID: 45
	public class SoundTrack : MonoBehaviour, ITrack
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009BEC File Offset: 0x00007DEC
		public string trackName
		{
			get
			{
				return this.soundTrackName;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00009BF4 File Offset: 0x00007DF4
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00009BFC File Offset: 0x00007DFC
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

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00009C05 File Offset: 0x00007E05
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00009C10 File Offset: 0x00007E10
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
					this.controlledObject = (AudioSource)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set SoundTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00009C50 File Offset: 0x00007E50
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00009C58 File Offset: 0x00007E58
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00009C61 File Offset: 0x00007E61
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00009C69 File Offset: 0x00007E69
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00009C72 File Offset: 0x00007E72
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00009C7A File Offset: 0x00007E7A
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00009C82 File Offset: 0x00007E82
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00009C91 File Offset: 0x00007E91
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009C9E File Offset: 0x00007E9E
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009CA0 File Offset: 0x00007EA0
		public IKeyframe CreateKey(float time)
		{
			SoundTrackKey soundTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<SoundTrackKey>();
			soundTrackKey.Track = this;
			soundTrackKey.keyTime = time;
			this.c.Add(soundTrackKey);
			this.UpdateTrack();
			return soundTrackKey;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009CF4 File Offset: 0x00007EF4
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			SoundTrackKey soundTrackKey = new GameObject("soundKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<SoundTrackKey>();
			soundTrackKey.Track = this;
			soundTrackKey.time = ((SoundTrackKey)keyframe).time;
			soundTrackKey.audioClip = ((SoundTrackKey)keyframe).audioClip;
			soundTrackKey.volume = ((SoundTrackKey)keyframe).volume;
			soundTrackKey.pitch = ((SoundTrackKey)keyframe).pitch;
			this.c.Add(soundTrackKey);
			this.UpdateTrack();
			return soundTrackKey;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009D84 File Offset: 0x00007F84
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((SoundTrackKey)keyframe);
			if (this.d == (SoundTrackKey)keyframe)
			{
				this.d = null;
			}
			UnityEngine.Object.DestroyImmediate(((SoundTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009DD3 File Offset: 0x00007FD3
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((SoundTrackKey)keyframe);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public void FindLeftAndRight(float time, out SoundTrackKey left, out SoundTrackKey right)
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

		// Token: 0x0600020D RID: 525 RVA: 0x00009EB4 File Offset: 0x000080B4
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			SoundTrackKey soundTrackKey;
			SoundTrackKey soundTrackKey2;
			this.FindLeftAndRight(time, out soundTrackKey, out soundTrackKey2);
			if (soundTrackKey)
			{
				if (time < soundTrackKey.time + soundTrackKey.Length)
				{
					if (this.d != soundTrackKey)
					{
						if (!(soundTrackKey.audioClip != null))
						{
							goto IL_640;
						}
						try
						{
							this.controlledObject.clip = soundTrackKey.audioClip;
							if (soundTrackKey2 != null)
							{
								float t = (time - soundTrackKey.time) / (soundTrackKey2.time - soundTrackKey.time);
								switch (soundTrackKey.type)
								{
								case CutSceneAudioType.Custom:
									this.controlledObject.volume = (soundTrackKey.volumeLerp ? Mathf.Lerp(soundTrackKey.volume, soundTrackKey2.volume, t) : soundTrackKey.volume);
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								case CutSceneAudioType.SFX:
									this.controlledObject.volume = CSComponent.AudioVolume.sfxVolume;
									break;
								case CutSceneAudioType.Speech:
									if (soundTrackKey.localized != null && CSComponent.Localization != null)
									{
										string text = CSComponent.Localization.GetCurrentLanguage().ToLower();
										foreach (SpeechLocalizationItem speechLocalizationItem in soundTrackKey.localized)
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
									this.controlledObject.volume = (soundTrackKey.volumeLerp ? Mathf.Lerp(soundTrackKey.volume, soundTrackKey2.volume, t) : soundTrackKey.volume);
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								}
								this.controlledObject.pitch = (soundTrackKey.pitchLerp ? Mathf.Lerp(soundTrackKey.pitch, soundTrackKey2.pitch, t) : soundTrackKey.pitch);
							}
							else
							{
								switch (soundTrackKey.type)
								{
								case CutSceneAudioType.Custom:
									this.controlledObject.volume = soundTrackKey.volume;
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								case CutSceneAudioType.SFX:
									this.controlledObject.volume = CSComponent.AudioVolume.sfxVolume;
									break;
								case CutSceneAudioType.Speech:
									if (soundTrackKey.localized != null && CSComponent.Localization != null)
									{
										string text2 = CSComponent.Localization.GetCurrentLanguage().ToLower();
										foreach (SpeechLocalizationItem speechLocalizationItem2 in soundTrackKey.localized)
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
									this.controlledObject.volume = soundTrackKey.volume;
									this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
									break;
								}
								this.controlledObject.pitch = soundTrackKey.pitch;
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
						if (soundTrackKey2 != null)
						{
							float t2 = (time - soundTrackKey.time) / (soundTrackKey2.time - soundTrackKey.time);
							switch (soundTrackKey.type)
							{
							case CutSceneAudioType.Custom:
								this.controlledObject.volume = (soundTrackKey.volumeLerp ? Mathf.Lerp(soundTrackKey.volume, soundTrackKey2.volume, t2) : soundTrackKey.volume);
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
								this.controlledObject.volume = (soundTrackKey.volumeLerp ? Mathf.Lerp(soundTrackKey.volume, soundTrackKey2.volume, t2) : soundTrackKey.volume);
								this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
								break;
							}
							this.controlledObject.pitch = (soundTrackKey.pitchLerp ? Mathf.Lerp(soundTrackKey.pitch, soundTrackKey2.pitch, t2) : soundTrackKey.pitch);
						}
						else
						{
							switch (soundTrackKey.type)
							{
							case CutSceneAudioType.Custom:
								this.controlledObject.volume = soundTrackKey.volume;
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
								this.controlledObject.volume = soundTrackKey.volume;
								this.controlledObject.volume *= CSComponent.AudioVolume.customVolume;
								break;
							}
							this.controlledObject.pitch = soundTrackKey.pitch;
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
			this.d = soundTrackKey;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A56C File Offset: 0x0000876C
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			SoundTrackKey[] componentsInChildren = base.GetComponentsInChildren<SoundTrackKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A5AC File Offset: 0x000087AC
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<SoundTrackKey>(this.CompareKeys));
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

		// Token: 0x06000210 RID: 528 RVA: 0x0000A678 File Offset: 0x00008878
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			SoundTrackKey soundTrackKey = (SoundTrackKey)one;
			SoundTrackKey soundTrackKey2 = (SoundTrackKey)other;
			if (soundTrackKey.keyTime > soundTrackKey2.keyTime)
			{
				return 1;
			}
			if (soundTrackKey.keyTime < soundTrackKey2.keyTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000A6B4 File Offset: 0x000088B4
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A6B7 File Offset: 0x000088B7
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000A6BA File Offset: 0x000088BA
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

		// Token: 0x06000214 RID: 532 RVA: 0x0000A6C8 File Offset: 0x000088C8
		public IKeyframe GetNextKey(SoundTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000A710 File Offset: 0x00008910
		public IKeyframe GetPreviousKey(SoundTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x04000116 RID: 278
		public AudioSource controlledObject;

		// Token: 0x04000117 RID: 279
		public string soundTrackName = "sound";

		// Token: 0x04000118 RID: 280
		public bool isDisabled;

		// Token: 0x04000119 RID: 281
		public WrapMode mode = WrapMode.Once;

		// Token: 0x0400011A RID: 282
		public Color trackColor = Color.white;

		// Token: 0x0400011B RID: 283
		private float a;

		// Token: 0x0400011C RID: 284
		private float b;

		// Token: 0x0400011D RID: 285
		private readonly List<SoundTrackKey> c = new List<SoundTrackKey>();

		// Token: 0x0400011E RID: 286
		private SoundTrackKey d;
	}
}
