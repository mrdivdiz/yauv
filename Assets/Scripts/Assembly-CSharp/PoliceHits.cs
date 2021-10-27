using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class PoliceHits : MonoBehaviour
{
	// Token: 0x06000665 RID: 1637 RVA: 0x00030FC0 File Offset: 0x0002F1C0
	private void Start()
	{
		this.axisCarController = base.GetComponent<AxisCarController>();
		this.mouseCarcontroller = base.GetComponent<MouseCarController>();
		this.mobileCarController = base.GetComponent<MobileCarController>();
		this.dashboard = base.GetComponentInChildren<DashBoard>();
		switch (DifficultyManager.difficulty)
		{
		case DifficultyManager.Difficulty.EASY:
			this.maxHits = DifficultyManager.easyCarHits;
			break;
		case DifficultyManager.Difficulty.MEDIUM:
			this.maxHits = DifficultyManager.mediumCarHits;
			break;
		case DifficultyManager.Difficulty.HARD:
			this.maxHits = DifficultyManager.hardCarHits;
			break;
		}
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00031050 File Offset: 0x0002F250
	private void Update()
	{
		if (this.startingY == 1000f)
		{
			this.startingY = base.transform.position.y;
		}
		if (base.transform.position.y < this.startingY)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.startingY, base.transform.position.z);
		}
		if (base.transform.up.y <= 0.2f)
		{
			if (this.flipTimer <= 0f && this.startTime == 0f)
			{
				this.startTime = Time.time;
			}
			else
			{
				this.flipTimer -= Time.deltaTime;
			}
		}
		else
		{
			this.flipTimer = 2f;
		}
		if (!this.faded && this.startTime != 0f)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x000311C0 File Offset: 0x0002F3C0
	private void OnCollisionEnter(Collision collision)
	{
		PadVibrator.VibrateInterval(true, 0.6f, 0.35f);
		if (collision.collider.tag == "PoliceCar" && (this.lastHitTime == 0f || Time.time > this.lastHitTime + 5f))
		{
			this.hits++;
			this.dashboard.decreaseHealth();
			this.lastHitTime = Time.time;
			if (this.hits >= this.maxHits - 2 && this.startTime == 0f && this.smokeObject != null)
			{
				this.smokeObject.SetActive(true);
				this.smokeObject = null;
			}
			if (this.hits >= this.maxHits && this.startTime == 0f)
			{
				this.axisCarController.enabled = false;
				this.mouseCarcontroller.enabled = false;
				this.mobileCarController.enabled = false;
				this.startTime = Time.time;
			}
		}
	}

	// Token: 0x040007A0 RID: 1952
	public int hits;

	// Token: 0x040007A1 RID: 1953
	private int maxHits = 5;

	// Token: 0x040007A2 RID: 1954
	private AxisCarController axisCarController;

	// Token: 0x040007A3 RID: 1955
	private MouseCarController mouseCarcontroller;

	// Token: 0x040007A4 RID: 1956
	private MobileCarController mobileCarController;

	// Token: 0x040007A5 RID: 1957
	private float startTime;

	// Token: 0x040007A6 RID: 1958
	private bool faded;

	// Token: 0x040007A7 RID: 1959
	private float flipTimer = 2f;

	// Token: 0x040007A8 RID: 1960
	private float lastHitTime;

	// Token: 0x040007A9 RID: 1961
	private DashBoard dashboard;

	// Token: 0x040007AA RID: 1962
	private float startingY = 1000f;

	// Token: 0x040007AB RID: 1963
	public GameObject smokeObject;
}
