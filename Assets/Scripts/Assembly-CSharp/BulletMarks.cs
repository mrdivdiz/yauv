using System;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class BulletMarks : MonoBehaviour
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x000675B4 File Offset: 0x000657B4
	public void start()
	{
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000675B8 File Offset: 0x000657B8
	public void GenerateDecal(DecalInfo decalInfo)
	{
		GameObject gameObject = null;
		HitType type = decalInfo.type;
		if (decalInfo.obj != null)
		{
			gameObject = decalInfo.obj;
			decalInfo.obj = null;
		}
		Texture2D mainTexture;
		switch (type)
		{
		case HitType.CONCRETE:
		{
			if (this.concrete == null)
			{
				return;
			}
			if (this.concrete.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.concrete.Length);
			mainTexture = this.concrete[num];
			break;
		}
		case HitType.WOOD:
		{
			if (this.wood == null)
			{
				return;
			}
			if (this.wood.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.wood.Length);
			mainTexture = this.wood[num];
			break;
		}
		case HitType.METAL:
		{
			if (this.metal == null)
			{
				return;
			}
			if (this.metal.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.metal.Length);
			mainTexture = this.metal[num];
			break;
		}
		case HitType.OLD_METAL:
		{
			if (this.oldMetal == null)
			{
				return;
			}
			if (this.oldMetal.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.oldMetal.Length);
			mainTexture = this.oldMetal[num];
			break;
		}
		case HitType.GLASS:
		{
			if (this.glass == null)
			{
				return;
			}
			if (this.glass.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.glass.Length);
			mainTexture = this.glass[num];
			break;
		}
		case HitType.GENERIC:
		{
			if (this.generic == null)
			{
				return;
			}
			if (this.generic.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.generic.Length);
			mainTexture = this.generic[num];
			break;
		}
		case HitType.BLOD:
		{
			if (this.blood == null)
			{
				return;
			}
			if (this.blood.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.blood.Length);
			mainTexture = this.blood[num];
			break;
		}
		default:
		{
			if (this.wood == null)
			{
				return;
			}
			if (this.wood.Length == 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(0, this.wood.Length);
			mainTexture = this.wood[num];
			return;
		}
		}
		base.transform.Rotate(new Vector3(0f, 0f, UnityEngine.Random.Range(-180f, 180f)));
		Decal.dCount++;
		Decal component = base.gameObject.GetComponent<Decal>();
		component.affectedObjects = new GameObject[1];
		component.affectedObjects[0] = gameObject;
		component.decalMode = DecalMode.MESH_COLLIDER;
		component.pushDistance = 0.009f + BulletMarkManager.Add(base.gameObject);
		component.decalMaterial = new Material(component.decalMaterial)
		{
			mainTexture = mainTexture
		};
		component.CalculateDecal();
		component.transform.parent = gameObject.transform;
	}

	// Token: 0x04000F38 RID: 3896
	public Texture2D[] concrete;

	// Token: 0x04000F39 RID: 3897
	public Texture2D[] wood;

	// Token: 0x04000F3A RID: 3898
	public Texture2D[] metal;

	// Token: 0x04000F3B RID: 3899
	public Texture2D[] oldMetal;

	// Token: 0x04000F3C RID: 3900
	public Texture2D[] glass;

	// Token: 0x04000F3D RID: 3901
	public Texture2D[] generic;

	// Token: 0x04000F3E RID: 3902
	public Texture2D[] blood;
}
