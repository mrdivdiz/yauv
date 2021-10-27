Shader "Hidden/Dof/Bokeh34" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Source ("Base (RGB)", 2D) = "black" {}
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend OneMinusDstColor One
Program "vp" {
SubProgram "opengl " {
"!!GLSL
#ifdef VERTEX
varying vec4 xlv_TEXCOORD1;
varying vec2 xlv_TEXCOORD0;
uniform sampler2D _Source;
uniform float _Intensity;
uniform vec4 _ArScale;
void main ()
{
  vec4 tmpvar_1;
  vec4 tmpvar_2;
  tmpvar_1 = gl_Vertex;
  vec4 tmpvar_3;
  tmpvar_3.zw = vec2(0.0, 0.0);
  tmpvar_3.xy = gl_MultiTexCoord1.xy;
  vec4 tmpvar_4;
  tmpvar_4 = texture2DLod (_Source, tmpvar_3.xy, 0.0);
  tmpvar_2 = tmpvar_4;
  tmpvar_1.xy = (gl_Vertex.xy + ((((gl_MultiTexCoord0.xy * 2.0) - 1.0) * _ArScale.xy) * tmpvar_4.w));
  tmpvar_2.xyz = (tmpvar_4.xyz * _Intensity);
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = gl_MultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
varying vec4 xlv_TEXCOORD1;
varying vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
void main ()
{
  vec4 color;
  vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  color = tmpvar_1;
  color.xyz = (tmpvar_1.xyz * xlv_TEXCOORD1.xyz);
  color.w = (tmpvar_1.w * dot ((xlv_TEXCOORD1.xyz * 0.25), vec3(0.22, 0.707, 0.071)));
  gl_FragData[0] = color;
}


#endif
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 0 [_ArScale]
Float 1 [_Intensity]
SetTexture 0 [_Source] 2D
"vs_3_0
; 10 ALU, 2 TEX
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
def c2, 2.00000000, -1.00000000, 1.00000000, 0.00000000
dcl_position0 v0
dcl_texcoord0 v1
dcl_texcoord1 v2
dcl_2d s0
mad r1.xy, v1, c2.x, c2.y
mov r0.z, c2.w
mad r0.xy, v2, c2.zyzw, c2.wzzw
texldl r0, r0.xyzz, s0
mul r1.xy, r1, c0
mul r1.xy, r1, r0.w
add o0.xy, v0, r1
mul o2.xyz, r0, c1.x
mov o2.w, r0
mov o0.zw, v0
mov o1.xy, v1
"
}
}
Program "fp" {
SubProgram "opengl " {
"!!GLSL"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
"ps_3_0
; 4 ALU, 1 TEX
dcl_2d s0
def c0, 0.25000000, 0.21997070, 0.70703125, 0.07098389
dcl_texcoord0 v0.xy
dcl_texcoord1 v1.xyz
mul_pp r0.xyz, v1, c0.x
dp3_pp r1.x, r0, c0.yzww
texld r0, v0, s0
mul_pp oC0.w, r0, r1.x
mul_pp oC0.xyz, r0, v1
"
}
}
 }
}
Fallback Off
}