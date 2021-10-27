using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class RepeatableBlocks : MonoBehaviour
{/*
	// Token: 0x0600066E RID: 1646 RVA: 0x000319C8 File Offset: 0x0002FBC8
	private void Awake()
	{
		RepeatableBlocks.lastMoveTime = 0f;
		RepeatableBlocks.currentlyMoving = false;
		RepeatableBlocks.blocks = new RepeatableBlocks[3, 3];
		RepeatableBlocks.tempBlocks = new RepeatableBlocks[3, 3];
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00031A00 File Offset: 0x0002FC00
	private void OnDestroy()
	{
		RepeatableBlocks.blocks = null;
		RepeatableBlocks.tempBlocks = null;
		RepeatableBlocks.lastColliderEntered = null;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00031A14 File Offset: 0x0002FC14
	private void Start()
	{
		RepeatableBlocks.blocks[this.row, this.col] = this;
		this.blockWidth *= base.transform.parent.transform.localScale.x;
		this.blockHeight *= base.transform.parent.transform.localScale.z;
		//this.naveMesh = GameObject.FindGameObjectWithTag("NavMesh").transform;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00031AA4 File Offset: 0x0002FCA4
	private IEnumerator OnTriggerStay(Collider other)
	{
		yield return new WaitForSeconds(2f);
		if (other.gameObject.tag != "PlayerCar")
		{
			yield break;
		}
		if (RepeatableBlocks.lastColliderEntered == this || Time.time < RepeatableBlocks.lastMoveTime + 5f || RepeatableBlocks.currentlyMoving)
		{
			yield break;
		}
		RepeatableBlocks.lastColliderEntered = this;
		RepeatableBlocks.lastMoveTime = Time.time;
		RepeatableBlocks.currentlyMoving = true;
		switch (this.row)
		{
		case 0:
			switch (this.col)
			{
			case 0:
				//this.naveMesh.Translate(this.blockHeight, 0f, this.blockHeight);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[2, 1];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[1, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				this.copyTempBlocks();
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[2, 1];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[1, 2];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 0].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 1].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 2].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				this.copyTempBlocks();
				break;
			case 1:
				//this.naveMesh.Translate(this.blockHeight, 0f, 0f);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[2, 1];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[1, 2];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 0].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 1].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 2].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				this.copyTempBlocks();
				break;
			case 2:
				//this.naveMesh.Translate(this.blockHeight, 0f, -this.blockHeight);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[2, 1];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[1, 2];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 0].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 1].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 2].transform.parent.Translate(3f * this.blockHeight, 0f, 0f);
				this.copyTempBlocks();
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[2, 1];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[1, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				this.copyTempBlocks();
				break;
			}
			break;
		case 1:
			switch (this.col)
			{
			case 0:
				//this.naveMesh.Translate(0f, 0f, this.blockHeight);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[2, 1];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[1, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				this.copyTempBlocks();
				break;
			case 2:
				//this.naveMesh.Translate(0f, 0f, -this.blockHeight);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[2, 1];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[1, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				this.copyTempBlocks();
				break;
			}
			break;
		case 2:
			switch (this.col)
			{
			case 0:
				//this.naveMesh.Translate(-this.blockHeight, 0f, this.blockHeight);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[2, 1];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[1, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 0].transform.parent.Translate(0f, 0f, 3f * this.blockWidth);
				this.copyTempBlocks();
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[2, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[2, 2];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 0].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 1].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 2].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				this.copyTempBlocks();
				break;
			case 1:
				//this.naveMesh.Translate(-this.blockHeight, 0f, 0f);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[2, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[2, 2];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 0].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 1].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 2].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				this.copyTempBlocks();
				break;
			case 2:
				//this.naveMesh.Translate(-this.blockHeight, 0f, -this.blockHeight);
				this.RecalculatePath();
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[2, 2];
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[2, 1];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[0, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[1, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 2].transform.parent.Translate(0f, 0f, -3f * this.blockWidth);
				this.copyTempBlocks();
				RepeatableBlocks.tempBlocks[2, 0] = RepeatableBlocks.blocks[0, 0];
				RepeatableBlocks.tempBlocks[2, 1] = RepeatableBlocks.blocks[0, 1];
				RepeatableBlocks.tempBlocks[2, 2] = RepeatableBlocks.blocks[0, 2];
				RepeatableBlocks.tempBlocks[0, 0] = RepeatableBlocks.blocks[1, 0];
				RepeatableBlocks.tempBlocks[0, 1] = RepeatableBlocks.blocks[1, 1];
				RepeatableBlocks.tempBlocks[0, 2] = RepeatableBlocks.blocks[1, 2];
				RepeatableBlocks.tempBlocks[1, 0] = RepeatableBlocks.blocks[2, 0];
				RepeatableBlocks.tempBlocks[1, 1] = RepeatableBlocks.blocks[2, 1];
				RepeatableBlocks.tempBlocks[1, 2] = RepeatableBlocks.blocks[2, 2];
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 0].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 1].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				yield return new WaitForSeconds(3f);
				RepeatableBlocks.tempBlocks[2, 2].transform.parent.Translate(-3f * this.blockHeight, 0f, 0f);
				this.copyTempBlocks();
				break;
			}
			break;
		}
		RepeatableBlocks.currentlyMoving = false;
		yield break;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00031AD0 File Offset: 0x0002FCD0
	private void copyTempBlocks()
	{
		RepeatableBlocks.tempBlocks[0, 0].row = 0;
		RepeatableBlocks.tempBlocks[0, 0].col = 0;
		RepeatableBlocks.tempBlocks[0, 1].row = 0;
		RepeatableBlocks.tempBlocks[0, 1].col = 1;
		RepeatableBlocks.tempBlocks[0, 2].row = 0;
		RepeatableBlocks.tempBlocks[0, 2].col = 2;
		RepeatableBlocks.tempBlocks[1, 0].row = 1;
		RepeatableBlocks.tempBlocks[1, 0].col = 0;
		RepeatableBlocks.tempBlocks[1, 1].row = 1;
		RepeatableBlocks.tempBlocks[1, 1].col = 1;
		RepeatableBlocks.tempBlocks[1, 2].row = 1;
		RepeatableBlocks.tempBlocks[1, 2].col = 2;
		RepeatableBlocks.tempBlocks[2, 0].row = 2;
		RepeatableBlocks.tempBlocks[2, 0].col = 0;
		RepeatableBlocks.tempBlocks[2, 1].row = 2;
		RepeatableBlocks.tempBlocks[2, 1].col = 1;
		RepeatableBlocks.tempBlocks[2, 2].row = 2;
		RepeatableBlocks.tempBlocks[2, 2].col = 2;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				RepeatableBlocks.blocks[i, j] = RepeatableBlocks.tempBlocks[i, j];
			}
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00031C60 File Offset: 0x0002FE60
	private void RecalculatePath()
	{
		float num = 0f;
		foreach (PoliceCar policeCar in UnityEngine.Object.FindObjectsOfType(typeof(PoliceCar)))
		{
			policeCar.waitBeforeRecalculating = 0.5f + num * 0.5f;
			policeCar.recalculatePath = true;
			num += 1f;
		}
	}
*/
	// Token: 0x040007B4 RID: 1972
	public static RepeatableBlocks[,] blocks = new RepeatableBlocks[3, 3];

	// Token: 0x040007B5 RID: 1973
	public static RepeatableBlocks[,] tempBlocks = new RepeatableBlocks[3, 3];

	// Token: 0x040007B6 RID: 1974
	public static RepeatableBlocks lastColliderEntered;

	// Token: 0x040007B7 RID: 1975
	public int row;

	// Token: 0x040007B8 RID: 1976
	public int col;

	// Token: 0x040007B9 RID: 1977
	public float blockWidth = 80f;

	// Token: 0x040007BA RID: 1978
	public float blockHeight = 80.5f;

	// Token: 0x040007BB RID: 1979
	//private Transform naveMesh;

	// Token: 0x040007BC RID: 1980
	private static float lastMoveTime = 0f;

	// Token: 0x040007BD RID: 1981
	private static bool currentlyMoving = false;
}
