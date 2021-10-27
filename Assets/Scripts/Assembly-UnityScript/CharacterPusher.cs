using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
[Serializable]
public class CharacterPusher : MonoBehaviour
{
	// Token: 0x0600008E RID: 142 RVA: 0x00003A48 File Offset: 0x00001C48
	public CharacterPusher()
	{
		this.pushPower = 2f;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00003A5C File Offset: 0x00001C5C
	public virtual void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (!(attachedRigidbody == null) && !attachedRigidbody.isKinematic)
		{
			if (hit.moveDirection.y >= -0.3f)
			{
				Vector3 a = new Vector3(hit.moveDirection.x, (float)0, hit.moveDirection.z);
				attachedRigidbody.velocity = a * this.pushPower;
			}
		}
	}

	// Token: 0x0400004B RID: 75
	public float pushPower;
}
