using System;
using UnityEngine;

[Serializable]
public class SunShafts : PostEffectsBase
{
	public SunShaftsResolution resolution;
	public ShaftsScreenBlendMode screenBlendMode;
	public Transform sunTransform;
	public int radialBlurIterations;
	public Color sunColor;
	public float sunShaftBlurRadius;
	public float sunShaftIntensity;
	public float useSkyBoxAlpha;
	public float maxRadius;
	public bool useDepthTexture;
	public Shader sunShaftsShader;
	public Shader simpleClearShader;
}
