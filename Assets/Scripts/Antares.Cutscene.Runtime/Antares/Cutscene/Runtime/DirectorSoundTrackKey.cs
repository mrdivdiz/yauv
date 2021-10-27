using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200000A RID: 10
	public class DirectorSoundTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002A40 File Offset: 0x00000C40
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002A6C File Offset: 0x00000C6C
		public DirectorSoundTrack dsTrack
		{
			get
			{
				if (this.ads == null)
				{
					this.ads = base.transform.parent.GetComponent<DirectorSoundTrack>();
				}
				return this.ads;
			}
			set
			{
				this.ads = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002A75 File Offset: 0x00000C75
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002A7D File Offset: 0x00000C7D
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.dsTrack.UpdateTrack();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002A96 File Offset: 0x00000C96
		public void CaptureCurrentState()
		{
			//this.dsTrack != null;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002AA5 File Offset: 0x00000CA5
		public void DeleteThis()
		{
			this.dsTrack.DeleteKeyframe(this);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002AB3 File Offset: 0x00000CB3
		public void SelectThis()
		{
			this.dsTrack.SelectKeyframe(this);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002AC1 File Offset: 0x00000CC1
		public void UpdateTrack()
		{
			this.dsTrack.UpdateTrack();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002ACE File Offset: 0x00000CCE
		public IKeyframe GetNextKey()
		{
			return this.dsTrack.GetNextKey(this);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002ADC File Offset: 0x00000CDC
		public IKeyframe GetPreviousKey()
		{
			return this.dsTrack.GetPreviousKey(this);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002AEA File Offset: 0x00000CEA
		public int GetIndex()
		{
			return this.dsTrack.GetKeyframeIndex(this);
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public bool HaveLength
		{
			get
			{
				return !(this.audioClip == null);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002B0B File Offset: 0x00000D0B
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002B2B File Offset: 0x00000D2B
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002B37 File Offset: 0x00000D37
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000023 RID: 35
		public float keyTime;

		// Token: 0x04000024 RID: 36
		public AudioClip audioClip;

		// Token: 0x04000025 RID: 37
		public float volume = 1f;

		// Token: 0x04000026 RID: 38
		public float pitch = 1f;

		// Token: 0x04000027 RID: 39
		public CutSceneAudioType type;

		// Token: 0x04000028 RID: 40
		public List<SpeechLocalizationItem> localized = new List<SpeechLocalizationItem>();

		// Token: 0x04000029 RID: 41
		public bool volumeLerp;

		// Token: 0x0400002A RID: 42
		public bool pitchLerp;

		// Token: 0x0400002B RID: 43
		private DirectorSoundTrack ads;
	}
}
