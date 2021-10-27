using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class OpenElevator : MonoBehaviour
{
	// Token: 0x060007EF RID: 2031 RVA: 0x00041144 File Offset: 0x0003F344
	private void Start()
	{
		this.bgMusic.start = true;
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00041154 File Offset: 0x0003F354
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (((mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0)) && collisionInfo.tag == "Player" && !this.entered)
		{
			iTween.MoveBy(this.LeftDoor, iTween.Hash(new object[]
			{
				"y",
				-0.72,
				"time",
				3,
				"delay",
				1.6
			}));
			iTween.MoveBy(this.RightDoor, iTween.Hash(new object[]
			{
				"y",
				0.72,
				"time",
				3,
				"delay",
				1.6
			}));
			this.elevatorSound.Play();
			this.entered = true;
		}
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00041270 File Offset: 0x0003F470
	private void Update()
	{
		if (OpenElevator.GruntPrologueDied)
		{
			Spawner spawner = (Spawner)UnityEngine.Object.FindObjectOfType(typeof(Spawner));
			if (spawner != null)
			{
				base.StartCoroutine(spawner.Spawn("A1", 1f));
			}
			base.Invoke("ShowBlindFireHint", 3.5f);
			iTween.MoveBy(this.LeftDoor, iTween.Hash(new object[]
			{
				"y",
				-0.72,
				"time",
				3,
				"delay",
				1.6
			}));
			iTween.MoveBy(this.RightDoor, iTween.Hash(new object[]
			{
				"y",
				0.72,
				"time",
				3,
				"delay",
				1.6
			}));
			this.elevatorSound.Play();
			this.entered = true;
			OpenElevator.GruntPrologueDied = false;
		}
		if (OpenElevator.ElevatorManDied)
		{
			base.Invoke("ShowExitCoverHint", 2f);
			OpenElevator.ElevatorManDied = false;
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000413BC File Offset: 0x0003F5BC
	private void ShowBlindFireHint()
	{
		if (this.blindFireHintObject != null)
		{
			this.blindFireHintObject.SetActive(true);
		}
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000413DC File Offset: 0x0003F5DC
	private void ShowExitCoverHint()
	{
		if (this.ExitCoverHintObject != null)
		{
			this.ExitCoverHintObject.SetActive(true);
		}
	}

	// Token: 0x04000A75 RID: 2677
	public GameObject LeftDoor;

	// Token: 0x04000A76 RID: 2678
	public GameObject RightDoor;

	// Token: 0x04000A77 RID: 2679
	public AudioSource elevatorSound;

	// Token: 0x04000A78 RID: 2680
	public BackgroundMusic bgMusic;

	// Token: 0x04000A79 RID: 2681
	public bool entered;

	// Token: 0x04000A7A RID: 2682
	public static bool GruntPrologueDied;

	// Token: 0x04000A7B RID: 2683
	public static bool ElevatorManDied;

	// Token: 0x04000A7C RID: 2684
	public GameObject blindFireHintObject;

	// Token: 0x04000A7D RID: 2685
	public GameObject ExitCoverHintObject;
}
