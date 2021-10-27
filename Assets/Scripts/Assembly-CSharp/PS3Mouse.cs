using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class PS3Mouse : MonoBehaviour
{
	// Token: 0x06000592 RID: 1426 RVA: 0x00023284 File Offset: 0x00021484
	private void Start()
	{
		PS3Mouse.pos = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x000232B4 File Offset: 0x000214B4
	private void Update()
	{
		float num = PS3Mouse.pos.x + Input.GetAxis("Horizontal") * this.speed * Time.deltaTime;
		float num2 = PS3Mouse.pos.y - Input.GetAxis("Vertical") * this.speed * Time.deltaTime;
		num = Mathf.Clamp(num, 0f, (float)(Screen.width - this.cursor.width));
		num2 = Mathf.Clamp(num2, 0f, (float)(Screen.height - this.cursor.height));
		PS3Mouse.pos = new Vector2(num, num2);
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00023350 File Offset: 0x00021550
	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(PS3Mouse.pos.x, PS3Mouse.pos.y, (float)this.cursor.width, (float)this.cursor.height), this.cursor);
	}

	// Token: 0x040005BF RID: 1471
	public Texture2D cursor;

	// Token: 0x040005C0 RID: 1472
	public static Vector2 pos;

	// Token: 0x040005C1 RID: 1473
	public float speed = 400f;
}
