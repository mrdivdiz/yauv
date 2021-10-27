using System;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class NPCWaypointWalk : MonoBehaviour
{
	// Token: 0x060005CA RID: 1482 RVA: 0x00027CBC File Offset: 0x00025EBC
	private void Start()
	{
		this.cc = base.gameObject.GetComponent<CharacterController>();
		if (this.path.Length > 0)
		{
			Vector3 position = this.path[0].transform.position;
			position.y = base.transform.position.y;
			base.transform.position = position;
		}
		this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		base.GetComponent<Animation>()[this.walkAnimation].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()[this.idleAnimation].wrapMode = WrapMode.Loop;
		this.counterToIdle = this.playIdleAfter;
		this.idleTimer = base.GetComponent<Animation>()[this.idleAnimation].length;
		this.state = NPCWaypointWalk.States.WALK;
		base.GetComponent<Animation>().CrossFade(this.walkAnimation);
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00027DA4 File Offset: 0x00025FA4
	private void FixedUpdate()
	{
		NPCWaypointWalk.States states = this.state;
		Vector3 position = this.path[this.currentDestination].transform.position;
		position.y = base.transform.position.y;
		if (Vector3.Distance(base.transform.position, position) < this.arrivalDistance)
		{
			this.reachedPoint = true;
		}
		if (this.currentDestination == this.path.Length - 1 && this.stopAtLastPoint && this.reachedPoint)
		{
			base.GetComponent<Animation>().CrossFade(this.idleAnimation);
			return;
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
				this.state = NPCWaypointWalk.States.IDLE;
			}
		}
		Vector3 position2 = base.transform.position;
		position2.y += 1f;
		if (this.playerTransform != null && Vector3.Distance(base.transform.position, this.playerTransform.position) > 15f)
		{
			NPCWaypointWalk.farisIsFar = true;
		}
		else if (this.playerTransform != null && NPCWaypointWalk.farisIsFar && Vector3.Distance(base.transform.position, this.playerTransform.position) < 3f)
		{
			NPCWaypointWalk.farisIsFar = false;
		}
		if (NPCWaypointWalk.farisIsFar)
		{
			this.state = NPCWaypointWalk.States.IDLE;
			if (!this.WaitForFarisSoundPlayed && this.WaitForFarisSound != string.Empty && !SpeechManager.instance.playing)
			{
				SpeechManager.PlayConversation(this.WaitForFarisSound);
				this.WaitForFarisSoundPlayed = true;
			}
		}
		else
		{
			this.state = NPCWaypointWalk.States.WALK;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(position2, base.transform.forward, out raycastHit, 1f))
		{
			this.state = NPCWaypointWalk.States.IDLE;
		}
		else if (!NPCWaypointWalk.farisIsFar)
		{
			this.state = NPCWaypointWalk.States.WALK;
		}
		if (this.state == NPCWaypointWalk.States.WALK)
		{
			this.MoveToPosition(this.path[this.currentDestination].transform.position);
		}
		else
		{
			if (this.idleTimer > 0f)
			{
				this.idleTimer -= Time.deltaTime;
			}
			else
			{
				this.state = NPCWaypointWalk.States.WALK;
				this.counterToIdle = this.playIdleAfter;
				this.idleTimer = base.GetComponent<Animation>()[this.idleAnimation].length;
			}
			Vector3 zero = Vector3.zero;
			zero.y -= 20f * Time.deltaTime;
			this.cc.Move(zero * Time.deltaTime);
		}
		if (this.state != states)
		{
			if (this.state == NPCWaypointWalk.States.WALK)
			{
				base.GetComponent<Animation>().CrossFade(this.walkAnimation);
			}
			else
			{
				base.GetComponent<Animation>().CrossFade(this.idleAnimation);
			}
		}
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00028154 File Offset: 0x00026354
	private void MoveToPosition(Vector3 target)
	{
		target.y = base.transform.position.y;
		base.transform.LookAt(target);
		float num = this.speed;
		if (this.walkAlongside != null && base.transform.InverseTransformPoint(this.walkAlongside.position).z > 0.1f)
		{
			num += this.catchupSpeed;
			base.GetComponent<Animation>()[this.walkAnimation].speed = 1.5f;
		}
		else if (this.walkAlongside != null)
		{
			base.GetComponent<Animation>()[this.walkAnimation].speed = 1f;
		}
		if (this.walkAlongside != null && this.speaking && base.transform.InverseTransformPoint(this.walkAlongside.position).z < 0.1f)
		{
			num -= 0.2f;
			base.GetComponent<Animation>()[this.walkAnimation].speed = 0.8f;
		}
		else if (this.walkAlongside != null)
		{
			base.GetComponent<Animation>()[this.walkAnimation].speed = 1f;
		}
		Vector3 a = Vector3.zero;
		if (this.cc.isGrounded)
		{
			a = (target - base.transform.position).normalized;
			a *= num;
		}
		a.y -= 20f * Time.deltaTime;
		this.cc.Move(a * Time.deltaTime);
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00028320 File Offset: 0x00026520
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

	// Token: 0x060005CE RID: 1486 RVA: 0x000283F4 File Offset: 0x000265F4
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

	// Token: 0x0400064E RID: 1614
	public WayPoint[] path;

	// Token: 0x0400064F RID: 1615
	public bool showPathWhileSelected = true;

	// Token: 0x04000650 RID: 1616
	public bool pathIsaCircle;

	// Token: 0x04000651 RID: 1617
	public bool stopAtLastPoint;

	// Token: 0x04000652 RID: 1618
	public float arrivalDistance = 0.5f;

	// Token: 0x04000653 RID: 1619
	public float speed = 18f;

	// Token: 0x04000654 RID: 1620
	public int playIdleAfter = 4;

	// Token: 0x04000655 RID: 1621
	public int currentDestination;

	// Token: 0x04000656 RID: 1622
	private bool reachedPoint;

	// Token: 0x04000657 RID: 1623
	private bool reversePath;

	// Token: 0x04000658 RID: 1624
	private CharacterController cc;

	// Token: 0x04000659 RID: 1625
	private NPCWaypointWalk.States state;

	// Token: 0x0400065A RID: 1626
	private int counterToIdle;

	// Token: 0x0400065B RID: 1627
	private float idleTimer;

	// Token: 0x0400065C RID: 1628
	public float maxVelocityChange = 0.2f;

	// Token: 0x0400065D RID: 1629
	private float rayCastTimer;

	// Token: 0x0400065E RID: 1630
	public string walkAnimation;

	// Token: 0x0400065F RID: 1631
	public string idleAnimation;

	// Token: 0x04000660 RID: 1632
	public Transform walkAlongside;

	// Token: 0x04000661 RID: 1633
	public float catchupSpeed;

	// Token: 0x04000662 RID: 1634
	public bool speaking;

	// Token: 0x04000663 RID: 1635
	public Transform playerTransform;

	// Token: 0x04000664 RID: 1636
	private static bool farisIsFar;

	// Token: 0x04000665 RID: 1637
	public string WaitForFarisSound = string.Empty;

	// Token: 0x04000666 RID: 1638
	private bool WaitForFarisSoundPlayed;

	// Token: 0x020000FF RID: 255
	private enum States
	{
		// Token: 0x04000668 RID: 1640
		IDLE,
		// Token: 0x04000669 RID: 1641
		WALK
	}
}
