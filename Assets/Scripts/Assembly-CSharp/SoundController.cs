using System;
using UnityEngine;

// Token: 0x0200024E RID: 590
public class SoundController : MonoBehaviour
{
	// Token: 0x06000B25 RID: 2853 RVA: 0x000888CC File Offset: 0x00086ACC
	public void SetDebug(bool db)
	{
		this.debug = db;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x000888D8 File Offset: 0x00086AD8
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

	// Token: 0x06000B27 RID: 2855 RVA: 0x00088994 File Offset: 0x00086B94
	private void Start()
	{
		this.carcontroller = base.GetComponent<CarController>();
		this.cardynamics = base.GetComponent<CarDynamics>();
		this.drivetrain = base.GetComponent<Drivetrain>();
		this.physicMaterials = base.GetComponent<PhysicMaterials>();
		this.engineThrottleSource = this.CreateAudioSource(this.engineThrottle, true, true, Vector3.zero);
		this.engineNoThrottleSource = this.CreateAudioSource(this.engineNoThrottle, true, true, Vector3.zero);
		this.transmissionSource = this.CreateAudioSource(this.transmission, true, true, Vector3.zero);
		this.hornSource = this.CreateAudioSource(this.hornSound, true, false, Vector3.zero);
		Array.Resize<AudioSource>(ref this.skidSource, this.cardynamics.allWheels.Length);
		this.i = 0;
		foreach (Wheel wheel in this.cardynamics.allWheels)
		{
			this.skidSource[this.i] = this.CreateAudioSource(this.skid, true, true, wheel.transform.localPosition);
			this.i++;
		}
		this.crashHiSpeedSource = this.CreateAudioSource(this.crashHiSpeed, false, false, Vector3.zero);
		this.crashLowSpeedSource = this.CreateAudioSource(this.crashLowSpeed, false, false, Vector3.zero);
		this.scrapeNoiseSource = this.CreateAudioSource(this.scrapeNoise, false, false, Vector3.zero);
		this.ABSTriggerSource = this.CreateAudioSource(this.ABSTrigger, false, false, Vector3.zero);
		this.shiftTriggerSource = this.CreateAudioSource(this.shiftTrigger, false, false, Vector3.zero);
		this.shiftTriggerSource.volume = this.shiftTriggerVolume;
		this.windSource = this.CreateAudioSource(this.wind, true, true, Vector3.zero);
		if (this.physicMaterials)
		{
			Array.Resize<AudioSource>(ref this.rollingNoiseSource, this.cardynamics.allWheels.Length);
			this.i = 0;
			foreach (Wheel wheel2 in this.cardynamics.allWheels)
			{
				this.rollingNoiseSource[this.i] = this.CreateAudioSource(this.rollingNoiseGrass, true, false, wheel2.transform.localPosition);
				this.i++;
			}
		}
		this.ABSTriggerSource.volume = 0.5f;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00088BF4 File Offset: 0x00086DF4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			this.hornSource.volume = 1f;
			this.hornSource.Play();
		}
		if (Input.GetKeyUp(KeyCode.JoystickButton0))
		{
			this.hornSource.Stop();
		}
		if (this.drivetrain)
		{
			if (this.drivetrain.throttle <= this.drivetrain.idlethrottle + 0.001f)
			{
				this.engineNoThrottleSource.volume = this.engineNoThrottleVolume + this.drivetrain.rpm / this.drivetrain.maxRPM * Mathf.Max(this.drivetrain.throttle, 0.5f);
				this.engineNoThrottleSource.pitch = 0.5f + this.engineNoThrottlePitchFactor * this.drivetrain.rpm / this.drivetrain.maxRPM;
				this.engineThrottleSource.volume = this.engineThrottleVolume - 0.4f + this.drivetrain.rpm / this.drivetrain.maxRPM * Mathf.Max(this.drivetrain.throttle, 0.7f);
				this.engineThrottleSource.pitch = 0.5f + this.engineThrottlePitchFactor * this.drivetrain.rpm / this.drivetrain.maxRPM;
			}
			else
			{
				this.engineNoThrottleSource.volume = 0f;
				this.engineThrottleSource.volume = this.engineThrottleVolume + this.drivetrain.rpm / this.drivetrain.maxRPM * Mathf.Max(this.drivetrain.throttle, 0.7f);
				this.engineThrottleSource.pitch = 0.5f + this.engineThrottlePitchFactor * this.drivetrain.rpm / this.drivetrain.maxRPM;
			}
			if (this.drivetrain.clutchPosition != 0f)
			{
				this.transmissionSource.volume = this.transmissionVolume + this.drivetrain.rpm / this.drivetrain.maxRPM * Mathf.Max(this.drivetrain.throttle, 0.7f);
				this.transmissionSource.pitch = 0.5f + this.transmissionPitchFactor * this.drivetrain.rpm / this.drivetrain.maxRPM;
			}
			else
			{
				this.transmissionSource.volume = 0f;
			}
			if (this.drivetrain.shiftTriggered)
			{
				this.shiftTriggerSource.PlayOneShot(this.shiftTrigger);
				this.drivetrain.shiftTriggered = false;
			}
		}
		if (this.carcontroller && this.carcontroller.ABSTriggered)
		{
			this.ABSTriggerSource.PlayOneShot(this.ABSTrigger);
		}
		this.windSource.volume = Mathf.Clamp01(Mathf.Abs(this.cardynamics.velo * 3.6f / 600f));
		this.k = 0;
		foreach (Wheel wheel in this.cardynamics.allWheels)
		{
			this.skidSource[this.k].pitch = this.skidPitchFactor;
			this.skidSource[this.k].volume = Mathf.Clamp(Mathf.Abs(wheel.slipVelo) * 0.00875f, 0f, this.skidVolume);
			if (this.skidSource[this.k].volume <= 0.01f)
			{
				this.skidSource[this.k].volume = 0f;
			}
			if (this.physicMaterials)
			{
				if (wheel.physicMaterial == this.physicMaterials.track)
				{
					this.rollingNoiseSource[this.k].volume = 0f;
				}
				else if (wheel.physicMaterial == this.physicMaterials.grass)
				{
					this.rollingNoiseSource[this.k].clip = this.rollingNoiseGrass;
					this.rollingNoiseSource[this.k].volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) / 120f + Mathf.Abs(wheel.slipVelo) * 0.035f);
					if (!this.rollingNoiseSource[this.k].isPlaying)
					{
						this.rollingNoiseSource[this.k].Play();
					}
					if (this.rollingNoiseSource[this.k].volume <= 0.01f || !wheel.onGroundDown)
					{
						this.rollingNoiseSource[this.k].volume = 0f;
					}
					this.skidSource[this.k].volume = 0f;
				}
				else if (wheel.physicMaterial == this.physicMaterials.sand)
				{
					this.rollingNoiseSource[this.k].clip = this.rollingNoiseSand;
					this.rollingNoiseSource[this.k].volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) / 120f + Mathf.Abs(wheel.slipVelo) * 0.035f);
					if (!this.rollingNoiseSource[this.k].isPlaying)
					{
						this.rollingNoiseSource[this.k].Play();
					}
					if (this.rollingNoiseSource[this.k].volume <= 0.01f || !wheel.onGroundDown)
					{
						this.rollingNoiseSource[this.k].volume = 0f;
					}
					this.skidSource[this.k].volume = 0f;
				}
				else if (wheel.physicMaterial == this.physicMaterials.offRoad)
				{
					this.rollingNoiseSource[this.k].clip = this.rollingNoiseOffroad;
					this.rollingNoiseSource[this.k].volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) / 120f + Mathf.Abs(wheel.slipVelo) * 0.035f);
					if (!this.rollingNoiseSource[this.k].isPlaying)
					{
						this.rollingNoiseSource[this.k].Play();
					}
					if (this.rollingNoiseSource[this.k].volume <= 0.01f || !wheel.onGroundDown)
					{
						this.rollingNoiseSource[this.k].volume = 0f;
					}
					this.skidSource[this.k].volume = 0f;
				}
			}
			this.k++;
		}
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x000892C8 File Offset: 0x000874C8
	private void OnCollisionEnter(Collision collInfo)
	{
		if (collInfo.contacts.Length > 0 && collInfo.contacts[0].thisCollider.gameObject.layer != LayerMask.NameToLayer("Wheel"))
		{
			this.volumeFactor = Mathf.Clamp01(collInfo.relativeVelocity.magnitude * 0.08f);
			this.volumeFactor *= Mathf.Clamp01(0.3f + Mathf.Abs(Vector3.Dot(collInfo.relativeVelocity.normalized, collInfo.contacts[0].normal)));
			this.volumeFactor = this.volumeFactor * 0.5f + 0.5f;
			if (this.volumeFactor > 0.9f && !this.crashHiSpeedSource.isPlaying)
			{
				this.crashHiSpeedSource.volume = Mathf.Clamp01(this.volumeFactor * this.crashHighVolume);
				this.crashHiSpeedSource.Play();
			}
			if (!this.crashLowSpeedSource.isPlaying)
			{
				this.crashLowSpeedSource.volume = Mathf.Clamp01(this.volumeFactor * this.crashLowVolume);
				this.crashLowSpeedSource.Play();
			}
			if (!this.scrapeNoiseSource.isPlaying)
			{
				float magnitude = collInfo.relativeVelocity.magnitude;
				float num = Vector3.Angle(collInfo.contacts[0].normal, collInfo.relativeVelocity);
				float num2 = Mathf.Abs(Mathf.Sin(num * 0.017453292f));
				this.scrapeNoiseSource.volume = Mathf.Clamp01(magnitude * num2 * this.scrapeNoiseVolume);
				this.scrapeNoiseSource.loop = false;
				this.scrapeNoiseSource.Play();
			}
		}
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x00089488 File Offset: 0x00087688
	private void OnCollisionExit()
	{
		this.scrapeNoiseSource.volume = 0f;
		this.scrapeNoiseSource.loop = false;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x000894A8 File Offset: 0x000876A8
	private void OnCollisionStay(Collision collInfo)
	{
		float num = collInfo.relativeVelocity.magnitude / 10f;
		float num2 = 0f;
		if (collInfo.contacts.Length > 0)
		{
			num2 = Vector3.Angle(collInfo.contacts[0].normal, collInfo.relativeVelocity);
		}
		float num3 = Mathf.Abs(Mathf.Sin(num2 * 0.017453292f));
		this.scrapeNoiseSource.volume = Mathf.Clamp01(num * num3 * this.scrapeNoiseVolume);
		this.scrapeNoiseSource.loop = true;
		if (!this.scrapeNoiseSource.isPlaying)
		{
			this.scrapeNoiseSource.Play();
		}
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00089550 File Offset: 0x00087750
	private void OnGUI()
	{
		if (this.debug)
		{
			GUI.Label(new Rect(0f, 500f, 400f, 200f), "crashLowSpeedSource.volume: " + this.crashLowSpeedSource.volume);
			GUI.Label(new Rect(0f, 520f, 200f, 200f), "engineThrottleSource.volume : " + this.engineThrottleSource.volume);
		}
	}

	// Token: 0x04001364 RID: 4964
	private bool debug;

	// Token: 0x04001365 RID: 4965
	public AudioClip engineThrottle;

	// Token: 0x04001366 RID: 4966
	public float engineThrottleVolume = 0.35f;

	// Token: 0x04001367 RID: 4967
	public float engineThrottlePitchFactor = 1f;

	// Token: 0x04001368 RID: 4968
	public AudioClip engineNoThrottle;

	// Token: 0x04001369 RID: 4969
	public float engineNoThrottleVolume = 0.35f;

	// Token: 0x0400136A RID: 4970
	public float engineNoThrottlePitchFactor = 1f;

	// Token: 0x0400136B RID: 4971
	public AudioClip transmission;

	// Token: 0x0400136C RID: 4972
	public float transmissionVolume = 0.5f;

	// Token: 0x0400136D RID: 4973
	public float transmissionPitchFactor = 1f;

	// Token: 0x0400136E RID: 4974
	public AudioClip skid;

	// Token: 0x0400136F RID: 4975
	public float skidVolume = 1f;

	// Token: 0x04001370 RID: 4976
	public float skidPitchFactor = 1f;

	// Token: 0x04001371 RID: 4977
	public AudioClip crashHiSpeed;

	// Token: 0x04001372 RID: 4978
	public float crashHighVolume = 0.75f;

	// Token: 0x04001373 RID: 4979
	public AudioClip crashLowSpeed;

	// Token: 0x04001374 RID: 4980
	public float crashLowVolume = 0.7f;

	// Token: 0x04001375 RID: 4981
	public AudioClip scrapeNoise;

	// Token: 0x04001376 RID: 4982
	public float scrapeNoiseVolume = 1f;

	// Token: 0x04001377 RID: 4983
	public AudioClip ABSTrigger;

	// Token: 0x04001378 RID: 4984
	public AudioClip shiftTrigger;

	// Token: 0x04001379 RID: 4985
	public float shiftTriggerVolume = 1f;

	// Token: 0x0400137A RID: 4986
	public AudioClip wind;

	// Token: 0x0400137B RID: 4987
	public AudioClip rollingNoiseGrass;

	// Token: 0x0400137C RID: 4988
	public AudioClip rollingNoiseSand;

	// Token: 0x0400137D RID: 4989
	public AudioClip rollingNoiseOffroad;

	// Token: 0x0400137E RID: 4990
	private AudioSource engineThrottleSource;

	// Token: 0x0400137F RID: 4991
	private AudioSource engineNoThrottleSource;

	// Token: 0x04001380 RID: 4992
	private AudioSource transmissionSource;

	// Token: 0x04001381 RID: 4993
	private AudioSource[] skidSource;

	// Token: 0x04001382 RID: 4994
	private AudioSource crashHiSpeedSource;

	// Token: 0x04001383 RID: 4995
	private AudioSource crashLowSpeedSource;

	// Token: 0x04001384 RID: 4996
	private AudioSource scrapeNoiseSource;

	// Token: 0x04001385 RID: 4997
	private AudioSource ABSTriggerSource;

	// Token: 0x04001386 RID: 4998
	private AudioSource shiftTriggerSource;

	// Token: 0x04001387 RID: 4999
	private AudioSource windSource;

	// Token: 0x04001388 RID: 5000
	private AudioSource[] rollingNoiseSource;

	// Token: 0x04001389 RID: 5001
	private CarController carcontroller;

	// Token: 0x0400138A RID: 5002
	private CarDynamics cardynamics;

	// Token: 0x0400138B RID: 5003
	private Drivetrain drivetrain;

	// Token: 0x0400138C RID: 5004
	private PhysicMaterials physicMaterials;

	// Token: 0x0400138D RID: 5005
	private float volumeFactor;

	// Token: 0x0400138E RID: 5006
	private int i;

	// Token: 0x0400138F RID: 5007
	private int k;

	// Token: 0x04001390 RID: 5008
	public AudioClip hornSound;

	// Token: 0x04001391 RID: 5009
	private AudioSource hornSource;
}
