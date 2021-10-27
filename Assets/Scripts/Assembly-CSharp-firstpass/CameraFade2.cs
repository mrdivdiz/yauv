using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class CameraFade2 : MonoBehaviour
{
	// Token: 0x0600000A RID: 10 RVA: 0x00002424 File Offset: 0x00000624
	private void Awake()
	{
		this.m_FadeTexture = new Texture2D(1, 1);
		this.m_BackgroundStyle.normal.background = this.m_FadeTexture;
		this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002458 File Offset: 0x00000658
	public void Start()
	{
		this.SetScreenOverlayColor(Color.black);
		this.StartFade(new Color(0f, 0f, 0f, 0f), 5.5f);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002494 File Offset: 0x00000694
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

	// Token: 0x0600000D RID: 13 RVA: 0x000025A0 File Offset: 0x000007A0
	public void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		this.m_CurrentScreenOverlayColor = newScreenOverlayColor;
		this.m_FadeTexture.SetPixel(0, 0, this.m_CurrentScreenOverlayColor);
		this.m_FadeTexture.Apply();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000025C8 File Offset: 0x000007C8
	public void StartFade(Color newScreenOverlayColor, float fadeDuration = 7f)
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

	// Token: 0x04000008 RID: 8
	private GUIStyle m_BackgroundStyle = new GUIStyle();

	// Token: 0x04000009 RID: 9
	private Texture2D m_FadeTexture;

	// Token: 0x0400000A RID: 10
	private Color m_CurrentScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400000B RID: 11
	private Color m_TargetScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400000C RID: 12
	private Color m_DeltaColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400000D RID: 13
	private int m_FadeGUIDepth = -1000;
}
