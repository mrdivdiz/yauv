using System;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class EventParameter
	{
		// Token: 0x04000003 RID: 3
		public EventParameterType parameterType = EventParameterType.String;

		// Token: 0x04000004 RID: 4
		public UnityEngine.Object objValue;

		// Token: 0x04000005 RID: 5
		public string stringValue;

		// Token: 0x04000006 RID: 6
		public bool boolValue;

		// Token: 0x04000007 RID: 7
		public int integerValue;

		// Token: 0x04000008 RID: 8
		public float floatValue;
	}
}
