using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class LocalizedAsset : MonoBehaviour
{
	// Token: 0x060004E3 RID: 1251 RVA: 0x00020548 File Offset: 0x0001E748
	public void Awake()
	{
		LocalizedAsset.LocalizeAsset(this.localizeTarget);
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00020558 File Offset: 0x0001E758
	public void LocalizeAsset()
	{
		LocalizedAsset.LocalizeAsset(this.localizeTarget);
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00020568 File Offset: 0x0001E768
	public static void LocalizeAsset(UnityEngine.Object target)
	{
		if (target == null)
		{
			Debug.LogError("LocalizedAsset target is null");
			return;
		}
		if (target.GetType() == typeof(GUITexture))
		{
			GUITexture guitexture = (GUITexture)target;
			if (guitexture.texture != null)
			{
				Texture texture = (Texture)Language.GetAsset(guitexture.texture.name);
				if (texture != null)
				{
					guitexture.texture = texture;
				}
			}
		}
		else if (target.GetType() == typeof(Material))
		{
			Material material = (Material)target;
			if (material.mainTexture != null)
			{
				Texture texture2 = (Texture)Language.GetAsset(material.mainTexture.name);
				if (texture2 != null)
				{
					material.mainTexture = texture2;
				}
			}
		}
		else if (target.GetType() == typeof(MeshRenderer))
		{
			MeshRenderer meshRenderer = (MeshRenderer)target;
			if (meshRenderer.material.mainTexture != null)
			{
				Texture texture3 = (Texture)Language.GetAsset(meshRenderer.material.mainTexture.name);
				if (texture3 != null)
				{
					meshRenderer.material.mainTexture = texture3;
				}
			}
		}
		else
		{
			Debug.LogError("Could not localize this object type: " + target.GetType());
		}
	}

	// Token: 0x04000549 RID: 1353
	public UnityEngine.Object localizeTarget;
}
