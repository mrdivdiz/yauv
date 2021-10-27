using System;
using UnityEngine;

// Token: 0x02000250 RID: 592
[AddComponentMenu("Physics/UnityCar")]
[ExecuteInEditMode]
public class UnityCar : MonoBehaviour
{
	// Token: 0x06000B36 RID: 2870 RVA: 0x00089BE8 File Offset: 0x00087DE8
	private void Start()
	{
		this.unityCar = base.gameObject.GetComponent<UnityCar>();
		if (base.gameObject.GetComponent<Drivetrain>() == null)
		{
			this.drivetrain = base.gameObject.AddComponent<Drivetrain>();
		}
		else
		{
			this.drivetrain = base.gameObject.GetComponent<Drivetrain>();
		}
		if (base.gameObject.GetComponent<AxisCarController>() == null)
		{
			this.axisCarController = base.gameObject.AddComponent<AxisCarController>();
		}
		else
		{
			this.axisCarController = base.gameObject.GetComponent<AxisCarController>();
		}
		if (base.gameObject.GetComponent<CarDynamics>() == null)
		{
			this.carDynamics = base.gameObject.AddComponent<CarDynamics>();
		}
		else
		{
			this.carDynamics = base.gameObject.GetComponent<CarDynamics>();
		}
		if (base.gameObject.GetComponent<SoundController>() == null)
		{
			this.soundController = base.gameObject.AddComponent<SoundController>();
		}
		else
		{
			this.soundController = base.gameObject.GetComponent<SoundController>();
		}
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			this.mrigidbody = base.gameObject.AddComponent<Rigidbody>();
			this.mrigidbody.mass = 1000f;
			this.mrigidbody.angularDrag = 0f;
			this.mrigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}
		else
		{
			this.mrigidbody = base.gameObject.GetComponent<Rigidbody>();
		}
		if (base.gameObject.GetComponent<Wheels>() == null)
		{
			this.wheels = base.gameObject.AddComponent<Wheels>();
		}
		else
		{
			this.wheels = base.gameObject.GetComponent<Wheels>();
		}
		this.carDynamics.showSlidersGUI = false;
		if (base.transform.Find("Body") == null)
		{
			this.body = new GameObject("Body");
			this.body.transform.parent = base.transform;
			this.body.transform.localPosition = Vector3.zero;
			this.body.transform.localRotation = Quaternion.identity;
			this.meshFilter = this.body.gameObject.AddComponent<MeshFilter>();
			this.meshRenderer = this.body.gameObject.AddComponent<MeshRenderer>();
		}
		else
		{
			this.body = base.transform.Find("Body").gameObject;
		}
		if (base.transform.Find("Collider") == null)
		{
			this.mcollider = new GameObject("Collider");
			this.mcollider.transform.parent = base.transform;
			this.mcollider.transform.localPosition = Vector3.zero;
			this.mcollider.transform.localRotation = Quaternion.identity;
			this.boxCollider = this.mcollider.gameObject.AddComponent<BoxCollider>();
			this.boxCollider.transform.localScale = new Vector3(1.5f, 0.5f, 4f);
		}
		else
		{
			this.mcollider = base.transform.Find("Collider").gameObject;
			this.boxCollider = this.mcollider.gameObject.GetComponent<BoxCollider>();
		}
		if (base.transform.Find("wheelFL") == null)
		{
			this.wheelFL = new GameObject("wheelFL");
			this.wheelFL.transform.parent = base.transform;
			this.wheelFL.transform.localPosition = new Vector3(-this.boxCollider.transform.localScale.x / 2f, 0f, this.boxCollider.transform.localScale.z / 2f - this.boxCollider.transform.localScale.z / 8f);
			this.wheelFL.transform.localRotation = Quaternion.identity;
			this.wheel = this.wheelFL.gameObject.AddComponent<Wheel>();
			this.wheel.showForces = true;
			this.wheels.frontLeftWheel = this.wheel;
			this.model = new GameObject("modelFL");
			this.model.transform.parent = this.wheelFL.transform;
			this.model.transform.localPosition = Vector3.zero;
			this.model.transform.localRotation = Quaternion.identity;
			this.meshFilter = this.model.gameObject.AddComponent<MeshFilter>();
			this.meshRenderer = this.model.gameObject.AddComponent<MeshRenderer>();
			this.wheel.model = this.model;
		}
		else
		{
			this.wheelFL = base.transform.Find("wheelFL").gameObject;
		}
		this.wheelFL.gameObject.layer = LayerMask.NameToLayer("Wheel");
		if (base.transform.Find("wheelFR") == null)
		{
			this.wheelFR = new GameObject("wheelFR");
			this.wheelFR.transform.parent = base.transform;
			this.wheelFR.transform.localPosition = new Vector3(this.boxCollider.transform.localScale.x / 2f, 0f, this.boxCollider.transform.localScale.z / 2f - this.boxCollider.transform.localScale.z / 8f);
			this.wheelFR.transform.localRotation = Quaternion.identity;
			this.wheel = this.wheelFR.gameObject.AddComponent<Wheel>();
			this.wheel.showForces = true;
			this.wheels.frontRightWheel = this.wheel;
			this.model = new GameObject("modelFR");
			this.model.transform.parent = this.wheelFR.transform;
			this.model.transform.localPosition = Vector3.zero;
			this.model.transform.localRotation = Quaternion.identity;
			this.meshFilter = this.model.gameObject.AddComponent<MeshFilter>();
			this.meshRenderer = this.model.gameObject.AddComponent<MeshRenderer>();
			this.wheel.model = this.model;
		}
		else
		{
			this.wheelFR = base.transform.Find("wheelFR").gameObject;
		}
		this.wheelFR.gameObject.layer = LayerMask.NameToLayer("Wheel");
		if (base.transform.Find("wheelRL") == null)
		{
			this.wheelRL = new GameObject("wheelRL");
			this.wheelRL.transform.parent = base.transform;
			this.wheelRL.transform.localPosition = new Vector3(-this.boxCollider.transform.localScale.x / 2f, 0f, -(this.boxCollider.transform.localScale.z / 2f - this.boxCollider.transform.localScale.z / 8f));
			this.wheelRL.transform.localRotation = Quaternion.identity;
			this.wheel = this.wheelRL.gameObject.AddComponent<Wheel>();
			this.wheel.showForces = true;
			this.wheel.maxSteeringAngle = 0f;
			this.wheels.rearLeftWheel = this.wheel;
			this.model = new GameObject("modelRL");
			this.model.transform.parent = this.wheelRL.transform;
			this.model.transform.localPosition = Vector3.zero;
			this.model.transform.localRotation = Quaternion.identity;
			this.meshFilter = this.model.gameObject.AddComponent<MeshFilter>();
			this.meshRenderer = this.model.gameObject.AddComponent<MeshRenderer>();
			this.wheel.model = this.model;
		}
		else
		{
			this.wheelRL = base.transform.Find("wheelRL").gameObject;
		}
		this.wheelRL.gameObject.layer = LayerMask.NameToLayer("Wheel");
		if (base.transform.Find("wheelRR") == null)
		{
			this.wheelRR = new GameObject("wheelRR");
			this.wheelRR.transform.parent = base.transform;
			this.wheelRR.transform.localPosition = new Vector3(this.boxCollider.transform.localScale.x / 2f, 0f, -(this.boxCollider.transform.localScale.z / 2f - this.boxCollider.transform.localScale.z / 8f));
			this.wheelRR.transform.localRotation = Quaternion.identity;
			this.wheel = this.wheelRR.gameObject.AddComponent<Wheel>();
			this.wheel.showForces = true;
			this.wheel.maxSteeringAngle = 0f;
			this.wheels.rearRightWheel = this.wheel;
			this.model = new GameObject("modelRR");
			this.model.transform.parent = this.wheelRR.transform;
			this.model.transform.localPosition = Vector3.zero;
			this.model.transform.localRotation = Quaternion.identity;
			this.meshFilter = this.model.gameObject.AddComponent<MeshFilter>();
			this.meshRenderer = this.model.gameObject.AddComponent<MeshRenderer>();
			this.wheel.model = this.model;
		}
		else
		{
			this.wheelRR = base.transform.Find("wheelRR").gameObject;
		}
		this.wheelRR.gameObject.layer = LayerMask.NameToLayer("Wheel");
		this.drivetrain.enabled = true;
		this.axisCarController.enabled = true;
		this.soundController.enabled = true;
		this.meshFilter.mesh = null;
		this.meshRenderer.enabled = true;
		UnityEngine.Object.DestroyImmediate(this.unityCar);
	}

	// Token: 0x0400139D RID: 5021
	private Drivetrain drivetrain;

	// Token: 0x0400139E RID: 5022
	private AxisCarController axisCarController;

	// Token: 0x0400139F RID: 5023
	private CarDynamics carDynamics;

	// Token: 0x040013A0 RID: 5024
	private SoundController soundController;

	// Token: 0x040013A1 RID: 5025
	private Rigidbody mrigidbody;

	// Token: 0x040013A2 RID: 5026
	private Wheels wheels;

	// Token: 0x040013A3 RID: 5027
	private Wheel wheel;

	// Token: 0x040013A4 RID: 5028
	private GameObject wheelFL;

	// Token: 0x040013A5 RID: 5029
	private GameObject wheelFR;

	// Token: 0x040013A6 RID: 5030
	private GameObject wheelRL;

	// Token: 0x040013A7 RID: 5031
	private GameObject wheelRR;

	// Token: 0x040013A8 RID: 5032
	private GameObject model;

	// Token: 0x040013A9 RID: 5033
	private BoxCollider boxCollider;

	// Token: 0x040013AA RID: 5034
	private GameObject body;

	// Token: 0x040013AB RID: 5035
	private GameObject mcollider;

	// Token: 0x040013AC RID: 5036
	private GameObject dashBoard;

	// Token: 0x040013AD RID: 5037
	private MeshFilter meshFilter;

	// Token: 0x040013AE RID: 5038
	private MeshRenderer meshRenderer;

	// Token: 0x040013AF RID: 5039
	private UnityCar unityCar;
}
