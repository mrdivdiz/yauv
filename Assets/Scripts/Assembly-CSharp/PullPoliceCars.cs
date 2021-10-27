using System;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class PullPoliceCars : MonoBehaviour
{
	// Token: 0x06000669 RID: 1641 RVA: 0x000312F4 File Offset: 0x0002F4F4
	private void Start()
	{
		this.nodesList = (NavNode[])UnityEngine.Object.FindObjectsOfType(typeof(NavNode));
		this.policeCars = (PoliceCar[])UnityEngine.Object.FindObjectsOfType(typeof(PoliceCar));
		this.civilianCars = (CivilianCar[])UnityEngine.Object.FindObjectsOfType(typeof(CivilianCar));
		this.mainCamera = Camera.main;
		this.mask = 1 << LayerMask.NameToLayer("Car");
		this.mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		this.mask |= 1 << LayerMask.NameToLayer("Police");
		this.mask = ~this.mask;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x000313BC File Offset: 0x0002F5BC
	private void OnDestroy()
	{
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x000313C0 File Offset: 0x0002F5C0
	private void Update()
	{
		if (Time.timeSinceLevelLoad < 11f)
		{
			return;
		}
		if (this.timer <= 0f)
		{
			Vector3 position = base.transform.position;
			PoliceCar policeCar = null;
			PoliceCar policeCar2 = null;
			int num = 0;
			foreach (PoliceCar policeCar3 in this.policeCars)
			{
				if (policeCar == null || Vector3.Distance(position, policeCar3.transform.position) < Vector3.Distance(position, policeCar.transform.position))
				{
					policeCar = policeCar3;
				}
				if (policeCar2 == null || Vector3.Distance(position, policeCar3.transform.position) > Vector3.Distance(position, policeCar2.transform.position))
				{
					Vector3 vector = this.mainCamera.WorldToViewportPoint(policeCar3.transform.position);
					if ((vector.x <= 0f || vector.x >= 1f || vector.y <= 0f || vector.y >= 1f || vector.z <= 0f) && Vector3.Distance(policeCar3.transform.position, position) > 30f)
					{
						policeCar2 = policeCar3;
					}
				}
				if (Vector3.Distance(position, policeCar3.transform.position) < 50f)
				{
					num++;
				}
			}
			if (policeCar2 != null && policeCar != null && num < 2)
			{
				this.popoutNode = null;
				foreach (NavNode navNode in this.nodesList)
				{
					if (this.popoutNode == null || Vector3.Distance(position, navNode.transform.position) < Vector3.Distance(position, this.popoutNode.transform.position))
					{
						Vector3 vector2 = this.mainCamera.WorldToViewportPoint(navNode.transform.position);
						if ((vector2.x <= 0f || vector2.x >= 1f || vector2.y <= 0f || vector2.y >= 1f || vector2.z <= 0f) && ((this.spownInFront && base.transform.InverseTransformPoint(navNode.transform.position).z > 10f && Physics.Linecast(position, navNode.transform.position, this.mask)) || (!this.spownInFront && base.transform.InverseTransformPoint(navNode.transform.position).z < -2f)))
						{
							bool flag = false;
							foreach (PoliceCar policeCar4 in this.policeCars)
							{
								if (Vector3.Distance(navNode.transform.position, policeCar4.transform.position) < 10f)
								{
									flag = true;
								}
							}
							if (!flag)
							{
								foreach (CivilianCar civilianCar in this.civilianCars)
								{
									if (Vector3.Distance(navNode.transform.position, civilianCar.transform.position) < 10f)
									{
										flag = true;
									}
								}
								if (!flag)
								{
									this.popoutNode = navNode;
								}
							}
						}
					}
				}
				NavNode navNode2 = null;
				foreach (int num2 in this.popoutNode.connectedNodesRefs)
				{
					if (navNode2 == null || Vector3.Distance(position, NavReference.instance.nodes[num2].transform.position) > Vector3.Distance(position, navNode2.transform.position))
					{
						navNode2 = NavReference.instance.nodes[num2];
					}
				}
				policeCar2.transform.position = new Vector3(navNode2.transform.position.x, navNode2.transform.position.y, navNode2.transform.position.z);
				policeCar2.donotReverse = 2f;
				policeCar2.waitBeforeRecalculating = 0.1f;
				policeCar2.recalculatePath = true;
				NavNode navNode3 = null;
				foreach (int num3 in navNode2.connectedNodesRefs)
				{
					if (navNode3 == null || Vector3.Distance(position, NavReference.instance.nodes[num3].transform.position) < Vector3.Distance(position, navNode3.transform.position))
					{
						navNode3 = NavReference.instance.nodes[num3];
					}
				}
				if (navNode3 != null)
				{
					Vector3 position2 = navNode3.transform.position;
					position2.y = policeCar2.transform.position.y;
					policeCar2.transform.rotation = Quaternion.identity;
					policeCar2.transform.LookAt(position2);
				}
			}
			this.spownInFront = !this.spownInFront;
			this.timer = 0.5f;
		}
		else
		{
			this.timer -= Time.deltaTime;
		}
	}

	// Token: 0x040007AC RID: 1964
	private float timer = 2f;

	// Token: 0x040007AD RID: 1965
	private NavNode[] nodesList;

	// Token: 0x040007AE RID: 1966
	private PoliceCar[] policeCars;

	// Token: 0x040007AF RID: 1967
	private CivilianCar[] civilianCars;

	// Token: 0x040007B0 RID: 1968
	private Camera mainCamera;

	// Token: 0x040007B1 RID: 1969
	public NavNode popoutNode;

	// Token: 0x040007B2 RID: 1970
	private bool spownInFront = true;

	// Token: 0x040007B3 RID: 1971
	private int mask;
}
