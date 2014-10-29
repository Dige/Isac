using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backGroundTexture;

	public Texture StartButtonTexture;
    public Texture StartHoverTexture;

	public Texture QuitButtonTexture;
    public Texture QuitHoverTexture;

    public AudioSource StartClip;

    public AudioSource QuitClip;

    private string _hover = string.Empty;

    private GUIContent _startButton = new GUIContent("Start", "Start");

	public void OnGUI()
	{
	   
        Debug.Log(_hover);
	    _startButton.image = _hover.Equals("Start") ? StartHoverTexture : StartButtonTexture;
        Debug.Log(_startButton.image.name);

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backGroundTexture);

		if (GUI.Button(new Rect(Screen.width / 4, Screen.height * 2 / 5, Screen.width / 4.0f, Screen.height / 4.0f), 
            _startButton, GUIStyle.none)) 
		{
		    if (StartClip != null)
		    {
		        StartClip.Play();
		    }
			Application.LoadLevel("ProceduralGenerationTest");
		}

		if (GUI.Button(new Rect((Screen.width / 4) * 2, Screen.height * 2 / 5, Screen.width / 4.0f, Screen.height / 4.0f),
            new GUIContent(_hover.Equals("Quit") ? QuitHoverTexture : QuitButtonTexture, "Quit"), GUIStyle.none))
		{
		    if (QuitClip != null)
		    {
		        QuitClip.Play();
		    }
			Application.Quit();
		}
        _hover = GUI.tooltip;
	}
}
