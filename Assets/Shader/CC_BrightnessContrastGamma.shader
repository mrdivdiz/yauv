Shader "Hidden/CC_BrightnessContrastGamma" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _rCoeff ("Luminance coeff (Red)", Range(0,1)) = 0.5
 _gCoeff ("Luminance coeff (Green)", Range(0,1)) = 0.5
 _bCoeff ("Luminance coeff (Blue)", Range(0,1)) = 0.5
 _brightness ("Brightness", Range(0,2)) = 0
 _contrast ("Contrast", Range(0,2)) = 1
 _gamma ("Gamma", Range(0.1,9.9)) = 1
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
# 8 ALU
PARAM c[9] = { { 0 },
		state.matrix.mvp,
		state.matrix.texture[0] };
TEMP R0;
MOV R0.zw, c[0].x;
MOV R0.xy, vertex.texcoord[0];
DP4 result.texcoord[0].y, R0, c[6];
DP4 result.texcoord[0].x, R0, c[5];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 8 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [glstate_matrix_texture0]
"vs_2_0
; 8 ALU
def c8, 0.00000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.zw, c8.x
mov r0.xy, v1
dp4 oT0.y, r0, c5
dp4 oT0.x, r0, c4
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Float 0 [_rCoeff]
Float 1 [_gCoeff]
Float 2 [_bCoeff]
Float 3 [_brightness]
Float 4 [_contrast]
Float 5 [_gamma]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 12 ALU, 1 TEX
PARAM c[6] = { program.local[0..5] };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0, fragment.texcoord[0], texture[0], 2D;
RCP R2.x, c[5].x;
MOV R1.w, R0;
MOV R1.x, c[0];
MOV R1.y, c[1].x;
MOV R1.z, c[2].x;
MAD R0, R0, c[3].x, -R1;
MAD_SAT R0, R0, c[4].x, R1;
POW result.color.x, R0.x, R2.x;
POW result.color.y, R0.y, R2.x;
POW result.color.z, R0.z, R2.x;
POW result.color.w, R0.w, R2.x;
END
# 12 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_rCoeff]
Float 1 [_gCoeff]
Float 2 [_bCoeff]
Float 3 [_brightness]
Float 4 [_contrast]
Float 5 [_gamma]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 24 ALU, 1 TEX
dcl_2d s0
dcl t0.xy
texld r1, t0, s0
rcp r3.x, c5.x
mov_pp r0.w, r1
mov_pp r0.x, c0
mov_pp r0.y, c1.x
mov_pp r0.z, c2.x
mad_pp r1, r1, c3.x, -r0
mad_sat r0, r1, c4.x, r0
pow_pp r1.x, r0.x, r3.x
pow_pp r2.x, r0.y, r3.x
mov_pp r0.x, r1.x
mov_pp r0.y, r2.x
pow_pp r2.x, r0.w, r3.x
pow_pp r1.x, r0.z, r3.x
mov_pp r0.w, r2.x
mov_pp r0.z, r1.x
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}