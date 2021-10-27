using System;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class CarCameras : MonoBehaviour
{
	// Token: 0x06000ACB RID: 2763 RVA: 0x00080460 File Offset: 0x0007E660
	private void Start()
	{
		this.mTransform = base.transform;
		this.mtarget = this.target;
		this.occludedDistance = this.distance;
		this.mask = 1 << LayerMask.NameToLayer("wall");
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0008049C File Offset: 0x0007E69C
	public void SetCamera(int index)
	{
		this.mouseOrbit = false;
		this.mapCamera = false;
		if (index == 0 && this.target != null)
		{
			this.locked = false;
			this.mtarget = this.target;
			this.rotationDamping = 3f;
			if (this.mtarget.name == "GMC Sierra Grande Fall Guy")
			{
				this.distance = 7f;
				this.height = 3f;
			}
			if (this.mtarget.name == "Truck Cabin")
			{
				this.distance = 17f;
				this.height = 10f;
			}
			else
			{
				this.distance = 6.5f;
				this.height = 1.9f;
			}
			this.yawAngle = 0f;
			this.pitchAngle = -0.25f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
		else if (index == 1 && this.target != null)
		{
			this.locked = true;
			foreach (object obj in this.target)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.tag == "Main_Camera_OnBoard" || transform.gameObject.name == "Main_Camera_OnBoard")
				{
					this.mtarget = transform;
				}
			}
			this.distance = 0f;
			this.height = 0f;
			this.pitchAngle = 0f;
			this.yawAngle = 0f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = false;
			}
		}
		else if (index == 2 && this.target != null)
		{
			this.locked = false;
			this.mtarget = this.target;
			this.rotationDamping = 3f;
			this.distance = 9f;
			this.height = 4f;
			this.pitchAngle = -1.5f;
			this.yawAngle = 0f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
		else if (index == 3 && this.target != null)
		{
			this.locked = true;
			foreach (object obj2 in this.target)
			{
				Transform transform2 = (Transform)obj2;
				if (transform2.gameObject.tag == "Main_Camera_WheelRL" || transform2.gameObject.name == "Main_Camera_WheelRL")
				{
					this.mtarget = transform2;
				}
			}
			this.distance = 0f;
			this.height = 0f;
			this.pitchAngle = 0f;
			this.yawAngle = 0f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
		else if (index == 4 && this.target != null)
		{
			this.locked = true;
			foreach (object obj3 in this.target)
			{
				Transform transform3 = (Transform)obj3;
				if (transform3.gameObject.tag == "Main_Camera_Bottom_Right" || transform3.gameObject.name == "Main_Camera_Bottom_Right")
				{
					this.mtarget = transform3;
				}
			}
			this.distance = 0f;
			this.height = 0f;
			this.pitchAngle = 0f;
			this.yawAngle = 0f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
		else if (index == 5 && this.target != null)
		{
			this.locked = false;
			this.mtarget = this.target;
			this.distance = 5f;
			this.height = 2f;
			this.pitchAngle = -2f;
			this.yawAngle = 90f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
		else if (index == 6 && this.target != null)
		{
			this.mouseOrbit = true;
			this.mtarget = this.target;
			if (this.distance < this.distanceMin || this.distance > this.distanceMax)
			{
				this.distance = 5f;
			}
			this.angles = this.mTransform.eulerAngles;
			this.x = this.angles.y;
			this.y = this.angles.x;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
		else if (index == 7 && this.target != null)
		{
			this.mapCamera = true;
			this.mtarget = this.target;
			this.distance = 0f;
			this.height = 0f;
			this.pitchAngle = 0f;
			this.yawAngle = 0f;
			if (this.playerFace != null)
			{
				this.playerFace.enabled = true;
			}
		}
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00080AD0 File Offset: 0x0007ECD0
	private void LateUpdate()
	{
		if (this.mtarget)
		{
			if (this.i != 1)
			{
				if ((double)Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal2")) > 0.2)
				{
					this.yawAngle += Input.GetAxis("Mouse X") * 5f + Input.GetAxis("Horizontal2") * 10f;
				}
				else
				{
					this.yawAngle = Mathf.SmoothDamp(this.yawAngle, 0f, ref this.currentYaw, 0.2f);
				}
				this.yawAngle = CarCameras.ClampAngle(this.yawAngle, -this.lookArroundAngle, this.lookArroundAngle);
			}
			if (this.mouseOrbit)
			{
				this.x += Input.GetAxis("Mouse X") * this.xSpeed;
				this.y -= Input.GetAxis("Mouse Y") * this.ySpeed;
				this.y = CarCameras.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Slerp(this.mTransform.rotation, Quaternion.Euler(this.y, this.x, 0f), Time.deltaTime * this.damping);
				this.distance -= Input.GetAxis("Mouse ScrollWheel") * 5f;
				this.distance = Mathf.Clamp(this.distance, this.distanceMin, this.distanceMax);
				Vector3 point = new Vector3(0f, 0f, -this.distance);
				Vector3 position = rotation * point + this.mtarget.position;
				this.mTransform.rotation = rotation;
				this.mTransform.position = position;
			}
			else if (this.mapCamera)
			{
				this.mTransform.position = new Vector3(this.mtarget.position.x, this.mTransform.position.y, this.mtarget.position.z);
				this.mTransform.eulerAngles = new Vector3(this.mTransform.eulerAngles.x, this.mtarget.eulerAngles.y, this.mTransform.eulerAngles.z);
			}
			else
			{
				this.wantedYawRotationAngle = this.mtarget.eulerAngles.y + this.yawAngle;
				this.currentYawRotationAngle = this.mTransform.eulerAngles.y;
				this.currentYawRotationAngle = Mathf.LerpAngle(this.currentYawRotationAngle, this.wantedYawRotationAngle, this.rotationDamping * Time.deltaTime);
				Quaternion rotation2 = Quaternion.Euler(0f, this.currentYawRotationAngle, 0f);
				Vector3 end = this.mtarget.position - rotation2 * Vector3.forward * (this.distance + 0.3f);
				Vector3 position2 = this.mtarget.position;
				position2.y += 1f;
				RaycastHit raycastHit;
				if (Physics.Linecast(position2, end, out raycastHit, this.mask))
				{
					this.occludedDistance = raycastHit.distance - 0.3f;
				}
				else
				{
					this.occludedDistance = Mathf.SmoothDamp(this.occludedDistance, this.distance, ref this.odr, 1f);
				}
				this.mTransform.position = this.mtarget.position;
				this.mTransform.position -= rotation2 * Vector3.forward * this.occludedDistance;
				if (this.locked)
				{
					this.mTransform.position = this.mtarget.position;
					this.mTransform.eulerAngles = new Vector3(this.mtarget.eulerAngles.x, this.mtarget.eulerAngles.y + this.yawAngle, this.mtarget.eulerAngles.z);
				}
				else
				{
					float num = this.mtarget.position.y + this.height;
					this.mTransform.position = new Vector3(this.mTransform.position.x, num, this.mTransform.position.z);
				}
				this.mTransform.LookAt(new Vector3(this.mtarget.position.x, this.mtarget.position.y + this.height + this.pitchAngle, this.mtarget.position.z));
			}
		}
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00080FD8 File Offset: 0x0007F1D8
	private void Update()
	{
		if (base.transform.gameObject.tag != "MapCamera" && base.transform.gameObject.name != "MapCamera" && (Input.GetKeyDown(KeyCode.Tab) || InputManager.GetButtonDown("Interaction") || MobileCarController.changeCamera))
		{
			this.i++;
			if (this.i == this.Cameras.Length)
			{
				this.i = 0;
			}
			this.SetCamera(this.i);
			MobileCarController.changeCamera = false;
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00081084 File Offset: 0x0007F284
	private static float ClampAngle(float yawAngle, float min, float max)
	{
		if (yawAngle < -360f)
		{
			yawAngle += 360f;
		}
		if (yawAngle > 360f)
		{
			yawAngle -= 360f;
		}
		return Mathf.Clamp(yawAngle, min, max);
	}

	// Token: 0x040011C6 RID: 4550
	private string[] Cameras = new string[]
	{
		"external",
		"onboard",
		"flybird"
	};

	// Token: 0x040011C7 RID: 4551
	public Transform target;

	// Token: 0x040011C8 RID: 4552
	private Transform mtarget;

	// Token: 0x040011C9 RID: 4553
	public float distance = 10f;

	// Token: 0x040011CA RID: 4554
	public float height = 2.5f;

	// Token: 0x040011CB RID: 4555
	public float yawAngle;

	// Token: 0x040011CC RID: 4556
	public float pitchAngle = -2.5f;

	// Token: 0x040011CD RID: 4557
	private float rotationDamping = 3f;

	// Token: 0x040011CE RID: 4558
	private bool locked;

	// Token: 0x040011CF RID: 4559
	private bool mapCamera;

	// Token: 0x040011D0 RID: 4560
	private bool mouseOrbit;

	// Token: 0x040011D1 RID: 4561
	private float xSpeed = 10f;

	// Token: 0x040011D2 RID: 4562
	private float ySpeed = 10f;

	// Token: 0x040011D3 RID: 4563
	private float yMinLimit = -20f;

	// Token: 0x040011D4 RID: 4564
	private float yMaxLimit = 80f;

	// Token: 0x040011D5 RID: 4565
	private float distanceMin = 4f;

	// Token: 0x040011D6 RID: 4566
	private float distanceMax = 30f;

	// Token: 0x040011D7 RID: 4567
	private float occludedDistance;

	// Token: 0x040011D8 RID: 4568
	private float odr;

	// Token: 0x040011D9 RID: 4569
	private float damping = 10f;

	// Token: 0x040011DA RID: 4570
	private Vector3 angles;

	// Token: 0x040011DB RID: 4571
	private float x;

	// Token: 0x040011DC RID: 4572
	private float y;

	// Token: 0x040011DD RID: 4573
	private int i;

	// Token: 0x040011DE RID: 4574
	private float currentYawRotationAngle;

	// Token: 0x040011DF RID: 4575
	private float wantedYawRotationAngle;

	// Token: 0x040011E0 RID: 4576
	public SkinnedMeshRenderer playerFace;

	// Token: 0x040011E1 RID: 4577
	private Transform mTransform;

	// Token: 0x040011E2 RID: 4578
	private int mask;

	// Token: 0x040011E3 RID: 4579
	private float currentYaw;

	// Token: 0x040011E4 RID: 4580
	public float lookArroundAngle = 90f;
}
