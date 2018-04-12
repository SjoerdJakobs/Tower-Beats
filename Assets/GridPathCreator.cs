using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathCreator : MonoBehaviour
{
    [SerializeField] private List<Tile> m_SelectedTiles = new List<Tile>();

    private bool m_IsSelectingTiles;

    public void StartSelection()
    {
        Tile.s_OnTileClicked += TileClicked;
        m_IsSelectingTiles = true;
    }

    public void StopSelection()
    {
        Tile.s_OnTileClicked -= TileClicked;
        m_IsSelectingTiles = false;
    }

    private void TileClicked(Tile tile)
    {
        if(m_IsSelectingTiles)
        {
            if (!AlreadySelected(tile))
            { 
                m_SelectedTiles.Add(tile);
            }
        }
    }

    private bool AlreadySelected(Tile tile)
    {
        return m_SelectedTiles.Contains(tile);
    }
}
