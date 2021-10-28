using UnityEngine;
using UnityEngine.PSVita;
using System.Collections;

public class ExampleRenderTexturePlayback2 : MonoBehaviour
{
    public string m_MoviePath;
    public RenderTexture m_RenderTexture;
    public GUISkin m_Skin;
	bool m_IsPlaying = false;
	private IEnumerator coroutine;

    void Start()
    {
		//coroutine = plei();
        PSVitaVideoPlayer.Init(m_RenderTexture);
		//PSVitaVideoPlayer.Stop();
        PSVitaVideoPlayer.Play(m_MoviePath, PSVitaVideoPlayer.Looping.None, PSVitaVideoPlayer.Mode.RenderToTexture);
    //StartCoroutine(coroutine);
	}

    void OnPreRender()
    {
        PSVitaVideoPlayer.Update();
    }
	void Update()
    {
		if(m_IsPlaying = false){
        PSVitaVideoPlayer.Play(m_MoviePath, PSVitaVideoPlayer.Looping.None, PSVitaVideoPlayer.Mode.RenderToTexture);
		}
    }

	void OnMovieEvent(int eventID)
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
	}
	
	
}
