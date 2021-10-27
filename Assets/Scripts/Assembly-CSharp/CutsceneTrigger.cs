using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class CutsceneTrigger : MonoBehaviour
{
	// Token: 0x060006F4 RID: 1780 RVA: 0x00037F7C File Offset: 0x0003617C
	public void Awake()
	{
		if (Camera.main != null)
		{
			this.cam = Camera.main;
		}
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00037F9C File Offset: 0x0003619C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player" && !this.played)
		{
			if (this.forceColt)
			{
				GunManager componentInChildren = collisionInfo.gameObject.GetComponentInChildren<GunManager>();
				if (componentInChildren != null)
				{
					componentInChildren.ForceColt();
				}
			}
			CutsceneTrigger.CutsceneType cutsceneType = this.type;
			if (cutsceneType != CutsceneTrigger.CutsceneType.CUTSCENE)
			{
				if (cutsceneType == CutsceneTrigger.CutsceneType.MELEE)
				{
					if (this.objects != null)
					{
						CutsceneManager.PlayMeleeEncounter(this.objects);
					}
					else
					{
						CutsceneManager.PlayMeleeEncounter();
					}
					if (this.killAllPreviousEnemies)
					{
						this.KillAllPreviousEnemies();
					}
				}
			}
			else
			{
				CutsceneManager.PlayCutscene(this.cutscene, this.objects);
				if (this.killAllPreviousEnemies)
				{
					this.KillAllPreviousEnemies();
				}
			}
			this.played = true;
		}
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00038078 File Offset: 0x00036278
	private void KillAllPreviousEnemies()
	{
		BotAI[] array = (BotAI[])UnityEngine.Object.FindObjectsOfType(typeof(BotAI));
		foreach (BotAI botAI in array)
		{
			UnityEngine.Object.Destroy(botAI.gameObject);
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x000380C0 File Offset: 0x000362C0
	public void Update()
	{
		if (this.cam == null && Camera.main != null)
		{
			this.cam = Camera.main;
		}
		if (this.triggered && !this.played && this.cutscene.IsFinished)
		{
			CutsceneManager.ExitCutscene(true);
			this.played = true;
		}
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0003812C File Offset: 0x0003632C
	public void PlayEncounter()
	{
		this.cam.enabled = true;
		this.triggered = true;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00038144 File Offset: 0x00036344
	public void resumePlay()
	{
		CutsceneManager.ExitCutscene(false);
	}

	// Token: 0x040008C2 RID: 2242
	public CSComponent cutscene;

	// Token: 0x040008C3 RID: 2243
	public CutsceneTrigger.CutsceneType type;

	// Token: 0x040008C4 RID: 2244
	public GameObject objects;

	// Token: 0x040008C5 RID: 2245
	public bool played;

	// Token: 0x040008C6 RID: 2246
	public bool triggered;

	// Token: 0x040008C7 RID: 2247
	public bool forceColt;

	// Token: 0x040008C8 RID: 2248
	public bool killAllPreviousEnemies;

	// Token: 0x040008C9 RID: 2249
	public Camera cam;

	// Token: 0x0200013C RID: 316
	public enum CutsceneType
	{
		// Token: 0x040008CB RID: 2251
		CUTSCENE,
		// Token: 0x040008CC RID: 2252
		MELEE
	}
}
