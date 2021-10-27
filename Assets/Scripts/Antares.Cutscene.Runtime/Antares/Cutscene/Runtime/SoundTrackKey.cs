using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200003C RID: 60
	public class SoundTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000E784 File Offset: 0x0000C984
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000E7B0 File Offset: 0x0000C9B0
		public SoundTrack Track
		{
			get
			{
				if (this.a == null)
				{
					this.a = base.transform.parent.GetComponent<SoundTrack>();
				}
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000E7B9 File Offset: 0x0000C9B9
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.Track.UpdateTrack();
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000E7DA File Offset: 0x0000C9DA
		public void CaptureCurrentState()
		{
			//this.Track != null;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E7E9 File Offset: 0x0000C9E9
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E7F7 File Offset: 0x0000C9F7
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E805 File Offset: 0x0000CA05
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E812 File Offset: 0x0000CA12
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000E820 File Offset: 0x0000CA20
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E82E File Offset: 0x0000CA2E
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000E83C File Offset: 0x0000CA3C
		public bool HaveLength
		{
			get
			{
				return !(this.audioClip == null);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000E84F File Offset: 0x0000CA4F
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000E86F File Offset: 0x0000CA6F
		public float Length
		{
			get
			{
				if (this.audioClip)
				{
					return this.audioClip.length;
				}
				return 0f;
			}
			set
			{
				Debug.Log("Length");
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000E87B File Offset: 0x0000CA7B
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x0400019C RID: 412
		public float keyTime;

		// Token: 0x0400019D RID: 413
		public AudioClip audioClip;

		// Token: 0x0400019E RID: 414
		public float volume = 1f;

		// Token: 0x0400019F RID: 415
		public float pitch = 1f;

		// Token: 0x040001A0 RID: 416
		public CutSceneAudioType type;

		// Token: 0x040001A1 RID: 417
		public List<SpeechLocalizationItem> localized = new List<SpeechLocalizationItem>();

		// Token: 0x040001A2 RID: 418
		public bool volumeLerp;

		// Token: 0x040001A3 RID: 419
		public bool pitchLerp;

		// Token: 0x040001A4 RID: 420
		private SoundTrack a;
	}
}
