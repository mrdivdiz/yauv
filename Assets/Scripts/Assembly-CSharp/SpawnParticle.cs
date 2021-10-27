using System;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class SpawnParticle : MonoBehaviour
{
	// Token: 0x06000BBA RID: 3002 RVA: 0x00094D64 File Offset: 0x00092F64
	public void Spawnparticle()
	{
		Transform transform = UnityEngine.Object.Instantiate(this.explosion, base.transform.position, Quaternion.identity) as Transform;
		transform.parent = base.transform;
	}

	// Token: 0x040015A3 RID: 5539
	public Transform explosion;
}
