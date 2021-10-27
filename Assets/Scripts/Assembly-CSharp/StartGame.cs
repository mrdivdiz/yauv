using System;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class StartGame : MonoBehaviour
{
	// Token: 0x06000B2E RID: 2862 RVA: 0x000895EC File Offset: 0x000877EC
	private void Awake()
	{
		this.carcameras = Camera.main.GetComponent<CarCameras>();
		//if (GameObject.FindWithTag("MapCamera"))
		//{
			//this.mapCamera = GameObject.FindWithTag("MapCamera").GetComponent<CarCameras>();
		//}
		//else if (GameObject.Find("MapCamera"))
		//{
			//this.mapCamera = GameObject.Find("MapCamera").GetComponent<CarCameras>();
		//}
		Time.fixedDeltaTime = this.fixedTimeStep;
		if (this.cars.Length == 0)
		{
			this.cars = GameObject.FindGameObjectsWithTag("Car");
		}
		foreach (GameObject gameObject in this.cars)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				if (gameObject.transform.GetComponent<CarDynamics>().skidmarks == null && this.skidmarks)
				{
					Skidmarks skidmarks = UnityEngine.Object.Instantiate(this.skidmarks, Vector3.zero, Quaternion.identity) as Skidmarks;
					gameObject.transform.GetComponent<CarDynamics>().skidmarks = skidmarks;
				}
			}
		}
		if (GameObject.FindWithTag("Trailer"))
		{
			GameObject.FindWithTag("Trailer").SetActive(true);
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00089738 File Offset: 0x00087938
	private void Start()
	{
		foreach (GameObject gameObject in this.cars)
		{
			if (gameObject != null)
			{
				this.DisableObject(gameObject);
			}
		}
		if (this.cars.Length != 0 && this.cars[0] != null)
		{
			this.selectedCar = this.ChangeCar(0, -1);
		}
		if (GameObject.Find("Monza Track"))
		{
			GameObject gameObject2 = GameObject.Find("Catamount UM001");
			GameObject gameObject3 = GameObject.Find("Ferrari");
			if (gameObject2)
			{
				gameObject2.GetComponent<CarDynamics>().LoadSetUp("catamount_track");
			}
			if (gameObject3)
			{
				gameObject3.GetComponent<CarDynamics>().LoadSetUp("ferrari_track");
			}
		}
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00089808 File Offset: 0x00087A08
	private void DisableObjects(GameObject selectedCar)
	{
		foreach (GameObject gameObject in this.cars)
		{
			if (gameObject != selectedCar)
			{
				this.DisableObject(gameObject);
			}
		}
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00089848 File Offset: 0x00087A48
	private void DisableObject(GameObject car)
	{
		car.transform.GetComponent<CarDynamics>().showHelpGUI = false;
		car.transform.GetComponent<CarDynamics>().showSlidersGUI = false;
		car.transform.GetComponent<CarDynamics>().SetController("None");
		foreach (object obj in car.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.tag == "DashBoard" || transform.gameObject.name == "DashBoard")
			{
				transform.gameObject.active = false;
			}
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x00089928 File Offset: 0x00087B28
	private void EnableObject(GameObject car)
	{
		car.transform.GetComponent<CarDynamics>().showHelpGUI = false;
		car.transform.GetComponent<CarDynamics>().showSlidersGUI = false;
		if (AndroidPlatform.IsJoystickConnected())
		{
			car.transform.GetComponent<CarDynamics>().SetController("Axis");
		}
		else
		{
			car.transform.GetComponent<CarDynamics>().SetController("Mobile");
		}
		foreach (object obj in car.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.tag == "DashBoard" || transform.gameObject.name == "DashBoard")
			{
				transform.gameObject.active = true;
			}
		}
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00089A2C File Offset: 0x00087C2C
	private void Update()
	{
		if (this.cars.Length > 1)
		{
			if (Input.GetKeyUp(KeyCode.PageUp))
			{
				this.lastIndex = this.index;
				this.index++;
				if (this.index > this.cars.Length - 1)
				{
					this.index = 0;
				}
				this.selectedCar = this.ChangeCar(this.index, this.lastIndex);
			}
			else if (Input.GetKeyUp(KeyCode.PageDown))
			{
				this.lastIndex = this.index;
				this.index--;
				if (this.index < 0)
				{
					this.index = this.cars.Length - 1;
				}
				this.selectedCar = this.ChangeCar(this.index, this.lastIndex);
			}
		}
		if (Input.GetKeyUp(KeyCode.M) && this.mapCamera != null)
		{
			//this.mapCamera.gameObject.active = !this.mapCamera.gameObject.active;
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00089B48 File Offset: 0x00087D48
	private GameObject ChangeCar(int index, int lastIndex)
	{
		if (lastIndex != -1)
		{
			this.DisableObject(this.cars[lastIndex]);
		}
		this.selectedCar = this.cars[index];
		this.EnableObject(this.selectedCar);
		this.carcameras.target = this.selectedCar.transform;
		this.carcameras.SetCamera(0);
		//if (this.mapCamera)
		//{
		//	this.mapCamera.target = this.selectedCar.transform;
		//	this.mapCamera.SetCamera(7);
		//}
		return this.selectedCar;
	}

	// Token: 0x04001392 RID: 5010
	private CarCameras carcameras;

	// Token: 0x04001393 RID: 5011
	private GameObject StartTimer;

	// Token: 0x04001394 RID: 5012
	private CarCameras mapCamera;

	// Token: 0x04001395 RID: 5013
	public GameObject[] cars;

	// Token: 0x04001396 RID: 5014
	private GameObject selectedCar;

	// Token: 0x04001397 RID: 5015
	private bool arcadeMode;

	// Token: 0x04001398 RID: 5016
	private bool altNormalForce;

	// Token: 0x04001399 RID: 5017
	public Skidmarks skidmarks;

	// Token: 0x0400139A RID: 5018
	public float fixedTimeStep = 0.02f;

	// Token: 0x0400139B RID: 5019
	private int index;

	// Token: 0x0400139C RID: 5020
	private int lastIndex;
}
