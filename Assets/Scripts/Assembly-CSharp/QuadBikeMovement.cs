using System;
using UnityEngine;

// Token: 0x0200025E RID: 606
public class QuadBikeMovement : MonoBehaviour
{
	// Token: 0x06000B75 RID: 2933 RVA: 0x00090578 File Offset: 0x0008E778
	private void Start()
	{
		this.cc = base.gameObject.GetComponent<CharacterController>();
		if (this.path.Length > 0)
		{
			if (!this.enemyVehicle && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 1) || (!mainmenu.replayLevel && SaveHandler.levelReached == 5 && SaveHandler.checkpointReached == 1)))
			{
				Vector3 position = this.path[37].transform.position;
				base.transform.position = position;
				this.currentDestination = 38;
				this.currentLap = 0;
			}
			else if (!this.enemyVehicle && ((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 2) || (!mainmenu.replayLevel && SaveHandler.levelReached == 5 && SaveHandler.checkpointReached == 2)))
			{
				Vector3 position2 = this.path[51].transform.position;
				base.transform.position = position2;
				this.currentDestination = 52;
				this.currentLap = 0;
			}
			else if (!this.enemyVehicle && this.startingPoint != 0)
			{
				Vector3 position3 = this.path[this.startingPoint].transform.position;
				base.transform.position = position3;
				this.currentDestination = this.startingPoint;
			}
			else
			{
				Vector3 position4 = this.path[0].transform.position;
				position4.y = base.transform.position.y;
				base.transform.position = position4;
			}
		}
		if (this.rotateWhileTurnining)
		{
			this.startingRotation = base.transform.rotation;
		}
		if (this.turningLeftAnim != null)
		{
			this.turningLeftAnim.wrapMode = WrapMode.ClampForever;
		}
		if (this.turningRightAnim != null)
		{
			this.turningRightAnim.wrapMode = WrapMode.ClampForever;
		}
		if (this.driverLeftAnim != null)
		{
			this.driverLeftAnim.wrapMode = WrapMode.ClampForever;
		}
		if (this.driverRightAnim != null)
		{
			this.driverRightAnim.wrapMode = WrapMode.ClampForever;
		}
		if (this.bikeTransform == null)
		{
			this.bikeTransform = base.transform;
		}
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x000907C0 File Offset: 0x0008E9C0
	private void FixedUpdate()
	{
		if (Time.deltaTime == 0f || Time.timeScale == 0f)
		{
			return;
		}
		if (this.daniaTransform == null)
		{
			if (this.driveTimeAfterDriverIsKilled <= 0f)
			{
				base.gameObject.AddComponent<DystroyAfterTime>();
				return;
			}
			this.driveTimeAfterDriverIsKilled -= Time.deltaTime;
			this.speed -= this.speed * Time.fixedDeltaTime / this.driveTimeAfterDriverIsKilled;
			if (this.rotateWhileTurnining)
			{
				float num = 20f;
				Quaternion to = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, -90f));
				float t = Mathf.Min(num * Time.deltaTime, 1f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
			}
		}
		QuadBikeMovement.States states = this.state;
		Vector3 position;
		if (!this.enemyVehicle && this.startedFinalPath)
		{
			position = this.finalPath[this.currentDestination].transform.position;
		}
		else
		{
			position = this.path[this.currentDestination].transform.position;
		}
		position.y = base.transform.position.y;
		if (Vector3.Distance(base.transform.position, position) < this.arrivalDistance)
		{
			this.reachedPoint = true;
		}
		if (this.reachedPoint)
		{
			if (!this.enemyVehicle)
			{
				VehicleSpawner.PlayerReachedWaypoint(this.currentDestination, this.currentLap);
				QBSpeechManager.PlayerReachedWaypoint(this.currentDestination, this.currentLap);
			}
			if (!this.enemyVehicle && this.startedFinalPath)
			{
				if (this.stopAtLastPoint && this.currentDestination == this.finalPath.Length - 1)
				{
					this.timeBeforeDespawning -= Time.deltaTime;
					if (this.timeBeforeDespawning <= 0f)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					return;
				}
				this.currentDestination = (this.currentDestination + 1) % this.finalPath.Length;
			}
			else if (!this.enemyVehicle && this.currentDestination == this.path.Length - 2 && this.currentLap >= this.finalLap)
			{
				this.currentDestination = 0;
				this.currentLap++;
				this.startedFinalPath = true;
			}
			else
			{
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
					if (!this.enemyVehicle && this.currentDestination == this.path.Length - 2)
					{
						this.currentLap++;
					}
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
			}
			this.reachedPoint = false;
		}
		if (!this.enemyVehicle && this.startedFinalPath)
		{
			this.MoveToPosition(this.finalPath[this.currentDestination].transform.position);
		}
		else
		{
			this.MoveToPosition(this.path[this.currentDestination].transform.position);
		}
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00090BE8 File Offset: 0x0008EDE8
	private void MoveToPosition(Vector3 target)
	{
		target.y = base.transform.position.y;
		Vector3 vector = target - base.transform.position;
		vector.y = 0f;
		vector = vector.normalized;
		float num = 5f;
		Quaternion to = Quaternion.LookRotation(vector);
		float t = Mathf.Min(num * Time.deltaTime, 1f);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
		if (Vector3.Angle(base.transform.forward, vector) > 1f)
		{
			if (Vector3.Cross(base.transform.forward, vector).y > 0f)
			{
				if (this.turningRightAnim != null && !this.bikeTransform.GetComponent<Animation>().IsPlaying(this.turningRightAnim.name))
				{
					this.bikeTransform.GetComponent<Animation>().CrossFade(this.turningRightAnim.name);
				}
				if (this.daniaTransform != null && this.driverRightAnim != null && !this.daniaTransform.GetComponent<Animation>().IsPlaying(this.driverRightAnim.name))
				{
					this.daniaTransform.GetComponent<Animation>().CrossFade(this.driverRightAnim.name);
				}
				this.turning = true;
				if (this.rotateWhileTurnining && this.daniaTransform != null)
				{
					to = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, -20f));
					t = Mathf.Min(num * Time.deltaTime, 1f);
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
				}
			}
			else
			{
				if (this.turningLeftAnim != null && !this.bikeTransform.GetComponent<Animation>().IsPlaying(this.turningLeftAnim.name))
				{
					this.bikeTransform.GetComponent<Animation>().CrossFade(this.turningLeftAnim.name);
				}
				if (this.daniaTransform != null && this.driverLeftAnim != null && !this.daniaTransform.GetComponent<Animation>().IsPlaying(this.driverLeftAnim.name))
				{
					this.daniaTransform.GetComponent<Animation>().CrossFade(this.driverLeftAnim.name);
				}
				this.turning = true;
				if (this.rotateWhileTurnining && this.daniaTransform != null)
				{
					to = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 20f));
					t = Mathf.Min(num * Time.deltaTime, 1f);
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
				}
			}
		}
		else
		{
			if (this.movingAnim != null && !this.bikeTransform.GetComponent<Animation>().IsPlaying(this.movingAnim.name))
			{
				this.bikeTransform.GetComponent<Animation>().CrossFade(this.movingAnim.name);
			}
			if (this.daniaTransform != null && this.driverIdleAnim != null && this.driverIdleAnim != null && !this.daniaTransform.GetComponent<Animation>().IsPlaying(this.driverIdleAnim.name))
			{
				this.daniaTransform.GetComponent<Animation>().CrossFade(this.driverIdleAnim.name);
			}
			this.turning = false;
			if (this.rotateWhileTurnining && this.daniaTransform != null)
			{
				base.transform.rotation = Quaternion.Euler(new Vector3(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 1f));
			}
		}
		float d = this.speed;
		Vector3 vector2 = Vector3.zero;
		vector2 = (target - base.transform.position).normalized;
		if (this.smoothingTime > 0f)
		{
			vector2 = Vector3.Slerp(this.startingFacingDirection, vector2, (0.5f - this.smoothingTime) / 0.5f);
			this.smoothingTime -= Time.deltaTime;
		}
		if (this.smoothingTime <= 0f && Mathf.Abs(Vector3.Angle(base.transform.forward, vector2)) > 5f)
		{
			this.startingFacingDirection = base.transform.forward;
			this.smoothingTime = 0.5f;
		}
		vector2 *= d;
		if (!this.cc.isGrounded)
		{
			vector2.y -= 10f;
		}
		this.cc.Move(vector2 * Time.deltaTime);
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00091178 File Offset: 0x0008F378
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

	// Token: 0x06000B79 RID: 2937 RVA: 0x00091294 File Offset: 0x0008F494
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

	// Token: 0x040014CB RID: 5323
	public bool enemyVehicle;

	// Token: 0x040014CC RID: 5324
	public WayPoint[] path;

	// Token: 0x040014CD RID: 5325
	public WayPoint[] finalPath;

	// Token: 0x040014CE RID: 5326
	public int finalLap;

	// Token: 0x040014CF RID: 5327
	public bool startedFinalPath;

	// Token: 0x040014D0 RID: 5328
	public bool showPathWhileSelected = true;

	// Token: 0x040014D1 RID: 5329
	public bool pathIsaCircle;

	// Token: 0x040014D2 RID: 5330
	public bool stopAtLastPoint;

	// Token: 0x040014D3 RID: 5331
	public float arrivalDistance = 0.5f;

	// Token: 0x040014D4 RID: 5332
	public float speed = 18f;

	// Token: 0x040014D5 RID: 5333
	public int playIdleAfter = 4;

	// Token: 0x040014D6 RID: 5334
	public int currentDestination;

	// Token: 0x040014D7 RID: 5335
	private bool reachedPoint;

	// Token: 0x040014D8 RID: 5336
	private bool reversePath;

	// Token: 0x040014D9 RID: 5337
	private CharacterController cc;

	// Token: 0x040014DA RID: 5338
	private QuadBikeMovement.States state;

	// Token: 0x040014DB RID: 5339
	private int counterToIdle;

	// Token: 0x040014DC RID: 5340
	private float idleTimer;

	// Token: 0x040014DD RID: 5341
	public float maxVelocityChange = 0.2f;

	// Token: 0x040014DE RID: 5342
	private float rayCastTimer;

	// Token: 0x040014DF RID: 5343
	public string walkAnimation;

	// Token: 0x040014E0 RID: 5344
	public string idleAnimation;

	// Token: 0x040014E1 RID: 5345
	public Transform walkAlongside;

	// Token: 0x040014E2 RID: 5346
	public float catchupSpeed;

	// Token: 0x040014E3 RID: 5347
	public bool speaking;

	// Token: 0x040014E4 RID: 5348
	public Transform playerTransform;

	// Token: 0x040014E5 RID: 5349
	private static bool farisIsFar;

	// Token: 0x040014E6 RID: 5350
	public Transform daniaTransform;

	// Token: 0x040014E7 RID: 5351
	public Transform bikeTransform;

	// Token: 0x040014E8 RID: 5352
	public AnimationClip driverIdleAnim;

	// Token: 0x040014E9 RID: 5353
	public AnimationClip driverLeftAnim;

	// Token: 0x040014EA RID: 5354
	public AnimationClip driverRightAnim;

	// Token: 0x040014EB RID: 5355
	public AnimationClip turningRightAnim;

	// Token: 0x040014EC RID: 5356
	public AnimationClip turningLeftAnim;

	// Token: 0x040014ED RID: 5357
	public AnimationClip movingAnim;

	// Token: 0x040014EE RID: 5358
	private float driveTimeAfterDriverIsKilled = 3f;

	// Token: 0x040014EF RID: 5359
	public bool rotateWhileTurnining;

	// Token: 0x040014F0 RID: 5360
	[HideInInspector]
	public bool turning;

	// Token: 0x040014F1 RID: 5361
	private Quaternion startingRotation;

	// Token: 0x040014F2 RID: 5362
	private float timeBeforeDespawning = 3f;

	// Token: 0x040014F3 RID: 5363
	public int currentLap;

	// Token: 0x040014F4 RID: 5364
	public int startingPoint;

	// Token: 0x040014F5 RID: 5365
	private float smoothingTime;

	// Token: 0x040014F6 RID: 5366
	private Vector3 startingFacingDirection;

	// Token: 0x0200025F RID: 607
	private enum States
	{
		// Token: 0x040014F8 RID: 5368
		IDLE,
		// Token: 0x040014F9 RID: 5369
		WALK
	}
}
