using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class SpeechManagerRes : MonoBehaviour
{
	// Token: 0x0600084F RID: 2127 RVA: 0x00044B40 File Offset: 0x00042D40
	private void Start()
	{
		if (SpeechManagerRes.enable3D)
		{
			Stereoscopic3D component = Camera.main.GetComponent<Stereoscopic3D>();
			if (component != null)
			{
				component.enabled = true;
			}
			SpeechManagerRes.previousEnable3d = SpeechManagerRes.enable3D;
		}
		if (this.defaultFacialAnim != null)
		{
			this.defaultFacialAnim.wrapMode = WrapMode.Loop;
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00044B9C File Offset: 0x00042D9C
	private void Awake()
	{
		SpeechManagerRes.instance = this;
		if (this.defaultAudioSource == null && base.GetComponent<AudioSource>() != null)
		{
			this.defaultAudioSource = base.GetComponent<AudioSource>();
		}
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00044BE0 File Offset: 0x00042DE0
	private void OnDestroy()
	{
		SpeechManagerRes.instance = null;
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x00044BE8 File Offset: 0x00042DE8
	private void FixedUpdate()
	{
		if (SpeechManagerRes.displayCheckpointReached > 0f)
		{
			SpeechManagerRes.displayCheckpointReached -= Time.deltaTime;
		}
		if (this.playing)
		{
			if (this.currentSentenceTimer > 0f)
			{
				this.currentSentenceTimer -= Time.deltaTime;
				return;
			}
			if (this.stopPlaying)
			{
				if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null)
				{
					HeadLookController component = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<HeadLookController>();
					if (component != null)
					{
						component.targetTransform = null;
					}
					NPCWaypointWalk component2 = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<NPCWaypointWalk>();
					if (component2 != null)
					{
						component2.speaking = false;
					}
				}
				if (this.conversations[this.currentConversation].stopToListen)
				{
					AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = true;
					AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = true;
					SpeechManagerRes.letterBox = false;
					AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = false;
					CutsceneManager.showGUI = true;
					WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD != null)
					{
						weaponsHUD.enabled = true;
					}
				}
				if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null)
				{
					if (this.conversations[this.currentConversation].sentences[this.currentSentence].facialAnim != null)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.GetComponent<Animation>().Stop(this.conversations[this.currentConversation].sentences[this.currentSentence].facialAnim.name);
					}
					else
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.GetComponent<Animation>().Stop(this.defaultFacialAnim.name);
					}
					if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip != null)
					{
						UnityEngine.Object.Destroy(this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip);
						this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = null;
					}
				}
				else if (this.defaultAudioSource.clip != null)
				{
					UnityEngine.Object.Destroy(this.defaultAudioSource.clip);
					this.defaultAudioSource.clip = null;
				}
				this.playing = false;
				this.stopPlaying = false;
				this.subtitleString = string.Empty;
				return;
			}
			if (this.currentSentence - 1 >= 0 && this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker != null)
			{
				HeadLookController component3 = this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.gameObject.GetComponent<HeadLookController>();
				if (component3 != null)
				{
					component3.targetTransform = null;
				}
				NPCWaypointWalk component4 = this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.gameObject.GetComponent<NPCWaypointWalk>();
				if (component4 != null)
				{
					component4.speaking = false;
				}
			}
			if (this.newSentanceDelayTimer > 0f)
			{
				this.newSentanceDelayTimer -= Time.deltaTime;
				return;
			}
			if (this.currentSentence - 1 >= 0)
			{
				if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker != null)
				{
					if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].facialAnim != null)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.GetComponent<Animation>().Stop(this.conversations[this.currentConversation].sentences[this.currentSentence - 1].facialAnim.name);
					}
					else
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.GetComponent<Animation>().Stop(this.defaultFacialAnim.name);
					}
					if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.clip != null)
					{
						UnityEngine.Object.Destroy(this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.clip);
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.clip = null;
					}
				}
				else if (this.defaultAudioSource.clip != null)
				{
					UnityEngine.Object.Destroy(this.defaultAudioSource.clip);
					this.defaultAudioSource.clip = null;
				}
			}
			if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null)
			{
				if (SpeechManagerRes.currentVoiceLanguage == SpeechManagerRes.VoiceLanguage.Arabic)
				{
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = (AudioClip)Resources.Load(this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeechName);
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.Play();
					this.currentAudioSource = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker;
				}
				else
				{
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = (AudioClip)Resources.Load(this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeechName);
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.Play();
					this.currentAudioSource = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker;
				}
			}
			else if (SpeechManagerRes.currentVoiceLanguage == SpeechManagerRes.VoiceLanguage.Arabic)
			{
				this.defaultAudioSource.clip = (AudioClip)Resources.Load(this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeechName);
				this.defaultAudioSource.volume = SpeechManagerRes.speechVolume;
				this.defaultAudioSource.Play();
				this.currentAudioSource = this.defaultAudioSource;
			}
			else
			{
				this.defaultAudioSource.clip = (AudioClip)Resources.Load(this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeechName);
				this.defaultAudioSource.volume = SpeechManagerRes.speechVolume;
				this.defaultAudioSource.Play();
				this.currentAudioSource = this.defaultAudioSource;
			}
			if (SpeechManagerRes.enableSubtitles && this.conversations[this.currentConversation].sentences[this.currentSentence].subtitleKeyword != string.Empty)
			{
				this.subtitleString = this.conversations[this.currentConversation].sentences[this.currentSentence].subtitleKeyword;
			}
			else
			{
				this.subtitleString = string.Empty;
			}
			if (this.conversations[this.currentConversation].sentences[this.currentSentence].lookAtObject != null)
			{
				HeadLookController component5 = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<HeadLookController>();
				if (component5 != null)
				{
					component5.targetTransform = this.conversations[this.currentConversation].sentences[this.currentSentence].lookAtObject;
					component5.overrideAnimation = true;
				}
				NPCWaypointWalk component6 = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<NPCWaypointWalk>();
				if (component6 != null)
				{
					component6.speaking = true;
				}
			}
			if (this.conversations[this.currentConversation].sentences[this.currentSentence].facialAnim != null)
			{
				string name = this.conversations[this.currentConversation].sentences[this.currentSentence].facialAnim.name;
				if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null && this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name] != null)
				{
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name].AddMixingTransform(this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head"));
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name].layer = 3;
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name].wrapMode = WrapMode.Once;
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>().Blend(name);
				}
			}
			else if (this.defaultFacialAnim != null)
			{
				string name2 = this.defaultFacialAnim.name;
				if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null && this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name2] != null)
				{
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name2].AddMixingTransform(this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head"));
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name2].layer = 3;
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>()[name2].wrapMode = WrapMode.Loop;
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.gameObject.GetComponent<Animation>().Blend(name2);
				}
			}
			if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null)
			{
				this.currentSentenceTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip.length + 1f;
			}
			else
			{
				this.currentSentenceTimer = this.defaultAudioSource.clip.length + 1f;
			}
			if (this.conversations[this.currentConversation].sentences.Length >= this.currentSentence + 2)
			{
				this.currentSentence++;
				if (SpeechManagerRes.currentVoiceLanguage == SpeechManagerRes.VoiceLanguage.Arabic)
				{
					this.newSentanceDelayTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].arabicDelay;
				}
				else
				{
					this.newSentanceDelayTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].englishDelay;
				}
			}
			else
			{
				this.stopPlaying = true;
			}
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00045930 File Offset: 0x00043B30
	private void Update()
	{
		if (this.playing && this.conversations[this.currentConversation].stopToListen)
		{
			bool jump = MobileInput.jump;
			if (jump)
			{
				this.stopPlaying = true;
				this.currentSentenceTimer = 0f;
				if (this.defaultFacialAnim != null)
				{
					AnimationHandler.instance.GetComponent<Animation>().Stop(this.defaultFacialAnim.name);
				}
				if (this.currentAudioSource != null)
				{
					this.currentAudioSource.Stop();
					this.currentAudioSource = null;
				}
				SpeechManagerRes.letterBox = false;
				AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = false;
				CutsceneManager.showGUI = true;
				WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
				if (weaponsHUD != null)
				{
					weaponsHUD.enabled = true;
				}
			}
		}
		if (SpeechManagerRes.enable3D != SpeechManagerRes.previousEnable3d)
		{
			Stereoscopic3D component = Camera.main.GetComponent<Stereoscopic3D>();
			if (component != null)
			{
				if (SpeechManagerRes.enable3D)
				{
					Camera.main.clearFlags = CameraClearFlags.Skybox;
					Camera.main.cullingMask = -1;
					component.enabled = true;
				}
				else
				{
					Camera.main.clearFlags = CameraClearFlags.Skybox;
					Camera.main.cullingMask = -1;
					component.enabled = false;
				}
			}
			SpeechManagerRes.previousEnable3d = SpeechManagerRes.enable3D;
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00045A90 File Offset: 0x00043C90
	public void Play(string conversationID)
	{
		for (int i = 0; i < this.conversations.Length; i++)
		{
			if (this.conversations[i].conversationID == conversationID)
			{
				this.currentConversation = i;
				this.currentSentence = 0;
				if (SpeechManagerRes.currentVoiceLanguage == SpeechManagerRes.VoiceLanguage.Arabic)
				{
					this.newSentanceDelayTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].arabicDelay;
				}
				else
				{
					this.newSentanceDelayTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].englishDelay;
				}
				this.playing = true;
				if (this.conversations[this.currentConversation].stopToListen)
				{
					AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = false;
					AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = false;
					SpeechManagerRes.letterBox = true;
					AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
					CutsceneManager.showGUI = false;
					WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD != null)
					{
						weaponsHUD.enabled = false;
					}
				}
				break;
			}
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00045BCC File Offset: 0x00043DCC
	public static void PlayConversation(string conversationID)
	{
		SpeechManagerRes.instance.Play(conversationID);
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00045BDC File Offset: 0x00043DDC
	public void OnGUI()
	{
		if (mainmenu.pause)
		{
			return;
		}
		if (this.subtitleString != null && this.subtitleString != string.Empty)
		{
			GUIStyle guistyle = new GUIStyle();
			guistyle.font = SpeechManagerRes.getCurrentLanguageFont();
			guistyle.alignment = TextAnchor.MiddleCenter;
			guistyle.wordWrap = true;
			string text = Language.Get(this.subtitleString, 60);
			guistyle.normal.textColor = Color.black;
			GUI.Label(new Rect((float)Screen.width / 4f + 1f, (float)Screen.height - 201f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			GUI.Label(new Rect((float)Screen.width / 4f + 1f, (float)Screen.height - 199f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			GUI.Label(new Rect((float)Screen.width / 4f - 1f, (float)Screen.height - 201f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			GUI.Label(new Rect((float)Screen.width / 4f - 1f, (float)Screen.height - 199f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
			guistyle.normal.textColor = Color.white;
			GUI.Label(new Rect((float)Screen.width / 4f, (float)Screen.height - 200f, (float)Screen.width - (float)Screen.width / 2f, 200f), text, guistyle);
		}
		if (SpeechManagerRes.displayCheckpointReached > 0f)
		{
			GUIStyle guistyle2 = new GUIStyle();
			guistyle2.font = SpeechManagerRes.getCurrentLanguageFont();
			guistyle2.alignment = TextAnchor.MiddleCenter;
			string text2 = Language.Get("GP_CheckpointReached", 60);
			guistyle2.normal.textColor = Color.black;
			GUI.Label(new Rect(1f, 51f, (float)Screen.width, 100f), text2, guistyle2);
			guistyle2.normal.textColor = Color.white;
			GUI.Label(new Rect(0f, 50f, (float)Screen.width, 100f), text2, guistyle2);
		}
		if (SpeechManagerRes.letterBox && this.blackTexture != null)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)(Screen.height / 15)), this.blackTexture, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(0f, (float)(Screen.height - Screen.height / 15), (float)Screen.width, (float)(Screen.height / 15)), this.blackTexture, ScaleMode.StretchToFill);
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00045EB4 File Offset: 0x000440B4
	public static Font getCurrentLanguageFont()
	{
		if (SpeechManagerRes.instance.specialFonts.Length > 0)
		{
			foreach (SpeechManagerRes.SpecialFont specialFont in SpeechManagerRes.instance.specialFonts)
			{
				if (specialFont.language == Language.CurrentLanguage())
				{
					return specialFont.subtitlesFont;
				}
			}
		}
		return SpeechManagerRes.instance.defaultSubtitlesFont;
	}

	// Token: 0x04000B0A RID: 2826
	public static SpeechManagerRes.VoiceLanguage currentVoiceLanguage = SpeechManagerRes.VoiceLanguage.English;

	// Token: 0x04000B0B RID: 2827
	public static bool enableSubtitles = true;

	// Token: 0x04000B0C RID: 2828
	public static bool enable3D;

	// Token: 0x04000B0D RID: 2829
	public static bool previousEnable3d;

	// Token: 0x04000B0E RID: 2830
	public static float musicVolume = 1f;

	// Token: 0x04000B0F RID: 2831
	public static float speechVolume = 1f;

	// Token: 0x04000B10 RID: 2832
	public static float sfxVolume = 1f;

	// Token: 0x04000B11 RID: 2833
	public AudioSource defaultAudioSource;

	// Token: 0x04000B12 RID: 2834
	public AnimationClip defaultFacialAnim;

	// Token: 0x04000B13 RID: 2835
	public SpeechManagerRes.Conversation[] conversations;

	// Token: 0x04000B14 RID: 2836
	[HideInInspector]
	public static SpeechManagerRes instance;

	// Token: 0x04000B15 RID: 2837
	private int currentConversation;

	// Token: 0x04000B16 RID: 2838
	private int currentSentence;

	// Token: 0x04000B17 RID: 2839
	private bool playing;

	// Token: 0x04000B18 RID: 2840
	private float currentSentenceTimer;

	// Token: 0x04000B19 RID: 2841
	private float newSentanceDelayTimer;

	// Token: 0x04000B1A RID: 2842
	private bool stopPlaying;

	// Token: 0x04000B1B RID: 2843
	public string subtitleString;

	// Token: 0x04000B1C RID: 2844
	public Font defaultSubtitlesFont;

	// Token: 0x04000B1D RID: 2845
	public SpeechManagerRes.SpecialFont[] specialFonts;

	// Token: 0x04000B1E RID: 2846
	public static float displayCheckpointReached;

	// Token: 0x04000B1F RID: 2847
	private AudioSource currentAudioSource;

	// Token: 0x04000B20 RID: 2848
	public Texture blackTexture;

	// Token: 0x04000B21 RID: 2849
	public static bool letterBox;

	// Token: 0x02000192 RID: 402
	[Serializable]
	public class Conversation
	{
		// Token: 0x04000B22 RID: 2850
		public string conversationID;

		// Token: 0x04000B23 RID: 2851
		public bool stopToListen;

		// Token: 0x04000B24 RID: 2852
		public SpeechManagerRes.Sentence[] sentences;
	}

	// Token: 0x02000193 RID: 403
	[Serializable]
	public class Sentence
	{
		// Token: 0x04000B25 RID: 2853
		public float englishDelay;

		// Token: 0x04000B26 RID: 2854
		public float arabicDelay;

		// Token: 0x04000B27 RID: 2855
		public AudioSource speaker;

		// Token: 0x04000B28 RID: 2856
		public AudioClip englishSpeech;

		// Token: 0x04000B29 RID: 2857
		public string englishSpeechName;

		// Token: 0x04000B2A RID: 2858
		public AudioClip arabicSpeech;

		// Token: 0x04000B2B RID: 2859
		public string arabicSpeechName;

		// Token: 0x04000B2C RID: 2860
		public string subtitleKeyword;

		// Token: 0x04000B2D RID: 2861
		public AnimationClip facialAnim;

		// Token: 0x04000B2E RID: 2862
		public Transform lookAtObject;
	}

	// Token: 0x02000194 RID: 404
	[Serializable]
	public class SpecialFont
	{
		// Token: 0x04000B2F RID: 2863
		public LanguageCode language;

		// Token: 0x04000B30 RID: 2864
		public Font subtitlesFont;
	}

	// Token: 0x02000195 RID: 405
	public enum VoiceLanguage
	{
		// Token: 0x04000B32 RID: 2866
		Arabic,
		// Token: 0x04000B33 RID: 2867
		English
	}
}
