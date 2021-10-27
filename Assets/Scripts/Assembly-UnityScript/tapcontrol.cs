using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
[Serializable]
public class tapcontrol : MonoBehaviour
{
	// Token: 0x060000FD RID: 253 RVA: 0x000061BC File Offset: 0x000043BC
	public tapcontrol()
	{
		this.inAirMultiplier = 0.25f;
		this.minimumDistanceToMove = 1f;
		this.minimumTimeUntilMove = 0.25f;
		this.rotateEpsilon = (float)1;
		this.state = ControlState.WaitingForFirstTouch;
		this.fingerDown = new int[2];
		this.fingerDownPosition = new Vector2[2];
		this.fingerDownFrame = new int[2];
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00006224 File Offset: 0x00004424
	public virtual void Start()
	{
		this.thisTransform = this.transform;
		this.zoomCamera = (ZoomCamera)this.cameraObject.GetComponent(typeof(ZoomCamera));
		this.cam = this.cameraObject.GetComponent<Camera>();
		this.character = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.ResetControlState();
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if (gameObject)
		{
			this.thisTransform.position = gameObject.transform.position;
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x000062BC File Offset: 0x000044BC
	public virtual void OnEndGame()
	{
		this.enabled = false;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x000062C8 File Offset: 0x000044C8
	public virtual void FaceMovementDirection()
	{
		Vector3 vector = this.character.velocity;
		vector.y = (float)0;
		if (vector.magnitude > 0.1f)
		{
			this.thisTransform.forward = vector.normalized;
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006310 File Offset: 0x00004510
	public virtual void CameraControl(Touch touch0, Touch touch1)
	{
		if (this.rotateEnabled && this.state == ControlState.RotatingCamera)
		{
			Vector2 a = touch1.position - touch0.position;
			Vector2 lhs = a / a.magnitude;
			Vector2 a2 = touch1.position - touch1.deltaPosition - (touch0.position - touch0.deltaPosition);
			Vector2 rhs = a2 / a2.magnitude;
			float num = Vector2.Dot(lhs, rhs);
			if (num < (float)1)
			{
				Vector3 lhs2 = new Vector3(a.x, a.y);
				Vector3 rhs2 = new Vector3(a2.x, a2.y);
				float z = Vector3.Cross(lhs2, rhs2).normalized.z;
				float num2 = Mathf.Acos(num);
				this.rotationTarget += num2 * 57.29578f * z;
				if (this.rotationTarget < (float)0)
				{
					this.rotationTarget += (float)360;
				}
				else if (this.rotationTarget >= (float)360)
				{
					this.rotationTarget -= (float)360;
				}
			}
		}
		else if (this.zoomEnabled && this.state == ControlState.ZoomingCamera)
		{
			float magnitude = (touch1.position - touch0.position).magnitude;
			float magnitude2 = (touch1.position - touch1.deltaPosition - (touch0.position - touch0.deltaPosition)).magnitude;
			float num3 = magnitude - magnitude2;
			this.zoomCamera.zoom = this.zoomCamera.zoom + num3 * this.zoomRate * Time.deltaTime;
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006520 File Offset: 0x00004720
	public virtual void CharacterControl()
	{
		int touchCount = Input.touchCount;
		if (touchCount == 1 && this.state == ControlState.MovingCharacter)
		{
			Touch touch = Input.GetTouch(0);
			if (this.character.isGrounded && this.jumpButton.HitTest(touch.position))
			{
				this.velocity = this.character.velocity;
				this.velocity.y = this.jumpSpeed;
			}
			else if (!this.jumpButton.HitTest(touch.position) && touch.phase != TouchPhase.Began)
			{
				Ray ray = this.cam.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y));
				RaycastHit raycastHit = default(RaycastHit);
				if (Physics.Raycast(ray, out raycastHit))
				{
					float magnitude = (this.transform.position - raycastHit.point).magnitude;
					if (magnitude > this.minimumDistanceToMove)
					{
						this.targetLocation = raycastHit.point;
					}
					this.moving = true;
				}
			}
		}
		Vector3 vector = Vector3.zero;
		if (this.moving)
		{
			vector = this.targetLocation - this.thisTransform.position;
			vector.y = (float)0;
			float magnitude2 = vector.magnitude;
			if (magnitude2 < (float)1)
			{
				this.moving = false;
			}
			else
			{
				vector = vector.normalized * this.speed;
			}
		}
		if (!this.character.isGrounded)
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
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00006778 File Offset: 0x00004978
	public virtual void ResetControlState()
	{
		this.state = ControlState.WaitingForFirstTouch;
		this.fingerDown[0] = -1;
		this.fingerDown[1] = -1;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006794 File Offset: 0x00004994
	public virtual void Update()
	{
		int touchCount = Input.touchCount;
		if (touchCount == 0)
		{
			this.ResetControlState();
		}
		else
		{
			int i = 0;
			Touch touch = default(Touch);
			Touch[] touches = Input.touches;
			Touch touch2 = default(Touch);
			Touch touch3 = default(Touch);
			bool flag = false;
			bool flag2 = false;
			if (this.state == ControlState.WaitingForFirstTouch)
			{
				for (i = 0; i < touchCount; i++)
				{
					touch = touches[i];
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
					{
						this.state = ControlState.WaitingForSecondTouch;
						this.firstTouchTime = Time.time;
						this.fingerDown[0] = touch.fingerId;
						this.fingerDownPosition[0] = touch.position;
						this.fingerDownFrame[0] = Time.frameCount;
						break;
					}
				}
			}
			if (this.state == ControlState.WaitingForSecondTouch)
			{
				for (i = 0; i < touchCount; i++)
				{
					touch = touches[i];
					if (touch.phase != TouchPhase.Canceled)
					{
						if (touchCount >= 2 && touch.fingerId != this.fingerDown[0])
						{
							this.state = ControlState.WaitingForMovement;
							this.fingerDown[1] = touch.fingerId;
							this.fingerDownPosition[1] = touch.position;
							this.fingerDownFrame[1] = Time.frameCount;
							break;
						}
						if (touchCount == 1)
						{
							Vector2 vector = touch.position - this.fingerDownPosition[0];
							if (touch.fingerId == this.fingerDown[0] && (Time.time > this.firstTouchTime + this.minimumTimeUntilMove || touch.phase == TouchPhase.Ended))
							{
								this.state = ControlState.MovingCharacter;
								break;
							}
						}
					}
				}
			}
			if (this.state == ControlState.WaitingForMovement)
			{
				for (i = 0; i < touchCount; i++)
				{
					touch = touches[i];
					if (touch.phase == TouchPhase.Began)
					{
						if (touch.fingerId == this.fingerDown[0] && this.fingerDownFrame[0] == Time.frameCount)
						{
							touch2 = touch;
							flag = true;
						}
						else if (touch.fingerId != this.fingerDown[0] && touch.fingerId != this.fingerDown[1])
						{
							this.fingerDown[1] = touch.fingerId;
							touch3 = touch;
							flag2 = true;
						}
					}
					if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Ended)
					{
						if (touch.fingerId == this.fingerDown[0])
						{
							touch2 = touch;
							flag = true;
						}
						else if (touch.fingerId == this.fingerDown[1])
						{
							touch3 = touch;
							flag2 = true;
						}
					}
				}
				if (flag)
				{
					if (flag2)
					{
						Vector2 a = this.fingerDownPosition[1] - this.fingerDownPosition[0];
						Vector2 a2 = touch3.position - touch2.position;
						Vector2 lhs = a / a.magnitude;
						Vector2 rhs = a2 / a2.magnitude;
						float num = Vector2.Dot(lhs, rhs);
						if (num < (float)1)
						{
							float num2 = Mathf.Acos(num);
							if (num2 > this.rotateEpsilon * 0.017453292f)
							{
								this.state = ControlState.RotatingCamera;
							}
						}
						if (this.state == ControlState.WaitingForMovement)
						{
							float f = a.magnitude - a2.magnitude;
							if (Mathf.Abs(f) > this.zoomEpsilon)
							{
								this.state = ControlState.ZoomingCamera;
							}
						}
					}
				}
				else
				{
					this.state = ControlState.WaitingForNoFingers;
				}
			}
			if (this.state == ControlState.RotatingCamera || this.state == ControlState.ZoomingCamera)
			{
				for (i = 0; i < touchCount; i++)
				{
					touch = touches[i];
					if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Ended)
					{
						if (touch.fingerId == this.fingerDown[0])
						{
							touch2 = touch;
							flag = true;
						}
						else if (touch.fingerId == this.fingerDown[1])
						{
							touch3 = touch;
							flag2 = true;
						}
					}
				}
				if (flag)
				{
					if (flag2)
					{
						this.CameraControl(touch2, touch3);
					}
				}
				else
				{
					this.state = ControlState.WaitingForNoFingers;
				}
			}
		}
		this.CharacterControl();
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00006C24 File Offset: 0x00004E24
	public virtual void LateUpdate()
	{
		float y = Mathf.SmoothDampAngle(this.cameraPivot.eulerAngles.y, this.rotationTarget, ref this.rotationVelocity, 0.3f);
		Vector3 eulerAngles = this.cameraPivot.eulerAngles;
		float num = eulerAngles.y = y;
		Vector3 vector = this.cameraPivot.eulerAngles = eulerAngles;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00006C8C File Offset: 0x00004E8C
	public virtual void Main()
	{
	}

	// Token: 0x040000D8 RID: 216
	public GameObject cameraObject;

	// Token: 0x040000D9 RID: 217
	public Transform cameraPivot;

	// Token: 0x040000DA RID: 218
	public GUITexture jumpButton;

	// Token: 0x040000DB RID: 219
	public float speed;

	// Token: 0x040000DC RID: 220
	public float jumpSpeed;

	// Token: 0x040000DD RID: 221
	public float inAirMultiplier;

	// Token: 0x040000DE RID: 222
	public float minimumDistanceToMove;

	// Token: 0x040000DF RID: 223
	public float minimumTimeUntilMove;

	// Token: 0x040000E0 RID: 224
	public bool zoomEnabled;

	// Token: 0x040000E1 RID: 225
	public float zoomEpsilon;

	// Token: 0x040000E2 RID: 226
	public float zoomRate;

	// Token: 0x040000E3 RID: 227
	public bool rotateEnabled;

	// Token: 0x040000E4 RID: 228
	public float rotateEpsilon;

	// Token: 0x040000E5 RID: 229
	private ZoomCamera zoomCamera;

	// Token: 0x040000E6 RID: 230
	private Camera cam;

	// Token: 0x040000E7 RID: 231
	private Transform thisTransform;

	// Token: 0x040000E8 RID: 232
	private CharacterController character;

	// Token: 0x040000E9 RID: 233
	private Vector3 targetLocation;

	// Token: 0x040000EA RID: 234
	private bool moving;

	// Token: 0x040000EB RID: 235
	private float rotationTarget;

	// Token: 0x040000EC RID: 236
	private float rotationVelocity;

	// Token: 0x040000ED RID: 237
	private Vector3 velocity;

	// Token: 0x040000EE RID: 238
	private ControlState state;

	// Token: 0x040000EF RID: 239
	private int[] fingerDown;

	// Token: 0x040000F0 RID: 240
	private Vector2[] fingerDownPosition;

	// Token: 0x040000F1 RID: 241
	private int[] fingerDownFrame;

	// Token: 0x040000F2 RID: 242
	private float firstTouchTime;
}
