using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class MummyRoomGameplay : MonoBehaviour
{
	// Token: 0x060007DB RID: 2011 RVA: 0x00040CBC File Offset: 0x0003EEBC
	private void Update()
	{
		if (MummyRoomGameplay.PiecesCounter == 4 && !this.cutScenePlayed)
		{
			CutsceneManager.PlayCutscene(this.Puzzle_Solved, null);
			this.Box.SetActive(true);
			this.cutScenePlayed = true;
			MonoBehaviour.print("started");
		}
		if (this.inside)
		{
			if (InputManager.GetButtonDown("Interaction") && base.gameObject.name == "Bottom_Right")
			{
				iTween.RotateTo(this.AxeBR, iTween.Hash(new object[]
				{
					"z",
					30f,
					"time",
					3.5,
					"delay",
					0
				}));
				this.audioSource.Play();
				MummyRoomGameplay.PiecesCounter++;
			}
			if (InputManager.GetButtonDown("Interaction") && base.gameObject.name == "Bottom_Left")
			{
				iTween.RotateTo(this.AxeBL, iTween.Hash(new object[]
				{
					"z",
					30f,
					"time",
					3.5,
					"delay",
					0
				}));
				this.audioSource.Play();
				MummyRoomGameplay.PiecesCounter++;
			}
			if (InputManager.GetButtonDown("Interaction") && base.gameObject.name == "Top_Right")
			{
				iTween.RotateTo(this.AxeTR, iTween.Hash(new object[]
				{
					"z",
					-30f,
					"time",
					3.5,
					"delay",
					0
				}));
				this.audioSource.Play();
				MummyRoomGameplay.PiecesCounter++;
			}
			if (InputManager.GetButtonDown("Interaction") && base.gameObject.name == "Top_Left")
			{
				iTween.RotateTo(this.AxeTL, iTween.Hash(new object[]
				{
					"z",
					30f,
					"time",
					3.5,
					"delay",
					0
				}));
				this.audioSource.Play();
				MummyRoomGameplay.PiecesCounter++;
			}
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00040F68 File Offset: 0x0003F168
	private void OnTriggerEnter()
	{
		this.inside = true;
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00040F74 File Offset: 0x0003F174
	private void OnTriggerExit()
	{
		this.inside = false;
	}

	// Token: 0x04000A68 RID: 2664
	public GameObject AxeBR;

	// Token: 0x04000A69 RID: 2665
	public GameObject AxeBL;

	// Token: 0x04000A6A RID: 2666
	public GameObject AxeTR;

	// Token: 0x04000A6B RID: 2667
	public GameObject AxeTL;

	// Token: 0x04000A6C RID: 2668
	public AudioSource audioSource;

	// Token: 0x04000A6D RID: 2669
	public static int PiecesCounter;

	// Token: 0x04000A6E RID: 2670
	public CSComponent Puzzle_Solved;

	// Token: 0x04000A6F RID: 2671
	private bool inside;

	// Token: 0x04000A70 RID: 2672
	private bool cutScenePlayed;

	// Token: 0x04000A71 RID: 2673
	public GameObject Box;
}
