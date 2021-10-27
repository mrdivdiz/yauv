using System;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class SpeechManager : MonoBehaviour
{
	// Token: 0x06000840 RID: 2112 RVA: 0x00043344 File Offset: 0x00041544
	private void Start()
	{
		if (SpeechManager.enable3D)
		{
			Stereoscopic3D component = Camera.main.GetComponent<Stereoscopic3D>();
			if (component != null)
			{
				component.enabled = true;
			}
			SpeechManager.previousEnable3d = SpeechManager.enable3D;
		}
		if (this.defaultFacialAnim != null)
		{
			this.defaultFacialAnim.wrapMode = WrapMode.Loop;
		}
		this.style = new GUIStyle();
		this.style.font = SpeechManager.getCurrentLanguageFont();
		this.style.alignment = TextAnchor.MiddleCenter;
		this.style.fontSize +=22;
		this.style.wordWrap = true;
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x000433D4 File Offset: 0x000415D4
	public void ChangedLanguage()
	{
		this.style = new GUIStyle();
		this.style.font = SpeechManager.getCurrentLanguageFont();
		this.style.alignment = TextAnchor.MiddleCenter;
		this.style.wordWrap = true;
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0004340C File Offset: 0x0004160C
	private void Awake()
	{
		SpeechManager.instance = this;
		if (this.defaultAudioSource == null && base.GetComponent<AudioSource>() != null)
		{
			this.defaultAudioSource = base.GetComponent<AudioSource>();
		}
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00043450 File Offset: 0x00041650
	private void OnDestroy()
	{
		SpeechManager.instance = null;
		this.defaultAudioSource = null;
		this.defaultFacialAnim = null;
		this.conversations = null;
		this.currentAudioSource = null;
		this.blackTexture = null;
		this.checkpointReachedSound = null;
		if (this.style != null)
		{
			this.style.font = null;
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x000434A4 File Offset: 0x000416A4
	private void FixedUpdate()
	{
		if (SpeechManager.displayCheckpointReached > 0f)
		{
			SpeechManager.displayCheckpointReached -= Time.deltaTime;
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
					SpeechManager.letterBox = false;
					AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = false;
					CutsceneManager.showGUI = true;
					WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD != null)
					{
						weaponsHUD.enabled = true;
					}
					if (PlatformCharacterController.joystickLeft != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(true);
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
					if (this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech != null && this.conversations[this.currentConversation].sentences[this.currentSentence].nullify)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech = null;
						this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = null;
					}
					if (this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech != null && this.conversations[this.currentConversation].sentences[this.currentSentence].nullify)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech = null;
						this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = null;
					}
				}
				else
				{
					if (this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech != null && this.conversations[this.currentConversation].sentences[this.currentSentence].nullify)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech = null;
						this.defaultAudioSource.clip = null;
					}
					if (this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech != null && this.conversations[this.currentConversation].sentences[this.currentSentence].nullify)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech = null;
						this.defaultAudioSource.clip = null;
					}
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
					if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].arabicSpeech != null)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].arabicSpeech = null;
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.clip = null;
					}
					if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].englishSpeech != null)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].englishSpeech = null;
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].speaker.clip = null;
					}
				}
				else
				{
					if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].arabicSpeech != null)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].arabicSpeech = null;
						this.defaultAudioSource.clip = null;
					}
					if (this.conversations[this.currentConversation].sentences[this.currentSentence - 1].englishSpeech != null)
					{
						this.conversations[this.currentConversation].sentences[this.currentSentence - 1].englishSpeech = null;
						this.defaultAudioSource.clip = null;
					}
				}
			}
			if (this.conversations[this.currentConversation].sentences[this.currentSentence].speaker != null)
			{
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
				{
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech;
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.Play();
					this.currentAudioSource = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker;
				}
				else
				{
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.clip = this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech;
					this.conversations[this.currentConversation].sentences[this.currentSentence].speaker.Play();
					this.currentAudioSource = this.conversations[this.currentConversation].sentences[this.currentSentence].speaker;
				}
			}
			else if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
			{
				this.defaultAudioSource.clip = this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech;
				this.defaultAudioSource.volume = SpeechManager.speechVolume;
				this.defaultAudioSource.Play();
				this.currentAudioSource = this.defaultAudioSource;
			}
			else
			{
				this.defaultAudioSource.clip = this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech;
				this.defaultAudioSource.volume = SpeechManager.speechVolume;
				this.defaultAudioSource.Play();
				this.currentAudioSource = this.defaultAudioSource;
			}
			if (SpeechManager.enableSubtitles && this.conversations[this.currentConversation].sentences[this.currentSentence].subtitleKeyword != string.Empty)
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
			if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
			{
				this.currentSentenceTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].arabicSpeech.length + 1f;
			}
			else
			{
				this.currentSentenceTimer = this.conversations[this.currentConversation].sentences[this.currentSentence].englishSpeech.length + 1f;
			}
			if (this.conversations[this.currentConversation].sentences.Length >= this.currentSentence + 2)
			{
				this.currentSentence++;
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
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

	// Token: 0x06000845 RID: 2117 RVA: 0x00044414 File Offset: 0x00042614
	private void Update()
	{
		if (this.playing && this.conversations[this.currentConversation].stopToListen)
		{
			bool flag = false;
			MobileInput.mouseX = 0f;
			MobileInput.mouseY = 0f;
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						this.timeTouchBegan = Time.time;
					}
					if (touch.phase == TouchPhase.Ended && Time.time < this.timeTouchBegan + 0.1f && Time.time > this.stopToListenStartTime + 2f)
					{
						flag = true;
					}
					if (touch.phase == TouchPhase.Moved)
					{
						MobileInput.mouseX = touch.deltaPosition.x * 0.25f;
						MobileInput.mouseY = touch.deltaPosition.y * 0.25f;
					}
				}
			}
			flag |= InputManager.GetButtonDown("Jump");
			if (flag)
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
				SpeechManager.letterBox = false;
				AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = false;
				AnimationHandler.instance.gameObject.GetComponent<HeadLookController>().targetTransform = null;
				CutsceneManager.showGUI = true;
				WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
				if (weaponsHUD != null)
				{
					weaponsHUD.enabled = true;
				}
				if (PlatformCharacterController.joystickLeft != null)
				{
					PlatformCharacterController.joystickLeft.gameObject.SetActive(true);
				}
			}
		}
		if (SpeechManager.enable3D != SpeechManager.previousEnable3d)
		{
			Stereoscopic3D stereoscopic3D = Camera.main.GetComponent<Stereoscopic3D>();
			if (stereoscopic3D == null)
			{
				stereoscopic3D = Camera.main.gameObject.AddComponent<Stereoscopic3D>();
			}
			if (stereoscopic3D != null)
			{
				if (SpeechManager.enable3D)
				{
					Camera.main.clearFlags = CameraClearFlags.Skybox;
					Camera.main.cullingMask = -1;
					stereoscopic3D.enabled = true;
				}
				else
				{
					Camera.main.clearFlags = CameraClearFlags.Skybox;
					Camera.main.cullingMask = -1;
					stereoscopic3D.enabled = false;
				}
			}
			SpeechManager.previousEnable3d = SpeechManager.enable3D;
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x000446B0 File Offset: 0x000428B0
	public void Play(string conversationID)
	{
		for (int i = 0; i < this.conversations.Length; i++)
		{
			if (this.conversations[i].conversationID == conversationID)
			{
				this.currentConversation = i;
				this.currentSentence = 0;
				if (SpeechManager.currentVoiceLanguage == SpeechManager.VoiceLanguage.Arabic)
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
					this.stopToListenStartTime = Time.time;
					AnimationHandler.instance.gameObject.GetComponent<PlatformCharacterController>().acceptUserInput = false;
					AnimationHandler.instance.gameObject.GetComponent<NormalCharacterMotor>().canJump = false;
					SpeechManager.letterBox = true;
					AnimationHandler.instance.gameObject.GetComponent<WeaponHandling>().disableUsingWeapons = true;
					CutsceneManager.showGUI = false;
					WeaponsHUD weaponsHUD = (WeaponsHUD)UnityEngine.Object.FindObjectOfType(typeof(WeaponsHUD));
					if (weaponsHUD != null)
					{
						weaponsHUD.enabled = false;
					}
					if (PlatformCharacterController.joystickLeft != null)
					{
						PlatformCharacterController.joystickLeft.gameObject.SetActive(false);
					}
				}
				break;
			}
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00044814 File Offset: 0x00042A14
	public static void PlayConversation(string conversationID)
	{
		SpeechManager.instance.Play(conversationID);
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00044824 File Offset: 0x00042A24
	public void OnGUI()
	{
		if (mainmenu.pause || mainmenu.disableHUD)
		{
			return;
		}
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 1366f, (float)Screen.height / 768f, 1f));
		if (this.subtitleString != null && this.subtitleString != string.Empty)
		{
			string text = Language.Get(this.subtitleString, 50);
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(342.5f, 567f, 683f, 200f), text, this.style);
			GUI.Label(new Rect(342.5f, 569f, 683f, 200f), text, this.style);
			GUI.Label(new Rect(340.5f, 567f, 683f, 200f), text, this.style);
			GUI.Label(new Rect(340.5f, 569f, 683f, 200f), text, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(341.5f, 568f, 683f, 200f), text, this.style);
		}
		if (SpeechManager.displayCheckpointReached > 0f)
		{
			this.style.alignment = TextAnchor.MiddleCenter;
			string text2 = Language.Get("GP_CheckpointReached", 60);
			this.style.normal.textColor = Color.black;
			GUI.Label(new Rect(1f, 51f, 1366f, 100f), text2, this.style);
			this.style.normal.textColor = Color.white;
			GUI.Label(new Rect(0f, 50f, 1366f, 100f), text2, this.style);
		}
		if (SpeechManager.letterBox && this.blackTexture != null)
		{
			GUI.DrawTexture(new Rect(0f, 0f, 1366f, 25.6f), this.blackTexture, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(0f, 742.4f, 1366f, 25.6f), this.blackTexture, ScaleMode.StretchToFill);
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00044A90 File Offset: 0x00042C90
	public static Font getCurrentLanguageFont()
	{
		return Language.GetFont29();
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00044A98 File Offset: 0x00042C98
	public static void PlayCheckpointSound()
	{
		if (SpeechManager.instance != null && SpeechManager.instance.checkpointReachedSound != null)
		{
			SpeechManager.instance.defaultAudioSource.PlayOneShot(SpeechManager.instance.checkpointReachedSound, SpeechManager.sfxVolume);
		}
	}

	// Token: 0x04000AE1 RID: 2785
	public static SpeechManager.VoiceLanguage currentVoiceLanguage = SpeechManager.VoiceLanguage.English;

	// Token: 0x04000AE2 RID: 2786
	public static bool enableSubtitles = true;

	// Token: 0x04000AE3 RID: 2787
	public static bool enable3D;

	// Token: 0x04000AE4 RID: 2788
	public static bool previousEnable3d;

	// Token: 0x04000AE5 RID: 2789
	public static float musicVolume = 1f;

	// Token: 0x04000AE6 RID: 2790
	public static float speechVolume = 1f;

	// Token: 0x04000AE7 RID: 2791
	public static float sfxVolume = 1f;

	// Token: 0x04000AE8 RID: 2792
	public AudioSource defaultAudioSource;

	// Token: 0x04000AE9 RID: 2793
	public AnimationClip defaultFacialAnim;

	// Token: 0x04000AEA RID: 2794
	public SpeechManager.Conversation[] conversations;

	// Token: 0x04000AEB RID: 2795
	[HideInInspector]
	public static SpeechManager instance;

	// Token: 0x04000AEC RID: 2796
	private int currentConversation;

	// Token: 0x04000AED RID: 2797
	private int currentSentence;

	// Token: 0x04000AEE RID: 2798
	public bool playing;

	// Token: 0x04000AEF RID: 2799
	private float currentSentenceTimer;

	// Token: 0x04000AF0 RID: 2800
	private float newSentanceDelayTimer;

	// Token: 0x04000AF1 RID: 2801
	private bool stopPlaying;

	// Token: 0x04000AF2 RID: 2802
	public string subtitleString;

	// Token: 0x04000AF3 RID: 2803
	public static float displayCheckpointReached;

	// Token: 0x04000AF4 RID: 2804
	private AudioSource currentAudioSource;

	// Token: 0x04000AF5 RID: 2805
	public Texture blackTexture;

	// Token: 0x04000AF6 RID: 2806
	public static bool letterBox;

	// Token: 0x04000AF7 RID: 2807
	public AudioClip checkpointReachedSound;

	// Token: 0x04000AF8 RID: 2808
	private GUIStyle style;

	// Token: 0x04000AF9 RID: 2809
	private float stopToListenStartTime;

	// Token: 0x04000AFA RID: 2810
	private float timeTouchBegan;

	// Token: 0x0200018E RID: 398
	[Serializable]
	public class Conversation
	{
		// Token: 0x04000AFB RID: 2811
		public string conversationID;

		// Token: 0x04000AFC RID: 2812
		public bool stopToListen;

		// Token: 0x04000AFD RID: 2813
		public SpeechManager.Sentence[] sentences;
	}

	// Token: 0x0200018F RID: 399
	[Serializable]
	public class Sentence
	{
		// Token: 0x04000AFE RID: 2814
		public float englishDelay;

		// Token: 0x04000AFF RID: 2815
		public float arabicDelay;

		// Token: 0x04000B00 RID: 2816
		public AudioSource speaker;

		// Token: 0x04000B01 RID: 2817
		public AudioClip englishSpeech;

		// Token: 0x04000B02 RID: 2818
		public AudioClip arabicSpeech;

		// Token: 0x04000B03 RID: 2819
		public string subtitleKeyword;

		// Token: 0x04000B04 RID: 2820
		public AnimationClip facialAnim;

		// Token: 0x04000B05 RID: 2821
		public Transform lookAtObject;

		// Token: 0x04000B06 RID: 2822
		public bool nullify = true;
	}

	// Token: 0x02000190 RID: 400
	public enum VoiceLanguage
	{
		// Token: 0x04000B08 RID: 2824
		Arabic,
		// Token: 0x04000B09 RID: 2825
		English
	}
}
