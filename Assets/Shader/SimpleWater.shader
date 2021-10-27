Shader "ShaderFusion/SimpleWater" {
Properties {
 _Color ("Diffuse Color", Color) = (1,1,1,1)
 _SpecColor ("Specular Color", Color) = (1,1,1,1)
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _ScrollSpeed ("ScrollSpeed", Vector) = (0.1,0.1,0,0)
 _UVScale ("UVScale", Float) = 1
 _BumpMap ("Normal Map", 2D) = "white" {}
 _Refraction ("Refraction", Float) = 1
 _SpecPower ("SpecPower", Float) = 1
 _Glossiness ("Glossiness", Float) = 1
}
SubShader { 
 LOD 500
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 GrabPass {
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  Lighting On
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [_ProjectionParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_SHAr]
Vector 18 [unity_SHAg]
Vector 19 [unity_SHAb]
Vector 20 [unity_SHBr]
Vector 21 [unity_SHBg]
Vector 22 [unity_SHBb]
Vector 23 [unity_SHC]
"!!ARBvp1.0
# 48 ALU
PARAM c[24] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..23] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R1.xyz, vertex.normal, c[14].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].y;
DP4 R2.z, R0, c[19];
DP4 R2.y, R0, c[18];
DP4 R2.x, R0, c[17];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[22];
DP4 R3.y, R1, c[21];
DP4 R3.x, R1, c[20];
ADD R2.xyz, R2, R3;
MAD R0.x, R0, R0, -R0.y;
MUL R3.xyz, R0.x, c[23];
MOV R1.xyz, vertex.attrib[14];
MUL R0.xyz, vertex.normal.zxyw, R1.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R1.zxyw, -R0;
MOV R0, c[16];
ADD result.texcoord[3].xyz, R2, R3;
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R2.z, R0, c[11];
DP4 R2.y, R0, c[10];
DP4 R2.x, R0, c[9];
MOV R0.xyz, c[15];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[14].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R3.xyz, R0.xyww, c[0].x;
MUL R3.y, R3, c[13].x;
DP3 result.texcoord[2].y, R2, R4;
DP3 result.texcoord[4].y, R4, R1;
ADD result.texcoord[0].xy, R3, R3.z;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 48 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_SHAr]
Vector 18 [unity_SHAg]
Vector 19 [unity_SHAb]
Vector 20 [unity_SHBr]
Vector 21 [unity_SHBg]
Vector 22 [unity_SHBb]
Vector 23 [unity_SHC]
"vs_2_0
; 51 ALU
def c24, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mul r1.xyz, v2, c14.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c24.y
dp4 r2.z, r0, c19
dp4 r2.y, r0, c18
dp4 r2.x, r0, c17
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c22
dp4 r3.y, r1, c21
dp4 r3.x, r1, c20
add r1.xyz, r2, r3
mad r0.x, r0, r0, -r0.y
mul r2.xyz, r0.x, c23
add oT3.xyz, r1, r2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c9
mov r0, c10
dp4 r2.z, c16, r0
mov r0, c8
dp4 r2.x, c16, r0
dp4 r2.y, c16, r1
mov r0.xyz, c15
mov r0.w, c24.y
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c14.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r3.xyz, r0.xyww, c24.x
mul r3.y, r3, c12.x
dp3 oT2.y, r2, r4
dp3 oT4.y, r4, r1
mad oT0.xy, r3.z, c13.zwzw, r3
dp3 oT2.z, v2, r2
dp3 oT2.x, r2, v1
dp3 oT4.z, v2, r1
dp3 oT4.x, v1, r1
mov oPos, r0
mov oT0.zw, r0
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 13 [_ProjectionParams]
Vector 15 [unity_LightmapST]
"!!ARBvp1.0
# 11 ALU
PARAM c[16] = { { 0.5 },
		state.matrix.mvp,
		program.local[5..15] };
TEMP R0;
TEMP R1;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R1.xyz, R0.xyww, c[0].x;
MUL R1.y, R1, c[13].x;
ADD result.texcoord[0].xy, R1, R1.z;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MOV result.texcoord[1].xy, vertex.texcoord[0];
MAD result.texcoord[2].xy, vertex.texcoord[1], c[15], c[15].zwzw;
END
# 11 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_LightmapST]
"vs_2_0
; 11 ALU
def c15, 0.50000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v3
dcl_texcoord1 v4
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r1.xyz, r0.xyww, c15.x
mul r1.y, r1, c12.x
mad oT0.xy, r1.z, c13.zwzw, r1
mov oPos, r0
mov oT0.zw, r0
mov oT1.xy, v3
mad oT2.xy, v4, c14, c14.zwzw
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [_ProjectionParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_SHAr]
Vector 18 [unity_SHAg]
Vector 19 [unity_SHAb]
Vector 20 [unity_SHBr]
Vector 21 [unity_SHBg]
Vector 22 [unity_SHBb]
Vector 23 [unity_SHC]
"!!ARBvp1.0
# 50 ALU
PARAM c[24] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..23] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R1.xyz, vertex.normal, c[14].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].y;
DP4 R2.z, R0, c[19];
DP4 R2.y, R0, c[18];
DP4 R2.x, R0, c[17];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[22];
DP4 R3.y, R1, c[21];
DP4 R3.x, R1, c[20];
ADD R2.xyz, R2, R3;
MAD R0.x, R0, R0, -R0.y;
MUL R3.xyz, R0.x, c[23];
MOV R1.xyz, vertex.attrib[14];
MUL R0.xyz, vertex.normal.zxyw, R1.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R1.zxyw, -R0;
MOV R0, c[16];
ADD result.texcoord[3].xyz, R2, R3;
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R2.z, R0, c[11];
DP4 R2.y, R0, c[10];
DP4 R2.x, R0, c[9];
MOV R0.w, c[0].y;
MOV R0.xyz, c[15];
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R3.xyz, R1, c[14].w, -vertex.position;
DP4 R1.w, vertex.position, c[4];
DP4 R1.x, vertex.position, c[1];
DP4 R1.y, vertex.position, c[2];
MUL R0.xyz, R1.xyww, c[0].x;
DP4 R1.z, vertex.position, c[3];
MUL R0.y, R0, c[13].x;
ADD R0.xy, R0, R0.z;
MOV R0.zw, R1;
DP3 result.texcoord[2].y, R2, R4;
DP3 result.texcoord[4].y, R4, R3;
MOV result.texcoord[0], R0;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, vertex.attrib[14], R3;
MOV result.texcoord[5], R0;
MOV result.position, R1;
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 50 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_SHAr]
Vector 18 [unity_SHAg]
Vector 19 [unity_SHAb]
Vector 20 [unity_SHBr]
Vector 21 [unity_SHBg]
Vector 22 [unity_SHBb]
Vector 23 [unity_SHC]
"vs_2_0
; 53 ALU
def c24, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mul r1.xyz, v2, c14.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c24.y
dp4 r2.z, r0, c19
dp4 r2.y, r0, c18
dp4 r2.x, r0, c17
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c22
dp4 r3.y, r1, c21
dp4 r3.x, r1, c20
add r1.xyz, r2, r3
mad r0.x, r0, r0, -r0.y
mul r2.xyz, r0.x, c23
add oT3.xyz, r1, r2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c9
dp4 r2.y, c16, r1
mov r0, c10
dp4 r2.z, c16, r0
mov r0, c8
dp4 r2.x, c16, r0
mov r0.w, c24.y
mov r0.xyz, c15
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r3.xyz, r1, c14.w, -v0
dp4 r1.w, v0, c3
dp4 r1.x, v0, c0
dp4 r1.y, v0, c1
mul r0.xyz, r1.xyww, c24.x
dp4 r1.z, v0, c2
mul r0.y, r0, c12.x
mad r0.xy, r0.z, c13.zwzw, r0
mov r0.zw, r1
dp3 oT2.y, r2, r4
dp3 oT4.y, r4, r3
mov oT0, r0
dp3 oT2.z, v2, r2
dp3 oT2.x, r2, v1
dp3 oT4.z, v2, r3
dp3 oT4.x, v1, r3
mov oT5, r0
mov oPos, r1
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 13 [_ProjectionParams]
Vector 15 [unity_LightmapST]
"!!ARBvp1.0
# 13 ALU
PARAM c[16] = { { 0.5 },
		state.matrix.mvp,
		program.local[5..15] };
TEMP R0;
TEMP R1;
DP4 R1.w, vertex.position, c[4];
DP4 R1.x, vertex.position, c[1];
DP4 R1.y, vertex.position, c[2];
MUL R0.xyz, R1.xyww, c[0].x;
DP4 R1.z, vertex.position, c[3];
MUL R0.y, R0, c[13].x;
ADD R0.xy, R0, R0.z;
MOV R0.zw, R1;
MOV result.texcoord[0], R0;
MOV result.texcoord[3], R0;
MOV result.position, R1;
MOV result.texcoord[1].xy, vertex.texcoord[0];
MAD result.texcoord[2].xy, vertex.texcoord[1], c[15], c[15].zwzw;
END
# 13 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_LightmapST]
"vs_2_0
; 13 ALU
def c15, 0.50000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v3
dcl_texcoord1 v4
dp4 r1.w, v0, c3
dp4 r1.x, v0, c0
dp4 r1.y, v0, c1
mul r0.xyz, r1.xyww, c15.x
dp4 r1.z, v0, c2
mul r0.y, r0, c12.x
mad r0.xy, r0.z, c13.zwzw, r0
mov r0.zw, r1
mov oT0, r0
mov oT3, r0
mov oPos, r1
mov oT1.xy, v3
mad oT2.xy, v4, c14, c14.zwzw
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [_ProjectionParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_4LightPosX0]
Vector 18 [unity_4LightPosY0]
Vector 19 [unity_4LightPosZ0]
Vector 20 [unity_4LightAtten0]
Vector 21 [unity_LightColor0]
Vector 22 [unity_LightColor1]
Vector 23 [unity_LightColor2]
Vector 24 [unity_LightColor3]
Vector 25 [unity_SHAr]
Vector 26 [unity_SHAg]
Vector 27 [unity_SHAb]
Vector 28 [unity_SHBr]
Vector 29 [unity_SHBg]
Vector 30 [unity_SHBb]
Vector 31 [unity_SHC]
"!!ARBvp1.0
# 79 ALU
PARAM c[32] = { { 0.5, 1, 0 },
		state.matrix.mvp,
		program.local[5..31] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R3.xyz, vertex.normal, c[14].w;
DP4 R0.x, vertex.position, c[6];
ADD R1, -R0.x, c[18];
DP3 R3.w, R3, c[6];
DP3 R4.x, R3, c[5];
DP3 R3.x, R3, c[7];
MUL R2, R3.w, R1;
DP4 R0.x, vertex.position, c[5];
ADD R0, -R0.x, c[17];
MUL R1, R1, R1;
MOV R4.z, R3.x;
MAD R2, R4.x, R0, R2;
MOV R4.w, c[0].y;
DP4 R4.y, vertex.position, c[7];
MAD R1, R0, R0, R1;
ADD R0, -R4.y, c[19];
MAD R1, R0, R0, R1;
MAD R0, R3.x, R0, R2;
MUL R2, R1, c[20];
MOV R4.y, R3.w;
RSQ R1.x, R1.x;
RSQ R1.y, R1.y;
RSQ R1.w, R1.w;
RSQ R1.z, R1.z;
MUL R0, R0, R1;
ADD R1, R2, c[0].y;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].z;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[22];
MAD R1.xyz, R0.x, c[21], R1;
MAD R0.xyz, R0.z, c[23], R1;
MAD R1.xyz, R0.w, c[24], R0;
MUL R0, R4.xyzz, R4.yzzx;
DP4 R3.z, R0, c[30];
DP4 R3.y, R0, c[29];
DP4 R3.x, R0, c[28];
MUL R1.w, R3, R3;
MAD R0.x, R4, R4, -R1.w;
DP4 R2.z, R4, c[27];
DP4 R2.y, R4, c[26];
DP4 R2.x, R4, c[25];
ADD R2.xyz, R2, R3;
MUL R3.xyz, R0.x, c[31];
ADD R3.xyz, R2, R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
ADD result.texcoord[3].xyz, R3, R1;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
MOV R0, c[16];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R2.z, R0, c[11];
DP4 R2.y, R0, c[10];
DP4 R2.x, R0, c[9];
MOV R0.xyz, c[15];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[14].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R3.xyz, R0.xyww, c[0].x;
MUL R3.y, R3, c[13].x;
DP3 result.texcoord[2].y, R2, R4;
DP3 result.texcoord[4].y, R4, R1;
ADD result.texcoord[0].xy, R3, R3.z;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 79 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_4LightPosX0]
Vector 18 [unity_4LightPosY0]
Vector 19 [unity_4LightPosZ0]
Vector 20 [unity_4LightAtten0]
Vector 21 [unity_LightColor0]
Vector 22 [unity_LightColor1]
Vector 23 [unity_LightColor2]
Vector 24 [unity_LightColor3]
Vector 25 [unity_SHAr]
Vector 26 [unity_SHAg]
Vector 27 [unity_SHAb]
Vector 28 [unity_SHBr]
Vector 29 [unity_SHBg]
Vector 30 [unity_SHBb]
Vector 31 [unity_SHC]
"vs_2_0
; 82 ALU
def c32, 0.50000000, 1.00000000, 0.00000000, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mul r3.xyz, v2, c14.w
dp4 r0.x, v0, c5
add r1, -r0.x, c18
dp3 r3.w, r3, c5
dp3 r4.x, r3, c4
dp3 r3.x, r3, c6
mul r2, r3.w, r1
dp4 r0.x, v0, c4
add r0, -r0.x, c17
mul r1, r1, r1
mov r4.z, r3.x
mad r2, r4.x, r0, r2
mov r4.w, c32.y
dp4 r4.y, v0, c6
mad r1, r0, r0, r1
add r0, -r4.y, c19
mad r1, r0, r0, r1
mad r0, r3.x, r0, r2
mul r2, r1, c20
mov r4.y, r3.w
rsq r1.x, r1.x
rsq r1.y, r1.y
rsq r1.w, r1.w
rsq r1.z, r1.z
mul r0, r0, r1
add r1, r2, c32.y
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c32.z
mul r0, r0, r1
mul r1.xyz, r0.y, c22
mad r1.xyz, r0.x, c21, r1
mad r0.xyz, r0.z, c23, r1
mad r1.xyz, r0.w, c24, r0
mul r0, r4.xyzz, r4.yzzx
mul r1.w, r3, r3
dp4 r3.z, r0, c30
dp4 r3.y, r0, c29
dp4 r3.x, r0, c28
mad r1.w, r4.x, r4.x, -r1
mul r0.xyz, r1.w, c31
dp4 r2.z, r4, c27
dp4 r2.y, r4, c26
dp4 r2.x, r4, c25
add r2.xyz, r2, r3
add r2.xyz, r2, r0
add oT3.xyz, r2, r1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c9
mov r0, c10
dp4 r2.z, c16, r0
mov r0, c8
dp4 r2.x, c16, r0
dp4 r2.y, c16, r1
mov r0.xyz, c15
mov r0.w, c32.y
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c14.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r3.xyz, r0.xyww, c32.x
mul r3.y, r3, c12.x
dp3 oT2.y, r2, r4
dp3 oT4.y, r4, r1
mad oT0.xy, r3.z, c13.zwzw, r3
dp3 oT2.z, v2, r2
dp3 oT2.x, r2, v1
dp3 oT4.z, v2, r1
dp3 oT4.x, v1, r1
mov oPos, r0
mov oT0.zw, r0
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [_ProjectionParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_4LightPosX0]
Vector 18 [unity_4LightPosY0]
Vector 19 [unity_4LightPosZ0]
Vector 20 [unity_4LightAtten0]
Vector 21 [unity_LightColor0]
Vector 22 [unity_LightColor1]
Vector 23 [unity_LightColor2]
Vector 24 [unity_LightColor3]
Vector 25 [unity_SHAr]
Vector 26 [unity_SHAg]
Vector 27 [unity_SHAb]
Vector 28 [unity_SHBr]
Vector 29 [unity_SHBg]
Vector 30 [unity_SHBb]
Vector 31 [unity_SHC]
"!!ARBvp1.0
# 81 ALU
PARAM c[32] = { { 0.5, 1, 0 },
		state.matrix.mvp,
		program.local[5..31] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R3.xyz, vertex.normal, c[14].w;
DP4 R0.x, vertex.position, c[6];
ADD R1, -R0.x, c[18];
DP3 R3.w, R3, c[6];
DP3 R4.x, R3, c[5];
DP3 R3.x, R3, c[7];
MUL R2, R3.w, R1;
DP4 R0.x, vertex.position, c[5];
ADD R0, -R0.x, c[17];
MUL R1, R1, R1;
MOV R4.z, R3.x;
MAD R2, R4.x, R0, R2;
MOV R4.w, c[0].y;
DP4 R4.y, vertex.position, c[7];
MAD R1, R0, R0, R1;
ADD R0, -R4.y, c[19];
MAD R1, R0, R0, R1;
MAD R0, R3.x, R0, R2;
MUL R2, R1, c[20];
MOV R4.y, R3.w;
RSQ R1.x, R1.x;
RSQ R1.y, R1.y;
RSQ R1.w, R1.w;
RSQ R1.z, R1.z;
MUL R0, R0, R1;
ADD R1, R2, c[0].y;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].z;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[22];
MAD R1.xyz, R0.x, c[21], R1;
MAD R0.xyz, R0.z, c[23], R1;
MAD R1.xyz, R0.w, c[24], R0;
MUL R0, R4.xyzz, R4.yzzx;
DP4 R3.z, R0, c[30];
DP4 R3.y, R0, c[29];
DP4 R3.x, R0, c[28];
MUL R1.w, R3, R3;
MAD R0.x, R4, R4, -R1.w;
DP4 R1.w, vertex.position, c[4];
DP4 R2.z, R4, c[27];
DP4 R2.y, R4, c[26];
DP4 R2.x, R4, c[25];
ADD R2.xyz, R2, R3;
MUL R3.xyz, R0.x, c[31];
ADD R3.xyz, R2, R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
ADD result.texcoord[3].xyz, R3, R1;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
MOV R0, c[16];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R2.z, R0, c[11];
DP4 R2.y, R0, c[10];
DP4 R2.x, R0, c[9];
MOV R0.w, c[0].y;
MOV R0.xyz, c[15];
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R3.xyz, R1, c[14].w, -vertex.position;
DP4 R1.x, vertex.position, c[1];
DP4 R1.y, vertex.position, c[2];
MUL R0.xyz, R1.xyww, c[0].x;
DP4 R1.z, vertex.position, c[3];
MUL R0.y, R0, c[13].x;
ADD R0.xy, R0, R0.z;
MOV R0.zw, R1;
DP3 result.texcoord[2].y, R2, R4;
DP3 result.texcoord[4].y, R4, R3;
MOV result.texcoord[0], R0;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, vertex.attrib[14], R3;
MOV result.texcoord[5], R0;
MOV result.position, R1;
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 81 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_Scale]
Vector 15 [_WorldSpaceCameraPos]
Vector 16 [_WorldSpaceLightPos0]
Vector 17 [unity_4LightPosX0]
Vector 18 [unity_4LightPosY0]
Vector 19 [unity_4LightPosZ0]
Vector 20 [unity_4LightAtten0]
Vector 21 [unity_LightColor0]
Vector 22 [unity_LightColor1]
Vector 23 [unity_LightColor2]
Vector 24 [unity_LightColor3]
Vector 25 [unity_SHAr]
Vector 26 [unity_SHAg]
Vector 27 [unity_SHAb]
Vector 28 [unity_SHBr]
Vector 29 [unity_SHBg]
Vector 30 [unity_SHBb]
Vector 31 [unity_SHC]
"vs_2_0
; 84 ALU
def c32, 0.50000000, 1.00000000, 0.00000000, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mul r3.xyz, v2, c14.w
dp4 r0.x, v0, c5
add r1, -r0.x, c18
dp3 r3.w, r3, c5
dp3 r4.x, r3, c4
dp3 r3.x, r3, c6
mul r2, r3.w, r1
dp4 r0.x, v0, c4
add r0, -r0.x, c17
mul r1, r1, r1
mov r4.z, r3.x
mad r2, r4.x, r0, r2
mov r4.w, c32.y
dp4 r4.y, v0, c6
mad r1, r0, r0, r1
add r0, -r4.y, c19
mad r1, r0, r0, r1
mad r0, r3.x, r0, r2
mul r2, r1, c20
mov r4.y, r3.w
rsq r1.x, r1.x
rsq r1.y, r1.y
rsq r1.w, r1.w
rsq r1.z, r1.z
mul r0, r0, r1
add r1, r2, c32.y
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c32.z
mul r0, r0, r1
mul r1.xyz, r0.y, c22
mad r1.xyz, r0.x, c21, r1
mad r0.xyz, r0.z, c23, r1
mad r1.xyz, r0.w, c24, r0
mul r0, r4.xyzz, r4.yzzx
mul r1.w, r3, r3
dp4 r3.z, r0, c30
dp4 r3.y, r0, c29
dp4 r3.x, r0, c28
mad r1.w, r4.x, r4.x, -r1
mul r0.xyz, r1.w, c31
dp4 r2.z, r4, c27
dp4 r2.y, r4, c26
dp4 r2.x, r4, c25
add r2.xyz, r2, r3
add r2.xyz, r2, r0
add oT3.xyz, r2, r1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c9
dp4 r2.y, c16, r1
mov r0, c10
dp4 r2.z, c16, r0
mov r0, c8
dp4 r2.x, c16, r0
mov r0.w, c32.y
mov r0.xyz, c15
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r3.xyz, r1, c14.w, -v0
dp4 r1.w, v0, c3
dp4 r1.x, v0, c0
dp4 r1.y, v0, c1
mul r0.xyz, r1.xyww, c32.x
dp4 r1.z, v0, c2
mul r0.y, r0, c12.x
mad r0.xy, r0.z, c13.zwzw, r0
mov r0.zw, r1
dp3 oT2.y, r2, r4
dp3 oT4.y, r4, r3
mov oT0, r0
dp3 oT2.z, v2, r2
dp3 oT2.x, r2, v1
dp3 oT4.z, v2, r3
dp3 oT4.x, v1, r3
mov oT5, r0
mov oPos, r1
mov oT1.xy, v3
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 51 ALU, 2 TEX
PARAM c[12] = { program.local[0..9],
		{ 2, 1, 0.5, 0 },
		{ 128, 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
TEX R0.yw, R0, texture[0], 2D;
MAD R1.xy, R0.wyzw, c[10].x, -c[10].y;
ADD R0.xy, R1, -c[10].z;
MUL R0.xy, R0, c[5].x;
MUL R0.zw, fragment.texcoord[0].z, c[6].xyxy;
MUL R0.zw, R0.xyxy, R0;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
RSQ R0.w, R0.w;
MUL R2.xyz, R0.w, fragment.texcoord[4];
MUL R0.w, R1.y, R1.y;
DP3 R1.z, R2, R2;
MAD R0.w, -R1.x, R1.x, -R0;
RSQ R1.z, R1.z;
MAD R2.xyz, R1.z, R2, fragment.texcoord[2];
DP3 R1.w, R2, R2;
RSQ R1.w, R1.w;
ADD R0.w, R0, c[10].y;
RSQ R0.w, R0.w;
RCP R1.z, R0.w;
MUL R2.xyz, R1.w, R2;
DP3 R1.w, R1, R2;
MOV R0.w, c[10];
MAX R2.w, R0, c[8].x;
MUL R2.x, R2.w, c[11];
DP3 R1.x, R1, fragment.texcoord[2];
MAX R1.w, R1, c[10];
POW R1.w, R1.w, R2.x;
MAX R2.w, R1.x, c[10];
TEX R0.xyz, R0, texture[1], 2D;
MUL R0.xyz, R0, c[9];
MUL R1.xyz, R0, c[10].z;
MAX R1.xyz, R1, c[10].w;
MUL R2.xyz, R1, c[1];
MOV R0.xyz, c[2];
MUL R0.xyz, R0, c[7].x;
MAX R0.xyz, R0, c[10].w;
MUL R3.xyz, R2, R2.w;
MUL R2.xyz, R0, c[1];
DP3 R0.x, R0, c[11].yzww;
MAD R2.xyz, R2, R1.w, R3;
MUL R2.xyz, R2, c[10].x;
MAD R2.xyz, R1, fragment.texcoord[3], R2;
MAX R0.y, R0.w, c[9].w;
MUL R0.x, R0, c[1].w;
ADD result.color.xyz, R2, R1;
MAD result.color.w, R1, R0.x, R0.y;
END
# 51 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
"ps_2_0
; 50 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c10, 2.00000000, -1.00000000, -0.50000000, 0.50000000
def c11, 1.00000000, 0.00000000, 128.00000000, 0
def c12, 0.21997070, 0.70703125, 0.07098389, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
dcl t4.xyz
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r3.xy, r0, c10.x, c10.y
add r0.xy, r3, c10.z
mul r0.xy, r0, c5.x
mul r1.xy, r0, r1
rcp r0.x, t0.w
mad r0.xy, t0, r0.x, r1
texld r2, r0, s1
dp3_pp r0.x, t4, t4
rsq_pp r0.x, r0.x
mul_pp r4.xyz, r0.x, t4
dp3_pp r1.x, r4, r4
mul_pp r0.x, r3.y, r3.y
rsq_pp r1.x, r1.x
mul r2.xyz, r2, c9
mad_pp r4.xyz, r1.x, r4, t2
mad_pp r0.x, -r3, r3, -r0
add_pp r1.x, r0, c11
dp3_pp r0.x, r4, r4
rsq_pp r1.x, r1.x
rcp_pp r3.z, r1.x
rsq_pp r0.x, r0.x
mul_pp r1.xyz, r0.x, r4
dp3_pp r1.x, r3, r1
mov_pp r0.x, c8
mul r2.xyz, r2, c10.w
mul_pp r0.x, c11.z, r0
max_pp r1.x, r1, c11.y
pow r4.w, r1.x, r0.x
dp3_pp r1.x, r3, t2
mov r0.x, r4.w
mov r3.x, c7
mul r3.xyz, c2, r3.x
max_pp r1.x, r1, c11.y
mul_pp r4.xyz, r2, c1
mul_pp r4.xyz, r4, r1.x
mul_pp r1.xyz, r3, c1
mad r4.xyz, r1, r0.x, r4
dp3_pp r1.x, r3, c12
mul r3.xyz, r4, c10.x
mul_pp r1.x, r1, c1.w
mad r0.w, r0.x, r1.x, c9
mad_pp r3.xyz, r2, t3, r3
add_pp r0.xyz, r3, r2
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_ScrollSpeed]
Float 2 [_UVScale]
Float 3 [_Refraction]
Vector 4 [_GrabTexture_TexelSize]
Vector 5 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [unity_Lightmap] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 21 ALU, 3 TEX
PARAM c[8] = { program.local[0..5],
		{ 0, 2, 1, 0.5 },
		{ 8 } };
TEMP R0;
TEMP R1;
MUL R0.zw, fragment.texcoord[1].xyxy, c[2].x;
MOV R0.xy, c[1];
MAD R0.xy, R0, c[0], R0.zwzw;
TEX R0.yw, R0, texture[0], 2D;
MAD R0.xy, R0.wyzw, c[6].y, -c[6].z;
ADD R0.zw, R0.xyxy, -c[6].w;
MUL R0.xy, fragment.texcoord[0].z, c[4];
MUL R0.zw, R0, c[3].x;
MUL R0.zw, R0, R0.xyxy;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
TEX R1.xyz, R0, texture[1], 2D;
TEX R0, fragment.texcoord[2], texture[2], 2D;
MUL R0.xyz, R0.w, R0;
MUL R1.xyz, R1, c[5];
MUL R1.xyz, R1, c[6].w;
MAX R1.xyz, R1, c[6].x;
MUL R0.xyz, R0, R1;
MOV R0.w, c[6].x;
MAD result.color.xyz, R0, c[7].x, R1;
MAX result.color.w, R0, c[5];
END
# 21 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_ScrollSpeed]
Float 2 [_UVScale]
Float 3 [_Refraction]
Vector 4 [_GrabTexture_TexelSize]
Vector 5 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [unity_Lightmap] 2D
"ps_2_0
; 18 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c6, 2.00000000, -1.00000000, -0.50000000, 0.50000000
def c7, 8.00000000, 0, 0, 0
dcl t0
dcl t1.xy
dcl t2.xy
mul r1.xy, t1, c2.x
mov r0.xy, c0
mad r0.xy, c1, r0, r1
mul r1.xy, t0.z, c4
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r0.xy, r0, c6.x, c6.y
add r0.xy, r0, c6.z
mul r0.xy, r0, c3.x
mul r0.xy, r0, r1
rcp r1.x, t0.w
mad r0.xy, t0, r1.x, r0
texld r0, r0, s1
texld r1, t2, s2
mul r0.xyz, r0, c5
mul r0.xyz, r0, c6.w
mul_pp r1.xyz, r1.w, r1
mul_pp r1.xyz, r1, r0
mov_pp r0.w, c5
mad_pp r0.xyz, r1, c7.x, r0
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_ShadowMapTexture] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 54 ALU, 3 TEX
PARAM c[12] = { program.local[0..9],
		{ 2, 1, 0.5, 0 },
		{ 128, 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TXP R1.x, fragment.texcoord[5], texture[2], 2D;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
TEX R0.yw, R0, texture[0], 2D;
MAD R3.xy, R0.wyzw, c[10].x, -c[10].y;
ADD R0.xy, R3, -c[10].z;
MUL R0.xy, R0, c[5].x;
MUL R0.zw, fragment.texcoord[0].z, c[6].xyxy;
MUL R0.zw, R0.xyxy, R0;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
RSQ R0.w, R0.w;
MUL R2.xyz, R0.w, fragment.texcoord[4];
MUL R0.w, R3.y, R3.y;
DP3 R1.y, R2, R2;
MAD R0.w, -R3.x, R3.x, -R0;
RSQ R1.y, R1.y;
MAD R2.xyz, R1.y, R2, fragment.texcoord[2];
DP3 R1.y, R2, R2;
RSQ R1.y, R1.y;
ADD R0.w, R0, c[10].y;
RSQ R0.w, R0.w;
RCP R3.z, R0.w;
MUL R2.xyz, R1.y, R2;
DP3 R1.y, R3, R2;
MOV R0.w, c[10];
MAX R1.z, R0.w, c[8].x;
MUL R1.z, R1, c[11].x;
MAX R1.y, R1, c[10].w;
POW R2.w, R1.y, R1.z;
DP3 R1.y, R3, fragment.texcoord[2];
MAX R3.w, R1.y, c[10];
TEX R0.xyz, R0, texture[1], 2D;
MUL R0.xyz, R0, c[9];
MUL R2.xyz, R0, c[10].z;
MAX R3.xyz, R2, c[10].w;
MOV R0.xyz, c[2];
MUL R2.xyz, R0, c[7].x;
MAX R1.yzw, R2.xxyz, c[10].w;
MUL R0.xyz, R3, c[1];
MUL R2.xyz, R0, R3.w;
MUL R0.xyz, R1.yzww, c[1];
MUL R3.w, R1.x, c[10].x;
MAD R0.xyz, R0, R2.w, R2;
MUL R0.xyz, R0, R3.w;
MAD R0.xyz, R3, fragment.texcoord[3], R0;
ADD result.color.xyz, R0, R3;
DP3 R1.y, R1.yzww, c[11].yzww;
MUL R0.x, R1.y, c[1].w;
MAX R0.y, R0.w, c[9].w;
MUL R0.x, R2.w, R0;
MAD result.color.w, R1.x, R0.x, R0.y;
END
# 54 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_ShadowMapTexture] 2D
"ps_2_0
; 52 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 2.00000000, -1.00000000, -0.50000000, 0.50000000
def c11, 1.00000000, 0.00000000, 128.00000000, 0
def c12, 0.21997070, 0.70703125, 0.07098389, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
dcl t4.xyz
dcl t5
texldp r5, t5, s2
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r3.xy, r0, c10.x, c10.y
add r0.xy, r3, c10.z
mul r0.xy, r0, c5.x
mul r1.xy, r0, r1
rcp r0.x, t0.w
mad r0.xy, t0, r0.x, r1
texld r2, r0, s1
dp3_pp r0.x, t4, t4
rsq_pp r0.x, r0.x
mul_pp r4.xyz, r0.x, t4
dp3_pp r1.x, r4, r4
mul_pp r0.x, r3.y, r3.y
rsq_pp r1.x, r1.x
mad_pp r4.xyz, r1.x, r4, t2
mad_pp r0.x, -r3, r3, -r0
dp3_pp r1.x, r4, r4
add_pp r0.x, r0, c11
rsq_pp r0.x, r0.x
rcp_pp r3.z, r0.x
rsq_pp r1.x, r1.x
mul_pp r1.xyz, r1.x, r4
dp3_pp r1.x, r3, r1
mov_pp r0.x, c8
mul r2.xyz, r2, c9
mul r2.xyz, r2, c10.w
mul_pp r0.x, c11.z, r0
max_pp r1.x, r1, c11.y
pow r4.w, r1.x, r0.x
dp3_pp r1.x, r3, t2
mov r0.x, r4.w
mov r3.x, c7
mul r3.xyz, c2, r3.x
max_pp r1.x, r1, c11.y
mul_pp r4.xyz, r2, c1
mul_pp r4.xyz, r4, r1.x
mul_pp r1.xyz, r3, c1
mad r4.xyz, r1, r0.x, r4
dp3_pp r1.x, r3, c12
mul_pp r1.x, r1, c1.w
mul r0.x, r0, r1
mul_pp r3.x, r5, c10
mul r3.xyz, r4, r3.x
mad r0.w, r5.x, r0.x, c9
mad_pp r1.xyz, r2, t3, r3
add_pp r0.xyz, r1, r2
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_ScrollSpeed]
Float 2 [_UVScale]
Float 3 [_Refraction]
Vector 4 [_GrabTexture_TexelSize]
Vector 5 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_ShadowMapTexture] 2D
SetTexture 3 [unity_Lightmap] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 27 ALU, 4 TEX
PARAM c[8] = { program.local[0..5],
		{ 0, 2, 1, 0.5 },
		{ 8 } };
TEMP R0;
TEMP R1;
TEMP R2;
TXP R2.x, fragment.texcoord[3], texture[2], 2D;
MUL R0.zw, fragment.texcoord[1].xyxy, c[2].x;
MOV R0.xy, c[1];
MAD R0.xy, R0, c[0], R0.zwzw;
TEX R0.yw, R0, texture[0], 2D;
MAD R0.xy, R0.wyzw, c[6].y, -c[6].z;
ADD R0.xy, R0, -c[6].w;
MUL R0.xy, R0, c[3].x;
MUL R0.zw, fragment.texcoord[0].z, c[4].xyxy;
MUL R0.zw, R0.xyxy, R0;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
TEX R1.xyz, R0, texture[1], 2D;
TEX R0, fragment.texcoord[2], texture[3], 2D;
MUL R2.yzw, R0.xxyz, R2.x;
MUL R0.xyz, R0.w, R0;
MUL R1.xyz, R1, c[5];
MUL R1.xyz, R1, c[6].w;
MOV R0.w, c[6].x;
MUL R0.xyz, R0, c[7].x;
MUL R2.yzw, R2, c[6].y;
MIN R2.yzw, R0.xxyz, R2;
MUL R0.xyz, R0, R2.x;
MAX R0.xyz, R2.yzww, R0;
MAX R1.xyz, R1, c[6].x;
MAD result.color.xyz, R1, R0, R1;
MAX result.color.w, R0, c[5];
END
# 27 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Vector 0 [_Time]
Vector 1 [_ScrollSpeed]
Float 2 [_UVScale]
Float 3 [_Refraction]
Vector 4 [_GrabTexture_TexelSize]
Vector 5 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_ShadowMapTexture] 2D
SetTexture 3 [unity_Lightmap] 2D
"ps_2_0
; 23 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
def c6, 2.00000000, -1.00000000, -0.50000000, 0.50000000
def c7, 8.00000000, 0, 0, 0
dcl t0
dcl t1.xy
dcl t2.xy
dcl t3
texldp r3, t3, s2
mul r1.xy, t1, c2.x
mov r0.xy, c0
mad r0.xy, c1, r0, r1
mul r1.xy, t0.z, c4
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r0.xy, r0, c6.x, c6.y
add r0.xy, r0, c6.z
mul r0.xy, r0, c3.x
mul r0.xy, r0, r1
rcp r1.x, t0.w
mad r0.xy, t0, r1.x, r0
texld r1, r0, s1
texld r0, t2, s3
mul_pp r2.xyz, r0, r3.x
mul_pp r0.xyz, r0.w, r0
mul r1.xyz, r1, c5
mul_pp r0.xyz, r0, c7.x
mul_pp r2.xyz, r2, c6.x
min_pp r2.xyz, r0, r2
mul_pp r0.xyz, r0, r3.x
max_pp r0.xyz, r2, r0
mul r1.xyz, r1, c6.w
mov_pp r0.w, c5
mad_pp r0.xyz, r1, r0, r1
mov_pp oC0, r0
"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  Lighting On
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "opengl " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [_ProjectionParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"!!ARBvp1.0
# 38 ALU
PARAM c[21] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..20] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[20];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R3.xyz, R1, c[18].w, -vertex.position;
MOV R0.xyz, c[19];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[18].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R2.xyz, R0.xyww, c[0].x;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MUL R2.y, R2, c[17].x;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R3, R4;
DP3 result.texcoord[3].y, R4, R1;
ADD result.texcoord[0].xy, R2, R2.z;
DP3 result.texcoord[2].z, vertex.normal, R3;
DP3 result.texcoord[2].x, R3, vertex.attrib[14];
DP3 result.texcoord[3].z, vertex.normal, R1;
DP3 result.texcoord[3].x, vertex.attrib[14], R1;
DP4 result.texcoord[4].z, R0, c[15];
DP4 result.texcoord[4].y, R0, c[14];
DP4 result.texcoord[4].x, R0, c[13];
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 38 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [_ProjectionParams]
Vector 17 [_ScreenParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"vs_2_0
; 41 ALU
def c21, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c8
mov r0, c10
dp4 r2.z, c20, r0
mov r0, c9
dp4 r2.y, c20, r0
dp4 r2.x, c20, r1
mad r3.xyz, r2, c18.w, -v0
mov r0.xyz, c19
mov r0.w, c21.y
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r2.xyz, r0.xyww, c21.x
mov oPos, r0
mov oT0.zw, r0
mul r2.y, r2, c16.x
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 oT2.y, r3, r4
dp3 oT3.y, r4, r1
mad oT0.xy, r2.z, c17.zwzw, r2
dp3 oT2.z, v2, r3
dp3 oT2.x, r3, v1
dp3 oT3.z, v2, r1
dp3 oT3.x, v1, r1
dp4 oT4.z, r0, c14
dp4 oT4.y, r0, c13
dp4 oT4.x, r0, c12
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_World2Object]
Vector 9 [_ProjectionParams]
Vector 10 [unity_Scale]
Vector 11 [_WorldSpaceCameraPos]
Vector 12 [_WorldSpaceLightPos0]
"!!ARBvp1.0
# 30 ALU
PARAM c[13] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..12] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[12];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R2.z, R0, c[7];
DP4 R2.y, R0, c[6];
DP4 R2.x, R0, c[5];
MOV R0.xyz, c[11];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[7];
DP4 R1.x, R0, c[5];
DP4 R1.y, R0, c[6];
MAD R1.xyz, R1, c[10].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R3.xyz, R0.xyww, c[0].x;
MUL R3.y, R3, c[9].x;
DP3 result.texcoord[2].y, R2, R4;
DP3 result.texcoord[3].y, R4, R1;
ADD result.texcoord[0].xy, R3, R3.z;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, R2, vertex.attrib[14];
DP3 result.texcoord[3].z, vertex.normal, R1;
DP3 result.texcoord[3].x, vertex.attrib[14], R1;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 30 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_World2Object]
Vector 8 [_ProjectionParams]
Vector 9 [_ScreenParams]
Vector 10 [unity_Scale]
Vector 11 [_WorldSpaceCameraPos]
Vector 12 [_WorldSpaceLightPos0]
"vs_2_0
; 33 ALU
def c13, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c5
mov r0, c6
dp4 r2.z, c12, r0
mov r0, c4
dp4 r2.x, c12, r0
dp4 r2.y, c12, r1
mov r0.xyz, c11
mov r0.w, c13.y
dp4 r1.z, r0, c6
dp4 r1.x, r0, c4
dp4 r1.y, r0, c5
mad r1.xyz, r1, c10.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r3.xyz, r0.xyww, c13.x
mul r3.y, r3, c8.x
dp3 oT2.y, r2, r4
dp3 oT3.y, r4, r1
mad oT0.xy, r3.z, c9.zwzw, r3
dp3 oT2.z, v2, r2
dp3 oT2.x, r2, v1
dp3 oT3.z, v2, r1
dp3 oT3.x, v1, r1
mov oPos, r0
mov oT0.zw, r0
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [_ProjectionParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"!!ARBvp1.0
# 39 ALU
PARAM c[21] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..20] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[20];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R3.xyz, R1, c[18].w, -vertex.position;
MOV R0.xyz, c[19];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[18].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R2.xyz, R0.xyww, c[0].x;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MUL R2.y, R2, c[17].x;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R3, R4;
DP3 result.texcoord[3].y, R4, R1;
ADD result.texcoord[0].xy, R2, R2.z;
DP3 result.texcoord[2].z, vertex.normal, R3;
DP3 result.texcoord[2].x, R3, vertex.attrib[14];
DP3 result.texcoord[3].z, vertex.normal, R1;
DP3 result.texcoord[3].x, vertex.attrib[14], R1;
DP4 result.texcoord[4].w, R0, c[16];
DP4 result.texcoord[4].z, R0, c[15];
DP4 result.texcoord[4].y, R0, c[14];
DP4 result.texcoord[4].x, R0, c[13];
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 39 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [_ProjectionParams]
Vector 17 [_ScreenParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"vs_2_0
; 42 ALU
def c21, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c8
mov r0, c10
dp4 r2.z, c20, r0
mov r0, c9
dp4 r2.y, c20, r0
dp4 r2.x, c20, r1
mad r3.xyz, r2, c18.w, -v0
mov r0.xyz, c19
mov r0.w, c21.y
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r2.xyz, r0.xyww, c21.x
mov oPos, r0
mov oT0.zw, r0
mul r2.y, r2, c16.x
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 oT2.y, r3, r4
dp3 oT3.y, r4, r1
mad oT0.xy, r2.z, c17.zwzw, r2
dp3 oT2.z, v2, r3
dp3 oT2.x, r3, v1
dp3 oT3.z, v2, r1
dp3 oT3.x, v1, r1
dp4 oT4.w, r0, c15
dp4 oT4.z, r0, c14
dp4 oT4.y, r0, c13
dp4 oT4.x, r0, c12
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [_ProjectionParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"!!ARBvp1.0
# 38 ALU
PARAM c[21] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..20] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[20];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R3.xyz, R1, c[18].w, -vertex.position;
MOV R0.xyz, c[19];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[18].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R2.xyz, R0.xyww, c[0].x;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MUL R2.y, R2, c[17].x;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R3, R4;
DP3 result.texcoord[3].y, R4, R1;
ADD result.texcoord[0].xy, R2, R2.z;
DP3 result.texcoord[2].z, vertex.normal, R3;
DP3 result.texcoord[2].x, R3, vertex.attrib[14];
DP3 result.texcoord[3].z, vertex.normal, R1;
DP3 result.texcoord[3].x, vertex.attrib[14], R1;
DP4 result.texcoord[4].z, R0, c[15];
DP4 result.texcoord[4].y, R0, c[14];
DP4 result.texcoord[4].x, R0, c[13];
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 38 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [_ProjectionParams]
Vector 17 [_ScreenParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"vs_2_0
; 41 ALU
def c21, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c8
mov r0, c10
dp4 r2.z, c20, r0
mov r0, c9
dp4 r2.y, c20, r0
dp4 r2.x, c20, r1
mad r3.xyz, r2, c18.w, -v0
mov r0.xyz, c19
mov r0.w, c21.y
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r2.xyz, r0.xyww, c21.x
mov oPos, r0
mov oT0.zw, r0
mul r2.y, r2, c16.x
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 oT2.y, r3, r4
dp3 oT3.y, r4, r1
mad oT0.xy, r2.z, c17.zwzw, r2
dp3 oT2.z, v2, r3
dp3 oT2.x, r3, v1
dp3 oT3.z, v2, r1
dp3 oT3.x, v1, r1
dp4 oT4.z, r0, c14
dp4 oT4.y, r0, c13
dp4 oT4.x, r0, c12
mov oT1.xy, v3
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [_ProjectionParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"!!ARBvp1.0
# 36 ALU
PARAM c[21] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..20] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[20];
MUL R4.xyz, R1, vertex.attrib[14].w;
DP4 R2.z, R0, c[11];
DP4 R2.y, R0, c[10];
DP4 R2.x, R0, c[9];
MOV R0.xyz, c[19];
MOV R0.w, c[0].y;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[18].w, -vertex.position;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R3.xyz, R0.xyww, c[0].x;
MOV result.position, R0;
MOV result.texcoord[0].zw, R0;
MUL R3.y, R3, c[17].x;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[2].y, R2, R4;
DP3 result.texcoord[3].y, R4, R1;
ADD result.texcoord[0].xy, R3, R3.z;
DP3 result.texcoord[2].z, vertex.normal, R2;
DP3 result.texcoord[2].x, R2, vertex.attrib[14];
DP3 result.texcoord[3].z, vertex.normal, R1;
DP3 result.texcoord[3].x, vertex.attrib[14], R1;
DP4 result.texcoord[4].y, R0, c[14];
DP4 result.texcoord[4].x, R0, c[13];
MOV result.texcoord[1].xy, vertex.texcoord[0];
END
# 36 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [_ProjectionParams]
Vector 17 [_ScreenParams]
Vector 18 [unity_Scale]
Vector 19 [_WorldSpaceCameraPos]
Vector 20 [_WorldSpaceLightPos0]
"vs_2_0
; 39 ALU
def c21, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r4.xyz, r1, v1.w
mov r1, c9
mov r0, c10
dp4 r2.z, c20, r0
mov r0, c8
dp4 r2.x, c20, r0
dp4 r2.y, c20, r1
mov r0.xyz, c19
mov r0.w, c21.y
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r3.xyz, r0.xyww, c21.x
mov oPos, r0
mov oT0.zw, r0
mul r3.y, r3, c16.x
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 oT2.y, r2, r4
dp3 oT3.y, r4, r1
mad oT0.xy, r3.z, c17.zwzw, r3
dp3 oT2.z, v2, r2
dp3 oT2.x, r2, v1
dp3 oT3.z, v2, r1
dp3 oT3.x, v1, r1
dp4 oT4.y, r0, c13
dp4 oT4.x, r0, c12
mov oT1.xy, v3
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "POINT" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTexture0] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 55 ALU, 3 TEX
PARAM c[12] = { program.local[0..9],
		{ 0, 2, 1, 128 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
DP3 R1.z, fragment.texcoord[2], fragment.texcoord[2];
RSQ R1.w, R1.z;
MUL R2.xyz, R1.w, fragment.texcoord[2];
DP3 R1.w, R2, R2;
RSQ R1.w, R1.w;
DP3 R1.z, fragment.texcoord[3], fragment.texcoord[3];
RSQ R1.z, R1.z;
MUL R3.xyz, R1.z, fragment.texcoord[3];
DP3 R1.z, R3, R3;
MUL R2.xyz, R1.w, R2;
RSQ R1.z, R1.z;
MAD R3.xyz, R1.z, R3, R2;
DP3 R1.w, R3, R3;
RSQ R1.w, R1.w;
MUL R3.xyz, R1.w, R3;
MOV R1.w, c[10].x;
MAX R2.w, R1, c[8].x;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
MUL R2.w, R2, c[10];
MOV result.color.w, c[10].x;
TEX R0.yw, R0, texture[0], 2D;
MAD R1.xy, R0.wyzw, c[10].y, -c[10].z;
ADD R0.xy, R1, -c[11].x;
MUL R1.z, R1.y, R1.y;
MAD R1.z, -R1.x, R1.x, -R1;
ADD R1.z, R1, c[10];
RSQ R1.z, R1.z;
RCP R1.z, R1.z;
DP3 R1.w, R1, R3;
DP3 R1.x, R1, R2;
MAX R1.w, R1, c[10].x;
MUL R0.xy, R0, c[5].x;
MUL R0.zw, fragment.texcoord[0].z, c[6].xyxy;
MUL R0.zw, R0.xyxy, R0;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
MAX R2.x, R1, c[10];
POW R1.w, R1.w, R2.w;
TEX R0.xyz, R0, texture[1], 2D;
TEX R0.w, R0.w, texture[2], 2D;
MUL R0.xyz, R0, c[9];
MUL R1.xyz, R0, c[11].x;
MOV R0.xyz, c[2];
MAX R1.xyz, R1, c[10].x;
MUL R0.xyz, R0, c[7].x;
MUL R1.xyz, R1, c[1];
MAX R0.xyz, R0, c[10].x;
MUL R1.xyz, R1, R2.x;
MUL R0.xyz, R0, c[1];
MUL R0.w, R0, c[10].y;
MAD R0.xyz, R0, R1.w, R1;
MUL result.color.xyz, R0, R0.w;
END
# 55 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTexture0] 2D
"ps_2_0
; 55 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 2.00000000, -1.00000000, 1.00000000, 0.00000000
def c11, 128.00000000, -0.50000000, 0.50000000, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
dcl t4.xyz
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r4.xy, r0, c10.x, c10.y
add r0.xy, r4, c11.y
mul r0.xy, r0, c5.x
mul r2.xy, r0, r1
dp3 r0.x, t4, t4
mov r0.xy, r0.x
rcp r1.x, t0.w
mad r1.xy, t0, r1.x, r2
mov_pp r0.w, c10
texld r2, r1, s1
texld r6, r0, s2
dp3_pp r1.x, t2, t2
rsq_pp r3.x, r1.x
mul_pp r0.x, r4.y, r4.y
mad_pp r0.x, -r4, r4, -r0
add_pp r0.x, r0, c10.z
rsq_pp r0.x, r0.x
rcp_pp r4.z, r0.x
mov_pp r0.x, c8
mul_pp r7.xyz, r3.x, t2
dp3_pp r1.x, t3, t3
rsq_pp r3.x, r1.x
mul_pp r5.xyz, r3.x, t3
dp3_pp r1.x, r7, r7
rsq_pp r3.x, r1.x
dp3_pp r1.x, r5, r5
mul_pp r3.xyz, r3.x, r7
rsq_pp r1.x, r1.x
mad_pp r5.xyz, r1.x, r5, r3
dp3_pp r1.x, r5, r5
rsq_pp r1.x, r1.x
mul_pp r1.xyz, r1.x, r5
dp3_pp r1.x, r4, r1
mul_pp r0.x, c11, r0
max_pp r1.x, r1, c10.w
pow r5.x, r1.x, r0.x
mul r1.xyz, r2, c9
dp3_pp r2.x, r4, r3
mul r3.xyz, r1, c11.z
max_pp r1.x, r2, c10.w
mul_pp r2.xyz, r3, c1
mul_pp r3.xyz, r2, r1.x
mov r1.x, c7
mul r2.xyz, c2, r1.x
mov r0.x, r5.x
mul_pp r2.xyz, r2, c1
mul_pp r1.x, r6, c10
mad r0.xyz, r2, r0.x, r3
mul r0.xyz, r0, r1.x
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 46 ALU, 2 TEX
PARAM c[12] = { program.local[0..9],
		{ 0, 2, 1, 128 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
MOV result.color.w, c[10].x;
TEX R0.yw, R0, texture[0], 2D;
MAD R1.xy, R0.wyzw, c[10].y, -c[10].z;
ADD R0.zw, R1.xyxy, -c[11].x;
MUL R0.xy, fragment.texcoord[0].z, c[6];
MUL R0.zw, R0, c[5].x;
MUL R0.zw, R0, R0.xyxy;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
DP3 R0.w, fragment.texcoord[3], fragment.texcoord[3];
RSQ R0.w, R0.w;
MUL R2.xyz, R0.w, fragment.texcoord[3];
MUL R0.w, R1.y, R1.y;
DP3 R1.z, R2, R2;
MAD R0.w, -R1.x, R1.x, -R0;
RSQ R1.z, R1.z;
MAD R2.xyz, R1.z, R2, fragment.texcoord[2];
DP3 R1.w, R2, R2;
RSQ R1.w, R1.w;
ADD R0.w, R0, c[10].z;
RSQ R0.w, R0.w;
RCP R1.z, R0.w;
MOV R0.w, c[10].x;
MUL R2.xyz, R1.w, R2;
MAX R1.w, R0, c[8].x;
DP3 R0.w, R1, R2;
MUL R1.w, R1, c[10];
MAX R0.w, R0, c[10].x;
POW R0.w, R0.w, R1.w;
DP3 R1.x, R1, fragment.texcoord[2];
MAX R1.w, R1.x, c[10].x;
TEX R0.xyz, R0, texture[1], 2D;
MUL R0.xyz, R0, c[9];
MUL R1.xyz, R0, c[11].x;
MOV R0.xyz, c[2];
MAX R1.xyz, R1, c[10].x;
MUL R0.xyz, R0, c[7].x;
MUL R1.xyz, R1, c[1];
MAX R0.xyz, R0, c[10].x;
MUL R1.xyz, R1, R1.w;
MUL R0.xyz, R0, c[1];
MAD R0.xyz, R0, R0.w, R1;
MUL result.color.xyz, R0, c[10].y;
END
# 46 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
"ps_2_0
; 46 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
def c10, 2.00000000, -1.00000000, 1.00000000, 0.00000000
def c11, 128.00000000, -0.50000000, 0.50000000, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r3.xy, r0, c10.x, c10.y
add r0.xy, r3, c11.y
mul r0.xy, r0, c5.x
mul r1.xy, r0, r1
rcp r0.x, t0.w
mad r0.xy, t0, r0.x, r1
mov_pp r0.w, c10
texld r2, r0, s1
dp3_pp r0.x, t3, t3
rsq_pp r0.x, r0.x
mul_pp r4.xyz, r0.x, t3
dp3_pp r1.x, r4, r4
mul_pp r0.x, r3.y, r3.y
rsq_pp r1.x, r1.x
mad_pp r4.xyz, r1.x, r4, t2
mad_pp r0.x, -r3, r3, -r0
add_pp r1.x, r0, c10.z
dp3_pp r0.x, r4, r4
rsq_pp r1.x, r1.x
rcp_pp r3.z, r1.x
rsq_pp r0.x, r0.x
mul_pp r1.xyz, r0.x, r4
dp3_pp r1.x, r3, r1
mov_pp r0.x, c8
mul r2.xyz, r2, c9
mul_pp r0.x, c11, r0
max_pp r1.x, r1, c10.w
pow r4.x, r1.x, r0.x
dp3_pp r1.x, r3, t2
mul r3.xyz, r2, c11.z
mov r2.x, c7
mul r2.xyz, c2, r2.x
mov r0.x, r4.x
max_pp r1.x, r1, c10.w
mul_pp r3.xyz, r3, c1
mul_pp r1.xyz, r3, r1.x
mul_pp r2.xyz, r2, c1
mad r0.xyz, r2, r0.x, r1
mul r0.xyz, r0, c10.x
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "SPOT" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTexture0] 2D
SetTexture 3 [_LightTextureB0] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 61 ALU, 4 TEX
PARAM c[12] = { program.local[0..9],
		{ 0, 2, 1, 128 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
RCP R1.z, fragment.texcoord[4].w;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
MAD R1.zw, fragment.texcoord[4].xyxy, R1.z, c[11].x;
DP3 R2.x, fragment.texcoord[4], fragment.texcoord[4];
MOV result.color.w, c[10].x;
TEX R0.yw, R0, texture[0], 2D;
MAD R1.xy, R0.wyzw, c[10].y, -c[10].z;
ADD R0.xy, R1, -c[11].x;
MUL R0.xy, R0, c[5].x;
MUL R0.zw, fragment.texcoord[0].z, c[6].xyxy;
MUL R0.zw, R0.xyxy, R0;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
TEX R0.w, R1.zwzw, texture[2], 2D;
TEX R0.xyz, R0, texture[1], 2D;
TEX R1.w, R2.x, texture[3], 2D;
DP3 R1.z, fragment.texcoord[2], fragment.texcoord[2];
RSQ R2.x, R1.z;
MUL R2.xyz, R2.x, fragment.texcoord[2];
DP3 R2.w, R2, R2;
RSQ R2.w, R2.w;
DP3 R1.z, fragment.texcoord[3], fragment.texcoord[3];
RSQ R1.z, R1.z;
MUL R3.xyz, R1.z, fragment.texcoord[3];
DP3 R1.z, R3, R3;
MUL R0.xyz, R0, c[9];
MUL R2.xyz, R2.w, R2;
RSQ R1.z, R1.z;
MAD R3.xyz, R1.z, R3, R2;
MUL R1.z, R1.y, R1.y;
DP3 R2.w, R3, R3;
MAD R1.z, -R1.x, R1.x, -R1;
RSQ R2.w, R2.w;
MUL R3.xyz, R2.w, R3;
MOV R2.w, c[10].x;
ADD R1.z, R1, c[10];
RSQ R1.z, R1.z;
RCP R1.z, R1.z;
MAX R3.w, R2, c[8].x;
DP3 R2.w, R1, R3;
DP3 R1.x, R1, R2;
MAX R2.x, R1, c[10];
MUL R0.xyz, R0, c[11].x;
MAX R1.xyz, R0, c[10].x;
MOV R0.xyz, c[2];
MUL R1.xyz, R1, c[1];
MUL R1.xyz, R1, R2.x;
MUL R0.xyz, R0, c[7].x;
MAX R0.xyz, R0, c[10].x;
SLT R2.x, c[10], fragment.texcoord[4].z;
MUL R0.w, R2.x, R0;
MUL R0.w, R0, R1;
MUL R3.x, R3.w, c[10].w;
MAX R2.w, R2, c[10].x;
POW R2.w, R2.w, R3.x;
MUL R0.xyz, R0, c[1];
MUL R0.w, R0, c[10].y;
MAD R0.xyz, R0, R2.w, R1;
MUL result.color.xyz, R0, R0.w;
END
# 61 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "SPOT" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTexture0] 2D
SetTexture 3 [_LightTextureB0] 2D
"ps_2_0
; 60 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
def c10, 2.00000000, -1.00000000, 1.00000000, 0.00000000
def c11, 128.00000000, -0.50000000, 0.50000000, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
dcl t4
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r4.xy, r0, c10.x, c10.y
add r0.xy, r4, c11.y
mul r0.xy, r0, c5.x
mul r1.xy, r0, r1
rcp r0.x, t0.w
mad r2.xy, t0, r0.x, r1
rcp r1.x, t4.w
dp3 r0.x, t4, t4
mov r0.xy, r0.x
mad r1.xy, t4, r1.x, c11.z
texld r6, r0, s3
texld r0, r1, s2
texld r3, r2, s1
dp3_pp r1.x, t2, t2
rsq_pp r2.x, r1.x
mul_pp r0.x, r4.y, r4.y
mad_pp r0.x, -r4, r4, -r0
add_pp r0.x, r0, c10.z
rsq_pp r0.x, r0.x
rcp_pp r4.z, r0.x
mov_pp r0.x, c8
mul_pp r7.xyz, r2.x, t2
dp3_pp r1.x, t3, t3
rsq_pp r2.x, r1.x
mul_pp r5.xyz, r2.x, t3
dp3_pp r1.x, r7, r7
rsq_pp r2.x, r1.x
dp3_pp r1.x, r5, r5
mul_pp r2.xyz, r2.x, r7
rsq_pp r1.x, r1.x
mad_pp r5.xyz, r1.x, r5, r2
dp3_pp r1.x, r5, r5
rsq_pp r1.x, r1.x
mul_pp r1.xyz, r1.x, r5
dp3_pp r1.x, r4, r1
mul_pp r0.x, c11, r0
max_pp r1.x, r1, c10.w
pow r5.x, r1.x, r0.x
mul r1.xyz, r3, c9
mov r0.x, r5.x
mul r3.xyz, r1, c11.z
dp3_pp r2.x, r4, r2
max_pp r1.x, r2, c10.w
mul_pp r2.xyz, r3, c1
mul_pp r3.xyz, r2, r1.x
mov r2.x, c7
mul r2.xyz, c2, r2.x
cmp r1.x, -t4.z, c10.w, c10.z
mul_pp r1.x, r1, r0.w
mul_pp r1.x, r1, r6
mul_pp r2.xyz, r2, c1
mul_pp r1.x, r1, c10
mad r0.xyz, r2, r0.x, r3
mul r0.xyz, r0, r1.x
mov_pp r0.w, c10
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTextureB0] 2D
SetTexture 3 [_LightTexture0] CUBE
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 57 ALU, 4 TEX
PARAM c[12] = { program.local[0..9],
		{ 0, 2, 1, 128 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R1.w, fragment.texcoord[4], texture[3], CUBE;
DP3 R1.z, fragment.texcoord[2], fragment.texcoord[2];
RSQ R2.x, R1.z;
MUL R2.xyz, R2.x, fragment.texcoord[2];
DP3 R2.w, R2, R2;
RSQ R2.w, R2.w;
DP3 R1.z, fragment.texcoord[3], fragment.texcoord[3];
RSQ R1.z, R1.z;
MUL R3.xyz, R1.z, fragment.texcoord[3];
DP3 R1.z, R3, R3;
MUL R2.xyz, R2.w, R2;
RSQ R1.z, R1.z;
MAD R3.xyz, R1.z, R3, R2;
DP3 R2.w, R3, R3;
RSQ R2.w, R2.w;
MUL R3.xyz, R2.w, R3;
MOV R2.w, c[10].x;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
MAX R3.w, R2, c[8].x;
MOV result.color.w, c[10].x;
TEX R0.yw, R0, texture[0], 2D;
MAD R1.xy, R0.wyzw, c[10].y, -c[10].z;
ADD R0.xy, R1, -c[11].x;
MUL R1.z, R1.y, R1.y;
MAD R1.z, -R1.x, R1.x, -R1;
ADD R1.z, R1, c[10];
RSQ R1.z, R1.z;
RCP R1.z, R1.z;
DP3 R2.w, R1, R3;
DP3 R1.x, R1, R2;
MUL R0.xy, R0, c[5].x;
MUL R0.zw, fragment.texcoord[0].z, c[6].xyxy;
MUL R0.zw, R0.xyxy, R0;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
MAX R2.x, R1, c[10];
MUL R3.x, R3.w, c[10].w;
MAX R2.w, R2, c[10].x;
POW R2.w, R2.w, R3.x;
TEX R0.xyz, R0, texture[1], 2D;
TEX R0.w, R0.w, texture[2], 2D;
MUL R0.xyz, R0, c[9];
MUL R0.xyz, R0, c[11].x;
MAX R1.xyz, R0, c[10].x;
MOV R0.xyz, c[2];
MUL R1.xyz, R1, c[1];
MUL R0.xyz, R0, c[7].x;
MAX R0.xyz, R0, c[10].x;
MUL R0.w, R0, R1;
MUL R1.xyz, R1, R2.x;
MUL R0.xyz, R0, c[1];
MUL R0.w, R0, c[10].y;
MAD R0.xyz, R0, R2.w, R1;
MUL result.color.xyz, R0, R0.w;
END
# 57 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTextureB0] 2D
SetTexture 3 [_LightTexture0] CUBE
"ps_2_0
; 56 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_cube s3
def c10, 2.00000000, -1.00000000, 1.00000000, 0.00000000
def c11, 128.00000000, -0.50000000, 0.50000000, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
dcl t4.xyz
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r4.xy, r0, c10.x, c10.y
add r0.xy, r4, c11.y
mul r0.xy, r0, c5.x
mul r2.xy, r0, r1
rcp r1.x, t0.w
mad r2.xy, t0, r1.x, r2
dp3 r0.x, t4, t4
mov r1.xy, r0.x
texld r6, r1, s2
texld r3, r2, s1
texld r0, t4, s3
dp3_pp r1.x, t2, t2
rsq_pp r2.x, r1.x
mul_pp r0.x, r4.y, r4.y
mad_pp r0.x, -r4, r4, -r0
add_pp r0.x, r0, c10.z
rsq_pp r0.x, r0.x
rcp_pp r4.z, r0.x
mov_pp r0.x, c8
mul_pp r7.xyz, r2.x, t2
dp3_pp r1.x, t3, t3
rsq_pp r2.x, r1.x
mul_pp r5.xyz, r2.x, t3
dp3_pp r1.x, r7, r7
rsq_pp r2.x, r1.x
dp3_pp r1.x, r5, r5
mul_pp r2.xyz, r2.x, r7
rsq_pp r1.x, r1.x
mad_pp r5.xyz, r1.x, r5, r2
dp3_pp r1.x, r5, r5
rsq_pp r1.x, r1.x
mul_pp r1.xyz, r1.x, r5
dp3_pp r1.x, r4, r1
mul_pp r0.x, c11, r0
max_pp r1.x, r1, c10.w
pow r5.x, r1.x, r0.x
mul r1.xyz, r3, c9
mov r0.x, r5.x
mul r3.xyz, r1, c11.z
dp3_pp r2.x, r4, r2
max_pp r1.x, r2, c10.w
mul_pp r2.xyz, r3, c1
mul_pp r3.xyz, r2, r1.x
mov r2.x, c7
mul r2.xyz, c2, r2.x
mul r1.x, r6, r0.w
mul_pp r2.xyz, r2, c1
mul_pp r1.x, r1, c10
mad r0.xyz, r2, r0.x, r3
mul r0.xyz, r0, r1.x
mov_pp r0.w, c10
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTexture0] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 48 ALU, 3 TEX
PARAM c[12] = { program.local[0..9],
		{ 0, 2, 1, 128 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
DP3 R1.z, fragment.texcoord[3], fragment.texcoord[3];
RSQ R1.z, R1.z;
MUL R2.xyz, R1.z, fragment.texcoord[3];
DP3 R1.w, R2, R2;
RSQ R1.w, R1.w;
MAD R2.xyz, R1.w, R2, fragment.texcoord[2];
DP3 R1.w, R2, R2;
RSQ R2.w, R1.w;
MUL R0.zw, fragment.texcoord[1].xyxy, c[4].x;
MOV R0.xy, c[3];
MAD R0.xy, R0, c[0], R0.zwzw;
MUL R2.xyz, R2.w, R2;
MOV R1.w, c[10].x;
MAX R2.w, R1, c[8].x;
MOV result.color.w, c[10].x;
TEX R0.yw, R0, texture[0], 2D;
MAD R1.xy, R0.wyzw, c[10].y, -c[10].z;
ADD R0.zw, R1.xyxy, -c[11].x;
MUL R1.z, R1.y, R1.y;
MAD R1.z, -R1.x, R1.x, -R1;
ADD R1.z, R1, c[10];
RSQ R1.z, R1.z;
RCP R1.z, R1.z;
DP3 R1.w, R1, R2;
MUL R0.xy, fragment.texcoord[0].z, c[6];
MUL R0.zw, R0, c[5].x;
MUL R0.zw, R0, R0.xyxy;
RCP R0.x, fragment.texcoord[0].w;
MAD R0.xy, fragment.texcoord[0], R0.x, R0.zwzw;
MUL R2.x, R2.w, c[10].w;
MAX R1.w, R1, c[10].x;
POW R1.w, R1.w, R2.x;
DP3 R1.x, R1, fragment.texcoord[2];
MAX R2.x, R1, c[10];
TEX R0.xyz, R0, texture[1], 2D;
TEX R0.w, fragment.texcoord[4], texture[2], 2D;
MUL R0.xyz, R0, c[9];
MUL R1.xyz, R0, c[11].x;
MOV R0.xyz, c[2];
MAX R1.xyz, R1, c[10].x;
MUL R0.xyz, R0, c[7].x;
MUL R1.xyz, R1, c[1];
MAX R0.xyz, R0, c[10].x;
MUL R1.xyz, R1, R2.x;
MUL R0.xyz, R0, c[1];
MUL R0.w, R0, c[10].y;
MAD R0.xyz, R0, R1.w, R1;
MUL result.color.xyz, R0, R0.w;
END
# 48 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_Time]
Vector 1 [_LightColor0]
Vector 2 [_SpecColor]
Vector 3 [_ScrollSpeed]
Float 4 [_UVScale]
Float 5 [_Refraction]
Vector 6 [_GrabTexture_TexelSize]
Float 7 [_SpecPower]
Float 8 [_Glossiness]
Vector 9 [_Color]
SetTexture 0 [_BumpMap] 2D
SetTexture 1 [_GrabTexture] 2D
SetTexture 2 [_LightTexture0] 2D
"ps_2_0
; 47 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c10, 2.00000000, -1.00000000, 1.00000000, 0.00000000
def c11, 128.00000000, -0.50000000, 0.50000000, 0
dcl t0
dcl t1.xy
dcl t2.xyz
dcl t3.xyz
dcl t4.xy
mul r1.xy, t1, c4.x
mov r0.xy, c0
mad r0.xy, c3, r0, r1
mul r1.xy, t0.z, c6
texld r0, r0, s0
mov r0.x, r0.w
mad_pp r3.xy, r0, c10.x, c10.y
add r0.xy, r3, c11.y
mul r0.xy, r0, c5.x
mul r1.xy, r0, r1
rcp r0.x, t0.w
mad r0.xy, t0, r0.x, r1
texld r2, r0, s1
texld r0, t4, s2
dp3_pp r0.x, t3, t3
rsq_pp r0.x, r0.x
mul_pp r4.xyz, r0.x, t3
dp3_pp r1.x, r4, r4
mul_pp r0.x, r3.y, r3.y
rsq_pp r1.x, r1.x
mad_pp r4.xyz, r1.x, r4, t2
mad_pp r0.x, -r3, r3, -r0
dp3_pp r1.x, r4, r4
add_pp r0.x, r0, c10.z
rsq_pp r0.x, r0.x
rcp_pp r3.z, r0.x
rsq_pp r1.x, r1.x
mul_pp r1.xyz, r1.x, r4
dp3_pp r1.x, r3, r1
mov_pp r0.x, c8
mul r2.xyz, r2, c9
mul r2.xyz, r2, c11.z
mul_pp r0.x, c11, r0
max_pp r1.x, r1, c10.w
pow r4.x, r1.x, r0.x
dp3_pp r1.x, r3, t2
mov r0.x, r4.x
max_pp r1.x, r1, c10.w
mul_pp r2.xyz, r2, c1
mul_pp r3.xyz, r2, r1.x
mov r1.x, c7
mul r2.xyz, c2, r1.x
mul_pp r1.x, r0.w, c10
mul_pp r2.xyz, r2, c1
mad r0.xyz, r2, r0.x, r3
mul r0.xyz, r0, r1.x
mov_pp r0.w, c10
mov_pp oC0, r0
"
}
}
 }
}
Fallback "Transparent/Cutout/VertexLit"
}