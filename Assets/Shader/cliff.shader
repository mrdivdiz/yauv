Shader "ShaderFusion/cliff" {
Properties {
 _Color ("Diffuse Color", Color) = (1,1,1,1)
 _SpecColor ("Specular Color", Color) = (1,1,1,1)
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _Diffuse1 ("Diffuse1", 2D) = "white" {}
 _Diffuse2 ("Diffuse2", 2D) = "white" {}
 _Blend ("Blend", 2D) = "white" {}
 _BlendValue ("BlendValue", Float) = 1
 _VertexColorValue ("VertexColorValue", Float) = 1
 _Bump1 ("Bump1", 2D) = "white" {}
 _Bump2 ("Bump2", 2D) = "white" {}
 _value ("value", Float) = 1
}
SubShader { 
 LOD 600
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
  Lighting On
  AlphaToMask On
  ColorMask RGB
Program "vp" {
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [unity_Scale]
Vector 14 [_WorldSpaceCameraPos]
Vector 15 [_WorldSpaceLightPos0]
Vector 16 [unity_SHAr]
Vector 17 [unity_SHAg]
Vector 18 [unity_SHAb]
Vector 19 [unity_SHBr]
Vector 20 [unity_SHBg]
Vector 21 [unity_SHBb]
Vector 22 [unity_SHC]
Vector 23 [_Diffuse1_ST]
Vector 24 [_Diffuse2_ST]
Vector 25 [_Blend_ST]
Vector 26 [_Bump1_ST]
Vector 27 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 48 ALU
PARAM c[28] = { { 1 },
		state.matrix.mvp,
		program.local[5..27] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[13].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MOV R0.w, c[0].x;
MUL R1, R0.xyzz, R0.yzzx;
DP4 R2.z, R0, c[18];
DP4 R2.y, R0, c[17];
DP4 R2.x, R0, c[16];
MUL R0.w, R2, R2;
MAD R0.w, R0.x, R0.x, -R0;
DP4 R0.z, R1, c[21];
DP4 R0.y, R1, c[20];
DP4 R0.x, R1, c[19];
ADD R0.xyz, R2, R0;
MUL R1.xyz, R0.w, c[22];
ADD result.texcoord[5].xyz, R0, R1;
MOV R1.xyz, c[14];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[13].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[15];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.y, R0, c[10];
DP4 R3.x, R0, c[9];
DP3 result.texcoord[4].y, R3, R1;
DP3 result.texcoord[6].y, R1, R2;
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, R3, vertex.attrib[14];
DP3 result.texcoord[6].z, vertex.normal, R2;
DP3 result.texcoord[6].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[24].xyxy, c[24];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[23], c[23].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[26].xyxy, c[26];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[25], c[25].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[27], c[27].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 48 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [unity_Scale]
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Vector 15 [unity_SHAr]
Vector 16 [unity_SHAg]
Vector 17 [unity_SHAb]
Vector 18 [unity_SHBr]
Vector 19 [unity_SHBg]
Vector 20 [unity_SHBb]
Vector 21 [unity_SHC]
Vector 22 [_Diffuse1_ST]
Vector 23 [_Diffuse2_ST]
Vector 24 [_Blend_ST]
Vector 25 [_Bump1_ST]
Vector 26 [_Bump2_ST]
"vs_3_0
; 51 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c27, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mul r1.xyz, v2, c12.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mov r0.w, c27.x
mul r1, r0.xyzz, r0.yzzx
dp4 r2.z, r0, c17
dp4 r2.y, r0, c16
dp4 r2.x, r0, c15
mul r0.w, r2, r2
mad r0.w, r0.x, r0.x, -r0
dp4 r0.z, r1, c20
dp4 r0.y, r1, c19
dp4 r0.x, r1, c18
mul r1.xyz, r0.w, c21
add r0.xyz, r2, r0
add o6.xyz, r0, r1
mov r0.w, c27.x
mov r0.xyz, c13
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c12.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c14, r0
mov r0, c9
mov r1, c8
dp4 r4.y, c14, r0
dp4 r4.x, c14, r1
dp3 o5.y, r4, r2
dp3 o7.y, r2, r3
dp3 o5.z, v2, r4
dp3 o5.x, r4, v1
dp3 o7.z, v2, r3
dp3 o7.x, v1, r3
mov o4, v4
mad o1.zw, v3.xyxy, c23.xyxy, c23
mad o1.xy, v3, c22, c22.zwzw
mad o2.zw, v3.xyxy, c25.xyxy, c25
mad o2.xy, v3, c24, c24.zwzw
mad o3.xy, v3, c26, c26.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 14 [unity_LightmapST]
Vector 15 [_Diffuse1_ST]
Vector 16 [_Diffuse2_ST]
Vector 17 [_Blend_ST]
Vector 18 [_Bump1_ST]
Vector 19 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 11 ALU
PARAM c[20] = { program.local[0],
		state.matrix.mvp,
		program.local[5..19] };
MOV result.texcoord[3], vertex.color;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[16].xyxy, c[16];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[15], c[15].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[18].xyxy, c[18];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[17], c[17].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[19], c[19].zwzw;
MAD result.texcoord[4].xy, vertex.texcoord[1], c[14], c[14].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 11 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [unity_LightmapST]
Vector 13 [_Diffuse1_ST]
Vector 14 [_Diffuse2_ST]
Vector 15 [_Blend_ST]
Vector 16 [_Bump1_ST]
Vector 17 [_Bump2_ST]
"vs_3_0
; 11 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_position0 v0
dcl_texcoord0 v3
dcl_texcoord1 v4
dcl_color0 v5
mov o4, v5
mad o1.zw, v3.xyxy, c14.xyxy, c14
mad o1.xy, v3, c13, c13.zwzw
mad o2.zw, v3.xyxy, c16.xyxy, c16
mad o2.xy, v3, c15, c15.zwzw
mad o3.xy, v3, c17, c17.zwzw
mad o5.xy, v4, c12, c12.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
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
Vector 24 [_Diffuse1_ST]
Vector 25 [_Diffuse2_ST]
Vector 26 [_Blend_ST]
Vector 27 [_Bump1_ST]
Vector 28 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 53 ALU
PARAM c[29] = { { 1, 0.5 },
		state.matrix.mvp,
		program.local[5..28] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[14].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MOV R0.w, c[0].x;
MUL R1, R0.xyzz, R0.yzzx;
DP4 R2.z, R0, c[19];
DP4 R2.y, R0, c[18];
DP4 R2.x, R0, c[17];
MUL R0.w, R2, R2;
MAD R0.w, R0.x, R0.x, -R0;
DP4 R0.z, R1, c[22];
DP4 R0.y, R1, c[21];
DP4 R0.x, R1, c[20];
ADD R0.xyz, R2, R0;
MUL R1.xyz, R0.w, c[23];
ADD result.texcoord[5].xyz, R0, R1;
MOV R1.xyz, c[15];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[14].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[16];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.y, R0, c[10];
DP4 R3.x, R0, c[9];
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
DP3 result.texcoord[4].y, R3, R1;
DP3 result.texcoord[6].y, R1, R2;
MUL R1.xyz, R0.xyww, c[0].y;
MUL R1.y, R1, c[13].x;
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, R3, vertex.attrib[14];
DP3 result.texcoord[6].z, vertex.normal, R2;
DP3 result.texcoord[6].x, vertex.attrib[14], R2;
ADD result.texcoord[7].xy, R1, R1.z;
MOV result.position, R0;
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[7].zw, R0;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[25].xyxy, c[25];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[24], c[24].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[27].xyxy, c[27];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[26], c[26].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[28], c[28].zwzw;
END
# 53 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
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
Vector 24 [_Diffuse1_ST]
Vector 25 [_Diffuse2_ST]
Vector 26 [_Blend_ST]
Vector 27 [_Bump1_ST]
Vector 28 [_Bump2_ST]
"vs_3_0
; 56 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
dcl_texcoord7 o8
def c29, 1.00000000, 0.50000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mul r1.xyz, v2, c14.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mov r0.w, c29.x
mul r1, r0.xyzz, r0.yzzx
dp4 r2.z, r0, c19
dp4 r2.y, r0, c18
dp4 r2.x, r0, c17
mul r0.w, r2, r2
mad r0.w, r0.x, r0.x, -r0
dp4 r0.z, r1, c22
dp4 r0.y, r1, c21
dp4 r0.x, r1, c20
mul r1.xyz, r0.w, c23
add r0.xyz, r2, r0
add o6.xyz, r0, r1
mov r0.w, c29.x
mov r0.xyz, c15
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c14.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c16, r0
mov r0, c9
dp4 r4.y, c16, r0
mov r1, c8
dp4 r4.x, c16, r1
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r1.xyz, r0.xyww, c29.y
mul r1.y, r1, c12.x
dp3 o5.y, r4, r2
dp3 o7.y, r2, r3
dp3 o5.z, v2, r4
dp3 o5.x, r4, v1
dp3 o7.z, v2, r3
dp3 o7.x, v1, r3
mad o8.xy, r1.z, c13.zwzw, r1
mov o0, r0
mov o4, v4
mov o8.zw, r0
mad o1.zw, v3.xyxy, c25.xyxy, c25
mad o1.xy, v3, c24, c24.zwzw
mad o2.zw, v3.xyxy, c27.xyxy, c27
mad o2.xy, v3, c26, c26.zwzw
mad o3.xy, v3, c28, c28.zwzw
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 13 [_ProjectionParams]
Vector 15 [unity_LightmapST]
Vector 16 [_Diffuse1_ST]
Vector 17 [_Diffuse2_ST]
Vector 18 [_Blend_ST]
Vector 19 [_Bump1_ST]
Vector 20 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 16 ALU
PARAM c[21] = { { 0.5 },
		state.matrix.mvp,
		program.local[5..20] };
TEMP R0;
TEMP R1;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R1.xyz, R0.xyww, c[0].x;
MUL R1.y, R1, c[13].x;
ADD result.texcoord[5].xy, R1, R1.z;
MOV result.position, R0;
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[5].zw, R0;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[17].xyxy, c[17];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[16], c[16].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[19].xyxy, c[19];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[18], c[18].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[4].xy, vertex.texcoord[1], c[15], c[15].zwzw;
END
# 16 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_LightmapST]
Vector 15 [_Diffuse1_ST]
Vector 16 [_Diffuse2_ST]
Vector 17 [_Blend_ST]
Vector 18 [_Bump1_ST]
Vector 19 [_Bump2_ST]
"vs_3_0
; 16 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c20, 0.50000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v3
dcl_texcoord1 v4
dcl_color0 v5
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r1.xyz, r0.xyww, c20.x
mul r1.y, r1, c12.x
mad o6.xy, r1.z, c13.zwzw, r1
mov o0, r0
mov o4, v5
mov o6.zw, r0
mad o1.zw, v3.xyxy, c16.xyxy, c16
mad o1.xy, v3, c15, c15.zwzw
mad o2.zw, v3.xyxy, c18.xyxy, c18
mad o2.xy, v3, c17, c17.zwzw
mad o3.xy, v3, c19, c19.zwzw
mad o5.xy, v4, c14, c14.zwzw
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [unity_Scale]
Vector 14 [_WorldSpaceCameraPos]
Vector 15 [_WorldSpaceLightPos0]
Vector 16 [unity_4LightPosX0]
Vector 17 [unity_4LightPosY0]
Vector 18 [unity_4LightPosZ0]
Vector 19 [unity_4LightAtten0]
Vector 20 [unity_LightColor0]
Vector 21 [unity_LightColor1]
Vector 22 [unity_LightColor2]
Vector 23 [unity_LightColor3]
Vector 24 [unity_SHAr]
Vector 25 [unity_SHAg]
Vector 26 [unity_SHAb]
Vector 27 [unity_SHBr]
Vector 28 [unity_SHBg]
Vector 29 [unity_SHBb]
Vector 30 [unity_SHC]
Vector 31 [_Diffuse1_ST]
Vector 32 [_Diffuse2_ST]
Vector 33 [_Blend_ST]
Vector 34 [_Bump1_ST]
Vector 35 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 79 ALU
PARAM c[36] = { { 1, 0 },
		state.matrix.mvp,
		program.local[5..35] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R3.xyz, vertex.normal, c[13].w;
DP4 R0.x, vertex.position, c[6];
ADD R1, -R0.x, c[17];
DP3 R3.w, R3, c[6];
DP3 R4.x, R3, c[5];
DP3 R3.x, R3, c[7];
MUL R2, R3.w, R1;
DP4 R0.x, vertex.position, c[5];
ADD R0, -R0.x, c[16];
MUL R1, R1, R1;
MOV R4.z, R3.x;
MAD R2, R4.x, R0, R2;
MOV R4.w, c[0].x;
DP4 R4.y, vertex.position, c[7];
MAD R1, R0, R0, R1;
ADD R0, -R4.y, c[18];
MAD R1, R0, R0, R1;
MAD R0, R3.x, R0, R2;
MUL R2, R1, c[19];
MOV R4.y, R3.w;
RSQ R1.x, R1.x;
RSQ R1.y, R1.y;
RSQ R1.w, R1.w;
RSQ R1.z, R1.z;
MUL R0, R0, R1;
ADD R1, R2, c[0].x;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].y;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[21];
MAD R1.xyz, R0.x, c[20], R1;
MAD R0.xyz, R0.z, c[22], R1;
MAD R1.xyz, R0.w, c[23], R0;
MUL R0, R4.xyzz, R4.yzzx;
MUL R1.w, R3, R3;
DP4 R3.z, R0, c[29];
DP4 R3.y, R0, c[28];
DP4 R3.x, R0, c[27];
MAD R1.w, R4.x, R4.x, -R1;
MUL R0.xyz, R1.w, c[30];
MOV R1.w, c[0].x;
DP4 R2.z, R4, c[26];
DP4 R2.y, R4, c[25];
DP4 R2.x, R4, c[24];
ADD R2.xyz, R2, R3;
ADD R0.xyz, R2, R0;
ADD result.texcoord[5].xyz, R0, R1;
MOV R1.xyz, c[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[13].w, -vertex.position;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R1, c[15];
MUL R0.xyz, R0, vertex.attrib[14].w;
DP4 R3.z, R1, c[11];
DP4 R3.y, R1, c[10];
DP4 R3.x, R1, c[9];
DP3 result.texcoord[4].y, R3, R0;
DP3 result.texcoord[6].y, R0, R2;
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, R3, vertex.attrib[14];
DP3 result.texcoord[6].z, vertex.normal, R2;
DP3 result.texcoord[6].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[32].xyxy, c[32];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[31], c[31].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[34].xyxy, c[34];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[33], c[33].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[35], c[35].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 79 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [unity_Scale]
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Vector 15 [unity_4LightPosX0]
Vector 16 [unity_4LightPosY0]
Vector 17 [unity_4LightPosZ0]
Vector 18 [unity_4LightAtten0]
Vector 19 [unity_LightColor0]
Vector 20 [unity_LightColor1]
Vector 21 [unity_LightColor2]
Vector 22 [unity_LightColor3]
Vector 23 [unity_SHAr]
Vector 24 [unity_SHAg]
Vector 25 [unity_SHAb]
Vector 26 [unity_SHBr]
Vector 27 [unity_SHBg]
Vector 28 [unity_SHBb]
Vector 29 [unity_SHC]
Vector 30 [_Diffuse1_ST]
Vector 31 [_Diffuse2_ST]
Vector 32 [_Blend_ST]
Vector 33 [_Bump1_ST]
Vector 34 [_Bump2_ST]
"vs_3_0
; 82 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c35, 1.00000000, 0.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mul r3.xyz, v2, c12.w
dp4 r0.x, v0, c5
add r1, -r0.x, c16
dp3 r3.w, r3, c5
dp3 r4.x, r3, c4
dp3 r3.x, r3, c6
mul r2, r3.w, r1
dp4 r0.x, v0, c4
add r0, -r0.x, c15
mul r1, r1, r1
mov r4.z, r3.x
mad r2, r4.x, r0, r2
mov r4.w, c35.x
dp4 r4.y, v0, c6
mad r1, r0, r0, r1
add r0, -r4.y, c17
mad r1, r0, r0, r1
mad r0, r3.x, r0, r2
mul r2, r1, c18
mov r4.y, r3.w
rsq r1.x, r1.x
rsq r1.y, r1.y
rsq r1.w, r1.w
rsq r1.z, r1.z
mul r0, r0, r1
add r1, r2, c35.x
dp4 r2.z, r4, c25
dp4 r2.y, r4, c24
dp4 r2.x, r4, c23
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c35.y
mul r0, r0, r1
mul r1.xyz, r0.y, c20
mad r1.xyz, r0.x, c19, r1
mad r0.xyz, r0.z, c21, r1
mad r1.xyz, r0.w, c22, r0
mul r0, r4.xyzz, r4.yzzx
mul r1.w, r3, r3
dp4 r3.z, r0, c28
dp4 r3.y, r0, c27
dp4 r3.x, r0, c26
mad r1.w, r4.x, r4.x, -r1
mul r0.xyz, r1.w, c29
add r2.xyz, r2, r3
add r0.xyz, r2, r0
add o6.xyz, r0, r1
mov r1.w, c35.x
mov r1.xyz, c13
dp4 r0.z, r1, c10
dp4 r0.y, r1, c9
dp4 r0.x, r1, c8
mad r3.xyz, r0, c12.w, -v0
mov r1.xyz, v1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r1.yzxw
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c14, r0
mov r1, c9
mov r0, c8
dp4 r4.y, c14, r1
dp4 r4.x, c14, r0
dp3 o5.y, r4, r2
dp3 o7.y, r2, r3
dp3 o5.z, v2, r4
dp3 o5.x, r4, v1
dp3 o7.z, v2, r3
dp3 o7.x, v1, r3
mov o4, v4
mad o1.zw, v3.xyxy, c31.xyxy, c31
mad o1.xy, v3, c30, c30.zwzw
mad o2.zw, v3.xyxy, c33.xyxy, c33
mad o2.xy, v3, c32, c32.zwzw
mad o3.xy, v3, c34, c34.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "color" Color
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
Vector 32 [_Diffuse1_ST]
Vector 33 [_Diffuse2_ST]
Vector 34 [_Blend_ST]
Vector 35 [_Bump1_ST]
Vector 36 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 84 ALU
PARAM c[37] = { { 1, 0, 0.5 },
		state.matrix.mvp,
		program.local[5..36] };
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
MOV R4.w, c[0].x;
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
ADD R1, R2, c[0].x;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].y;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[22];
MAD R1.xyz, R0.x, c[21], R1;
MAD R0.xyz, R0.z, c[23], R1;
MAD R1.xyz, R0.w, c[24], R0;
MUL R0, R4.xyzz, R4.yzzx;
MUL R1.w, R3, R3;
DP4 R3.z, R0, c[30];
DP4 R3.y, R0, c[29];
DP4 R3.x, R0, c[28];
MAD R1.w, R4.x, R4.x, -R1;
MUL R0.xyz, R1.w, c[31];
MOV R1.w, c[0].x;
DP4 R0.w, vertex.position, c[4];
DP4 R2.z, R4, c[27];
DP4 R2.y, R4, c[26];
DP4 R2.x, R4, c[25];
ADD R2.xyz, R2, R3;
ADD R0.xyz, R2, R0;
ADD result.texcoord[5].xyz, R0, R1;
MOV R1.xyz, c[15];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[14].w, -vertex.position;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R1, c[16];
MUL R0.xyz, R0, vertex.attrib[14].w;
DP4 R3.z, R1, c[11];
DP4 R3.y, R1, c[10];
DP4 R3.x, R1, c[9];
DP3 result.texcoord[4].y, R3, R0;
DP3 result.texcoord[6].y, R0, R2;
DP4 R0.z, vertex.position, c[3];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R1.xyz, R0.xyww, c[0].z;
MUL R1.y, R1, c[13].x;
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, R3, vertex.attrib[14];
DP3 result.texcoord[6].z, vertex.normal, R2;
DP3 result.texcoord[6].x, vertex.attrib[14], R2;
ADD result.texcoord[7].xy, R1, R1.z;
MOV result.position, R0;
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[7].zw, R0;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[33].xyxy, c[33];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[32], c[32].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[35].xyxy, c[35];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[34], c[34].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[36], c[36].zwzw;
END
# 84 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "color" Color
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
Vector 32 [_Diffuse1_ST]
Vector 33 [_Diffuse2_ST]
Vector 34 [_Blend_ST]
Vector 35 [_Bump1_ST]
Vector 36 [_Bump2_ST]
"vs_3_0
; 87 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
dcl_texcoord7 o8
def c37, 1.00000000, 0.00000000, 0.50000000, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
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
mov r4.w, c37.x
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
add r1, r2, c37.x
dp4 r2.z, r4, c27
dp4 r2.y, r4, c26
dp4 r2.x, r4, c25
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c37.y
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
add r2.xyz, r2, r3
add r0.xyz, r2, r0
add o6.xyz, r0, r1
mov r1.w, c37.x
mov r1.xyz, c15
dp4 r0.z, r1, c10
dp4 r0.y, r1, c9
dp4 r0.x, r1, c8
mad r3.xyz, r0, c14.w, -v0
mov r1.xyz, v1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r1.yzxw
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c16, r0
mov r0, c8
dp4 r4.x, c16, r0
mov r1, c9
dp4 r4.y, c16, r1
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r1.xyz, r0.xyww, c37.z
mul r1.y, r1, c12.x
dp3 o5.y, r4, r2
dp3 o7.y, r2, r3
dp3 o5.z, v2, r4
dp3 o5.x, r4, v1
dp3 o7.z, v2, r3
dp3 o7.x, v1, r3
mad o8.xy, r1.z, c13.zwzw, r1
mov o0, r0
mov o4, v4
mov o8.zw, r0
mad o1.zw, v3.xyxy, c33.xyxy, c33
mad o1.xy, v3, c32, c32.zwzw
mad o2.zw, v3.xyxy, c35.xyxy, c35
mad o2.xy, v3, c34, c34.zwzw
mad o3.xy, v3, c36, c36.zwzw
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 53 ALU, 5 TEX
PARAM c[8] = { program.local[0..6],
		{ 0, 2, 1, 128 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
DP3 R0.w, fragment.texcoord[6], fragment.texcoord[6];
RSQ R0.w, R0.w;
MUL R4.xyz, R0.w, fragment.texcoord[6];
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
MUL R3.xyz, fragment.texcoord[3], c[3].x;
MUL R0.xyz, R0, R3;
MUL R0.xyz, R0, c[5];
MAX R3.xyz, R0, c[7].x;
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R4, R4;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
MUL R0.xyz, R0, c[4].x;
DP3 R1.w, R0, fragment.texcoord[4];
RSQ R0.w, R2.w;
MAD R1.xyz, R0.w, R4, fragment.texcoord[4];
DP3 R0.w, R1, R1;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, R1;
DP3 R0.x, R0, R1;
MAX R1.x, R0, c[7];
MOV R0.w, c[7].x;
MAX R0.xyz, R0.w, c[1];
MUL R0.xyz, R0, c[0];
MUL R5.xyz, R3, c[0];
MAX R1.w, R1, c[7].x;
MUL R2.xyz, R5, R1.w;
POW R1.x, R1.x, c[7].w;
MAD R1.xyz, R0, R1.x, R2;
MAX R0.x, R0.w, c[5].w;
MUL R1.xyz, R1, c[7].y;
SLT R0.y, R0.x, c[6].x;
MAD result.color.xyz, R3, fragment.texcoord[5], R1;
KIL -R0.y;
MOV result.color.w, R0.x;
END
# 53 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
"ps_3_0
; 50 ALU, 6 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6.xyz
texld r2.yw, v1.zwzw, s3
mad_pp r2.xy, r2.wyzw, c7.z, c7.w
mul_pp r0.z, r2.y, r2.y
mad_pp r0.z, -r2.x, r2.x, -r0
dp3_pp r0.x, v6, v6
rsq_pp r0.x, r0.x
mul_pp r1.xyz, r0.x, v6
dp3_pp r0.x, r1, r1
rsq_pp r0.x, r0.x
add_pp r0.z, r0, c7.y
rsq_pp r0.z, r0.z
mov_pp r1.w, c5
mad_pp r1.xyz, r0.x, r1, v4
texld r0.yw, v2, s4
mad_pp r0.xy, r0.wyzw, c7.z, c7.w
mul_pp r0.w, r0.y, r0.y
mad_pp r0.w, -r0.x, r0.x, -r0
add_pp r0.w, r0, c7.y
rcp_pp r2.z, r0.z
rsq_pp r0.w, r0.w
rcp_pp r0.z, r0.w
add r3.xyz, r0, -r2
dp3_pp r0.w, r1, r1
texld r0.xyz, v1, s2
mul r0.xyz, r0, c2.x
mad r2.xyz, r0, r3, r2
rsq_pp r0.w, r0.w
mul_pp r3.xyz, r0.w, r1
mul r1.xyz, r2, c4.x
dp3_pp r0.w, r1, r3
max_pp r0.w, r0, c7.x
pow r2, r0.w, c8.x
dp3_pp r0.w, r1, v4
mul r2.yzw, v3.xxyz, c3.x
texld r3.xyz, v0, s0
texld r4.xyz, v0.zwzw, s1
add r4.xyz, r4, -r3
mad r0.xyz, r0, r4, r3
mul r0.xyz, r0, r2.yzww
mul r0.xyz, r0, c5
max_pp r0.w, r0, c7.x
mul_pp r1.xyz, r0, c0
mul_pp r1.xyz, r1, r0.w
mov r0.w, r2.x
mov_pp r2.xyz, c0
mul_pp r2.xyz, c1, r2
mad r1.xyz, r2, r0.w, r1
add_pp r1.w, -c6.x, r1
cmp r0.w, r1, c7.x, c7.y
mul r1.xyz, r1, c7.z
mov_pp r2, -r0.w
mad_pp oC0.xyz, r0, v5, r1
texkill r2.xyzw
mov_pp oC0.w, c5
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Float 0 [_BlendValue]
Float 1 [_VertexColorValue]
Vector 2 [_Color]
Float 3 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [unity_Lightmap] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 19 ALU, 4 TEX
PARAM c[5] = { program.local[0..3],
		{ 0, 8 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R2.xyz, R1, -R0;
TEX R1.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R1.xyz, R1, c[0].x;
MAD R0.xyz, R1, R2, R0;
MUL R3.xyz, fragment.texcoord[3], c[1].x;
MUL R0.xyz, R0, R3;
MUL R0.xyz, R0, c[2];
MAX R1.xyz, R0, c[4].x;
TEX R0, fragment.texcoord[4], texture[5], 2D;
MUL R0.xyz, R0.w, R0;
MUL R0.xyz, R0, R1;
MOV R1.w, c[4].x;
MAX R0.w, R1, c[2];
SLT R1.x, R0.w, c[3];
MUL result.color.xyz, R0, c[4].y;
KIL -R1.x;
MOV result.color.w, R0;
END
# 19 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Float 0 [_BlendValue]
Float 1 [_VertexColorValue]
Vector 2 [_Color]
Float 3 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [unity_Lightmap] 2D
"ps_3_0
; 14 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s5
def c4, 0.00000000, 1.00000000, 8.00000000, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xy
mov_pp r1.w, c2
texld r0.xyz, v0, s0
texld r1.xyz, v0.zwzw, s1
add r2.xyz, r1, -r0
texld r1.xyz, v1, s2
mul r1.xyz, r1, c0.x
mad r0.xyz, r1, r2, r0
mul r3.xyz, v3, c1.x
mul r0.xyz, r0, r3
mul r1.xyz, r0, c2
texld r0, v4, s5
mul_pp r0.xyz, r0.w, r0
mul_pp r1.xyz, r0, r1
add_pp r1.w, -c3.x, r1
cmp r0.w, r1, c4.x, c4.y
mov_pp r0, -r0.w
mul_pp oC0.xyz, r1, c4.z
texkill r0.xyzw
mov_pp oC0.w, c2
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_ShadowMapTexture] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 55 ALU, 6 TEX
PARAM c[8] = { program.local[0..6],
		{ 0, 2, 1, 128 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
DP3 R0.w, fragment.texcoord[6], fragment.texcoord[6];
RSQ R0.w, R0.w;
MUL R4.xyz, R0.w, fragment.texcoord[6];
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
MUL R3.xyz, fragment.texcoord[3], c[3].x;
MUL R0.xyz, R0, R3;
MUL R0.xyz, R0, c[5];
MAX R3.xyz, R0, c[7].x;
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R4, R4;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
MUL R0.xyz, R0, c[4].x;
DP3 R1.w, R0, fragment.texcoord[4];
RSQ R0.w, R2.w;
MAD R1.xyz, R0.w, R4, fragment.texcoord[4];
DP3 R0.w, R1, R1;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, R1;
DP3 R0.x, R0, R1;
MAX R1.x, R0, c[7];
MOV R0.w, c[7].x;
MAX R0.xyz, R0.w, c[1];
POW R1.y, R1.x, c[7].w;
MAX R0.w, R0, c[5];
TXP R1.x, fragment.texcoord[7], texture[5], 2D;
MUL R1.x, R1, c[7].y;
MUL R5.xyz, R3, c[0];
MAX R1.w, R1, c[7].x;
MUL R2.xyz, R5, R1.w;
MUL R0.xyz, R0, c[0];
MAD R0.xyz, R0, R1.y, R2;
MUL R0.xyz, R0, R1.x;
SLT R1.x, R0.w, c[6];
MAD result.color.xyz, R3, fragment.texcoord[5], R0;
KIL -R1.x;
MOV result.color.w, R0;
END
# 55 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_ShadowMapTexture] 2D
"ps_3_0
; 51 ALU, 7 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
dcl_2d s5
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6.xyz
dcl_texcoord7 v7
dp3_pp r0.x, v6, v6
rsq_pp r0.x, r0.x
mul_pp r1.xyz, r0.x, v6
dp3_pp r0.x, r1, r1
rsq_pp r0.x, r0.x
mad_pp r2.xyz, r0.x, r1, v4
texld r1.yw, v1.zwzw, s3
mad_pp r1.xy, r1.wyzw, c7.z, c7.w
texld r0.yw, v2, s4
mad_pp r0.xy, r0.wyzw, c7.z, c7.w
mul_pp r0.z, r1.y, r1.y
mul_pp r0.w, r0.y, r0.y
mad_pp r0.z, -r1.x, r1.x, -r0
mad_pp r0.w, -r0.x, r0.x, -r0
add_pp r0.z, r0, c7.y
rsq_pp r0.z, r0.z
add_pp r0.w, r0, c7.y
rcp_pp r1.z, r0.z
rsq_pp r0.w, r0.w
rcp_pp r0.z, r0.w
add r3.xyz, r0, -r1
dp3_pp r0.w, r2, r2
rsq_pp r0.w, r0.w
texld r0.xyz, v1, s2
mul r0.xyz, r0, c2.x
mad r1.xyz, r0, r3, r1
mul_pp r2.xyz, r0.w, r2
mul r1.xyz, r1, c4.x
dp3_pp r0.w, r1, r2
max_pp r0.w, r0, c7.x
pow r2, r0.w, c8.x
dp3_pp r0.w, r1, v4
texld r3.xyz, v0, s0
texld r4.xyz, v0.zwzw, s1
add r2.yzw, r4.xxyz, -r3.xxyz
mad r0.xyz, r0, r2.yzww, r3
mul r4.xyz, v3, c3.x
mul r0.xyz, r0, r4
mul r0.xyz, r0, c5
max_pp r0.w, r0, c7.x
mul_pp r1.xyz, r0, c0
mul_pp r1.xyz, r1, r0.w
mov r0.w, r2.x
mov_pp r3.xyz, c0
mul_pp r2.xyz, c1, r3
mad r2.xyz, r2, r0.w, r1
mov_pp r0.w, c5
add_pp r1.y, -c6.x, r0.w
texldp r1.x, v7, s5
cmp r1.w, r1.y, c7.x, c7.y
mul_pp r0.w, r1.x, c7.z
mul r1.xyz, r2, r0.w
mov_pp r2, -r1.w
mad_pp oC0.xyz, r0, v5, r1
texkill r2.xyzw
mov_pp oC0.w, c5
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Float 0 [_BlendValue]
Float 1 [_VertexColorValue]
Vector 2 [_Color]
Float 3 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_ShadowMapTexture] 2D
SetTexture 6 [unity_Lightmap] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 25 ALU, 5 TEX
PARAM c[5] = { program.local[0..3],
		{ 0, 8, 2 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R2.xyz, R1, -R0;
TEX R1.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R1.xyz, R1, c[0].x;
MAD R0.xyz, R1, R2, R0;
MUL R3.xyz, fragment.texcoord[3], c[1].x;
MUL R0.xyz, R0, R3;
MUL R0.xyz, R0, c[2];
MAX R1.xyz, R0, c[4].x;
TEX R0, fragment.texcoord[4], texture[6], 2D;
TXP R3.x, fragment.texcoord[5], texture[5], 2D;
MUL R2.xyz, R0, R3.x;
MUL R0.xyz, R0.w, R0;
MOV R0.w, c[4].x;
MAX R0.w, R0, c[2];
SLT R1.w, R0, c[3].x;
MUL R0.xyz, R0, c[4].y;
MUL R2.xyz, R2, c[4].z;
MIN R2.xyz, R0, R2;
MUL R0.xyz, R0, R3.x;
MAX R0.xyz, R2, R0;
MUL result.color.xyz, R1, R0;
KIL -R1.w;
MOV result.color.w, R0;
END
# 25 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
Float 0 [_BlendValue]
Float 1 [_VertexColorValue]
Vector 2 [_Color]
Float 3 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_ShadowMapTexture] 2D
SetTexture 6 [unity_Lightmap] 2D
"ps_3_0
; 19 ALU, 6 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s5
dcl_2d s6
def c4, 0.00000000, 1.00000000, 8.00000000, 2.00000000
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xy
dcl_texcoord5 v5
texld r0.xyz, v0, s0
texld r1.xyz, v0.zwzw, s1
add r2.xyz, r1, -r0
texld r1.xyz, v1, s2
mul r1.xyz, r1, c0.x
mad r0.xyz, r1, r2, r0
mul r3.xyz, v3, c1.x
mul r0.xyz, r0, r3
mul r1.xyz, r0, c2
texld r0, v4, s6
mul_pp r2.xyz, r0.w, r0
texldp r3.x, v5, s5
mul_pp r0.xyz, r0, r3.x
mov_pp r0.w, c2
add_pp r0.w, -c3.x, r0
mul_pp r2.xyz, r2, c4.z
mul_pp r0.xyz, r0, c4.w
min_pp r0.xyz, r2, r0
mul_pp r2.xyz, r2, r3.x
max_pp r2.xyz, r0, r2
cmp r0.w, r0, c4.x, c4.y
mov_pp r0, -r0.w
mul_pp oC0.xyz, r1, r2
texkill r0.xyzw
mov_pp oC0.w, c2
"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "RenderType"="Opaque" }
  Lighting On
  AlphaToMask On
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
  ColorMask RGB
Program "vp" {
SubProgram "opengl " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Vector 20 [_Diffuse1_ST]
Vector 21 [_Diffuse2_ST]
Vector 22 [_Blend_ST]
Vector 23 [_Bump1_ST]
Vector 24 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 38 ALU
PARAM c[25] = { { 1 },
		state.matrix.mvp,
		program.local[5..24] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.x, R0, c[9];
DP4 R3.y, R0, c[10];
MAD R0.xyz, R3, c[17].w, -vertex.position;
DP3 result.texcoord[4].y, R0, R1;
DP3 result.texcoord[4].z, vertex.normal, R0;
DP3 result.texcoord[4].x, R0, vertex.attrib[14];
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[5].y, R1, R2;
DP3 result.texcoord[5].z, vertex.normal, R2;
DP3 result.texcoord[5].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
DP4 result.texcoord[6].z, R0, c[15];
DP4 result.texcoord[6].y, R0, c[14];
DP4 result.texcoord[6].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[23].xyxy, c[23];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[22], c[22].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[24], c[24].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 38 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Vector 19 [_Diffuse1_ST]
Vector 20 [_Diffuse2_ST]
Vector 21 [_Blend_ST]
Vector 22 [_Bump1_ST]
Vector 23 [_Bump2_ST]
"vs_3_0
; 41 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c24, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mov r0.w, c24.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
mad r0.xyz, r4, c16.w, -v0
dp3 o5.y, r0, r2
dp3 o5.z, v2, r0
dp3 o5.x, r0, v1
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o6.y, r2, r3
dp3 o6.z, v2, r3
dp3 o6.x, v1, r3
mov o4, v4
dp4 o7.z, r0, c14
dp4 o7.y, r0, c13
dp4 o7.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
mad o2.zw, v3.xyxy, c22.xyxy, c22
mad o2.xy, v3, c21, c21.zwzw
mad o3.xy, v3, c23, c23.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_World2Object]
Vector 9 [unity_Scale]
Vector 10 [_WorldSpaceCameraPos]
Vector 11 [_WorldSpaceLightPos0]
Vector 12 [_Diffuse1_ST]
Vector 13 [_Diffuse2_ST]
Vector 14 [_Blend_ST]
Vector 15 [_Bump1_ST]
Vector 16 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 30 ALU
PARAM c[17] = { { 1 },
		state.matrix.mvp,
		program.local[5..16] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[10];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[7];
DP4 R2.y, R1, c[6];
DP4 R2.x, R1, c[5];
MAD R2.xyz, R2, c[9].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[11];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[7];
DP4 R3.y, R0, c[6];
DP4 R3.x, R0, c[5];
DP3 result.texcoord[4].y, R3, R1;
DP3 result.texcoord[5].y, R1, R2;
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, R3, vertex.attrib[14];
DP3 result.texcoord[5].z, vertex.normal, R2;
DP3 result.texcoord[5].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[13].xyxy, c[13];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[12], c[12].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[15].xyxy, c[15];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[14], c[14].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[16], c[16].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 30 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_World2Object]
Vector 8 [unity_Scale]
Vector 9 [_WorldSpaceCameraPos]
Vector 10 [_WorldSpaceLightPos0]
Vector 11 [_Diffuse1_ST]
Vector 12 [_Diffuse2_ST]
Vector 13 [_Blend_ST]
Vector 14 [_Bump1_ST]
Vector 15 [_Bump2_ST]
"vs_3_0
; 33 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c16, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mov r0.w, c16.x
mov r0.xyz, c9
dp4 r1.z, r0, c6
dp4 r1.y, r0, c5
dp4 r1.x, r0, c4
mad r3.xyz, r1, c8.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c6
dp4 r4.z, c10, r0
mov r0, c5
mov r1, c4
dp4 r4.y, c10, r0
dp4 r4.x, c10, r1
dp3 o5.y, r4, r2
dp3 o6.y, r2, r3
dp3 o5.z, v2, r4
dp3 o5.x, r4, v1
dp3 o6.z, v2, r3
dp3 o6.x, v1, r3
mov o4, v4
mad o1.zw, v3.xyxy, c12.xyxy, c12
mad o1.xy, v3, c11, c11.zwzw
mad o2.zw, v3.xyxy, c14.xyxy, c14
mad o2.xy, v3, c13, c13.zwzw
mad o3.xy, v3, c15, c15.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Vector 20 [_Diffuse1_ST]
Vector 21 [_Diffuse2_ST]
Vector 22 [_Blend_ST]
Vector 23 [_Bump1_ST]
Vector 24 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 39 ALU
PARAM c[25] = { { 1 },
		state.matrix.mvp,
		program.local[5..24] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.x, R0, c[9];
DP4 R3.y, R0, c[10];
MAD R0.xyz, R3, c[17].w, -vertex.position;
DP4 R0.w, vertex.position, c[8];
DP3 result.texcoord[4].y, R0, R1;
DP3 result.texcoord[4].z, vertex.normal, R0;
DP3 result.texcoord[4].x, R0, vertex.attrib[14];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[5].y, R1, R2;
DP3 result.texcoord[5].z, vertex.normal, R2;
DP3 result.texcoord[5].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
DP4 result.texcoord[6].w, R0, c[16];
DP4 result.texcoord[6].z, R0, c[15];
DP4 result.texcoord[6].y, R0, c[14];
DP4 result.texcoord[6].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[23].xyxy, c[23];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[22], c[22].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[24], c[24].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 39 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Vector 19 [_Diffuse1_ST]
Vector 20 [_Diffuse2_ST]
Vector 21 [_Blend_ST]
Vector 22 [_Bump1_ST]
Vector 23 [_Bump2_ST]
"vs_3_0
; 42 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c24, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mov r0.w, c24.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
mad r0.xyz, r4, c16.w, -v0
dp4 r0.w, v0, c7
dp3 o5.y, r0, r2
dp3 o5.z, v2, r0
dp3 o5.x, r0, v1
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o6.y, r2, r3
dp3 o6.z, v2, r3
dp3 o6.x, v1, r3
mov o4, v4
dp4 o7.w, r0, c15
dp4 o7.z, r0, c14
dp4 o7.y, r0, c13
dp4 o7.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
mad o2.zw, v3.xyxy, c22.xyxy, c22
mad o2.xy, v3, c21, c21.zwzw
mad o3.xy, v3, c23, c23.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Vector 20 [_Diffuse1_ST]
Vector 21 [_Diffuse2_ST]
Vector 22 [_Blend_ST]
Vector 23 [_Bump1_ST]
Vector 24 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 38 ALU
PARAM c[25] = { { 1 },
		state.matrix.mvp,
		program.local[5..24] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.x, R0, c[9];
DP4 R3.y, R0, c[10];
MAD R0.xyz, R3, c[17].w, -vertex.position;
DP3 result.texcoord[4].y, R0, R1;
DP3 result.texcoord[4].z, vertex.normal, R0;
DP3 result.texcoord[4].x, R0, vertex.attrib[14];
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[5].y, R1, R2;
DP3 result.texcoord[5].z, vertex.normal, R2;
DP3 result.texcoord[5].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
DP4 result.texcoord[6].z, R0, c[15];
DP4 result.texcoord[6].y, R0, c[14];
DP4 result.texcoord[6].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[23].xyxy, c[23];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[22], c[22].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[24], c[24].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 38 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Vector 19 [_Diffuse1_ST]
Vector 20 [_Diffuse2_ST]
Vector 21 [_Blend_ST]
Vector 22 [_Bump1_ST]
Vector 23 [_Bump2_ST]
"vs_3_0
; 41 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c24, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mov r0.w, c24.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
mad r0.xyz, r4, c16.w, -v0
dp3 o5.y, r0, r2
dp3 o5.z, v2, r0
dp3 o5.x, r0, v1
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o6.y, r2, r3
dp3 o6.z, v2, r3
dp3 o6.x, v1, r3
mov o4, v4
dp4 o7.z, r0, c14
dp4 o7.y, r0, c13
dp4 o7.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
mad o2.zw, v3.xyxy, c22.xyxy, c22
mad o2.xy, v3, c21, c21.zwzw
mad o3.xy, v3, c23, c23.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Matrix 13 [_LightMatrix0]
Vector 17 [unity_Scale]
Vector 18 [_WorldSpaceCameraPos]
Vector 19 [_WorldSpaceLightPos0]
Vector 20 [_Diffuse1_ST]
Vector 21 [_Diffuse2_ST]
Vector 22 [_Blend_ST]
Vector 23 [_Bump1_ST]
Vector 24 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 36 ALU
PARAM c[25] = { { 1 },
		state.matrix.mvp,
		program.local[5..24] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R1.xyz, c[18];
MOV R1.w, c[0].x;
MOV R0.xyz, vertex.attrib[14];
DP4 R2.z, R1, c[11];
DP4 R2.y, R1, c[10];
DP4 R2.x, R1, c[9];
MAD R2.xyz, R2, c[17].w, -vertex.position;
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R1.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MOV R0, c[19];
MUL R1.xyz, R1, vertex.attrib[14].w;
DP4 R3.z, R0, c[11];
DP4 R3.y, R0, c[10];
DP4 R3.x, R0, c[9];
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[4].y, R3, R1;
DP3 result.texcoord[5].y, R1, R2;
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, R3, vertex.attrib[14];
DP3 result.texcoord[5].z, vertex.normal, R2;
DP3 result.texcoord[5].x, vertex.attrib[14], R2;
MOV result.texcoord[3], vertex.color;
DP4 result.texcoord[6].y, R0, c[14];
DP4 result.texcoord[6].x, R0, c[13];
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[23].xyxy, c[23];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[22], c[22].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[24], c[24].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 36 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Matrix 12 [_LightMatrix0]
Vector 16 [unity_Scale]
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Vector 19 [_Diffuse1_ST]
Vector 20 [_Diffuse2_ST]
Vector 21 [_Blend_ST]
Vector 22 [_Bump1_ST]
Vector 23 [_Bump2_ST]
"vs_3_0
; 39 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c24, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mov r0.w, c24.x
mov r0.xyz, c17
dp4 r1.z, r0, c10
dp4 r1.y, r0, c9
dp4 r1.x, r0, c8
mad r3.xyz, r1, c16.w, -v0
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r1.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r1, v1.w
mov r0, c10
dp4 r4.z, c18, r0
mov r0, c9
dp4 r4.y, c18, r0
mov r1, c8
dp4 r4.x, c18, r1
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o5.y, r4, r2
dp3 o6.y, r2, r3
dp3 o5.z, v2, r4
dp3 o5.x, r4, v1
dp3 o6.z, v2, r3
dp3 o6.x, v1, r3
mov o4, v4
dp4 o7.y, r0, c13
dp4 o7.x, r0, c12
mad o1.zw, v3.xyxy, c20.xyxy, c20
mad o1.xy, v3, c19, c19.zwzw
mad o2.zw, v3.xyxy, c22.xyxy, c22
mad o2.xy, v3, c21, c21.zwzw
mad o3.xy, v3, c23, c23.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "POINT" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTexture0] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 61 ALU, 6 TEX
PARAM c[8] = { program.local[0..6],
		{ 0, 2, 1, 128 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, fragment.texcoord[4];
DP3 R1.w, R1, R1;
RSQ R1.w, R1.w;
MUL R3.xyz, R1.w, R1;
MUL R1.xyz, fragment.texcoord[3], c[3].x;
MUL R5.xyz, R0, R1;
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
DP3 R0.w, fragment.texcoord[5], fragment.texcoord[5];
RSQ R0.w, R0.w;
MUL R4.xyz, R0.w, fragment.texcoord[5];
DP3 R0.w, R4, R4;
RSQ R0.w, R0.w;
MAD R4.xyz, R0.w, R4, R3;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R4, R4;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
RSQ R0.w, R2.w;
MUL R0.xyz, R0, c[4].x;
MUL R1.xyz, R0.w, R4;
DP3 R0.w, R0, R1;
DP3 R0.x, R0, R3;
MUL R1.xyz, R5, c[5];
MAX R0.w, R0, c[7].x;
MAX R1.xyz, R1, c[7].x;
MAX R2.x, R0, c[7];
MOV R1.w, c[7].x;
MUL R1.xyz, R1, c[0];
MUL R1.xyz, R1, R2.x;
MAX R0.xyz, R1.w, c[1];
MUL R0.xyz, R0, c[0];
POW R0.w, R0.w, c[7].w;
MAD R1.xyz, R0, R0.w, R1;
MAX R0.x, R1.w, c[5].w;
DP3 R2.x, fragment.texcoord[6], fragment.texcoord[6];
TEX R0.w, R2.x, texture[5], 2D;
MUL R0.y, R0.w, c[7];
SLT R0.z, R0.x, c[6].x;
MUL result.color.xyz, R1, R0.y;
KIL -R0.z;
MOV result.color.w, R0.x;
END
# 61 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTexture0] 2D
"ps_3_0
; 57 ALU, 7 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
dcl_2d s5
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6.xyz
dp3_pp r0.x, v4, v4
rsq_pp r0.y, r0.x
mul_pp r1.xyz, r0.y, v4
dp3_pp r0.w, r1, r1
rsq_pp r1.w, r0.w
mul_pp r3.xyz, r1.w, r1
dp3_pp r0.x, v5, v5
rsq_pp r0.x, r0.x
mul_pp r0.xyz, r0.x, v5
dp3_pp r0.w, r0, r0
rsq_pp r0.w, r0.w
mad_pp r4.xyz, r0.w, r0, r3
dp3_pp r0.x, r4, r4
rsq_pp r0.w, r0.x
texld r5.yw, v1.zwzw, s3
mad_pp r0.xy, r5.wyzw, c7.z, c7.w
mul_pp r1.w, r0.y, r0.y
mad_pp r1.w, -r0.x, r0.x, -r1
add_pp r1.w, r1, c7.y
texld r1.xyz, v1, s2
texld r2.yw, v2, s4
mad_pp r2.xy, r2.wyzw, c7.z, c7.w
mul_pp r0.z, r2.y, r2.y
mad_pp r0.z, -r2.x, r2.x, -r0
rsq_pp r2.z, r1.w
add_pp r0.z, r0, c7.y
rsq_pp r1.w, r0.z
rcp_pp r0.z, r2.z
rcp_pp r2.z, r1.w
add r2.xyz, r2, -r0
mul r1.xyz, r1, c2.x
mad r0.xyz, r1, r2, r0
mul r2.xyz, r0, c4.x
mul_pp r4.xyz, r0.w, r4
dp3_pp r0.x, r2, r4
max_pp r1.w, r0.x, c7.x
pow r0, r1.w, c8.x
texld r4.xyz, v0, s0
texld r5.xyz, v0.zwzw, s1
add r0.yzw, r5.xxyz, -r4.xxyz
mad r1.xyz, r1, r0.yzww, r4
mul r5.xyz, v3, c3.x
mul r1.xyz, r1, r5
mov r0.w, r0.x
mul r0.xyz, r1, c5
dp3_pp r1.x, r2, r3
max_pp r1.w, r1.x, c7.x
mul_pp r0.xyz, r0, c0
mov_pp r1.xyz, c0
mul_pp r0.xyz, r0, r1.w
mul_pp r1.xyz, c1, r1
mad r1.xyz, r1, r0.w, r0
mov_pp r0.y, c5.w
dp3 r0.x, v6, v6
texld r0.x, r0.x, s5
mul_pp r1.w, r0.x, c7.z
add_pp r0.y, -c6.x, r0
cmp r0.y, r0, c7.x, c7
mov_pp r0, -r0.y
mul oC0.xyz, r1, r1.w
texkill r0.xyzw
mov_pp oC0.w, c5
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 52 ALU, 5 TEX
PARAM c[8] = { program.local[0..6],
		{ 0, 2, 1, 128 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
MUL R3.xyz, fragment.texcoord[3], c[3].x;
MUL R1.xyz, R0, R3;
DP3 R0.x, fragment.texcoord[5], fragment.texcoord[5];
RSQ R0.w, R0.x;
MUL R1.xyz, R1, c[5];
MAX R0.xyz, R1, c[7].x;
MUL R3.xyz, R0.w, fragment.texcoord[5];
MUL R4.xyz, R0, c[0];
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R3, R3;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
MUL R0.xyz, R0, c[4].x;
DP3 R1.w, R0, fragment.texcoord[4];
RSQ R0.w, R2.w;
MAD R1.xyz, R0.w, R3, fragment.texcoord[4];
DP3 R0.w, R1, R1;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, R1;
DP3 R0.x, R0, R1;
MAX R1.x, R0, c[7];
MOV R0.w, c[7].x;
MAX R0.xyz, R0.w, c[1];
MAX R0.w, R0, c[5];
MAX R1.w, R1, c[7].x;
POW R1.x, R1.x, c[7].w;
MUL R2.xyz, R4, R1.w;
MUL R0.xyz, R0, c[0];
MAD R0.xyz, R0, R1.x, R2;
SLT R1.x, R0.w, c[6];
MUL result.color.xyz, R0, c[7].y;
KIL -R1.x;
MOV result.color.w, R0;
END
# 52 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
"ps_3_0
; 49 ALU, 6 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
texld r2.yw, v1.zwzw, s3
mad_pp r2.xy, r2.wyzw, c7.z, c7.w
mul_pp r0.z, r2.y, r2.y
mad_pp r0.z, -r2.x, r2.x, -r0
dp3_pp r0.x, v5, v5
rsq_pp r0.x, r0.x
mul_pp r1.xyz, r0.x, v5
dp3_pp r0.x, r1, r1
rsq_pp r0.x, r0.x
add_pp r0.z, r0, c7.y
rsq_pp r0.z, r0.z
mov_pp r1.w, c5
add_pp r1.w, -c6.x, r1
mad_pp r1.xyz, r0.x, r1, v4
texld r0.yw, v2, s4
mad_pp r0.xy, r0.wyzw, c7.z, c7.w
mul_pp r0.w, r0.y, r0.y
mad_pp r0.w, -r0.x, r0.x, -r0
add_pp r0.w, r0, c7.y
rcp_pp r2.z, r0.z
rsq_pp r0.w, r0.w
rcp_pp r0.z, r0.w
add r3.xyz, r0, -r2
dp3_pp r0.w, r1, r1
texld r0.xyz, v1, s2
mul r0.xyz, r0, c2.x
mad r2.xyz, r0, r3, r2
rsq_pp r0.w, r0.w
mul_pp r3.xyz, r0.w, r1
mul r1.xyz, r2, c4.x
dp3_pp r0.w, r1, r3
max_pp r0.w, r0, c7.x
pow r2, r0.w, c8.x
texld r3.xyz, v0, s0
texld r4.xyz, v0.zwzw, s1
add r4.xyz, r4, -r3
mad r0.xyz, r0, r4, r3
mul r2.yzw, v3.xxyz, c3.x
mul r3.xyz, r0, r2.yzww
dp3_pp r0.x, r1, v4
mul r1.xyz, r3, c5
mul_pp r1.xyz, r1, c0
max_pp r0.x, r0, c7
mul_pp r0.xyz, r1, r0.x
mov_pp r1.xyz, c0
mul_pp r1.xyz, c1, r1
mov r0.w, r2.x
mad r0.xyz, r1, r0.w, r0
cmp r1.w, r1, c7.x, c7.y
mov_pp r1, -r1.w
mul oC0.xyz, r0, c7.z
texkill r1.xyzw
mov_pp oC0.w, c5
"
}
SubProgram "opengl " {
Keywords { "SPOT" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTexture0] 2D
SetTexture 6 [_LightTextureB0] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 67 ALU, 7 TEX
PARAM c[9] = { program.local[0..6],
		{ 0, 2, 1, 128 },
		{ 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, fragment.texcoord[4];
DP3 R1.w, R1, R1;
RSQ R1.w, R1.w;
MUL R3.xyz, R1.w, R1;
MUL R1.xyz, fragment.texcoord[3], c[3].x;
MUL R5.xyz, R0, R1;
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
DP3 R0.w, fragment.texcoord[5], fragment.texcoord[5];
RSQ R0.w, R0.w;
MUL R4.xyz, R0.w, fragment.texcoord[5];
DP3 R0.w, R4, R4;
RSQ R0.w, R0.w;
MAD R4.xyz, R0.w, R4, R3;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R4, R4;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
RSQ R0.w, R2.w;
MUL R0.xyz, R0, c[4].x;
MUL R1.xyz, R0.w, R4;
DP3 R0.w, R0, R1;
DP3 R0.x, R0, R3;
MUL R1.xyz, R5, c[5];
MAX R0.w, R0, c[7].x;
MAX R1.xyz, R1, c[7].x;
MAX R1.w, R0.x, c[7].x;
MOV R2.x, c[7];
MUL R1.xyz, R1, c[0];
MUL R1.xyz, R1, R1.w;
MAX R0.xyz, R2.x, c[1];
POW R0.w, R0.w, c[7].w;
MUL R0.xyz, R0, c[0];
MAD R0.xyz, R0, R0.w, R1;
RCP R1.w, fragment.texcoord[6].w;
MAD R1.xy, fragment.texcoord[6], R1.w, c[8].x;
TEX R0.w, R1, texture[5], 2D;
SLT R1.x, c[7], fragment.texcoord[6].z;
DP3 R1.z, fragment.texcoord[6], fragment.texcoord[6];
MUL R0.w, R1.x, R0;
TEX R1.w, R1.z, texture[6], 2D;
MUL R1.x, R0.w, R1.w;
MAX R0.w, R2.x, c[5];
MUL R1.x, R1, c[7].y;
SLT R1.y, R0.w, c[6].x;
MUL result.color.xyz, R0, R1.x;
KIL -R1.y;
MOV result.color.w, R0;
END
# 67 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "SPOT" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTexture0] 2D
SetTexture 6 [_LightTextureB0] 2D
"ps_3_0
; 62 ALU, 8 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
dcl_2d s5
dcl_2d s6
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0.50000000, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6
dp3_pp r0.x, v4, v4
rsq_pp r0.y, r0.x
mul_pp r1.xyz, r0.y, v4
dp3_pp r0.w, r1, r1
rsq_pp r1.w, r0.w
mul_pp r3.xyz, r1.w, r1
texld r1.yw, v2, s4
dp3_pp r0.x, v5, v5
rsq_pp r0.x, r0.x
mul_pp r0.xyz, r0.x, v5
dp3_pp r0.w, r0, r0
rsq_pp r0.w, r0.w
mad_pp r4.xyz, r0.w, r0, r3
mad_pp r1.xy, r1.wyzw, c7.z, c7.w
dp3_pp r0.x, r4, r4
rsq_pp r0.w, r0.x
texld r5.yw, v1.zwzw, s3
mad_pp r0.xy, r5.wyzw, c7.z, c7.w
mul_pp r1.z, r0.y, r0.y
mul_pp r0.z, r1.y, r1.y
mad_pp r1.z, -r0.x, r0.x, -r1
mad_pp r0.z, -r1.x, r1.x, -r0
add_pp r1.z, r1, c7.y
texld r2.xyz, v1, s2
rsq_pp r1.w, r1.z
add_pp r0.z, r0, c7.y
rsq_pp r1.z, r0.z
rcp_pp r0.z, r1.w
rcp_pp r1.z, r1.z
add r1.xyz, r1, -r0
mul r2.xyz, r2, c2.x
mad r0.xyz, r2, r1, r0
mul r1.xyz, r0, c4.x
mul_pp r4.xyz, r0.w, r4
dp3_pp r0.x, r1, r4
max_pp r1.w, r0.x, c7.x
pow r0, r1.w, c8.x
mov r0.w, r0.x
dp3_pp r1.x, r1, r3
texld r0.xyz, v0, s0
texld r4.xyz, v0.zwzw, s1
add r4.xyz, r4, -r0
mad r0.xyz, r2, r4, r0
mul r5.xyz, v3, c3.x
mul r0.xyz, r0, r5
mul r0.xyz, r0, c5
mul_pp r0.xyz, r0, c0
max_pp r1.x, r1, c7
mul_pp r1.xyz, r0, r1.x
mov_pp r0.xyz, c0
mul_pp r2.xyz, c1, r0
rcp r1.w, v6.w
mad r0.xy, v6, r1.w, c8.y
mad r1.xyz, r2, r0.w, r1
texld r0.w, r0, s5
cmp r0.x, -v6.z, c7, c7.y
mul_pp r0.z, r0.x, r0.w
dp3 r0.x, v6, v6
texld r0.x, r0.x, s6
mul_pp r0.z, r0, r0.x
mul_pp r1.w, r0.z, c7.z
mov_pp r0.y, c5.w
add_pp r0.y, -c6.x, r0
cmp r0.x, r0.y, c7, c7.y
mov_pp r0, -r0.x
mul oC0.xyz, r1, r1.w
texkill r0.xyzw
mov_pp oC0.w, c5
"
}
SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTextureB0] 2D
SetTexture 6 [_LightTexture0] CUBE
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 63 ALU, 7 TEX
PARAM c[8] = { program.local[0..6],
		{ 0, 2, 1, 128 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
DP3 R0.w, fragment.texcoord[4], fragment.texcoord[4];
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, fragment.texcoord[4];
DP3 R1.w, R1, R1;
RSQ R1.w, R1.w;
MUL R3.xyz, R1.w, R1;
MUL R1.xyz, fragment.texcoord[3], c[3].x;
MUL R5.xyz, R0, R1;
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
DP3 R0.w, fragment.texcoord[5], fragment.texcoord[5];
RSQ R0.w, R0.w;
MUL R4.xyz, R0.w, fragment.texcoord[5];
DP3 R0.w, R4, R4;
RSQ R0.w, R0.w;
MAD R4.xyz, R0.w, R4, R3;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R4, R4;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
RSQ R0.w, R2.w;
MUL R0.xyz, R0, c[4].x;
MUL R1.xyz, R0.w, R4;
DP3 R0.w, R0, R1;
DP3 R0.x, R0, R3;
MUL R1.xyz, R5, c[5];
MAX R0.w, R0, c[7].x;
MAX R1.xyz, R1, c[7].x;
MAX R1.w, R0.x, c[7].x;
MOV R2.x, c[7];
MUL R1.xyz, R1, c[0];
MAX R0.xyz, R2.x, c[1];
MUL R1.xyz, R1, R1.w;
POW R0.w, R0.w, c[7].w;
MUL R0.xyz, R0, c[0];
MAD R0.xyz, R0, R0.w, R1;
DP3 R1.x, fragment.texcoord[6], fragment.texcoord[6];
TEX R0.w, fragment.texcoord[6], texture[6], CUBE;
TEX R1.w, R1.x, texture[5], 2D;
MUL R1.x, R1.w, R0.w;
MAX R0.w, R2.x, c[5];
MUL R1.x, R1, c[7].y;
SLT R1.y, R0.w, c[6].x;
MUL result.color.xyz, R0, R1.x;
KIL -R1.y;
MOV result.color.w, R0;
END
# 63 instructions, 6 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTextureB0] 2D
SetTexture 6 [_LightTexture0] CUBE
"ps_3_0
; 58 ALU, 8 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
dcl_2d s5
dcl_cube s6
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6.xyz
dp3_pp r0.x, v4, v4
rsq_pp r0.y, r0.x
mul_pp r1.xyz, r0.y, v4
dp3_pp r0.w, r1, r1
rsq_pp r1.w, r0.w
mul_pp r3.xyz, r1.w, r1
dp3_pp r0.x, v5, v5
rsq_pp r0.x, r0.x
mul_pp r0.xyz, r0.x, v5
dp3_pp r0.w, r0, r0
rsq_pp r0.w, r0.w
mad_pp r4.xyz, r0.w, r0, r3
dp3_pp r0.x, r4, r4
rsq_pp r0.w, r0.x
texld r5.yw, v1.zwzw, s3
mad_pp r0.xy, r5.wyzw, c7.z, c7.w
mul_pp r1.w, r0.y, r0.y
mad_pp r1.w, -r0.x, r0.x, -r1
add_pp r1.w, r1, c7.y
texld r1.xyz, v1, s2
texld r2.yw, v2, s4
mad_pp r2.xy, r2.wyzw, c7.z, c7.w
mul_pp r0.z, r2.y, r2.y
mad_pp r0.z, -r2.x, r2.x, -r0
rsq_pp r2.z, r1.w
add_pp r0.z, r0, c7.y
rsq_pp r1.w, r0.z
rcp_pp r0.z, r2.z
rcp_pp r2.z, r1.w
add r2.xyz, r2, -r0
mul r1.xyz, r1, c2.x
mad r0.xyz, r1, r2, r0
mul r2.xyz, r0, c4.x
mul_pp r4.xyz, r0.w, r4
dp3_pp r0.x, r2, r4
max_pp r1.w, r0.x, c7.x
pow r0, r1.w, c8.x
texld r4.xyz, v0, s0
texld r5.xyz, v0.zwzw, s1
add r0.yzw, r5.xxyz, -r4.xxyz
mad r1.xyz, r1, r0.yzww, r4
mul r5.xyz, v3, c3.x
mul r1.xyz, r1, r5
mov r0.w, r0.x
mul r0.xyz, r1, c5
dp3_pp r1.x, r2, r3
max_pp r1.w, r1.x, c7.x
mul_pp r0.xyz, r0, c0
mul_pp r0.xyz, r0, r1.w
mov_pp r1.xyz, c0
mul_pp r1.xyz, c1, r1
mad r1.xyz, r1, r0.w, r0
mov_pp r1.w, c5
dp3 r0.x, v6, v6
add_pp r0.y, -c6.x, r1.w
texld r0.w, v6, s6
texld r0.x, r0.x, s5
mul r0.z, r0.x, r0.w
mul_pp r1.w, r0.z, c7.z
cmp r0.x, r0.y, c7, c7.y
mov_pp r0, -r0.x
mul oC0.xyz, r1, r1.w
texkill r0.xyzw
mov_pp oC0.w, c5
"
}
SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTexture0] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 54 ALU, 6 TEX
PARAM c[8] = { program.local[0..6],
		{ 0, 2, 1, 128 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEX R2.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R2.xyz, R2, c[2].x;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R1.xyz, R1, -R0;
MAD R0.xyz, R2, R1, R0;
MUL R3.xyz, fragment.texcoord[3], c[3].x;
MUL R1.xyz, R0, R3;
DP3 R0.x, fragment.texcoord[5], fragment.texcoord[5];
RSQ R0.w, R0.x;
MUL R1.xyz, R1, c[5];
MAX R0.xyz, R1, c[7].x;
MUL R3.xyz, R0.w, fragment.texcoord[5];
MUL R4.xyz, R0, c[0];
TEX R1.yw, fragment.texcoord[1].zwzw, texture[3], 2D;
TEX R0.yw, fragment.texcoord[2], texture[4], 2D;
MAD R0.xy, R0.wyzw, c[7].y, -c[7].z;
MAD R1.xy, R1.wyzw, c[7].y, -c[7].z;
MUL R0.w, R1.y, R1.y;
MUL R0.z, R0.y, R0.y;
MAD R0.w, -R1.x, R1.x, -R0;
MAD R0.z, -R0.x, R0.x, -R0;
ADD R0.w, R0, c[7].z;
RSQ R0.w, R0.w;
ADD R0.z, R0, c[7];
RSQ R0.z, R0.z;
RCP R1.z, R0.w;
DP3 R2.w, R3, R3;
RCP R0.z, R0.z;
ADD R0.xyz, R0, -R1;
MAD R0.xyz, R2, R0, R1;
MUL R0.xyz, R0, c[4].x;
DP3 R1.w, R0, fragment.texcoord[4];
RSQ R0.w, R2.w;
MAD R1.xyz, R0.w, R3, fragment.texcoord[4];
DP3 R0.w, R1, R1;
RSQ R0.w, R0.w;
MUL R1.xyz, R0.w, R1;
DP3 R0.x, R0, R1;
MAX R1.w, R1, c[7].x;
MUL R2.xyz, R4, R1.w;
MAX R1.x, R0, c[7];
MOV R0.w, c[7].x;
MAX R0.xyz, R0.w, c[1];
MUL R0.xyz, R0, c[0];
POW R1.x, R1.x, c[7].w;
MAD R1.xyz, R0, R1.x, R2;
MAX R0.x, R0.w, c[5].w;
TEX R1.w, fragment.texcoord[6], texture[5], 2D;
MUL R0.y, R1.w, c[7];
SLT R0.z, R0.x, c[6].x;
MUL result.color.xyz, R1, R0.y;
KIL -R0.z;
MOV result.color.w, R0.x;
END
# 54 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_SpecColor]
Float 2 [_BlendValue]
Float 3 [_VertexColorValue]
Float 4 [_value]
Vector 5 [_Color]
Float 6 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
SetTexture 5 [_LightTexture0] 2D
"ps_3_0
; 50 ALU, 7 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s3
dcl_2d s4
dcl_2d s5
def c7, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c8, 128.00000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1
dcl_texcoord2 v2.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6.xy
dp3_pp r0.x, v5, v5
rsq_pp r0.x, r0.x
mul_pp r1.xyz, r0.x, v5
dp3_pp r0.x, r1, r1
rsq_pp r0.x, r0.x
mad_pp r2.xyz, r0.x, r1, v4
texld r1.yw, v1.zwzw, s3
mad_pp r1.xy, r1.wyzw, c7.z, c7.w
texld r0.yw, v2, s4
mad_pp r0.xy, r0.wyzw, c7.z, c7.w
mul_pp r0.z, r1.y, r1.y
mul_pp r0.w, r0.y, r0.y
mad_pp r0.z, -r1.x, r1.x, -r0
mad_pp r0.w, -r0.x, r0.x, -r0
add_pp r0.z, r0, c7.y
rsq_pp r0.z, r0.z
add_pp r0.w, r0, c7.y
rcp_pp r1.z, r0.z
rsq_pp r0.w, r0.w
rcp_pp r0.z, r0.w
add r3.xyz, r0, -r1
dp3_pp r0.w, r2, r2
rsq_pp r0.w, r0.w
texld r0.xyz, v1, s2
mul r0.xyz, r0, c2.x
mad r1.xyz, r0, r3, r1
mul_pp r2.xyz, r0.w, r2
mul r1.xyz, r1, c4.x
dp3_pp r0.w, r1, r2
max_pp r0.w, r0, c7.x
pow r2, r0.w, c8.x
texld r3.xyz, v0, s0
texld r4.xyz, v0.zwzw, s1
add r2.yzw, r4.xxyz, -r3.xxyz
mad r0.xyz, r0, r2.yzww, r3
mul r4.xyz, v3, c3.x
mul r3.xyz, r0, r4
dp3_pp r0.x, r1, v4
mul r1.xyz, r3, c5
mul_pp r1.xyz, r1, c0
max_pp r0.x, r0, c7
mul_pp r0.xyz, r1, r0.x
mov_pp r1.xyz, c0
mul_pp r1.xyz, c1, r1
mov r0.w, r2.x
mad r0.xyz, r1, r0.w, r0
mov_pp r1.w, c5
add_pp r1.x, -c6, r1.w
texld r0.w, v6, s5
cmp r1.x, r1, c7, c7.y
mul_pp r0.w, r0, c7.z
mov_pp r1, -r1.x
mul oC0.xyz, r0, r0.w
texkill r1.xyzw
mov_pp oC0.w, c5
"
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassBase" "RenderType"="Opaque" }
  Lighting On
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Matrix 5 [_Object2World]
Vector 9 [unity_Scale]
Vector 10 [_Blend_ST]
Vector 11 [_Bump1_ST]
Vector 12 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 24 ALU
PARAM c[13] = { program.local[0],
		state.matrix.mvp,
		program.local[5..12] };
TEMP R0;
TEMP R1;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MUL R1.xyz, R0, vertex.attrib[14].w;
DP3 R0.y, R1, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[3].xyz, R0, c[9].w;
DP3 R0.y, R1, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[4].xyz, R0, c[9].w;
DP3 R0.y, R1, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[5].xyz, R0, c[9].w;
MOV result.texcoord[2], vertex.color;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[11].xyxy, c[11];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[10], c[10].zwzw;
MAD result.texcoord[1].xy, vertex.texcoord[0], c[12], c[12].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 24 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Vector 8 [unity_Scale]
Vector 9 [_Blend_ST]
Vector 10 [_Bump1_ST]
Vector 11 [_Bump2_ST]
"vs_3_0
; 25 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord0 v3
dcl_color0 v4
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r1.xyz, r0, v1.w
dp3 r0.y, r1, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o4.xyz, r0, c8.w
dp3 r0.y, r1, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o5.xyz, r0, c8.w
dp3 r0.y, r1, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o6.xyz, r0, c8.w
mov o3, v4
mad o1.zw, v3.xyxy, c10.xyxy, c10
mad o1.xy, v3, c9, c9.zwzw
mad o2.xy, v3, c11, c11.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Float 0 [_BlendValue]
Float 1 [_value]
Vector 2 [_Color]
Float 3 [_Cutoff]
SetTexture 2 [_Blend] 2D
SetTexture 3 [_Bump1] 2D
SetTexture 4 [_Bump2] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 28 ALU, 3 TEX
PARAM c[5] = { program.local[0..3],
		{ 1, 2, 0.5, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R0.xyz, fragment.texcoord[0], texture[2], 2D;
TEX R1.yw, fragment.texcoord[0].zwzw, texture[3], 2D;
MAD R1.xy, R1.wyzw, c[4].y, -c[4].x;
MUL R1.z, R1.y, R1.y;
MAD R1.z, -R1.x, R1.x, -R1;
TEX R2.yw, fragment.texcoord[1], texture[4], 2D;
MAD R2.xy, R2.wyzw, c[4].y, -c[4].x;
MUL R0.w, R2.y, R2.y;
MAD R0.w, -R2.x, R2.x, -R0;
ADD R1.z, R1, c[4].x;
ADD R0.w, R0, c[4].x;
RSQ R0.w, R0.w;
RCP R2.z, R0.w;
RSQ R1.z, R1.z;
RCP R1.z, R1.z;
MOV R0.w, c[4];
ADD R2.xyz, R2, -R1;
MUL R0.xyz, R0, c[0].x;
MAD R0.xyz, R0, R2, R1;
MUL R0.xyz, R0, c[1].x;
DP3 R1.z, fragment.texcoord[5], R0;
DP3 R1.y, R0, fragment.texcoord[4];
DP3 R1.x, R0, fragment.texcoord[3];
MAX R0.w, R0, c[2];
SLT R0.x, R0.w, c[3];
MAD result.color.xyz, R1, c[4].z, c[4].z;
KIL -R0.x;
MOV result.color.w, c[4].x;
END
# 28 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_BlendValue]
Float 1 [_value]
Vector 2 [_Color]
Float 3 [_Cutoff]
SetTexture 0 [_Blend] 2D
SetTexture 1 [_Bump1] 2D
SetTexture 2 [_Bump2] 2D
"ps_3_0
; 25 ALU, 4 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c4, 0.00000000, 1.00000000, 2.00000000, -1.00000000
def c5, 0.50000000, 0, 0, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
texld r1.yw, v0.zwzw, s1
mad_pp r1.xy, r1.wyzw, c4.z, c4.w
mul_pp r1.z, r1.y, r1.y
mad_pp r1.z, -r1.x, r1.x, -r1
texld r0.xyz, v0, s0
texld r2.yw, v1, s2
mad_pp r2.xy, r2.wyzw, c4.z, c4.w
mul_pp r0.w, r2.y, r2.y
mad_pp r0.w, -r2.x, r2.x, -r0
add_pp r1.z, r1, c4.y
add_pp r0.w, r0, c4.y
rsq_pp r0.w, r0.w
rsq_pp r1.z, r1.z
rcp_pp r1.z, r1.z
rcp_pp r2.z, r0.w
add r2.xyz, r2, -r1
mul r0.xyz, r0, c0.x
mad r0.xyz, r0, r2, r1
mul r1.xyz, r0, c1.x
mov_pp r0.x, c2.w
add_pp r0.x, -c3, r0
cmp r0.w, r0.x, c4.x, c4.y
dp3 r0.z, v5, r1
dp3 r0.y, r1, v4
dp3 r0.x, r1, v3
mov_pp r1, -r0.w
mad_pp oC0.xyz, r0, c5.x, c5.x
texkill r1.xyzw
mov_pp oC0.w, c4.y
"
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "RenderType"="Opaque" }
  Lighting On
  ZWrite Off
Program "vp" {
SubProgram "opengl " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 5 [_Object2World]
Vector 9 [_ProjectionParams]
Vector 10 [unity_Scale]
Vector 11 [unity_SHAr]
Vector 12 [unity_SHAg]
Vector 13 [unity_SHAb]
Vector 14 [unity_SHBr]
Vector 15 [unity_SHBg]
Vector 16 [unity_SHBb]
Vector 17 [unity_SHC]
Vector 18 [_Diffuse1_ST]
Vector 19 [_Diffuse2_ST]
Vector 20 [_Blend_ST]
Vector 21 [_Bump1_ST]
Vector 22 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 33 ALU
PARAM c[23] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..22] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[10].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].y;
DP4 R2.z, R0, c[13];
DP4 R2.y, R0, c[12];
DP4 R2.x, R0, c[11];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[16];
DP4 R3.y, R1, c[15];
DP4 R3.x, R1, c[14];
DP4 R1.w, vertex.position, c[4];
DP4 R1.z, vertex.position, c[3];
MAD R0.x, R0, R0, -R0.y;
ADD R3.xyz, R2, R3;
MUL R2.xyz, R0.x, c[17];
DP4 R1.x, vertex.position, c[1];
DP4 R1.y, vertex.position, c[2];
MUL R0.xyz, R1.xyww, c[0].x;
MUL R0.y, R0, c[9].x;
ADD result.texcoord[5].xyz, R3, R2;
ADD result.texcoord[4].xy, R0, R0.z;
MOV result.position, R1;
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[4].zw, R1;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[19].xyxy, c[19];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[18], c[18].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[22], c[22].zwzw;
END
# 33 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Vector 8 [_ProjectionParams]
Vector 9 [_ScreenParams]
Vector 10 [unity_Scale]
Vector 11 [unity_SHAr]
Vector 12 [unity_SHAg]
Vector 13 [unity_SHAb]
Vector 14 [unity_SHBr]
Vector 15 [unity_SHBg]
Vector 16 [unity_SHBb]
Vector 17 [unity_SHC]
Vector 18 [_Diffuse1_ST]
Vector 19 [_Diffuse2_ST]
Vector 20 [_Blend_ST]
Vector 21 [_Bump1_ST]
Vector 22 [_Bump2_ST]
"vs_3_0
; 33 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c23, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_normal0 v1
dcl_texcoord0 v2
dcl_color0 v3
mul r1.xyz, v1, c10.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c23.y
dp4 r2.z, r0, c13
dp4 r2.y, r0, c12
dp4 r2.x, r0, c11
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c16
dp4 r3.y, r1, c15
dp4 r3.x, r1, c14
dp4 r1.w, v0, c3
dp4 r1.z, v0, c2
mad r0.x, r0, r0, -r0.y
add r3.xyz, r2, r3
mul r2.xyz, r0.x, c17
dp4 r1.x, v0, c0
dp4 r1.y, v0, c1
mul r0.xyz, r1.xyww, c23.x
mul r0.y, r0, c8.x
add o6.xyz, r3, r2
mad o5.xy, r0.z, c9.zwzw, r0
mov o0, r1
mov o4, v3
mov o5.zw, r1
mad o1.zw, v2.xyxy, c19.xyxy, c19
mad o1.xy, v2, c18, c18.zwzw
mad o2.zw, v2.xyxy, c21.xyxy, c21
mad o2.xy, v2, c20, c20.zwzw
mad o3.xy, v2, c22, c22.zwzw
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 9 [_Object2World]
Vector 13 [_ProjectionParams]
Vector 14 [unity_LightmapST]
Vector 15 [unity_ShadowFadeCenterAndType]
Vector 16 [_Diffuse1_ST]
Vector 17 [_Diffuse2_ST]
Vector 18 [_Blend_ST]
Vector 19 [_Bump1_ST]
Vector 20 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 25 ALU
PARAM c[21] = { { 0.5, 1 },
		state.matrix.modelview[0],
		state.matrix.mvp,
		program.local[9..20] };
TEMP R0;
TEMP R1;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
MUL R1.xyz, R0.xyww, c[0].x;
MUL R1.y, R1, c[13].x;
ADD result.texcoord[4].xy, R1, R1.z;
MOV result.position, R0;
MOV R0.x, c[0].y;
ADD R0.y, R0.x, -c[15].w;
DP4 R0.x, vertex.position, c[3];
DP4 R1.z, vertex.position, c[11];
DP4 R1.x, vertex.position, c[9];
DP4 R1.y, vertex.position, c[10];
ADD R1.xyz, R1, -c[15];
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[4].zw, R0;
MUL result.texcoord[6].xyz, R1, c[15].w;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[17].xyxy, c[17];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[16], c[16].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[19].xyxy, c[19];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[18], c[18].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[5].xy, vertex.texcoord[1], c[14], c[14].zwzw;
MUL result.texcoord[6].w, -R0.x, R0.y;
END
# 25 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_modelview0]
Matrix 4 [glstate_matrix_mvp]
Matrix 8 [_Object2World]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_LightmapST]
Vector 15 [unity_ShadowFadeCenterAndType]
Vector 16 [_Diffuse1_ST]
Vector 17 [_Diffuse2_ST]
Vector 18 [_Blend_ST]
Vector 19 [_Bump1_ST]
Vector 20 [_Bump2_ST]
"vs_3_0
; 25 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c21, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
dcl_texcoord1 v2
dcl_color0 v3
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
mul r1.xyz, r0.xyww, c21.x
mul r1.y, r1, c12.x
mad o5.xy, r1.z, c13.zwzw, r1
mov o0, r0
mov r0.x, c15.w
add r0.y, c21, -r0.x
dp4 r0.x, v0, c2
dp4 r1.z, v0, c10
dp4 r1.x, v0, c8
dp4 r1.y, v0, c9
add r1.xyz, r1, -c15
mov o4, v3
mov o5.zw, r0
mul o7.xyz, r1, c15.w
mad o1.zw, v1.xyxy, c17.xyxy, c17
mad o1.xy, v1, c16, c16.zwzw
mad o2.zw, v1.xyxy, c19.xyxy, c19
mad o2.xy, v1, c18, c18.zwzw
mad o3.xy, v1, c20, c20.zwzw
mad o6.xy, v2, c14, c14.zwzw
mul o7.w, -r0.x, r0.y
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 5 [_Object2World]
Vector 9 [_ProjectionParams]
Vector 10 [unity_Scale]
Vector 11 [unity_SHAr]
Vector 12 [unity_SHAg]
Vector 13 [unity_SHAb]
Vector 14 [unity_SHBr]
Vector 15 [unity_SHBg]
Vector 16 [unity_SHBb]
Vector 17 [unity_SHC]
Vector 18 [_Diffuse1_ST]
Vector 19 [_Diffuse2_ST]
Vector 20 [_Blend_ST]
Vector 21 [_Bump1_ST]
Vector 22 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 33 ALU
PARAM c[23] = { { 0.5, 1 },
		state.matrix.mvp,
		program.local[5..22] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[10].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].y;
DP4 R2.z, R0, c[13];
DP4 R2.y, R0, c[12];
DP4 R2.x, R0, c[11];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[16];
DP4 R3.y, R1, c[15];
DP4 R3.x, R1, c[14];
DP4 R1.w, vertex.position, c[4];
DP4 R1.z, vertex.position, c[3];
MAD R0.x, R0, R0, -R0.y;
ADD R3.xyz, R2, R3;
MUL R2.xyz, R0.x, c[17];
DP4 R1.x, vertex.position, c[1];
DP4 R1.y, vertex.position, c[2];
MUL R0.xyz, R1.xyww, c[0].x;
MUL R0.y, R0, c[9].x;
ADD result.texcoord[5].xyz, R3, R2;
ADD result.texcoord[4].xy, R0, R0.z;
MOV result.position, R1;
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[4].zw, R1;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[19].xyxy, c[19];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[18], c[18].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[21].xyxy, c[21];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[22], c[22].zwzw;
END
# 33 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Vector 8 [_ProjectionParams]
Vector 9 [_ScreenParams]
Vector 10 [unity_Scale]
Vector 11 [unity_SHAr]
Vector 12 [unity_SHAg]
Vector 13 [unity_SHAb]
Vector 14 [unity_SHBr]
Vector 15 [unity_SHBg]
Vector 16 [unity_SHBb]
Vector 17 [unity_SHC]
Vector 18 [_Diffuse1_ST]
Vector 19 [_Diffuse2_ST]
Vector 20 [_Blend_ST]
Vector 21 [_Bump1_ST]
Vector 22 [_Bump2_ST]
"vs_3_0
; 33 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c23, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_normal0 v1
dcl_texcoord0 v2
dcl_color0 v3
mul r1.xyz, v1, c10.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c23.y
dp4 r2.z, r0, c13
dp4 r2.y, r0, c12
dp4 r2.x, r0, c11
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c16
dp4 r3.y, r1, c15
dp4 r3.x, r1, c14
dp4 r1.w, v0, c3
dp4 r1.z, v0, c2
mad r0.x, r0, r0, -r0.y
add r3.xyz, r2, r3
mul r2.xyz, r0.x, c17
dp4 r1.x, v0, c0
dp4 r1.y, v0, c1
mul r0.xyz, r1.xyww, c23.x
mul r0.y, r0, c8.x
add o6.xyz, r3, r2
mad o5.xy, r0.z, c9.zwzw, r0
mov o0, r1
mov o4, v3
mov o5.zw, r1
mad o1.zw, v2.xyxy, c19.xyxy, c19
mad o1.xy, v2, c18, c18.zwzw
mad o2.zw, v2.xyxy, c21.xyxy, c21
mad o2.xy, v2, c20, c20.zwzw
mad o3.xy, v2, c22, c22.zwzw
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 9 [_Object2World]
Vector 13 [_ProjectionParams]
Vector 14 [unity_LightmapST]
Vector 15 [unity_ShadowFadeCenterAndType]
Vector 16 [_Diffuse1_ST]
Vector 17 [_Diffuse2_ST]
Vector 18 [_Blend_ST]
Vector 19 [_Bump1_ST]
Vector 20 [_Bump2_ST]
"3.0-!!ARBvp1.0
# 25 ALU
PARAM c[21] = { { 0.5, 1 },
		state.matrix.modelview[0],
		state.matrix.mvp,
		program.local[9..20] };
TEMP R0;
TEMP R1;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
MUL R1.xyz, R0.xyww, c[0].x;
MUL R1.y, R1, c[13].x;
ADD result.texcoord[4].xy, R1, R1.z;
MOV result.position, R0;
MOV R0.x, c[0].y;
ADD R0.y, R0.x, -c[15].w;
DP4 R0.x, vertex.position, c[3];
DP4 R1.z, vertex.position, c[11];
DP4 R1.x, vertex.position, c[9];
DP4 R1.y, vertex.position, c[10];
ADD R1.xyz, R1, -c[15];
MOV result.texcoord[3], vertex.color;
MOV result.texcoord[4].zw, R0;
MUL result.texcoord[6].xyz, R1, c[15].w;
MAD result.texcoord[0].zw, vertex.texcoord[0].xyxy, c[17].xyxy, c[17];
MAD result.texcoord[0].xy, vertex.texcoord[0], c[16], c[16].zwzw;
MAD result.texcoord[1].zw, vertex.texcoord[0].xyxy, c[19].xyxy, c[19];
MAD result.texcoord[1].xy, vertex.texcoord[0], c[18], c[18].zwzw;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[20], c[20].zwzw;
MAD result.texcoord[5].xy, vertex.texcoord[1], c[14], c[14].zwzw;
MUL result.texcoord[6].w, -R0.x, R0.y;
END
# 25 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_modelview0]
Matrix 4 [glstate_matrix_mvp]
Matrix 8 [_Object2World]
Vector 12 [_ProjectionParams]
Vector 13 [_ScreenParams]
Vector 14 [unity_LightmapST]
Vector 15 [unity_ShadowFadeCenterAndType]
Vector 16 [_Diffuse1_ST]
Vector 17 [_Diffuse2_ST]
Vector 18 [_Blend_ST]
Vector 19 [_Bump1_ST]
Vector 20 [_Bump2_ST]
"vs_3_0
; 25 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c21, 0.50000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
dcl_texcoord1 v2
dcl_color0 v3
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
mul r1.xyz, r0.xyww, c21.x
mul r1.y, r1, c12.x
mad o5.xy, r1.z, c13.zwzw, r1
mov o0, r0
mov r0.x, c15.w
add r0.y, c21, -r0.x
dp4 r0.x, v0, c2
dp4 r1.z, v0, c10
dp4 r1.x, v0, c8
dp4 r1.y, v0, c9
add r1.xyz, r1, -c15
mov o4, v3
mov o5.zw, r0
mul o7.xyz, r1, c15.w
mad o1.zw, v1.xyxy, c17.xyxy, c17
mad o1.xy, v1, c16, c16.zwzw
mad o2.zw, v1.xyxy, c19.xyxy, c19
mad o2.xy, v1, c18, c18.zwzw
mad o3.xy, v1, c20, c20.zwzw
mad o6.xy, v2, c14, c14.zwzw
mul o7.w, -r0.x, r0.y
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Float 4 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 26 ALU, 4 TEX
PARAM c[6] = { program.local[0..4],
		{ 0, 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R2.xyz, R1, -R0;
TEX R1.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R1.xyz, R1, c[1].x;
MAD R0.xyz, R1, R2, R0;
MUL R3.xyz, fragment.texcoord[3], c[2].x;
MUL R0.xyz, R0, R3;
MUL R2.xyz, R0, c[3];
TXP R0, fragment.texcoord[4], texture[5], 2D;
MOV R1.w, c[5].x;
MAX R1.xyz, R1.w, c[0];
LG2 R0.w, R0.w;
MUL R1.xyz, -R0.w, R1;
LG2 R0.x, R0.x;
LG2 R0.z, R0.z;
LG2 R0.y, R0.y;
ADD R0.xyz, -R0, fragment.texcoord[5];
MUL R3.xyz, R0, R1;
MAX R2.xyz, R2, c[5].x;
MAD result.color.xyz, R2, R0, R3;
MAX R0.x, R1.w, c[3].w;
DP3 R0.y, R1, c[5].yzww;
SLT R0.z, R0.x, c[4].x;
ADD result.color.w, R0.x, R0.y;
KIL -R0.z;
END
# 26 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Float 4 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
"ps_3_0
; 20 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s5
def c5, 0.00000000, 1.00000000, 0, 0
def c6, 0.21997070, 0.70703125, 0.07098389, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4
dcl_texcoord5 v5.xyz
texld r0.xyz, v0, s0
texld r1.xyz, v0.zwzw, s1
add r2.xyz, r1, -r0
texld r1.xyz, v1, s2
mul r1.xyz, r1, c1.x
mad r0.xyz, r1, r2, r0
mul r3.xyz, v3, c2.x
mul r1.xyz, r0, r3
texldp r0, v4, s5
log_pp r1.w, r0.w
mov_pp r0.w, c3
add_pp r0.w, -c4.x, r0
mul r1.xyz, r1, c3
mul_pp r2.xyz, -r1.w, c0
log_pp r0.x, r0.x
log_pp r0.z, r0.z
log_pp r0.y, r0.y
add_pp r0.xyz, -r0, v5
mul_pp r3.xyz, r0, r2
mad_pp oC0.xyz, r1, r0, r3
cmp r0.w, r0, c5.x, c5.y
mov_pp r0, -r0.w
dp3_pp r1.x, r2, c6
texkill r0.xyzw
add_pp oC0.w, r1.x, c3
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Vector 4 [unity_LightmapFade]
Float 5 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
SetTexture 6 [unity_Lightmap] 2D
SetTexture 7 [unity_LightmapInd] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 37 ALU, 6 TEX
PARAM c[8] = { program.local[0..5],
		{ 0, 8 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R2.xyz, R1, -R0;
TEX R1.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R1.xyz, R1, c[1].x;
MAD R0.xyz, R1, R2, R0;
MUL R3.xyz, fragment.texcoord[3], c[2].x;
MUL R1.xyz, R0, R3;
MUL R2.xyz, R1, c[3];
TEX R0, fragment.texcoord[5], texture[6], 2D;
MUL R1.xyz, R0.w, R0;
TEX R0, fragment.texcoord[5], texture[7], 2D;
MUL R0.xyz, R0.w, R0;
MUL R0.xyz, R0, c[6].y;
DP4 R1.w, fragment.texcoord[6], fragment.texcoord[6];
RSQ R0.w, R1.w;
MOV R1.w, c[6].x;
RCP R0.w, R0.w;
MAD R1.xyz, R1, c[6].y, -R0;
MAD_SAT R0.w, R0, c[4].z, c[4];
MAD R1.xyz, R0.w, R1, R0;
TXP R0, fragment.texcoord[4], texture[5], 2D;
MAX R3.xyz, R1.w, c[0];
LG2 R0.w, R0.w;
MUL R3.xyz, -R0.w, R3;
LG2 R0.x, R0.x;
LG2 R0.y, R0.y;
LG2 R0.z, R0.z;
ADD R0.xyz, -R0, R1;
MUL R1.xyz, R0, R3;
MAX R2.xyz, R2, c[6].x;
MAD result.color.xyz, R2, R0, R1;
MAX R0.x, R1.w, c[3].w;
DP3 R0.y, R3, c[7];
SLT R0.z, R0.x, c[5].x;
ADD result.color.w, R0.x, R0.y;
KIL -R0.z;
END
# 37 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Vector 4 [unity_LightmapFade]
Float 5 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
SetTexture 6 [unity_Lightmap] 2D
SetTexture 7 [unity_LightmapInd] 2D
"ps_3_0
; 29 ALU, 7 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s5
dcl_2d s6
dcl_2d s7
def c6, 0.00000000, 1.00000000, 8.00000000, 0
def c7, 0.21997070, 0.70703125, 0.07098389, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4
dcl_texcoord5 v5.xy
dcl_texcoord6 v6
texld r1.xyz, v0, s0
texld r0.xyz, v0.zwzw, s1
add r2.xyz, r0, -r1
texld r0.xyz, v1, s2
mul r0.xyz, r0, c1.x
mad r0.xyz, r0, r2, r1
mul r3.xyz, v3, c2.x
mul r0.xyz, r0, r3
mul r2.xyz, r0, c3
texld r0, v5, s6
mul_pp r0.xyz, r0.w, r0
texld r1, v5, s7
mul_pp r1.xyz, r1.w, r1
mul_pp r1.xyz, r1, c6.z
dp4 r0.w, v6, v6
rsq r0.w, r0.w
rcp r1.w, r0.w
mad_pp r3.xyz, r0, c6.z, -r1
texldp r0, v4, s5
mad_sat r1.w, r1, c4.z, c4
mad_pp r1.xyz, r1.w, r3, r1
log_pp r0.x, r0.x
log_pp r0.y, r0.y
log_pp r0.z, r0.z
add_pp r0.xyz, -r0, r1
log_pp r1.x, r0.w
mul_pp r1.xyz, -r1.x, c0
mul_pp r3.xyz, r0, r1
mov_pp r0.w, c3
add_pp r0.w, -c5.x, r0
dp3_pp r1.x, r1, c7
cmp r0.w, r0, c6.x, c6.y
mad_pp oC0.xyz, r2, r0, r3
mov_pp r0, -r0.w
texkill r0.xyzw
add_pp oC0.w, r1.x, c3
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Float 4 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 22 ALU, 4 TEX
PARAM c[6] = { program.local[0..4],
		{ 0, 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R2.xyz, R1, -R0;
TEX R1.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R1.xyz, R1, c[1].x;
MAD R0.xyz, R1, R2, R0;
MUL R3.xyz, fragment.texcoord[3], c[2].x;
MUL R0.xyz, R0, R3;
MUL R2.xyz, R0, c[3];
MOV R1.w, c[5].x;
TXP R0, fragment.texcoord[4], texture[5], 2D;
MAX R1.xyz, R1.w, c[0];
ADD R0.xyz, R0, fragment.texcoord[5];
MUL R1.xyz, R0.w, R1;
MUL R3.xyz, R0, R1;
MAX R2.xyz, R2, c[5].x;
MAD result.color.xyz, R2, R0, R3;
MAX R0.x, R1.w, c[3].w;
DP3 R0.y, R1, c[5].yzww;
SLT R0.z, R0.x, c[4].x;
ADD result.color.w, R0.x, R0.y;
KIL -R0.z;
END
# 22 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Float 4 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
"ps_3_0
; 16 ALU, 5 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s5
def c5, 0.00000000, 1.00000000, 0, 0
def c6, 0.21997070, 0.70703125, 0.07098389, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4
dcl_texcoord5 v5.xyz
texld r0.xyz, v0, s0
texld r1.xyz, v0.zwzw, s1
add r2.xyz, r1, -r0
texld r1.xyz, v1, s2
mul r1.xyz, r1, c1.x
mad r0.xyz, r1, r2, r0
mul r3.xyz, v3, c2.x
mul r1.xyz, r0, r3
texldp r0, v4, s5
add_pp r0.xyz, r0, v5
mul_pp r2.xyz, r0.w, c0
mov_pp r1.w, c3
add_pp r0.w, -c4.x, r1
mul r1.xyz, r1, c3
mul_pp r3.xyz, r0, r2
mad_pp oC0.xyz, r1, r0, r3
cmp r0.w, r0, c5.x, c5.y
mov_pp r0, -r0.w
dp3_pp r1.x, r2, c6
texkill r0.xyzw
add_pp oC0.w, r1.x, c3
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Vector 4 [unity_LightmapFade]
Float 5 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
SetTexture 6 [unity_Lightmap] 2D
SetTexture 7 [unity_LightmapInd] 2D
"3.0-!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 33 ALU, 6 TEX
PARAM c[8] = { program.local[0..5],
		{ 0, 8 },
		{ 0.2199707, 0.70703125, 0.070983887 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEX R0.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R1.xyz, fragment.texcoord[0].zwzw, texture[1], 2D;
ADD R2.xyz, R1, -R0;
TEX R1.xyz, fragment.texcoord[1], texture[2], 2D;
MUL R1.xyz, R1, c[1].x;
MAD R0.xyz, R1, R2, R0;
MUL R3.xyz, fragment.texcoord[3], c[2].x;
MUL R0.xyz, R0, R3;
MUL R2.xyz, R0, c[3];
TEX R1, fragment.texcoord[5], texture[6], 2D;
TEX R0, fragment.texcoord[5], texture[7], 2D;
MUL R0.xyz, R0.w, R0;
MUL R1.xyz, R1.w, R1;
MUL R0.xyz, R0, c[6].y;
MOV R1.w, c[6].x;
DP4 R0.w, fragment.texcoord[6], fragment.texcoord[6];
RSQ R0.w, R0.w;
RCP R0.w, R0.w;
MAD R1.xyz, R1, c[6].y, -R0;
MAD_SAT R0.w, R0, c[4].z, c[4];
MAD R1.xyz, R0.w, R1, R0;
TXP R0, fragment.texcoord[4], texture[5], 2D;
MAX R3.xyz, R1.w, c[0];
ADD R0.xyz, R0, R1;
MUL R3.xyz, R0.w, R3;
MUL R1.xyz, R0, R3;
MAX R2.xyz, R2, c[6].x;
MAD result.color.xyz, R2, R0, R1;
MAX R0.x, R1.w, c[3].w;
DP3 R0.y, R3, c[7];
SLT R0.z, R0.x, c[5].x;
ADD result.color.w, R0.x, R0.y;
KIL -R0.z;
END
# 33 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
Vector 0 [_SpecColor]
Float 1 [_BlendValue]
Float 2 [_VertexColorValue]
Vector 3 [_Color]
Vector 4 [unity_LightmapFade]
Float 5 [_Cutoff]
SetTexture 0 [_Diffuse1] 2D
SetTexture 1 [_Diffuse2] 2D
SetTexture 2 [_Blend] 2D
SetTexture 5 [_LightBuffer] 2D
SetTexture 6 [unity_Lightmap] 2D
SetTexture 7 [unity_LightmapInd] 2D
"ps_3_0
; 25 ALU, 7 TEX
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl_2d s5
dcl_2d s6
dcl_2d s7
def c6, 0.00000000, 1.00000000, 8.00000000, 0
def c7, 0.21997070, 0.70703125, 0.07098389, 0
dcl_texcoord0 v0
dcl_texcoord1 v1.xy
dcl_texcoord3 v3.xyz
dcl_texcoord4 v4
dcl_texcoord5 v5.xy
dcl_texcoord6 v6
texld r1.xyz, v0, s0
texld r0.xyz, v0.zwzw, s1
add r2.xyz, r0, -r1
texld r0.xyz, v1, s2
mul r0.xyz, r0, c1.x
mad r0.xyz, r0, r2, r1
mul r3.xyz, v3, c2.x
mul r1.xyz, r0, r3
texld r0, v5, s6
mul_pp r2.xyz, r0.w, r0
texld r0, v5, s7
mul_pp r0.xyz, r0.w, r0
mul_pp r0.xyz, r0, c6.z
dp4 r1.w, v6, v6
rsq r0.w, r1.w
rcp r0.w, r0.w
mad_pp r2.xyz, r2, c6.z, -r0
mad_sat r0.w, r0, c4.z, c4
mad_pp r2.xyz, r0.w, r2, r0
texldp r0, v4, s5
add_pp r0.xyz, r0, r2
mul_pp r2.xyz, r0.w, c0
mov_pp r1.w, c3
add_pp r0.w, -c5.x, r1
mul r1.xyz, r1, c3
mul_pp r3.xyz, r0, r2
mad_pp oC0.xyz, r1, r0, r3
cmp r0.w, r0, c6.x, c6.y
mov_pp r0, -r0.w
dp3_pp r1.x, r2, c7
texkill r0.xyzw
add_pp oC0.w, r1.x, c3
"
}
}
 }
}
Fallback "Transparent/Cutout/VertexLit"
}