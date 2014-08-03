﻿using UnityEngine;
using System.Collections;

public class GlowManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// for on pc/mac
		if (Input.GetButtonDown("Fire1")) 
		{
			checkClickArea();

		}

		// for ipad
		foreach (Touch touch in Input.touches) 
		{
			if (touch.phase == TouchPhase.Began)
			{
				checkClickArea();
			}
		}
	}

	void checkClickArea()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		// ignore if the bottom menu is clicked on
		if (ray.origin.z < -11.6)
		{
			return;
		}
		
		// check for menu button click
		if (ray.origin.x > 19.7 && ray.origin.z > 16)
		{
			return;
		}
		
		// check for 2x speed click
		if (ray.origin.x < -22 && ray.origin.z < -8.8)
		{
			return;
		}
		
		// if menu box is up don't light
		if (Time.timeScale < .01f)
		{
			return;
		}
		
		// ignore if a tower is directly clicked on
		if (Physics.Raycast(ray, out hit, 20))
		{
			if (hit.transform.gameObject.name.Contains("Tower"))
			{
				return;
			}
		}
		
		GameObject closestSegment = FindClosestSegment(ray);
		GlowSegment glowScript = closestSegment.GetComponent<GlowSegment> ();
		StartCoroutine(glowScript.onTouch ());
	}

	GameObject FindClosestSegment(Ray ray) 
	{
		GameObject[] segments = new GameObject[4];
		GameObject segmentUp = null;
		GameObject segmentDown = null; 
		GameObject segmentLeft = null; 
		GameObject segmentRight = null;
		GameObject closest = null;

		float distance = Mathf.Infinity;
		RaycastHit hit;

		// reset the y position to be at the desired height
		ray.origin = new Vector3 (ray.origin.x, 4.51f, ray.origin.z);

		// look for the closest segment above the click
		ray.direction = new Vector3 (0, 0, 1);
		if (Physics.Raycast(ray, out hit, 20, 1 << LayerMask.NameToLayer("Glow")))
		{
			segmentUp = hit.transform.gameObject;
		}

		// look for the closest segment below the click
		ray.direction = new Vector3 (0, 0, -1);
		if (Physics.Raycast(ray, out hit, 20, 1 << LayerMask.NameToLayer("Glow")))
		{
			segmentDown = hit.transform.gameObject;
		}

		// look for the closest segment to the left of the click
		ray.direction = new Vector3 (-1, 0, 0);
		if (Physics.Raycast(ray, out hit, 20, 1 << LayerMask.NameToLayer("Glow")))
		{
			segmentLeft = hit.transform.gameObject;
		}

		// look for the closest segment to the right of the click
		ray.direction = new Vector3 (1, 0, 0);
		if (Physics.Raycast(ray, out hit, 20, 1 << LayerMask.NameToLayer("Glow")))
		{
			segmentRight = hit.transform.gameObject;
		}

		// store the found segments into the arrays
		segments [0] = segmentUp;
		segments [1] = segmentDown;
		segments [2] = segmentLeft;
		segments [3] = segmentRight;

		// iterate over the 4 segments to find the closest one
		foreach (GameObject seg in segments) 
		{
			if (seg == null)
			{
				continue;
			}
			Vector3 diff = seg.transform.position - ray.origin;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) 
			{
				closest = seg;
				distance = curDistance;
			}
		}

		return closest;
	}
}