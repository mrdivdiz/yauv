Shader "Hidden/dirtFxBasic" {
Properties {
 _MainTex ("Input", 2D) = "white" {}
 _OrgTex ("Input", 2D) = "white" {}
 _lensDirt ("Input", 2D) = "white" {}
 _threshold ("", Float) = 0.5
 _int ("", Float) = 1
 _haloint ("", Float) = 1
 _visualize ("", Float) = 1
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
Vector 8 [_OrgTex_TexelSize]
"vs_2_0
; 16 ALU
def c9, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c9
slt r0.x, c8.y, r0
max r0.z, -r0.x, r0.x
slt r1.x, c9, r0.z
mov r0.xy, v1
mov r0.zw, c9.x
dp4 r1.y, r0, c5
add r1.z, -r1.x, c9.y
mul r1.z, r1.y, r1
add r1.y, -r1, c9
mad oT0.y, r1.x, r1, r1.z
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
Float 0 [_int]
Float 1 [_threshold]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 13 ALU, 1 TEX
PARAM c[4] = { program.local[0..1],
		{ 0.29890001, 0.58660001, 0.1145, 1 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
MUL R0.w, R0.y, c[2].y;
MAD R0.w, R0.x, c[2].x, R0;
MAD_SAT R1.x, R0.z, c[2].z, R0.w;
ADD R1.xyz, R1.x, -R0;
MAD R1.xyz, R1, c[1].x, R0;
MOV R0.w, c[2];
ADD R0.w, R0, -c[1].x;
RCP R0.x, R0.w;
ADD R1.xyz, R1, -c[1].x;
MUL_SAT R0.xyz, R1, R0.x;
MUL R0.xyz, R0, c[0].x;
DP3 result.color, R0, c[3];
END
# 13 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_int]
Float 1 [_threshold]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 14 ALU, 1 TEX
dcl_2d s0
def c2, 1.00000000, 0.58660001, 0.29890001, 0.11450000
def c3, 0.21997070, 0.70703125, 0.07098389, 0
dcl t0.xy
texld r1, t0, s0
mul r0.x, r1.y, c2.y
mad r0.x, r1, c2.z, r0
mad_sat r2.x, r1.z, c2.w, r0
mov_pp r0.x, c1
add r2.xyz, r2.x, -r1
add_pp r0.x, c2, -r0
mad r1.xyz, r2, c1.x, r1
rcp_pp r0.x, r0.x
add r1.xyz, r1, -c1.x
mul_sat r0.xyz, r1, r0.x
mul r0.xyz, r0, c0.x
dp3_pp r0.x, r0, c3
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
Vector 8 [_OrgTex_TexelSize]
"vs_2_0
; 16 ALU
def c9, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c9
slt r0.x, c8.y, r0
max r0.z, -r0.x, r0.x
slt r1.x, c9, r0.z
mov r0.xy, v1
mov r0.zw, c9.x
dp4 r1.y, r0, c5
add r1.z, -r1.x, c9.y
mul r1.z, r1.y, r1
add r1.y, -r1, c9
mad oT0.y, r1.x, r1, r1.z
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
Float 0 [_int]
Float 1 [_visualize]
Vector 2 [tintColor]
SetTexture 0 [_OrgTex] 2D
SetTexture 1 [_MainTex] 2D
SetTexture 2 [_lensDirt] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 3 TEX
PARAM c[4] = { program.local[0..2],
		{ 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R2.xyz, fragment.texcoord[0], texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[0], texture[2], 2D;
MUL R2.xyz, R2, c[2];
MOV R2.w, c[3].x;
MUL R1, R2, R1;
MUL R1, R1, c[0].x;
MAD result.color, R0, c[1].x, R1;
END
# 8 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_int]
Float 1 [_visualize]
Vector 2 [tintColor]
SetTexture 0 [_OrgTex] 2D
SetTexture 1 [_MainTex] 2D
SetTexture 2 [_lensDirt] 2D
"ps_2_0
; 6 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c3, 0.00000000, 0, 0, 0
dcl t0.xy
texld r0, t0, s1
texld r2, t0, s0
texld r1, t0, s2
mul r0.xyz, r0, c2
mov r0.w, c3.x
mul r0, r0, r1
mul r0, r0, c0.x
mad r0, r2, c1.x, r0
mov_pp oC0, r0
"
}
}
 }
}
}