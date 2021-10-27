using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000018 RID: 24
	public class TransformSoundTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000041ED File Offset: 0x000023ED
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00004219 File Offset: 0x00002419
		public TransformSoundTrack Track
		{
			get
			{
				if (this.a == null)
				{
					this.a = base.transform.parent.GetComponent<TransformSoundTrack>();
				}
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004222 File Offset: 0x00002422
		// (set) Token: 0x060000DD RID: 221 RVA: 0x0000422A File Offset: 0x0000242A
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

		// Token: 0x060000DE RID: 222 RVA: 0x00004243 File Offset: 0x00002443
		public void CaptureCurrentState()
		{
			//this.Track != null;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004252 File Offset: 0x00002452
		public void DeleteThis()
		{
			this.Track.DeleteKeyframe(this);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004260 File Offset: 0x00002460
		public void SelectThis()
		{
			this.Track.SelectKeyframe(this);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000426E File Offset: 0x0000246E
		public void UpdateTrack()
		{
			this.Track.UpdateTrack();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000427B File Offset: 0x0000247B
		public IKeyframe GetNextKey()
		{
			return this.Track.GetNextKey(this);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004289 File Offset: 0x00002489
		public IKeyframe GetPreviousKey()
		{
			return this.Track.GetPreviousKey(this);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004297 File Offset: 0x00002497
		public int GetIndex()
		{
			return this.Track.GetKeyframeIndex(this);
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000042A5 File Offset: 0x000024A5
		public bool HaveLength
		{
			get
			{
				return !(this.audioClip == null);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000042B8 File Offset: 0x000024B8
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000042D8 File Offset: 0x000024D8
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000042E4 File Offset: 0x000024E4
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x0400005D RID: 93
		public float keyTime;

		// Token: 0x0400005E RID: 94
		public AudioClip audioClip;

		// Token: 0x0400005F RID: 95
		public float volume = 1f;

		// Token: 0x04000060 RID: 96
		public float pitch = 1f;

		// Token: 0x04000061 RID: 97
		public CutSceneAudioType type;

		// Token: 0x04000062 RID: 98
		public List<SpeechLocalizationItem> localized = new List<SpeechLocalizationItem>();

		// Token: 0x04000063 RID: 99
		private TransformSoundTrack a;
	}
}
