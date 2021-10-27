using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class XMLNode : Hashtable
{
	// Token: 0x060004DB RID: 1243 RVA: 0x0001FD78 File Offset: 0x0001DF78
	public XMLNodeList GetNodeList(string path)
	{
		return (XMLNodeList)this.GetObject(path);
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0001FD88 File Offset: 0x0001DF88
	public XMLNode GetNode(string path)
	{
		return (XMLNode)this.GetObject(path);
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0001FD98 File Offset: 0x0001DF98
	public string GetValue(string path)
	{
		return this.GetObject(path) as string;
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0001FDA8 File Offset: 0x0001DFA8
	private object GetObject(string path)
	{
		string[] array = path.Split(new char[]
		{
			">"[0]
		});
		XMLNode xmlnode = this;
		XMLNodeList xmlnodeList = null;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (flag)
			{
				xmlnode = (XMLNode)xmlnodeList[int.Parse(array[i])];
				flag = false;
			}
			else
			{
				object obj = xmlnode[array[i]];
				if (!(obj is ArrayList))
				{
					if (i != array.Length - 1)
					{
						string text = string.Empty;
						for (int j = 0; j <= i; j++)
						{
							text = text + ">" + array[j];
						}
						Debug.Log("xml path search truncated. Wanted: " + path + " got: " + text);
					}
					return obj;
				}
				xmlnodeList = (XMLNodeList)obj;
				flag = true;
			}
		}
		if (flag)
		{
			return xmlnodeList;
		}
		return xmlnode;
	}
}
