using System;
using UnityEngine;

// Token: 0x0200023A RID: 570
[RequireComponent(typeof(Wheels))]
public abstract class CarController : MonoBehaviour
{
	// Token: 0x06000AD1 RID: 2769
	protected abstract void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out int targetGear);

	// Token: 0x06000AD2 RID: 2770 RVA: 0x000811B4 File Offset: 0x0007F3B4
	public void SetDebug(bool db)
	{
		this.debug = db;
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x000811C0 File Offset: 0x0007F3C0
	private void Awake()
	{
		this.body = base.GetComponent<Rigidbody>();
		this.cardynamics = base.GetComponent<CarDynamics>();
		this.drivetrain = base.GetComponent<Drivetrain>();
		this.cardamage = base.GetComponent<CarDamage>();
		this.wheels = base.GetComponent<Wheels>();
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0008120C File Offset: 0x0007F40C
	private void Start()
	{
		if (this.wheels == null)
		{
			Debug.LogError("Wheels Script is needed in order to use UnityCar");
			this.ok = false;
			Debug.Break();
		}
		else if (this.wheels.frontLeftWheel == null)
		{
			Debug.LogError("wheels.frontLeftWheel must be assigned to the front left car wheel");
			this.ok = false;
			Debug.Break();
		}
		else if (this.wheels.frontRightWheel == null)
		{
			Debug.LogError("wheels.frontRightWheel must be assigned to the right left car wheel");
			this.ok = false;
			Debug.Break();
		}
		else if (this.wheels.rearLeftWheel == null)
		{
			Debug.LogError("wheels.rearLeftWheel must be assigned to the rear left car wheel");
			this.ok = false;
			Debug.Break();
		}
		else if (this.wheels.rearRightWheel == null)
		{
			Debug.LogError("wheels.rearRightWheel must be assigned to the rear right car wheel");
			this.ok = false;
			Debug.Break();
		}
		else
		{
			this.allWheels = this.wheels.allWheels;
			this.engageRPM = this.drivetrain.engageRPM;
			this.shiftDownRPM = this.drivetrain.shiftDownRPM;
			this.shiftDownRPMHandBrake = this.drivetrain.shiftDownRPM + this.drivetrain.shiftDownRPM / 2f;
			this.oldTCSValue = this.TCS;
			this.oldESPValue = this.ESP;
			this.oldVeloSteerTime = this.veloSteerTime;
			this.oldVeloSteerReleaseTime = this.veloSteerReleaseTime;
			this.maxSteer = 1f;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.tag == "DashBoard" || transform.gameObject.name == "DashBoard")
				{
					this.dashBoard = transform.gameObject;
				}
			}
		}
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00081438 File Offset: 0x0007F638
	private float Lerp(float from, float to, float value)
	{
		if (value < 0f)
		{
			return from;
		}
		if (value > 1f)
		{
			return to;
		}
		return (to - from) * value + from;
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0008145C File Offset: 0x0007F65C
	private void Update()
	{
		if (this.ok)
		{
			int num;
			if (Time.timeSinceLevelLoad > 3f)
			{
				this.GetInput(out this.throttleInput, out this.brakeInput, out this.steerInput, out this.handbrakeInput, out this.clutchInput, out num);
			}
			else
			{
				num = 0;
			}
			if (Input.GetKey(KeyCode.JoystickButton1))
			{
				this.handbrakeInput = 1f;
			}
			if (!this.drivetrain.ChangingGear && num != this.drivetrain.gear)
			{
				this.drivetrain.Shift(num);
			}
			if (this.cardynamics.arcadeMode)
			{
				this.veloSteerTime = this.oldVeloSteerTime + 0.01f;
				this.veloSteerReleaseTime = this.oldVeloSteerReleaseTime + 0.01f;
				this.ESP = false;
				this.ESPTriggered = false;
			}
			if (this.drivetrain.automatic)
			{
				if (this.brakeInput > 0f && this.fVelo <= 0.5f)
				{
					this.reverse = true;
					if (this.drivetrain.gear != this.drivetrain.firstReverse)
					{
						this.drivetrain.Shift(this.drivetrain.firstReverse);
					}
				}
				if (this.throttleInput > 0f && this.fVelo <= 0.5f)
				{
					this.reverse = false;
					if (this.drivetrain.gear != this.drivetrain.first)
					{
						this.drivetrain.Shift(this.drivetrain.first);
					}
				}
				if (this.reverse)
				{
					float num2 = this.throttleInput;
					this.throttleInput = this.brakeInput;
					this.brakeInput = num2;
				}
			}
			this.brakeKey = (this.brakeInput > 0f);
			this.accelKey = (this.throttleInput > 0f);
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0008164C File Offset: 0x0007F84C
	private void FixedUpdate()
	{
		this.fVelo = this.cardynamics.velo;
		this.veloKmh = this.fVelo * 3.6f;
		bool flag = this.drivetrain.OnGround();
		if (this.smoothInput)
		{
			if (this.steerAssistance && flag && this.drivetrain.gearRatios[this.drivetrain.gear] > 0f && this.veloKmh > this.SteerAssistanceMinVelocity)
			{
				this.doSteerAssistance();
			}
			else
			{
				this.doNormalSteering();
			}
		}
		else
		{
			this.steering = this.steerInput;
			this.steerAssistance = false;
		}
		this.steering = Mathf.Clamp(this.steering, -this.maxSteer, this.maxSteer);
		if (this.ESP && flag && this.veloKmh > this.ESPMinVelocity)
		{
			this.DoESP();
		}
		else
		{
			this.maxThrottle = 1f;
			this.ESPTriggered = false;
		}
		if (this.smoothInput)
		{
			if (this.throttleInput > 0f && (!this.drivetrain.ChangingGear || !this.drivetrain.automatic))
			{
				if (this.throttleInput < this.throttle)
				{
					this.throttle -= Time.deltaTime / this.throttleReleaseTime;
					if (this.throttleInput > this.throttle)
					{
						this.throttle = this.throttleInput;
					}
				}
				else if (this.throttleInput > this.throttle)
				{
					this.throttle += Time.deltaTime / this.throttleTime;
					if (this.throttleInput < this.throttle)
					{
						this.throttle = this.throttleInput;
					}
				}
			}
			else
			{
				this.throttle -= Time.deltaTime / this.throttleReleaseTime;
			}
		}
		else if (!this.drivetrain.ChangingGear || !this.drivetrain.automatic)
		{
			this.throttle = this.throttleInput;
		}
		else
		{
			this.throttle = 0f;
		}
		this.TCSTriggered = false;
		if (this.TCS && this.drivetrain.ratio != 0f && this.drivetrain.clutchPosition != 0f && flag && this.veloKmh > this.TCSMinVelocity)
		{
			this.DoTCS();
		}
		else
		{
			this.TCSTriggered = false;
		}
		if (this.drivetrain.gearRatios[this.drivetrain.gear] < 0f)
		{
			this.throttle = Mathf.Clamp(this.throttle, this.drivetrain.idlethrottle, this.maxThrottleInReverse);
		}
		else
		{
			this.throttle = Mathf.Clamp(this.throttle, this.drivetrain.idlethrottle, Mathf.Min(this.maxThrottle, this.drivetrain.maxThrottle));
		}
		if (this.smoothInput)
		{
			if (this.brakeInput > 0f)
			{
				if (this.brakeInput < this.brake)
				{
					this.brake -= Time.deltaTime / this.BrakesReleaseTime;
					if (this.brakeInput > this.brake)
					{
						this.brake = this.brakeInput;
					}
				}
				else if (this.brakeInput > this.brake)
				{
					this.brake += Time.deltaTime / this.BrakesTime;
					if (this.brakeInput < this.brake)
					{
						this.brake = this.brakeInput;
					}
				}
			}
			else
			{
				this.brake -= Time.deltaTime / this.BrakesReleaseTime;
			}
		}
		else
		{
			this.brake = this.brakeInput;
		}
		this.brake = Mathf.Clamp01(this.brake);
		this.ABSTriggered = false;
		if (this.ABS && this.brake > 0f && this.veloKmh > this.ABSMinVelocity && flag)
		{
			this.DoABS();
		}
		else
		{
			this.ABSTriggered = false;
		}
		if (this.startTimer)
		{
			this.timer += Time.deltaTime;
			if (this.timer >= 2.5f)
			{
				this.timer = 0f;
				this.startTimer = false;
				this.drivetrain.engageRPM = this.engageRPM;
				this.drivetrain.shiftDownRPM = this.shiftDownRPM;
			}
		}
		if (this.handbrakeInput != 0f)
		{
			this.startTimer = true;
			if (this.steerAssistance)
			{
				this.steerAssistance = false;
				this.steerAssistanceStateChanged = true;
			}
			if (this.ESP)
			{
				this.ESP = false;
				this.ESPStateChanged = true;
			}
			if (this.ABS)
			{
				this.ABS = false;
				this.ABSStateChanged = true;
			}
			this.drivetrain.shiftDownRPM = this.shiftDownRPMHandBrake;
			this.drivetrain.clutch.SetClutchPosition(0f);
			if (this.TCS)
			{
				this.bothDirections = true;
				this.TCSStateChanged = true;
			}
			if (this.cardynamics.arcadeMode)
			{
				this.cardynamics.arcadeMode = false;
				this.arcadeModeStateChanged = true;
			}
		}
		else
		{
			if (this.steerAssistanceStateChanged && !this.startTimer)
			{
				this.steerAssistance = !this.steerAssistance;
				this.steerAssistanceStateChanged = false;
			}
			if (this.ESPStateChanged && !this.startTimer)
			{
				this.ESP = !this.ESP;
				this.ESPStateChanged = false;
			}
			if (this.ABSStateChanged)
			{
				this.ABS = !this.ABS;
				this.ABSStateChanged = false;
			}
			if (this.TCSStateChanged && !this.startTimer)
			{
				this.bothDirections = true;
				this.TCSStateChanged = false;
			}
			if (this.arcadeModeStateChanged && !this.startTimer)
			{
				this.cardynamics.arcadeMode = !this.cardynamics.arcadeMode;
				this.arcadeModeStateChanged = false;
			}
		}
		foreach (Wheel wheel in this.allWheels)
		{
			if (!this.ABS || this.veloKmh <= this.ABSMinVelocity || this.brakeInput <= 0f)
			{
				wheel.brake = this.brake;
			}
			wheel.handbrake = this.handbrakeInput;
			wheel.steering = this.steering;
		}
		this.drivetrain.throttle = this.throttle;
		this.drivetrain.clutch.SetClutchPosition(1f - this.clutchInput);
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00081D3C File Offset: 0x0007FF3C
	private void doSteerAssistance()
	{
		float num = Mathf.Abs(this.allWheels[0].slipAngle / this.allWheels[0].idealSlipAngle);
		float num2 = Mathf.Abs(this.allWheels[1].slipAngle / this.allWheels[1].idealSlipAngle);
		if (num > 1f || num2 > 1f)
		{
			float num3 = (this.allWheels[0].slipAngle + this.allWheels[1].slipAngle) / 2f;
			float num4 = 1f / (this.steerTime + this.veloSteerTime * this.fVelo);
			if (num3 < 0f)
			{
				this.steering += num4 * Time.deltaTime;
			}
			else
			{
				this.steering -= num4 * Time.deltaTime;
			}
		}
		else
		{
			this.doNormalSteering();
		}
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00081E24 File Offset: 0x00080024
	private void doNormalSteering()
	{
		this.steerInput = Mathf.Min(this.maxSteer, this.steerInput);
		if (this.steerInput < this.steering)
		{
			float num = (this.steering <= 0f) ? (1f / (this.steerTime + this.veloSteerTime * this.fVelo)) : (1f / (this.steerReleaseTime + this.veloSteerReleaseTime * this.fVelo));
			this.steering -= num * Time.deltaTime;
			if (this.steerInput > this.steering)
			{
				this.steering = this.steerInput;
			}
		}
		else if (this.steerInput > this.steering)
		{
			float num = (this.steering >= 0f) ? (1f / (this.steerTime + this.veloSteerTime * this.fVelo)) : (1f / (this.steerReleaseTime + this.veloSteerReleaseTime * this.fVelo));
			this.steering += num * Time.deltaTime;
			if (this.steerInput < this.steering)
			{
				this.steering = this.steerInput;
			}
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00081F68 File Offset: 0x00080168
	private void DoABS()
	{
		foreach (Wheel wheel in this.allWheels)
		{
			float num = -wheel.longitudinalSlip;
			if (num >= 1f + this.ABSThreshold)
			{
				wheel.brake = 0f;
				this.ABSTriggered = true;
			}
			else
			{
				wheel.brake = this.brake;
				this.ABSTriggered = false;
			}
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00081FD8 File Offset: 0x000801D8
	private void DoTCS()
	{
		foreach (Wheel wheel in this.allWheels)
		{
			float longitudinalSlip = wheel.longitudinalSlip;
			float num = Mathf.Abs(wheel.lateralSlip);
			bool flag;
			float num2;
			if (this.bothDirections)
			{
				flag = (longitudinalSlip >= 1f + this.TCSThreshold || num >= 1f + this.TCSThreshold);
				num2 = Mathf.Max(longitudinalSlip, num);
			}
			else
			{
				flag = (longitudinalSlip >= 1f + this.TCSThreshold);
				num2 = longitudinalSlip;
			}
			if (flag)
			{
				this.throttle -= num2;
				this.TCSTriggered = true;
				break;
			}
			this.TCSTriggered = false;
		}
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0008209C File Offset: 0x0008029C
	private void DoESP()
	{
		Vector3 forward = base.transform.forward;
		Vector3 vector = this.body.velocity;
		vector -= base.transform.up * Vector3.Dot(vector, base.transform.up);
		vector.Normalize();
		if (this.fVelo < 1f)
		{
			this.angle = 0f;
		}
		else
		{
			this.angle = -Mathf.Asin(Vector3.Dot(Vector3.Cross(forward, vector), base.transform.up));
		}
		this.ESPTriggered = false;
		if (this.angle > 0.1f)
		{
			this.wheels.frontLeftWheel.brake = Mathf.Clamp01(this.wheels.frontLeftWheel.brake + Mathf.Abs(this.angle) * this.ESPStrength);
			this.maxThrottle = Mathf.Max(this.throttle / 2f, this.drivetrain.idlethrottle);
			this.ESPTriggered = true;
		}
		else if (this.angle < -0.1f)
		{
			this.wheels.frontRightWheel.brake = Mathf.Clamp01(this.wheels.frontRightWheel.brake + Mathf.Abs(this.angle) * this.ESPStrength);
			this.maxThrottle = Mathf.Max(this.throttle / 2f, this.drivetrain.idlethrottle);
			this.ESPTriggered = true;
		}
		else
		{
			this.maxThrottle = 1f;
			this.ESPTriggered = false;
		}
	}

	// Token: 0x040011E5 RID: 4581
	protected bool debug;

	// Token: 0x040011E6 RID: 4582
	private bool ok = true;

	// Token: 0x040011E7 RID: 4583
	private float timer;

	// Token: 0x040011E8 RID: 4584
	protected Wheel[] allWheels;

	// Token: 0x040011E9 RID: 4585
	protected float brake;

	// Token: 0x040011EA RID: 4586
	protected float throttle;

	// Token: 0x040011EB RID: 4587
	protected float steering;

	// Token: 0x040011EC RID: 4588
	private float maxSteer;

	// Token: 0x040011ED RID: 4589
	private float maxThrottle = 1f;

	// Token: 0x040011EE RID: 4590
	[HideInInspector]
	public bool brakeKey;

	// Token: 0x040011EF RID: 4591
	[HideInInspector]
	public bool accelKey;

	// Token: 0x040011F0 RID: 4592
	private bool bothDirections = true;

	// Token: 0x040011F1 RID: 4593
	protected float steerInput;

	// Token: 0x040011F2 RID: 4594
	protected float brakeInput;

	// Token: 0x040011F3 RID: 4595
	protected float throttleInput;

	// Token: 0x040011F4 RID: 4596
	[HideInInspector]
	public float handbrakeInput;

	// Token: 0x040011F5 RID: 4597
	[HideInInspector]
	public float clutchInput;

	// Token: 0x040011F6 RID: 4598
	protected Rigidbody body;

	// Token: 0x040011F7 RID: 4599
	protected Drivetrain drivetrain;

	// Token: 0x040011F8 RID: 4600
	protected CarDynamics cardynamics;

	// Token: 0x040011F9 RID: 4601
	protected CarDamage cardamage;

	// Token: 0x040011FA RID: 4602
	protected Wheels wheels;

	// Token: 0x040011FB RID: 4603
	protected float engageRPM;

	// Token: 0x040011FC RID: 4604
	protected float shiftDownRPM;

	// Token: 0x040011FD RID: 4605
	protected float shiftDownRPMHandBrake;

	// Token: 0x040011FE RID: 4606
	protected float fVelo;

	// Token: 0x040011FF RID: 4607
	private float veloKmh;

	// Token: 0x04001200 RID: 4608
	public bool enableKeys = true;

	// Token: 0x04001201 RID: 4609
	public bool smoothInput = true;

	// Token: 0x04001202 RID: 4610
	public float throttleTime = 0.1f;

	// Token: 0x04001203 RID: 4611
	public float throttleReleaseTime = 0.01f;

	// Token: 0x04001204 RID: 4612
	public float maxThrottleInReverse = 1f;

	// Token: 0x04001205 RID: 4613
	public float BrakesTime = 0.5f;

	// Token: 0x04001206 RID: 4614
	public float BrakesReleaseTime = 0.1f;

	// Token: 0x04001207 RID: 4615
	public float steerTime = 0.1f;

	// Token: 0x04001208 RID: 4616
	public float steerReleaseTime = 0.1f;

	// Token: 0x04001209 RID: 4617
	public float veloSteerTime = 0.05f;

	// Token: 0x0400120A RID: 4618
	public float veloSteerReleaseTime = 0.05f;

	// Token: 0x0400120B RID: 4619
	public bool steerAssistance = true;

	// Token: 0x0400120C RID: 4620
	public float SteerAssistanceMinVelocity = 20f;

	// Token: 0x0400120D RID: 4621
	private bool steerAssistanceStateChanged;

	// Token: 0x0400120E RID: 4622
	private float oldVeloSteerTime;

	// Token: 0x0400120F RID: 4623
	private float oldVeloSteerReleaseTime;

	// Token: 0x04001210 RID: 4624
	protected bool oldTCSValue;

	// Token: 0x04001211 RID: 4625
	protected bool oldESPValue;

	// Token: 0x04001212 RID: 4626
	public bool TCS = true;

	// Token: 0x04001213 RID: 4627
	[HideInInspector]
	public bool TCSTriggered;

	// Token: 0x04001214 RID: 4628
	public float TCSThreshold;

	// Token: 0x04001215 RID: 4629
	public float TCSMinVelocity = 20f;

	// Token: 0x04001216 RID: 4630
	public bool ABS = true;

	// Token: 0x04001217 RID: 4631
	[HideInInspector]
	public bool ABSTriggered;

	// Token: 0x04001218 RID: 4632
	public float ABSThreshold;

	// Token: 0x04001219 RID: 4633
	public float ABSMinVelocity = 20f;

	// Token: 0x0400121A RID: 4634
	public bool ESP = true;

	// Token: 0x0400121B RID: 4635
	[HideInInspector]
	public bool ESPTriggered;

	// Token: 0x0400121C RID: 4636
	public float ESPStrength = 2f;

	// Token: 0x0400121D RID: 4637
	public float ESPMinVelocity = 35f;

	// Token: 0x0400121E RID: 4638
	private float angle;

	// Token: 0x0400121F RID: 4639
	private float angularVelo;

	// Token: 0x04001220 RID: 4640
	[HideInInspector]
	public bool reverse;

	// Token: 0x04001221 RID: 4641
	private bool startTimer;

	// Token: 0x04001222 RID: 4642
	private bool ESPStateChanged;

	// Token: 0x04001223 RID: 4643
	private bool ABSStateChanged;

	// Token: 0x04001224 RID: 4644
	private bool TCSStateChanged;

	// Token: 0x04001225 RID: 4645
	private bool arcadeModeStateChanged;

	// Token: 0x04001226 RID: 4646
	protected GameObject dashBoard;
}
