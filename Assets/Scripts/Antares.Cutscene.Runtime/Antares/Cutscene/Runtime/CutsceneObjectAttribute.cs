using System;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Class)]
	public class CutsceneObjectAttribute : Attribute
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020C4 File Offset: 0x000002C4
		public CutsceneObjectAttribute(string objectName, params Type[] supportedTypes)
		{
			this.supportedTypes = supportedTypes;
			this.objectName = objectName;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020DC File Offset: 0x000002DC
		public bool IsTypeSupported(Type testType)
		{
			foreach (Type type in this.supportedTypes)
			{
				if (testType.IsSubclassOf(type) || type == testType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000018 RID: 24
		public readonly string objectName;

		// Token: 0x04000019 RID: 25
		public readonly Type[] supportedTypes;
	}
}
