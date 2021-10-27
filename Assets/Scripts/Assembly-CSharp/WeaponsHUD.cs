using System;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class WeaponsHUD : MonoBehaviour
{
	// Token: 0x06000A3C RID: 2620 RVA: 0x0006FADC File Offset: 0x0006DCDC
	private void Start()
	{
		this.myStyle = new GUIStyle();
		this.myStyle.alignment = TextAnchor.MiddleRight;
		this.myStyle.normal.textColor = Color.white;
		this.myStyle.font = this.font;
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x0006FB28 File Offset: 0x0006DD28
	public void setValues(string weaponName, int currentRounds, int currentBullets, int clipSize, WeaponTypes type)
	{
		for (int i = 0; i < this.weaponsNames.Length; i++)
		{
			if (this.weaponsNames[i] == weaponName)
			{
				this.weaponTexture = this.weaponsTextures[i];
				this.bullets = (currentBullets + currentRounds).ToString();
				this.rounds = currentRounds.ToString();
				this.roundsPercentage = Mathf.CeilToInt((float)int.Parse(this.rounds) / (float)clipSize * ((clipSize >= 10) ? 10f : ((float)clipSize)));
				this.weaponType = type;
				WeaponsHUD.weaponName = weaponName;
				break;
			}
		}
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0006FBD4 File Offset: 0x0006DDD4
	public void setGenades(int noOfGrenades)
	{
		this.currentNoOfGrenades = noOfGrenades;
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0006FBE0 File Offset: 0x0006DDE0
	public void OnGUI()
	{
		if (mainmenu.pause || mainmenu.disableHUD)
		{
			return;
		}
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		float num = 326f;
		float num2 = 134f;
		GUI.BeginGroup(new Rect(1366f - num, 10f, num, num2));
		if (!WeaponsHUD.weaponsHUDRectSet)
		{
			WeaponsHUD.weaponsHUDRect = new Rect(1366f - num, 768f - num2, num, num2);
			WeaponsHUD.weaponsHUDRectSet = true;
		}
		if (!WeaponsHUD.grenadesHUDRectSet)
		{
			WeaponsHUD.grenadesHUDRect = new Rect(1226f, 768f - num2, 140f, num2);
			WeaponsHUD.grenadesHUDRectSet = true;
		}
		GUI.Label(new Rect(0f, 0f, num, num2), this.background);
		GUI.Label(new Rect(16f, 6f, num * 0.9f, num2 * 0.9f), this.weaponTexture);
		GUI.Label(new Rect(150f, 90f, 50f, 20f), this.bullets, this.myStyle);
		for (int i = 1; i <= 4; i++)
		{
			if (i <= this.currentNoOfGrenades)
			{
				GUI.Label(new Rect(num - 70f, 23f * (float)(i - 1), 60f, 45f), this.grenade);
			}
		}
		for (int j = 0; j < this.roundsPercentage; j++)
		{
			if (WeaponsHUD.weaponName == "Spas12")
			{
				GUI.DrawTexture(new Rect(39f + (float)j * 8f, num2 - 50f, 8f, 32f), this.spassBullet, ScaleMode.StretchToFill);
			}
			else if (WeaponsHUD.weaponName == "RPJ7")
			{
				GUI.DrawTexture(new Rect(39f + (float)j * 8f, num2 - 50f, 8f, 32f), this.rpjBullet, ScaleMode.StretchToFill);
			}
			else if (this.weaponType == WeaponTypes.SECONDARY)
			{
				GUI.DrawTexture(new Rect(39f + (float)j * 8f, num2 - 50f, 8f, 32f), this.smallBullet, ScaleMode.StretchToFill);
			}
			else
			{
				GUI.DrawTexture(new Rect(39f + (float)j * 7f, num2 - 50f, 8f, 32f), this.largeBullet, ScaleMode.StretchToFill);
			}
		}
		GUI.EndGroup();
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0006FE88 File Offset: 0x0006E088
	private void OnDestroy()
	{
		this.background = null;
		this.grenade = null;
		this.grenadeDisabled = null;
		this.weaponsTextures = null;
		this.weaponsNames = null;
		this.weaponTexture = null;
		this.bullets = null;
		this.rounds = null;
		this.font = null;
	}

	// Token: 0x04001038 RID: 4152
	public Texture background;

	// Token: 0x04001039 RID: 4153
	public Texture grenade;

	// Token: 0x0400103A RID: 4154
	public Texture grenadeDisabled;

	// Token: 0x0400103B RID: 4155
	public Texture[] weaponsTextures;

	// Token: 0x0400103C RID: 4156
	public string[] weaponsNames;

	// Token: 0x0400103D RID: 4157
	public Texture weaponTexture;

	// Token: 0x0400103E RID: 4158
	public string bullets;

	// Token: 0x0400103F RID: 4159
	public string rounds;

	// Token: 0x04001040 RID: 4160
	private int roundsPercentage = 10;

	// Token: 0x04001041 RID: 4161
	private WeaponTypes weaponType;

	// Token: 0x04001042 RID: 4162
	public Font font;

	// Token: 0x04001043 RID: 4163
	private GUIStyle myStyle;

	// Token: 0x04001044 RID: 4164
	private int currentNoOfGrenades;

	// Token: 0x04001045 RID: 4165
	public static Rect weaponsHUDRect;

	// Token: 0x04001046 RID: 4166
	public static bool weaponsHUDRectSet;

	// Token: 0x04001047 RID: 4167
	public static Rect grenadesHUDRect;

	// Token: 0x04001048 RID: 4168
	public static bool grenadesHUDRectSet;

	// Token: 0x04001049 RID: 4169
	public Texture smallBullet;

	// Token: 0x0400104A RID: 4170
	public Texture largeBullet;

	// Token: 0x0400104B RID: 4171
	public Texture spassBullet;

	// Token: 0x0400104C RID: 4172
	public Texture rpjBullet;

	// Token: 0x0400104D RID: 4173
	public static string weaponName = string.Empty;
}
