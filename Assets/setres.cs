using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setres : MonoBehaviour {

	public int width = 720;
	public int height = 408;
	// Use this for initialization
	void Start () {
		Screen.SetResolution(width, height, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
