using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
[AddComponentMenu("Camera-Control/Smooth Follow")]
[Serializable]
public class SmoothFollow : MonoBehaviour
{
	// Token: 0x060000C4 RID: 196 RVA: 0x00009758 File Offset: 0x00007958
	public SmoothFollow()
	{
		this.distance = 10f;
		this.height = 5f;
		this.heightDamping = 2f;
		this.rotationDamping = 3f;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00009798 File Offset: 0x00007998
	public virtual void Start()
	{
		this.occludedDistance = this.distance;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000097A8 File Offset: 0x000079A8
	public virtual void LateUpdate()
	{
		if (this.target)
		{
			Vector3 position = this.target.position;
			if (this.occludedDistance < this.distance)
			{
				position.y += 0.06f;
			}
			float y = this.target.eulerAngles.y;
			float to = position.y + this.height;
			float num = this.transform.eulerAngles.y;
			float num2 = position.y;
			num = Mathf.LerpAngle(num, y, this.rotationDamping * Time.deltaTime);
			num2 = Mathf.Lerp(num2, to, this.heightDamping * Time.deltaTime);
			Quaternion rotation = Quaternion.Euler((float)0, num, (float)0);
			Vector3 a = position - rotation * Vector3.forward * this.distance;
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(position, a - position, out raycastHit, this.distance))
			{
				this.occludedDistance = Mathf.SmoothDamp(this.occludedDistance, raycastHit.distance - 0.01f, ref this.odr, 0.1f);
			}
			else
			{
				this.occludedDistance = Mathf.SmoothDamp(this.occludedDistance, this.distance, ref this.odr, 1f);
			}
			this.transform.position = position;
			this.transform.position = this.transform.position - rotation * Vector3.forward * this.occludedDistance;
			float y2 = num2;
			Vector3 position2 = this.transform.position;
			float num3 = position2.y = y2;
			Vector3 vector = this.transform.position = position2;
			this.transform.LookAt(position);
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00009988 File Offset: 0x00007B88
	public virtual void OnGUI()
	{
		if (this.hitTexture != null)
		{
			Color color = default(Color);
			Color color2 = default(Color);
			color2 = GUI.color;
			color2.a = this.hitAlpha;
			GUI.color = color2;
			GUI.DrawTexture(new Rect((float)0, (float)0, (float)Screen.width, (float)Screen.height), this.hitTexture);
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000099F4 File Offset: 0x00007BF4
	public virtual void Main()
	{
	}

	// Token: 0x04000190 RID: 400
	public Transform target;

	// Token: 0x04000191 RID: 401
	public float distance;

	// Token: 0x04000192 RID: 402
	public float height;

	// Token: 0x04000193 RID: 403
	public float heightDamping;

	// Token: 0x04000194 RID: 404
	public float rotationDamping;

	// Token: 0x04000195 RID: 405
	public float occludedDistance;

	// Token: 0x04000196 RID: 406
	public float odr;

	// Token: 0x04000197 RID: 407
	public float hitAlpha;

	// Token: 0x04000198 RID: 408
	public Texture hitTexture;
}
