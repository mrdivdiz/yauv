Shader "Hidden/FXAA III (Console)" {
Properties {
 _MainTex ("-", 2D) = "white" {}
 _EdgeThresholdMin ("Edge threshold min", Float) = 0.125
 _EdgeThreshold ("Edge Threshold", Float) = 0.25
 _EdgeSharpness ("Edge sharpness", Float) = 4
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
varying vec4 xlv_TEXCOORD3;
varying vec4 xlv_TEXCOORD2;
varying vec4 xlv_TEXCOORD1;
varying vec2 xlv_TEXCOORD0;

uniform vec4 _MainTex_TexelSize;
void main ()
{
  vec4 rcpSize;
  vec4 extents;
  vec4 tmpvar_1;
  vec2 tmpvar_2;
  tmpvar_2 = (_MainTex_TexelSize.xy * 0.5);
  extents.xy = (gl_MultiTexCoord0.xy - tmpvar_2);
  extents.zw = (gl_MultiTexCoord0.xy + tmpvar_2);
  rcpSize.xy = (-(_MainTex_TexelSize.xy) * 0.5);
  rcpSize.zw = (_MainTex_TexelSize.xy * 0.5);
  tmpvar_1 = rcpSize;
  tmpvar_1.xy = (rcpSize.xy * 4.0);
  tmpvar_1.zw = (rcpSize.zw * 4.0);
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = gl_MultiTexCoord0.xy;
  xlv_TEXCOORD1 = extents;
  xlv_TEXCOORD2 = rcpSize;
  xlv_TEXCOORD3 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
#extension GL_ARB_shader_texture_lod : enable
varying vec4 xlv_TEXCOORD3;
varying vec4 xlv_TEXCOORD2;
varying vec4 xlv_TEXCOORD1;
varying vec2 xlv_TEXCOORD0;
uniform sampler2D _MainTex;
uniform float _EdgeThresholdMin;
uniform float _EdgeThreshold;
uniform float _EdgeSharpness;
void main ()
{
  vec3 tmpvar_1;
  vec2 dir;
  vec4 tmpvar_2;
  tmpvar_2.zw = vec2(0.0, 0.0);
  tmpvar_2.xy = xlv_TEXCOORD1.xy;
  float tmpvar_3;
  tmpvar_3 = dot (texture2DLod (_MainTex, tmpvar_2.xy, 0.0).xyz, vec3(0.22, 0.707, 0.071));
  vec4 tmpvar_4;
  tmpvar_4.zw = vec2(0.0, 0.0);
  tmpvar_4.xy = xlv_TEXCOORD1.xw;
  float tmpvar_5;
  tmpvar_5 = dot (texture2DLod (_MainTex, tmpvar_4.xy, 0.0).xyz, vec3(0.22, 0.707, 0.071));
  vec4 tmpvar_6;
  tmpvar_6.zw = vec2(0.0, 0.0);
  tmpvar_6.xy = xlv_TEXCOORD1.zy;
  vec4 tmpvar_7;
  tmpvar_7.zw = vec2(0.0, 0.0);
  tmpvar_7.xy = xlv_TEXCOORD1.zw;
  float tmpvar_8;
  tmpvar_8 = dot (texture2DLod (_MainTex, tmpvar_7.xy, 0.0).xyz, vec3(0.22, 0.707, 0.071));
  vec4 tmpvar_9;
  tmpvar_9.zw = vec2(0.0, 0.0);
  tmpvar_9.xy = xlv_TEXCOORD0;
  vec4 tmpvar_10;
  tmpvar_10 = texture2DLod (_MainTex, tmpvar_9.xy, 0.0);
  float tmpvar_11;
  tmpvar_11 = dot (tmpvar_10.xyz, vec3(0.22, 0.707, 0.071));
  float tmpvar_12;
  tmpvar_12 = (dot (texture2DLod (_MainTex, tmpvar_6.xy, 0.0).xyz, vec3(0.22, 0.707, 0.071)) + 0.00260417);
  float tmpvar_13;
  tmpvar_13 = max (max (tmpvar_12, tmpvar_8), max (tmpvar_3, tmpvar_5));
  float tmpvar_14;
  tmpvar_14 = min (min (tmpvar_12, tmpvar_8), min (tmpvar_3, tmpvar_5));
  float tmpvar_15;
  tmpvar_15 = max (_EdgeThresholdMin, (tmpvar_13 * _EdgeThreshold));
  float tmpvar_16;
  tmpvar_16 = (tmpvar_5 - tmpvar_12);
  float tmpvar_17;
  tmpvar_17 = (max (tmpvar_13, tmpvar_11) - min (tmpvar_14, tmpvar_11));
  float tmpvar_18;
  tmpvar_18 = (tmpvar_8 - tmpvar_3);
  if ((tmpvar_17 < tmpvar_15)) {
    tmpvar_1 = tmpvar_10.xyz;
  } else {
    dir.x = (tmpvar_16 + tmpvar_18);
    dir.y = (tmpvar_16 - tmpvar_18);
    vec2 tmpvar_19;
    tmpvar_19 = normalize (dir);
    vec4 tmpvar_20;
    tmpvar_20.zw = vec2(0.0, 0.0);
    tmpvar_20.xy = (xlv_TEXCOORD0 - (tmpvar_19 * xlv_TEXCOORD2.zw));
    vec4 tmpvar_21;
    tmpvar_21.zw = vec2(0.0, 0.0);
    tmpvar_21.xy = (xlv_TEXCOORD0 + (tmpvar_19 * xlv_TEXCOORD2.zw));
    vec2 tmpvar_22;
    tmpvar_22 = clamp ((tmpvar_19 / (min (abs (tmpvar_19.x), abs (tmpvar_19.y)) * _EdgeSharpness)), vec2(-2.0, -2.0), vec2(2.0, 2.0));
    dir = tmpvar_22;
    vec4 tmpvar_23;
    tmpvar_23.zw = vec2(0.0, 0.0);
    tmpvar_23.xy = (xlv_TEXCOORD0 - (tmpvar_22 * xlv_TEXCOORD3.zw));
    vec4 tmpvar_24;
    tmpvar_24.zw = vec2(0.0, 0.0);
    tmpvar_24.xy = (xlv_TEXCOORD0 + (tmpvar_22 * xlv_TEXCOORD3.zw));
    vec3 tmpvar_25;
    tmpvar_25 = (texture2DLod (_MainTex, tmpvar_20.xy, 0.0).xyz + texture2DLod (_MainTex, tmpvar_21.xy, 0.0).xyz);
    vec3 tmpvar_26;
    tmpvar_26 = (((texture2DLod (_MainTex, tmpvar_23.xy, 0.0).xyz + texture2DLod (_MainTex, tmpvar_24.xy, 0.0).xyz) * 0.25) + (tmpvar_25 * 0.25));
    float tmpvar_27;
    tmpvar_27 = dot (tmpvar_25, vec3(0.22, 0.707, 0.071));
    bool tmpvar_28;
    if ((tmpvar_27 < tmpvar_14)) {
      tmpvar_28 = bool(1);
    } else {
      tmpvar_28 = (dot (tmpvar_26, vec3(0.22, 0.707, 0.071)) > tmpvar_13);
    };
    if (tmpvar_28) {
      tmpvar_1 = (tmpvar_25 * 0.5);
    } else {
      tmpvar_1 = tmpvar_26;
    };
  };
  vec4 tmpvar_29;
  tmpvar_29.w = 1.0;
  tmpvar_29.xyz = tmpvar_1;
  gl_FragData[0] = tmpvar_29;
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
; 16 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
def c5, 0.50000000, 4.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mul r1.xy, c5.x, r0
mul r0, r1.xyxy, c5.y
mov o4.zw, r0
mov o4.xy, -r0
mov r0.zw, c4.xyxy
mov r0.xy, c4
mov o1.xy, v1
mad o2.zw, c5.x, r0, v1.xyxy
mad o2.xy, c5.x, -r0, v1
mov o3.zw, r1.xyxy
mov o3.xy, -r1
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
Float 0 [_EdgeThresholdMin]
Float 1 [_EdgeThreshold]
Float 2 [_EdgeSharpness]
SetTexture 0 [_MainTex] 2D
"ps_3_0
; 72 ALU, 18 TEX, 1 FLOW
dcl_2d s0
def c3, 0.00000000, 0.21997070, 0.70703125, 0.07098389
def c4, 0.00260353, 1.00000000, 0.00000000, 2.00000000
def c5, -2.00000000, 0.25000000, 0.50000000, 0
dcl_texcoord0 v0.xy
dcl_texcoord1 v1
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
mov r0.z, c3.x
mov r0.xy, v1.xwzw
texldl r2.xyz, r0.xyzz, s0
dp3_pp r2.w, r2, c3.yzww
mov r0.z, c3.x
mov r0.xy, v1
texldl r0.xyz, r0.xyzz, s0
dp3_pp r3.x, r0, c3.yzww
min_pp r3.z, r3.x, r2.w
max_pp r0.w, r3.x, r2
mov r0.z, c3.x
mov r0.xy, v1.zyzw
texldl r0.xyz, r0.xyzz, s0
dp3_pp r0.x, r0, c3.yzww
mov r2.z, c3.x
mov r2.xy, v1.zwzw
texldl r2.xyz, r2.xyzz, s0
dp3_pp r2.x, r2, c3.yzww
add_pp r2.y, r0.x, c4.x
max_pp r0.x, r2.y, r2
max_pp r0.w, r0.x, r0
mul_pp r0.x, r0.w, c1
max_pp r2.z, r0.x, c0.x
min_pp r1.w, r2.y, r2.x
mov r0.z, c3.x
mov r0.xy, v0
texldl r0.xyz, r0.xyzz, s0
dp3_pp r3.y, r0, c3.yzww
min_pp r1.w, r1, r3.z
min_pp r3.z, r1.w, r3.y
max_pp r3.y, r0.w, r3
add_pp r3.y, r3, -r3.z
add_pp r2.z, r3.y, -r2
cmp_pp r1.xyz, r2.z, r1, r0
cmp_pp r0.z, r2, c4.y, c4
add_pp r0.x, -r2.y, r2.w
add_pp r0.y, r2.x, -r3.x
if_gt r0.z, c3.x
add_pp r1.x, r0, r0.y
add_pp r1.y, r0.x, -r0
mul_pp r0.xy, r1, r1
add_pp r0.x, r0, r0.y
rsq_pp r0.x, r0.x
mul_pp r3.xy, r0.x, r1
abs_pp r0.y, r3
abs_pp r0.x, r3
min_pp r0.x, r0, r0.y
mul_pp r0.x, r0, c2
rcp_pp r0.x, r0.x
mul_pp r0.xy, r3, r0.x
min_pp r0.xy, r0, c4.w
max_pp r0.xy, r0, c5.x
mul r0.xy, -r0, v3.zwzw
add r1.xy, v0, -r0
mov r1.z, c3.x
texldl r1.xyz, r1.xyzz, s0
mov r0.z, c3.x
add r0.xy, v0, r0
texldl r0.xyz, r0.xyzz, s0
add_pp r2.xyz, r0, r1
mul r0.xy, -r3, v2.zwzw
add r1.xy, -r0, v0
mov r1.z, c3.x
texldl r1.xyz, r1.xyzz, s0
mov r0.z, c3.x
add r0.xy, r0, v0
texldl r0.xyz, r0.xyzz, s0
add_pp r0.xyz, r0, r1
add_pp r1.xyz, r0, r2
mul_pp r1.xyz, r1, c5.y
dp3_pp r2.y, r1, c3.yzww
add_pp r2.y, -r2, r0.w
dp3_pp r2.x, r0, c3.yzww
add_pp r0.w, r2.x, -r1
cmp_pp r1.w, r2.y, c4.z, c4.y
cmp_pp r0.w, r0, c4.z, c4.y
mul_pp r2.xyz, r0, c5.z
add_pp_sat r0.w, r0, r1
abs_pp r0.x, r0.w
cmp_pp r1.xyz, -r0.x, r1, r2
endif
mov_pp oC0.xyz, r1
mov_pp oC0.w, c4.y
"
}
}
 }
}
Fallback Off
}