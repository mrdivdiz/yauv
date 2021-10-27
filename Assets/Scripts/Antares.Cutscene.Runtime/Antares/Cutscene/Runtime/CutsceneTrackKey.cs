using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000034 RID: 52
	public class CutsceneTrackKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000C3B3 File Offset: 0x0000A5B3
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000C3DF File Offset: 0x0000A5DF
		public CutsceneTrack ctTrack
		{
			get
			{
				if (this.cta == null)
				{
					this.cta = base.transform.parent.GetComponent<CutsceneTrack>();
				}
				return this.cta;
			}
			set
			{
				this.cta = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000C3E8 File Offset: 0x0000A5E8
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.ctTrack.UpdateTrack();
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000C409 File Offset: 0x0000A609
		public void CaptureCurrentState()
		{
			//this.ctTrack != null;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000C418 File Offset: 0x0000A618
		public void DeleteThis()
		{
			this.ctTrack.DeleteKeyframe(this);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000C426 File Offset: 0x0000A626
		public void SelectThis()
		{
			this.ctTrack.SelectKeyframe(this);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000C434 File Offset: 0x0000A634
		public void UpdateTrack()
		{
			this.ctTrack.UpdateTrack();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000C441 File Offset: 0x0000A641
		public IKeyframe GetNextKey()
		{
			return this.ctTrack.GetNextKey(this);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000C44F File Offset: 0x0000A64F
		public IKeyframe GetPreviousKey()
		{
			return this.ctTrack.GetPreviousKey(this);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000C45D File Offset: 0x0000A65D
		public int GetIndex()
		{
			return this.ctTrack.GetKeyframeIndex(this);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000C46B File Offset: 0x0000A66B
		public bool HaveLength
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000C46E File Offset: 0x0000A66E
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000C475 File Offset: 0x0000A675
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000C481 File Offset: 0x0000A681
		public KeyType keyType
		{
			get
			{
				return KeyType.STANDART;
			}
		}

		// Token: 0x04000162 RID: 354
		public float keyTime;

		// Token: 0x04000163 RID: 355
		public CSComponent cutscene;

		// Token: 0x04000164 RID: 356
		public float playSpeed = 1f;

		// Token: 0x04000165 RID: 357
		private CutsceneTrack cta;
	}
}
