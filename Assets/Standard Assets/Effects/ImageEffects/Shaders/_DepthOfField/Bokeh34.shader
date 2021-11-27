
Shader "Hidden/Dof/Bokeh34" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Source ("Base (RGB)", 2D) = "black" {}
}

SubShader {
	CGINCLUDE

	#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	sampler2D _Source;
	
	uniform fixed4 _ArScale;		
	uniform fixed _Intensity; 
	uniform fixed4 _Source_TexelSize;
	fixed4 _Source_ST;
	
	struct v2f {
		fixed4 pos : SV_POSITION;
		fixed2 uv2 : TEXCOORD0;
		fixed4 source : TEXCOORD1;
	};
	
	#define COC bokeh.a
	
	v2f vert (appdata_full v)
	{
		v2f o;
		
		o.pos = v.vertex; 
				
		o.uv2.xy = v.texcoord.xy;// * 2.0; <- needed when using Triangles.js and not Quads.js
		
		#if UNITY_UV_STARTS_AT_TOP
			fixed4 bokeh = tex2Dlod (_Source, fixed4 (UnityStereoScreenSpaceUVAdjust(v.texcoord1.xy * fixed2(1,-1) + fixed2(0,1), _Source_ST), 0, 0));
		#else
			fixed4 bokeh = tex2Dlod (_Source, fixed4 (UnityStereoScreenSpaceUVAdjust(v.texcoord1.xy, _Source_ST), 0, 0));
		#endif
		
		o.source = bokeh;			

		o.pos.xy += (v.texcoord.xy * 2.0 - 1.0) * _ArScale.xy * COC;// + _ArScale.zw * coc;
		o.source.rgb *= _Intensity;		
								
		return o;
	}
	
	
	fixed4 frag (v2f i) : SV_Target 
	{
		fixed4 color = tex2D (_MainTex, i.uv2.xy);
		color.rgb *= i.source.rgb;	
		color.a *= Luminance(i.source.rgb*0.25);
		return color;
	}
	
	ENDCG

	Pass {
		Blend OneMinusDstColor One 
		ZTest Always Cull Off ZWrite Off

		CGPROGRAM
				
		#pragma vertex vert
		#pragma fragment frag
		
		ENDCG
	}

}

Fallback off

}
