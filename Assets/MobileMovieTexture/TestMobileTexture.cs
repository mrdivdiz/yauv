using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MMT.MobileMovieTexture))]
public class TestMobileTexture : MonoBehaviour 
{
    private MMT.MobileMovieTexture m_movieTexture;

    void Awake()
    {
        m_movieTexture = GetComponent<MMT.MobileMovieTexture>();

        m_movieTexture.onFinished += OnFinished;
    }

    void OnFinished(MMT.MobileMovieTexture sender)
    {
        Debug.Log(sender.Path + " has finished ");
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0.0f, 0.0f, Screen.width, Screen.height));

        var currentPosition = (float)m_movieTexture.playPosition;

        var newPosition = GUILayout.HorizontalSlider(currentPosition,0.0f,(float)m_movieTexture.duration);

        if (newPosition != currentPosition)
        {
			m_movieTexture.playPosition = newPosition;
        }
        
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
		if (GUILayout.Button("Play"))
		{
			m_movieTexture.Play ();
        }
        
        if (GUILayout.Button("Play/Pause"))
        {
			m_movieTexture.pause = !m_movieTexture.pause;
        }

		if (GUILayout.Button("Stop"))
		{
			m_movieTexture.Stop();
		}

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();

     }
}
