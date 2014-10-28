using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backGroundTexture;

	public Texture StartButtonTexture;
	public Texture QuitButtonTexture;

	public void OnGUI()
	{
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height),backGroundTexture);

		if (GUI.Button(new Rect(Screen.width / 4, Screen.height * 2 / 5, Screen.width / 4.0f, Screen.height / 4.0f), StartButtonTexture, GUIStyle.none)) 
		{
			Application.LoadLevel("ProceduralGenerationTest");
		}

		if (GUI.Button(new Rect((Screen.width / 4) * 2, Screen.height * 2 / 5, Screen.width / 4.0f, Screen.height / 4.0f), QuitButtonTexture, GUIStyle.none))
		{
			Application.Quit();
		}
	}
}
