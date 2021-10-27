using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
internal class BattleDeactivator : MonoBehaviour
{
	// Token: 0x06000616 RID: 1558 RVA: 0x0002CA50 File Offset: 0x0002AC50
	private void Deactivate()
	{
		this.player.BattleMode();
		this.enemy.BattleMode();
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0002CA68 File Offset: 0x0002AC68
	private void Start()
	{
		base.Invoke("Deactivate", 5f);
	}

	// Token: 0x040006D5 RID: 1749
	public FightingControl player;

	// Token: 0x040006D6 RID: 1750
	public FightingEnemy enemy;
}
