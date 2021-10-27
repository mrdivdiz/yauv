using System;
using UnityEngine;

[Serializable]
public class Move : MonoBehaviour
{
	public Transform target;
	public float speed;
	public float smokeDestroyTime;
	public ParticleRenderer smokeStem;
	public float destroySpeed;
	public float destroySpeedStem;
}
