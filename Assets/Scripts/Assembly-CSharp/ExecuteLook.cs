using System;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class ExecuteLook : MonoBehaviour
{
	// Token: 0x0600096C RID: 2412 RVA: 0x00054C8C File Offset: 0x00052E8C
	private void Start()
	{
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00054C90 File Offset: 0x00052E90
	private void Update()
	{
		if (this.changingValue)
		{
			this.CharacterLook.effect = Mathf.SmoothDamp(this.CharacterLook.effect, this.desiredValue, ref this.effectIn, 0.5f);
			if (this.CharacterLook.effect == this.desiredValue)
			{
				this.changingValue = false;
			}
		}
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00054CF4 File Offset: 0x00052EF4
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.CharacterLook = (other.GetComponent(typeof(HeadLookController)) as HeadLookController);
			if (!this.playedLookAtThatSound)
			{
				AnimationHandler component = other.GetComponent<AnimationHandler>();
				if (!component.isholdingWeapon && !component.stealthMode)
				{
					BasicAgility component2 = other.GetComponent<BasicAgility>();
					if (this.conversationID != null && this.conversationID != string.Empty && SpeechManager.instance != null)
					{
						SpeechManager.PlayConversation(this.conversationID);
					}
					else if (this.voice != null)
					{
						component2.FarisHead.PlayOneShot(this.voice, SpeechManager.speechVolume);
						if (this.facialAnim != null)
						{
							this.CharacterLook.gameObject.GetComponent<Animation>()[this.facialAnim.name].AddMixingTransform(this.head);
							this.CharacterLook.gameObject.GetComponent<Animation>()[this.facialAnim.name].layer = 2;
							this.CharacterLook.gameObject.GetComponent<Animation>()[this.facialAnim.name].wrapMode = WrapMode.Once;
							this.CharacterLook.gameObject.GetComponent<Animation>().Blend(this.facialAnim.name);
						}
					}
					else
					{
						component2.FarisHead.PlayOneShot(component2.lookAtThatSounds[ExecuteLook.currentlookAtThatSound], SpeechManager.speechVolume);
						ExecuteLook.currentlookAtThatSound = (ExecuteLook.currentlookAtThatSound + 1) % component2.lookAtThatSounds.Length;
					}
					this.playedLookAtThatSound = true;
				}
			}
			this.CharacterLook.targetTransform = this.TargetObject.transform;
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00054EC4 File Offset: 0x000530C4
	public void OnTriggerExit(Collider other)
	{
		if (this.CharacterLook != null && other.tag == "Player")
		{
			this.CharacterLook.targetTransform = null;
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00054F04 File Offset: 0x00053104
	public void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && !this.playedLookAtThatSound)
		{
			AnimationHandler component = other.GetComponent<AnimationHandler>();
			if (!component.isholdingWeapon && !component.stealthMode)
			{
				BasicAgility component2 = other.GetComponent<BasicAgility>();
				if (this.conversationID != null && this.conversationID != string.Empty && SpeechManager.instance != null)
				{
					SpeechManager.PlayConversation(this.conversationID);
				}
				else if (this.voice != null)
				{
					component2.FarisHead.PlayOneShot(this.voice, SpeechManager.speechVolume);
				}
				else
				{
					component2.FarisHead.PlayOneShot(component2.lookAtThatSounds[ExecuteLook.currentlookAtThatSound], SpeechManager.speechVolume);
					ExecuteLook.currentlookAtThatSound = (ExecuteLook.currentlookAtThatSound + 1) % component2.lookAtThatSounds.Length;
					this.playedLookAtThatSound = true;
				}
			}
		}
	}

	// Token: 0x04000D87 RID: 3463
	private HeadLookController CharacterLook;

	// Token: 0x04000D88 RID: 3464
	public GameObject TargetObject;

	// Token: 0x04000D89 RID: 3465
	public Health PlayerObject;

	// Token: 0x04000D8A RID: 3466
	public bool changingValue;

	// Token: 0x04000D8B RID: 3467
	public float desiredValue;

	// Token: 0x04000D8C RID: 3468
	public AudioClip voice;

	// Token: 0x04000D8D RID: 3469
	public AnimationClip facialAnim;

	// Token: 0x04000D8E RID: 3470
	public string conversationID;

	// Token: 0x04000D8F RID: 3471
	public Transform head;

	// Token: 0x04000D90 RID: 3472
	private float effectIn;

	// Token: 0x04000D91 RID: 3473
	private float effectOut;

	// Token: 0x04000D92 RID: 3474
	private bool playedLookAtThatSound;

	// Token: 0x04000D93 RID: 3475
	private static int currentlookAtThatSound;
}
