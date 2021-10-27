using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class HintTrigger : MonoBehaviour
{
	// Token: 0x06000791 RID: 1937 RVA: 0x0003D98C File Offset: 0x0003BB8C
	private void Start()
	{
		if (this.autoPlayTime != 0f)
		{
			base.Invoke("ShowHint", this.autoPlayTime);
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0003D9B0 File Offset: 0x0003BBB0
	private void Update()
	{
		if (!this.played && this.triggered && !SpeechManager.instance.playing)
		{
			if (this.delay == 0f)
			{
				this.ShowHint();
			}
			else
			{
				base.Invoke("ShowHint", this.delay);
			}
			this.triggered = false;
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0003DA18 File Offset: 0x0003BC18
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((collisionInfo.tag == "Player" || collisionInfo.tag == "Bike" || collisionInfo.tag == "PlayerCar") && !this.played && (!this.showOnlyWhenJoystickConnected || AndroidPlatform.IsJoystickConnected()))
		{
			if (SpeechManager.instance == null || !SpeechManager.instance.playing)
			{
				if (this.delay == 0f)
				{
					this.ShowHint();
				}
				else
				{
					base.Invoke("ShowHint", this.delay);
				}
			}
			else
			{
				this.triggered = true;
			}
		}
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0003DADC File Offset: 0x0003BCDC
	private void ShowHint()
	{
		if (!this.played)
		{
			HintsViewer.ShowHint(Language.Get(this.hintKeyword, 120), this.size, this.scale);
			if (this.isPausingHint)
			{
				if (this.lookAtObject != null && Camera.main != null)
				{
					ShooterGameCamera component = Camera.main.GetComponent<ShooterGameCamera>();
					if (component != null)
					{
						component.lockTarget = this.lookAtObject;
						component.focusOnTarget = true;
						component.hintFocus = true;
					}
				}
				mainmenu.hintPause = true;
				mainmenu.pause = true;
				Time.timeScale = 1E-05f;
			}
			this.played = true;
			if (this.ObjectToShow != null)
			{
				this.ObjectToShow.SetActive(true);
			}
			if (this.ObjectToHide != null)
			{
				this.ObjectToHide.SetActive(false);
			}
		}
	}

	// Token: 0x040009DD RID: 2525
	public string hintKeyword = string.Empty;

	// Token: 0x040009DE RID: 2526
	public string hintKeywordPSMove = string.Empty;

	// Token: 0x040009DF RID: 2527
	public HintSize size;

	// Token: 0x040009E0 RID: 2528
	public float scale = 1f;

	// Token: 0x040009E1 RID: 2529
	private bool played;

	// Token: 0x040009E2 RID: 2530
	public float autoPlayTime;

	// Token: 0x040009E3 RID: 2531
	public float delay;

	// Token: 0x040009E4 RID: 2532
	private bool triggered;

	// Token: 0x040009E5 RID: 2533
	public bool isPausingHint;

	// Token: 0x040009E6 RID: 2534
	public GameObject ObjectToShow;

	// Token: 0x040009E7 RID: 2535
	public GameObject ObjectToHide;

	// Token: 0x040009E8 RID: 2536
	public bool showOnlyWhenJoystickConnected;

	// Token: 0x040009E9 RID: 2537
	public Transform lookAtObject;
}
