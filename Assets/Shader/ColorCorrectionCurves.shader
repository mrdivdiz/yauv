Shader "Hidden/ColorCorrectionCurves" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _RgbTex ("_RgbTex (RGB)", 2D) = "" {}
 _ZCurve ("_ZCurve (RGB)", 2D) = "" {}
 _RgbDepthTex ("_RgbDepthTex (RGB)", 2D) = "" {}
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
Vector 5 [_CameraDepthTexture_ST]
"!!ARBvp1.0
# 6 ALU
PARAM c[6] = { program.local[0],
		state.matrix.mvp,
		program.local[5] };
MOV result.texcoord[0].xy, vertex.texcoord[0];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_CameraDepthTexture_ST]
Vector 5 [_MainTex_TexelSize]
"vs_2_0
; 15 ALU
def c6, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c6
slt r0.x, c5.y, r0
max r0.x, -r0, r0
slt r0.z, c6.x, r0.x
mad r0.xy, v1, c4, c4.zwzw
add r0.w, -r0.z, c6.y
mul r0.w, r0.y, r0
add r0.y, -r0, c6
mad oT1.y, r0.z, r0, r0.w
mov oT0.xy, v1
mov oT1.x, r0
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_RgbTex] 2D
SetTexture 2 [_CameraDepthTexture] 2D
SetTexture 3 [_ZCurve] 2D
SetTexture 4 [_RgbDepthTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 31 ALU, 9 TEX
PARAM c[3] = { program.local[0],
		{ 0.125, 1, 0, 0.375 },
		{ 0.625, 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1.x, fragment.texcoord[1], texture[2], 2D;
MOV R2.x, R0.y;
MAD R0.y, R1.x, c[0].x, c[0];
MOV R1.x, R0;
RCP R0.x, R0.y;
MOV R2.y, c[1].w;
MOV R1.y, c[1].x;
MOV R0.y, c[2];
MOV R3.x, R0.z;
MOV R3.y, c[2].x;
MOV result.color.w, R0;
TEX R4.xyz, R1, texture[4], 2D;
TEX R5.xyz, R3, texture[4], 2D;
TEX R6.xyz, R2, texture[4], 2D;
TEX R1.xyz, R1, texture[1], 2D;
TEX R2.xyz, R2, texture[1], 2D;
TEX R3.xyz, R3, texture[1], 2D;
TEX R0.x, R0, texture[3], 2D;
MUL R3.xyz, R3, c[1].zzyw;
MUL R2.xyz, R2, c[1].zyzw;
MUL R1.xyz, R1, c[1].yzzw;
ADD R1.xyz, R1, R2;
ADD R1.xyz, R1, R3;
MUL R2.xyz, R6, c[1].zyzw;
MUL R3.xyz, R5, c[1].zzyw;
MUL R4.xyz, R4, c[1].yzzw;
ADD R3.xyz, R4, R3;
ADD R2.xyz, R3, R2;
ADD R2.xyz, R2, -R1;
MAD result.color.xyz, R0.x, R2, R1;
END
# 31 instructions, 7 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_RgbTex] 2D
SetTexture 2 [_CameraDepthTexture] 2D
SetTexture 3 [_ZCurve] 2D
SetTexture 4 [_RgbDepthTex] 2D
"ps_2_0
; 35 ALU, 9 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
def c1, 0.12500000, 1.00000000, 0.00000000, 0.37500000
def c2, 0.62500000, 0.50000000, 0, 0
dcl t0.xy
dcl t1.xy
texld r2, t1, s2
texld r6, t0, s0
mad r3.x, r2, c0, c0.y
mov_pp r0.x, r6.z
mov_pp r0.y, c2.x
mov_pp r1.x, r6.y
mov_pp r2.x, r6
mov_pp r2.y, c1.x
rcp r3.x, r3.x
mov_pp r3.y, c2
mov_pp r1.y, c1.w
mov r6.z, c1.y
mov r6.xy, c1.z
texld r7, r3, s3
texld r5, r2, s4
texld r4, r0, s4
texld r3, r1, s4
texld r0, r0, s1
texld r2, r2, s1
texld r1, r1, s1
mul r6.xyz, r0, r6
mov r0.xz, c1.z
mov r0.y, c1
mul r1.xyz, r1, r0
mov r0.yz, c1.z
mov r0.x, c1.y
mul r0.xyz, r2, r0
add_pp r0.xyz, r0, r1
add_pp r1.xyz, r0, r6
mov r0.xz, c1.z
mov r0.y, c1
mul r2.xyz, r3, r0
mov r0.xy, c1.z
mov r0.z, c1.y
mul r3.xyz, r4, r0
mov r0.yz, c1.z
mov r0.x, c1.y
mul r0.xyz, r5, r0
add_pp r0.xyz, r0, r3
add_pp r0.xyz, r0, r2
add_pp r0.xyz, r0, -r1
mov_pp r0.w, r6
mad_pp r0.xyz, r7.x, r0, r1
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}