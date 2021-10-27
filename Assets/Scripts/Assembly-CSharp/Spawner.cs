using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class Spawner : MonoBehaviour
{
	// Token: 0x060005DD RID: 1501 RVA: 0x00028D88 File Offset: 0x00026F88
	private void Awake()
	{
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x00028D8C File Offset: 0x00026F8C
	private void OnDestroy()
	{
		foreach (Spawner.SpawnGroup spawnGroup in this.groups)
		{
			foreach (Spawner.SpawnPosition spawnPosition in spawnGroup.positions)
			{
				spawnPosition.NPC_Prefab = null;
				spawnPosition.entryAnim = null;
				spawnPosition.weapon = null;
				spawnPosition.spawnPosition = null;
			}
			for (int k = 0; k < spawnGroup.positions.Length; k++)
			{
				spawnGroup.positions[k] = null;
			}
			spawnGroup.positions = null;
			spawnGroup.groupID = null;
			spawnGroup.DefaultNPC_Prefab = null;
			spawnGroup.defautWeapon = null;
			spawnGroup.positions = null;
		}
		for (int l = 0; l < this.groups.Length; l++)
		{
			this.groups[l] = null;
		}
		this.groups = null;
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00028E74 File Offset: 0x00027074
	public void Spawn(string groupID)
	{
		foreach (Spawner.SpawnGroup spawnGroup in this.groups)
		{
			if (spawnGroup.groupID == groupID)
			{
				foreach (Spawner.SpawnPosition spawnPosition in spawnGroup.positions)
				{
					if (spawnPosition.NPC_Prefab != null)
					{
						spawnPosition.NPC_Prefab.priorityToCover = spawnPosition.priorityToCover;
						if (spawnPosition.weapon != null)
						{
							Gun weaponPrefab = spawnPosition.NPC_Prefab.weaponPrefab;
							spawnPosition.NPC_Prefab.weaponPrefab = spawnPosition.weapon;
							if (spawnPosition.entryAnim != null)
							{
								spawnPosition.NPC_Prefab.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(spawnPosition.NPC_Prefab, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							spawnPosition.NPC_Prefab.weaponPrefab = weaponPrefab;
							if (spawnPosition.entryAnim != null)
							{
								spawnPosition.NPC_Prefab.EntryAnim = null;
							}
						}
						else if (spawnGroup.defautWeapon != null)
						{
							Gun weaponPrefab2 = spawnPosition.NPC_Prefab.weaponPrefab;
							spawnPosition.NPC_Prefab.weaponPrefab = spawnGroup.defautWeapon;
							if (spawnPosition.entryAnim != null)
							{
								spawnPosition.NPC_Prefab.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(spawnPosition.NPC_Prefab, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							spawnPosition.NPC_Prefab.weaponPrefab = weaponPrefab2;
							if (spawnPosition.entryAnim != null)
							{
								spawnPosition.NPC_Prefab.EntryAnim = null;
							}
						}
						else
						{
							if (spawnPosition.entryAnim != null)
							{
								spawnPosition.NPC_Prefab.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(spawnPosition.NPC_Prefab, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							if (spawnPosition.entryAnim != null)
							{
								spawnPosition.NPC_Prefab.EntryAnim = null;
							}
						}
						spawnPosition.NPC_Prefab = null;
						spawnPosition.weapon = null;
						spawnPosition.entryAnim = null;
					}
					else if (spawnGroup.DefaultNPC_Prefab != null)
					{
						spawnGroup.DefaultNPC_Prefab.priorityToCover = spawnPosition.priorityToCover;
						if (spawnPosition.weapon != null)
						{
							Gun weaponPrefab3 = spawnGroup.DefaultNPC_Prefab.weaponPrefab;
							spawnGroup.DefaultNPC_Prefab.weaponPrefab = spawnPosition.weapon;
							if (spawnPosition.entryAnim != null)
							{
								spawnGroup.DefaultNPC_Prefab.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(spawnGroup.DefaultNPC_Prefab, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							spawnGroup.DefaultNPC_Prefab.weaponPrefab = weaponPrefab3;
							if (spawnPosition.entryAnim != null)
							{
								spawnGroup.DefaultNPC_Prefab.EntryAnim = null;
							}
						}
						else if (spawnGroup.defautWeapon != null)
						{
							Gun weaponPrefab4 = spawnGroup.DefaultNPC_Prefab.weaponPrefab;
							spawnGroup.DefaultNPC_Prefab.weaponPrefab = spawnGroup.defautWeapon;
							if (spawnPosition.entryAnim != null)
							{
								spawnGroup.DefaultNPC_Prefab.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(spawnGroup.DefaultNPC_Prefab, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							spawnGroup.DefaultNPC_Prefab.weaponPrefab = weaponPrefab4;
							if (spawnPosition.entryAnim != null)
							{
								spawnGroup.DefaultNPC_Prefab.EntryAnim = null;
							}
						}
						else
						{
							if (spawnPosition.entryAnim != null)
							{
								spawnGroup.DefaultNPC_Prefab.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(spawnGroup.DefaultNPC_Prefab, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							if (spawnPosition.entryAnim != null)
							{
								spawnGroup.DefaultNPC_Prefab.EntryAnim = null;
							}
						}
					}
					spawnPosition.NPC_Prefab = null;
					spawnPosition.entryAnim = null;
					spawnPosition.weapon = null;
					spawnPosition.spawnPosition = null;
				}
				spawnGroup.DefaultNPC_Prefab = null;
				spawnGroup.defautWeapon = null;
			}
		}
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x000292AC File Offset: 0x000274AC
	public IEnumerator Spawn(string groupID, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.Spawn(groupID);
		yield break;
	}

	// Token: 0x04000684 RID: 1668
	public Spawner.SpawnGroup[] groups;

	// Token: 0x02000104 RID: 260
	[Serializable]
	public class SpawnPosition
	{
		// Token: 0x04000685 RID: 1669
		public BotAI NPC_Prefab;

		// Token: 0x04000686 RID: 1670
		public Transform spawnPosition;

		// Token: 0x04000687 RID: 1671
		public Gun weapon;

		// Token: 0x04000688 RID: 1672
		public AnimationClip entryAnim;

		// Token: 0x04000689 RID: 1673
		public bool priorityToCover;
	}

	// Token: 0x02000105 RID: 261
	[Serializable]
	public class SpawnGroup
	{
		// Token: 0x0400068A RID: 1674
		public string groupID;

		// Token: 0x0400068B RID: 1675
		public BotAI DefaultNPC_Prefab;

		// Token: 0x0400068C RID: 1676
		public Gun defautWeapon;

		// Token: 0x0400068D RID: 1677
		public Spawner.SpawnPosition[] positions;
	}
}
