using System;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class HelicopterMovement : MonoBehaviour
{
	// Token: 0x06000B62 RID: 2914 RVA: 0x0008F144 File Offset: 0x0008D344
	private void Start()
	{
		if (this.playerTransform == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this.playerTransform = gameObject.transform;
			}
		}
		this.cc = base.gameObject.GetComponent<CharacterController>();
		if (this.path.Length > 0)
		{
			Vector3 position = this.path[0].transform.position;
			position.y = base.transform.position.y;
			base.transform.position = position;
		}
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0008F1E0 File Offset: 0x0008D3E0
	private void Update()
	{
		if (Time.deltaTime == 0f || Time.timeScale == 0f)
		{
			return;
		}
		if (this.path.Length > 0)
		{
			Vector3 position = this.path[this.currentDestination].transform.position;
			position.y = base.transform.position.y;
			if (Vector3.Distance(base.transform.position, position) < this.arrivalDistance)
			{
				this.reachedPoint = true;
			}
			if (this.reachedPoint)
			{
				if (this.currentDestination == this.startShootingAt)
				{
					this.shootAtPlayer = true;
				}
				if (this.currentDestination == this.stopShootingAt)
				{
					this.shootAtPlayer = false;
				}
				if (this.stopAtLastPoint && this.currentDestination == this.path.Length - 1)
				{
					this.timeBeforeDespawning -= Time.deltaTime;
					if (this.timeBeforeDespawning <= 0f)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					return;
				}
				if (this.pathIsaCircle)
				{
					this.currentDestination = (this.currentDestination + 1) % (this.path.Length - 1);
				}
				else
				{
					if (!this.reversePath && this.currentDestination == this.path.Length - 1)
					{
						this.reversePath = true;
					}
					else if (this.reversePath & this.currentDestination == 0)
					{
						this.reversePath = false;
					}
					if (this.reversePath)
					{
						this.currentDestination--;
					}
					else
					{
						this.currentDestination++;
					}
				}
				this.reachedPoint = false;
			}
			this.MoveToPosition(this.path[this.currentDestination].transform.position);
		}
		if (this.shootAtPlayer && this.playerTransform != null && (this.path.Length != 0 || Time.timeScale == 0.15f))
		{
			this.weapon1.fire = true;
			this.weapon2.fire = true;
		}
		else
		{
			this.weapon1.fire = false;
			this.weapon2.fire = false;
		}
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0008F428 File Offset: 0x0008D628
	private void MoveToPosition(Vector3 target)
	{
		float num = 5f;
		if (this.shootAtPlayer)
		{
			Vector3 forward = this.playerTransform.position - base.transform.position;
			forward.y = 0f;
			forward = forward.normalized;
			Quaternion to = Quaternion.LookRotation(forward);
			float t = Mathf.Min(num * Time.deltaTime, 1f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
		}
		else
		{
			Vector3 forward = target - base.transform.position;
			forward.y = 0f;
			forward = forward.normalized;
			Quaternion to = Quaternion.LookRotation(forward);
			float t = Mathf.Min(num * Time.deltaTime, 1f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
		}
		if (this.shootAtPlayer)
		{
			if (base.transform.InverseTransformPoint(target).x > 0f)
			{
				Quaternion to = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, -10f));
				float t = Mathf.Min(num * Time.deltaTime, 1f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
			}
			else
			{
				Quaternion to = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 10f));
				float t = Mathf.Min(num * Time.deltaTime, 1f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
			}
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 1f));
		}
		if (base.transform.InverseTransformPoint(target).z > 0f)
		{
			Quaternion to = Quaternion.Euler(new Vector3(10f, base.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z));
			float t = Mathf.Min(num * Time.deltaTime, 1f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
		}
		else
		{
			Quaternion to = Quaternion.Euler(new Vector3(-10f, base.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z));
			float t = Mathf.Min(num * Time.deltaTime, 1f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
		}
		base.transform.position = Vector3.Lerp(base.transform.position, target, Time.deltaTime / (Vector3.Distance(target, base.transform.position) / this.speed));
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0008F7D8 File Offset: 0x0008D9D8
	private void OnDrawGizmos()
	{
		if (this.showPathWhileSelected)
		{
			return;
		}
		if (this.path.Length == 0)
		{
			return;
		}
		WayPoint wayPoint = this.path[0];
		foreach (WayPoint wayPoint2 in this.path)
		{
			if (!(wayPoint2 == wayPoint))
			{
				if (wayPoint != null && wayPoint2 != null && Physics.Linecast(wayPoint.transform.position, wayPoint2.transform.position))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				else if (wayPoint != null && wayPoint2 != null)
				{
					if (this.enemyVehicle)
					{
						Gizmos.color = Color.black;
					}
					else
					{
						Gizmos.color = Color.green;
					}
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				wayPoint = wayPoint2;
			}
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0008F8F4 File Offset: 0x0008DAF4
	private void OnDrawGizmosSelected()
	{
		if (!this.showPathWhileSelected)
		{
			return;
		}
		if (this.path.Length == 0)
		{
			return;
		}
		WayPoint wayPoint = this.path[0];
		foreach (WayPoint wayPoint2 in this.path)
		{
			if (!(wayPoint2 == wayPoint))
			{
				if (wayPoint != null && wayPoint2 != null && Physics.Linecast(wayPoint.transform.position, wayPoint2.transform.position))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				else
				{
					if (this.enemyVehicle)
					{
						Gizmos.color = Color.black;
					}
					else
					{
						Gizmos.color = Color.green;
					}
					if (wayPoint != null && wayPoint2 != null)
					{
						Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
					}
				}
				wayPoint = wayPoint2;
			}
		}
	}

	// Token: 0x040014A0 RID: 5280
	public bool enemyVehicle;

	// Token: 0x040014A1 RID: 5281
	public WayPoint[] path;

	// Token: 0x040014A2 RID: 5282
	public bool showPathWhileSelected = true;

	// Token: 0x040014A3 RID: 5283
	public bool pathIsaCircle;

	// Token: 0x040014A4 RID: 5284
	public bool stopAtLastPoint;

	// Token: 0x040014A5 RID: 5285
	public float arrivalDistance = 0.5f;

	// Token: 0x040014A6 RID: 5286
	public float speed = 18f;

	// Token: 0x040014A7 RID: 5287
	public int currentDestination;

	// Token: 0x040014A8 RID: 5288
	private bool reachedPoint;

	// Token: 0x040014A9 RID: 5289
	private bool reversePath;

	// Token: 0x040014AA RID: 5290
	private CharacterController cc;

	// Token: 0x040014AB RID: 5291
	public float maxVelocityChange = 0.2f;

	// Token: 0x040014AC RID: 5292
	private float rayCastTimer;

	// Token: 0x040014AD RID: 5293
	public Transform playerTransform;

	// Token: 0x040014AE RID: 5294
	[HideInInspector]
	public bool turning;

	// Token: 0x040014AF RID: 5295
	private float timeBeforeDespawning = 3f;

	// Token: 0x040014B0 RID: 5296
	public QuadBikeGun weapon1;

	// Token: 0x040014B1 RID: 5297
	public QuadBikeGun weapon2;

	// Token: 0x040014B2 RID: 5298
	public bool shootAtPlayer;

	// Token: 0x040014B3 RID: 5299
	public int startShootingAt = 1;

	// Token: 0x040014B4 RID: 5300
	public int stopShootingAt = 1;
}
