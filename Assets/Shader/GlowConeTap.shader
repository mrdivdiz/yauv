Shader "Hidden/GlowConeTap" {
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
Vector 10 [_BlurOffsets]
"!!ARBvp1.0
# 20 ALU
PARAM c[11] = { { 0 },
		state.matrix.mvp,
		state.matrix.texture[0],
		program.local[9..10] };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R0.xy, c[9];
MUL R0.zw, R0.xyxy, c[10].xyxy;
ADD R1.xy, vertex.texcoord[0], -R0.zwzw;
MOV R1.zw, c[0].x;
DP4 R2.y, R1, c[6];
DP4 R2.x, R1, c[5];
MOV R0.xy, c[10];
MUL R1.y, R0, c[9];
MUL R1.x, R0, c[9];
MOV R0.y, R1;
MOV R0.x, -R1;
MOV R1.y, -R1;
ADD result.texcoord[0].zw, R2.xyxy, R0.xyxy;
ADD result.texcoord[0].xy, R2, R0.zwzw;
ADD result.texcoord[1].zw, R2.xyxy, -R0;
ADD result.texcoord[1].xy, R2, R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 20 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [glstate_matrix_texture0]
Vector 8 [_MainTex_TexelSize]
Vector 9 [_BlurOffsets]
"vs_2_0
; 21 ALU
def c10, 0.00000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c9
mul r1.xy, c8, r0
mov r0.zw, c10.x
add r0.xy, v1, -r1
dp4 r1.w, r0, c5
dp4 r1.z, r0, c4
mov r0.y, c8
mul r0.y, c9, r0
mov r0.w, r0.y
mov r0.x, c8
mul r0.x, c9, r0
mov r0.z, -r0.x
mov r0.y, -r0
add oT0.zw, r1, r0
add oT0.xy, r1.zwzw, r1
add oT1.zw, r1, -r1.xyxy
add oT1.xy, r1.zwzw, r0
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
# 9 ALU, 4 TEX
PARAM c[1] = { program.local[0] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R3, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R2, fragment.texcoord[1], texture[0], 2D;
TEX R1, fragment.texcoord[0].zwzw, texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R0, R0, R1;
ADD R0, R0, R2;
ADD R0, R0, R3;
MUL R0.xyz, R0, c[0];
MUL result.color, R0, c[0].w;
END
# 9 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Color]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 11 ALU, 4 TEX
dcl_2d s0
dcl t0
dcl t1
texld r3, t0, s0
mov r1.y, t0.w
mov r1.x, t0.z
mov r2.xy, r1
mov r0.y, t1.w
mov r0.x, t1.z
texld r0, r0, s0
texld r1, t1, s0
texld r2, r2, s0
add_pp r2, r3, r2
add_pp r1, r2, r1
add_pp r0, r1, r0
mul_pp r0.xyz, r0, c0
mul_pp r0, r0, c0.w
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
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant alpha }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant + previous }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant + previous }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant + previous }
 }
}
Fallback Off
}