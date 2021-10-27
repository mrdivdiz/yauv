Shader "Hidden/VignettingShader" {
Properties {
 _MainTex ("Base", 2D) = "" {}
 _VignetteTex ("Vignette", 2D) = "" {}
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
Float 0 [_Intensity]
Float 1 [_Blur]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_VignetteTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 12 ALU, 2 TEX
PARAM c[3] = { program.local[0..1],
		{ 0.5, 2, 1, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[0], texture[1], 2D;
ADD R2.xy, fragment.texcoord[0], -c[2].x;
MUL R2.xy, R2, c[2].y;
MUL R2.xy, R2, R2;
ADD R2.x, R2, R2.y;
MUL R2.y, R2.x, c[0].x;
ADD R1, R1, -R0;
MUL_SAT R2.x, R2, c[1];
MAD R2.y, -R2, c[2].w, c[2].z;
MAD R0, R2.x, R1, R0;
MUL result.color, R0, R2.y;
END
# 12 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_Intensity]
Float 1 [_Blur]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_VignetteTex] 2D
"ps_2_0
; 11 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c2, -0.50000000, 2.00000000, 0.10000000, 1.00000000
dcl t0.xy
texld r2, t0, s0
texld r3, t0, s1
add_pp r0.xy, t0, c2.x
mul_pp r0.xy, r0, c2.y
mul_pp r0.xy, r0, r0
add_pp r0.x, r0, r0.y
mul r1.x, r0, c0
add_pp r3, r3, -r2
mul_sat r0.x, r0, c1
mad r1.x, -r1, c2.z, c2.w
mad_pp r0, r0.x, r3, r2
mul r0, r0, r1.x
mov_pp oC0, r0
"
}
}
 }
}
Fallback Off
}