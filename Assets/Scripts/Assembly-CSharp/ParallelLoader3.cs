using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallelLoader3 : MonoBehaviour {
	public int delay = 0;
	public int delayd = 5;
	public string scn;
	public bool m_isUnloading = true;
	AsyncOperation asyncLoad;
	// Use this for initialization
	
	void OnTriggerEnter (Collider playercol) {
		
			if (playercol.tag == "Player") {
			StartCoroutine(DelayedLoad(delayd, m_isUnloading));
			Debug.Log("TEnter");
			}
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.GetButton("Fire1")){
			asyncLoad.allowSceneActivation = true;
		}
	}
	IEnumerator LoadAsyncScene(int delayed, string scName)
    {
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scName);
        asyncLoad.allowSceneActivation = false;
		Debug.Log("Loading :" + asyncLoad.progress);
        //When the load is still in progress, output the Text and progress bar
        yield return new WaitForSeconds(delayed);
		asyncLoad.allowSceneActivation = true;
	}
	IEnumerator DelayedLoad(int delayed2, bool undloadr)
    {
       yield return new WaitForSeconds(delayed2);
		asyncLoad = SceneManager.LoadSceneAsync(scn);
		StartCoroutine(LoadAsyncScene(delayed2, scn));
		if(undloadr){
		Resources.UnloadUnusedAssets();
		}
	}
}
