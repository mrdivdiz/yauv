using UnityEngine;

public class OutlineGlowEffectScript : MonoBehaviour
{
	public int SecondCameraLayer;
	public int BlurSteps;
	public float BlurSpread;
	public bool QuarterResolutionSecondRender;
	public bool SmootherOutlines;
	public bool SplitObjects;
	public bool UseObjectColors;
	public bool SeeThrough;
	public Color OutlineColor;
	public float OutlineStrength;
	public Shader SecondPassShader;
	public Shader BlurPassShader;
	public Shader MixPassShader;
}
