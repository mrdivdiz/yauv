using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class IOChud : MonoBehaviour
{
	// Token: 0x0600032B RID: 811 RVA: 0x00018A28 File Offset: 0x00016C28
	private void Awake()
	{
		this.Icon = (Texture2D)Resources.Load("Icon");
		this.hud = false;
		this.dirty = false;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00018A50 File Offset: 0x00016C50
	private void Start()
	{
		this.ioc = Camera.main.transform.GetComponent<IOCcam>();
		this.iocActive = this.ioc.enabled;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00018A84 File Offset: 0x00016C84
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.I))
		{
			this.iocActive = !this.iocActive;
			this.ToggleIOC();
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			this.ToggleHUD();
		}
		if (Input.GetMouseButtonUp(0) && this.dirty)
		{
			this.ToggleIOC();
			this.dirty = false;
		}
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00018AE8 File Offset: 0x00016CE8
	private void OnGUI()
	{
		GUI.Label(new Rect(25f, 10f, 360f, 20f), "Press 'i' to toggle InstantOC - Press 'Esc' to toggle HUD");
		if (this.hud)
		{
			GUI.Label(new Rect(25f, 35f, 320f, 20f), "Samples");
			this.ioc.samples = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(25f, 55f, 150f, 20f), (float)this.ioc.samples, 10f, 1500f));
			GUI.Label(new Rect(180f, 50f, 50f, 20f), this.ioc.samples.ToString());
			GUI.Label(new Rect(25f, 65f, 320f, 20f), "Hide delay");
			this.ioc.hideDelay = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(25f, 85f, 150f, 20f), (float)this.ioc.hideDelay, 10f, 300f));
			GUI.Label(new Rect(180f, 80f, 50f, 20f), this.ioc.hideDelay.ToString());
			GUI.Label(new Rect(25f, 95f, 320f, 20f), "View Distance");
			this.ioc.viewDistance = (float)Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(25f, 115f, 150f, 20f), this.ioc.viewDistance, 100f, 3000f));
			GUI.Label(new Rect(180f, 110f, 50f, 20f), this.ioc.viewDistance.ToString());
			GUI.Label(new Rect(25f, 125f, 320f, 20f), "Lod 1");
			this.ioc.lod1Distance = Mathf.Round(GUI.HorizontalSlider(new Rect(25f, 145f, 150f, 20f), this.ioc.lod1Distance, 10f, 300f));
			GUI.Label(new Rect(180f, 140f, 50f, 20f), this.ioc.lod1Distance.ToString());
			GUI.Label(new Rect(25f, 155f, 320f, 20f), "Lod 2");
			this.ioc.lod2Distance = Mathf.Round(GUI.HorizontalSlider(new Rect(25f, 175f, 150f, 20f), this.ioc.lod2Distance, 10f, 600f));
			GUI.Label(new Rect(180f, 170f, 50f, 20f), this.ioc.lod2Distance.ToString());
			GUI.Label(new Rect(25f, 185f, 320f, 20f), "Lod margin");
			this.ioc.lodMargin = Mathf.Round(GUI.HorizontalSlider(new Rect(25f, 205f, 150f, 20f), this.ioc.lodMargin, 1f, 100f));
			GUI.Label(new Rect(180f, 200f, 50f, 20f), this.ioc.lodMargin.ToString());
		}
		if (this.iocActive)
		{
			GUI.Label(new Rect((float)Screen.width - 74f, 10f, 64f, 64f), this.Icon);
		}
		if (GUI.changed)
		{
			this.dirty = true;
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00018EE8 File Offset: 0x000170E8
	private void ToggleHUD()
	{
		this.hud = !this.hud;
		try
		{
			this.ioc.GetComponent<MouseLook>().enabled = !this.ioc.GetComponent<MouseLook>().enabled;
			this.ioc.transform.parent.GetComponent<MouseLook>().enabled = !this.ioc.transform.parent.GetComponent<MouseLook>().enabled;
		}
		catch
		{
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00018F88 File Offset: 0x00017188
	private void ToggleIOC()
	{
		this.ioc.enabled = this.iocActive;
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject in array)
		{
			IOClod component = gameObject.GetComponent<IOClod>();
			if (component != null)
			{
				bool flag = this.iocActive;
				if (flag)
				{
					if (flag)
					{
						component.UpdateValues();
						component.Initialize();
						component.enabled = true;
					}
				}
				else
				{
					component.enabled = false;
					component.UpdateValues();
					component.Initialize();
				}
			}
		}
	}

	// Token: 0x040003D2 RID: 978
	private Texture2D Icon;

	// Token: 0x040003D3 RID: 979
	private bool iocActive;

	// Token: 0x040003D4 RID: 980
	private IOCcam ioc;

	// Token: 0x040003D5 RID: 981
	private bool realtimeShadows;

	// Token: 0x040003D6 RID: 982
	private bool hud;

	// Token: 0x040003D7 RID: 983
	private bool dirty;
}
