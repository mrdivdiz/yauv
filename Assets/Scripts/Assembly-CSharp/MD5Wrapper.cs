using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x02000099 RID: 153
//[NotRenamed]
//[NotConverted]
public static class MD5Wrapper
{
	// Token: 0x060002D6 RID: 726 RVA: 0x0001671C File Offset: 0x0001491C
	//[NotRenamed]
	public static string Md5Sum(string strToEncrypt)
	{
		/*UTF8Encoding utf8Encoding = new UTF8Encoding();
		byte[] bytes = utf8Encoding.GetBytes(strToEncrypt);
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = md5CryptoServiceProvider.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');*/
		return "0";
	}
}
