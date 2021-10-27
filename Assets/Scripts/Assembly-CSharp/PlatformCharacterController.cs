using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class PlatformCharacterController : MonoBehaviour
{
	// Token: 0x06000133 RID: 307 RVA: 0x00009234 File Offset: 0x00007434
	private void Start()
	{
		this.motor = (base.GetComponent(typeof(CharacterMotorScript)) as CharacterMotorScript);
		if (this.motor == null)
		{
			Debug.Log("Motor is null!!");
		}
		this.basicAgility = base.GetComponent<BasicAgility>();
		this.animationHandler = base.GetComponent<AnimationHandler>();
		this.shooterCamera = GlobalFarisCam.farisCamera.GetComponent<ShooterGameCamera>();
	}

	// Token: 0x06000134 RID: 308 RVA: 0x000092A0 File Offset: 0x000074A0
	private void Awake()
	{
		if (this.joystickPrefab)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.joystickPrefab) as GameObject;
			gameObject.name = "Joystick Left";
			PlatformCharacterController.joystickLeft = gameObject.GetComponent<Joystick>();
			CutsceneManager.mobileControls = gameObject;
			MobileInput.inputMode = this.inputMode;
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000092F8 File Offset: 0x000074F8
	private void Update()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		if (!this.acceptUserInput || (this.basicAgility != null && this.basicAgility.animating) || (this.animationHandler != null && (this.animationHandler.animState == AnimationHandler.AnimStates.JUMPING || this.animationHandler.animState == AnimationHandler.AnimStates.FALLING)))
		{
			this.motor.desiredMovementDirection = Vector3.zero;
			return;
		}
		bool flag = false;
		Vector3 vector = new Vector3(PlatformCharacterController.joystickLeft.position.x + Input.GetAxis("Horizontal"), PlatformCharacterController.joystickLeft.position.y + Input.GetAxis("Vertical"), 0f);
		if (vector.magnitude < 0.4f)
		{
			vector = Vector3.zero;
		}
		if (vector.magnitude > 1f)
		{
			vector = vector.normalized;
		}
		if (vector.magnitude < 0.9f)
		{
			flag = true;
		}
		vector = vector.normalized;
		vector = Camera.main.transform.rotation * vector;
		Quaternion rotation = Quaternion.FromToRotation(Camera.main.transform.forward * -1f, base.transform.up);
		vector = rotation * vector;
		if (this.shooterCamera.allowCircleCamera)
		{
			float num = Vector3.Angle(vector, new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z));
			if (Vector3.Cross(new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z), vector).normalized.y > 0f)
			{
				num *= -1f;
			}
			if (!AnimationHandler.dontRotateCamera && vector != Vector3.zero && 125f > num && num > 70f)
			{
				vector = new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z);
				vector = vector.normalized;
				vector = Vector3.Cross(vector, base.transform.up);
				Camera.main.GetComponent<ShooterGameCamera>().lookAtTarget = true;
			}
			else if (!AnimationHandler.dontRotateCamera && vector != Vector3.zero && -125f < num && num < -70f)
			{
				vector = new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z) - new Vector3(base.transform.position.x, 0f, base.transform.position.z);
				vector = vector.normalized;
				vector = Vector3.Cross(vector, base.transform.up) * -1f;
				Camera.main.GetComponent<ShooterGameCamera>().lookAtTarget = true;
			}
			else if (Camera.main.GetComponent<ShooterGameCamera>() != null)
			{
				Camera.main.GetComponent<ShooterGameCamera>().lookAtTarget = false;
			}
		}
		else
		{
			Camera.main.GetComponent<ShooterGameCamera>().lookAtTarget = false;
		}
		if (this.restrectionTransform != null && Vector3.Distance(base.transform.position, this.restrectionTransform.position) > this.restrectionDistance)
		{
			Vector3 from = vector;
			from.y = 0f;
			Vector3 to = this.restrectionTransform.position - base.transform.position;
			to.y = 0f;
			if (Vector3.Angle(from, to) > 90f)
			{
				vector = Vector3.zero;
			}
		}
		vector = Quaternion.Inverse(base.transform.rotation) * vector;
		if (this.walkMultiplier != 1f && (AnimationHandler.instance == null || (!AnimationHandler.instance.engagedMode && !AnimationHandler.instance.stealthMode)) && (Input.GetKey(KeyCode.LeftControl) || (this.forceWalk && (Application.loadedLevelName != "part1" || (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.JoystickButton3)))) || flag) != this.defaultIsWalk)
		{
			vector *= this.walkMultiplier;
		}
		this.motor.desiredMovementDirection = vector;
	}

	// Token: 0x04000139 RID: 313
	private CharacterMotorScript motor;

	// Token: 0x0400013A RID: 314
	public float walkMultiplier = 0.5f;

	// Token: 0x0400013B RID: 315
	public bool defaultIsWalk;

	// Token: 0x0400013C RID: 316
	public bool forceWalk;

	// Token: 0x0400013D RID: 317
	public GameObject joystickPrefab;

	// Token: 0x0400013E RID: 318
	public MobileInput.InputModes inputMode;

	// Token: 0x0400013F RID: 319
	private Vector3 previousDirectionVector;

	// Token: 0x04000140 RID: 320
	private BasicAgility basicAgility;

	// Token: 0x04000141 RID: 321
	private AnimationHandler animationHandler;

	// Token: 0x04000142 RID: 322
	public bool acceptUserInput = true;

	// Token: 0x04000143 RID: 323
	private ShooterGameCamera shooterCamera;

	// Token: 0x04000144 RID: 324
	[HideInInspector]
	public static Joystick joystickLeft;

	// Token: 0x04000145 RID: 325
	private Joystick joystickRight;

	// Token: 0x04000146 RID: 326
	public Transform restrectionTransform;

	// Token: 0x04000147 RID: 327
	public float restrectionDistance;
}
