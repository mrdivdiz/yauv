using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFarisCam : MonoBehaviour {
	
	public static GameObject farisCamera;
	public GameObject farisCamera_temp;

	// Use this for initialization
	void Awake () {
		if(farisCamera == null){
			farisCamera = farisCamera_temp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
