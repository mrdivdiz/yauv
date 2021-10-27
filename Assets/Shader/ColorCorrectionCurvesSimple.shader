Shader "Hidden/ColorCorrectionCurvesSimple" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _RgbTex ("_RgbTex (RGB)", 2D) = "" {}
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
# 5 ALU
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
"vs_2_0
; 5 ALU
dcl_position0 v0
dcl_texcoord0 v1
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_RgbTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 15 ALU, 4 TEX
PARAM c[2] = { { 0.125, 1, 0, 0.375 },
		{ 0.625 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MOV R1.x, R0.y;
MOV R1.z, R0;
MOV R0.y, c[0].x;
MOV R1.w, c[1].x;
MOV R1.y, c[0].w;
MOV result.color.w, R0;
TEX R2.xyz, R1.zwzw, texture[1], 2D;
TEX R0.xyz, R0, texture[1], 2D;
TEX R1.xyz, R1, texture[1], 2D;
MUL R2.xyz, R2, c[0].zzyw;
MUL R1.xyz, R1, c[0].zyzw;
MUL R0.xyz, R0, c[0].yzzw;
ADD R0.xyz, R0, R1;
ADD result.color.xyz, R0, R2;
END
# 15 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_RgbTex] 2D
"ps_2_0
; 19 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
def c0, 0.12500000, 1.00000000, 0.00000000, 0.37500000
def c1, 0.62500000, 0, 0, 0
dcl t0.xy
texld r3, t0, s0
mov_pp r1.x, r3.y
mov_pp r1.y, c0.w
mov_pp r2.x, r3
mov_pp r0.x, r3.z
mov_pp r2.y, c0.x
mov_pp r0.y, c1.x
mov r3.y, c0
mov r3.xz, c0.z
texld r0, r0, s1
texld r1, r1, s1
texld r2, r2, s1
mul r3.xyz, r1, r3
mov r1.yz, c0.z
mov r1.x, c0.y
mul r1.xyz, r2, r1
add_pp r2.xyz, r1, r3
mov r1.z, c0.y
mov r1.xy, c0.z
mul r0.xyz, r0, r1
mov_pp r0.w, r3
add_pp r0.xyz, r2, r0
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}