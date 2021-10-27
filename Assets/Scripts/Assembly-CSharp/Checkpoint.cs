using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class Checkpoint : MonoBehaviour
{
	// Token: 0x060006D3 RID: 1747 RVA: 0x00036F9C File Offset: 0x0003519C
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			GunManager componentInChildren = collisionInfo.GetComponentInChildren<GunManager>();
			//if (SaveHandler.checkpointReached == this.checkpointNo - 1)
			//{
				if (componentInChildren != null)
				{
					SaveHandler.SaveCheckpoint(this.levelNo, this.checkpointNo, collisionInfo.transform.position, collisionInfo.transform.rotation.eulerAngles, (!(componentInChildren.currentSecondaryGun != null)) ? string.Empty : componentInChildren.currentSecondaryGun.gunName, (!(componentInChildren.currentPrimaryGun != null)) ? string.Empty : componentInChildren.currentPrimaryGun.gunName, (!(componentInChildren.currentSecondaryGun != null)) ? 0 : componentInChildren.currentSecondaryGun.totalClips, (!(componentInChildren.currentSecondaryGun != null)) ? 0 : componentInChildren.currentSecondaryGun.currentRounds, (!(componentInChildren.currentPrimaryGun != null)) ? 0 : componentInChildren.currentPrimaryGun.totalClips, (!(componentInChildren.currentPrimaryGun != null)) ? 0 : componentInChildren.currentPrimaryGun.currentRounds, componentInChildren.currentGrenades);
				}
				else
				{
					SaveHandler.SaveCheckpoint(this.levelNo, this.checkpointNo, collisionInfo.transform.position, collisionInfo.transform.rotation.eulerAngles, string.Empty, string.Empty, 0, 0, 0, 0, 0);
				}
				SpeechManager.displayCheckpointReached = 4f;
				SpeechManager.PlayCheckpointSound();
			//}
			if (mainmenu.replayLevel && SaveHandler.replayCheckpointReached == this.checkpointNo - 1)
			{
				if (componentInChildren != null)
				{
					SaveHandler.SaveCheckpointOnReplay(this.checkpointNo, collisionInfo.transform.position, collisionInfo.transform.rotation.eulerAngles, (!(componentInChildren.currentSecondaryGun != null)) ? string.Empty : componentInChildren.currentSecondaryGun.gunName, (!(componentInChildren.currentPrimaryGun != null)) ? string.Empty : componentInChildren.currentPrimaryGun.gunName, (!(componentInChildren.currentSecondaryGun != null)) ? 0 : componentInChildren.currentSecondaryGun.totalClips, (!(componentInChildren.currentSecondaryGun != null)) ? 0 : componentInChildren.currentSecondaryGun.currentRounds, (!(componentInChildren.currentPrimaryGun != null)) ? 0 : componentInChildren.currentPrimaryGun.totalClips, (!(componentInChildren.currentPrimaryGun != null)) ? 0 : componentInChildren.currentPrimaryGun.currentRounds, componentInChildren.currentGrenades);
				}
				else
				{
					SaveHandler.SaveCheckpointOnReplay(this.checkpointNo, collisionInfo.transform.position, collisionInfo.transform.rotation.eulerAngles, string.Empty, string.Empty, 0, 0, 0, 0, 0);
				}
				SpeechManager.displayCheckpointReached = 4f;
				SpeechManager.PlayCheckpointSound();
			}
		}
		PlayerPrefs.Save();
		Debug.Log("LR " + " + CN ");
	}

	// Token: 0x040008A7 RID: 2215
	public int levelNo;

	// Token: 0x040008A8 RID: 2216
	public int checkpointNo;
}
