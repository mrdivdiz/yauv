Shader "Hidden/SeparableWeightedBlurDof34" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _TapMedium ("TapMedium (RGB)", 2D) = "" {}
 _TapLow ("TapLow (RGB)", 2D) = "" {}
 _TapHigh ("TapHigh (RGB)", 2D) = "" {}
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
Vector 5 [offsets]
"!!ARBvp1.0
# 10 ALU
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
; 11 ALU
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
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
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 23 ALU, 5 TEX
PARAM c[1] = { { 1.25, 1.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R3, fragment.texcoord[2], texture[0], 2D;
TEX R4, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R2, fragment.texcoord[1].zwzw, texture[0], 2D;
MUL R1.xyz, R1, R1.w;
MUL R1.xyz, R1, c[0].x;
MAD R0.xyz, R0, R0.w, R1;
MUL R1.xyz, R2, R2.w;
MAD R0.xyz, R1, c[0].x, R0;
MUL R1.xyz, R3, R3.w;
MAD R0.xyz, R1, c[0].y, R0;
MUL R1.xyz, R4, R4.w;
MOV R5.x, R1.w;
MOV R5.y, R2.w;
MOV R5.w, R4;
MOV R5.z, R3.w;
DP4 R5.x, R5, c[0].xxyy;
ADD R1.w, R0, R5.x;
RCP R1.w, R1.w;
MAD R0.xyz, R1, c[0].y, R0;
MUL result.color.xyz, R0, R1.w;
MOV result.color.w, R0;
END
# 23 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 25 ALU, 5 TEX
dcl_2d s0
def c0, 1.25000000, 1.50000000, 0, 0
dcl t0.xy
dcl t1
dcl t2
texld r4, t0, s0
texld r2, t2, s0
texld r6, t1, s0
mov r0.zw, c0.y
mov r0.y, t1.w
mov r0.x, t1.z
mov r1.y, t2.w
mov r1.x, t2.z
mov_pp r5.x, r6.w
mov_pp r5.z, r2.w
mul_pp r2.xyz, r2, r2.w
texld r3, r1, s0
texld r1, r0, s0
mov_pp r5.y, r1.w
mov r0.xy, c0.x
mov_pp r5.w, r3
dp4_pp r0.x, r5, r0
mul_pp r5.xyz, r6, r6.w
add_pp r0.x, r4.w, r0
mul_pp r5.xyz, r5, c0.x
rcp_pp r0.x, r0.x
mad_pp r4.xyz, r4, r4.w, r5
mul_pp r1.xyz, r1, r1.w
mad_pp r1.xyz, r1, c0.x, r4
mad_pp r1.xyz, r2, c0.y, r1
mul_pp r2.xyz, r3, r3.w
mad_pp r1.xyz, r2, c0.y, r1
mul_pp r0.xyz, r1, r0.x
mov_pp r0.w, r4
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
Vector 5 [offsets]
"!!ARBvp1.0
# 10 ALU
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
; 11 ALU
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
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
SetTexture 1 [_TapHigh] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 13 ALU, 6 TEX
PARAM c[1] = { { 0.19995117 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R4, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R3, fragment.texcoord[2], texture[0], 2D;
TEX R2, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R5.w, fragment.texcoord[0], texture[1], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
ADD R0, R0, R4;
MUL R0, R0, c[0].x;
MAX result.color.w, R5, R0;
MOV result.color.xyz, R0;
END
# 13 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_TapHigh] 2D
"ps_2_0
; 12 ALU, 6 TEX
dcl_2d s0
dcl_2d s1
def c0, 0.19995117, 0, 0, 0
dcl t0.xy
dcl t1
dcl t2
texld r5, t0, s1
texld r1, t2, s0
texld r4, t0, s0
texld r3, t1, s0
mov r0.y, t1.w
mov r0.x, t1.z
mov r2.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
add_pp r3, r4, r3
texld r0, r0, s0
texld r2, r2, s0
add_pp r2, r3, r2
add_pp r1, r2, r1
add_pp r0, r1, r0
mul_pp r0, r0, c0.x
max r0.w, r5, r0
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
Vector 5 [offsets]
"!!ARBvp1.0
# 10 ALU
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
; 11 ALU
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
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
SetTexture 1 [_TapHigh] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 13 ALU, 6 TEX
PARAM c[1] = { { 0.2857143, 0.75, 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R4, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R3, fragment.texcoord[2], texture[0], 2D;
TEX R2, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R5.w, fragment.texcoord[0], texture[1], 2D;
MAD R0, R1, c[0].y, R0;
MAD R0, R2, c[0].y, R0;
MAD R0, R3, c[0].z, R0;
MAD R0, R4, c[0].z, R0;
MUL R0, R0, c[0].x;
MAX result.color.w, R5, R0;
MOV result.color.xyz, R0;
END
# 13 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_TapHigh] 2D
"ps_2_0
; 12 ALU, 6 TEX
dcl_2d s0
dcl_2d s1
def c0, 0.75000000, 0.50000000, 0.28571430, 0
dcl t0.xy
dcl t1
dcl t2
texld r5, t0, s1
texld r1, t2, s0
texld r4, t0, s0
texld r3, t1, s0
mov r0.y, t1.w
mov r0.x, t1.z
mov r2.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
mad_pp r3, r3, c0.x, r4
texld r0, r0, s0
texld r2, r2, s0
mad_pp r2, r2, c0.x, r3
mad_pp r1, r1, c0.y, r2
mad_pp r0, r0, c0.y, r1
mul_pp r0, r0, c0.z
max r0.w, r5, r0
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
SetTexture 0 [_TapMedium] 2D
SetTexture 1 [_TapLow] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 7 ALU, 2 TEX
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[0], texture[1], 2D;
MUL R0.w, R0, R0;
ADD R1.xyz, -R0, R1;
MUL R0.w, R0, R0;
MAD result.color.xyz, R0.w, R1, R0;
MOV result.color.w, R1;
END
# 7 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_TapMedium] 2D
SetTexture 1 [_TapLow] 2D
"ps_2_0
; 5 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
dcl t0.xy
texld r0, t0, s1
texld r1, t0, s0
add_pp r2.xyz, -r1, r0
mul_pp r0.x, r1.w, r1.w
mul_pp r0.x, r0, r0
mad_pp r0.xyz, r0.x, r2, r1
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
Vector 5 [offsets]
"!!ARBvp1.0
# 10 ALU
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
; 11 ALU
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
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
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 23 ALU, 5 TEX
PARAM c[1] = { { 0.75, 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R3, fragment.texcoord[2], texture[0], 2D;
TEX R4, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R2, fragment.texcoord[1].zwzw, texture[0], 2D;
MUL R1.xyz, R1, R1.w;
MUL R1.xyz, R1, c[0].x;
MAD R0.xyz, R0, R0.w, R1;
MUL R1.xyz, R2, R2.w;
MAD R0.xyz, R1, c[0].x, R0;
MUL R1.xyz, R3, R3.w;
MAD R0.xyz, R1, c[0].y, R0;
MUL R1.xyz, R4, R4.w;
MOV R5.x, R1.w;
MOV R5.y, R2.w;
MOV R5.w, R4;
MOV R5.z, R3.w;
DP4 R5.x, R5, c[0].xxyy;
ADD R1.w, R0, R5.x;
RCP R1.w, R1.w;
MAD R0.xyz, R1, c[0].y, R0;
MUL result.color.xyz, R0, R1.w;
MOV result.color.w, R0;
END
# 23 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 25 ALU, 5 TEX
dcl_2d s0
def c0, 0.75000000, 0.50000000, 0, 0
dcl t0.xy
dcl t1
dcl t2
texld r4, t0, s0
texld r2, t2, s0
texld r6, t1, s0
mov r0.zw, c0.y
mov r0.y, t1.w
mov r0.x, t1.z
mov r1.y, t2.w
mov r1.x, t2.z
mov_pp r5.x, r6.w
mov_pp r5.z, r2.w
mul_pp r2.xyz, r2, r2.w
texld r3, r1, s0
texld r1, r0, s0
mov_pp r5.y, r1.w
mov r0.xy, c0.x
mov_pp r5.w, r3
dp4_pp r0.x, r5, r0
mul_pp r5.xyz, r6, r6.w
add_pp r0.x, r4.w, r0
mul_pp r5.xyz, r5, c0.x
rcp_pp r0.x, r0.x
mad_pp r4.xyz, r4, r4.w, r5
mul_pp r1.xyz, r1, r1.w
mad_pp r1.xyz, r1, c0.x, r4
mad_pp r1.xyz, r2, c0.y, r1
mul_pp r2.xyz, r3, r3.w
mad_pp r1.xyz, r2, c0.y, r1
mul_pp r0.xyz, r1, r0.x
mov_pp r0.w, r4
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}