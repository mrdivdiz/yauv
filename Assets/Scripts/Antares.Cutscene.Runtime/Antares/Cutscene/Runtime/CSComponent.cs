using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x0200002A RID: 42
	public class CSComponent : MonoBehaviour
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00007B4C File Offset: 0x00005D4C
		internal static RenderTexture EffectRenderTexture
		{
			get
			{
				if (CSComponent.j == null)
				{
					CSComponent.j = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
					CSComponent.j.hideFlags = HideFlags.HideAndDontSave;
				}
				return CSComponent.j;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007B83 File Offset: 0x00005D83
		internal static Texture2D EffectTexture
		{
			get
			{
				if (CSComponent.k == null)
				{
					CSComponent.k = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
					CSComponent.k.hideFlags = HideFlags.HideAndDontSave;
				}
				return CSComponent.k;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00007BBC File Offset: 0x00005DBC
		public void Init()
		{
			CutsceneObjectBase[] componentsInChildren = this.GetComponentsInChildren<CutsceneObjectBase>();
			this.cdict.Clear();
			foreach (CutsceneObjectBase cutsceneObjectBase in componentsInChildren)
			{
				if (!this.cdict.ContainsKey(cutsceneObjectBase.gameObject))
				{
					this.cdict.Add(cutsceneObjectBase.gameObject, new List<CutsceneObjectBase>());
				}
				this.cdict[cutsceneObjectBase.gameObject].Add(cutsceneObjectBase);
				cutsceneObjectBase.Init();
			}
			foreach (DirectorCameraTrack directorCameraTrack in base.GetComponentsInChildren<DirectorCameraTrack>())
			{
				foreach (DirectorCameraKey directorCameraKey in directorCameraTrack.Keys)
				{
					if (directorCameraKey != null && directorCameraKey.keyCamera != null)
					{
						directorCameraKey.keyCamera.gameObject.active = false;
					}
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00007CB2 File Offset: 0x00005EB2
		public void Awake()
		{
			base.useGUILayout = false;
			this.Init();
			if (this.playOnLoad)
			{
				this.StartCutscene();
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00007CCF File Offset: 0x00005ECF
		public bool IsPlaying
		{
			get
			{
				return beatha;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00007CD7 File Offset: 0x00005ED7
		public bool IsSkipped
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00007CDF File Offset: 0x00005EDF
		public bool IsPaused
		{
			get
			{
				return this.l;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007CE7 File Offset: 0x00005EE7
		public bool IsFinished
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00007CEF File Offset: 0x00005EEF
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00007CF7 File Offset: 0x00005EF7
		public float PlayingTime
		{
			get
			{
				return this.alepta;
			}
			set
			{
				this.alepta = value;
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007D00 File Offset: 0x00005F00
		public void Pause()
		{
			this.l = true;
			AudioSource[] componentsInChildren = base.GetComponentsInChildren<AudioSource>();
			this.n.Clear();
			foreach (AudioSource audioSource in componentsInChildren)
			{
				if (audioSource.isPlaying)
				{
					audioSource.Pause();
					CSDestroyObjectAterPlaySoundInternal component = audioSource.GetComponent<CSDestroyObjectAterPlaySoundInternal>();
					if (component != null)
					{
						component.paused = true;
					}
					this.n.Add(audioSource);
				}
			}
			foreach (SoundTrack soundTrack in base.GetComponentsInChildren<SoundTrack>())
			{
				AudioSource controlledObject = soundTrack.controlledObject;
				if (controlledObject.isPlaying)
				{
					controlledObject.Pause();
					CSDestroyObjectAterPlaySoundInternal component2 = controlledObject.GetComponent<CSDestroyObjectAterPlaySoundInternal>();
					if (component2 != null)
					{
						component2.paused = true;
					}
					this.n.Add(controlledObject);
				}
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007DD4 File Offset: 0x00005FD4
		public void Resume()
		{
			this.l = false;
			foreach (AudioSource audioSource in this.n)
			{
				if (audioSource != null)
				{
					CSDestroyObjectAterPlaySoundInternal component = base.GetComponent<CSDestroyObjectAterPlaySoundInternal>();
					if (component != null)
					{
						component.paused = false;
					}
					audioSource.Play();
				}
			}
			this.n.Clear();
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007E58 File Offset: 0x00006058
		private void b2f(Camera A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.gameObject.active = true;
				A_0.enabled = A_1;
				A_0.rect = new Rect(0f, 0f, 1f, 1f);
				AudioListener component = A_0.GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = A_1;
				}
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007EB8 File Offset: 0x000060B8
		private void a2f(Camera A_0, bool A_1)
		{
			if (A_0 != null)
			{
				AudioListener component = A_0.GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = A_1;
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00007EE5 File Offset: 0x000060E5
		private static Material ShapeMaterial
		{
			get
			{
				if (!CSComponent.o)
				{
					CSComponent.o = new Material("Shader \"DepthMask\" {   SubShader {\t   Tags { \"Queue\" = \"Background\" }\t   Lighting Off ZTest Always ZWrite On Cull Off \t   Pass {}   }}");
					CSComponent.o.hideFlags = HideFlags.HideAndDontSave;
				}
				return CSComponent.o;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00007F13 File Offset: 0x00006113
		private static Material DreamWipeMaterial
		{
			get
			{
				if (CSComponent.p == null)
				{
					CSComponent.p = new Material(Resources.Load("DreamWipe") as Shader);
					CSComponent.p.hideFlags = HideFlags.HideAndDontSave;
				}
				return CSComponent.p;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00007F4C File Offset: 0x0000614C
		private static Material ShapeWipeMaterial
		{
			get
			{
				if (CSComponent.q == null)
				{
					CSComponent.q = new Material(Resources.Load("ShapeWipe") as Shader);
					CSComponent.q.hideFlags = HideFlags.HideAndDontSave;
				}
				return CSComponent.q;
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007F88 File Offset: 0x00006188
		private static void a6f(Camera A_0, Camera A_1, float A_2, Mesh A_3, bool A_4, float A_5)
		{
			RenderTexture.active = CSComponent.EffectRenderTexture;
			Camera camera = A_4 ? A_0 : A_1;
			Camera camera2 = A_4 ? A_1 : A_0;
			Camera camera3 = new GameObject("tempCam").AddComponent<Camera>();
			camera3.clearFlags = CameraClearFlags.Color;
			camera3.backgroundColor = Color.black;
			camera3.cullingMask = 0;
			if (camera2 == null)
			{
				camera2 = camera3;
			}
			if (camera == null)
			{
				camera = camera3;
			}
			Vector3 one = Vector3.one;
			Quaternion quaternion = Quaternion.identity;
			if (A_4)
			{
				A_2 = 1f - A_2;
				float num = Mathf.Lerp(1f, 0f, Mathf.Sin((1f - A_2) * 3.1415927f * 0.5f));
				one = new Vector3(num, num, num);
				quaternion = Quaternion.Euler(new Vector3(0f, 0f, A_2 * A_5));
			}
			else
			{
				float num2 = Mathf.Lerp(1f, 0f, Mathf.Sin((1f - A_2) * 3.1415927f * 0.5f));
				one = new Vector3(num2, num2, num2);
				quaternion = Quaternion.Euler(new Vector3(0f, 0f, -A_2 * A_5));
			}
			RenderTexture.active = CSComponent.EffectRenderTexture;
			RenderTexture targetTexture = camera.targetTexture;
			camera.targetTexture = CSComponent.EffectRenderTexture;
			if (camera3 == camera)
			{
				;
			}
			else
			{
				camera.Render();
			}
			camera.targetTexture = targetTexture;
			RenderTexture.active = null;
			RenderTexture temporary = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
			RenderTexture temporary2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
			RenderTexture.active = temporary;
			GL.Clear(true, true, Color.white);
			if (A_3 != null)
			{
				GL.PushMatrix();
				GL.LoadProjectionMatrix(camera2.projectionMatrix);
				CSComponent.ShapeMaterial.SetPass(0);
				Graphics.DrawMeshNow(A_3, Matrix4x4.TRS(-Vector3.forward * 1.01f * camera2.near, quaternion, one));
				GL.PopMatrix();
			}
			else
			{
				Debug.Log("Cutscene: Shape Mesh == null");
			}
			RenderTexture.active = null;
			CSComponent.a2frt(camera2, temporary2);
			CSComponent.ShapeWipeMaterial.SetTexture("_Camera1", temporary2);
			CSComponent.ShapeWipeMaterial.SetTexture("_Mask", temporary);
			Graphics.Blit(null, CSComponent.EffectRenderTexture, CSComponent.ShapeWipeMaterial);
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			UnityEngine.Object.Destroy(camera3.gameObject);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000081EC File Offset: 0x000063EC
		private static void a6ft(Camera A_0, Camera A_1, float A_2, float A_3 = 0.1f, float A_4 = 50f, float A_5 = 2f)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
			RenderTexture temporary2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
			CSComponent.a2frt(A_0, temporary);
			CSComponent.a2frt(A_1, temporary2);
			CSComponent.DreamWipeMaterial.SetTexture("_Camera1", temporary);
			CSComponent.DreamWipeMaterial.SetTexture("_Camera2", temporary2);
			CSComponent.DreamWipeMaterial.SetFloat("_T", Mathf.SmoothStep(0f, 1f, A_2));
			CSComponent.DreamWipeMaterial.SetFloat("_WaveScale", A_3);
			CSComponent.DreamWipeMaterial.SetFloat("_WaveFrequency", A_4);
			CSComponent.DreamWipeMaterial.SetFloat("_AnimationFrequency", A_5);
			Graphics.Blit(null, CSComponent.EffectRenderTexture, CSComponent.DreamWipeMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000082BC File Offset: 0x000064BC
		private static void a1f(Camera A_0)
		{
			if (A_0 != null)
			{
				RenderTexture.active = CSComponent.EffectRenderTexture;
				RenderTexture targetTexture = A_0.targetTexture;
				A_0.targetTexture = CSComponent.EffectRenderTexture;
				A_0.Render();
				A_0.targetTexture = targetTexture;
				RenderTexture.active = null;
				return;
			}
			A_0 = new GameObject("tempCam").AddComponent<Camera>();
			A_0.clearFlags = CameraClearFlags.Color;
			A_0.backgroundColor = Color.black;
			RenderTexture.active = CSComponent.EffectRenderTexture;
			A_0.cullingMask = 0;
			;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(A_0.gameObject);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008350 File Offset: 0x00006550
		private static void a2frt(Camera A_0, RenderTexture A_1)
		{
			if (A_0 != null)
			{
				RenderTexture.active = A_1;
				RenderTexture targetTexture = A_0.targetTexture;
				A_0.targetTexture = A_1;
				A_0.Render();
				A_0.targetTexture = targetTexture;
				RenderTexture.active = null;
				return;
			}
			A_0 = new GameObject("tempCam").AddComponent<Camera>();
			A_0.clearFlags = CameraClearFlags.Color;
			A_0.backgroundColor = Color.black;
			RenderTexture.active = A_1;
			A_0.cullingMask = 0;
			;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(A_0.gameObject);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000083D8 File Offset: 0x000065D8
		private void a4f(Camera A_0, Camera A_1, Rect A_2, Rect A_3)
		{
			if (this.g.c < 1f)
			{
				this.b2f(A_0, true);
				this.b2f(A_1, true);
				this.a2f(A_0, false);
				Matrix4x4 projectionMatrix = A_0.projectionMatrix;
				Matrix4x4 projectionMatrix2 = A_1.projectionMatrix;
				if (A_0 != null)
				{
					A_0.rect = A_2;
				}
				if (A_1 != null)
				{
					A_1.rect = A_3;
				}
				A_0.projectionMatrix = projectionMatrix;
				A_1.projectionMatrix = projectionMatrix2;
				return;
			}
			this.b2f(A_0, false);
			this.b2f(A_1, true);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008460 File Offset: 0x00006660
		public void LateUpdate()
		{
			if (this.IsPaused)
			{
				return;
			}
			this._LateUpdate();
			if (skippable && beatha && Input.anyKeyDown)
			{
				this.StopCutscene(true);
				this.d = true;
			}
			if (!beatha)
			{
				return;
			}
			if (this.playerCamera)
			{
				b2f(this.playerCamera, this.interactive);
			}
			if (this.g != null)
			{
				Camera camera = this.g.a;
				Camera camera2 = this.g.b;
				if (this.interactive)
				{
					this.b2f(camera, false);
					this.b2f(camera2, false);
					this.b2f(this.playerCamera, true);
					return;
				}
				if (camera == null && camera2 == null)
				{
					b2f(this.playerCamera, true);
				}
				switch (this.g.d)
				{
				case Effect.Fade:
					switch (this.g.ef)
					{
					case at.a:
						this.b2f(camera, false);
						this.b2f(camera2, true);
						return;
					case at.b:
						this.b2f(camera, true);
						this.b2f(camera2, false);
						return;
					case at.c:
						if ((double)this.g.c < 0.5)
						{
							this.b2f(camera, true);
							this.b2f(camera2, false);
							return;
						}
						this.b2f(camera, false);
						this.b2f(camera2, true);
						return;
					default:
						return;
					}
					break;
				case Effect.CrossFade:
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				case Effect.CrossFadePro:
					this.b2f(camera, false);
					this.b2f(camera2, true);
					if (this.g.c <= 1f)
					{
						CSComponent.a1f(camera);
						return;
					}
					break;
				case Effect.RectangleGrow:
					if (this.g.c < 1f)
					{
						this.b2f(camera, true);
						this.b2f(camera2, false);
						CSComponent.a1f(camera2);
						return;
					}
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				case Effect.RectangleShrink:
					this.b2f(camera, false);
					this.b2f(camera2, true);
					if (this.g.c < 1f)
					{
						CSComponent.a1f(camera);
						return;
					}
					break;
				case Effect.SquishLeft:
					this.a4f(camera, camera2, new Rect(0f, 0f, 1f - this.g.c, 1f), new Rect(1f - this.g.c, 0f, 1f, 1f));
					return;
				case Effect.SquishRight:
					this.a4f(camera, camera2, new Rect(this.g.c, 0f, 1f, 1f), new Rect(0f, 0f, this.g.c, 1f));
					return;
				case Effect.SquishUp:
					this.a4f(camera, camera2, new Rect(0f, this.g.c, 1f, 1f), new Rect(0f, 0f, 1f, this.g.c));
					return;
				case Effect.SquishDown:
					this.a4f(camera, camera2, new Rect(0f, 0f, 1f, 1f - this.g.c), new Rect(0f, 1f - this.g.c, 1f, 1f));
					return;
				case Effect.DreamWipe:
					if (this.g.c < 1f)
					{
						this.b2f(camera, false);
						this.b2f(camera2, false);
						this.a2f(camera2, true);
						CSComponent.a6ft(camera, camera2, this.g.c, this.g.g, this.g.h, this.g.i);
						return;
					}
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				case Effect.ShapeGrow:
					if (this.g.c < 1f)
					{
						this.b2f(camera, false);
						this.b2f(camera2, false);
						this.a2f(camera2, true);
						CSComponent.a6f(camera, camera2, this.g.c, this.g.j, false, this.g.k);
						return;
					}
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				case Effect.ShapeShrink:
					if (this.g.c < 1f)
					{
						this.b2f(camera, false);
						this.b2f(camera2, false);
						this.a2f(camera2, true);
						CSComponent.a6f(camera, camera2, this.g.c, this.g.j, true, this.g.k);
						return;
					}
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				case Effect.FadeToColor:
					this.b2f(camera, true);
					this.b2f(camera2, false);
					return;
				case Effect.FadeFromColor:
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				default:
					this.b2f(camera, false);
					this.b2f(camera2, true);
					return;
				}
			}
			else
			{
				this.b2f(this.playerCamera, true);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000894C File Offset: 0x00006B4C
		public bool EvaluateTime(float time)
		{
			this.f.Clear();
			this.g = null;
			this.h = null;
			this.i = null;
			bool result = false;
			foreach (KeyValuePair<GameObject, List<CutsceneObjectBase>> keyValuePair in this.cdict)
			{
				foreach (CutsceneObjectBase cutsceneObjectBase in keyValuePair.Value)
				{
					foreach (ITrack track in cutsceneObjectBase.GetTracks())
					{
						if (track.endTime >= this.alepta)
						{
							result = true;
						}
					}
					if (time == 0f)
					{
						time = 0.01f;
					}
					cutsceneObjectBase.EvaluateTime(time);
				}
			}
			return result;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008A48 File Offset: 0x00006C48
		public bool EvaluateTimeAndSwitchCamera(float time)
		{
			bool result = this.EvaluateTime(time);
			if (this.g != null)
			{
				Camera a_ = this.g.a;
				Camera a_2 = this.g.b;
				this.b2f(this.playerCamera, false);
				this.b2f(a_, false);
				this.b2f(a_2, true);
			}
			else
			{
				this.b2f(this.playerCamera, true);
			}
			return result;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008AAA File Offset: 0x00006CAA
		public Camera GetCurrentCamera()
		{
			if (this.g != null)
			{
				return this.g.b;
			}
			return null;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008AC4 File Offset: 0x00006CC4
		private void cmeth()
		{
			AudioSource[] componentsInChildren = base.GetComponentsInChildren<AudioSource>();
			foreach (AudioSource audioSource in componentsInChildren)
			{
				if (audioSource.isPlaying)
				{
					audioSource.Stop();
					CSDestroyObjectAterPlaySoundInternal component = audioSource.GetComponent<CSDestroyObjectAterPlaySoundInternal>();
					if (component != null)
					{
						UnityEngine.Object.DestroyImmediate(component.gameObject);
					}
				}
			}
			foreach (SoundTrack soundTrack in base.GetComponentsInChildren<SoundTrack>())
			{
				AudioSource controlledObject = soundTrack.controlledObject;
				if (controlledObject.isPlaying)
				{
					controlledObject.Stop();
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008B58 File Offset: 0x00006D58
		public void _LateUpdate()
		{
			if (beatha)
			{
				this.EvaluateTime(this.alepta);
				if (this.alepta > this.e)
				{
					this.StopCutscene(true);
				}
				if (!this.IsPaused)
				{
					this.alepta += Time.deltaTime * this.playSpeed;
				}
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008BB0 File Offset: 0x00006DB0
		public float betta()
		{
			if (this.endType == CSComponent.CutsceneEndType.TimelineLength)
			{
				return (float)this.timeLineLenght;
			}
			float num = 0f;
			foreach (KeyValuePair<GameObject, List<CutsceneObjectBase>> keyValuePair in this.cdict)
			{
				foreach (CutsceneObjectBase cutsceneObjectBase in keyValuePair.Value)
				{
					foreach (ITrack track in cutsceneObjectBase.GetTracks())
					{
						if (track.endTime > num)
						{
							num = track.endTime;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008C84 File Offset: 0x00006E84
		public void StartCutscene()
		{
			this.StartCutscene(0.01f);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008C91 File Offset: 0x00006E91
		public void StartCutscene(float startTime)
		{
			this.StartCutscene(Mathf.Clamp(startTime, 0.01f, betta()), betta());
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008CB0 File Offset: 0x00006EB0
		public void StartCutscene(float startTime, float endTime)
		{
			this.m = false;
			this.d = false;
			this.l = false;
			this.alepta = startTime;
			this.e = endTime;
			beatha = true;
			if (this.playerCamera)
			{
				this.playerCamera.enabled = this.interactive;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008D08 File Offset: 0x00006F08
		public void StopCutscene(bool evaluateTame = false)
		{
			this.cmeth();
			this.m = true;
			this.l = false;
			beatha = false;
			if (CSComponent.j != null)
			{
				UnityEngine.Object.Destroy(CSComponent.j);
			}
			if (CSComponent.k != null)
			{
				UnityEngine.Object.Destroy(CSComponent.k);
			}
			if (evaluateTame)
			{
				foreach (KeyValuePair<GameObject, List<CutsceneObjectBase>> keyValuePair in this.cdict)
				{
					foreach (CutsceneObjectBase cutsceneObjectBase in keyValuePair.Value)
					{
						cutsceneObjectBase.EvaluateTime(this.alepta + 0.05f);
					}
				}
			}
			if (this.i != null)
			{
				this.i.StopCutscene(false);
			}
			b2f(this.playerCamera, true);
			foreach (DirectorCameraTrack directorCameraTrack in base.GetComponentsInChildren<DirectorCameraTrack>())
			{
				foreach (DirectorCameraKey directorCameraKey in directorCameraTrack.Keys)
				{
					if (directorCameraKey != null)
					{
						b2f(directorCameraKey.keyCamera, false);
					}
				}
			}
			if (this.stopMessageTarget != null && !string.IsNullOrEmpty(this.stopMessageName ?? ""))
			{
				this.stopMessageTarget.SendMessage(this.stopMessageName, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008EAC File Offset: 0x000070AC
		internal void a(float A_0)
		{
			this.cmeth();
			this.m = true;
			this.l = false;
			beatha = false;
			if (CSComponent.j != null)
			{
				UnityEngine.Object.DestroyImmediate(CSComponent.j);
			}
			if (CSComponent.k != null)
			{
				UnityEngine.Object.DestroyImmediate(CSComponent.k);
			}
			foreach (KeyValuePair<GameObject, List<CutsceneObjectBase>> keyValuePair in this.cdict)
			{
				foreach (CutsceneObjectBase cutsceneObjectBase in keyValuePair.Value)
				{
					if (A_0 == 0f)
					{
						A_0 = 0.01f;
					}
					cutsceneObjectBase.EvaluateTime(A_0);
				}
			}
			if (this.i != null)
			{
				this.i.StopCutscene(false);
			}
			foreach (DirectorCameraTrack directorCameraTrack in base.GetComponentsInChildren<DirectorCameraTrack>())
			{
				foreach (DirectorCameraKey directorCameraKey in directorCameraTrack.Keys)
				{
					if (directorCameraKey != null && directorCameraKey.keyCamera != null)
					{
						directorCameraKey.keyCamera.gameObject.active = false;
					}
				}
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009020 File Offset: 0x00007220
		public static CSComponent FindCutscene(string cutsceneName)
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(CSComponent));
			int i = 0;
			while (i < array.Length)
			{
				CSComponent cscomponent = (CSComponent)array[i];
				CSComponent result;
				if (cscomponent.cutSceneName == cutsceneName)
				{
					result = cscomponent;
				}
				else
				{
					if (!(cscomponent.gameObject.name == cutsceneName))
					{
						i++;
						continue;
					}
					result = cscomponent;
				}
				return result;
			}
			Debug.Log("Cutscene with name \"" + cutsceneName + "\" not found!");
			return null;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009098 File Offset: 0x00007298
		public CSComponent()
		{
			this.l = false;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00009134 File Offset: 0x00007334
		internal static Texture2D WhiteTexture
		{
			get
			{
				if (CSComponent.r == null)
				{
					CSComponent.r = new Texture2D(16, 16, TextureFormat.ARGB32, false);
					CSComponent.r.hideFlags = HideFlags.HideAndDontSave;
					Color[] pixels = CSComponent.r.GetPixels();
					for (int i = 0; i < pixels.Length; i++)
					{
						pixels[i] = Color.white;
					}
					CSComponent.r.SetPixels(pixels);
					CSComponent.r.Compress(false);
					CSComponent.r.Apply();
				}
				return CSComponent.r;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000091B9 File Offset: 0x000073B9
		public void OnDisable()
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000091BC File Offset: 0x000073BC
		public void OnDestroy()
		{
			if (CSComponent.o != null)
			{
				UnityEngine.Object.Destroy(CSComponent.o);
				CSComponent.o = null;
			}
			if (CSComponent.q != null)
			{
				UnityEngine.Object.Destroy(CSComponent.q);
				CSComponent.q = null;
			}
			if (CSComponent.p != null)
			{
				UnityEngine.Object.Destroy(CSComponent.p);
				CSComponent.p = null;
			}
			if (CSComponent.j != null)
			{
				UnityEngine.Object.Destroy(CSComponent.j);
				CSComponent.j = null;
			}
			if (CSComponent.k != null)
			{
				UnityEngine.Object.Destroy(CSComponent.k);
				CSComponent.k = null;
			}
			if (CSComponent.r != null)
			{
				UnityEngine.Object.Destroy(CSComponent.r);
				CSComponent.r = null;
			}
			this.g = null;
			CSComponent.Localization = null;
			CSComponent.CustomDrawer = null;
			if (!Application.isEditor)
			{
				CSCore.blist = null;
				return;
			}
			CSCore.SubmitChanges();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000092A0 File Offset: 0x000074A0
		public void OnGUI()
		{
			GUI.depth = -99999999;
			Color color = GUI.color;
			if (this.i != null)
			{
				this.i.OnGUI();
			}
			if (beatha && this.g != null && !this.interactive)
			{
				switch (this.g.d)
				{
				case Effect.Fade:
				{
					Rect position = new Rect(-1f, -1f, (float)(Screen.width + 2), (float)(Screen.height + 2));
					switch (this.g.ef)
					{
					case at.a:
					{
						Color color2 = this.g.e;
						color2.a = Mathf.Lerp(1f, 0f, this.g.c);
						GUI.color = color2;
						GUI.DrawTexture(position, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
						break;
					}
					case at.b:
					{
						Color color2 = this.g.e;
						color2.a = Mathf.Lerp(0f, 1f, this.g.c);
						GUI.color = color2;
						GUI.DrawTexture(position, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
						break;
					}
					case at.c:
						if ((double)this.g.c < 0.5)
						{
							Color color2 = this.g.e;
							color2.a = Mathf.Lerp(0f, 1f, this.g.c / 0.5f);
							GUI.color = color2;
							GUI.DrawTexture(position, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
						}
						else
						{
							Color color2 = this.g.e;
							color2.a = Mathf.Lerp(1f, 0f, (this.g.c - 0.5f) / 0.5f);
							GUI.color = color2;
							GUI.DrawTexture(position, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
						}
						break;
					}
					break;
				}
				case Effect.CrossFade:
					if (this.g.c < 1f)
					{
						Color color3 = GUI.color;
						GUI.color = new Color(1f, 1f, 1f, 1f - this.g.c);
						GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), CSComponent.EffectTexture, ScaleMode.StretchToFill, false);
						GUI.color = color3;
					}
					break;
				case Effect.CrossFadePro:
					if (this.g.c < 1f)
					{
						Color color4 = GUI.color;
						GUI.color = new Color(1f, 1f, 1f, 1f - this.g.c);
						GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), CSComponent.EffectRenderTexture, ScaleMode.StretchToFill, false);
						GUI.color = color4;
					}
					break;
				case Effect.RectangleGrow:
					if (this.g.c <= 1f)
					{
						float num = (float)Screen.width * this.g.c;
						float num2 = (float)Screen.height * this.g.c;
						Rect position2 = new Rect((float)Screen.width / 2f - num / 2f, (float)Screen.height / 2f - num2 / 2f, num, num2);
						GUI.DrawTexture(position2, CSComponent.EffectRenderTexture, ScaleMode.StretchToFill, false);
					}
					break;
				case Effect.RectangleShrink:
					if (this.g.c <= 1f)
					{
						float num3 = (float)Screen.width * (1f - this.g.c);
						float num4 = (float)Screen.height * (1f - this.g.c);
						Rect position3 = new Rect((float)Screen.width / 2f - num3 / 2f, (float)Screen.height / 2f - num4 / 2f, num3, num4);
						GUI.DrawTexture(position3, CSComponent.EffectRenderTexture, ScaleMode.StretchToFill, false);
					}
					break;
				case Effect.DreamWipe:
				case Effect.ShapeGrow:
				case Effect.ShapeShrink:
					if (this.g.c <= 1f)
					{
						GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), CSComponent.EffectRenderTexture, ScaleMode.StretchToFill, false);
					}
					break;
				case Effect.FadeToColor:
				{
					Rect position4 = new Rect(-1f, -1f, (float)(Screen.width + 2), (float)(Screen.height + 2));
					Color color5 = this.g.e;
					color5.a = Mathf.Lerp(0f, 1f, this.g.c);
					GUI.color = color5;
					GUI.DrawTexture(position4, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
					break;
				}
				case Effect.FadeFromColor:
				{
					Rect position5 = new Rect(-1f, -1f, (float)(Screen.width + 2), (float)(Screen.height + 2));
					Color color6 = this.g.e;
					color6.a = Mathf.Lerp(1f, 0f, this.g.c);
					GUI.color = color6;
					GUI.DrawTexture(position5, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
					break;
				}
				}
			}
			GUI.color = color;
			float num5 = (float)Screen.height * this.letterboxSize;
			if (this.letterbox && beatha)
			{
				Rect position6 = new Rect(-5f, -5f, (float)(Screen.width + 10), num5 + 5f);
				Rect position7 = new Rect(-5f, (float)Screen.height - num5, (float)(Screen.width + 10), num5 + 5f);
				GUI.color = Color.black;
				GUI.DrawTexture(position6, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
				GUI.DrawTexture(position7, CSComponent.WhiteTexture, ScaleMode.StretchToFill);
			}
			GUI.color = color;
			if (beatha && this.showSubtitles && this.f.Count > 0)
			{
				if (CSComponent.CustomDrawer != null)
				{
					using (List<SubtitlesDrawInfo>.Enumerator enumerator = this.f.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SubtitlesDrawInfo drawInfo = enumerator.Current;
							CSComponent.CustomDrawer.DrawSubtitles(drawInfo);
						}
						goto IL_8C3;
					}
				}
				Rect rect = new Rect(0f, num5, (float)Screen.width, (float)Screen.height - num5 * 2f);
				GUIStyle guistyle = new GUIStyle(GUI.skin.label);
				guistyle.alignment = TextAnchor.LowerCenter;
				foreach (SubtitlesDrawInfo subtitlesDrawInfo in this.f)
				{
					if (subtitlesDrawInfo.useCustomPlacement)
					{
						float num6 = subtitlesDrawInfo.customMarginX.x * (float)Screen.width;
						float num7 = subtitlesDrawInfo.customMarginX.y * (float)Screen.width;
						float num8 = subtitlesDrawInfo.customMarginY.x * (float)Screen.height;
						float num9 = subtitlesDrawInfo.customMarginY.y * (float)Screen.height;
						rect = new Rect(num6, num8, (float)Screen.width - num6 - num7, (float)Screen.height - num8 - num9);
						guistyle.alignment = subtitlesDrawInfo.textAnchor;
					}
					if (CSComponent.Localization != null)
					{
						Font currentLanguageFont = CSComponent.Localization.GetCurrentLanguageFont();
						if (currentLanguageFont != null)
						{
							guistyle.font = currentLanguageFont;
						}
						else if (subtitlesDrawInfo.font)
						{
							guistyle.font = subtitlesDrawInfo.font;
						}
						else if (this.subtitlesDefaultFont)
						{
							guistyle.font = this.subtitlesDefaultFont;
						}
						else
						{
							guistyle.font = GUI.skin.font;
						}
					}
					else if (subtitlesDrawInfo.font)
					{
						guistyle.font = subtitlesDrawInfo.font;
					}
					else if (this.subtitlesDefaultFont)
					{
						guistyle.font = this.subtitlesDefaultFont;
					}
					else
					{
						guistyle.font = GUI.skin.font;
					}
					if (subtitlesDrawInfo.useCustomStyle)
					{
						guistyle.fontSize = subtitlesDrawInfo.fontSize + 3;
						guistyle.fontStyle = subtitlesDrawInfo.fontStyle;
					}
					if (this.textShadow)
					{
						Color color7 = Color.black;
						if (subtitlesDrawInfo.color == Color.black)
						{
							color7 = Color.white;
						}
						color7.a = subtitlesDrawInfo.color.a;
						Rect position8 = new Rect(rect);
						position8.x += 2f;
						position8.y += 2f;
						GUI.color = color7;
						GUI.Label(position8, subtitlesDrawInfo.text, guistyle);
					}
					GUI.color = subtitlesDrawInfo.color;
					GUI.Label(rect, subtitlesDrawInfo.text, guistyle);
				}
			}
			IL_8C3:
			GUI.color = color;
		}

		// Token: 0x040000E3 RID: 227
		public GameObject stopMessageTarget;

		// Token: 0x040000E4 RID: 228
		public string stopMessageName;

		// Token: 0x040000E5 RID: 229
		public string id = "";

		// Token: 0x040000E6 RID: 230
		public string cutSceneName = "New Cutscene";

		// Token: 0x040000E7 RID: 231
		[HideInInspector]
		public float currentTime;

		// Token: 0x040000E8 RID: 232
		[HideInInspector]
		public float scaleTimelineEditor = 1f;

		// Token: 0x040000E9 RID: 233
		[HideInInspector]
		public float leftZoneWidth = 256f;

		// Token: 0x040000EA RID: 234
		[HideInInspector]
		public int timeLineLenght = 180;

		// Token: 0x040000EB RID: 235
		[HideInInspector]
		public bool isLocked;

		// Token: 0x040000EC RID: 236
		[HideInInspector]
		public List<GameObject> foldoutGameObjects = new List<GameObject>();

		// Token: 0x040000ED RID: 237
		public float alepta;

		// Token: 0x040000EE RID: 238
		public bool beatha;

		// Token: 0x040000EF RID: 239
		private readonly Dictionary<GameObject, List<CutsceneObjectBase>> cdict = new Dictionary<GameObject, List<CutsceneObjectBase>>();

		// Token: 0x040000F0 RID: 240
		public static readonly CSComponent.AudioVolumeManager AudioVolume = new CSComponent.AudioVolumeManager();

		// Token: 0x040000F1 RID: 241
		public CSComponent.CutsceneEndType endType;

		// Token: 0x040000F2 RID: 242
		public bool showSubtitles = true;

		// Token: 0x040000F3 RID: 243
		public bool playOnLoad;

		// Token: 0x040000F4 RID: 244
		public float playSpeed = 1f;

		// Token: 0x040000F5 RID: 245
		public bool skippable;

		// Token: 0x040000F6 RID: 246
		private bool d;

		// Token: 0x040000F7 RID: 247
		public bool interactive;

		// Token: 0x040000F8 RID: 248
		public bool letterbox;

		// Token: 0x040000F9 RID: 249
		public float letterboxSize = 0.1f;

		// Token: 0x040000FA RID: 250
		public Font subtitlesDefaultFont;

		// Token: 0x040000FB RID: 251
		public static ICutsceneLocalization Localization;

		// Token: 0x040000FC RID: 252
		public static ICutsceneCustomDraw CustomDrawer;

		// Token: 0x040000FD RID: 253
		public Camera playerCamera;

		// Token: 0x040000FE RID: 254
		public bool textShadow;

		// Token: 0x040000FF RID: 255
		public bool directorFoldout;

		// Token: 0x04000100 RID: 256
		public bool runEventsInEditMode;

		// Token: 0x04000101 RID: 257
		private float e;

		// Token: 0x04000102 RID: 258
		internal readonly List<SubtitlesDrawInfo> f = new List<SubtitlesDrawInfo>();

		// Token: 0x04000103 RID: 259
		internal f g;

		// Token: 0x04000104 RID: 260
		internal Camera h;

		// Token: 0x04000105 RID: 261
		internal CSComponent i;

		// Token: 0x04000106 RID: 262
		private static RenderTexture j;

		// Token: 0x04000107 RID: 263
		private static Texture2D k;

		// Token: 0x04000108 RID: 264
		private bool l;

		// Token: 0x04000109 RID: 265
		private bool m;

		// Token: 0x0400010A RID: 266
		private List<AudioSource> n = new List<AudioSource>();

		// Token: 0x0400010B RID: 267
		private static Material o;

		// Token: 0x0400010C RID: 268
		private static Material p;

		// Token: 0x0400010D RID: 269
		private static Material q;

		// Token: 0x0400010E RID: 270
		private static Texture2D r;

		// Token: 0x0200002B RID: 43
		public enum CutsceneEndType
		{
			// Token: 0x04000110 RID: 272
			TimelineLength,
			// Token: 0x04000111 RID: 273
			LastKeyframe
		}

		// Token: 0x0200002C RID: 44
		public class AudioVolumeManager
		{
			// Token: 0x04000112 RID: 274
			public float customVolume = 1f;

			// Token: 0x04000113 RID: 275
			public float sfxVolume = 1f;

			// Token: 0x04000114 RID: 276
			public float musicVolume = 1f;

			// Token: 0x04000115 RID: 277
			public float speechVolume = 1f;
		}
	}
}
