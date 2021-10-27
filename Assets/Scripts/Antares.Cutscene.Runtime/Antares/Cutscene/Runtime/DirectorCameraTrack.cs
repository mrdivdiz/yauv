using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antares.Cutscene.Runtime
{
	// Token: 0x02000008 RID: 8
	public class DirectorCameraTrack : MonoBehaviour, ITrack
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002116 File Offset: 0x00000316
		public string trackName
		{
			get
			{
				return this.cameraTrackName;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000211E File Offset: 0x0000031E
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002126 File Offset: 0x00000326
		public bool IsDisabled
		{
			get
			{
				return this.isDisabled;
			}
			set
			{
				this.isDisabled = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000212F File Offset: 0x0000032F
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002138 File Offset: 0x00000338
		public object ControlledObject
		{
			get
			{
				return this.controlledObject;
			}
			set
			{
				try
				{
					this.controlledObject = (CSComponent)value;
				}
				catch (Exception arg)
				{
					Debug.Log("Faild to set DirectorCameraTrack.ControlledObject!\n" + arg);
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002178 File Offset: 0x00000378
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002180 File Offset: 0x00000380
		public WrapMode playMode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002189 File Offset: 0x00000389
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002191 File Offset: 0x00000391
		public Color color
		{
			get
			{
				return this.trackColor;
			}
			set
			{
				this.trackColor = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000219A File Offset: 0x0000039A
		public float startTime
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000021A2 File Offset: 0x000003A2
		public float endTime
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000021AA File Offset: 0x000003AA
		public float length
		{
			get
			{
				return this.b - this.a;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000021B9 File Offset: 0x000003B9
		public IKeyframe[] Keys
		{
			get
			{
				return this.c.ToArray();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000021C6 File Offset: 0x000003C6
		public void SelectKeyframe(IKeyframe keyframe)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000021C8 File Offset: 0x000003C8
		public IKeyframe CreateKey(float time)
		{
			DirectorCameraKey directorCameraKey = new GameObject("cameraKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<DirectorCameraKey>();
			directorCameraKey.dcTrack = this;
			directorCameraKey.keyTime = time;
			this.c.Add(directorCameraKey);
			this.UpdateTrack();
			return directorCameraKey;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000221C File Offset: 0x0000041C
		public IKeyframe DuplicateKey(IKeyframe keyframe)
		{
			DirectorCameraKey directorCameraKey = new GameObject("cameraKey")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<DirectorCameraKey>();
			directorCameraKey.dcTrack = this;
			directorCameraKey.time = keyframe.time;
			directorCameraKey.keyCamera = ((DirectorCameraKey)keyframe).keyCamera;
			directorCameraKey.duration = ((DirectorCameraKey)keyframe).duration;
			directorCameraKey.effect = ((DirectorCameraKey)keyframe).effect;
			directorCameraKey.fadeColor = ((DirectorCameraKey)keyframe).fadeColor;
			directorCameraKey.dwScale = ((DirectorCameraKey)keyframe).dwScale;
			directorCameraKey.dwFrequency = ((DirectorCameraKey)keyframe).dwFrequency;
			directorCameraKey.dwAnimSpeed = ((DirectorCameraKey)keyframe).dwAnimSpeed;
			directorCameraKey.rotateAmount = ((DirectorCameraKey)keyframe).rotateAmount;
			directorCameraKey.shapeMesh = ((DirectorCameraKey)keyframe).shapeMesh;
			this.c.Add(directorCameraKey);
			this.UpdateTrack();
			return directorCameraKey;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000230C File Offset: 0x0000050C
		public void DeleteKeyframe(IKeyframe keyframe)
		{
			this.c.Remove((DirectorCameraKey)keyframe);
			if (this.d == (DirectorCameraKey)keyframe)
			{
				this.d = null;
			}
			UnityEngine.Object.DestroyImmediate(((DirectorCameraKey)keyframe).gameObject);
			this.UpdateTrack();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000235B File Offset: 0x0000055B
		public int GetKeyframeIndex(IKeyframe keyframe)
		{
			return this.c.IndexOf((DirectorCameraKey)keyframe);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002370 File Offset: 0x00000570
		public void FindLeftAndRight(float time, out DirectorCameraKey left, out DirectorCameraKey right)
		{
			left = null;
			right = null;
			if (time <= this.a)
			{
				if (this.c.Count > 0)
				{
					right = this.c[0];
				}
				return;
			}
			if (time >= this.b)
			{
				if (this.c.Count > 0)
				{
					left = this.c[this.c.Count - 1];
				}
				return;
			}
			int num = 0;
			int num2 = this.c.Count - 1;
			while (num != num2)
			{
				int num3 = (num + num2) / 2;
				if (num3 == num)
				{
					break;
				}
				if (num3 == num2)
				{
					num = num2;
					break;
				}
				if (time > this.c[num3].keyTime)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			left = this.c[num];
			right = this.c[num2];
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000243C File Offset: 0x0000063C
		public void EvaluateTime(float time)
		{
			if (this.c.Count == 0)
			{
				return;
			}
			DirectorCameraKey directorCameraKey;
			DirectorCameraKey directorCameraKey2;
			this.FindLeftAndRight(time, out directorCameraKey, out directorCameraKey2);
			if (directorCameraKey)
			{
				Camera camera = null;
				DirectorCameraKey directorCameraKey3 = (DirectorCameraKey)directorCameraKey.GetPreviousKey();
				if (directorCameraKey3 != null && directorCameraKey3.time < directorCameraKey.time)
				{
					camera = directorCameraKey3.keyCamera;
				}
				Camera keyCamera = directorCameraKey.keyCamera;
				if (this.d != directorCameraKey)
				{
					switch (directorCameraKey.effect)
					{
					case Effect.CrossFadePro:
						if (camera != null)
						{
							RenderTexture.active = CSComponent.EffectRenderTexture;
							RenderTexture targetTexture = camera.targetTexture;
							camera.targetTexture = CSComponent.EffectRenderTexture;
							camera.Render();
							camera.targetTexture = targetTexture;
							RenderTexture.active = null;
						}
						else
						{
							Camera camera2 = new GameObject("tempCam").AddComponent<Camera>();
							camera2.clearFlags = CameraClearFlags.Color;
							camera2.backgroundColor = Color.black;
							RenderTexture.active = CSComponent.EffectRenderTexture;
							camera2.Render();
							RenderTexture.active = null;
							UnityEngine.Object.Destroy(camera2.gameObject);
						}
						break;
					case Effect.RectangleGrow:
						if (keyCamera != null)
						{
							RenderTexture.active = CSComponent.EffectRenderTexture;
							RenderTexture targetTexture2 = keyCamera.targetTexture;
							keyCamera.targetTexture = CSComponent.EffectRenderTexture;
							keyCamera.Render();
							keyCamera.targetTexture = targetTexture2;
							RenderTexture.active = null;
						}
						else
						{
							Camera camera3 = new GameObject("tempCam").AddComponent<Camera>();
							camera3.clearFlags = CameraClearFlags.Color;
							camera3.backgroundColor = Color.black;
							RenderTexture.active = CSComponent.EffectRenderTexture;
							camera3.Render();
							RenderTexture.active = null;
							UnityEngine.Object.Destroy(camera3.gameObject);
						}
						break;
					case Effect.RectangleShrink:
						if (camera != null)
						{
							RenderTexture.active = CSComponent.EffectRenderTexture;
							RenderTexture targetTexture3 = camera.targetTexture;
							camera.targetTexture = CSComponent.EffectRenderTexture;
							camera.Render();
							camera.targetTexture = targetTexture3;
							RenderTexture.active = null;
						}
						else
						{
							Camera camera4 = new GameObject("tempCam").AddComponent<Camera>();
							camera4.clearFlags = CameraClearFlags.Color;
							camera4.backgroundColor = Color.black;
							RenderTexture.active = CSComponent.EffectRenderTexture;
							camera4.Render();
							RenderTexture.active = null;
							UnityEngine.Object.Destroy(camera4.gameObject);
						}
						break;
					}
				}
				if (this.d != directorCameraKey && directorCameraKey.effect == Effect.CrossFade)
				{
					CSComponent.EffectTexture.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
					CSComponent.EffectTexture.Apply();
				}
				f f = new f();
				f.d = directorCameraKey.effect;
				f.c = (time - directorCameraKey.time) / directorCameraKey.duration;
				if (f.c > 1f)
				{
					f.c = 1.001f;
				}
				f.a = camera;
				f.b = keyCamera;
				f.e = directorCameraKey.fadeColor;
				f.g = directorCameraKey.dwScale;
				f.h = directorCameraKey.dwFrequency;
				f.i = directorCameraKey.dwAnimSpeed;
				f.k = directorCameraKey.rotateAmount;
				f.j = directorCameraKey.shapeMesh;
				if (f.d == Effect.Fade)
				{
					if (keyCamera == null)
					{
						f.ef = at.b;
					}
					else if (camera == null)
					{
						f.ef = at.a;
					}
					else
					{
						f.ef = at.c;
					}
				}
				else if (f.d != Effect.CrossFadePro && f.d != Effect.RectangleGrow)
				{
					Effect effect = f.d;
				}
				this.controlledObject.g = f;
			}
			else
			{
				DirectorCameraKey directorCameraKey4 = this.c[0];
				if (directorCameraKey4.effect == Effect.Fade)
				{
					f f2 = new f();
					f2.d = directorCameraKey4.effect;
					f2.c = 0f;
					f2.a = null;
					f2.b = directorCameraKey4.keyCamera;
					f2.e = directorCameraKey4.fadeColor;
					f2.ef = at.a;
					this.controlledObject.g = f2;
				}
			}
			this.d = directorCameraKey;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002840 File Offset: 0x00000A40
		public void InitTrack()
		{
			this.c.Clear();
			this.d = null;
			DirectorCameraKey[] componentsInChildren = base.gameObject.GetComponentsInChildren<DirectorCameraKey>();
			if (componentsInChildren.Length > 0)
			{
				this.c.AddRange(componentsInChildren);
			}
			this.UpdateTrack();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002884 File Offset: 0x00000A84
		public void UpdateTrack()
		{
			this.c.Sort(new Comparison<DirectorCameraKey>(this.CompareKeys));
			for (int i = 0; i < this.c.Count; i++)
			{
				this.c[i].gameObject.name = "cameraKey" + i;
			}
			if (this.c.Count == 0)
			{
				this.a = (this.b = 0f);
			}
			else
			{
				this.a = this.c[0].time;
				this.b = this.c[this.c.Count - 1].time;
			}
			if (Application.isEditor)
			{
				CSCore.a(this);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000294E File Offset: 0x00000B4E
		public int CompareKeys(IKeyframe one, IKeyframe other)
		{
			if (one.time > other.time)
			{
				return 1;
			}
			if (one.time < other.time)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002971 File Offset: 0x00000B71
		public bool IsCanShowInScene
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002974 File Offset: 0x00000B74
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002977 File Offset: 0x00000B77
		public bool ShowInScene
		{
			get
			{
				return false;
			}
			set
			{
				throw new Exception("ShowInScene");
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002984 File Offset: 0x00000B84
		public IKeyframe GetNextKey(DirectorCameraKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == this.c.Count - 1)
			{
				return this.c[0];
			}
			return this.c[num + 1];
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000029CC File Offset: 0x00000BCC
		public IKeyframe GetPreviousKey(DirectorCameraKey key)
		{
			int num = this.c.IndexOf(key);
			if (num == 0)
			{
				return this.c[this.c.Count - 1];
			}
			return this.c[num - 1];
		}

		// Token: 0x0400001A RID: 26
		public CSComponent controlledObject;

		// Token: 0x0400001B RID: 27
		public string cameraTrackName = "cutscene";

		// Token: 0x0400001C RID: 28
		public bool isDisabled;

		// Token: 0x0400001D RID: 29
		public WrapMode mode = WrapMode.Once;

		// Token: 0x0400001E RID: 30
		public Color trackColor = Color.white;

		// Token: 0x0400001F RID: 31
		private float a;

		// Token: 0x04000020 RID: 32
		private float b;

		// Token: 0x04000021 RID: 33
		private readonly List<DirectorCameraKey> c = new List<DirectorCameraKey>();

		// Token: 0x04000022 RID: 34
		private DirectorCameraKey d;
	}
}
