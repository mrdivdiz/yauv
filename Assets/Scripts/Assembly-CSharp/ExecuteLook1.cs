using System;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class ExecuteLook1 : MonoBehaviour
{
	// Token: 0x06000973 RID: 2419 RVA: 0x00055008 File Offset: 0x00053208
	private void Start()
	{
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0005500C File Offset: 0x0005320C
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

	// Token: 0x06000975 RID: 2421 RVA: 0x00055070 File Offset: 0x00053270
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
					this.playedLookAtThatSound = true;
				}
			}
			this.CharacterLook.targetTransform = this.TargetObject.transform;
		}
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00055210 File Offset: 0x00053410
	public void OnTriggerExit(Collider other)
	{
		if (this.CharacterLook != null && other.tag == "Player")
		{
			this.CharacterLook.targetTransform = null;
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00055250 File Offset: 0x00053450
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
					component2.FarisHead.PlayOneShot(component2.lookAtThatSounds[ExecuteLook1.currentlookAtThatSound], SpeechManager.speechVolume);
					ExecuteLook1.currentlookAtThatSound = (ExecuteLook1.currentlookAtThatSound + 1) % component2.lookAtThatSounds.Length;
					this.playedLookAtThatSound = true;
				}
			}
		}
	}

	// Token: 0x04000D94 RID: 3476
	private HeadLookController CharacterLook;

	// Token: 0x04000D95 RID: 3477
	public GameObject TargetObject;

	// Token: 0x04000D96 RID: 3478
	public Health PlayerObject;

	// Token: 0x04000D97 RID: 3479
	public bool changingValue;

	// Token: 0x04000D98 RID: 3480
	public float desiredValue;

	// Token: 0x04000D99 RID: 3481
	public AudioClip voice;

	// Token: 0x04000D9A RID: 3482
	public AnimationClip facialAnim;

	// Token: 0x04000D9B RID: 3483
	public string conversationID;

	// Token: 0x04000D9C RID: 3484
	public Transform head;

	// Token: 0x04000D9D RID: 3485
	private float effectIn;

	// Token: 0x04000D9E RID: 3486
	private float effectOut;

	// Token: 0x04000D9F RID: 3487
	private bool playedLookAtThatSound;

	// Token: 0x04000DA0 RID: 3488
	private static int currentlookAtThatSound;
}
