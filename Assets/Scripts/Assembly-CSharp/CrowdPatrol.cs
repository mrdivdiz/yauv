using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class CrowdPatrol : MonoBehaviour
{
	// Token: 0x060005BC RID: 1468 RVA: 0x00027498 File Offset: 0x00025698
	private void Start()
	{
		this.cc = base.gameObject.GetComponent<CharacterController>();
		if (this.path.Length > 0)
		{
			base.transform.position = this.path[0].transform.position;
		}
		base.GetComponent<Animation>()["Crowd-Walk"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Crowd-Idle1"].wrapMode = WrapMode.Loop;
		this.counterToIdle = this.playIdleAfter;
		this.idleTimer = base.GetComponent<Animation>()["Crowd-Idle1"].length;
		this.state = CrowdPatrol.States.WALK;
		base.GetComponent<Animation>().CrossFade("Crowd-Walk");
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002754C File Offset: 0x0002574C
	private void Update()
	{
		CrowdPatrol.States states = this.state;
		if (Vector3.Distance(base.transform.position, this.path[this.currentDestination].transform.position) < this.arrivalDistance)
		{
			this.reachedPoint = true;
		}
		if (this.reachedPoint)
		{
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
			this.counterToIdle--;
			if (this.counterToIdle <= 0)
			{
				this.state = CrowdPatrol.States.IDLE;
			}
		}
		Vector3 position = base.transform.position;
		position.y += 1f;
		RaycastHit raycastHit;
		if (Time.time > this.rayCastTimer + 0.5f && Physics.Raycast(position, base.transform.forward, out raycastHit, 1f))
		{
			this.state = CrowdPatrol.States.IDLE;
			this.rayCastTimer = Time.time;
		}
		if (this.state == CrowdPatrol.States.WALK)
		{
			this.MoveToPosition(this.path[this.currentDestination].transform.position);
		}
		else if (this.idleTimer > 0f)
		{
			this.idleTimer -= Time.deltaTime;
		}
		else
		{
			this.state = CrowdPatrol.States.WALK;
			this.counterToIdle = this.playIdleAfter;
			this.idleAnim = this.GetIdleAnim();
			this.idleTimer = base.GetComponent<Animation>()[this.idleAnim].length;
		}
		if (this.state != states)
		{
			if (this.state == CrowdPatrol.States.WALK)
			{
				base.GetComponent<Animation>().CrossFade("Crowd-Walk");
			}
			else
			{
				base.GetComponent<Animation>().CrossFade(this.idleAnim);
			}
		}
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0002779C File Offset: 0x0002599C
	private void MoveToPosition(Vector3 target)
	{
		Vector3 vector = target - base.transform.position;
		vector.y = 0f;
		vector = vector.normalized;
		CharacterController characterController = base.GetComponent(typeof(CharacterController)) as CharacterController;
		Vector3 velocity = characterController.velocity;
		float num = 0.9f;
		Quaternion to = Quaternion.LookRotation(vector);
		float t = Mathf.Min(num * Time.deltaTime, 1f);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, t);
		float num2 = velocity.magnitude;
		float num3 = this.speed - velocity.magnitude;
		if (num3 > this.maxVelocityChange)
		{
			num3 = this.maxVelocityChange;
		}
		num2 += num3;
		float d = num2;
		if (Vector3.Angle(base.transform.forward, new Vector3(vector.x, 0f, vector.z)) < 30f)
		{
			this.cc.SimpleMove(vector * d);
		}
		else
		{
			this.cc.SimpleMove(base.transform.forward * d);
		}
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x000278D0 File Offset: 0x00025AD0
	private string GetIdleAnim()
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (num == 0)
		{
			return "Crowd-Idle1";
		}
		if (num == 1)
		{
			return "Crowd-Idle2";
		}
		return "Crowd-Idle1";
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00027904 File Offset: 0x00025B04
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
				if (Physics.Linecast(wayPoint.transform.position, wayPoint2.transform.position))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				else
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				wayPoint = wayPoint2;
			}
		}
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x000279D8 File Offset: 0x00025BD8
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
				if (wayPoint2 != null && Physics.Linecast(wayPoint.transform.position, wayPoint2.transform.position))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				else
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
				}
				wayPoint = wayPoint2;
			}
		}
	}

	// Token: 0x04000635 RID: 1589
	public WayPoint[] path;

	// Token: 0x04000636 RID: 1590
	public bool showPathWhileSelected = true;

	// Token: 0x04000637 RID: 1591
	public bool pathIsaCircle;

	// Token: 0x04000638 RID: 1592
	public float arrivalDistance = 0.5f;

	// Token: 0x04000639 RID: 1593
	public float speed = 18f;

	// Token: 0x0400063A RID: 1594
	public int playIdleAfter = 4;

	// Token: 0x0400063B RID: 1595
	public int currentDestination;

	// Token: 0x0400063C RID: 1596
	private bool reachedPoint;

	// Token: 0x0400063D RID: 1597
	private bool reversePath;

	// Token: 0x0400063E RID: 1598
	private CharacterController cc;

	// Token: 0x0400063F RID: 1599
	private CrowdPatrol.States state;

	// Token: 0x04000640 RID: 1600
	private int counterToIdle;

	// Token: 0x04000641 RID: 1601
	private float idleTimer;

	// Token: 0x04000642 RID: 1602
	private string idleAnim = "Crowd-Idle1";

	// Token: 0x04000643 RID: 1603
	public float maxVelocityChange = 0.2f;

	// Token: 0x04000644 RID: 1604
	private float rayCastTimer;

	// Token: 0x020000FA RID: 250
	private enum States
	{
		// Token: 0x04000646 RID: 1606
		IDLE,
		// Token: 0x04000647 RID: 1607
		WALK
	}
}
