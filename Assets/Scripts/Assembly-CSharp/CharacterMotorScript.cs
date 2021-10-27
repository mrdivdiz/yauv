using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public abstract class CharacterMotorScript : MonoBehaviour
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000121 RID: 289 RVA: 0x00008AAC File Offset: 0x00006CAC
	// (set) Token: 0x06000122 RID: 290 RVA: 0x00008AB4 File Offset: 0x00006CB4
	public bool grounded
	{
		get
		{
			return this.m_Grounded;
		}
		protected set
		{
			this.m_Grounded = value;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000123 RID: 291 RVA: 0x00008AC0 File Offset: 0x00006CC0
	// (set) Token: 0x06000124 RID: 292 RVA: 0x00008AC8 File Offset: 0x00006CC8
	public bool jumping
	{
		get
		{
			return this.m_Jumping;
		}
		protected set
		{
			this.m_Jumping = value;
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00008AD4 File Offset: 0x00006CD4
	private void Start()
	{
		this.alignCorrection = default(Quaternion);
		this.alignCorrection.SetLookRotation(this.forwardVector, Vector3.up);
		this.alignCorrection = Quaternion.Inverse(this.alignCorrection);
		this.Impulse = Vector3.zero;
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000126 RID: 294 RVA: 0x00008B24 File Offset: 0x00006D24
	// (set) Token: 0x06000127 RID: 295 RVA: 0x00008B2C File Offset: 0x00006D2C
	public Vector3 desiredMovementDirection
	{
		get
		{
			return this.m_desiredMovementDirection;
		}
		set
		{
			this.m_desiredMovementDirection = value;
			if (this.m_desiredMovementDirection.magnitude > 1f || AnimationHandler.instance.stealthMode)
			{
				this.m_desiredMovementDirection = this.m_desiredMovementDirection.normalized;
			}
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000128 RID: 296 RVA: 0x00008B78 File Offset: 0x00006D78
	// (set) Token: 0x06000129 RID: 297 RVA: 0x00008B80 File Offset: 0x00006D80
	public Vector3 desiredFacingDirection
	{
		get
		{
			return this.m_desiredFacingDirection;
		}
		set
		{
			this.m_desiredFacingDirection = value;
			if (this.m_desiredFacingDirection.magnitude > 1f)
			{
				this.m_desiredFacingDirection = this.m_desiredFacingDirection.normalized;
			}
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600012A RID: 298 RVA: 0x00008BB0 File Offset: 0x00006DB0
	public Vector3 desiredVelocity
	{
		get
		{
			if (this.m_desiredMovementDirection == Vector3.zero)
			{
				return Vector3.zero;
			}
			float num = this.maxForwardSpeed;
			if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton3)) && !AnimationHandler.instance.isholdingWeapon && (Application.loadedLevelName == "part1" || Application.loadedLevelName == "part2" || Application.loadedLevelName == "Survival_Temple" || Application.loadedLevelName == "Survival_Prologue" || Application.loadedLevelName == "Survival_Desert" || Application.loadedLevelName == "Survival_TangierCity" || Application.loadedLevelName == "Survival_TangierCafe" || Application.loadedLevelName == "Survival_TangierMarket" || Application.loadedLevelName == "Survival_NightJungle") && !AnimationHandler.instance.stealthMode && !AnimationHandler.forcedWalk)
			{
				num = this.maxSprintSpeed;
			}
			else if (AnimationHandler.instance.engagedMode)
			{
				num = this.maxEngagedSpeed;
			}
			else if (AnimationHandler.instance.stealthMode)
			{
				num = this.maxStealthSpeed;
			}
			float num2 = ((this.m_desiredMovementDirection.z <= 0f) ? this.maxBackwardsSpeed : num) / this.maxSidewaysSpeed;
			Vector3 vector = new Vector3(this.m_desiredMovementDirection.x, 0f, this.m_desiredMovementDirection.z / num2);
			Vector3 normalized = vector.normalized;
			Vector3 vector2 = new Vector3(normalized.x, 0f, normalized.z * num2);
			float d = vector2.magnitude * this.maxSidewaysSpeed;
			Vector3 point = this.m_desiredMovementDirection * d;
			return base.transform.rotation * point;
		}
	}

	// Token: 0x04000120 RID: 288
	public float maxForwardSpeed = 1.5f;

	// Token: 0x04000121 RID: 289
	public float maxSprintSpeed = 9f;

	// Token: 0x04000122 RID: 290
	public float maxBackwardsSpeed = 1.5f;

	// Token: 0x04000123 RID: 291
	public float maxSidewaysSpeed = 1.5f;

	// Token: 0x04000124 RID: 292
	public float maxStealthSpeed = 4f;

	// Token: 0x04000125 RID: 293
	public float maxEngagedSpeed = 10f;

	// Token: 0x04000126 RID: 294
	public float maxVelocityChange = 0.2f;

	// Token: 0x04000127 RID: 295
	public float gravity = 10f;

	// Token: 0x04000128 RID: 296
	public bool canJump = true;

	// Token: 0x04000129 RID: 297
	public float jumpHeight = 1f;

	// Token: 0x0400012A RID: 298
	public Vector3 forwardVector = Vector3.forward;

	// Token: 0x0400012B RID: 299
	protected Quaternion alignCorrection;

	// Token: 0x0400012C RID: 300
	private bool m_Grounded;

	// Token: 0x0400012D RID: 301
	private bool m_Jumping;

	// Token: 0x0400012E RID: 302
	public Vector3 Impulse;

	// Token: 0x0400012F RID: 303
	private Vector3 m_desiredMovementDirection;

	// Token: 0x04000130 RID: 304
	private Vector3 m_desiredFacingDirection;
}
