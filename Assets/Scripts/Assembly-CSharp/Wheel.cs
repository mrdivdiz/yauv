using System;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class Wheel : MonoBehaviour
{
	// Token: 0x06000B38 RID: 2872 RVA: 0x0008A7F4 File Offset: 0x000889F4
	public void SetDebug(bool db)
	{
		this.debug = db;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0008A800 File Offset: 0x00088A00
	private float CalcLongitudinalForce(float Fz, float slipRatio)
	{
		Fz *= 0.001f;
		slipRatio *= 100f;
		float num = this.b[1] * Fz + this.b[2];
		float num2 = num * Fz * this.m_gripMaterial;
		float num3 = (this.b[3] * Fz + this.b[4]) * Mathf.Exp(-this.b[5] * Fz) / (this.b[0] * num);
		float num4 = this.b[6] * Fz * Fz + this.b[7] * Fz + this.b[8];
		float num5 = slipRatio;
		return num2 * Mathf.Sin(this.b[0] * Mathf.Atan(num3 * num5 + num4 * (Mathf.Atan(num3 * num5) - num3 * num5)));
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0008A8C0 File Offset: 0x00088AC0
	private float CalcLateralForce(float Fz, float slipAngle)
	{
		Fz *= 0.001f;
		float num = this.a[1] * Fz + this.a[2];
		float num2 = num * Fz * this.m_gripMaterial;
		float num3 = this.a[3] * Mathf.Sin(2f * Mathf.Atan(Fz / this.a[4])) / (this.a[0] * num * Fz);
		float num4 = this.a[6] * Fz + this.a[7];
		float num5 = this.a[12] * Fz + this.a[13];
		return num2 * Mathf.Sin(this.a[0] * Mathf.Atan(num3 * slipAngle + num4 * (Mathf.Atan(num3 * slipAngle) - num3 * slipAngle))) + num5;
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x0008A988 File Offset: 0x00088B88
	private float normalizeAngle(float a, float center)
	{
		return a - 6.2831855f * Mathf.Floor((a + 3.1415927f - center) / 2f * 3.1415927f);
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x0008A9B8 File Offset: 0x00088BB8
	private void CalcForces(float Fz)
	{
		this.LookupIdealSlipRatioIdealSlipAngle(Fz);
		this.wheelTireVelo = this.angularVelocity * this.radius;
		this.absRoadVelo = Mathf.Abs(this.wheelRoadVelo);
		float num = (float)this.slipRes / this.slipResFactor;
		float num2 = num * 2f;
		if (num2 < 1f)
		{
			num2 = 1f;
		}
		if (num < 1f)
		{
			num = 1f;
		}
		float num3 = Mathf.Max(this.absRoadVelo, this.dampAbsRoadVelo);
		if (this.cardynamics.targetPlatform == CarDynamics.TargetPlatform.Mobile)
		{
			this.deltaRatio1 = this.wheelTireVelo - this.wheelRoadVelo - num3 * this.differentialSlipRatio;
			this.deltaRatio1 /= num;
			this.differentialSlipRatio += this.deltaRatio1 * Time.deltaTime;
		}
		else
		{
			this.deltaRatio1 = this.wheelTireVelo - this.wheelRoadVelo - num3 * this.differentialSlipRatio;
			this.deltaRatio1 /= num;
			float num4 = this.differentialSlipRatio;
			num4 += this.deltaRatio1 * 0.5f * Time.deltaTime;
			this.deltaRatio2 = this.wheelTireVelo - this.wheelRoadVelo - num3 * num4;
			this.deltaRatio2 /= num;
			num4 = this.differentialSlipRatio;
			num4 += this.deltaRatio2 * 0.5f * Time.deltaTime;
			this.deltaRatio3 = this.wheelTireVelo - this.wheelRoadVelo - num3 * num4;
			this.deltaRatio3 /= num;
			num4 = this.differentialSlipRatio;
			num4 += this.deltaRatio3 * Time.deltaTime;
			this.deltaRatio4 = this.wheelTireVelo - this.wheelRoadVelo - num3 * num4;
			this.deltaRatio4 /= num;
			this.differentialSlipRatio += this.deltaRatio1 * Time.deltaTime / 6f;
			this.differentialSlipRatio += this.deltaRatio2 * Time.deltaTime / 3f;
			this.differentialSlipRatio += this.deltaRatio3 * Time.deltaTime / 3f;
			this.differentialSlipRatio += this.deltaRatio4 * Time.deltaTime / 6f;
		}
		this.slipRatio = this.differentialSlipRatio / this.m_gripMaterial;
		this.slipRatio = Mathf.Clamp(this.slipRatio, -1.5f, 1.5f);
		this.deltaAngle1 = this.wheelRoadVeloLat - num3 * this.tanSlipAngle;
		this.deltaAngle1 /= num2;
		this.tanSlipAngle += this.deltaAngle1 * Time.deltaTime;
		this.slipAngle = -Mathf.Atan(this.tanSlipAngle) * 57.29578f / this.m_gripMaterial;
		this.longitudinalSlip = this.slipRatio / this.idealSlipRatio;
		this.lateralSlip = this.slipAngle / this.idealSlipAngle;
		float num5 = this.cardynamics.velo * 3.6f;
		if (this.cardynamics.arcadeMode && num5 > 35f)
		{
			float num6 = Mathf.Clamp01(Mathf.Abs(this.lateralSlip));
			float num7 = (Mathf.Abs(num5) - 35f) / 1325f;
			this.m_gripMaterial = (this.gripMaterial + num6 + num7) * this.gripFactor;
		}
		else
		{
			this.m_gripMaterial = this.gripMaterial * this.gripFactor;
		}
		if (this.cardynamics.beckman)
		{
			float num8 = Mathf.Max(Mathf.Sqrt(this.longitudinalSlip * this.longitudinalSlip + this.lateralSlip * this.lateralSlip), 0.0001f);
			this.Fx = this.longitudinalSlip / num8 * this.CalcLongitudinalForce(Fz, num8 * this.idealSlipRatio);
			this.Fy = this.lateralSlip / num8 * this.CalcLateralForce(Fz, num8 * this.idealSlipAngle);
		}
		else
		{
			this.Fx = this.CalcLongitudinalForce(Fz, this.slipRatio);
			this.Fy = this.CalcLateralForce(Fz, this.slipAngle);
			float f = this.slipAngle * 3.1415927f / 180f;
			float num9 = Mathf.Cos(f);
			num9 = Mathf.Abs(this.Fx) / (Mathf.Abs(this.Fx) + Mathf.Abs(this.Fy));
			float num10 = 1f - num9;
			float num11 = 1f;
			if (this.slipRatio < 0f)
			{
				num11 = -1f;
			}
			float num12 = this.slipRatio / (num11 + this.slipRatio);
			float num13 = Mathf.Tan(f) / (num11 + this.slipRatio);
			float num14 = Mathf.Sqrt(num12 * num12 + num13 * num13);
			float num15 = num14 / (1f - num14);
			float num16 = Mathf.Atan((num11 + this.slipRatio) * num14);
			float num17 = this.CalcLongitudinalForce(Fz, num15);
			float num18 = this.CalcLateralForce(Fz, num16 * 57.29578f);
			float num19 = Mathf.Abs(num9 * num17) + Mathf.Abs(num10 * num18);
			float num20 = Mathf.Sqrt(this.Fx * this.Fx + this.Fy * this.Fy);
			if (num20 > num19)
			{
				float num21 = num19 / num20;
				this.Fx *= num21;
				this.Fy *= num21;
			}
		}
		if (float.IsInfinity(this.Fx) || float.IsNaN(this.Fx))
		{
			this.Fx = 0f;
		}
		if (float.IsInfinity(this.Fy) || float.IsNaN(this.Fy))
		{
			this.Fy = 0f;
		}
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x0008AF70 File Offset: 0x00089170
	private void LookupIdealSlipRatioIdealSlipAngle(float load)
	{
		int length = this.slipRatio_hat.GetLength(0);
		float num = 0.5f;
		float num2 = load * 0.001f;
		if (num2 < num)
		{
			this.idealSlipRatio = this.slipRatio_hat[0];
			this.idealSlipAngle = this.slipAngle_hat[0];
		}
		else if (num2 >= num * (float)length)
		{
			this.idealSlipRatio = this.slipRatio_hat[length - 1];
			this.idealSlipAngle = this.slipAngle_hat[length - 1];
		}
		else
		{
			int num3 = (int)(num2 / num);
			num3--;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 >= this.slipRatio_hat.GetLength(0))
			{
				num3 = this.slipRatio_hat.GetLength(0) - 1;
			}
			float num4 = (num2 - num * (float)(num3 + 1)) / num;
			this.idealSlipRatio = this.slipRatio_hat[num3] * (1f - num4) + this.slipRatio_hat[num3 + 1] * num4;
			this.idealSlipAngle = this.slipAngle_hat[num3] * (1f - num4) + this.slipAngle_hat[num3 + 1] * num4;
		}
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x0008B078 File Offset: 0x00089278
	private void CalculateIdealSlipRatioIdealSlipAngle(int tablesize)
	{
		float num = 0.5f;
		Array.Resize<float>(ref this.slipRatio_hat, tablesize);
		Array.Resize<float>(ref this.slipAngle_hat, tablesize);
		for (int i = 0; i < tablesize; i++)
		{
			this.FindIdealSlipRatioIdealSlipAngle((float)(i + 1) * num, i, 400);
		}
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0008B0C8 File Offset: 0x000892C8
	private void FindIdealSlipRatioIdealSlipAngle(float load, int i, int iterations)
	{
		float num = 0f;
		load *= 1000f;
		for (float num2 = -2f; num2 < 2f; num2 += 4f / (float)iterations)
		{
			float num3 = this.CalcLongitudinalForce(load, num2);
			if (num3 > num)
			{
				this.slipRatio_hat[i] = num2;
				num = num3;
			}
		}
		num = 0f;
		for (float num2 = -20f; num2 < 20f; num2 += 40f / (float)iterations)
		{
			float num3 = this.CalcLateralForce(load, num2);
			if (num3 > num)
			{
				this.slipAngle_hat[i] = num2;
				num = num3;
			}
		}
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0008B168 File Offset: 0x00089368
	public void LoadTiresData(string tires)
	{
		if (tires == "truck")
		{
			if (this.wheelPos == WheelPos.FRONT_RIGHT || this.wheelPos == WheelPos.FRONT_LEFT)
			{
				Array.Copy(this.tireParameters.aFMonster, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bFMonster, this.b, this.b.Length);
			}
			else
			{
				Array.Copy(this.tireParameters.aRMonster, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bRMonster, this.b, this.b.Length);
			}
			Debug.Log(this.wheelPos + " Tire data loaded: " + tires);
		}
		else if (tires == "offroad")
		{
			if (this.wheelPos == WheelPos.FRONT_RIGHT || this.wheelPos == WheelPos.FRONT_LEFT)
			{
				Array.Copy(this.tireParameters.aFGmc, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bFGmc, this.b, this.b.Length);
			}
			else
			{
				Array.Copy(this.tireParameters.aRGmc, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bRGmc, this.b, this.b.Length);
			}
			Debug.Log(this.wheelPos + " Tire data loaded: " + tires);
		}
		else if (tires == "competition")
		{
			if (this.wheelPos == WheelPos.FRONT_RIGHT || this.wheelPos == WheelPos.FRONT_LEFT)
			{
				Array.Copy(this.tireParameters.aFSim, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bFSim, this.b, this.b.Length);
			}
			else
			{
				Array.Copy(this.tireParameters.aRSim, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bRSim, this.b, this.b.Length);
			}
			Debug.Log(this.wheelPos + " Tire data loaded: " + tires);
		}
		else if (tires == "touring")
		{
			if (this.wheelPos == WheelPos.FRONT_RIGHT || this.wheelPos == WheelPos.FRONT_LEFT)
			{
				Array.Copy(this.tireParameters.aFTouring, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bFTouring, this.b, this.b.Length);
			}
			else
			{
				Array.Copy(this.tireParameters.aRTouring, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bRTouring, this.b, this.b.Length);
			}
		}
		else if (tires == "sport")
		{
			if (this.wheelPos == WheelPos.FRONT_RIGHT || this.wheelPos == WheelPos.FRONT_LEFT)
			{
				Array.Copy(this.tireParameters.aFSport, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bFSport, this.b, this.b.Length);
			}
			else
			{
				Array.Copy(this.tireParameters.aRSport, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bRSport, this.b, this.b.Length);
			}
		}
		else if (tires == "superSport")
		{
			if (this.wheelPos == WheelPos.FRONT_RIGHT || this.wheelPos == WheelPos.FRONT_LEFT)
			{
				Array.Copy(this.tireParameters.aFRace, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bFRace, this.b, this.b.Length);
			}
			else
			{
				Array.Copy(this.tireParameters.aRRace, this.a, this.a.Length);
				Array.Copy(this.tireParameters.bRRace, this.b, this.b.Length);
			}
		}
		else if (!(tires == "standard"))
		{
			Debug.LogError("Tire data not found. tires = " + tires);
		}
		this.CalculateIdealSlipRatioIdealSlipAngle(20);
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x0008B5F0 File Offset: 0x000897F0
	private void Awake()
	{
		if (this.model == null)
		{
			Debug.LogError("You must specify the wheel model in " + this.wheelPos + " wheel.");
			Debug.Break();
		}
		else
		{
			this.modelTransform = this.model.transform;
			if (this.tireModel)
			{
				this.tireModelTransform = this.tireModel.transform;
				this.originalLocalScale = this.tireModelTransform.localScale;
			}
			this.trs = base.transform.parent;
			while (this.trs != null && this.trs.GetComponent<Rigidbody>() == null)
			{
				this.trs = this.trs.parent;
			}
			if (this.trs != null)
			{
				this.body = this.trs.GetComponent<Rigidbody>();
			}
			this.trs = base.transform.parent;
			while (this.trs.GetComponent<CarDynamics>() == null)
			{
				this.trs = this.trs.parent;
			}
			this.cardynamics = this.trs.GetComponent<CarDynamics>();
			this.trs = base.transform.parent;
			while (this.trs.GetComponent<PhysicMaterials>() == null)
			{
				if (!this.trs.parent)
				{
					break;
				}
				this.trs = this.trs.parent;
			}
			this.physicMaterials = this.trs.GetComponent<PhysicMaterials>();
			this.layerMask = (1 << this.trs.gameObject.layer | 1 << base.transform.gameObject.layer);
			this.layerMask = ~this.layerMask;
			this.tireParameters = new TireParameters();
			this.normalForceLimit = this.body.mass * 20f;
			this.radiusUnloaded = this.radius;
			if (this.rimRadius == 0f)
			{
				this.rimRadius = this.radiusUnloaded * 0.25f;
			}
			this.m_gripMaterial = this.gripMaterial;
			this.normalMass = 1f / this.mass;
			this.halfWidth = this.width / 2f;
			if (this.rotationalInertia == 0f)
			{
				this.rotationalInertia = this.mass / 2f * this.radiusUnloaded * this.radiusUnloaded;
			}
		}
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0008B894 File Offset: 0x00089A94
	private void Start()
	{
		this.skidmarks = this.cardynamics.skidmarks;
		if (this.skidmarks)
		{
			this.skidSmoke = (this.skidmarks.GetComponentInChildren(typeof(ParticleEmitter)) as ParticleEmitter);
		}
		this.suspensionLineRenderer = base.gameObject.AddComponent<LineRenderer>();
		this.suspensionLineRenderer.material = new Material(Shader.Find("Diffuse"));
		this.suspensionLineRenderer.material.color = Color.yellow;
		this.suspensionLineRenderer.SetWidth(0.01f, 0.1f);
		this.suspensionLineRenderer.useWorldSpace = false;
		this.suspensionLineRenderer.castShadows = false;
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0008B950 File Offset: 0x00089B50
	private Vector3 RoadForce(float normalForce, Vector3 groundNormal)
	{
		if (this.cardynamics.targetPlatform == CarDynamics.TargetPlatform.Mobile)
		{
			this.slipResFactor = 20f;
		}
		else if (this.cardynamics.targetPlatform == CarDynamics.TargetPlatform.PCandMAC)
		{
			this.slipResFactor = 5f;
		}
		else
		{
			this.slipResFactor = 5f;
		}
		this.slipRes = (int)((100f - Mathf.Abs(this.angularVelocity)) / this.slipResFactor);
		if (this.slipRes < 1)
		{
			this.slipRes = 1;
		}
		float num = 1f / (float)this.slipRes;
		this.rollingResistanceTorque = normalForce * this.rollingFrictionCoefficient * this.radius;
		float num2 = this.rotationalInertia + this.drivetrainInertia;
		float num3 = Time.deltaTime * num / num2;
		float num4 = this.driveTorque * num3;
		this.totalFrictionTorque = this.brakeFrictionTorque * 2f * this.brake + this.handbrakeFrictionTorque * 2f * this.handbrake + this.rollingResistanceTorque + this.driveFrictionTorque + this.wheelFrictionTorque;
		float num5 = this.totalFrictionTorque * num3;
		this.totalForce = Vector3.zero;
		this.newAngle = this.maxSteeringAngle * this.steering;
		Vector3 forward = Vector3.forward;
		Vector3 vector = Vector3.right;
		if (this.physicMaterials)
		{
			this.physicMaterial = this.hitDown.collider.sharedMaterial;
			if (this.physicMaterial == this.physicMaterials.track)
			{
				this.gripMaterial = this.physicMaterials.trackGrip;
				this.rollingFrictionCoefficient = this.physicMaterials.trackRollingFriction;
				this.dynamicFrictionCoefficient = this.physicMaterials.trackDynamicFriction;
				this.staticFrictionCoefficient = this.physicMaterials.trackStaticFriction;
			}
			else if (this.physicMaterial == this.physicMaterials.grass)
			{
				this.gripMaterial = this.physicMaterials.grassGrip;
				this.rollingFrictionCoefficient = this.physicMaterials.grassRollingFriction;
				this.dynamicFrictionCoefficient = this.physicMaterials.grassDynamicFriction;
				this.staticFrictionCoefficient = this.physicMaterials.grassStaticFriction;
			}
			else if (this.physicMaterial == this.physicMaterials.sand)
			{
				this.gripMaterial = this.physicMaterials.sandGrip;
				this.rollingFrictionCoefficient = this.physicMaterials.sandRollingFriction;
				this.dynamicFrictionCoefficient = this.physicMaterials.sandDynamicFriction;
				this.staticFrictionCoefficient = this.physicMaterials.sandStaticFriction;
			}
			else if (this.physicMaterial == this.physicMaterials.offRoad)
			{
				this.gripMaterial = this.physicMaterials.offRoadGrip;
				this.rollingFrictionCoefficient = this.physicMaterials.offRoadRollingFriction;
				this.dynamicFrictionCoefficient = this.physicMaterials.offRoadDynamicFriction;
				this.staticFrictionCoefficient = this.physicMaterials.offRoadStaticFriction;
			}
		}
		for (int i = 0; i < this.slipRes; i++)
		{
			float num6 = (float)i * 1f / (float)this.slipRes;
			this.localRotation = Quaternion.Euler(0f, this.oldAngle + (this.newAngle - this.oldAngle) * num6, 0f);
			this.right = base.transform.TransformDirection(this.localRotation * Vector3.right);
			this.camber_sin = Vector3.Dot(this.right, groundNormal);
			this.rightNormal = this.right - groundNormal * this.camber_sin;
			this.forwardNormal = Vector3.Cross(this.rightNormal, groundNormal);
			this.wheelRoadVelo = Vector3.Dot(this.wheelVelo, this.forwardNormal);
			this.wheelRoadVeloLat = Vector3.Dot(this.wheelVelo, this.rightNormal);
			this.CalcForces(normalForce);
			this.force = num * (forward * this.Fx + vector * this.Fy);
			this.longForce = this.forwardNormal * this.Fx;
			this.latForce = this.rightNormal * this.Fy;
			this.worldForce = num * (this.longForce + this.latForce);
			this.angularVelocity += num4;
			this.angularVelocity -= this.force.z * this.radius * Time.deltaTime / num2;
			if (Mathf.Abs(this.angularVelocity) > num5)
			{
				this.angularVelocity -= num5 * Mathf.Sign(this.angularVelocity);
			}
			else
			{
				this.angularVelocity = 0f;
			}
			this.totalForce += this.worldForce;
		}
		this.oldAngle = this.newAngle;
		return this.totalForce;
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0008BE4C File Offset: 0x0008A04C
	private void OnDrawGizmos()
	{
		if (this.cardynamics && this.cardynamics.tridimensionalTire)
		{
			Gizmos.DrawWireSphere(this.capsuleCenterDown, this.radius);
			if (this.capsuleCenterLat != Vector3.zero)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(this.capsuleCenterLat, this.radius / 2f);
			}
		}
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x0008BEC0 File Offset: 0x0008A0C0
	private void Update()
	{
		if (this.cardynamics.targetPlatform == CarDynamics.TargetPlatform.PCandMAC)
		{
			this.bumpStopLenght = this.suspensionTravel * 0.2f;
			this.pos = base.transform.position;
			this.up = base.transform.up;
			this.modelPosition = this.modelTransform.position;
			this.onGroundDown = Physics.Raycast(this.pos, -this.up, out this.hitDownR, this.suspensionTravel + this.radiusUnloaded, this.layerMask);
			this.hitDown = this.hitDownR;
			if (this.cardynamics.tridimensionalTire)
			{
				this.capsuleCenterDown = this.pos + this.up * this.radius;
				this.onGroundDown = Physics.SphereCast(this.capsuleCenterDown, this.radius, -this.up, out this.hitDownS, this.suspensionTravel + this.radiusUnloaded, this.layerMask);
				this.hitDown = this.hitDownS;
				this.sign = 1;
				if (this.wheelPos == WheelPos.FRONT_LEFT || this.wheelPos == WheelPos.REAR_LEFT)
				{
					this.sign = -1;
				}
				if (!this.onGroundDown)
				{
					this.right = base.transform.TransformDirection(this.modelTransform.localRotation * Vector3.right);
				}
				this.distance = this.radius / 2f;
				Vector3 direction = (float)this.sign * this.right;
				this.capsuleCenterLat = this.modelPosition - direction * this.halfWidth;
				this.lateralHit = Physics.SphereCast(this.capsuleCenterLat, this.radius / 2f, direction, out this.hitLat, this.distance + this.halfWidth, this.layerMask);
			}
			else
			{
				this.lateralHit = false;
			}
			if (this.lateralHit)
			{
				this.groundNormalLat = this.hitLat.normal;
				this.anglenormalLat = Vector3.Angle(this.groundNormalLat, this.up);
				this.anglelatLat = Vector3.Angle(this.groundNormalLat, this.right);
				this.sinLat = Mathf.Sin(this.anglenormalLat * 0.017453292f);
				this.cosLat = Mathf.Abs(Mathf.Cos(this.anglelatLat * 0.017453292f));
				this.angle = this.anglelatLat + (float)this.sign * this.maxSteeringAngle * Mathf.Abs(this.steering);
				if (this.angle <= 160f && this.angle >= 20f)
				{
					this.lateralHit = false;
				}
			}
			if (this.onGroundDown)
			{
				this.groundNormal = this.hitDown.normal;
				this.anglenormal = Vector3.Angle(this.groundNormal, this.up);
				this.anglelat = Vector3.Angle(this.groundNormal, this.right);
				if (!this.cardynamics.tridimensionalTire || (this.anglelat >= 35f && this.anglelat <= 145f && !this.lateralHit))
				{
					this.u_compression = this.suspensionTravel - (this.hitDown.distance - this.radius);
				}
				else
				{
					this.u_compression = this.suspensionTravel - (this.hitDownR.distance - this.radius);
				}
				this.u_normalCompression = this.u_compression / this.suspensionTravel;
				this.u_normalCompression = Mathf.Clamp(this.u_normalCompression, 0f, 1.1f);
				Debug.DrawRay(this.modelPosition, this.suspensionForce / 1000f, Color.yellow);
				Debug.DrawRay(this.hitDown.point, this.longForce / 1000f, Color.red);
				Debug.DrawRay(this.hitDown.point, this.latForce / 1000f, Color.blue);
				if (this.skidmarks != null)
				{
					this.CalcSkidmarks();
				}
			}
			else
			{
				this.u_compression = (this.u_normalCompression = 0f);
				this.deflection = 0f;
				this.radius = this.radiusUnloaded;
			}
			if (this.model != null)
			{
				this.rotation += this.angularVelocity * Time.deltaTime;
				if (float.IsInfinity(this.rotation) || float.IsNaN(this.rotation))
				{
					this.rotation = 0f;
				}
				this.modelTransform.localPosition = Vector3.up * (this.u_normalCompression - 1f) * this.suspensionTravel;
				this.modelTransform.localRotation = Quaternion.Euler(57.29578f * this.rotation, this.maxSteeringAngle * this.steering, 0f);
				if (this.tireModel != null)
				{
					if (this.cardynamics.tireRate != 0f)
					{
						this.tireModelTransform.localScale = new Vector3(this.tireModelTransform.localScale.x, this.radius / this.radiusUnloaded * this.originalLocalScale.y, this.radius / this.radiusUnloaded * this.originalLocalScale.z);
					}
					else
					{
						this.tireModelTransform.localScale = new Vector3(this.modelTransform.localScale.x, this.originalLocalScale.y, this.originalLocalScale.z);
					}
				}
			}
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x0008C494 File Offset: 0x0008A694
	private void FixedUpdate()
	{
		this.pos = base.transform.position;
		this.up = base.transform.up;
		this.modelPosition = this.modelTransform.position;
		this.wheelVelo = this.body.GetPointVelocity(this.modelPosition);
		this.dampAbsRoadVelo = this.cardynamics.dampAbsRoadVelo;
		this.onGroundDown = Physics.Raycast(this.pos, -this.up, out this.hitDownR, this.suspensionTravel + this.radiusUnloaded, this.layerMask);
		this.hitDown = this.hitDownR;
		if (this.cardynamics.tridimensionalTire)
		{
			this.capsuleCenterDown = this.pos + this.up * this.radius;
			this.onGroundDown = Physics.SphereCast(this.capsuleCenterDown, this.radius, -this.up, out this.hitDownS, this.suspensionTravel + this.radiusUnloaded, this.layerMask);
			this.hitDown = this.hitDownS;
			this.sign = 1;
			if (this.wheelPos == WheelPos.FRONT_LEFT || this.wheelPos == WheelPos.REAR_LEFT)
			{
				this.sign = -1;
			}
			this.distance = this.radius / 2f;
			if (!this.onGroundDown)
			{
				this.right = base.transform.TransformDirection(this.modelTransform.localRotation * Vector3.right);
			}
			Vector3 direction = (float)this.sign * this.right;
			this.capsuleCenterLat = this.modelPosition - direction * this.halfWidth;
			this.lateralHit = Physics.SphereCast(this.capsuleCenterLat, this.radius / 2f, direction, out this.hitLat, this.distance + this.halfWidth, this.layerMask);
		}
		else
		{
			this.lateralHit = false;
		}
		if (this.lateralHit)
		{
			this.groundNormalLat = this.hitLat.normal;
			this.anglenormalLat = Vector3.Angle(this.groundNormalLat, this.up);
			this.anglelatLat = Vector3.Angle(this.groundNormalLat, this.right);
			this.sinLat = Mathf.Sin(this.anglenormalLat * 0.017453292f);
			this.cosLat = Mathf.Abs(Mathf.Cos(this.anglelatLat * 0.017453292f));
			this.angle = this.anglelatLat + (float)this.sign * this.maxSteeringAngle * Mathf.Abs(this.steering);
			if (this.angle <= 160f && this.angle >= 20f)
			{
				this.lateralHit = false;
			}
			Debug.DrawRay(this.hitLat.point, this.hitLat.distance * (float)(-(float)this.sign) * this.right, Color.green);
			if (this.hitLat.distance * this.sinLat * this.cosLat <= this.halfWidth)
			{
				this.onGroundLat = true;
			}
			else
			{
				this.onGroundLat = false;
			}
			if (this.onGroundLat)
			{
				float sink = Mathf.Abs(this.hitLat.distance - this.distance);
				this.tireForceLat = this.TireForceLat(sink, this.groundNormalLat, false);
				this.body.AddForceAtPosition(this.tireForceLat, this.modelPosition);
				this.tireForceLat = this.ImpulseForce((float)(-(float)this.sign) * base.transform.right, this.groundNormalLat, this.body.inertiaTensor.z);
				this.body.AddForceAtPosition(this.tireForceLat, this.modelPosition, ForceMode.Impulse);
				Debug.DrawRay(this.modelPosition, this.tireForceLat, Color.black);
				if (this.wheelVelo.sqrMagnitude > 0.5f)
				{
					float d = this.dynamicFrictionCoefficient * this.m_gripMaterial * this.normalTireForceLat * this.cosLat * this.sinLat;
					this.body.AddForceAtPosition(d * -this.wheelVelo.normalized, this.modelPosition);
					Debug.DrawRay(this.modelPosition, this.wheelVelo.normalized, Color.red);
					Debug.DrawRay(this.modelPosition, d * -this.wheelVelo.normalized, Color.black);
				}
			}
			else
			{
				this.tireForceLat = Vector3.zero;
			}
		}
		else
		{
			this.onGroundLat = false;
		}
		if (this.onGroundDown)
		{
			this.localVelo = this.localRotation * this.wheelVelo;
			this.groundNormal = this.hitDown.normal;
			this.anglenormal = Vector3.Angle(this.groundNormal, this.up);
			this.anglelat = Vector3.Angle(this.groundNormal, this.right);
			if (this.cardynamics.tridimensionalTire)
			{
				if (this.anglelat > 145f || this.anglelat < 35f)
				{
					this.groundNormal = this.up;
					Debug.DrawRay(this.body.position, this.up, Color.white);
				}
			}
			else if (this.anglenormal > 45f)
			{
				this.groundNormal = this.up;
				Debug.DrawRay(this.body.position, this.up, Color.white);
			}
			if (Vector3.Angle(this.hitDownR.normal, this.up) > 45f)
			{
				this.hitDownR.normal = this.up;
			}
			if (!this.cardynamics.tridimensionalTire || (this.anglelat >= 35f && this.anglelat <= 145f && !this.lateralHit))
			{
				this.compression = this.suspensionTravel - (Mathf.Max(this.hitDown.distance, this.radius) - this.radius);
			}
			else
			{
				this.compression = this.suspensionTravel - (Mathf.Max(this.hitDownR.distance, this.radius) - this.radius);
			}
			this.compression = Mathf.Clamp(this.compression, 0f, this.suspensionTravel);
			this.normalCompression = this.compression / this.suspensionTravel;
			this.suspensionForce = this.SuspensionForce(this.compression, this.up);
			this.normalForce = this.normalSuspensionForce + this.normalTireForce;
			if (this.compression >= this.suspensionTravel - this.bumpStopLenght)
			{
				this.bumpStopForce = this.ImpulseForce(this.up, this.hitDownR.normal, this.body.inertiaTensor.z);
				this.body.AddForceAtPosition(this.bumpStopForce, this.pos, ForceMode.Impulse);
			}
			else
			{
				this.J = 0f;
				this.bumpStopForce = Vector3.zero;
			}
			if (this.showForces)
			{
				this.suspensionLineRenderer.enabled = true;
				this.suspensionLineRenderer.SetPosition(1, new Vector3(0f, 0.0005f * this.suspensionForce.y, 0f));
			}
			else
			{
				this.suspensionLineRenderer.enabled = false;
			}
			if (this.cardynamics.tireRate != 0f)
			{
				if (this.anglelat >= 70f && this.anglelat <= 110f)
				{
					this.tireForce = this.TireForce(this.groundNormal, true);
					this.radius = this.radiusUnloaded - this.deflection;
					this.body.AddForceAtPosition(this.tireForce, this.modelPosition);
					Debug.DrawRay(this.hitDown.point, this.tireForce / 1000f, Color.magenta);
				}
				else
				{
					this.tireForce = Vector3.zero;
					this.deflection = 0f;
					this.normalTireForce = 0f;
				}
			}
			else
			{
				this.tireForce = Vector3.zero;
				this.deflection = 0f;
				this.normalTireForce = 0f;
			}
			this.roadForce = this.RoadForce(this.normalForce, this.groundNormal);
			this.body.AddForceAtPosition(this.roadForce + this.suspensionForce, this.modelPosition);
			if (this.hitDown.rigidbody != null)
			{
				this.hitDown.rigidbody.AddForceAtPosition(-this.suspensionForce, this.hitDown.point);
				Debug.DrawRay(this.hitDown.point, -this.suspensionForce);
			}
			float sqrMagnitude = this.body.velocity.sqrMagnitude;
			if (sqrMagnitude <= 4f)
			{
				Vector3 vector;
				if (sqrMagnitude < 1f && this.brake != 0f)
				{
					vector = this.right * (1f - this.brake) + base.transform.right * this.brake;
				}
				else
				{
					vector = this.right;
				}
				float num = Vector3.Angle(-vector, Vector3.up);
				float num2 = this.CalculateFractionalMass() * -Physics.gravity.y;
				float num3 = this.staticFrictionCoefficient * this.m_gripMaterial * num2;
				float num4 = num2 * Mathf.Cos(num * 0.017453292f);
				if (num4 < num3)
				{
					this.body.AddForceAtPosition(num4 * -vector, this.modelPosition);
					Debug.DrawRay(this.modelPosition, num4 * -vector / 1000f, Color.yellow);
				}
				if (sqrMagnitude < 1f && this.brake != 0f)
				{
					Vector3 from = base.transform.forward * this.brake;
					float num5 = Vector3.Angle(from, Vector3.up);
					float num6 = num2 * Mathf.Cos(num5 * 0.017453292f);
					if (num6 < this.totalFrictionTorque)
					{
						this.body.AddForceAtPosition(num6 * from, this.modelPosition);
						Debug.DrawRay(this.modelPosition, num6 * from / 1000f, Color.yellow);
					}
				}
			}
			this.longitunalSlipVelo = Mathf.Abs(this.wheelTireVelo - this.wheelRoadVelo);
			this.lateralSlipVelo = this.wheelRoadVeloLat;
			this.slipVelo = Mathf.Sqrt(this.longitunalSlipVelo * this.longitunalSlipVelo + this.lateralSlipVelo * this.lateralSlipVelo);
		}
		else
		{
			this.compression = 0f;
			this.normalCompression = 0f;
			this.normalForce = 0f;
			this.suspensionForce = Vector3.zero;
			this.roadForce = Vector3.zero;
			this.tireForce = Vector3.zero;
			this.deflection = 0f;
			this.Fx = (this.Fy = 0f);
			this.slipAngle = (this.slipRatio = 0f);
			this.idealSlipRatio = (this.idealSlipAngle = 0f);
			this.suspensionLineRenderer.enabled = false;
			this.lastSkid = -1;
			float num7 = this.rotationalInertia + this.drivetrainInertia;
			float num8 = Time.deltaTime / num7;
			float num9 = this.driveTorque * num8;
			this.totalFrictionTorque = this.brakeFrictionTorque * 2f * this.brake + this.handbrakeFrictionTorque * 2f * this.handbrake + this.driveFrictionTorque + this.wheelFrictionTorque;
			if (!this.onGroundLat)
			{
				this.body.AddForceAtPosition(this.mass * Physics.gravity, this.modelPosition);
				Debug.DrawRay(this.modelPosition, this.mass * Physics.gravity / 1000f);
			}
			float num10 = this.totalFrictionTorque * num8;
			this.angularVelocity += num9;
			if (Mathf.Abs(this.angularVelocity) > num10)
			{
				this.angularVelocity -= num10 * Mathf.Sign(this.angularVelocity);
			}
			else
			{
				this.angularVelocity = 0f;
			}
			this.slipRatio = 0f;
			this.slipVelo = 0f;
		}
		if (this.cardynamics.targetPlatform == CarDynamics.TargetPlatform.Mobile && this.model != null)
		{
			this.rotation += this.angularVelocity * Time.deltaTime;
			if (float.IsInfinity(this.rotation) || float.IsNaN(this.rotation))
			{
				this.rotation = 0f;
			}
			this.modelTransform.localPosition = Vector3.up * (this.normalCompression - 1f) * this.suspensionTravel;
			this.modelTransform.localRotation = Quaternion.Euler(57.29578f * this.rotation, this.maxSteeringAngle * this.steering, 0f);
			if (this.tireModel != null)
			{
				if (this.cardynamics.tireRate != 0f)
				{
					this.tireModelTransform.localScale = new Vector3(this.tireModelTransform.localScale.x, this.radius / this.radiusUnloaded * this.originalLocalScale.y, this.radius / this.radiusUnloaded * this.originalLocalScale.z);
				}
				else
				{
					this.tireModelTransform.localScale = new Vector3(this.modelTransform.localScale.x, this.originalLocalScale.y, this.originalLocalScale.z);
				}
			}
		}
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x0008D2D8 File Offset: 0x0008B4D8
	private float CalculateFractionalMass()
	{
		float num;
		if (this.wheelPos == WheelPos.FRONT_LEFT || this.wheelPos == WheelPos.FRONT_RIGHT)
		{
			num = this.cardynamics.frontRearWeightRepartition / (float)(this.cardynamics.allWheels.Length / 2);
		}
		else
		{
			num = (1f - this.cardynamics.frontRearWeightRepartition) / (float)(this.cardynamics.allWheels.Length / 2);
		}
		return num * this.body.mass;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0008D350 File Offset: 0x0008B550
	private void CalcSkidmarks()
	{
		if (this.slipVelo > this.slipVeloThreshold)
		{
			this.slipAmount = (this.slipVelo - this.slipVeloThreshold) / 15f;
			this.skidmarks.markWidth = this.width;
			this.lastSkid = this.skidmarks.AddSkidMark(this.hitDown.point, this.hitDown.normal, this.slipAmount, this.lastSkid);
			Vector3 vector = base.transform.TransformDirection(this.skidSmoke.localVelocity) + this.skidSmoke.worldVelocity;
			float num = this.absRoadVelo - 15f;
			this.skidSmoke.Emit(this.hitDown.point + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f)), vector + this.wheelVelo * 0.05f, UnityEngine.Random.Range(this.skidSmoke.minSize, this.skidSmoke.maxSize) * Mathf.Clamp(num * 0.1f, 0.5f, 1f), UnityEngine.Random.Range(this.skidSmoke.minEnergy, this.skidSmoke.maxEnergy), Color.white);
		}
		else
		{
			this.lastSkid = -1;
		}
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x0008D4C0 File Offset: 0x0008B6C0
	private Vector3 SuspensionForce(float compression, Vector3 normal)
	{
		this.springForce = this.suspensionRate * compression;
		float num = this.springForce;
		this.normalVelocity = -Vector3.Dot(this.wheelVelo, this.hitDownR.normal);
		float num2 = num / this.mass * Time.deltaTime;
		float num3 = (-this.normalVelocity + num2) * Time.deltaTime;
		this.normalVelocity += num3;
		float num4;
		if (this.normalVelocity >= 0f)
		{
			num4 = this.bumpRate * this.normalVelocity;
		}
		else
		{
			num4 = -this.reboundRate * Mathf.Sqrt(-this.normalVelocity);
		}
		num += num4;
		num += this.antiRollBarForce;
		float num5 = this.normalVelocity * this.normalMass / Time.deltaTime;
		float num6 = (-num5 + num) * Time.deltaTime;
		num += num6;
		this.normalSuspensionForce = num;
		if (this.normalSuspensionForce < 0f)
		{
			this.normalSuspensionForce = 0f;
		}
		if (this.normalSuspensionForce > this.normalForceLimit)
		{
			this.normalSuspensionForce = this.normalForceLimit;
		}
		Debug.DrawRay(this.modelPosition, this.normalSuspensionForce * normal, Color.black);
		return normal * this.normalSuspensionForce;
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x0008D600 File Offset: 0x0008B800
	private Vector3 TireForce(Vector3 normal, bool clamp)
	{
		float num = this.springForce;
		this.deflection = num / this.cardynamics.tireRate;
		this.deflection = Mathf.Clamp(this.deflection, 0f, this.radiusUnloaded - this.rimRadius);
		float num2 = Vector3.Dot(this.localVelo, normal);
		float num3 = num / this.mass * Time.deltaTime;
		float num4 = (num2 - num3) * Time.deltaTime;
		num2 += num4;
		float num5;
		if (Mathf.Abs(this.angularVelocity) <= 5f)
		{
			num5 = num2 * this.cardynamics.tireDampingLow;
		}
		else
		{
			num5 = num2 * this.cardynamics.tireDamping;
		}
		num -= num5;
		float num6 = num2 * this.normalMass / Time.deltaTime;
		float num7 = (-num6 + num) * Time.deltaTime;
		num += num7;
		this.normalTireForce = num;
		if (this.normalTireForce < 0f)
		{
			this.normalTireForce = 0f;
		}
		return this.normalTireForce * normal;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0008D700 File Offset: 0x0008B900
	private Vector3 TireForceLat(float sink, Vector3 normal, bool clamp)
	{
		if (clamp)
		{
			sink = Mathf.Clamp(sink, 0f, this.radius / 2f);
		}
		float num = 60000f * sink;
		this.normalTireForceLat = num;
		float num2 = Vector3.Dot(this.localVelo, normal);
		float num3 = num2 * 250f;
		num -= num3;
		if (num < 0f)
		{
			num = 0f;
		}
		return num * normal;
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0008D76C File Offset: 0x0008B96C
	private Vector3 ImpulseForce(Vector3 normal, Vector3 hitDownRnormal, float inertiaTensor)
	{
		float num = Vector3.Dot(this.wheelVelo, hitDownRnormal);
		Vector3 lhs = this.body.worldCenterOfMass - this.modelPosition;
		float num2 = Vector3.Dot(Vector3.Cross(lhs, normal), Vector3.Cross(lhs, normal)) / inertiaTensor;
		this.J = -num / (1f / this.body.mass + num2);
		if (this.cardynamics.otherWheels.Length != 0)
		{
			this.J /= (float)this.cardynamics.otherWheels.Length;
		}
		if (this.J < 0f)
		{
			this.J = 0f;
		}
		return this.J * normal;
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0008D824 File Offset: 0x0008BA24
	private float RoundTo(float value, int precision)
	{
		int num = 1;
		for (int i = 1; i <= precision; i++)
		{
			num *= 10;
		}
		return Mathf.Round(value * (float)num) / (float)num;
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0008D858 File Offset: 0x0008BA58
	private void OnGUI()
	{
		if (this.debug)
		{
			if (this.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 280f, 500f, 200f), string.Concat(new object[]
				{
					"hitDown.distance - radius : ",
					this.hitDown.distance,
					"-       ",
					(float)this.slipRes / this.slipResFactor
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 300f, 500f, 200f), string.Concat(new object[]
				{
					"hitDown.normal .x         .z ",
					this.hitDown.normal.x,
					" ",
					this.hitDown.normal.z
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 320f, 500f, 200f), string.Concat(new object[]
				{
					"groundNormal.x groundNormal.z ",
					this.groundNormal.x,
					" ",
					this.groundNormal.z
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 340f, 500f, 200f), "wheelRoadVeloLatFL: " + this.wheelRoadVeloLat);
			}
			if (this.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 360f, 500f, 200f), "wheelRoadVeloLatFR: " + this.wheelRoadVeloLat);
			}
			if (this.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 380f, 500f, 200f), "absRoadVelo : " + Mathf.Abs(this.wheelRoadVelo));
			}
			if (this.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 400f, 500f, 200f), string.Concat(new object[]
				{
					"idealSlipRatioFL :",
					this.idealSlipRatio,
					" idealSlipAngleFL ",
					this.idealSlipAngle
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 420f, 500f, 200f), string.Concat(new object[]
				{
					"idealSlipRatioFR :",
					this.idealSlipRatio,
					" idealSlipAngleFR ",
					this.idealSlipAngle
				}));
			}
			if (this.wheelPos == WheelPos.REAR_LEFT)
			{
				GUI.Label(new Rect(300f, 440f, 500f, 200f), string.Concat(new object[]
				{
					"idealSlipRatioRL :",
					this.idealSlipRatio,
					" idealSlipAngleRL ",
					this.idealSlipAngle
				}));
			}
			if (this.wheelPos == WheelPos.REAR_RIGHT)
			{
				GUI.Label(new Rect(300f, 460f, 500f, 200f), string.Concat(new object[]
				{
					"idealSlipRatioRR :",
					this.idealSlipRatio,
					" idealSlipAngleRR ",
					this.idealSlipAngle
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 480f, 500f, 200f), string.Concat(new object[]
				{
					"slipRatioFL: ",
					this.slipRatio,
					"    Fx: ",
					this.Fx,
					" s:",
					this.RoundTo(this.slipRatio / this.idealSlipRatio, 3),
					" normalForce:",
					this.normalForce
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 500f, 500f, 200f), string.Concat(new object[]
				{
					"slipRatioFR: ",
					this.slipRatio,
					"    Fx: ",
					this.Fx,
					" s:",
					this.RoundTo(this.slipRatio / this.idealSlipRatio, 3),
					" normalForce:",
					this.normalForce
				}));
			}
			if (this.wheelPos == WheelPos.REAR_LEFT)
			{
				GUI.Label(new Rect(300f, 520f, 500f, 200f), string.Concat(new object[]
				{
					"slipRatioRL: ",
					this.slipRatio,
					"    Fx: ",
					this.Fx,
					" s:",
					this.RoundTo(this.slipRatio / this.idealSlipRatio, 3),
					" normalForce:",
					this.normalForce
				}));
			}
			if (this.wheelPos == WheelPos.REAR_RIGHT)
			{
				GUI.Label(new Rect(300f, 540f, 500f, 200f), string.Concat(new object[]
				{
					"slipRatioRR: ",
					this.slipRatio,
					"    Fx: ",
					this.Fx,
					" s:",
					this.RoundTo(this.slipRatio / this.idealSlipRatio, 3),
					" normalForce:",
					this.normalForce
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_LEFT)
			{
				GUI.Label(new Rect(300f, 560f, 500f, 200f), string.Concat(new object[]
				{
					"slipAngleFL: ",
					this.slipAngle,
					"    Fy: ",
					this.Fy,
					" a:",
					this.RoundTo(this.slipAngle / this.idealSlipAngle, 3),
					" rho:",
					Mathf.Sqrt(this.slipAngle / this.idealSlipAngle * (this.slipAngle / this.idealSlipAngle) + this.slipRatio / this.idealSlipRatio * (this.slipRatio / this.idealSlipRatio))
				}));
			}
			if (this.wheelPos == WheelPos.FRONT_RIGHT)
			{
				GUI.Label(new Rect(300f, 580f, 500f, 200f), string.Concat(new object[]
				{
					"slipAngleFR: ",
					this.slipAngle,
					"    Fy: ",
					this.Fy,
					" a:",
					this.RoundTo(this.slipAngle / this.idealSlipAngle, 3),
					" rho:",
					Mathf.Sqrt(this.slipAngle / this.idealSlipAngle * (this.slipAngle / this.idealSlipAngle) + this.slipRatio / this.idealSlipRatio * (this.slipRatio / this.idealSlipRatio))
				}));
			}
			if (this.wheelPos == WheelPos.REAR_LEFT)
			{
				GUI.Label(new Rect(300f, 600f, 500f, 200f), string.Concat(new object[]
				{
					"slipAngleRL: ",
					this.slipAngle,
					"    Fy: ",
					this.Fy,
					" a:",
					this.RoundTo(this.slipAngle / this.idealSlipAngle, 3),
					" rho:",
					Mathf.Sqrt(this.slipAngle / this.idealSlipAngle * (this.slipAngle / this.idealSlipAngle) + this.slipRatio / this.idealSlipRatio * (this.slipRatio / this.idealSlipRatio))
				}));
			}
			if (this.wheelPos == WheelPos.REAR_RIGHT)
			{
				GUI.Label(new Rect(300f, 620f, 500f, 200f), string.Concat(new object[]
				{
					"slipAngleRR: ",
					this.slipAngle,
					"    Fy: ",
					this.Fy,
					" a:",
					this.RoundTo(this.slipAngle / this.idealSlipAngle, 3),
					" rho:",
					Mathf.Sqrt(this.slipAngle / this.idealSlipAngle * (this.slipAngle / this.idealSlipAngle) + this.slipRatio / this.idealSlipRatio * (this.slipRatio / this.idealSlipRatio))
				}));
			}
		}
	}

	// Token: 0x040013B0 RID: 5040
	[HideInInspector]
	public WheelPos wheelPos;

	// Token: 0x040013B1 RID: 5041
	[HideInInspector]
	public PhysicMaterial physicMaterial;

	// Token: 0x040013B2 RID: 5042
	public GameObject model;

	// Token: 0x040013B3 RID: 5043
	public GameObject tireModel;

	// Token: 0x040013B4 RID: 5044
	private LineRenderer suspensionLineRenderer;

	// Token: 0x040013B5 RID: 5045
	[HideInInspector]
	public bool showForces;

	// Token: 0x040013B6 RID: 5046
	private float anglenormal;

	// Token: 0x040013B7 RID: 5047
	private float anglelat;

	// Token: 0x040013B8 RID: 5048
	private float anglenormalLat;

	// Token: 0x040013B9 RID: 5049
	private float anglelatLat;

	// Token: 0x040013BA RID: 5050
	private float angle;

	// Token: 0x040013BB RID: 5051
	private float anglez;

	// Token: 0x040013BC RID: 5052
	private float differentialSlipRatio;

	// Token: 0x040013BD RID: 5053
	private float tanSlipAngle;

	// Token: 0x040013BE RID: 5054
	private float deltaRatio1;

	// Token: 0x040013BF RID: 5055
	private float deltaRatio2;

	// Token: 0x040013C0 RID: 5056
	private float deltaRatio3;

	// Token: 0x040013C1 RID: 5057
	private float deltaRatio4;

	// Token: 0x040013C2 RID: 5058
	private float deltaAngle1;

	// Token: 0x040013C3 RID: 5059
	private float deltaAngle2;

	// Token: 0x040013C4 RID: 5060
	private float deltaAngle3;

	// Token: 0x040013C5 RID: 5061
	private float deltaAngle4;

	// Token: 0x040013C6 RID: 5062
	private float cosLat;

	// Token: 0x040013C7 RID: 5063
	private float sinLat;

	// Token: 0x040013C8 RID: 5064
	private float coeffLateral;

	// Token: 0x040013C9 RID: 5065
	private bool debug;

	// Token: 0x040013CA RID: 5066
	private Vector3 wheelMotionDirection;

	// Token: 0x040013CB RID: 5067
	private int slipRes;

	// Token: 0x040013CC RID: 5068
	private float slipResFactor;

	// Token: 0x040013CD RID: 5069
	private float lateralSlipVelo;

	// Token: 0x040013CE RID: 5070
	private float longitunalSlipVelo;

	// Token: 0x040013CF RID: 5071
	private float dampingslipAngle;

	// Token: 0x040013D0 RID: 5072
	private float dampingslipRatio;

	// Token: 0x040013D1 RID: 5073
	private float[] slipRatio_hat;

	// Token: 0x040013D2 RID: 5074
	private float[] slipAngle_hat;

	// Token: 0x040013D3 RID: 5075
	private Vector3 force;

	// Token: 0x040013D4 RID: 5076
	private Vector3 worldForce;

	// Token: 0x040013D5 RID: 5077
	private Vector3 totalForce;

	// Token: 0x040013D6 RID: 5078
	private Vector3 pos;

	// Token: 0x040013D7 RID: 5079
	private Vector3 modelPosition;

	// Token: 0x040013D8 RID: 5080
	private float frontRearRep;

	// Token: 0x040013D9 RID: 5081
	private float Fx;

	// Token: 0x040013DA RID: 5082
	private float Fy;

	// Token: 0x040013DB RID: 5083
	private Vector3 latForce;

	// Token: 0x040013DC RID: 5084
	private Vector3 longForce;

	// Token: 0x040013DD RID: 5085
	private Vector3 directionLat;

	// Token: 0x040013DE RID: 5086
	private int layerMask;

	// Token: 0x040013DF RID: 5087
	[HideInInspector]
	public float feedback;

	// Token: 0x040013E0 RID: 5088
	public bool onGroundDown;

	// Token: 0x040013E1 RID: 5089
	public bool onGroundLat;

	// Token: 0x040013E2 RID: 5090
	private bool lateralHit;

	// Token: 0x040013E3 RID: 5091
	private Vector3 tireForce;

	// Token: 0x040013E4 RID: 5092
	private Vector3 tireForceLat;

	// Token: 0x040013E5 RID: 5093
	private Vector3 tireForceVector;

	// Token: 0x040013E6 RID: 5094
	private RaycastHit hitDown;

	// Token: 0x040013E7 RID: 5095
	private RaycastHit hitDownR;

	// Token: 0x040013E8 RID: 5096
	private RaycastHit hitDownS;

	// Token: 0x040013E9 RID: 5097
	private RaycastHit hitLat;

	// Token: 0x040013EA RID: 5098
	private Vector3 capsuleCenterLat;

	// Token: 0x040013EB RID: 5099
	private Vector3 capsuleCenterDown;

	// Token: 0x040013EC RID: 5100
	private float halfWidth;

	// Token: 0x040013ED RID: 5101
	public float deflection;

	// Token: 0x040013EE RID: 5102
	private Vector3 originalLocalScale;

	// Token: 0x040013EF RID: 5103
	public float mass = 20f;

	// Token: 0x040013F0 RID: 5104
	private float normalMass;

	// Token: 0x040013F1 RID: 5105
	public float radius = 0.34f;

	// Token: 0x040013F2 RID: 5106
	public float rimRadius;

	// Token: 0x040013F3 RID: 5107
	private float radiusUnloaded;

	// Token: 0x040013F4 RID: 5108
	public float width = 0.2f;

	// Token: 0x040013F5 RID: 5109
	[HideInInspector]
	public float suspensionTravel = 0.2f;

	// Token: 0x040013F6 RID: 5110
	[HideInInspector]
	public float suspensionRate = 20000f;

	// Token: 0x040013F7 RID: 5111
	[HideInInspector]
	public float bumpRate = 4000f;

	// Token: 0x040013F8 RID: 5112
	[HideInInspector]
	public float reboundRate = 4000f;

	// Token: 0x040013F9 RID: 5113
	[HideInInspector]
	public float bumpStopLenght = 0.04f;

	// Token: 0x040013FA RID: 5114
	public float rotationalInertia;

	// Token: 0x040013FB RID: 5115
	public float brakeFrictionTorque = 1500f;

	// Token: 0x040013FC RID: 5116
	public float handbrakeFrictionTorque;

	// Token: 0x040013FD RID: 5117
	private float rollingResistanceTorque;

	// Token: 0x040013FE RID: 5118
	public float rollingFrictionCoefficient = 0.018f;

	// Token: 0x040013FF RID: 5119
	private float totalFrictionTorque;

	// Token: 0x04001400 RID: 5120
	private float dynamicFrictionCoefficient = 0.6f;

	// Token: 0x04001401 RID: 5121
	private float staticFrictionCoefficient = 1f;

	// Token: 0x04001402 RID: 5122
	public float gripMaterial = 1f;

	// Token: 0x04001403 RID: 5123
	[HideInInspector]
	public float gripFactor = 1f;

	// Token: 0x04001404 RID: 5124
	private float m_gripMaterial;

	// Token: 0x04001405 RID: 5125
	private float long_shapeFactor;

	// Token: 0x04001406 RID: 5126
	private float long_frictioncoefficient;

	// Token: 0x04001407 RID: 5127
	private float long_loadinfluence;

	// Token: 0x04001408 RID: 5128
	private float long_stiffnessFactor;

	// Token: 0x04001409 RID: 5129
	private float lat_shapeFactor;

	// Token: 0x0400140A RID: 5130
	private float lat_frictioncoefficient;

	// Token: 0x0400140B RID: 5131
	private float lat_loadinfluence;

	// Token: 0x0400140C RID: 5132
	private float lat_stiffnessFactor;

	// Token: 0x0400140D RID: 5133
	public float maxSteeringAngle = 33f;

	// Token: 0x0400140E RID: 5134
	private float[] a = new float[]
	{
		1.5f,
		-40f,
		1600f,
		2600f,
		8.7f,
		0.014f,
		-0.24f,
		1f,
		-0.03f,
		-0.0013f,
		-0.06f,
		-8.5f,
		-0.29f,
		17.8f,
		-2.4f
	};

	// Token: 0x0400140F RID: 5135
	private float[] b = new float[]
	{
		1.5f,
		-80f,
		1950f,
		23.3f,
		390f,
		0.05f,
		0f,
		0.055f,
		-0.024f,
		0.014f,
		0.26f
	};

	// Token: 0x04001410 RID: 5136
	private float[] c;

	// Token: 0x04001411 RID: 5137
	private CarDynamics cardynamics;

	// Token: 0x04001412 RID: 5138
	private PhysicMaterials physicMaterials;

	// Token: 0x04001413 RID: 5139
	private TireParameters tireParameters;

	// Token: 0x04001414 RID: 5140
	private float wheelFrictionTorque = 0.5f;

	// Token: 0x04001415 RID: 5141
	public float driveTorque;

	// Token: 0x04001416 RID: 5142
	[HideInInspector]
	public float driveFrictionTorque;

	// Token: 0x04001417 RID: 5143
	[HideInInspector]
	public float drivetrainInertia;

	// Token: 0x04001418 RID: 5144
	public float brake;

	// Token: 0x04001419 RID: 5145
	public float handbrake;

	// Token: 0x0400141A RID: 5146
	public float steering;

	// Token: 0x0400141B RID: 5147
	[HideInInspector]
	public float antiRollBarForce;

	// Token: 0x0400141C RID: 5148
	public float angularVelocity;

	// Token: 0x0400141D RID: 5149
	[HideInInspector]
	public float slipVelo;

	// Token: 0x0400141E RID: 5150
	private float slipVeloThreshold = 4f;

	// Token: 0x0400141F RID: 5151
	[HideInInspector]
	public float slipRatio;

	// Token: 0x04001420 RID: 5152
	[HideInInspector]
	public float slipAngle;

	// Token: 0x04001421 RID: 5153
	[HideInInspector]
	public float longitudinalSlip;

	// Token: 0x04001422 RID: 5154
	[HideInInspector]
	public float lateralSlip;

	// Token: 0x04001423 RID: 5155
	[HideInInspector]
	public float idealSlipRatio;

	// Token: 0x04001424 RID: 5156
	[HideInInspector]
	public float idealSlipAngle;

	// Token: 0x04001425 RID: 5157
	private float slipAmount;

	// Token: 0x04001426 RID: 5158
	public float compression;

	// Token: 0x04001427 RID: 5159
	private float u_compression;

	// Token: 0x04001428 RID: 5160
	private float mlastCompression;

	// Token: 0x04001429 RID: 5161
	private float normalCompression;

	// Token: 0x0400142A RID: 5162
	private float u_normalCompression;

	// Token: 0x0400142B RID: 5163
	private float wheelTireVelo;

	// Token: 0x0400142C RID: 5164
	private float wheelRoadVelo;

	// Token: 0x0400142D RID: 5165
	private float absRoadVelo;

	// Token: 0x0400142E RID: 5166
	private float dampAbsRoadVelo;

	// Token: 0x0400142F RID: 5167
	private float wheelRoadVeloLat;

	// Token: 0x04001430 RID: 5168
	private Vector3 wheelVelo;

	// Token: 0x04001431 RID: 5169
	private Vector3 lastWheelVelo;

	// Token: 0x04001432 RID: 5170
	private Vector3 acceleration;

	// Token: 0x04001433 RID: 5171
	private Vector3 localVelo;

	// Token: 0x04001434 RID: 5172
	private Vector3 groundNormal;

	// Token: 0x04001435 RID: 5173
	private Vector3 groundNormalLat;

	// Token: 0x04001436 RID: 5174
	private float rotation;

	// Token: 0x04001437 RID: 5175
	private float J;

	// Token: 0x04001438 RID: 5176
	public float normalForce;

	// Token: 0x04001439 RID: 5177
	private float normalForceLimit;

	// Token: 0x0400143A RID: 5178
	private float normalSuspensionForce;

	// Token: 0x0400143B RID: 5179
	public float normalTireForce;

	// Token: 0x0400143C RID: 5180
	private float normalTireForceLat;

	// Token: 0x0400143D RID: 5181
	private float springForce;

	// Token: 0x0400143E RID: 5182
	private float normalVelocity;

	// Token: 0x0400143F RID: 5183
	private float camber_sin;

	// Token: 0x04001440 RID: 5184
	private Vector3 suspensionForce;

	// Token: 0x04001441 RID: 5185
	private Vector3 bumpStopForce;

	// Token: 0x04001442 RID: 5186
	private Vector3 roadForce;

	// Token: 0x04001443 RID: 5187
	private Vector3 up;

	// Token: 0x04001444 RID: 5188
	private Vector3 right;

	// Token: 0x04001445 RID: 5189
	private Vector3 forwardNormal;

	// Token: 0x04001446 RID: 5190
	private Vector3 rightNormal;

	// Token: 0x04001447 RID: 5191
	private Quaternion localRotation = Quaternion.identity;

	// Token: 0x04001448 RID: 5192
	private Quaternion quatz;

	// Token: 0x04001449 RID: 5193
	private int lastSkid = -1;

	// Token: 0x0400144A RID: 5194
	private Rigidbody body;

	// Token: 0x0400144B RID: 5195
	private Transform trs;

	// Token: 0x0400144C RID: 5196
	private Transform modelTransform;

	// Token: 0x0400144D RID: 5197
	private Transform tireModelTransform;

	// Token: 0x0400144E RID: 5198
	private float maxSlip;

	// Token: 0x0400144F RID: 5199
	private float maxAngle;

	// Token: 0x04001450 RID: 5200
	private float oldAngle;

	// Token: 0x04001451 RID: 5201
	private float newAngle;

	// Token: 0x04001452 RID: 5202
	private Skidmarks skidmarks;

	// Token: 0x04001453 RID: 5203
	private ParticleEmitter skidSmoke;

	// Token: 0x04001454 RID: 5204
	private Rigidbody rigidbodyLat;

	// Token: 0x04001455 RID: 5205
	private int sign = 1;

	// Token: 0x04001456 RID: 5206
	private float distance;
}
