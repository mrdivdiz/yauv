using System;
using UnityEngine;

[Serializable]
public class PlayerRelativeControl : MonoBehaviour
{
	public Joystick moveJoystick;
	public Joystick rotateJoystick;
	public Transform cameraPivot;
	public float forwardSpeed;
	public float backwardSpeed;
	public float sidestepSpeed;
	public float jumpSpeed;
	public float inAirMultiplier;
	public Vector2 rotationSpeed;
}
