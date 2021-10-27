using System;
using UnityEngine;

[Serializable]
public class CharacterMotorMovement
{
	public float maxForwardSpeed;
	public float maxSidewaysSpeed;
	public float maxBackwardsSpeed;
	public AnimationCurve slopeSpeedMultiplier;
	public float maxGroundAcceleration;
	public float maxAirAcceleration;
	public float gravity;
	public float maxFallSpeed;
}
