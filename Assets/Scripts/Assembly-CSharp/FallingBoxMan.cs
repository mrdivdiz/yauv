using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class FallingBoxMan : MonoBehaviour
{
	// Token: 0x060005C5 RID: 1477 RVA: 0x00027B68 File Offset: 0x00025D68
	private void Start()
	{
		base.GetComponent<Animation>()["Take 001"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Take 001");
		this.startPosition = base.transform.position;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00027BB0 File Offset: 0x00025DB0
	private void Update()
	{
		if (Timer.cutsceneFinishTime == -1f)
		{
			return;
		}
		float num = Mathf.Clamp01((Time.timeSinceLevelLoad - Timer.cutsceneFinishTime) / this.duration);
		if (num < 1f)
		{
			base.transform.position = Vector3.Lerp(this.startPosition, this.targetPosition.position, num);
		}
		else
		{
			UnityEngine.Object.Instantiate(this.FallingBoxesManPrefab, this.targetPosition.position + this.positionAdjustment, this.targetPosition.rotation);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000649 RID: 1609
	private float duration = 2.6f;

	// Token: 0x0400064A RID: 1610
	public Transform targetPosition;

	// Token: 0x0400064B RID: 1611
	public Transform FallingBoxesManPrefab;

	// Token: 0x0400064C RID: 1612
	private Vector3 positionAdjustment = new Vector3(0.0257f, 0f, -0.0129f);

	// Token: 0x0400064D RID: 1613
	private Vector3 startPosition;
}
