using System;
using Antares.Cutscene.Runtime;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class car1_check : MonoBehaviour
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x00048DCC File Offset: 0x00046FCC
	private void Start()
	{
		this.block1StartingPosition = this.block1.transform.position;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00048DE4 File Offset: 0x00046FE4
	private void OnTriggerEnter()
	{
		if (!this.MovedForward)
		{
			this.sound.Play();
			iTween.MoveBy(this.tile1, iTween.Hash(new object[]
			{
				"Z",
				-0.03f,
				"time",
				1,
				"delay",
				0
			}));
			this.block1.transform.position = this.block1StartingPosition;
			iTween.MoveBy(this.block1, iTween.Hash(new object[]
			{
				"y",
				-9.7f,
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
			this.col1.GetComponent<Collider>().isTrigger = true;
		}
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00048F58 File Offset: 0x00047158
	private void OnTriggerExit()
	{
		this.sound.Play();
		iTween.MoveBy(this.tile1, iTween.Hash(new object[]
		{
			"Z",
			0.03f,
			"time",
			1,
			"delay",
			0
		}));
		iTween.MoveBy(this.block1, iTween.Hash(new object[]
		{
			"y",
			9.7f,
			"time",
			4,
			"delay",
			0
		}));
		this.tilesound.Play();
		this.MovedForward = false;
		this.col1.GetComponent<Collider>().isTrigger = false;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00049030 File Offset: 0x00047230
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

	// Token: 0x060008B8 RID: 2232 RVA: 0x00049098 File Offset: 0x00047298
	private void PreventCar()
	{
		this.rcCarScript.disabled = true;
	}

	// Token: 0x04000BAB RID: 2987
	public GameObject block1;

	// Token: 0x04000BAC RID: 2988
	public GameObject tile1;

	// Token: 0x04000BAD RID: 2989
	public AudioSource sound;

	// Token: 0x04000BAE RID: 2990
	public AudioSource tilesound;

	// Token: 0x04000BAF RID: 2991
	public ShooterGameCamera PlayerCam;

	// Token: 0x04000BB0 RID: 2992
	private bool MovedForward;

	// Token: 0x04000BB1 RID: 2993
	private bool MovedBackward;

	// Token: 0x04000BB2 RID: 2994
	public RCcar rcCarScript;

	// Token: 0x04000BB3 RID: 2995
	public GameObject col1;

	// Token: 0x04000BB4 RID: 2996
	public CSComponent cutscene1;

	// Token: 0x04000BB5 RID: 2997
	private Vector3 block1StartingPosition;
}
