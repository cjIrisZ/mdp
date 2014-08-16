﻿using UnityEngine;
using System.Collections;

public class ZymePopupScript : MonoBehaviour 
{
	// texture and value for drawing zyme properly
	public Texture zyme;
	float ratio = 1.4250681198910081743869209809264f;

	// for the button
	public GUIStyle gotIt;

	// set the text for zyme
	private string currentZymeText;

	// booleans
	private bool drawZyme;
	private bool drawButton;
	private bool buttonPressed;

	// for fonts
	public GUIStyle style;

	// Use this for initialization
	void Start () 
	{
		// set the font size with code so it can be made relative to the screen. 
		// this can't be done in the unity editor as far as I know
		style.fontSize = (int)(18f / 597f * Screen.height);  // set font relative to screen 
		gotIt.fontSize = (int)(20f / 597f * Screen.height);	 // set font relative to screen
	}
	
	// Update is called once per frame
	void Update () {}

	// function to set the text to display in the zyme popup
	public void setText(string text)
	{
		currentZymeText = text;
	}

	// function to set the boolean on whether or not we should currently be drawing
	// the zyme popup
	public void setDraw(bool draw)
	{
		drawZyme = draw;
	}

	// function to set the boolean on whether or not we should currently be drawing
	// the button on the zyme popup (if the zyme popup is currently being drawn).
	// this means for the button to show both drawButton and drawZyme must be true.
	public void setShowButton(bool show)
	{
		drawButton = show;
	}

	// function to check if the button was pressed.
	// this is needed to help pass the message between this helper script and the 
	// main script that is wanting to display the zyme popup
	public bool getButtonPressed()
	{
		bool temp = buttonPressed;
		buttonPressed = false;
		return temp;
	}

	void OnGUI()
	{
		if (drawZyme)
		{
			// this part draws the zyme popup box in the bottom right corner with 
			// the current text that has been set in the "speaking box"
			GUI.DrawTexture(new Rect(Screen.width - (.4f * Screen.height * ratio), 
			                         (Screen.height * 0.82421875f) - (.4f * Screen.height),
			                         (.4f * Screen.height * ratio),
			                         (.4f * Screen.height)), zyme);
			GUI.Label(new Rect(.58f*Screen.width, .42f*Screen.height, .8f*Screen.width, .8f*Screen.height),
			          currentZymeText,
			          style);

			if(drawButton)
			{
				// this part draws the button that users click on to make the popup 
				// disappear.
				// the button is drawn below the "speaking box" on the zyme popup
				if (GUI.Button(new Rect(Screen.width - (.5112f * Screen.height), 
				                        (Screen.height * 0.82421875f) - (.15f * Screen.height),
				                        (.12f * Screen.width),
				                        (.1f * Screen.height)), "Got it!", gotIt))
				{
					// change the values of some variables if the button is clicked.
					buttonPressed = true;	// set this so the user can check if it was clicked
					drawButton = false;		// set this to false so the user doesn't have to turn it off
				}
			}
		}
	}
}
