using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class CameraFade : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x00002174 File Offset: 0x00000374
	private void Awake()
	{
		this.m_FadeTexture = new Texture2D(1, 1);
		this.m_BackgroundStyle.normal.background = this.m_FadeTexture;
		this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021A8 File Offset: 0x000003A8
	public void Start()
	{
		this.SetScreenOverlayColor(Color.black);
		if (this.delayStart != 0f)
		{
			base.Invoke("StartDelayed", this.delayStart);
		}
		else
		{
			this.StartDelayed();
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000021E4 File Offset: 0x000003E4
	public void StartDelayed()
	{
		this.StartFade(new Color(0f, 0f, 0f, 0f), 5.5f);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002218 File Offset: 0x00000418
	private void OnGUI()
	{
		if (this.m_CurrentScreenOverlayColor != this.m_TargetScreenOverlayColor)
		{
			if (Mathf.Abs(this.m_CurrentScreenOverlayColor.a - this.m_TargetScreenOverlayColor.a) < Mathf.Abs(this.m_DeltaColor.a) * Time.deltaTime)
			{
				this.m_CurrentScreenOverlayColor = this.m_TargetScreenOverlayColor;
				this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
				this.m_DeltaColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor + this.m_DeltaColor * Time.deltaTime);
			}
		}
		if (this.m_CurrentScreenOverlayColor.a > 0f)
		{
			GUI.depth = this.m_FadeGUIDepth;
			GUI.Label(new Rect(-10f, -10f, (float)(Screen.width + 10), (float)(Screen.height + 10)), this.m_FadeTexture, this.m_BackgroundStyle);
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002324 File Offset: 0x00000524
	public void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		this.m_CurrentScreenOverlayColor = newScreenOverlayColor;
		this.m_FadeTexture.SetPixel(0, 0, this.m_CurrentScreenOverlayColor);
		this.m_FadeTexture.Apply();
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0000234C File Offset: 0x0000054C
	public void StartFade(Color newScreenOverlayColor, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			this.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			this.m_TargetScreenOverlayColor = newScreenOverlayColor;
			this.m_DeltaColor = (this.m_TargetScreenOverlayColor - this.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000238C File Offset: 0x0000058C
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_FadeTexture);
	}

	// Token: 0x04000001 RID: 1
	private GUIStyle m_BackgroundStyle = new GUIStyle();

	// Token: 0x04000002 RID: 2
	private Texture2D m_FadeTexture;

	// Token: 0x04000003 RID: 3
	private Color m_CurrentScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000004 RID: 4
	private Color m_TargetScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000005 RID: 5
	private Color m_DeltaColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000006 RID: 6
	private int m_FadeGUIDepth = -1000;

	// Token: 0x04000007 RID: 7
	public float delayStart;
}
