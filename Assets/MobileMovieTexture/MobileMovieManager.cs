using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace MMT
{
    public class MobileMovieManager : MonoBehaviour
    {
        public static MobileMovieManager Instance;

#if UNITY_IPHONE && !UNITY_EDITOR
        private const string PLATFORM_DLL = "__Internal";
#else
        private const string PLATFORM_DLL = "theorawrapper";
#endif

        [DllImport(PLATFORM_DLL)]
        private static extern void UnityRenderEvent(int eventID);

        [DllImport(PLATFORM_DLL)]
        private static extern void UnitySetGraphicsDevice(System.IntPtr device, int deviceType, int eventType);

        enum GfxDeviceRenderer
        {
            kGfxRendererOpenGL = 0,          // OpenGL
            kGfxRendererD3D9,                // Direct3D 9
            kGfxRendererD3D11,               // Direct3D 11
            kGfxRendererGCM,                 // Sony PlayStation 3 GCM
            kGfxRendererNull,                // "null" device (used in batch mode)
            kGfxRendererHollywood,           // Nintendo Wii
            kGfxRendererXenon,               // Xbox 360
            kGfxRendererOpenGLES,            // OpenGL ES 1.1
            kGfxRendererOpenGLES20Mobile,    // OpenGL ES 2.0 mobile variant
            kGfxRendererMolehill,            // Flash 11 Stage3D
            kGfxRendererOpenGLES20Desktop,   // OpenGL ES 2.0 desktop variant (i.e. NaCl)
            kGfxRendererCount
        };

        enum GfxDeviceEventType
        {
            kGfxDeviceEventInitialize = 0,
            kGfxDeviceEventShutdown,
            kGfxDeviceEventBeforeReset,
            kGfxDeviceEventAfterReset,
        };

        void Awake()
        {
            Instance = this;

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            UnitySetGraphicsDevice(System.IntPtr.Zero, (int)GfxDeviceRenderer.kGfxRendererOpenGLES20Mobile, (int)GfxDeviceEventType.kGfxDeviceEventInitialize);
            GL.InvalidateState();
#endif
        }

        void OnEnable()
        {
            StartCoroutine(DecodeCoroutine());
        }

        void OnDisable()
        {
            StopCoroutine("DecodeCoroutine");
        }

        private IEnumerator DecodeCoroutine()
        {
            // Wait until all frame rendering is done
            while (true)
            {
                //Start a frame decoding
                yield return new WaitForEndOfFrame();

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
                UnityRenderEvent(7);
#else
                GL.IssuePluginEvent(7);
#endif
				GL.InvalidateState();
            }
        }
    }
}
