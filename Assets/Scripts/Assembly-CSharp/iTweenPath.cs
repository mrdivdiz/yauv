using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class iTweenPath : MonoBehaviour
{
	// Token: 0x06000CDC RID: 3292 RVA: 0x000A3554 File Offset: 0x000A1754
	private void OnEnable()
	{
		if (!iTweenPath.paths.ContainsKey(this.pathName.ToLower()))
		{
			iTweenPath.paths.Add(this.pathName.ToLower(), this);
		}
		else
		{
			iTweenPath.paths[this.pathName.ToLower()] = this;
		}
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x000A35AC File Offset: 0x000A17AC
	private void OnDrawGizmosSelected()
	{
		if (this.nodes.Count > 0)
		{
			Vector3[] array = this.nodes.ToArray();
			if (this.isParentRelative)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = base.transform.TransformPoint(array[i]);
				}
				if (this.isClosed)
				{
					Vector3[] array2 = new Vector3[array.Length + 1];
					array.CopyTo(array2, 0);
					array2[array2.Length - 1] = array2[0];
					array = array2;
				}
			}
			iTween.DrawPath(array, this.pathColor);
		}
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x000A3660 File Offset: 0x000A1860
	public static Vector3[] GetPath(string requestedName)
	{
		requestedName = requestedName.ToLower();
		if (iTweenPath.paths.ContainsKey(requestedName))
		{
			Vector3[] array = iTweenPath.paths[requestedName].nodes.ToArray();
			if (iTweenPath.paths[requestedName].isParentRelative)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = iTweenPath.paths[requestedName].transform.TransformPoint(array[i]);
				}
			}
			if (iTweenPath.paths[requestedName].isClosed)
			{
				Vector3[] array2 = new Vector3[array.Length + 1];
				array.CopyTo(array2, 0);
				array2[array2.Length - 1] = array2[0];
				array = array2;
			}
			return array;
		}
		Debug.Log("No path with that name exists! Are you sure you wrote it correctly?");
		return null;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x000A3744 File Offset: 0x000A1944
	public static bool IsClosed(string requestedName)
	{
		requestedName = requestedName.ToLower();
		if (iTweenPath.paths.ContainsKey(requestedName))
		{
			return iTweenPath.paths[requestedName].isClosed;
		}
		Debug.Log("No path with that name exists! Are you sure you wrote it correctly?");
		return false;
	}

	// Token: 0x0400167F RID: 5759
	public string pathName = string.Empty;

	// Token: 0x04001680 RID: 5760
	public Color pathColor = Color.cyan;

	// Token: 0x04001681 RID: 5761
	public List<Vector3> nodes = new List<Vector3>
	{
		Vector3.zero,
		Vector3.zero
	};

	// Token: 0x04001682 RID: 5762
	public int nodeCount;

	// Token: 0x04001683 RID: 5763
	public static Dictionary<string, iTweenPath> paths = new Dictionary<string, iTweenPath>();

	// Token: 0x04001684 RID: 5764
	public bool initialized;

	// Token: 0x04001685 RID: 5765
	public string initialName = string.Empty;

	// Token: 0x04001686 RID: 5766
	public bool isParentRelative;

	// Token: 0x04001687 RID: 5767
	public bool isClosed;

	// Token: 0x04001688 RID: 5768
	public float radius = 1f;
}
