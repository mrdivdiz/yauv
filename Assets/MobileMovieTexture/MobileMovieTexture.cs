using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace MMT
{
    public class MobileMovieTexture : MonoBehaviour
    {
        #region Types

        public delegate void OnFinished(MobileMovieTexture sender);

        #endregion

        #region Editor Variables

        /// <summary>
        /// File path to the video file, includes the extension, usually .ogg or .ogv
        /// </summary>
        [SerializeField]
        private string m_path;

        /// <summary>
        /// Whether the path is absolute or in the streaming assets directory
        /// </summary>
        [SerializeField]
        private bool m_absolutePath;

        /// <summary>
        /// Material(s) to decode the movie on to, MMT sets up the textures and the texture scale/offset
        /// </summary>
        [SerializeField]
        private Material[] m_movieMaterials;

        /// <summary>
        /// Whether to start playing automatically, be careful to set advance
        /// </summary>
        [SerializeField]
        private bool m_playAutomatically = true;

        /// <summary>
        /// Whether the movie should advance, used to pause, or also to just decode the first frame
        /// </summary>
        [SerializeField]
        private bool m_advance = true;

        /// <summary>
        /// How many times to loop, -1 == infinite
        /// </summary>
        [SerializeField]
        private int m_loopCount = -1;

        /// <summary>
        /// Playback speed, has to be positive, can't play backwards
        /// </summary>
        [SerializeField]
        private float m_playSpeed = 1.0f;

        /// <summary>
        /// Whether to scan the duration of the movie on opening. This makes opening movies more expensive as it reads the whole file. Ideally cache this off if you need it
        /// </summary>
        [SerializeField]
        private bool m_scanDuration = true;

        /// <summary>
        /// When seeking, it tries to find a keyframe to seek to, however it often fails. If this is set, after a seek, it will decode till it hits a keyframe. Without it set, you may see artifacts on a seek
        /// </summary>
        [SerializeField]
        private bool m_seekKeyFrame = false;

        #endregion

        #region Other Variables

        private IntPtr m_nativeContext = IntPtr.Zero;
        private IntPtr m_nativeTextureContext = IntPtr.Zero;

        private int m_picWidth = 0;
        private int m_picHeight = 0;
        private int m_picX = 0;
        private int m_picY = 0;

        private int m_yStride = 0;
        private int m_yHeight = 0;
        private int m_uvStride = 0;
        private int m_uvHeight = 0;

        private Vector2 m_uvYScale;
        private Vector2 m_uvYOffset;
        
        private Vector2 m_uvCrCbScale;
        private Vector2 m_uvCrCbOffset;

        private const int CHANNELS = 3; //Y,Cr,Cb
        private Texture2D[] m_ChannelTextures = new Texture2D[CHANNELS];

        private double m_elapsedTime;

        private bool m_hasFinished = true;

        #endregion

        /// <summary>
        /// Function to call on finish
        /// </summary>
        public event OnFinished onFinished;

        #region Properties

        /// <summary>
        /// File path to the video file, includes the extension, usually .ogg or .ogv
        /// </summary>
        public string Path { get { return m_path; } set { m_path = value; } }

        /// <summary>
        /// Whether the path is absolute or in the streaming assets directory
        /// </summary>
        public bool AbsolutePath { get { return m_absolutePath; } set { m_absolutePath = value; } }

        /// <summary>
        /// Material to decode the movie on to, MMT sets up the textures and the texture scale/offset
        /// </summary>
        public Material[] MovieMaterial { get { return m_movieMaterials; } }

        /// <summary>
        /// Whether to start playing automatically, be careful to set advance
        /// </summary>
        public bool PlayAutomatically { set { m_playAutomatically = value; } }

        /// <summary>
        /// How many times to loop, -1 == infinite
        /// </summary>
        public int LoopCount { get { return m_loopCount; } set { m_loopCount = value; } }

        /// <summary>
        /// Playback speed, has to be positive, can't play backwards
        /// </summary>
        public float PlaySpeed { get { return m_playSpeed; } set { m_playSpeed = value; } }

        /// <summary>
        /// Whether to scan the duration of the movie on opening. This makes opening movies more expensive as it reads the whole file. Ideally cache this off if you need it
        /// </summary>
        public bool ScanDuration { get { return m_scanDuration; } set { m_scanDuration = value; } }

        /// <summary>
        /// When seeking, it tries to find a keyframe to seek to, however it often fails. If this is set, after a seek, it will decode till it hits a keyframe. Without it set, you may see artifacts on a seek
        /// </summary>
        public bool SeekKeyFrame { get { return m_seekKeyFrame; } set { m_seekKeyFrame = value; } }

        /// <summary>
        /// Width of the movie in pixels
        /// </summary>
        public int Width { get { return m_picWidth; } }

        /// <summary>
        /// Height of the movie in pixels
        /// </summary>
        public int Height { get { return m_picHeight; } }

        /// <summary>
        /// Aspect ratio (width/height) of movie
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if (m_nativeContext != IntPtr.Zero)
                {
                    return GetAspectRatio(m_nativeContext);
                }
                else
                {
                    return 1.0f;
                }
            }
        }

        /// <summary>
        /// Frames per second of movie
        /// </summary>
        public double FPS
        {
            get
            {
                if (m_nativeContext != IntPtr.Zero)
                {
                    return GetVideoFPS(m_nativeContext);
                }
                else
                {
                    return 1.0;
                }
            }
        }

        /// <summary>
        /// Is the movie currently playing
        /// </summary>
        public bool isPlaying
        {
            get { return m_nativeContext != IntPtr.Zero && !m_hasFinished && m_advance; }
        }

        public bool pause { get { return !m_advance; } set { m_advance = !value; } }

        /// <summary>
        /// Use this to retrieve the play position and to seek. NB after you seek, the play position will not be exactly what you seeked to, as it is tries to find a key frame
        /// </summary>
        public double playPosition
        {
            get { return m_elapsedTime; }
            set 
            {
                if (m_nativeContext != IntPtr.Zero)
                {
                    m_elapsedTime = Seek(m_nativeContext, value, m_seekKeyFrame);
                }
            }
        }

        /// <summary>
        /// The lenght of the movie, this is only valid if you have ScanDuration set
        /// </summary>
        public double duration
        {
            get { return m_nativeContext != IntPtr.Zero ? GetDuration(m_nativeContext) : 0.0; }
        }

        #endregion

        #region Native Interface

#if UNITY_IPHONE && !UNITY_EDITOR
    private const string PLATFORM_DLL = "__Internal";
#else
        private const string PLATFORM_DLL = "theorawrapper";
#endif
        [DllImport(PLATFORM_DLL)]
        private static extern IntPtr CreateContext();

        [DllImport(PLATFORM_DLL)]
        private static extern void DestroyContext(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern bool OpenStream(IntPtr context, string path, int offset, int size, bool pot, bool scanDuration, int maxSkipFrames);

        [DllImport(PLATFORM_DLL)]
        private static extern void CloseStream(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetPicWidth(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetPicHeight(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetPicX(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetPicY(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetYStride(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetYHeight(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetUVStride(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern int GetUVHeight(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern bool HasFinished(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern double GetDecodedFrameTime(IntPtr context);

		[DllImport(PLATFORM_DLL)]
		private static extern double GetUploadedFrameTime(IntPtr context);

		[DllImport(PLATFORM_DLL)]
		private static extern double GetTargetDecodeFrameTime(IntPtr context);
        
        [DllImport(PLATFORM_DLL)]
        private static extern void SetTargetDisplayDecodeTime(IntPtr context, double targetTime);

        [DllImport(PLATFORM_DLL)]
        private static extern double GetVideoFPS(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern float GetAspectRatio(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern double Seek(IntPtr context, double seconds, bool waitKeyFrame);

        [DllImport(PLATFORM_DLL)]
        private static extern double GetDuration(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern IntPtr GetNativeYHandle(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern IntPtr GetNativeCrHandle(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern IntPtr GetNativeCbHandle(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern IntPtr GetNativeTextureContext(IntPtr context);

        [DllImport(PLATFORM_DLL)]
        private static extern void SetPostProcessingLevel(IntPtr context, int level);

        #endregion

        #region Behaviour Overrides

        void Start()
        {
            m_nativeContext = CreateContext();

            if (m_nativeContext == IntPtr.Zero)
            {
                Debug.LogError("Unable to create Mobile Movie Texture native context");
                return;
            }

            if (m_playAutomatically)
            {
                Play();
            }
        }

        void OnDestroy()
        {
            DestroyTextures();
            DestroyContext(m_nativeContext);
        }

        void Update()
        {
            if (m_nativeContext != IntPtr.Zero && !m_hasFinished)
            {
                //Texture context can change when resizing windows
                //when put into the background etc
                var textureContext = GetNativeTextureContext(m_nativeContext);

                if (textureContext != m_nativeTextureContext)
                {
                    DestroyTextures();
                    AllocateTexures();

                    textureContext = m_nativeTextureContext;
                }

                m_hasFinished = HasFinished(m_nativeContext);

                if (!m_hasFinished)
                {
                    if (m_advance)
                    {
                        m_elapsedTime += Time.deltaTime * Mathf.Max(m_playSpeed, 0.0f);
                    }
                }
                else
                {
                    if ((m_loopCount - 1) > 0 || m_loopCount == -1)
                    {
                        if (m_loopCount != -1)
                        {
                            m_loopCount--;
                        }

                        m_elapsedTime = m_elapsedTime % GetDecodedFrameTime(m_nativeContext);

                        Seek(m_nativeContext, 0, false);

                        m_hasFinished = false;
                    }
                    else if (onFinished != null)
                    {
						m_elapsedTime = GetDecodedFrameTime(m_nativeContext);

                        onFinished(this);
                    }

                }

                SetTargetDisplayDecodeTime(m_nativeContext, m_elapsedTime);

            }
        }


        #endregion

        #region Methods

        public void Play()
        {
            m_elapsedTime = 0.0;

            Open();

            m_hasFinished = false;

            //Create a manager if we don't have one
            if (MobileMovieManager.Instance == null)
            {
                gameObject.AddComponent<MobileMovieManager>();
            }
        }

        public void Stop()
        {
            CloseStream(m_nativeContext);
			m_hasFinished = true;
        }


#if UNITY_EDITOR
    private bool SanityCheckPath(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            Debug.LogError("Unable to find movie at path: " + System.IO.Path.GetFullPath(path));
            return false;
        }

        if (path.Contains("\\"))
        {
            Debug.LogError("Movie path contains a back slash, these don't work on Android, please use forward slashes, path " + path);
            return false;
        }

        return true;
    }
#endif //UNITY_EDITOR

        private void Open()
        {
            string path = m_path;
            long offset = 0;
            long length = 0;

            if (!m_absolutePath)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        path = Application.dataPath;

                        if (!AssetStream.GetZipFileOffsetLength(Application.dataPath, m_path, out offset, out length))
                        {
                            throw new System.Exception("problem opening movie");
                        }
                        break;
                    default:
                        path = Application.streamingAssetsPath + "/" + m_path;
                        break;
                }
            }


#if UNITY_EDITOR
            if (!SanityCheckPath(path))
            {
                throw new System.Exception("Can't find video at path" + path);
            }
#endif //UNITY_EDITOR

            //No platform should need power of 2 textures anymore
            const bool powerOf2Textures = false;

            //This is maximum frames decoded/uploaded
            const int maxSkipFrames = 16;

            if (m_nativeContext != IntPtr.Zero && OpenStream(m_nativeContext, path, (int)offset, (int)length, powerOf2Textures, m_scanDuration, maxSkipFrames))
            {
                m_picWidth = GetPicWidth(m_nativeContext);
                m_picHeight = GetPicHeight(m_nativeContext);

                m_picX = GetPicX(m_nativeContext);
                m_picY = GetPicY(m_nativeContext);

                int yStride = GetYStride(m_nativeContext);
                int yHeight = GetYHeight(m_nativeContext);
                int uvStride = GetUVStride(m_nativeContext);
                int uvHeight = GetUVHeight(m_nativeContext);

                m_yStride = yStride;
                m_yHeight = yHeight;
                m_uvStride = uvStride;
                m_uvHeight = uvHeight;

                CalculateUVScaleOffset();
            }
            else
            {
                throw new System.Exception("problem opening movie");
            }
        }

        private void AllocateTexures()
        {
            m_ChannelTextures[0] = Texture2D.CreateExternalTexture(m_yStride, m_yHeight, TextureFormat.BGRA32, false, false, GetNativeYHandle(m_nativeContext));
            m_ChannelTextures[1] = Texture2D.CreateExternalTexture(m_uvStride, m_uvHeight, TextureFormat.RGBA32, false, false, GetNativeCrHandle(m_nativeContext));
            m_ChannelTextures[2] = Texture2D.CreateExternalTexture(m_uvStride, m_uvHeight, TextureFormat.RGBA32, false, false, GetNativeCbHandle(m_nativeContext));

            if (m_movieMaterials != null)
            {
                for (int i = 0; i < m_movieMaterials.Length; ++i)
                {
                    var mat = m_movieMaterials[i];

                    if (mat != null)
                    {
                        SetTextures(mat);
                    }
                }
            }
            
        }

        public void SetTextures(Material material)
        {
            material.SetTexture("_YTex", m_ChannelTextures[0]);
            material.SetTexture("_CrTex", m_ChannelTextures[1]);
            material.SetTexture("_CbTex", m_ChannelTextures[2]);

            material.SetTextureScale("_YTex", m_uvYScale);
            material.SetTextureOffset("_YTex", m_uvYOffset);

            material.SetTextureScale("_CbTex", m_uvCrCbScale);
            material.SetTextureOffset("_CbTex", m_uvCrCbOffset);
        }

        public void RemoveTextures(Material material)
        {
            material.SetTexture("_YTex", null);
            material.SetTexture("_CrTex", null);
            material.SetTexture("_CbTex", null);
        }

        private void CalculateUVScaleOffset()
        {
            m_uvYScale = new Vector2((float)(m_picWidth) / (float)(m_yStride), -((float)(m_picHeight) / (float)(m_yHeight)));
            m_uvYOffset = new Vector2((float)m_picX / (float)(m_yStride), ((float)(m_picHeight) + (float)(m_picY)) / (float)(m_yHeight));

            m_uvCrCbScale = new Vector2();
            m_uvCrCbOffset = new Vector2();

            if (m_uvStride == m_yStride)
            {
                m_uvCrCbScale.x = m_uvYScale.x;
            }
            else
            {
                m_uvCrCbScale.x = ((float)(m_picWidth) / 2.0f) / (float)(m_uvStride);
            }

            if (m_uvHeight == m_yHeight)
            {
                m_uvCrCbScale.y = m_uvYScale.y;
                m_uvCrCbOffset = m_uvYOffset;
            }
            else
            {
                m_uvCrCbScale.y = -(((float)(m_picHeight) / 2.0f) / (float)(m_uvHeight));
                m_uvCrCbOffset = new Vector2((((float)m_picX) / 2.0f) / (float)(m_uvStride), ((((float)(m_picHeight) + (float)(m_picY)) / 2.0f) / (float)(m_uvHeight)));
            }
        }

        private void DestroyTextures()
        {
            if (m_movieMaterials != null)
            {
                for (int i = 0; i < m_movieMaterials.Length; ++i)
                {
                    var mat = m_movieMaterials[i];

                    if (mat != null)
                    {
                        RemoveTextures(mat);
                    }
                }
            }

            for (int i = 0; i < CHANNELS; ++i)
            {
                if (m_ChannelTextures[i] != null)
                {
                    Destroy(m_ChannelTextures[i]);
                    m_ChannelTextures[i] = null;
                }
            }
        }

       
        #endregion
    }
}

