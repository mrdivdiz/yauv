using System;
using UnityEngine;

// Token: 0x0200025B RID: 603
public class QBSpeechManager : MonoBehaviour
{
	// Token: 0x06000B6D RID: 2925 RVA: 0x0009014C File Offset: 0x0008E34C
	private void Awake()
	{
		QBSpeechManager.instance = this;
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00090154 File Offset: 0x0008E354
	private void OnDestroy()
	{
		QBSpeechManager.instance = null;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0009015C File Offset: 0x0008E35C
	public static void PlayerReachedWaypoint(int waypointReached, int currentLap)
	{
		foreach (QBSpeechManager.Sentence sentence in QBSpeechManager.instance.sentences)
		{
			if (sentence.wayPoint == waypointReached)
			{
				switch (sentence.playType)
				{
				case QBSpeechManager.PlayTypes.ALL_LAPS:
					QBSpeechManager.instance.Play(sentence);
					break;
				case QBSpeechManager.PlayTypes.THIS_LAP_ONLY:
					if (sentence.lapNo == currentLap)
					{
						QBSpeechManager.instance.Play(sentence);
					}
					break;
				case QBSpeechManager.PlayTypes.THIS_AND_PREVIOUS_LAPS:
					if (sentence.lapNo >= currentLap)
					{
						QBSpeechManager.instance.Play(sentence);
					}
					break;
				case QBSpeechManager.PlayTypes.THIS_AND_COMING_LAPS:
					if (sentence.lapNo <= currentLap)
					{
						QBSpeechManager.instance.Play(sentence);
					}
					break;
				}
			}
		}
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00090220 File Offset: 0x0008E420
	public void Play(QBSpeechManager.Sentence s)
	{
		if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.English)
		{
			if (s.enAudioClip != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(s.enAudioClip, SpeechManager.speechVolume);
				if (s.subtitleKeyword != string.Empty)
				{
					this.subtitleString = Language.Get(s.subtitleKeyword, 60);
					this.subtitleTimer = s.enAudioClip.length;
				}
			}
		}
		else if (s.arAudioClip != null)
		{
			base.GetComponent<AudioSource>().PlayOneShot(s.arAudioClip, SpeechManager.speechVolume);
			if (s.subtitleKeyword != string.Empty)
			{
				this.subtitleString = Language.Get(s.subtitleKeyword, 60);
				this.subtitleTimer = s.arAudioClip.length;
			}
		}
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x00090300 File Offset: 0x0008E500
	public void Update()
	{
		if (this.subtitleString != string.Empty)
		{
			if (this.subtitleTimer >= 0f)
			{
				this.subtitleTimer -= Time.deltaTime;
			}
			else
			{
				this.subtitleString = string.Empty;
			}
		}
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x00090354 File Offset: 0x0008E554
	public void OnGUI()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.subtitleString != string.Empty)
		{
			GUIStyle guistyle = new GUIStyle();
			guistyle.font = SpeechManager.getCurrentLanguageFont();
			guistyle.alignment = TextAnchor.MiddleCenter;
			guistyle.wordWrap = true;
			string text = this.subtitleString;
			guistyle.normal.textColor = Color.black;
			GUI.Label(new Rect((float)Screen.width / 4f + 1f, (float)Screen.height - 201f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			GUI.Label(new Rect((float)Screen.width / 4f + 1f, (float)Screen.height - 199f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			GUI.Label(new Rect((float)Screen.width / 4f - 1f, (float)Screen.height - 201f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			GUI.Label(new Rect((float)Screen.width / 4f - 1f, (float)Screen.height - 199f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			guistyle.normal.textColor = Color.white;
			GUI.Label(new Rect((float)Screen.width / 4f, (float)Screen.height - 200f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
		}
	}

	// Token: 0x040014BC RID: 5308
	public QBSpeechManager.Sentence[] sentences;

	// Token: 0x040014BD RID: 5309
	public static QBSpeechManager instance;

	// Token: 0x040014BE RID: 5310
	private string subtitleString;

	// Token: 0x040014BF RID: 5311
	private float subtitleTimer;

	// Token: 0x0200025C RID: 604
	public enum PlayTypes
	{
		// Token: 0x040014C1 RID: 5313
		ALL_LAPS,
		// Token: 0x040014C2 RID: 5314
		THIS_LAP_ONLY,
		// Token: 0x040014C3 RID: 5315
		THIS_AND_PREVIOUS_LAPS,
		// Token: 0x040014C4 RID: 5316
		THIS_AND_COMING_LAPS
	}

	// Token: 0x0200025D RID: 605
	[Serializable]
	public class Sentence
	{
		// Token: 0x040014C5 RID: 5317
		public AudioClip enAudioClip;

		// Token: 0x040014C6 RID: 5318
		public AudioClip arAudioClip;

		// Token: 0x040014C7 RID: 5319
		public string subtitleKeyword = string.Empty;

		// Token: 0x040014C8 RID: 5320
		public QBSpeechManager.PlayTypes playType;

		// Token: 0x040014C9 RID: 5321
		public int wayPoint;

		// Token: 0x040014CA RID: 5322
		public int lapNo;
	}
}
