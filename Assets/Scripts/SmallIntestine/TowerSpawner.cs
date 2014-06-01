using UnityEngine;
using System.Collections.Generic;

public class TowerSpawner : MonoBehaviour
{
	// for holding the tracker
	private GameObject statTracker;
	private TrackStatVariables trackStatVariables;

	// Randomly selects a tower model upon creation
	public GameObject[] Towers;
	public GameObject SpawnIndicator;
	DebugConfig debugConfig;

	// texture for bottom bar area
	public Texture bottomBar;

	// Set available tower colors in editor
	public Color[] AvailableColors;
	public GUIStyle ButtonStyle;

	// make styles for the buttons for the button bar
	public GUIStyle fatsActive;
	public GUIStyle fatsInactive;
	public GUIStyle fats2Active;
	public GUIStyle fats2Inactive;
	public GUIStyle proteinActive;
	public GUIStyle proteinInactive;
	public GUIStyle carbsActive;
	public GUIStyle carbsInactive;

	//arrays to hold buttons
	public GUIStyle[] activeButtons;
	public GUIStyle[] inactiveButtons;

	// for sounds
	public GameObject placementSound;
	
	public Rect Dimensions;
	public bool IsMouseOverWall;
	public GameObject wall;

	// Need to poll from last frame, otherwise
	// will return false as soon as user releases mouse
	private bool m_IsMouseOverWallLastFrame;
	private List<GameObject> m_SpawnAreas;
	private Rect m_ButtonSize;
	private bool m_IsSpawnActive;
	private GameObject m_SpawnedTower;
	private GameObject m_Indicator;
	private IntestineGameManager m_GameManager;
	public int TOWER_BASE_COST = 20;
	private const float timer = 2.0f;
	private float timePassed = 0.0f;
		
	void Start ()
	{
		statTracker = GameObject.Find ("SIStatTracker(Clone)");
		trackStatVariables = statTracker.GetComponent<TrackStatVariables>();

		debugConfig = ((GameObject)GameObject.Find("Debug Config")).GetComponent<DebugConfig>();
		TOWER_BASE_COST = debugConfig.TOWER_BASE_COST;
		m_IsSpawnActive = false;

		// button size
		Dimensions.x = Screen.width * 0.0148f;		// x location of first button
		Dimensions.y = Screen.height * 0.89f;			// y location of first button
		Dimensions.width = Screen.width * 0.197f;		// width of a button
		Dimensions.height = Screen.height * 0.091f;	// height of a button
		m_ButtonSize = Dimensions;

		m_GameManager = GameObject.Find ("Managers").GetComponent<IntestineGameManager> ();

		// initialize the arrays for holding the buttons
		activeButtons = new GUIStyle[4];
		inactiveButtons = new GUIStyle[4];

		// fill arrays
		activeButtons [0] = fatsActive;
		activeButtons [1] = fats2Active;
		activeButtons [2] = proteinActive;
		activeButtons [3] = carbsActive;
		inactiveButtons [0] = fatsInactive;
		inactiveButtons [1] = fats2Inactive;
		inactiveButtons [2] = proteinInactive;
		inactiveButtons [3] = carbsInactive;
	}

	void Update ()
	{
		if(Time.timeScale <= 0.001f)
		{
			if(m_IsSpawnActive)
			{
				m_IsSpawnActive = false;
				DestroyImmediate (m_Indicator.renderer.material);
				Destroy (m_Indicator);
				Destroy (m_SpawnedTower);
				m_SpawnedTower = null;
			}
		}
		TOWER_BASE_COST = debugConfig.TOWER_BASE_COST;
		// Handle valid spawn locations if player is spawning a tower
		if (m_IsSpawnActive) 
		{
			if (Input.GetMouseButtonUp (0)) 
			{
				m_IsSpawnActive = false;

				DestroyImmediate (m_Indicator.renderer.material);
				Destroy (m_Indicator);
			
				if (m_IsMouseOverWallLastFrame && m_GameManager.nutrients - TOWER_BASE_COST >= 0) 
				{
					
					m_SpawnedTower.GetComponent<Tower> ().enabled = true;
					m_SpawnedTower.transform.position = wall.transform.position + new Vector3 (0, 0.5f, 0);
					m_SpawnedTower.GetComponent<Tower>().wall = wall;
					m_SpawnedTower.GetComponent<TowerMenu> ().Initialize ();
					m_GameManager.nutrients = m_GameManager.nutrients - TOWER_BASE_COST;  // cost nutrients for testing

					// play placement sound
					Instantiate (placementSound);

					// track stats
					trackStatVariables.increaseTowersPlaced();
					trackStatVariables.increaseNutrientsSpent(TOWER_BASE_COST);
				} else 
				{
					Destroy (m_SpawnedTower);
					m_SpawnedTower = null;
				}
			} else 
			{
				m_SpawnedTower.transform.position = MDPUtility.MouseToWorldPosition () + Vector3.up;

				m_Indicator.transform.position = MDPUtility.MouseToWorldPosition () + Vector3.up;
				Color color = m_IsMouseOverWallLastFrame ? Color.green : Color.red;
				color.a = 0.5f;
				m_Indicator.renderer.material.color = color;
			}
		}
		
		m_IsMouseOverWallLastFrame = IsMouseOverWall;
	}

	void OnGUI ()
	{
		// draw the bottom GUI bar

		GUI.DrawTexture (new Rect (0, Screen.height * 0.82421875f, Screen.width, Screen.height * 0.17578125f), bottomBar);

//		Matrix4x4 orig = GUI.matrix;
//		GUI.matrix = GuiUtility.CachedScaledMatrix;

		if (m_GameManager.nutrients - TOWER_BASE_COST >= 0 && Time.timeScale != 0) 
		{
			for (int i = 0; i < AvailableColors.Length; i++)
			{
				if (i == 0)
				{
					m_ButtonSize.x = Dimensions.x;
				} else 
				{
					m_ButtonSize.x = Dimensions.x + i*(Screen.width * 0.0123f + Dimensions.width);
				}
				if (GUI.RepeatButton (m_ButtonSize, "", activeButtons[i])) 
				{
					if (!m_IsSpawnActive) 
					{
						SpawnTower (AvailableColors [i]);
						m_SpawnedTower.GetComponent<Tower> ().enabled = false;
						return;
					}
				}
			}
		} else
		{
			for (int i = 0; i < AvailableColors.Length; i++)
			{
				if (i == 0)
				{
					m_ButtonSize.x = Dimensions.x;
				} else 
				{
					m_ButtonSize.x = Dimensions.x + i*(Screen.width*0.0123f + Dimensions.width);
				}
				GUI.RepeatButton (m_ButtonSize, "", inactiveButtons[i]);
			}
		}

//		GUI.matrix = orig;
	}

	private void SpawnTower (Color color)
	{
		GameObject towerType = Towers [MDPUtility.RandomInt (Towers.Length)];
		m_SpawnedTower = Instantiate (towerType) as GameObject;
		Tower tower = m_SpawnedTower.GetComponent<Tower> ();
		tower.SetColor (color);

		// Set up spawn indicator
		float range = tower.FiringRange;
		m_Indicator = Instantiate (SpawnIndicator) as GameObject;
		m_Indicator.transform.localScale = new Vector3 (range * 2, 1, range * 2);

		m_IsSpawnActive = true;
	}
}

