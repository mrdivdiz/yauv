using System;
using UnityEngine;

// Token: 0x02000257 RID: 599
[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacterControlZOMBIE : MonoBehaviour
{
	// Token: 0x06000B5B RID: 2907 RVA: 0x0008EAD0 File Offset: 0x0008CCD0
	private void Start()
	{
		if (this.computer == null)
		{
			this.computer = (base.GetComponent(typeof(RootMotionComputer)) as RootMotionComputer);
		}
		if (this.character == null)
		{
			this.character = (base.GetComponent(typeof(CharacterController)) as CharacterController);
		}
		this.computer.applyMotion = false;
		this.computer.isManagedExternally = true;
		this.computer.computationMode = RootMotionComputationMode.ZTranslation;
		this.computer.Initialize();
		base.GetComponent<Animation>()["idle"].layer = 0;
		base.GetComponent<Animation>()["idle"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["walk01"].layer = 1;
		base.GetComponent<Animation>()["walk01"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["run"].layer = 1;
		base.GetComponent<Animation>()["run"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["attack"].layer = 3;
		base.GetComponent<Animation>()["attack"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["headbutt"].layer = 3;
		base.GetComponent<Animation>()["headbutt"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["scratchidle"].layer = 3;
		base.GetComponent<Animation>()["scratchidle"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["walk02"].layer = 3;
		base.GetComponent<Animation>()["walk02"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["standup"].layer = 3;
		base.GetComponent<Animation>()["standup"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("idle");
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0008ECD8 File Offset: 0x0008CED8
	private void Update()
	{
		float num = 0f;
		float num2 = 0f;
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Rotate(Vector3.down, this.turningSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Rotate(Vector3.up, this.turningSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.W) && (base.GetComponent<Animation>()["walk01"].weight == 0f || base.GetComponent<Animation>()["run"].weight == 0f))
		{
			base.GetComponent<Animation>()["walk01"].normalizedTime = 0f;
			base.GetComponent<Animation>()["run"].normalizedTime = 0f;
		}
		if (Input.GetKey(KeyCode.W))
		{
			num = 1f;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			num2 = 1f;
		}
		base.GetComponent<Animation>().Blend("run", num * num2, 0.5f);
		base.GetComponent<Animation>().Blend("walk01", num * (1f - num2), 0.5f);
		base.GetComponent<Animation>().SyncLayer(1);
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<Animation>().CrossFade("attack", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<Animation>().CrossFade("headbutt", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<Animation>().CrossFade("scratchidle", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			base.GetComponent<Animation>().CrossFade("walk02", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			base.GetComponent<Animation>().CrossFade("standup", 0.2f);
		}
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0008EEDC File Offset: 0x0008D0DC
	private void LateUpdate()
	{
		this.computer.ComputeRootMotion();
		this.character.SimpleMove(base.transform.TransformDirection(this.computer.deltaPosition) / Time.deltaTime);
	}

	// Token: 0x04001495 RID: 5269
	public float turningSpeed = 90f;

	// Token: 0x04001496 RID: 5270
	public RootMotionComputer computer;

	// Token: 0x04001497 RID: 5271
	public CharacterController character;
}
