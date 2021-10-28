using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockLoader : MonoBehaviour {
	public int delay;
	public string m_mainScn;
	public string[] m_additiveScn;
	public bool m_isUnloading = true;
	AsyncOperation asyncLoad;
	//AsyncOperation asyncAdditive;
	// Use this for initialization
	void Start () {
		asyncLoad = SceneManager.LoadSceneAsync(m_mainScn);
		StartCoroutine(LoadAsyncScene(delay, m_mainScn));
		foreach(string stir in m_additiveScn){
		AsyncOperation asyi = new AsyncOperation();
		asyi = SceneManager.LoadSceneAsync(stir, LoadSceneMode.Additive);
		asyi.allowSceneActivation = true;
		}
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
