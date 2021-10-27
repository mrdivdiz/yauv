using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
[Serializable]
public class RainGlobalControl : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x00002674 File Offset: 0x00000874
	public RainGlobalControl()
	{
		this.transitionSpeed = 2;
		this.rainSlider = 0.5f;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0000269C File Offset: 0x0000089C
	public virtual void Update()
	{
		this.rainSlider = Input.GetAxis("RainAmount");
		RainGlobalControl.rainLevel = Mathf.Clamp(this.rainSlider, (float)0, 1f);
		float t = Time.deltaTime * (float)this.transitionSpeed;
		GameObject[] array = GameObject.FindGameObjectsWithTag("RainParticles");
		int i = 0;
		GameObject[] array2 = array;
		int length = array2.Length;
		while (i < length)
		{
			float a = array2[i].GetComponent<Renderer>().material.color.a;
			float a2 = Mathf.Lerp(a, RainGlobalControl.rainLevel * 0.2f, t);
			Color color = array2[i].GetComponent<Renderer>().material.color;
			float num = color.a = a2;
			Color color2 = array2[i].GetComponent<Renderer>().material.color = color;
			i++;
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("RainPro");
		int j = 0;
		GameObject[] array4 = array3;
		int length2 = array4.Length;
		while (j < length2)
		{
			if (RainGlobalControl.rainLevel == (float)0)
			{
				array4[j].GetComponent<Renderer>().enabled = false;
			}
			else
			{
				array4[j].GetComponent<Renderer>().enabled = true;
			}
			float @float = array4[j].GetComponent<Renderer>().material.GetFloat("_BumpAmt");
			array4[j].GetComponent<Renderer>().material.SetFloat("_BumpAmt", Mathf.Lerp(@float, RainGlobalControl.rainLevel * (float)60, t));
			j++;
		}
		GameObject[] array5 = GameObject.FindGameObjectsWithTag("RainRipples");
		int k = 0;
		GameObject[] array6 = array5;
		int length3 = array6.Length;
		while (k < length3)
		{
			float a3 = array6[k].GetComponent<Renderer>().material.color.a;
			float a4 = Mathf.Lerp(a3, RainGlobalControl.rainLevel * 0.5f, t);
			Color color3 = array6[k].GetComponent<Renderer>().material.color;
			float num2 = color3.a = a4;
			Color color4 = array6[k].GetComponent<Renderer>().material.color = color3;
			k++;
		}
		GameObject[] array7 = GameObject.FindGameObjectsWithTag("RainSound");
		int l = 0;
		GameObject[] array8 = array7;
		int length4 = array8.Length;
		while (l < length4)
		{
			float volume = array8[l].GetComponent<AudioSource>().volume;
			float pitch = array8[l].GetComponent<AudioSource>().pitch;
			array8[l].GetComponent<AudioSource>().volume = Mathf.Lerp(volume, RainGlobalControl.rainLevel, t);
			l++;
		}
		GameObject[] array9 = GameObject.FindGameObjectsWithTag("FullscreenEffectPro");
		Vector3 up = Vector3.up;
		int m = 0;
		GameObject[] array10 = array9;
		int length5 = array10.Length;
		while (m < length5)
		{
			if (RainGlobalControl.rainLevel <= 0.7f)
			{
				array10[m].GetComponent<Renderer>().enabled = false;
			}
			else if (Physics.Raycast(Camera.main.transform.position, up, (float)100))
			{
				array10[m].GetComponent<Renderer>().enabled = false;
			}
			else
			{
				array10[m].GetComponent<Renderer>().enabled = true;
			}
			m++;
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000029E0 File Offset: 0x00000BE0
	public virtual void Main()
	{
	}

	// Token: 0x0400000F RID: 15
	public int transitionSpeed;

	// Token: 0x04000010 RID: 16
	[NonSerialized]
	public static float rainLevel = (float)1;

	// Token: 0x04000011 RID: 17
	private float rainSlider;
}
