using System;
using UnityEngine;

[Serializable]
public class Crease : PostEffectsBase
{
	public float intensity;
	public int softness;
	public float spread;
	public Shader blurShader;
	public Shader depthFetchShader;
	public Shader creaseApplyShader;
}
