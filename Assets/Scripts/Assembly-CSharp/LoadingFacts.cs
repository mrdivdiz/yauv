using System;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class LoadingFacts : MonoBehaviour
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x00040A74 File Offset: 0x0003EC74
	private void Start()
	{
		this.currentFact = UnityEngine.Random.Range(0, this.facts.Length - 1);
		this.ShowHint();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00040A94 File Offset: 0x0003EC94
	private void ShowHint()
	{
		if (this.facts.Length > 0)
		{
			this.currentFact = (this.currentFact + 1) % this.facts.Length;
			FactsViewer.ShowFact(Language.Get(this.facts[this.currentFact].hintKeyword, 53), this.facts[this.currentFact].size, this.facts[this.currentFact].scale, 10000f);
		}
	}

	// Token: 0x04000A5F RID: 2655
	public Fact[] facts;

	// Token: 0x04000A60 RID: 2656
	private float time = 10f;

	// Token: 0x04000A61 RID: 2657
	private int currentFact;
}
