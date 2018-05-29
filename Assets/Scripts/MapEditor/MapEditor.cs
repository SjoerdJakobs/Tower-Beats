using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Map
{
    public string Name;
    public Vector2Int GridSize;
    public List<TileData> TilesData = new List<TileData>();
}

[System.Serializable]
public class Maps
{
    public List<Map> MapsData = new List<Map>();
}

public class MapEditor : MonoBehaviour
{
    public static MapEditor s_Instance;

    public static Action s_OnTileDataAdded;

    public Tile CurrentSelectedTile { get; private set; }

    private Maps m_Maps;

    private Map m_Map;
    private int m_PathCounter;

    private bool m_ShowHeadQuarters;

    private string m_FilePath;

    private void Awake()
    {
        Init();
        Tile.s_OnTileClicked += TileClicked;
        PropSelector.s_OnPropSelectorConfirm += PropSelected;
    }

    private void OnDestroy()
    {
        Tile.s_OnTileClicked -= TileClicked;
        PropSelector.s_OnPropSelectorConfirm -= PropSelected;
    }

    private void Init()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        m_FilePath = Path.Combine(Application.dataPath, "Data/Maps.json");

        m_Maps = new Maps();
        m_Map = new Map();

        LoadData();
    }

    private void TileClicked(Tile tile)
    {
        CurrentSelectedTile = tile;
        if (m_ShowHeadQuarters)
            ToggleHeadQuarters();
    }

    private void PropSelected(string propPath)
    {
        m_Map.TilesData.Add(new TileData(TileState.PROP, -1, CurrentSelectedTile.PositionInGrid, propPath));
        CurrentSelectedTile.SetTileVisualsState(TileVisualState.PROP, true, propPath);
    }

    public void SelectPath()
    {
        int pathCount = CurrentPathCount();
        m_Map.TilesData.Add(new TileData(TileState.PATH, (pathCount > 0 ? pathCount : 0), CurrentSelectedTile.PositionInGrid));

        if (pathCount <= 0)
            CurrentSelectedTile.SetTileVisualsState(TileVisualState.ENEMY_SPAWN);
        else
            CurrentSelectedTile.SetTileVisualsState(TileVisualState.PATH);

        TileDataAdded();
    }

    public void SelectTurretSpawn()
    {
        m_Map.TilesData.Add(new TileData(TileState.TURRET_SPAWN, -1, CurrentSelectedTile.PositionInGrid));
        CurrentSelectedTile.SetTileVisualsState(TileVisualState.TURRET_SPAWN);

        TileDataAdded();
    }

    public void ToggleHeadQuarters()
    {
        m_ShowHeadQuarters = !m_ShowHeadQuarters;

        int counter = 0;
        for (int i = m_Map.TilesData.Count - 1; i >= 0; i--)
        {
            if(m_Map.TilesData[i].State == TileState.PATH)
            {
                if (counter < 2)
                {
                        if (counter <= 0)
                            HexGrid.s_Instance.GetTile(m_Map.TilesData[i].PathTilePosition).SetTileVisualsState(( m_ShowHeadQuarters ? TileVisualState.HEADQUARTERS : TileVisualState.PATH));
                        else
                            HexGrid.s_Instance.GetTile(m_Map.TilesData[i].PathTilePosition).SetTileVisualsState((m_ShowHeadQuarters ? TileVisualState.BASE : TileVisualState.PATH));

                    counter++;
                }
                else
                    return;
            }
        }
       
        TileDataAdded();
    }

    public void LoadMapData()
    {
        m_Map = MapLoader.s_Instance.MapData;
    }

    public void ClearEditor()
    {
        m_Map = new Map();
        HexGrid.s_Instance.ClearGrid();
    }

    private void TileDataAdded()
    {
        CurrentSelectedTile = null;
        if (s_OnTileDataAdded != null) s_OnTileDataAdded();
    }
    
    private int CurrentPathCount()
    {
        int counter = 0;
        for (int i = 0; i < m_Map.TilesData.Count; i++)
        {
            if (m_Map.TilesData[i].PathTileIndex > -1)
                counter++;
        }

        return counter;
    }

    public void PrintPath()
    {
        for (int i = 0; i < m_Map.TilesData.Count; i++)
        {
            if(m_Map.TilesData[i].State == TileState.PATH)
            {
                print("Tile index in pathdata = " + m_Map.TilesData[i].PathTileIndex + " Position in grid = " + m_Map.TilesData[i].PathTilePosition);
            }
        }
    }

    public void SetMapName(string name)
    {
        m_Map.Name = name;
    }

    public void ResetTile()
    {
        TileData currentTileData = GetTileDataFromMap(CurrentSelectedTile.PositionInGrid);

        if(currentTileData.State == TileState.PATH)
        {
            for (int i = m_Map.TilesData.Count - 1; i >= 0; i--)
            {
                if (m_Map.TilesData[i].State == TileState.PATH)
                {
                    if (m_Map.TilesData[i].PathTileIndex >= currentTileData.PathTileIndex)
                    {
                        ResetTileVisual(m_Map.TilesData[i]);
                        RemoveTile(m_Map.TilesData[i]);
                    }
                }
            }
        }
        else
        {
            ResetTileVisual(currentTileData);
            RemoveTile(currentTileData);
        }

    }

    private void RemoveTile(TileData tile)
    {
        m_Map.TilesData.Remove(tile);
    }

    private void ResetTileVisual(TileData tile)
    {
        HexGrid.s_Instance.GetTile(tile.PathTilePosition).SetTileVisualsState(TileVisualState.BASE);
    }

    public TileData GetTileDataFromMap(Vector2Int tilePosition)
    {
        for (int i = 0; i < m_Map.TilesData.Count; i++)
        {
            if (m_Map.TilesData[i].PathTilePosition == tilePosition)
                return m_Map.TilesData[i];
        }
        return null;
    }

    public void SaveMap()
    {
        if(!MapNameAlreadyExists(m_Map.Name))
        {
            m_Map.GridSize = HexGrid.s_Instance.GridSize;
            m_Maps.MapsData.Add(m_Map);
            SaveData();
            print("Map saved");
        }
        else
        {
            print("Could not save map, map name already exists");
        }
    }

    private bool MapNameAlreadyExists(string mapName)
    {
        for (int i = 0; i < m_Maps.MapsData.Count; i++)
        {
            if (m_Maps.MapsData[i].Name.ToUpper() == mapName.ToUpper())
                return true;
        }
        return false;
    }

    private void CopyToClipboard(string s)
    {
        TextEditor te = new TextEditor
        {
            text = s
        };

        te.SelectAll();
        te.Copy();
    }

    public void CopyMapToClipboard()
    {
        CopyToClipboard(JsonUtility.ToJson(m_Map, true));
    }

    #region Save And Load Data

    /// <summary>
    /// Loads all the data
    /// </summary>
    public void LoadData()
    {
#if UNITY_EDITOR
        TryCreateFile();
#endif  

        string jsonString = File.ReadAllText(m_FilePath);
        JsonUtility.FromJsonOverwrite(jsonString, m_Maps);
    }

    /// <summary>
    /// Saves all the data
    /// </summary>
    public void SaveData()
    {
        TryCreateFile();

        string jsonString = JsonUtility.ToJson(m_Maps, true);
        File.WriteAllText(m_FilePath, jsonString);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// Tries to create a .JSON file
    /// </summary>
    private void TryCreateFile()
    {
        if (!File.Exists(m_FilePath))
            File.Create(m_FilePath).Dispose();
    }

    #endregion
}
