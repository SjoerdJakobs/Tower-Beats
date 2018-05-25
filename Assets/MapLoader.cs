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

    private List<Vector3> m_Path = new List<Vector3>();
    public List<Vector3> Path { get { return m_Path; } }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        LoadMapsData();
    }

    public void LoadMap(string mapName)
    {
        if(!HexGrid.s_Instance.GridCreated)
        {
            Debug.LogError("[MapLoader] Could not load map. Grid is not created yet.");
            return;
        }

        HexGrid.s_Instance.ClearGrid();

        LoadMapsData();

        m_Map = m_MapsData.MapsData.Find(x => x.Name.ToUpper() == mapName.ToUpper());

        if(m_Map != null)
        {
            List<Vector3> tempPath = new List<Vector3>();
            for (int i = 0; i < m_Map.TilesData.Count; i++)
            {
                Tile tile = HexGrid.s_Instance.GetTile(m_Map.TilesData[i].PathTilePosition);
                tile.CurrentState = m_Map.TilesData[i].State;

                switch (m_Map.TilesData[i].State)
                {
                    case TileState.PATH:

                        tempPath.Add((Vector2)m_Map.TilesData[i].PathTilePosition);

                        if (m_Map.TilesData[i].PathTileIndex == 0)
                            tile.SetTileVisualsState(TileVisualState.ENEMY_SPAWN);
                        else
                            tile.SetTileVisualsState(TileVisualState.PATH);

                        break;
                    case TileState.TURRET_SPAWN:
                        tile.SetTileVisualsState(TileVisualState.TURRET_SPAWN);
                        break;
                    case TileState.PROP:
                        tile.SetTileVisualsState(TileVisualState.PROP, m_Map.TilesData[i].FilePathToAsset);
                        break;
                }

            }

            // Show Head Quarters
            int counter = 0;
            for (int i = tempPath.Count - 1; i >= 0; i--)
            {
                if (counter < 2)
                {
                    Tile tile = HexGrid.s_Instance.GetTile(new Vector2Int((int)tempPath[i].x, (int)tempPath[i].y));
                    tile.CurrentState = TileState.NOT_USABLE;
                    tempPath.Remove(tempPath[i]);

                    if (counter == 0)
                        tile.SetTileVisualsState(TileVisualState.HEADQUARTERS);
                    else
                        tile.SetTileVisualsState(TileVisualState.BASE);

                    counter++;
                }
            }
            m_Path = tempPath;
            Debug.Log("[MapLoader] Map loaded succesfully.");
        }
        else
        {
            Debug.LogError("[MapLoader] Could not load map. Map name does not exist.");
        }
    }

    private void LoadMapsData()
    {
        string jsonString = m_MapsFile.ToString();
        JsonUtility.FromJsonOverwrite(jsonString, m_MapsData);
    }
}
