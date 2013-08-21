using UnityEngine;
using System.Collections.Generic;

public class TowerSpawner : MonoBehaviour {
    // Randomly selects a tower model upon creation
    public GameObject[] Towers;

    public GameObject SpawnIndicator;

    // Set available tower colors in editor
    public Color[] AvailableColors;

    public GUIStyle ButtonStyle;
    public Rect Dimensions;
	
	public bool IsMouseOverWall;		
	// Need to poll from last frame, otherwise
	// will return false as soon as user releases mouse
	private bool m_IsMouseOverWallLastFrame;

    private List<GameObject> m_SpawnAreas;

    private Rect m_ButtonSize;
    private bool m_IsSpawnActive;

    private GameObject m_SpawnedTower;
    private GameObject m_Indicator;

    void Start ()
    {
        m_IsSpawnActive = false;

        float totalWidth = Dimensions.width * AvailableColors.Length;

        Dimensions.x = (GuiUtility.ORIG_SCREEN_WIDTH - totalWidth) / 2;
        Dimensions.y = GuiUtility.ORIG_SCREEN_HEIGHT - Dimensions.height;
        m_ButtonSize = Dimensions;

        GameObject spawnAreaObject = GameObject.FindGameObjectWithTag("SpawnArea");
        m_SpawnAreas = new List<GameObject>();
        foreach (Transform child in spawnAreaObject.transform)
        {
            m_SpawnAreas.Add(child.gameObject);
        }
    }

    void Update ()
    {
        // Handle valid spawn locations if player is spawning a tower
        if (m_IsSpawnActive)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_IsSpawnActive = false;

                DestroyImmediate(m_Indicator.renderer.material);
                Destroy(m_Indicator);
				
                if (m_IsMouseOverWallLastFrame)
                {
                    m_SpawnedTower.GetComponent<Tower>().enabled = true;
                    m_SpawnedTower.transform.position = MDPUtility.MouseToWorldPosition() + new Vector3(0, 0.5f, 0);
					m_SpawnedTower.GetComponent<TowerMenu>().Initialize();
                }
                else
                {
                    Destroy(m_SpawnedTower);
                    m_SpawnedTower = null;
                }
            }
            else
            {
                m_SpawnedTower.transform.position = MDPUtility.MouseToWorldPosition();
                m_Indicator.transform.position = MDPUtility.MouseToWorldPosition() + Vector3.up;
                Color color = m_IsMouseOverWallLastFrame ? Color.green : Color.red;
                color.a = 0.5f;
                m_Indicator.renderer.material.color = color;
            }
        }
		
		m_IsMouseOverWallLastFrame = IsMouseOverWall;
    }

    void OnGUI ()
    {
        Matrix4x4 orig = GUI.matrix;
        GUI.matrix = GuiUtility.CachedScaledMatrix;

        for (int i = 0; i < AvailableColors.Length; i++)
        {
            GUI.backgroundColor = AvailableColors[i];
            m_ButtonSize.x = Dimensions.x + i * m_ButtonSize.width;

            if (GUI.RepeatButton(m_ButtonSize, "Drag", ButtonStyle) && !m_IsSpawnActive)
            {
                SpawnTower(AvailableColors[i]);
                m_SpawnedTower.GetComponent<Tower>().enabled = false;
                return;
            }
        }

        GUI.matrix = orig;
    }

    private void SpawnTower(Color color)
    {
        GameObject towerType = Towers[MDPUtility.RandomInt(Towers.Length)];
        m_SpawnedTower = Instantiate(towerType) as GameObject;
        Tower tower = m_SpawnedTower.GetComponent<Tower>();
        tower.SetColor(color);

        // Set up spawn indicator
        float range = tower.FiringRange;
        m_Indicator = Instantiate(SpawnIndicator) as GameObject;
        m_Indicator.transform.localScale = new Vector3(range * 2, 1, range * 2);

        m_IsSpawnActive = true;
    }
}
