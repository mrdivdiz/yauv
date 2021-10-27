using System;
using UnityEngine;

[Serializable]
public class MouseOrbit : MonoBehaviour
{
	public Transform target;
	public float distance;
	public float xSpeed;
	public float ySpeed;
	public int yMinLimit;
	public int yMaxLimit;
}
