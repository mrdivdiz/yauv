Shader "Hidden/BrightPassFilterForBloom" {
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
Vector 0 [threshhold]
Float 1 [useSrcAlphaAsMask]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 6 ALU, 1 TEX
PARAM c[3] = { program.local[0..1],
		{ 0, 1 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R1.y, R0.w, -c[2];
MOV R1.x, c[2].y;
MAD R1.x, R1.y, c[1], R1;
MAD R0, R0, R1.x, -c[0].x;
MAX result.color, R0, c[2].x;
END
# 6 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [threshhold]
Float 1 [useSrcAlphaAsMask]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 6 ALU, 1 TEX
dcl_2d s0
def c2, -1.00000000, 1.00000000, 0.00000000, 0
dcl t0.xy
texld r1, t0, s0
mov_pp r0.y, c2
add_pp r0.x, r1.w, c2
mad_pp r0.x, r0, c1, r0.y
mad_pp r0, r1, r0.x, -c0.x
max_pp r0, r0, c2.z
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}