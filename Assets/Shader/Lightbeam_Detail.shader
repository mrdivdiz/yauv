Shader "Lightbeam/Lightbeam Detail" {
Properties {
 _Color ("Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _DetailTex ("Detail Texture", 2D) = "white" {}
 _Width ("Width", Float) = 8.71
 _Tweak ("Tweak", Float) = 0.65
 _DetailContrast ("Detail Strength", Range(0,1)) = 0
 _DetailAnim ("Detail Animation", Vector) = (0,0,0,0)
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 9 [_World2Object]
Vector 13 [_Time]
Vector 14 [_ProjectionParams]
Vector 15 [unity_Scale]
Vector 16 [_WorldSpaceCameraPos]
Vector 17 [_DetailTex_ST]
Float 18 [_Width]
Float 19 [_Tweak]
Vector 20 [_DetailAnim]
"!!ARBvp1.0
# 37 ALU
PARAM c[21] = { { 1, 0.5 },
		state.matrix.modelview[0],
		state.matrix.mvp,
		program.local[9..20] };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.w, c[0].x;
MOV R1.xyz, c[16];
DP4 R0.z, R1, c[11];
DP4 R0.x, R1, c[9];
DP4 R0.y, R1, c[10];
MAD R0.xyz, R0, c[15].w, -vertex.position;
DP3 R0.w, R0, R0;
RSQ R0.w, R0.w;
MUL R0.xyz, R0.w, R0;
DP3 R0.w, R0, vertex.normal;
MOV R1.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R1.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R1.zxyw, -R2;
MUL R1.xyz, vertex.attrib[14].w, R1;
DP3 R1.x, R0, R1;
DP3 R0.z, vertex.attrib[14], R0;
DP4 R1.w, vertex.position, c[8];
ADD R0.w, R0, c[19].x;
MUL R0.w, R0, c[18].x;
RSQ R2.x, R0.w;
MAD result.texcoord[1].xz, R0.z, R2.x, c[0].y;
MOV R0.w, R1;
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP4 R0.z, vertex.position, c[7];
MAD result.texcoord[1].w, R2.x, R1.x, c[0].y;
MUL R1.xyz, R0.xyww, c[0].y;
MOV result.position, R0;
MUL R1.y, R1, c[14].x;
MOV R0.x, c[13];
MAD R0.zw, vertex.texcoord[0].xyxy, c[17].xyxy, c[17];
MAD result.texcoord[0].xy, R0.x, c[20], R0.zwzw;
DP4 R0.x, vertex.position, c[3];
ADD result.texcoord[2].xy, R1, R1.z;
MOV result.texcoord[1].y, vertex.texcoord[0];
MOV result.texcoord[2].z, -R0.x;
MOV result.texcoord[2].w, R1;
END
# 37 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_modelview0]
Matrix 4 [glstate_matrix_mvp]
Matrix 8 [_World2Object]
Vector 12 [_Time]
Vector 13 [_ProjectionParams]
Vector 14 [_ScreenParams]
Vector 15 [unity_Scale]
Vector 16 [_WorldSpaceCameraPos]
Vector 17 [_DetailTex_ST]
Float 18 [_Width]
Float 19 [_Tweak]
Vector 20 [_DetailAnim]
"vs_2_0
; 38 ALU
def c21, 1.00000000, 0.50000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r1.w, c21.x
mov r1.xyz, c16
dp4 r0.z, r1, c10
dp4 r0.x, r1, c8
dp4 r0.y, r1, c9
mad r0.xyz, r0, c15.w, -v0
dp3 r0.w, r0, r0
rsq r0.w, r0.w
mul r1.xyz, r0.w, r0
mov r0.xyz, v1
mul r2.xyz, v2.zxyw, r0.yzxw
dp3 r0.w, r1, v2
dp4 r1.w, v0, c7
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r2
mul r0.xyz, v1.w, r0
dp3 r0.x, r1, r0
add r0.w, r0, c19.x
mul r0.w, r0, c18.x
rsq r0.z, r0.w
mad oT1.w, r0.z, r0.x, c21.y
dp3 r2.x, v1, r1
mad oT1.xz, r2.x, r0.z, c21.y
mov r0.w, r1
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
mul r1.xyz, r0.xyww, c21.y
dp4 r0.z, v0, c6
mov oPos, r0
mul r1.y, r1, c13.x
mov r0.xy, c20
mad r0.zw, v3.xyxy, c17.xyxy, c17
mad oT0.xy, c12.x, r0, r0.zwzw
dp4 r0.x, v0, c2
mad oT2.xy, r1.z, c14.zwzw, r1
mov oT1.y, v3
mov oT2.z, -r0.x
mov oT2.w, r1
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_Color]
Float 1 [_DetailContrast]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DetailTex] 2D
"!!ARBfp1.0
# 13 ALU, 3 TEX
PARAM c[3] = { program.local[0..1],
		{ 1, 0.2 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1, fragment.texcoord[0], texture[1], 2D;
TEX R0.x, fragment.texcoord[1], texture[0], 2D;
TEX R0.y, fragment.texcoord[1].zwzw, texture[0], 2D;
MUL R0.w, R0.x, R0.y;
ADD R1, R1, -c[2].x;
MUL R1, R1, c[1].x;
ADD R1, R1, c[2].x;
MOV R0.xyz, c[0];
MUL R0.w, R0, c[0];
MUL R0, R0, R1;
MUL_SAT R2.x, fragment.texcoord[2].z, c[2].y;
MUL result.color.w, R0, R2.x;
MOV result.color.xyz, R0;
END
# 13 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Color]
Float 1 [_DetailContrast]
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DetailTex] 2D
"ps_2_0
; 12 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
def c2, -1.00000000, 1.00000000, 0.20000000, 0
dcl t0.xy
dcl t1
dcl t2.xyz
texld r2, t1, s0
texld r1, t0, s1
add r1, r1, c2.x
mov r0.y, t1.w
mov r0.x, t1.z
texld r0, r0, s0
mul_pp r0.x, r2, r0.y
mul r2, r1, c1.x
mul r1.w, r0.x, c0
add r0, r2, c2.y
mov r1.xyz, c0
mul r1, r1, r0
mul_sat r0.x, t2.z, c2.z
mul r1.w, r1, r0.x
mov_pp oC0, r1
"
}
}
 }
}
Fallback "Lightbeam/Lightbeam"
}