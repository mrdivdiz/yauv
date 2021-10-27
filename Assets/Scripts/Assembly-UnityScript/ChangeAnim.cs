using System;
using System.Collections;
using Boo.Lang.Runtime;
using CompilerGenerated;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x02000017 RID: 23
[Serializable]
public class ChangeAnim : MonoBehaviour
{
	// Token: 0x06000042 RID: 66 RVA: 0x00002B90 File Offset: 0x00000D90
	public virtual void Update()
	{
		//__ChangeAnim_Updatecallable07_35__ _ChangeAnim_Updatecallable07_35__ = new __ChangeAnim_Updatecallable07_35__(this.GetComponent<Animation>().GetClipCount);
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.GetComponent<Animation>());
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				object obj3;
				object obj2 = obj3 = obj;
				if (!(obj2 is AnimationState))
				{
					obj3 = RuntimeServices.Coerce(obj2, typeof(AnimationState));
				}
				AnimationState animationState = (AnimationState)obj3;
				animationState.speed += 0.2f;
				UnityRuntimeServices.Update(enumerator, animationState);
			}
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(this.GetComponent<Animation>());
			while (enumerator2.MoveNext())
			{
				object obj4 = enumerator2.Current;
				object obj6;
				object obj5 = obj6 = obj4;
				if (!(obj5 is AnimationState))
				{
					obj6 = RuntimeServices.Coerce(obj5, typeof(AnimationState));
				}
				AnimationState animationState2 = (AnimationState)obj6;
				animationState2.speed -= 0.2f;
				UnityRuntimeServices.Update(enumerator2, animationState2);
			}
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (this.state == 0 && this.subState == 0)
			{
				this.GetComponent<Animation>().CrossFade("walk", 0.2f);
				this.state = 1;
			}
			else if (this.state == 1 && this.subState == 0)
			{
				this.GetComponent<Animation>().CrossFade("run", 0.2f);
				this.state = 2;
			}
			else if (this.state == 2 && this.subState == 0)
			{
				this.subState = 1;
				this.GetComponent<Animation>().CrossFade("sniffA", 0.2f);
			}
			else if (this.state == 2 && this.subState == 2)
			{
				this.subState = 3;
				this.GetComponent<Animation>().CrossFade("sniffC", 0.2f);
			}
		}
		if (this.state == 2 && this.subState == 1)
		{
			if (!this.GetComponent<Animation>().isPlaying)
			{
				this.subState = 2;
				this.GetComponent<Animation>().Play("sniffB");
			}
		}
		else if (this.state == 2 && this.subState == 3 && !this.GetComponent<Animation>().isPlaying)
		{
			this.GetComponent<Animation>().CrossFade("walk", 0.2f);
			this.subState = 0;
			this.state = 1;
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002E08 File Offset: 0x00001008
	public virtual void Main()
	{
	}

	// Token: 0x0400001C RID: 28
	public int currentClip;

	// Token: 0x0400001D RID: 29
	public int state;

	// Token: 0x0400001E RID: 30
	public int subState;
}
