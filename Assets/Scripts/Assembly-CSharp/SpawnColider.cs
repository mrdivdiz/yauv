using System;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class SpawnColider : MonoBehaviour
{
	// Token: 0x060005D7 RID: 1495 RVA: 0x00028B18 File Offset: 0x00026D18
	private void Awake()
	{
		if (this.spawner == null)
		{
			this.spawner = (Spawner)UnityEngine.Object.FindObjectOfType(typeof(Spawner));
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x00028B48 File Offset: 0x00026D48
	private void OnDestroy()
	{
		this.spawner = null;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00028B54 File Offset: 0x00026D54
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((!this.onStartOnly || (mainmenu.replayLevel && SaveHandler.replayCheckpointReached == 0) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == 0)) && (!this.checkLastCheckpointReached || (mainmenu.replayLevel && SaveHandler.replayCheckpointReached == this.lastCheckpointReached) || (!mainmenu.replayLevel && SaveHandler.checkpointReached == this.lastCheckpointReached)) && collisionInfo.tag == "Player" && !this.used)
		{
			if (this.enterStealthMode)
			{
				AnimationHandler.instance.stealthMode = true;
			}
			if (this.groupID != string.Empty)
			{
				if (this.delay == 0f)
				{
					this.spawner.Spawn(this.groupID);
				}
				else
				{
					base.StartCoroutine(this.spawner.Spawn(this.groupID, this.delay));
				}
			}
			this.used = true;
		}
	}

	// Token: 0x04000675 RID: 1653
	public string groupID;

	// Token: 0x04000676 RID: 1654
	private bool used;

	// Token: 0x04000677 RID: 1655
	public bool enterStealthMode;

	// Token: 0x04000678 RID: 1656
	public float delay;

	// Token: 0x04000679 RID: 1657
	public bool onStartOnly;

	// Token: 0x0400067A RID: 1658
	public bool checkLastCheckpointReached;

	// Token: 0x0400067B RID: 1659
	public int lastCheckpointReached;

	// Token: 0x0400067C RID: 1660
	public Spawner spawner;
}
