using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
[RequireComponent(typeof(CharacterController))]
public class NormalCharacterMotor : CharacterMotorScript
{
	// Token: 0x0600012C RID: 300 RVA: 0x00008DE0 File Offset: 0x00006FE0
	private void OnEnable()
	{
		this.firstframe = true;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00008DEC File Offset: 0x00006FEC
	public void Start()
	{
		if (base.GetComponent<Animation>()["Run2Stop"] != null)
		{
			base.GetComponent<Animation>()["Run2Stop"].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()["Run2Stop"].layer = 99;
			base.GetComponent<Animation>()["Run2Stop"].speed = 1.5f;
		}
		this.animationHandler = AnimationHandler.instance;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008E68 File Offset: 0x00007068
	private void UpdateFacingDirection()
	{
		float magnitude = base.desiredFacingDirection.magnitude;
		Vector3 vector = base.transform.rotation * base.desiredMovementDirection * (1f - magnitude) + base.desiredFacingDirection * magnitude;
		vector = Util.ProjectOntoPlane(vector, base.transform.up);
		vector = this.alignCorrection * vector;
		if (vector.sqrMagnitude > 0.01f)
		{
			Vector3 vector2 = Util.ConstantSlerp(base.transform.forward, vector, this.maxRotationSpeed * Time.deltaTime);
			vector2 = Util.ProjectOntoPlane(vector2, base.transform.up);
			Quaternion rotation = default(Quaternion);
			rotation.SetLookRotation(vector2, base.transform.up);
			base.transform.rotation = rotation;
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00008F40 File Offset: 0x00007140
	private void UpdateVelocity()
	{
		CharacterController characterController = base.GetComponent(typeof(CharacterController)) as CharacterController;
		Vector3 vector = characterController.velocity;
		if (this.firstframe)
		{
			vector = Vector3.zero;
			this.firstframe = false;
		}
		if (base.grounded)
		{
			vector = Util.ProjectOntoPlane(vector, base.transform.up);
		}
		Vector3 a = vector;
		base.jumping = false;
		if (base.grounded)
		{
			if (!this.running && (double)base.desiredVelocity.magnitude > 5.8)
			{
				this.running = true;
			}
			if (this.running && (double)base.desiredVelocity.magnitude < 4.0 && (this.animationHandler == null || (this.animationHandler.animState != AnimationHandler.AnimStates.JUMPING && this.animationHandler.animState != AnimationHandler.AnimStates.FALLING)))
			{
				if ((base.desiredVelocity.magnitude < 0.5f * this.scaleFactor || Vector3.Angle(base.transform.forward, base.desiredVelocity) < 30f) && !this.justRolled)
				{
					base.GetComponent<Animation>().CrossFade("Run2Stop");
				}
				this.running = false;
				this.justRolled = false;
			}
			Vector3 b = base.desiredVelocity - vector;
			if (Vector3.Angle(base.transform.forward, vector) < 10f && b.magnitude > this.maxVelocityChange)
			{
				b = b.normalized * this.maxVelocityChange;
			}
			a += b;
			if (this.Impulse != Vector3.zero)
			{
				a = new Vector3(this.Impulse.x * this.scaleFactor, this.Impulse.y * this.scaleFactor, this.Impulse.z * this.scaleFactor);
				this.Impulse = Vector3.zero;
			}
		}
		a += base.transform.up * -this.gravity * Time.deltaTime;
		if (base.jumping)
		{
			a -= base.transform.up * -this.gravity * Time.deltaTime / 2f;
		}
		CollisionFlags collisionFlags = characterController.Move(a * Time.deltaTime);
		base.grounded = ((collisionFlags & CollisionFlags.Below) != CollisionFlags.None);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000091E4 File Offset: 0x000073E4
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (!this.disableRotation)
		{
			this.UpdateFacingDirection();
		}
		if (!this.disableMovement)
		{
			this.UpdateVelocity();
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00009214 File Offset: 0x00007414
	public void Death()
	{
	}

	// Token: 0x04000131 RID: 305
	public float maxRotationSpeed = 270f;

	// Token: 0x04000132 RID: 306
	private bool firstframe = true;

	// Token: 0x04000133 RID: 307
	public bool disableMovement;

	// Token: 0x04000134 RID: 308
	public bool disableRotation;

	// Token: 0x04000135 RID: 309
	private bool running;

	// Token: 0x04000136 RID: 310
	public bool justRolled;

	// Token: 0x04000137 RID: 311
	public float scaleFactor = 1f;

	// Token: 0x04000138 RID: 312
	private AnimationHandler animationHandler;
}
