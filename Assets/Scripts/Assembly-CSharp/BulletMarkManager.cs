using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class BulletMarkManager : MonoBehaviour
{
	// Token: 0x060009F0 RID: 2544 RVA: 0x00067270 File Offset: 0x00065470
	private void Start()
	{
		if (BulletMarkManager.instance == null)
		{
			BulletMarkManager.instance = this;
		}
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00067288 File Offset: 0x00065488
	private void OnDestroy()
	{
		if (BulletMarkManager.instance == this)
		{
			BulletMarkManager.instance = null;
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x000672A0 File Offset: 0x000654A0
	public static float Add(GameObject go)
	{
		if (BulletMarkManager.instance == null)
		{
			GameObject gameObject = new GameObject("BulletMarkManager");
			BulletMarkManager.instance = gameObject.AddComponent<BulletMarkManager>();
			BulletMarkManager.instance.marks = new ArrayList();
			BulletMarkManager.instance.pushDistances = new ArrayList();
			BulletMarkManager.instance.maxMarks = 10;
		}
		Transform transform = go.transform;
		Vector3 position = transform.position;
		float num = transform.localScale.x * transform.localScale.x * 0.25f + transform.localScale.y * transform.localScale.y * 0.25f + transform.localScale.z * transform.localScale.z * 0.25f;
		num = Mathf.Sqrt(num);
		float num2 = num * 2f;
		num *= 0.2f;
		if (BulletMarkManager.instance.marks.Count == BulletMarkManager.instance.maxMarks)
		{
			GameObject gameObject2 = BulletMarkManager.instance.marks[0] as GameObject;
			UnityEngine.Object.Destroy(gameObject2);
			BulletMarkManager.instance.marks.RemoveAt(0);
			BulletMarkManager.instance.pushDistances.RemoveAt(0);
		}
		float num3 = 0.0001f;
		int num4 = BulletMarkManager.instance.marks.Count;
		for (int i = 0; i < num4; i++)
		{
			GameObject gameObject2 = BulletMarkManager.instance.marks[i] as GameObject;
			if (gameObject2 != null)
			{
				Transform transform2 = gameObject2.transform;
				float magnitude = (transform2.position - position).magnitude;
				if (magnitude < num)
				{
					UnityEngine.Object.Destroy(gameObject2);
					BulletMarkManager.instance.marks.RemoveAt(i);
					BulletMarkManager.instance.pushDistances.RemoveAt(i);
					i--;
					num4--;
				}
				else if (magnitude < num2)
				{
					float b = (float)BulletMarkManager.instance.pushDistances[i];
					num3 = Mathf.Max(num3, b);
				}
			}
			else
			{
				BulletMarkManager.instance.marks.RemoveAt(i);
				BulletMarkManager.instance.pushDistances.RemoveAt(i);
				i--;
				num4--;
			}
		}
		num3 += 0.0005f;
		BulletMarkManager.instance.marks.Add(go);
		BulletMarkManager.instance.pushDistances.Add(num3);
		return num3;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0006753C File Offset: 0x0006573C
	private static void ClearMarks()
	{
		if (BulletMarkManager.instance.marks.Count > 0)
		{
			for (int i = 0; i < BulletMarkManager.instance.marks.Count; i++)
			{
				GameObject obj = BulletMarkManager.instance.marks[i] as GameObject;
				UnityEngine.Object.Destroy(obj);
			}
			BulletMarkManager.instance.marks.Clear();
		}
	}

	// Token: 0x04000F2C RID: 3884
	private static BulletMarkManager instance;

	// Token: 0x04000F2D RID: 3885
	public int maxMarks;

	// Token: 0x04000F2E RID: 3886
	public ArrayList marks;

	// Token: 0x04000F2F RID: 3887
	public ArrayList pushDistances;
}
