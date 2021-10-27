using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000027 RID: 39
	public interface ICutsceneLocalization
	{
		// Token: 0x060001AD RID: 429
		string GetText(string keyword);

		// Token: 0x060001AE RID: 430
		Font GetCurrentLanguageFont();

		// Token: 0x060001AF RID: 431
		string GetCurrentLanguage();
	}
}
