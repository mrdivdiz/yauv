Shader "Custom/QuadChaseBlend" {
         Properties {
			_BumpMap ("Bumpmap", 2D) = "bump" {}
             _Blend ("Texture Blend", 2D) = "white" {}
             _MainTex ("Albedo (RGB)", 2D) = "white" {}
             _MainTex2 ("Albedo 2 (RGB)", 2D) = "white" {}
             //_Glossiness ("Smoothness", Range(0,1)) = 0.5
         }
         SubShader {
             Tags { "RenderType"="Opaque" }
             LOD 200
            
             CGPROGRAM
             // Physically based Standard lighting model, and enable shadows on all light types
             #pragma surface surf BlinnPhong
      
             // Use shader model 3.0 target, to get nicer looking lighting
             //#pragma target 3.0
      
             sampler2D _MainTex;
             sampler2D _MainTex2;
			 sampler2D _Blend;
			 sampler2D _BumpMap;
      
             struct Input {
                 half2 uv_MainTex;
                 half2 uv_MainTex2;
				 half2 uv_Blend;
				 half2 uv_BumpMap;
             };
      
             //half _Blend;
             //half _Glossiness;
      
             void surf (Input IN, inout SurfaceOutput  o) {
                 // Albedo comes from a texture tinted by color
                 fixed4 c = lerp (tex2D (_MainTex, IN.uv_MainTex), tex2D (_MainTex2, IN.uv_MainTex), tex2D (_Blend, IN.uv_Blend).a);
                 o.Albedo = c.rgb;
				 o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
                 // Metallic and smoothness come from slider variables
                 o.Alpha = c.a;
             }
             ENDCG
         }
         FallBack "Diffuse"
     }