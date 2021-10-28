using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadableBoks : MonoBehaviour {
	AsyncOperation asyncLoad;
	public string scn;
	//public GameObject player;
	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider playercol) {
		
			if (playercol.tag == "Player") {
			asyncLoad = SceneManager.LoadSceneAsync(scn, LoadSceneMode.Additive	);
			asyncLoad.allowSceneActivation = true;
			Resources.UnloadUnusedAssets();
			//StartCoroutine(LoadAsyncScene(0, scn));
			Debug.Log("TEnter");
			}
	}
	void OnTriggerExit (Collider playercol2) {
		if (playercol2.tag == "Player") {
			asyncLoad = SceneManager.UnloadSceneAsync(scn);
			Resources.UnloadUnusedAssets();
			}
	
	}
}
