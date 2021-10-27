using System;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class VehicleSpawner : MonoBehaviour
{
	// Token: 0x06000B82 RID: 2946 RVA: 0x000915D4 File Offset: 0x0008F7D4
	private void Awake()
	{
		VehicleSpawner.instance = this;
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x000915DC File Offset: 0x0008F7DC
	private void OnDestroy()
	{
		VehicleSpawner.instance = null;
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x000915E4 File Offset: 0x0008F7E4
	public static void Spawn(string groupID)
	{
		foreach (VehicleSpawner.SpawnGroup spawnGroup in VehicleSpawner.instance.groups)
		{
			if (spawnGroup.groupID == groupID)
			{
				foreach (VehicleSpawner.SpawnPath spawnPath in spawnGroup.paths)
				{
					if (spawnPath.helicopter != null)
					{
						spawnPath.helicopter.path = spawnPath.path;
						UnityEngine.Object.Instantiate(spawnPath.helicopter, spawnPath.path[0].transform.position, spawnPath.path[0].transform.rotation);
						spawnPath.helicopter.path = null;
					}
					else if (spawnPath.NPC_Prefab != null)
					{
						spawnPath.NPC_Prefab.path = spawnPath.path;
						UnityEngine.Object.Instantiate(spawnPath.NPC_Prefab, spawnPath.path[0].transform.position, spawnPath.path[0].transform.rotation);
						spawnPath.NPC_Prefab.path = null;
					}
					else if (spawnGroup.DefaultNPC_Prefab != null)
					{
						spawnGroup.DefaultNPC_Prefab.path = spawnPath.path;
						UnityEngine.Object.Instantiate(spawnGroup.DefaultNPC_Prefab, spawnPath.path[0].transform.position, spawnPath.path[0].transform.rotation);
						spawnGroup.DefaultNPC_Prefab.path = null;
					}
				}
			}
		}
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00091770 File Offset: 0x0008F970
	public static void PlayerReachedWaypoint(int waypointReached, int currentLap)
	{
		if (waypointReached == 37 && currentLap == 0)
		{
			VehicleSpawner.SaveCheckpoint(1);
		}
		if (waypointReached == 51 && currentLap == 0)
		{
			VehicleSpawner.SaveCheckpoint(2);
		}
		if (waypointReached == 52 && currentLap == 0)
		{
			VehicleSpawner.instance.cutSceneHeli.gameObject.SetActive(true);
		}
		foreach (VehicleSpawner.SpawnGroup spawnGroup in VehicleSpawner.instance.groups)
		{
			if (spawnGroup.spawnOnPlayerWaypointReached && spawnGroup.spawnWayPoint == waypointReached)
			{
				switch (spawnGroup.spawnType)
				{
				case VehicleSpawner.SpawnTypes.ALL_LAPS:
					VehicleSpawner.Spawn(spawnGroup.groupID);
					break;
				case VehicleSpawner.SpawnTypes.THIS_LAP_ONLY:
					if (spawnGroup.lapNo == currentLap)
					{
						VehicleSpawner.Spawn(spawnGroup.groupID);
					}
					break;
				case VehicleSpawner.SpawnTypes.THIS_AND_PREVIOUS_LAPS:
					if (spawnGroup.lapNo >= currentLap)
					{
						VehicleSpawner.Spawn(spawnGroup.groupID);
					}
					break;
				case VehicleSpawner.SpawnTypes.THIS_AND_COMING_LAPS:
					if (spawnGroup.lapNo <= currentLap)
					{
						VehicleSpawner.Spawn(spawnGroup.groupID);
					}
					break;
				}
			}
		}
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00091888 File Offset: 0x0008FA88
	private static void SaveCheckpoint(int checkpointNo)
	{
		int num = 5;
		if (SaveHandler.levelReached == num && SaveHandler.checkpointReached == checkpointNo - 1)
		{
			SaveHandler.SaveCheckpoint(num, checkpointNo, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
			SpeechManager.displayCheckpointReached = 4f;
		}
		if (mainmenu.replayLevel && SaveHandler.replayCheckpointReached == checkpointNo - 1)
		{
			SaveHandler.SaveCheckpointOnReplay(checkpointNo, Vector3.zero, Vector3.zero, string.Empty, string.Empty, 0, 0, 0, 0, 0);
			SpeechManager.displayCheckpointReached = 4f;
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0009191C File Offset: 0x0008FB1C
	private void OnDrawGizmos()
	{
		foreach (VehicleSpawner.SpawnGroup spawnGroup in this.groups)
		{
			foreach (VehicleSpawner.SpawnPath spawnPath in spawnGroup.paths)
			{
				if (spawnPath.path.Length == 0)
				{
					return;
				}
				WayPoint wayPoint = spawnPath.path[0];
				foreach (WayPoint wayPoint2 in spawnPath.path)
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
							Gizmos.color = Color.black;
							Gizmos.DrawLine(wayPoint.transform.position, wayPoint2.transform.position);
						}
						wayPoint = wayPoint2;
					}
				}
			}
		}
	}

	// Token: 0x04001504 RID: 5380
	public VehicleSpawner.SpawnGroup[] groups;

	// Token: 0x04001505 RID: 5381
	public static VehicleSpawner instance;

	// Token: 0x04001506 RID: 5382
	public HelicopterMovement cutSceneHeli;

	// Token: 0x02000263 RID: 611
	public enum SpawnTypes
	{
		// Token: 0x04001508 RID: 5384
		ALL_LAPS,
		// Token: 0x04001509 RID: 5385
		THIS_LAP_ONLY,
		// Token: 0x0400150A RID: 5386
		THIS_AND_PREVIOUS_LAPS,
		// Token: 0x0400150B RID: 5387
		THIS_AND_COMING_LAPS
	}

	// Token: 0x02000264 RID: 612
	[Serializable]
	public class SpawnPath
	{
		// Token: 0x0400150C RID: 5388
		public QuadBikeMovement NPC_Prefab;

		// Token: 0x0400150D RID: 5389
		public HelicopterMovement helicopter;

		// Token: 0x0400150E RID: 5390
		public WayPoint[] path;
	}

	// Token: 0x02000265 RID: 613
	[Serializable]
	public class SpawnGroup
	{
		// Token: 0x0400150F RID: 5391
		public string groupID;

		// Token: 0x04001510 RID: 5392
		public QuadBikeMovement DefaultNPC_Prefab;

		// Token: 0x04001511 RID: 5393
		public VehicleSpawner.SpawnPath[] paths;

		// Token: 0x04001512 RID: 5394
		public bool spawnOnPlayerWaypointReached;

		// Token: 0x04001513 RID: 5395
		public VehicleSpawner.SpawnTypes spawnType;

		// Token: 0x04001514 RID: 5396
		public int spawnWayPoint;

		// Token: 0x04001515 RID: 5397
		public int lapNo = 1;
	}
}
