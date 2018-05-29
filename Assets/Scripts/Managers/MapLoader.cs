﻿using System;
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

    private List<Tile> m_Path = new List<Tile>();
    public List<Tile> Path { get { return m_Path; } }
    public Tile HeadQuarters { get; private set; }
    public Vector2 HeadQuartersPosition { get { return HeadQuarters.transform.position; } }

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
            Debug.LogError("<color=orange>[MapLoader]</color> Could not load map. HexGrid does not exist yet.");
            return;
        }

        // Set the starting time of the loading
        float startLoadTime = Time.time;

        // Load all the maps' data in case the one during the initialization failed
        LoadAllMapsData();

        // Load the entered map
        m_Map = m_MapsData.MapsData.Find(x => x.Name.ToUpper() == mapName.ToUpper());

        StartCoroutine(UpdateVisuals(() => {
            Debug.Log("<color=orange>[MapLoader]</color> Map (" + mapName + ") loaded succesfully. It took " + (Time.time - startLoadTime) + " second(s) to load the map.");
        }));
    }

    private IEnumerator UpdateVisuals(Action onComplete)
    {
        // Update the HexGrid
        bool gridUpdated = UpdateGrid();

        // Wait for the grid to update
        yield return new WaitUntil(() => gridUpdated);

        // Update the Tiles
        bool tilesUpdated = UpdateTiles(true);

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
            Debug.LogWarning("<color=orange>[MapLoader]</color> Could not update Grid. Map data is null");
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
    private bool UpdateTiles(bool animate = false)
    {
        if(m_Map == null)
        {
            Debug.LogWarning("<color=orange>[MapLoader]</color> Could not update Tiles. Map data is null");
            return false;
        }

        // Create a temp list to hold the path's tiles
        List<Tile> tempPath = new List<Tile>();
        List<Tile> tempProps = new List<Tile>();
        List<Tile> tempSpawns = new List<Tile>();

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
                    // Add the tile to the temp paths
                    tempPath.Add(tile);

                    // If the index of the path tile is 0, set the tile visual state as an enemy spawner. The other path tiles will just be normal path visual wise
                    if (m_Map.TilesData[i].PathTileIndex == 0)
                        tile.SetTileVisualsState(TileVisualState.ENEMY_SPAWN, !animate);
                    else
                        tile.SetTileVisualsState(TileVisualState.PATH, !animate);
                    break;
                case TileState.TURRET_SPAWN:
                    // Add the tile to the temp turret spawn list
                    tempSpawns.Add(tile);

                    // Set the visual state of the turret spawns
                    tile.SetTileVisualsState(TileVisualState.TURRET_SPAWN, !animate);
                    break;
                case TileState.PROP:
                    // Add the tile to the temp prop list
                    tempProps.Add(tile);

                    // Set the visual state of the props
                    tile.SetTileVisualsState(TileVisualState.PROP, !animate, m_Map.TilesData[i].FilePathToAsset);
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
                // Get the tile
                Tile tile = tempPath[i];
                // Set the state back to not usable
                tile.CurrentState = TileState.NOT_USABLE;
                // Remove it from the temp path list
                tempPath.Remove(tile);

                // Set the visuals for the headquarters and set the rest back to default
                if (counter == 0)
                {
                    HeadQuarters = tile;
                    tile.SetTileVisualsState(TileVisualState.HEADQUARTERS, !animate);
                }
                else
                    tile.SetTileVisualsState(TileVisualState.BASE);

                counter++;
            }
        }

        // Set the pathdata
        m_Path = tempPath;
        if (animate)
        {
            StartCoroutine(AnimateLoadPath(tempPath));
            StartCoroutine(AnimateLoadSpawns(tempSpawns));
            StartCoroutine(AnimateLoadProps(tempProps));
        }
        return true;
    }

    private IEnumerator AnimateLoadPath(List<Tile> path, Action onComplete = null)
    {
        HeadQuarters.AnimateFadeScaleIn(TileVisualState.HEADQUARTERS);
        for (int i = path.Count - 1; i >= 0; i--)
        {
            if(i == 0)
                path[i].AnimateFadeScaleIn(TileVisualState.ENEMY_SPAWN);
            else
                path[i].AnimateFadeScaleIn(TileVisualState.PATH);

            yield return new WaitForSeconds(0.025f);
        }
        if (onComplete != null) onComplete();
    }

    private IEnumerator AnimateLoadSpawns(List<Tile> spawns, Action onComplete = null)
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].AnimateFadeScaleIn(TileVisualState.TURRET_SPAWN);
            yield return new WaitForSeconds(0.1f);
        }
        if (onComplete != null) onComplete();
    }

    private IEnumerator AnimateLoadProps(List<Tile> props, Action onComplete = null)
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].AnimateScaleBounceIn(TileVisualState.PROP);
            yield return new WaitForSeconds(0.025f);
        }
        if (onComplete != null) onComplete();
    }

    /// <summary>
    /// Loads all the maps' data
    /// </summary>
    private void LoadAllMapsData()
    {
        string jsonString = m_MapsFile.ToString();
        JsonUtility.FromJsonOverwrite(jsonString, m_MapsData);
    }

    /// <summary>
    /// Get the waypoints from the path
    /// </summary>
    /// <param name="reversed">Get the array reversed?</param>
    /// <returns>An array with the waypoints from the path</returns>
    public Vector3[] GetWaypointsFromPath(bool reversed = false)
    {
        Vector3[] temp = new Vector3[m_Path.Count];
        if(reversed)
        {
            for (int i = m_Path.Count - 1; i >= 0; i--)
                temp[i] = m_Path[i].transform.position;
        }
        else
        {
            for (int i = 0; i < m_Path.Count; i++)
                temp[i] = m_Path[i].transform.position;
        }

        return temp;
    }
}