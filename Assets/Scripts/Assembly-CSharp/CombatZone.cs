using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class CombatZone : MonoBehaviour
{
	// Token: 0x060006DB RID: 1755 RVA: 0x00037408 File Offset: 0x00035608
	private void Start()
	{
		if (this.player == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this.player = gameObject.transform;
			}
		}
		this.style = new GUIStyle();
		this.style.font = SpeechManager.getCurrentLanguageFont();
		this.style.alignment = TextAnchor.MiddleCenter;
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00037470 File Offset: 0x00035670
	private void Update()
	{
		if (this.player == null)
		{
			return;
		}
		if (this.faded && Time.time > this.startTime + 3f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
		Vector3 position = base.transform.position;
		position.y = this.player.position.y;
		this.inside = (Vector3.Distance(this.player.position, position) < this.radius);
		if (this.inside)
		{
			this.timeToReturn = 5f;
		}
		else if (this.timeToReturn > -0.5f)
		{
			this.timeToReturn -= Time.deltaTime;
		}
		else if (!this.faded)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 3f);
			this.faded = true;
			this.startTime = Time.time;
		}
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00037584 File Offset: 0x00035784
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x000375B4 File Offset: 0x000357B4
	public void OnGUI()
	{
		if (!this.inside)
		{
			if (this.showBlinkingText)
			{
				this.style.normal.textColor = Color.black;
				GUI.Label(new Rect((float)(Screen.width / 2) - 300f, (float)Screen.height * 0.7f, 600f, 100f), Language.Get("GP_GoBack", 60), this.style);
				this.style.normal.textColor = Color.red;
				GUI.Label(new Rect((float)(Screen.width / 2) - 300f - 2f, (float)Screen.height * 0.7f - 2f, 600f, 100f), Language.Get("GP_GoBack", 60), this.style);
			}
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect((float)(Screen.width / 2) - 300f, (float)Screen.height * 0.75f, 600f, 100f), Mathf.CeilToInt(this.timeToReturn).ToString(), this.style);
			this.style.normal.textColor = Color.red;
			GUI.Label(new Rect((float)(Screen.width / 2) - 300f - 2f, (float)Screen.height * 0.75f - 2f, 600f, 100f), Mathf.CeilToInt(this.timeToReturn).ToString(), this.style);
			if (Time.time - this.showBlinkingTextTimer > 0.1f)
			{
				this.showBlinkingText = !this.showBlinkingText;
				this.showBlinkingTextTimer = Time.time;
			}
		}
	}

	// Token: 0x040008AE RID: 2222
	public float radius = 50f;

	// Token: 0x040008AF RID: 2223
	private Transform player;

	// Token: 0x040008B0 RID: 2224
	private bool inside = true;

	// Token: 0x040008B1 RID: 2225
	private float showBlinkingTextTimer;

	// Token: 0x040008B2 RID: 2226
	private bool showBlinkingText = true;

	// Token: 0x040008B3 RID: 2227
	private GUIStyle style;

	// Token: 0x040008B4 RID: 2228
	private float timeToReturn = 5f;

	// Token: 0x040008B5 RID: 2229
	private float startTime;

	// Token: 0x040008B6 RID: 2230
	private bool stopTimer;

	// Token: 0x040008B7 RID: 2231
	private bool faded;
}
