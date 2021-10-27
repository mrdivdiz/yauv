using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000037 RID: 55
	public class SubtitlesTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000D200 File Offset: 0x0000B400
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000D22C File Offset: 0x0000B42C
		public SubtitlesTrack sbTrack
		{
			get
			{
				if (this.sbta == null)
				{
					this.sbta = base.transform.parent.GetComponent<SubtitlesTrack>();
				}
				return this.sbta;
			}
			set
			{
				this.sbta = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000D235 File Offset: 0x0000B435
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000D23D File Offset: 0x0000B43D
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.sbTrack.UpdateTrack();
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D256 File Offset: 0x0000B456
		public void CaptureCurrentState()
		{
			//this.sbTrack != null;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D265 File Offset: 0x0000B465
		public void DeleteThis()
		{
			this.sbTrack.DeleteKeyframe(this);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D273 File Offset: 0x0000B473
		public void SelectThis()
		{
			this.sbTrack.SelectKeyframe(this);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D281 File Offset: 0x0000B481
		public void UpdateTrack()
		{
			this.sbTrack.UpdateTrack();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D28E File Offset: 0x0000B48E
		public IKeyframe GetNextKey()
		{
			return this.sbTrack.GetNextKey(this);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D29C File Offset: 0x0000B49C
		public IKeyframe GetPreviousKey()
		{
			return this.sbTrack.GetPreviousKey(this);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000D2AA File Offset: 0x0000B4AA
		public int GetIndex()
		{
			return this.sbTrack.GetKeyframeIndex(this);
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
		public bool HaveLength
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000D2BB File Offset: 0x0000B4BB
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000D2C3 File Offset: 0x0000B4C3
		public float Length
		{
			get
			{
				return this.duration;
			}
			set
			{
				Debug.Log("Length");
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000D2CF File Offset: 0x0000B4CF
		public KeyType keyType
		{
			get
			{
				return KeyType.SUBTITLES;
			}
		}

		// Token: 0x04000171 RID: 369
		public float keyTime;

		// Token: 0x04000172 RID: 370
		public string text = "";

		// Token: 0x04000173 RID: 371
		public string keyword = "";

		// Token: 0x04000174 RID: 372
		public float duration = 2f;

		// Token: 0x04000175 RID: 373
		public float fadeInTime = 0.3f;

		// Token: 0x04000176 RID: 374
		public float fadeOutTime = 0.3f;

		// Token: 0x04000177 RID: 375
		public Color color = Color.white;

		// Token: 0x04000178 RID: 376
		public Font font;

		// Token: 0x04000179 RID: 377
		public int fontSize = 23;

		// Token: 0x0400017A RID: 378
		public FontStyle fontStyle;

		// Token: 0x0400017B RID: 379
		public bool customStyle;

		// Token: 0x0400017C RID: 380
		public bool customPlacement;

		// Token: 0x0400017D RID: 381
		public TextAnchor textAnchor = TextAnchor.LowerCenter;

		// Token: 0x0400017E RID: 382
		public Vector2 customMarginX = new Vector2(0.01f, 0.01f);

		// Token: 0x0400017F RID: 383
		public Vector2 customMarginY = new Vector2(0.1f, 0.1f);

		// Token: 0x04000180 RID: 384
		private SubtitlesTrack sbta;
	}
}
