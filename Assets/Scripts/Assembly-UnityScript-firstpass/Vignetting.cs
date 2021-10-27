using System;
using UnityEngine;

[Serializable]
public class Vignetting : PostEffectsBase
{
	public float intensity;
	public float chromaticAberration;
	public float blur;
	public float blurSpread;
	public Shader vignetteShader;
	public Shader separableBlurShader;
	public Shader chromAberrationShader;
}
