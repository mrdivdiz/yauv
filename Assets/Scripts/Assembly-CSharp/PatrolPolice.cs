using System;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class PatrolPolice : MonoBehaviour
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x000284F8 File Offset: 0x000266F8
	private void Awake()
	{
		PatrolPolice.isAware = false;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00028500 File Offset: 0x00026700
	private void Start()
	{
		this.visualMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.visualMask |= 1 << LayerMask.NameToLayer("Enemy");
		this.visualMask = ~this.visualMask;
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00028550 File Offset: 0x00026750
	private void Update()
	{
		if (PatrolPolice.isAware)
		{
			this.RootMotion();
		}
		else
		{
			this.IsAware();
		}
		if (!this.faded && this.startTime != 0f)
		{
			Camera.main.GetComponent<CameraFade>().StartFade(Color.black, 5f);
			this.faded = true;
		}
		if (this.faded && Time.time > this.startTime + 5f)
		{
			Application.LoadLevel("PreLoading" + Application.loadedLevelName);
		}
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x000285E8 File Offset: 0x000267E8
	private void IsAware()
	{
		if (this.target == null)
		{
			this.RootMotion();
			PatrolPolice.isAware = true;
			return;
		}
		this.lastTargetPosition = this.target.transform.position;
		Vector3 position = this.target.transform.position;
		position.y = base.transform.position.y + 1.8f;
		Vector3 position2 = this.headLookPosition.position;
		RaycastHit raycastHit;
		if ((!Physics.Linecast(position2, position, out raycastHit, this.visualMask) || raycastHit.collider.gameObject.layer == this.target.layer) && Vector3.Distance(position2, position) < this.viewConeLength && Mathf.Abs(Vector3.Angle(this.headLookPosition.forward, (position - position2).normalized)) <= this.viewCone / 2f && this.target.transform.position.y <= base.transform.position.y + 2f)
		{
			AnimationHandler.instance.stealthMode = false;
			if (!PatrolPolice.isAware)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.enemyJustSawSounds[UnityEngine.Random.Range(0, this.enemyJustSawSounds.Length)], SpeechManager.speechVolume);
				this.startTime = Time.time;
				this.RootMotion();
				PatrolPolice.isAware = true;
			}
			return;
		}
		if (Vector3.Distance(position2, position) < 1f)
		{
			AnimationHandler.instance.stealthMode = false;
			if (!PatrolPolice.isAware)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.enemyJustSawSounds[UnityEngine.Random.Range(0, this.enemyJustSawSounds.Length)], SpeechManager.speechVolume);
				this.startTime = Time.time;
				this.RootMotion();
				PatrolPolice.isAware = true;
			}
			return;
		}
		if (Debug.isDebugBuild)
		{
			float num = 0.017453292f * (this.viewCone / 2f);
			Vector3 vector = new Vector3(0f, 0f, 0f);
			vector.z += this.viewConeLength * Mathf.Cos(num);
			vector.x += this.viewConeLength * Mathf.Sin(num);
			Vector3 vector2 = new Vector3(0f, 0f, 0f);
			vector2.z += this.viewConeLength * Mathf.Cos(num);
			vector2.x -= this.viewConeLength * Mathf.Sin(num);
			vector = this.headLookPosition.TransformDirection(vector);
			vector2 = this.headLookPosition.TransformDirection(vector2);
			Vector3 b = this.headLookPosition.TransformDirection(0f, 0f, this.viewConeLength);
			Debug.DrawLine(position2, position2 + vector, Color.magenta);
			Debug.DrawLine(position2, position2 + vector2, Color.magenta);
			Vector3 vector3 = new Vector3(0f, 0f, 0f);
			vector3.z += this.viewConeLength * Mathf.Cos(num / 2f);
			vector3.x += this.viewConeLength * Mathf.Sin(num / 2f);
			Vector3 vector4 = new Vector3(0f, 0f, 0f);
			vector4.z += this.viewConeLength * Mathf.Cos(num / 2f);
			vector4.x -= this.viewConeLength * Mathf.Sin(num / 2f);
			vector3 = this.headLookPosition.TransformDirection(vector3);
			vector4 = this.headLookPosition.TransformDirection(vector4);
			Debug.DrawLine(position2 + vector, position2 + vector3, Color.magenta);
			Debug.DrawLine(position2 + vector3, position2 + b, Color.magenta);
			Debug.DrawLine(position2 + b, position2 + vector4, Color.magenta);
			Debug.DrawLine(position2 + vector4, position2 + vector2, Color.magenta);
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00028A20 File Offset: 0x00026C20
	private void RootMotion()
	{
		if (!this.rooted)
		{
			Vector3 position = base.transform.Find("Bip01/Root").transform.position;
			base.GetComponent<Animation>().Stop();
			base.GetComponent<Animation>()["Shooting-Handgun2-Draw"].speed = 2f;
			base.GetComponent<Animation>().Play("Shooting-Handgun2-Draw");
			base.transform.position = position;
			base.GetComponent<Animation>().CrossFadeQueued("Shooting-Handgun2-Idle-Aimed", 0.2f, QueueMode.CompleteOthers, PlayMode.StopAll);
			this.rooted = true;
		}
		Vector3 worldPosition = Vector3.zero;
		if (this.target != null)
		{
			worldPosition = this.target.transform.position;
		}
		else
		{
			worldPosition = this.lastTargetPosition;
		}
		worldPosition.y = base.transform.position.y;
		base.transform.LookAt(worldPosition);
	}

	// Token: 0x0400066A RID: 1642
	public GameObject target;

	// Token: 0x0400066B RID: 1643
	public Transform headLookPosition;

	// Token: 0x0400066C RID: 1644
	public float viewCone = 90f;

	// Token: 0x0400066D RID: 1645
	public float viewConeLength = 30f;

	// Token: 0x0400066E RID: 1646
	public static bool isAware;

	// Token: 0x0400066F RID: 1647
	private int visualMask;

	// Token: 0x04000670 RID: 1648
	private float startTime;

	// Token: 0x04000671 RID: 1649
	private bool faded;

	// Token: 0x04000672 RID: 1650
	public AudioClip[] enemyJustSawSounds;

	// Token: 0x04000673 RID: 1651
	public bool rooted;

	// Token: 0x04000674 RID: 1652
	private Vector3 lastTargetPosition;
}
