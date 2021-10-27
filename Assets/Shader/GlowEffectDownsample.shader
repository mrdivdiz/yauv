Shader "Hidden/Glow Downsample" {
Properties {
 _Color ("Color", Color) = (1,1,1,0)
 _MainTex ("", 2D) = "white" {}
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
Vector 9 [_MainTex_TexelSize]
"!!ARBvp1.0
# 23 ALU
PARAM c[10] = { { 0, 1 },
		state.matrix.mvp,
		state.matrix.texture[0],
		program.local[9] };
TEMP R0;
TEMP R1;
MOV R1.zw, c[0].x;
MOV R0.zw, c[0].x;
MOV R0.xy, vertex.texcoord[0];
DP4 R1.y, R0, c[6];
DP4 R1.x, R0, c[5];
MOV R0.xy, -c[9];
MOV R0.zw, c[0].xyxy;
ADD result.texcoord[0], R1, R0;
MOV R0.zw, c[0].xyxy;
MOV R0.x, c[9];
MOV R0.y, -c[9];
ADD result.texcoord[1], R1, R0;
MOV R0.xy, c[9];
MOV R0.zw, c[0].xyxy;
ADD result.texcoord[2], R1, R0;
MOV R0.zw, c[0].xyxy;
MOV R0.x, -c[9];
MOV R0.y, c[9];
ADD result.texcoord[3], R1, R0;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 23 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [glstate_matrix_texture0]
Vector 8 [_MainTex_TexelSize]
"vs_2_0
; 27 ALU
def c9, 0.00000000, 2.00000000, 1.00000000, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r1.zw, c9.x
mov r0.zw, c9.x
mov r0.xy, v1
dp4 r1.y, r0, c5
dp4 r0.y, r0, c4
mov r1.x, c8.y
mad r1.y, c9, r1.x, r1
mov r0.x, c8
mad r1.x, c9.y, r0, r0.y
mov r0.xy, -c8
mov r0.zw, c9.xyxz
add oT0, r1, r0
mov r0.zw, c9.xyxz
mov r0.x, c8
mov r0.y, -c8
add oT1, r1, r0
mov r0.xy, c8
mov r0.zw, c9.xyxz
add oT2, r1, r0
mov r0.zw, c9.xyxz
mov r0.x, -c8
mov r0.y, c8
add oT3, r1, r0
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_Color]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 12 ALU, 4 TEX
PARAM c[2] = { program.local[0],
		{ 0.25, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R3, fragment.texcoord[3], texture[0], 2D;
TEX R2, fragment.texcoord[2], texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
MUL R0, R0, c[1].x;
ADD R0.w, R0, c[0];
MUL R0.xyz, R0, c[0];
MUL result.color.xyz, R0, R0.w;
MOV result.color.w, c[1].y;
END
# 12 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Color]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 9 ALU, 4 TEX
dcl_2d s0
def c1, 0.25000000, 0.00000000, 0, 0
dcl t0.xy
dcl t1.xy
dcl t2.xy
dcl t3.xy
texld r0, t3, s0
texld r1, t2, s0
texld r2, t1, s0
texld r3, t0, s0
add_pp r2, r3, r2
add_pp r1, r2, r1
add_pp r0, r1, r0
mul_pp r0, r0, c1.x
add_pp r1.x, r0.w, c0.w
mul_pp r0.xyz, r0, c0
mul_pp r0.xyz, r0, r1.x
mov_pp r0.w, c1.y
mov_pp oC0, r0
"
}
}
 }
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
Vector 9 [_MainTex_TexelSize]
"!!ARBvp1.0
# 23 ALU
PARAM c[10] = { { 0, 1 },
		state.matrix.mvp,
		state.matrix.texture[0],
		program.local[9] };
TEMP R0;
TEMP R1;
MOV R1.zw, c[0].x;
MOV R0.zw, c[0].x;
MOV R0.xy, vertex.texcoord[0];
DP4 R1.y, R0, c[6];
DP4 R1.x, R0, c[5];
MOV R0.xy, -c[9];
MOV R0.zw, c[0].xyxy;
ADD result.texcoord[0], R1, R0;
MOV R0.zw, c[0].xyxy;
MOV R0.x, c[9];
MOV R0.y, -c[9];
ADD result.texcoord[1], R1, R0;
MOV R0.xy, c[9];
MOV R0.zw, c[0].xyxy;
ADD result.texcoord[2], R1, R0;
MOV R0.zw, c[0].xyxy;
MOV R0.x, -c[9];
MOV R0.y, c[9];
ADD result.texcoord[3], R1, R0;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 23 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [glstate_matrix_texture0]
Vector 8 [_MainTex_TexelSize]
"vs_2_0
; 27 ALU
def c9, 0.00000000, 2.00000000, 1.00000000, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r1.zw, c9.x
mov r0.zw, c9.x
mov r0.xy, v1
dp4 r1.y, r0, c5
dp4 r0.y, r0, c4
mov r1.x, c8.y
mad r1.y, c9, r1.x, r1
mov r0.x, c8
mad r1.x, c9.y, r0, r0.y
mov r0.xy, -c8
mov r0.zw, c9.xyxz
add oT0, r1, r0
mov r0.zw, c9.xyxz
mov r0.x, c8
mov r0.y, -c8
add oT1, r1, r0
mov r0.xy, c8
mov r0.zw, c9.xyxz
add oT2, r1, r0
mov r0.zw, c9.xyxz
mov r0.x, -c8
mov r0.y, c8
add oT3, r1, r0
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
  SetTexture [_MainTex] { ConstantColor (0,0,0,0.25) combine texture * constant alpha }
  SetTexture [_MainTex] { ConstantColor (0,0,0,0.25) combine texture * constant + previous }
  SetTexture [_MainTex] { ConstantColor (0,0,0,0.25) combine texture * constant + previous }
  SetTexture [_MainTex] { ConstantColor (0,0,0,0.25) combine texture * constant + previous }
  SetTexture [_MainTex] { ConstantColor [_Color] combine previous * constant, previous alpha + constant alpha }
  SetTexture [_MainTex] { ConstantColor (0,0,0,0) combine previous * previous alpha, constant alpha }
 }
}
Fallback Off
}