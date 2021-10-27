using System;
using System.Collections;

// Token: 0x0200010F RID: 271
public static class Arabic
{
	// Token: 0x0600060C RID: 1548 RVA: 0x0002A8D4 File Offset: 0x00028AD4
	static Arabic()
	{
		Arabic.cache = new Hashtable();
		Arabic.endingCharacters = new Hashtable();
		Arabic.endingCharacters.Add(1570, 0);
		Arabic.endingCharacters.Add(1571, 0);
		Arabic.endingCharacters.Add(1572, 0);
		Arabic.endingCharacters.Add(1573, 0);
		Arabic.endingCharacters.Add(1575, 0);
		Arabic.endingCharacters.Add(1583, 0);
		Arabic.endingCharacters.Add(1584, 0);
		Arabic.endingCharacters.Add(1585, 0);
		Arabic.endingCharacters.Add(1586, 0);
		Arabic.endingCharacters.Add(1608, 0);
		Arabic.endingCharacters.Add(1688, 0);
		Arabic.endingCharacters.Add(1569, 0);
		Arabic.endingCharacters.Add(65153, 0);
		Arabic.endingCharacters.Add(65155, 0);
		Arabic.endingCharacters.Add(65157, 0);
		Arabic.endingCharacters.Add(65159, 0);
		Arabic.endingCharacters.Add(65165, 0);
		Arabic.endingCharacters.Add(65193, 0);
		Arabic.endingCharacters.Add(65195, 0);
		Arabic.endingCharacters.Add(65197, 0);
		Arabic.endingCharacters.Add(65199, 0);
		Arabic.endingCharacters.Add(65261, 0);
		Arabic.endingCharacters.Add(64394, 0);
		Arabic.endingCharacters.Add(65154, 0);
		Arabic.endingCharacters.Add(65156, 0);
		Arabic.endingCharacters.Add(65158, 0);
		Arabic.endingCharacters.Add(65160, 0);
		Arabic.endingCharacters.Add(65166, 0);
		Arabic.endingCharacters.Add(65194, 0);
		Arabic.endingCharacters.Add(65196, 0);
		Arabic.endingCharacters.Add(65198, 0);
		Arabic.endingCharacters.Add(65200, 0);
		Arabic.endingCharacters.Add(65262, 0);
		Arabic.endingCharacters.Add(64395, 0);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0002AC7C File Offset: 0x00028E7C
	public static string ParseTextField(string originalText)
	{
		if (originalText.Length > 1 && (Arabic.isArabicCharacter(originalText, originalText.Length - 1) || (char.IsWhiteSpace(originalText, originalText.Length - 1) && Arabic.isArabicCharacter(originalText, originalText.Length - 2))))
		{
			char[] array = originalText.ToCharArray();
			char[] array2 = new char[array.Length];
			array2[0] = array[array.Length - 1];
			for (int i = 1; i < array.Length; i++)
			{
				array2[i] = array[i - 1];
			}
			string text = string.Empty;
			int num = -1;
			char[] array3 = new char[array2.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				if (Arabic.isArabicCharacter(array2[array2.Length - 1 - j]) || char.IsWhiteSpace(array2[array2.Length - 1 - j]))
				{
					if (text != string.Empty)
					{
						char[] array4 = text.ToCharArray();
						int k = num;
						int num2 = 0;
						while (k < num + text.Length)
						{
							array3[k] = array4[num2];
							k++;
							num2++;
						}
						text = string.Empty;
						num = -1;
					}
					array3[j] = array2[array2.Length - 1 - j];
				}
				else
				{
					if (text == string.Empty)
					{
						num = j;
					}
					text = array2[array2.Length - 1 - j] + text;
				}
			}
			originalText = new string(array3);
			return Arabic.Parse(originalText, 60);
		}
		return originalText;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0002ADFC File Offset: 0x00028FFC
	public static bool isArabicCharacter(string text, int position)
	{
		int num = char.ConvertToUtf32(text, position);
		return (num > 1536 && num < 1791) || (num > 64336 && num < 65023) || (num > 65136 && num < 65279);
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002AE5C File Offset: 0x0002905C
	public static bool isArabicCharacter(char c)
	{
		return Arabic.isArabicCharacter(c.ToString(), 0);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002AE6C File Offset: 0x0002906C
	public static string Parse(string originalText, int charsEachLine)
	{
		if (Arabic.cache.ContainsKey(originalText))
		{
			return Arabic.cache[originalText].ToString();
		}
		bool flag = false;
		if (originalText.IndexOf('<') != -1)
		{
			flag = true;
		}
		string text = originalText;
		if (originalText.Length > charsEachLine && !originalText.Contains("<BR>"))
		{
			int num = 0;
			for (int i = charsEachLine; i < originalText.Length; i += charsEachLine)
			{
				if (text.IndexOf(" ", i + num) != -1)
				{
					text = text.Insert(text.IndexOf(" ", i + num), "<BR>");
					num += 4;
				}
			}
		}
		string text2 = string.Empty;
		string[] array = text.Split(Arabic.lineBreak, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length > 1)
		{
			foreach (string originalText2 in array)
			{
				if (!flag)
				{
					text2 = text2 + Environment.NewLine + Arabic.Parser(originalText2);
				}
				else
				{
					text2 = text2 + "<BR>" + Arabic.Parser(originalText2);
				}
			}
		}
		else if (array.Length > 0)
		{
			text2 = Arabic.Parser(array[0]);
		}
		Arabic.cache.Add(originalText, text2);
		return text2;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002AFB8 File Offset: 0x000291B8
	public static string Parser(string originalText)
	{
		char[] array = originalText.ToCharArray();
		int num = 0;
		for (int i = 0; i < originalText.Length; i++)
		{
			if (num > 0)
			{
				num--;
				array[i] = '\u0001';
			}
			else
			{
				Arabic.charPosition charPosition;
				if (i == 0)
				{
					if (char.ConvertToUtf32(originalText, i + 1) == 32)
					{
						charPosition = Arabic.charPosition.ALONE;
					}
					else
					{
						charPosition = Arabic.charPosition.BEGINNING;
					}
				}
				else if (i == originalText.Length - 1)
				{
					if (char.ConvertToUtf32(originalText, i - 1) == 32 || Arabic.endingCharacters.ContainsKey(char.ConvertToUtf32(originalText, i - 1)))
					{
						charPosition = Arabic.charPosition.ALONE;
					}
					else
					{
						charPosition = Arabic.charPosition.END;
					}
				}
				else if ((char.ConvertToUtf32(originalText, i + 1) == 32 || Arabic.IsPunctuation(originalText, i + 1)) && (char.ConvertToUtf32(originalText, i - 1) == 32 || Arabic.IsPunctuation(originalText, i - 1) || Arabic.endingCharacters.ContainsKey(char.ConvertToUtf32(originalText, i - 1))))
				{
					charPosition = Arabic.charPosition.ALONE;
				}
				else if (char.ConvertToUtf32(originalText, i - 1) == 32 || Arabic.IsPunctuation(originalText, i - 1) || Arabic.endingCharacters.ContainsKey(char.ConvertToUtf32(originalText, i - 1)))
				{
					charPosition = Arabic.charPosition.BEGINNING;
				}
				else if (char.ConvertToUtf32(originalText, i + 1) == 32 || Arabic.IsPunctuation(originalText, i + 1) || char.ConvertToUtf32(originalText, i + 1) == 1569)
				{
					charPosition = Arabic.charPosition.END;
				}
				else
				{
					charPosition = Arabic.charPosition.MIDDLE;
				}
				int num2 = char.ConvertToUtf32(originalText, i);
				switch (num2)
				{
				case 65153:
				case 65154:
					break;
				case 65155:
				case 65156:
					goto IL_554;
				case 65157:
				case 65158:
					goto IL_596;
				case 65159:
				case 65160:
					goto IL_5D8;
				case 65161:
				case 65162:
				case 65163:
				case 65164:
					goto IL_61A;
				case 65165:
				case 65166:
					goto IL_69D;
				case 65167:
				case 65168:
				case 65169:
				case 65170:
					goto IL_6DF;
				case 65171:
				case 65172:
					goto IL_7E5;
				case 65173:
				case 65174:
				case 65175:
				case 65176:
					goto IL_827;
				case 65177:
				case 65178:
				case 65179:
				case 65180:
					goto IL_8AA;
				case 65181:
				case 65182:
				case 65183:
				case 65184:
					goto IL_92D;
				case 65185:
				case 65186:
				case 65187:
				case 65188:
					goto IL_A33;
				case 65189:
				case 65190:
				case 65191:
				case 65192:
					goto IL_AB6;
				case 65193:
				case 65194:
					goto IL_B39;
				case 65195:
				case 65196:
					goto IL_B7B;
				case 65197:
				case 65198:
					goto IL_BBD;
				case 65199:
				case 65200:
					goto IL_BFF;
				case 65201:
				case 65202:
				case 65203:
				case 65204:
					goto IL_C83;
				case 65205:
				case 65206:
				case 65207:
				case 65208:
					goto IL_D06;
				case 65209:
				case 65210:
				case 65211:
				case 65212:
					goto IL_D89;
				case 65213:
				case 65214:
				case 65215:
				case 65216:
					goto IL_E0C;
				case 65217:
				case 65218:
				case 65219:
				case 65220:
					goto IL_E8F;
				case 65221:
				case 65222:
				case 65223:
				case 65224:
					goto IL_F12;
				case 65225:
				case 65226:
				case 65227:
				case 65228:
					goto IL_F95;
				case 65229:
				case 65230:
				case 65231:
				case 65232:
					goto IL_1018;
				case 65233:
				case 65234:
				case 65235:
				case 65236:
					goto IL_109B;
				case 65237:
				case 65238:
				case 65239:
				case 65240:
					goto IL_111E;
				case 65241:
				case 65242:
				case 65243:
				case 65244:
					goto IL_11A1;
				case 65245:
				case 65246:
				case 65247:
				case 65248:
					goto IL_132A;
				case 65249:
				case 65250:
				case 65251:
				case 65252:
					goto IL_15C9;
				case 65253:
				case 65254:
				case 65255:
				case 65256:
					goto IL_164C;
				case 65257:
				case 65258:
				case 65259:
				case 65260:
					goto IL_16CF;
				case 65261:
				case 65262:
					goto IL_1752;
				case 65263:
				case 65264:
					goto IL_1817;
				case 65265:
				case 65266:
				case 65267:
				case 65268:
					goto IL_1794;
				default:
					switch (num2)
					{
					case 1570:
						break;
					case 1571:
						goto IL_554;
					case 1572:
						goto IL_596;
					case 1573:
						goto IL_5D8;
					case 1574:
						goto IL_61A;
					case 1575:
						goto IL_69D;
					case 1576:
						goto IL_6DF;
					case 1577:
						goto IL_7E5;
					case 1578:
						goto IL_827;
					case 1579:
						goto IL_8AA;
					case 1580:
						goto IL_92D;
					case 1581:
						goto IL_A33;
					case 1582:
						goto IL_AB6;
					case 1583:
						goto IL_B39;
					case 1584:
						goto IL_B7B;
					case 1585:
						goto IL_BBD;
					case 1586:
						goto IL_BFF;
					case 1587:
						goto IL_C83;
					case 1588:
						goto IL_D06;
					case 1589:
						goto IL_D89;
					case 1590:
						goto IL_E0C;
					case 1591:
						goto IL_E8F;
					case 1592:
						goto IL_F12;
					case 1593:
						goto IL_F95;
					case 1594:
						goto IL_1018;
					default:
						switch (num2)
						{
						case 64378:
						case 64379:
						case 64380:
						case 64381:
							break;
						default:
							switch (num2)
							{
							case 64342:
							case 64343:
							case 64344:
							case 64345:
								break;
							default:
								switch (num2)
								{
								case 64508:
								case 64509:
								case 64510:
								case 64511:
									break;
								default:
									if (num2 == 1662)
									{
										goto IL_762;
									}
									if (num2 == 1670)
									{
										goto IL_9B0;
									}
									if (num2 == 1688)
									{
										goto IL_C41;
									}
									if (num2 == 1705)
									{
										goto IL_1224;
									}
									if (num2 == 1711)
									{
										goto IL_12A7;
									}
									if (num2 != 1740)
									{
										goto IL_18DC;
									}
									break;
								}
								if (charPosition == Arabic.charPosition.ALONE)
								{
									array[i] = char.ConvertFromUtf32(64508).ToCharArray()[0];
								}
								else if (charPosition == Arabic.charPosition.BEGINNING)
								{
									array[i] = char.ConvertFromUtf32(64510).ToCharArray()[0];
								}
								else if (charPosition == Arabic.charPosition.MIDDLE)
								{
									array[i] = char.ConvertFromUtf32(64511).ToCharArray()[0];
								}
								else if (charPosition == Arabic.charPosition.END)
								{
									array[i] = char.ConvertFromUtf32(64509).ToCharArray()[0];
								}
								goto IL_18DC;
							}
							IL_762:
							if (charPosition == Arabic.charPosition.ALONE)
							{
								array[i] = char.ConvertFromUtf32(64342).ToCharArray()[0];
							}
							else if (charPosition == Arabic.charPosition.BEGINNING)
							{
								array[i] = char.ConvertFromUtf32(64344).ToCharArray()[0];
							}
							else if (charPosition == Arabic.charPosition.MIDDLE)
							{
								array[i] = char.ConvertFromUtf32(64345).ToCharArray()[0];
							}
							else if (charPosition == Arabic.charPosition.END)
							{
								array[i] = char.ConvertFromUtf32(64343).ToCharArray()[0];
							}
							goto IL_18DC;
						case 64394:
						case 64395:
							goto IL_C41;
						case 64398:
						case 64399:
						case 64400:
						case 64401:
							goto IL_1224;
						case 64402:
						case 64403:
						case 64404:
						case 64405:
							goto IL_12A7;
						}
						IL_9B0:
						if (charPosition == Arabic.charPosition.ALONE)
						{
							array[i] = char.ConvertFromUtf32(64378).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.BEGINNING)
						{
							array[i] = char.ConvertFromUtf32(64380).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.MIDDLE)
						{
							array[i] = char.ConvertFromUtf32(64381).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.END)
						{
							array[i] = char.ConvertFromUtf32(64379).ToCharArray()[0];
						}
						goto IL_18DC;
						IL_C41:
						if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
						{
							array[i] = char.ConvertFromUtf32(64395).ToCharArray()[0];
						}
						else
						{
							array[i] = char.ConvertFromUtf32(64394).ToCharArray()[0];
						}
						goto IL_18DC;
						IL_1224:
						if (charPosition == Arabic.charPosition.ALONE)
						{
							array[i] = char.ConvertFromUtf32(64398).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.BEGINNING)
						{
							array[i] = char.ConvertFromUtf32(64400).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.MIDDLE)
						{
							array[i] = char.ConvertFromUtf32(64401).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.END)
						{
							array[i] = char.ConvertFromUtf32(64399).ToCharArray()[0];
						}
						goto IL_18DC;
						IL_12A7:
						if (charPosition == Arabic.charPosition.ALONE)
						{
							array[i] = char.ConvertFromUtf32(64402).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.BEGINNING)
						{
							array[i] = char.ConvertFromUtf32(64404).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.MIDDLE)
						{
							array[i] = char.ConvertFromUtf32(64405).ToCharArray()[0];
						}
						else if (charPosition == Arabic.charPosition.END)
						{
							array[i] = char.ConvertFromUtf32(64403).ToCharArray()[0];
						}
						goto IL_18DC;
					case 1601:
						goto IL_109B;
					case 1602:
						goto IL_111E;
					case 1603:
						goto IL_11A1;
					case 1604:
						goto IL_132A;
					case 1605:
						goto IL_15C9;
					case 1606:
						goto IL_164C;
					case 1607:
						goto IL_16CF;
					case 1608:
						goto IL_1752;
					case 1609:
						goto IL_1817;
					case 1610:
						goto IL_1794;
					}
					break;
				}
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65154).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65153).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_554:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65156).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65155).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_596:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65158).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65157).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_5D8:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65160).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65159).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_61A:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65161).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65163).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65164).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65162).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_69D:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65166).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65165).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_6DF:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65167).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65169).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65170).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65168).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_7E5:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65172).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65171).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_827:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65173).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65175).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65176).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65174).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_8AA:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65177).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65179).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65180).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65178).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_92D:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65181).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65183).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65184).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65182).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_A33:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65185).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65187).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65188).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65186).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_AB6:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65189).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65191).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65192).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65190).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_B39:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65194).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65193).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_B7B:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65196).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65195).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_BBD:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65198).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65197).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_BFF:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65200).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65199).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_C83:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65201).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65203).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65204).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65202).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_D06:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65205).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65207).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65208).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65206).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_D89:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65209).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65211).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65212).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65210).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_E0C:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65213).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65215).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65216).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65214).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_E8F:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65217).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65219).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65220).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65218).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_F12:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65221).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65223).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65224).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65222).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_F95:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65225).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65227).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65228).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65226).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_1018:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65229).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65231).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65232).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65230).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_109B:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65233).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65235).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65236).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65234).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_111E:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65237).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65239).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65240).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65238).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_11A1:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65241).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65243).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65244).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65242).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_132A:
				if (originalText.Length - 1 >= i + 1 && char.ConvertToUtf32(originalText, i + 1) == 1575)
				{
					num = 1;
					if (charPosition == Arabic.charPosition.BEGINNING)
					{
						array[i] = char.ConvertFromUtf32(65275).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.MIDDLE)
					{
						array[i] = char.ConvertFromUtf32(65276).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.END)
					{
						array[i] = char.ConvertFromUtf32(65276).ToCharArray()[0];
					}
				}
				else if (originalText.Length - 1 >= i + 1 && char.ConvertToUtf32(originalText, i + 1) == 1571)
				{
					num = 1;
					if (charPosition == Arabic.charPosition.BEGINNING)
					{
						array[i] = char.ConvertFromUtf32(65271).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.MIDDLE)
					{
						array[i] = char.ConvertFromUtf32(65272).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.END)
					{
						array[i] = char.ConvertFromUtf32(65272).ToCharArray()[0];
					}
				}
				else if (originalText.Length - 1 >= i + 1 && char.ConvertToUtf32(originalText, i + 1) == 1570)
				{
					num = 1;
					if (charPosition == Arabic.charPosition.BEGINNING)
					{
						array[i] = char.ConvertFromUtf32(65269).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.MIDDLE)
					{
						array[i] = char.ConvertFromUtf32(65270).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.END)
					{
						array[i] = char.ConvertFromUtf32(65270).ToCharArray()[0];
					}
				}
				else if (originalText.Length - 1 >= i + 1 && char.ConvertToUtf32(originalText, i + 1) == 1573)
				{
					num = 1;
					if (charPosition == Arabic.charPosition.BEGINNING)
					{
						array[i] = char.ConvertFromUtf32(65273).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.MIDDLE)
					{
						array[i] = char.ConvertFromUtf32(65274).ToCharArray()[0];
					}
					else if (charPosition == Arabic.charPosition.END)
					{
						array[i] = char.ConvertFromUtf32(65274).ToCharArray()[0];
					}
				}
				else if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65245).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65247).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65248).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65246).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_15C9:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65249).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65251).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65252).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65250).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_164C:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65253).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65255).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65256).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65254).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_16CF:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(1749).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65259).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65260).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65258).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_1752:
				if (charPosition == Arabic.charPosition.END || charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65262).ToCharArray()[0];
				}
				else
				{
					array[i] = char.ConvertFromUtf32(65261).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_1794:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65265).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.BEGINNING)
				{
					array[i] = char.ConvertFromUtf32(65267).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.MIDDLE)
				{
					array[i] = char.ConvertFromUtf32(65268).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65266).ToCharArray()[0];
				}
				goto IL_18DC;
				IL_1817:
				if (charPosition == Arabic.charPosition.ALONE)
				{
					array[i] = char.ConvertFromUtf32(65263).ToCharArray()[0];
				}
				else if (charPosition == Arabic.charPosition.END)
				{
					array[i] = char.ConvertFromUtf32(65264).ToCharArray()[0];
				}
			}
			IL_18DC:;
		}
		char[] array2 = new char[array.Length];
		string text = string.Empty;
		int num3 = -1;
		for (int j = 0; j < array.Length; j++)
		{
			if (Arabic.isArabicCharacter(array[array.Length - 1 - j]))
			{
				if (text != string.Empty)
				{
					char[] array3 = text.ToCharArray();
					int k = num3;
					int num4 = 0;
					while (k < num3 + text.Length)
					{
						array2[k] = array3[num4];
						k++;
						num4++;
					}
					text = string.Empty;
					num3 = -1;
				}
				array2[j] = array[array.Length - 1 - j];
			}
			else
			{
				if (text == string.Empty)
				{
					num3 = j;
				}
				text = array[array.Length - 1 - j] + text;
			}
		}
		if (text != string.Empty)
		{
			char[] array4 = text.ToCharArray();
			int l = num3;
			int num5 = 0;
			while (l < num3 + text.Length)
			{
				array2[l] = array4[num5];
				l++;
				num5++;
			}
			text = string.Empty;
		}
		return new string(array2);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002C9EC File Offset: 0x0002ABEC
	private static bool IsPunctuation(string parsedText, int p)
	{
		return char.IsPunctuation(parsedText, p) || char.IsSymbol(parsedText, p);
	}

	// Token: 0x040006CB RID: 1739
	private static Hashtable endingCharacters;

	// Token: 0x040006CC RID: 1740
	private static Hashtable cache;

	// Token: 0x040006CD RID: 1741
	private static string[] lineBreak = new string[]
	{
		"<BR>"
	};

	// Token: 0x02000110 RID: 272
	private enum charPosition
	{
		// Token: 0x040006CF RID: 1743
		BEGINNING,
		// Token: 0x040006D0 RID: 1744
		MIDDLE,
		// Token: 0x040006D1 RID: 1745
		END,
		// Token: 0x040006D2 RID: 1746
		ALONE
	}
}
