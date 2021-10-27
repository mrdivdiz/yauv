Shader "Hidden/FXAA II" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
"!!GLSL
#ifdef VERTEX
varying vec4 xlv_TEXCOORD0;

uniform vec4 _MainTex_TexelSize;
void main ()
{
  vec4 posPos;
  posPos.xy = ((((gl_MultiTexCoord0.xy * 2.0) - 1.0) * 0.5) + 0.5);
  posPos.zw = (posPos.xy - (_MainTex_TexelSize.xy * 0.75));
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = posPos;
}


#endif
#ifdef FRAGMENT
#extension GL_ARB_shader_texture_lod : enable
varying vec4 xlv_TEXCOORD0;
uniform vec4 _MainTex_TexelSize;
uniform sampler2D _MainTex;
void main ()
{
  vec3 tmpvar_1;
  vec2 dir;
  vec4 tmpvar_2;
  tmpvar_2.zw = vec2(0.0, 0.0);
  tmpvar_2.xy = xlv_TEXCOORD0.zw;
  vec4 tmpvar_3;
  tmpvar_3.zw = vec2(0.0, 0.0);
  tmpvar_3.xy = (xlv_TEXCOORD0.zw + (vec2(1.0, 0.0) * _MainTex_TexelSize.xy));
  vec4 tmpvar_4;
  tmpvar_4.zw = vec2(0.0, 0.0);
  tmpvar_4.xy = (xlv_TEXCOORD0.zw + (vec2(0.0, 1.0) * _MainTex_TexelSize.xy));
  vec4 tmpvar_5;
  tmpvar_5.zw = vec2(0.0, 0.0);
  tmpvar_5.xy = (xlv_TEXCOORD0.zw + _MainTex_TexelSize.xy);
  vec4 tmpvar_6;
  tmpvar_6.zw = vec2(0.0, 0.0);
  tmpvar_6.xy = xlv_TEXCOORD0.xy;
  float tmpvar_7;
  tmpvar_7 = dot (texture2DLod (_MainTex, tmpvar_2.xy, 0.0).xyz, vec3(0.299, 0.587, 0.114));
  float tmpvar_8;
  tmpvar_8 = dot (texture2DLod (_MainTex, tmpvar_3.xy, 0.0).xyz, vec3(0.299, 0.587, 0.114));
  float tmpvar_9;
  tmpvar_9 = dot (texture2DLod (_MainTex, tmpvar_4.xy, 0.0).xyz, vec3(0.299, 0.587, 0.114));
  float tmpvar_10;
  tmpvar_10 = dot (texture2DLod (_MainTex, tmpvar_5.xy, 0.0).xyz, vec3(0.299, 0.587, 0.114));
  float tmpvar_11;
  tmpvar_11 = dot (texture2DLod (_MainTex, tmpvar_6.xy, 0.0).xyz, vec3(0.299, 0.587, 0.114));
  float tmpvar_12;
  tmpvar_12 = min (tmpvar_11, min (min (tmpvar_7, tmpvar_8), min (tmpvar_9, tmpvar_10)));
  float tmpvar_13;
  tmpvar_13 = max (tmpvar_11, max (max (tmpvar_7, tmpvar_8), max (tmpvar_9, tmpvar_10)));
  dir.x = ((tmpvar_9 + tmpvar_10) - (tmpvar_7 + tmpvar_8));
  dir.y = ((tmpvar_7 + tmpvar_9) - (tmpvar_8 + tmpvar_10));
  vec2 tmpvar_14;
  tmpvar_14 = (min (vec2(8.0, 8.0), max (vec2(-8.0, -8.0), (dir * (1.0/((min (abs (dir.x), abs (dir.y)) + max (((((tmpvar_7 + tmpvar_8) + tmpvar_9) + tmpvar_10) * 0.03125), 0.0078125))))))) * _MainTex_TexelSize.xy);
  dir = tmpvar_14;
  vec4 tmpvar_15;
  tmpvar_15.zw = vec2(0.0, 0.0);
  tmpvar_15.xy = (xlv_TEXCOORD0.xy + (tmpvar_14 * -0.166667));
  vec4 tmpvar_16;
  tmpvar_16.zw = vec2(0.0, 0.0);
  tmpvar_16.xy = (xlv_TEXCOORD0.xy + (tmpvar_14 * 0.166667));
  vec3 tmpvar_17;
  tmpvar_17 = (0.5 * (texture2DLod (_MainTex, tmpvar_15.xy, 0.0).xyz + texture2DLod (_MainTex, tmpvar_16.xy, 0.0).xyz));
  vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, 0.0);
  tmpvar_18.xy = (xlv_TEXCOORD0.xy + (tmpvar_14 * -0.5));
  vec4 tmpvar_19;
  tmpvar_19.zw = vec2(0.0, 0.0);
  tmpvar_19.xy = (xlv_TEXCOORD0.xy + (tmpvar_14 * 0.5));
  vec3 tmpvar_20;
  tmpvar_20 = ((tmpvar_17 * 0.5) + (0.25 * (texture2DLod (_MainTex, tmpvar_18.xy, 0.0).xyz + texture2DLod (_MainTex, tmpvar_19.xy, 0.0).xyz)));
  float tmpvar_21;
  tmpvar_21 = dot (tmpvar_20, vec3(0.299, 0.587, 0.114));
  bool tmpvar_22;
  if ((tmpvar_21 < tmpvar_12)) {
    tmpvar_22 = bool(1);
  } else {
    tmpvar_22 = (tmpvar_21 > tmpvar_13);
  };
  if (tmpvar_22) {
    tmpvar_1 = tmpvar_17;
  } else {
    tmpvar_1 = tmpvar_20;
  };
  vec4 tmpvar_23;
  tmpvar_23.w = 0.0;
  tmpvar_23.xyz = tmpvar_1;
  gl_FragData[0] = tmpvar_23;
}


#endif
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_3_0
; 9 ALU
dcl_position o0
dcl_texcoord0 o1
def c5, 2.00000000, -1.00000000, 0.50000000, 0.75000000
dcl_position0 v0
dcl_texcoord0 v1
mad r0.xy, v1, c5.x, c5.y
mad r0.xy, r0, c5.z, c5.z
mov r0.zw, c4.xyxy
mad r0.zw, c5.w, -r0, r0.xyxy
mov o1, r0
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
"!!GLSL"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"ps_3_0
; 69 ALU, 18 TEX
dcl_2d s0
def c1, 0.00000000, 0.29899999, 0.58700001, 0.11400000
def c2, 0.00000000, 1.00000000, 0.03125000, 0.00781250
def c3, -8.00000000, 8.00000000, -0.16666666, 0.16666669
def c4, 0.50000000, -0.50000000, 0.25000000, 0
dcl_texcoord0 v0
mov r0.z, c1.x
add r0.xy, v0.zwzw, c0
texldl r1.xyz, r0.xyzz, s0
mov r0.xy, c0
dp3 r3.w, r1, c1.yzww
mov r0.z, c1.x
mad r0.xy, c2, r0, v0.zwzw
texldl r0.xyz, r0.xyzz, s0
dp3 r1.w, r0, c1.yzww
mov r0.xy, c0
add r3.x, r1.w, r3.w
mov r0.z, c1.x
mad r0.xy, c2.yxzw, r0, v0.zwzw
texldl r1.xyz, r0.xyzz, s0
dp3 r2.w, r1, c1.yzww
add r1.x, r2.w, r3.w
mov r1.z, c1.x
mov r0.z, c1.x
mov r0.xy, v0.zwzw
texldl r0.xyz, r0.xyzz, s0
dp3 r0.w, r0, c1.yzww
add r0.y, r0.w, r2.w
add r0.z, r1.w, r0.y
add r1.y, r3.w, r0.z
add r0.x, r0.y, -r3
add r0.z, r0.w, r1.w
add r0.z, r0, -r1.x
abs r0.y, -r0.x
abs r1.x, r0.z
mul r1.y, r1, c2.z
min r0.y, r0, r1.x
max r1.y, r1, c2.w
add r0.y, r0, r1
rcp r1.x, r0.y
mov r0.y, r0.z
mov r0.x, -r0
mul r0.xy, r0, r1.x
max r0.xy, r0, c3.x
min r0.xy, r0, c3.y
mul r0.xy, r0, c0
mad r1.xy, r0, c4.x, v0
texldl r3.xyz, r1.xyzz, s0
mad r1.xy, r0, c4.y, v0
mov r1.z, c1.x
texldl r1.xyz, r1.xyzz, s0
add r3.xyz, r1, r3
mad r1.xy, r0, c3.w, v0
mov r1.z, c1.x
texldl r1.xyz, r1.xyzz, s0
mov r0.z, c1.x
mad r0.xy, r0, c3.z, v0
texldl r0.xyz, r0.xyzz, s0
add r0.xyz, r0, r1
mul r1.xyz, r3, c4.z
mul r0.xyz, r0, c4.x
mad r1.xyz, r0, c4.x, r1
min r3.y, r1.w, r3.w
min r3.x, r0.w, r2.w
min r4.y, r3.x, r3
dp3 r4.x, r1, c1.yzww
max r0.w, r0, r2
max r1.w, r1, r3
max r1.w, r0, r1
mov r3.z, c1.x
mov r3.xy, v0
texldl r3.xyz, r3.xyzz, s0
dp3 r0.w, r3, c1.yzww
max r1.w, r0, r1
min r0.w, r0, r4.y
add r1.w, -r4.x, r1
add r0.w, r4.x, -r0
cmp r1.w, r1, c2.x, c2.y
cmp r0.w, r0, c2.x, c2.y
add_pp_sat r0.w, r0, r1
cmp r2.xyz, -r0.w, r2, r0
cmp_pp r0.x, -r0.w, c2.y, c2
cmp oC0.xyz, -r0.x, r2, r1
mov oC0.w, c1.x
"
}
}
 }
}
Fallback Off
}