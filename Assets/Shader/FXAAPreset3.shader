Shader "Hidden/FXAA Preset 3" {
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
varying vec2 xlv_TEXCOORD0;

void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
  xlv_TEXCOORD0 = gl_MultiTexCoord0.xy;
}


#endif
#ifdef FRAGMENT
#extension GL_ARB_shader_texture_lod : enable
varying vec2 xlv_TEXCOORD0;
uniform vec4 _MainTex_TexelSize;
uniform sampler2D _MainTex;
void main ()
{
  vec3 tmpvar_1;
  int i;
  bool doneP;
  bool doneN;
  float lumaEndP;
  float lumaEndN;
  vec2 offNP;
  vec2 posP;
  vec2 posN;
  float gradientN;
  float lengthSign;
  float lumaS;
  float lumaN;
  doneN = bool(0);
  doneP = bool(0);
  i = 0;
  vec4 tmpvar_2;
  tmpvar_2.zw = vec2(0.0, 0.0);
  tmpvar_2.xy = (xlv_TEXCOORD0 + (vec2(0.0, -1.0) * _MainTex_TexelSize.xy));
  vec4 tmpvar_3;
  tmpvar_3 = texture2DLod (_MainTex, tmpvar_2.xy, 0.0);
  vec4 tmpvar_4;
  tmpvar_4.zw = vec2(0.0, 0.0);
  tmpvar_4.xy = (xlv_TEXCOORD0 + (vec2(-1.0, 0.0) * _MainTex_TexelSize.xy));
  vec4 tmpvar_5;
  tmpvar_5 = texture2DLod (_MainTex, tmpvar_4.xy, 0.0);
  vec4 tmpvar_6;
  tmpvar_6.zw = vec2(0.0, 0.0);
  tmpvar_6.xy = xlv_TEXCOORD0;
  vec4 tmpvar_7;
  tmpvar_7 = texture2DLod (_MainTex, tmpvar_6.xy, 0.0);
  vec4 tmpvar_8;
  tmpvar_8.zw = vec2(0.0, 0.0);
  tmpvar_8.xy = (xlv_TEXCOORD0 + (vec2(1.0, 0.0) * _MainTex_TexelSize.xy));
  vec4 tmpvar_9;
  tmpvar_9 = texture2DLod (_MainTex, tmpvar_8.xy, 0.0);
  vec4 tmpvar_10;
  tmpvar_10.zw = vec2(0.0, 0.0);
  tmpvar_10.xy = (xlv_TEXCOORD0 + (vec2(0.0, 1.0) * _MainTex_TexelSize.xy));
  vec4 tmpvar_11;
  tmpvar_11 = texture2DLod (_MainTex, tmpvar_10.xy, 0.0);
  float tmpvar_12;
  tmpvar_12 = ((tmpvar_3.y * 1.96321) + tmpvar_3.x);
  lumaN = tmpvar_12;
  float tmpvar_13;
  tmpvar_13 = ((tmpvar_5.y * 1.96321) + tmpvar_5.x);
  float tmpvar_14;
  tmpvar_14 = ((tmpvar_7.y * 1.96321) + tmpvar_7.x);
  float tmpvar_15;
  tmpvar_15 = ((tmpvar_9.y * 1.96321) + tmpvar_9.x);
  float tmpvar_16;
  tmpvar_16 = ((tmpvar_11.y * 1.96321) + tmpvar_11.x);
  lumaS = tmpvar_16;
  float tmpvar_17;
  tmpvar_17 = max (tmpvar_14, max (max (tmpvar_12, tmpvar_13), max (tmpvar_16, tmpvar_15)));
  float tmpvar_18;
  tmpvar_18 = (tmpvar_17 - min (tmpvar_14, min (min (tmpvar_12, tmpvar_13), min (tmpvar_16, tmpvar_15))));
  float tmpvar_19;
  tmpvar_19 = max (0.0416667, (tmpvar_17 * 0.125));
  if ((tmpvar_18 < tmpvar_19)) {
    tmpvar_1 = tmpvar_7.xyz;
  } else {
    float tmpvar_20;
    tmpvar_20 = min (0.75, (max (0.0, ((abs ((((((tmpvar_12 + tmpvar_13) + tmpvar_15) + tmpvar_16) * 0.25) - tmpvar_14)) / tmpvar_18) - 0.25)) * 1.33333));
    vec4 tmpvar_21;
    tmpvar_21.zw = vec2(0.0, 0.0);
    tmpvar_21.xy = (xlv_TEXCOORD0 + (vec2(-1.0, -1.0) * _MainTex_TexelSize.xy));
    vec4 tmpvar_22;
    tmpvar_22 = texture2DLod (_MainTex, tmpvar_21.xy, 0.0);
    vec4 tmpvar_23;
    tmpvar_23.zw = vec2(0.0, 0.0);
    tmpvar_23.xy = (xlv_TEXCOORD0 + (vec2(1.0, -1.0) * _MainTex_TexelSize.xy));
    vec4 tmpvar_24;
    tmpvar_24 = texture2DLod (_MainTex, tmpvar_23.xy, 0.0);
    vec4 tmpvar_25;
    tmpvar_25.zw = vec2(0.0, 0.0);
    tmpvar_25.xy = (xlv_TEXCOORD0 + (vec2(-1.0, 1.0) * _MainTex_TexelSize.xy));
    vec4 tmpvar_26;
    tmpvar_26 = texture2DLod (_MainTex, tmpvar_25.xy, 0.0);
    vec4 tmpvar_27;
    tmpvar_27.zw = vec2(0.0, 0.0);
    tmpvar_27.xy = (xlv_TEXCOORD0 + _MainTex_TexelSize.xy);
    vec4 tmpvar_28;
    tmpvar_28 = texture2DLod (_MainTex, tmpvar_27.xy, 0.0);
    vec3 tmpvar_29;
    tmpvar_29 = ((((((tmpvar_3.xyz + tmpvar_5.xyz) + tmpvar_7.xyz) + tmpvar_9.xyz) + tmpvar_11.xyz) + (((tmpvar_22.xyz + tmpvar_24.xyz) + tmpvar_26.xyz) + tmpvar_28.xyz)) * vec3(0.111111, 0.111111, 0.111111));
    float tmpvar_30;
    tmpvar_30 = ((tmpvar_22.y * 1.96321) + tmpvar_22.x);
    float tmpvar_31;
    tmpvar_31 = ((tmpvar_24.y * 1.96321) + tmpvar_24.x);
    float tmpvar_32;
    tmpvar_32 = ((tmpvar_26.y * 1.96321) + tmpvar_26.x);
    float tmpvar_33;
    tmpvar_33 = ((tmpvar_28.y * 1.96321) + tmpvar_28.x);
    bool tmpvar_34;
    tmpvar_34 = (((abs ((((0.25 * tmpvar_30) + (-(0.5) * tmpvar_13)) + (0.25 * tmpvar_32))) + abs ((((0.5 * tmpvar_12) + (-(1.0) * tmpvar_14)) + (0.5 * tmpvar_16)))) + abs ((((0.25 * tmpvar_31) + (-(0.5) * tmpvar_15)) + (0.25 * tmpvar_33)))) >= ((abs ((((0.25 * tmpvar_30) + (-(0.5) * tmpvar_12)) + (0.25 * tmpvar_31))) + abs ((((0.5 * tmpvar_13) + (-(1.0) * tmpvar_14)) + (0.5 * tmpvar_15)))) + abs ((((0.25 * tmpvar_32) + (-(0.5) * tmpvar_16)) + (0.25 * tmpvar_33)))));
    float tmpvar_35;
    if (tmpvar_34) {
      tmpvar_35 = -(_MainTex_TexelSize.y);
    } else {
      tmpvar_35 = -(_MainTex_TexelSize.x);
    };
    lengthSign = tmpvar_35;
    if (!(tmpvar_34)) {
      lumaN = tmpvar_13;
    };
    if (!(tmpvar_34)) {
      lumaS = tmpvar_15;
    };
    float tmpvar_36;
    tmpvar_36 = abs ((lumaN - tmpvar_14));
    gradientN = tmpvar_36;
    float tmpvar_37;
    tmpvar_37 = abs ((lumaS - tmpvar_14));
    lumaN = ((lumaN + tmpvar_14) * 0.5);
    float tmpvar_38;
    tmpvar_38 = ((lumaS + tmpvar_14) * 0.5);
    lumaS = tmpvar_38;
    bool tmpvar_39;
    tmpvar_39 = (tmpvar_36 >= tmpvar_37);
    if (!(tmpvar_39)) {
      lumaN = tmpvar_38;
    };
    if (!(tmpvar_39)) {
      gradientN = tmpvar_37;
    };
    if (!(tmpvar_39)) {
      lengthSign = (tmpvar_35 * -1.0);
    };
    float tmpvar_40;
    if (tmpvar_34) {
      tmpvar_40 = 0.0;
    } else {
      tmpvar_40 = (lengthSign * 0.5);
    };
    posN.x = (xlv_TEXCOORD0.x + tmpvar_40);
    float tmpvar_41;
    if (tmpvar_34) {
      tmpvar_41 = (lengthSign * 0.5);
    } else {
      tmpvar_41 = 0.0;
    };
    posN.y = (xlv_TEXCOORD0.y + tmpvar_41);
    gradientN = (gradientN * 0.25);
    posP = posN;
    vec2 tmpvar_42;
    if (tmpvar_34) {
      vec2 tmpvar_43;
      tmpvar_43.y = 0.0;
      tmpvar_43.x = _MainTex_TexelSize.x;
      tmpvar_42 = tmpvar_43;
    } else {
      vec2 tmpvar_44;
      tmpvar_44.x = 0.0;
      tmpvar_44.y = _MainTex_TexelSize.y;
      tmpvar_42 = tmpvar_44;
    };
    offNP = tmpvar_42;
    lumaEndN = lumaN;
    lumaEndP = lumaN;
    posN = (posN + (tmpvar_42 * vec2(-1.0, -1.0)));
    posP = (posP + tmpvar_42);
    while (true) {
      if ((i >= 16)) {
        break;
      };
      if (!(doneN)) {
        vec4 tmpvar_45;
        tmpvar_45.zw = vec2(0.0, 0.0);
        tmpvar_45.xy = posN;
        vec4 tmpvar_46;
        tmpvar_46 = texture2DLod (_MainTex, tmpvar_45.xy, 0.0);
        lumaEndN = ((tmpvar_46.y * 1.96321) + tmpvar_46.x);
      };
      if (!(doneP)) {
        vec4 tmpvar_47;
        tmpvar_47.zw = vec2(0.0, 0.0);
        tmpvar_47.xy = posP;
        vec4 tmpvar_48;
        tmpvar_48 = texture2DLod (_MainTex, tmpvar_47.xy, 0.0);
        lumaEndP = ((tmpvar_48.y * 1.96321) + tmpvar_48.x);
      };
      bool tmpvar_49;
      if (doneN) {
        tmpvar_49 = bool(1);
      } else {
        tmpvar_49 = (abs ((lumaEndN - lumaN)) >= gradientN);
      };
      doneN = tmpvar_49;
      bool tmpvar_50;
      if (doneP) {
        tmpvar_50 = bool(1);
      } else {
        tmpvar_50 = (abs ((lumaEndP - lumaN)) >= gradientN);
      };
      doneP = tmpvar_50;
      bool tmpvar_51;
      if (tmpvar_49) {
        tmpvar_51 = tmpvar_50;
      } else {
        tmpvar_51 = bool(0);
      };
      if (tmpvar_51) {
        break;
      };
      if (!(tmpvar_49)) {
        posN = (posN - offNP);
      };
      if (!(tmpvar_50)) {
        posP = (posP + offNP);
      };
      i = (i + 1);
    };
    float tmpvar_52;
    if (tmpvar_34) {
      tmpvar_52 = (xlv_TEXCOORD0.x - posN.x);
    } else {
      tmpvar_52 = (xlv_TEXCOORD0.y - posN.y);
    };
    float tmpvar_53;
    if (tmpvar_34) {
      tmpvar_53 = (posP.x - xlv_TEXCOORD0.x);
    } else {
      tmpvar_53 = (posP.y - xlv_TEXCOORD0.y);
    };
    bool tmpvar_54;
    tmpvar_54 = (tmpvar_52 < tmpvar_53);
    float tmpvar_55;
    if (tmpvar_54) {
      tmpvar_55 = lumaEndN;
    } else {
      tmpvar_55 = lumaEndP;
    };
    lumaEndN = tmpvar_55;
    if ((((tmpvar_14 - lumaN) < 0.0) == ((tmpvar_55 - lumaN) < 0.0))) {
      lengthSign = 0.0;
    };
    float tmpvar_56;
    tmpvar_56 = (tmpvar_53 + tmpvar_52);
    float tmpvar_57;
    if (tmpvar_54) {
      tmpvar_57 = tmpvar_52;
    } else {
      tmpvar_57 = tmpvar_53;
    };
    float tmpvar_58;
    tmpvar_58 = ((0.5 + (tmpvar_57 * (-1.0 / tmpvar_56))) * lengthSign);
    float tmpvar_59;
    if (tmpvar_34) {
      tmpvar_59 = 0.0;
    } else {
      tmpvar_59 = tmpvar_58;
    };
    float tmpvar_60;
    if (tmpvar_34) {
      tmpvar_60 = tmpvar_58;
    } else {
      tmpvar_60 = 0.0;
    };
    vec2 tmpvar_61;
    tmpvar_61.x = (xlv_TEXCOORD0.x + tmpvar_59);
    tmpvar_61.y = (xlv_TEXCOORD0.y + tmpvar_60);
    vec4 tmpvar_62;
    tmpvar_62.zw = vec2(0.0, 0.0);
    tmpvar_62.xy = tmpvar_61;
    vec4 tmpvar_63;
    tmpvar_63 = texture2DLod (_MainTex, tmpvar_62.xy, 0.0);
    vec3 tmpvar_64;
    tmpvar_64.x = -(tmpvar_20);
    tmpvar_64.y = -(tmpvar_20);
    tmpvar_64.z = -(tmpvar_20);
    tmpvar_1 = ((tmpvar_64 * tmpvar_63.xyz) + ((tmpvar_29 * vec3(tmpvar_20)) + tmpvar_63.xyz));
  };
  vec4 tmpvar_65;
  tmpvar_65.w = 0.0;
  tmpvar_65.xyz = tmpvar_1;
  gl_FragData[0] = tmpvar_65;
}


#endif
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
"vs_3_0
; 5 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_position0 v0
dcl_texcoord0 v1
mov o1.xy, v1
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
; 183 ALU, 24 TEX, 8 FLOW
dcl_2d s0
def c1, 0.00000000, -1.00000000, 1.00000000, 1.96321082
def c2, 0.12500000, 0.04166667, 0.11111111, 0.25000000
def c3, -0.25000000, 1.33333337, 0.75000000, -0.50000000
def c4, 0.50000000, 0, 0, 0
defi i0, 16, 0, 1, 0
dcl_texcoord0 v0.xy
mov r0.xy, c0
mov r2.xy, c0
mov r0.z, c1.x
mad r0.xy, c1.yxzw, r0, v0
texldl r1.xyz, r0.xyzz, s0
mov r0.xy, c0
mad r1.w, r1.y, c1, r1.x
mov r2.z, c1.x
mad r2.xy, c1.xzzw, r2, v0
texldl r4.xyz, r2.xyzz, s0
mov r2.xy, c0
mov r2.z, c1.x
mad r2.xy, c1.zxzw, r2, v0
texldl r3.xyz, r2.xyzz, s0
mad r4.w, r4.y, c1, r4.x
mad r3.w, r3.y, c1, r3.x
min r6.y, r3.w, r4.w
mov r0.z, c1.x
mad r0.xy, c1, r0, v0
texldl r0.xyz, r0.xyzz, s0
mad r0.w, r0.y, c1, r0.x
min r6.x, r0.w, r1.w
min r6.x, r6, r6.y
max r2.w, r0, r1
max r5.w, r3, r4
max r5.w, r2, r5
mov r2.z, c1.x
mov r2.xy, v0
texldl r2.xyz, r2.xyzz, s0
mad r2.w, r2.y, c1, r2.x
max r5.w, r2, r5
mul r6.y, r5.w, c2.x
min r6.x, r2.w, r6
max r6.y, r6, c2
add r5.w, r5, -r6.x
add r6.x, r5.w, -r6.y
cmp_pp r6.y, r6.x, c1.z, c1.x
cmp r5.xyz, r6.x, r5, r2
if_gt r6.y, c1.x
add r0.xyz, r0, r1
add r0.xyz, r0, r2
add r0.xyz, r0, r3
mad r9.w, r1, c4.x, -r2
mov r5.xy, c0
add r0.xyz, r0, r4
mad r5.xy, c1.yzzw, r5, v0
mov r5.z, c1.x
texldl r7.xyz, r5.xyzz, s0
mul r5.x, r4.w, c3.w
mad r6.w, r7.y, c1, r7.x
mad r8.w, r6, c2, r5.x
mad r9.w, r3, c4.x, r9
mov r5.z, c1.x
add r5.xy, v0, c0
texldl r8.xyz, r5.xyzz, s0
mov r5.xy, c0
mad r5.xy, c1.zyzw, r5, v0
mov r5.z, c1.x
texldl r6.xyz, r5.xyzz, s0
mul r5.x, r3.w, c3.w
mad r7.w, r6.y, c1, r6.x
mad r5.y, r7.w, c2.w, r5.x
mad r5.x, r8.y, c1.w, r8
mad r8.w, r5.x, c2, r8
mad r9.x, r5, c2.w, r5.y
mul r9.z, r0.w, c3.w
mov r2.y, c0
mov r5.z, c1.x
add r5.xy, v0, -c0
texldl r5.xyz, r5.xyzz, s0
mad r9.y, r5, c1.w, r5.x
add r1.xyz, r5, r6
mad r9.z, r9.y, c2.w, r9
mad r7.w, r7, c2, r9.z
add r1.xyz, r1, r7
add r1.xyz, r1, r8
add r0.xyz, r0, r1
abs r9.z, r9.w
abs r7.w, r7
add r7.w, r7, r9.z
abs r8.w, r8
add r8.w, r7, r8
abs r7.w, r9.x
mul r9.x, r1.w, c3.w
mad r9.x, r9.y, c2.w, r9
mad r9.z, r0.w, c4.x, -r2.w
mad r6.w, r6, c2, r9.x
mad r9.y, r4.w, c4.x, r9.z
abs r9.x, r9.y
abs r6.w, r6
add r6.w, r6, r9.x
add r7.w, r6, r7
add r6.w, r7, -r8
cmp r6.w, r6, c1.z, c1.x
abs_pp r9.x, r6.w
cmp r9.z, -r9.x, r3.w, r4.w
cmp r9.x, -r9, r1.w, r0.w
mul r1.xyz, r0, c2.z
add r9.w, -r2, r9.z
add r9.y, -r2.w, r9.x
add r0.y, r2.w, r9.x
add r0.x, r2.w, r9.z
add r3.z, r7.w, -r8.w
abs r9.w, r9
abs r9.y, r9
add r10.x, r9.y, -r9.w
cmp r2.x, r10, c1.z, c1
abs_pp r0.z, r2.x
mul r0.y, r0, c4.x
mul r0.x, r0, c4
cmp r0.x, -r0.z, r0, r0.y
add r0.y, r0.w, r1.w
add r0.y, r0, r3.w
add r0.y, r0, r4.w
cmp r1.w, r3.z, -c0.y, -c0.x
cmp r1.w, -r0.z, -r1, r1
cmp r0.z, -r0, r9.w, r9.y
mad r0.y, r0, c2.w, -r2.w
mul r2.x, r1.w, c4
mul r2.z, r0, c2.w
cmp r0.z, r3, r2.x, c1.x
add r4.y, v0, r0.z
cmp r2.x, r3.z, c1, r2
add r4.x, v0, r2
mov r2.x, c1
mov r3.x, c0
mov r3.y, c1.x
cmp r3.xy, r3.z, r3, r2
add r2.xy, r4, -r3
add r3.zw, r4.xyxy, r3.xyxy
rcp r0.z, r5.w
abs r0.y, r0
mad r0.y, r0, r0.z, c3.x
max r0.y, r0, c1.x
mul r0.y, r0, c3
mov r0.w, r0.x
mov r4.y, r0.x
mov r4.z, r0.x
min r4.x, r0.y, c3.z
mov_pp r4.w, c1.x
mov_pp r5.x, c1
loop aL, i0
if_eq r4.w, c1.x
mov r0.z, c1.x
mov r0.xy, r2
texldl r0.xy, r0.xyzz, s0
mad r4.y, r0, c1.w, r0.x
endif
if_eq r5.x, c1.x
mov r0.z, c1.x
mov r0.xy, r3.zwzw
texldl r0.xy, r0.xyzz, s0
mad r4.z, r0.y, c1.w, r0.x
endif
add r0.y, -r0.w, r4.z
add r0.x, r4.y, -r0.w
abs r0.y, r0
abs r0.x, r0
add r0.y, -r2.z, r0
add r0.x, r0, -r2.z
cmp r0.x, r0, c1.z, c1
cmp r0.y, r0, c1.z, c1.x
add_pp_sat r5.x, r5, r0.y
add_pp_sat r4.w, r4, r0.x
mul_pp r0.x, r4.w, r5
break_gt r0.x, c1.x
add r0.xy, r2, -r3
abs_pp r0.z, r4.w
cmp r2.xy, -r0.z, r0, r2
add r0.xy, r3, r3.zwzw
abs_pp r0.z, r5.x
cmp r3.zw, -r0.z, r0.xyxy, r3
endloop
add r0.y, -v0.x, r3.z
add r0.x, -v0.y, r3.w
cmp r0.z, -r6.w, r0.x, r0.y
add r0.y, v0.x, -r2.x
add r0.x, v0.y, -r2.y
cmp r0.x, -r6.w, r0, r0.y
add r0.y, r0.x, -r0.z
cmp r2.x, r0.y, r4.z, r4.y
add r2.x, -r0.w, r2
cmp r0.y, r0, r0.z, r0.x
add r0.z, r0.x, r0
rcp r0.z, r0.z
mad r0.y, r0, -r0.z, c4.x
add r0.w, r2, -r0
cmp r2.x, r2, c1, c1.z
cmp r0.w, r0, c1.x, c1.z
add_pp r0.w, r0, -r2.x
abs_pp r0.x, r0.w
cmp r0.x, -r0, c1, r1.w
mul r0.y, r0, r0.x
cmp r0.x, -r6.w, r0.y, c1
cmp r0.y, -r6.w, c1.x, r0
add r0.x, v0, r0
mov r0.z, c1.x
add r0.y, v0, r0
texldl r0.xyz, r0.xyzz, s0
mad r1.xyz, r4.x, r1, r0
mad r5.xyz, -r4.x, r0, r1
endif
mov oC0.xyz, r5
mov oC0.w, c1.x
"
}
}
 }
}
Fallback "Hidden/FXAA II"
}