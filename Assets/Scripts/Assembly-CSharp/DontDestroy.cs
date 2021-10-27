using System;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class DontDestroy : MonoBehaviour
{
	// Token: 0x0600072A RID: 1834 RVA: 0x000390C4 File Offset: 0x000372C4
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x000390D8 File Offset: 0x000372D8
	public void Start()
	{
		MonoBehaviour.print("start function called");
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach (UnityEngine.Object obj in array)
		{
			GameObject gameObject = (GameObject)obj;
			foreach (string text in this.loadedObjects)
			{
				if (text != null && text != string.Empty && text == gameObject.name)
				{
					MonoBehaviour.print(text);
					break;
				}
			}
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00039180 File Offset: 0x00037380
	public void StoreLoadedObjects()
	{
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		this.loadedObjects = new string[array.Length];
		int num = 0;
		foreach (UnityEngine.Object obj in array)
		{
			GameObject gameObject = (GameObject)obj;
			this.loadedObjects[num] = gameObject.name;
			num++;
		}
	}

	// Token: 0x0400090B RID: 2315
	public string[] loadedObjects;
}
