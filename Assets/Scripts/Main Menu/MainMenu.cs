using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	public Texture background;
	public GUIStyle startBtn;
	void OnGUI()
    {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), background);

		if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.8f,
		                        Screen.width * 0.2f, Screen.height * 0.1f), "", startBtn))
        {
           // Application.LoadLevel("LoadLevelSmallIntestine");
			Application.LoadLevel("IntroStoryboard");
        }
    }
}