using System;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class Tangier_Fight : MonoBehaviour
{
	// Token: 0x06000873 RID: 2163 RVA: 0x00046100 File Offset: 0x00044300
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			this.playerCam.GetComponent<Camera>().enabled = false;
			this.playerCam.GetComponent<AudioListener>().enabled = false;
			this.fightCam.SetActive(true);
			this.thePlayer.SetActive(false);
			this.theEncounter.SetActive(true);
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00046168 File Offset: 0x00044368
	private void Update()
	{
	}

	// Token: 0x04000B3D RID: 2877
	public GameObject playerCam;

	// Token: 0x04000B3E RID: 2878
	public GameObject fightCam;

	// Token: 0x04000B3F RID: 2879
	public GameObject thePlayer;

	// Token: 0x04000B40 RID: 2880
	public GameObject theEncounter;
}
