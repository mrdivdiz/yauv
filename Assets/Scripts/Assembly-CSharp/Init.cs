using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class Init : MonoBehaviour
{
	// Token: 0x06000313 RID: 787 RVA: 0x0001768C File Offset: 0x0001588C
	private void Awake()
	{
		this.GenerateLevel();
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00017694 File Offset: 0x00015894
	private void GenerateLevel()
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject in array)
		{
			if (gameObject.layer == 8)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		for (int j = 0; j < this.PrefabNum; j++)
		{
			Vector3 position = new Vector3(UnityEngine.Random.Range(50f, 950f), this.PosY, UnityEngine.Random.Range(50f, 950f));
			GameObject gameObject2 = UnityEngine.Object.Instantiate(this.Prefabs[UnityEngine.Random.Range(0, this.Prefabs.Length)], position, Quaternion.identity) as GameObject;
			gameObject2.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0f, 360f));
		}
	}

	// Token: 0x04000390 RID: 912
	public GameObject[] Prefabs;

	// Token: 0x04000391 RID: 913
	public int PrefabNum;

	// Token: 0x04000392 RID: 914
	public float PosY;
}
