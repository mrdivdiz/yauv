using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000024 RID: 36
[RequireComponent(typeof(WaterBase))]
[ExecuteInEditMode]
public class PlanarReflection : MonoBehaviour
{
	// Token: 0x0600008E RID: 142 RVA: 0x00006E0C File Offset: 0x0000500C
	public void Start()
	{
		this.sharedMaterial = ((WaterBase)base.gameObject.GetComponent(typeof(WaterBase))).sharedMaterial;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00006E34 File Offset: 0x00005034
	private Camera CreateReflectionCameraFor(Camera cam)
	{
		string name = base.gameObject.name + "Reflection" + cam.name;
		GameObject gameObject = GameObject.Find(name);
		if (!gameObject)
		{
			gameObject = new GameObject(name, new Type[]
			{
				typeof(Camera)
			});
		}
		if (!gameObject.GetComponent(typeof(Camera)))
		{
			gameObject.AddComponent(typeof(Camera));
		}
		Camera camera = gameObject.GetComponent<Camera>();
		camera.backgroundColor = this.clearColor;
		camera.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
		this.SetStandardCameraParameter(camera, this.reflectionMask);
		if (!camera.targetTexture)
		{
			camera.targetTexture = this.CreateTextureFor(cam);
		}
		return camera;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00006F0C File Offset: 0x0000510C
	private void SetStandardCameraParameter(Camera cam, LayerMask mask)
	{
		cam.cullingMask = (mask & ~(1 << LayerMask.NameToLayer("Water")));
		cam.backgroundColor = Color.black;
		cam.enabled = false;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00006F48 File Offset: 0x00005148
	private RenderTexture CreateTextureFor(Camera cam)
	{
		return new RenderTexture(Mathf.FloorToInt(cam.pixelWidth * 0.5f), Mathf.FloorToInt(cam.pixelHeight * 0.5f), 24)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00006F88 File Offset: 0x00005188
	public void RenderHelpCameras(Camera currentCam)
	{
		if (this.helperCameras == null)
		{
			this.helperCameras = new Dictionary<Camera, bool>();
		}
		if (!this.helperCameras.ContainsKey(currentCam))
		{
			this.helperCameras.Add(currentCam, false);
		}
		if (this.helperCameras[currentCam])
		{
			return;
		}
		if (!this.reflectionCamera)
		{
			this.reflectionCamera = this.CreateReflectionCameraFor(currentCam);
		}
		this.RenderReflectionFor(currentCam, this.reflectionCamera);
		this.helperCameras[currentCam] = true;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00007014 File Offset: 0x00005214
	public void LateUpdate()
	{
		if (this.helperCameras != null)
		{
			this.helperCameras.Clear();
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x0000702C File Offset: 0x0000522C
	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		this.RenderHelpCameras(currentCam);
		if (this.reflectionCamera && this.sharedMaterial)
		{
			this.sharedMaterial.SetTexture(this.reflectionSampler, this.reflectionCamera.targetTexture);
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0000707C File Offset: 0x0000527C
	public void OnEnable()
	{
		Shader.EnableKeyword("WATER_REFLECTIVE");
		Shader.DisableKeyword("WATER_SIMPLE");
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00007094 File Offset: 0x00005294
	public void OnDisable()
	{
		Shader.EnableKeyword("WATER_SIMPLE");
		Shader.DisableKeyword("WATER_REFLECTIVE");
	}

	// Token: 0x06000097 RID: 151 RVA: 0x000070AC File Offset: 0x000052AC
	private void RenderReflectionFor(Camera cam, Camera reflectCamera)
	{
		if (!reflectCamera)
		{
			return;
		}
		if (this.sharedMaterial && !this.sharedMaterial.HasProperty(this.reflectionSampler))
		{
			return;
		}
		reflectCamera.cullingMask = (this.reflectionMask & ~(1 << LayerMask.NameToLayer("Water")));
		this.SaneCameraSettings(reflectCamera);
		reflectCamera.backgroundColor = this.clearColor;
		reflectCamera.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
		if (this.reflectSkybox && cam.gameObject.GetComponent(typeof(Skybox)))
		{
			Skybox skybox = (Skybox)reflectCamera.gameObject.GetComponent(typeof(Skybox));
			if (!skybox)
			{
				skybox = (Skybox)reflectCamera.gameObject.AddComponent(typeof(Skybox));
			}
			skybox.material = ((Skybox)cam.GetComponent(typeof(Skybox))).material;
		}
		GL.SetRevertBackfacing(true);
		Transform transform = base.transform;
		Vector3 eulerAngles = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectCamera.transform.position = cam.transform.position;
		Vector3 position = transform.transform.position;
		position.y = transform.position.y;
		Vector3 up = transform.transform.up;
		float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		Matrix4x4 matrix4x = Matrix4x4.zero;
		matrix4x = PlanarReflection.CalculateReflectionMatrix(matrix4x, plane);
		this.oldpos = cam.transform.position;
		Vector3 position2 = matrix4x.MultiplyPoint(this.oldpos);
		reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, position, up, 1f);
		Matrix4x4 matrix4x2 = cam.projectionMatrix;
		matrix4x2 = PlanarReflection.CalculateObliqueMatrix(matrix4x2, clipPlane);
		reflectCamera.projectionMatrix = matrix4x2;
		reflectCamera.transform.position = position2;
		Vector3 eulerAngles2 = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		reflectCamera.Render();
		GL.SetRevertBackfacing(false);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00007338 File Offset: 0x00005538
	private void SaneCameraSettings(Camera helperCam)
	{
		helperCam.depthTextureMode = DepthTextureMode.None;
		helperCam.backgroundColor = Color.black;
		helperCam.clearFlags = CameraClearFlags.Color;
		helperCam.renderingPath = RenderingPath.Forward;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007368 File Offset: 0x00005568
	private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(PlanarReflection.sgn(clipPlane.x), PlanarReflection.sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
		return projection;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00007424 File Offset: 0x00005624
	private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
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
		return reflectionMat;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000075DC File Offset: 0x000057DC
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

	// Token: 0x0600009C RID: 156 RVA: 0x00007608 File Offset: 0x00005808
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 v = pos + normal * this.clipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x040000BE RID: 190
	public LayerMask reflectionMask;

	// Token: 0x040000BF RID: 191
	public bool reflectSkybox;

	// Token: 0x040000C0 RID: 192
	public Color clearColor = Color.grey;

	// Token: 0x040000C1 RID: 193
	public string reflectionSampler = "_ReflectionTex";

	// Token: 0x040000C2 RID: 194
	public float clipPlaneOffset = 0.07f;

	// Token: 0x040000C3 RID: 195
	private Vector3 oldpos = Vector3.zero;

	// Token: 0x040000C4 RID: 196
	private Camera reflectionCamera;

	// Token: 0x040000C5 RID: 197
	private Material sharedMaterial;

	// Token: 0x040000C6 RID: 198
	private Dictionary<Camera, bool> helperCameras;
}
