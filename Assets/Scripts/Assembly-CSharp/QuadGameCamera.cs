using System;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class QuadGameCamera : MonoBehaviour
{
	// Token: 0x06000BAF RID: 2991 RVA: 0x00093AB4 File Offset: 0x00091CB4
	private void Start()
	{
		if (this.player == null)
		{
			this.player = GameObject.FindGameObjectWithTag("Player").transform;
		}
		Transform transform = this.player.Find("Bip01");
		if (transform != null)
		{
			this.pivotOffset.y = this.pivotOffset.y - (transform.position.y - this.player.position.y);
			this.player = transform;
		}
		this.resetCameraPosition();
		this.previousCamRotation = Quaternion.identity;
		this.mask = 1 << this.player.gameObject.layer;
		this.mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.mask |= 1 << LayerMask.NameToLayer("Enemy");
		this.mask = ~this.mask;
		this.cam = base.transform;
		this.smoothPlayerPos = this.player.position;
		this.maxCamDist = 3f;
		this.previousLookAtTarget = this.lookAtTarget;
		this.previousFixedCameraPosition = this.fixedCameraPosition;
		this.shakingCamera = true;
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00093C20 File Offset: 0x00091E20
	public void OnDestory()
	{
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x00093C24 File Offset: 0x00091E24
	public void resetCameraPosition()
	{
		float num = this.player.root.transform.eulerAngles.y;
		if (AnimationHandler.instance != null && AnimationHandler.instance.insuredMode)
		{
			num -= 90f;
		}
		while (num > 360f)
		{
			num -= 360f;
		}
		while (num < 0f)
		{
			num += 360f;
		}
		this.angleH = num;
		this.angleV = 0f;
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00093CB8 File Offset: 0x00091EB8
	public void LimitHorizontalAngle(float playerYRotation, bool leftCover)
	{
		if (this.minHorizontalAngle != 0f || this.maxHorizontalAngle != 0f)
		{
			return;
		}
		float num;
		if (leftCover)
		{
			num = playerYRotation + 30f;
		}
		else
		{
			num = playerYRotation - 30f;
		}
		while (num > 360f)
		{
			num -= 360f;
		}
		while (num < 0f)
		{
			num += 360f;
		}
		this.minHorizontalAngle = num;
		if (leftCover)
		{
			num += 180f;
		}
		else
		{
			num -= 180f;
		}
		this.maxHorizontalAngle = num;
		if (leftCover)
		{
			this.angleH = this.maxHorizontalAngle - 30f;
		}
		else
		{
			this.angleH = this.maxHorizontalAngle + 30f;
		}
		if (this.maxHorizontalAngle < this.minHorizontalAngle)
		{
			float num2 = this.minHorizontalAngle;
			this.minHorizontalAngle = this.maxHorizontalAngle;
			this.maxHorizontalAngle = num2;
		}
		this.previousminVerticalAngle = this.minVerticalAngle;
		this.previousmaxVerticalAngle = this.maxVerticalAngle;
		this.minVerticalAngle = -20f;
		this.maxVerticalAngle = 70f;
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00093DE4 File Offset: 0x00091FE4
	public void UnlimitHorizontalAngle()
	{
		this.minHorizontalAngle = 0f;
		this.maxHorizontalAngle = 0f;
		this.minVerticalAngle = this.previousminVerticalAngle;
		this.maxVerticalAngle = this.previousmaxVerticalAngle;
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00093E20 File Offset: 0x00092020
	private void LateUpdate()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (Time.deltaTime == 0f || Time.timeScale == 0f || this.player == null)
		{
			return;
		}
		float num = ShooterGameCamera.mouseSensitivity;
		if (this.lockTarget != null)
		{
			Vector3 direction = this.lockTarget.position + this.RandomSemiAssest - this.cam.position;
			direction = this.cam.InverseTransformDirection(direction).normalized;
			if (this.focusOnTarget)
			{
				if (Mathf.Abs(this.angleH - (this.angleH + direction.x)) < 0.01f && Mathf.Abs(this.angleV - (this.angleV + direction.y)) < 0.01f && ShooterGameCamera.aimAssestType == ShooterGameCamera.AimAssestTypes.SEMI)
				{
					this.focusOnTarget = false;
				}
				this.angleH += direction.x * 40f;
				this.angleV += direction.y * 40f;
			}
			else if (Mathf.Abs(this.angleH - (this.angleH + direction.x)) < 0.05f && Mathf.Abs(this.angleV - (this.angleV + direction.y)) < 0.05f)
			{
				num *= 0.4f;
			}
		}
		if (this.lockTarget == null)
		{
			this.SelectAimTarget();
		}
		int touchCount = Input.touchCount;
		if (touchCount > 0)
		{
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Moved)
				{
					this.mouseX = touch.deltaPosition.x * 0.1f;
					this.mouseY = touch.deltaPosition.y * 0.1f;
				}
			}
		}
		else
		{
			this.mouseX = 0f;
			this.mouseY = 0f;
		}
		for (int j = 0; j < touchCount; j++)
		{
			Touch touch2 = Input.GetTouch(j);
		}
		if (touchCount == 2)
		{
			QuadGameCamera.fire = true;
		}
		else
		{
			QuadGameCamera.fire = false;
		}
		this.angleH += this.mouseX * ShooterGameCamera.mouseSensitivity * 2f * this.horizontalAimingSpeed * (float)((!ShooterGameCamera.inverseX) ? 1 : -1) * Time.deltaTime;
		this.angleV -= this.mouseY * ShooterGameCamera.mouseSensitivity * 2f * this.verticalAimingSpeed * (float)((!ShooterGameCamera.inverseY) ? 1 : -1) * Time.deltaTime;
		this.angleH += Mathf.Clamp(Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal2"), -1f, 1f) * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? 1f : 4.76f) * num * this.horizontalAimingSpeed * (float)((!ShooterGameCamera.inverseX) ? 1 : -1) * Time.deltaTime;
		this.angleV -= Mathf.Clamp(Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical2"), -1f, 1f) * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? 1f : 4.76f) * num * this.verticalAimingSpeed * (float)((!ShooterGameCamera.inverseY) ? 1 : -1) * Time.deltaTime;
		this.angleV = Mathf.Clamp(this.angleV, this.minVerticalAngle, this.maxVerticalAngle);
		if (this.minHorizontalAngle != 0f || this.maxHorizontalAngle != 0f)
		{
			this.angleH = Mathf.Clamp(this.angleH, this.minHorizontalAngle, this.maxHorizontalAngle);
		}
		float magnitude = (this.aimTarget.position - this.cam.position).magnitude;
		Quaternion rotation = Quaternion.Euler(-this.angleV, this.angleH, 0f);
		Quaternion rotation2 = Quaternion.Euler(0f, this.angleH, 0f);
		if (this.previousCamRotation != Quaternion.identity)
		{
			this.cam.rotation = this.previousCamRotation;
			rotation = this.previousCamRotation;
			this.angleH = this.previousCamRotation.eulerAngles.y;
			this.angleV = -this.previousCamRotation.eulerAngles.x;
			while (this.angleV < -180f)
			{
				this.angleV += 360f;
			}
			while (this.angleV > 180f)
			{
				this.angleV -= 360f;
			}
			rotation2 = Quaternion.Euler(0f, this.previousCamRotation.eulerAngles.y, 0f);
			this.previousCamRotation = Quaternion.identity;
		}
		else
		{
			this.cam.rotation = rotation;
		}
		this.smoothPlayerPos = this.player.position;
		if (this.aim)
		{
			this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(0.5f, 0.9f, this.aimedZoom), ref this.refAim, 0.075f);
		}
		else
		{
			this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(0.5f, 1.1f, this.unaimedZoom), ref this.refAim, 0.15f);
		}
		Vector3 a = this.smoothPlayerPos + rotation2 * this.pivotOffset + rotation * this.camOffset;
		Vector3 vector = this.player.position + rotation2 * this.closeOffset;
		float num2 = Vector3.Distance(a, vector);
		this.maxCamDist = Mathf.SmoothDamp(this.maxCamDist, num2, ref this.refD, this.smoothingTime);
		Vector3 vector2 = (a - vector) / num2;
		float num3 = 0.3f;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, vector2, out raycastHit, this.maxCamDist + num3, this.mask))
		{
			this.maxCamDist = raycastHit.distance - num3;
		}
		if (this.fixedCameraPosition)
		{
			this.cam.position = Vector3.SmoothDamp(this.cam.position, this.cameraPosition.position, ref this.smoothPosition, this.inTransitionTime);
			Vector3 a2 = this.smoothPlayerPos;
			a2 += this.pivotOffset;
			this.cam.rotation = Quaternion.LookRotation(a2 - this.cam.position);
		}
		else if (this.previousFixedCameraPosition != this.fixedCameraPosition)
		{
			this.cameraPositionAdjustmentTimer = this.outTransitionTime + this.outTransitionTime * 0.75f;
		}
		else if (this.cameraPositionAdjustmentTimer > 0f)
		{
			this.resetCameraPosition();
			this.cameraPositionAdjustmentTimer -= Time.deltaTime;
			this.smoothedMaxCamDist = Mathf.SmoothDamp(this.smoothedMaxCamDist, this.maxCamDist, ref this.refD2, this.cameraPositionAdjustmentTimer);
			this.cam.position = Vector3.SmoothDamp(this.cam.position, vector + vector2 * this.smoothedMaxCamDist, ref this.smoothPosition, this.cameraPositionAdjustmentTimer);
			Vector3 a3 = this.smoothPlayerPos;
			a3 += this.pivotOffset;
			this.cam.rotation = Quaternion.LookRotation(a3 - this.cam.position);
		}
		else
		{
			if (!this.lookAtTarget && (this.previousLookAtTarget != this.lookAtTarget || this.specialCaseTimer > 0f))
			{
				if (this.previousLookAtTarget != this.lookAtTarget)
				{
					this.specialCaseTimer = 3f;
				}
				this.specialCaseTimer -= Time.deltaTime;
				this.smoothedMaxCamDist = Mathf.SmoothDamp(this.smoothedMaxCamDist, this.maxCamDist, ref this.refD2, 3f);
				this.cam.position = vector + vector2 * this.smoothedMaxCamDist;
			}
			if (!this.lookAtTarget)
			{
				this.cam.position = vector + vector2 * this.maxCamDist;
			}
			else
			{
				Vector3 a4 = this.smoothPlayerPos;
				a4 += this.pivotOffset;
				this.cam.rotation = Quaternion.LookRotation(a4 - this.cam.position);
				this.previousCamRotation = this.cam.rotation;
			}
		}
		float d;
		if (Physics.Raycast(this.cam.position, this.cam.forward, out raycastHit, 100f, this.mask))
		{
			d = raycastHit.distance + 0.05f;
		}
		else
		{
			d = Mathf.Max(5f, magnitude);
		}
		this.aimTarget.position = this.cam.position + this.cam.forward * d;
		this.previousLookAtTarget = this.lookAtTarget;
		this.previousFixedCameraPosition = this.fixedCameraPosition;
		this.HandleCameraShake();
		if (this.shakingCamera)
		{
			this.angleV += UnityEngine.Random.RandomRange(-0.25f, 0.3f);
			this.angleH += UnityEngine.Random.RandomRange(-0.3f, 0.25f);
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00094824 File Offset: 0x00092A24
	public void SelectAimTarget()
	{
		if (ShooterGameCamera.aimAssestType == ShooterGameCamera.AimAssestTypes.OFF)
		{
			return;
		}
		QBHealth[] array = (QBHealth[])UnityEngine.Object.FindObjectsOfType(typeof(QBHealth));
		Transform transform = null;
		foreach (QBHealth qbhealth in array)
		{
			if ((transform == null || Vector3.Distance(qbhealth.transform.position, this.player.position) < Vector3.Distance(transform.position, this.player.position)) && Mathf.Abs(Vector3.Angle(this.cam.forward, qbhealth.transform.position - this.player.position)) < 90f && qbhealth.tag != "Player")
			{
				Transform transform2 = qbhealth.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
				if (!Physics.Linecast(transform2.position, this.player.position, this.mask))
				{
					transform = transform2;
				}
				else
				{
					Transform transform3 = qbhealth.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
					if (!Physics.Linecast(transform3.position, this.player.position, this.mask))
					{
						transform = transform3;
					}
				}
			}
		}
		if (transform == null)
		{
			foreach (QBHealth qbhealth2 in array)
			{
				if ((transform == null || Vector3.Distance(qbhealth2.transform.position, this.player.position) < Vector3.Distance(transform.position, this.player.position)) && qbhealth2.tag != "Player")
				{
					Transform transform4 = qbhealth2.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
					if (!Physics.Linecast(transform4.position, this.player.position, this.mask))
					{
						transform = transform4;
					}
					else
					{
						Transform transform5 = qbhealth2.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
						if (!Physics.Linecast(transform5.position, this.player.position, this.mask))
						{
							transform = transform5;
						}
					}
				}
			}
		}
		if (transform != null)
		{
			this.lockTarget = transform;
			if (ShooterGameCamera.aimAssestType == ShooterGameCamera.AimAssestTypes.SEMI)
			{
				this.RandomSemiAssest = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
			}
			else
			{
				this.RandomSemiAssest = Vector3.zero;
			}
			this.focusOnTarget = true;
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00094AF4 File Offset: 0x00092CF4
	private void OnGUI()
	{
		if (mainmenu.pause)
		{
			return;
		}
		Color color2;
		Color color = color2 = GUI.color;
		color.a = this.hitAlpha;
		GUI.color = color;
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.hitTexture);
		color.a = this.blackAlpha;
		GUI.color = color;
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackTexture);
		GUI.color = color2;
		if (Time.time != 0f && Time.timeScale != 0f && this.reticle != null)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)this.reticle.width * 0.5f, (float)(Screen.height / 2) - (float)this.reticle.height * 0.5f, (float)this.reticle.width, (float)this.reticle.height), this.reticle);
		}
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00094C1C File Offset: 0x00092E1C
	private void HandleCameraShake()
	{
		if (this.shake)
		{
			this.cShake += this.cShakeSpeed * Time.deltaTime;
			this.cShakeSpeed *= -1f;
			this.cShakeTimes++;
			if (this.cShakeTimes >= this.shakeTimes)
			{
				this.shake = false;
			}
			if (this.cShake < 0f)
			{
				this.cShake = this.maxShake;
			}
			else
			{
				this.cShake = -this.maxShake;
			}
			this.angleV += this.cShake;
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00094CC8 File Offset: 0x00092EC8
	public void StartShake(float distance)
	{
		float num = distance / this.maxShakeDistance;
		if (num > 1f)
		{
			return;
		}
		num = Mathf.Clamp(num, 0f, 1f);
		num = 1f - num;
		this.cShakeSpeed = Mathf.Lerp(this.minShakeSpeed, this.maxShakeSpeed, num);
		this.shakeTimes = (int)Mathf.Lerp((float)this.minShakeTimes, (float)this.maxShakeTimes, num);
		this.cShakeTimes = 0;
		this.cShakePos = Mathf.Lerp(this.minShake, this.maxShake, num);
		this.shake = true;
	}

	// Token: 0x0400155A RID: 5466
	public Transform player;

	// Token: 0x0400155B RID: 5467
	public Transform aimTarget;

	// Token: 0x0400155C RID: 5468
	public float smoothingTime = 0.15f;

	// Token: 0x0400155D RID: 5469
	public Vector3 pivotOffset = new Vector3(1.3f, 0.4f, 0f);

	// Token: 0x0400155E RID: 5470
	public Vector3 camOffset = new Vector3(0f, 0.7f, -2.4f);

	// Token: 0x0400155F RID: 5471
	public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0f);

	// Token: 0x04001560 RID: 5472
	public float horizontalAimingSpeed = 270f;

	// Token: 0x04001561 RID: 5473
	public float verticalAimingSpeed = 270f;

	// Token: 0x04001562 RID: 5474
	public float maxVerticalAngle = 80f;

	// Token: 0x04001563 RID: 5475
	public float minVerticalAngle = -80f;

	// Token: 0x04001564 RID: 5476
	public Texture reticle;

	// Token: 0x04001565 RID: 5477
	public Texture2D hitTexture;

	// Token: 0x04001566 RID: 5478
	public Texture2D blackTexture;

	// Token: 0x04001567 RID: 5479
	private float angleH;

	// Token: 0x04001568 RID: 5480
	private float angleV;

	// Token: 0x04001569 RID: 5481
	private Transform cam;

	// Token: 0x0400156A RID: 5482
	private float maxCamDist = 1f;

	// Token: 0x0400156B RID: 5483
	private LayerMask mask;

	// Token: 0x0400156C RID: 5484
	private Vector3 smoothPlayerPos;

	// Token: 0x0400156D RID: 5485
	[HideInInspector]
	public bool lookAtTarget;

	// Token: 0x0400156E RID: 5486
	private bool previousLookAtTarget;

	// Token: 0x0400156F RID: 5487
	private Quaternion previousCamRotation;

	// Token: 0x04001570 RID: 5488
	private float refX;

	// Token: 0x04001571 RID: 5489
	private float refY;

	// Token: 0x04001572 RID: 5490
	private float refZ;

	// Token: 0x04001573 RID: 5491
	private float refD;

	// Token: 0x04001574 RID: 5492
	private float refD2;

	// Token: 0x04001575 RID: 5493
	private float smoothedMaxCamDist = 1f;

	// Token: 0x04001576 RID: 5494
	private float specialCaseTimer;

	// Token: 0x04001577 RID: 5495
	[HideInInspector]
	public bool fixedCameraPosition;

	// Token: 0x04001578 RID: 5496
	private bool previousFixedCameraPosition;

	// Token: 0x04001579 RID: 5497
	private float cameraPositionAdjustmentTimer;

	// Token: 0x0400157A RID: 5498
	[HideInInspector]
	public Transform cameraPosition;

	// Token: 0x0400157B RID: 5499
	public float inTransitionTime = 1f;

	// Token: 0x0400157C RID: 5500
	public float outTransitionTime = 1f;

	// Token: 0x0400157D RID: 5501
	private Vector3 smoothPosition;

	// Token: 0x0400157E RID: 5502
	public bool allowCircleCamera = true;

	// Token: 0x0400157F RID: 5503
	[HideInInspector]
	public float hitAlpha;

	// Token: 0x04001580 RID: 5504
	[HideInInspector]
	public float blackAlpha;

	// Token: 0x04001581 RID: 5505
	private float minHorizontalAngle;

	// Token: 0x04001582 RID: 5506
	private float maxHorizontalAngle;

	// Token: 0x04001583 RID: 5507
	private float previousminVerticalAngle;

	// Token: 0x04001584 RID: 5508
	private float previousmaxVerticalAngle;

	// Token: 0x04001585 RID: 5509
	private float minShakeSpeed;

	// Token: 0x04001586 RID: 5510
	private float maxShakeSpeed;

	// Token: 0x04001587 RID: 5511
	private float minShake = 10f;

	// Token: 0x04001588 RID: 5512
	private float maxShake = 10f;

	// Token: 0x04001589 RID: 5513
	private int minShakeTimes = 3;

	// Token: 0x0400158A RID: 5514
	private int maxShakeTimes = 8;

	// Token: 0x0400158B RID: 5515
	private float maxShakeDistance = 20f;

	// Token: 0x0400158C RID: 5516
	private bool shake;

	// Token: 0x0400158D RID: 5517
	private float shakeSpeed = 2f;

	// Token: 0x0400158E RID: 5518
	private float cShakePos;

	// Token: 0x0400158F RID: 5519
	private int shakeTimes = 8;

	// Token: 0x04001590 RID: 5520
	private float cShake;

	// Token: 0x04001591 RID: 5521
	private float cShakeSpeed;

	// Token: 0x04001592 RID: 5522
	private int cShakeTimes;

	// Token: 0x04001593 RID: 5523
	public bool shakingCamera;

	// Token: 0x04001594 RID: 5524
	private float mouseX;

	// Token: 0x04001595 RID: 5525
	private float mouseY;

	// Token: 0x04001596 RID: 5526
	public static bool fire;

	// Token: 0x04001597 RID: 5527
	public bool aim;

	// Token: 0x04001598 RID: 5528
	public float unaimedZoom = -10f;

	// Token: 0x04001599 RID: 5529
	public float aimedZoom = -3.25f;

	// Token: 0x0400159A RID: 5530
	private Vector3 refAim;

	// Token: 0x0400159B RID: 5531
	public Transform lockTarget;

	// Token: 0x0400159C RID: 5532
	public bool focusOnTarget;

	// Token: 0x0400159D RID: 5533
	private Vector3 RandomSemiAssest;

	// Token: 0x0400159E RID: 5534
	public Texture moveCurser;

	// Token: 0x0400159F RID: 5535
	private Vector3 previousMoveControllerPosition;

	// Token: 0x040015A0 RID: 5536
	private static int sphereVisible = 1;

	// Token: 0x040015A1 RID: 5537
	private bool cameraConnected = true;

	// Token: 0x040015A2 RID: 5538
	public Texture sphereNotVisibleTex;
}
