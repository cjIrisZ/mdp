﻿using UnityEngine;
using System.Collections;

/**
 * Script to keep track of stomach food blobs
 */
public class StomachFoodBlob : MonoBehaviour 
{
	public Sprite[] wholeFood;					//!< to hold the whole food representation images of each color
	public Sprite[] digestedFood;				//!< to hold the digested food representation images of each color
	
	public GameObject parent;					//!< the parent game object
	
	public float digestTime;					//!< Let developer setup time to digest the food
	
	private Sprite wholeRepresentation;			//!< to temporarily hold the whole food representation we are going to use
	private Sprite digestedRepresentation;		//!< to temporarily hold the digested food representation we are going to use
	
	private float spawnLocation;				//!< hold the spawn location for a food blob
	
	private int index;							//!< to keep track of the index. we will randomly assign a number to choose the color
	
	private float timer;						//!< to counte while the enzyme is on
	private StomachGameManager stomanager;
	
	
	
	// Use this for initialization
	void Start () 
	{
		stomanager = FindObjectOfType (typeof(StomachGameManager)) as StomachGameManager;
		
		// choose a random number to choose a color and select the correct images
		index = Random.Range (0, 4);
		wholeRepresentation = wholeFood [index];
		digestedRepresentation = digestedFood [index];
		
		// get the spawn location and assign the sprite image.
		spawnLocation = Random.Range (.6f * Screen.width, .85f * Screen.width);
		parent.transform.position = new Vector3 ((spawnLocation * 15f / Screen.width) - 7.5f, 
		                                         11f - (0f * 11f / Screen.height) - 5.5f, -2.0f);
		GetComponent<SpriteRenderer>().sprite = wholeRepresentation;
		
	}
	
	void Update()
	{
		if(stomanager.getCurrentAcidLevel() == "acidic")
		{
			timer = timer + Time.deltaTime;
		}
		if(timer> digestTime + 2f)
		{
			Destroy (this.gameObject);
			timer = 0;
		}
		else if(timer > digestTime)
		{
			Debug.Log("timerMax reached !");
			GetComponent<SpriteRenderer>().sprite = digestedRepresentation;
			
		}
		
	}
}