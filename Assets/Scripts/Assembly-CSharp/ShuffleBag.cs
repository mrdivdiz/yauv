using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x02000003 RID: 3
[Serializable]
public class ShuffleBag
{
	// Token: 0x06000003 RID: 3 RVA: 0x000020F8 File Offset: 0x000002F8
	public ShuffleBag()
	{
		this.data = new UnityScript.Lang.Array();
		this.cursor = -1;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002114 File Offset: 0x00000314
	public virtual void add(object item, int num)
	{
		int num2 = 1;
		if(num != null){
			num2 = num;
		}
		// = num ?? 1;
		for (;;)
		{
			int num3;
			num2 = (num3 = num2) - 1;
			if (num3 == 0)
			{
				break;
			}
			this.data.push(item);
		}
		this.cursor = this.data.length - 1;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002160 File Offset: 0x00000360
	public virtual int next()
	{
		int num = 0;
		int result;
		if (this.cursor < 1)
		{
			this.cursor = this.data.length - 1;
			result = RuntimeServices.UnboxInt32(this.data[0]);
		}
		else
		{
			object value = UnityEngine.Random.Range(0, this.cursor + 1);
			num = RuntimeServices.UnboxInt32(this.data[RuntimeServices.UnboxInt32(value)]);
			this.data[RuntimeServices.UnboxInt32(value)] = this.data[this.cursor];
			this.data[this.cursor] = num;
			this.cursor--;
			result = num;
		}
		return result;
	}

	// Token: 0x04000001 RID: 1
	public UnityScript.Lang.Array data;

	// Token: 0x04000002 RID: 2
	public int cursor;
}
