using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class SpawnColider1 : MonoBehaviour
{
	// Token: 0x060005DB RID: 1499 RVA: 0x00028C74 File Offset: 0x00026E74
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
					SpawnerRes.Spawn(this.groupID);
				}
				else
				{
					base.StartCoroutine(SpawnerRes.Spawn(this.groupID, this.delay));
				}
			}
			this.used = true;
		}
	}

	// Token: 0x0400067D RID: 1661
	public string groupID;

	// Token: 0x0400067E RID: 1662
	private bool used;

	// Token: 0x0400067F RID: 1663
	public bool enterStealthMode;

	// Token: 0x04000680 RID: 1664
	public float delay;

	// Token: 0x04000681 RID: 1665
	public bool onStartOnly;

	// Token: 0x04000682 RID: 1666
	public bool checkLastCheckpointReached;

	// Token: 0x04000683 RID: 1667
	public int lastCheckpointReached;
}
