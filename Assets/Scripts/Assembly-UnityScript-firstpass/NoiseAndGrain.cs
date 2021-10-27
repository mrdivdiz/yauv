using System;
using UnityEngine;

[Serializable]
public class NoiseAndGrain : PostEffectsBase
{
	public float strength;
	public float blackIntensity;
	public float whiteIntensity;
	public float redChannelNoise;
	public float greenChannelNoise;
	public float blueChannelNoise;
	public float redChannelTiling;
	public float greenChannelTiling;
	public float blueChannelTiling;
	public FilterMode filterMode;
	public Shader noiseShader;
	public Texture2D noiseTexture;
}
