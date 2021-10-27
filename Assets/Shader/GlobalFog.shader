Shader "Hidden/GlobalFog" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "black" {}
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
Matrix 5 [_FrustumCornersWS]
"!!ARBvp1.0
# 10 ALU
PARAM c[9] = { { 0.1 },
		state.matrix.mvp,
		program.local[5..8] };
TEMP R0;
ADDRESS A0;
ARL A0.x, vertex.position.z;
MOV R0.xyw, vertex.position;
MOV R0.z, c[0].x;
DP4 result.position.w, R0, c[4];
DP4 result.position.z, R0, c[3];
DP4 result.position.y, R0, c[2];
DP4 result.position.x, R0, c[1];
MOV result.texcoord[1].xyz, c[A0.x + 5];
MOV result.texcoord[0].xy, vertex.texcoord[0];
MOV result.texcoord[1].w, vertex.position.z;
END
# 10 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_FrustumCornersWS]
Vector 8 [_MainTex_TexelSize]
"vs_2_0
; 18 ALU
dcl_position0 v0
dcl_texcoord0 v1
def c9, 0.10000000, 0.00000000, 1.00000000, 0
mov r1.x, c9.y
slt r1.x, c8.y, r1
max r1.x, -r1, r1
mova a0.x, v0.z
mov r0.xyw, v0
mov r0.z, c9.x
dp4 oPos.w, r0, c3
dp4 oPos.z, r0, c2
dp4 oPos.y, r0, c1
slt r1.x, c9.y, r1
dp4 oPos.x, r0, c0
add r0.x, -r1, c9.z
mul r0.y, v1, r0.x
add r0.x, -v1.y, c9.z
mad oT0.y, r1.x, r0.x, r0
mov oT1.xyz, c[a0.x + 4]
mov oT0.x, v1
mov oT1.w, v0.z
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
Float 1 [_GlobalDensity]
Vector 2 [_FogColor]
Vector 3 [_StartDistance]
Vector 4 [_Y]
Vector 5 [_CameraWS]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 23 ALU, 2 TEX
PARAM c[7] = { program.local[0..5],
		{ 1, 2.718282, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1.x, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MAD R1.x, R1, c[0], c[0].y;
RCP R1.x, R1.x;
MUL R2.xyz, R1.x, fragment.texcoord[1];
DP3 R1.x, R2, R2;
RSQ R2.x, R1.x;
RCP R2.x, R2.x;
ADD R2.y, R2, c[5];
MUL R2.x, R2, c[3];
ADD R2.y, R2, -c[4].x;
ADD_SAT R2.x, R2, -c[6];
MUL R2.y, R2, c[4];
MUL R2.x, R2, c[3].y;
MAX R2.y, R2, c[6].z;
MUL R2.x, R2, -c[1];
MUL R2.y, R2, R2;
POW R2.x, c[6].y, R2.x;
ADD R1, -R0, c[2];
POW R2.y, c[6].y, -R2.y;
ADD R2.x, -R2, c[6];
MUL R2.x, R2, R2.y;
MAD result.color, R2.x, R1, R0;
END
# 23 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Float 1 [_GlobalDensity]
Vector 2 [_FogColor]
Vector 3 [_StartDistance]
Vector 4 [_Y]
Vector 5 [_CameraWS]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 27 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c6, -1.00000000, 0.00000000, 2.71828198, 1.00000000
dcl t0.xy
dcl t1.xyz
texld r0, t0, s0
texld r2, t0, s1
mad r0.x, r0, c0, c0.y
rcp r0.x, r0.x
mul r0.xyz, r0.x, t1
dp3 r0.x, r0, r0
rsq r0.x, r0.x
rcp r0.x, r0.x
mul r1.x, r0, c3
add r0.y, r0, c5
add r0.x, r0.y, -c4
add_sat r1.x, r1, c6
mul r0.x, r0, c4.y
mul r1.x, r1, c3.y
mul r1.x, r1, -c1
pow r3.x, c6.z, r1.x
max r0.x, r0, c6.y
mul r0.x, r0, r0
pow r1.x, c6.z, -r0.x
mov r0.x, r3.x
add r4, -r2, c2
add r0.x, -r0, c6.w
mul r0.x, r0, r1.x
mad r0, r0.x, r4, r2
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
Matrix 5 [_FrustumCornersWS]
"!!ARBvp1.0
# 10 ALU
PARAM c[9] = { { 0.1 },
		state.matrix.mvp,
		program.local[5..8] };
TEMP R0;
ADDRESS A0;
ARL A0.x, vertex.position.z;
MOV R0.xyw, vertex.position;
MOV R0.z, c[0].x;
DP4 result.position.w, R0, c[4];
DP4 result.position.z, R0, c[3];
DP4 result.position.y, R0, c[2];
DP4 result.position.x, R0, c[1];
MOV result.texcoord[1].xyz, c[A0.x + 5];
MOV result.texcoord[0].xy, vertex.texcoord[0];
MOV result.texcoord[1].w, vertex.position.z;
END
# 10 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_FrustumCornersWS]
Vector 8 [_MainTex_TexelSize]
"vs_2_0
; 18 ALU
dcl_position0 v0
dcl_texcoord0 v1
def c9, 0.10000000, 0.00000000, 1.00000000, 0
mov r1.x, c9.y
slt r1.x, c8.y, r1
max r1.x, -r1, r1
mova a0.x, v0.z
mov r0.xyw, v0
mov r0.z, c9.x
dp4 oPos.w, r0, c3
dp4 oPos.z, r0, c2
dp4 oPos.y, r0, c1
slt r1.x, c9.y, r1
dp4 oPos.x, r0, c0
add r0.x, -r1, c9.z
mul r0.y, v1, r0.x
add r0.x, -v1.y, c9.z
mad oT0.y, r1.x, r0.x, r0
mov oT1.xyz, c[a0.x + 4]
mov oT0.x, v1
mov oT1.w, v0.z
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
Vector 1 [_FogColor]
Vector 2 [_Y]
Vector 3 [_CameraWS]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 12 ALU, 2 TEX
PARAM c[5] = { program.local[0..3],
		{ 2.718282, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1.x, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MAD R1.x, R1, c[0], c[0].y;
RCP R1.x, R1.x;
MAD R1.x, R1, fragment.texcoord[1].y, c[3].y;
ADD R1.x, R1, -c[2];
MUL R1.x, R1, c[2].y;
MAX R1.x, R1, c[4].y;
MUL R2.x, R1, R1;
ADD R1, -R0, c[1];
POW R2.x, c[4].x, -R2.x;
MAD result.color, R2.x, R1, R0;
END
# 12 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Vector 1 [_FogColor]
Vector 2 [_Y]
Vector 3 [_CameraWS]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 13 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c4, 0.00000000, 2.71828198, 0, 0
dcl t0.xy
dcl t1.xy
texld r0, t0, s0
texld r1, t0, s1
mad r0.x, r0, c0, c0.y
rcp r0.x, r0.x
mad r0.y, r0.x, t1, c3
add r0.x, r0.y, -c2
mul r0.x, r0, c2.y
max r0.x, r0, c4
mul r0.x, r0, r0
pow r2.x, c4.y, -r0.x
add r0, -r1, c1
mad r0, r2.x, r0, r1
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
Matrix 5 [_FrustumCornersWS]
"!!ARBvp1.0
# 10 ALU
PARAM c[9] = { { 0.1 },
		state.matrix.mvp,
		program.local[5..8] };
TEMP R0;
ADDRESS A0;
ARL A0.x, vertex.position.z;
MOV R0.xyw, vertex.position;
MOV R0.z, c[0].x;
DP4 result.position.w, R0, c[4];
DP4 result.position.z, R0, c[3];
DP4 result.position.y, R0, c[2];
DP4 result.position.x, R0, c[1];
MOV result.texcoord[1].xyz, c[A0.x + 5];
MOV result.texcoord[0].xy, vertex.texcoord[0];
MOV result.texcoord[1].w, vertex.position.z;
END
# 10 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_FrustumCornersWS]
Vector 8 [_MainTex_TexelSize]
"vs_2_0
; 18 ALU
dcl_position0 v0
dcl_texcoord0 v1
def c9, 0.10000000, 0.00000000, 1.00000000, 0
mov r1.x, c9.y
slt r1.x, c8.y, r1
max r1.x, -r1, r1
mova a0.x, v0.z
mov r0.xyw, v0
mov r0.z, c9.x
dp4 oPos.w, r0, c3
dp4 oPos.z, r0, c2
dp4 oPos.y, r0, c1
slt r1.x, c9.y, r1
dp4 oPos.x, r0, c0
add r0.x, -r1, c9.z
mul r0.y, v1, r0.x
add r0.x, -v1.y, c9.z
mad oT0.y, r1.x, r0.x, r0
mov oT1.xyz, c[a0.x + 4]
mov oT0.x, v1
mov oT1.w, v0.z
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
Float 1 [_GlobalDensity]
Vector 2 [_FogColor]
Vector 3 [_StartDistance]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 15 ALU, 2 TEX
PARAM c[5] = { program.local[0..3],
		{ 2.718282, 1 } };
TEMP R0;
TEMP R1;
TEX R1.x, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MAD R1.x, R1, c[0], c[0].y;
RCP R1.x, R1.x;
MUL R1, R1.x, fragment.texcoord[1];
DP4 R1.x, R1, R1;
RSQ R1.x, R1.x;
RCP R1.x, R1.x;
MUL R1.x, R1, c[3];
ADD_SAT R1.x, R1, -c[4].y;
MUL R1.x, R1, c[3].y;
MUL R1.x, R1, -c[1];
ADD R0, R0, -c[2];
POW R1.x, c[4].x, R1.x;
MAD result.color, R1.x, R0, c[2];
END
# 15 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Float 1 [_GlobalDensity]
Vector 2 [_FogColor]
Vector 3 [_StartDistance]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 17 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c4, -1.00000000, 2.71828198, 0, 0
dcl t0.xy
dcl t1
texld r0, t0, s0
texld r2, t0, s1
mad r0.x, r0, c0, c0.y
rcp r0.x, r0.x
mul r0, r0.x, t1
dp4 r0.x, r0, r0
rsq r0.x, r0.x
rcp r0.x, r0.x
mul r0.x, r0, c3
add_sat r0.x, r0, c4
mul r0.x, r0, c3.y
mul r0.x, r0, -c1
pow r1.x, c4.y, r0.x
add r2, r2, -c2
mov r0.x, r1.x
mad r0, r0.x, r2, c2
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
Matrix 5 [_FrustumCornersWS]
"!!ARBvp1.0
# 10 ALU
PARAM c[9] = { { 0.1 },
		state.matrix.mvp,
		program.local[5..8] };
TEMP R0;
ADDRESS A0;
ARL A0.x, vertex.position.z;
MOV R0.xyw, vertex.position;
MOV R0.z, c[0].x;
DP4 result.position.w, R0, c[4];
DP4 result.position.z, R0, c[3];
DP4 result.position.y, R0, c[2];
DP4 result.position.x, R0, c[1];
MOV result.texcoord[1].xyz, c[A0.x + 5];
MOV result.texcoord[0].xy, vertex.texcoord[0];
MOV result.texcoord[1].w, vertex.position.z;
END
# 10 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_FrustumCornersWS]
Vector 8 [_MainTex_TexelSize]
"vs_2_0
; 18 ALU
dcl_position0 v0
dcl_texcoord0 v1
def c9, 0.10000000, 0.00000000, 1.00000000, 0
mov r1.x, c9.y
slt r1.x, c8.y, r1
max r1.x, -r1, r1
mova a0.x, v0.z
mov r0.xyw, v0
mov r0.z, c9.x
dp4 oPos.w, r0, c3
dp4 oPos.z, r0, c2
dp4 oPos.y, r0, c1
slt r1.x, c9.y, r1
dp4 oPos.x, r0, c0
add r0.x, -r1, c9.z
mul r0.y, v1, r0.x
add r0.x, -v1.y, c9.z
mad oT0.y, r1.x, r0.x, r0
mov oT1.xyz, c[a0.x + 4]
mov oT0.x, v1
mov oT1.w, v0.z
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
Float 1 [_GlobalDensity]
Vector 2 [_FogColor]
Vector 3 [_StartDistance]
Vector 4 [_Y]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 22 ALU, 2 TEX
PARAM c[6] = { program.local[0..4],
		{ 1, 2.718282, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1.x, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MAD R1.x, R1, c[0], c[0].y;
RCP R1.x, R1.x;
MUL R2.xyz, R1.x, fragment.texcoord[1];
DP3 R1.x, R2, R2;
RSQ R1.x, R1.x;
RCP R2.x, R1.x;
MUL R2.x, R2, c[3];
ADD R2.y, R2, -c[4].x;
ADD_SAT R2.x, R2, -c[5];
MUL R2.y, R2, c[4];
MUL R2.x, R2, c[3].y;
MAX R2.y, R2, c[5].z;
MUL R2.x, R2, -c[1];
MUL R2.y, R2, R2;
POW R2.x, c[5].y, R2.x;
ADD R1, -R0, c[2];
POW R2.y, c[5].y, -R2.y;
ADD R2.x, -R2, c[5];
MUL R2.x, R2, R2.y;
MAD result.color, R2.x, R1, R0;
END
# 22 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Float 1 [_GlobalDensity]
Vector 2 [_FogColor]
Vector 3 [_StartDistance]
Vector 4 [_Y]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 26 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c5, -1.00000000, 0.00000000, 2.71828198, 1.00000000
dcl t0.xy
dcl t1.xyz
texld r0, t0, s0
texld r2, t0, s1
mad r0.x, r0, c0, c0.y
rcp r0.x, r0.x
mul r0.xyz, r0.x, t1
dp3 r0.x, r0, r0
rsq r0.x, r0.x
rcp r0.x, r0.x
mul r1.x, r0, c3
add r0.x, r0.y, -c4
add_sat r1.x, r1, c5
mul r0.x, r0, c4.y
mul r1.x, r1, c3.y
mul r1.x, r1, -c1
pow r3.x, c5.z, r1.x
max r0.x, r0, c5.y
mul r0.x, r0, r0
pow r1.x, c5.z, -r0.x
mov r0.x, r3.x
add r4, -r2, c2
add r0.x, -r0, c5.w
mul r0.x, r0, r1.x
mad r0, r0.x, r4, r2
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}