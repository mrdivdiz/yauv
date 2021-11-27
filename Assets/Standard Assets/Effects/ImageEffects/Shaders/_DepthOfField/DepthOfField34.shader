 Shader "Hidden/Dof/DepthOfField34" {
	Properties {
		_MainTex ("Base", 2D) = "" {}
		_TapLowBackground ("TapLowBackground", 2D) = "" {}
		_TapLowForeground ("TapLowForeground", 2D) = "" {}
		_TapMedium ("TapMedium", 2D) = "" {}
	}

	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		fixed4 pos : SV_POSITION;
		fixed2 uv1 : TEXCOORD0;
	};
	
	struct v2fDofApply {
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
	};
	
	struct v2fRadius {
		fixed4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		fixed4 uv1[4] : TEXCOORD1;
	};
	
	struct v2fDown {
		fixed4 pos : SV_POSITION;
		fixed2 uv0 : TEXCOORD0;
		fixed2 uv[2] : TEXCOORD1;
	};	 
			
	sampler2D _MainTex;
	sampler2D_half _CameraDepthTexture;
	sampler2D _TapLowBackground;	
	sampler2D _TapLowForeground;
	sampler2D _TapMedium;
			
	fixed4 _CurveParams;
	fixed _ForegroundBlurExtrude;
	uniform fixed3 _Threshhold;	
	uniform fixed4 _MainTex_TexelSize;
	fixed4 _MainTex_ST;
	uniform fixed2 _InvRenderTargetSize;
	fixed4 _CameraDepthTexture_ST;
	fixed4 _TapLowBackground_ST;
	fixed4 _TapLowForeground_ST;
	fixed4 _TapMedium_ST;
	
	v2f vert( appdata_img v ) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv1.xy = v.texcoord.xy;
		return o;
	} 

	v2fRadius vertWithRadius( appdata_img v ) {
		v2fRadius o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy, _MainTex_ST);

		const fixed2 blurOffsets[4] = {
			fixed2(-0.5, +1.5),
			fixed2(+0.5, -1.5),
			fixed2(+1.5, +0.5),
			fixed2(-1.5, -0.5)
		}; 	
				
		o.uv1[0].xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 5.0 * _MainTex_TexelSize.xy * blurOffsets[0], _MainTex_ST);
		o.uv1[1].xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 5.0 * _MainTex_TexelSize.xy * blurOffsets[1], _MainTex_ST);
		o.uv1[2].xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 5.0 * _MainTex_TexelSize.xy * blurOffsets[2], _MainTex_ST);
		o.uv1[3].xy = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 5.0 * _MainTex_TexelSize.xy * blurOffsets[3], _MainTex_ST);
		
		o.uv1[0].zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 3.0 * _MainTex_TexelSize.xy * blurOffsets[0], _MainTex_ST);
		o.uv1[1].zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 3.0 * _MainTex_TexelSize.xy * blurOffsets[1], _MainTex_ST);
		o.uv1[2].zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 3.0 * _MainTex_TexelSize.xy * blurOffsets[2], _MainTex_ST);
		o.uv1[3].zw = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy + 3.0 * _MainTex_TexelSize.xy * blurOffsets[3], _MainTex_ST);
		
		return o;
	} 
	
	v2fDofApply vertDofApply( appdata_img v ) {
		v2fDofApply o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv.xy = v.texcoord.xy;
		return o;
	} 	
		
	v2fDown vertDownsampleWithCocConserve(appdata_img v) {
		v2fDown o;
		o.pos = UnityObjectToClipPos(v.vertex);	
		o.uv0.xy = v.texcoord.xy;
		o.uv[0].xy = v.texcoord.xy + fixed2(-1.0,-1.0) * _InvRenderTargetSize;
		o.uv[1].xy = v.texcoord.xy + fixed2(1.0,-1.0) * _InvRenderTargetSize;		
		return o; 
	} 
	
	fixed4 BokehPrereqs (sampler2D tex, fixed4 uv1[4], fixed4 center, fixed considerCoc) {		
		
		// @NOTE 1:
		// we are checking for 3 things in order to create a bokeh.
		// goal is to get the highest bang for the buck.
		// 1.) contrast/frequency should be very high (otherwise bokeh mostly unvisible)
		// 2.) luminance should be high
		// 3.) no occluder nearby (stored in alpha channel)
		
		// @NOTE 2: about the alpha channel in littleBlur:
		// the alpha channel stores an heuristic on how likely it is 
		// that there is no bokeh occluder nearby.
		// if we didn't' check for that, we'd get very noise bokeh
		// popping because of the sudden contrast changes

		fixed4 sampleA = tex2D(tex, uv1[0].zw);
		fixed4 sampleB = tex2D(tex, uv1[1].zw);
		fixed4 sampleC = tex2D(tex, uv1[2].zw);
		fixed4 sampleD = tex2D(tex, uv1[3].zw);
		
		fixed4 littleBlur = 0.125 * (sampleA + sampleB + sampleC + sampleD);
		
		sampleA = tex2D(tex, uv1[0].xy);
		sampleB = tex2D(tex, uv1[1].xy);
		sampleC = tex2D(tex, uv1[2].xy);
		sampleD = tex2D(tex, uv1[3].xy);		

		littleBlur += 0.125 * (sampleA + sampleB + sampleC + sampleD);
				
		//littleBlur = lerp (littleBlur, center, saturate(100.0 * considerCoc * abs(littleBlur.a - center.a)));
				
		return littleBlur;
	}	
	
	fixed4 fragDownsampleWithCocConserve(v2fDown i) : SV_Target {
		fixed2 rowOfs[4];   
		
  		rowOfs[0] = fixed2(0.0, 0.0);  
  		rowOfs[1] = fixed2(0.0, _InvRenderTargetSize.y);  
  		rowOfs[2] = fixed2(0.0, _InvRenderTargetSize.y) * 2.0;  
  		rowOfs[3] = fixed2(0.0, _InvRenderTargetSize.y) * 3.0; 
  		
  		fixed4 color = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv0.xy, _MainTex_ST));
			
		fixed4 sampleA = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv[0].xy + rowOfs[0], _MainTex_ST));
		fixed4 sampleB = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv[1].xy + rowOfs[0], _MainTex_ST));
		fixed4 sampleC = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv[0].xy + rowOfs[2], _MainTex_ST));
		fixed4 sampleD = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv[1].xy + rowOfs[2], _MainTex_ST));
		
		color += sampleA + sampleB + sampleC + sampleD;
		color *= 0.2;
		
		// @NOTE we are doing max on the alpha channel for 2 reasons:
		// 1) foreground blur likes a slightly bigger radius
		// 2) otherwise we get an ugly outline between high blur- and medium blur-areas
		// drawback: we get a little bit of color bleeding  		
		
		color.a = max(max(sampleA.a, sampleB.a), max(sampleC.a, sampleD.a));
  		
		return color;
	}
	
	fixed4 fragDofApplyBg (v2fDofApply i) : SV_Target {		
		fixed4 tapHigh = tex2D (_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MainTex_ST));
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			i.uv.xy = i.uv.xy * fixed2(1,-1)+fixed2(0,1);
		#endif
		
		fixed4 tapLow = tex2D (_TapLowBackground, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _TapLowBackground_ST)); // already mixed with medium blur
		tapHigh = lerp (tapHigh, tapLow, tapHigh.a);
		return tapHigh; 
	}	
	
	fixed4 fragDofApplyBgDebug (v2fDofApply i) : SV_Target {		
		fixed4 tapHigh = tex2D (_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MainTex_ST));
		
		fixed4 tapLow = tex2D (_TapLowBackground, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _TapLowBackground_ST));
		
		fixed4 tapMedium = tex2D (_TapMedium, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _TapMedium_ST));
		tapMedium.rgb = (tapMedium.rgb + fixed3 (1, 1, 0)) * 0.5;	
		tapLow.rgb = (tapLow.rgb + fixed3 (0, 1, 0)) * 0.5;
		
		tapLow = lerp (tapMedium, tapLow, saturate (tapLow.a * tapLow.a));		
		tapLow = tapLow * 0.5 + tex2D (_TapLowBackground, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _TapLowBackground_ST)) * 0.5;

		return lerp (tapHigh, tapLow, tapHigh.a);
	}		
	
	fixed4 fragDofApplyFg (v2fDofApply i) : SV_Target {
		fixed4 fgBlur = tex2D(_TapLowForeground, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _TapLowForeground_ST));
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			i.uv.xy = i.uv.xy * fixed2(1,-1)+fixed2(0,1);
		#endif
				
		fixed4 fgColor = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MainTex_ST));
				
		//fgBlur.a = saturate(fgBlur.a*_ForegroundBlurWeight+saturate(fgColor.a-fgBlur.a));
		//fgBlur.a = max (fgColor.a, (2.0 * fgBlur.a - fgColor.a)) * _ForegroundBlurExtrude;
		fgBlur.a = max(fgColor.a, fgBlur.a * _ForegroundBlurExtrude); //max (fgColor.a, (2.0*fgBlur.a-fgColor.a)) * _ForegroundBlurExtrude;
		
		return lerp (fgColor, fgBlur, saturate(fgBlur.a));
	}	
	
	fixed4 fragDofApplyFgDebug (v2fDofApply i) : SV_Target {
		fixed4 fgBlur = tex2D(_TapLowForeground, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _TapLowForeground_ST));
					
		fixed4 fgColor = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv.xy, _MainTex_ST));
		
		fgBlur.a = max(fgColor.a, fgBlur.a * _ForegroundBlurExtrude); //max (fgColor.a, (2.0*fgBlur.a-fgColor.a)) * _ForegroundBlurExtrude;
		
		fixed4 tapMedium = fixed4 (1, 1, 0, fgBlur.a);	
		tapMedium.rgb = 0.5 * (tapMedium.rgb + fgColor.rgb);
		
		fgBlur.rgb = 0.5 * (fgBlur.rgb + fixed3(0,1,0));
		fgBlur.rgb = lerp (tapMedium.rgb, fgBlur.rgb, saturate (fgBlur.a * fgBlur.a));
		
		return lerp ( fgColor, fgBlur, saturate(fgBlur.a));
	}	
		
	fixed4 fragCocBg (v2f i) : SV_Target {
		
		fixed d = SAMPLE_DEPTH_TEXTURE (_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv1.xy, _MainTex_ST));
		d = Linear01Depth (d);
		fixed coc = 0.0; 
		
		fixed focalDistance01 = _CurveParams.w + _CurveParams.z;
		
		if (d > focalDistance01) 
			coc = (d - focalDistance01);
	
		coc = saturate (coc * _CurveParams.y);	
		return coc;
	} 
	
	fixed4 fragCocFg (v2f i) : SV_Target {		
		fixed4 color = tex2D (_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv1.xy, _MainTex_ST));
		color.a = 0.0;

		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			i.uv1.xy = i.uv1.xy * fixed2(1,-1)+fixed2(0,1);
		#endif

		fixed d = SAMPLE_DEPTH_TEXTURE (_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(i.uv1.xy, _MainTex_ST));
		d = Linear01Depth (d);	
		
		fixed focalDistance01 = (_CurveParams.w - _CurveParams.z);	
		
		if (d < focalDistance01) 
			color.a = (focalDistance01 - d);
		
		color.a = saturate (color.a * _CurveParams.x);	
		return color;	
	}	
	
	// not being used atm
	
	fixed4 fragMask (v2f i) : SV_Target {
		return fixed4(0,0,0,0); 
	}	
	
	// used for simple one one blend
	
	fixed4 fragAddBokeh (v2f i) : SV_Target {	
		fixed4 from = tex2D( _MainTex, UnityStereoScreenSpaceUVAdjust(i.uv1.xy, _MainTex_ST) );
		return from;
	}
	
	fixed4 fragAddFgBokeh (v2f i) : SV_Target {		
		fixed4 from = tex2D( _MainTex, UnityStereoScreenSpaceUVAdjust(i.uv1.xy, _MainTex_ST) );
		return from; 
	}
		
	fixed4 fragDarkenForBokeh(v2fRadius i) : SV_Target {		
		fixed4 fromOriginal = tex2D(_MainTex, i.uv.xy);
		fixed4 lowRez = BokehPrereqs (_MainTex, i.uv1, fromOriginal, _Threshhold.z);
		fixed4 outColor = fixed4(0,0,0, fromOriginal.a);
		fixed modulate = fromOriginal.a;		
		
		// this code imitates the if-then-else conditions below
		fixed2 conditionCheck = fixed2( dot(abs(fromOriginal.rgb-lowRez.rgb), fixed3(0.3,0.5,0.2)), Luminance(fromOriginal.rgb));
		conditionCheck *= fromOriginal.a;
		conditionCheck = saturate(_Threshhold.xy - conditionCheck);
		outColor = lerp (outColor, fromOriginal, saturate (dot(conditionCheck, fixed2(1000.0,1000.0))));
		
		/*
		if ( abs(dot(fromOriginal.rgb - lowRez.rgb,  fixed3 (0.3,0.5,0.2))) * modulate < _Threshhold.x)
			outColor = fromOriginal; // no darkening
		if (Luminance(fromOriginal.rgb) * modulate < _Threshhold.y)
			outColor = fromOriginal; // no darkening
		if (lowRez.a < _Threshhold.z) // need to make foreground not cast false bokeh's
			outColor = fromOriginal; // no darkenin
		*/	
		 
		return outColor;
	}
 
 	fixed4 fragExtractAndAddToBokeh (v2fRadius i) : SV_Target {	
		fixed4 from = tex2D(_MainTex, i.uv.xy);
		fixed4 lowRez = BokehPrereqs(_MainTex, i.uv1, from, _Threshhold.z);
		fixed4 outColor = from;

		// this code imitates the if-then-else conditions below
		fixed2 conditionCheck = fixed2( dot(abs(from.rgb-lowRez.rgb), fixed3(0.3,0.5,0.2)), Luminance(from.rgb));
		conditionCheck *= from.a;
		conditionCheck = saturate(_Threshhold.xy - conditionCheck);
		outColor = lerp (outColor, fixed4(0,0,0,0), saturate (dot(conditionCheck, fixed2(1000.0,1000.0))));
		
		/*
		if ( abs(dot(from.rgb - lowRez.rgb,  fixed3 (0.3,0.5,0.2))) * modulate < _Threshhold.x)
			outColor = fixed4(0,0,0,0); // don't add
		if (Luminance(from.rgb) * modulate < _Threshhold.y)
			outColor = fixed4(0,0,0,0); // don't add
		if (lowRez.a < _Threshhold.z) // need to make foreground not cast false bokeh's
			outColor = fixed4(0,0,0,0); // don't add
		*/
							
		return outColor;
	}
 
	ENDCG
	
Subshader {
 
 // pass 0
 
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vertDofApply
      #pragma fragment fragDofApplyBg
      ENDCG
  	}

 // pass 1
 
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  ColorMask RGB

      CGPROGRAM
      #pragma vertex vertDofApply
      #pragma fragment fragDofApplyFgDebug
      ENDCG
  	}

 // pass 2

 Pass {
	  ZTest Always Cull Off ZWrite Off
	  ColorMask RGB

      CGPROGRAM
      #pragma vertex vertDofApply
      #pragma fragment fragDofApplyBgDebug
      ENDCG
  	}
  	
  	
 
 // pass 3
 
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  ColorMask A

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragCocBg
      ENDCG
  	}  
  	 	
	
 // pass 4

  
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  ColorMask RGB
	  //Blend One One

      CGPROGRAM
      #pragma vertex vertDofApply
      #pragma fragment fragDofApplyFg
      ENDCG
  	}  	

 // pass 5
  
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  ColorMask ARGB

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragCocFg
      ENDCG
  	} 

 // pass 6
 
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vertDownsampleWithCocConserve
      #pragma fragment fragDownsampleWithCocConserve
      ENDCG
  	} 

 // pass 7
 // not being used atm
 
 Pass { 
	  ZTest Always Cull Off ZWrite Off
	  ColorMask RGBA

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragMask
      ENDCG
  	} 

 // pass 8
 
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Blend SrcAlpha OneMinusSrcAlpha
	  ColorMask RGB

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment fragAddBokeh
      ENDCG
  	} 
  	
 // pass 9
 
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Blend One One
	  ColorMask RGB

      CGPROGRAM
      #pragma vertex vertWithRadius
      #pragma fragment fragExtractAndAddToBokeh
      ENDCG
  	} 
  	
 // pass 10
 
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vertWithRadius
      #pragma fragment fragDarkenForBokeh
      ENDCG
  	}   	
  	
 // pass 11
 
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vertWithRadius
      #pragma fragment fragExtractAndAddToBokeh
      ENDCG
  	}   	
  }
  
Fallback off

}
