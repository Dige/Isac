using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backGroundTexture;

	void OnGUI()
	{
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height),backGroundTexture);

		if (GUI.Button (new Rect (Screen.width * 2 / 5, Screen.height * 2 / 5, Screen.width / 5, Screen.height / 5), "Play")) 
		{
			Application.LoadLevel("ProceduralGenerationTest");
		}
	}
}
