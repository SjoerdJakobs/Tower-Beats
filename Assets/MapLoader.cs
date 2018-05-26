using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public static MapLoader s_Instance;
    [SerializeField] private TextAsset m_MapsFile;

    private Maps m_MapsData = new Maps();
    private Map m_Map;
    public Map MapData { get { return m_Map; } }

    private List<Vector3> m_Path = new List<Vector3>();
    public List<Vector3> Path { get { return m_Path; } }

    private void Awake()
    {
        // Initialize the MapLoader
        Init();
    }

    /// <summary>
    /// Initializes the MapLoader
    /// </summary>
    private void Init()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        // Load all the maps' data
        LoadAllMapsData();
    }

    /// <summary>
    /// Load a Map
    /// </summary>
    /// <param name="mapName">Name of the map</param>
    public void LoadMap(string mapName)
    {
        // Check if the HexGrid instance is present
        if (HexGrid.s_Instance == null)
        {
            Debug.LogError("[MapLoader] Could not load map. HexGrid does not exist yet.");
            return;
        }

        // Set the starting time of the loading
        float startLoadTime = Time.time;

        // Load all the maps' data in case the one during the initialization failed
        LoadAllMapsData();

        // Load the entered map
        m_Map = m_MapsData.MapsData.Find(x => x.Name.ToUpper() == mapName.ToUpper());

        StartCoroutine(UpdateVisuals(() => {
            Debug.Log("[MapLoader] Map loaded succesfully. It took " + (Time.time - startLoadTime) + " second(s) to load the map.");
        }));
    }

    private IEnumerator UpdateVisuals(Action onComplete)
    {
        // Update the HexGrid
        bool gridUpdated = UpdateGrid();

        // Wait for the grid to update
        yield return new WaitUntil(() => gridUpdated);

        // Update the Tiles
        bool tilesUpdated = UpdateTiles();

        yield return new WaitUntil(() => tilesUpdated);

        onComplete();
    }

    /// <summary>
    /// Updates the HexGrid to match the map data's values
    /// </summary>
    /// <returns>Is updating the HexGrid successful?</returns>
    private bool UpdateGrid()
    {
        // Check if the map data is present
        if(m_Map == null)
        {
            Debug.LogWarning("[MapLoader] Could not update Grid. Map data is null");
            return false;
        }

        // Update the Grid
        if (!HexGrid.s_Instance.GridCreated)
        {
            HexGrid.s_Instance.CreateGrid(m_Map.GridSize);
        }
        else if (HexGrid.s_Instance.GridSize != m_Map.GridSize)
        {
            HexGrid.s_Instance.DestroyGrid(false);
            HexGrid.s_Instance.CreateGrid(m_Map.GridSize);
        }
        else
        {
            HexGrid.s_Instance.ClearGrid();
        }

        return true;
    }

    /// <summary>
    /// Updates the Tiles to match the map data's values
    /// </summary>
    /// <returns>Is updating the Tiles successful?</returns>
    private bool UpdateTiles()
    {
        if(m_Map == null)
        {
            Debug.LogWarning("[MapLoader] Could not update Tiles. Map data is null");
            return false;
        }

        // Create a temp list to hold the path's positions
        List<Vector3> tempPath = new List<Vector3>();

        // Loop through all the tiles in the map's tilesdata
        for (int i = 0; i < m_Map.TilesData.Count; i++)
        {
            // Get the tile from the HexGrid
            Tile tile = HexGrid.s_Instance.GetTile(m_Map.TilesData[i].PathTilePosition);

            // Set the state of the tile to the state from the map's tiledata
            tile.CurrentState = m_Map.TilesData[i].State;

            // Check the state of the tile from the tilesdata
            switch (m_Map.TilesData[i].State)
            {
                case TileState.PATH:
                    // Add the tile to the temp path
                    tempPath.Add((Vector2)m_Map.TilesData[i].PathTilePosition);

                    // If the index of the path tile is 0, set the tile visual state as an enemy spawner. The other path tiles will just be normal path visual wise
                    if (m_Map.TilesData[i].PathTileIndex == 0)
                        tile.SetTileVisualsState(TileVisualState.ENEMY_SPAWN);
                    else
                        tile.SetTileVisualsState(TileVisualState.PATH);
                    break;
                case TileState.TURRET_SPAWN:
                    // Set the visual state of the turret spawns
                    tile.SetTileVisualsState(TileVisualState.TURRET_SPAWN);
                    break;
                case TileState.PROP:
                    // Set the visual state of the props
                    tile.SetTileVisualsState(TileVisualState.PROP, m_Map.TilesData[i].FilePathToAsset);
                    break;
            }
        }

        // Set the last path tile as the "Head Quarters" and remove the last 2 path tiles so that the path matches the "Head Quarters" visuals
        int counter = 0;
        // Loop through all the path tiles
        for (int i = tempPath.Count - 1; i >= 0; i--)
        {
            if (counter < 2)
            {
                // Get the tile from the HexGrid
                Tile tile = HexGrid.s_Instance.GetTile(new Vector2Int((int)tempPath[i].x, (int)tempPath[i].y));
                // Set the state back to not usable
                tile.CurrentState = TileState.NOT_USABLE;
                // Remove it from the temp path list
                tempPath.Remove(tempPath[i]);

                // Set the visuals for the headquarters and set the rest back to default
                if (counter == 0)
                    tile.SetTileVisualsState(TileVisualState.HEADQUARTERS);
                else
                    tile.SetTileVisualsState(TileVisualState.BASE);

                counter++;
            }
        }

        // Set the pathdata
        m_Path = tempPath;
        return true;
    }

    /// <summary>
    /// Loads all the maps' data
    /// </summary>
    private void LoadAllMapsData()
    {
        string jsonString = m_MapsFile.ToString();
        JsonUtility.FromJsonOverwrite(jsonString, m_MapsData);
    }
}
