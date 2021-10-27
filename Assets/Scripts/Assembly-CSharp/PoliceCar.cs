using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class PoliceCar : MonoBehaviour
{
	// Token: 0x06000661 RID: 1633 RVA: 0x0003067C File Offset: 0x0002E87C
	private void Start()
	{
		base.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0.1f, 1f);
		base.GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
		base.GetComponent<Rigidbody>().inertiaTensor = new Vector3(1f, 1f, 2f) * base.GetComponent<Rigidbody>().mass;
		this.RecalculatePath();
		if (this.target == null)
		{
			this.target = GameObject.FindGameObjectWithTag("PlayerCar").transform;
		}
		this.layerMask = 1 << LayerMask.NameToLayer("Police");
		this.layerMask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.layerMask = ~this.layerMask;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00030754 File Offset: 0x0002E954
	private void FixedUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = this.target.position;
		if (this.donotReverse > 0f)
		{
			this.donotReverse -= Time.deltaTime;
			this.movingInReverse = Time.time - 2f;
		}
		RaycastHit raycastHit;
		Vector3 a;
		if (Vector3.Distance(position, position2) < 30f && (!Physics.Linecast(position, position2, out raycastHit, this.layerMask) || raycastHit.collider.tag == "PlayerCar"))
		{
			a = position2;
			a.y = position.y;
			base.GetComponent<Rigidbody>().drag = 0f;
		}
		else
		{
			if (this.recalculatePath)
			{
				if (this.waitBeforeRecalculating <= 0f)
				{
					this.startNode = null;
					foreach (NavNode navNode in UnityEngine.Object.FindObjectsOfType(typeof(NavNode)))
					{
						if (this.startNode == null || Vector3.Distance(position, navNode.transform.position) < Vector3.Distance(position, this.startNode.transform.position))
						{
							this.startNode = navNode;
						}
					}
					this.path[this.currentPoint] = this.startNode;
					this.recalculatePath = false;
				}
				else
				{
					this.waitBeforeRecalculating -= Time.fixedDeltaTime;
				}
			}
			if (this.path[this.currentPoint] != null && Vector3.Distance(position, this.path[this.currentPoint].transform.position) < 3f)
			{
				NavNode navNode2 = null;
				foreach (int num in this.path[this.currentPoint].connectedNodesRefs)
				{
					if (navNode2 == null || (Vector3.Distance(position2, NavReference.instance.nodes[num].transform.position) < Vector3.Distance(position2, navNode2.transform.position) && (this.currentPoint < 1 || NavReference.instance.nodes[num] != this.path[this.currentPoint - 1])))
					{
						navNode2 = NavReference.instance.nodes[num];
					}
				}
				Array.Resize<NavNode>(ref this.path, this.path.Length + 1);
				this.path[this.path.Length - 1] = navNode2;
				this.currentPoint++;
			}
			else if (this.path[this.currentPoint] != null && Vector3.Distance(position, this.path[this.currentPoint].transform.position) < 15f && this.path[this.currentPoint].connectedNodesRefs.Length > 2)
			{
				if (base.GetComponent<Rigidbody>().velocity.sqrMagnitude > 25f)
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
			if (this.path[this.currentPoint] != null)
			{
				a = this.path[this.currentPoint].transform.position;
				a.y = position.y;
			}
			else
			{
				a = this.target.transform.position;
				a.y = position.y;
			}
		}
		float num2 = Vector3.Angle(base.transform.forward, a - position);
		num2 *= Mathf.Clamp01(100f / (2f * this.FLWheel.rpm));
		if (Vector3.Cross(base.transform.forward, a - position).y > 0f)
		{
			num2 *= -1f;
		}
		this.FLWheel.steerAngle = Mathf.Clamp(-num2, -45f, 45f);
		this.FRWheel.steerAngle = Mathf.Clamp(-num2, -45f, 45f);
		this.FLWheelTransform.localEulerAngles = new Vector3(this.FLWheelTransform.localEulerAngles.x, this.FLWheel.steerAngle, 0f);
		this.FRWheelTransform.localEulerAngles = new Vector3(this.FRWheelTransform.localEulerAngles.x, this.FRWheel.steerAngle, 0f);
		this.FLWheelTransform.Rotate(this.FLWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		this.FRWheelTransform.Rotate(this.FRWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		this.RLWheelTransform.Rotate(this.RLWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		this.RRWheelTransform.Rotate(this.RRWheel.rpm * 6f * Time.deltaTime, 0f, 0f);
		if (Time.time >= this.movingInReverse + 2f || Time.time < 6f)
		{
			base.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, 6400f);
		}
		else
		{
			base.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, -6400f);
		}
		float sqrMagnitude = base.GetComponent<Rigidbody>().velocity.sqrMagnitude;
		if ((this.FLWheel.steerAngle > 5f || this.FLWheel.steerAngle < -5f) && sqrMagnitude > 20f)
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
		else
		{
			base.GetComponent<Rigidbody>().drag = 0f;
		}
		if ((double)sqrMagnitude <= 0.1 && base.GetComponent<Rigidbody>().drag == 0f && Time.time > this.movingInReverse + 4f)
		{
			this.movingInReverse = Time.time;
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00030E94 File Offset: 0x0002F094
	private void RecalculatePath()
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = this.target.position;
		foreach (NavNode navNode in UnityEngine.Object.FindObjectsOfType(typeof(NavNode)))
		{
			if (this.destNode == null || Vector3.Distance(position2, navNode.transform.position) < Vector3.Distance(position2, this.destNode.transform.position))
			{
				this.destNode = navNode;
			}
			if (this.startNode == null || Vector3.Distance(position, navNode.transform.position) < Vector3.Distance(position, this.startNode.transform.position))
			{
				this.startNode = navNode;
			}
		}
		Array.Resize<NavNode>(ref this.path, this.path.Length + 1);
		this.path[0] = this.startNode;
	}

	// Token: 0x0400078D RID: 1933
	public WheelCollider RLWheel;

	// Token: 0x0400078E RID: 1934
	public WheelCollider RRWheel;

	// Token: 0x0400078F RID: 1935
	public WheelCollider FLWheel;

	// Token: 0x04000790 RID: 1936
	public WheelCollider FRWheel;

	// Token: 0x04000791 RID: 1937
	public Transform RLWheelTransform;

	// Token: 0x04000792 RID: 1938
	public Transform RRWheelTransform;

	// Token: 0x04000793 RID: 1939
	public Transform FLWheelTransform;

	// Token: 0x04000794 RID: 1940
	public Transform FRWheelTransform;

	// Token: 0x04000795 RID: 1941
	public NavNode[] path;

	// Token: 0x04000796 RID: 1942
	public int currentPoint;

	// Token: 0x04000797 RID: 1943
	public Transform target;

	// Token: 0x04000798 RID: 1944
	public NavNode destNode;

	// Token: 0x04000799 RID: 1945
	public NavNode startNode;

	// Token: 0x0400079A RID: 1946
	public bool recalculatePath;

	// Token: 0x0400079B RID: 1947
	public float movingInReverse;

	// Token: 0x0400079C RID: 1948
	public bool chaseTarget;

	// Token: 0x0400079D RID: 1949
	private int layerMask;

	// Token: 0x0400079E RID: 1950
	public float donotReverse = 1f;

	// Token: 0x0400079F RID: 1951
	public float waitBeforeRecalculating;
}
