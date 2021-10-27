using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class FixedCameraHint : MonoBehaviour
{
	// Token: 0x06000622 RID: 1570 RVA: 0x0002CBF4 File Offset: 0x0002ADF4
	private void Awake()
	{
		this.cam = Camera.main.GetComponent<ShooterGameCamera>();
		this.dof = Camera.main.GetComponent<DepthOfField34>();
		FixedCameraHint.sawHint = false;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002CC28 File Offset: 0x0002AE28
	private void Start()
	{
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002CC2C File Offset: 0x0002AE2C
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !FixedCameraHint.sawHint && Inventory.enableHinting && !GotUp.DidGetUp)
		{
			this.cam.player = this.Obelisk.transform;
			this.cam.cameraPosition = this.CameraPosition;
			this.cam.inTransitionTime = this.inTransitionTime;
			this.cam.outTransitionTime = this.outTransitionTime;
			this.cam.fixedCameraPosition = true;
			this.cam.GetComponent<Camera>().fieldOfView = 60f;
			if (this.dof != null)
			{
				this.dof.enabled = true;
			}
			AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = false;
			AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = false;
			AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
			CutsceneManager.showGUI = false;
			FixedCameraHint.sawHint = true;
			base.Invoke("EndDOF", 8f);
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002CD50 File Offset: 0x0002AF50
	public void EndDOF()
	{
		this.cam.fixedCameraPosition = false;
		this.cam.lookAtTarget = true;
		this.cam.player = this.FarisHead.transform;
		this.cam.GetComponent<Camera>().fieldOfView = 45f;
		if (this.dof != null)
		{
			this.dof.enabled = false;
		}
		AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = true;
		AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = true;
		AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = false;
		CutsceneManager.showGUI = true;
	}

	// Token: 0x040006DC RID: 1756
	public Transform CameraPosition;

	// Token: 0x040006DD RID: 1757
	public float inTransitionTime = 1f;

	// Token: 0x040006DE RID: 1758
	public float outTransitionTime = 1f;

	// Token: 0x040006DF RID: 1759
	public GameObject Obelisk;

	// Token: 0x040006E0 RID: 1760
	public GameObject FarisHead;

	// Token: 0x040006E1 RID: 1761
	private ShooterGameCamera cam;

	// Token: 0x040006E2 RID: 1762
	private DepthOfField34 dof;

	// Token: 0x040006E3 RID: 1763
	public static bool sawHint;
}
