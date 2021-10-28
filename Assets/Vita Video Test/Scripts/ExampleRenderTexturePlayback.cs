using UnityEngine;
using UnityEngine.PSVita;
using System.Collections;

public class ExampleRenderTexturePlayback : MonoBehaviour
{
    public string m_MoviePath;
    public RenderTexture m_RenderTexture;
    public GUISkin m_Skin;
	bool m_IsPlaying = false;
	private IEnumerator coroutine;

    void Start()
    {
		// = plei();
        PSVitaVideoPlayer.Init(m_RenderTexture);
		//PSVitaVideoPlayer.Stop();
        PSVitaVideoPlayer.Play(m_MoviePath, PSVitaVideoPlayer.Looping.None, PSVitaVideoPlayer.Mode.RenderToTexture);
    //StartCoroutine(coroutine);
	}

    void OnPreRender()
    {
        PSVitaVideoPlayer.Update();
    }
	void Update () {
		if(InputManager.GetButton("Fire1")){
			PSVitaVideoPlayer.Stop();
		}
	}

   /* void OnGUI()
    {
        GUI.skin = m_Skin;
        GUILayout.BeginArea(new Rect(10,10,200,Screen.height));
        if (GUILayout.Button("Stop/Play"))
        {
			if (m_IsPlaying)
			{
				PSVitaVideoPlayer.Stop();
			}
			else
			{
                PSVitaVideoPlayer.Init(m_RenderTexture);
				PSVitaVideoPlayer.Play(m_MoviePath, PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.RenderToTexture);
			plei();
			}
        }
        GUILayout.EndArea();
    }*/

	/*void OnMovieEvent(int eventID)
	{
		PSVitaVideoPlayer.MovieEvent movieEvent = (PSVitaVideoPlayer.MovieEvent)eventID;
		switch (movieEvent)
		{
			case PSVitaVideoPlayer.MovieEvent.PLAY:
				m_IsPlaying = true;
				break;

			case PSVitaVideoPlayer.MovieEvent.STOP:
				m_IsPlaying = false;
				break;
		}
	}*/
	
	/*IEnumerator plei(){
		yield return new WaitForSeconds(0.2f);
		PSVitaVideoPlayer.Init(m_RenderTexture);
		PSVitaVideoPlayer.Play(m_MoviePath, PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.RenderToTexture);
		yield break;
	}*/
}
