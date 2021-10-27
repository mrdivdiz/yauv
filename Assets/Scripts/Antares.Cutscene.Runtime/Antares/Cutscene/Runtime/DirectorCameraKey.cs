using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000010 RID: 16
	public class DirectorCameraKey : MonoBehaviour, IKeyframe
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003632 File Offset: 0x00001832
		// (set) Token: 0x060000AB RID: 171 RVA: 0x0000365E File Offset: 0x0000185E
		public DirectorCameraTrack dcTrack
		{
			get
			{
				if (this.dcta == null)
				{
					this.dcta = base.transform.parent.GetComponent<DirectorCameraTrack>();
				}
				return this.dcta;
			}
			set
			{
				this.dcta = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003667 File Offset: 0x00001867
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000366F File Offset: 0x0000186F
		public float time
		{
			get
			{
				return this.keyTime;
			}
			set
			{
				this.keyTime = CSCore.a(value);
				this.dcTrack.UpdateTrack();
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003688 File Offset: 0x00001888
		public void CaptureCurrentState()
		{
			//this.dcTrack != null;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003697 File Offset: 0x00001897
		public void DeleteThis()
		{
			this.dcTrack.DeleteKeyframe(this);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000036A5 File Offset: 0x000018A5
		public void SelectThis()
		{
			this.dcTrack.SelectKeyframe(this);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000036B3 File Offset: 0x000018B3
		public void UpdateTrack()
		{
			this.dcTrack.UpdateTrack();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000036C0 File Offset: 0x000018C0
		public IKeyframe GetNextKey()
		{
			return this.dcTrack.GetNextKey(this);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000036CE File Offset: 0x000018CE
		public IKeyframe GetPreviousKey()
		{
			return this.dcTrack.GetPreviousKey(this);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000036DC File Offset: 0x000018DC
		public int GetIndex()
		{
			return this.dcTrack.GetKeyframeIndex(this);
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000036EA File Offset: 0x000018EA
		public bool HaveLength
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000036ED File Offset: 0x000018ED
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000036F5 File Offset: 0x000018F5
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00003701 File Offset: 0x00001901
		public KeyType keyType
		{
			get
			{
				return KeyType.CAMERA;
			}
		}

		// Token: 0x0400003D RID: 61
		public float keyTime;

		// Token: 0x0400003E RID: 62
		public Camera keyCamera;

		// Token: 0x0400003F RID: 63
		public Effect effect;

		// Token: 0x04000040 RID: 64
		public Color fadeColor = Color.black;

		// Token: 0x04000041 RID: 65
		private DirectorCameraTrack dcta;

		// Token: 0x04000042 RID: 66
		public float duration;

		// Token: 0x04000043 RID: 67
		public float dwScale = 0.07f;

		// Token: 0x04000044 RID: 68
		public float dwFrequency = 50f;

		// Token: 0x04000045 RID: 69
		public float dwAnimSpeed = 1f;

		// Token: 0x04000046 RID: 70
		public Mesh shapeMesh;

		// Token: 0x04000047 RID: 71
		public float rotateAmount = 360f;
	}
}
