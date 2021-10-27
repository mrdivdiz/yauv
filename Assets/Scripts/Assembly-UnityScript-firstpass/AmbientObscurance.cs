using System;
using UnityEngine;

[Serializable]
public class AmbientObscurance : PostEffectsBase
{
	public float intensity;
	public float radius;
	public int blurIterations;
	public float blurFilterDistance;
	public int downsample;
	public Texture2D rand;
	public Shader aoShader;
}
