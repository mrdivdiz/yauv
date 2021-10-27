using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
[RequireComponent(typeof(CharacterController))]
[Serializable]
public class CameraRelativeControl : MonoBehaviour
{
	// Token: 0x060000C0 RID: 192 RVA: 0x0000404C File Offset: 0x0000224C
	public CameraRelativeControl()
	{
		this.speed = (float)5;
		this.jumpSpeed = (float)8;
		this.inAirMultiplier = 0.25f;
		this.rotationSpeed = new Vector2((float)50, (float)25);
		this.canJump = true;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00004088 File Offset: 0x00002288
	public virtual void Start()
	{
		this.thisTransform = (Transform)this.GetComponent(typeof(Transform));
		this.character = (CharacterController)this.GetComponent(typeof(CharacterController));
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if (gameObject)
		{
			this.thisTransform.position = gameObject.transform.position;
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000040F8 File Offset: 0x000022F8
	public virtual void FaceMovementDirection()
	{
		Vector3 vector = this.character.velocity;
		vector.y = (float)0;
		if (vector.magnitude > 0.1f)
		{
			this.thisTransform.forward = vector.normalized;
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00004140 File Offset: 0x00002340
	public virtual void OnEndGame()
	{
		//this.moveJoystick.Disable();
		//this.rotateJoystick.Disable();
		//this.enabled = false;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00004160 File Offset: 0x00002360
	public virtual void Update()
	{
		Vector3 vector = this.cameraTransform.TransformDirection(new Vector3(this.moveJoystick.position.x, (float)0, this.moveJoystick.position.y));
		vector.y = (float)0;
		vector.Normalize();
		Vector2 vector2 = new Vector2(Mathf.Abs(this.moveJoystick.position.x), Mathf.Abs(this.moveJoystick.position.y));
		vector *= this.speed * ((vector2.x <= vector2.y) ? vector2.y : vector2.x);
		if (this.character.isGrounded)
		{
			//if (!this.rotateJoystick.IsFingerDown())
			//{
			//	this.canJump = true;
			//}
			if (this.canJump && this.rotateJoystick.tapCount == 2)
			{
				this.velocity = this.character.velocity;
				this.velocity.y = this.jumpSpeed;
				this.canJump = false;
			}
		}
		else
		{
			this.velocity.y = this.velocity.y + Physics.gravity.y * Time.deltaTime;
			vector.x *= this.inAirMultiplier;
			vector.z *= this.inAirMultiplier;
		}
		vector += this.velocity;
		vector += Physics.gravity;
		vector *= Time.deltaTime;
		this.character.Move(vector);
		if (this.character.isGrounded)
		{
			this.velocity = Vector3.zero;
		}
		this.FaceMovementDirection();
		Vector2 a = this.rotateJoystick.position;
		a.x *= this.rotationSpeed.x;
		a.y *= this.rotationSpeed.y;
		a *= Time.deltaTime;
		this.cameraPivot.Rotate((float)0, a.x, (float)0, Space.World);
		this.cameraPivot.Rotate(a.y, (float)0, (float)0);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x000043A8 File Offset: 0x000025A8
	public virtual void Main()
	{
	}

	// Token: 0x0400005B RID: 91
	public Joystick moveJoystick;

	// Token: 0x0400005C RID: 92
	public Joystick rotateJoystick;

	// Token: 0x0400005D RID: 93
	public Transform cameraPivot;

	// Token: 0x0400005E RID: 94
	public Transform cameraTransform;

	// Token: 0x0400005F RID: 95
	public float speed;

	// Token: 0x04000060 RID: 96
	public float jumpSpeed;

	// Token: 0x04000061 RID: 97
	public float inAirMultiplier;

	// Token: 0x04000062 RID: 98
	public Vector2 rotationSpeed;

	// Token: 0x04000063 RID: 99
	private Transform thisTransform;

	// Token: 0x04000064 RID: 100
	private CharacterController character;

	// Token: 0x04000065 RID: 101
	private Vector3 velocity;

	// Token: 0x04000066 RID: 102
	private bool canJump;
}
