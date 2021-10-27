Shader "Hidden/FisheyeShader" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
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
Vector 0 [intensity]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 10 ALU, 1 TEX
PARAM c[2] = { program.local[0],
		{ 1, 0.5, 2 } };
TEMP R0;
ADD R0.xy, fragment.texcoord[0], -c[1].y;
MUL R0.xy, R0, c[1].z;
MAD R0.z, -R0.y, R0.y, c[1].x;
MAD R0.w, -R0.x, R0.x, c[1].x;
MUL R0.z, R0, c[0].y;
MUL R0.w, R0, c[0].x;
MUL R0.x, R0, R0.z;
MUL R0.y, R0.w, R0;
ADD R0.xy, fragment.texcoord[0], -R0;
TEX result.color, R0, texture[0], 2D;
END
# 10 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [intensity]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 10 ALU, 1 TEX
dcl_2d s0
def c1, -0.50000000, 2.00000000, 1.00000000, 0
dcl t0.xy
add_pp r0.xy, t0, c1.x
mul_pp r2.xy, r0, c1.y
mad_pp r1.x, -r2.y, r2.y, c1.z
mad_pp r0.x, -r2, r2, c1.z
mul r0.x, r0, c0
mul r1.x, r1, c0.y
mul r1.x, r2, r1
mul r1.y, r0.x, r2
add r0.xy, t0, -r1
texld r0, r0, s0
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}