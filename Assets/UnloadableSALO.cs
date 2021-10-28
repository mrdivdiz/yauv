using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadableSALO : MonoBehaviour {
	AsyncOperation asyncLoad;
	public PortableEffects eesalo;
	//public GameObject player;
	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider playercol) {
		
			//if (playercol.tag == "Player") {
			//eesalo.enabled = false;
			//}
	}
	void OnTriggerExit (Collider playercol2) {
		if (playercol2.tag == "Player") {
			eesalo.enabled = true;
			}
	
	}
}
