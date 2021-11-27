using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class DoFD : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if(InputManager.GetButton("Fire2")){
			transform.gameObject.GetComponent<DepthOfFieldDeprecated>().enabled = true;
		}else{
			transform.gameObject.GetComponent<DepthOfFieldDeprecated>().enabled = false;
		}
	}
}
