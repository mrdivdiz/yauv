using UnityEngine;
using System.Net.NetworkInformation;
using System;
using UnityEngine.PSVita;

public class ExampleFullScreenPlayback : MonoBehaviour
{
    public string m_MoviePath;
    public RenderTexture m_RenderTexture;
    public GUISkin m_Skin;
	public float m_Volume = 1.0f;
	public int m_AudioStreamIndex = 0;
	int m_AudioStreamMaxIndex = 4;
	public int m_TextStreamIndex = 0;
	int m_TextStreamMaxIndex = 4;
	GUIStyle m_TimeTextStyle;
	GUIStyle m_SubtitleTextStyle;
	string m_SubtitleText = "";
	long m_SubtitleTimeStamp;
	bool m_IsPlaying = false;

    void Start()
    {
		//Initialise the video player.
        PSVitaVideoPlayer.Init(m_RenderTexture);

		//Start the movie.
		PSVitaVideoPlayer.PlayParams vidParams = new PSVitaVideoPlayer.PlayParams();
		vidParams.volume = m_Volume;
		vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
		vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
		vidParams.audioStreamIndex = m_AudioStreamIndex;
		vidParams.textStreamIndex = m_TextStreamIndex;
		PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
	}

    void OnPostRender()
    {
        PSVitaVideoPlayer.Update();
    }

    void OnGUI()
    { 
		m_TimeTextStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
		m_TimeTextStyle.alignment = TextAnchor.MiddleCenter;

		m_SubtitleTextStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
		m_SubtitleTextStyle.alignment = TextAnchor.MiddleCenter;
		m_SubtitleTextStyle.fontSize = 16;
		
		GUIStyle areaStyle = GUI.skin.GetStyle("Button");
		areaStyle.fixedHeight = 0.0f;
		GUI.skin = m_Skin;

		GUILayout.BeginArea(new Rect(10, 10, 200, Screen.height-20), areaStyle);

        if (GUILayout.Button("Stop/Play"))
        {
			if(m_IsPlaying)
			{
				PSVitaVideoPlayer.Stop();
			}
			else
			{
				//Start the movie.
				PSVitaVideoPlayer.PlayParams vidParams = new PSVitaVideoPlayer.PlayParams();
				vidParams.volume = m_Volume;
				vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
				vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
				vidParams.audioStreamIndex = m_AudioStreamIndex;
				vidParams.textStreamIndex = m_TextStreamIndex;
				PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
            }
        }

		GUILayout.Label("Volume: " + m_Volume.ToString("0.00"));
		if (GUILayout.Button("Volume +"))
		{
			m_Volume += 0.1f;
			if (m_Volume > 1.0f)
			{
				m_Volume = 1.0f;
			}
			PSVitaVideoPlayer.SetVolume(m_Volume);
		}

		if (GUILayout.Button("Volume -"))
		{
			m_Volume -= 0.1f;
			if (m_Volume < 0.0f)
			{
				m_Volume = 0.0f;
			}
			PSVitaVideoPlayer.SetVolume(m_Volume);
		}

		GUILayout.Label("Audio Stream: " + m_AudioStreamIndex);
		if (GUILayout.Button("Audio Stream +"))
		{
			m_AudioStreamIndex++;
			if (m_AudioStreamIndex > m_AudioStreamMaxIndex)
			{
				m_AudioStreamIndex = 0;
			}

			PSVitaVideoPlayer.Stop();

            //NOTE: audioStreamIndex refers to the zero based Nth audio stream in the MP4, not the absolute MP4 stream number.
            //If no matching stream exists in the MP4 then audio is not played.
            PSVitaVideoPlayer.PlayParams vidParams = new PSVitaVideoPlayer.PlayParams();
			vidParams.volume = m_Volume;
			vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
			vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
			vidParams.audioStreamIndex = m_AudioStreamIndex;
			vidParams.textStreamIndex = m_TextStreamIndex;
			PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
		}

		if (GUILayout.Button("Audio Stream -"))
		{
			//NOTE: audioStreamIndex refers to the zero based Nth audio stream in the MP4, not the absolute MP4 stream number.
			//If no matching stream exists in the MP4 then audio is not played.
			m_AudioStreamIndex--;
			if (m_AudioStreamIndex < 0)
			{
				m_AudioStreamIndex = m_AudioStreamMaxIndex;
			}

			PSVitaVideoPlayer.Stop();

            //NOTE: audioStreamIndex refers to the zero based Nth audio stream in the MP4, not the absolute MP4 stream number.
            //If no matching stream exists in the MP4 then audio is not played.
            PSVitaVideoPlayer.PlayParams vidParams = new PSVitaVideoPlayer.PlayParams();
			vidParams.volume = m_Volume;
			vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
			vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
			vidParams.audioStreamIndex = m_AudioStreamIndex;
			vidParams.textStreamIndex = m_TextStreamIndex;
			PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
		}

		GUILayout.Label("Text Stream: " + m_TextStreamIndex);
		if (GUILayout.Button("Text Stream +"))
		{
			m_TextStreamIndex++;
			if (m_TextStreamIndex > m_TextStreamMaxIndex)
			{
				m_TextStreamIndex = 0;
			}

			PSVitaVideoPlayer.Stop();

            //NOTE: textStreamIndex refers to the zero based Nth text stream in the MP4, not the absolute MP4 stream number.
            //If no matching stream exists in the MP4 then text is not played.
            PSVitaVideoPlayer.PlayParams vidParams = new PSVitaVideoPlayer.PlayParams();
			vidParams.volume = m_Volume;
			vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
			vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
			vidParams.audioStreamIndex = m_AudioStreamIndex;
			vidParams.textStreamIndex = m_TextStreamIndex;
			PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
		}

		if (GUILayout.Button("Text Stream -"))
		{
			//NOTE: textStreamIndex refers to the zero based Nth text stream in the MP4, not the absolute MP4 stream number.
			//If no matching stream exists in the MP4 then text is not played.
			m_TextStreamIndex--;
			if (m_TextStreamIndex < 0)
			{
				m_TextStreamIndex = m_TextStreamMaxIndex;
			}

			PSVitaVideoPlayer.Stop();

            //NOTE: textStreamIndex refers to the zero based Nth text stream in the MP4, not the absolute MP4 stream number.
            //If no matching stream exists in the MP4 then text is not played.
            PSVitaVideoPlayer.PlayParams vidParams = new PSVitaVideoPlayer.PlayParams();
			vidParams.volume = m_Volume;
			vidParams.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
			vidParams.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
			vidParams.audioStreamIndex = m_AudioStreamIndex;
			vidParams.textStreamIndex = m_TextStreamIndex;
			PSVitaVideoPlayer.PlayEx(m_MoviePath, vidParams);
		}

		GUILayout.EndArea();

		GUI.Box(new Rect(220, Screen.height - 130, Screen.width - 230, 20)
				, "Time - " + UnityEngine.PSVita.PSVitaVideoPlayer.videoTime
				+ " / " + UnityEngine.PSVita.PSVitaVideoPlayer.videoDuration
				, m_TimeTextStyle);

		if (m_SubtitleText.Length > 0)
		{
			GUI.Box(new Rect(220, Screen.height - 110, Screen.width - 230, 20), "Subtitle Time - " + m_SubtitleTimeStamp, m_TimeTextStyle);
			GUI.Box(new Rect(220, Screen.height - 90, Screen.width - 230, 80), m_SubtitleText, m_SubtitleTextStyle);
		}
	}

	void OnMovieEvent(int eventID)
	{
		PSVitaVideoPlayer.MovieEvent movieEvent = (PSVitaVideoPlayer.MovieEvent)eventID;
		switch(movieEvent)
		{
			case PSVitaVideoPlayer.MovieEvent.PLAY:
				m_IsPlaying = true;
				break;

			case PSVitaVideoPlayer.MovieEvent.STOP:
				m_IsPlaying = false;
				m_SubtitleText = "";
				break;

			case PSVitaVideoPlayer.MovieEvent.TIMED_TEXT_DELIVERY:
				m_SubtitleText = UnityEngine.PSVita.PSVitaVideoPlayer.subtitleText;
				m_SubtitleTimeStamp = UnityEngine.PSVita.PSVitaVideoPlayer.subtitleTimeStamp;
				break;
		}
	}

}
