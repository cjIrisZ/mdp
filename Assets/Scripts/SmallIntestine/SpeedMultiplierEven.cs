﻿using UnityEngine;
using System.Collections;

public class SpeedMultiplierEven : MonoBehaviour 
{
	bool value;
	public GUIStyle speedButtonOff;
	public GUIStyle speedButtonOn;

	// Use this for initialization
	void Start () 
	{
		value = false;
	}
	
	// Update is called once per frame
	void Update () {}
	
	void OnGUI()
	{
		if(value && 
		   GUI.Button(new Rect(Screen.width * .9f, Screen.height * .745f, 
		                    Screen.width * .05f, Screen.width * .05f), "", speedButtonOn))
		{
			value = false;
		}
		if(!value && 
		   GUI.Button(new Rect(Screen.width * .9f, Screen.height * .745f, 
		                    Screen.width * .05f, Screen.width * .05f), "", speedButtonOff))
		{
			value = true;
		}

		// this changes the speed by alteriing the time scale of the game
		if (value && Time.timeScale == 1)
			Time.timeScale = 2;
		else if(!value && Time.timeScale == 2)
			Time.timeScale = 1;
	}
}