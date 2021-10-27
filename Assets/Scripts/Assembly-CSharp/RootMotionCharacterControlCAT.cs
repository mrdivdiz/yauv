using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacterControlCAT : MonoBehaviour
{
	// Token: 0x06000110 RID: 272 RVA: 0x000074C4 File Offset: 0x000056C4
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
		base.GetComponent<Animation>()["walk"].layer = 1;
		base.GetComponent<Animation>()["walk"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["run"].layer = 1;
		base.GetComponent<Animation>()["run"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["playing"].layer = 3;
		base.GetComponent<Animation>()["playing"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["sniff"].layer = 3;
		base.GetComponent<Animation>()["sniff"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["swiping"].layer = 3;
		base.GetComponent<Animation>()["swiping"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["lay"].layer = 3;
		base.GetComponent<Animation>()["lay"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().Play("idle");
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000076A0 File Offset: 0x000058A0
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
		if (Input.GetKeyDown(KeyCode.W) && (base.GetComponent<Animation>()["walk"].weight == 0f || base.GetComponent<Animation>()["run"].weight == 0f))
		{
			base.GetComponent<Animation>()["walk"].normalizedTime = 0f;
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
		base.GetComponent<Animation>().Blend("walk", num * (1f - num2), 0.5f);
		base.GetComponent<Animation>().SyncLayer(1);
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			base.GetComponent<Animation>().CrossFade("playing", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			base.GetComponent<Animation>().CrossFade("sniff", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			base.GetComponent<Animation>().CrossFade("swiping", 0.2f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			base.GetComponent<Animation>().CrossFade("lay", 0.2f);
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00007884 File Offset: 0x00005A84
	private void LateUpdate()
	{
		this.computer.ComputeRootMotion();
		this.character.SimpleMove(base.transform.TransformDirection(this.computer.deltaPosition) / Time.deltaTime);
	}

	// Token: 0x040000FA RID: 250
	public float turningSpeed = 90f;

	// Token: 0x040000FB RID: 251
	public RootMotionComputer computer;

	// Token: 0x040000FC RID: 252
	public CharacterController character;
}
