using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public static MapEditor s_Instance;

    private List<TileData> m_TilesData = new List<TileData>();

    [SerializeField] private List<Sprite> m_PropSprites = new List<Sprite>();
    public List<Sprite> PropSprites { get { return m_PropSprites; } }

    private Tile m_CurrentSelectedTile;

    private List<TileData> m_Path = new List<TileData>();

    private void Awake()
    {
        Init();
        Tile.s_OnTileClicked += TileClicked;
    }

    private void OnDestroy()
    {
        Tile.s_OnTileClicked -= TileClicked;
    }

    private void Init()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);
    }

    private void TileClicked(Tile tile)
    {
        m_CurrentSelectedTile = tile;
    }

    public void SelectPath()
    {

    }
}
