using System;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class LoadingScene : MonoBehaviour
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x00070C78 File Offset: 0x0006EE78
	private void Awake()
	{
		GameObject gameObject = GameObject.Find("Astrolab");
		if (gameObject != null && gameObject.transform.localPosition.x > 0.5f)
		{
			if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.4f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.2f)
			{
				gameObject.transform.localPosition = new Vector3(1.037f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
			}
			else if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.6f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.4f)
			{
				gameObject.transform.localPosition = new Vector3(1.16f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
			}
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00070DDC File Offset: 0x0006EFDC
	private void Start()
	{
		this.style = new GUIStyle();
		this.style.font = Language.GetFont18();
		this.style.alignment = TextAnchor.MiddleCenter;
		this.style.wordWrap = true;
		this.style.normal.textColor = Color.white;
		base.Invoke("loadit", 0f);
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00070E44 File Offset: 0x0006F044
	private void loadit()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		Application.LoadLevelAsync(this.levelToBeLoaded);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00070E5C File Offset: 0x0006F05C
	private void Update()
	{
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00070E60 File Offset: 0x0006F060
	private void OnDestroy()
	{
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00070E64 File Offset: 0x0006F064
	private void report()
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

	// Token: 0x06000A72 RID: 2674 RVA: 0x0007124C File Offset: 0x0006F44C
	private void OnGUI()
	{
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick || AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
		{
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect((float)(Screen.width / 2) - 100f, (float)Screen.height - 28f, 200f, 25f), Language.Get("M_Loading", 60), this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect((float)(Screen.width / 2) - 100f - 2f, (float)Screen.height - 25f - 2f, 200f, 25f), Language.Get("M_Loading", 60), this.style);
		}
		else
		{
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect((float)(Screen.width / 2) - 100f, (float)Screen.height - 35f, 200f, 25f), Language.Get("M_Loading", 60), this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect((float)(Screen.width / 2) - 100f - 2f, (float)Screen.height - 35f - 2f, 200f, 25f), Language.Get("M_Loading", 60), this.style);
		}
	}

	// Token: 0x040010B1 RID: 4273
	public string levelToBeLoaded = "Prototype";

	// Token: 0x040010B2 RID: 4274
	public string levelTitleKeyword = "M_Prototype";

	// Token: 0x040010B3 RID: 4275
	private GUIStyle style;
}
