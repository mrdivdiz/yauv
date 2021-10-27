using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class RunningThief : MonoBehaviour
{
	// Token: 0x06000825 RID: 2085 RVA: 0x000423FC File Offset: 0x000405FC
	private void Start()
	{
		this.cc = base.gameObject.GetComponent<CharacterController>();
		if (this.playOnStart)
		{
			this.play = true;
		}
		base.GetComponent<Animation>()["General-Run"].wrapMode = WrapMode.Loop;
		if (!this.playOnStart)
		{
			foreach (Renderer renderer in base.transform.GetComponentsInChildren<Renderer>())
			{
				renderer.enabled = false;
			}
		}
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00042478 File Offset: 0x00040678
	private void FixedUpdate()
	{
		if (this.play)
		{
			switch (this.steps[this.currentStep].stepType)
			{
			case RunningThief.StepType.WAYPOINT:
			{
				if (this.steps[this.currentStep].anim == null)
				{
					if (this.cc.isGrounded)
					{
						if (!base.GetComponent<Animation>().IsPlaying("General-Run"))
						{
							base.GetComponent<Animation>().CrossFade("General-Run");
						}
					}
					else if (!base.GetComponent<Animation>().IsPlaying("General-Fall-Medium"))
					{
						base.GetComponent<Animation>().CrossFade("General-Fall-Medium", 0.1f);
					}
					this.MoveToPosition(this.steps[this.currentStep].waypoint.transform.position, this.runningSpeed);
				}
				else
				{
					if (!base.GetComponent<Animation>().IsPlaying(this.steps[this.currentStep].anim.name))
					{
						base.GetComponent<Animation>().CrossFade(this.steps[this.currentStep].anim.name);
					}
					this.MoveToPosition(this.steps[this.currentStep].waypoint.transform.position, this.steps[this.currentStep].customSpeed);
				}
				Vector3 position = base.transform.position;
				position.y = 0f;
				Vector3 position2 = this.steps[this.currentStep].waypoint.transform.position;
				position2.y = 0f;
				if (Vector3.Distance(position, position2) < 0.1f)
				{
					this.NextStep();
				}
				break;
			}
			case RunningThief.StepType.ANIM:
				if (!base.GetComponent<Animation>().IsPlaying(this.steps[this.currentStep].anim.name))
				{
					if (!this.animationPlayed)
					{
						if (this.steps[this.currentStep].adjustmentVector != Vector3.zero)
						{
							base.transform.position += this.steps[this.currentStep].adjustmentVector;
						}
						base.GetComponent<Animation>()[this.steps[this.currentStep].anim.name].wrapMode = WrapMode.Once;
						base.GetComponent<Animation>().CrossFade(this.steps[this.currentStep].anim.name);
						this.animationPlayed = true;
					}
					else
					{
						if (this.steps[this.currentStep].rootMotion)
						{
							this.RootMotion();
						}
						this.NextStep();
					}
				}
				break;
			case RunningThief.StepType.LOOPANIM:
				if (this.steps[this.currentStep].duration >= 0f)
				{
					if (!base.GetComponent<Animation>().IsPlaying(this.steps[this.currentStep].anim.name))
					{
						base.GetComponent<Animation>()[this.steps[this.currentStep].anim.name].wrapMode = WrapMode.Loop;
						base.GetComponent<Animation>().CrossFade(this.steps[this.currentStep].anim.name);
					}
					this.MoveUpward(this.steps[this.currentStep].customSpeed);
					this.steps[this.currentStep].duration -= Time.deltaTime;
				}
				else
				{
					if (this.steps[this.currentStep].rootMotion)
					{
						this.RootMotion();
					}
					this.NextStep();
				}
				break;
			}
		}
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x0004282C File Offset: 0x00040A2C
	private void NextStep()
	{
		if (this.currentStep <= this.steps.Length - 2)
		{
			this.currentStep++;
			this.animationPlayed = false;
		}
		else
		{
			this.play = false;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x0004287C File Offset: 0x00040A7C
	private void RootMotion()
	{
		Vector3 position = base.transform.Find("Bip01/Root").transform.position;
		base.GetComponent<Animation>().Stop();
		base.GetComponent<Animation>().Play("General-Run");
		base.transform.position = position;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000428CC File Offset: 0x00040ACC
	private void MoveToPosition(Vector3 target, float speed)
	{
		target.y = base.transform.position.y;
		base.transform.LookAt(target);
		Vector3 a = Vector3.zero;
		if (this.cc.isGrounded)
		{
			a = (target - base.transform.position).normalized;
			a *= speed;
		}
		a.y -= 5f;
		this.cc.Move(a * Time.deltaTime);
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00042964 File Offset: 0x00040B64
	private void MoveUpward(float speed)
	{
		Vector3 zero = Vector3.zero;
		zero.y += 1f * speed;
		this.cc.Move(zero * Time.deltaTime);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x000429A4 File Offset: 0x00040BA4
	internal void Play()
	{
		this.play = true;
		foreach (Renderer renderer in base.transform.GetComponentsInChildren<Renderer>())
		{
			renderer.enabled = true;
		}
	}

	// Token: 0x04000AB3 RID: 2739
	private float runningSpeed = 7f;

	// Token: 0x04000AB4 RID: 2740
	public bool playOnStart;

	// Token: 0x04000AB5 RID: 2741
	public RunningThief.Step[] steps;

	// Token: 0x04000AB6 RID: 2742
	private int currentStep;

	// Token: 0x04000AB7 RID: 2743
	private bool play;

	// Token: 0x04000AB8 RID: 2744
	private CharacterController cc;

	// Token: 0x04000AB9 RID: 2745
	private bool animationPlayed;

	// Token: 0x0200018A RID: 394
	public enum StepType
	{
		// Token: 0x04000ABB RID: 2747
		WAYPOINT,
		// Token: 0x04000ABC RID: 2748
		ANIM,
		// Token: 0x04000ABD RID: 2749
		LOOPANIM
	}

	// Token: 0x0200018B RID: 395
	[Serializable]
	public class Step
	{
		// Token: 0x04000ABE RID: 2750
		public RunningThief.StepType stepType;

		// Token: 0x04000ABF RID: 2751
		public WayPoint waypoint;

		// Token: 0x04000AC0 RID: 2752
		public float customSpeed;

		// Token: 0x04000AC1 RID: 2753
		public AnimationClip anim;

		// Token: 0x04000AC2 RID: 2754
		public bool rootMotion = true;

		// Token: 0x04000AC3 RID: 2755
		public float duration;

		// Token: 0x04000AC4 RID: 2756
		public Vector3 adjustmentVector = Vector3.zero;
	}
}
