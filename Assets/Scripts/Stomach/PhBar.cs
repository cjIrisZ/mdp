﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * script that controls drawing the phBar and level indicators for the stomach game
 */
public class PhBar : MonoBehaviour 
{
	public RectTransform currentLevelRect;			//!< to hold the draw location and size data for the current level bar4
	private float currentLevelHeight;				//!< store the indicator level for currentLevelRect

	public float startingAcidSpeed;					//!< the speed the bar initially moves when acid is added
	public float acidSpeedDecayTime;				//!< the time for the bar to slow to 0 after acid is added
	
	public float startingBaseSpeed;					//!< the speed the bar initially moves when base is added
	public float baseSpeedDecayTime;				//!< the time for the bar to slow to 0 after base is added

	private bool startAddAcid;						//!< flag to mark whether we are currently adding acid
	private bool startAddBase;						//!< flag to mark whether we are currently adding base
	private float elapsedTime;						//!< to count the time spent adding acid or base for velocity vector

	/**
	 * Use this for initialization
	 * Sets all dimenstions relative to screen size
	 * Chooses a semi-random starting height for the indicator bar
	 */
	void Start () 
	{
		// starting level of the current level indicator
		// right now just start it at a random height...
		while (currentLevelHeight == 0 || (currentLevelHeight > 120 && currentLevelHeight < 190))
		{
			currentLevelHeight = Random.Range (-90f, 292f);
		}
		currentLevelRect.anchoredPosition = new Vector2(currentLevelRect.anchoredPosition.x, currentLevelHeight);
	}
	
	/**
	 * Update is called once per frame
	 * Handles adding acid/base if the button has been pressed
	 * Controls acid decay over time
	 */
	void Update () 
	{
		// first check if the time that the acid/base should be added is up
		if (elapsedTime > acidSpeedDecayTime && startAddAcid)			// check if we're adding acid and time is up
		{																// if it is...
			startAddAcid = false;										// set the flag to indicate we aren't adding acid
			elapsedTime = 0f;											// reset the elapsed timer
			return;
		} else if (elapsedTime > baseSpeedDecayTime && startAddBase)	// check if we're adding base and the time is up
		{																// if it is...
			startAddBase = false;										// set the flag to indicate we aren't adding base
			elapsedTime = 0f;											// reset the elapsed timer
			return;
		}

		// check if we are currently adding acid
		if (startAddAcid)
		{
			// increment the timer
			elapsedTime += Time.deltaTime;

			// set the new destination the current acid level indicator should be drawn at
			// it should move at a decreasing speed so to do so we multiply the desired speed by the percentage of time
			// that is left to move the indicator line
			moveCurrentLevelRect(startingAcidSpeed * Time.deltaTime * ( 1 - (elapsedTime / acidSpeedDecayTime)));

			return;
		}

		// check if we are currently adding base
		if (startAddBase)
		{
			// increment the timer
			elapsedTime += Time.deltaTime;

			// set the new destination the current base level indicator should be drawn at
			// it should move at a decreasing speed so to do so we multiply the desired speed by the percentage of time
			// that is left to move the indicator line
			moveCurrentLevelRect(-(startingBaseSpeed * Time.deltaTime * ( 1 - (elapsedTime / baseSpeedDecayTime))));
			
			return;
		}

		// if we aren't adding acid or base decay the bar somewhat
		moveCurrentLevelRect(-200f * Time.deltaTime);

	}

	/**
	 * Handles moving the current level of acidity bar
	 */
	private void moveCurrentLevelRect(float speed)
	{
		float prevHeight = currentLevelHeight;
		currentLevelHeight = currentLevelRect.anchoredPosition.y + (speed * Time.deltaTime * .01f * Screen.height);
		if (currentLevelHeight > (1.08f * Screen.height/2f) || currentLevelHeight < -(1f * Screen.height/2f))
		{
			if (currentLevelHeight > 0)
			{
				currentLevelHeight = 1.08f * Screen.height/2f;
			} else
			{
				currentLevelHeight = -(1f * Screen.height/2f);
			}

		}
		currentLevelRect.anchoredPosition = new Vector2(currentLevelRect.anchoredPosition.x, currentLevelHeight);
	}

	/**
	 * function that can be called to simulate adding acid to the stomach and its effect on the ph bar
	 */
	public void addAcid()
	{
		startAddAcid = true;		// throw the flag to indicate we should start adding acid
		startAddBase = false;		// if we were adding base, override the decision (no longer add base)
		elapsedTime = 0f;			// reset elapsed time
	}

	/**
	 * function that can be called to simulate adding base to the stomach and its effect on the ph bar
	 */
	public void addBase()
	{
		startAddBase = true;		// throw the flag to indicate that we should start adding base
		startAddAcid = false;		// if we were adding acid, override the decision (no longer add acid)
		elapsedTime = 0f;			// reset elapsed time
	}

	/**
	 * Function that can be used to get the current level of the acidity level in the stomach
	 */
	public float getCurrentLevelRectHeight()
	{
		return currentLevelHeight;
	}
}
