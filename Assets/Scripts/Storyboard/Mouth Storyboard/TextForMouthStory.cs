﻿using UnityEngine;
using System.Collections;
using System.IO;

public class TextForMouthStory : MonoBehaviour 
{
	MouthStoryboard mouthStoryboard;
	private string[] text;
	private float timer;

	// Use this for initialization
	void Start () 
	{
		mouthStoryboard = this.gameObject.GetComponent<MouthStoryboard> ();
		
		TextAsset mouthText = Resources.Load ("MouthText") as TextAsset;
		StringReader reader = new StringReader (mouthText.text);
		text = mouthText.text.Split("\n"[0]);	
		
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
	}

	void OnGUI()
	{
		GUI.depth--;
		
		GUIStyle statsStyle = GUI.skin.box;
		statsStyle.font = (Font)Resources.Load ("Fonts/JandaManateeSolid");
		statsStyle.normal.textColor = Color.yellow;
		statsStyle.fontSize = (int)(20f / 597f * Screen.height);
		statsStyle.wordWrap = true;
		statsStyle.alignment = TextAnchor.MiddleCenter;
		
		if (mouthStoryboard.getCurrPage() == 1)
		{
			GUI.Box(new Rect(.05f*Screen.width, (625f/768f)*Screen.height, .9f*Screen.width,
			                 .15f*Screen.height), text[0], statsStyle);
			timer = 0;
		} 
		
		if (mouthStoryboard.getCurrPage() == 2)
		{
			if (timer < 4)
			{
				GUI.Box(new Rect(.05f*Screen.width, (625f/768f)*Screen.height, .9f*Screen.width,
			    	             .15f*Screen.height), text[1], statsStyle);
			} else if (timer < 11)
			{
				GUI.Box(new Rect(.05f*Screen.width, (625f/768f)*Screen.height, .9f*Screen.width,
				                 .15f*Screen.height), text[2], statsStyle);
			} else
			{
				GUI.Box(new Rect(.05f*Screen.width, (625f/768f)*Screen.height, .9f*Screen.width,
				                 .15f*Screen.height), text[3], statsStyle);
			}
		}		
	}
}