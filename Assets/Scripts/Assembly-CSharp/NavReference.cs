using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class NavReference : MonoBehaviour
{
	// Token: 0x0600065C RID: 1628 RVA: 0x00030550 File Offset: 0x0002E750
	private void Awake()
	{
		NavReference.instance = this;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00030558 File Offset: 0x0002E758
	private void OnDestroy()
	{
		for (int i = 0; i < this.nodes.Length; i++)
		{
			this.nodes[i] = null;
		}
		NavReference.instance = null;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00030590 File Offset: 0x0002E790
	private void Update()
	{
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00030594 File Offset: 0x0002E794
	private void OnDrawGizmos()
	{
		foreach (NavNode navNode in this.nodes)
		{
			foreach (int num in navNode.connectedNodesRefs)
			{
				if (this.nodes[num].connectedNodesRefs != null)
				{
					foreach (int num2 in this.nodes[num].connectedNodesRefs)
					{
						if (this.nodes[num2] == navNode)
						{
							Gizmos.color = Color.blue;
							Gizmos.DrawLine(navNode.transform.position, this.nodes[num].transform.position);
						}
					}
				}
			}
		}
	}

	// Token: 0x0400078B RID: 1931
	public NavNode[] nodes;

	// Token: 0x0400078C RID: 1932
	public static NavReference instance;
}
