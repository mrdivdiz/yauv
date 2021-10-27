using System;
using UnityEngine;

[Serializable]
public class Quality : MonoBehaviour
{
	public SSAOEffect ssaoscript;
	public GlowEffect glowscript;
	public SunShafts sunshafts;
	public Vignetting vignetting;
	public BloomAndLensFlares bloom;
	public ColorCorrectionCurves colorcorrection;
	public DepthOfField34 dof;
	public GlobalFog fog;
	public bool doOnce;
	public bool in3D;
	public Camera FarisCam;
	public AntialiasingAsPostEffect aa;
}
