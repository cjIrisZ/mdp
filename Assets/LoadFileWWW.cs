﻿using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class LoadFileWWW : MonoBehaviour 
{
	public string fileName;

	private WWW w;
	private AudioSource audioSource;
	private bool loaded = false;

	private string labelText;

	// Use this for initialization
	void Start () 
	{
		//Get the attached AudioSource component
		audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!loaded && w != null && w.isDone)
		{
			loaded = true;
			audioSource.clip = w.audioClip;
			audioSource.Play ();
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2+75, 200, 50), "Replay"))
		{
			string fileURL = Path.Combine(Application.persistentDataPath, fileName);
			if (File.Exists(fileURL))
			{
				labelText = fileURL;
				Debug.Log (fileURL);
				w = new WWW("file:///" + fileURL);
				loaded = false;
			} else
			{
				Debug.Log ("invalid path");
				labelText = "invalidPath";
			}
		}

		GUI.Label(new Rect(Screen.width/2-100, Screen.height/2+125, 200, 50), labelText);
	}
}
