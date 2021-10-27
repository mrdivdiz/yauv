using System;
using UnityEngine;

[Serializable]
public class GlobalFog : PostEffectsBase
{
	public enum FogMode
	{
		AbsoluteYAndDistance = 0,
		AbsoluteY = 1,
		Distance = 2,
		RelativeYAndDistance = 3,
	}

	public FogMode fogMode;
	public float startDistance;
	public float globalDensity;
	public float heightScale;
	public float height;
	public Color globalFogColor;
	public Shader fogShader;
}
