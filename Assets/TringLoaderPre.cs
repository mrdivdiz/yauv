using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TringLoaderPre : MonoBehaviour {

public string scne;
public AsyncOperation asyncLoad2;

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
			asyncLoad2 = SceneManager.LoadSceneAsync(scne);
		//StartCoroutine(LoadAsyncScene(scne));
		asyncLoad2.allowSceneActivation = true;
		}
	}
}
