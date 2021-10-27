Shader "Hidden/VignetteShader" {
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
Float 0 [vignetteIntensity]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 1 TEX
PARAM c[2] = { program.local[0],
		{ 1, 0.5, 2 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R1.xy, fragment.texcoord[0], -c[1].y;
MUL R1.xy, R1, c[1].z;
MUL R1.xy, R1, R1;
ADD R1.x, R1, R1.y;
MUL R1.x, -R1, c[0];
ADD R1.x, R1, c[1];
MUL result.color, R0, R1.x;
END
# 8 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [vignetteIntensity]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 8 ALU, 1 TEX
dcl_2d s0
def c1, -0.50000000, 2.00000000, 1.00000000, 0
dcl t0.xy
texld r1, t0, s0
add_pp r0.xy, t0, c1.x
mul_pp r0.xy, r0, c1.y
mul_pp r0.xy, r0, r0
add_pp r0.x, r0, r0.y
mul r0.x, -r0, c0
add r0.x, r0, c1.z
mul r0, r1, r0.x
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}