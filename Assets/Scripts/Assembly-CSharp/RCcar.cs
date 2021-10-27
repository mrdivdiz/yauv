using System;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class RCcar : MonoBehaviour
{
	// Token: 0x06000810 RID: 2064 RVA: 0x00041B18 File Offset: 0x0003FD18
	private void Start()
	{
		this.startingY = base.transform.position.y;
		this.startingPosition = base.transform.position;
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00041B50 File Offset: 0x0003FD50
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.disabled)
		{
			if (this.isMoving)
			{
				this.Sound.Stop();
				base.gameObject.GetComponent<Animation>().Stop();
				this.isMoving = false;
			}
			return;
		}
		float num = PlatformCharacterController.joystickLeft.position.y + Input.GetAxis("Vertical");
		float num2 = PlatformCharacterController.joystickLeft.position.x + Input.GetAxis("Horizontal");
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameStick)
		{
			if (Input.GetKey(KeyCode.JoystickButton7))
			{
				num += 1f;
			}
			if (Input.GetKey(KeyCode.JoystickButton6))
			{
				num -= 1f;
			}
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
		{
			num = Mathf.Clamp(num + Input.GetAxisRaw("Throttle") - Input.GetAxisRaw("Brake"), -1f, 1f);
		}
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.IOS && Input.GetJoystickNames().Length > 0)
		{
			if (Input.GetJoystickNames()[0].Contains("basic"))
			{
				if (Input.GetKey(KeyCode.JoystickButton9))
				{
					num += 1f;
				}
				if (Input.GetKey(KeyCode.JoystickButton8))
				{
					num -= 1f;
				}
			}
			else
			{
				if (Input.GetKey(KeyCode.JoystickButton11))
				{
					num += 1f;
				}
				if (Input.GetKey(KeyCode.JoystickButton10))
				{
					num -= 1f;
				}
			}
		}
		Vector3 position = base.transform.position;
		position.y += 0.1f;
		if (num > 0.1f && !this.disabled && !Physics.Raycast(position, base.transform.forward, 0.8f))
		{
			base.transform.Translate(Vector3.forward * Time.deltaTime * 4.5f * Mathf.Abs(num));
			this.isMoving = true;
		}
		if (num2 < -0.3f && num > 0.1f)
		{
			base.transform.Rotate(-Vector3.up * Time.deltaTime * 45f * Mathf.Abs(num2), Space.World);
		}
		if (num2 < -0.3f && num < -0.1f)
		{
			base.transform.Rotate(Vector3.up * Time.deltaTime * 45f * Mathf.Abs(num2), Space.World);
		}
		if (num2 > 0.3f && num > 0.1f)
		{
			base.transform.Rotate(Vector3.up * Time.deltaTime * 45f * Mathf.Abs(num2), Space.World);
		}
		if (num2 > 0.3f && num < -0.1f)
		{
			base.transform.Rotate(-Vector3.up * Time.deltaTime * 45f * Mathf.Abs(num2), Space.World);
		}
		if (num < -0.1f && !this.disabled && !Physics.Raycast(position, -base.transform.forward, 0.8f))
		{
			base.transform.Translate(-Vector3.forward * Time.deltaTime * 3f * Mathf.Abs(num));
			this.isMoving = true;
		}
		if (num < 0.1f && !Input.GetKey(KeyCode.JoystickButton5) && num > -0.1f)
		{
			this.isMoving = false;
		}
		if (this.isMoving)
		{
			if (!this.Sound.isPlaying)
			{
				this.Sound.Play();
			}
			base.gameObject.GetComponent<Animation>().Play();
		}
		else if (!this.isMoving)
		{
			this.Sound.Stop();
			base.gameObject.GetComponent<Animation>().Stop();
		}
		if (Vector3.Angle(base.transform.up, Vector3.up) > 45f)
		{
			base.transform.Rotate(base.transform.forward, Vector3.Angle(base.transform.up, Vector3.up) * 3f * Time.deltaTime);
			base.transform.Rotate(base.transform.right, Vector3.Angle(base.transform.up, Vector3.up) * 3f * Time.deltaTime);
		}
		if (base.transform.position.y < this.startingY)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.startingY, base.transform.position.z);
		}
		if (Vector3.Distance(base.transform.position, this.startingPosition) > 31f)
		{
			base.transform.position = this.startingPosition;
		}
	}

	// Token: 0x04000AA3 RID: 2723
	public bool isMoving;

	// Token: 0x04000AA4 RID: 2724
	public AudioSource Sound;

	// Token: 0x04000AA5 RID: 2725
	private float startingY;

	// Token: 0x04000AA6 RID: 2726
	private Vector3 startingPosition;

	// Token: 0x04000AA7 RID: 2727
	public bool disabled = true;

	// Token: 0x04000AA8 RID: 2728
	public static bool CarMode;
}
