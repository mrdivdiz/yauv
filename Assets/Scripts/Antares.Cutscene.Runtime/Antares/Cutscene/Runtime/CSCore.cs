using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000012 RID: 18
	public class CSCore
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000037B0 File Offset: 0x000019B0
		internal static Type a(string A_0)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (type.Name.Equals(A_0, StringComparison.CurrentCultureIgnoreCase) || type.Name.Contains('+' + A_0))
					{
						return type;
					}
				}
			}
			return null;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000383C File Offset: 0x00001A3C
		public static List<Type> GetInheritedTypeFromAllAssemblies(Type baseClassType)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						if (type.IsSubclassOf(baseClassType))
						{
							list.Add(type);
						}
					}
				}
				catch
				{
				}
			}
			return list;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000038C4 File Offset: 0x00001AC4
		internal static void a(UnityEngine.Object A_0)
		{
			if (!Application.isPlaying)
			{
				if (CSCore.blist == null)
				{
					CSCore.blist= new List<UnityEngine.Object>();
				}
				if (!CSCore.blist.Contains(A_0))
				{
					CSCore.blist.Add(A_0);
					return;
				}
			}
			else
			{
				CSCore.blist = null;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003900 File Offset: 0x00001B00
		public static UnityEngine.Object[] SubmitChanges()
		{
			if (Application.isPlaying)
			{
				return null;
			}
			if (CSCore.blist == null)
			{
				CSCore.blist = new List<UnityEngine.Object>();
			}
			CSCore.blist.Remove(null);
			UnityEngine.Object[] result = CSCore.blist.ToArray();
			CSCore.blist.Clear();
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003949 File Offset: 0x00001B49
		internal static float a(float A_0)
		{
			return (float)Mathf.RoundToInt(A_0 * 10f) / 10f;
		}

		// Token: 0x0400004A RID: 74
		//private const StringComparison a = StringComparison.CurrentCultureIgnoreCase;

		// Token: 0x0400004B RID: 75
		internal static List<UnityEngine.Object> blist;
	}
}
