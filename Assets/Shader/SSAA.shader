Shader "Hidden/SSAA" {
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
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_TexelSize]
"!!ARBvp1.0
# 13 ALU
PARAM c[6] = { { 0, 1.75 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
MOV R0.w, c[5].y;
MOV R0.z, c[0].x;
MOV R0.y, c[0].x;
MOV R0.x, c[5];
MAD result.texcoord[0].xy, -R0.zwzw, c[0].y, vertex.texcoord[0];
MAD result.texcoord[1].xy, -R0, c[0].y, vertex.texcoord[0];
MAD result.texcoord[2].xy, R0, c[0].y, vertex.texcoord[0];
MAD result.texcoord[3].xy, R0.zwzw, c[0].y, vertex.texcoord[0];
MOV result.texcoord[4].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 13 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 13 ALU
def c5, 0.00000000, 1.75000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.w, c4.y
mov r0.z, c5.x
mov r0.y, c5.x
mov r0.x, c4
mad oT0.xy, -r0.zwzw, c5.y, v1
mad oT1.xy, -r0, c5.y, v1
mad oT2.xy, r0, c5.y, v1
mad oT3.xy, r0.zwzw, c5.y, v1
mov oT4.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 40 ALU, 9 TEX
PARAM c[4] = { program.local[0],
		{ 0.2325159, 1, 0, 0.0625 },
		{ 0.2199707, 0.70703125, 0.070983887, 0.5 },
		{ 0.89999998, 0.75 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R1.xyz, fragment.texcoord[3], texture[0], 2D;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R2.xyz, fragment.texcoord[2], texture[0], 2D;
TEX R3.xyz, fragment.texcoord[1], texture[0], 2D;
DP3 R0.x, R0, c[2];
DP3 R0.w, R1, c[2];
ADD R0.x, R0, -R0.w;
DP3 R0.z, R3, c[2];
DP3 R0.y, R2, c[2];
ADD R0.y, R0, -R0.z;
MOV R0.x, -R0;
MUL R0.zw, R0.xyxy, R0.xyxy;
ADD R0.z, R0, R0.w;
RSQ R5.x, R0.z;
MUL R0.zw, R5.x, c[0].xyxy;
MUL R1.xy, R0, R0.zwzw;
MUL R0.zw, R1.xyxy, c[2].w;
ADD R1.zw, fragment.texcoord[4].xyxy, -R1.xyxy;
ADD R0.xy, fragment.texcoord[4], R0.zwzw;
ADD R0.zw, fragment.texcoord[4].xyxy, -R0;
ADD R1.xy, fragment.texcoord[4], R1;
TEX R3, R1, texture[0], 2D;
TEX R4, R1.zwzw, texture[0], 2D;
TEX R2, R0.zwzw, texture[0], 2D;
TEX R1, R0, texture[0], 2D;
TEX R0, fragment.texcoord[4], texture[0], 2D;
MUL R1, R1, c[3].x;
MUL R2, R2, c[3].x;
ADD R1, R0, R1;
ADD R1, R1, R2;
MUL R3, R3, c[3].y;
ADD R1, R1, R3;
MUL R2, R4, c[3].y;
ADD R1, R1, R2;
RCP R3.x, R5.x;
SLT R3.x, R3, c[1].w;
ABS R2.x, R3;
MUL R1, R1, c[1].x;
CMP R2.x, -R2, c[1].z, c[1].y;
CMP result.color, -R2.x, R1, R0;
END
# 40 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 32 ALU, 9 TEX
dcl_2d s0
def c1, 0.21997070, 0.70703125, 0.07098389, -0.06250000
def c2, 0.00000000, 1.00000000, 0.50000000, 0.89999998
def c3, 0.75000000, 0.23251590, 0, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
dcl t4.xy
texld r1, t2, s0
texld r0, t1, s0
texld r2, t3, s0
texld r3, t0, s0
texld r5, t4, s0
dp3_pp r1.x, r1, c1
dp3_pp r0.x, r0, c1
dp3_pp r2.x, r2, c1
dp3_pp r3.x, r3, c1
add r1.y, r1.x, -r0.x
add r2.x, r3, -r2
mov_pp r1.x, -r2
mul_pp r0.xy, r1, r1
add_pp r0.x, r0, r0.y
rsq_pp r0.x, r0.x
mul r2.xy, r0.x, c0
mul_pp r1.xy, r1, r2
mul_pp r4.xy, r1, c2.z
add r2.xy, t4, r1
add r3.xy, t4, -r4
add r1.xy, t4, -r1
add r4.xy, t4, r4
rcp_pp r0.x, r0.x
add r0.x, r0, c1.w
cmp r0.x, r0, c2, c2.y
abs_pp r0.x, r0
texld r1, r1, s0
texld r4, r4, s0
texld r3, r3, s0
texld r2, r2, s0
mul r4, r4, c2.w
mul r2, r2, c3.x
mul r3, r3, c2.w
add_pp r4, r5, r4
add_pp r3, r4, r3
add_pp r2, r3, r2
mul r1, r1, c3.x
add_pp r1, r2, r1
mul_pp r1, r1, c3.y
cmp_pp r0, -r0.x, r1, r5
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}