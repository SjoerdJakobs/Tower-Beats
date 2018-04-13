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

    public void FinishSelection()
    {
        Tile.s_OnTileClicked -= TileClicked;
        m_IsSelectingTiles = false;
    }

    private void ShowPath()
    {
        for (int i = 0; i < m_SelectedTiles.Count; i++)
        {
            m_SelectedTiles[i].SetAsPath();
        }
    }

    private void TileClicked(Tile tile)
    {
        if(m_IsSelectingTiles)
        {
            if (!AlreadySelected(tile))
            { 
                m_SelectedTiles.Add(tile);
                tile.SetHighlightState(true);
            }
            else
            {
                int positionInList = m_SelectedTiles.IndexOf(tile);
                List<Tile> tilesToRemove = m_SelectedTiles.GetRange(positionInList, (m_SelectedTiles.Count - positionInList));
                for (int i = tilesToRemove.Count; i > 0; i--)
                {
                    tilesToRemove[i].SetHighlightState(false);
                }
            }
        }
    }

    private bool AlreadySelected(Tile tile)
    {
        return m_SelectedTiles.Contains(tile);
    }
}
