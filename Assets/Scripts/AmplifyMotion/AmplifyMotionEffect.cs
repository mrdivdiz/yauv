using UnityEngine;
using AmplifyMotion;

public class AmplifyMotionEffect : MonoBehaviour
{
	public Quality QualityLevel;
	public bool AutoRegisterObjs;
	public Camera[] OverlayCameras;
	public LayerMask CullingMask;
	public int QualitySteps;
	public float MotionScale;
	public float MaxVelocity;
}
