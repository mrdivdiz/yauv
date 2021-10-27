using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class car2_check : MonoBehaviour
{
	// Token: 0x060008BA RID: 2234 RVA: 0x000490B0 File Offset: 0x000472B0
	private void Start()
	{
		this.block2StartingPosition = this.block2.transform.position;
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x000490C8 File Offset: 0x000472C8
	private void OnTriggerEnter()
	{
		if (!this.MovedForward)
		{
			this.sound.Play();
			iTween.MoveBy(this.tile2, iTween.Hash(new object[]
			{
				"Z",
				-0.03f,
				"time",
				1,
				"delay",
				0
			}));
			this.block2.transform.position = this.block2StartingPosition;
			iTween.MoveBy(this.block2, iTween.Hash(new object[]
			{
				"y",
				-11.7f,
				"time",
				4,
				"delay",
				0
			}));
			this.tilesound.Play();
			this.PlayerCam.enabled = false;
			this.cutscene1.StartCutscene();
			CutsceneManager.showGUI = false;
			WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
			if (weaponsHUD != null)
			{
				weaponsHUD.enabled = false;
			}
			if (PlatformCharacterController.joystickLeft != null)
			{
				PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
			}
			this.MovedForward = true;
			this.PreventCar();
			base.Invoke("AllowCar", 4f);
			this.col2.GetComponent<Collider>().isTrigger = true;
		}
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0004923C File Offset: 0x0004743C
	private void OnTriggerExit()
	{
		this.sound.Play();
		iTween.MoveBy(this.tile2, iTween.Hash(new object[]
		{
			"Z",
			0.03f,
			"time",
			1,
			"delay",
			0
		}));
		iTween.MoveBy(this.block2, iTween.Hash(new object[]
		{
			"y",
			11.7f,
			"time",
			4,
			"delay",
			0
		}));
		this.tilesound.Play();
		this.MovedForward = false;
		this.col2.GetComponent<Collider>().isTrigger = false;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00049314 File Offset: 0x00047514
	private void AllowCar()
	{
		this.rcCarScript.disabled = false;
		CutsceneManager.showGUI = true;
		WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
		if (weaponsHUD != null)
		{
			weaponsHUD.enabled = true;
		}
		if (PlatformCharacterController.joystickLeft != null)
		{
			PlatformCharacterController.joystickLeft.gameObject.SetActive(true);
		}
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0004937C File Offset: 0x0004757C
	private void PreventCar()
	{
		this.rcCarScript.disabled = true;
	}

	// Token: 0x04000BB6 RID: 2998
	public GameObject block2;

	// Token: 0x04000BB7 RID: 2999
	public GameObject tile2;

	// Token: 0x04000BB8 RID: 3000
	public AudioSource sound;

	// Token: 0x04000BB9 RID: 3001
	public AudioSource tilesound;

	// Token: 0x04000BBA RID: 3002
	public ShooterGameCamera PlayerCam;

	// Token: 0x04000BBB RID: 3003
	private bool MovedForward;

	// Token: 0x04000BBC RID: 3004
	private bool MovedBackward;

	// Token: 0x04000BBD RID: 3005
	public RCcar rcCarScript;

	// Token: 0x04000BBE RID: 3006
	public GameObject col2;

	// Token: 0x04000BBF RID: 3007
	private Vector3 block2StartingPosition;

	// Token: 0x04000BC0 RID: 3008
	public CSComponent cutscene1;
}
