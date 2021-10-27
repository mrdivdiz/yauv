using UnityEngine;

public class GlowEffect : MonoBehaviour
{
	public float glowIntensity;
	public int blurIterations;
	public float blurSpread;
	public Color glowTint;
	public Shader compositeShader;
	public Shader blurShader;
	public Shader downsampleShader;
}
