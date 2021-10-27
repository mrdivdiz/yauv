using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class FindWhoReferences
{
	// Token: 0x060001AB RID: 427 RVA: 0x0000E220 File Offset: 0x0000C420
	private static List<Component> FindObjectsReferencing<T>(T mb) where T : Component
	{
		List<Component> list = new List<Component>();
		Component[] array = Resources.FindObjectsOfTypeAll(typeof(Component)) as Component[];
		if (array == null)
		{
			return list;
		}
		foreach (Component component in array)
		{
			FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (FindWhoReferences.FieldReferencesComponent<T>(component, fieldInfo, mb))
				{
					list.Add(component);
					Debug.Log(string.Concat(new object[]
					{
						"Ref: Component ",
						component.GetType(),
						" from Object ",
						component.name
					}));
				}
			}
		}
		return list;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000E2EC File Offset: 0x0000C4EC
	private static bool FieldReferencesComponent<T>(Component obj, FieldInfo fieldInfo, T mb) where T : Component
	{
		if (fieldInfo.FieldType.IsArray)
		{
			Array array = fieldInfo.GetValue(obj) as Array;
			foreach (object obj2 in array)
			{
				if (obj2.GetType() == mb.GetType())
				{
					T t = obj2 as T;
					if (t == mb)
					{
						return true;
					}
				}
			}
			return false;
		}
		if (fieldInfo.FieldType == mb.GetType())
		{
			T t2 = fieldInfo.GetValue(obj) as T;
			if (t2 == mb)
			{
				return true;
			}
		}
		return false;
	}
}
