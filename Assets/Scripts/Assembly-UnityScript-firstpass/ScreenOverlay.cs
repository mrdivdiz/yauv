using System;
using UnityEngine;

[Serializable]
public class ScreenOverlay : PostEffectsBase
{
	public enum OverlayBlendMode
	{
		AddSub = 0,
		ScreenBlend = 1,
		Multiply = 2,
		Overlay = 3,
		AlphaBlend = 4,
	}

	public OverlayBlendMode blendMode;
	public float intensity;
	public Texture2D texture;
	public Shader overlayShader;
}
