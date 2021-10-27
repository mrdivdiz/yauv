using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
[Serializable]
public class setupVertexLitShader : MonoBehaviour
{
	// Token: 0x0600010E RID: 270 RVA: 0x00006D54 File Offset: 0x00004F54
	public setupVertexLitShader()
	{
		this.VertexLitTranslucencyColor = new Color(0.73f, 0.85f, 0.4f, (float)1);
		this.VertexLitWaveScale = 2;
		this.VertexLitDetailDistance = 60;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00006D88 File Offset: 0x00004F88
	public virtual void Start()
	{
		Shader.SetGlobalColor("_VertexLitTranslucencyColor", this.VertexLitTranslucencyColor);
		Shader.SetGlobalFloat("_VertexLitWaveScale", (float)this.VertexLitWaveScale);
		float[] array = new float[32];
		array[9] = (float)this.VertexLitDetailDistance;
		Camera.main.layerCullDistances = array;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00006DD4 File Offset: 0x00004FD4
	public virtual void Update()
	{
		if (Input.GetKeyDown("1"))
		{
			Camera.main.renderingPath = RenderingPath.Forward;
		}
		if (Input.GetKeyDown("2"))
		{
			Camera.main.renderingPath = RenderingPath.DeferredLighting;
		}
		if (Input.GetKeyDown("3"))
		{
			GameObject gameObject = GameObject.Find("01 Sun");
			gameObject.transform.Rotate(Vector3.up * (float)30, Space.World);
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00006E48 File Offset: 0x00005048
	public virtual void Main()
	{
	}

	// Token: 0x040000F4 RID: 244
	public Color VertexLitTranslucencyColor;

	// Token: 0x040000F5 RID: 245
	public int VertexLitWaveScale;

	// Token: 0x040000F6 RID: 246
	public int VertexLitDetailDistance;
}
