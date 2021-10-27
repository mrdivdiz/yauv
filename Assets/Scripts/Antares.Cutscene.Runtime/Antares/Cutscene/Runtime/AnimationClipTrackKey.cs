using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000033 RID: 51
	public class AnimationClipTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C1EC File Offset: 0x0000A3EC
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000C218 File Offset: 0x0000A418
		public AnimationClipTrack Track
		{
			get
			{
				if (this.a == null)
				{
					this.a = base.transform.parent.GetComponent<AnimationClipTrack>();
				}
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000C221 File Offset: 0x0000A421
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000C229 File Offset: 0x0000A429
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				if (Application.isEditor)
				{
					CSCore.a(this);
				}
				this.Track.UpdateTrack();
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000C24F File Offset: 0x0000A44F
		public void CaptureCurrentState()
		{
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C251 File Offset: 0x0000A451
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000C25F File Offset: 0x0000A45F
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C27A File Offset: 0x0000A47A
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C287 File Offset: 0x0000A487
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C295 File Offset: 0x0000A495
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C2A3 File Offset: 0x0000A4A3
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000C2B1 File Offset: 0x0000A4B1
		public KeyType keyType
		{
			get
			{
				return KeyType.ANIMATION;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public bool HaveLength
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000C2B7 File Offset: 0x0000A4B7
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000C2BE File Offset: 0x0000A4BE
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

		// Token: 0x06000293 RID: 659 RVA: 0x0000C2CA File Offset: 0x0000A4CA
		public AnimationClip GetClip()
		{
			if (this.b)
			{
				return this.b;
			}
			return this.clip;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		public void UpdateLocale(string currentLocale)
		{
			foreach (AnimationLocalizationItem animationLocalizationItem in this.localized)
			{
				if (animationLocalizationItem.LowerdLocale == currentLocale)
				{
					this.b = animationLocalizationItem.anim;
					break;
				}
			}
		}

		// Token: 0x0400014C RID: 332
		public float keyTime;

		// Token: 0x0400014D RID: 333
		public float startTime;

		// Token: 0x0400014E RID: 334
		public float weight = 1f;

		// Token: 0x0400014F RID: 335
		public float speed = 1f;

		// Token: 0x04000150 RID: 336
		public bool lerpSpeed;

		// Token: 0x04000151 RID: 337
		public int layer;

		// Token: 0x04000152 RID: 338
		public WrapMode wrapMode = WrapMode.Loop;

		// Token: 0x04000153 RID: 339
		public AnimationBlendMode blendMode;

		// Token: 0x04000154 RID: 340
		public bool play = true;

		// Token: 0x04000155 RID: 341
		public bool crossFade;

		// Token: 0x04000156 RID: 342
		public float crossFadeTime = 0.5f;

		// Token: 0x04000157 RID: 343
		public bool forTransform;

		// Token: 0x04000158 RID: 344
		public Transform selectedTransform;

		// Token: 0x04000159 RID: 345
		private AnimationClipTrack a;

		// Token: 0x0400015A RID: 346
		public AnimationClip clip;

		// Token: 0x0400015B RID: 347
		public bool playSound;

		// Token: 0x0400015C RID: 348
		public float playSoundOffset;

		// Token: 0x0400015D RID: 349
		public AudioClip audioClip;

		// Token: 0x0400015E RID: 350
		public float volume = 1f;

		// Token: 0x0400015F RID: 351
		public float pitch = 1f;

		// Token: 0x04000160 RID: 352
		public List<AnimationLocalizationItem> localized = new List<AnimationLocalizationItem>();

		// Token: 0x04000161 RID: 353
		private AnimationClip b;
	}
}
