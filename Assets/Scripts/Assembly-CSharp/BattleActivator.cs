using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
internal class BattleActivator : MonoBehaviour
{
	// Token: 0x06000614 RID: 1556 RVA: 0x0002CA0C File Offset: 0x0002AC0C
	private void Start()
	{
		this.player.BattleMode(this.enemy.gameObject);
		this.enemy.BattleMode(this.player.gameObject);
	}

	// Token: 0x040006D3 RID: 1747
	public FightingControl player;

	// Token: 0x040006D4 RID: 1748
	public FightingEnemy enemy;
}
