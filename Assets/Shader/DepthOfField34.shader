Shader "Hidden/Dof/DepthOfField34" {
Properties {
 _MainTex ("Base", 2D) = "" {}
 _TapLowBackground ("TapLowBackground", 2D) = "" {}
 _TapLowForeground ("TapLowForeground", 2D) = "" {}
 _TapMedium ("TapMedium", 2D) = "" {}
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
SetTexture 1 [_TapLowBackground] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 4 ALU, 2 TEX
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[0], texture[1], 2D;
ADD R1, -R0, R1;
MAD result.color, R0.w, R1, R0;
END
# 4 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_TapLowBackground] 2D
"ps_2_0
; 5 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c1, 1.00000000, -1.00000000, 0.00000000, 0
dcl t0.xy
texld r1, t0, s0
mad_pp r0.xy, t0, c1, c1.zxyw
cmp_pp r0.xy, c0.y, t0, r0
texld r0, r0, s1
add_pp r0, -r1, r0
mad_pp r0, r1.w, r0, r1
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
  ColorMask RGB
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
Float 0 [_ForegroundBlurExtrude]
SetTexture 0 [_TapLowForeground] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 14 ALU, 2 TEX
PARAM c[2] = { program.local[0],
		{ 1, 0, 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R1, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MUL R1.w, R1, c[0].x;
MAX R2.w, R0, R1;
ADD R2.xyz, R0, c[1].xxyw;
MUL R2.xyz, R2, c[1].z;
ADD R1.xyz, R1, c[1].yxyw;
MAD R1.xyz, R1, c[1].z, -R2;
MUL_SAT R3.x, R2.w, R2.w;
MAD R1.xyz, R3.x, R1, R2;
MOV R1.w, R2;
ADD R1, -R0, R1;
MOV_SAT R2.x, R2.w;
MAD result.color, R2.x, R1, R0;
END
# 14 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_ForegroundBlurExtrude]
SetTexture 0 [_TapLowForeground] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 17 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c1, 1.00000000, 0.00000000, 0.50000000, 0
dcl t0.xy
texld r0, t0, s0
texld r2, t0, s1
mov r1.y, c1.x
mov r1.xz, c1.y
add_pp r1.xyz, r0, r1
mov r0.xy, c1.x
mov r0.z, c1.y
add_pp r3.xyz, r2, r0
mul_pp r0.x, r0.w, c0
max_pp r0.x, r2.w, r0
mul_pp r3.xyz, r3, c1.z
mad_pp r4.xyz, r1, c1.z, -r3
mul_pp_sat r1.x, r0, r0
mov_pp r1.w, r0.x
mad_pp r1.xyz, r1.x, r4, r3
add_pp r1, -r2, r1
mov_pp_sat r0.x, r0
mad_pp r0, r0.x, r1, r2
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
  ColorMask RGB
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
SetTexture 1 [_TapLowBackground] 2D
SetTexture 2 [_TapMedium] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 15 ALU, 3 TEX
PARAM c[1] = { { 1, 0, 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEX R2, fragment.texcoord[0], texture[1], 2D;
TEX R1, fragment.texcoord[0], texture[2], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R3.xyz, R2, c[0].yxyw;
ADD R1.xyz, R1, c[0].xxyw;
MUL R1.xyz, R1, c[0].z;
MUL R3.xyz, R3, c[0].z;
MOV R3.w, R2;
ADD R3, R3, -R1;
MUL_SAT R4.x, R2.w, R2.w;
MAD R1, R4.x, R3, R1;
MUL R1, R1, c[0].z;
MAD R1, R2, c[0].z, R1;
ADD R1, -R0, R1;
MAD result.color, R0.w, R1, R0;
END
# 15 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_TapLowBackground] 2D
SetTexture 2 [_TapMedium] 2D
"ps_2_0
; 17 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c0, 1.00000000, 0.00000000, 0.50000000, 0
dcl t0.xy
texld r3, t0, s0
texld r2, t0, s1
texld r0, t0, s2
mov r1.xz, c0.y
mov r1.y, c0.x
add_pp r4.xyz, r2, r1
mov r1.z, c0.y
mov r1.xy, c0.x
add_pp r0.xyz, r0, r1
mul_pp r1.xyz, r4, c0.z
mul_pp r0.xyz, r0, c0.z
mov_pp r1.w, r2
add_pp r1, r1, -r0
mul_pp_sat r4.x, r2.w, r2.w
mad_pp r0, r4.x, r1, r0
mul_pp r0, r0, c0.z
mad r0, r2, c0.z, r0
add_pp r0, -r3, r0
mad_pp r0, r3.w, r0, r3
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
  ColorMask A
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
Vector 0 [_ZBufferParams]
Vector 1 [_CurveParams]
SetTexture 0 [_CameraDepthTexture] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 7 ALU, 1 TEX
PARAM c[3] = { program.local[0..1],
		{ 0 } };
TEMP R0;
TEX R0.x, fragment.texcoord[0], texture[0], 2D;
MAD R0.x, R0, c[0], c[0].y;
ADD R0.y, c[1].w, c[1].z;
RCP R0.x, R0.x;
ADD R0.x, R0, -R0.y;
CMP R0.x, -R0, R0, c[2];
MUL_SAT result.color, R0.x, c[1].y;
END
# 7 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Vector 1 [_CurveParams]
SetTexture 0 [_CameraDepthTexture] 2D
"ps_2_0
; 8 ALU, 1 TEX
dcl_2d s0
def c2, 0.00000000, 0, 0, 0
dcl t0.xy
texld r0, t0, s0
mad r1.x, r0, c0, c0.y
add_pp r0.x, c1.w, c1.z
rcp r1.x, r1.x
add r0.x, r1, -r0
cmp_pp r0.x, -r0, c2, r0
mul_pp_sat r0.x, r0, c1.y
mov_pp r0, r0.x
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
  ColorMask RGB
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
Float 0 [_ForegroundBlurExtrude]
SetTexture 0 [_TapLowForeground] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 2 TEX
PARAM c[1] = { program.local[0] };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MUL R1.w, R1, c[0].x;
MAX R2.x, R0.w, R1.w;
MOV R1.w, R2.x;
ADD R1, -R0, R1;
MOV_SAT R2.x, R2;
MAD result.color, R2.x, R1, R0;
END
# 8 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_ForegroundBlurExtrude]
Vector 1 [_MainTex_TexelSize]
SetTexture 0 [_TapLowForeground] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 9 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c2, 1.00000000, -1.00000000, 0.00000000, 0
dcl t0.xy
texld r2, t0, s0
mad_pp r0.xy, t0, c2, c2.zxyw
cmp_pp r0.xy, c1.y, t0, r0
texld r1, r0, s1
mul_pp r0.x, r2.w, c0
max_pp r0.x, r1.w, r0
mov_pp r2.w, r0.x
add_pp r2, -r1, r2
mov_pp_sat r0.x, r0
mad_pp r0, r0.x, r2, r1
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
Vector 0 [_ZBufferParams]
Vector 1 [_CurveParams]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_CameraDepthTexture] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 2 TEX
PARAM c[3] = { program.local[0..1],
		{ 0 } };
TEMP R0;
TEX R0.x, fragment.texcoord[0], texture[1], 2D;
TEX result.color.xyz, fragment.texcoord[0], texture[0], 2D;
MAD R0.x, R0, c[0], c[0].y;
ADD R0.y, c[1].w, -c[1].z;
RCP R0.x, R0.x;
ADD R0.x, R0, -R0.y;
CMP R0.x, R0, -R0, c[2];
MUL_SAT result.color.w, R0.x, c[1].x;
END
# 8 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Vector 1 [_CurveParams]
Vector 2 [_MainTex_TexelSize]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_CameraDepthTexture] 2D
"ps_2_0
; 9 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c3, 1.00000000, -1.00000000, 0.00000000, 0
dcl t0.xy
texld r2, t0, s0
mad_pp r0.xy, t0, c3, c3.zxyw
cmp_pp r0.xy, c2.y, t0, r0
texld r0, r0, s1
mad r1.x, r0, c0, c0.y
add_pp r0.x, c1.w, -c1.z
rcp r1.x, r1.x
add r0.x, r1, -r0
cmp_pp r0.x, r0, c3.z, -r0
mul_pp_sat r2.w, r0.x, c1.x
mov_pp oC0, r2
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
Vector 5 [_InvRenderTargetSize]
"!!ARBvp1.0
# 8 ALU
PARAM c[6] = { { 1, -1 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
MOV R0.xy, c[0];
MOV result.texcoord[0].xy, vertex.texcoord[0];
ADD result.texcoord[1].xy, vertex.texcoord[0], -c[5];
MAD result.texcoord[2].xy, R0, c[5], vertex.texcoord[0];
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
Vector 4 [_InvRenderTargetSize]
"vs_2_0
; 8 ALU
def c5, 1.00000000, -1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov oT0.xy, v1
add oT1.xy, v1, -c4
mad oT2.xy, c5, r0, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_InvRenderTargetSize]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 17 ALU, 5 TEX
PARAM c[2] = { program.local[0],
		{ 0, 2, 0.19995117 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEX R1, fragment.texcoord[2], texture[0], 2D;
TEX R4.xyz, fragment.texcoord[0], texture[0], 2D;
MOV R0.y, c[0];
MOV R0.x, c[1];
MAD R0.zw, R0.xyxy, c[1].y, fragment.texcoord[2].xyxy;
MAD R0.xy, R0, c[1].y, fragment.texcoord[1];
TEX R3, R0.zwzw, texture[0], 2D;
TEX R2, R0, texture[0], 2D;
TEX R0, fragment.texcoord[1], texture[0], 2D;
ADD R0.xyz, R0, R1;
ADD R0.xyz, R2, R0;
ADD R0.xyz, R3, R0;
ADD R0.xyz, R4, R0;
MUL result.color.xyz, R0, c[1].z;
MAX R0.y, R2.w, R3.w;
MAX R0.x, R0.w, R1.w;
MAX result.color.w, R0.x, R0.y;
END
# 17 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_InvRenderTargetSize]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 13 ALU, 5 TEX
dcl_2d s0
def c1, 0.00000000, 2.00000000, 0.19995117, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
texld r4, t0, s0
texld r2, t2, s0
texld r3, t1, s0
mov_pp r0.y, c0
mov_pp r0.x, c1
mad_pp r1.xy, r0, c1.y, t1
mad_pp r0.xy, r0, c1.y, t2
add_pp r2.xyz, r3, r2
texld r0, r0, s0
texld r1, r1, s0
add_pp r1.xyz, r1, r2
add_pp r0.xyz, r0, r1
add_pp r0.xyz, r4, r0
mul_pp r1.xyz, r0, c1.z
max_pp r0.x, r1.w, r0.w
max_pp r2.x, r3.w, r2.w
max_pp r1.w, r2.x, r0.x
mov_pp oC0, r1
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
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 1 ALU, 0 TEX
PARAM c[1] = { { 0 } };
MOV result.color, c[0].x;
END
# 1 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
"ps_2_0
; 2 ALU
def c0, 0.00000000, 0, 0, 0
mov_pp r0, c0.x
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
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
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
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 1 ALU, 1 TEX
TEX result.color, fragment.texcoord[0], texture[0], 2D;
END
# 1 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 1 ALU, 1 TEX
dcl_2d s0
dcl t0.xy
texld r0, t0, s0
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
  Blend One One
  ColorMask RGB
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_TexelSize]
"!!ARBvp1.0
# 17 ALU
PARAM c[9] = { { -1.5, 4.5, 0, 1.5 },
		state.matrix.mvp,
		program.local[5],
		{ -2.5, 7.5, 0, 2.5 },
		{ 1.5, -4.5, 0, -1.5 },
		{ 2.5, -7.5, 0, -2.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[0].xyww;
MOV R0.xyz, c[6].xyww;
MOV R2.xyz, c[7].xyww;
MOV R3.xyz, c[8].xyww;
MAD result.texcoord[1].zw, R1.xyxy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[1].xy, R0, c[5], vertex.texcoord[0];
MAD result.texcoord[2].zw, R2.xyxy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2].xy, R3, c[5], vertex.texcoord[0];
MAD result.texcoord[3].zw, R1.xyyz, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3].xy, R0.yzzw, c[5], vertex.texcoord[0];
MAD result.texcoord[4].zw, R2.xyyz, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[4].xy, R3.yzzw, c[5], vertex.texcoord[0];
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 17 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 21 ALU
def c5, -1.50000000, 4.50000000, -2.50000000, 7.50000000
def c6, 1.50000000, -4.50000000, 2.50000000, -7.50000000
def c7, 4.50000000, 1.50000000, 7.50000000, 2.50000000
def c8, -4.50000000, -1.50000000, -7.50000000, -2.50000000
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT1.zw, c5.xyxy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mad oT1.xy, c5.zwzw, r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT2.zw, c6.xyxy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mad oT2.xy, c6.zwzw, r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT3.zw, c7.xyxy, r0.xyxy, v1.xyxy
mad oT3.xy, c7.zwzw, r0.zwzw, v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mov oT0.xy, v1
mad oT4.zw, c8.xyxy, r0.xyxy, v1.xyxy
mad oT4.xy, c8.zwzw, r0.zwzw, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 31 ALU, 9 TEX
PARAM c[4] = { program.local[0],
		{ 0.125, 100, 1000 },
		{ 0.30004883, 0.5, 0.19995117 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEMP R8;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R2, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R1, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R4, fragment.texcoord[4].zwzw, texture[0], 2D;
TEX R3, fragment.texcoord[3].zwzw, texture[0], 2D;
TEX R8, fragment.texcoord[4], texture[0], 2D;
TEX R7, fragment.texcoord[3], texture[0], 2D;
TEX R6, fragment.texcoord[2], texture[0], 2D;
TEX R5, fragment.texcoord[1], texture[0], 2D;
ADD R1, R1, R2;
ADD R5, R5, R6;
ADD R5, R5, R7;
ADD R1, R1, R3;
ADD R5, R5, R8;
ADD R1, R1, R4;
ADD R1, R1, R5;
MUL R2.xyz, R1, c[1].x;
MAD R1.w, R1, c[1].x, -R0;
ABS R1.w, R1;
MUL R1.w, R1, c[0].z;
MUL_SAT R1.w, R1, c[1].y;
MAD R1.xyz, -R1, c[1].x, R0;
MAD R1.xyz, R1.w, R1, R2;
ADD R1.xyz, R0, -R1;
ABS R2.xyz, R1;
DP3 R1.y, R0, c[3];
DP3 R1.x, R2, c[2];
MAD_SAT R1.xy, -R1, R0.w, c[0];
MUL R1.xy, R1, c[1].z;
ADD_SAT R1.x, R1, R1.y;
MAD result.color, R1.x, -R0, R0;
END
# 31 instructions, 9 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 35 ALU, 9 TEX
dcl_2d s0
def c1, 0.12500000, 100.00000000, 1000.00000000, 0
def c2, 0.30004883, 0.50000000, 0.19995117, 0
def c3, 0.21997070, 0.70703125, 0.07098389, 0
dcl t0.xy
dcl t1
dcl t2
dcl t3
dcl t4
texld r8, t0, s0
texld r1, t3, s0
texld r3, t1, s0
texld r2, t2, s0
mov r0.y, t1.w
mov r0.x, t1.z
mov r7.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
mov r6.xy, r0
mov r0.y, t3.w
mov r0.x, t3.z
mov r5.xy, r0
mov r0.y, t4.w
mov r0.x, t4.z
mov r4.xy, r0
add_pp r2, r3, r2
add_pp r1, r2, r1
texld r0, t4, s0
texld r4, r4, s0
texld r5, r5, s0
texld r6, r6, s0
texld r7, r7, s0
add_pp r0, r1, r0
add_pp r6, r7, r6
add_pp r5, r6, r5
add_pp r4, r5, r4
add_pp r1, r4, r0
mul_pp r2.xyz, r1, c1.x
mad_pp r0.x, r1.w, c1, -r8.w
abs_pp r0.x, r0
mul_pp r0.x, r0, c0.z
mad_pp r1.xyz, -r1, c1.x, r8
mul_pp_sat r0.x, r0, c1.y
mad_pp r0.xyz, r0.x, r1, r2
add_pp r0.xyz, r8, -r0
abs_pp r0.xyz, r0
dp3_pp r1.x, r0, c2
dp3_pp r1.y, r8, c3
mad_pp_sat r0.xy, -r1, r8.w, c0
mul_pp r0.xy, r0, c1.z
add_pp_sat r0.x, r0, r0.y
mad_pp r0, r0.x, -r8, r8
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
# 17 ALU
PARAM c[9] = { { -1.5, 4.5, 0, 1.5 },
		state.matrix.mvp,
		program.local[5],
		{ -2.5, 7.5, 0, 2.5 },
		{ 1.5, -4.5, 0, -1.5 },
		{ 2.5, -7.5, 0, -2.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[0].xyww;
MOV R0.xyz, c[6].xyww;
MOV R2.xyz, c[7].xyww;
MOV R3.xyz, c[8].xyww;
MAD result.texcoord[1].zw, R1.xyxy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[1].xy, R0, c[5], vertex.texcoord[0];
MAD result.texcoord[2].zw, R2.xyxy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2].xy, R3, c[5], vertex.texcoord[0];
MAD result.texcoord[3].zw, R1.xyyz, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3].xy, R0.yzzw, c[5], vertex.texcoord[0];
MAD result.texcoord[4].zw, R2.xyyz, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[4].xy, R3.yzzw, c[5], vertex.texcoord[0];
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 17 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 21 ALU
def c5, -1.50000000, 4.50000000, -2.50000000, 7.50000000
def c6, 1.50000000, -4.50000000, 2.50000000, -7.50000000
def c7, 4.50000000, 1.50000000, 7.50000000, 2.50000000
def c8, -4.50000000, -1.50000000, -7.50000000, -2.50000000
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT1.zw, c5.xyxy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mad oT1.xy, c5.zwzw, r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT2.zw, c6.xyxy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mad oT2.xy, c6.zwzw, r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT3.zw, c7.xyxy, r0.xyxy, v1.xyxy
mad oT3.xy, c7.zwzw, r0.zwzw, v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mov oT0.xy, v1
mad oT4.zw, c8.xyxy, r0.xyxy, v1.xyxy
mad oT4.xy, c8.zwzw, r0.zwzw, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 34 ALU, 9 TEX
PARAM c[4] = { program.local[0],
		{ 0, 0.125, 100, 1000 },
		{ 0.30004883, 0.5, 0.19995117 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEMP R8;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R3, fragment.texcoord[3].zwzw, texture[0], 2D;
TEX R2, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R1, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R4, fragment.texcoord[4].zwzw, texture[0], 2D;
TEX R8, fragment.texcoord[4], texture[0], 2D;
TEX R7, fragment.texcoord[3], texture[0], 2D;
TEX R6, fragment.texcoord[2], texture[0], 2D;
TEX R5, fragment.texcoord[1], texture[0], 2D;
ADD R1, R1, R2;
ADD R1, R1, R3;
ADD R5, R5, R6;
ADD R5, R5, R7;
ADD R5, R5, R8;
ADD R1, R1, R4;
ADD R1, R1, R5;
MUL R2.xyz, R1, c[1].y;
MAD R1.w, R1, c[1].y, -R0;
ABS R1.w, R1;
MUL R1.w, R1, c[0].z;
MAD R1.xyz, -R1, c[1].y, R0;
MUL_SAT R1.w, R1, c[1].z;
MAD R1.xyz, R1.w, R1, R2;
ADD R1.xyz, R0, -R1;
ABS R2.xyz, R1;
DP3 R1.y, R0, c[3];
DP3 R1.x, R2, c[2];
MAD_SAT R1.xy, -R1, R0.w, c[0];
MUL R2.xy, R1, c[1].w;
MOV R1.w, R0;
MOV R1.xyz, c[1].x;
ADD R3, R0, -R1;
ADD_SAT R0.x, R2, R2.y;
MAD result.color, R0.x, R3, R1;
END
# 34 instructions, 9 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 38 ALU, 9 TEX
dcl_2d s0
def c1, 0.00000000, 0.12500000, 100.00000000, 1000.00000000
def c2, 0.30004883, 0.50000000, 0.19995117, 0
def c3, 0.21997070, 0.70703125, 0.07098389, 0
dcl t0.xy
dcl t1
dcl t2
dcl t3
dcl t4
texld r8, t0, s0
texld r1, t3, s0
texld r3, t1, s0
texld r2, t2, s0
mov r0.y, t1.w
mov r0.x, t1.z
mov r7.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
mov r6.xy, r0
mov r0.y, t3.w
mov r0.x, t3.z
mov r5.xy, r0
mov r0.y, t4.w
mov r0.x, t4.z
mov r4.xy, r0
add_pp r2, r3, r2
add_pp r1, r2, r1
texld r0, t4, s0
texld r4, r4, s0
texld r5, r5, s0
texld r6, r6, s0
texld r7, r7, s0
add_pp r0, r1, r0
add_pp r6, r7, r6
add_pp r5, r6, r5
add_pp r4, r5, r4
add_pp r1, r4, r0
mad_pp r0.x, r1.w, c1.y, -r8.w
mul_pp r2.xyz, r1, c1.y
abs_pp r0.x, r0
mul_pp r0.x, r0, c0.z
mad_pp r1.xyz, -r1, c1.y, r8
mul_pp_sat r0.x, r0, c1.z
mad_pp r0.xyz, r0.x, r1, r2
add_pp r0.xyz, r8, -r0
abs_pp r0.xyz, r0
dp3_pp r1.x, r0, c2
dp3_pp r1.y, r8, c3
mad_pp_sat r0.xy, -r1, r8.w, c0
mul_pp r0.xy, r0, c1.w
mov_pp r1.w, r8
mov_pp r1.xyz, c1.x
add_pp r2, r8, -r1
add_pp_sat r0.x, r0, r0.y
mad_pp r0, r0.x, r2, r1
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
# 17 ALU
PARAM c[9] = { { -1.5, 4.5, 0, 1.5 },
		state.matrix.mvp,
		program.local[5],
		{ -2.5, 7.5, 0, 2.5 },
		{ 1.5, -4.5, 0, -1.5 },
		{ 2.5, -7.5, 0, -2.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[0].xyww;
MOV R0.xyz, c[6].xyww;
MOV R2.xyz, c[7].xyww;
MOV R3.xyz, c[8].xyww;
MAD result.texcoord[1].zw, R1.xyxy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[1].xy, R0, c[5], vertex.texcoord[0];
MAD result.texcoord[2].zw, R2.xyxy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2].xy, R3, c[5], vertex.texcoord[0];
MAD result.texcoord[3].zw, R1.xyyz, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3].xy, R0.yzzw, c[5], vertex.texcoord[0];
MAD result.texcoord[4].zw, R2.xyyz, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[4].xy, R3.yzzw, c[5], vertex.texcoord[0];
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 17 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 21 ALU
def c5, -1.50000000, 4.50000000, -2.50000000, 7.50000000
def c6, 1.50000000, -4.50000000, 2.50000000, -7.50000000
def c7, 4.50000000, 1.50000000, 7.50000000, 2.50000000
def c8, -4.50000000, -1.50000000, -7.50000000, -2.50000000
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT1.zw, c5.xyxy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mad oT1.xy, c5.zwzw, r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT2.zw, c6.xyxy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mad oT2.xy, c6.zwzw, r0.zwzw, v1
mov r0.zw, c4.xyxy
mad oT3.zw, c7.xyxy, r0.xyxy, v1.xyxy
mad oT3.xy, c7.zwzw, r0.zwzw, v1
mov r0.xy, c4
mov r0.zw, c4.xyxy
mov oT0.xy, v1
mad oT4.zw, c8.xyxy, r0.xyxy, v1.xyxy
mad oT4.xy, c8.zwzw, r0.zwzw, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 31 ALU, 9 TEX
PARAM c[4] = { program.local[0],
		{ 0.125, 100, 1000 },
		{ 0.30004883, 0.5, 0.19995117 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEMP R8;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R2, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R1, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R4, fragment.texcoord[4].zwzw, texture[0], 2D;
TEX R3, fragment.texcoord[3].zwzw, texture[0], 2D;
TEX R8, fragment.texcoord[4], texture[0], 2D;
TEX R7, fragment.texcoord[3], texture[0], 2D;
TEX R6, fragment.texcoord[2], texture[0], 2D;
TEX R5, fragment.texcoord[1], texture[0], 2D;
ADD R1, R1, R2;
ADD R5, R5, R6;
ADD R5, R5, R7;
ADD R1, R1, R3;
ADD R5, R5, R8;
ADD R1, R1, R4;
ADD R1, R1, R5;
MUL R2.xyz, R1, c[1].x;
MAD R1.w, R1, c[1].x, -R0;
ABS R1.w, R1;
MUL R1.w, R1, c[0].z;
MUL_SAT R1.w, R1, c[1].y;
MAD R1.xyz, -R1, c[1].x, R0;
MAD R1.xyz, R1.w, R1, R2;
ADD R1.xyz, R0, -R1;
ABS R2.xyz, R1;
DP3 R1.y, R0, c[3];
DP3 R1.x, R2, c[2];
MAD_SAT R1.xy, -R1, R0.w, c[0];
MUL R1.xy, R1, c[1].z;
ADD_SAT R1.x, R1, R1.y;
MAD result.color, R1.x, -R0, R0;
END
# 31 instructions, 9 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Threshhold]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 35 ALU, 9 TEX
dcl_2d s0
def c1, 0.12500000, 100.00000000, 1000.00000000, 0
def c2, 0.30004883, 0.50000000, 0.19995117, 0
def c3, 0.21997070, 0.70703125, 0.07098389, 0
dcl t0.xy
dcl t1
dcl t2
dcl t3
dcl t4
texld r8, t0, s0
texld r1, t3, s0
texld r3, t1, s0
texld r2, t2, s0
mov r0.y, t1.w
mov r0.x, t1.z
mov r7.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
mov r6.xy, r0
mov r0.y, t3.w
mov r0.x, t3.z
mov r5.xy, r0
mov r0.y, t4.w
mov r0.x, t4.z
mov r4.xy, r0
add_pp r2, r3, r2
add_pp r1, r2, r1
texld r0, t4, s0
texld r4, r4, s0
texld r5, r5, s0
texld r6, r6, s0
texld r7, r7, s0
add_pp r0, r1, r0
add_pp r6, r7, r6
add_pp r5, r6, r5
add_pp r4, r5, r4
add_pp r1, r4, r0
mul_pp r2.xyz, r1, c1.x
mad_pp r0.x, r1.w, c1, -r8.w
abs_pp r0.x, r0
mul_pp r0.x, r0, c0.z
mad_pp r1.xyz, -r1, c1.x, r8
mul_pp_sat r0.x, r0, c1.y
mad_pp r0.xyz, r0.x, r1, r2
add_pp r0.xyz, r8, -r0
abs_pp r0.xyz, r0
dp3_pp r1.x, r0, c2
dp3_pp r1.y, r8, c3
mad_pp_sat r0.xy, -r1, r8.w, c0
mul_pp r0.xy, r0, c1.z
add_pp_sat r0.x, r0, r0.y
mad_pp r0, r0.x, -r8, r8
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}