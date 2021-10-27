using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
[Serializable]
public class mouse : MonoBehaviour
{
	// Token: 0x06000106 RID: 262 RVA: 0x00006A68 File Offset: 0x00004C68
	public virtual void Start()
	{
		Screen.lockCursor = false;
		Cursor.visible = true;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00006A78 File Offset: 0x00004C78
	public virtual void Main()
	{
	}

	// Token: 0x040000EA RID: 234
	public Texture cursorImage;
}
