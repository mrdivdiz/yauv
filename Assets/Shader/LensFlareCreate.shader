Shader "Hidden/LensFlareCreate" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend One One
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
# 10 ALU
PARAM c[6] = { { 0.5, -0.85009766, -1.4501953, -2.5507813 },
		state.matrix.mvp,
		{ -4.1484375 } };
TEMP R0;
ADD R0.zw, vertex.texcoord[0].xyxy, -c[0].x;
MOV R0.x, c[0];
MAD result.texcoord[3].xy, R0.zwzw, c[5].x, R0.x;
MAD result.texcoord[0].xy, R0.zwzw, c[0].y, c[0].x;
MAD result.texcoord[1].xy, R0.zwzw, c[0].z, c[0].x;
MAD result.texcoord[2].xy, R0.zwzw, c[0].w, c[0].x;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
"vs_2_0
; 9 ALU
def c4, -0.50000000, -0.85009766, 0.50000000, -1.45019531
def c5, -2.55078125, 0.50000000, -4.14843750, 0
dcl_position0 v0
dcl_texcoord0 v1
add r0.xy, v1, c4.x
mad oT0.xy, r0, c4.y, c4.z
mad oT1.xy, r0, c4.w, c4.z
mad oT2.xy, r0, c5.x, c5.y
mad oT3.xy, r0, c5.z, c5.y
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [colorA]
Vector 1 [colorB]
Vector 2 [colorC]
Vector 3 [colorD]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 11 ALU, 4 TEX
PARAM c[4] = { program.local[0..3] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R2, fragment.texcoord[2], texture[0], 2D;
TEX R3, fragment.texcoord[3], texture[0], 2D;
MUL R1, R1, c[1];
MUL R0, R0, c[0];
ADD R0, R0, R1;
MUL R1, R2, c[2];
MUL R2, R3, c[3];
ADD R0, R0, R1;
ADD result.color, R0, R2;
END
# 11 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [colorA]
Vector 1 [colorB]
Vector 2 [colorC]
Vector 3 [colorD]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 8 ALU, 4 TEX
dcl_2d s0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
texld r0, t3, s0
texld r1, t2, s0
texld r2, t1, s0
texld r3, t0, s0
mul r2, r2, c1
mul r3, r3, c0
add_pp r2, r3, r2
mul r1, r1, c2
mul r0, r0, c3
add_pp r1, r2, r1
add_pp r0, r1, r0
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}