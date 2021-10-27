using System;
using UnityEngine;

[Serializable]
public class CharacterMotor : MonoBehaviour
{
	public bool canControl;
	public bool useFixedUpdate;
	public CharacterMotorMovement movement;
	public CharacterMotorJumping jumping;
	public CharacterMotorMovingPlatform movingPlatform;
	public CharacterMotorSliding sliding;
}
