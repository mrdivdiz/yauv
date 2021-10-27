using System;
using UnityEngine;

// Token: 0x0200023D RID: 573
[RequireComponent(typeof(Wheels))]
public class CarDynamics : MonoBehaviour
{
	// Token: 0x06000AE3 RID: 2787 RVA: 0x00082BDC File Offset: 0x00080DDC
	private float RoundTo(float value, int precision)
	{
		int num = 1;
		for (int i = 1; i <= precision; i++)
		{
			num *= 10;
		}
		return Mathf.Round(value * (float)num) / (float)num;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00082C10 File Offset: 0x00080E10
	public void SetDebug(bool db)
	{
		this.debug = db;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x00082C1C File Offset: 0x00080E1C
	private void Awake()
	{
		if (AndroidPlatform.platform == AndroidPlatform.AndroidPlatforms.GameShield)
		{
			this.targetPlatform = CarDynamics.TargetPlatform.PCandMAC;
		}
		this.body = base.GetComponent<Rigidbody>();
		this.drivetrain = base.GetComponent<Drivetrain>();
		this.axisCarController = base.GetComponent<AxisCarController>();
		this.mouseCarcontroller = base.GetComponent<MouseCarController>();
		this.mobileCarController = base.GetComponent<MobileCarController>();
		this.brakeLights = base.GetComponent<BrakeLights>();
		this.SetController(this.controller.ToString());
		this.wheels = base.GetComponent<Wheels>();
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
			this.frontWheels = this.wheels.frontWheels;
			this.rearWheels = this.wheels.rearWheels;
			this.otherWheels = this.wheels.otherWheels;
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00082DDC File Offset: 0x00080FDC
	private void Start()
	{
		if (this.ok)
		{
			if (this.centerOfMass == null)
			{
				this.centerOfMassObject = new GameObject("COG");
				this.centerOfMassObject.transform.parent = base.transform;
				this.centerOfMass = this.centerOfMassObject.transform;
				this.centerOfMass.position = this.body.worldCenterOfMass;
			}
			this.GetCarParams();
			this.LoadSetUp(this.setup.ToString());
			this.SetBrakes();
			this.lastMouseController = this.mouseController;
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00082E80 File Offset: 0x00081080
	private void SetBrakes()
	{
		this.frontRearBrakeBalance = Mathf.Clamp01(this.frontRearBrakeBalance);
		foreach (Wheel wheel in this.frontWheels)
		{
			wheel.brakeFrictionTorque = this.brakeFrictionTorque * Mathf.Min(this.frontRearBrakeBalance, 0.5f) * 2f;
		}
		foreach (Wheel wheel2 in this.rearWheels)
		{
			wheel2.brakeFrictionTorque = this.brakeFrictionTorque * Mathf.Min(1f - this.frontRearBrakeBalance, 0.5f) * 2f;
		}
		foreach (Wheel wheel3 in this.otherWheels)
		{
			wheel3.brakeFrictionTorque = this.brakeFrictionTorque * Mathf.Min(1f - this.frontRearBrakeBalance, 0.5f) * 2f;
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00082F84 File Offset: 0x00081184
	private void GetCarParams()
	{
		if (this.centerOfMass != null)
		{
			this.zlocalPosition = (this.zlocalPosition_bkup = this.centerOfMass.localPosition.z);
			this.ylocalPosition = this.centerOfMass.localPosition.y;
		}
		else
		{
			this.zlocalPosition = (this.zlocalPosition_bkup = this.body.worldCenterOfMass.z);
			this.ylocalPosition = this.body.worldCenterOfMass.y;
		}
		this.body.inertiaTensor *= this.inertiaFactor;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0008303C File Offset: 0x0008123C
	private void SetCarParams()
	{
		foreach (Wheel wheel in this.frontWheels)
		{
			wheel.gripFactor = this.gripFactor;
			wheel.suspensionTravel = this.suspensionTravelFront;
			wheel.suspensionRate = this.suspensionRateFront;
			wheel.bumpRate = this.bumpRateFront;
			wheel.reboundRate = this.reboundRateFront;
		}
		foreach (Wheel wheel2 in this.rearWheels)
		{
			wheel2.gripFactor = this.gripFactor;
			wheel2.suspensionTravel = this.suspensionTravelRear;
			wheel2.suspensionRate = this.suspensionRateRear;
			wheel2.bumpRate = this.bumpRateRear;
			wheel2.reboundRate = this.reboundRateRear;
		}
		foreach (Wheel wheel3 in this.otherWheels)
		{
			wheel3.gripFactor = this.gripFactor;
			wheel3.suspensionTravel = this.suspensionTravelRear;
			wheel3.suspensionRate = this.suspensionRateRear;
			wheel3.bumpRate = this.bumpRateRear;
			wheel3.reboundRate = this.reboundRateRear;
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00083170 File Offset: 0x00081370
	private void SetTires(CarDynamics.Tires tires)
	{
		foreach (Wheel wheel in this.allWheels)
		{
			wheel.LoadTiresData(tires.ToString());
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000831B0 File Offset: 0x000813B0
	public void SetController(string strcontroller)
	{
		if (strcontroller == CarDynamics.Controller.Axis.ToString() && this.actualController != CarDynamics.Controller.Axis.ToString())
		{
			if (this.axisCarController == null)
			{
				this.axisCarController = base.transform.gameObject.AddComponent<AxisCarController>();
			}
			this.axisCarController.enabled = true;
			this.carcontroller = this.axisCarController;
			this.drivetrain.carcontroller = this.axisCarController;
			if (this.brakeLights != null)
			{
				this.brakeLights.carcontroller = this.axisCarController;
			}
			if (this.mouseCarcontroller != null)
			{
				this.mouseCarcontroller.enabled = false;
			}
			if (this.mobileCarController != null)
			{
				this.mobileCarController.enabled = false;
			}
			if (AndroidPlatform.IsJoystickConnected())
			{
				this.controller = CarDynamics.Controller.Axis;
			}
			else
			{
				this.controller = CarDynamics.Controller.Mobile;
			}
		}
		else if (strcontroller == CarDynamics.Controller.Mouse.ToString() && this.actualController != CarDynamics.Controller.Mouse.ToString())
		{
			if (this.mouseCarcontroller == null)
			{
				this.mouseCarcontroller = base.transform.gameObject.AddComponent<MouseCarController>();
			}
			this.mouseCarcontroller.enabled = true;
			this.mouseCarcontroller.smoothInput = false;
			this.carcontroller = this.mouseCarcontroller;
			this.drivetrain.carcontroller = this.mouseCarcontroller;
			if (this.brakeLights != null)
			{
				this.brakeLights.carcontroller = this.mouseCarcontroller;
			}
			if (this.axisCarController != null)
			{
				this.axisCarController.enabled = false;
			}
			if (this.mobileCarController != null)
			{
				this.mobileCarController.enabled = false;
			}
			this.controller = CarDynamics.Controller.Mouse;
		}
		else if (strcontroller == CarDynamics.Controller.Mobile.ToString() && this.actualController != CarDynamics.Controller.Mobile.ToString())
		{
			if (this.mobileCarController == null)
			{
				this.mobileCarController = base.transform.gameObject.AddComponent<MobileCarController>();
			}
			this.mobileCarController.enabled = true;
			this.carcontroller = this.mobileCarController;
			this.drivetrain.carcontroller = this.mobileCarController;
			if (this.brakeLights != null)
			{
				this.brakeLights.carcontroller = this.mobileCarController;
			}
			if (this.axisCarController != null)
			{
				this.axisCarController.enabled = false;
			}
			if (this.mouseCarcontroller != null)
			{
				this.mouseCarcontroller.enabled = false;
			}
			this.controller = CarDynamics.Controller.Mobile;
		}
		else if (strcontroller == CarDynamics.Controller.None.ToString() && this.actualController != CarDynamics.Controller.None.ToString())
		{
			if (this.mouseCarcontroller != null)
			{
				this.mouseCarcontroller.enabled = false;
			}
			if (this.axisCarController != null)
			{
				this.axisCarController.enabled = false;
			}
			if (this.mobileCarController != null)
			{
				this.mobileCarController.enabled = false;
			}
			this.controller = CarDynamics.Controller.None;
		}
		this.actualController = strcontroller;
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x00083530 File Offset: 0x00081730
	private void FixedUpdate()
	{
		if (this.ok)
		{
			this.velo = this.body.velocity.magnitude;
			if (Application.isEditor)
			{
				this.SetBrakes();
				if (this.carcontroller != null)
				{
					this.SetController(this.controller.ToString());
				}
			}
			float num = (this.frontWheels[0].compression - this.frontWheels[1].compression) * this.antiRollBarRateFront;
			this.frontWheels[0].antiRollBarForce = num;
			this.frontWheels[1].antiRollBarForce = -num;
			float num2 = (this.rearWheels[0].compression - this.rearWheels[1].compression) * this.antiRollBarRateRear;
			this.rearWheels[0].antiRollBarForce = num2;
			this.rearWheels[1].antiRollBarForce = -num2;
			this.normalForceF = (this.normalForceR = 0f);
			foreach (Wheel wheel in this.frontWheels)
			{
				this.normalForceF += wheel.normalForce;
			}
			this.normalForceF /= 2f;
			foreach (Wheel wheel2 in this.rearWheels)
			{
				this.normalForceR += wheel2.normalForce;
			}
			this.normalForceR /= 2f;
			if (this.normalForceF + this.normalForceR != 0f)
			{
				this.frontRearWeightRepartition = this.RoundTo(this.normalForceF / (this.normalForceF + this.normalForceR), 2);
			}
			if (this.arcadeMode && this.velo * 3.6f > 35f)
			{
				this.lateralSlip = this.LateralSlip();
				float num3 = Mathf.Abs(this.lateralSlip);
				this.COGYShift = Mathf.Clamp01(num3) * 0.35f;
				if (num3 > 1f)
				{
					this.body.AddRelativeTorque(-Vector3.up * ((num3 - 1f) * Mathf.Sign(this.lateralSlip)) * 10000f);
				}
			}
			else
			{
				this.COGYShift = 0f;
			}
			if (this.centerOfMass != null)
			{
				this.centerOfMass.localPosition = new Vector3(this.centerOfMass.localPosition.x, this.ylocalPosition - this.COGYShift, this.zlocalPosition);
				this.body.centerOfMass = this.centerOfMass.localPosition;
			}
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00083800 File Offset: 0x00081A00
	public void LoadSetUp(string setupToString)
	{
		if (setupToString == CarDynamics.Setup.peugeot.ToString())
		{
			this.suspensionTravelFront = 0.27f;
			this.suspensionTravelRear = 0.27f;
			this.suspensionRateFront = 18000f;
			this.suspensionRateRear = 19000f;
			this.bumpRateFront = 3500f;
			this.reboundRateFront = 6000f;
			this.bumpRateRear = 4000f;
			this.reboundRateRear = 7000f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.tireRate = 0f;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.03f;
			this.carcontroller.veloSteerReleaseTime = 0.03f;
			this.antiRollBarRateFront = 4000f;
			this.antiRollBarRateRear = 8000f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = true;
			this.carcontroller.TCSThreshold = 0.5f;
			this.carcontroller.ESP = false;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.peugeot;
		}
		else if (setupToString == CarDynamics.Setup.clio.ToString())
		{
			this.suspensionTravelFront = 0.27f;
			this.suspensionTravelRear = 0.27f;
			this.suspensionRateFront = 20000f;
			this.suspensionRateRear = 19000f;
			this.bumpRateFront = 3500f;
			this.reboundRateFront = 6000f;
			this.bumpRateRear = 4000f;
			this.reboundRateRear = 7000f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.tireRate = 0f;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.03f;
			this.carcontroller.veloSteerReleaseTime = 0.03f;
			this.antiRollBarRateFront = 2000f;
			this.antiRollBarRateRear = 6000f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = true;
			this.carcontroller.TCSThreshold = 0.5f;
			this.carcontroller.ESP = false;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.clio;
		}
		else if (setupToString == CarDynamics.Setup.monster.ToString())
		{
			this.suspensionTravelFront = 0.7f;
			this.suspensionTravelRear = 0.7f;
			this.suspensionRateFront = 70000f;
			this.suspensionRateRear = 70000f;
			this.bumpRateFront = 3000f;
			this.bumpRateRear = 3000f;
			this.reboundRateFront = 4000f;
			this.reboundRateRear = 4000f;
			this.tireRate = 150000f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.03f;
			this.carcontroller.veloSteerReleaseTime = 0.03f;
			this.antiRollBarRateFront = 0f;
			this.antiRollBarRateRear = 0f;
			this.carcontroller.steerAssistance = false;
			this.carcontroller.TCS = false;
			this.carcontroller.ESP = false;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.monster;
		}
		else if (setupToString == CarDynamics.Setup.gmc.ToString())
		{
			this.suspensionTravelFront = 0.5f;
			this.suspensionTravelRear = 0.5f;
			this.suspensionRateFront = 60000f;
			this.suspensionRateRear = 60000f;
			this.bumpRateFront = 3000f;
			this.bumpRateRear = 3000f;
			this.reboundRateFront = 6000f;
			this.reboundRateRear = 12000f;
			this.tireRate = 0f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.03f;
			this.carcontroller.veloSteerReleaseTime = 0.03f;
			this.antiRollBarRateFront = 0f;
			this.antiRollBarRateRear = 0f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = false;
			this.carcontroller.ESP = false;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 5f;
			this.setup = CarDynamics.Setup.gmc;
		}
		else if (setupToString == CarDynamics.Setup.catamount_city.ToString())
		{
			this.suspensionTravelFront = 0.29f;
			this.suspensionTravelRear = 0.29f;
			this.suspensionRateFront = 30000f;
			this.suspensionRateRear = 30000f;
			this.bumpRateFront = 3000f;
			this.bumpRateRear = 4500f;
			this.reboundRateFront = 6000f;
			this.reboundRateRear = 9000f;
			this.tireRate = 0f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
				this.drivetrain.finalDriveRatio = 6.09f;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.03f;
			this.carcontroller.veloSteerReleaseTime = 0.03f;
			this.antiRollBarRateFront = 4000f;
			this.antiRollBarRateRear = 6000f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = true;
			this.carcontroller.TCSThreshold = 0.5f;
			this.carcontroller.ESP = true;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.catamount_city;
		}
		else if (setupToString == CarDynamics.Setup.catamount_track.ToString())
		{
			this.suspensionTravelFront = 0.2f;
			this.suspensionTravelRear = 0.2f;
			this.suspensionRateFront = 30000f;
			this.suspensionRateRear = 30000f;
			this.bumpRateFront = 3000f;
			this.bumpRateRear = 4500f;
			this.reboundRateFront = 6000f;
			this.reboundRateRear = 9000f;
			this.tireRate = 0f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
				this.drivetrain.finalDriveRatio = 5f;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.06f;
			this.carcontroller.veloSteerReleaseTime = 0.06f;
			this.antiRollBarRateFront = 4000f;
			this.antiRollBarRateRear = 6000f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = true;
			this.carcontroller.ESP = true;
			this.carcontroller.ESPStrength = 4f;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.catamount_track;
		}
		else if (setupToString == CarDynamics.Setup.ferrari_city.ToString())
		{
			this.suspensionTravelFront = 0.2f;
			this.suspensionTravelRear = 0.2f;
			this.suspensionRateFront = 30000f;
			this.suspensionRateRear = 30000f;
			this.bumpRateFront = 3000f;
			this.bumpRateRear = 4500f;
			this.reboundRateFront = 6000f;
			this.reboundRateRear = 9000f;
			this.tireRate = 0f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.03f;
			this.carcontroller.veloSteerReleaseTime = 0.03f;
			this.antiRollBarRateFront = 10000f;
			this.antiRollBarRateRear = 10000f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = true;
			this.carcontroller.TCSThreshold = 0.5f;
			this.carcontroller.ESP = true;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.ferrari_city;
		}
		else if (setupToString == CarDynamics.Setup.ferrari_track.ToString())
		{
			this.suspensionTravelFront = 0.15f;
			this.suspensionTravelRear = 0.15f;
			this.suspensionRateFront = 30000f;
			this.suspensionRateRear = 30000f;
			this.bumpRateFront = 3000f;
			this.bumpRateRear = 4500f;
			this.reboundRateFront = 6000f;
			this.reboundRateRear = 9000f;
			this.tireRate = 0f;
			this.zlocalPosition = this.zlocalPosition_bkup;
			this.gripFactor = 1f;
			if (this.drivetrain)
			{
				this.drivetrain.powerMultiplier = 1f;
				this.drivetrain.automatic = true;
			}
			this.carcontroller.steerTime = 0.1f;
			this.carcontroller.steerReleaseTime = 0.1f;
			this.carcontroller.veloSteerTime = 0.06f;
			this.carcontroller.veloSteerReleaseTime = 0.06f;
			this.antiRollBarRateFront = 10000f;
			this.antiRollBarRateRear = 10000f;
			this.carcontroller.steerAssistance = true;
			this.carcontroller.TCS = true;
			this.carcontroller.ESP = true;
			this.carcontroller.ESPStrength = 4f;
			this.tridimensionalTire = true;
			this.dampAbsRoadVelo = 8f;
			this.setup = CarDynamics.Setup.ferrari_track;
		}
		this.SetTires(this.tires);
		this.suspensionTravel = this.suspensionTravelFront;
		this.m_dampAbsRoadVelo = this.dampAbsRoadVelo;
		if (this.drivetrain)
		{
			this.drivetrain.CalcIdleThrottle();
		}
		this.SetCarParams();
		if (setupToString != CarDynamics.Setup.none.ToString())
		{
			Debug.Log("Setup loaded: " + this.setup);
		}
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x000843A0 File Offset: 0x000825A0
	private float LateralSlip()
	{
		if (this.allWheels != null)
		{
			float num = 0f;
			foreach (Wheel wheel in this.allWheels)
			{
				float num2 = Mathf.Abs(wheel.lateralSlip);
				if (num < num2)
				{
					num = wheel.lateralSlip;
				}
			}
			return num;
		}
		return 0f;
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0008440C File Offset: 0x0008260C
	public float SlipVelo()
	{
		if (this.allWheels != null)
		{
			float num = 0f;
			foreach (Wheel wheel in this.allWheels)
			{
				num += wheel.slipVelo;
			}
			return num / (float)this.allWheels.Length;
		}
		return 0f;
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00084464 File Offset: 0x00082664
	private void OnGUI()
	{
		if (this.ok)
		{
			if (!this.debug)
			{
				if (this.showSlidersGUI)
				{
					GUI.Box(new Rect((float)(Screen.width - 600), 5f, 430f, 50f), string.Empty);
					if (this.drivetrain)
					{
						GUI.Label(new Rect((float)(Screen.width - 590), 5f, 120f, 20f), "Power multiplier: " + this.drivetrain.powerMultiplier);
						this.drivetrain.powerMultiplier = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 590), 25f, 100f, 20f), this.drivetrain.powerMultiplier, 0.1f, 2f), 1);
						GUI.Label(new Rect((float)(Screen.width - 590), 35f, 100f, 20f), string.Concat(new object[]
						{
							string.Empty,
							Mathf.Round(this.drivetrain.maxPowerNet * this.drivetrain.powerMultiplier),
							" HP @ ",
							this.drivetrain.maxPowerRPM
						}));
					}
					GUI.Label(new Rect((float)(Screen.width - 470), 5f, 100f, 20f), "Grip: " + this.gripFactor);
					this.gripFactor = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 470), 25f, 100f, 20f), this.gripFactor, 0.1f, 2f), 1);
					if (this.drivetrain)
					{
						GUI.Label(new Rect((float)(Screen.width - 470), 35f, 150f, 20f), string.Concat(new object[]
						{
							string.Empty,
							Mathf.Round(this.drivetrain.currentPower),
							" HP @ ",
							Mathf.Round(this.drivetrain.rpm)
						}));
					}
					float num = Mathf.Round(this.velo * 3.6f);
					GUI.Label(new Rect((float)(Screen.width - 310), 5f, 130f, 20f), string.Concat(new object[]
					{
						"Weight (F:R) ",
						this.frontRearWeightRepartition * 100f,
						":",
						100f - this.frontRearWeightRepartition * 100f
					}));
					this.zlocalPosition = GUI.HorizontalSlider(new Rect((float)(Screen.width - 310), 25f, 130f, 20f), this.zlocalPosition, 1f, -1f);
					GUI.Label(new Rect((float)(Screen.width - 310), 35f, 150f, 200f), string.Concat(new object[]
					{
						"Km/h: ",
						num,
						" MPH: ",
						Mathf.Round(num * 0.621f)
					}));
					GUI.Box(new Rect((float)(Screen.width - 145), 5f, 140f, 45f), string.Empty);
					GUI.Label(new Rect((float)(Screen.width - 135), 5f, 120f, 20f), "Susp Travel " + this.suspensionTravel);
					this.suspensionTravel = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 25f, 120f, 20f), this.suspensionTravel, 0.1f, 1f), 2);
					this.suspensionTravelFront = (this.suspensionTravelRear = this.suspensionTravel);
					GUI.Box(new Rect((float)(Screen.width - 145), 60f, 140f, 90f), string.Empty);
					GUI.Label(new Rect((float)(Screen.width - 135), 60f, 130f, 20f), "Spring Rate F: " + this.suspensionRateFront);
					this.suspensionRateFront = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 85f, 120f, 20f), this.suspensionRateFront, 10000f, 100000f), 0);
					GUI.Label(new Rect((float)(Screen.width - 135), 105f, 140f, 20f), "Spring Rate R: " + this.suspensionRateRear);
					this.suspensionRateRear = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 130f, 120f, 20f), this.suspensionRateRear, 10000f, 100000f), 0);
					GUI.Box(new Rect((float)(Screen.width - 145), 160f, 140f, 90f), string.Empty);
					GUI.Label(new Rect((float)(Screen.width - 135), 160f, 140f, 20f), "Bump Rate F: " + this.bumpRateFront);
					this.bumpRateFront = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 185f, 120f, 20f), this.bumpRateFront, 1000f, 20000f), 0);
					GUI.Label(new Rect((float)(Screen.width - 135), 205f, 140f, 20f), "Rebound Rate F: " + this.reboundRateFront);
					this.reboundRateFront = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 230f, 120f, 20f), this.reboundRateFront, 1000f, 20000f), 0);
					GUI.Box(new Rect((float)(Screen.width - 145), 260f, 140f, 90f), string.Empty);
					GUI.Label(new Rect((float)(Screen.width - 135), 260f, 140f, 20f), "Bump Rate R: " + this.bumpRateRear);
					this.bumpRateRear = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 285f, 120f, 20f), this.bumpRateRear, 1000f, 20000f), 0);
					GUI.Label(new Rect((float)(Screen.width - 135), 305f, 140f, 20f), "Rebound Rate R: " + this.reboundRateRear);
					this.reboundRateRear = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 330f, 120f, 20f), this.reboundRateRear, 1000f, 20000f), 0);
					GUI.Box(new Rect((float)(Screen.width - 145), 360f, 140f, 90f), string.Empty);
					GUI.Label(new Rect((float)(Screen.width - 135), 360f, 140f, 20f), "Anti RollBar F: " + this.antiRollBarRateFront);
					this.antiRollBarRateFront = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 385f, 120f, 20f), this.antiRollBarRateFront, 0f, 40000f), 0);
					GUI.Label(new Rect((float)(Screen.width - 135), 400f, 140f, 20f), "Anti RollBar R: " + this.antiRollBarRateRear);
					this.antiRollBarRateRear = this.RoundTo(GUI.HorizontalSlider(new Rect((float)(Screen.width - 135), 425f, 120f, 20f), this.antiRollBarRateRear, 0f, 40000f), 0);
					if (this.showTiresGUI)
					{
						GUI.Box(new Rect((float)(Screen.width - 295), 60f, 140f, 90f), string.Empty);
						GUI.Label(new Rect((float)(Screen.width - 285), 60f, 140f, 20f), "Tires Type:");
						this.tireType = GUI.SelectionGrid(new Rect((float)(Screen.width - 285), 85f, 120f, 60f), this.tireType, this.tireTypes, 1);
					}
					GUI.Box(new Rect((float)(Screen.width - 295), 160f, 140f, 40f), string.Empty);
					if (GUI.Button(new Rect((float)(Screen.width - 285), 170f, 120f, 20f), "Reset Setup"))
					{
						this.LoadSetUp(this.setup.ToString());
						this.tireType = 0;
					}
					if (GUI.changed)
					{
						if (this.gripFactor > 1f)
						{
							this.dampAbsRoadVelo = this.m_dampAbsRoadVelo * this.gripFactor;
						}
						else
						{
							this.dampAbsRoadVelo = this.m_dampAbsRoadVelo;
						}
						if (this.drivetrain)
						{
							this.drivetrain.CalcIdleThrottle();
						}
						this.SetCarParams();
						if (this.mtireType != this.tireType)
						{
							switch (this.tireType)
							{
							case 0:
								this.tires = CarDynamics.Tires.superSport;
								break;
							case 1:
								this.tires = CarDynamics.Tires.sport;
								break;
							case 2:
								this.tires = CarDynamics.Tires.touring;
								break;
							}
							this.SetTires(this.tires);
							this.mtireType = this.tireType;
						}
					}
				}
				if (this.showHelpGUI)
				{
					GUI.BeginGroup(new Rect(5f, 10f, 420f, 520f));
					GUI.Box(new Rect(0f, 0f, 300f, 460f), string.Empty);
					GUI.Label(new Rect(5f, 0f, 300f, 20f), "Use arrows keys or wasd keys to control the car");
					GUI.Label(new Rect(5f, 20f, 300f, 20f), "'Page Up' and 'Page Down' keys to change car");
					GUI.Label(new Rect(5f, 40f, 300f, 20f), " 'f' and 'v' keys to change gear");
					GUI.Label(new Rect(5f, 60f, 300f, 20f), " 'c' key to change camera");
					GUI.Label(new Rect(5f, 80f, 300f, 20f), " 'space' key to handbrake");
					GUI.Label(new Rect(5f, 100f, 300f, 20f), " 'r' key to realign the car");
					GUI.Label(new Rect(5f, 120f, 300f, 20f), " 'o' key to change transmission (" + this.drivetrain.transmission + ")");
					GUI.Label(new Rect(5f, 140f, 300f, 20f), " 'esc' key to restart the level");
					GUI.Label(new Rect(5f, 160f, 300f, 20f), " 'h' key to toggle this help window");
					GUI.Label(new Rect(5f, 180f, 300f, 20f), " 'g' key to toggle forces lines");
					GUI.Label(new Rect(5f, 200f, 300f, 20f), " 'b' key to toggle setup GUI");
					GUI.Label(new Rect(5f, 220f, 300f, 20f), " 'n' key to toggle the dashboard");
					GUI.Label(new Rect(5f, 240f, 300f, 20f), " 'm' key to toggle the map");
					GUI.Label(new Rect(5f, 260f, 300f, 20f), " 'shift + r' keys to repair the car");
					GUI.Label(new Rect(5f, 280f, 300f, 20f), " 'F1' key to toggle slow motion");
					this.arcadeMode = GUI.Toggle(new Rect(5f, 300f, 300f, 20f), this.arcadeMode, "Arcade Mode (toggle with 'l' key)");
					this.carcontroller.TCS = GUI.Toggle(new Rect(5f, 320f, 300f, 20f), this.carcontroller.TCS, "TCS (Traction Control, hold 'shift' to bypass)");
					this.carcontroller.ABS = GUI.Toggle(new Rect(5f, 340f, 300f, 20f), this.carcontroller.ABS, "ABS (AntiLock Braking, toggle with 'q' key)");
					this.carcontroller.ESP = GUI.Toggle(new Rect(5f, 360f, 300f, 20f), this.carcontroller.ESP, "ESP (Stability Control, toggle with 'e' key)");
					this.carcontroller.steerAssistance = GUI.Toggle(new Rect(5f, 380f, 300f, 20f), this.carcontroller.steerAssistance, "Steer Assistance (toggle with 's' key)");
					this.drivetrain.automatic = GUI.Toggle(new Rect(5f, 400f, 300f, 20f), this.drivetrain.automatic, "Automatic Transmission (toggle with 't' key)");
					this.tridimensionalTire = GUI.Toggle(new Rect(5f, 420f, 300f, 20f), this.tridimensionalTire, "Tridimensional Tire");
					this.mouseController = GUI.Toggle(new Rect(5f, 440f, 300f, 20f), this.mouseController, "Mouse Controller");
					if (GUI.changed && this.lastMouseController != this.mouseController)
					{
						if (this.mouseController)
						{
							this.controller = CarDynamics.Controller.Mouse;
						}
						else
						{
							this.controller = CarDynamics.Controller.Axis;
						}
						if (this.carcontroller != null)
						{
							this.SetController(this.controller.ToString());
						}
					}
					GUI.EndGroup();
					this.lastMouseController = this.mouseController;
				}
			}
		}
	}

	// Token: 0x0400123F RID: 4671
	private float lateralSlip;

	// Token: 0x04001240 RID: 4672
	private bool debug;

	// Token: 0x04001241 RID: 4673
	private bool ok = true;

	// Token: 0x04001242 RID: 4674
	private int tireType;

	// Token: 0x04001243 RID: 4675
	private int mtireType;

	// Token: 0x04001244 RID: 4676
	private string[] tireTypes = new string[]
	{
		"SuperSport",
		"Sport",
		"Touring"
	};

	// Token: 0x04001245 RID: 4677
	[HideInInspector]
	public float velo;

	// Token: 0x04001246 RID: 4678
	public CarDynamics.Controller controller;

	// Token: 0x04001247 RID: 4679
	private string actualController;

	// Token: 0x04001248 RID: 4680
	public CarDynamics.TargetPlatform targetPlatform;

	// Token: 0x04001249 RID: 4681
	private Drivetrain drivetrain;

	// Token: 0x0400124A RID: 4682
	private CarController carcontroller;

	// Token: 0x0400124B RID: 4683
	private AxisCarController axisCarController;

	// Token: 0x0400124C RID: 4684
	private MouseCarController mouseCarcontroller;

	// Token: 0x0400124D RID: 4685
	private MobileCarController mobileCarController;

	// Token: 0x0400124E RID: 4686
	private BrakeLights brakeLights;

	// Token: 0x0400124F RID: 4687
	private Wheels wheels;

	// Token: 0x04001250 RID: 4688
	private bool mouseController;

	// Token: 0x04001251 RID: 4689
	private bool lastMouseController;

	// Token: 0x04001252 RID: 4690
	[HideInInspector]
	public Wheel[] allWheels;

	// Token: 0x04001253 RID: 4691
	[HideInInspector]
	public Wheel[] frontWheels;

	// Token: 0x04001254 RID: 4692
	[HideInInspector]
	public Wheel[] rearWheels;

	// Token: 0x04001255 RID: 4693
	[HideInInspector]
	public Wheel[] otherWheels;

	// Token: 0x04001256 RID: 4694
	private float suspensionTravel;

	// Token: 0x04001257 RID: 4695
	public float suspensionTravelFront = 0.2f;

	// Token: 0x04001258 RID: 4696
	private float suspensionTravelFront_bkup;

	// Token: 0x04001259 RID: 4697
	public float suspensionTravelRear = 0.2f;

	// Token: 0x0400125A RID: 4698
	private float suspensionTravelRear_bkup;

	// Token: 0x0400125B RID: 4699
	public float suspensionRateFront = 20000f;

	// Token: 0x0400125C RID: 4700
	private float suspensionRateFront_bkup;

	// Token: 0x0400125D RID: 4701
	public float suspensionRateRear = 20000f;

	// Token: 0x0400125E RID: 4702
	private float suspensionRateRear_bkup;

	// Token: 0x0400125F RID: 4703
	public float bumpRateFront = 4000f;

	// Token: 0x04001260 RID: 4704
	private float bumpRateFront_bkup;

	// Token: 0x04001261 RID: 4705
	public float bumpRateRear = 4000f;

	// Token: 0x04001262 RID: 4706
	private float bumpRateRear_bkup;

	// Token: 0x04001263 RID: 4707
	public float reboundRateFront = 4000f;

	// Token: 0x04001264 RID: 4708
	private float reboundRateFront_bkup;

	// Token: 0x04001265 RID: 4709
	public float reboundRateRear = 4000f;

	// Token: 0x04001266 RID: 4710
	private float reboundRateRear_bkup;

	// Token: 0x04001267 RID: 4711
	[HideInInspector]
	public float transitionDamperVelo = 0.127f;

	// Token: 0x04001268 RID: 4712
	public float antiRollBarRateFront = 20000f;

	// Token: 0x04001269 RID: 4713
	public float antiRollBarRateRear = 10000f;

	// Token: 0x0400126A RID: 4714
	public Transform centerOfMass;

	// Token: 0x0400126B RID: 4715
	private GameObject centerOfMassObject;

	// Token: 0x0400126C RID: 4716
	public float dampAbsRoadVelo = 8f;

	// Token: 0x0400126D RID: 4717
	private float m_dampAbsRoadVelo;

	// Token: 0x0400126E RID: 4718
	public float inertiaFactor = 1f;

	// Token: 0x0400126F RID: 4719
	public float frontRearWeightRepartition = 0.5f;

	// Token: 0x04001270 RID: 4720
	public float brakeFrictionTorque = 1500f;

	// Token: 0x04001271 RID: 4721
	public float frontRearBrakeBalance = 0.65f;

	// Token: 0x04001272 RID: 4722
	public float tireRate;

	// Token: 0x04001273 RID: 4723
	[HideInInspector]
	public float tireDamping = 500f;

	// Token: 0x04001274 RID: 4724
	[HideInInspector]
	public float tireDampingLow = 2500f;

	// Token: 0x04001275 RID: 4725
	public bool beckman = true;

	// Token: 0x04001276 RID: 4726
	public bool tridimensionalTire;

	// Token: 0x04001277 RID: 4727
	public bool arcadeMode;

	// Token: 0x04001278 RID: 4728
	private float COGYShift;

	// Token: 0x04001279 RID: 4729
	public CarDynamics.Setup setup;

	// Token: 0x0400127A RID: 4730
	public CarDynamics.Tires tires;

	// Token: 0x0400127B RID: 4731
	public float airDensity = 1.2041f;

	// Token: 0x0400127C RID: 4732
	public bool showSlidersGUI = true;

	// Token: 0x0400127D RID: 4733
	public bool showHelpGUI = true;

	// Token: 0x0400127E RID: 4734
	public bool showTiresGUI;

	// Token: 0x0400127F RID: 4735
	public bool disableAllGUI;

	// Token: 0x04001280 RID: 4736
	public Skidmarks skidmarks;

	// Token: 0x04001281 RID: 4737
	private float gripFactor = 1f;

	// Token: 0x04001282 RID: 4738
	private float zlocalPosition;

	// Token: 0x04001283 RID: 4739
	private float zlocalPosition_bkup;

	// Token: 0x04001284 RID: 4740
	private float ylocalPosition;

	// Token: 0x04001285 RID: 4741
	private float normalForceR;

	// Token: 0x04001286 RID: 4742
	private float normalForceF;

	// Token: 0x04001287 RID: 4743
	private Rigidbody body;

	// Token: 0x0200023E RID: 574
	public enum Controller
	{
		// Token: 0x04001289 RID: 4745
		Axis,
		// Token: 0x0400128A RID: 4746
		Mouse,
		// Token: 0x0400128B RID: 4747
		Mobile,
		// Token: 0x0400128C RID: 4748
		None
	}

	// Token: 0x0200023F RID: 575
	public enum TargetPlatform
	{
		// Token: 0x0400128E RID: 4750
		PCandMAC,
		// Token: 0x0400128F RID: 4751
		Mobile
	}

	// Token: 0x02000240 RID: 576
	public enum Setup
	{
		// Token: 0x04001291 RID: 4753
		none,
		// Token: 0x04001292 RID: 4754
		ferrari_city,
		// Token: 0x04001293 RID: 4755
		ferrari_track,
		// Token: 0x04001294 RID: 4756
		catamount_city,
		// Token: 0x04001295 RID: 4757
		catamount_track,
		// Token: 0x04001296 RID: 4758
		peugeot,
		// Token: 0x04001297 RID: 4759
		clio,
		// Token: 0x04001298 RID: 4760
		monster,
		// Token: 0x04001299 RID: 4761
		gmc
	}

	// Token: 0x02000241 RID: 577
	public enum Tires
	{
		// Token: 0x0400129B RID: 4763
		standard,
		// Token: 0x0400129C RID: 4764
		touring,
		// Token: 0x0400129D RID: 4765
		sport,
		// Token: 0x0400129E RID: 4766
		superSport,
		// Token: 0x0400129F RID: 4767
		competition,
		// Token: 0x040012A0 RID: 4768
		truck,
		// Token: 0x040012A1 RID: 4769
		offroad
	}
}
