using System;
using UnityEngine;

// Token: 0x02000242 RID: 578
[ExecuteInEditMode]
public class DashBoard : MonoBehaviour
{
	// Token: 0x06000AF2 RID: 2802 RVA: 0x000853C4 File Offset: 0x000835C4
	private void Start()
	{
		this.carcontroller = base.transform.parent.GetComponent<CarController>();
		this.drivetrain = base.transform.parent.GetComponent<Drivetrain>();
		if (this.tachoMeterNeedle && this.tachoMeterNeedleSize == Vector2.zero)
		{
			this.tachoMeterNeedleSize.x = (float)this.tachoMeterNeedle.width;
			this.tachoMeterNeedleSize.y = (float)this.tachoMeterNeedle.height;
		}
		if (this.speedoMeterNeedle && this.speedoMeterNeedleSize == Vector2.zero)
		{
			this.speedoMeterNeedleSize.x = (float)this.speedoMeterNeedle.width;
			this.speedoMeterNeedleSize.y = (float)this.speedoMeterNeedle.height;
		}
		switch (DifficultyManager.difficulty)
		{
		case DifficultyManager.Difficulty.EASY:
			this.maxHealth = (this.health = DifficultyManager.easyCarHits);
			break;
		case DifficultyManager.Difficulty.MEDIUM:
			this.maxHealth = (this.health = DifficultyManager.mediumCarHits);
			break;
		case DifficultyManager.Difficulty.HARD:
			this.maxHealth = (this.health = DifficultyManager.hardCarHits);
			break;
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00085510 File Offset: 0x00083710
	public void decreaseHealth()
	{
		if (this.health > 0)
		{
			this.health--;
		}
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0008552C File Offset: 0x0008372C
	private void OnGUI()
	{
		if (mainmenu.pause || mainmenu.disableHUD)
		{
			return;
		}
		float num = 768f;
		if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.4f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.2f)
		{
			num = 1024.5f;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / num, 1f));
		}
		else if (Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f < 1.6f && Mathf.Round((float)Screen.width / (float)Screen.height * 10f) / 10f > 1.4f)
		{
			num = 910.6667f;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		}
		else
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		}
		GUI.depth = this.depth;
		if (this.tachoMeter)
		{
			this.tachoMeterRect = new Rect(this.tachoMeterPosition.x, (float)(Screen.height - this.tachoMeter.height) - this.tachoMeterPosition.y, (float)this.tachoMeter.width, (float)(this.tachoMeter.height + 8));
			GUI.Label(this.tachoMeterRect, this.tachoMeter);
			if (this.tachoMeterNeedle)
			{
				this.tachoMeterNeedleRect = new Rect(this.tachoMeterRect.x + this.tachoMeterRect.width / 2f - this.tachoMeterNeedleSize.x * 0.5f, this.tachoMeterRect.y + this.tachoMeterRect.height / 2f - this.tachoMeterNeedleSize.y * 0.5f, this.tachoMeterNeedleSize.x, this.tachoMeterNeedleSize.y);
				this.pivot = new Vector2(this.tachoMeterNeedleRect.xMin + this.tachoMeterNeedleRect.width * 0.5f, this.tachoMeterNeedleRect.yMin + this.tachoMeterNeedleRect.height * 0.5f);
				if (this.RPMFactor < 1f)
				{
					this.RPMFactor = 1f;
				}
				if (this.drivetrain)
				{
					this.actualtachoMeterNeedleAngle = this.drivetrain.rpm / (this.RPMFactor * 10f) + this.tachoMeterNeedleAngle;
				}
				GUIUtility.RotateAroundPivot(this.actualtachoMeterNeedleAngle, this.pivot);
				GUI.DrawTexture(this.tachoMeterNeedleRect, this.tachoMeterNeedle);
			}
		}
		if (this.speedoMeter)
		{
			Rect position = new Rect(this.speedoMeterPosition.x + 1366f - (float)this.speedoMeter.width, this.speedoMeterPosition.y * num, (float)this.speedoMeter.width, (float)(this.speedoMeter.height + 8));
			GUI.Label(position, this.speedoMeter);
			Rect position2 = new Rect(position.x + 47f + (float)this.HealthBar.width * 0.5f * (1f - Mathf.Clamp01((float)this.health / (float)this.maxHealth)), position.y + 57f, (float)this.HealthBar.width * 0.5f * Mathf.Clamp01((float)this.health / (float)this.maxHealth), (float)this.HealthBar.height * 0.5f);
			GUI.DrawTexture(position2, this.HealthBar);
			if (this.speedoMeterNeedle)
			{
				this.speedoMeterNeedleRect = new Rect(position.x + position.width * 0.79f - this.speedoMeterNeedleSize.x * 0.5f, position.y + position.height / 2f - this.speedoMeterNeedleSize.y * 0.5f, this.speedoMeterNeedleSize.x, this.speedoMeterNeedleSize.y);
				this.pivot = new Vector2(this.speedoMeterNeedleRect.xMin + this.speedoMeterNeedleRect.width * 0.5f, this.speedoMeterNeedleRect.yMin + this.speedoMeterNeedleRect.height * 0.5f);
				float num2 = (float)Screen.width / 1366f;
				float num3 = (float)Screen.height / num;
				this.pivot = new Vector2(num2 * this.pivot.x, num3 * this.pivot.y);
				if (this.tachoFactor < 0.5f)
				{
					this.tachoFactor = 0.5f;
				}
				if (this.drivetrain)
				{
					this.actualspeedoMeterNeedleAngle = this.drivetrain.velo * 3.6f / this.tachoFactor + this.speedoMeterNeedleAngle;
				}
				Matrix4x4 matrix = GUI.matrix;
				GUIUtility.RotateAroundPivot(this.actualspeedoMeterNeedleAngle, this.pivot);
				GUI.DrawTexture(this.speedoMeterNeedleRect, this.speedoMeterNeedle);
				GUI.matrix = matrix;
			}
		}
		if (this.carcontroller)
		{
			if (this.TCS && this.carcontroller.TCSTriggered)
			{
				GUI.Label(new Rect((float)(Screen.width / 2 - this.TCS.width * 3), (float)(Screen.height - this.TCS.height), (float)this.TCS.width, (float)this.TCS.height), this.TCS);
			}
			if (this.ABS && this.carcontroller.ABSTriggered)
			{
				GUI.Label(new Rect((float)(Screen.width / 2 - this.TCS.width * 3 + this.ABS.width), (float)(Screen.height - this.ABS.height), (float)this.ABS.width, (float)this.ABS.height), this.ABS);
			}
			if (this.ESP && this.carcontroller.ESPTriggered)
			{
				GUI.Label(new Rect((float)(Screen.width / 2 - this.TCS.width * 3 + this.ABS.width + this.TCS.width), (float)(Screen.height - this.ESP.height), (float)this.ESP.width, (float)this.ESP.height), this.ESP);
			}
		}
	}

	// Token: 0x040012A2 RID: 4770
	private int health = 5;

	// Token: 0x040012A3 RID: 4771
	private int maxHealth = 5;

	// Token: 0x040012A4 RID: 4772
	public int depth = 2;

	// Token: 0x040012A5 RID: 4773
	public GUIStyle gearStyle;

	// Token: 0x040012A6 RID: 4774
	public Texture2D tachoMeter;

	// Token: 0x040012A7 RID: 4775
	public Vector2 tachoMeterPosition;

	// Token: 0x040012A8 RID: 4776
	public Texture2D tachoMeterNeedle;

	// Token: 0x040012A9 RID: 4777
	public Vector2 tachoMeterNeedleSize;

	// Token: 0x040012AA RID: 4778
	public float tachoMeterNeedleAngle;

	// Token: 0x040012AB RID: 4779
	private float actualtachoMeterNeedleAngle;

	// Token: 0x040012AC RID: 4780
	public float RPMFactor = 3.5714f;

	// Token: 0x040012AD RID: 4781
	public Texture2D HealthBar;

	// Token: 0x040012AE RID: 4782
	public Texture2D speedoMeter;

	// Token: 0x040012AF RID: 4783
	public Vector2 speedoMeterPosition;

	// Token: 0x040012B0 RID: 4784
	public Texture2D speedoMeterNeedle;

	// Token: 0x040012B1 RID: 4785
	public Vector2 speedoMeterNeedleSize;

	// Token: 0x040012B2 RID: 4786
	public float speedoMeterNeedleAngle;

	// Token: 0x040012B3 RID: 4787
	private float actualspeedoMeterNeedleAngle;

	// Token: 0x040012B4 RID: 4788
	public float tachoFactor = 1.36f;

	// Token: 0x040012B5 RID: 4789
	public Texture2D ABS;

	// Token: 0x040012B6 RID: 4790
	public Texture2D TCS;

	// Token: 0x040012B7 RID: 4791
	public Texture2D ESP;

	// Token: 0x040012B8 RID: 4792
	private CarController carcontroller;

	// Token: 0x040012B9 RID: 4793
	private Drivetrain drivetrain;

	// Token: 0x040012BA RID: 4794
	private Rect tachoMeterRect;

	// Token: 0x040012BB RID: 4795
	private Rect tachoMeterNeedleRect;

	// Token: 0x040012BC RID: 4796
	private Rect speedoMeterRect;

	// Token: 0x040012BD RID: 4797
	private Rect speedoMeterNeedleRect;

	// Token: 0x040012BE RID: 4798
	private Rect instrumentalPanelRect;

	// Token: 0x040012BF RID: 4799
	private Rect gearRect;

	// Token: 0x040012C0 RID: 4800
	private Vector2 pivot;
}
