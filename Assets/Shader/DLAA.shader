Shader "Hidden/DLAA" {
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
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
# 18 ALU, 5 TEX
PARAM c[2] = { program.local[0],
		{ 4, 1, -1, 0.33000001 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.xy, c[1].yzzw;
MAD R2.xy, R0.yxzw, c[0], fragment.texcoord[0];
MAD R1.zw, R0.xyxy, c[0].xyxy, fragment.texcoord[0].xyxy;
ADD R2.zw, fragment.texcoord[0].xyxy, c[0].xyxy;
ADD R1.xy, fragment.texcoord[0], -c[0];
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R4.xyz, R2.zwzw, texture[0], 2D;
TEX R3.xyz, R2, texture[0], 2D;
TEX R2.xyz, R1.zwzw, texture[0], 2D;
TEX R1.xyz, R1, texture[0], 2D;
ADD R1.xyz, R1, R2;
ADD R1.xyz, R1, R3;
ADD R1.xyz, R1, R4;
MAD R1.xyz, -R0, c[1].x, R1;
ABS R1.xyz, R1;
MUL R1.xyz, R1, c[1].x;
DP3 R0.w, R1, c[1].w;
MOV result.color, R0;
END
# 18 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 16 ALU, 5 TEX
dcl_2d s0
def c1, 1.00000000, -1.00000000, 4.00000000, 0.33000001
dcl t0.xy
add r4.xy, t0, -c0
mov r1.xy, c0
mov r0.x, c1.y
mov r0.y, c1.x
mad r2.xy, r0, r1, t0
mov r0.xy, c0
mad r3.xy, c1, r0, t0
add r1.xy, t0, c0
texld r0, t0, s0
texld r1, r1, s0
texld r2, r2, s0
texld r3, r3, s0
texld r4, r4, s0
add r3.xyz, r4, r3
add r2.xyz, r3, r2
add r1.xyz, r2, r1
mad r1.xyz, -r0, c1.z, r1
abs r1.xyz, r1
mul r1.xyz, r1, c1.z
dp3 r0.w, r1, c1.w
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
"3.0-!!ARBvp1.0
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
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"3.0-!!ARBfp1.0
# 156 ALU, 21 TEX
PARAM c[7] = { program.local[0],
		{ 0, 1.5, 3.5, 5.5 },
		{ 7.5, 0, -1.5, -3.5 },
		{ -5.5, 0, -7.5, 0.25 },
		{ 1, 0.33000001, 0, 0.125 },
		{ 0, -1, 4, 2 },
		{ 0.99000001, 0, 0.1, 0.16666667 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEMP R8;
TEMP R9;
TEMP R10;
TEMP R11;
TEMP R12;
TEMP R13;
TEMP R14;
TEMP R15;
MOV R9, c[2];
MOV R8, c[1];
MAD R0.zw, R9.xyzy, c[0].xyxy, fragment.texcoord[0].xyxy;
MAD R2.zw, R9.xyyz, c[0].xyxy, fragment.texcoord[0].xyxy;
TEX R4, R2.zwzw, texture[0], 2D;
MAD R2.xy, R8, c[0], fragment.texcoord[0];
TEX R2, R2, texture[0], 2D;
ADD R3, R2, R4;
TEX R1, R0.zwzw, texture[0], 2D;
MAD R0.xy, R8.yxzw, c[0], fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
ADD R5, R0, R1;
MUL R6, R5, c[5].w;
TEX R5, fragment.texcoord[0], texture[0], 2D;
MAD R7, R5, c[5].w, R6;
MUL R7, R7, c[6].w;
MUL R3, R3, c[5].w;
MAD R10.xyz, -R5, c[5].z, R3;
DP3 R8.y, R7, c[4].y;
ABS R10.xyz, R10;
MUL R10.xyz, R10, c[3].w;
DP3 R6.w, R10, c[6].x;
MAD R3, R5, c[5].w, R3;
MUL R3, R3, c[6].w;
MAD R10.xyz, -R5, c[5].z, R6;
ADD R6.w, R6, -c[6].z;
RCP R8.y, R8.y;
MUL_SAT R8.y, R6.w, R8;
ADD R6, -R5, R7;
ABS R7.xyz, R10;
MAD R10.xy, R8.xwzw, c[0], fragment.texcoord[0];
MAD R6, R8.y, R6, R5;
MUL R7.xyz, R7, c[3].w;
DP3 R8.y, R7, c[6].x;
ADD R7, R3, -R6;
DP3 R3.z, R3, c[4].y;
RCP R9.z, R3.z;
MAD R3.xy, R8.xzzw, c[0], fragment.texcoord[0];
TEX R3, R3, texture[0], 2D;
ADD R2.xyz, R2, R3;
MAD R3.xy, R9.yxzw, c[0], fragment.texcoord[0];
TEX R10, R10, texture[0], 2D;
ADD R2.w, R2, R3;
ADD R8.y, R8, -c[6].z;
MUL_SAT R8.y, R8, R9.z;
MAD R6, R8.y, R7, R6;
TEX R11, R3, texture[0], 2D;
ADD R2.w, R2, R10;
ADD R2.w, R2, R11;
ADD R2.xyz, R10, R2;
ADD R2.xyz, R11, R2;
ADD R3.xyz, R4, R2;
MOV R2.xyz, c[3];
MAD R4.xy, R9.ywzw, c[0], fragment.texcoord[0];
TEX R14, R4, texture[0], 2D;
ADD R4.xyz, R14, R3;
MAD R7.xy, R2.yxzw, c[0], fragment.texcoord[0];
TEX R15, R7, texture[0], 2D;
ADD R2.w, R2, R4;
MOV R3.xy, c[5];
MAD R7.zw, R3.xyyx, c[0].xyxy, fragment.texcoord[0].xyxy;
TEX R12, R7.zwzw, texture[0], 2D;
MAD R7.xy, R2.yzzw, c[0], fragment.texcoord[0];
ADD R2.w, R2, R14;
TEX R7, R7, texture[0], 2D;
ADD R4.xyz, R15, R4;
ADD R4.xyz, R7, R4;
MUL R4.xyz, R4, c[4].w;
ADD R2.w, R2, R15;
ADD R2.w, R2, R7;
DP3 R3.z, R5, c[4].y;
DP3 R7.y, R12, c[4].y;
DP3 R4.z, R4, c[4].y;
ADD R4.x, R4.z, -R7.y;
ADD R7.x, R3.z, -R7.y;
RCP R4.y, R7.x;
MUL_SAT R7.y, R4.x, R4;
ABS R7.x, R7;
MAD R3.xy, R3, c[0], fragment.texcoord[0];
MOV R4.xy, c[4].xzzw;
CMP R7.z, -R7.x, R7.y, c[1].x;
ADD R13, R5, -R12;
MAD R7.xy, R4, c[0], fragment.texcoord[0];
MAD R13, R7.z, R13, R12;
TEX R12, R7, texture[0], 2D;
DP3 R3.w, R12, c[4].y;
ADD R3.w, R3.z, -R3;
RCP R7.x, R3.w;
ADD R4.z, -R3, R4;
MAD_SAT R4.z, R4, R7.x, c[4].x;
ABS R3.w, R3;
CMP R3.w, -R3, R4.z, c[1].x;
ADD R13, -R12, R13;
MAD R10, R3.w, R13, R12;
MUL R2.w, R2, c[3];
ADD_SAT R3.w, R2, -c[4].x;
ADD R7, R10, -R6;
MAD R4.zw, R8.xyzx, c[0].xyxy, fragment.texcoord[0].xyxy;
TEX R10, R4.zwzw, texture[0], 2D;
ADD R10.xyz, R0, R10;
ADD R2.w, R0, R10;
MAD R0.xy, R8.wxzw, c[0], fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
MAD R4.zw, R9.xyxy, c[0].xyxy, fragment.texcoord[0].xyxy;
TEX R8, R4.zwzw, texture[0], 2D;
MAD R4.zw, R9.xywy, c[0].xyxy, fragment.texcoord[0].xyxy;
TEX R9, R4.zwzw, texture[0], 2D;
ADD R0.w, R2, R0;
ADD R0.w, R0, R8;
ADD R0.w, R0, R1;
ADD R0.xyz, R0, R10;
ADD R0.xyz, R8, R0;
ADD R0.xyz, R1, R0;
MAD R4.zw, R2.xyzy, c[0].xyxy, fragment.texcoord[0].xyxy;
MAD R11.xy, R2, c[0], fragment.texcoord[0];
TEX R2, R11, texture[0], 2D;
ADD R1.xyz, R9, R0;
ADD R0.w, R0, R9;
TEX R11, R4.zwzw, texture[0], 2D;
ADD R0.w, R0, R2;
ADD R2.xyz, R2, R1;
ADD R0.w, R0, R11;
MUL R0.w, R0, c[3];
ADD_SAT R4.z, R0.w, -c[4].x;
ADD R2.xyz, R11, R2;
MUL R2.xyz, R2, c[4].w;
SLT R1.w, c[1].x, R3;
MAD R7, R3.w, R7, R6;
SLT R0.w, c[1].x, R4.z;
ADD_SAT R3.w, R0, R1;
TEX R0, R3, texture[0], 2D;
DP3 R2.w, R0, c[4].y;
DP3 R3.y, R2, c[4].y;
ADD R3.x, R3.z, -R2.w;
RCP R2.y, R3.x;
ADD R2.x, R3.y, -R2.w;
MUL_SAT R4.w, R2.x, R2.y;
MAD R2.xy, R4.yxzw, c[0], fragment.texcoord[0];
ABS R3.x, R3;
ADD R1, R5, -R0;
CMP R4.x, -R3, R4.w, c[1];
MAD R0, R4.x, R1, R0;
TEX R2, R2, texture[0], 2D;
DP3 R3.x, R2, c[4].y;
ADD R1.x, R3.z, -R3;
RCP R1.z, R1.x;
ADD R1.y, -R3.z, R3;
CMP R6, -R3.w, R7, R6;
ADD R0, -R2, R0;
MAD_SAT R1.y, R1, R1.z, c[4].x;
ABS R1.x, R1;
CMP R1.x, -R1, R1.y, c[1];
MAD R0, R1.x, R0, R2;
ADD R0, R0, -R6;
MAD R0, R4.z, R0, R6;
CMP result.color, -R3.w, R0, R6;
END
# 156 instructions, 16 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"ps_3_0
; 153 ALU, 21 TEX, 1 FLOW
dcl_2d s0
def c1, -1.50000000, 0.00000000, 1.50000000, 2.00000000
def c2, 4.00000000, 0.25000000, 0.99000001, -0.10000000
def c3, 0.16666667, 0.33000001, 3.50000000, 0.00000000
def c4, 5.50000000, 0.00000000, 7.50000000, -3.50000000
def c5, -5.50000000, 0.00000000, -7.50000000, -1.00000000
def c6, 0.25000000, -1.00000000, 1.00000000, 0.00000000
def c7, 0.12500000, 0, 0, 0
dcl_texcoord0 v0.xy
mov r0.zw, c0.xyxy
mad r1.xy, c1.zyzw, r0.zwzw, v0
mov r0.zw, c0.xyxy
mad r4.xy, c1.yzzw, r0.zwzw, v0
mov r0.xy, c0
mad r0.xy, c1, r0, v0
texld r2, r0, s0
texld r3, r1, s0
add r1, r2, r3
mul r6, r1, c1.w
texld r1, v0, s0
mad r7, r1, c1.w, r6
mul r7, r7, c3.x
dp3 r8.w, r7, c3.y
mov r0.xy, c0
mov r9.xy, c0
mad r9.xy, c4.ywzw, r9, v0
texld r10, r9, s0
mov r9.zw, c0.xyxy
mad r9.xy, c5.yxzw, r9.zwzw, v0
texld r11, r9, s0
mov r9.xy, c0
mad r12.xy, c5.yzzw, r9, v0
mov r9.xy, c0
mad r9.xy, c3.zwzw, r9, v0
texld r9, r9, s0
mov r13.xy, c0
mad r13.xy, c4, r13, v0
mov r14.xy, c0
mad r14.xy, c4.zyzw, r14, v0
mov r15.xy, c0
mad r15.xy, c4.wyzw, r15, v0
mov r16.xy, c0
mad r16.xy, c5, r16, v0
mov r17.xy, c0
mad r17.xy, c5.zyzw, r17, v0
add r7, -r1, r7
texld r5, r4, s0
mad r0.xy, c1.yxzw, r0, v0
texld r4, r0, s0
add r0, r4, r5
mul r0, r0, c1.w
mad r8.xyz, -r1, c2.x, r0
mov r20.xyz, r4
abs r8.xyz, r8
mul r8.xyz, r8, c2.y
dp3 r6.w, r8, c2.z
mad r0, r1, c1.w, r0
mov r4.xy, c0
mul r0, r0, c3.x
rcp r8.x, r8.w
add r6.w, r6, c2
mul_sat r6.w, r6, r8.x
mad r8.xyz, -r1, c2.x, r6
mad r6, r6.w, r7, r1
abs r7.xyz, r8
mul r8.xyz, r7, c2.y
add r7, r0, -r6
dp3 r0.w, r8, c2.z
dp3 r0.x, r0, c3.y
add r0.z, r0.w, c2.w
rcp r0.w, r0.x
mov r0.xy, c0
mad r8.xy, c3.wzzw, r0, v0
mul_sat r0.z, r0, r0.w
mad r0, r0.z, r7, r6
texld r6, r8, s0
mov r7.xy, c0
mad r7.xy, c4.yxzw, r7, v0
texld r7, r7, s0
add r5.w, r5, r6
mov r8.xy, c0
mad r8.xy, c4.yzzw, r8, v0
texld r8, r8, s0
add r5.w, r7, r5
add r5.w, r8, r5
add r4.w, r4, r5
add r4.w, r10, r4
mov r18.xyz, r7
add r4.w, r11, r4
texld r12, r12, s0
add r4.w, r12, r4
add r3.w, r3, r9
texld r13, r13, s0
add r3.w, r13, r3
texld r14, r14, s0
add r3.w, r14, r3
add r2.w, r2, r3
mad_sat r3.w, r4, c6.x, c6.y
texld r15, r15, s0
add r2.w, r15, r2
texld r16, r16, s0
add r2.w, r16, r2
texld r17, r17, s0
add r2.w, r17, r2
mad_sat r2.w, r2, c6.x, c6.y
cmp r4.w, -r2, c6, c6.z
cmp r5.w, -r3, c6, c6.z
add_pp_sat r5.w, r4, r5
mov r4.zw, c0.xyxy
mad r7.xy, c5.wyzw, r4.zwzw, v0
mad r4.xy, c6.zwzw, r4, v0
texld r7, r7, s0
mov r19.xyz, r8
texld r4, r4, s0
mov r21.xyz, r10
mov r10, r4
mov r4.xy, c0
mov r8, r7
mov r4.zw, c0.xyxy
mad r7.xy, c6.wzzw, r4.zwzw, v0
mad r4.xy, c5.ywzw, r4, v0
texld r4, r4, s0
texld r7, r7, s0
if_gt r5.w, c1.y
add r5.xyz, r5, r6
add r5.xyz, r5, r18
add r5.xyz, r5, r19
add r5.xyz, r5, r20
add r5.xyz, r5, r21
add r5.xyz, r5, r11
add r6.xyz, r5, r12
mul r6.xyz, r6, c7.x
add r3.xyz, r3, r9
add r3.xyz, r3, r13
add r3.xyz, r3, r14
add r2.xyz, r3, r2
add r2.xyz, r2, r15
add r2.xyz, r2, r16
add r2.xyz, r2, r17
mul r2.xyz, r2, c7.x
dp3 r6.w, r1, c3.y
dp3 r3.x, r4, c3.y
dp3 r2.y, r2, c3.y
add r3.y, r6.w, -r3.x
add r5, r1, -r8
dp3 r9.w, r8, c3.y
dp3 r6.x, r6, c3.y
add r6.y, r6.x, -r9.w
add r11.x, r6.w, -r9.w
rcp r6.z, r11.x
mul_sat r6.z, r6.y, r6
abs r6.y, r11.x
cmp r6.y, -r6, c1, r6.z
mad r5, r6.y, r5, r8
dp3 r6.y, r10, c3.y
add r6.y, r6.w, -r6
add r1, r1, -r4
add r2.x, r2.y, -r3
rcp r2.z, r3.y
mul_sat r2.z, r2.x, r2
abs r2.x, r3.y
cmp r2.z, -r2.x, c1.y, r2
add r6.x, -r6.w, r6
rcp r6.z, r6.y
mad_sat r6.z, r6.x, r6, c6
abs r6.x, r6.y
add r5, -r10, r5
cmp r6.x, -r6, c1.y, r6.z
mad r5, r6.x, r5, r10
add r5, r5, -r0
mad r0, r3.w, r5, r0
mad r3, r2.z, r1, r4
dp3 r2.x, r7, c3.y
add r1.x, r6.w, -r2
rcp r1.z, r1.x
add r1.y, -r6.w, r2
mad_sat r1.y, r1, r1.z, c6.z
abs r1.x, r1
add r3, -r7, r3
cmp r1.x, -r1, c1.y, r1.y
mad r1, r1.x, r3, r7
add r1, r1, -r0
mad r0, r2.w, r1, r0
endif
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
"3.0-!!ARBvp1.0
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
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"3.0-!!ARBfp1.0
# 157 ALU, 21 TEX
PARAM c[8] = { program.local[0],
		{ 1.5, 0, 3.5, 5.5 },
		{ 7.5, 0, -1.5, -3.5 },
		{ -5.5, 0, -7.5, 0.25 },
		{ 1, 0.2, 0.33000001, 0 },
		{ 0.125, 0, -1, 2 },
		{ 0.5, 0.99000001, 0, 0.1 },
		{ 0.33333334 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEMP R8;
TEMP R9;
TEMP R10;
TEMP R11;
TEMP R12;
TEMP R13;
TEMP R14;
TEMP R15;
TEMP R16;
TEMP R17;
TEMP R18;
TEMP R19;
MOV R14, c[1];
MOV R15, c[2];
MOV R11.zw, c[5].xyyz;
MAD R5.zw, R11, c[0].xyxy, fragment.texcoord[0].xyxy;
MAD R0.xy, R14.yxzw, c[0], fragment.texcoord[0];
TEX R2, R0, texture[0], 2D;
MOV R11.xy, c[4].xwzw;
MAD R5.xy, R11.yxzw, c[0], fragment.texcoord[0];
MAD R0.zw, R15.xyyz, c[0].xyxy, fragment.texcoord[0].xyxy;
TEX R4, R0.zwzw, texture[0], 2D;
MAD R0.zw, R15.xyzy, c[0].xyxy, fragment.texcoord[0].xyxy;
ADD R3, R2, R4;
TEX R1, R0.zwzw, texture[0], 2D;
MAD R0.xy, R14, c[0], fragment.texcoord[0];
TEX R0, R0, texture[0], 2D;
TEX R7, R5.zwzw, texture[0], 2D;
TEX R6, R5, texture[0], 2D;
TEX R5, fragment.texcoord[0], texture[0], 2D;
ADD R8, R0, R1;
ADD R9.xyz, R6, R7;
MAD R9.xyz, -R5, c[5].w, R9;
ABS R10.xyz, R9;
ADD R8, R5, R8;
MUL R8, R8, c[7].x;
ADD R9, -R5, R8;
DP3 R8.y, R8, c[4].z;
MUL R10.xyz, R10, c[6].x;
DP3 R8.x, R10, c[6].y;
ADD R3, R5, R3;
MUL R3, R3, c[7].x;
MAD R8.zw, R11.xywz, c[0].xyxy, fragment.texcoord[0].xyxy;
RCP R8.y, R8.y;
ADD R8.x, R8, -c[6].w;
MUL_SAT R8.x, R8, R8.y;
MAD R10, R8.x, R9, R5;
TEX R9, R8.zwzw, texture[0], 2D;
MAD R8.xy, R11, c[0], fragment.texcoord[0];
TEX R8, R8, texture[0], 2D;
ADD R11.xyz, R8, R9;
MAD R12.xyz, -R5, c[5].w, R11;
ADD R11, R3, -R10;
ABS R12.xyz, R12;
DP3 R3.w, R3, c[4].z;
MUL R3.xyz, R12, c[6].x;
DP3 R3.z, R3, c[6].y;
RCP R12.y, R3.w;
ADD R12.x, R3.z, -c[6].w;
MUL_SAT R14.x, R12, R12.y;
MUL R11, R14.x, R11;
MAD R10, R11, c[6].x, R10;
MAD R3.xy, R14.yzzw, c[0], fragment.texcoord[0];
TEX R3, R3, texture[0], 2D;
ADD R2.xyz, R2, R3;
MAD R3.xy, R15.yxzw, c[0], fragment.texcoord[0];
MAD R12.xy, R14.ywzw, c[0], fragment.texcoord[0];
TEX R12, R12, texture[0], 2D;
ADD R2.w, R2, R3;
MAD R11.xy, R15.wyzw, c[0], fragment.texcoord[0];
TEX R13, R3, texture[0], 2D;
ADD R2.xyz, R12, R2;
ADD R2.xyz, R13, R2;
ADD R3.xyz, R4, R2;
MOV R2.xyz, c[3];
ADD R2.w, R2, R12;
MAD R4.xy, R15.ywzw, c[0], fragment.texcoord[0];
TEX R16, R4, texture[0], 2D;
MAD R4.xy, R2.yxzw, c[0], fragment.texcoord[0];
TEX R17, R4, texture[0], 2D;
MAD R4.xy, R2.yzzw, c[0], fragment.texcoord[0];
TEX R18, R4, texture[0], 2D;
ADD R3.xyz, R16, R3;
ADD R3.xyz, R17, R3;
ADD R3.xyz, R18, R3;
MUL R3.xyz, R3, c[5].x;
ADD R2.w, R2, R13;
ADD R2.w, R2, R4;
ADD R2.w, R2, R16;
ADD R2.w, R2, R17;
ADD R2.w, R2, R18;
MUL R2.w, R2, c[3];
DP3 R13.x, R5, c[4].z;
DP3 R4.y, R9, c[4].z;
DP3 R3.x, R3, c[4].z;
ADD R4.x, R13, -R4.y;
ADD R3.y, R3.x, -R4;
RCP R3.z, R4.x;
MUL_SAT R3.z, R3.y, R3;
ABS R3.y, R4.x;
CMP R3.y, -R3, R3.z, c[1];
ADD R19, R5, -R9;
MAD R9, R3.y, R19, R9;
ADD R3.y, -R13.x, R3.x;
DP3 R3.x, R8, c[4].z;
ADD R3.x, R13, -R3;
RCP R3.z, R3.x;
ADD R9, -R8, R9;
ADD_SAT R13.y, R2.w, -c[4].x;
MAD R12.zw, R2.xyxy, c[0].xyxy, fragment.texcoord[0].xyxy;
MAD R12.xy, R2.zyzw, c[0], fragment.texcoord[0];
TEX R2, R12.zwzw, texture[0], 2D;
MAD_SAT R3.y, R3, R3.z, c[4].x;
ABS R3.x, R3;
CMP R3.x, -R3, R3.y, c[1].y;
MAD R3, R3.x, R9, R8;
ADD R3, R3, -R10;
MAD R9.xy, R15, c[0], fragment.texcoord[0];
MAD R8.xy, R14.zyzw, c[0], fragment.texcoord[0];
MAD R4, R13.y, R3, R10;
TEX R3, R8, texture[0], 2D;
ADD R0.xyz, R0, R3;
DP3 R3.y, R7, c[4].z;
MAD R8.xy, R14.wyzw, c[0], fragment.texcoord[0];
TEX R8, R8, texture[0], 2D;
ADD R0.w, R0, R3;
TEX R9, R9, texture[0], 2D;
ADD R0.w, R0, R8;
ADD R0.w, R0, R9;
ADD R0.xyz, R8, R0;
ADD R0.xyz, R9, R0;
ADD R0.w, R0, R1;
TEX R11, R11, texture[0], 2D;
ADD R0.xyz, R1, R0;
ADD R0.xyz, R11, R0;
ADD R0.w, R0, R11;
ADD R0.w, R0, R2;
TEX R12, R12, texture[0], 2D;
ADD R2.xyz, R2, R0;
ADD R0.w, R0, R12;
MUL R0.w, R0, c[3];
ADD_SAT R2.w, R0, -c[4].x;
ADD R0.w, R2, -R13.y;
ABS R0.w, R0;
ADD R3.x, R0.w, -c[4].y;
ADD R2.xyz, R12, R2;
MUL R2.xyz, R2, c[5].x;
DP3 R2.y, R2, c[4].z;
ADD R2.x, R2.y, -R3.y;
ADD R3.z, R13.x, -R3.y;
RCP R2.z, R3.z;
MUL_SAT R2.z, R2.x, R2;
ABS R2.x, R3.z;
CMP R2.z, -R2.x, R2, c[1].y;
ADD R0, R5, -R7;
MAD R0, R2.z, R0, R7;
DP3 R2.x, R6, c[4].z;
ADD R2.x, R13, -R2;
RCP R2.z, R2.x;
ADD R2.y, -R13.x, R2;
CMP R1, -R3.x, R4, R10;
ADD R0, -R6, R0;
MAD_SAT R2.y, R2, R2.z, c[4].x;
ABS R2.x, R2;
CMP R2.x, -R2, R2.y, c[1].y;
MAD R0, R2.x, R0, R6;
ADD R0, R0, -R1;
MAD R0, R2.w, R0, R1;
CMP result.color, -R3.x, R0, R1;
END
# 157 instructions, 20 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
"ps_3_0
; 153 ALU, 21 TEX, 1 FLOW
dcl_2d s0
def c1, -1.00000000, 0.00000000, 1.00000000, 2.00000000
def c2, 0.50000000, 0.99000001, -0.10000000, 0.33333334
def c3, 0.00000000, -1.50000000, 1.50000000, 0.33000001
def c4, 3.50000000, 0.00000000, 5.50000000, 7.50000000
def c5, -3.50000000, 0.00000000, -5.50000000, -7.50000000
def c6, 0.25000000, -1.00000000, 0.20000000, 0.12500000
dcl_texcoord0 v0.xy
mov r0.zw, c0.xyxy
mad r1.xy, c3.zxzw, r0.zwzw, v0
mov r0.zw, c0.xyxy
mad r2.xy, c1.yzzw, r0.zwzw, v0
mov r14.xy, c0
mad r15.xy, c5.yxzw, r14, v0
mov r0.xy, c0
mad r0.xy, c3.yxzw, r0, v0
texld r7, r0, s0
texld r8, r1, s0
mov r0.xy, c0
mad r0.xy, c1.yxzw, r0, v0
texld r3, r0, s0
texld r4, r2, s0
mov r13.xy, c0
mad r13.xy, c4.ywzw, r13, v0
mov r14.xy, c0
mad r14.xy, c5.yzzw, r14, v0
texld r16, r14, s0
mov r14.xy, c0
mad r17.xy, c5.ywzw, r14, v0
mov r14.xy, c0
mad r14.xy, c4, r14, v0
mov r18.xy, c0
mad r18.xy, c4.zyzw, r18, v0
mov r19.xy, c0
mad r19.xy, c4.wyzw, r19, v0
mov r20.xy, c0
mad r20.xy, c5, r20, v0
mov r21.xy, c0
mad r21.xy, c5.zyzw, r21, v0
mov r22.xy, c0
mad r22.xy, c5.wyzw, r22, v0
texld r0, v0, s0
add r1, r7, r8
add r1, r0, r1
add r2.xyz, r3, r4
mad r2.xyz, -r0, c1.w, r2
abs r5.xyz, r2
mul r1, r1, c2.w
add r2, -r0, r1
dp3 r1.y, r1, c3.w
mul r5.xyz, r5, c2.x
dp3 r1.x, r5, c2.y
texld r14, r14, s0
texld r18, r18, s0
texld r19, r19, s0
texld r20, r20, s0
texld r21, r21, s0
rcp r1.y, r1.y
add r1.x, r1, c2.z
mul_sat r1.x, r1, r1.y
mad r10, r1.x, r2, r0
mov r1.zw, c0.xyxy
mad r2.xy, c3.xzzw, r1.zwzw, v0
mov r1.xy, c0
mad r1.xy, c3, r1, v0
texld r5, r1, s0
texld r6, r2, s0
add r9, r5, r6
mov r1.zw, c0.xyxy
mad r2.xy, c1.zyzw, r1.zwzw, v0
add r9, r0, r9
mov r1.xy, c0
mad r1.xy, c1, r1, v0
mul r9, r9, c2.w
texld r22, r22, s0
texld r2, r2, s0
texld r1, r1, s0
add r11.xyz, r1, r2
mad r11.xyz, -r0, c1.w, r11
abs r12.xyz, r11
add r11, r9, -r10
dp3 r9.y, r9, c3.w
mul r12.xyz, r12, c2.x
dp3 r9.x, r12, c2.y
mov r12.xy, c0
mad r12.xy, c4.yzzw, r12, v0
rcp r9.w, r9.y
add r9.z, r9.x, c2
mul_sat r9.z, r9, r9.w
mul r11, r9.z, r11
mov r9.xy, c0
mad r9.xy, c4.yxzw, r9, v0
texld r9, r9, s0
mad r10, r11, c2.x, r10
mov r11.xyz, r14
mov r14.xyz, r18
mov r18.xyz, r19
mov r19.xyz, r20
mov r20.xyz, r21
add r6.w, r6, r9
texld r12, r12, s0
add r6.w, r12, r6
texld r13, r13, s0
add r6.w, r13, r6
add r5.w, r5, r6
texld r15, r15, s0
add r5.w, r15, r5
add r6.w, r8, r14
add r6.w, r18, r6
add r6.w, r19, r6
add r6.w, r7, r6
add r6.w, r20, r6
add r6.w, r21, r6
add r6.w, r22, r6
add r5.w, r16, r5
texld r17, r17, s0
add r5.w, r17, r5
mad_sat r7.w, r5, c6.x, c6.y
mad_sat r6.w, r6, c6.x, c6.y
add r5.w, r6, -r7
abs r5.w, r5
mov r21.xyz, r22
if_gt r5.w, c6.z
add r6.xyz, r6, r9
add r6.xyz, r6, r12
add r6.xyz, r6, r13
add r5.xyz, r6, r5
add r5.xyz, r5, r15
add r5.xyz, r5, r16
add r6.xyz, r5, r17
mul r6.xyz, r6, c6.w
dp3 r8.w, r0, c3.w
add r5, r0, -r1
dp3 r9.x, r1, c3.w
dp3 r6.x, r6, c3.w
add r6.y, r6.x, -r9.x
add r9.y, r8.w, -r9.x
rcp r6.z, r9.y
mul_sat r6.z, r6.y, r6
abs r6.y, r9
cmp r6.y, -r6, c1, r6.z
mad r1, r6.y, r5, r1
dp3 r5.w, r2, c3.w
add r5.w, r8, -r5
rcp r6.y, r5.w
add r5.xyz, r8, r11
add r5.xyz, r5, r14
add r6.x, -r8.w, r6
add r0, r0, -r3
add r1, -r2, r1
add r5.xyz, r5, r18
mad_sat r6.x, r6, r6.y, c1.z
abs r5.w, r5
cmp r5.w, -r5, c1.y, r6.x
mad r1, r5.w, r1, r2
add r1, r1, -r10
add r2.xyz, r5, r7
dp3 r2.w, r3, c3.w
add r2.xyz, r2, r19
add r2.xyz, r2, r20
add r2.xyz, r2, r21
mul r2.xyz, r2, c6.w
dp3 r2.y, r2, c3.w
add r5.x, r8.w, -r2.w
mad r1, r7.w, r1, r10
add r2.x, r2.y, -r2.w
rcp r2.z, r5.x
mul_sat r2.z, r2.x, r2
abs r2.x, r5
cmp r2.z, -r2.x, c1.y, r2
mad r3, r2.z, r0, r3
dp3 r2.x, r4, c3.w
add r0.x, r8.w, -r2
rcp r0.z, r0.x
add r0.y, -r8.w, r2
mad_sat r0.y, r0, r0.z, c1.z
abs r0.x, r0
add r3, -r4, r3
cmp r0.x, -r0, c1.y, r0.y
mad r0, r0.x, r3, r4
add r0, r0, -r1
mad r10, r6.w, r0, r1
endif
mov_pp oC0, r10
"
}
}
 }
}
Fallback Off
}