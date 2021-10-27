Shader "MADFINGER/Environment/Scroll 2 Layers Multiplicative No Lightmap Sine)" {
Properties {
 _MainTex ("Base layer (RGB)", 2D) = "white" {}
 _DetailTex ("2nd layer (RGB)", 2D) = "white" {}
 _ScrollX ("Base layer Scroll speed X", Float) = 1
 _ScrollY ("Base layer Scroll speed Y", Float) = 0
 _Scroll2X ("2nd layer Scroll speed X", Float) = 1
 _Scroll2Y ("2nd layer Scroll speed Y", Float) = 0
 _SineAmplX ("Base layer sine amplitude X", Float) = 0.5
 _SineAmplY ("Base layer sine amplitude Y", Float) = 0.5
 _SineFreqX ("Base layer sine freq X", Float) = 10
 _SineFreqY ("Base layer sine freq Y", Float) = 10
 _SineAmplX2 ("2nd layer sine amplitude X", Float) = 0.5
 _SineAmplY2 ("2nd layer sine amplitude Y", Float) = 0.5
 _SineFreqX2 ("2nd layer sine freq X", Float) = 10
 _SineFreqY2 ("2nd layer sine freq Y", Float) = 10
 _MMultiplier ("Layer Multiplier", Float) = 2
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "opengl " {
Keywords { "LIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_Time]
Vector 6 [_MainTex_ST]
Vector 7 [_DetailTex_ST]
Float 8 [_ScrollX]
Float 9 [_ScrollY]
Float 10 [_Scroll2X]
Float 11 [_Scroll2Y]
Float 12 [_MMultiplier]
Float 13 [_SineAmplX]
Float 14 [_SineAmplY]
Float 15 [_SineFreqX]
Float 16 [_SineFreqY]
Float 17 [_SineAmplX2]
Float 18 [_SineAmplY2]
Float 19 [_SineFreqX2]
Float 20 [_SineFreqY2]
"!!ARBvp1.0
# 89 ALU
PARAM c[25] = { { 24.980801, -24.980801, 0.15915491, 0.25 },
		state.matrix.mvp,
		program.local[5..20],
		{ 0, 0.5, 1, -1 },
		{ -60.145809, 60.145809, 85.453789, -85.453789 },
		{ -64.939346, 64.939346, 19.73921, -19.73921 },
		{ -9, 0.75 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.x, c[20];
MUL R0.x, R0, c[5];
MAD R0.x, R0, c[0].z, -c[0].w;
FRC R1.w, R0.x;
ADD R1.xyz, -R1.w, c[21];
MOV R0.x, c[19];
MUL R0.w, R0.x, c[5].x;
MUL R2.xyz, R1, R1;
MUL R0.xyz, R2, c[0].xyxw;
MAD R0.w, R0, c[0].z, -c[0];
FRC R0.w, R0;
ADD R1.xyz, -R0.w, c[21];
ADD R0.xyz, R0, c[22].xyxw;
MAD R0.xyz, R0, R2, c[22].zwzw;
MAD R0.xyz, R0, R2, c[23].xyxw;
MAD R3.xyz, R0, R2, c[23].zwzw;
MUL R1.xyz, R1, R1;
MAD R3.xyz, R3, R2, c[21].wzww;
MUL R0.xyz, R1, c[0].xyxw;
ADD R2.xyz, R0, c[22].xyxw;
SLT R4.x, R1.w, c[0].w;
SGE R4.yz, R1.w, c[24].xxyw;
MOV R0.xz, R4;
DP3 R0.y, R4, c[21].wzww;
DP3 R1.w, R3, -R0;
MAD R0.xyz, R2, R1, c[22].zwzw;
MAD R0.xyz, R0, R1, c[23].xyxw;
MAD R0.xyz, R0, R1, c[23].zwzw;
MAD R1.xyz, R0, R1, c[21].wzww;
SGE R0.yz, R0.w, c[24].xxyw;
SLT R0.x, R0.w, c[0].w;
DP3 R3.y, R0, c[21].wzww;
MOV R3.xz, R0;
MOV R0.x, c[15];
MUL R0.x, R0, c[5];
DP3 R0.y, R1, -R3;
MAD R2.zw, vertex.texcoord[0].xyxy, c[7].xyxy, c[7];
MAD R0.x, R0, c[0].z, -c[0].w;
MOV R2.y, c[11].x;
MOV R2.x, c[10];
MUL R2.xy, R2, c[5];
FRC R2.xy, R2;
ADD R2.xy, R2.zwzw, R2;
MAD result.texcoord[0].w, R1, c[18].x, R2.y;
FRC R1.w, R0.x;
MAD result.texcoord[0].z, R0.y, c[17].x, R2.x;
MOV R0.y, c[16].x;
MUL R0.y, R0, c[5].x;
MAD R0.w, R0.y, c[0].z, -c[0];
FRC R0.w, R0;
ADD R0.xyz, -R1.w, c[21];
MUL R0.xyz, R0, R0;
MUL R1.xyz, R0, c[0].xyxw;
ADD R2.xyz, -R0.w, c[21];
MUL R2.xyz, R2, R2;
MUL R3.xyz, R2, c[0].xyxw;
ADD R1.xyz, R1, c[22].xyxw;
MAD R1.xyz, R1, R0, c[22].zwzw;
MAD R1.xyz, R1, R0, c[23].xyxw;
MAD R1.xyz, R1, R0, c[23].zwzw;
ADD R3.xyz, R3, c[22].xyxw;
MAD R3.xyz, R3, R2, c[22].zwzw;
MAD R3.xyz, R3, R2, c[23].xyxw;
MAD R1.xyz, R1, R0, c[21].wzww;
SLT R4.x, R1.w, c[0].w;
SGE R4.yz, R1.w, c[24].xxyw;
MOV R0.xz, R4;
DP3 R0.y, R4, c[21].wzww;
DP3 R1.w, R1, -R0;
MAD R1.xyz, R3, R2, c[23].zwzw;
MAD R1.xyz, R1, R2, c[21].wzww;
MAD R2.xy, vertex.texcoord[0], c[6], c[6].zwzw;
MOV R0.y, c[9].x;
MOV R0.x, c[8];
MUL R0.xy, R0, c[5];
FRC R0.xy, R0;
ADD R3.xy, R2, R0;
SLT R0.x, R0.w, c[0].w;
SGE R0.yz, R0.w, c[24].xxyw;
MOV R2.xz, R0;
DP3 R2.y, R0, c[21].wzww;
DP3 R0.x, R1, -R2;
MAD result.texcoord[0].y, R0.x, c[14].x, R3;
MAD result.texcoord[0].x, R1.w, c[13], R3;
MOV result.texcoord[1], c[12].x;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 89 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_OFF" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_Time]
Vector 5 [_MainTex_ST]
Vector 6 [_DetailTex_ST]
Float 7 [_ScrollX]
Float 8 [_ScrollY]
Float 9 [_Scroll2X]
Float 10 [_Scroll2Y]
Float 11 [_MMultiplier]
Float 12 [_SineAmplX]
Float 13 [_SineAmplY]
Float 14 [_SineFreqX]
Float 15 [_SineFreqY]
Float 16 [_SineAmplX2]
Float 17 [_SineAmplY2]
Float 18 [_SineFreqX2]
Float 19 [_SineFreqY2]
"vs_2_0
; 85 ALU
def c20, -0.02083333, -0.12500000, 1.00000000, 0.50000000
def c21, -0.00000155, -0.00002170, 0.00260417, 0.00026042
def c22, 0.15915491, 0.50000000, 6.28318501, -3.14159298
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c4
mul r0.x, c19, r0
mad r0.x, r0, c22, c22.y
frc r0.x, r0
mad r1.x, r0, c22.z, c22.w
sincos r0.xy, r1.x, c21.xyzw, c20.xyzw
mov r0.x, c4
mul r0.x, c18, r0
mad r0.x, r0, c22, c22.y
frc r0.x, r0
mad r1.z, r0.x, c22, c22.w
mad r0.zw, v1.xyxy, c6.xyxy, c6
mov r1.y, c10.x
mov r1.x, c9
mul r1.xy, r1, c4
frc r1.xy, r1
add r1.xy, r0.zwzw, r1
mad oT0.w, r0.y, c17.x, r1.y
sincos r0.xy, r1.z, c21.xyzw, c20.xyzw
mov r0.x, c4
mul r0.x, c14, r0
mad oT0.z, r0.y, c16.x, r1.x
mad r0.y, r0.x, c22.x, c22
frc r0.y, r0
mov r0.x, c4
mul r0.x, c15, r0
mad r1.y, r0, c22.z, c22.w
mad r1.x, r0, c22, c22.y
sincos r0.xy, r1.y, c21.xyzw, c20.xyzw
frc r0.x, r1
mad r0.x, r0, c22.z, c22.w
sincos r1.xy, r0.x, c21.xyzw, c20.xyzw
mov r0.w, c8.x
mov r0.z, c7.x
mul r0.zw, r0, c4.xyxy
frc r1.zw, r0
mad r0.zw, v1.xyxy, c5.xyxy, c5
add r0.zw, r0, r1
mad oT0.y, r1, c13.x, r0.w
mad oT0.x, r0.y, c12, r0.z
mov oT1, c11.x
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_Time]
Vector 6 [_MainTex_ST]
Vector 7 [_DetailTex_ST]
Float 8 [_ScrollX]
Float 9 [_ScrollY]
Float 10 [_Scroll2X]
Float 11 [_Scroll2Y]
Float 12 [_MMultiplier]
Float 13 [_SineAmplX]
Float 14 [_SineAmplY]
Float 15 [_SineFreqX]
Float 16 [_SineFreqY]
Float 17 [_SineAmplX2]
Float 18 [_SineAmplY2]
Float 19 [_SineFreqX2]
Float 20 [_SineFreqY2]
"!!ARBvp1.0
# 89 ALU
PARAM c[25] = { { 24.980801, -24.980801, 0.15915491, 0.25 },
		state.matrix.mvp,
		program.local[5..20],
		{ 0, 0.5, 1, -1 },
		{ -60.145809, 60.145809, 85.453789, -85.453789 },
		{ -64.939346, 64.939346, 19.73921, -19.73921 },
		{ -9, 0.75 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MOV R0.x, c[20];
MUL R0.x, R0, c[5];
MAD R0.x, R0, c[0].z, -c[0].w;
FRC R1.w, R0.x;
ADD R1.xyz, -R1.w, c[21];
MOV R0.x, c[19];
MUL R0.w, R0.x, c[5].x;
MUL R2.xyz, R1, R1;
MUL R0.xyz, R2, c[0].xyxw;
MAD R0.w, R0, c[0].z, -c[0];
FRC R0.w, R0;
ADD R1.xyz, -R0.w, c[21];
ADD R0.xyz, R0, c[22].xyxw;
MAD R0.xyz, R0, R2, c[22].zwzw;
MAD R0.xyz, R0, R2, c[23].xyxw;
MAD R3.xyz, R0, R2, c[23].zwzw;
MUL R1.xyz, R1, R1;
MAD R3.xyz, R3, R2, c[21].wzww;
MUL R0.xyz, R1, c[0].xyxw;
ADD R2.xyz, R0, c[22].xyxw;
SLT R4.x, R1.w, c[0].w;
SGE R4.yz, R1.w, c[24].xxyw;
MOV R0.xz, R4;
DP3 R0.y, R4, c[21].wzww;
DP3 R1.w, R3, -R0;
MAD R0.xyz, R2, R1, c[22].zwzw;
MAD R0.xyz, R0, R1, c[23].xyxw;
MAD R0.xyz, R0, R1, c[23].zwzw;
MAD R1.xyz, R0, R1, c[21].wzww;
SGE R0.yz, R0.w, c[24].xxyw;
SLT R0.x, R0.w, c[0].w;
DP3 R3.y, R0, c[21].wzww;
MOV R3.xz, R0;
MOV R0.x, c[15];
MUL R0.x, R0, c[5];
DP3 R0.y, R1, -R3;
MAD R2.zw, vertex.texcoord[0].xyxy, c[7].xyxy, c[7];
MAD R0.x, R0, c[0].z, -c[0].w;
MOV R2.y, c[11].x;
MOV R2.x, c[10];
MUL R2.xy, R2, c[5];
FRC R2.xy, R2;
ADD R2.xy, R2.zwzw, R2;
MAD result.texcoord[0].w, R1, c[18].x, R2.y;
FRC R1.w, R0.x;
MAD result.texcoord[0].z, R0.y, c[17].x, R2.x;
MOV R0.y, c[16].x;
MUL R0.y, R0, c[5].x;
MAD R0.w, R0.y, c[0].z, -c[0];
FRC R0.w, R0;
ADD R0.xyz, -R1.w, c[21];
MUL R0.xyz, R0, R0;
MUL R1.xyz, R0, c[0].xyxw;
ADD R2.xyz, -R0.w, c[21];
MUL R2.xyz, R2, R2;
MUL R3.xyz, R2, c[0].xyxw;
ADD R1.xyz, R1, c[22].xyxw;
MAD R1.xyz, R1, R0, c[22].zwzw;
MAD R1.xyz, R1, R0, c[23].xyxw;
MAD R1.xyz, R1, R0, c[23].zwzw;
ADD R3.xyz, R3, c[22].xyxw;
MAD R3.xyz, R3, R2, c[22].zwzw;
MAD R3.xyz, R3, R2, c[23].xyxw;
MAD R1.xyz, R1, R0, c[21].wzww;
SLT R4.x, R1.w, c[0].w;
SGE R4.yz, R1.w, c[24].xxyw;
MOV R0.xz, R4;
DP3 R0.y, R4, c[21].wzww;
DP3 R1.w, R1, -R0;
MAD R1.xyz, R3, R2, c[23].zwzw;
MAD R1.xyz, R1, R2, c[21].wzww;
MAD R2.xy, vertex.texcoord[0], c[6], c[6].zwzw;
MOV R0.y, c[9].x;
MOV R0.x, c[8];
MUL R0.xy, R0, c[5];
FRC R0.xy, R0;
ADD R3.xy, R2, R0;
SLT R0.x, R0.w, c[0].w;
SGE R0.yz, R0.w, c[24].xxyw;
MOV R2.xz, R0;
DP3 R2.y, R0, c[21].wzww;
DP3 R0.x, R1, -R2;
MAD result.texcoord[0].y, R0.x, c[14].x, R3;
MAD result.texcoord[0].x, R1.w, c[13], R3;
MOV result.texcoord[1], c[12].x;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 89 instructions, 5 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_ON" }
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_Time]
Vector 5 [_MainTex_ST]
Vector 6 [_DetailTex_ST]
Float 7 [_ScrollX]
Float 8 [_ScrollY]
Float 9 [_Scroll2X]
Float 10 [_Scroll2Y]
Float 11 [_MMultiplier]
Float 12 [_SineAmplX]
Float 13 [_SineAmplY]
Float 14 [_SineFreqX]
Float 15 [_SineFreqY]
Float 16 [_SineAmplX2]
Float 17 [_SineAmplY2]
Float 18 [_SineFreqX2]
Float 19 [_SineFreqY2]
"vs_2_0
; 85 ALU
def c20, -0.02083333, -0.12500000, 1.00000000, 0.50000000
def c21, -0.00000155, -0.00002170, 0.00260417, 0.00026042
def c22, 0.15915491, 0.50000000, 6.28318501, -3.14159298
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c4
mul r0.x, c19, r0
mad r0.x, r0, c22, c22.y
frc r0.x, r0
mad r1.x, r0, c22.z, c22.w
sincos r0.xy, r1.x, c21.xyzw, c20.xyzw
mov r0.x, c4
mul r0.x, c18, r0
mad r0.x, r0, c22, c22.y
frc r0.x, r0
mad r1.z, r0.x, c22, c22.w
mad r0.zw, v1.xyxy, c6.xyxy, c6
mov r1.y, c10.x
mov r1.x, c9
mul r1.xy, r1, c4
frc r1.xy, r1
add r1.xy, r0.zwzw, r1
mad oT0.w, r0.y, c17.x, r1.y
sincos r0.xy, r1.z, c21.xyzw, c20.xyzw
mov r0.x, c4
mul r0.x, c14, r0
mad oT0.z, r0.y, c16.x, r1.x
mad r0.y, r0.x, c22.x, c22
frc r0.y, r0
mov r0.x, c4
mul r0.x, c15, r0
mad r1.y, r0, c22.z, c22.w
mad r1.x, r0, c22, c22.y
sincos r0.xy, r1.y, c21.xyzw, c20.xyzw
frc r0.x, r1
mad r0.x, r0, c22.z, c22.w
sincos r1.xy, r0.x, c21.xyzw, c20.xyzw
mov r0.w, c8.x
mov r0.z, c7.x
mul r0.zw, r0, c4.xyxy
frc r1.zw, r0
mad r0.zw, v1.xyxy, c5.xyxy, c5
add r0.zw, r0, r1
mad oT0.y, r1, c13.x, r0.w
mad oT0.x, r0.y, c12, r0.z
mov oT1, c11.x
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Keywords { "LIGHTMAP_OFF" }
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DetailTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 4 ALU, 2 TEX
TEMP R0;
TEMP R1;
TEX R1, fragment.texcoord[0].zwzw, texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MUL R0, R0, R1;
MUL result.color, R0, fragment.texcoord[1];
END
# 4 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_OFF" }
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DetailTex] 2D
"ps_2_0
; 5 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
dcl t0
dcl t1
texld r1, t0, s0
mov r0.y, t0.w
mov r0.x, t0.z
texld r0, r0, s1
mul_pp r0, r1, r0
mul_pp r0, r0, t1
mov_pp oC0, r0
"
}
SubProgram "opengl " {
Keywords { "LIGHTMAP_ON" }
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DetailTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 4 ALU, 2 TEX
TEMP R0;
TEMP R1;
TEX R1, fragment.texcoord[0].zwzw, texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MUL R0, R0, R1;
MUL result.color, R0, fragment.texcoord[1];
END
# 4 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Keywords { "LIGHTMAP_ON" }
SetTexture 0 [_MainTex] 2D
SetTexture 1 [_DetailTex] 2D
"ps_2_0
; 5 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
dcl t0
dcl t1
texld r1, t0, s0
mov r0.y, t0.w
mov r0.x, t0.z
texld r0, r0, s1
mul_pp r0, r1, r0
mul_pp r0, r0, t1
mov_pp oC0, r0
"
}
}
 }
}
}