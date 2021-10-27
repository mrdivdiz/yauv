using System;
using UnityEngine;

// Token: 0x02000243 RID: 579
[RequireComponent(typeof(Wheels))]
public class Drivetrain : MonoBehaviour
{
	// Token: 0x06000AF6 RID: 2806 RVA: 0x00085E04 File Offset: 0x00084004
	public void SetDebug(bool db)
	{
		this.debug = db;
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00085E10 File Offset: 0x00084010
	private float Sqr(float x)
	{
		return x * x;
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00085E18 File Offset: 0x00084018
	private void Awake()
	{
		this.body = base.GetComponent<Rigidbody>();
		this.clutch = new Clutch();
		this.carcontroller = base.GetComponent<CarController>();
		this.wheels = base.GetComponent<Wheels>();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00085E4C File Offset: 0x0008404C
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
			this.SetTransmission();
			this.StartEngine();
			bool flag = false;
			for (int i = 0; i < this.gearRatios.Length; i++)
			{
				if (this.gearRatios[i] == 0f)
				{
					this.neutral = i;
					this.first = this.neutral + 1;
					this.firstReverse = this.neutral - 1;
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.LogError("Neutral gear (a gear with value 0). is missing in gearRatios array. Neutral gear is mandatory");
			}
			this.mengageSpeed = this.engageSpeed;
			this.maxPowerDriveShaft = this.maxPower;
			this.maxPowerAngVel = this.maxPowerRPM * this.RPM2angularVelo;
			this.maxPowerKW = this.maxPower * this.CV2KW;
			this.maxPowerEngineTorque = this.CalcEngineTorque(1f, this.maxPowerRPM);
			this.maxPowerNet = this.CalcEnginePower(this.maxPowerRPM, true, 1f);
			this.thresholdLimit = 0.005f * Time.fixedDeltaTime / 0.02f;
			this.rpmFactor = 1000000f / (Time.fixedDeltaTime / 0.02f);
			this.CalcengineMaxTorque();
			if (this.clutchMaxTorque == 0f)
			{
				this.clutchMaxTorque = Mathf.Round(this.maxTorqueNet * 1.6f);
			}
			this.clutch.maxTorque = this.clutchMaxTorque;
			this.clutch.maxPressure = this.clutch.maxTorque / (this.clutch.slidingFriction * this.clutch.area * this.clutch.radius);
			this.CalcIdleThrottle();
			this.DisengageClutch();
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x000860D8 File Offset: 0x000842D8
	public void SetTransmission()
	{
		foreach (Wheel wheel in this.wheels.allWheels)
		{
			wheel.driveTorque = 0f;
		}
		if (this.transmission == Drivetrain.Transmissions.FWD)
		{
			this.poweredWheels = this.wheels.frontWheels;
		}
		else if (this.transmission == Drivetrain.Transmissions.RWD)
		{
			this.poweredWheels = this.wheels.rearWheels;
		}
		else if (this.transmission == Drivetrain.Transmissions.AWD)
		{
			this.poweredWheels = this.wheels.allWheels;
		}
		this.drivetrainFraction = 1f / (float)this.poweredWheels.Length;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0008618C File Offset: 0x0008438C
	private int FindRightPoint(float RPM)
	{
		int i;
		for (i = 0; i <= this.torqueRPMValuesLen - 1; i++)
		{
			if (this.torqueRPMValues[i, 0] > RPM)
			{
				break;
			}
		}
		return i;
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x000861CC File Offset: 0x000843CC
	private float CalcEngineTorque(float factor, float RPM)
	{
		if (this.loadExternalTorqueData)
		{
			return this.CalcEngineTorqueExt(factor, RPM);
		}
		return this.CalcEngineTorqueInt(factor, RPM);
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x000861EC File Offset: 0x000843EC
	private float CalcEngineTorqueExt(float factor, float RPM)
	{
		if (this.torqueRPMValuesLen == 0)
		{
			return 0f;
		}
		this.rp = this.FindRightPoint(RPM);
		if (this.rp == 0 || this.rp == this.torqueRPMValuesLen)
		{
			return 0f;
		}
		float num = (RPM - this.torqueRPMValues[this.rp, 0]) / (this.torqueRPMValues[this.rp - 1, 0] - this.torqueRPMValues[this.rp, 0]) * this.torqueRPMValues[this.rp - 1, 1] - (RPM - this.torqueRPMValues[this.rp - 1, 0]) / (this.torqueRPMValues[this.rp - 1, 0] - this.torqueRPMValues[this.rp, 0]) * this.torqueRPMValues[this.rp, 1];
		return num * factor;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x000862E0 File Offset: 0x000844E0
	private float CalcEngineTorqueInt(float factor, float RPM)
	{
		float num = 0f;
		float num2 = 0f;
		if (this.engine == Drivetrain.Engine.Petrol)
		{
			num = this.maxPowerKW / this.maxPowerAngVel;
			num2 = this.maxPowerKW / (this.maxPowerAngVel * this.maxPowerAngVel);
		}
		else if (this.engine == Drivetrain.Engine.Diesel)
		{
			num = 0.87f * this.maxPowerKW / this.maxPowerAngVel;
			num2 = 1.13f * this.maxPowerKW / (this.maxPowerAngVel * this.maxPowerAngVel);
		}
		float num3 = -this.maxPowerKW / (this.maxPowerAngVel * this.maxPowerAngVel * this.maxPowerAngVel);
		float num4 = RPM * this.RPM2angularVelo;
		float num5 = num + num2 * num4 + num3 * (num4 * num4);
		if (RPM > this.maxPowerRPM && !this.revLimiter)
		{
			num5 *= 1f - (RPM - this.maxPowerRPM) * 0.001f;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
		}
		return num5 * 1000f * factor;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x000863EC File Offset: 0x000845EC
	private float CalcEngineTorque_oo(float factor, float RPM)
	{
		float num = 0f;
		if (RPM >= 400f && RPM <= 1500f)
		{
			num = (RPM - 1500f) / -1100f * 0.45f - (RPM - 400f) / -1100f * 0.8f;
		}
		else if (RPM > 1500f && RPM <= 4000f)
		{
			num = (RPM - 4000f) / -2500f * 0.8f - (RPM - 1500f) / -2500f * 1f;
		}
		else if (RPM > 4000f && RPM <= 5500f)
		{
			num = 1f;
		}
		else if (RPM > 5500f && RPM <= 7000f)
		{
			num = (RPM - 7000f) / -1500f * 1f - (RPM - 5500f) / -1500f * 0.9f;
		}
		else if (RPM > 7000f && RPM <= 8000f)
		{
			num = (RPM - 8000f) / -1000f * 0.9f - (RPM - 7000f) / -1000f * 0.9f;
		}
		else if (RPM > 8000f && RPM <= 9000f)
		{
			num = (RPM - 9000f) / -1000f * 0.9f - (RPM - 8000f) / -1000f * 0.75f;
		}
		else if (RPM > 9000f && RPM <= 10000f)
		{
			num = (RPM - 10000f) / -1000f * 0.75f - (RPM - 9000f) / -1000f * 0.1f;
		}
		return num * (this.maxTorque * factor);
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x000865BC File Offset: 0x000847BC
	private float CalcEngineFrictionTorque(float factor, float rpm)
	{
		return this.maxPowerEngineTorque * factor * this.engineFrictionFactor * (0.1f + 0.9f * rpm / this.maxRPM);
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x000865F0 File Offset: 0x000847F0
	private float CalcEnginePower(float rpm, bool total, float factor)
	{
		if (total)
		{
			return (this.CalcEngineTorque(factor, rpm) - this.CalcEngineFrictionTorque(factor, rpm)) * rpm * this.RPM2angularVelo / 1000f * this.KW2CV;
		}
		return this.CalcEngineTorque(factor, rpm) * rpm * this.RPM2angularVelo / 1000f * this.KW2CV;
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0008664C File Offset: 0x0008484C
	public void StartEngine()
	{
		this.engineAngularVelo = this.minRPM * this.RPM2angularVelo * 1.5f;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x00086668 File Offset: 0x00084868
	private void CalcEnginePowerMax()
	{
		for (float num = this.minRPM; num < this.maxRPM; num += 1f)
		{
			float num2 = this.CalcEnginePower(num, true, 1f);
			float num3 = this.CalcEnginePower(num + 1f, true, 1f);
			if (num3 > num2)
			{
				this.maxPowerRPM = num + 1f;
				this.maxPowerNet = num3;
			}
		}
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x000866D4 File Offset: 0x000848D4
	private void CalcengineMaxTorque()
	{
		for (float num = this.minRPM; num < this.maxRPM; num += 1f)
		{
			float num2 = this.CalcEngineTorque(1f, num) - this.CalcEngineFrictionTorque(1f, num);
			float num3 = this.CalcEngineTorque(1f, num + 1f) - this.CalcEngineFrictionTorque(1f, num + 1f);
			if (num3 > num2)
			{
				this.maxTorqueRPM = num + 1f;
				this.maxTorqueNet = num3;
			}
		}
		this.maxTorque = this.CalcEngineTorque(1f, this.maxTorqueRPM);
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00086774 File Offset: 0x00084974
	public void CalcIdleThrottle()
	{
		this.idlethrottle = 0f;
		while (this.idlethrottle < 1f)
		{
			if (this.CalcEngineTorque(this.powerMultiplier, this.minRPM) * this.idlethrottle >= this.CalcEngineFrictionTorque(this.powerMultiplier, this.minRPM))
			{
				break;
			}
			this.idlethrottle += 0.0001f;
		}
		this.idlethrottleMinRPMDown = this.idlethrottle;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x000867F4 File Offset: 0x000849F4
	private void FixedUpdate()
	{
		if (this.ok)
		{
			if (this.clutch == null)
			{
				this.clutch = new Clutch();
			}
			if (Application.isEditor)
			{
				this.clutch.maxTorque = this.clutchMaxTorque;
				this.clutch.maxPressure = this.clutch.maxTorque / (this.clutch.slidingFriction * this.clutch.area * this.clutch.radius);
			}
			this.ratio = this.gearRatios[this.gear] * this.finalDriveRatio;
			if (this.rpm <= this.minRPM + 500f)
			{
				this.idlethrottle = this.idlethrottleMinRPMDown * ((this.minRPM + 500f - this.rpm) / 500f);
			}
			else
			{
				this.idlethrottle = 0f;
			}
			this.currentPower = this.CalcEnginePower(this.rpm, true, this.powerMultiplier);
			this.engineInertia = (this.rpm - this.maxRPM) / (this.minRPM - this.maxRPM) * this.engineInertiaMax - (this.rpm - this.minRPM) / (this.minRPM - this.maxRPM) * this.engineInertiaMin;
			if (this.powerMultiplier < 1f)
			{
				this.engineInertia *= this.powerMultiplier;
			}
			this.velo = this.body.velocity.magnitude;
			if ((this.rpm > this.engageRPM || this.engaging) && this.autoClutch && this.carcontroller.clutchInput == 0f && this.clutchPosition != 1f && this.carcontroller.handbrakeInput == 0f && this.ratio != 0f && !this.ChangingGear)
			{
				this.EngageClutch();
			}
			else
			{
				this.engaging = false;
			}
			if (this.rpm <= this.disengageRPM && !this.engaging && this.autoClutch)
			{
				this.DisengageClutch();
			}
			if (this.ChangingGear)
			{
				this.DoGearShifting();
			}
			if (this.automatic)
			{
				this.autoClutch = true;
				if (!this.CanShiftAgain)
				{
					this.TimeToShiftAgain = Mathf.Clamp01((Time.time - this.ShiftDelay) / (this.shiftSpeed + this.shiftSpeed / 2f));
					if (this.TimeToShiftAgain >= 1f)
					{
						this.CanShiftAgain = true;
					}
				}
				if (!this.ChangingGear)
				{
					if (this.gearRatios[this.gear] < 0f && this.clutchPosition == 0f && !this.carcontroller.reverse)
					{
						this.gear = this.first;
					}
					if (this.gear == this.neutral && this.rpm > this.minRPM + 100f)
					{
						this.throttle = 0f;
						if (this.carcontroller.reverse)
						{
							this.ShiftDown();
						}
						else
						{
							this.ShiftUp();
						}
					}
					if (this.gear == this.first && this.clutchPosition == 0f && this.carcontroller.reverse)
					{
						this.gear = this.neutral;
					}
					if (this.rpm >= this.maxPowerRPM && this.clutch.IsLocked() && Mathf.Abs(this.slipRatio) / this.idealSlipRatio <= 1f)
					{
						if (this.CanShiftAgain && this.OnGround())
						{
							this.throttle = 0f;
							if (this.gearRatios[this.gear] > 0f)
							{
								this.ShiftUp();
							}
							else
							{
								this.ShiftDown();
							}
						}
						this.CanShiftAgain = false;
						this.ShiftDelay = Time.time;
					}
					else if (this.rpm <= this.shiftDownRPM && this.gear != this.first && this.gear != this.firstReverse && this.gear != this.neutral && this.CanShiftAgain && this.OnGround())
					{
						this.throttle = 0f;
						if (this.velo < 3f)
						{
							this.Shift(this.first);
						}
						else if (this.gearRatios[this.gear] > 0f)
						{
							this.ShiftDown();
						}
						else
						{
							this.ShiftUp();
						}
						this.CanShiftAgain = false;
						this.ShiftDelay = Time.time;
					}
				}
			}
			float num = 0f;
			foreach (Wheel wheel in this.poweredWheels)
			{
				num += wheel.angularVelocity * this.drivetrainFraction;
			}
			this.driveShaftSpeed = num * this.finalDriveRatio;
			this.clutchSpeed = this.driveShaftSpeed * this.gearRatios[this.gear];
			this.torque = this.CalcEngineTorque(this.powerMultiplier, this.rpm) * this.throttle;
			this.frictionTorque = this.CalcEngineFrictionTorque(this.powerMultiplier, this.rpm);
			if (Application.isEditor)
			{
				this.thresholdLimit = 0.005f * Time.fixedDeltaTime / 0.02f;
				this.rpmFactor = 1000000f / (Time.fixedDeltaTime / 0.02f);
			}
			if (!this.clutch.IsLocked() && this.clutchPosition != 0f)
			{
				this.clutchThreshold = Mathf.Clamp(this.rpm / this.rpmFactor * Mathf.Max(1f, this.powerMultiplier), 0f, this.thresholdLimit);
			}
			else if (this.clutchThreshold < this.thresholdLimit)
			{
				this.clutchThreshold += Time.deltaTime * 0.1f;
			}
			else
			{
				this.clutchThreshold = this.thresholdLimit;
			}
			this.clutch.threshold = this.clutchThreshold;
			this.clutchDrag = this.clutch.GetDrag(this.engineAngularVelo, this.clutchSpeed, this.powerMultiplier);
			this.slipRatio = 0f;
			this.idealSlipRatio = 0f;
			if (this.ratio == 0f || this.clutchPosition == 0f)
			{
				if (this.autoClutch)
				{
					this.DisengageClutch();
				}
				this.ratio = 0f;
				this.torqueNet = this.torque - this.frictionTorque;
				this.engineAngularVelo += this.torqueNet / this.engineInertia * Time.deltaTime;
				foreach (Wheel wheel2 in this.poweredWheels)
				{
					this.lockingTorque = (num - wheel2.angularVelocity) * this.differentialLockCoefficient;
					wheel2.drivetrainInertia = this.drivetrainInertia;
					wheel2.driveFrictionTorque = 0f;
					wheel2.driveTorque = this.lockingTorque;
					this.slipRatio += wheel2.slipRatio * this.drivetrainFraction;
					this.idealSlipRatio += wheel2.idealSlipRatio * this.drivetrainFraction;
				}
				this.body.AddTorque(-this.engineOrientation * this.torqueNet);
			}
			else
			{
				if (this.gear == this.first || this.gear == this.firstReverse)
				{
					this.torqueNet = this.torque - this.frictionTorque - this.clutchDrag / 2f;
				}
				else
				{
					this.torqueNet = this.torque - this.frictionTorque - this.clutchDrag;
				}
				if (this.clutch.IsLocked())
				{
					this.TransferredTorque = this.torque;
					this.engineAngularVelo = this.clutchSpeed;
				}
				else
				{
					this.TransferredTorque = this.clutchDrag;
					this.engineAngularVelo += this.torqueNet / this.engineInertia * Time.deltaTime;
				}
				foreach (Wheel wheel3 in this.poweredWheels)
				{
					if (wheel3.angularVelocity * this.finalDriveRatio * this.gearRatios[this.gear] * this.angularVelo2RPM > this.maxRPM)
					{
						wheel3.angularVelocity = this.maxRPM / (this.finalDriveRatio * this.gearRatios[this.gear] * this.angularVelo2RPM);
						this.startTimer = true;
					}
					this.lockingTorque = (num - wheel3.angularVelocity) * this.differentialLockCoefficient;
					wheel3.drivetrainInertia = ((!this.clutch.IsLocked()) ? this.drivetrainInertia : (this.engineInertia + this.drivetrainInertia)) * this.Sqr(this.ratio) * this.drivetrainFraction;
					wheel3.driveFrictionTorque = this.frictionTorque * Mathf.Abs(this.ratio) * this.drivetrainFraction;
					wheel3.driveTorque = this.TransferredTorque * this.ratio * this.drivetrainFraction + this.lockingTorque;
					this.slipRatio += wheel3.slipRatio * this.drivetrainFraction;
					this.idealSlipRatio += wheel3.idealSlipRatio * this.drivetrainFraction;
				}
			}
			if (this.startTimer && this.revLimiter)
			{
				this.maxThrottle = 0f;
				this.timer += Time.deltaTime;
				if (this.timer >= this.revLimiterTime)
				{
					this.maxThrottle = 1f;
					this.timer = 0f;
					this.startTimer = false;
				}
			}
			else
			{
				this.maxThrottle = 1f;
			}
			if (this.engineAngularVelo >= this.maxRPM * this.RPM2angularVelo)
			{
				this.startTimer = true;
				this.engineAngularVelo = this.maxRPM * this.RPM2angularVelo;
			}
			else if (this.engineAngularVelo <= this.minRPM * this.RPM2angularVelo)
			{
				this.engineAngularVelo = this.minRPM * this.RPM2angularVelo;
			}
			this.rpm = this.engineAngularVelo * this.angularVelo2RPM;
			this.clutchPosition = this.clutch.GetClutchPosition();
		}
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x000872C8 File Offset: 0x000854C8
	private void DoGearShifting()
	{
		this.shiftTime = Mathf.Clamp01((Time.time - this.lastShiftTime) / this.shiftSpeed);
		if (this.shiftTime >= 0.5f)
		{
			this.gear = this.neutral;
		}
		if (this.shiftTime >= 1f)
		{
			this.ChangingGear = false;
			this.gear = this.nextGear;
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00087334 File Offset: 0x00085534
	private void EngageClutch()
	{
		if (!this.engaging)
		{
			if (this.gear == this.first || this.gear == this.firstReverse)
			{
				this.clutchPosition = Mathf.Clamp01(this.startEngage + (this.rpm - this.engageRPM) / this.maxRPM);
				this.mengageSpeed = this.engageSpeed;
			}
			else
			{
				this.clutchPosition = 0f;
				this.mengageSpeed = this.engageSpeed * 0.25f;
			}
			this.engaging = true;
		}
		else if (this.rpm <= this.minRPM)
		{
			this.engaging = false;
		}
		else
		{
			this.clutchPosition += Time.deltaTime / this.mengageSpeed;
			this.clutchPosition = Mathf.Clamp01(this.clutchPosition);
			if (this.clutchPosition == 1f)
			{
				this.engaging = false;
			}
		}
		this.clutch.SetClutchPosition(this.clutchPosition);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00087440 File Offset: 0x00085640
	private void DisengageClutch()
	{
		this.clutchPosition = 0f;
		this.clutch.SetClutchPosition(this.clutchPosition);
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00087460 File Offset: 0x00085660
	public bool OnGround()
	{
		bool flag = false;
		if (this.poweredWheels != null)
		{
			foreach (Wheel wheel in this.poweredWheels)
			{
				flag = wheel.onGroundDown;
				if (flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x000874AC File Offset: 0x000856AC
	public void Shift(int m_gear)
	{
		if (m_gear <= this.gearRatios.Length - 1 && m_gear >= 0 && !this.ChangingGear)
		{
			if (this.autoClutch)
			{
				this.DisengageClutch();
			}
			if (this.clutchPosition <= 0.5f)
			{
				this.nextGear = m_gear;
				if (this.nextGear != this.neutral)
				{
					this.shiftTriggered = true;
				}
				if (this.nextGear == this.neutral || (this.gear == this.neutral && this.nextGear == this.first))
				{
					this.gear = this.nextGear;
				}
				else
				{
					this.lastShiftTime = Time.time;
					this.ChangingGear = true;
				}
			}
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00087578 File Offset: 0x00085778
	public void ShiftUp()
	{
		if (this.gear < this.gearRatios.Length - 1 && !this.ChangingGear)
		{
			if (this.autoClutch)
			{
				this.DisengageClutch();
			}
			if (this.clutchPosition <= 0.5f)
			{
				this.nextGear = this.gear + 1;
				if (this.nextGear != this.neutral)
				{
					this.shiftTriggered = true;
				}
				if (this.nextGear == this.neutral || (this.gear == this.neutral && this.nextGear == this.first))
				{
					this.gear = this.nextGear;
				}
				else
				{
					this.lastShiftTime = Time.time;
					this.ChangingGear = true;
				}
			}
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00087648 File Offset: 0x00085848
	public void ShiftDown()
	{
		if (this.gear > 0 && !this.ChangingGear)
		{
			if (this.autoClutch)
			{
				this.DisengageClutch();
			}
			if (this.clutchPosition <= 0.5f)
			{
				this.nextGear = this.gear - 1;
				if (this.nextGear != this.neutral)
				{
					this.shiftTriggered = true;
				}
				if (this.nextGear == this.neutral || (this.gear == this.neutral && this.nextGear == this.firstReverse))
				{
					this.gear = this.nextGear;
				}
				else
				{
					this.lastShiftTime = Time.time;
					this.ChangingGear = true;
				}
			}
		}
	}

	// Token: 0x040012C1 RID: 4801
	private bool debug;

	// Token: 0x040012C2 RID: 4802
	private bool ok = true;

	// Token: 0x040012C3 RID: 4803
	private string url;

	// Token: 0x040012C4 RID: 4804
	private int rp;

	// Token: 0x040012C5 RID: 4805
	private int torqueRPMValuesLen;

	// Token: 0x040012C6 RID: 4806
	private float[,] torqueRPMValues = new float[0, 0];

	// Token: 0x040012C7 RID: 4807
	private int torqueValuesCount;

	// Token: 0x040012C8 RID: 4808
	public Clutch clutch;

	// Token: 0x040012C9 RID: 4809
	[HideInInspector]
	public CarController carcontroller;

	// Token: 0x040012CA RID: 4810
	private Wheels wheels;

	// Token: 0x040012CB RID: 4811
	private Rigidbody body;

	// Token: 0x040012CC RID: 4812
	public bool loadExternalTorqueData;

	// Token: 0x040012CD RID: 4813
	public Drivetrain.Transmissions transmission;

	// Token: 0x040012CE RID: 4814
	public Drivetrain.Engine engine;

	// Token: 0x040012CF RID: 4815
	private Wheel[] poweredWheels;

	// Token: 0x040012D0 RID: 4816
	public float[] gearRatios = new float[]
	{
		-2.66f,
		0f,
		2.66f,
		1.91f,
		1.39f,
		1f,
		0.71f
	};

	// Token: 0x040012D1 RID: 4817
	[HideInInspector]
	public int neutral = 1;

	// Token: 0x040012D2 RID: 4818
	[HideInInspector]
	public int first;

	// Token: 0x040012D3 RID: 4819
	[HideInInspector]
	public int firstReverse;

	// Token: 0x040012D4 RID: 4820
	public float finalDriveRatio = 6.09f;

	// Token: 0x040012D5 RID: 4821
	public float maxPower = 210f;

	// Token: 0x040012D6 RID: 4822
	public float maxPowerRPM = 5000f;

	// Token: 0x040012D7 RID: 4823
	[HideInInspector]
	public float maxPowerNet;

	// Token: 0x040012D8 RID: 4824
	public float minRPM = 1000f;

	// Token: 0x040012D9 RID: 4825
	public float maxRPM = 6000f;

	// Token: 0x040012DA RID: 4826
	public bool revLimiter = true;

	// Token: 0x040012DB RID: 4827
	private float revLimiterTime = 0.05f;

	// Token: 0x040012DC RID: 4828
	private bool startTimer;

	// Token: 0x040012DD RID: 4829
	private float timer;

	// Token: 0x040012DE RID: 4830
	public float engageRPM = 1500f;

	// Token: 0x040012DF RID: 4831
	public float disengageRPM = 1000f;

	// Token: 0x040012E0 RID: 4832
	public float engineInertiaMin = 0.15f;

	// Token: 0x040012E1 RID: 4833
	public float engineInertiaMax = 0.5f;

	// Token: 0x040012E2 RID: 4834
	private float engineInertia = 0.3f;

	// Token: 0x040012E3 RID: 4835
	public float drivetrainInertia = 0.06f;

	// Token: 0x040012E4 RID: 4836
	public float engineFrictionFactor = 0.25f;

	// Token: 0x040012E5 RID: 4837
	public Vector3 engineOrientation = Vector3.forward;

	// Token: 0x040012E6 RID: 4838
	public float differentialLockCoefficient = 80f;

	// Token: 0x040012E7 RID: 4839
	public bool automatic = true;

	// Token: 0x040012E8 RID: 4840
	public float shiftDownRPM = 2000f;

	// Token: 0x040012E9 RID: 4841
	public float shiftSpeed = 0.5f;

	// Token: 0x040012EA RID: 4842
	public float clutchMaxTorque;

	// Token: 0x040012EB RID: 4843
	private float clutchThreshold = 0.005f;

	// Token: 0x040012EC RID: 4844
	private float thresholdLimit;

	// Token: 0x040012ED RID: 4845
	private float rpmFactor;

	// Token: 0x040012EE RID: 4846
	public bool autoClutch = true;

	// Token: 0x040012EF RID: 4847
	public float engageSpeed = 0.3f;

	// Token: 0x040012F0 RID: 4848
	private float mengageSpeed;

	// Token: 0x040012F1 RID: 4849
	[HideInInspector]
	public float clutchPosition;

	// Token: 0x040012F2 RID: 4850
	public float throttle;

	// Token: 0x040012F3 RID: 4851
	[HideInInspector]
	public float idlethrottle;

	// Token: 0x040012F4 RID: 4852
	private float idlethrottleMinRPMDown;

	// Token: 0x040012F5 RID: 4853
	[HideInInspector]
	public float maxThrottle = 1f;

	// Token: 0x040012F6 RID: 4854
	[HideInInspector]
	public bool shiftTriggered;

	// Token: 0x040012F7 RID: 4855
	private float startEngage;

	// Token: 0x040012F8 RID: 4856
	private float clutchDrag;

	// Token: 0x040012F9 RID: 4857
	private float TransferredTorque;

	// Token: 0x040012FA RID: 4858
	private float driveShaftSpeed;

	// Token: 0x040012FB RID: 4859
	private float clutchSpeed;

	// Token: 0x040012FC RID: 4860
	private bool engaging;

	// Token: 0x040012FD RID: 4861
	private float shiftTime;

	// Token: 0x040012FE RID: 4862
	private float TimeToShiftAgain;

	// Token: 0x040012FF RID: 4863
	private bool CanShiftAgain = true;

	// Token: 0x04001300 RID: 4864
	private float ShiftDelay = -1f;

	// Token: 0x04001301 RID: 4865
	private float lastShiftTime = -1f;

	// Token: 0x04001302 RID: 4866
	public int gear = 1;

	// Token: 0x04001303 RID: 4867
	public float rpm;

	// Token: 0x04001304 RID: 4868
	private float slipRatio;

	// Token: 0x04001305 RID: 4869
	private float idealSlipRatio;

	// Token: 0x04001306 RID: 4870
	private float engineAngularVelo;

	// Token: 0x04001307 RID: 4871
	private float angularVelo2RPM = 9.549296f;

	// Token: 0x04001308 RID: 4872
	private float RPM2angularVelo = 0.10471976f;

	// Token: 0x04001309 RID: 4873
	private float KW2CV = 1.359f;

	// Token: 0x0400130A RID: 4874
	private float CV2KW = 0.7358f;

	// Token: 0x0400130B RID: 4875
	private float maxPowerDriveShaft;

	// Token: 0x0400130C RID: 4876
	[HideInInspector]
	public float currentPower;

	// Token: 0x0400130D RID: 4877
	private float maxPowerKW;

	// Token: 0x0400130E RID: 4878
	private float maxPowerAngVel;

	// Token: 0x0400130F RID: 4879
	private float maxPowerEngineTorque;

	// Token: 0x04001310 RID: 4880
	private float torque;

	// Token: 0x04001311 RID: 4881
	private float maxTorque;

	// Token: 0x04001312 RID: 4882
	private float torqueNet;

	// Token: 0x04001313 RID: 4883
	private float maxTorqueNet;

	// Token: 0x04001314 RID: 4884
	private float maxTorqueRPM;

	// Token: 0x04001315 RID: 4885
	private float frictionTorque;

	// Token: 0x04001316 RID: 4886
	[HideInInspector]
	public float powerMultiplier = 1f;

	// Token: 0x04001317 RID: 4887
	[HideInInspector]
	public float ratio;

	// Token: 0x04001318 RID: 4888
	[HideInInspector]
	public bool ChangingGear;

	// Token: 0x04001319 RID: 4889
	private int nextGear;

	// Token: 0x0400131A RID: 4890
	private float lockingTorque;

	// Token: 0x0400131B RID: 4891
	private float max_power;

	// Token: 0x0400131C RID: 4892
	private float drivetrainFraction;

	// Token: 0x0400131D RID: 4893
	[HideInInspector]
	public float velo;

	// Token: 0x02000244 RID: 580
	public enum Transmissions
	{
		// Token: 0x0400131F RID: 4895
		RWD,
		// Token: 0x04001320 RID: 4896
		FWD,
		// Token: 0x04001321 RID: 4897
		AWD
	}

	// Token: 0x02000245 RID: 581
	public enum Engine
	{
		// Token: 0x04001323 RID: 4899
		Petrol,
		// Token: 0x04001324 RID: 4900
		Diesel
	}
}
