using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class Joystick : MonoBehaviour
{
	// Token: 0x06000923 RID: 2339 RVA: 0x0004B31C File Offset: 0x0004951C
	private void Start()
	{
		this.gui = base.GetComponent<GUITexture>();
		this.guiBackGround = base.transform.Find("Background").GetComponent<GUITexture>();
		if (Screen.width > 1500)
		{
			this.gui.pixelInset = new Rect(this.gui.pixelInset.x, this.gui.pixelInset.y, 200f, 200f);
			this.guiBackGround.pixelInset = new Rect(this.guiBackGround.pixelInset.x, this.guiBackGround.pixelInset.y, 420f, 420f);
		}
		this.defaultRect = this.gui.pixelInset;
		this.defaultRect.x = this.defaultRect.x + base.transform.position.x * (float)Screen.width;
		this.defaultRect.y = this.defaultRect.y + base.transform.position.y * (float)Screen.height;
		base.transform.position = new Vector3(0f, 0f, 1f);
		if (this.touchPad)
		{
			this.touchZone = new Rect(0f, 0f, (float)Screen.width / 3f, (float)Screen.height / 2f);
		}
		else
		{
			this.guiTouchOffset.x = this.defaultRect.width * 0.5f;
			this.guiTouchOffset.y = this.defaultRect.height * 0.5f;
			this.guiCenter.x = this.defaultRect.x + this.guiTouchOffset.x;
			this.guiCenter.y = this.defaultRect.y + this.guiTouchOffset.y;
			this.guiBoundary.min.x = this.defaultRect.x - this.guiTouchOffset.x;
			this.guiBoundary.max.x = this.defaultRect.x + this.guiTouchOffset.x;
			this.guiBoundary.min.y = this.defaultRect.y - this.guiTouchOffset.y;
			this.guiBoundary.max.y = this.defaultRect.y + this.guiTouchOffset.y;
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0004B5C0 File Offset: 0x000497C0
	private void Disable()
	{
		base.gameObject.SetActive(false);
		Joystick.enumeratedJoysticks = false;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0004B5D4 File Offset: 0x000497D4
	private void ResetJoystick()
	{
		this.lastFingerId = -1f;
		this.position = Vector2.zero;
		this.fingerDownPos = Vector2.zero;
		if (this.touchPad)
		{
			this.gui.color = new Color(this.gui.color.r, this.gui.color.g, this.gui.color.b, 0f);
			this.guiBackGround.color = new Color(this.guiBackGround.color.r, this.guiBackGround.color.g, this.guiBackGround.color.b, 0f);
			MobileInput.joystickShown = false;
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0004B6B4 File Offset: 0x000498B4
	private bool IsFingerDown()
	{
		return this.lastFingerId != -1f;
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0004B6C8 File Offset: 0x000498C8
	private void LatchedFinger(int fingerId)
	{
		if (this.lastFingerId == (float)fingerId)
		{
			this.ResetJoystick();
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0004B6E0 File Offset: 0x000498E0
	private void Update()
	{
		if (!Joystick.enumeratedJoysticks)
		{
			Joystick.joysticks = (UnityEngine.Object.FindObjectsOfType(typeof(Joystick)) as Joystick[]);
			Joystick.enumeratedJoysticks = true;
		}
		int touchCount = Input.touchCount;
		if (this.tapTimeWindow > 0f)
		{
			this.tapTimeWindow -= Time.deltaTime;
		}
		else
		{
			this.tapCount = 0;
		}
		if (touchCount == 0)
		{
			this.ResetJoystick();
		}
		else
		{
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				bool flag = false;
				if (this.touchPad)
				{
					if (this.touchZone.Contains(touch.position))
					{
						flag = true;
					}
				}
				else if (this.gui.HitTest(touch.position))
				{
					flag = true;
				}
				if (flag && (this.lastFingerId == -1f || this.lastFingerId != (float)touch.fingerId) && touch.phase == TouchPhase.Began)
				{
					if (this.touchPad)
					{
						this.gui.color = new Color(this.gui.color.r, this.gui.color.g, this.gui.color.b, 0.4f);
						this.guiBackGround.color = new Color(this.guiBackGround.color.r, this.guiBackGround.color.g, this.guiBackGround.color.b, 0.3f);
						MobileInput.joystickShown = true;
						this.gui.pixelInset = new Rect(touch.position.x - this.gui.pixelInset.width / 2f, touch.position.y - this.gui.pixelInset.height / 2f, this.gui.pixelInset.width, this.gui.pixelInset.height);
						this.guiBackGround.pixelInset = new Rect(touch.position.x - this.guiBackGround.pixelInset.width / 2f, touch.position.y - this.guiBackGround.pixelInset.height / 2f, this.guiBackGround.pixelInset.width, this.guiBackGround.pixelInset.height);
						this.centerPosition = new Vector2(this.gui.pixelInset.x, this.gui.pixelInset.y);
						this.lastFingerId = (float)touch.fingerId;
						this.fingerDownPos = touch.position;
					}
					this.lastFingerId = (float)touch.fingerId;
					if (this.tapTimeWindow > 0f)
					{
						this.tapCount++;
					}
					else
					{
						this.tapCount = 1;
						this.tapTimeWindow = Joystick.tapTimeDelta;
					}
					foreach (Joystick joystick in Joystick.joysticks)
					{
						if (joystick != null && joystick != this)
						{
							joystick.LatchedFinger(touch.fingerId);
						}
					}
				}
				if (this.lastFingerId == (float)touch.fingerId)
				{
					if (touch.tapCount > this.tapCount)
					{
						this.tapCount = touch.tapCount;
					}
					if (this.touchPad)
					{
						this.position.x = Mathf.Clamp((touch.position.x - this.fingerDownPos.x) * 0.02f, -1f, 1f);
						this.position.y = Mathf.Clamp((touch.position.y - this.fingerDownPos.y) * 0.02f, -1f, 1f);
						this.gui.pixelInset = new Rect(this.centerPosition.x + this.position.x * (this.gui.pixelInset.width / 2f), this.centerPosition.y + this.position.y * (this.gui.pixelInset.height / 2f), this.gui.pixelInset.width, this.gui.pixelInset.height);
					}
					else
					{
						this.position.x = (touch.position.x - this.guiCenter.x) / this.guiTouchOffset.x;
						this.position.y = (touch.position.y - this.guiCenter.y) / this.guiTouchOffset.y;
					}
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						this.ResetJoystick();
					}
				}
			}
		}
		float magnitude = this.position.magnitude;
		if (magnitude < this.deadZone)
		{
			this.position = Vector2.zero;
		}
		else if (magnitude > 1f)
		{
			this.position /= magnitude;
		}
		else if (this.normalize)
		{
			this.position = this.position / magnitude * Mathf.InverseLerp(magnitude, this.deadZone, 1f);
		}
		if (!this.touchPad)
		{
			this.gui.pixelInset = new Rect((this.position.x - 1f) * this.guiTouchOffset.x + this.guiCenter.x, (this.position.y - 1f) * this.guiTouchOffset.y + this.guiCenter.y, this.gui.pixelInset.width, this.gui.pixelInset.height);
		}
	}

	// Token: 0x04000C41 RID: 3137
	private static Joystick[] joysticks;

	// Token: 0x04000C42 RID: 3138
	private static bool enumeratedJoysticks;

	// Token: 0x04000C43 RID: 3139
	private static float tapTimeDelta = 0.3f;

	// Token: 0x04000C44 RID: 3140
	public bool touchPad;

	// Token: 0x04000C45 RID: 3141
	public Rect touchZone;

	// Token: 0x04000C46 RID: 3142
	public float deadZone;

	// Token: 0x04000C47 RID: 3143
	public bool normalize;

	// Token: 0x04000C48 RID: 3144
	public Vector2 position;

	// Token: 0x04000C49 RID: 3145
	public int tapCount;

	// Token: 0x04000C4A RID: 3146
	private float lastFingerId = -1f;

	// Token: 0x04000C4B RID: 3147
	private float tapTimeWindow;

	// Token: 0x04000C4C RID: 3148
	private Vector2 fingerDownPos;

	// Token: 0x04000C4D RID: 3149
	public GUITexture gui;

	// Token: 0x04000C4E RID: 3150
	public GUITexture guiBackGround;

	// Token: 0x04000C4F RID: 3151
	private Rect defaultRect;

	// Token: 0x04000C50 RID: 3152
	private Joystick.Boundary guiBoundary = new Joystick.Boundary();

	// Token: 0x04000C51 RID: 3153
	private Vector2 guiTouchOffset;

	// Token: 0x04000C52 RID: 3154
	private Vector2 guiCenter;

	// Token: 0x04000C53 RID: 3155
	private Vector2 centerPosition;

	// Token: 0x020001CC RID: 460
	[RequireComponent(typeof(GUITexture))]
	private class Boundary
	{
		// Token: 0x04000C54 RID: 3156
		public Vector2 min = Vector2.zero;

		// Token: 0x04000C55 RID: 3157
		public Vector2 max = Vector2.zero;
	}
}
