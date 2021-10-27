Shader "Hidden/MultipassHollywoodFlares" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _NonBlurredTex ("Base (RGB)", 2D) = "" {}
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
Vector 0 [tintColor]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_NonBlurredTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 2 TEX
PARAM c[2] = { program.local[0],
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[0], texture[1], 2D;
DP4 R2.x, c[0], c[0];
RSQ R2.x, R2.x;
MUL R2, R2.x, c[0];
MUL R1, R1, R2;
MAD R0, R0, c[0], R1;
MUL result.color, R0, c[1].x;
END
# 8 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [tintColor]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_NonBlurredTex] 2D
"ps_2_0
; 7 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c1, 0.50000000, 0, 0, 0
dcl t0.xy
texld r1, t0, s0
texld r0, t0, s1
dp4_pp r2.x, c0, c0
rsq_pp r2.x, r2.x
mul_pp r2, r2.x, c0
mul_pp r0, r0, r2
mad_pp r0, r1, c0, r0
mul_pp r0, r0, c1.x
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
Float 6 [stretchWidth]
"!!ARBvp1.0
# 13 ALU
PARAM c[7] = { { 2, 4, 6 },
		state.matrix.mvp,
		program.local[5..6] };
TEMP R0;
MOV R0.xy, c[5];
MUL R0.xy, R0, c[6].x;
MOV result.texcoord[0].xy, vertex.texcoord[0];
MAD result.texcoord[1].xy, R0, c[0].x, vertex.texcoord[0];
MAD result.texcoord[2].xy, -R0, c[0].x, vertex.texcoord[0];
MAD result.texcoord[3].xy, R0, c[0].y, vertex.texcoord[0];
MAD result.texcoord[4].xy, -R0, c[0].y, vertex.texcoord[0];
MAD result.texcoord[5].xy, R0, c[0].z, vertex.texcoord[0];
MAD result.texcoord[6].xy, -R0, c[0].z, vertex.texcoord[0];
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
Vector 4 [offsets]
Float 5 [stretchWidth]
"vs_2_0
; 13 ALU
def c6, 2.00000000, 4.00000000, 6.00000000, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
mul r0.xy, c4, r0.x
mov oT0.xy, v1
mad oT1.xy, r0, c6.x, v1
mad oT2.xy, -r0, c6.x, v1
mad oT3.xy, r0, c6.y, v1
mad oT4.xy, -r0, c6.y, v1
mad oT5.xy, r0, c6.z, v1
mad oT6.xy, -r0, c6.z, v1
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
# 13 ALU, 7 TEX
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEX R6, fragment.texcoord[6], texture[0], 2D;
TEX R5, fragment.texcoord[5], texture[0], 2D;
TEX R4, fragment.texcoord[4], texture[0], 2D;
TEX R3, fragment.texcoord[3], texture[0], 2D;
TEX R2, fragment.texcoord[2], texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MAX R0, R0, R1;
MAX R0, R0, R2;
MAX R0, R0, R3;
MAX R0, R0, R4;
MAX R0, R0, R5;
MAX result.color, R0, R6;
END
# 13 instructions, 7 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 7 ALU, 7 TEX
dcl_2d s0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
dcl t4.xy
dcl t5.xy
dcl t6.xy
texld r0, t6, s0
texld r1, t5, s0
texld r2, t4, s0
texld r3, t3, s0
texld r6, t0, s0
texld r4, t2, s0
texld r5, t1, s0
max_pp r5, r6, r5
max_pp r4, r5, r4
max_pp r3, r4, r3
max_pp r2, r3, r2
max_pp r1, r2, r1
max_pp r0, r1, r0
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
"!!ARBvp1.0
# 12 ALU
PARAM c[6] = { { 0, 0.5, 1.5, 2.5 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
MOV R0, c[0];
MAD result.texcoord[1].xy, R0, c[5], vertex.texcoord[0];
MAD result.texcoord[2].xy, R0, -c[5], vertex.texcoord[0];
MAD result.texcoord[3].xy, R0.xzzw, c[5], vertex.texcoord[0];
MAD result.texcoord[4].xy, R0.xzzw, -c[5], vertex.texcoord[0];
MAD result.texcoord[5].xy, R0.xwzw, c[5], vertex.texcoord[0];
MAD result.texcoord[6].xy, R0.xwzw, -c[5], vertex.texcoord[0];
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 12 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 17 ALU
def c5, 0.00000000, 0.50000000, 1.50000000, 2.50000000
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT1.xy, c5, r0, v1
mov r0.xy, c4
mad oT2.xy, c5, -r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT3.xy, c5.xzzw, r0, v1
mad oT4.xy, c5.xzzw, -r0.zwzw, v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mov oT0.xy, v1
mad oT5.xy, c5.xwzw, r0, v1
mad oT6.xy, c5.xwzw, -r0.zwzw, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [tintColor]
Vector 1 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 18 ALU, 7 TEX
PARAM c[3] = { program.local[0..1],
		{ 0.14285715, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R6, fragment.texcoord[6], texture[0], 2D;
TEX R5, fragment.texcoord[5], texture[0], 2D;
TEX R4, fragment.texcoord[4], texture[0], 2D;
TEX R3, fragment.texcoord[3], texture[0], 2D;
TEX R2, fragment.texcoord[2], texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
ADD R0, R0, R4;
ADD R0, R0, R5;
MOV R1.x, c[1];
ADD R0, R0, R6;
MAD R0, R0, c[2].x, -R1.x;
MAX R0, R0, c[2].y;
MUL R0, R0, c[1].y;
MUL result.color, R0, c[0];
END
# 18 instructions, 7 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [tintColor]
Vector 1 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 12 ALU, 7 TEX
dcl_2d s0
def c2, 0.14285715, 0.00000000, 0, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
dcl t4.xy
dcl t5.xy
dcl t6.xy
texld r0, t6, s0
texld r1, t5, s0
texld r2, t4, s0
texld r3, t3, s0
texld r4, t2, s0
texld r5, t1, s0
texld r6, t0, s0
add_pp r5, r6, r5
add_pp r4, r5, r4
add_pp r3, r4, r3
add_pp r2, r3, r2
add_pp r1, r2, r1
mov_pp r2.x, c1
add_pp r0, r1, r0
mad_pp r0, r0, c2.x, -r2.x
max_pp r0, r0, c2.y
mul_pp r0, r0, c1.y
mul_pp r0, r0, c0
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
"!!ARBvp1.0
# 12 ALU
PARAM c[6] = { { 0, 0.5, 1.5, 2.5 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
MOV R0, c[0];
MAD result.texcoord[1].xy, R0, c[5], vertex.texcoord[0];
MAD result.texcoord[2].xy, R0, -c[5], vertex.texcoord[0];
MAD result.texcoord[3].xy, R0.xzzw, c[5], vertex.texcoord[0];
MAD result.texcoord[4].xy, R0.xzzw, -c[5], vertex.texcoord[0];
MAD result.texcoord[5].xy, R0.xwzw, c[5], vertex.texcoord[0];
MAD result.texcoord[6].xy, R0.xwzw, -c[5], vertex.texcoord[0];
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 12 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 17 ALU
def c5, 0.00000000, 0.50000000, 1.50000000, 2.50000000
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT1.xy, c5, r0, v1
mov r0.xy, c4
mad oT2.xy, c5, -r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT3.xy, c5.xzzw, r0, v1
mad oT4.xy, c5.xzzw, -r0.zwzw, v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mov oT0.xy, v1
mad oT5.xy, c5.xwzw, r0, v1
mad oT6.xy, c5.xwzw, -r0.zwzw, v1
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
# 17 ALU, 7 TEX
PARAM c[2] = { { 7.5, 0 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R6, fragment.texcoord[6], texture[0], 2D;
TEX R5, fragment.texcoord[5], texture[0], 2D;
TEX R4, fragment.texcoord[4], texture[0], 2D;
TEX R3, fragment.texcoord[3], texture[0], 2D;
TEX R2, fragment.texcoord[2], texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
ADD R0, R0, R4;
ADD R0, R0, R5;
ADD R0, R0, R6;
DP3 R1.x, R0, c[1];
ADD R1.x, R1, c[0];
RCP R1.x, R1.x;
MUL result.color, R0, R1.x;
END
# 17 instructions, 7 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 11 ALU, 7 TEX
dcl_2d s0
def c0, 0.21997070, 0.70703125, 0.07098389, 7.50000000
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
dcl t4.xy
dcl t5.xy
dcl t6.xy
texld r0, t6, s0
texld r1, t5, s0
texld r2, t4, s0
texld r3, t3, s0
texld r6, t0, s0
texld r4, t2, s0
texld r5, t1, s0
add_pp r5, r6, r5
add_pp r4, r5, r4
add_pp r3, r4, r3
add_pp r2, r3, r2
add_pp r1, r2, r1
add_pp r1, r1, r0
dp3_pp r0.x, r1, c0
add_pp r0.x, r0, c0.w
rcp_pp r0.x, r0.x
mul_pp r0, r1, r0.x
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}