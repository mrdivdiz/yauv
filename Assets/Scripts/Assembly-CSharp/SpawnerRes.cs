using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class SpawnerRes : MonoBehaviour
{
	// Token: 0x060005E4 RID: 1508 RVA: 0x000292FC File Offset: 0x000274FC
	private void Awake()
	{
		SpawnerRes.instance = this;
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00029304 File Offset: 0x00027504
	private void OnDestroy()
	{
		SpawnerRes.instance = null;
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002930C File Offset: 0x0002750C
	public static void Spawn(string groupID)
	{
		foreach (SpawnerRes.SpawnGroup spawnGroup in SpawnerRes.instance.groups)
		{
			if (spawnGroup.groupID == groupID)
			{
				foreach (SpawnerRes.SpawnPosition spawnPosition in spawnGroup.positions)
				{
					if (spawnPosition.NPC_Prefab_Name != null && spawnPosition.NPC_Prefab_Name != string.Empty)
					{
						BotAI component = ((GameObject)Resources.Load(spawnPosition.NPC_Prefab_Name)).GetComponent<BotAI>();
						component.priorityToCover = spawnPosition.priorityToCover;
						if (spawnPosition.weaponName != null && spawnPosition.weaponName != string.Empty)
						{
							Gun weaponPrefab = component.weaponPrefab;
							component.weaponPrefab = ((GameObject)Resources.Load(spawnPosition.weaponName)).GetComponent<Gun>();
							if (spawnPosition.entryAnim != null)
							{
								component.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(component, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							component.weaponPrefab = weaponPrefab;
							if (spawnPosition.entryAnim != null)
							{
								component.EntryAnim = null;
							}
						}
						else if (spawnGroup.defautWeaponName != null && spawnGroup.defautWeaponName != string.Empty)
						{
							Gun weaponPrefab2 = component.weaponPrefab;
							component.weaponPrefab = ((GameObject)Resources.Load(spawnGroup.defautWeaponName)).GetComponent<Gun>();
							if (spawnPosition.entryAnim != null)
							{
								component.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(component, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							component.weaponPrefab = weaponPrefab2;
							if (spawnPosition.entryAnim != null)
							{
								component.EntryAnim = null;
							}
						}
						else
						{
							if (spawnPosition.entryAnim != null)
							{
								component.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(component, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							if (spawnPosition.entryAnim != null)
							{
								component.EntryAnim = null;
							}
						}
					}
					else if (spawnGroup.DefaultNPC_Prefab_Name != null && spawnGroup.DefaultNPC_Prefab_Name != string.Empty)
					{
						BotAI component2 = ((GameObject)Resources.Load(spawnGroup.DefaultNPC_Prefab_Name)).GetComponent<BotAI>();
						component2.priorityToCover = spawnPosition.priorityToCover;
						if (spawnPosition.weaponName != null && spawnPosition.weaponName != string.Empty)
						{
							Gun weaponPrefab3 = component2.weaponPrefab;
							component2.weaponPrefab = ((GameObject)Resources.Load(spawnPosition.weaponName)).GetComponent<Gun>();
							if (spawnPosition.entryAnim != null)
							{
								component2.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(component2, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							component2.weaponPrefab = weaponPrefab3;
							if (spawnPosition.entryAnim != null)
							{
								component2.EntryAnim = null;
							}
						}
						else if (spawnGroup.defautWeaponName != null && spawnGroup.defautWeaponName != string.Empty)
						{
							Gun weaponPrefab4 = component2.weaponPrefab;
							component2.weaponPrefab = ((GameObject)Resources.Load(spawnGroup.defautWeaponName)).GetComponent<Gun>();
							if (spawnPosition.entryAnim != null)
							{
								component2.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(component2, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							component2.weaponPrefab = weaponPrefab4;
							if (spawnPosition.entryAnim != null)
							{
								component2.EntryAnim = null;
							}
						}
						else
						{
							if (spawnPosition.entryAnim != null)
							{
								component2.EntryAnim = spawnPosition.entryAnim;
							}
							UnityEngine.Object.Instantiate(component2, spawnPosition.spawnPosition.position, spawnPosition.spawnPosition.rotation);
							if (spawnPosition.entryAnim != null)
							{
								component2.EntryAnim = null;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002974C File Offset: 0x0002794C
	public static IEnumerator Spawn(string groupID, float delay)
	{
		yield return new WaitForSeconds(delay);
		SpawnerRes.Spawn(groupID);
		yield break;
	}

	// Token: 0x0400068E RID: 1678
	public SpawnerRes.SpawnGroup[] groups;

	// Token: 0x0400068F RID: 1679
	public static SpawnerRes instance;

	// Token: 0x02000107 RID: 263
	[Serializable]
	public class SpawnPosition
	{
		// Token: 0x04000690 RID: 1680
		public BotAI NPC_Prefab;

		// Token: 0x04000691 RID: 1681
		public string NPC_Prefab_Name;

		// Token: 0x04000692 RID: 1682
		public Transform spawnPosition;

		// Token: 0x04000693 RID: 1683
		public Gun weapon;

		// Token: 0x04000694 RID: 1684
		public string weaponName;

		// Token: 0x04000695 RID: 1685
		public AnimationClip entryAnim;

		// Token: 0x04000696 RID: 1686
		public bool priorityToCover;
	}

	// Token: 0x02000108 RID: 264
	[Serializable]
	public class SpawnGroup
	{
		// Token: 0x04000697 RID: 1687
		public string groupID;

		// Token: 0x04000698 RID: 1688
		public BotAI DefaultNPC_Prefab;

		// Token: 0x04000699 RID: 1689
		public string DefaultNPC_Prefab_Name;

		// Token: 0x0400069A RID: 1690
		public Gun defautWeapon;

		// Token: 0x0400069B RID: 1691
		public string defautWeaponName;

		// Token: 0x0400069C RID: 1692
		public SpawnerRes.SpawnPosition[] positions;
	}
}
