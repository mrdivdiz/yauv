using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TringLoaderRD : MonoBehaviour {

//public string scne;
public TringLoaderPre trlpr;
//AsyncOperation asyncLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerEnter(Collider collisionInfo)
	{
		if ((collisionInfo.tag == "Player" || collisionInfo.tag == "Bike" || collisionInfo.tag == "PlayerCar"))
		{
		trlpr.asyncLoad2.allowSceneActivation = true;
		}
	}
}
