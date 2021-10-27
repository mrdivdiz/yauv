using System;
using UnityEngine;

[Serializable]
public class DepthOfField34 : PostEffectsBase
{
	public Dof34QualitySetting quality;
	public DofResolution resolution;
	public bool simpleTweakMode;
	public float focalPoint;
	public float smoothness;
	public float focalZDistance;
	public float focalZStartCurve;
	public float focalZEndCurve;
	public Transform objectFocus;
	public float focalSize;
	public DofBlurriness bluriness;
	public float maxBlurSpread;
	public float foregroundBlurExtrude;
	public Shader dofBlurShader;
	public Shader dofShader;
	public bool visualize;
	public BokehDestination bokehDestination;
	public bool bokeh;
	public bool bokehSupport;
	public Shader bokehShader;
	public Texture2D bokehTexture;
	public float bokehScale;
	public float bokehIntensity;
	public float bokehThreshholdContrast;
	public float bokehThreshholdLuminance;
	public int bokehDownsample;
}
