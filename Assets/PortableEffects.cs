using System;
using System.Collections;
using UnityEngine;


    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu("Image Effects/Portable Effects")]
    public class PortableEffects : MonoBehaviour
    {
		
	
        public enum SSAOSamples
		{
            Low = 0,
            Medium = 1,
            High = 2,
        }
		//[Tooltip("Shows FPS in top left corner.")]
		//public bool showFps = true;
        [Range(0.01f, 1.0f)]
		float deltaTime = 0.0f;
		public bool works = true;
		private Camera _camera;
        public float m_Radius = 0.037f;
		public bool m_ambientOcclusionEnabled = true;
		public bool m_motionBlurEnabled = true;
		public bool m_depthOfFieldEnabled = true;
		public bool m_ReflectionsEnabled = true;
        public SSAOSamples m_SampleCount = SSAOSamples.Medium;
        [Range(0.2f, 16.0f)]
        public float m_OcclusionIntensity = 2.63f;
        [Range(1,16)]
		public int m_DownsamplingDefault = 2;
        public int m_Downsampling = 2;
		public int m_DownsamplingRate = 1;
		public int m_targetFramerate = 30;
		public int m_framerateStep = 4;
        [Range(0.2f, 16.0f)]
        public float m_OcclusionAttenuation = 0.2f;
        [Range(0.00001f, 0.5f)]
        public float m_MinZ = 0.034f;

        public Shader m_SSAOShader;
        private Material m_SSAOMaterial;

        public Texture2D m_RandomTexture;

        private bool m_Supported;

        private static Material CreateMaterial (Shader shader)
        {
            if (!shader)
                return null;
            Material m = new Material (shader);
            m.hideFlags = HideFlags.HideAndDontSave;
            return m;
        }
        private static void DestroyMaterial (Material mat)
        {
            if (mat)
            {
                DestroyImmediate (mat);
                mat = null;
            }
        }


        void OnDisable()
        {
            DestroyMaterial (m_SSAOMaterial);
        }
		
		void Update(){
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		}

        void Start()
        {
			StartCoroutine(FPSCounter());
            /*if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth))
            {
                m_Supported = false;
                enabled = false;
                return;
            }

            CreateMaterials ();
            if (!m_SSAOMaterial || m_SSAOMaterial.passCount != 5)
            {
                m_Supported = false;
                enabled = false;
                return;
            }*/

            //CreateRandomTable (26, 0.2f);
			//_camera = GetComponent<Camera>();
           // _camera.depthTextureMode |= DepthTextureMode.Depth;
            m_Supported = true;
        }

        void OnEnable () {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        private void CreateMaterials ()
        {	if(works){
			//dofBlurMaterial = CheckShaderAndCreateMaterial (dofBlurShader, dofBlurMaterial);
            //dofMaterial = CheckShaderAndCreateMaterial (dofShader,dofMaterial);
			if (!m_SSAOMaterial && m_SSAOShader.isSupported)
            {
                m_SSAOMaterial = CreateMaterial (m_SSAOShader);
                m_SSAOMaterial.SetTexture ("_RandomTexture", m_RandomTexture);
            }
		}
            
        }

		float FocalDistance01 ( float worldDist) {
            return _camera.WorldToViewportPoint((worldDist-_camera.nearClipPlane) * _camera.transform.forward + _camera.transform.position).z / (_camera.farClipPlane-_camera.nearClipPlane);
        }

        int GetDividerBasedOnQuality () {
            return 2;
        }

        int GetLowResolutionDividerBasedOnQuality ( int baseDivider) {
            return baseDivider*2;
        }

        private RenderTexture foregroundTexture = null;
        private RenderTexture mediumRezWorkTexture = null;
        private RenderTexture finalDefocus = null;
        private RenderTexture lowRezWorkTexture = null;
        private RenderTexture bokehSource = null;
        private RenderTexture bokehSource2 = null;
		
        [ImageEffectOpaque]
        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
			//if (!m_Supported || !m_SSAOShader.isSupported) {
            //    enabled = false;                        //NOT WORKS WELL
            //    return;
            //}
			if(works){
				 CreateMaterials ();
				 m_Downsampling = Mathf.Clamp (m_Downsampling, 1, 16);
				m_Radius = Mathf.Clamp (m_Radius, 0.01f, 1.0f);
				m_MinZ = Mathf.Clamp (m_MinZ, 0.00001f, 0.5f);
				m_OcclusionIntensity = Mathf.Clamp (m_OcclusionIntensity, 0.5f, 16.0f);
				m_OcclusionAttenuation = Mathf.Clamp (m_OcclusionAttenuation, 0.2f, 2.0f);
				//RenderTexture rtAO = new RenderTexture();
				//rtAO = RenderTexture.GetTemporary (source.width / m_Downsampling, source.height / m_Downsampling, 0);
				//RenderTexture rtAO = RenderTexture.GetTemporary (source.width / m_Downsampling, source.height / m_Downsampling, 0);
				RenderTexture rtAO = RenderTexture.GetTemporary (source.width / m_Downsampling, source.height / m_Downsampling, 0, RenderTextureFormat.R8);
				
				if(m_ambientOcclusionEnabled){
			
           

            
			if(m_ambientOcclusionEnabled){
            float fovY = GetComponent<Camera>().fieldOfView;
            float far = GetComponent<Camera>().farClipPlane;
            float y = Mathf.Tan (fovY * Mathf.Deg2Rad * 0.5f) * far;
            float x = y * GetComponent<Camera>().aspect;
            m_SSAOMaterial.SetVector ("_FarCorner", new Vector3(x,y,far));
            int noiseWidth, noiseHeight;
            if (m_RandomTexture) {
                noiseWidth = m_RandomTexture.width;
                noiseHeight = m_RandomTexture.height;
            } else {
                noiseWidth = 1; noiseHeight = 1;
            }
            m_SSAOMaterial.SetVector ("_NoiseScale", new Vector3 ((float)rtAO.width / noiseWidth, (float)rtAO.height / noiseHeight, 0.0f));
            m_SSAOMaterial.SetVector ("_Params", new Vector4(
                                                     m_Radius,
                                                     m_MinZ,
                                                     1.0f / m_OcclusionAttenuation,
                                                     m_OcclusionIntensity));
			Graphics.Blit(source, rtAO, m_SSAOMaterial, (int)m_SampleCount);


            m_SSAOMaterial.SetTexture ("_SSAO", rtAO);
			}
            Graphics.Blit (source, destination, m_SSAOMaterial, 3);

            
			
				}/*
				bool  blurForeground = false;
            float focal01Size = focalSize / (_camera.farClipPlane - _camera.nearClipPlane);;

            if (simpleTweakMode) {
                focalDistance01 = objectFocus ? (_camera.WorldToViewportPoint (objectFocus.position)).z / (_camera.farClipPlane) : FocalDistance01 (focalPoint);
                focalStartCurve = focalDistance01 * smoothness;
                focalEndCurve = focalStartCurve;
                blurForeground = blurForeground && (focalPoint > (_camera.nearClipPlane + Mathf.Epsilon));
            }
            else {
                if (objectFocus) {
                    var vpPoint= _camera.WorldToViewportPoint (objectFocus.position);
                    vpPoint.z = (vpPoint.z) / (_camera.farClipPlane);
                    focalDistance01 = vpPoint.z;
                }
                else
                    focalDistance01 = FocalDistance01 (focalZDistance);

                focalStartCurve = focalZStartCurve;
                focalEndCurve = focalZEndCurve;
                blurForeground = blurForeground && (focalPoint > (_camera.nearClipPlane + Mathf.Epsilon));
            }

            widthOverHeight = (1.0f * source.width) / (1.0f * source.height);
            oneOverBaseSize = 1.0f / 512.0f;

            dofMaterial.SetFloat ("_ForegroundBlurExtrude", foregroundBlurExtrude);
            dofMaterial.SetVector ("_CurveParams", new Vector4 (simpleTweakMode ? 1.0f / focalStartCurve : focalStartCurve, simpleTweakMode ? 1.0f / focalEndCurve : focalEndCurve, focal01Size * 0.5f, focalDistance01));
            dofMaterial.SetVector ("_InvRenderTargetSize", new Vector4 (1.0f / (1.0f * source.width), 1.0f / (1.0f * source.height),0.0f,0.0f));

            int divider =  GetDividerBasedOnQuality ();
            int lowTexDivider = GetLowResolutionDividerBasedOnQuality (divider);

            AllocateTextures (blurForeground, source, divider, lowTexDivider);

            // WRITE COC to alpha channel
            // source is only being bound to detect y texcoord flip
            Graphics.Blit (source, source, dofMaterial, 3);

            // better DOWNSAMPLE (could actually be weighted for higher quality)
            Downsample (source, mediumRezWorkTexture);

            // BLUR A LITTLE first, which has two purposes
            // 1.) reduce jitter, noise, aliasing
            // 2.) produce the little-blur buffer used in composition later
            Blur (mediumRezWorkTexture, mediumRezWorkTexture, DofBlurriness.Low, 4, maxBlurSpread);

            if ((bokeh) && ((BokehDestination.Foreground & bokehDestination) != 0))
            {
                dofMaterial.SetVector ("_Threshhold", new Vector4(bokehThresholdContrast, bokehThresholdLuminance, 0.95f, 0.0f));

                // add and mark the parts that should end up as bokeh shapes
                Graphics.Blit (mediumRezWorkTexture, bokehSource2, dofMaterial, 11);

                // remove those parts (maybe even a little tittle bittle more) from the regurlarly blurred buffer
                //Graphics.Blit (mediumRezWorkTexture, lowRezWorkTexture, dofMaterial, 10);
                Graphics.Blit (mediumRezWorkTexture, lowRezWorkTexture);//, dofMaterial, 10);

                // maybe you want to reblur the small blur ... but not really needed.
                //Blur (mediumRezWorkTexture, mediumRezWorkTexture, DofBlurriness.Low, 4, maxBlurSpread);

                // bigger BLUR
                Blur (lowRezWorkTexture, lowRezWorkTexture, bluriness, 0, maxBlurSpread * bokehBlurAmplifier);
            }
            else  {
                // bigger BLUR
                Downsample (mediumRezWorkTexture, lowRezWorkTexture);
                Blur (lowRezWorkTexture, lowRezWorkTexture, bluriness, 0, maxBlurSpread);
            }

            dofBlurMaterial.SetTexture ("_TapLow", lowRezWorkTexture);
            dofBlurMaterial.SetTexture ("_TapMedium", mediumRezWorkTexture);
            Graphics.Blit (null, finalDefocus, dofBlurMaterial, 3);

            // we are only adding bokeh now if the background is the only part we have to deal with
            if ((bokeh) && ((BokehDestination.Foreground & bokehDestination) != 0))
                AddBokeh (bokehSource2, bokehSource, finalDefocus);

            dofMaterial.SetTexture ("_TapLowBackground", finalDefocus);
            dofMaterial.SetTexture ("_TapMedium", mediumRezWorkTexture); // needed for debugging/visualization

            // FINAL DEFOCUS (background)
            Graphics.Blit (source, blurForeground ? foregroundTexture : destination, dofMaterial, visualize ? 2 : 0);

            // FINAL DEFOCUS (foreground)
            if (blurForeground) {
                // WRITE COC to alpha channel
                Graphics.Blit (foregroundTexture, source, dofMaterial, 5);

                // DOWNSAMPLE (unweighted)
                Downsample (source, mediumRezWorkTexture);

                // BLUR A LITTLE first, which has two purposes
                // 1.) reduce jitter, noise, aliasing
                // 2.) produce the little-blur buffer used in composition later
                BlurFg (mediumRezWorkTexture, mediumRezWorkTexture, DofBlurriness.Low, 2, maxBlurSpread);

                if ((bokeh) && ((BokehDestination.Foreground & bokehDestination) != 0))
                {
                    dofMaterial.SetVector ("_Threshhold", new Vector4(bokehThresholdContrast * 0.5f, bokehThresholdLuminance, 0.0f, 0.0f));

                    // add and mark the parts that should end up as bokeh shapes
                    Graphics.Blit (mediumRezWorkTexture, bokehSource2, dofMaterial, 11);

                    // remove the parts (maybe even a little tittle bittle more) that will end up in bokeh space
                    //Graphics.Blit (mediumRezWorkTexture, lowRezWorkTexture, dofMaterial, 10);
                    Graphics.Blit (mediumRezWorkTexture, lowRezWorkTexture);//, dofMaterial, 10);

                    // big BLUR
                    BlurFg (lowRezWorkTexture, lowRezWorkTexture, bluriness, 1, maxBlurSpread * bokehBlurAmplifier);
                }
                else  {
                    // big BLUR
                    BlurFg (mediumRezWorkTexture, lowRezWorkTexture, bluriness, 1, maxBlurSpread);
                }

                // simple upsample once
                Graphics.Blit (lowRezWorkTexture, finalDefocus);

                dofMaterial.SetTexture ("_TapLowForeground", finalDefocus);
                Graphics.Blit (source, destination, dofMaterial, visualize ? 1 : 4);

                if ((bokeh) && ((BokehDestination.Foreground & bokehDestination) != 0))
                    AddBokeh (bokehSource2, bokehSource, destination);
            }

            ReleaseTextures ();
				*/
			
			RenderTexture.ReleaseTemporary (rtAO);
			}else{
				Graphics.Blit (source, destination);
			}
        }

		IEnumerator FPSCounter(){
		int w = Screen.width, h = Screen.height;

		//GUIStyle style = new GUIStyle();
	
		//Rect rect = new Rect(0, 0, w, h * 2 / 100);
		//style.alignment = TextAnchor.UpperLeft;
		//style.fontSize = h * 2 / 100;
		//style.normal.textColor = Color.white;
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		m_Downsampling = m_DownsamplingDefault;
		works = true;
		if(fps < m_targetFramerate - m_framerateStep){
			m_Downsampling = m_DownsamplingDefault + m_DownsamplingRate;
			if(fps < m_targetFramerate - m_framerateStep - m_framerateStep){
				m_Downsampling = m_DownsamplingDefault + m_DownsamplingRate + m_DownsamplingRate;
				if(fps < m_targetFramerate - m_framerateStep - m_framerateStep - m_framerateStep){
					m_Downsampling = m_DownsamplingDefault + m_DownsamplingRate + m_DownsamplingRate + m_DownsamplingRate;
					if(fps < m_targetFramerate - m_framerateStep - m_framerateStep - m_framerateStep - m_framerateStep){
						works = false;
					}
				}
			}
		}
		//string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		//if(showFps){
		//GUI.Label(rect, text, style);
		//}
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(FPSCounter());
		Debug.Log("BEPIS");
		}
    }
