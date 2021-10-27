Shader "Hidden/ChromaticAberrationShader" {
Properties {
 _MainTex ("Base", 2D) = "" {}
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
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 13 ALU
dcl_position0 v0
dcl_texcoord0 v1
def c5, 0.00000000, 1.00000000, 0, 0
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT0.y, r0.x, r0, r0.z
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT0.x, v1
"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
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
Vector 4 [_MainTex_TexelSize]
"vs_2_0
; 13 ALU
dcl_position0 v0
dcl_texcoord0 v1
def c5, 0.00000000, 1.00000000, 0, 0
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT0.y, r0.x, r0, r0.z
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT0.x, v1
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_MainTex_TexelSize]
Float 1 [_ChromaticAberration]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
# 10 ALU, 2 TEX
PARAM c[3] = { program.local[0..1],
		{ 0.5, 2 } };
TEMP R0;
TEMP R1;
TEX result.color.xzw, fragment.texcoord[0], texture[0], 2D;
ADD R0.xy, fragment.texcoord[0], -c[2].x;
MUL R0.zw, R0.xyxy, c[2].y;
MOV R0.x, c[1];
MUL R1.xy, R0.zwzw, R0.zwzw;
MUL R0.xy, R0.x, c[0];
ADD R1.x, R1, R1.y;
MUL R0.xy, R0, R0.zwzw;
MAD R0.xy, -R0, R1.x, fragment.texcoord[0];
TEX result.color.y, R0, texture[0], 2D;
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_MainTex_TexelSize]
Float 1 [_ChromaticAberration]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 11 ALU, 2 TEX
dcl_2d s0
def c2, -0.50000000, 2.00000000, 0.00010002, 0
dcl t0.xy
add_pp r0.xy, t0, c2.x
mul_pp r1.xy, r0, c2.y
mul_pp r0.xy, r1, r1
mov r2.xy, c0
mul r2.xy, c1.x, r2
mul r1.xy, r2, r1
add_pp r0.x, r0, r0.y
mad r0.xy, -r1, r0.x, t0
texld r0, r0, s0
texld r1, t0, s0
mul_pp r0.x, r1.y, c2.z
add r1.y, r0.x, r0
mov_pp oC0, r1
"
}
}
 }
}
Fallback Off
}