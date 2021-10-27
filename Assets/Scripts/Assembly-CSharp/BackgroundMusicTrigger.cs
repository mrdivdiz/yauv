using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class BackgroundMusicTrigger : MonoBehaviour
{
	// Token: 0x060006C9 RID: 1737 RVA: 0x00036E80 File Offset: 0x00035080
	private void Start()
	{
		if (this.MusicPlayer == null)
		{
			this.MusicPlayer = (BackgroundMusic)UnityEngine.Object.FindObjectOfType(typeof(BackgroundMusic));
		}
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x00036EB0 File Offset: 0x000350B0
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if (collisionInfo.tag == "Player")
		{
			if (!this.MusicPlayer.started)
			{
				this.MusicPlayer.currentAudioClip = 2;
				this.MusicPlayer.started = true;
			}
			else
			{
				this.MusicPlayer.PlayNextAudioClip();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040008A6 RID: 2214
	public BackgroundMusic MusicPlayer;
}
