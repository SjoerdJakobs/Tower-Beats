using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    public List<TileData> TilesData = new List<TileData>();
}

public class MapEditor : MonoBehaviour
{
    public static MapEditor s_Instance;

    [SerializeField] private List<Sprite> m_PropSprites = new List<Sprite>();
    public List<Sprite> PropSprites { get { return m_PropSprites; } }

    public static Action s_OnTileDataAdded;

    public Tile CurrentSelectedTile { get; private set; }

    private Map m_Map;
    private int m_PathCounter;

    private bool m_ShowHeadQuarters;

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

        m_Map = new Map();
    }

    private void TileClicked(Tile tile)
    {
        CurrentSelectedTile = tile;
        if (m_ShowHeadQuarters)
            ToggleHeadQuarters();
    }

    private void PropSelected(string propPath)
    {
        m_Map.TilesData.Add(new TileData(TileState.NOT_USABLE, -1, CurrentSelectedTile.PositionInGrid, propPath));
        CurrentSelectedTile.SetTileVisualsState(TileVisualState.PROP, propPath);
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

    public void ResetTile()
    {
        TileData currentTileData = GetTileDataFromMap(CurrentSelectedTile.PositionInGrid);
        m_Map.TilesData.Remove(currentTileData);
        CurrentSelectedTile.SetTileVisualsState(TileVisualState.BASE);
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
}
