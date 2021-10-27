using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class IOClod : MonoBehaviour
{
	// Token: 0x0600031B RID: 795 RVA: 0x00017A48 File Offset: 0x00015C48
	private void Awake()
	{
		this.shadowDistance = QualitySettings.shadowDistance * 2f;
		this.iocCam = Camera.main.GetComponent<IOCcam>();
		if (this.iocCam == null)
		{
			base.enabled = false;
		}
		else
		{
			this.prevDist = 0f;
			this.prevHitTime = Time.time;
			this.sleeping = true;
			this.h = default(RaycastHit);
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00017AC0 File Offset: 0x00015CC0
	private void Start()
	{
		this.UpdateValues();
		if (base.transform.Find("Lod_0"))
		{
			this.lods = 1;
			this.rs0 = base.transform.Find("Lod_0").GetComponentsInChildren<Renderer>(false);
			this.sh0 = new Shader[this.rs0.Length];
			for (int i = 0; i < this.rs0.Length; i++)
			{
				this.sh0[i] = this.rs0[i].material.shader;
			}
			if (base.transform.Find("Lod_1"))
			{
				this.lods++;
				this.rs1 = base.transform.Find("Lod_1").GetComponentsInChildren<Renderer>(false);
				if (base.transform.Find("Lod_2"))
				{
					this.lods++;
					this.rs2 = base.transform.Find("Lod_2").GetComponentsInChildren<Renderer>(false);
				}
			}
		}
		else
		{
			this.lods = 0;
		}
		this.rs = base.GetComponentsInChildren<Renderer>(false);
		this.sh = new Shader[this.rs.Length];
		for (int j = 0; j < this.rs.Length; j++)
		{
			this.sh[j] = this.rs[j].material.shader;
		}
		this.shInvisible = Shader.Find("Custom/Invisible");
		this.Initialize();
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00017C54 File Offset: 0x00015E54
	public void Initialize()
	{
		if (this.iocCam.enabled)
		{
			this.HideAll();
		}
		else
		{
			base.enabled = false;
			this.ShowLod(1f);
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00017C90 File Offset: 0x00015E90
	private void Update()
	{
		this.frameInterval = Time.frameCount % 10;
		if (this.frameInterval == 0)
		{
			bool lodOnly = this.LodOnly;
			if (lodOnly)
			{
				if (lodOnly)
				{
					if (!this.sleeping && Time.frameCount - this.counter > this.iocCam.hideDelay)
					{
						this.ShowLod(3000f);
						this.sleeping = true;
					}
				}
			}
			else if (!this.hidden && Time.frameCount - this.counter > this.iocCam.hideDelay)
			{
				switch (this.currentLod)
				{
				case 0:
					this.visible = this.rs0[0].isVisible;
					break;
				case 1:
					this.visible = this.rs1[0].isVisible;
					break;
				case 2:
					this.visible = this.rs2[0].isVisible;
					break;
				default:
					this.visible = this.rs[0].isVisible;
					break;
				}
				if (this.visible && this.hitDistance > 100f)
				{
					this.r = new Ray(this.hitPoint, this.iocCam.transform.position - this.hitPoint);
					if (Physics.Raycast(this.r, out this.h, this.iocCam.viewDistance))
					{
						if (this.h.transform.tag != this.iocCam.tag)
						{
							this.Hide();
						}
						else
						{
							this.counter = Time.frameCount;
						}
					}
				}
				else
				{
					this.Hide();
				}
			}
		}
		else if (this.realtimeShadows && this.frameInterval == 5)
		{
			this.distanceFromCam = Vector3.Distance(base.transform.position, this.iocCam.transform.position);
			if (this.hidden)
			{
				int num = this.lods;
				if (num != 0)
				{
					if (this.distanceFromCam > this.shadowDistance)
					{
						if (this.rs0[0].enabled)
						{
							for (int i = 0; i < this.rs0.Length; i++)
							{
								this.rs0[i].enabled = false;
								this.rs0[i].material.shader = this.sh0[i];
							}
						}
					}
					else if (!this.rs0[0].enabled)
					{
						for (int j = 0; j < this.rs0.Length; j++)
						{
							this.rs0[j].material.shader = this.shInvisible;
							this.rs0[j].enabled = true;
						}
					}
				}
				else if (this.distanceFromCam > this.shadowDistance)
				{
					if (this.rs[0].enabled)
					{
						for (int k = 0; k < this.rs.Length; k++)
						{
							this.rs[k].enabled = false;
							this.rs[k].material.shader = this.sh[k];
						}
					}
				}
				else if (!this.rs[0].enabled)
				{
					for (int l = 0; l < this.rs.Length; l++)
					{
						this.rs[l].material.shader = this.shInvisible;
						this.rs[l].enabled = true;
					}
				}
			}
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0001804C File Offset: 0x0001624C
	public void UpdateValues()
	{
		if (this.Lod1 != 0f)
		{
			this.lod_1 = this.Lod1;
		}
		else
		{
			this.lod_1 = this.iocCam.lod1Distance;
		}
		if (this.Lod2 != 0f)
		{
			this.lod_2 = this.Lod2;
		}
		else
		{
			this.lod_2 = this.iocCam.lod2Distance;
		}
		if (this.LodMargin != 0f)
		{
			this.lodMargin = this.LodMargin;
		}
		else
		{
			this.lodMargin = this.iocCam.lodMargin;
		}
		this.realtimeShadows = this.iocCam.realtimeShadows;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00018100 File Offset: 0x00016300
	public void UnHide(RaycastHit h)
	{
		this.counter = Time.frameCount;
		this.hitPoint = h.point;
		this.hitDistance = h.distance;
		if (this.hidden)
		{
			this.hidden = false;
			this.ShowLod(h.distance);
		}
		else if (this.lods > 0)
		{
			this.distOffset = this.prevDist - h.distance;
			this.hitTimeOffset = Time.time - this.prevHitTime;
			this.prevHitTime = Time.time;
			if (Mathf.Abs(this.distOffset) > this.lodMargin | this.hitTimeOffset > 1f)
			{
				this.ShowLod(h.distance);
				this.prevDist = h.distance;
				this.sleeping = false;
			}
		}
	}

	// Token: 0x06000321 RID: 801 RVA: 0x000181DC File Offset: 0x000163DC
	public void ShowLod(float d)
	{
		switch (this.lods)
		{
		case 0:
			this.currentLod = -1;
			break;
		case 2:
			if (d < this.lod_1)
			{
				this.currentLod = 0;
			}
			else
			{
				this.currentLod = 1;
			}
			break;
		case 3:
			if (d < this.lod_1)
			{
				this.currentLod = 0;
			}
			else if (d > this.lod_1 & d < this.lod_2)
			{
				this.currentLod = 1;
			}
			else
			{
				this.currentLod = 2;
			}
			break;
		}
		switch (this.currentLod)
		{
		case 0:
			if (!this.LodOnly && this.rs0[0].enabled)
			{
				for (int i = 0; i < this.rs0.Length; i++)
				{
					this.rs0[i].material.shader = this.sh0[i];
				}
			}
			else
			{
				for (int i = 0; i < this.rs0.Length; i++)
				{
					this.rs0[i].enabled = true;
				}
			}
			for (int i = 0; i < this.rs1.Length; i++)
			{
				this.rs1[i].enabled = false;
			}
			if (this.lods == 3)
			{
				for (int i = 0; i < this.rs2.Length; i++)
				{
					this.rs2[i].enabled = false;
				}
			}
			break;
		case 1:
			for (int i = 0; i < this.rs1.Length; i++)
			{
				this.rs1[i].enabled = true;
			}
			for (int i = 0; i < this.rs0.Length; i++)
			{
				this.rs0[i].enabled = false;
				if (!this.LodOnly && this.realtimeShadows)
				{
					this.rs0[i].material.shader = this.sh0[i];
				}
			}
			if (this.lods == 3)
			{
				for (int i = 0; i < this.rs2.Length; i++)
				{
					this.rs2[i].enabled = false;
				}
			}
			break;
		case 2:
			for (int i = 0; i < this.rs2.Length; i++)
			{
				this.rs2[i].enabled = true;
			}
			for (int i = 0; i < this.rs0.Length; i++)
			{
				this.rs0[i].enabled = false;
				if (!this.LodOnly && this.realtimeShadows)
				{
					this.rs0[i].material.shader = this.sh0[i];
				}
			}
			for (int i = 0; i < this.rs1.Length; i++)
			{
				this.rs1[i].enabled = false;
			}
			break;
		default:
			if (!this.LodOnly && this.rs[0].enabled)
			{
				for (int i = 0; i < this.rs.Length; i++)
				{
					this.rs[i].material.shader = this.sh[i];
				}
			}
			else
			{
				for (int i = 0; i < this.rs.Length; i++)
				{
					this.rs[i].enabled = true;
				}
			}
			break;
		}
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00018560 File Offset: 0x00016760
	public void Hide()
	{
		this.hidden = true;
		switch (this.currentLod)
		{
		case 0:
			if (this.realtimeShadows && this.distanceFromCam <= this.shadowDistance)
			{
				for (int i = 0; i < this.rs0.Length; i++)
				{
					this.rs0[i].material.shader = this.shInvisible;
				}
			}
			else
			{
				for (int i = 0; i < this.rs0.Length; i++)
				{
					this.rs0[i].enabled = false;
				}
			}
			break;
		case 1:
			for (int i = 0; i < this.rs1.Length; i++)
			{
				this.rs1[i].enabled = false;
			}
			break;
		case 2:
			for (int i = 0; i < this.rs2.Length; i++)
			{
				this.rs2[i].enabled = false;
			}
			break;
		default:
			if (this.realtimeShadows && this.distanceFromCam <= this.shadowDistance)
			{
				for (int i = 0; i < this.rs.Length; i++)
				{
					this.rs[i].material.shader = this.shInvisible;
				}
			}
			else
			{
				for (int i = 0; i < this.rs.Length; i++)
				{
					this.rs[i].enabled = false;
				}
			}
			break;
		}
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000186E8 File Offset: 0x000168E8
	public void HideAll()
	{
		bool lodOnly = this.LodOnly;
		if (lodOnly)
		{
			if (lodOnly)
			{
				this.prevHitTime -= 3f;
				this.ShowLod(3000f);
			}
		}
		else
		{
			this.hidden = true;
			if (this.lods == 0 && this.rs != null)
			{
				for (int i = 0; i < this.rs.Length; i++)
				{
					this.rs[i].enabled = false;
					if (this.realtimeShadows)
					{
						this.rs[i].material.shader = this.sh[i];
					}
				}
			}
			else
			{
				for (int i = 0; i < this.rs0.Length; i++)
				{
					this.rs0[i].enabled = false;
					if (this.realtimeShadows)
					{
						this.rs0[i].material.shader = this.sh0[i];
					}
				}
				for (int i = 0; i < this.rs1.Length; i++)
				{
					this.rs1[i].enabled = false;
				}
				for (int i = 0; i < this.rs2.Length; i++)
				{
					this.rs2[i].enabled = false;
				}
			}
		}
	}

	// Token: 0x040003A5 RID: 933
	public float Lod1;

	// Token: 0x040003A6 RID: 934
	public float Lod2;

	// Token: 0x040003A7 RID: 935
	public float LodMargin;

	// Token: 0x040003A8 RID: 936
	public bool LodOnly;

	// Token: 0x040003A9 RID: 937
	private Vector3 hitPoint;

	// Token: 0x040003AA RID: 938
	private float hitDistance;

	// Token: 0x040003AB RID: 939
	private float lod_1;

	// Token: 0x040003AC RID: 940
	private float lod_2;

	// Token: 0x040003AD RID: 941
	private float lodMargin;

	// Token: 0x040003AE RID: 942
	private bool realtimeShadows;

	// Token: 0x040003AF RID: 943
	private IOCcam iocCam;

	// Token: 0x040003B0 RID: 944
	private int counter;

	// Token: 0x040003B1 RID: 945
	private Renderer[] rs0;

	// Token: 0x040003B2 RID: 946
	private Renderer[] rs1;

	// Token: 0x040003B3 RID: 947
	private Renderer[] rs2;

	// Token: 0x040003B4 RID: 948
	private Renderer[] rs;

	// Token: 0x040003B5 RID: 949
	private bool hidden;

	// Token: 0x040003B6 RID: 950
	private int currentLod;

	// Token: 0x040003B7 RID: 951
	private float prevDist;

	// Token: 0x040003B8 RID: 952
	private float distOffset;

	// Token: 0x040003B9 RID: 953
	private int lods;

	// Token: 0x040003BA RID: 954
	private float dt;

	// Token: 0x040003BB RID: 955
	private float lodDistanceFromCam;

	// Token: 0x040003BC RID: 956
	private float hitTimeOffset;

	// Token: 0x040003BD RID: 957
	private float prevHitTime;

	// Token: 0x040003BE RID: 958
	private bool sleeping;

	// Token: 0x040003BF RID: 959
	private Shader shInvisible;

	// Token: 0x040003C0 RID: 960
	private Shader[] sh;

	// Token: 0x040003C1 RID: 961
	private Shader[] sh0;

	// Token: 0x040003C2 RID: 962
	private float distanceFromCam;

	// Token: 0x040003C3 RID: 963
	private float shadowDistance;

	// Token: 0x040003C4 RID: 964
	private int frameInterval;

	// Token: 0x040003C5 RID: 965
	private RaycastHit h;

	// Token: 0x040003C6 RID: 966
	private Ray r;

	// Token: 0x040003C7 RID: 967
	private bool visible;
}
