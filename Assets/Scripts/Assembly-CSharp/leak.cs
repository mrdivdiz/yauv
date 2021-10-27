using System;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class leak : MonoBehaviour
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x000A3790 File Offset: 0x000A1990
	private void Start()
	{
		MonoBehaviour.print("==========================Totals==========================");
		MonoBehaviour.print("All " + UnityEngine.Object.FindObjectsOfType(typeof(UnityEngine.Object)).Length);
		MonoBehaviour.print("Textures " + UnityEngine.Object.FindObjectsOfType(typeof(Texture)).Length);
		MonoBehaviour.print("AudioClips " + UnityEngine.Object.FindObjectsOfType(typeof(AudioClip)).Length);
		MonoBehaviour.print("AnimationClips " + UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)).Length);
		MonoBehaviour.print("Meshes " + UnityEngine.Object.FindObjectsOfType(typeof(Mesh)).Length);
		MonoBehaviour.print("Materials " + UnityEngine.Object.FindObjectsOfType(typeof(Material)).Length);
		MonoBehaviour.print("GameObjects " + UnityEngine.Object.FindObjectsOfType(typeof(GameObject)).Length);
		MonoBehaviour.print("Components " + UnityEngine.Object.FindObjectsOfType(typeof(Component)).Length);
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("=========================Textures=========================");
		Texture[] array = (Texture[])Resources.FindObjectsOfTypeAll(typeof(Texture));
		foreach (Texture texture in array)
		{
			MonoBehaviour.print("Texture: " + texture.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("=========================AudioClips=======================");
		AudioClip[] array3 = (AudioClip[])Resources.FindObjectsOfTypeAll(typeof(AudioClip));
		foreach (AudioClip audioClip in array3)
		{
			MonoBehaviour.print("AudioClip: " + audioClip.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("=======================AnimationClips=====================");
		AnimationClip[] array5 = (AnimationClip[])Resources.FindObjectsOfTypeAll(typeof(AnimationClip));
		foreach (AnimationClip animationClip in array5)
		{
			MonoBehaviour.print("AnimationClip: " + animationClip.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("===========================Meshs==========================");
		UnityEngine.Object[] array7 = Resources.FindObjectsOfTypeAll(typeof(Mesh));
		foreach (UnityEngine.Object @object in array7)
		{
			MonoBehaviour.print("Mesh: " + @object.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("===========================Meshs==========================");
		UnityEngine.Object[] array9 = Resources.FindObjectsOfTypeAll(typeof(Material));
		foreach (UnityEngine.Object object2 in array9)
		{
			MonoBehaviour.print("Material: " + object2.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("=========================GameObjects=======================");
		UnityEngine.Object[] array11 = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (UnityEngine.Object object3 in array11)
		{
			MonoBehaviour.print("GameObject: " + object3.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
		MonoBehaviour.print("=========================Components========================");
		UnityEngine.Object[] array13 = Resources.FindObjectsOfTypeAll(typeof(Component));
		foreach (UnityEngine.Object object4 in array13)
		{
			MonoBehaviour.print("Component: " + object4.name);
		}
		MonoBehaviour.print("----------------------------------------------------------");
	}
}
