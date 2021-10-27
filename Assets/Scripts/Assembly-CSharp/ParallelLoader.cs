using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallelLoader : MonoBehaviour {
	public int delay;
	public string scn;
	public bool m_isUnloading = true;
	AsyncOperation asyncLoad;
	// Use this for initialization
	void Start () {
		asyncLoad = SceneManager.LoadSceneAsync(scn);
		StartCoroutine(LoadAsyncScene(delay, scn));
		if(m_isUnloading){
		Resources.UnloadUnusedAssets();
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
}
