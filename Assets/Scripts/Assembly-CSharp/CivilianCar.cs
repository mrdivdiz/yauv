using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class CivilianCar : MonoBehaviour
{
	// Token: 0x06000656 RID: 1622 RVA: 0x0002FDE4 File Offset: 0x0002DFE4
	private void Start()
	{
		this.playerTransform = GameObject.FindGameObjectWithTag("PlayerCar").transform;
		base.GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
		base.GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
		base.GetComponent<Rigidbody>().inertiaTensor = new Vector3(1f, 1f, 2f) * base.GetComponent<Rigidbody>().mass;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0002FE58 File Offset: 0x0002E058
	private void Update()
	{
		if (Vector3.Distance(base.transform.position, this.playerTransform.position) > 100f)
		{
			if (!this.stopped)
			{
				this.FLWheel.brakeTorque = 70f;
				this.FRWheel.brakeTorque = 70f;
				this.stopped = true;
			}
			return;
		}
		if (this.stopped)
		{
			this.FLWheel.brakeTorque = 0f;
			this.FRWheel.brakeTorque = 0f;
			this.stopped = false;
		}
		if (this.stopForAccident)
		{
			if (this.accidentTimer > 0f)
			{
				this.FLWheel.brakeTorque = 70f;
				this.FRWheel.brakeTorque = 70f;
				this.accidentTimer -= Time.deltaTime;
			}
			else
			{
				this.FLWheel.brakeTorque = 0f;
				this.FRWheel.brakeTorque = 0f;
			}
			this.FLWheelTransform.Rotate(this.FLWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
			this.FRWheelTransform.Rotate(this.FRWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
			this.RLWheelTransform.Rotate(this.RLWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
			this.RRWheelTransform.Rotate(this.RRWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
			return;
		}
		Vector3 position = base.transform.position;
		if (Physics.Raycast(position, base.transform.forward, 5f))
		{
			base.GetComponent<Rigidbody>().drag = 4f;
			return;
		}
		base.GetComponent<Rigidbody>().drag = 0f;
		if (Vector3.Distance(position, this.path[this.currentPoint].position) < 6f)
		{
			if (this.currentPoint < this.path.Length - 1)
			{
				this.currentPoint++;
			}
			else
			{
				this.currentPoint = 0;
			}
			base.GetComponent<Rigidbody>().drag = 0f;
		}
		else if (Vector3.Distance(position, this.path[this.currentPoint].position) < 15f)
		{
			if (base.GetComponent<Rigidbody>().velocity.magnitude > 5f)
			{
				base.GetComponent<Rigidbody>().drag = 4f;
			}
			else
			{
				base.GetComponent<Rigidbody>().drag = 0f;
			}
		}
		else
		{
			base.GetComponent<Rigidbody>().drag = 0f;
		}
		Vector3 position2 = this.path[this.currentPoint].position;
		position2.y = position.y;
		float num = Vector3.Angle(base.transform.forward, position2 - position);
		num *= Mathf.Clamp01(100f / (2f * this.FLWheel.rpm));
		if (Vector3.Cross(base.transform.forward, this.path[this.currentPoint].position - position).y > 0f)
		{
			num *= -1f;
		}
		this.FLWheel.steerAngle = Mathf.Clamp(-num, -45f, 45f);
		this.FRWheel.steerAngle = Mathf.Clamp(-num, -45f, 45f);
		this.FLWheelTransform.localEulerAngles = new Vector3(this.FLWheelTransform.localEulerAngles.x, this.FLWheel.steerAngle, 0f);
		this.FRWheelTransform.localEulerAngles = new Vector3(this.FRWheelTransform.localEulerAngles.x, this.FRWheel.steerAngle, 0f);
		this.FLWheelTransform.Rotate(this.FLWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		this.FRWheelTransform.Rotate(this.FRWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		this.RLWheelTransform.Rotate(this.RLWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		this.RRWheelTransform.Rotate(this.RRWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		base.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, 3200f);
		float sqrMagnitude = base.GetComponent<Rigidbody>().velocity.sqrMagnitude;
		if ((this.FLWheel.steerAngle > 5f || this.FLWheel.steerAngle < -5f) && sqrMagnitude > 100f)
		{
			base.GetComponent<Rigidbody>().drag = 4f;
		}
		else if ((this.FLWheel.steerAngle > 10f || this.FLWheel.steerAngle < -10f) && sqrMagnitude > 25f)
		{
			base.GetComponent<Rigidbody>().drag = 4f;
		}
		else if ((this.FLWheel.steerAngle > 15f || this.FLWheel.steerAngle < -15f) && sqrMagnitude > 9f)
		{
			base.GetComponent<Rigidbody>().drag = 4f;
		}
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00030430 File Offset: 0x0002E630
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "PlayerCar")
		{
			this.stopForAccident = true;
			if (this.hornSound != null)
			{
				this.CreateAudioSource(this.hornSound, false, true, Vector3.zero);
			}
		}
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00030484 File Offset: 0x0002E684
	private AudioSource CreateAudioSource(AudioClip clip, bool loop, bool playImmediately, Vector3 position)
	{
		GameObject gameObject = new GameObject("audio");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = position;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.AddComponent(typeof(AudioSource));
		gameObject.GetComponent<AudioSource>().clip = clip;
		gameObject.GetComponent<AudioSource>().playOnAwake = false;
		if (loop)
		{
			gameObject.GetComponent<AudioSource>().volume = 0f;
			gameObject.GetComponent<AudioSource>().loop = true;
		}
		else
		{
			gameObject.GetComponent<AudioSource>().loop = false;
		}
		if (playImmediately)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
		return gameObject.GetComponent<AudioSource>();
	}

	// Token: 0x0400077B RID: 1915
	public WheelCollider RLWheel;

	// Token: 0x0400077C RID: 1916
	public WheelCollider RRWheel;

	// Token: 0x0400077D RID: 1917
	public WheelCollider FLWheel;

	// Token: 0x0400077E RID: 1918
	public WheelCollider FRWheel;

	// Token: 0x0400077F RID: 1919
	public Transform RLWheelTransform;

	// Token: 0x04000780 RID: 1920
	public Transform RRWheelTransform;

	// Token: 0x04000781 RID: 1921
	public Transform FLWheelTransform;

	// Token: 0x04000782 RID: 1922
	public Transform FRWheelTransform;

	// Token: 0x04000783 RID: 1923
	public Transform[] path;

	// Token: 0x04000784 RID: 1924
	public int currentPoint;

	// Token: 0x04000785 RID: 1925
	private bool stopForAccident;

	// Token: 0x04000786 RID: 1926
	private float accidentTimer = 2f;

	// Token: 0x04000787 RID: 1927
	private Transform playerTransform;

	// Token: 0x04000788 RID: 1928
	private bool stopped;

	// Token: 0x04000789 RID: 1929
	public AudioClip hornSound;
}
