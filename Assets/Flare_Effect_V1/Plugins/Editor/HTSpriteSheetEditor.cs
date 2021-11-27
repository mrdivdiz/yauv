// HTSpriteSheet v1.0 (July 2012)
// HTSpriteSheet.cs library is copyright (c) of Hedgehog Team
// Please send feedback or bug reports to npoursin@hotmail.com

using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// HTSpriteSheet editor.
/// </summary>
[CustomEditor(typeof(HTSpriteSheet))]
public class HTSpriteSheetEditor : Editor {

	// Use this for initialization
	public override void OnInspectorGUI(){
		
		GUIStyle style;
		HTSpriteSheet t;
		
		t = (HTSpriteSheet)target;
		style = new GUIStyle();
		style.fontStyle =FontStyle.Bold;
			
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Turret properties
		GUILayout.Label("Sprite sheet properties",style);	
		EditorGUILayout.Space();
		t.spriteSheetMaterial = (Material)EditorGUILayout.ObjectField("Sprite sheet material", t.spriteSheetMaterial,typeof(Material),true); 
		t.uvAnimationTileX = EditorGUILayout.IntField("Tile X",t.uvAnimationTileX);
		t.uvAnimationTileY = EditorGUILayout.IntField("Tile Y",t.uvAnimationTileY);
		t.spriteCount = EditorGUILayout.IntField("Number of sprite",t.spriteCount);
		t.framesPerSecond = EditorGUILayout.IntField("Frames per second",t.framesPerSecond);
		t.isOneShot = EditorGUILayout.Toggle( "One shot",t.isOneShot);

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		GUILayout.Label("Sprite properties",style);
		EditorGUILayout.Space();
		t.billboarding = (HTSpriteSheet.CameraFacingMode)EditorGUILayout.EnumPopup("Camera facing",t.billboarding);
		t.size = EditorGUILayout.Vector3Field("Size",t.size);
		t.speedGrowing = EditorGUILayout.FloatField("Speed growing",t.speedGrowing);
		t.randomRotation = EditorGUILayout.Toggle( "Random rotation",t.randomRotation);
		
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// Light Effect
		GUILayout.Label("Light properties",style);			
		t.addLightEffect = EditorGUILayout.Toggle( "Add light effect",t.addLightEffect);
		if ( t.addLightEffect ){
			t.lightRange = EditorGUILayout.FloatField("Light range",t.lightRange);	
			t.lightColor = EditorGUILayout.ColorField( "Light color", t.lightColor);
			t.lightFadeSpeed = EditorGUILayout.FloatField("Light fade speed",t.lightFadeSpeed);	
		}

		// Refresh
		if (GUI.changed){
			EditorUtility.SetDirty (target);
		}
		 		 	
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
}
