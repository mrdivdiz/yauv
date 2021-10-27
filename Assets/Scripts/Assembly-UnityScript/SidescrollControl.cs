using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
[RequireComponent(typeof(CharacterController))]
[Serializable]
public class SidescrollControl : MonoBehaviour
{
	// Token: 0x060000F0 RID: 240 RVA: 0x00005C5C File Offset: 0x00003E5C
	public SidescrollControl()
	{
		this.forwardSpeed = (float)4;
		this.backwardSpeed = (float)4;
		this.jumpSpeed = (float)16;
		this.inAirMultiplier = 0.25f;
		this.canJump = true;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00005C90 File Offset: 0x00003E90
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

	// Token: 0x060000F2 RID: 242 RVA: 0x00005D00 File Offset: 0x00003F00
	public virtual void OnEndGame()
	{
		this.moveTouchPad.enabled = false;
		this.jumpTouchPad.enabled = false;
		this.enabled = false;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00005D20 File Offset: 0x00003F20
	public virtual void Update()
	{
		Vector3 vector = Vector3.zero;
		if (this.moveTouchPad.position.x > (float)0)
		{
			vector = Vector3.right * this.forwardSpeed * this.moveTouchPad.position.x;
		}
		else
		{
			vector = Vector3.right * this.backwardSpeed * this.moveTouchPad.position.x;
		}
		if (this.character.isGrounded)
		{
			bool flag = false;
			Joystick joystick = this.jumpTouchPad;
			//if (!joystick.IsFingerDown())
			//{
				this.canJump = true;
			//}
			if (this.canJump)
			{
				flag = true;
				this.canJump = false;
			}
			if (flag)
			{
				this.velocity = this.character.velocity;
				this.velocity.y = this.jumpSpeed;
			}
		}
		else
		{
			this.velocity.y = this.velocity.y + Physics.gravity.y * Time.deltaTime;
			vector.x *= this.inAirMultiplier;
		}
		vector += this.velocity;
		vector += Physics.gravity;
		vector *= Time.deltaTime;
		this.character.Move(vector);
		if (this.character.isGrounded)
		{
			this.velocity = Vector3.zero;
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00005EA0 File Offset: 0x000040A0
	public virtual void Main()
	{
	}

	// Token: 0x040000B7 RID: 183
	public Joystick moveTouchPad;

	// Token: 0x040000B8 RID: 184
	public Joystick jumpTouchPad;

	// Token: 0x040000B9 RID: 185
	public float forwardSpeed;

	// Token: 0x040000BA RID: 186
	public float backwardSpeed;

	// Token: 0x040000BB RID: 187
	public float jumpSpeed;

	// Token: 0x040000BC RID: 188
	public float inAirMultiplier;

	// Token: 0x040000BD RID: 189
	private Transform thisTransform;

	// Token: 0x040000BE RID: 190
	private CharacterController character;

	// Token: 0x040000BF RID: 191
	private Vector3 velocity;

	// Token: 0x040000C0 RID: 192
	private bool canJump;
}
