using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[RequireComponent(typeof(Rigidbody))]
[Serializable]
public class RollABall : MonoBehaviour
{
	// Token: 0x060000E7 RID: 231 RVA: 0x00005958 File Offset: 0x00003B58
	public RollABall()
	{
		this.tilt = Vector3.zero;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000596C File Offset: 0x00003B6C
	public virtual void Start()
	{
		this.circ = 6.2831855f * this.GetComponent<Collider>().bounds.extents.x;
		this.previousPosition = this.transform.position;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000059B4 File Offset: 0x00003BB4
	public virtual void Update()
	{
		this.tilt.x = -Input.acceleration.y;
		this.tilt.z = Input.acceleration.x;
		this.GetComponent<Rigidbody>().AddForce(this.tilt * this.speed * Time.deltaTime);
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00005A18 File Offset: 0x00003C18
	public virtual void LateUpdate()
	{
		Vector3 a = this.transform.position - this.previousPosition;
		a = new Vector3(a.z, (float)0, -a.x);
		this.transform.Rotate(a / this.circ * (float)360, Space.World);
		this.previousPosition = this.transform.position;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005A88 File Offset: 0x00003C88
	public virtual void Main()
	{
	}

	// Token: 0x040000A7 RID: 167
	public Vector3 tilt;

	// Token: 0x040000A8 RID: 168
	public float speed;

	// Token: 0x040000A9 RID: 169
	private float circ;

	// Token: 0x040000AA RID: 170
	private Vector3 previousPosition;
}
