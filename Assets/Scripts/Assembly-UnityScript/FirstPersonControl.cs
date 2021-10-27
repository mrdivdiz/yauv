using System;
using UnityEngine;

[Serializable]
public class FirstPersonControl : MonoBehaviour
{
	public Joystick moveTouchPad;
	public Joystick rotateTouchPad;
	public Transform cameraPivot;
	public float forwardSpeed;
	public float backwardSpeed;
	public float sidestepSpeed;
	public float jumpSpeed;
	public float inAirMultiplier;
	public Vector2 rotationSpeed;
	public float tiltPositiveYAxis;
	public float tiltNegativeYAxis;
	public float tiltXAxisMinimum;
}
