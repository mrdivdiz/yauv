using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class ShooterGameCamera : MonoBehaviour
{
	// Token: 0x0600062E RID: 1582 RVA: 0x0002D068 File Offset: 0x0002B268
	private void Start()
	{
		if (this.player == null)
		{
			//this.player = GameObject.FindGameObjectWithTag("Player").transform;
		}
		this.player = GameObject.FindGameObjectWithTag("Player").transform;
		this.cc = this.player.root.GetComponent<CharacterController>();
		Transform transform = this.player.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
		if (transform != null)
		{
			this.pivotOffset.y = this.pivotOffset.y - (transform.position.y - this.player.position.y);
			this.player = transform;
		}
		this.playerRenderers = this.player.root.GetComponentsInChildren<Renderer>();
		this.resetCameraPosition();
		this.previousCamRotation = Quaternion.identity;
		this.mask = 1 << this.player.gameObject.layer;
		this.mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.mask |= 1 << LayerMask.NameToLayer("wall");
		this.mask |= 1 << LayerMask.NameToLayer("Enemy");
		this.mask = ~this.mask;
		this.cam = base.transform;
		this.smoothPlayerPos = this.player.position;
		this.maxCamDist = 3f;
		this.previousLookAtTarget = this.lookAtTarget;
		this.previousFixedCameraPosition = this.fixedCameraPosition;
		if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.4f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.2f)
		{
			this.shootingCamOffsetX = 0.5f;
		}
		else if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.6f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.4f)
		{
			this.shootingCamOffsetX = 0.58f;
		}
		if(prologVsrat){
		StartCoroutine(VsratoProlog());
		}
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0002D2E4 File Offset: 0x0002B4E4
	public void OnDestory()
	{
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0002D2E8 File Offset: 0x0002B4E8
	public void resetCameraPosition()
	{
		if (this.player == null)
		{
			//this.player = GameObject.FindGameObjectWithTag("Player").transform;
			Transform transform = this.player.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
			if (transform != null)
			{
				this.pivotOffset.y = this.pivotOffset.y - (transform.position.y - this.player.position.y);
				this.player = transform;
			}
		}
		float num;
		if (AnimationHandler.instance != null && AnimationHandler.instance.insuredMode)
		{
			num = this.player.root.eulerAngles.y + 90f;
		}
		else
		{
			num = this.player.transform.eulerAngles.y + 90f;
		}
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

	// Token: 0x06000631 RID: 1585 RVA: 0x0002D440 File Offset: 0x0002B640
	public void resetCameraMeleePosition()
	{
		if (this.player == null)
		{
			//this.player = GameObject.FindGameObjectWithTag("Player").transform;
			Transform transform = this.player.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
			if (transform != null)
			{
				this.pivotOffset.y = this.pivotOffset.y - (transform.position.y - this.player.position.y);
				this.player = transform;
			}
		}
		float num;
		if (Application.loadedLevelName == "part2")
		{
			num = this.player.root.eulerAngles.y - 90f;
		}
		else if (Application.loadedLevelName == "Chase1")
		{
			num = this.player.root.eulerAngles.y + 130f;
		}
		else
		{
			num = this.player.root.eulerAngles.y;
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

	// Token: 0x06000632 RID: 1586 RVA: 0x0002D59C File Offset: 0x0002B79C
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
		if (this.maxHorizontalAngle < this.minHorizontalAngle)
		{
			float num2 = this.minHorizontalAngle;
			this.minHorizontalAngle = this.maxHorizontalAngle;
			this.maxHorizontalAngle = num2;
		}
		while (this.angleH > this.maxHorizontalAngle)
		{
			this.angleH -= 360f;
		}
		while (this.angleH < this.minHorizontalAngle)
		{
			this.angleH += 360f;
		}
		this.previousminVerticalAngle = this.minVerticalAngle;
		this.previousmaxVerticalAngle = this.maxVerticalAngle;
		this.minVerticalAngle = -20f;
		this.maxVerticalAngle = 70f;
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
	public void UnlimitHorizontalAngle()
	{
		this.minHorizontalAngle = 0f;
		this.maxHorizontalAngle = 0f;
		this.minVerticalAngle = this.previousminVerticalAngle;
		this.maxVerticalAngle = this.previousmaxVerticalAngle;
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
	private void LateUpdate()
	{
		if (mainmenu.pause && !this.hintFocus)
		{
			return;
		}
		if (Time.deltaTime == 0f || Time.timeScale == 0f || this.player == null)
		{
			return;
		}
		if (this.player != null && this.cc != null && (double)this.cc.velocity.magnitude > 1.0 && !this.aim && !this.shoot)
		{
			//this.angleV = Mathf.SmoothDamp(this.angleV, -5f, ref this.refVAng, 0.5f);
		}
		if (this.recoil > 0f)
		{
			//this.angleV += this.recoil;
			this.recoil = -this.recoil;
			this.lastRecoilTime = Time.time;
		}
		if (this.recoil < 0f && Time.time > this.lastRecoilTime + 0.25f)
		{
			//this.angleV += this.recoil;
			this.recoil = 0f;
		}
		float num = ShooterGameCamera.mouseSensitivity;
		if (this.lockTarget != null && !this.hintFocus && Interaction.playerInteraction != null && Interaction.playerInteraction.weaponHandler != null && (Interaction.playerInteraction.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED || Interaction.playerInteraction.weaponHandler.status == WeaponHandling.WeaponStatus.ENGAGED))
		{
			this.lockTarget = null;
		}
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
				this.angleH += direction.x * 20f;
				//this.angleV += direction.y * 20f;
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
		if (!this.meleeCamera)
		{
			this.angleH += (MobileInput.mouseX + Mathf.Clamp(Input.GetAxis("Horizontal2"), -1f, 1f) * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? 1f : 4.76f)) * num * this.horizontalAimingSpeed * (float)((!ShooterGameCamera.inverseX) ? 1 : -1) * Time.deltaTime;
			this.angleV -= (MobileInput.mouseY + Mathf.Clamp(Input.GetAxis("Vertical2"), -1f, 1f) * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? 1f : 4.76f)) * num * this.verticalAimingSpeed * (float)((!ShooterGameCamera.inverseY) ? 1 : -1) * Time.deltaTime;
		}
		else
		{
			this.angleH += (MeleeMobileInput.mouseX + Mathf.Clamp(Input.GetAxis("Horizontal2"), -1f, 1f) * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? 1f : 4.76f)) * num * this.horizontalAimingSpeed * (float)((!ShooterGameCamera.inverseX) ? 1 : -1) * Time.deltaTime;
			this.angleV -= (MeleeMobileInput.mouseY + Mathf.Clamp(Input.GetAxis("Vertical2"), -1f, 1f) * ((AndroidPlatform.platform != AndroidPlatform.AndroidPlatforms.IOS) ? 1f : 4.76f)) * num * this.verticalAimingSpeed * (float)((!ShooterGameCamera.inverseY) ? 1 : -1) * Time.deltaTime;
		}
		if (!this.meleeCamera)
		{
			if (this.shortCoverY == 0f)
			{
				//this.angleV = Mathf.Clamp(this.angleV, this.minVerticalAngle, this.maxVerticalAngle);
			}
			else
			{
				//this.angleV = Mathf.Clamp(this.angleV, -15f, 30f);
			}
		}
		else
		{
			//this.angleV = Mathf.Clamp(this.angleV, -18f, -18f);
		}
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
		if (!this.aim || this.coverOffset == 0f)
		{
			if (!this.meleeCamera)
			{
				this.smoothPlayerPos.x = Mathf.SmoothDamp(this.smoothPlayerPos.x, this.player.position.x, ref this.refX, 0.075f);
				this.smoothPlayerPos.y = Mathf.SmoothDamp(this.smoothPlayerPos.y, this.player.position.y, ref this.refY, 0.1f);
				this.smoothPlayerPos.z = Mathf.SmoothDamp(this.smoothPlayerPos.z, this.player.position.z, ref this.refZ, 0.075f);
			}
			else
			{
				if (this.previousMeleeCamera != this.meleeCamera)
				{
					this.resetCameraMeleePosition();
					this.smoothPlayerPos.x = this.player.position.x;
					this.smoothPlayerPos.y = this.player.position.y;
					this.smoothPlayerPos.z = this.player.position.z;
					this.previousMeleeCamera = this.meleeCamera;
				}
				this.smoothPlayerPos.x = Mathf.SmoothDamp(this.smoothPlayerPos.x, this.player.position.x, ref this.refX, 1f);
				this.smoothPlayerPos.y = Mathf.SmoothDamp(this.smoothPlayerPos.y, this.player.position.y, ref this.refY, 0.1f);
				this.smoothPlayerPos.z = Mathf.SmoothDamp(this.smoothPlayerPos.z, this.player.position.z, ref this.refZ, 1f);
			}
		}
		if (this.takeDown)
		{
			this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(0f, this.camOffset.y, this.takeDownZoom), ref this.refAim, 0.075f);
		}
		else if (this.aim)
		{
			if (this.coverOffset != 0f)
			{
				if (this.inversedAiming)
				{
					this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(-this.shootingCamOffsetX, this.camOffset.y, this.aimedZoom), ref this.refAim, 0.075f);
					this.pivotOffset = new Vector3(-this.shootingCamOffsetX, 0f, 0f);
				}
				else
				{
					this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(this.shootingCamOffsetX, this.camOffset.y, this.aimedZoom), ref this.refAim, 0.075f);
					this.pivotOffset = new Vector3(this.shootingCamOffsetX, 0f, 0f);
				}
			}
			else
			{
				if (this.inversedAiming)
				{
					this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(-this.shootingCamOffsetX, this.camOffset.y, this.aimedZoom), ref this.refAim, 0.075f);
				}
				else
				{
					this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(this.shootingCamOffsetX, this.camOffset.y, this.aimedZoom), ref this.refAim, 0.075f);
				}
				this.pivotOffset = new Vector3(0f, 0f, 0f);
			}
		}
		else if (this.shoot && this.coverOffset == 0f && this.shortCoverY == 0f)
		{
			if (this.inversedAiming)
			{
				this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(-this.shootingCamOffsetX, this.camOffset.y, this.unaimedZoom), ref this.refAim, 0.075f);
			}
			else
			{
				this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(this.shootingCamOffsetX, this.camOffset.y, this.unaimedZoom), ref this.refAim, 0.075f);
			}
			this.pivotOffset = new Vector3(0f, 0f, 0f);
		}
		else
		{
			this.camOffset = Vector3.SmoothDamp(this.camOffset, new Vector3(this.coverOffset, this.camOffset.y, this.unaimedZoom), ref this.refAim, 0.15f);
			this.pivotOffset = new Vector3(this.coverOffset, this.shortCoverY, 0f);
		}
		Vector3 a = this.smoothPlayerPos + rotation2 * this.pivotOffset + rotation * this.camOffset;
		Vector3 vector = this.smoothPlayerPos + rotation2 * this.closeOffset;
		float d = Vector3.Distance(a, vector);
		this.maxCamDist = d;
		Vector3 vector2 = (a - vector) / d;
		float num2 = 0.3f;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, vector2, out raycastHit, this.maxCamDist + num2, this.mask))
		{
			this.maxCamDist = raycastHit.distance - num2;
		}
		if (this.fixedCameraPosition)
		{
			if (this.cameraPosition != null)
			{
				this.cam.position = Vector3.SmoothDamp(this.cam.position, this.cameraPosition.position, ref this.smoothPosition, this.inTransitionTime);
			}
			Vector3 a2 = this.smoothPlayerPos;
			a2 += this.pivotOffset;
			this.cam.rotation = Quaternion.LookRotation(a2 - this.cam.position);
		}
		else if (this.previousFixedCameraPosition != this.fixedCameraPosition)
		{
			this.cameraPositionAdjustmentTimer = this.outTransitionTime + this.outTransitionTime * 0.75f;
			Vector3 a3 = this.smoothPlayerPos;
			a3 += this.pivotOffset;
			this.cam.rotation = Quaternion.LookRotation(a3 - this.cam.position);
		}
		else if (this.cameraPositionAdjustmentTimer > 0f)
		{
			this.resetCameraPosition();
			this.cameraPositionAdjustmentTimer -= Time.deltaTime;
			this.smoothedMaxCamDist = Mathf.SmoothDamp(this.smoothedMaxCamDist, this.maxCamDist, ref this.refD2, this.cameraPositionAdjustmentTimer);
			this.cam.position = Vector3.SmoothDamp(this.cam.position, vector + vector2 * this.smoothedMaxCamDist, ref this.smoothPosition, this.cameraPositionAdjustmentTimer);
			Vector3 a4 = this.smoothPlayerPos;
			a4 += this.pivotOffset;
			this.cam.rotation = Quaternion.LookRotation(a4 - this.cam.position);
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
			else if (this.angleV > 10f)
			{
				this.cam.position = vector + vector2 * this.maxCamDist;
			}
			else
			{
				Vector3 a5 = this.smoothPlayerPos;
				a5 += this.pivotOffset;
				this.cam.rotation = Quaternion.LookRotation(a5 - this.cam.position);
				this.previousCamRotation = this.cam.rotation;
			}
		}
		float d2;
		if (Physics.Raycast(this.cam.position, this.cam.forward, out raycastHit, 100f, this.mask))
		{
			d2 = raycastHit.distance + 0.05f;
		}
		else
		{
			d2 = Mathf.Max(5f, magnitude);
		}
		this.aimTarget.position = this.cam.position + this.cam.forward * d2;
		this.previousLookAtTarget = this.lookAtTarget;
		this.previousFixedCameraPosition = this.fixedCameraPosition;
		this.HandleCameraShake();
		if (this.shakingCamera)
		{
			this.angleV += UnityEngine.Random.Range(-0.5f, 0.5f);
			this.angleH += UnityEngine.Random.Range(-0.5f, 0.5f);
		}
		if (Vector3.Distance(base.transform.position, this.player.position) < 0.36f)
		{
			if (!this.hidePlayer)
			{
				foreach (Renderer renderer in this.playerRenderers)
				{
					if (renderer != null)
					{
						renderer.enabled = false;
					}
				}
				this.hidePlayer = true;
			}
		}
		else if (this.hidePlayer)
		{
			foreach (Renderer renderer2 in this.playerRenderers)
			{
				if (renderer2 != null)
				{
					renderer2.enabled = true;
				}
			}
			GunManager componentInChildren = this.player.root.GetComponentInChildren<GunManager>();
			if (componentInChildren != null)
			{
				componentInChildren.HideUnusedWeapons();
			}
			this.hidePlayer = false;
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0002E814 File Offset: 0x0002CA14
	public void SelectAimTarget()
	{
		if (ShooterGameCamera.aimAssestType == ShooterGameCamera.AimAssestTypes.OFF || (ShooterGameCamera.aimAssestType == ShooterGameCamera.AimAssestTypes.SEMI && this.inCover) || (Interaction.playerInteraction != null && Interaction.playerInteraction.weaponHandler != null && (Interaction.playerInteraction.weaponHandler.status == WeaponHandling.WeaponStatus.RELAXED || Interaction.playerInteraction.weaponHandler.status == WeaponHandling.WeaponStatus.ENGAGED)))
		{
			return;
		}
		if (this.prioritizedAimTarget != null)
		{
			this.lockTarget = this.prioritizedAimTarget;
			this.focusOnTarget = true;
			this.RandomSemiAssest = Vector3.zero;
			return;
		}
		Transform transform = null;
		BotAI[] array = (BotAI[])UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
		foreach (BotAI botAI in array)
		{
			if ((transform == null || Vector3.Distance(botAI.transform.position, this.player.position) < Vector3.Distance(transform.position, this.player.position)) && Mathf.Abs(Vector3.Angle(this.cam.forward, botAI.transform.position - this.player.position)) < 90f)
			{
				Transform transform2 = botAI.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
				if (!Physics.Linecast(transform2.position, this.player.position, this.mask))
				{
					transform = transform2;
				}
				else
				{
					Transform transform3 = botAI.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
					if (!Physics.Linecast(transform3.position, this.player.position, this.mask))
					{
						transform = transform3;
					}
				}
			}
		}
		if (transform == null)
		{
			foreach (BotAI botAI2 in array)
			{
				if (transform == null || Vector3.Distance(botAI2.transform.position, this.player.position) < Vector3.Distance(transform.position, this.player.position))
				{
					Transform transform4 = botAI2.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
					if (!Physics.Linecast(transform4.position, this.player.position, this.mask))
					{
						transform = transform4;
					}
					else
					{
						Transform transform5 = botAI2.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
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

	// Token: 0x06000636 RID: 1590 RVA: 0x0002EB50 File Offset: 0x0002CD50
	private void OnGUI()
	{
		if (mainmenu.pause || mainmenu.disableHUD)
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
		if (Time.time != 0f && Time.timeScale != 0f && this.reticle != null && !WeaponHandling.holdFire && Interaction.playerInteraction != null && Interaction.playerInteraction.weaponHandler != null && Interaction.playerInteraction.weaponHandler.status != WeaponHandling.WeaponStatus.ENGAGED && !Interaction.playerInteraction.weaponHandler.blindFire)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)this.reticle.width * 0.5f, (float)(Screen.height / 2) - (float)this.reticle.height * 0.5f, (float)this.reticle.width, (float)this.reticle.height), this.reticle);
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0002ECD8 File Offset: 0x0002CED8
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

	// Token: 0x06000638 RID: 1592 RVA: 0x0002ED84 File Offset: 0x0002CF84
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
	
	IEnumerator VsratoProlog(){
		yield return new WaitForSeconds(0.5f);
		//Debug.Log("PRODAM GARAZH");
		this.camOffset = new Vector3(0,0,0);
	}

	// Token: 0x040006E4 RID: 1764
	public Transform player;

	// Token: 0x040006E5 RID: 1765
	public Transform aimTarget;

	// Token: 0x040006E6 RID: 1766
	public float smoothingTime = 0.15f;

	// Token: 0x040006E7 RID: 1767
	public Vector3 pivotOffset = new Vector3(1.3f, 0.4f, 0f);

	// Token: 0x040006E8 RID: 1768
	public Vector3 camOffset = new Vector3(0f, 0.7f, -2.4f);

	// Token: 0x040006E9 RID: 1769
	public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0f);

	// Token: 0x040006EA RID: 1770
	public float horizontalAimingSpeed = 270f;

	// Token: 0x040006EB RID: 1771
	public float verticalAimingSpeed = 270f;

	// Token: 0x040006EC RID: 1772
	public float maxVerticalAngle = 80f;

	// Token: 0x040006ED RID: 1773
	public float minVerticalAngle = -80f;

	// Token: 0x040006EE RID: 1774
	public static float mouseSensitivity = 0.34f;

	// Token: 0x040006EF RID: 1775
	public Texture reticle;

	// Token: 0x040006F0 RID: 1776
	public Texture2D hitTexture;

	// Token: 0x040006F1 RID: 1777
	public Texture2D blackTexture;

	// Token: 0x040006F2 RID: 1778
	private float angleH;

	// Token: 0x040006F3 RID: 1779
	public float angleV;

	// Token: 0x040006F4 RID: 1780
	private Transform cam;

	// Token: 0x040006F5 RID: 1781
	private float maxCamDist = 1f;

	// Token: 0x040006F6 RID: 1782
	private LayerMask mask;

	// Token: 0x040006F7 RID: 1783
	private Vector3 smoothPlayerPos;

	// Token: 0x040006F8 RID: 1784
	[HideInInspector]
	public bool lookAtTarget;

	// Token: 0x040006F9 RID: 1785
	private bool previousLookAtTarget;

	// Token: 0x040006FA RID: 1786
	private Quaternion previousCamRotation;

	// Token: 0x040006FB RID: 1787
	private float refX;

	// Token: 0x040006FC RID: 1788
	private float refY;

	// Token: 0x040006FD RID: 1789
	private float refZ;

	// Token: 0x040006FE RID: 1790
	private float refD;

	// Token: 0x040006FF RID: 1791
	private float refD2;

	// Token: 0x04000700 RID: 1792
	private float refVAng;

	// Token: 0x04000701 RID: 1793
	private Vector3 refAim;

	// Token: 0x04000702 RID: 1794
	private float coverOffsetRef;

	// Token: 0x04000703 RID: 1795
	private float smoothedMaxCamDist = 1f;

	// Token: 0x04000704 RID: 1796
	private float specialCaseTimer;

	// Token: 0x04000705 RID: 1797
	public bool fixedCameraPosition;

	// Token: 0x04000706 RID: 1798
	private bool previousFixedCameraPosition;

	// Token: 0x04000707 RID: 1799
	private float cameraPositionAdjustmentTimer;

	// Token: 0x04000708 RID: 1800
	[HideInInspector]
	public Transform cameraPosition;

	// Token: 0x04000709 RID: 1801
	public float inTransitionTime = 1f;

	// Token: 0x0400070A RID: 1802
	public float outTransitionTime = 1f;

	// Token: 0x0400070B RID: 1803
	private Vector3 smoothPosition;

	// Token: 0x0400070C RID: 1804
	public bool allowCircleCamera = true;

	// Token: 0x0400070D RID: 1805
	[HideInInspector]
	public float hitAlpha;

	// Token: 0x0400070E RID: 1806
	[HideInInspector]
	public float blackAlpha;

	// Token: 0x0400070F RID: 1807
	private float minHorizontalAngle;

	// Token: 0x04000710 RID: 1808
	private float maxHorizontalAngle;

	// Token: 0x04000711 RID: 1809
	private float previousminVerticalAngle;

	// Token: 0x04000712 RID: 1810
	private float previousmaxVerticalAngle;

	// Token: 0x04000713 RID: 1811
	private float minShakeSpeed;

	// Token: 0x04000714 RID: 1812
	private float maxShakeSpeed;

	// Token: 0x04000715 RID: 1813
	private float minShake = 10f;

	// Token: 0x04000716 RID: 1814
	private float maxShake = 10f;

	// Token: 0x04000717 RID: 1815
	private int minShakeTimes = 3;

	// Token: 0x04000718 RID: 1816
	private int maxShakeTimes = 8;

	// Token: 0x04000719 RID: 1817
	private float maxShakeDistance = 20f;

	// Token: 0x0400071A RID: 1818
	private bool shake;

	// Token: 0x0400071B RID: 1819
	private float shakeSpeed = 2f;

	// Token: 0x0400071C RID: 1820
	private float cShakePos;

	// Token: 0x0400071D RID: 1821
	private int shakeTimes = 8;

	// Token: 0x0400071E RID: 1822
	private float cShake;

	// Token: 0x0400071F RID: 1823
	private float cShakeSpeed;

	// Token: 0x04000720 RID: 1824
	private int cShakeTimes;

	// Token: 0x04000721 RID: 1825
	public static bool inverseX;

	// Token: 0x04000722 RID: 1826
	public static bool inverseY;

	// Token: 0x04000723 RID: 1827
	public bool shakingCamera;

	// Token: 0x04000724 RID: 1828
	public Texture moveCurser;

	// Token: 0x04000725 RID: 1829
	private Vector3 previousMoveControllerPosition;

	// Token: 0x04000726 RID: 1830
	public float coverOffset;

	// Token: 0x04000727 RID: 1831
	public bool aim;

	// Token: 0x04000728 RID: 1832
	public bool shoot;

	// Token: 0x04000729 RID: 1833
	public bool takeDown;

	// Token: 0x0400072A RID: 1834
	public bool inversedAiming;

	// Token: 0x0400072B RID: 1835
	public float shootingCamOffsetX = 0.6f;

	// Token: 0x0400072C RID: 1836
	public float unaimedZoom = -4f;

	// Token: 0x0400072D RID: 1837
	public float aimedZoom = -1f;

	// Token: 0x0400072E RID: 1838
	private float takeDownZoom = -2.5f;

	// Token: 0x0400072F RID: 1839
	public Transform lockTarget;

	// Token: 0x04000730 RID: 1840
	public bool focusOnTarget;

	// Token: 0x04000731 RID: 1841
	private Vector3 RandomSemiAssest;

	// Token: 0x04000732 RID: 1842
	public static ShooterGameCamera.AimAssestTypes aimAssestType = ShooterGameCamera.AimAssestTypes.HARD;

	// Token: 0x04000733 RID: 1843
	private static int sphereVisible = 1;

	// Token: 0x04000734 RID: 1844
	private bool cameraConnected = true;

	// Token: 0x04000735 RID: 1845
	public Texture sphereNotVisibleTex;

	// Token: 0x04000736 RID: 1846
	public static bool moveCalibrated;

	// Token: 0x04000737 RID: 1847
	public static float moveCalibrationX = 150f;

	// Token: 0x04000738 RID: 1848
	public static float moveCalibrationY = 150f;

	// Token: 0x04000739 RID: 1849
	public static float moveCalibrationYZero;

	// Token: 0x0400073A RID: 1850
	private Renderer[] playerRenderers;

	// Token: 0x0400073B RID: 1851
	private bool hidePlayer;

	// Token: 0x0400073C RID: 1852
	public bool inCover;

	// Token: 0x0400073D RID: 1853
	public AudioClip fallingSound;

	// Token: 0x0400073E RID: 1854
	public float shortCoverY;

	// Token: 0x0400073F RID: 1855
	[HideInInspector]
	public float recoil;

	// Token: 0x04000740 RID: 1856
	public float lastRecoilTime;

	// Token: 0x04000741 RID: 1857
	private CharacterController cc;

	// Token: 0x04000742 RID: 1858
	public bool hintFocus;

	// Token: 0x04000743 RID: 1859
	public Transform prioritizedAimTarget;

	// Token: 0x04000744 RID: 1860
	public bool meleeCamera;
	public bool prologVsrat = false;

	// Token: 0x04000745 RID: 1861
	public bool previousMeleeCamera;

	// Token: 0x0200011B RID: 283
	public enum AimAssestTypes
	{
		// Token: 0x04000747 RID: 1863
		OFF,
		// Token: 0x04000748 RID: 1864
		SEMI,
		// Token: 0x04000749 RID: 1865
		HARD
	}
}
