Shader "Hidden/CC_FastVignette" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _sharpness ("Sharpness", Range(-1,1)) = 0.1
 _darkness ("Darkness", Range(0,2)) = 0.3
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
"vs_2_0
; 8 ALU
def c8, 0.00000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.zw, c8.x
mov r0.xy, v1
dp4 oT0.y, r0, c5
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
Float 0 [_sharpness]
Float 1 [_darkness]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 19 ALU, 1 TEX
PARAM c[4] = { program.local[0..1],
		{ 0.5, 0.79980469, 0.79882813, 3 },
		{ 2 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R1.xy, -fragment.texcoord[0], c[2].x;
MUL R1.xy, R1, R1;
ADD R1.y, R1.x, R1;
MOV R1.x, c[0];
ADD R1.z, R1.x, c[1].x;
RSQ R1.y, R1.y;
RCP R1.y, R1.y;
MUL R1.y, R1, R1.z;
ADD R1.z, R1.y, -c[2].y;
MAD R1.x, R1, c[2].z, -c[2].y;
RCP R1.y, R1.x;
MUL_SAT R1.y, R1.z, R1;
MOV R1.x, c[2].w;
MAD R1.z, -R1.y, c[3].x, R1.x;
MUL R1.x, R1.y, R1.y;
MUL R1.x, R1, R1.z;
MUL result.color.xyz, R0, R1.x;
MOV result.color.w, R0;
END
# 19 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_sharpness]
Float 1 [_darkness]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 19 ALU, 1 TEX
dcl_2d s0
def c2, 0.50000000, -0.79980469, 0.79882813, 0
def c3, 2.00000000, 3.00000000, 0, 0
dcl t0.xy
texld r3, t0, s0
add_pp r0.xy, -t0, c2.x
mul_pp r0.xy, r0, r0
add_pp r0.x, r0, r0.y
rsq_pp r0.x, r0.x
mov_pp r2.x, c1
mov_pp r1.x, c0
mad_pp r1.x, r1, c2.z, c2.y
rcp_pp r0.x, r0.x
add_pp r2.x, c0, r2
mul r0.x, r0, r2
add_pp r0.x, r0, c2.y
rcp_pp r1.x, r1.x
mul_pp_sat r1.x, r0, r1
mad_pp r0.x, -r1, c3, c3.y
mul_pp r1.x, r1, r1
mul_pp r0.x, r1, r0
mov_pp r0.w, r3
mul_pp r0.xyz, r3, r0.x
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}