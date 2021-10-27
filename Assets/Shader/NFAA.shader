Shader "Hidden/NFAA" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _BlurTex ("Base (RGB)", 2D) = "white" {}
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
Float 6 [_OffsetScale]
"!!ARBvp1.0
# 18 ALU
PARAM c[7] = { { 0 },
		state.matrix.mvp,
		program.local[5..6] };
TEMP R0;
TEMP R1;
MOV R0.x, c[0];
MOV R0.y, c[5];
MOV R0.w, c[0].x;
MOV R0.z, c[5].x;
MAD R1.xy, -R0.zwzw, c[6].x, vertex.texcoord[0];
MAD R0.zw, R0, c[6].x, vertex.texcoord[0].xyxy;
MAD result.texcoord[4].xy, R0, c[6].x, R1;
MAD result.texcoord[5].xy, -R0, c[6].x, R1;
MAD result.texcoord[6].xy, R0, c[6].x, R0.zwzw;
MAD result.texcoord[7].xy, -R0, c[6].x, R0.zwzw;
MAD result.texcoord[0].xy, R0, c[6].x, vertex.texcoord[0];
MAD result.texcoord[1].xy, -R0, c[6].x, vertex.texcoord[0];
MOV result.texcoord[2].xy, R0.zwzw;
MOV result.texcoord[3].xy, R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 18 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
Float 5 [_OffsetScale]
"vs_2_0
; 18 ALU
def c6, 0.00000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c6
mov r0.y, c4
mov r0.w, c6.x
mov r0.z, c4.x
mad r1.xy, -r0.zwzw, c5.x, v1
mad r0.zw, r0, c5.x, v1.xyxy
mad oT4.xy, r0, c5.x, r1
mad oT5.xy, -r0, c5.x, r1
mad oT6.xy, r0, c5.x, r0.zwzw
mad oT7.xy, -r0, c5.x, r0.zwzw
mad oT0.xy, r0, c5.x, v1
mad oT1.xy, -r0, c5.x, v1
mov oT2.xy, r0.zwzw
mov oT3.xy, r1
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
Float 1 [_BlurRadius]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 48 ALU, 13 TEX
PARAM c[4] = { program.local[0..1],
		{ 0.5, 1, 0.2 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEX R0.xyz, fragment.texcoord[7], texture[0], 2D;
TEX R2.xyz, fragment.texcoord[4], texture[0], 2D;
TEX R5.xyz, fragment.texcoord[6], texture[0], 2D;
TEX R3.xyz, fragment.texcoord[5], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[1], texture[0], 2D;
TEX R4.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R7.xyz, fragment.texcoord[2], texture[0], 2D;
TEX R6.xyz, fragment.texcoord[3], texture[0], 2D;
DP3 R3.x, R3, c[3];
DP3 R2.x, R2, c[3];
DP3 R0.x, R0, c[3];
DP3 R5.x, R5, c[3];
DP3 R3.y, R4, c[3];
MOV R3.z, R5.x;
DP3 R0.w, R3, c[2].y;
MOV R0.z, R2.x;
DP3 R0.y, R1, c[3];
DP3 R0.y, R0, c[2].y;
ADD R0.z, R0.y, -R0.w;
MOV R0.w, c[1].x;
MOV R2.z, R3.x;
DP3 R2.y, R7, c[3];
DP3 R0.y, R2, c[2].y;
MUL R1.xy, R0.w, c[0];
MOV R5.z, R0.x;
DP3 R5.y, R6, c[3];
DP3 R0.x, R5, c[2].y;
ADD R0.w, R0.x, -R0.y;
MUL R1.xy, R0.zwzw, R1;
MOV R0.xy, fragment.texcoord[1];
ADD R0.xy, fragment.texcoord[0], R0;
MUL R0.xy, R0, c[2].x;
MOV R0.z, R1.x;
MOV R0.w, -R1.y;
ADD R1.zw, R0.xyxy, R0;
ADD R2.xy, R0, -R0.zwzw;
ADD R0.zw, R0.xyxy, R1.xyxy;
ADD R1.xy, R0, -R1;
TEX R4, R2, texture[0], 2D;
TEX R3, R1.zwzw, texture[0], 2D;
TEX R2, R1, texture[0], 2D;
TEX R1, R0.zwzw, texture[0], 2D;
TEX R0, R0, texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
ADD R0, R0, R4;
MUL result.color, R0, c[2].z;
END
# 48 instructions, 8 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
Float 1 [_BlurRadius]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 36 ALU, 13 TEX
dcl_2d s0
def c2, 0.21997070, 0.70703125, 0.07098389, 1.00000000
def c3, 0.50000000, 0.20000000, 0, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
dcl t4.xy
dcl t5.xy
dcl t6.xy
dcl t7.xy
texld r1, t3, s0
texld r0, t2, s0
texld r2, t1, s0
texld r3, t4, s0
texld r4, t7, s0
texld r5, t0, s0
texld r7, t5, s0
texld r6, t6, s0
dp3_pp r4.x, r4, c2
dp3_pp r3.x, r3, c2
dp3_pp r7.x, r7, c2
dp3_pp r6.x, r6, c2
dp3_pp r4.y, r2, c2
mov r4.z, r3.x
dp3_pp r3.y, r0, c2
mov r3.z, r7.x
dp3_pp r6.y, r1, c2
mov r6.z, r4.x
dp3 r2.x, r4, c2.w
dp3 r0.x, r3, c2.w
dp3 r1.x, r6, c2.w
add r2.y, r1.x, -r0.x
mov r4.xy, c0
mov r1.xy, t1
dp3_pp r7.y, r5, c2
mov r7.z, r6.x
dp3 r5.x, r7, c2.w
add r2.x, r2, -r5
mul r3.xy, c1.x, r4
mul r0.xy, r2, r3
add r1.xy, t0, r1
mul r4.xy, r1, c3.x
add r3.xy, r4, r0
mov r2.x, r0
mov r2.y, -r0
add r1.xy, r4, r2
add r5.xy, r4, -r2
add r2.xy, r4, -r0
texld r0, r5, s0
texld r1, r1, s0
texld r3, r3, s0
texld r2, r2, s0
texld r4, r4, s0
add r3, r4, r3
add r2, r3, r2
add r1, r2, r1
add r0, r1, r0
mul r0, r0, c3.y
mov_pp oC0, r0
"
}
}
 }
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
Float 6 [_OffsetScale]
"!!ARBvp1.0
# 18 ALU
PARAM c[7] = { { 0 },
		state.matrix.mvp,
		program.local[5..6] };
TEMP R0;
TEMP R1;
MOV R0.x, c[0];
MOV R0.y, c[5];
MOV R0.w, c[0].x;
MOV R0.z, c[5].x;
MAD R1.xy, -R0.zwzw, c[6].x, vertex.texcoord[0];
MAD R0.zw, R0, c[6].x, vertex.texcoord[0].xyxy;
MAD result.texcoord[4].xy, R0, c[6].x, R1;
MAD result.texcoord[5].xy, -R0, c[6].x, R1;
MAD result.texcoord[6].xy, R0, c[6].x, R0.zwzw;
MAD result.texcoord[7].xy, -R0, c[6].x, R0.zwzw;
MAD result.texcoord[0].xy, R0, c[6].x, vertex.texcoord[0];
MAD result.texcoord[1].xy, -R0, c[6].x, vertex.texcoord[0];
MOV result.texcoord[2].xy, R0.zwzw;
MOV result.texcoord[3].xy, R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 18 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
Float 5 [_OffsetScale]
"vs_2_0
; 18 ALU
def c6, 0.00000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c6
mov r0.y, c4
mov r0.w, c6.x
mov r0.z, c4.x
mad r1.xy, -r0.zwzw, c5.x, v1
mad r0.zw, r0, c5.x, v1.xyxy
mad oT4.xy, r0, c5.x, r1
mad oT5.xy, -r0, c5.x, r1
mad oT6.xy, r0, c5.x, r0.zwzw
mad oT7.xy, -r0, c5.x, r0.zwzw
mad oT0.xy, r0, c5.x, v1
mad oT1.xy, -r0, c5.x, v1
mov oT2.xy, r0.zwzw
mov oT3.xy, r1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Float 1 [_BlurRadius]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 33 ALU, 8 TEX
PARAM c[4] = { program.local[0..1],
		{ 0.2199707, 0.70703125, 0.070983887, 1 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEX R0.xyz, fragment.texcoord[7], texture[0], 2D;
TEX R2.xyz, fragment.texcoord[4], texture[0], 2D;
TEX R5.xyz, fragment.texcoord[6], texture[0], 2D;
TEX R3.xyz, fragment.texcoord[5], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[1], texture[0], 2D;
TEX R6.xyz, fragment.texcoord[3], texture[0], 2D;
TEX R7.xyz, fragment.texcoord[2], texture[0], 2D;
TEX R4.xyz, fragment.texcoord[0], texture[0], 2D;
DP3 R0.x, R0, c[2];
DP3 R5.x, R5, c[2];
DP3 R3.x, R3, c[2];
DP3 R2.x, R2, c[2];
DP3 R0.y, R1, c[2];
MOV R0.z, R2.x;
DP3 R0.y, R0, c[2].w;
MOV R3.z, R5.x;
DP3 R3.y, R4, c[2];
DP3 R0.w, R3, c[2].w;
ADD R1.x, R0.y, -R0.w;
MOV R2.z, R3.x;
DP3 R2.y, R7, c[2];
DP3 R0.y, R2, c[2].w;
MOV R5.z, R0.x;
DP3 R5.y, R6, c[2];
DP3 R0.x, R5, c[2].w;
ADD R1.y, R0.x, -R0;
MOV R0.z, c[2].w;
MUL R0.xy, R1, c[1].x;
MAD R0.xyz, R0, c[3].x, c[3].x;
DP3 R0.w, R0, R0;
RSQ R0.w, R0.w;
MUL result.color.xyz, R0.w, R0;
MOV result.color.w, c[2];
END
# 33 instructions, 8 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_BlurRadius]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 26 ALU, 8 TEX
dcl_2d s0
def c1, 0.21997070, 0.70703125, 0.07098389, 1.00000000
def c2, 0.50000000, 0, 0, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
dcl t4.xy
dcl t5.xy
dcl t6.xy
dcl t7.xy
texld r0, t3, s0
texld r1, t2, s0
texld r2, t1, s0
texld r3, t4, s0
texld r4, t7, s0
texld r5, t0, s0
texld r7, t5, s0
texld r6, t6, s0
dp3_pp r6.x, r6, c1
dp3_pp r7.x, r7, c1
dp3_pp r4.x, r4, c1
dp3_pp r3.x, r3, c1
dp3_pp r6.y, r0, c1
mov r6.z, r4.x
dp3_pp r4.y, r2, c1
mov r4.z, r3.x
dp3_pp r3.y, r1, c1
mov r3.z, r7.x
dp3 r1.x, r3, c1.w
dp3 r0.x, r6, c1.w
add r2.y, r0.x, -r1.x
mov_pp r0.z, c1.w
mov r7.z, r6.x
dp3_pp r7.y, r5, c1
dp3 r5.x, r7, c1.w
dp3 r2.x, r4, c1.w
add r2.x, r2, -r5
mul r0.xy, r2, c0.x
mad_pp r1.xyz, r0, c2.x, c2.x
dp3_pp r0.x, r1, r1
rsq_pp r0.x, r0.x
mov_pp r0.w, c1
mul_pp r0.xyz, r0.x, r1
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}