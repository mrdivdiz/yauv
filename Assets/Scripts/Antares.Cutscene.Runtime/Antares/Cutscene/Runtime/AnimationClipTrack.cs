using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000029 RID: 41
	public class AnimationClipTrack : MonoBehaviour, ITrack
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00006E67 File Offset: 0x00005067
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00006E6F File Offset: 0x0000506F
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00006E78 File Offset: 0x00005078
		public string trackName
		{
			get
			{
				if (string.IsNullOrEmpty(this.trackUserName))
				{
					this.trackUserName = this.controlledclip.name;
				}
				return this.trackUserName;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00006E9E File Offset: 0x0000509E
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00006EA8 File Offset: 0x000050A8
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
					this.controlledObject = (Animation)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set AnimationClipTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006EE8 File Offset: 0x000050E8
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00006EF0 File Offset: 0x000050F0
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00006EF9 File Offset: 0x000050F9
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00006F01 File Offset: 0x00005101
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00006F0A File Offset: 0x0000510A
		public float startTime
		{
			get
			{
				return this.auf;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00006F12 File Offset: 0x00005112
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00006F1A File Offset: 0x0000511A
		public float length
		{
			get
			{
				return this.b - this.auf;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00006F29 File Offset: 0x00005129
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00006F36 File Offset: 0x00005136
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006F38 File Offset: 0x00005138
		public IKeyframe CreateKey(float time)
		{
			AnimationClipTrackKey animationClipTrackKey = new GameObject("Key").AddComponent<AnimationClipTrackKey>();
			AnimationClipTrackKey animationClipTrackKey2 = null;
			AnimationClipTrackKey animationClipTrackKey3 = null;
			this.FindLeftAndRight(time, out animationClipTrackKey2, out animationClipTrackKey3);
			animationClipTrackKey.transform.parent = base.transform;
			animationClipTrackKey.Track = this;
			if (animationClipTrackKey2)
			{
				animationClipTrackKey.clip = animationClipTrackKey2.clip;
			}
			else
			{
				animationClipTrackKey.clip = this.controlledclip;
			}
			animationClipTrackKey.keyTime = time;
			this.c.Add(animationClipTrackKey);
			this.UpdateTrack();
			return animationClipTrackKey;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006FB8 File Offset: 0x000051B8
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			AnimationClipTrackKey animationClipTrackKey = new GameObject("Key").AddComponent<AnimationClipTrackKey>();
			animationClipTrackKey.transform.parent = base.transform;
			animationClipTrackKey.Track = this;
			animationClipTrackKey.clip = ((AnimationClipTrackKey)keyframe).clip;
			animationClipTrackKey.wrapMode = ((AnimationClipTrackKey)keyframe).wrapMode;
			animationClipTrackKey.weight = ((AnimationClipTrackKey)keyframe).weight;
			animationClipTrackKey.startTime = ((AnimationClipTrackKey)keyframe).startTime;
			animationClipTrackKey.selectedTransform = ((AnimationClipTrackKey)keyframe).selectedTransform;
			animationClipTrackKey.forTransform = ((AnimationClipTrackKey)keyframe).forTransform;
			animationClipTrackKey.layer = ((AnimationClipTrackKey)keyframe).layer;
			animationClipTrackKey.blendMode = ((AnimationClipTrackKey)keyframe).blendMode;
			animationClipTrackKey.time = keyframe.time;
			this.c.Add(animationClipTrackKey);
			this.UpdateTrack();
			return animationClipTrackKey;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007094 File Offset: 0x00005294
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((AnimationClipTrackKey)keyframe);
			UnityEngine.Object.DestroyImmediate(((AnimationClipTrackKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000070BE File Offset: 0x000052BE
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((AnimationClipTrackKey)keyframe);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000070D4 File Offset: 0x000052D4
		public void FindLeftAndRight(float time, out AnimationClipTrackKey left, out AnimationClipTrackKey right)
		{
			left = null;
			right = null;
			if (time <= this.auf)
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

		// Token: 0x060001C4 RID: 452 RVA: 0x000071A0 File Offset: 0x000053A0
		private float a(float A_0, float A_1, float A_2, float A_3, float A_4, bool A_5)
		{
			float result;
			if (A_5)
			{
				float num = Mathf.Lerp(A_3, A_4, (A_0 - A_1) / (A_2 - A_1));
				result = (A_0 - A_1) * ((A_3 + num) / 2f);
			}
			else
			{
				result = (A_0 - A_1) * A_3;
			}
			return result;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000071DB File Offset: 0x000053DB
		public void DestroyLastAudio()
		{
			if (this.e != null)
			{
				UnityEngine.Object.DestroyImmediate(this.e.gameObject);
			}
			this.d = null;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007204 File Offset: 0x00005404
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			AnimationClipTrackKey animationClipTrackKey = null;
			AnimationClipTrackKey animationClipTrackKey2 = null;
			this.FindLeftAndRight(time, out animationClipTrackKey, out animationClipTrackKey2);
			if (animationClipTrackKey == null)
			{
				return;
			}
			if (animationClipTrackKey != this.d)
			{
				this.d = animationClipTrackKey;
				if (!Application.isPlaying)
				{
					this.DestroyLastAudio();
				}
				if (CSComponent.Localization != null)
				{
					this.f = CSComponent.Localization.GetCurrentLanguage().ToLower();
				}
				animationClipTrackKey.UpdateLocale(this.f);
				if (animationClipTrackKey.audioClip != null && animationClipTrackKey.playSound)
				{
					GameObject gameObject = new GameObject("CSOneShotAudio");
					CSDestroyObjectAterPlaySoundInternal csdestroyObjectAterPlaySoundInternal = gameObject.AddComponent<CSDestroyObjectAterPlaySoundInternal>();
					if (csdestroyObjectAterPlaySoundInternal)
					{
						csdestroyObjectAterPlaySoundInternal.trargetSoundPosition = this.controlledObject.transform;
						csdestroyObjectAterPlaySoundInternal.timeOffset = animationClipTrackKey.playSoundOffset;
						csdestroyObjectAterPlaySoundInternal.started = false;
						csdestroyObjectAterPlaySoundInternal.transform.parent = base.transform;
					}
					AudioSource audioSource = gameObject.AddComponent<AudioSource>();
					audioSource.clip = animationClipTrackKey.audioClip;
					audioSource.volume = animationClipTrackKey.volume;
					audioSource.pitch = animationClipTrackKey.pitch;
					audioSource.playOnAwake = false;
					this.e = audioSource;
				}
			}
			if (!animationClipTrackKey.play || !(animationClipTrackKey.GetClip() != null) || animationClipTrackKey.crossFade)
			{
				if (animationClipTrackKey.play && animationClipTrackKey.GetClip() != null)
				{
					AnimationClipTrackKey animationClipTrackKey3 = (AnimationClipTrackKey)animationClipTrackKey.GetPreviousKey();
					AnimationState animationState = this.controlledObject[animationClipTrackKey.GetClip().name];
					if (animationClipTrackKey3.time < animationClipTrackKey.time && animationClipTrackKey3.GetClip() != null)
					{
						if (animationClipTrackKey.forTransform && animationClipTrackKey.selectedTransform != null)
						{
							animationState.AddMixingTransform(animationClipTrackKey.selectedTransform, true);
						}
						List<AnimationClipTrackKey> list = new List<AnimationClipTrackKey>();
						AnimationClipTrackKey animationClipTrackKey4 = animationClipTrackKey;
						list.Add(animationClipTrackKey);
						for (;;)
						{
							AnimationClipTrackKey animationClipTrackKey5 = (AnimationClipTrackKey)this.GetPreviousKey(animationClipTrackKey);
							if (animationClipTrackKey5.time >= animationClipTrackKey4.time - 0.01f || !(animationClipTrackKey5 != animationClipTrackKey) || !animationClipTrackKey5.play || !(animationClipTrackKey5.GetClip() == animationClipTrackKey.GetClip()))
							{
								break;
							}
							list.Insert(0, animationClipTrackKey5);
							animationClipTrackKey4 = animationClipTrackKey5;
						}
						float num = list[0].startTime;
						for (int i = 1; i < list.Count; i++)
						{
							AnimationClipTrackKey animationClipTrackKey6 = list[i - 1];
							AnimationClipTrackKey animationClipTrackKey7 = list[i];
							num += this.a(animationClipTrackKey7.time, animationClipTrackKey6.time, animationClipTrackKey7.time, animationClipTrackKey6.speed, animationClipTrackKey7.speed, animationClipTrackKey6.lerpSpeed);
						}
						float time2;
						if (animationClipTrackKey2)
						{
							num += this.a(time, animationClipTrackKey.time, animationClipTrackKey2.time, animationClipTrackKey.speed, animationClipTrackKey2.speed, animationClipTrackKey.lerpSpeed);
							time2 = animationClipTrackKey.startTime + this.a(time, animationClipTrackKey.time, animationClipTrackKey2.time, animationClipTrackKey.speed, animationClipTrackKey2.speed, animationClipTrackKey.lerpSpeed);
						}
						else
						{
							num += (time - animationClipTrackKey.time) * animationClipTrackKey.speed;
							time2 = animationClipTrackKey.startTime + (time - animationClipTrackKey.time) * animationClipTrackKey.speed;
						}
						float num2 = animationClipTrackKey.time + animationClipTrackKey.crossFadeTime;
						float num3 = 1f - (num2 - time) / animationClipTrackKey.crossFadeTime;
						animationState.enabled = true;
						animationState.wrapMode = animationClipTrackKey.wrapMode;
						animationState.blendMode = animationClipTrackKey.blendMode;
						animationState.layer = animationClipTrackKey.layer;
						float num4;
						if (animationClipTrackKey2)
						{
							float t = (time - animationClipTrackKey.time) / (animationClipTrackKey2.time - animationClipTrackKey.time);
							num4 = Mathf.Lerp(animationClipTrackKey.weight, animationClipTrackKey2.weight, t);
						}
						else
						{
							num4 = animationClipTrackKey.weight;
						}
						if (time <= num2)
						{
							AnimationState animationState2 = this.controlledObject[animationClipTrackKey3.GetClip().name];
							animationState2.enabled = true;
							animationState2.wrapMode = animationClipTrackKey3.wrapMode;
							animationState2.blendMode = animationClipTrackKey3.blendMode;
							animationState2.layer = animationClipTrackKey3.layer;
							animationState.weight = num3 * num4;
							animationState.time = time2;
							animationState2.weight = Mathf.Lerp(animationClipTrackKey.weight, 0f, num3);
							animationState2.time = num;
							return;
						}
						animationState.weight = num4;
						animationState.time = (time - animationClipTrackKey.time + animationClipTrackKey.startTime) * animationClipTrackKey.speed;
						return;
					}
					else
					{
						animationState.enabled = true;
						animationState.wrapMode = animationClipTrackKey.wrapMode;
						animationState.blendMode = animationClipTrackKey.blendMode;
						animationState.layer = animationClipTrackKey.layer;
						float num5;
						if (animationClipTrackKey2)
						{
							float t2 = (time - animationClipTrackKey.time) / (animationClipTrackKey2.time - animationClipTrackKey.time);
							num5 = Mathf.Lerp(animationClipTrackKey.weight, animationClipTrackKey2.weight, t2);
						}
						else
						{
							num5 = animationClipTrackKey.weight;
						}
						float num6 = animationClipTrackKey.time + animationClipTrackKey.crossFadeTime;
						float num7 = (num6 - time) / animationClipTrackKey.crossFadeTime;
						if (time <= num6)
						{
							animationState.weight = num5 * num7;
						}
						else
						{
							animationState.weight = num5;
						}
						animationState.time = animationClipTrackKey.startTime + (time - animationClipTrackKey.time) * animationClipTrackKey.speed;
					}
				}
				return;
			}
			AnimationState animationState3 = this.controlledObject[animationClipTrackKey.GetClip().name];
			animationState3.enabled = true;
			animationState3.wrapMode = animationClipTrackKey.wrapMode;
			animationState3.blendMode = animationClipTrackKey.blendMode;
			animationState3.layer = animationClipTrackKey.layer;
			if (animationClipTrackKey.forTransform && animationClipTrackKey.selectedTransform != null)
			{
				animationState3.AddMixingTransform(animationClipTrackKey.selectedTransform, true);
			}
			List<AnimationClipTrackKey> list2 = new List<AnimationClipTrackKey>();
			AnimationClipTrackKey animationClipTrackKey8 = animationClipTrackKey;
			list2.Add(animationClipTrackKey);
			for (;;)
			{
				AnimationClipTrackKey animationClipTrackKey9 = (AnimationClipTrackKey)this.GetPreviousKey(animationClipTrackKey);
				if (animationClipTrackKey9.time >= animationClipTrackKey8.time - 0.01f || !(animationClipTrackKey9 != animationClipTrackKey) || !animationClipTrackKey9.play || !(animationClipTrackKey9.GetClip() == animationClipTrackKey.GetClip()))
				{
					break;
				}
				list2.Insert(0, animationClipTrackKey9);
				animationClipTrackKey8 = animationClipTrackKey9;
			}
			float num8 = list2[0].startTime;
			for (int j = 1; j < list2.Count; j++)
			{
				AnimationClipTrackKey animationClipTrackKey10 = list2[j - 1];
				AnimationClipTrackKey animationClipTrackKey11 = list2[j];
				num8 += this.a(animationClipTrackKey11.time, animationClipTrackKey10.time, animationClipTrackKey11.time, animationClipTrackKey10.speed, animationClipTrackKey11.speed, animationClipTrackKey10.lerpSpeed);
			}
			if (animationClipTrackKey2)
			{
				num8 += this.a(time, animationClipTrackKey.time, animationClipTrackKey2.time, animationClipTrackKey.speed, animationClipTrackKey2.speed, animationClipTrackKey.lerpSpeed);
			}
			else
			{
				num8 += (time - animationClipTrackKey.time) * animationClipTrackKey.speed;
			}
			animationState3.time = num8;
			if (animationClipTrackKey2)
			{
				float t3 = (time - animationClipTrackKey.time) / (animationClipTrackKey2.time - animationClipTrackKey.time);
				animationState3.weight = Mathf.Lerp(animationClipTrackKey.weight, animationClipTrackKey2.weight, t3);
				return;
			}
			animationState3.weight = animationClipTrackKey.weight;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000793C File Offset: 0x00005B3C
		public void InitTrack()
		{
			AnimationClipTrackKey[] componentsInChildren = base.GetComponentsInChildren<AnimationClipTrackKey>();
			this.c.Clear();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			foreach (AnimationClipTrackKey animationClipTrackKey in this.c)
			{
				if (animationClipTrackKey.clip == null)
				{
					animationClipTrackKey.clip = this.controlledclip;
				}
			}
			this.UpdateTrack();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000079CC File Offset: 0x00005BCC
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<AnimationClipTrackKey>(this.CompareKeys));
			if (this.c.Count == 0)
			{
				this.auf = 0f;
			}
			else
			{
				this.auf = this.c[0].time;
				this.b = this.c[this.c.Count - 1].time;
			}
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007A5A File Offset: 0x00005C5A
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00007A7D File Offset: 0x00005C7D
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00007A80 File Offset: 0x00005C80
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00007A83 File Offset: 0x00005C83
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				Debug.Log("AnimarionClipTrack can't show in scene. Use ITrack.IsCanShowInScene to check this.");
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007A90 File Offset: 0x00005C90
		public IKeyframe GetNextKey(AnimationClipTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00007AD8 File Offset: 0x00005CD8
		public IKeyframe GetPreviousKey(AnimationClipTrackKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x040000D7 RID: 215
		public Animation controlledObject;

		// Token: 0x040000D8 RID: 216
		public AnimationClip controlledclip;

		// Token: 0x040000D9 RID: 217
		public string trackUserName;

		// Token: 0x040000DA RID: 218
		public bool isDisabled;

		// Token: 0x040000DB RID: 219
		public WrapMode mode = WrapMode.Once;

		// Token: 0x040000DC RID: 220
		public Color trackColor = Color.white;

		// Token: 0x040000DD RID: 221
		private float auf;

		// Token: 0x040000DE RID: 222
		private float b;

		// Token: 0x040000DF RID: 223
		private readonly List<AnimationClipTrackKey> c = new List<AnimationClipTrackKey>();

		// Token: 0x040000E0 RID: 224
		private AnimationClipTrackKey d;

		// Token: 0x040000E1 RID: 225
		private AudioSource e;

		// Token: 0x040000E2 RID: 226
		private string f = "";
	}
}

