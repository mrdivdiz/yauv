using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class CSB_ScrollInScript : MonoBehaviour
{
	// Token: 0x0600019D RID: 413 RVA: 0x0000C238 File Offset: 0x0000A438
	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000C288 File Offset: 0x0000A488
	private void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			this.distance = Mathf.Min(Mathf.Max(this.distance - Input.GetAxis("Mouse ScrollWheel"), this.MinDist), this.MaxDist);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.rotateMe = !this.rotateMe;
		}
		if (this.rotateMe)
		{
			base.transform.RotateAround(this.target.transform.position, new Vector3(1f, 0f, 0f), this.OnKeyRotation.x * Time.deltaTime);
			base.transform.RotateAround(this.target.transform.position, new Vector3(0f, 1f, 0f), this.OnKeyRotation.y * Time.deltaTime);
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.z = 0f;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
	private void LateUpdate()
	{
		if (this.target && (Input.GetMouseButton(0) || !this.started || Input.GetAxis("Mouse ScrollWheel") != 0f) && Input.mousePosition.y < (float)(Screen.height - 70))
		{
			this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.y = CSB_ScrollInScript.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
			Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
			Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.target.position;
			base.transform.rotation = rotation;
			base.transform.position = position;
			this.started = true;
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000C4DC File Offset: 0x0000A6DC
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x040001AB RID: 427
	public float MinDist = 1.3f;

	// Token: 0x040001AC RID: 428
	public float MaxDist = 8f;

	// Token: 0x040001AD RID: 429
	public Transform target;

	// Token: 0x040001AE RID: 430
	public float distance = 10f;

	// Token: 0x040001AF RID: 431
	public float xSpeed = 250f;

	// Token: 0x040001B0 RID: 432
	public float ySpeed = 120f;

	// Token: 0x040001B1 RID: 433
	public float yMinLimit = -20f;

	// Token: 0x040001B2 RID: 434
	public float yMaxLimit = 80f;

	// Token: 0x040001B3 RID: 435
	private float x;

	// Token: 0x040001B4 RID: 436
	private float y;

	// Token: 0x040001B5 RID: 437
	private bool started;

	// Token: 0x040001B6 RID: 438
	public Vector3 OnKeyRotation;

	// Token: 0x040001B7 RID: 439
	public bool rotateMe;
}
