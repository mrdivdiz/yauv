using System;
using System.Text;
using UnityEngine;

namespace Prime31
{
	// Token: 0x02000004 RID: 4
	public static class Utils
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000026D7 File Offset: 0x000008D7
		private static System.Random random
		{
			get
			{
				if (Utils._random == null)
				{
					Utils._random = new System.Random();
				}
				return Utils._random;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000026F4 File Offset: 0x000008F4
		public static string randomString(int size = 38)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				char value = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * Utils.random.NextDouble() + 65.0)));
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002758 File Offset: 0x00000958
		public static void logObject(object obj)
		{
			//string json = Json.encode(obj);
			//Utils.prettyPrintJson(json);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002774 File Offset: 0x00000974
		public static void prettyPrintJson(string json)
		{
			string text = string.Empty;
			if (json != null)
			{
				//text = JsonFormatter.prettyPrint(json);
			}
			try
			{
				Debug.Log(text);
			}
			catch (Exception)
			{
				Console.WriteLine(text);
			}
		}

		// Token: 0x0400000B RID: 11
		private static System.Random _random;
	}
}
