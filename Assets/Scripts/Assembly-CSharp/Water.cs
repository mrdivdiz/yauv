using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000029 RID: 41
[ExecuteInEditMode]
public class Water : MonoBehaviour
{
	// Token: 0x060000FE RID: 254 RVA: 0x00006614 File Offset: 0x00004814
	public void OnWillRenderObject()
	{
		if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
		{
			return;
		}
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		if (Water.s_InsideWater)
		{
			return;
		}
		Water.s_InsideWater = true;
		this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
		Water.WaterMode waterMode = this.GetWaterMode();
		Camera camera;
		Camera camera2;
		this.CreateWaterObjects(current, out camera, out camera2);
		Vector3 position = base.transform.position;
		Vector3 up = base.transform.up;
		int pixelLightCount = QualitySettings.pixelLightCount;
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = 0;
		}
		this.UpdateCameraModes(current, camera);
		this.UpdateCameraModes(current, camera2);
		if (waterMode >= Water.WaterMode.Reflective)
		{
			float w = -Vector3.Dot(up, position) - this.m_ClipPlaneOffset;
			Vector4 plane = new Vector4(up.x, up.y, up.z, w);
			Matrix4x4 zero = Matrix4x4.zero;
			Water.CalculateReflectionMatrix(ref zero, plane);
			Vector3 position2 = current.transform.position;
			Vector3 position3 = zero.MultiplyPoint(position2);
			camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
			Vector4 clipPlane = this.CameraSpacePlane(camera, position, up, 1f);
			Matrix4x4 projectionMatrix = current.projectionMatrix;
			Water.CalculateObliqueMatrix(ref projectionMatrix, clipPlane);
			camera.projectionMatrix = projectionMatrix;
			camera.cullingMask = (-17 & this.m_ReflectLayers.value);
			camera.targetTexture = this.m_ReflectionTexture;
			GL.SetRevertBackfacing(true);
			camera.transform.position = position3;
			Vector3 eulerAngles = current.transform.eulerAngles;
			camera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
			camera.Render();
			camera.transform.position = position2;
			GL.SetRevertBackfacing(false);
			base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
		}
		if (waterMode >= Water.WaterMode.Refractive)
		{
			camera2.worldToCameraMatrix = current.worldToCameraMatrix;
			Vector4 clipPlane2 = this.CameraSpacePlane(camera2, position, up, -1f);
			Matrix4x4 projectionMatrix2 = current.projectionMatrix;
			Water.CalculateObliqueMatrix(ref projectionMatrix2, clipPlane2);
			camera2.projectionMatrix = projectionMatrix2;
			camera2.cullingMask = (-17 & this.m_RefractLayers.value);
			camera2.targetTexture = this.m_RefractionTexture;
			camera2.transform.position = current.transform.position;
			camera2.transform.rotation = current.transform.rotation;
			camera2.Render();
			base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
		}
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = pixelLightCount;
		}
		switch (waterMode)
		{
		case Water.WaterMode.Simple:
			Shader.EnableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_REFRACTIVE");
			break;
		case Water.WaterMode.Reflective:
			Shader.DisableKeyword("WATER_SIMPLE");
			Shader.EnableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_REFRACTIVE");
			break;
		case Water.WaterMode.Refractive:
			Shader.DisableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
			Shader.EnableKeyword("WATER_REFRACTIVE");
			break;
		}
		Water.s_InsideWater = false;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006964 File Offset: 0x00004B64
	private void OnDisable()
	{
		if (this.m_ReflectionTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			this.m_ReflectionTexture = null;
		}
		if (this.m_RefractionTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
			this.m_RefractionTexture = null;
		}
		foreach (object obj in this.m_ReflectionCameras)
		{
			UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)obj).Value).gameObject);
		}
		this.m_ReflectionCameras.Clear();
		foreach (object obj2 in this.m_RefractionCameras)
		{
			UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)obj2).Value).gameObject);
		}
		this.m_RefractionCameras.Clear();
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006AB0 File Offset: 0x00004CB0
	private void Update()
	{
		if (!base.GetComponent<Renderer>())
		{
			return;
		}
		Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		if (!sharedMaterial)
		{
			return;
		}
		Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
		float @float = sharedMaterial.GetFloat("_WaveScale");
		Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
		double num = (double)Time.timeSinceLevelLoad / 20.0;
		Vector4 vector3 = new Vector4((float)Math.IEEERemainder((double)(vector.x * vector2.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * vector2.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * vector2.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * vector2.w) * num, 1.0));
		sharedMaterial.SetVector("_WaveOffset", vector3);
		sharedMaterial.SetVector("_WaveScale4", vector2);
		Vector3 size = base.GetComponent<Renderer>().bounds.size;
		Vector3 s = new Vector3(size.x * vector2.x, size.z * vector2.y, 1f);
		Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(vector3.x, vector3.y, 0f), Quaternion.identity, s);
		sharedMaterial.SetMatrix("_WaveMatrix", matrix);
		s = new Vector3(size.x * vector2.z, size.z * vector2.w, 1f);
		matrix = Matrix4x4.TRS(new Vector3(vector3.z, vector3.w, 0f), Quaternion.identity, s);
		sharedMaterial.SetMatrix("_WaveMatrix2", matrix);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006CA0 File Offset: 0x00004EA0
	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox skybox = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox2 = dest.GetComponent(typeof(Skybox)) as Skybox;
			if (!skybox || !skybox.material)
			{
				skybox2.enabled = false;
			}
			else
			{
				skybox2.enabled = true;
				skybox2.material = skybox.material;
			}
		}
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006D8C File Offset: 0x00004F8C
	private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
	{
		Water.WaterMode waterMode = this.GetWaterMode();
		reflectionCamera = null;
		refractionCamera = null;
		if (waterMode >= Water.WaterMode.Reflective)
		{
			if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.m_TextureSize)
			{
				if (this.m_ReflectionTexture)
				{
					UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
				}
				this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID();
				this.m_ReflectionTexture.isPowerOfTwo = true;
				this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
				this.m_OldReflectionTextureSize = this.m_TextureSize;
			}
			reflectionCamera = (this.m_ReflectionCameras[currentCamera] as Camera);
			if (!reflectionCamera)
			{
				GameObject gameObject = new GameObject(string.Concat(new object[]
				{
					"Water Refl Camera id",
					base.GetInstanceID(),
					" for ",
					currentCamera.GetInstanceID()
				}), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				reflectionCamera = gameObject.GetComponent<Camera>();
				reflectionCamera.enabled = false;
				reflectionCamera.transform.position = base.transform.position;
				reflectionCamera.transform.rotation = base.transform.rotation;
				reflectionCamera.gameObject.AddComponent<FlareLayer>();
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				this.m_ReflectionCameras[currentCamera] = reflectionCamera;
			}
		}
		if (waterMode >= Water.WaterMode.Refractive)
		{
			if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.m_TextureSize)
			{
				if (this.m_RefractionTexture)
				{
					UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
				}
				this.m_RefractionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID();
				this.m_RefractionTexture.isPowerOfTwo = true;
				this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
				this.m_OldRefractionTextureSize = this.m_TextureSize;
			}
			refractionCamera = (this.m_RefractionCameras[currentCamera] as Camera);
			if (!refractionCamera)
			{
				GameObject gameObject2 = new GameObject(string.Concat(new object[]
				{
					"Water Refr Camera id",
					base.GetInstanceID(),
					" for ",
					currentCamera.GetInstanceID()
				}), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				refractionCamera = gameObject2.GetComponent<Camera>();
				refractionCamera.enabled = false;
				refractionCamera.transform.position = base.transform.position;
				refractionCamera.transform.rotation = base.transform.rotation;
				refractionCamera.gameObject.AddComponent<FlareLayer>();
				gameObject2.hideFlags = HideFlags.HideAndDontSave;
				this.m_RefractionCameras[currentCamera] = refractionCamera;
			}
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x000070AC File Offset: 0x000052AC
	private Water.WaterMode GetWaterMode()
	{
		if (this.m_HardwareWaterSupport < this.m_WaterMode)
		{
			return this.m_HardwareWaterSupport;
		}
		return this.m_WaterMode;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000070CC File Offset: 0x000052CC
	private Water.WaterMode FindHardwareWaterSupport()
	{
		if (!SystemInfo.supportsRenderTextures || !base.GetComponent<Renderer>())
		{
			return Water.WaterMode.Simple;
		}
		Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		if (!sharedMaterial)
		{
			return Water.WaterMode.Simple;
		}
		string tag = sharedMaterial.GetTag("WATERMODE", false);
		if (tag == "Refractive")
		{
			return Water.WaterMode.Refractive;
		}
		if (tag == "Reflective")
		{
			return Water.WaterMode.Reflective;
		}
		return Water.WaterMode.Simple;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00007140 File Offset: 0x00005340
	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x0000716C File Offset: 0x0000536C
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 v = pos + normal * this.m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000071D8 File Offset: 0x000053D8
	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(Water.sgn(clipPlane.x), Water.sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00007288 File Offset: 0x00005488
	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}

	// Token: 0x040000E6 RID: 230
	public Water.WaterMode m_WaterMode = Water.WaterMode.Refractive;

	// Token: 0x040000E7 RID: 231
	public bool m_DisablePixelLights = true;

	// Token: 0x040000E8 RID: 232
	public int m_TextureSize = 256;

	// Token: 0x040000E9 RID: 233
	public float m_ClipPlaneOffset = 0.07f;

	// Token: 0x040000EA RID: 234
	public LayerMask m_ReflectLayers = -1;

	// Token: 0x040000EB RID: 235
	public LayerMask m_RefractLayers = -1;

	// Token: 0x040000EC RID: 236
	private Hashtable m_ReflectionCameras = new Hashtable();

	// Token: 0x040000ED RID: 237
	private Hashtable m_RefractionCameras = new Hashtable();

	// Token: 0x040000EE RID: 238
	private RenderTexture m_ReflectionTexture;

	// Token: 0x040000EF RID: 239
	private RenderTexture m_RefractionTexture;

	// Token: 0x040000F0 RID: 240
	private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;

	// Token: 0x040000F1 RID: 241
	private int m_OldReflectionTextureSize;

	// Token: 0x040000F2 RID: 242
	private int m_OldRefractionTextureSize;

	// Token: 0x040000F3 RID: 243
	private static bool s_InsideWater;

	// Token: 0x0200002A RID: 42
	public enum WaterMode
	{
		// Token: 0x040000F5 RID: 245
		Simple,
		// Token: 0x040000F6 RID: 246
		Reflective,
		// Token: 0x040000F7 RID: 247
		Refractive
	}
}
