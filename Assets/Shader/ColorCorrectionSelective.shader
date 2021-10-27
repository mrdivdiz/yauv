Shader "Hidden/ColorCorrectionSelective" {
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
Vector 0 [selColor]
Vector 1 [targetColor]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 8 ALU, 1 TEX
PARAM c[3] = { program.local[0..1],
		{ 1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R1.xyz, R0, -c[0];
DP3 R1.x, R1, R1;
RSQ R1.x, R1.x;
RCP_SAT R2.x, R1.x;
ADD R1, -R0, c[1];
ADD R2.x, -R2, c[2];
MAD result.color, R2.x, R1, R0;
END
# 8 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [selColor]
Vector 1 [targetColor]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 8 ALU, 1 TEX
dcl_2d s0
def c2, 1.00000000, 0, 0, 0
dcl t0.xy
texld r1, t0, s0
add r0.xyz, r1, -c0
dp3 r0.x, r0, r0
rsq r0.x, r0.x
rcp_sat r0.x, r0.x
add r2, -r1, c1
add r0.x, -r0, c2
mad r0, r0.x, r2, r1
mov oC0, r0
"
}
}
 }
}
Fallback Off
}