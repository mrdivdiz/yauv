using System;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x02000032 RID: 50
[Serializable]
public class Footsteps : MonoBehaviour
{
	// Token: 0x06000091 RID: 145 RVA: 0x00003AF0 File Offset: 0x00001CF0
	public virtual void Start()
	{
		this.cc = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.t = this.transform;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00003B1C File Offset: 0x00001D1C
	public virtual void OnFootStrikeVol(float vol)
	{
		if (Time.time >= 0.5f)
		{
			if (this.cc != null)
			{
				this.volume = Mathf.Clamp01(0.1f + this.cc.velocity.magnitude * 0.3f);
			}
			else
			{
				this.volume = (float)1;
			}
			this.footAudioSource.PlayOneShot(this.GetAudio(), this.volume * vol);
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00003BA0 File Offset: 0x00001DA0
	public virtual void OnFootStrike()
	{
		this.OnFootStrikeVol(0.25f);
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00003BB0 File Offset: 0x00001DB0
	public virtual AudioClip GetAudio()
	{
		RaycastHit raycastHit = default(RaycastHit);
		if (Physics.Raycast(this.t.position + new Vector3((float)0, 0.5f, (float)0), -Vector3.up, out raycastHit, float.PositiveInfinity))
		{
			this.cTag = raycastHit.collider.tag.ToLower();
		}
		AudioClip result;
		if (this.cTag == "wood")
		{
			result = this.woodSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.woodSteps))];
		}
		else if (this.cTag == "metal")
		{
			result = this.metalSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.metalSteps))];
		}
		else if (this.cTag == "concrete")
		{
			this.volume = 1f;
			result = this.concreteSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.concreteSteps))];
		}
		else if (this.cTag == "dirt")
		{
			this.volume = 1f;
			result = this.dirtSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.dirtSteps))];
		}
		else if (this.cTag == "sand")
		{
			this.volume = 1f;
			result = this.sandSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.sandSteps))];
		}
		else if (this.cTag == "carpet")
		{
			this.volume = 1f;
			result = this.carpetSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.sandSteps))];
		}
		else if (this.cTag == "mud")
		{
			this.volume = 1f;
			result = this.mudSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.mudSteps))];
		}
		else if (this.cTag == "grass")
		{
			this.volume = 1f;
			result = this.grassSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.grassSteps))];
		}
		else if (this.cTag == "gravel")
		{
			this.volume = 1f;
			result = this.gravelSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.gravelSteps))];
		}
		else if (this.cTag == "water")
		{
			this.volume = 1f;
			if (this.waterParticles != null)
			{
				UnityEngine.Object.Instantiate(this.waterParticles, this.t.position, Quaternion.identity);
			}
			result = this.waterSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.waterSteps))];
		}
		else
		{
			this.volume = 1f;
			result = this.concreteSteps[UnityEngine.Random.Range(0, Extensions.get_length(this.concreteSteps))];
		}
		return result;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00003EB8 File Offset: 0x000020B8
	public virtual void Main()
	{
	}

	// Token: 0x0400004C RID: 76
	public Transform spine;

	// Token: 0x0400004D RID: 77
	public AudioSource footAudioSource;

	// Token: 0x0400004E RID: 78
	public AudioClip[] woodSteps;

	// Token: 0x0400004F RID: 79
	public AudioClip[] metalSteps;

	// Token: 0x04000050 RID: 80
	public AudioClip[] concreteSteps;

	// Token: 0x04000051 RID: 81
	public AudioClip[] sandSteps;

	// Token: 0x04000052 RID: 82
	public AudioClip[] carpetSteps;

	// Token: 0x04000053 RID: 83
	public AudioClip[] gravelSteps;

	// Token: 0x04000054 RID: 84
	public AudioClip[] grassSteps;

	// Token: 0x04000055 RID: 85
	public AudioClip[] mudSteps;

	// Token: 0x04000056 RID: 86
	public AudioClip[] waterSteps;

	// Token: 0x04000057 RID: 87
	public AudioClip[] dirtSteps;

	// Token: 0x04000058 RID: 88
	private float volume;

	// Token: 0x04000059 RID: 89
	private CharacterController cc;

	// Token: 0x0400005A RID: 90
	private Transform t;

	// Token: 0x0400005B RID: 91
	public LayerMask hitLayer;

	// Token: 0x0400005C RID: 92
	private string cTag;

	// Token: 0x0400005D RID: 93
	public Transform waterParticles;
}
